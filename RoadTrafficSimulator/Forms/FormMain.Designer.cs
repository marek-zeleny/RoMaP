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
            this.tableLayoutPanelMapButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonBuildMap = new System.Windows.Forms.Button();
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
            this.buttonCenter.Location = new System.Drawing.Point(3, 3);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(201, 42);
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
            this.buttonZoom.Size = new System.Drawing.Size(201, 42);
            this.buttonZoom.TabIndex = 2;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxInfo.Location = new System.Drawing.Point(0, 0);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Size = new System.Drawing.Size(414, 900);
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
            this.labelInfo.Size = new System.Drawing.Size(406, 866);
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
            this.mapPanel.Size = new System.Drawing.Size(900, 996);
            this.mapPanel.TabIndex = 0;
            this.mapPanel.Zoom = 1F;
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mapPanel_Paint);
            this.mapPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseClick);
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
            this.splitContainer.Panel2.Controls.Add(this.groupBoxInfo);
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanelMapButtons);
            this.splitContainer.Size = new System.Drawing.Size(1318, 996);
            this.splitContainer.SplitterDistance = 900;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.TabStop = false;
            // 
            // tableLayoutPanelMapButtons
            // 
            this.tableLayoutPanelMapButtons.ColumnCount = 2;
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonBuildMap, 1, 0);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonZoom, 0, 1);
            this.tableLayoutPanelMapButtons.Controls.Add(this.buttonCenter, 0, 0);
            this.tableLayoutPanelMapButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelMapButtons.Location = new System.Drawing.Point(0, 900);
            this.tableLayoutPanelMapButtons.Name = "tableLayoutPanelMapButtons";
            this.tableLayoutPanelMapButtons.RowCount = 2;
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMapButtons.Size = new System.Drawing.Size(414, 96);
            this.tableLayoutPanelMapButtons.TabIndex = 0;
            // 
            // buttonBuildMap
            // 
            this.buttonBuildMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBuildMap.Location = new System.Drawing.Point(210, 3);
            this.buttonBuildMap.Name = "buttonBuildMap";
            this.tableLayoutPanelMapButtons.SetRowSpan(this.buttonBuildMap, 2);
            this.buttonBuildMap.Size = new System.Drawing.Size(201, 90);
            this.buttonBuildMap.TabIndex = 3;
            this.buttonBuildMap.Text = "Build Map";
            this.buttonBuildMap.UseVisualStyleBackColor = true;
            this.buttonBuildMap.Click += new System.EventHandler(this.buttonBuildMap_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1318, 996);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Road Traffic Simulator";
            this.groupBoxInfo.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
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
        private System.Windows.Forms.Button buttonBuildMap;
    }
}

