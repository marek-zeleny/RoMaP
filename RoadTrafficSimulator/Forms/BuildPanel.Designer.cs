
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
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTwoWayRoad = new System.Windows.Forms.Label();
            this.groupBoxCrossroad = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelCrossroadButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTrafficLight = new System.Windows.Forms.Button();
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
            this.groupBoxCrossroad.SuspendLayout();
            this.tableLayoutPanelCrossroadButtons.SuspendLayout();
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
            this.comboBoxMode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(387, 28);
            this.comboBoxMode.TabIndex = 1;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.AutoSize = true;
            this.groupBoxBuild.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxBuild.Controls.Add(this.checkBoxTwoWayRoad);
            this.groupBoxBuild.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBuild.Location = new System.Drawing.Point(0, 100);
            this.groupBoxBuild.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxBuild.Size = new System.Drawing.Size(387, 52);
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
            this.checkBoxTwoWayRoad.Location = new System.Drawing.Point(3, 24);
            this.checkBoxTwoWayRoad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxTwoWayRoad.Name = "checkBoxTwoWayRoad";
            this.checkBoxTwoWayRoad.Size = new System.Drawing.Size(381, 24);
            this.checkBoxTwoWayRoad.TabIndex = 2;
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
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelTwoWayRoad);
            this.groupBoxRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRoad.Location = new System.Drawing.Point(0, 152);
            this.groupBoxRoad.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Padding = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.groupBoxRoad.Size = new System.Drawing.Size(387, 197);
            this.groupBoxRoad.TabIndex = 0;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road Properties";
            this.groupBoxRoad.Visible = false;
            // 
            // tableLayoutPanelRoadButtons
            // 
            this.tableLayoutPanelRoadButtons.ColumnCount = 2;
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.Controls.Add(this.buttonOpenRoad, 0, 0);
            this.tableLayoutPanelRoadButtons.Controls.Add(this.buttonCloseRoad, 0, 0);
            this.tableLayoutPanelRoadButtons.Controls.Add(this.buttonDestroyRoad, 1, 0);
            this.tableLayoutPanelRoadButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelRoadButtons.Location = new System.Drawing.Point(1, 153);
            this.tableLayoutPanelRoadButtons.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanelRoadButtons.Name = "tableLayoutPanelRoadButtons";
            this.tableLayoutPanelRoadButtons.RowCount = 1;
            this.tableLayoutPanelRoadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoadButtons.Size = new System.Drawing.Size(385, 41);
            this.tableLayoutPanelRoadButtons.TabIndex = 0;
            // 
            // buttonOpenRoad
            // 
            this.buttonOpenRoad.AutoSize = true;
            this.buttonOpenRoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenRoad.Location = new System.Drawing.Point(193, 3);
            this.buttonOpenRoad.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.buttonOpenRoad.Name = "buttonOpenRoad";
            this.buttonOpenRoad.Size = new System.Drawing.Size(191, 14);
            this.buttonOpenRoad.TabIndex = 6;
            this.buttonOpenRoad.Text = "Open Road";
            this.buttonOpenRoad.UseVisualStyleBackColor = true;
            this.buttonOpenRoad.Visible = false;
            // 
            // buttonCloseRoad
            // 
            this.buttonCloseRoad.AutoSize = true;
            this.buttonCloseRoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCloseRoad.Location = new System.Drawing.Point(1, 3);
            this.buttonCloseRoad.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.buttonCloseRoad.Name = "buttonCloseRoad";
            this.buttonCloseRoad.Size = new System.Drawing.Size(190, 14);
            this.buttonCloseRoad.TabIndex = 4;
            this.buttonCloseRoad.Text = "Close Road";
            this.buttonCloseRoad.UseVisualStyleBackColor = true;
            // 
            // buttonDestroyRoad
            // 
            this.buttonDestroyRoad.AutoSize = true;
            this.buttonDestroyRoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDestroyRoad.Location = new System.Drawing.Point(1, 23);
            this.buttonDestroyRoad.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.buttonDestroyRoad.Name = "buttonDestroyRoad";
            this.buttonDestroyRoad.Size = new System.Drawing.Size(190, 15);
            this.buttonDestroyRoad.TabIndex = 5;
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
            this.flowLayoutPanelMaxSpeed.Location = new System.Drawing.Point(1, 118);
            this.flowLayoutPanelMaxSpeed.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flowLayoutPanelMaxSpeed.Name = "flowLayoutPanelMaxSpeed";
            this.flowLayoutPanelMaxSpeed.Size = new System.Drawing.Size(385, 35);
            this.flowLayoutPanelMaxSpeed.TabIndex = 0;
            // 
            // labelMaxSpeed
            // 
            this.labelMaxSpeed.AutoSize = true;
            this.labelMaxSpeed.Location = new System.Drawing.Point(1, 0);
            this.labelMaxSpeed.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelMaxSpeed.Name = "labelMaxSpeed";
            this.labelMaxSpeed.Size = new System.Drawing.Size(84, 20);
            this.labelMaxSpeed.TabIndex = 0;
            this.labelMaxSpeed.Text = "Max speed:";
            // 
            // numericUpDownMaxSpeed
            // 
            this.numericUpDownMaxSpeed.Location = new System.Drawing.Point(89, 4);
            this.numericUpDownMaxSpeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.numericUpDownMaxSpeed.Size = new System.Drawing.Size(49, 27);
            this.numericUpDownMaxSpeed.TabIndex = 3;
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
            this.labelKmph.Location = new System.Drawing.Point(144, 0);
            this.labelKmph.Name = "labelKmph";
            this.labelKmph.Size = new System.Drawing.Size(43, 20);
            this.labelKmph.TabIndex = 3;
            this.labelKmph.Text = "km/h";
            // 
            // flowLayoutPanelLength
            // 
            this.flowLayoutPanelLength.AutoSize = true;
            this.flowLayoutPanelLength.Controls.Add(this.labelLength);
            this.flowLayoutPanelLength.Controls.Add(this.numericUpDownLength);
            this.flowLayoutPanelLength.Controls.Add(this.labelM);
            this.flowLayoutPanelLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelLength.Location = new System.Drawing.Point(1, 83);
            this.flowLayoutPanelLength.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flowLayoutPanelLength.Name = "flowLayoutPanelLength";
            this.flowLayoutPanelLength.Size = new System.Drawing.Size(385, 35);
            this.flowLayoutPanelLength.TabIndex = 2;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(1, 0);
            this.labelLength.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(57, 20);
            this.labelLength.TabIndex = 0;
            this.labelLength.Text = "Length:";
            // 
            // numericUpDownLength
            // 
            this.numericUpDownLength.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownLength.Location = new System.Drawing.Point(62, 4);
            this.numericUpDownLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericUpDownLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownLength.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownLength.Name = "numericUpDownLength";
            this.numericUpDownLength.Size = new System.Drawing.Size(64, 27);
            this.numericUpDownLength.TabIndex = 3;
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
            this.labelM.Location = new System.Drawing.Point(132, 0);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(22, 20);
            this.labelM.TabIndex = 3;
            this.labelM.Text = "m";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTo.Location = new System.Drawing.Point(1, 63);
            this.labelTo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(47, 20);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFrom.Location = new System.Drawing.Point(1, 43);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(65, 20);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelTwoWayRoad
            // 
            this.labelTwoWayRoad.AutoSize = true;
            this.labelTwoWayRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTwoWayRoad.Location = new System.Drawing.Point(1, 23);
            this.labelTwoWayRoad.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTwoWayRoad.Name = "labelTwoWayRoad";
            this.labelTwoWayRoad.Size = new System.Drawing.Size(15, 20);
            this.labelTwoWayRoad.TabIndex = 0;
            this.labelTwoWayRoad.Text = "-";
            // 
            // groupBoxCrossroad
            // 
            this.groupBoxCrossroad.AutoSize = true;
            this.groupBoxCrossroad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxCrossroad.Controls.Add(this.tableLayoutPanelCrossroadButtons);
            this.groupBoxCrossroad.Controls.Add(this.trackBarCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.labelCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.labelOutIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelInIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelCoords);
            this.groupBoxCrossroad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCrossroad.Location = new System.Drawing.Point(0, 349);
            this.groupBoxCrossroad.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBoxCrossroad.Size = new System.Drawing.Size(387, 205);
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
            this.tableLayoutPanelCrossroadButtons.Location = new System.Drawing.Point(2, 159);
            this.tableLayoutPanelCrossroadButtons.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tableLayoutPanelCrossroadButtons.Name = "tableLayoutPanelCrossroadButtons";
            this.tableLayoutPanelCrossroadButtons.RowCount = 1;
            this.tableLayoutPanelCrossroadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.Size = new System.Drawing.Size(383, 43);
            this.tableLayoutPanelCrossroadButtons.TabIndex = 0;
            // 
            // buttonTrafficLight
            // 
            this.buttonTrafficLight.AutoSize = true;
            this.buttonTrafficLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTrafficLight.Location = new System.Drawing.Point(2, 3);
            this.buttonTrafficLight.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonTrafficLight.Name = "buttonTrafficLight";
            this.buttonTrafficLight.Size = new System.Drawing.Size(187, 37);
            this.buttonTrafficLight.TabIndex = 7;
            this.buttonTrafficLight.Text = "Customize Traffic Light";
            this.buttonTrafficLight.UseVisualStyleBackColor = true;
            // 
            // buttonDestroyCrossroad
            // 
            this.buttonDestroyCrossroad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDestroyCrossroad.Location = new System.Drawing.Point(193, 3);
            this.buttonDestroyCrossroad.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonDestroyCrossroad.Name = "buttonDestroyCrossroad";
            this.buttonDestroyCrossroad.Size = new System.Drawing.Size(188, 37);
            this.buttonDestroyCrossroad.TabIndex = 8;
            this.buttonDestroyCrossroad.Text = "Destroy Crossroad";
            this.buttonDestroyCrossroad.UseVisualStyleBackColor = true;
            // 
            // trackBarCarSpawnRate
            // 
            this.trackBarCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBarCarSpawnRate.Location = new System.Drawing.Point(2, 103);
            this.trackBarCarSpawnRate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarCarSpawnRate.Maximum = 100;
            this.trackBarCarSpawnRate.Name = "trackBarCarSpawnRate";
            this.trackBarCarSpawnRate.Size = new System.Drawing.Size(383, 56);
            this.trackBarCarSpawnRate.TabIndex = 6;
            this.trackBarCarSpawnRate.Value = 10;
            this.trackBarCarSpawnRate.Scroll += new System.EventHandler(this.trackBarCarSpawnRate_Scroll);
            // 
            // labelCarSpawnRate
            // 
            this.labelCarSpawnRate.AutoSize = true;
            this.labelCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCarSpawnRate.Location = new System.Drawing.Point(2, 83);
            this.labelCarSpawnRate.Name = "labelCarSpawnRate";
            this.labelCarSpawnRate.Size = new System.Drawing.Size(136, 20);
            this.labelCarSpawnRate.TabIndex = 0;
            this.labelCarSpawnRate.Text = "Car spawn rate: - %";
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOutIndex.Location = new System.Drawing.Point(2, 63);
            this.labelOutIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(137, 20);
            this.labelOutIndex.TabIndex = 0;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInIndex.Location = new System.Drawing.Point(2, 43);
            this.labelInIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(125, 20);
            this.labelInIndex.TabIndex = 0;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCoords.Location = new System.Drawing.Point(2, 23);
            this.labelCoords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(78, 20);
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
            this.tableLayoutPanelMapButtons.Location = new System.Drawing.Point(0, 554);
            this.tableLayoutPanelMapButtons.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tableLayoutPanelMapButtons.Name = "tableLayoutPanelMapButtons";
            this.tableLayoutPanelMapButtons.RowCount = 1;
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelMapButtons.Size = new System.Drawing.Size(387, 39);
            this.tableLayoutPanelMapButtons.TabIndex = 0;
            // 
            // buttonLoadMap
            // 
            this.buttonLoadMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadMap.Location = new System.Drawing.Point(2, 3);
            this.buttonLoadMap.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonLoadMap.Name = "buttonLoadMap";
            this.buttonLoadMap.Size = new System.Drawing.Size(189, 33);
            this.buttonLoadMap.TabIndex = 9;
            this.buttonLoadMap.Text = "Load Map";
            this.buttonLoadMap.UseVisualStyleBackColor = true;
            // 
            // buttonSaveMap
            // 
            this.buttonSaveMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveMap.Location = new System.Drawing.Point(195, 3);
            this.buttonSaveMap.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonSaveMap.Name = "buttonSaveMap";
            this.buttonSaveMap.Size = new System.Drawing.Size(190, 33);
            this.buttonSaveMap.TabIndex = 10;
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
            this.groupBoxMap.Location = new System.Drawing.Point(0, 28);
            this.groupBoxMap.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxMap.Name = "groupBoxMap";
            this.groupBoxMap.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxMap.Size = new System.Drawing.Size(387, 72);
            this.groupBoxMap.TabIndex = 0;
            this.groupBoxMap.TabStop = false;
            this.groupBoxMap.Text = "Map Properties";
            // 
            // radioButtonDriveRight
            // 
            this.radioButtonDriveRight.AutoSize = true;
            this.radioButtonDriveRight.Checked = true;
            this.radioButtonDriveRight.Location = new System.Drawing.Point(150, 24);
            this.radioButtonDriveRight.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDriveRight.Name = "radioButtonDriveRight";
            this.radioButtonDriveRight.Size = new System.Drawing.Size(61, 24);
            this.radioButtonDriveRight.TabIndex = 11;
            this.radioButtonDriveRight.TabStop = true;
            this.radioButtonDriveRight.Text = "right";
            this.radioButtonDriveRight.UseVisualStyleBackColor = true;
            // 
            // labelDriveSide
            // 
            this.labelDriveSide.AutoSize = true;
            this.labelDriveSide.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDriveSide.Location = new System.Drawing.Point(2, 22);
            this.labelDriveSide.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDriveSide.Name = "labelDriveSide";
            this.labelDriveSide.Size = new System.Drawing.Size(91, 20);
            this.labelDriveSide.TabIndex = 0;
            this.labelDriveSide.Text = "Driving side:";
            // 
            // radioButtonDriveLeft
            // 
            this.radioButtonDriveLeft.AutoSize = true;
            this.radioButtonDriveLeft.Location = new System.Drawing.Point(95, 24);
            this.radioButtonDriveLeft.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDriveLeft.Name = "radioButtonDriveLeft";
            this.radioButtonDriveLeft.Size = new System.Drawing.Size(52, 24);
            this.radioButtonDriveLeft.TabIndex = 10;
            this.radioButtonDriveLeft.Text = "left";
            this.radioButtonDriveLeft.UseVisualStyleBackColor = true;
            // 
            // BuildPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMapButtons);
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.groupBoxRoad);
            this.Controls.Add(this.groupBoxBuild);
            this.Controls.Add(this.groupBoxMap);
            this.Controls.Add(this.comboBoxMode);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(189, 0);
            this.Name = "BuildPanel";
            this.Size = new System.Drawing.Size(387, 652);
            this.groupBoxBuild.ResumeLayout(false);
            this.groupBoxBuild.PerformLayout();
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.tableLayoutPanelRoadButtons.ResumeLayout(false);
            this.tableLayoutPanelRoadButtons.PerformLayout();
            this.flowLayoutPanelMaxSpeed.ResumeLayout(false);
            this.flowLayoutPanelMaxSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).EndInit();
            this.flowLayoutPanelLength.ResumeLayout(false);
            this.flowLayoutPanelLength.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLength)).EndInit();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            this.tableLayoutPanelCrossroadButtons.ResumeLayout(false);
            this.tableLayoutPanelCrossroadButtons.PerformLayout();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCrossroadButtons;
        private System.Windows.Forms.Button buttonTrafficLight;
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
    }
}
