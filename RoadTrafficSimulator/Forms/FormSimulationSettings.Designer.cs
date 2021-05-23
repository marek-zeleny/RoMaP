
namespace RoadTrafficSimulator.Forms
{
    partial class FormSimulationSettings
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
            this.groupBoxSpawnRate = new System.Windows.Forms.GroupBox();
            this.panelSpawnRate = new System.Windows.Forms.Panel();
            this.flowLayoutPanelDistribution = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDistributionDescription = new System.Windows.Forms.Label();
            this.labelDistributionDetail = new System.Windows.Forms.Label();
            this.comboBoxSpawnRateDetail = new System.Windows.Forms.ComboBox();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelParameters = new System.Windows.Forms.TableLayoutPanel();
            this.trackBarNavigation = new System.Windows.Forms.TrackBar();
            this.labelDuration = new System.Windows.Forms.Label();
            this.labelNavigation = new System.Windows.Forms.Label();
            this.trackBarDuration = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.groupBoxSpawnRate.SuspendLayout();
            this.flowLayoutPanelDistribution.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.tableLayoutPanelParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNavigation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).BeginInit();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSpawnRate
            // 
            this.groupBoxSpawnRate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxSpawnRate.Controls.Add(this.panelSpawnRate);
            this.groupBoxSpawnRate.Controls.Add(this.flowLayoutPanelDistribution);
            this.groupBoxSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSpawnRate.Location = new System.Drawing.Point(5, 5);
            this.groupBoxSpawnRate.Name = "groupBoxSpawnRate";
            this.groupBoxSpawnRate.Size = new System.Drawing.Size(560, 214);
            this.groupBoxSpawnRate.TabIndex = 0;
            this.groupBoxSpawnRate.TabStop = false;
            this.groupBoxSpawnRate.Text = "Car spawn rate distribution";
            // 
            // panelSpawnRate
            // 
            this.panelSpawnRate.AutoScroll = true;
            this.panelSpawnRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpawnRate.Location = new System.Drawing.Point(3, 78);
            this.panelSpawnRate.Name = "panelSpawnRate";
            this.panelSpawnRate.Size = new System.Drawing.Size(554, 133);
            this.panelSpawnRate.TabIndex = 0;
            // 
            // flowLayoutPanelDistribution
            // 
            this.flowLayoutPanelDistribution.AutoSize = true;
            this.flowLayoutPanelDistribution.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelDistribution.Controls.Add(this.labelDistributionDescription);
            this.flowLayoutPanelDistribution.Controls.Add(this.labelDistributionDetail);
            this.flowLayoutPanelDistribution.Controls.Add(this.comboBoxSpawnRateDetail);
            this.flowLayoutPanelDistribution.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelDistribution.Location = new System.Drawing.Point(3, 19);
            this.flowLayoutPanelDistribution.Name = "flowLayoutPanelDistribution";
            this.flowLayoutPanelDistribution.Size = new System.Drawing.Size(554, 59);
            this.flowLayoutPanelDistribution.TabIndex = 0;
            // 
            // labelDistributionDescription
            // 
            this.labelDistributionDescription.AutoSize = true;
            this.flowLayoutPanelDistribution.SetFlowBreak(this.labelDistributionDescription, true);
            this.labelDistributionDescription.Location = new System.Drawing.Point(3, 0);
            this.labelDistributionDescription.Name = "labelDistributionDescription";
            this.labelDistributionDescription.Size = new System.Drawing.Size(506, 30);
            this.labelDistributionDescription.TabIndex = 0;
            this.labelDistributionDescription.Text = "Determine how many cars will be spawn during  the simulation. The distribution wi" +
    "ll be spread uniformly throughout the duration of the simulation.";
            // 
            // labelDistributionDetail
            // 
            this.labelDistributionDetail.AutoSize = true;
            this.labelDistributionDetail.Location = new System.Drawing.Point(3, 30);
            this.labelDistributionDetail.Name = "labelDistributionDetail";
            this.labelDistributionDetail.Size = new System.Drawing.Size(104, 15);
            this.labelDistributionDetail.TabIndex = 0;
            this.labelDistributionDetail.Text = "Distribution detail:";
            // 
            // comboBoxSpawnRateDetail
            // 
            this.comboBoxSpawnRateDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpawnRateDetail.FormattingEnabled = true;
            this.comboBoxSpawnRateDetail.Location = new System.Drawing.Point(113, 33);
            this.comboBoxSpawnRateDetail.Name = "comboBoxSpawnRateDetail";
            this.comboBoxSpawnRateDetail.Size = new System.Drawing.Size(120, 23);
            this.comboBoxSpawnRateDetail.TabIndex = 1;
            this.comboBoxSpawnRateDetail.SelectedIndexChanged += new System.EventHandler(this.comboBoxSpawnRateDetail_SelectedIndexChanged);
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.tableLayoutPanelParameters);
            this.groupBoxParameters.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxParameters.Location = new System.Drawing.Point(5, 219);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(560, 83);
            this.groupBoxParameters.TabIndex = 0;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Simulation parameters";
            // 
            // tableLayoutPanelParameters
            // 
            this.tableLayoutPanelParameters.ColumnCount = 2;
            this.tableLayoutPanelParameters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanelParameters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelParameters.Controls.Add(this.trackBarNavigation, 1, 1);
            this.tableLayoutPanelParameters.Controls.Add(this.labelDuration, 0, 0);
            this.tableLayoutPanelParameters.Controls.Add(this.labelNavigation, 0, 1);
            this.tableLayoutPanelParameters.Controls.Add(this.trackBarDuration, 1, 0);
            this.tableLayoutPanelParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelParameters.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanelParameters.Name = "tableLayoutPanelParameters";
            this.tableLayoutPanelParameters.RowCount = 2;
            this.tableLayoutPanelParameters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelParameters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelParameters.Size = new System.Drawing.Size(554, 61);
            this.tableLayoutPanelParameters.TabIndex = 0;
            // 
            // trackBarNavigation
            // 
            this.trackBarNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarNavigation.Location = new System.Drawing.Point(153, 33);
            this.trackBarNavigation.Maximum = 100;
            this.trackBarNavigation.Name = "trackBarNavigation";
            this.trackBarNavigation.Size = new System.Drawing.Size(398, 25);
            this.trackBarNavigation.TabIndex = 3;
            this.trackBarNavigation.Value = 50;
            this.trackBarNavigation.Scroll += new System.EventHandler(this.trackBarNavigation_Scroll);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDuration.Location = new System.Drawing.Point(3, 0);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(144, 30);
            this.labelDuration.TabIndex = 0;
            this.labelDuration.Text = "Duration: 1h";
            this.labelDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelNavigation
            // 
            this.labelNavigation.AutoSize = true;
            this.labelNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNavigation.Location = new System.Drawing.Point(3, 30);
            this.labelNavigation.Name = "labelNavigation";
            this.labelNavigation.Size = new System.Drawing.Size(144, 31);
            this.labelNavigation.TabIndex = 0;
            this.labelNavigation.Text = "Navigation rate: 50 %";
            this.labelNavigation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackBarDuration
            // 
            this.trackBarDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarDuration.LargeChange = 2;
            this.trackBarDuration.Location = new System.Drawing.Point(153, 3);
            this.trackBarDuration.Maximum = 19;
            this.trackBarDuration.Name = "trackBarDuration";
            this.trackBarDuration.Size = new System.Drawing.Size(398, 24);
            this.trackBarDuration.TabIndex = 2;
            this.trackBarDuration.Scroll += new System.EventHandler(this.trackBarDuration_Scroll);
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelButtons.ColumnCount = 2;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Controls.Add(this.buttonCancel, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonSimulate, 1, 0);
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(334, 303);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 1;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(231, 31);
            this.tableLayoutPanelButtons.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(3, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(109, 25);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSimulate.Location = new System.Drawing.Point(118, 3);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(110, 25);
            this.buttonSimulate.TabIndex = 5;
            this.buttonSimulate.Text = "Start Simulation";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // FormSimulationSettings
            // 
            this.AcceptButton = this.buttonSimulate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(570, 338);
            this.Controls.Add(this.tableLayoutPanelButtons);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.groupBoxSpawnRate);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormSimulationSettings";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Road Traffic Simulator - Simulation Settings";
            this.groupBoxSpawnRate.ResumeLayout(false);
            this.groupBoxSpawnRate.PerformLayout();
            this.flowLayoutPanelDistribution.ResumeLayout(false);
            this.flowLayoutPanelDistribution.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.tableLayoutPanelParameters.ResumeLayout(false);
            this.tableLayoutPanelParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNavigation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).EndInit();
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxSpawnRate;
        private System.Windows.Forms.ComboBox comboBoxSpawnRateDetail;
        private System.Windows.Forms.Panel panelSpawnRate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDistribution;
        private System.Windows.Forms.Label labelDistributionDetail;
        private System.Windows.Forms.Label labelDistributionDescription;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelParameters;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.Label labelNavigation;
        private System.Windows.Forms.TrackBar trackBarNavigation;
        private System.Windows.Forms.TrackBar trackBarDuration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSimulate;
    }
}