namespace Krea.Asset_Manager
{
    partial class SpriteManagerPanel
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
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.spriteSetParentCmbBx = new System.Windows.Forms.ToolStripComboBox();
            this.validBt = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.frameSpriteSetListView = new System.Windows.Forms.ListView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.sequenceFramesListView = new System.Windows.Forms.ListView();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.playAnimBt = new System.Windows.Forms.ToolStripButton();
            this.stopAnimBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.animPictBx = new System.Windows.Forms.PictureBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.sequencesCmbBx = new System.Windows.Forms.ToolStripComboBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animPictBx)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.spriteSetParentCmbBx,
            this.validBt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(600, 31);
            this.toolStrip1.TabIndex = 27;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(96, 28);
            this.toolStripLabel1.Text = "Sprite Set parent:";
            // 
            // spriteSetParentCmbBx
            // 
            this.spriteSetParentCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.spriteSetParentCmbBx.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.spriteSetParentCmbBx.Name = "spriteSetParentCmbBx";
            this.spriteSetParentCmbBx.Size = new System.Drawing.Size(121, 31);
            this.spriteSetParentCmbBx.SelectedIndexChanged += new System.EventHandler(this.spriteSetParentCmbBx_SelectedIndexChanged);
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
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.frameSpriteSetListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(600, 503);
            this.splitContainer1.SplitterDistance = 143;
            this.splitContainer1.SplitterWidth = 15;
            this.splitContainer1.TabIndex = 28;
            // 
            // frameSpriteSetListView
            // 
            this.frameSpriteSetListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.frameSpriteSetListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frameSpriteSetListView.FullRowSelect = true;
            this.frameSpriteSetListView.HideSelection = false;
            this.frameSpriteSetListView.Location = new System.Drawing.Point(0, 0);
            this.frameSpriteSetListView.MultiSelect = false;
            this.frameSpriteSetListView.Name = "frameSpriteSetListView";
            this.frameSpriteSetListView.Size = new System.Drawing.Size(600, 143);
            this.frameSpriteSetListView.TabIndex = 0;
            this.frameSpriteSetListView.UseCompatibleStateImageBehavior = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer2.Panel1.Controls.Add(this.sequenceFramesListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel2.Controls.Add(this.toolStrip3);
            this.splitContainer2.Panel2.Controls.Add(this.animPictBx);
            this.splitContainer2.Size = new System.Drawing.Size(600, 320);
            this.splitContainer2.SplitterDistance = 219;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 29;
            // 
            // sequenceFramesListView
            // 
            this.sequenceFramesListView.BackColor = System.Drawing.Color.White;
            this.sequenceFramesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sequenceFramesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceFramesListView.FullRowSelect = true;
            this.sequenceFramesListView.HideSelection = false;
            this.sequenceFramesListView.Location = new System.Drawing.Point(0, 0);
            this.sequenceFramesListView.MultiSelect = false;
            this.sequenceFramesListView.Name = "sequenceFramesListView";
            this.sequenceFramesListView.Size = new System.Drawing.Size(219, 320);
            this.sequenceFramesListView.TabIndex = 1;
            this.sequenceFramesListView.UseCompatibleStateImageBehavior = false;
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playAnimBt,
            this.stopAnimBt,
            this.toolStripSeparator4});
            this.toolStrip3.Location = new System.Drawing.Point(0, 285);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(371, 35);
            this.toolStrip3.TabIndex = 37;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // playAnimBt
            // 
            this.playAnimBt.AutoSize = false;
            this.playAnimBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.playAnimBt.Image = global::Krea.Properties.Resources.playItem;
            this.playAnimBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playAnimBt.Name = "playAnimBt";
            this.playAnimBt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.playAnimBt.Size = new System.Drawing.Size(32, 32);
            this.playAnimBt.Text = "Text";
            this.playAnimBt.Click += new System.EventHandler(this.playAnimBt_Click);
            // 
            // stopAnimBt
            // 
            this.stopAnimBt.AutoSize = false;
            this.stopAnimBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopAnimBt.Image = global::Krea.Properties.Resources.stopIcon;
            this.stopAnimBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopAnimBt.Name = "stopAnimBt";
            this.stopAnimBt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stopAnimBt.Size = new System.Drawing.Size(32, 32);
            this.stopAnimBt.Text = "Text";
            this.stopAnimBt.Click += new System.EventHandler(this.stopAnimBt_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 35);
            // 
            // animPictBx
            // 
            this.animPictBx.BackColor = System.Drawing.Color.White;
            this.animPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animPictBx.Location = new System.Drawing.Point(0, 0);
            this.animPictBx.Name = "animPictBx";
            this.animPictBx.Size = new System.Drawing.Size(371, 320);
            this.animPictBx.TabIndex = 36;
            this.animPictBx.TabStop = false;
            this.animPictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.animPictBx_Paint);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.sequencesCmbBx});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(600, 25);
            this.toolStrip2.TabIndex = 28;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel2.Text = "Sequences:";
            // 
            // sequencesCmbBx
            // 
            this.sequencesCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.sequencesCmbBx.Name = "sequencesCmbBx";
            this.sequencesCmbBx.Size = new System.Drawing.Size(121, 25);
            this.sequencesCmbBx.SelectedIndexChanged += new System.EventHandler(this.sequencesCmbBx_SelectedIndexChanged);
            // 
            // SpriteManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SpriteManagerPanel";
            this.Size = new System.Drawing.Size(600, 534);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animPictBx)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox spriteSetParentCmbBx;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView frameSpriteSetListView;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox sequencesCmbBx;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView sequenceFramesListView;
        private System.Windows.Forms.PictureBox animPictBx;
        private System.Windows.Forms.ToolStripButton validBt;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton playAnimBt;
        private System.Windows.Forms.ToolStripButton stopAnimBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}
