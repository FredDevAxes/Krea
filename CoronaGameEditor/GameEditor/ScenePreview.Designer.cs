namespace Krea.GameEditor
{
    partial class ScenePreview
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
            this.previewPictBx = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictBx)).BeginInit();
            this.SuspendLayout();
            // 
            // previewPictBx
            // 
            this.previewPictBx.BackColor = System.Drawing.Color.White;
            this.previewPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPictBx.Location = new System.Drawing.Point(0, 0);
            this.previewPictBx.Name = "previewPictBx";
            this.previewPictBx.Size = new System.Drawing.Size(161, 159);
            this.previewPictBx.TabIndex = 7;
            this.previewPictBx.TabStop = false;
            this.previewPictBx.Tag = "false";
            this.previewPictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPictBx_Paint);
            this.previewPictBx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.previewPictBx_MouseDown);
            this.previewPictBx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.previewPictBx_MouseMove);
            this.previewPictBx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.previewPictBx_MouseUp);
            // 
            // ScenePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewPictBx);
            this.DoubleBuffered = true;
            this.Name = "ScenePreview";
            this.Size = new System.Drawing.Size(161, 159);
            this.MouseEnter += new System.EventHandler(this.ScenePreview_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.previewPictBx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox previewPictBx;



    }
}
