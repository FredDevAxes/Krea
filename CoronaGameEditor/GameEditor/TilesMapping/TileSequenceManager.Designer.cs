namespace Krea.GameEditor.TilesMapping
{
    partial class TileSequenceManager
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textureSequenceListView = new System.Windows.Forms.ListView();
            this.textureSequenceImageList = new System.Windows.Forms.ImageList(this.components);
            this.textureSequencePropGrid = new System.Windows.Forms.PropertyGrid();
            this.textureSequencePreviewPictBx = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.removeTextureSequenceBt = new System.Windows.Forms.ToolStripButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.objectSequenceListView = new System.Windows.Forms.ListView();
            this.objectSequenceImageList = new System.Windows.Forms.ImageList(this.components);
            this.objectSequencePropGrid = new System.Windows.Forms.PropertyGrid();
            this.objectSequencePreview = new System.Windows.Forms.PictureBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.removeObjectSequenceBt = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textureSequencePreviewPictBx)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectSequencePreview)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(552, 166);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(544, 140);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Textures";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 34);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textureSequencePreviewPictBx);
            this.splitContainer1.Size = new System.Drawing.Size(538, 103);
            this.splitContainer1.SplitterDistance = 378;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textureSequenceListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textureSequencePropGrid);
            this.splitContainer2.Size = new System.Drawing.Size(378, 103);
            this.splitContainer2.SplitterDistance = 48;
            this.splitContainer2.TabIndex = 0;
            // 
            // textureSequenceListView
            // 
            this.textureSequenceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureSequenceListView.HideSelection = false;
            this.textureSequenceListView.LargeImageList = this.textureSequenceImageList;
            this.textureSequenceListView.Location = new System.Drawing.Point(0, 0);
            this.textureSequenceListView.Name = "textureSequenceListView";
            this.textureSequenceListView.Size = new System.Drawing.Size(378, 48);
            this.textureSequenceListView.TabIndex = 0;
            this.textureSequenceListView.UseCompatibleStateImageBehavior = false;
            this.textureSequenceListView.ItemActivate += new System.EventHandler(this.textureSequenceListView_ItemActivate);
            this.textureSequenceListView.SelectedIndexChanged += new System.EventHandler(this.textureSequenceListView_SelectedIndexChanged);
            // 
            // textureSequenceImageList
            // 
            this.textureSequenceImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.textureSequenceImageList.ImageSize = new System.Drawing.Size(24, 24);
            this.textureSequenceImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // textureSequencePropGrid
            // 
            this.textureSequencePropGrid.BackColor = System.Drawing.Color.White;
            this.textureSequencePropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureSequencePropGrid.HelpVisible = false;
            this.textureSequencePropGrid.Location = new System.Drawing.Point(0, 0);
            this.textureSequencePropGrid.Margin = new System.Windows.Forms.Padding(0);
            this.textureSequencePropGrid.Name = "textureSequencePropGrid";
            this.textureSequencePropGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textureSequencePropGrid.Size = new System.Drawing.Size(378, 51);
            this.textureSequencePropGrid.TabIndex = 31;
            this.textureSequencePropGrid.Tag = "false";
            this.textureSequencePropGrid.ToolbarVisible = false;
            // 
            // textureSequencePreviewPictBx
            // 
            this.textureSequencePreviewPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureSequencePreviewPictBx.Location = new System.Drawing.Point(0, 0);
            this.textureSequencePreviewPictBx.Name = "textureSequencePreviewPictBx";
            this.textureSequencePreviewPictBx.Size = new System.Drawing.Size(156, 103);
            this.textureSequencePreviewPictBx.TabIndex = 0;
            this.textureSequencePreviewPictBx.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeTextureSequenceBt});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(538, 31);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // removeTextureSequenceBt
            // 
            this.removeTextureSequenceBt.BackColor = System.Drawing.Color.White;
            this.removeTextureSequenceBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeTextureSequenceBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeTextureSequenceBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeTextureSequenceBt.Name = "removeTextureSequenceBt";
            this.removeTextureSequenceBt.Size = new System.Drawing.Size(28, 28);
            this.removeTextureSequenceBt.Text = "Remove selected sequence";
            this.removeTextureSequenceBt.ToolTipText = "Remove selected sequence in the list above";
            this.removeTextureSequenceBt.Click += new System.EventHandler(this.removeTextureSequenceBt_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer3);
            this.tabPage2.Controls.Add(this.toolStrip2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(544, 140);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Objects";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 34);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.objectSequencePreview);
            this.splitContainer3.Size = new System.Drawing.Size(538, 103);
            this.splitContainer3.SplitterDistance = 378;
            this.splitContainer3.TabIndex = 8;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.objectSequenceListView);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.objectSequencePropGrid);
            this.splitContainer4.Size = new System.Drawing.Size(378, 103);
            this.splitContainer4.SplitterDistance = 48;
            this.splitContainer4.TabIndex = 0;
            // 
            // objectSequenceListView
            // 
            this.objectSequenceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectSequenceListView.HideSelection = false;
            this.objectSequenceListView.LargeImageList = this.objectSequenceImageList;
            this.objectSequenceListView.Location = new System.Drawing.Point(0, 0);
            this.objectSequenceListView.Name = "objectSequenceListView";
            this.objectSequenceListView.Size = new System.Drawing.Size(378, 48);
            this.objectSequenceListView.TabIndex = 0;
            this.objectSequenceListView.UseCompatibleStateImageBehavior = false;
            this.objectSequenceListView.ItemActivate += new System.EventHandler(this.objectSequenceListView_ItemActivate);
            this.objectSequenceListView.SelectedIndexChanged += new System.EventHandler(this.objectSequenceListView_SelectedIndexChanged);
            // 
            // objectSequenceImageList
            // 
            this.objectSequenceImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.objectSequenceImageList.ImageSize = new System.Drawing.Size(24, 24);
            this.objectSequenceImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // objectSequencePropGrid
            // 
            this.objectSequencePropGrid.BackColor = System.Drawing.Color.White;
            this.objectSequencePropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectSequencePropGrid.HelpVisible = false;
            this.objectSequencePropGrid.Location = new System.Drawing.Point(0, 0);
            this.objectSequencePropGrid.Margin = new System.Windows.Forms.Padding(0);
            this.objectSequencePropGrid.Name = "objectSequencePropGrid";
            this.objectSequencePropGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.objectSequencePropGrid.Size = new System.Drawing.Size(378, 51);
            this.objectSequencePropGrid.TabIndex = 31;
            this.objectSequencePropGrid.Tag = "false";
            this.objectSequencePropGrid.ToolbarVisible = false;
            // 
            // objectSequencePreview
            // 
            this.objectSequencePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectSequencePreview.Location = new System.Drawing.Point(0, 0);
            this.objectSequencePreview.Name = "objectSequencePreview";
            this.objectSequencePreview.Size = new System.Drawing.Size(156, 103);
            this.objectSequencePreview.TabIndex = 0;
            this.objectSequencePreview.TabStop = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.White;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeObjectSequenceBt});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(538, 31);
            this.toolStrip2.TabIndex = 7;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // removeObjectSequenceBt
            // 
            this.removeObjectSequenceBt.BackColor = System.Drawing.Color.White;
            this.removeObjectSequenceBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeObjectSequenceBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeObjectSequenceBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeObjectSequenceBt.Name = "removeObjectSequenceBt";
            this.removeObjectSequenceBt.Size = new System.Drawing.Size(28, 28);
            this.removeObjectSequenceBt.Text = "Remove selected sequence";
            this.removeObjectSequenceBt.ToolTipText = "Remove selected sequence in the list above";
            this.removeObjectSequenceBt.Click += new System.EventHandler(this.removeObjectSequenceBt_Click);
            // 
            // TileSequenceManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabControl1);
            this.Name = "TileSequenceManager";
            this.Size = new System.Drawing.Size(552, 166);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textureSequencePreviewPictBx)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectSequencePreview)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid textureSequencePropGrid;
        private System.Windows.Forms.PictureBox textureSequencePreviewPictBx;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton removeTextureSequenceBt;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.PropertyGrid objectSequencePropGrid;
        private System.Windows.Forms.PictureBox objectSequencePreview;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton removeObjectSequenceBt;
        private System.Windows.Forms.ImageList textureSequenceImageList;
        private System.Windows.Forms.ImageList objectSequenceImageList;
        public System.Windows.Forms.ListView textureSequenceListView;
        public System.Windows.Forms.ListView objectSequenceListView;

    }
}
