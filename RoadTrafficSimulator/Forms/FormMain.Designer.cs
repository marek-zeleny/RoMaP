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
            this.components = new System.ComponentModel.Container();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonZoom = new System.Windows.Forms.Button();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.mapPanel = new RoadTrafficSimulator.Forms.MapPanel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.simulationPanel = new RoadTrafficSimulator.Forms.SimulationPanel();
            this.buildPanel = new RoadTrafficSimulator.Forms.BuildPanel();
            this.tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonMode = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.timerSimulation = new System.Windows.Forms.Timer(this.components);
            this.groupBoxInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCenter
            // 
            this.buttonCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCenter.Location = new System.Drawing.Point(3, 3);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(186, 42);
            this.buttonCenter.TabIndex = 1;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoom.Location = new System.Drawing.Point(3, 51);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(186, 43);
            this.buttonZoom.TabIndex = 2;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxInfo.Location = new System.Drawing.Point(6, 685);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Size = new System.Drawing.Size(384, 190);
            this.groupBoxInfo.TabIndex = 8;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(4, 29);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(376, 156);
            this.labelInfo.TabIndex = 0;
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.mapPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPanel.Location = new System.Drawing.Point(0, 0);
            this.mapPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(897, 979);
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
            this.splitContainer.Location = new System.Drawing.Point(6, 7);
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
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutButtons);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.splitContainer.Size = new System.Drawing.Size(1305, 983);
            this.splitContainer.SplitterDistance = 901;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.TabStop = false;
            // 
            // simulationPanel
            // 
            this.simulationPanel.AutoSize = true;
            this.simulationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.simulationPanel.Location = new System.Drawing.Point(6, 151);
            this.simulationPanel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.simulationPanel.Name = "simulationPanel";
            this.simulationPanel.Size = new System.Drawing.Size(384, 0);
            this.simulationPanel.TabIndex = 10;
            // 
            // buildPanel
            // 
            this.buildPanel.AutoSize = true;
            this.buildPanel.CurrentMode = RoadTrafficSimulator.Forms.BuildPanel.Mode.Build;
            this.buildPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buildPanel.Location = new System.Drawing.Point(6, 7);
            this.buildPanel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.buildPanel.MaxSpeed = 50;
            this.buildPanel.MinimumSize = new System.Drawing.Size(236, 0);
            this.buildPanel.Name = "buildPanel";
            this.buildPanel.Size = new System.Drawing.Size(384, 144);
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
            // tableLayoutButtons
            // 
            this.tableLayoutButtons.ColumnCount = 2;
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.Controls.Add(this.buttonMode, 1, 0);
            this.tableLayoutButtons.Controls.Add(this.buttonZoom, 0, 1);
            this.tableLayoutButtons.Controls.Add(this.buttonCenter, 0, 0);
            this.tableLayoutButtons.Controls.Add(this.buttonSimulate, 1, 1);
            this.tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutButtons.Location = new System.Drawing.Point(6, 875);
            this.tableLayoutButtons.Name = "tableLayoutButtons";
            this.tableLayoutButtons.RowCount = 2;
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.Size = new System.Drawing.Size(384, 97);
            this.tableLayoutButtons.TabIndex = 0;
            // 
            // buttonMode
            // 
            this.buttonMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMode.Location = new System.Drawing.Point(196, 5);
            this.buttonMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonMode.Name = "buttonMode";
            this.buttonMode.Size = new System.Drawing.Size(184, 38);
            this.buttonMode.TabIndex = 3;
            this.buttonMode.Text = "Build Map";
            this.buttonMode.UseVisualStyleBackColor = true;
            this.buttonMode.Click += new System.EventHandler(this.buttonMode_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSimulate.Location = new System.Drawing.Point(196, 51);
            this.buttonSimulate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(184, 43);
            this.buttonSimulate.TabIndex = 4;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // timerSimulation
            // 
            this.timerSimulation.Tick += new System.EventHandler(this.timerSimulation_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 997);
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Road Traffic Simulator";
            this.groupBoxInfo.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.Button buttonZoom;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelInfo;
        private MapPanel mapPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutButtons;
        private System.Windows.Forms.Button buttonSimulate;
        private BuildPanel buildPanel;
        private SimulationPanel simulationPanel;
        private System.Windows.Forms.Button buttonMode;
        private System.Windows.Forms.Timer timerSimulation;
    }
}

