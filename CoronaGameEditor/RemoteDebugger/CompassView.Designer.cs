namespace Krea.RemoteDebugger
{
    partial class CompassView
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
            this.geographicToolStrip = new System.Windows.Forms.ToolStrip();
            this.angleTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.directionLabel = new System.Windows.Forms.ToolStripLabel();
            this.surfaceDessin = new System.Windows.Forms.PictureBox();
            this.geographicToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.surfaceDessin)).BeginInit();
            this.SuspendLayout();
            // 
            // geographicToolStrip
            // 
            this.geographicToolStrip.AutoSize = false;
            this.geographicToolStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.geographicToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.geographicToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.angleTxtBx,
            this.directionLabel});
            this.geographicToolStrip.Location = new System.Drawing.Point(310, 0);
            this.geographicToolStrip.Name = "geographicToolStrip";
            this.geographicToolStrip.Size = new System.Drawing.Size(56, 301);
            this.geographicToolStrip.TabIndex = 1;
            // 
            // angleTxtBx
            // 
            this.angleTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.angleTxtBx.ForeColor = System.Drawing.Color.Blue;
            this.angleTxtBx.Name = "angleTxtBx";
            this.angleTxtBx.Size = new System.Drawing.Size(52, 23);
            this.angleTxtBx.Text = "0°";
            this.angleTxtBx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // directionLabel
            // 
            this.directionLabel.Name = "directionLabel";
            this.directionLabel.Size = new System.Drawing.Size(54, 15);
            this.directionLabel.Text = "N";
            // 
            // surfaceDessin
            // 
            this.surfaceDessin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.surfaceDessin.Location = new System.Drawing.Point(0, 0);
            this.surfaceDessin.Name = "surfaceDessin";
            this.surfaceDessin.Size = new System.Drawing.Size(310, 301);
            this.surfaceDessin.TabIndex = 2;
            this.surfaceDessin.TabStop = false;
            this.surfaceDessin.Paint += new System.Windows.Forms.PaintEventHandler(this.CompassView_Paint);
            this.surfaceDessin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CompassView_MouseDown);
            this.surfaceDessin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.surfaceDessin_MouseMove);
            this.surfaceDessin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.surfaceDessin_MouseUp);
            // 
            // CompassView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.surfaceDessin);
            this.Controls.Add(this.geographicToolStrip);
            this.Name = "CompassView";
            this.Size = new System.Drawing.Size(366, 301);
            this.SizeChanged += new System.EventHandler(this.CompassView_SizeChanged);
            this.geographicToolStrip.ResumeLayout(false);
            this.geographicToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.surfaceDessin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip geographicToolStrip;
        private System.Windows.Forms.ToolStripTextBox angleTxtBx;
        private System.Windows.Forms.ToolStripLabel directionLabel;
        private System.Windows.Forms.PictureBox surfaceDessin;
    }
}
