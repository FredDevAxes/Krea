namespace Krea.Asset_Manager
{
    partial class SnippetManagerPanel
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
            this.validBt = new System.Windows.Forms.ToolStripButton();
            this.snippetEditor1 = new Krea.SnippetEditor();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.validBt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(408, 31);
            this.toolStrip1.TabIndex = 26;
            // 
            // validBt
            // 
            this.validBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.validBt.Image = global::Krea.Properties.Resources.tickIcon;
            this.validBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.validBt.Name = "validBt";
            this.validBt.Size = new System.Drawing.Size(28, 28);
            this.validBt.Text = "Submit";
            this.validBt.Click += new System.EventHandler(this.validBt_Click);
            // 
            // snippetEditor1
            // 
            this.snippetEditor1.BackColor = System.Drawing.Color.Transparent;
            this.snippetEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snippetEditor1.Location = new System.Drawing.Point(0, 31);
            this.snippetEditor1.Name = "snippetEditor1";
            this.snippetEditor1.Size = new System.Drawing.Size(408, 353);
            this.snippetEditor1.TabIndex = 29;
            // 
            // SnippetManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.snippetEditor1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SnippetManagerPanel";
            this.Size = new System.Drawing.Size(408, 384);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton validBt;
        private SnippetEditor snippetEditor1;
    }
}
