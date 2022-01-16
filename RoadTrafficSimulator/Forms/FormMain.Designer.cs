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
            this.tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonMode = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.buildPanel = new RoadTrafficSimulator.Forms.BuildPanel();
            this.simulationPanel = new RoadTrafficSimulator.Forms.SimulationPanel();
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
            this.buttonCenter.Location = new System.Drawing.Point(2, 2);
            this.buttonCenter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(187, 35);
            this.buttonCenter.TabIndex = 1;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoom.Location = new System.Drawing.Point(2, 41);
            this.buttonZoom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(187, 35);
            this.buttonZoom.TabIndex = 2;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxInfo.Location = new System.Drawing.Point(5, 546);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxInfo.Size = new System.Drawing.Size(383, 152);
            this.groupBoxInfo.TabIndex = 8;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(3, 24);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(377, 124);
            this.labelInfo.TabIndex = 0;
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.mapPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPanel.Location = new System.Drawing.Point(0, 0);
            this.mapPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(640, 782);
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
            this.splitContainer.Location = new System.Drawing.Point(5, 6);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.mapPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBoxInfo);
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutButtons);
            this.splitContainer.Panel2.Controls.Add(this.buildPanel);
            this.splitContainer.Panel2.Controls.Add(this.simulationPanel);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.splitContainer.Size = new System.Drawing.Size(1044, 786);
            this.splitContainer.SplitterDistance = 644;
            this.splitContainer.SplitterWidth = 3;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.TabStop = false;
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
            this.tableLayoutButtons.Location = new System.Drawing.Point(5, 698);
            this.tableLayoutButtons.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutButtons.Name = "tableLayoutButtons";
            this.tableLayoutButtons.RowCount = 2;
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.Size = new System.Drawing.Size(383, 78);
            this.tableLayoutButtons.TabIndex = 0;
            // 
            // buttonMode
            // 
            this.buttonMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMode.Location = new System.Drawing.Point(194, 4);
            this.buttonMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonMode.Name = "buttonMode";
            this.buttonMode.Size = new System.Drawing.Size(186, 31);
            this.buttonMode.TabIndex = 3;
            this.buttonMode.Text = "Build Map";
            this.buttonMode.UseVisualStyleBackColor = true;
            this.buttonMode.Click += new System.EventHandler(this.buttonMode_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSimulate.Location = new System.Drawing.Point(194, 41);
            this.buttonSimulate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(186, 35);
            this.buttonSimulate.TabIndex = 4;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // buildPanel
            // 
            this.buildPanel.AutoSize = true;
            this.buildPanel.CurrentMode = RoadTrafficSimulator.Forms.BuildPanel.Mode.Build;
            this.buildPanel.CurrentRoadSide = RoadTrafficSimulator.Forms.BuildPanel.RoadSide.Right;
            this.buildPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buildPanel.Length = 100;
            this.buildPanel.Location = new System.Drawing.Point(5, 184);
            this.buildPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buildPanel.MaxSpeed = 50;
            this.buildPanel.MinimumSize = new System.Drawing.Size(189, 0);
            this.buildPanel.Name = "buildPanel";
            this.buildPanel.Size = new System.Drawing.Size(383, 191);
            this.buildPanel.TabIndex = 9;
            this.buildPanel.CloseRoadClick += new System.EventHandler(this.buildPanel_CloseRoadClick);
            this.buildPanel.OpenRoadClick += new System.EventHandler(this.buildPanel_OpenRoadClick);
            this.buildPanel.DestroyRoadClick += new System.EventHandler(this.buildPanel_DestroyRoadClick);
            this.buildPanel.TrafficLightClick += new System.EventHandler(this.buildPanel_TrafficLightClick);
            this.buildPanel.DestroyCrossroadClick += new System.EventHandler(this.buildPanel_DestroyCrossroadClick);
            this.buildPanel.SaveMapClick += new System.EventHandler(this.buildPanel_SaveMapClick);
            this.buildPanel.LoadMapClick += new System.EventHandler(this.buildPanel_LoadMapClick);
            this.buildPanel.CurrentRoadSideChanged += new System.EventHandler(this.buildPanel_CurrentRoadSideChanged);
            this.buildPanel.CurrentModeChanged += new System.EventHandler(this.buildPanel_CurrentModeChanged);
            this.buildPanel.LengthChanged += new System.EventHandler(this.buildPanel_LengthChanged);
            this.buildPanel.MaxSpeedChanged += new System.EventHandler(this.buildPanel_MaxSpeedChanged);
            this.buildPanel.SpawnRateChanged += new System.EventHandler(this.buildPanel_SpawnRateChanged);
            // 
            // simulationPanel
            // 
            this.simulationPanel.AutoSize = true;
            this.simulationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.simulationPanel.Location = new System.Drawing.Point(5, 6);
            this.simulationPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.simulationPanel.Name = "simulationPanel";
            this.simulationPanel.Size = new System.Drawing.Size(383, 178);
            this.simulationPanel.TabIndex = 10;
            // 
            // timerSimulation
            // 
            this.timerSimulation.Interval = 300;
            this.timerSimulation.Tick += new System.EventHandler(this.timerSimulation_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 798);
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
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

