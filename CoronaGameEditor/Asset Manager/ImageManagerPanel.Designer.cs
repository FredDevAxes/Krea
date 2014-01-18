namespace Krea.Asset_Manager
{
    partial class ImageManagerPanel
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.rotateLeftBt = new System.Windows.Forms.ToolStripButton();
            this.rotateRightBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.changeBackColorBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.validBt = new System.Windows.Forms.ToolStripButton();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.imagePictBx = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagePictBx)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateLeftBt,
            this.rotateRightBt,
            this.toolStripSeparator1,
            this.changeBackColorBt,
            this.toolStripSeparator2,
            this.validBt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(408, 31);
            this.toolStrip1.TabIndex = 26;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // rotateLeftBt
            // 
            this.rotateLeftBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rotateLeftBt.Image = global::Krea.Properties.Resources.rotateLeftIcon;
            this.rotateLeftBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateLeftBt.Name = "rotateLeftBt";
            this.rotateLeftBt.Size = new System.Drawing.Size(92, 28);
            this.rotateLeftBt.Text = "Rotate Left";
            this.rotateLeftBt.Click += new System.EventHandler(this.rotateLeftBt_Click);
            // 
            // rotateRightBt
            // 
            this.rotateRightBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rotateRightBt.Image = global::Krea.Properties.Resources.rotateRightIcon;
            this.rotateRightBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateRightBt.Name = "rotateRightBt";
            this.rotateRightBt.Size = new System.Drawing.Size(100, 28);
            this.rotateRightBt.Text = "Rotate Right";
            this.rotateRightBt.Click += new System.EventHandler(this.rotateRightBt_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // changeBackColorBt
            // 
            this.changeBackColorBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.changeBackColorBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.changeBackColorBt.Image = global::Krea.Properties.Resources.paletteIcon;
            this.changeBackColorBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.changeBackColorBt.Name = "changeBackColorBt";
            this.changeBackColorBt.Size = new System.Drawing.Size(28, 28);
            this.changeBackColorBt.Text = "Change background color";
            this.changeBackColorBt.Click += new System.EventHandler(this.changeBackColorBt_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // validBt
            // 
            this.validBt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.validBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.validBt.Image = global::Krea.Properties.Resources.tickIcon;
            this.validBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.validBt.Name = "validBt";
            this.validBt.Size = new System.Drawing.Size(28, 28);
            this.validBt.Text = "Submit";
            this.validBt.Click += new System.EventHandler(this.validBt_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.Color = System.Drawing.Color.AliceBlue;
            // 
            // imagePictBx
            // 
            this.imagePictBx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imagePictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePictBx.Location = new System.Drawing.Point(0, 31);
            this.imagePictBx.Name = "imagePictBx";
            this.imagePictBx.Size = new System.Drawing.Size(408, 353);
            this.imagePictBx.TabIndex = 29;
            this.imagePictBx.TabStop = false;
            this.imagePictBx.DragDrop += new System.Windows.Forms.DragEventHandler(this.imagePictBx_DragDrop);
            this.imagePictBx.DragEnter += new System.Windows.Forms.DragEventHandler(this.imagePictBx_DragEnter);
            this.imagePictBx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imagePictBx_MouseUp);
            // 
            // ImageManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.imagePictBx);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ImageManagerPanel";
            this.Size = new System.Drawing.Size(408, 384);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagePictBx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton rotateLeftBt;
        private System.Windows.Forms.ToolStripButton rotateRightBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton changeBackColorBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton validBt;
        private System.Windows.Forms.ColorDialog colorDialog1;
        public System.Windows.Forms.PictureBox imagePictBx;
    }
}
