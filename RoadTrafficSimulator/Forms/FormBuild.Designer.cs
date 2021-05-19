
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
            this.mapPanel1 = new RoadTrafficSimulator.Forms.MapPanel();
            this.SuspendLayout();
            // 
            // mapPanel1
            // 
            this.mapPanel1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mapPanel1.Location = new System.Drawing.Point(0, 0);
            this.mapPanel1.Name = "mapPanel1";
            this.mapPanel1.Size = new System.Drawing.Size(200, 100);
            this.mapPanel1.TabIndex = 0;
            this.mapPanel1.Zoom = 1F;
            // 
            // FormBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 637);
            this.Controls.Add(this.mapPanel1);
            this.Name = "FormBuild";
            this.Text = "Road Traffic Simulator - Map Builder";
            this.ResumeLayout(false);

        }

        #endregion

        private RoadTrafficSimulator.Forms.MapPanel mapPanel1;
    }
}