
namespace RoadTrafficSimulator.Forms
{
    partial class FormStatistics
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBoxStatistics = new System.Windows.Forms.GroupBox();
            this.labelAverageDelay = new System.Windows.Forms.Label();
            this.labelAverageSpeed = new System.Windows.Forms.Label();
            this.labelStationaryCars = new System.Windows.Forms.Label();
            this.labelFinishedCars = new System.Windows.Forms.Label();
            this.labelActiveCars = new System.Windows.Forms.Label();
            this.labelTotalCars = new System.Windows.Forms.Label();
            this.comboBoxTimeSpan = new System.Windows.Forms.ComboBox();
            this.labelTimeSpan = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBoxStatistics.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(721, 701);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(6, 8);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanel);
            this.splitContainer.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.AutoScroll = true;
            this.splitContainer.Panel2.Controls.Add(this.buttonClose);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxStatistics);
            this.splitContainer.Panel2.Controls.Add(this.comboBoxTimeSpan);
            this.splitContainer.Panel2.Controls.Add(this.labelTimeSpan);
            this.splitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.splitContainer.Size = new System.Drawing.Size(1057, 705);
            this.splitContainer.SplitterDistance = 725;
            this.splitContainer.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(6, 643);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(312, 50);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBoxStatistics
            // 
            this.groupBoxStatistics.AutoSize = true;
            this.groupBoxStatistics.Controls.Add(this.labelAverageDelay);
            this.groupBoxStatistics.Controls.Add(this.labelAverageSpeed);
            this.groupBoxStatistics.Controls.Add(this.labelStationaryCars);
            this.groupBoxStatistics.Controls.Add(this.labelFinishedCars);
            this.groupBoxStatistics.Controls.Add(this.labelActiveCars);
            this.groupBoxStatistics.Controls.Add(this.labelTotalCars);
            this.groupBoxStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxStatistics.Location = new System.Drawing.Point(6, 66);
            this.groupBoxStatistics.Name = "groupBoxStatistics";
            this.groupBoxStatistics.Size = new System.Drawing.Size(312, 180);
            this.groupBoxStatistics.TabIndex = 2;
            this.groupBoxStatistics.TabStop = false;
            this.groupBoxStatistics.Text = "Statistics";
            // 
            // labelAverageDelay
            // 
            this.labelAverageDelay.AutoSize = true;
            this.labelAverageDelay.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAverageDelay.Location = new System.Drawing.Point(3, 152);
            this.labelAverageDelay.Name = "labelAverageDelay";
            this.labelAverageDelay.Size = new System.Drawing.Size(175, 25);
            this.labelAverageDelay.TabIndex = 5;
            this.labelAverageDelay.Text = "Average delay: - min";
            // 
            // labelAverageSpeed
            // 
            this.labelAverageSpeed.AutoSize = true;
            this.labelAverageSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAverageSpeed.Location = new System.Drawing.Point(3, 127);
            this.labelAverageSpeed.Name = "labelAverageSpeed";
            this.labelAverageSpeed.Size = new System.Drawing.Size(193, 25);
            this.labelAverageSpeed.TabIndex = 4;
            this.labelAverageSpeed.Text = "Average speed: - km/h";
            // 
            // labelStationaryCars
            // 
            this.labelStationaryCars.AutoSize = true;
            this.labelStationaryCars.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelStationaryCars.Location = new System.Drawing.Point(3, 102);
            this.labelStationaryCars.Name = "labelStationaryCars";
            this.labelStationaryCars.Size = new System.Drawing.Size(143, 25);
            this.labelStationaryCars.TabIndex = 3;
            this.labelStationaryCars.Text = "Stationary cars: -";
            // 
            // labelFinishedCars
            // 
            this.labelFinishedCars.AutoSize = true;
            this.labelFinishedCars.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFinishedCars.Location = new System.Drawing.Point(3, 77);
            this.labelFinishedCars.Name = "labelFinishedCars";
            this.labelFinishedCars.Size = new System.Drawing.Size(129, 25);
            this.labelFinishedCars.TabIndex = 2;
            this.labelFinishedCars.Text = "Finished cars: -";
            // 
            // labelActiveCars
            // 
            this.labelActiveCars.AutoSize = true;
            this.labelActiveCars.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelActiveCars.Location = new System.Drawing.Point(3, 52);
            this.labelActiveCars.Name = "labelActiveCars";
            this.labelActiveCars.Size = new System.Drawing.Size(112, 25);
            this.labelActiveCars.TabIndex = 1;
            this.labelActiveCars.Text = "Active cars: -";
            // 
            // labelTotalCars
            // 
            this.labelTotalCars.AutoSize = true;
            this.labelTotalCars.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTotalCars.Location = new System.Drawing.Point(3, 27);
            this.labelTotalCars.Name = "labelTotalCars";
            this.labelTotalCars.Size = new System.Drawing.Size(101, 25);
            this.labelTotalCars.TabIndex = 0;
            this.labelTotalCars.Text = "Total cars: -";
            // 
            // comboBoxTimeSpan
            // 
            this.comboBoxTimeSpan.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxTimeSpan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTimeSpan.FormattingEnabled = true;
            this.comboBoxTimeSpan.Location = new System.Drawing.Point(6, 33);
            this.comboBoxTimeSpan.Name = "comboBoxTimeSpan";
            this.comboBoxTimeSpan.Size = new System.Drawing.Size(312, 33);
            this.comboBoxTimeSpan.TabIndex = 1;
            this.comboBoxTimeSpan.SelectedIndexChanged += new System.EventHandler(this.comboBoxTimeSpan_SelectedIndexChanged);
            // 
            // labelTimeSpan
            // 
            this.labelTimeSpan.AutoSize = true;
            this.labelTimeSpan.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTimeSpan.Location = new System.Drawing.Point(6, 8);
            this.labelTimeSpan.Name = "labelTimeSpan";
            this.labelTimeSpan.Size = new System.Drawing.Size(97, 25);
            this.labelTimeSpan.TabIndex = 0;
            this.labelTimeSpan.Text = "Time span:";
            // 
            // FormStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 721);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormStatistics";
            this.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormStatistics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStatistics_FormClosing);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBoxStatistics.ResumeLayout(false);
            this.groupBoxStatistics.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ComboBox comboBoxTimeSpan;
        private System.Windows.Forms.Label labelTimeSpan;
        private System.Windows.Forms.GroupBox groupBoxStatistics;
        private System.Windows.Forms.Label labelStationaryCars;
        private System.Windows.Forms.Label labelFinishedCars;
        private System.Windows.Forms.Label labelActiveCars;
        private System.Windows.Forms.Label labelTotalCars;
        private System.Windows.Forms.Label labelAverageDelay;
        private System.Windows.Forms.Label labelAverageSpeed;
        private System.Windows.Forms.Button buttonClose;
    }
}