namespace Krea.GameEditor.PropertyGridEditors
{
    partial class SliderPropertyEditor
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
            this.previewValueBt = new System.Windows.Forms.Button();
            this.trackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // previewValueBt
            // 
            this.previewValueBt.Dock = System.Windows.Forms.DockStyle.Right;
            this.previewValueBt.Location = new System.Drawing.Point(137, 0);
            this.previewValueBt.Name = "previewValueBt";
            this.previewValueBt.Size = new System.Drawing.Size(38, 42);
            this.previewValueBt.TabIndex = 0;
            this.previewValueBt.Text = "0";
            this.previewValueBt.UseVisualStyleBackColor = true;
            this.previewValueBt.Click += new System.EventHandler(this.previewValueBt_Click);
            // 
            // trackBar
            // 
            this.trackBar.BackColor = System.Drawing.Color.White;
            this.trackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar.Location = new System.Drawing.Point(0, 0);
            this.trackBar.Maximum = 100;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(137, 42);
            this.trackBar.TabIndex = 1;
            this.trackBar.TickFrequency = 5;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // SliderPropertyEditor
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.previewValueBt);
            this.Name = "SliderPropertyEditor";
            this.Size = new System.Drawing.Size(175, 42);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button previewValueBt;
        public System.Windows.Forms.TrackBar trackBar;
    }
}
