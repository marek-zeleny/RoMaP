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
    // Enums are outside the Chart class so that they are not generic (Chart<TData, TDataCarrier>.[enum] would be long)

    /// <summary>
    /// Mode of determining value range of a chart
    /// </summary>
    public enum ChartRangeMode
    {
        /// <summary>
        /// Fixed manually with minimum and maximum values
        /// </summary>
        Fixed,
        /// <summary>
        /// Minimum value fixed manually, maximum derived dynamically from current values
        /// </summary>
        FixedMin,
        /// <summary>
        /// Minimum derived dynamically from current values, maximum value fixed manually
        /// </summary>
        FixedMax,
        /// <summary>
        /// Minimum and maximum values derived dynamically from current values
        /// </summary>
        Auto,
    }

    /// <summary>
    /// Unit of time used for measuring the X axis of a chart
    /// </summary>
    public enum ChartTimeUnit { Millisecond, Second, Minute, Hour, Day }

    /// <summary>
    /// Component for visualising data progression in time.
    /// The chart is optimised for dynamically changing data in an incremental manner.
    /// </summary>
    /// <remarks>
    /// The component is designed to be highly compatible with <see cref="Statistics"/>. It is not intended as a general
    /// purpose Windows Forms component.
    /// </remarks>
    /// <typeparam name="TData">Type of data plotted on the Y axis</typeparam>
    /// <typeparam name="TDataCarrier">
    /// Type of data source; can either contain a list of timestamped data, or contain a single timestamped datum and
    /// be given as a list of data carriers
    /// </typeparam>
    class Chart<TData, TDataCarrier> : Panel
        where TDataCarrier : class
    {
        /// <summary>
        /// Function type for aggregating a list of timestamped data from a data source
        /// </summary>
        public delegate IReadOnlyList<Timestamp<TData>> DataListAggregator(TDataCarrier stats);

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

        private ChartRangeMode mode;
        private double minValue;
        private double maxValue;
        private string valueUnit;

        private Time timeSpan;
        private IClock clock;
        private ChartTimeUnit timeRepr;
        private Func<Time, string> timeToString;

        #region properties

        /// <summary>
        /// Caption of the chart
        /// </summary>
        public string Caption
        {
            get => caption;
            set
            {
                caption = value;
                UpdateChart();
            }
        }
        /// <summary>
        /// Mode in which the chart's value range is determined
        /// </summary>
        public ChartRangeMode Mode
        {
            get => mode;
            set
            {
                mode = value;
                UpdateChart();
            }
        }
        /// <summary>
        /// Minimum displayed value of the chart's Y axis
        /// </summary>
        /// <remarks>This value is ignored for some chart range modes.</remarks>
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
        /// <summary>
        /// Maximum displayed value of the chart's Y axis
        /// </summary>
        /// <remarks>This value is ignored for some chart range modes.</remarks>
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
        /// <summary>
        /// String displayed as the Y axis' value unit
        /// </summary>
        public string ValueUnit
        {
            get => valueUnit;
            set
            {
                valueUnit = value;
                UpdateChart();
            }
        }
        /// <summary>
        /// Time span of the chart's X axis
        /// </summary>
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
        /// <summary>
        /// Time (X) axis' value unit
        /// </summary>
        public ChartTimeUnit TimeRepresentation
        {
            get => timeRepr;
            set
            {
                timeRepr = value;
                timeToString = timeRepr switch
                {
                    ChartTimeUnit.Millisecond => time => $"{time.ToMilliseconds()} ms",
                    ChartTimeUnit.Second => time => $"{time.ToSeconds()} s",
                    ChartTimeUnit.Minute => time => $"{time.ToMinutes()} min",
                    ChartTimeUnit.Hour => time => $"{time.ToHours()} h",
                    ChartTimeUnit.Day => time => $"{time.ToDays()} d",
                    _ => throw new NotImplementedException(),
                };
                UpdateChart();
            }
        }

        #endregion properties

        /// <summary>
        /// Creates a new chart.
        /// </summary>
        /// <param name="dataToDouble">Default function for converting data to double values</param>
        public Chart(Func<TData, double> dataToDouble)
        {
            // Panel properties
            DoubleBuffered = true;
            BackColor = Color.White;
            // Custom properties
            TimeRepresentation = ChartTimeUnit.Second;
            this.dataToDouble = dataToDouble;
            dataCache = new Queue<Timestamp<TData>>();
        }

        #region interface

        /// <summary>
        /// Sets a new data source containing a list of timestamped data.
        /// </summary>
        /// <param name="stats">Data source</param>
        /// <param name="aggregator">Function aggregating a list of timestamped data from the data source</param>
        /// <param name="clock">Global clock of the simulation</param>
        /// <param name="dataToDouble">
        /// Function for converting data to double values; if <c>null</c>, a previously given function is used
        /// </param>
        public void SetDataSource(TDataCarrier stats, DataListAggregator aggregator, IClock clock,
            Func<TData, double> dataToDouble = null)
        {
            lock (dataCache)
            {
                statsList = null;
                statsListAggregator = null;

                this.stats = stats;
                statsAggregator = aggregator;
                this.clock = clock;
                if (dataToDouble != null)
                    this.dataToDouble = dataToDouble;
            }
            ClearCache();
            UpdateChart();
        }

        /// <summary>
        /// Sets a new data source as a list of data carriers containing a single timestamped datum.
        /// </summary>
        /// <param name="statsList">Data source</param>
        /// <param name="aggregator">Function aggregating timestamped data from data carriers</param>
        /// <inheritdoc cref="SetDataSource(TDataCarrier, DataListAggregator, IClock, Func{TData, double})"/>
        public void SetDataSource(IReadOnlyList<TDataCarrier> statsList,
            Func<TDataCarrier, Timestamp<TData>> aggregator, IClock clock,
            Func<TData, double> dataToDouble = null)
        {
            lock (dataCache)
            {
                stats = null;
                statsAggregator = null;

                this.statsList = statsList;
                statsListAggregator = aggregator;
                this.clock = clock;
                if (dataToDouble != null)
                    this.dataToDouble = dataToDouble;
            }
            ClearCache();
            UpdateChart();
        }

        /// <summary>
        /// Disconnects the current data source.
        /// </summary>
        public void ClearDataSource()
        {
            lock (dataCache)
            {
                stats = null;
                statsAggregator = null;
                statsList = null;
                statsListAggregator = null;
            }
            ClearCache();
            UpdateChart();
        }

        /// <summary>
        /// Ensures the chart is updated.
        /// </summary>
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

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateChart();
        }

        #region helper_functions

        /// <summary>
        /// Draws the chart onto given graphics.
        /// </summary>
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
                    case ChartRangeMode.Fixed:
                        min = MinValue;
                        max = MaxValue;
                        break;
                    case ChartRangeMode.FixedMin:
                        min = MinValue;
                        (_, max) = FindExtremes(data);
                        break;
                    case ChartRangeMode.FixedMax:
                        (min, _) = FindExtremes(data);
                        max = MaxValue;
                        break;
                    case ChartRangeMode.Auto:
                        (min, max) = FindExtremes(data);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                var border = DrawAxes(gr, min, max);
                DrawData(gr, data, min, max, border);
            }
        }

        /// <summary>
        /// Draws the chart's X and Y axes onto given graphics.
        /// </summary>
        /// <param name="min">Minimum value of the Y axis</param>
        /// <param name="max">Maximum value of the Y axis</param>
        /// <returns>Pixel offsets of the chart's borders encapsulating the area for drawing data</returns>
        private (int top, int bot, int left, int right) DrawAxes(Graphics gr, double min, double max)
        {
            const int labelMargin = 5;

            int decimals = (int)-Math.Floor(Math.Log10(max - min));
            if (decimals < 0)
                decimals = 0;

            Time now = new();
            if (clock != null)
                now = clock.Time;
            string yMin = $"{min.ToString($"F{decimals}")} {ValueUnit}";
            string yMax = $"{max.ToString($"F{decimals}")} {ValueUnit}";
            string xMin = timeToString(now - TimeSpan);
            string xMax = timeToString(now);

            Font axisFont = new(SystemFonts.DefaultFont.FontFamily, 10f);
            Font captionFont = new(axisFont, FontStyle.Bold);

            Size captionSize = gr.MeasureString(caption, captionFont).ToSize();
            Size yMaxSize = gr.MeasureString(yMax, axisFont).ToSize();
            Size xMaxSize = gr.MeasureString(xMax, axisFont).ToSize();

            int top = labelMargin * 2 + captionSize.Height;
            int bot = Height - (labelMargin + xMaxSize.Height);
            int left = labelMargin + yMaxSize.Width;
            int right = Width - (labelMargin + xMaxSize.Width / 2);
            Brush brush = Brushes.Black;
            StringFormat format = new();
            // axes
            gr.DrawLine(Pens.Black, left, top, left, bot);
            gr.DrawLine(Pens.Black, left, bot, right, bot);
            // value (y) axis labels
            format.LineAlignment = StringAlignment.Center;
            gr.DrawString(yMin, axisFont, brush, labelMargin, bot, format);
            gr.DrawString(yMax, axisFont, brush, labelMargin, top, format);
            // time (x) axis labels
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Center;
            gr.DrawString(xMin, axisFont, brush, left, bot + labelMargin, format);
            gr.DrawString(xMax, axisFont, brush, right, bot + labelMargin, format);
            // caption
            gr.DrawString(caption, captionFont, brush, Width / 2, labelMargin, format);
            return (top, bot, left, right);
        }

        /// <summary>
        /// Draws data onto given graphics.
        /// </summary>
        /// <param name="data">Data to be drawn</param>
        /// <param name="min">Minimum value of the Y axis</param>
        /// <param name="max">Maximum value of the Y axis</param>
        /// <param name="border">Pixel offsets of the chart's borders encapsulating the area for drawing data</param>
        private void DrawData(Graphics gr, IEnumerable<Timestamp<TData>> data, double min, double max,
            (int top, int bot, int left, int right) border)
        {
            var (top, bot, left, right) = border;
            int height = bot - top;
            int width = right - left;
            // Prevent weird behaviour
            if (max == min)
                max += 0.1;
            double valueSpan = max - min;
            Time timeMin = clock.Time - TimeSpan;

            PointF GetPoint(Timestamp<TData> timestamp)
            {
                double relX = (double)(timestamp.time - timeMin) / TimeSpan;
                double relY = (dataToDouble(timestamp.data) - min) / valueSpan;
                float x = left + (float)(width * relX);
                float y = bot - (float)(height * relY);
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

        /// <summary>
        /// Prints that no data is available onto given graphics.
        /// </summary>
        /// <param name="gr"></param>
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

        /// <summary>
        /// Gets data to be drawn and updates the data cache.
        /// </summary>
        /// <remarks>
        /// In order for the data cache to work, the data source must provide linearly ordered data in time.
        /// </remarks>
        private IEnumerable<Timestamp<TData>> GetData()
        {
            IEnumerable<Timestamp<TData>> newData;
            // Updating data cache must be mutually exclusive with setting new data source
            lock (dataCache)
            {
                if (stats != null)
                {
                    Debug.Assert(statsAggregator != null);
                    var data = statsAggregator(stats);
                    if (data == null || data.Count == 0)
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

                return dataCache.Count > 0 ? dataCache : null;
            }
        }

        /// <summary>
        /// Finds extreme values of the given data.
        /// </summary>
        /// <returns>Minimum and maximum value in the data</returns>
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

        /// <summary>
        /// Clears the data cache.
        /// </summary>
        private void ClearCache()
        {
            lock (dataCache)
            {
                dataCache.Clear();
                cacheEndIndex = 0;
            }
        }

        #endregion helper_functions
    }
}
