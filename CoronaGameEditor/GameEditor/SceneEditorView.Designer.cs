namespace Krea.GameEditor
{
    partial class SceneEditorView
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
            this.vScrollBar = new System.Windows.Forms.TrackBar();
            this.hScrollBar = new System.Windows.Forms.TrackBar();
            this.surfacePictBx = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.vScrollBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hScrollBar)).BeginInit();
            this.SuspendLayout();
            // 
            // vScrollBar
            // 
            this.vScrollBar.AutoSize = false;
            this.vScrollBar.BackColor = System.Drawing.Color.White;
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(527, 0);
            this.vScrollBar.Maximum = 0;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.vScrollBar.Size = new System.Drawing.Size(20, 375);
            this.vScrollBar.TabIndex = 7;
            this.vScrollBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.vScrollBar.Scroll += new System.EventHandler(this.vScrollBar_Scroll);
            this.vScrollBar.MouseLeave += new System.EventHandler(this.hScrollBar_MouseLeave);
            // 
            // hScrollBar
            // 
            this.hScrollBar.AutoSize = false;
            this.hScrollBar.BackColor = System.Drawing.Color.White;
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 355);
            this.hScrollBar.Maximum = 0;
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(527, 20);
            this.hScrollBar.TabIndex = 8;
            this.hScrollBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.hScrollBar.Scroll += new System.EventHandler(this.vScrollBar_Scroll);
            this.hScrollBar.MouseLeave += new System.EventHandler(this.hScrollBar_MouseLeave);
            // 
            // surfacePictBx
            // 
            this.surfacePictBx.AllowDrop = true;
            this.surfacePictBx.BackColor = System.Drawing.Color.Black;
            this.surfacePictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.surfacePictBx.Location = new System.Drawing.Point(0, 0);
            this.surfacePictBx.Name = "surfacePictBx";
            this.surfacePictBx.Size = new System.Drawing.Size(527, 355);
            this.surfacePictBx.TabIndex = 9;
            this.surfacePictBx.SizeChanged += new System.EventHandler(this.surfacePictBx_SizeChanged);
            this.surfacePictBx.DragDrop += new System.Windows.Forms.DragEventHandler(this.surfacePictBx_DragDrop);
            this.surfacePictBx.DragEnter += new System.Windows.Forms.DragEventHandler(this.surfacePictBx_DragEnter);
            this.surfacePictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.surfacePictBx_Paint);
            this.surfacePictBx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseClick);
            this.surfacePictBx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseDown);
            this.surfacePictBx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseMove);
            this.surfacePictBx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.surfacePictBx_MouseUp);
            this.surfacePictBx.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.surfacePictBx_PreviewKeyDown);
            // 
            // SceneEditorView
            // 
            this.AllowDrop = true;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.surfacePictBx);
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.vScrollBar);
            this.DoubleBuffered = true;
            this.Name = "SceneEditorView";
            this.Size = new System.Drawing.Size(547, 375);
            this.Load += new System.EventHandler(this.SceneEditorView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SceneEditorView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.vScrollBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hScrollBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TrackBar hScrollBar;
        public System.Windows.Forms.TrackBar vScrollBar;
        public System.Windows.Forms.Panel surfacePictBx;


    }
}
