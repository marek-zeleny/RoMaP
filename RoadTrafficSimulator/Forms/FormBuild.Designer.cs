
namespace RoadTrafficSimulator.Forms
{
    partial class FormBuild
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
            this.mapPanel = new RoadTrafficSimulator.Forms.MapPanel();
            this.groupBoxRoad = new System.Windows.Forms.GroupBox();
            this.buttonDestroyRoad = new System.Windows.Forms.Button();
            this.flowLayoutPanelMaxSpeed = new System.Windows.Forms.FlowLayoutPanel();
            this.labelMaxSpeed = new System.Windows.Forms.Label();
            this.numericUpDownMaxSpeed = new System.Windows.Forms.NumericUpDown();
            this.labelMps = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTwoWayRoad = new System.Windows.Forms.Label();
            this.panelControls = new System.Windows.Forms.Panel();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanelMapButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLoadMap = new System.Windows.Forms.Button();
            this.buttonSaveMap = new System.Windows.Forms.Button();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonZoom = new System.Windows.Forms.Button();
            this.buttonFinish = new System.Windows.Forms.Button();
            this.groupBoxCrossroad = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelCrossroadButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTrafficLight = new System.Windows.Forms.Button();
            this.buttonDestroyCrossroad = new System.Windows.Forms.Button();
            this.trackBarCarSpawnRate = new System.Windows.Forms.TrackBar();
            this.labelCarSpawnRate = new System.Windows.Forms.Label();
            this.labelOutIndex = new System.Windows.Forms.Label();
            this.labelInIndex = new System.Windows.Forms.Label();
            this.labelCoords = new System.Windows.Forms.Label();
            this.groupBoxBuild = new System.Windows.Forms.GroupBox();
            this.checkBoxTwoWayRoad = new System.Windows.Forms.CheckBox();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBoxRoad.SuspendLayout();
            this.flowLayoutPanelMaxSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).BeginInit();
            this.panelControls.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.tableLayoutPanelMapButtons.SuspendLayout();
            this.groupBoxCrossroad.SuspendLayout();
            this.tableLayoutPanelCrossroadButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).BeginInit();
            this.groupBoxBuild.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.mapPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPanel.Location = new System.Drawing.Point(0, 0);
            this.mapPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(933, 842);
            this.mapPanel.TabIndex = 0;
            this.mapPanel.Zoom = 1F;
            this.mapPanel.ZoomChanged += new System.EventHandler(this.mapPanel_ZoomChanged);
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mapPanel_Paint);
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.Controls.Add(this.buttonDestroyRoad);
            this.groupBoxRoad.Controls.Add(this.flowLayoutPanelMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelTwoWayRoad);
            this.groupBoxRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRoad.Location = new System.Drawing.Point(0, 100);
            this.groupBoxRoad.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBoxRoad.Size = new System.Drawing.Size(371, 188);
            this.groupBoxRoad.TabIndex = 0;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road Properties";
            this.groupBoxRoad.Visible = false;
            // 
            // buttonDestroyRoad
            // 
            this.buttonDestroyRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonDestroyRoad.Location = new System.Drawing.Point(2, 142);
            this.buttonDestroyRoad.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.buttonDestroyRoad.Name = "buttonDestroyRoad";
            this.buttonDestroyRoad.Size = new System.Drawing.Size(367, 41);
            this.buttonDestroyRoad.TabIndex = 4;
            this.buttonDestroyRoad.Text = "Destroy Road";
            this.buttonDestroyRoad.UseVisualStyleBackColor = true;
            this.buttonDestroyRoad.Click += new System.EventHandler(this.buttonDestroyRoad_Click);
            // 
            // flowLayoutPanelMaxSpeed
            // 
            this.flowLayoutPanelMaxSpeed.Controls.Add(this.labelMaxSpeed);
            this.flowLayoutPanelMaxSpeed.Controls.Add(this.numericUpDownMaxSpeed);
            this.flowLayoutPanelMaxSpeed.Controls.Add(this.labelMps);
            this.flowLayoutPanelMaxSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelMaxSpeed.Location = new System.Drawing.Point(2, 103);
            this.flowLayoutPanelMaxSpeed.Name = "flowLayoutPanelMaxSpeed";
            this.flowLayoutPanelMaxSpeed.Size = new System.Drawing.Size(367, 39);
            this.flowLayoutPanelMaxSpeed.TabIndex = 4;
            // 
            // labelMaxSpeed
            // 
            this.labelMaxSpeed.AutoSize = true;
            this.labelMaxSpeed.Location = new System.Drawing.Point(2, 0);
            this.labelMaxSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMaxSpeed.Name = "labelMaxSpeed";
            this.labelMaxSpeed.Size = new System.Drawing.Size(102, 25);
            this.labelMaxSpeed.TabIndex = 0;
            this.labelMaxSpeed.Text = "Max speed:";
            // 
            // numericUpDownMaxSpeed
            // 
            this.numericUpDownMaxSpeed.Location = new System.Drawing.Point(110, 5);
            this.numericUpDownMaxSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownMaxSpeed.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.Name = "numericUpDownMaxSpeed";
            this.numericUpDownMaxSpeed.Size = new System.Drawing.Size(61, 31);
            this.numericUpDownMaxSpeed.TabIndex = 3;
            this.numericUpDownMaxSpeed.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.ValueChanged += new System.EventHandler(this.numericUpDownMaxSpeed_ValueChanged);
            // 
            // labelMps
            // 
            this.labelMps.AutoSize = true;
            this.labelMps.Location = new System.Drawing.Point(179, 0);
            this.labelMps.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMps.Name = "labelMps";
            this.labelMps.Size = new System.Drawing.Size(47, 25);
            this.labelMps.TabIndex = 3;
            this.labelMps.Text = "mps";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTo.Location = new System.Drawing.Point(2, 78);
            this.labelTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(57, 25);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFrom.Location = new System.Drawing.Point(2, 53);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(81, 25);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelTwoWayRoad
            // 
            this.labelTwoWayRoad.AutoSize = true;
            this.labelTwoWayRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTwoWayRoad.Location = new System.Drawing.Point(2, 28);
            this.labelTwoWayRoad.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTwoWayRoad.Name = "labelTwoWayRoad";
            this.labelTwoWayRoad.Size = new System.Drawing.Size(19, 25);
            this.labelTwoWayRoad.TabIndex = 0;
            this.labelTwoWayRoad.Text = "-";
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.groupBoxInfo);
            this.panelControls.Controls.Add(this.tableLayoutPanelMapButtons);
            this.panelControls.Controls.Add(this.groupBoxCrossroad);
            this.panelControls.Controls.Add(this.groupBoxRoad);
            this.panelControls.Controls.Add(this.groupBoxBuild);
            this.panelControls.Controls.Add(this.comboBoxMode);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Margin = new System.Windows.Forms.Padding(4);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(371, 842);
            this.panelControls.TabIndex = 0;
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxInfo.Location = new System.Drawing.Point(0, 557);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Size = new System.Drawing.Size(371, 153);
            this.groupBoxInfo.TabIndex = 9;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(4, 29);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(363, 119);
            this.labelInfo.TabIndex = 0;
            // 
            // tableLayoutPanelMapButtons
            // 
            this.tableLayoutPanelMapButtons.ColumnCount = 2;
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonLoadMap, 1, 0);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonSaveMap, 1, 1);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonCenter, 0, 0);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonZoom, 0, 1);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonFinish, 0, 2);
            this.tableLayoutPanelMapButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelMapButtons.Location = new System.Drawing.Point(0, 710);
            this.tableLayoutPanelMapButtons.Name = "tableLayoutPanelMapButtons";
            this.tableLayoutPanelMapButtons.RowCount = 3;
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelMapButtons.Size = new System.Drawing.Size(371, 132);
            this.tableLayoutPanelMapButtons.TabIndex = 0;
            // 
            // buttonLoadMap
            // 
            this.buttonLoadMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadMap.Location = new System.Drawing.Point(188, 3);
            this.buttonLoadMap.Name = "buttonLoadMap";
            this.buttonLoadMap.Size = new System.Drawing.Size(180, 37);
            this.buttonLoadMap.TabIndex = 10;
            this.buttonLoadMap.Text = "Load Map";
            this.buttonLoadMap.UseVisualStyleBackColor = true;
            this.buttonLoadMap.Click += new System.EventHandler(this.buttonLoadMap_Click);
            // 
            // buttonSaveMap
            // 
            this.buttonSaveMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveMap.Location = new System.Drawing.Point(188, 46);
            this.buttonSaveMap.Name = "buttonSaveMap";
            this.buttonSaveMap.Size = new System.Drawing.Size(180, 38);
            this.buttonSaveMap.TabIndex = 11;
            this.buttonSaveMap.Text = "Save Map";
            this.buttonSaveMap.UseVisualStyleBackColor = true;
            this.buttonSaveMap.Click += new System.EventHandler(this.buttonSaveMap_Click);
            // 
            // buttonCenter
            // 
            this.buttonCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCenter.Location = new System.Drawing.Point(3, 3);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(179, 37);
            this.buttonCenter.TabIndex = 8;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoom.Location = new System.Drawing.Point(3, 46);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(179, 38);
            this.buttonZoom.TabIndex = 9;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // buttonFinish
            // 
            this.tableLayoutPanelMapButtons.SetColumnSpan(this.buttonFinish, 2);
            this.buttonFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFinish.Location = new System.Drawing.Point(3, 90);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(365, 39);
            this.buttonFinish.TabIndex = 12;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // groupBoxCrossroad
            // 
            this.groupBoxCrossroad.Controls.Add(this.tableLayoutPanelCrossroadButtons);
            this.groupBoxCrossroad.Controls.Add(this.trackBarCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.labelCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.labelOutIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelInIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelCoords);
            this.groupBoxCrossroad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCrossroad.Location = new System.Drawing.Point(0, 288);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Size = new System.Drawing.Size(371, 269);
            this.groupBoxCrossroad.TabIndex = 0;
            this.groupBoxCrossroad.TabStop = false;
            this.groupBoxCrossroad.Text = "Crossroad Properties";
            this.groupBoxCrossroad.Visible = false;
            // 
            // tableLayoutPanelCrossroadButtons
            // 
            this.tableLayoutPanelCrossroadButtons.ColumnCount = 2;
            this.tableLayoutPanelCrossroadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.Controls.Add(this.buttonTrafficLight, 0, 0);
            this.tableLayoutPanelCrossroadButtons.Controls.Add(this.buttonDestroyCrossroad, 1, 0);
            this.tableLayoutPanelCrossroadButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelCrossroadButtons.Location = new System.Drawing.Point(3, 196);
            this.tableLayoutPanelCrossroadButtons.Name = "tableLayoutPanelCrossroadButtons";
            this.tableLayoutPanelCrossroadButtons.RowCount = 1;
            this.tableLayoutPanelCrossroadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.Size = new System.Drawing.Size(365, 66);
            this.tableLayoutPanelCrossroadButtons.TabIndex = 0;
            // 
            // buttonTrafficLight
            // 
            this.buttonTrafficLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTrafficLight.Location = new System.Drawing.Point(3, 3);
            this.buttonTrafficLight.Name = "buttonTrafficLight";
            this.buttonTrafficLight.Size = new System.Drawing.Size(176, 60);
            this.buttonTrafficLight.TabIndex = 6;
            this.buttonTrafficLight.Text = "Customize Traffic Light";
            this.buttonTrafficLight.UseVisualStyleBackColor = true;
            this.buttonTrafficLight.Click += new System.EventHandler(this.buttonTrafficLight_Click);
            // 
            // buttonDestroyCrossroad
            // 
            this.buttonDestroyCrossroad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDestroyCrossroad.Location = new System.Drawing.Point(185, 3);
            this.buttonDestroyCrossroad.Name = "buttonDestroyCrossroad";
            this.buttonDestroyCrossroad.Size = new System.Drawing.Size(177, 60);
            this.buttonDestroyCrossroad.TabIndex = 7;
            this.buttonDestroyCrossroad.Text = "Destroy Crossroad";
            this.buttonDestroyCrossroad.UseVisualStyleBackColor = true;
            this.buttonDestroyCrossroad.Click += new System.EventHandler(this.buttonDestroyCrossroad_Click);
            // 
            // trackBarCarSpawnRate
            // 
            this.trackBarCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBarCarSpawnRate.Location = new System.Drawing.Point(3, 127);
            this.trackBarCarSpawnRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarCarSpawnRate.Maximum = 100;
            this.trackBarCarSpawnRate.Minimum = 1;
            this.trackBarCarSpawnRate.Name = "trackBarCarSpawnRate";
            this.trackBarCarSpawnRate.Size = new System.Drawing.Size(365, 69);
            this.trackBarCarSpawnRate.TabIndex = 5;
            this.trackBarCarSpawnRate.Value = 10;
            this.trackBarCarSpawnRate.Scroll += new System.EventHandler(this.trackBarCarSpawnRate_Scroll);
            // 
            // labelCarSpawnRate
            // 
            this.labelCarSpawnRate.AutoSize = true;
            this.labelCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCarSpawnRate.Location = new System.Drawing.Point(3, 102);
            this.labelCarSpawnRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCarSpawnRate.Name = "labelCarSpawnRate";
            this.labelCarSpawnRate.Size = new System.Drawing.Size(165, 25);
            this.labelCarSpawnRate.TabIndex = 0;
            this.labelCarSpawnRate.Text = "Car spawn rate: - %";
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOutIndex.Location = new System.Drawing.Point(3, 77);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(168, 25);
            this.labelOutIndex.TabIndex = 0;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInIndex.Location = new System.Drawing.Point(3, 52);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(153, 25);
            this.labelInIndex.TabIndex = 0;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCoords.Location = new System.Drawing.Point(3, 27);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(97, 25);
            this.labelCoords.TabIndex = 0;
            this.labelCoords.Text = "Coords: -;-";
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.Controls.Add(this.checkBoxTwoWayRoad);
            this.groupBoxBuild.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBuild.Location = new System.Drawing.Point(0, 33);
            this.groupBoxBuild.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBuild.Size = new System.Drawing.Size(371, 67);
            this.groupBoxBuild.TabIndex = 0;
            this.groupBoxBuild.TabStop = false;
            this.groupBoxBuild.Text = "Build Properties";
            this.groupBoxBuild.Visible = false;
            // 
            // checkBoxTwoWayRoad
            // 
            this.checkBoxTwoWayRoad.AutoSize = true;
            this.checkBoxTwoWayRoad.Checked = true;
            this.checkBoxTwoWayRoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTwoWayRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxTwoWayRoad.Location = new System.Drawing.Point(4, 29);
            this.checkBoxTwoWayRoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxTwoWayRoad.Name = "checkBoxTwoWayRoad";
            this.checkBoxTwoWayRoad.Size = new System.Drawing.Size(363, 29);
            this.checkBoxTwoWayRoad.TabIndex = 2;
            this.checkBoxTwoWayRoad.Text = "Two-way road";
            this.checkBoxTwoWayRoad.UseVisualStyleBackColor = true;
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxMode.Location = new System.Drawing.Point(0, 0);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(371, 33);
            this.comboBoxMode.TabIndex = 1;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.mapPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelControls);
            this.splitContainer.Size = new System.Drawing.Size(1308, 842);
            this.splitContainer.SplitterDistance = 933;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.TabStop = false;
            // 
            // FormBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1308, 842);
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormBuild";
            this.Text = "Road Traffic Simulator - Map Builder";
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.flowLayoutPanelMaxSpeed.ResumeLayout(false);
            this.flowLayoutPanelMaxSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).EndInit();
            this.panelControls.ResumeLayout(false);
            this.groupBoxInfo.ResumeLayout(false);
            this.tableLayoutPanelMapButtons.ResumeLayout(false);
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            this.tableLayoutPanelCrossroadButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).EndInit();
            this.groupBoxBuild.ResumeLayout(false);
            this.groupBoxBuild.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RoadTrafficSimulator.Forms.MapPanel mapPanel;
        private System.Windows.Forms.GroupBox groupBoxRoad;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxSpeed;
        private System.Windows.Forms.Label labelMps;
        private System.Windows.Forms.Button buttonDestroyRoad;
        private System.Windows.Forms.Label labelMaxSpeed;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTwoWayRoad;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.GroupBox groupBoxBuild;
        private System.Windows.Forms.CheckBox checkBoxTwoWayRoad;
        private System.Windows.Forms.GroupBox groupBoxCrossroad;
        private System.Windows.Forms.Label labelCarSpawnRate;
        private System.Windows.Forms.TrackBar trackBarCarSpawnRate;
        private System.Windows.Forms.Button buttonDestroyCrossroad;
        private System.Windows.Forms.Button buttonTrafficLight;
        private System.Windows.Forms.Label labelOutIndex;
        private System.Windows.Forms.Label labelInIndex;
        private System.Windows.Forms.Label labelCoords;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMaxSpeed;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCrossroadButtons;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMapButtons;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.Button buttonZoom;
        private System.Windows.Forms.Button buttonSaveMap;
        private System.Windows.Forms.Button buttonLoadMap;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonFinish;
    }
}