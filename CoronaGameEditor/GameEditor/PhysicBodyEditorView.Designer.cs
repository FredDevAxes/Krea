namespace Krea.GameEditor
{
    partial class PhysicBodyEditorView
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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.graduationBarX = new Krea.GameEditor.GraduationBar();
            this.graduationBarY = new Krea.GameEditor.GraduationBar();
            this.surfacePictBx = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.surfacePictBx)).BeginInit();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 133);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(133, 17);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(133, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 150);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // graduationBarX
            // 
            this.graduationBarX.BackColor = System.Drawing.Color.White;
            this.graduationBarX.Dock = System.Windows.Forms.DockStyle.Top;
            this.graduationBarX.Location = new System.Drawing.Point(20, 0);
            this.graduationBarX.Name = "graduationBarX";
            this.graduationBarX.Size = new System.Drawing.Size(113, 20);
            this.graduationBarX.TabIndex = 45;
            // 
            // graduationBarY
            // 
            this.graduationBarY.BackColor = System.Drawing.Color.White;
            this.graduationBarY.Dock = System.Windows.Forms.DockStyle.Left;
            this.graduationBarY.Location = new System.Drawing.Point(0, 0);
            this.graduationBarY.Name = "graduationBarY";
            this.graduationBarY.Size = new System.Drawing.Size(20, 133);
            this.graduationBarY.TabIndex = 44;
            // 
            // surfacePictBx
            // 
            this.surfacePictBx.BackColor = System.Drawing.Color.White;
            this.surfacePictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.surfacePictBx.Location = new System.Drawing.Point(20, 20);
            this.surfacePictBx.Name = "surfacePictBx";
            this.surfacePictBx.Size = new System.Drawing.Size(113, 113);
            this.surfacePictBx.TabIndex = 46;
            this.surfacePictBx.TabStop = false;
            this.surfacePictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.surfacePictBx_Paint);
            this.surfacePictBx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseClick);
            this.surfacePictBx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseDown);
            this.surfacePictBx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseMove);
            this.surfacePictBx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseUp);
            // 
            // PhysicBodyEditorView
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.surfacePictBx);
            this.Controls.Add(this.graduationBarX);
            this.Controls.Add(this.graduationBarY);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Name = "PhysicBodyEditorView";
            ((System.ComponentModel.ISupportInitialize)(this.surfacePictBx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.HScrollBar hScrollBar1;
        public System.Windows.Forms.VScrollBar vScrollBar1;
        public GraduationBar graduationBarX;
        public GraduationBar graduationBarY;
        public System.Windows.Forms.PictureBox surfacePictBx;

    }
}
