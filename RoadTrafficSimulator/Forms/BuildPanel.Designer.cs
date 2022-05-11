
namespace RoadTrafficSimulator.Forms
{
    partial class BuildPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.groupBoxBuild = new System.Windows.Forms.GroupBox();
            this.checkBoxTwoWayRoad = new System.Windows.Forms.CheckBox();
            this.groupBoxRoad = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelRoadButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpenRoad = new System.Windows.Forms.Button();
            this.buttonCloseRoad = new System.Windows.Forms.Button();
            this.buttonDestroyRoad = new System.Windows.Forms.Button();
            this.flowLayoutPanelMaxSpeed = new System.Windows.Forms.FlowLayoutPanel();
            this.labelMaxSpeed = new System.Windows.Forms.Label();
            this.numericUpDownMaxSpeed = new System.Windows.Forms.NumericUpDown();
            this.labelKmph = new System.Windows.Forms.Label();
            this.flowLayoutPanelLength = new System.Windows.Forms.FlowLayoutPanel();
            this.labelLength = new System.Windows.Forms.Label();
            this.numericUpDownLength = new System.Windows.Forms.NumericUpDown();
            this.labelM = new System.Windows.Forms.Label();
            this.flowLayoutPanelLanes = new System.Windows.Forms.FlowLayoutPanel();
            this.labelLanes = new System.Windows.Forms.Label();
            this.numericUpDownLanes = new System.Windows.Forms.NumericUpDown();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTwoWayRoad = new System.Windows.Forms.Label();
            this.groupBoxCrossroad = new System.Windows.Forms.GroupBox();
            this.trafficLightPanel = new RoadTrafficSimulator.Forms.TrafficLightPanel();
            this.buttonDestroyCrossroad = new System.Windows.Forms.Button();
            this.trackBarCarSpawnRate = new System.Windows.Forms.TrackBar();
            this.labelCarSpawnRate = new System.Windows.Forms.Label();
            this.labelOutIndex = new System.Windows.Forms.Label();
            this.labelInIndex = new System.Windows.Forms.Label();
            this.labelCoords = new System.Windows.Forms.Label();
            this.tableLayoutPanelMapButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLoadMap = new System.Windows.Forms.Button();
            this.buttonSaveMap = new System.Windows.Forms.Button();
            this.groupBoxMap = new System.Windows.Forms.GroupBox();
            this.radioButtonDriveRight = new System.Windows.Forms.RadioButton();
            this.labelDriveSide = new System.Windows.Forms.Label();
            this.radioButtonDriveLeft = new System.Windows.Forms.RadioButton();
            this.groupBoxBuild.SuspendLayout();
            this.groupBoxRoad.SuspendLayout();
            this.tableLayoutPanelRoadButtons.SuspendLayout();
            this.flowLayoutPanelMaxSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).BeginInit();
            this.flowLayoutPanelLength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).BeginInit();
            this.flowLayoutPanelLanes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLanes)).BeginInit();
            this.groupBoxCrossroad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).BeginInit();
            this.tableLayoutPanelMapButtons.SuspendLayout();
            this.groupBoxMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxMode.Location = new System.Drawing.Point(0, 0);
            this.comboBoxMode.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(697, 33);
            this.comboBoxMode.TabIndex = 0;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.AutoSize = true;
            this.groupBoxBuild.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxBuild.Controls.Add(this.checkBoxTwoWayRoad);
            this.groupBoxBuild.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBuild.Location = new System.Drawing.Point(0, 120);
            this.groupBoxBuild.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBuild.Size = new System.Drawing.Size(697, 63);
            this.groupBoxBuild.TabIndex = 2;
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
            this.checkBoxTwoWayRoad.Size = new System.Drawing.Size(689, 29);
            this.checkBoxTwoWayRoad.TabIndex = 0;
            this.checkBoxTwoWayRoad.Text = "Two-way road";
            this.checkBoxTwoWayRoad.UseVisualStyleBackColor = true;
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.AutoSize = true;
            this.groupBoxRoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxRoad.Controls.Add(this.tableLayoutPanelRoadButtons);
            this.groupBoxRoad.Controls.Add(this.flowLayoutPanelMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.flowLayoutPanelLength);
            this.groupBoxRoad.Controls.Add(this.flowLayoutPanelLanes);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelTwoWayRoad);
            this.groupBoxRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRoad.Location = new System.Drawing.Point(0, 183);
            this.groupBoxRoad.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Padding = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.groupBoxRoad.Size = new System.Drawing.Size(697, 281);
            this.groupBoxRoad.TabIndex = 3;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road Properties";
            this.groupBoxRoad.Visible = false;
            // 
            // tableLayoutPanelRoadButtons
            // 
            this.tableLayoutPanelRoadButtons.ColumnCount = 2;
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRoadButtons.Controls.Add(this.buttonOpenRoad, 0, 0);
            this.tableLayoutPanelRoadButtons.Controls.Add(this.buttonCloseRoad, 0, 0);
            this.tableLayoutPanelRoadButtons.Controls.Add(this.buttonDestroyRoad, 1, 0);
            this.tableLayoutPanelRoadButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelRoadButtons.Location = new System.Drawing.Point(1, 226);
            this.tableLayoutPanelRoadButtons.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanelRoadButtons.Name = "tableLayoutPanelRoadButtons";
            this.tableLayoutPanelRoadButtons.RowCount = 1;
            this.tableLayoutPanelRoadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.Size = new System.Drawing.Size(695, 51);
            this.tableLayoutPanelRoadButtons.TabIndex = 9;
            // 
            // buttonOpenRoad
            // 
            this.buttonOpenRoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenRoad.Location = new System.Drawing.Point(349, 2);
            this.buttonOpenRoad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOpenRoad.Name = "buttonOpenRoad";
            this.buttonOpenRoad.Size = new System.Drawing.Size(344, 21);
            this.buttonOpenRoad.TabIndex = 1;
            this.buttonOpenRoad.Text = "Open Road";
            this.buttonOpenRoad.UseVisualStyleBackColor = true;
            this.buttonOpenRoad.Visible = false;
            // 
            // buttonCloseRoad
            // 
            this.buttonCloseRoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCloseRoad.Location = new System.Drawing.Point(2, 2);
            this.buttonCloseRoad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCloseRoad.Name = "buttonCloseRoad";
            this.buttonCloseRoad.Size = new System.Drawing.Size(343, 21);
            this.buttonCloseRoad.TabIndex = 0;
            this.buttonCloseRoad.Text = "Close Road";
            this.buttonCloseRoad.UseVisualStyleBackColor = true;
            // 
            // buttonDestroyRoad
            // 
            this.buttonDestroyRoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDestroyRoad.Location = new System.Drawing.Point(2, 27);
            this.buttonDestroyRoad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDestroyRoad.Name = "buttonDestroyRoad";
            this.buttonDestroyRoad.Size = new System.Drawing.Size(343, 22);
            this.buttonDestroyRoad.TabIndex = 2;
            this.buttonDestroyRoad.Text = "Destroy Road";
            this.buttonDestroyRoad.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelMaxSpeed
            // 
            this.flowLayoutPanelMaxSpeed.AutoSize = true;
            this.flowLayoutPanelMaxSpeed.Controls.Add(this.labelMaxSpeed);
            this.flowLayoutPanelMaxSpeed.Controls.Add(this.numericUpDownMaxSpeed);
            this.flowLayoutPanelMaxSpeed.Controls.Add(this.labelKmph);
            this.flowLayoutPanelMaxSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelMaxSpeed.Location = new System.Drawing.Point(1, 185);
            this.flowLayoutPanelMaxSpeed.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.flowLayoutPanelMaxSpeed.Name = "flowLayoutPanelMaxSpeed";
            this.flowLayoutPanelMaxSpeed.Size = new System.Drawing.Size(695, 41);
            this.flowLayoutPanelMaxSpeed.TabIndex = 0;
            // 
            // labelMaxSpeed
            // 
            this.labelMaxSpeed.AutoSize = true;
            this.labelMaxSpeed.Location = new System.Drawing.Point(1, 0);
            this.labelMaxSpeed.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelMaxSpeed.Name = "labelMaxSpeed";
            this.labelMaxSpeed.Size = new System.Drawing.Size(102, 25);
            this.labelMaxSpeed.TabIndex = 6;
            this.labelMaxSpeed.Text = "Max speed:";
            // 
            // numericUpDownMaxSpeed
            // 
            this.numericUpDownMaxSpeed.Location = new System.Drawing.Point(108, 5);
            this.numericUpDownMaxSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownMaxSpeed.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.Name = "numericUpDownMaxSpeed";
            this.numericUpDownMaxSpeed.Size = new System.Drawing.Size(61, 31);
            this.numericUpDownMaxSpeed.TabIndex = 7;
            this.numericUpDownMaxSpeed.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.ValueChanged += new System.EventHandler(this.numericUpDownMaxSpeed_ValueChanged);
            // 
            // labelKmph
            // 
            this.labelKmph.AutoSize = true;
            this.labelKmph.Location = new System.Drawing.Point(177, 0);
            this.labelKmph.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKmph.Name = "labelKmph";
            this.labelKmph.Size = new System.Drawing.Size(54, 25);
            this.labelKmph.TabIndex = 8;
            this.labelKmph.Text = "km/h";
            // 
            // flowLayoutPanelLength
            // 
            this.flowLayoutPanelLength.AutoSize = true;
            this.flowLayoutPanelLength.Controls.Add(this.labelLength);
            this.flowLayoutPanelLength.Controls.Add(this.numericUpDownLength);
            this.flowLayoutPanelLength.Controls.Add(this.labelM);
            this.flowLayoutPanelLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelLength.Location = new System.Drawing.Point(1, 144);
            this.flowLayoutPanelLength.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.flowLayoutPanelLength.Name = "flowLayoutPanelLength";
            this.flowLayoutPanelLength.Size = new System.Drawing.Size(695, 41);
            this.flowLayoutPanelLength.TabIndex = 2;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(1, 0);
            this.labelLength.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(70, 25);
            this.labelLength.TabIndex = 3;
            this.labelLength.Text = "Length:";
            // 
            // numericUpDownLength
            // 
            this.numericUpDownLength.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownLength.Location = new System.Drawing.Point(76, 5);
            this.numericUpDownLength.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownLength.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownLength.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownLength.Name = "numericUpDownLength";
            this.numericUpDownLength.Size = new System.Drawing.Size(80, 31);
            this.numericUpDownLength.TabIndex = 4;
            this.numericUpDownLength.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownLength.ValueChanged += new System.EventHandler(this.numericUpDownLength_ValueChanged);
            // 
            // labelM
            // 
            this.labelM.AutoSize = true;
            this.labelM.Location = new System.Drawing.Point(164, 0);
            this.labelM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(28, 25);
            this.labelM.TabIndex = 5;
            this.labelM.Text = "m";
            // 
            // flowLayoutPanelLanes
            // 
            this.flowLayoutPanelLanes.AutoSize = true;
            this.flowLayoutPanelLanes.Controls.Add(this.labelLanes);
            this.flowLayoutPanelLanes.Controls.Add(this.numericUpDownLanes);
            this.flowLayoutPanelLanes.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelLanes.Location = new System.Drawing.Point(1, 103);
            this.flowLayoutPanelLanes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.flowLayoutPanelLanes.Name = "flowLayoutPanelLanes";
            this.flowLayoutPanelLanes.Size = new System.Drawing.Size(695, 41);
            this.flowLayoutPanelLanes.TabIndex = 10;
            // 
            // labelLanes
            // 
            this.labelLanes.AutoSize = true;
            this.labelLanes.Location = new System.Drawing.Point(1, 0);
            this.labelLanes.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelLanes.Name = "labelLanes";
            this.labelLanes.Size = new System.Drawing.Size(60, 25);
            this.labelLanes.TabIndex = 3;
            this.labelLanes.Text = "Lanes:";
            // 
            // numericUpDownLanes
            // 
            this.numericUpDownLanes.Location = new System.Drawing.Point(66, 5);
            this.numericUpDownLanes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownLanes.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownLanes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLanes.Name = "numericUpDownLanes";
            this.numericUpDownLanes.Size = new System.Drawing.Size(50, 31);
            this.numericUpDownLanes.TabIndex = 4;
            this.numericUpDownLanes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLanes.ValueChanged += new System.EventHandler(this.numericUpDownLanes_ValueChanged);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTo.Location = new System.Drawing.Point(1, 78);
            this.labelTo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(57, 25);
            this.labelTo.TabIndex = 2;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFrom.Location = new System.Drawing.Point(1, 53);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(81, 25);
            this.labelFrom.TabIndex = 1;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelTwoWayRoad
            // 
            this.labelTwoWayRoad.AutoSize = true;
            this.labelTwoWayRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTwoWayRoad.Location = new System.Drawing.Point(1, 28);
            this.labelTwoWayRoad.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTwoWayRoad.Name = "labelTwoWayRoad";
            this.labelTwoWayRoad.Size = new System.Drawing.Size(19, 25);
            this.labelTwoWayRoad.TabIndex = 0;
            this.labelTwoWayRoad.Text = "-";
            // 
            // groupBoxCrossroad
            // 
            this.groupBoxCrossroad.AutoSize = true;
            this.groupBoxCrossroad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxCrossroad.Controls.Add(this.trafficLightPanel);
            this.groupBoxCrossroad.Controls.Add(this.buttonDestroyCrossroad);
            this.groupBoxCrossroad.Controls.Add(this.trackBarCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.labelCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.labelOutIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelInIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelCoords);
            this.groupBoxCrossroad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCrossroad.Location = new System.Drawing.Point(0, 464);
            this.groupBoxCrossroad.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.groupBoxCrossroad.Size = new System.Drawing.Size(697, 651);
            this.groupBoxCrossroad.TabIndex = 4;
            this.groupBoxCrossroad.TabStop = false;
            this.groupBoxCrossroad.Text = "Crossroad Properties";
            this.groupBoxCrossroad.Visible = false;
            // 
            // trafficLightPanel
            // 
            this.trafficLightPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.trafficLightPanel.Location = new System.Drawing.Point(2, 247);
            this.trafficLightPanel.Name = "trafficLightPanel";
            this.trafficLightPanel.Size = new System.Drawing.Size(693, 400);
            this.trafficLightPanel.TabIndex = 5;
            // 
            // buttonDestroyCrossroad
            // 
            this.buttonDestroyCrossroad.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonDestroyCrossroad.Location = new System.Drawing.Point(2, 197);
            this.buttonDestroyCrossroad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDestroyCrossroad.Name = "buttonDestroyCrossroad";
            this.buttonDestroyCrossroad.Size = new System.Drawing.Size(693, 50);
            this.buttonDestroyCrossroad.TabIndex = 1;
            this.buttonDestroyCrossroad.Text = "Destroy Crossroad";
            this.buttonDestroyCrossroad.UseVisualStyleBackColor = true;
            // 
            // trackBarCarSpawnRate
            // 
            this.trackBarCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBarCarSpawnRate.Location = new System.Drawing.Point(2, 128);
            this.trackBarCarSpawnRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarCarSpawnRate.Maximum = 100;
            this.trackBarCarSpawnRate.Name = "trackBarCarSpawnRate";
            this.trackBarCarSpawnRate.Size = new System.Drawing.Size(693, 69);
            this.trackBarCarSpawnRate.TabIndex = 4;
            this.trackBarCarSpawnRate.Value = 10;
            this.trackBarCarSpawnRate.Scroll += new System.EventHandler(this.trackBarCarSpawnRate_Scroll);
            // 
            // labelCarSpawnRate
            // 
            this.labelCarSpawnRate.AutoSize = true;
            this.labelCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCarSpawnRate.Location = new System.Drawing.Point(2, 103);
            this.labelCarSpawnRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCarSpawnRate.Name = "labelCarSpawnRate";
            this.labelCarSpawnRate.Size = new System.Drawing.Size(165, 25);
            this.labelCarSpawnRate.TabIndex = 3;
            this.labelCarSpawnRate.Text = "Car spawn rate: - %";
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOutIndex.Location = new System.Drawing.Point(2, 78);
            this.labelOutIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(168, 25);
            this.labelOutIndex.TabIndex = 2;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInIndex.Location = new System.Drawing.Point(2, 53);
            this.labelInIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(153, 25);
            this.labelInIndex.TabIndex = 1;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCoords.Location = new System.Drawing.Point(2, 28);
            this.labelCoords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(97, 25);
            this.labelCoords.TabIndex = 0;
            this.labelCoords.Text = "Coords: -;-";
            // 
            // tableLayoutPanelMapButtons
            // 
            this.tableLayoutPanelMapButtons.ColumnCount = 2;
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonLoadMap, 0, 0);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonSaveMap, 1, 0);
            this.tableLayoutPanelMapButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelMapButtons.Location = new System.Drawing.Point(0, 1115);
            this.tableLayoutPanelMapButtons.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.tableLayoutPanelMapButtons.Name = "tableLayoutPanelMapButtons";
            this.tableLayoutPanelMapButtons.RowCount = 1;
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelMapButtons.Size = new System.Drawing.Size(697, 49);
            this.tableLayoutPanelMapButtons.TabIndex = 5;
            // 
            // buttonLoadMap
            // 
            this.buttonLoadMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadMap.Location = new System.Drawing.Point(2, 2);
            this.buttonLoadMap.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoadMap.Name = "buttonLoadMap";
            this.buttonLoadMap.Size = new System.Drawing.Size(344, 45);
            this.buttonLoadMap.TabIndex = 0;
            this.buttonLoadMap.Text = "Load Map";
            this.buttonLoadMap.UseVisualStyleBackColor = true;
            // 
            // buttonSaveMap
            // 
            this.buttonSaveMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveMap.Location = new System.Drawing.Point(350, 2);
            this.buttonSaveMap.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveMap.Name = "buttonSaveMap";
            this.buttonSaveMap.Size = new System.Drawing.Size(345, 45);
            this.buttonSaveMap.TabIndex = 1;
            this.buttonSaveMap.Text = "Save Map";
            this.buttonSaveMap.UseVisualStyleBackColor = true;
            // 
            // groupBoxMap
            // 
            this.groupBoxMap.AutoSize = true;
            this.groupBoxMap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxMap.Controls.Add(this.radioButtonDriveRight);
            this.groupBoxMap.Controls.Add(this.labelDriveSide);
            this.groupBoxMap.Controls.Add(this.radioButtonDriveLeft);
            this.groupBoxMap.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxMap.Location = new System.Drawing.Point(0, 33);
            this.groupBoxMap.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxMap.Name = "groupBoxMap";
            this.groupBoxMap.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxMap.Size = new System.Drawing.Size(697, 87);
            this.groupBoxMap.TabIndex = 1;
            this.groupBoxMap.TabStop = false;
            this.groupBoxMap.Text = "Map Properties";
            // 
            // radioButtonDriveRight
            // 
            this.radioButtonDriveRight.AutoSize = true;
            this.radioButtonDriveRight.Checked = true;
            this.radioButtonDriveRight.Location = new System.Drawing.Point(188, 30);
            this.radioButtonDriveRight.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDriveRight.Name = "radioButtonDriveRight";
            this.radioButtonDriveRight.Size = new System.Drawing.Size(74, 29);
            this.radioButtonDriveRight.TabIndex = 2;
            this.radioButtonDriveRight.TabStop = true;
            this.radioButtonDriveRight.Text = "right";
            this.radioButtonDriveRight.UseVisualStyleBackColor = true;
            // 
            // labelDriveSide
            // 
            this.labelDriveSide.AutoSize = true;
            this.labelDriveSide.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDriveSide.Location = new System.Drawing.Point(2, 26);
            this.labelDriveSide.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDriveSide.Name = "labelDriveSide";
            this.labelDriveSide.Size = new System.Drawing.Size(110, 25);
            this.labelDriveSide.TabIndex = 0;
            this.labelDriveSide.Text = "Driving side:";
            // 
            // radioButtonDriveLeft
            // 
            this.radioButtonDriveLeft.AutoSize = true;
            this.radioButtonDriveLeft.Location = new System.Drawing.Point(119, 30);
            this.radioButtonDriveLeft.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDriveLeft.Name = "radioButtonDriveLeft";
            this.radioButtonDriveLeft.Size = new System.Drawing.Size(62, 29);
            this.radioButtonDriveLeft.TabIndex = 1;
            this.radioButtonDriveLeft.Text = "left";
            this.radioButtonDriveLeft.UseVisualStyleBackColor = true;
            // 
            // BuildPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMapButtons);
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.groupBoxRoad);
            this.Controls.Add(this.groupBoxBuild);
            this.Controls.Add(this.groupBoxMap);
            this.Controls.Add(this.comboBoxMode);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(236, 0);
            this.Name = "BuildPanel";
            this.Size = new System.Drawing.Size(697, 1270);
            this.groupBoxBuild.ResumeLayout(false);
            this.groupBoxBuild.PerformLayout();
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.tableLayoutPanelRoadButtons.ResumeLayout(false);
            this.flowLayoutPanelMaxSpeed.ResumeLayout(false);
            this.flowLayoutPanelMaxSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).EndInit();
            this.flowLayoutPanelLength.ResumeLayout(false);
            this.flowLayoutPanelLength.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).EndInit();
            this.flowLayoutPanelLanes.ResumeLayout(false);
            this.flowLayoutPanelLanes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLanes)).EndInit();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).EndInit();
            this.tableLayoutPanelMapButtons.ResumeLayout(false);
            this.groupBoxMap.ResumeLayout(false);
            this.groupBoxMap.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.GroupBox groupBoxBuild;
        private System.Windows.Forms.CheckBox checkBoxTwoWayRoad;
        private System.Windows.Forms.GroupBox groupBoxRoad;
        private System.Windows.Forms.Button buttonDestroyRoad;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMaxSpeed;
        private System.Windows.Forms.Label labelMaxSpeed;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxSpeed;
        private System.Windows.Forms.Label labelKmph;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTwoWayRoad;
        private System.Windows.Forms.GroupBox groupBoxCrossroad;
        private System.Windows.Forms.Button buttonDestroyCrossroad;
        private System.Windows.Forms.TrackBar trackBarCarSpawnRate;
        private System.Windows.Forms.Label labelCarSpawnRate;
        private System.Windows.Forms.Label labelOutIndex;
        private System.Windows.Forms.Label labelInIndex;
        private System.Windows.Forms.Label labelCoords;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMapButtons;
        private System.Windows.Forms.Button buttonLoadMap;
        private System.Windows.Forms.Button buttonSaveMap;
        private System.Windows.Forms.GroupBox groupBoxMap;
        private System.Windows.Forms.RadioButton radioButtonDriveRight;
        private System.Windows.Forms.Label labelDriveSide;
        private System.Windows.Forms.RadioButton radioButtonDriveLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRoadButtons;
        private System.Windows.Forms.Button buttonCloseRoad;
        private System.Windows.Forms.Button buttonOpenRoad;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLength;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.NumericUpDown numericUpDownLength;
        private System.Windows.Forms.Label labelM;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLanes;
        private System.Windows.Forms.Label labelLanes;
        private System.Windows.Forms.NumericUpDown numericUpDownLanes;
        private TrafficLightPanel trafficLightPanel;
    }
}
