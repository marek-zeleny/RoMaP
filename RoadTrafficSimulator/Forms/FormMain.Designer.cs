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
            this.buildPanel = new RoadTrafficSimulator.Forms.BuildPanel();
            this.tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonMode = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
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
            this.buttonCenter.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(369, 20);
            this.buttonCenter.TabIndex = 0;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoom.Location = new System.Drawing.Point(375, 26);
            this.buttonZoom.Margin = new System.Windows.Forms.Padding(2);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(369, 20);
            this.buttonZoom.TabIndex = 3;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxInfo.Location = new System.Drawing.Point(6, 708);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Size = new System.Drawing.Size(746, 190);
            this.groupBoxInfo.TabIndex = 3;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(4, 29);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(738, 156);
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
            this.mapPanel.Size = new System.Drawing.Size(796, 1004);
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
            this.splitContainer.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(6, 8);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.mapPanel);
            this.splitContainer.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.AutoScroll = true;
            this.splitContainer.Panel2.Controls.Add(this.buildPanel);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxInfo);
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutButtons);
            this.splitContainer.Panel2.Controls.Add(this.simulationPanel);
            this.splitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.splitContainer.Size = new System.Drawing.Size(1566, 1008);
            this.splitContainer.SplitterDistance = 800;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.TabStop = false;
            // 
            // buildPanel
            // 
            this.buildPanel.AutoSize = true;
            this.buildPanel.CurrentMode = RoadTrafficSimulator.Forms.BuildPanel.Mode.Build;
            this.buildPanel.CurrentRoadSide = RoadTrafficSimulator.GUI.RoadSide.Right;
            this.buildPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buildPanel.Lanes = 1;
            this.buildPanel.Length = 100;
            this.buildPanel.Location = new System.Drawing.Point(6, 276);
            this.buildPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buildPanel.MaxSpeed = 50;
            this.buildPanel.MinimumSize = new System.Drawing.Size(236, 0);
            this.buildPanel.Name = "buildPanel";
            this.buildPanel.Size = new System.Drawing.Size(746, 232);
            this.buildPanel.TabIndex = 2;
            this.buildPanel.CloseRoadClick += new System.EventHandler(this.buildPanel_CloseRoadClick);
            this.buildPanel.OpenRoadClick += new System.EventHandler(this.buildPanel_OpenRoadClick);
            this.buildPanel.DestroyRoadClick += new System.EventHandler(this.buildPanel_DestroyRoadClick);
            this.buildPanel.DestroyCrossroadClick += new System.EventHandler(this.buildPanel_DestroyCrossroadClick);
            this.buildPanel.SaveMapClick += new System.EventHandler(this.buildPanel_SaveMapClick);
            this.buildPanel.LoadMapClick += new System.EventHandler(this.buildPanel_LoadMapClick);
            this.buildPanel.TrafficLightMapClick += new System.EventHandler(this.buildPanel_TrafficLightMapClick);
            this.buildPanel.CurrentRoadSideChanged += new System.EventHandler(this.buildPanel_CurrentRoadSideChanged);
            this.buildPanel.CurrentModeChanged += new System.EventHandler(this.buildPanel_CurrentModeChanged);
            this.buildPanel.LanesChanged += new System.EventHandler(this.buildPanel_LanesChanged);
            this.buildPanel.LengthChanged += new System.EventHandler(this.buildPanel_LengthChanged);
            this.buildPanel.MaxSpeedChanged += new System.EventHandler(this.buildPanel_MaxSpeedChanged);
            this.buildPanel.SpawnRateChanged += new System.EventHandler(this.buildPanel_SpawnRateChanged);
            // 
            // tableLayoutButtons
            // 
            this.tableLayoutButtons.ColumnCount = 2;
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.Controls.Add(this.buttonContinue, 1, 1);
            this.tableLayoutButtons.Controls.Add(this.buttonStop, 1, 0);
            this.tableLayoutButtons.Controls.Add(this.buttonPause, 1, 1);
            this.tableLayoutButtons.Controls.Add(this.buttonMode, 1, 0);
            this.tableLayoutButtons.Controls.Add(this.buttonZoom, 0, 1);
            this.tableLayoutButtons.Controls.Add(this.buttonCenter, 0, 0);
            this.tableLayoutButtons.Controls.Add(this.buttonSimulate, 1, 1);
            this.tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutButtons.Location = new System.Drawing.Point(6, 898);
            this.tableLayoutButtons.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutButtons.Name = "tableLayoutButtons";
            this.tableLayoutButtons.RowCount = 2;
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutButtons.Size = new System.Drawing.Size(746, 98);
            this.tableLayoutButtons.TabIndex = 4;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonContinue.Location = new System.Drawing.Point(2, 75);
            this.buttonContinue.Margin = new System.Windows.Forms.Padding(2);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(369, 21);
            this.buttonContinue.TabIndex = 6;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Visible = false;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStop.Location = new System.Drawing.Point(2, 26);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(369, 20);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Visible = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPause.Location = new System.Drawing.Point(375, 50);
            this.buttonPause.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(369, 21);
            this.buttonPause.TabIndex = 5;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Visible = false;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonMode
            // 
            this.buttonMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMode.Location = new System.Drawing.Point(375, 2);
            this.buttonMode.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMode.Name = "buttonMode";
            this.buttonMode.Size = new System.Drawing.Size(369, 20);
            this.buttonMode.TabIndex = 1;
            this.buttonMode.Text = "Build Map";
            this.buttonMode.UseVisualStyleBackColor = true;
            this.buttonMode.Click += new System.EventHandler(this.buttonMode_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSimulate.Location = new System.Drawing.Point(2, 50);
            this.buttonSimulate.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(369, 21);
            this.buttonSimulate.TabIndex = 4;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // simulationPanel
            // 
            this.simulationPanel.AutoSize = true;
            this.simulationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.simulationPanel.Location = new System.Drawing.Point(6, 8);
            this.simulationPanel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.simulationPanel.Name = "simulationPanel";
            this.simulationPanel.Size = new System.Drawing.Size(746, 268);
            this.simulationPanel.TabIndex = 1;
            this.simulationPanel.StatisticsClick += new System.EventHandler(this.simulationPanel_StatisticsClick);
            // 
            // timerSimulation
            // 
            this.timerSimulation.Interval = 300;
            this.timerSimulation.Tick += new System.EventHandler(this.timerSimulation_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 1024);
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RoMaP - Simulation";
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
        private SimulationPanel simulationPanel;
        private System.Windows.Forms.Button buttonMode;
        private System.Windows.Forms.Timer timerSimulation;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonContinue;
        private BuildPanel buildPanel;
    }
}

