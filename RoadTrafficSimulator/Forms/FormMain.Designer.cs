namespace RoadTrafficSimulator.Forms
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonZoom = new System.Windows.Forms.Button();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.mapPanel = new RoadTrafficSimulator.Forms.MapPanel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.simulationPanel = new RoadTrafficSimulator.Forms.SimulationPanel();
            this.buildPanel = new RoadTrafficSimulator.Forms.BuildPanel();
            this.tableLayoutPanelMapButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonMode = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.groupBoxInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanelMapButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCenter
            // 
            this.buttonCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCenter.Location = new System.Drawing.Point(2, 2);
            this.buttonCenter.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(190, 25);
            this.buttonCenter.TabIndex = 1;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoom.Location = new System.Drawing.Point(2, 31);
            this.buttonZoom.Margin = new System.Windows.Forms.Padding(2);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(190, 25);
            this.buttonZoom.TabIndex = 2;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxInfo.Location = new System.Drawing.Point(4, 410);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(389, 114);
            this.groupBoxInfo.TabIndex = 8;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(3, 19);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(383, 92);
            this.labelInfo.TabIndex = 0;
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.mapPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPanel.Location = new System.Drawing.Point(0, 0);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(506, 586);
            this.mapPanel.TabIndex = 0;
            this.mapPanel.Zoom = 1F;
            this.mapPanel.ZoomChanged += new System.EventHandler(this.mapPanel_ZoomChanged);
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mapPanel_Paint);
            this.mapPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseClick);
            this.mapPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseDoubleClick);
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(4, 4);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.mapPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.simulationPanel);
            this.splitContainer.Panel2.Controls.Add(this.buildPanel);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxInfo);
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanelMapButtons);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(4);
            this.splitContainer.Size = new System.Drawing.Size(914, 590);
            this.splitContainer.SplitterDistance = 510;
            this.splitContainer.SplitterWidth = 3;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.TabStop = false;
            // 
            // simulationPanel
            // 
            this.simulationPanel.AutoSize = true;
            this.simulationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.simulationPanel.Location = new System.Drawing.Point(4, 97);
            this.simulationPanel.Name = "simulationPanel";
            this.simulationPanel.Size = new System.Drawing.Size(389, 0);
            this.simulationPanel.TabIndex = 10;
            // 
            // buildPanel
            // 
            this.buildPanel.AutoSize = true;
            this.buildPanel.CurrentMode = RoadTrafficSimulator.Forms.BuildPanel.Mode.Build;
            this.buildPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buildPanel.Location = new System.Drawing.Point(4, 4);
            this.buildPanel.MaxSpeed = 50;
            this.buildPanel.MinimumSize = new System.Drawing.Size(165, 0);
            this.buildPanel.Name = "buildPanel";
            this.buildPanel.Size = new System.Drawing.Size(389, 93);
            this.buildPanel.TabIndex = 9;
            this.buildPanel.DestroyRoadClick += new System.EventHandler(this.buildPanel_DestroyRoadClick);
            this.buildPanel.TrafficLightClick += new System.EventHandler(this.buildPanel_TrafficLightClick);
            this.buildPanel.DestroyCrossroadClick += new System.EventHandler(this.buildPanel_DestroyCrossroadClick);
            this.buildPanel.SaveMapClick += new System.EventHandler(this.buildPanel_SaveMapClick);
            this.buildPanel.LoadMapClick += new System.EventHandler(this.buildPanel_LoadMapClick);
            this.buildPanel.CurrentModeChanged += new System.EventHandler(this.buildPanel_CurrentModeChanged);
            this.buildPanel.MaxSpeedChanged += new System.EventHandler(this.buildPanel_MaxSpeedChanged);
            this.buildPanel.SpawnRateChanged += new System.EventHandler(this.buildPanel_SpawnRateChanged);
            // 
            // tableLayoutPanelMapButtons
            // 
            this.tableLayoutPanelMapButtons.ColumnCount = 2;
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonMode, 1, 0);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonZoom, 0, 1);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonCenter, 0, 0);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonSimulate, 1, 1);
            this.tableLayoutPanelMapButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelMapButtons.Location = new System.Drawing.Point(4, 524);
            this.tableLayoutPanelMapButtons.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanelMapButtons.Name = "tableLayoutPanelMapButtons";
            this.tableLayoutPanelMapButtons.RowCount = 2;
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.Size = new System.Drawing.Size(389, 58);
            this.tableLayoutPanelMapButtons.TabIndex = 0;
            // 
            // buttonMode
            // 
            this.buttonMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMode.Location = new System.Drawing.Point(197, 3);
            this.buttonMode.Name = "buttonMode";
            this.buttonMode.Size = new System.Drawing.Size(189, 23);
            this.buttonMode.TabIndex = 3;
            this.buttonMode.Text = "Build Map";
            this.buttonMode.UseVisualStyleBackColor = true;
            this.buttonMode.Click += new System.EventHandler(this.buttonMode_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSimulate.Location = new System.Drawing.Point(197, 31);
            this.buttonSimulate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(189, 25);
            this.buttonSimulate.TabIndex = 4;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 598);
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Road Traffic Simulator";
            this.groupBoxInfo.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanelMapButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.Button buttonZoom;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelInfo;
        private MapPanel mapPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMapButtons;
        private System.Windows.Forms.Button buttonSimulate;
        private BuildPanel buildPanel;
        private SimulationPanel simulationPanel;
        private System.Windows.Forms.Button buttonMode;
    }
}

