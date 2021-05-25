
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
            this.buttonDestroyRoad = new System.Windows.Forms.Button();
            this.flowLayoutPanelMaxSpeed = new System.Windows.Forms.FlowLayoutPanel();
            this.labelMaxSpeed = new System.Windows.Forms.Label();
            this.numericUpDownMaxSpeed = new System.Windows.Forms.NumericUpDown();
            this.labelKmph = new System.Windows.Forms.Label();
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
            this.buttonFinish = new System.Windows.Forms.Button();
            this.groupBoxBuild.SuspendLayout();
            this.groupBoxRoad.SuspendLayout();
            this.flowLayoutPanelMaxSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).BeginInit();
            this.groupBoxCrossroad.SuspendLayout();
            this.tableLayoutPanelCrossroadButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).BeginInit();
            this.tableLayoutPanelMapButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxMode.Location = new System.Drawing.Point(0, 0);
            this.comboBoxMode.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(339, 23);
            this.comboBoxMode.TabIndex = 1;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.AutoSize = true;
            this.groupBoxBuild.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxBuild.Controls.Add(this.checkBoxTwoWayRoad);
            this.groupBoxBuild.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBuild.Location = new System.Drawing.Point(0, 23);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Size = new System.Drawing.Size(339, 41);
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
            this.checkBoxTwoWayRoad.Location = new System.Drawing.Point(3, 19);
            this.checkBoxTwoWayRoad.Name = "checkBoxTwoWayRoad";
            this.checkBoxTwoWayRoad.Size = new System.Drawing.Size(333, 19);
            this.checkBoxTwoWayRoad.TabIndex = 2;
            this.checkBoxTwoWayRoad.Text = "Two-way road";
            this.checkBoxTwoWayRoad.UseVisualStyleBackColor = true;
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.AutoSize = true;
            this.groupBoxRoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxRoad.Controls.Add(this.buttonDestroyRoad);
            this.groupBoxRoad.Controls.Add(this.flowLayoutPanelMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelTwoWayRoad);
            this.groupBoxRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRoad.Location = new System.Drawing.Point(0, 64);
            this.groupBoxRoad.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBoxRoad.Size = new System.Drawing.Size(339, 119);
            this.groupBoxRoad.TabIndex = 0;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road Properties";
            this.groupBoxRoad.Visible = false;
            // 
            // buttonDestroyRoad
            // 
            this.buttonDestroyRoad.AutoSize = true;
            this.buttonDestroyRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonDestroyRoad.Location = new System.Drawing.Point(1, 92);
            this.buttonDestroyRoad.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.buttonDestroyRoad.Name = "buttonDestroyRoad";
            this.buttonDestroyRoad.Size = new System.Drawing.Size(337, 25);
            this.buttonDestroyRoad.TabIndex = 4;
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
            this.flowLayoutPanelMaxSpeed.Location = new System.Drawing.Point(1, 63);
            this.flowLayoutPanelMaxSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanelMaxSpeed.Name = "flowLayoutPanelMaxSpeed";
            this.flowLayoutPanelMaxSpeed.Size = new System.Drawing.Size(337, 29);
            this.flowLayoutPanelMaxSpeed.TabIndex = 0;
            // 
            // labelMaxSpeed
            // 
            this.labelMaxSpeed.AutoSize = true;
            this.labelMaxSpeed.Location = new System.Drawing.Point(1, 0);
            this.labelMaxSpeed.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelMaxSpeed.Name = "labelMaxSpeed";
            this.labelMaxSpeed.Size = new System.Drawing.Size(67, 15);
            this.labelMaxSpeed.TabIndex = 0;
            this.labelMaxSpeed.Text = "Max speed:";
            // 
            // numericUpDownMaxSpeed
            // 
            this.numericUpDownMaxSpeed.Location = new System.Drawing.Point(72, 3);
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
            this.numericUpDownMaxSpeed.Size = new System.Drawing.Size(43, 23);
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
            this.labelKmph.Location = new System.Drawing.Point(121, 0);
            this.labelKmph.Name = "labelKmph";
            this.labelKmph.Size = new System.Drawing.Size(36, 15);
            this.labelKmph.TabIndex = 3;
            this.labelKmph.Text = "km/h";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTo.Location = new System.Drawing.Point(1, 48);
            this.labelTo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(38, 15);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFrom.Location = new System.Drawing.Point(1, 33);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(54, 15);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelTwoWayRoad
            // 
            this.labelTwoWayRoad.AutoSize = true;
            this.labelTwoWayRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTwoWayRoad.Location = new System.Drawing.Point(1, 18);
            this.labelTwoWayRoad.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTwoWayRoad.Name = "labelTwoWayRoad";
            this.labelTwoWayRoad.Size = new System.Drawing.Size(12, 15);
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
            this.groupBoxCrossroad.Location = new System.Drawing.Point(0, 183);
            this.groupBoxCrossroad.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxCrossroad.Size = new System.Drawing.Size(339, 157);
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
            this.tableLayoutPanelCrossroadButtons.Location = new System.Drawing.Point(2, 123);
            this.tableLayoutPanelCrossroadButtons.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanelCrossroadButtons.Name = "tableLayoutPanelCrossroadButtons";
            this.tableLayoutPanelCrossroadButtons.RowCount = 1;
            this.tableLayoutPanelCrossroadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCrossroadButtons.Size = new System.Drawing.Size(335, 32);
            this.tableLayoutPanelCrossroadButtons.TabIndex = 0;
            // 
            // buttonTrafficLight
            // 
            this.buttonTrafficLight.AutoSize = true;
            this.buttonTrafficLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTrafficLight.Location = new System.Drawing.Point(2, 2);
            this.buttonTrafficLight.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTrafficLight.Name = "buttonTrafficLight";
            this.buttonTrafficLight.Size = new System.Drawing.Size(163, 28);
            this.buttonTrafficLight.TabIndex = 6;
            this.buttonTrafficLight.Text = "Customize Traffic Light";
            this.buttonTrafficLight.UseVisualStyleBackColor = true;
            // 
            // buttonDestroyCrossroad
            // 
            this.buttonDestroyCrossroad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDestroyCrossroad.Location = new System.Drawing.Point(169, 2);
            this.buttonDestroyCrossroad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDestroyCrossroad.Name = "buttonDestroyCrossroad";
            this.buttonDestroyCrossroad.Size = new System.Drawing.Size(164, 28);
            this.buttonDestroyCrossroad.TabIndex = 7;
            this.buttonDestroyCrossroad.Text = "Destroy Crossroad";
            this.buttonDestroyCrossroad.UseVisualStyleBackColor = true;
            // 
            // trackBarCarSpawnRate
            // 
            this.trackBarCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBarCarSpawnRate.Location = new System.Drawing.Point(2, 78);
            this.trackBarCarSpawnRate.Maximum = 100;
            this.trackBarCarSpawnRate.Minimum = 1;
            this.trackBarCarSpawnRate.Name = "trackBarCarSpawnRate";
            this.trackBarCarSpawnRate.Size = new System.Drawing.Size(335, 45);
            this.trackBarCarSpawnRate.TabIndex = 5;
            this.trackBarCarSpawnRate.Value = 10;
            this.trackBarCarSpawnRate.Scroll += new System.EventHandler(this.trackBarCarSpawnRate_Scroll);
            // 
            // labelCarSpawnRate
            // 
            this.labelCarSpawnRate.AutoSize = true;
            this.labelCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCarSpawnRate.Location = new System.Drawing.Point(2, 63);
            this.labelCarSpawnRate.Name = "labelCarSpawnRate";
            this.labelCarSpawnRate.Size = new System.Drawing.Size(109, 15);
            this.labelCarSpawnRate.TabIndex = 0;
            this.labelCarSpawnRate.Text = "Car spawn rate: - %";
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOutIndex.Location = new System.Drawing.Point(2, 48);
            this.labelOutIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(111, 15);
            this.labelOutIndex.TabIndex = 0;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInIndex.Location = new System.Drawing.Point(2, 33);
            this.labelInIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(101, 15);
            this.labelInIndex.TabIndex = 0;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCoords.Location = new System.Drawing.Point(2, 18);
            this.labelCoords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(64, 15);
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
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonFinish, 0, 1);
            this.tableLayoutPanelMapButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelMapButtons.Location = new System.Drawing.Point(0, 340);
            this.tableLayoutPanelMapButtons.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanelMapButtons.Name = "tableLayoutPanelMapButtons";
            this.tableLayoutPanelMapButtons.RowCount = 2;
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelMapButtons.Size = new System.Drawing.Size(339, 61);
            this.tableLayoutPanelMapButtons.TabIndex = 0;
            // 
            // buttonLoadMap
            // 
            this.buttonLoadMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadMap.Location = new System.Drawing.Point(2, 2);
            this.buttonLoadMap.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoadMap.Name = "buttonLoadMap";
            this.buttonLoadMap.Size = new System.Drawing.Size(165, 26);
            this.buttonLoadMap.TabIndex = 8;
            this.buttonLoadMap.Text = "Load Map";
            this.buttonLoadMap.UseVisualStyleBackColor = true;
            // 
            // buttonSaveMap
            // 
            this.buttonSaveMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveMap.Location = new System.Drawing.Point(171, 2);
            this.buttonSaveMap.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveMap.Name = "buttonSaveMap";
            this.buttonSaveMap.Size = new System.Drawing.Size(166, 26);
            this.buttonSaveMap.TabIndex = 9;
            this.buttonSaveMap.Text = "Save Map";
            this.buttonSaveMap.UseVisualStyleBackColor = true;
            // 
            // buttonFinish
            // 
            this.tableLayoutPanelMapButtons.SetColumnSpan(this.buttonFinish, 2);
            this.buttonFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFinish.Location = new System.Drawing.Point(2, 32);
            this.buttonFinish.Margin = new System.Windows.Forms.Padding(2);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(335, 27);
            this.buttonFinish.TabIndex = 10;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            // 
            // BuildPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMapButtons);
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.groupBoxRoad);
            this.Controls.Add(this.groupBoxBuild);
            this.Controls.Add(this.comboBoxMode);
            this.MinimumSize = new System.Drawing.Size(165, 0);
            this.Name = "BuildPanel";
            this.Size = new System.Drawing.Size(339, 489);
            this.groupBoxBuild.ResumeLayout(false);
            this.groupBoxBuild.PerformLayout();
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.flowLayoutPanelMaxSpeed.ResumeLayout(false);
            this.flowLayoutPanelMaxSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).EndInit();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            this.tableLayoutPanelCrossroadButtons.ResumeLayout(false);
            this.tableLayoutPanelCrossroadButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).EndInit();
            this.tableLayoutPanelMapButtons.ResumeLayout(false);
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
        private System.Windows.Forms.Button buttonFinish;
    }
}
