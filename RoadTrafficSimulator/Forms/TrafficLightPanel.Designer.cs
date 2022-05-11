
namespace RoadTrafficSimulator.Forms
{
    partial class TrafficLightPanel
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
            this.panelMap = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMainRoad = new System.Windows.Forms.TabPage();
            this.dataGridViewMainRoad = new System.Windows.Forms.DataGridView();
            this.tabPageTrafficLights = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddSetting = new System.Windows.Forms.Button();
            this.buttonDeleteSetting = new System.Windows.Forms.Button();
            this.groupBoxAllowedDirections = new System.Windows.Forms.GroupBox();
            this.checkBoxDown = new System.Windows.Forms.CheckBox();
            this.checkBoxUp = new System.Windows.Forms.CheckBox();
            this.checkBoxRight = new System.Windows.Forms.CheckBox();
            this.checkBoxLeft = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelDuration = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDuration = new System.Windows.Forms.Label();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.labelS = new System.Windows.Forms.Label();
            this.comboBoxSetting = new System.Windows.Forms.ComboBox();
            this.checkBoxActivateTrafficLight = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageMainRoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMainRoad)).BeginInit();
            this.tabPageTrafficLights.SuspendLayout();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.groupBoxAllowedDirections.SuspendLayout();
            this.flowLayoutPanelDuration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.panelMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMap.Location = new System.Drawing.Point(5, 5);
            this.panelMap.Margin = new System.Windows.Forms.Padding(4);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(390, 400);
            this.panelMap.TabIndex = 0;
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            this.panelMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseClick);
            this.panelMap.Resize += new System.EventHandler(this.panelMap_Resize);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageMainRoad);
            this.tabControl.Controls.Add(this.tabPageTrafficLights);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(5, 405);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(390, 370);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageMainRoad
            // 
            this.tabPageMainRoad.AutoScroll = true;
            this.tabPageMainRoad.Controls.Add(this.dataGridViewMainRoad);
            this.tabPageMainRoad.Location = new System.Drawing.Point(4, 34);
            this.tabPageMainRoad.Name = "tabPageMainRoad";
            this.tabPageMainRoad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMainRoad.Size = new System.Drawing.Size(382, 332);
            this.tabPageMainRoad.TabIndex = 0;
            this.tabPageMainRoad.Text = "Main Road";
            this.tabPageMainRoad.UseVisualStyleBackColor = true;
            // 
            // dataGridViewMainRoad
            // 
            this.dataGridViewMainRoad.AllowUserToAddRows = false;
            this.dataGridViewMainRoad.AllowUserToDeleteRows = false;
            this.dataGridViewMainRoad.AllowUserToResizeColumns = false;
            this.dataGridViewMainRoad.AllowUserToResizeRows = false;
            this.dataGridViewMainRoad.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewMainRoad.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridViewMainRoad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMainRoad.ColumnHeadersVisible = false;
            this.dataGridViewMainRoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMainRoad.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewMainRoad.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewMainRoad.MultiSelect = false;
            this.dataGridViewMainRoad.Name = "dataGridViewMainRoad";
            this.dataGridViewMainRoad.ReadOnly = true;
            this.dataGridViewMainRoad.RowHeadersVisible = false;
            this.dataGridViewMainRoad.RowHeadersWidth = 62;
            this.dataGridViewMainRoad.RowTemplate.Height = 33;
            this.dataGridViewMainRoad.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewMainRoad.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMainRoad.Size = new System.Drawing.Size(376, 326);
            this.dataGridViewMainRoad.TabIndex = 0;
            this.dataGridViewMainRoad.SelectionChanged += new System.EventHandler(this.dataGridViewMainRoad_SelectionChanged);
            // 
            // tabPageTrafficLights
            // 
            this.tabPageTrafficLights.AutoScroll = true;
            this.tabPageTrafficLights.Controls.Add(this.tableLayoutPanelButtons);
            this.tabPageTrafficLights.Controls.Add(this.groupBoxAllowedDirections);
            this.tabPageTrafficLights.Controls.Add(this.flowLayoutPanelDuration);
            this.tabPageTrafficLights.Controls.Add(this.comboBoxSetting);
            this.tabPageTrafficLights.Controls.Add(this.checkBoxActivateTrafficLight);
            this.tabPageTrafficLights.Location = new System.Drawing.Point(4, 34);
            this.tabPageTrafficLights.Name = "tabPageTrafficLights";
            this.tabPageTrafficLights.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTrafficLights.Size = new System.Drawing.Size(382, 332);
            this.tabPageTrafficLights.TabIndex = 1;
            this.tabPageTrafficLights.Text = "Traffic Lights";
            this.tabPageTrafficLights.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.ColumnCount = 1;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Controls.Add(this.buttonAddSetting, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonDeleteSetting, 1, 0);
            this.tableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(3, 234);
            this.tableLayoutPanelButtons.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 2;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(376, 92);
            this.tableLayoutPanelButtons.TabIndex = 4;
            // 
            // buttonAddSetting
            // 
            this.buttonAddSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddSetting.Location = new System.Drawing.Point(4, 5);
            this.buttonAddSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAddSetting.Name = "buttonAddSetting";
            this.buttonAddSetting.Size = new System.Drawing.Size(368, 36);
            this.buttonAddSetting.TabIndex = 0;
            this.buttonAddSetting.Text = "Add New Period";
            this.buttonAddSetting.UseVisualStyleBackColor = true;
            this.buttonAddSetting.Click += new System.EventHandler(this.buttonAddSetting_Click);
            // 
            // buttonDeleteSetting
            // 
            this.buttonDeleteSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteSetting.Location = new System.Drawing.Point(4, 51);
            this.buttonDeleteSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDeleteSetting.Name = "buttonDeleteSetting";
            this.buttonDeleteSetting.Size = new System.Drawing.Size(368, 36);
            this.buttonDeleteSetting.TabIndex = 1;
            this.buttonDeleteSetting.Text = "Remove Period";
            this.buttonDeleteSetting.UseVisualStyleBackColor = true;
            this.buttonDeleteSetting.Click += new System.EventHandler(this.buttonDeleteSetting_Click);
            // 
            // groupBoxAllowedDirections
            // 
            this.groupBoxAllowedDirections.AutoSize = true;
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxDown);
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxUp);
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxRight);
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxLeft);
            this.groupBoxAllowedDirections.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxAllowedDirections.Location = new System.Drawing.Point(3, 108);
            this.groupBoxAllowedDirections.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxAllowedDirections.Name = "groupBoxAllowedDirections";
            this.groupBoxAllowedDirections.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxAllowedDirections.Size = new System.Drawing.Size(376, 126);
            this.groupBoxAllowedDirections.TabIndex = 3;
            this.groupBoxAllowedDirections.TabStop = false;
            this.groupBoxAllowedDirections.Text = "Allowed Directions";
            // 
            // checkBoxDown
            // 
            this.checkBoxDown.AutoSize = true;
            this.checkBoxDown.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxDown.Enabled = false;
            this.checkBoxDown.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxDown.Location = new System.Drawing.Point(59, 75);
            this.checkBoxDown.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxDown.Name = "checkBoxDown";
            this.checkBoxDown.Size = new System.Drawing.Size(258, 47);
            this.checkBoxDown.TabIndex = 3;
            this.checkBoxDown.Text = "▼";
            this.checkBoxDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxDown.UseVisualStyleBackColor = true;
            this.checkBoxDown.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // checkBoxUp
            // 
            this.checkBoxUp.AutoSize = true;
            this.checkBoxUp.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxUp.Enabled = false;
            this.checkBoxUp.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxUp.Location = new System.Drawing.Point(59, 28);
            this.checkBoxUp.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxUp.Name = "checkBoxUp";
            this.checkBoxUp.Size = new System.Drawing.Size(258, 47);
            this.checkBoxUp.TabIndex = 1;
            this.checkBoxUp.Text = "▲";
            this.checkBoxUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxUp.UseVisualStyleBackColor = true;
            this.checkBoxUp.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // checkBoxRight
            // 
            this.checkBoxRight.AutoSize = true;
            this.checkBoxRight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxRight.Enabled = false;
            this.checkBoxRight.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxRight.Location = new System.Drawing.Point(317, 28);
            this.checkBoxRight.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxRight.Name = "checkBoxRight";
            this.checkBoxRight.Size = new System.Drawing.Size(55, 94);
            this.checkBoxRight.TabIndex = 2;
            this.checkBoxRight.Text = "►";
            this.checkBoxRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxRight.UseVisualStyleBackColor = true;
            this.checkBoxRight.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // checkBoxLeft
            // 
            this.checkBoxLeft.AutoSize = true;
            this.checkBoxLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxLeft.Enabled = false;
            this.checkBoxLeft.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxLeft.Location = new System.Drawing.Point(4, 28);
            this.checkBoxLeft.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxLeft.Name = "checkBoxLeft";
            this.checkBoxLeft.Size = new System.Drawing.Size(55, 94);
            this.checkBoxLeft.TabIndex = 0;
            this.checkBoxLeft.Text = "◄";
            this.checkBoxLeft.UseVisualStyleBackColor = true;
            this.checkBoxLeft.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // flowLayoutPanelDuration
            // 
            this.flowLayoutPanelDuration.AutoSize = true;
            this.flowLayoutPanelDuration.Controls.Add(this.labelDuration);
            this.flowLayoutPanelDuration.Controls.Add(this.numericUpDownDuration);
            this.flowLayoutPanelDuration.Controls.Add(this.labelS);
            this.flowLayoutPanelDuration.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelDuration.Location = new System.Drawing.Point(3, 65);
            this.flowLayoutPanelDuration.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanelDuration.Name = "flowLayoutPanelDuration";
            this.flowLayoutPanelDuration.Padding = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanelDuration.Size = new System.Drawing.Size(376, 43);
            this.flowLayoutPanelDuration.TabIndex = 2;
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(5, 1);
            this.labelDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(81, 25);
            this.labelDuration.TabIndex = 0;
            this.labelDuration.Text = "Duration";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(94, 6);
            this.numericUpDownDuration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(61, 31);
            this.numericUpDownDuration.TabIndex = 1;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDuration.ValueChanged += new System.EventHandler(this.numericUpDownDuration_ValueChanged);
            // 
            // labelS
            // 
            this.labelS.AutoSize = true;
            this.labelS.Location = new System.Drawing.Point(163, 1);
            this.labelS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(20, 25);
            this.labelS.TabIndex = 2;
            this.labelS.Text = "s";
            // 
            // comboBoxSetting
            // 
            this.comboBoxSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSetting.FormattingEnabled = true;
            this.comboBoxSetting.Location = new System.Drawing.Point(3, 32);
            this.comboBoxSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxSetting.Name = "comboBoxSetting";
            this.comboBoxSetting.Size = new System.Drawing.Size(376, 33);
            this.comboBoxSetting.TabIndex = 1;
            this.comboBoxSetting.SelectedIndexChanged += new System.EventHandler(this.comboBoxSetting_SelectedIndexChanged);
            // 
            // checkBoxActivateTrafficLight
            // 
            this.checkBoxActivateTrafficLight.AutoSize = true;
            this.checkBoxActivateTrafficLight.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxActivateTrafficLight.Location = new System.Drawing.Point(3, 3);
            this.checkBoxActivateTrafficLight.Name = "checkBoxActivateTrafficLight";
            this.checkBoxActivateTrafficLight.Size = new System.Drawing.Size(376, 29);
            this.checkBoxActivateTrafficLight.TabIndex = 0;
            this.checkBoxActivateTrafficLight.Text = "Activate traffic lights";
            this.checkBoxActivateTrafficLight.UseVisualStyleBackColor = true;
            this.checkBoxActivateTrafficLight.CheckedChanged += new System.EventHandler(this.checkBoxActivateTrafficLight_CheckedChanged);
            // 
            // TrafficLightPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelMap);
            this.Name = "TrafficLightPanel";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(400, 833);
            this.tabControl.ResumeLayout(false);
            this.tabPageMainRoad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMainRoad)).EndInit();
            this.tabPageTrafficLights.ResumeLayout(false);
            this.tabPageTrafficLights.PerformLayout();
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.groupBoxAllowedDirections.ResumeLayout(false);
            this.groupBoxAllowedDirections.PerformLayout();
            this.flowLayoutPanelDuration.ResumeLayout(false);
            this.flowLayoutPanelDuration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageMainRoad;
        private System.Windows.Forms.TabPage tabPageTrafficLights;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private System.Windows.Forms.Button buttonAddSetting;
        private System.Windows.Forms.Button buttonDeleteSetting;
        private System.Windows.Forms.GroupBox groupBoxAllowedDirections;
        private System.Windows.Forms.CheckBox checkBoxDown;
        private System.Windows.Forms.CheckBox checkBoxUp;
        private System.Windows.Forms.CheckBox checkBoxRight;
        private System.Windows.Forms.CheckBox checkBoxLeft;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDuration;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label labelS;
        private System.Windows.Forms.ComboBox comboBoxSetting;
        private System.Windows.Forms.CheckBox checkBoxActivateTrafficLight;
        private System.Windows.Forms.DataGridView dataGridViewMainRoad;
    }
}
