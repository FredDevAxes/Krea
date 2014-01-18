namespace Krea.GameEditor
{
    partial class FileExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileExplorer));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.fileMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeFileBt = new System.Windows.Forms.ToolStripMenuItem();
            this.folderMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFilesBt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.refreshBt = new System.Windows.Forms.ToolStripButton();
            this.fileMenuStrip.SuspendLayout();
            this.folderMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "audioIcon.png");
            this.imageList1.Images.SetKeyName(1, "bmpFileIcon.png");
            this.imageList1.Images.SetKeyName(2, "excel.png");
            this.imageList1.Images.SetKeyName(3, "exe.png");
            this.imageList1.Images.SetKeyName(4, "flash.png");
            this.imageList1.Images.SetKeyName(5, "folderIcon.png");
            this.imageList1.Images.SetKeyName(6, "gif.png");
            this.imageList1.Images.SetKeyName(7, "jpg.png");
            this.imageList1.Images.SetKeyName(8, "note-file.png");
            this.imageList1.Images.SetKeyName(9, "plainFileIcon.png");
            this.imageList1.Images.SetKeyName(10, "png.png");
            this.imageList1.Images.SetKeyName(11, "powerpoint.png");
            this.imageList1.Images.SetKeyName(12, "psd.png");
            this.imageList1.Images.SetKeyName(13, "settings.png");
            this.imageList1.Images.SetKeyName(14, "textFileIcon.png");
            this.imageList1.Images.SetKeyName(15, "ttf.png");
            this.imageList1.Images.SetKeyName(16, "videoFileIcon.png");
            this.imageList1.Images.SetKeyName(17, "word.png");
            this.imageList1.Images.SetKeyName(18, "zip.png");
            // 
            // fileMenuStrip
            // 
            this.fileMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.fileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeFileBt});
            this.fileMenuStrip.Name = "fileMenuStrip";
            this.fileMenuStrip.Size = new System.Drawing.Size(150, 34);
            // 
            // removeFileBt
            // 
            this.removeFileBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeFileBt.Name = "removeFileBt";
            this.removeFileBt.Size = new System.Drawing.Size(149, 30);
            this.removeFileBt.Text = "Remove files";
            this.removeFileBt.Click += new System.EventHandler(this.removeFileBt_Click);
            // 
            // folderMenuStrip
            // 
            this.folderMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.folderMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilesBt});
            this.folderMenuStrip.Name = "folderMenuStrip";
            this.folderMenuStrip.Size = new System.Drawing.Size(131, 34);
            // 
            // addFilesBt
            // 
            this.addFilesBt.Image = global::Krea.Properties.Resources.addFile;
            this.addFilesBt.Name = "addFilesBt";
            this.addFilesBt.Size = new System.Drawing.Size(130, 30);
            this.addFilesBt.Text = "Add Files";
            this.addFilesBt.Click += new System.EventHandler(this.addFilesBt_Click_1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshBt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(181, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.ImageIndex = 7;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 9;
            this.treeView1.Size = new System.Drawing.Size(181, 436);
            this.treeView1.TabIndex = 4;
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // refreshBt
            // 
            this.refreshBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshBt.Image = global::Krea.Properties.Resources.refreshIcon;
            this.refreshBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshBt.Name = "refreshBt";
            this.refreshBt.Size = new System.Drawing.Size(23, 22);
            this.refreshBt.Text = "Refresh Tree";
            this.refreshBt.Click += new System.EventHandler(this.refreshBt_Click);
            // 
            // FileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FileExplorer";
            this.Size = new System.Drawing.Size(181, 461);
            this.fileMenuStrip.ResumeLayout(false);
            this.folderMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip fileMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeFileBt;
        private System.Windows.Forms.ContextMenuStrip folderMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addFilesBt;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripButton refreshBt;
    }
}
