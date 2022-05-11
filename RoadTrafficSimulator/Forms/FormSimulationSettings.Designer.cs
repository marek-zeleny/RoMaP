
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSimulationSettings));
            this.groupBoxSpawnFrequency = new System.Windows.Forms.GroupBox();
            this.panelSpawnFrequency = new System.Windows.Forms.Panel();
            this.flowLayoutPanelDistribution = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDistributionDescription = new System.Windows.Forms.Label();
            this.labelDistributionDetail = new System.Windows.Forms.Label();
            this.comboBoxSpawnFrequencyDetail = new System.Windows.Forms.ComboBox();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelParameters = new System.Windows.Forms.TableLayoutPanel();
            this.trackBarSpawnFrequency = new System.Windows.Forms.TrackBar();
            this.trackBarNavigation = new System.Windows.Forms.TrackBar();
            this.labelDuration = new System.Windows.Forms.Label();
            this.labelNavigation = new System.Windows.Forms.Label();
            this.trackBarDuration = new System.Windows.Forms.TrackBar();
            this.labelSpawnFrequency = new System.Windows.Forms.Label();
            this.labelStatisticsDetail = new System.Windows.Forms.Label();
            this.comboBoxStatisticsDetail = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.groupBoxSpawnFrequency.SuspendLayout();
            this.flowLayoutPanelDistribution.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.tableLayoutPanelParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpawnFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNavigation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).BeginInit();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSpawnFrequency
            // 
            this.groupBoxSpawnFrequency.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxSpawnFrequency.Controls.Add(this.panelSpawnFrequency);
            this.groupBoxSpawnFrequency.Controls.Add(this.flowLayoutPanelDistribution);
            this.groupBoxSpawnFrequency.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSpawnFrequency.Location = new System.Drawing.Point(6, 238);
            this.groupBoxSpawnFrequency.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSpawnFrequency.Name = "groupBoxSpawnFrequency";
            this.groupBoxSpawnFrequency.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSpawnFrequency.Size = new System.Drawing.Size(856, 358);
            this.groupBoxSpawnFrequency.TabIndex = 0;
            this.groupBoxSpawnFrequency.TabStop = false;
            this.groupBoxSpawnFrequency.Text = "Car spawn frequency distribution";
            // 
            // panelSpawnFrequency
            // 
            this.panelSpawnFrequency.AutoScroll = true;
            this.panelSpawnFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpawnFrequency.Location = new System.Drawing.Point(4, 147);
            this.panelSpawnFrequency.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelSpawnFrequency.Name = "panelSpawnFrequency";
            this.panelSpawnFrequency.Size = new System.Drawing.Size(848, 206);
            this.panelSpawnFrequency.TabIndex = 3;
            // 
            // flowLayoutPanelDistribution
            // 
            this.flowLayoutPanelDistribution.AutoSize = true;
            this.flowLayoutPanelDistribution.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelDistribution.Controls.Add(this.labelDistributionDescription);
            this.flowLayoutPanelDistribution.Controls.Add(this.labelDistributionDetail);
            this.flowLayoutPanelDistribution.Controls.Add(this.comboBoxSpawnFrequencyDetail);
            this.flowLayoutPanelDistribution.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelDistribution.Location = new System.Drawing.Point(4, 29);
            this.flowLayoutPanelDistribution.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanelDistribution.Name = "flowLayoutPanelDistribution";
            this.flowLayoutPanelDistribution.Size = new System.Drawing.Size(848, 118);
            this.flowLayoutPanelDistribution.TabIndex = 0;
            // 
            // labelDistributionDescription
            // 
            this.labelDistributionDescription.AutoSize = true;
            this.flowLayoutPanelDistribution.SetFlowBreak(this.labelDistributionDescription, true);
            this.labelDistributionDescription.Location = new System.Drawing.Point(4, 0);
            this.labelDistributionDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDistributionDescription.Name = "labelDistributionDescription";
            this.labelDistributionDescription.Size = new System.Drawing.Size(839, 75);
            this.labelDistributionDescription.TabIndex = 0;
            this.labelDistributionDescription.Text = resources.GetString("labelDistributionDescription.Text");
            // 
            // labelDistributionDetail
            // 
            this.labelDistributionDetail.AutoSize = true;
            this.labelDistributionDetail.Location = new System.Drawing.Point(4, 75);
            this.labelDistributionDetail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDistributionDetail.Name = "labelDistributionDetail";
            this.labelDistributionDetail.Size = new System.Drawing.Size(157, 25);
            this.labelDistributionDetail.TabIndex = 1;
            this.labelDistributionDetail.Text = "Distribution detail:";
            // 
            // comboBoxSpawnFrequencyDetail
            // 
            this.comboBoxSpawnFrequencyDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpawnFrequencyDetail.FormattingEnabled = true;
            this.comboBoxSpawnFrequencyDetail.Location = new System.Drawing.Point(169, 80);
            this.comboBoxSpawnFrequencyDetail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxSpawnFrequencyDetail.Name = "comboBoxSpawnFrequencyDetail";
            this.comboBoxSpawnFrequencyDetail.Size = new System.Drawing.Size(170, 33);
            this.comboBoxSpawnFrequencyDetail.TabIndex = 2;
            this.comboBoxSpawnFrequencyDetail.SelectedIndexChanged += new System.EventHandler(this.comboBoxSpawnFrequencyDetail_SelectedIndexChanged);
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.tableLayoutPanelParameters);
            this.groupBoxParameters.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxParameters.Location = new System.Drawing.Point(6, 8);
            this.groupBoxParameters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxParameters.Size = new System.Drawing.Size(856, 230);
            this.groupBoxParameters.TabIndex = 1;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Simulation parameters";
            // 
            // tableLayoutPanelParameters
            // 
            this.tableLayoutPanelParameters.ColumnCount = 2;
            this.tableLayoutPanelParameters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanelParameters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelParameters.Controls.Add(this.trackBarSpawnFrequency, 1, 3);
            this.tableLayoutPanelParameters.Controls.Add(this.trackBarNavigation, 1, 2);
            this.tableLayoutPanelParameters.Controls.Add(this.labelDuration, 0, 1);
            this.tableLayoutPanelParameters.Controls.Add(this.labelNavigation, 0, 2);
            this.tableLayoutPanelParameters.Controls.Add(this.trackBarDuration, 1, 1);
            this.tableLayoutPanelParameters.Controls.Add(this.labelSpawnFrequency, 0, 3);
            this.tableLayoutPanelParameters.Controls.Add(this.labelStatisticsDetail, 0, 0);
            this.tableLayoutPanelParameters.Controls.Add(this.comboBoxStatisticsDetail, 1, 0);
            this.tableLayoutPanelParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelParameters.Location = new System.Drawing.Point(4, 29);
            this.tableLayoutPanelParameters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanelParameters.Name = "tableLayoutPanelParameters";
            this.tableLayoutPanelParameters.RowCount = 4;
            this.tableLayoutPanelParameters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanelParameters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanelParameters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tableLayoutPanelParameters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tableLayoutPanelParameters.Size = new System.Drawing.Size(848, 196);
            this.tableLayoutPanelParameters.TabIndex = 0;
            // 
            // trackBarSpawnFrequency
            // 
            this.trackBarSpawnFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarSpawnFrequency.Location = new System.Drawing.Point(264, 151);
            this.trackBarSpawnFrequency.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarSpawnFrequency.Maximum = 100;
            this.trackBarSpawnFrequency.Minimum = 1;
            this.trackBarSpawnFrequency.Name = "trackBarSpawnFrequency";
            this.trackBarSpawnFrequency.Size = new System.Drawing.Size(580, 40);
            this.trackBarSpawnFrequency.TabIndex = 5;
            this.trackBarSpawnFrequency.Value = 10;
            this.trackBarSpawnFrequency.Scroll += new System.EventHandler(this.trackBarSpawnFrequency_Scroll);
            // 
            // trackBarNavigation
            // 
            this.trackBarNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarNavigation.Location = new System.Drawing.Point(264, 102);
            this.trackBarNavigation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarNavigation.Maximum = 100;
            this.trackBarNavigation.Name = "trackBarNavigation";
            this.trackBarNavigation.Size = new System.Drawing.Size(580, 39);
            this.trackBarNavigation.TabIndex = 3;
            this.trackBarNavigation.Value = 50;
            this.trackBarNavigation.Scroll += new System.EventHandler(this.trackBarNavigation_Scroll);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDuration.Location = new System.Drawing.Point(4, 48);
            this.labelDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(252, 49);
            this.labelDuration.TabIndex = 0;
            this.labelDuration.Text = "Duration: 1h";
            this.labelDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelNavigation
            // 
            this.labelNavigation.AutoSize = true;
            this.labelNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNavigation.Location = new System.Drawing.Point(4, 97);
            this.labelNavigation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNavigation.Name = "labelNavigation";
            this.labelNavigation.Size = new System.Drawing.Size(252, 49);
            this.labelNavigation.TabIndex = 2;
            this.labelNavigation.Text = "Navigation rate: 50 %";
            this.labelNavigation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackBarDuration
            // 
            this.trackBarDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarDuration.LargeChange = 2;
            this.trackBarDuration.Location = new System.Drawing.Point(264, 53);
            this.trackBarDuration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarDuration.Maximum = 19;
            this.trackBarDuration.Name = "trackBarDuration";
            this.trackBarDuration.Size = new System.Drawing.Size(580, 39);
            this.trackBarDuration.TabIndex = 1;
            this.trackBarDuration.Scroll += new System.EventHandler(this.trackBarDuration_Scroll);
            // 
            // labelSpawnFrequency
            // 
            this.labelSpawnFrequency.AutoSize = true;
            this.labelSpawnFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpawnFrequency.Location = new System.Drawing.Point(2, 146);
            this.labelSpawnFrequency.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSpawnFrequency.Name = "labelSpawnFrequency";
            this.labelSpawnFrequency.Size = new System.Drawing.Size(256, 50);
            this.labelSpawnFrequency.TabIndex = 4;
            this.labelSpawnFrequency.Text = "Car spawn frequency: 10 %";
            this.labelSpawnFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStatisticsDetail
            // 
            this.labelStatisticsDetail.AutoSize = true;
            this.labelStatisticsDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatisticsDetail.Location = new System.Drawing.Point(2, 0);
            this.labelStatisticsDetail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStatisticsDetail.Name = "labelStatisticsDetail";
            this.labelStatisticsDetail.Size = new System.Drawing.Size(256, 48);
            this.labelStatisticsDetail.TabIndex = 6;
            this.labelStatisticsDetail.Text = "Statistics detail level:";
            this.labelStatisticsDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxStatisticsDetail
            // 
            this.comboBoxStatisticsDetail.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxStatisticsDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatisticsDetail.FormattingEnabled = true;
            this.comboBoxStatisticsDetail.Location = new System.Drawing.Point(262, 2);
            this.comboBoxStatisticsDetail.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxStatisticsDetail.Name = "comboBoxStatisticsDetail";
            this.comboBoxStatisticsDetail.Size = new System.Drawing.Size(182, 33);
            this.comboBoxStatisticsDetail.TabIndex = 7;
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelButtons.ColumnCount = 2;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Controls.Add(this.buttonCancel, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonSimulate, 1, 0);
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(526, 600);
            this.tableLayoutPanelButtons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 1;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(330, 52);
            this.tableLayoutPanelButtons.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(4, 5);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(157, 42);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSimulate.Location = new System.Drawing.Point(169, 5);
            this.buttonSimulate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(157, 42);
            this.buttonSimulate.TabIndex = 1;
            this.buttonSimulate.Text = "Start Simulation";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // FormSimulationSettings
            // 
            this.AcceptButton = this.buttonSimulate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(868, 666);
            this.Controls.Add(this.tableLayoutPanelButtons);
            this.Controls.Add(this.groupBoxSpawnFrequency);
            this.Controls.Add(this.groupBoxParameters);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "FormSimulationSettings";
            this.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RoMaP - Simulation Settings";
            this.groupBoxSpawnFrequency.ResumeLayout(false);
            this.groupBoxSpawnFrequency.PerformLayout();
            this.flowLayoutPanelDistribution.ResumeLayout(false);
            this.flowLayoutPanelDistribution.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.tableLayoutPanelParameters.ResumeLayout(false);
            this.tableLayoutPanelParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpawnFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNavigation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).EndInit();
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxSpawnFrequency;
        private System.Windows.Forms.ComboBox comboBoxSpawnFrequencyDetail;
        private System.Windows.Forms.Panel panelSpawnFrequency;
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
        private System.Windows.Forms.TrackBar trackBarSpawnFrequency;
        private System.Windows.Forms.Label labelSpawnFrequency;
        private System.Windows.Forms.Label labelStatisticsDetail;
        private System.Windows.Forms.ComboBox comboBoxStatisticsDetail;
    }
}