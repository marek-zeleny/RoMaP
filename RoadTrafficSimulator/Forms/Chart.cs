using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using RoadTrafficSimulator.Statistics;
using RoadTrafficSimulator.ValueTypes;
using DataStructures.Miscellaneous;

namespace RoadTrafficSimulator.Forms
{
    class Chart<TData, TDataCarrier> : Panel
        where TDataCarrier : class
    {
        public enum RangeMode { Fixed, FixedMin, FixedMax, Auto }
        public enum TimeUnit { Millisecond, Second, Minute, Hour, Day }
        public delegate IReadOnlyList<Timestamp<TData>> DataListAggregator(TDataCarrier stats);

        private const int axisOffset = 25;
        // data
        private Func<TData, double> dataToDouble;
        private TDataCarrier stats;
        private DataListAggregator statsAggregator;
        private IReadOnlyList<TDataCarrier> statsList;
        private Func<TDataCarrier, Timestamp<TData>> statsListAggregator;
        // cache
        private Queue<Timestamp<TData>> dataCache;
        private int cacheEndIndex;
        // properties
        private string caption;

        private RangeMode mode;
        private double minValue;
        private double maxValue;
        private string valueUnit;

        private Time timeSpan;
        private IClock clock;
        private TimeUnit timeRepr;
        private Func<Time, string> timeToString;

        #region properties

        public string Caption
        {
            get => caption;
            set
            {
                caption = value;
                UpdateChart();
            }
        }
        public RangeMode Mode
        {
            get => mode;
            set
            {
                mode = value;
                UpdateChart();
            }
        }
        public double MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                if (minValue > maxValue)
                    maxValue = minValue;
                UpdateChart();
            }
        }
        public double MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                if (maxValue < minValue)
                    minValue = maxValue;
                UpdateChart();
            }
        }
        public string ValueUnit
        {
            get => valueUnit;
            set
            {
                valueUnit = value;
                UpdateChart();
            }
        }
        public Time TimeSpan
        {
            get => timeSpan;
            set
            {
                if (value < 0)
                    timeSpan = new Time(0);
                else
                    timeSpan = value;
                ClearCache();
                UpdateChart();
            }
        }
        public TimeUnit TimeRepresentation
        {
            get => timeRepr;
            set
            {
                timeRepr = value;
                timeToString = timeRepr switch
                {
                    TimeUnit.Millisecond => time => $"{time.ToMilliseconds()}ms",
                    TimeUnit.Second => time => $"{time.ToSeconds()}s",
                    TimeUnit.Minute => time => $"{time.ToMinutes()}min",
                    TimeUnit.Hour => time => $"{time.ToHours()}h",
                    TimeUnit.Day => time => $"{time.ToDays()}d",
                    _ => throw new NotImplementedException(),
                };
                UpdateChart();
            }
        }

        #endregion properties

        public Chart(Func<TData, double> dataToDouble)
        {
            // Panel properties
            DoubleBuffered = true;
            BackColor = Color.White;
            // Custom properties
            TimeRepresentation = TimeUnit.Second;
            this.dataToDouble = dataToDouble;
            dataCache = new Queue<Timestamp<TData>>();
        }

        #region interface

        public void SetDataSource(TDataCarrier stats, DataListAggregator aggregator, IClock clock,
            Func<TData, double> dataToDouble = null)
        {
            statsList = null;
            statsListAggregator = null;

            this.stats = stats;
            statsAggregator = aggregator;
            this.clock = clock;
            if (dataToDouble != null)
                this.dataToDouble = dataToDouble;
            ClearCache();
            UpdateChart();
        }

        public void SetDataSource(IReadOnlyList<TDataCarrier> statsList,
            Func<TDataCarrier, Timestamp<TData>> aggregator, IClock clock,
            Func<TData, double> dataToDouble = null)
        {
            stats = null;
            statsAggregator = null;

            this.statsList = statsList;
            statsListAggregator = aggregator;
            this.clock = clock;
            if (dataToDouble != null)
                this.dataToDouble = dataToDouble;
            ClearCache();
            UpdateChart();
        }

        public void UpdateChart()
        {
            Invalidate();
        }

        #endregion interface

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawChart(e.Graphics);
        }

        #region helper_functions

        private void DrawChart(Graphics gr)
        {
            var data = GetData();
            if (data == null)
            {
                DrawAxes(gr, MinValue, MaxValue);
                PrintNoData(gr);
            }
            else
            {
                double min;
                double max;
                switch (Mode)
                {
                    case RangeMode.Fixed:
                        min = MinValue;
                        max = MaxValue;
                        break;
                    case RangeMode.FixedMin:
                        min = MinValue;
                        (_, max) = FindExtremes(data);
                        break;
                    case RangeMode.FixedMax:
                        (min, _) = FindExtremes(data);
                        max = MaxValue;
                        break;
                    case RangeMode.Auto:
                        (min, max) = FindExtremes(data);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                DrawAxes(gr, min, max);
                DrawData(gr, data, min, max);
            }
        }

        private void DrawAxes(Graphics gr, double min, double max)
        {
            const int labelMargin = 5;
            int top = axisOffset;
            int bot = Height - axisOffset;
            int left = axisOffset;
            int right = Width - axisOffset;
            Font font = new(SystemFonts.DefaultFont.FontFamily, 10f);
            Brush brush = Brushes.Black;
            StringFormat format = new();
            // axes
            gr.DrawLine(Pens.Black, left, top, left, bot);
            gr.DrawLine(Pens.Black, left, bot, right, bot);
            // value (y) axis labels
            format.LineAlignment = StringAlignment.Center;
            gr.DrawString($"{min} {ValueUnit}", font, brush, labelMargin, bot, format);
            gr.DrawString($"{max} {ValueUnit}", font, brush, labelMargin, top, format);
            // time (x) axis labels
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Center;
            Time now = new();
            if (clock != null)
                now = clock.Time;
            gr.DrawString(timeToString(now - TimeSpan), font, brush, left, bot + labelMargin, format);
            gr.DrawString(timeToString(now), font, brush, right, bot + labelMargin, format);
            // caption
            font = new(font, FontStyle.Bold);
            gr.DrawString(caption, font, brush, Width / 2, labelMargin, format);
        }

        private void DrawData(Graphics gr, IEnumerable<Timestamp<TData>> data, double min, double max)
        {
            int top = axisOffset;
            int bot = Height - axisOffset;
            int left = axisOffset;
            int right = Width - axisOffset;
            int height = bot - top;
            int width = right - left;

            double valueSpan = max - min;
            Time timeMin = clock.Time - TimeSpan;

            PointF GetPoint(Timestamp<TData> timestamp)
            {
                double relX = (double)(timestamp.time - timeMin) / TimeSpan;
                double relY = (dataToDouble(timestamp.data) - min) / valueSpan;
                float x = left + (float)(width * relX);
                float y = top + (float)(height * relY);
                return new(x, y);
            }

            bool PointInRange(PointF point) => point.Y >= top && point.Y <= bot;

            PointF lastPoint = GetPoint(data.First());
            foreach (var timestamp in data.Skip(1))
            {
                PointF point = GetPoint(timestamp);
                if (PointInRange(lastPoint) || PointInRange(point))
                    gr.DrawLine(Pens.Red, lastPoint, point);
                lastPoint = point;
            }
        }

        private void PrintNoData(Graphics gr)
        {
            const string text = "No data available";
            Font font = new(SystemFonts.DefaultFont.FontFamily, 10f);
            Brush brush = Brushes.Black;
            StringFormat format = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            gr.DrawString(text, font, brush, Width / 2, Height / 2, format);
        }

        private IEnumerable<Timestamp<TData>> GetData()
        {
            // Expects time linearity of the data
            IEnumerable<Timestamp<TData>> newData;
            if (stats != null)
            {
                Debug.Assert(statsAggregator != null);
                var data = statsAggregator(stats);
                if (data.Count == 0)
                    return null;
                newData = data.Slice(cacheEndIndex);
                cacheEndIndex = data.Count;
            }
            else if (statsList != null)
            {
                Debug.Assert(statsListAggregator != null);
                if (statsList.Count == 0)
                    return null;
                newData = statsList.Slice(cacheEndIndex).Select(statsListAggregator);
                cacheEndIndex = statsList.Count;
            }
            else
                return null;

            Debug.Assert(clock != null);
            Time since = clock.Time - TimeSpan;
            while (dataCache.Count > 0 && dataCache.Peek().time < since)
                dataCache.Dequeue();
            foreach (var item in newData.SkipWhile(timestamp => timestamp.time < since))
                dataCache.Enqueue(item);
            return dataCache;
        }

        private (double min, double max) FindExtremes(IEnumerable<Timestamp<TData>> data)
        {
            double min = dataToDouble(data.First().data);
            double max = min;
            foreach (var timestamp in data)
            {
                double d = dataToDouble(timestamp.data);
                if (d < min)
                    min = d;
                else if (d > max)
                    max = d;
            }
            return (min, max);
        }

        private void ClearCache()
        {
            dataCache.Clear();
            cacheEndIndex = 0;
        }

        #endregion helper_functions
    }
}
