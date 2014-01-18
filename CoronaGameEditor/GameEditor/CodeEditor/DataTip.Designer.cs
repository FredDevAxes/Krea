namespace Krea.GameEditor.CodeEditor
{
    partial class DataTip
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer2 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTip));
            this.localsListView = new System.Windows.Forms.TreeListView();
            this.localsNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeLocalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.localValueHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.treeListView1 = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typesImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // localsListView
            // 
            this.localsListView.AutoArrange = false;
            this.localsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.localsNameHeader,
            this.typeLocalHeader,
            this.localValueHeader});
            treeListViewItemCollectionComparer1.Column = 0;
            treeListViewItemCollectionComparer1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.localsListView.Comparer = treeListViewItemCollectionComparer1;
            this.localsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localsListView.GridLines = true;
            this.localsListView.Location = new System.Drawing.Point(0, 0);
            this.localsListView.MultiSelect = false;
            this.localsListView.Name = "localsListView";
            this.localsListView.Size = new System.Drawing.Size(212, 157);
            this.localsListView.Sorting = System.Windows.Forms.SortOrder.None;
            this.localsListView.TabIndex = 2;
            this.localsListView.UseCompatibleStateImageBehavior = false;
            // 
            // localsNameHeader
            // 
            this.localsNameHeader.Text = "Name";
            this.localsNameHeader.Width = 108;
            // 
            // typeLocalHeader
            // 
            this.typeLocalHeader.Text = "Type";
            this.typeLocalHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.typeLocalHeader.Width = 97;
            // 
            // localValueHeader
            // 
            this.localValueHeader.Text = "Value";
            this.localValueHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.localValueHeader.Width = 78;
            // 
            // treeListView1
            // 
            this.treeListView1.AutoArrange = false;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            treeListViewItemCollectionComparer2.Column = 0;
            treeListViewItemCollectionComparer2.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeListView1.Comparer = treeListViewItemCollectionComparer2;
            this.treeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView1.GridLines = true;
            this.treeListView1.HideSelection = false;
            this.treeListView1.LargeImageList = this.typesImageList;
            this.treeListView1.Location = new System.Drawing.Point(0, 0);
            this.treeListView1.MultiSelect = false;
            this.treeListView1.Name = "treeListView1";
            this.treeListView1.Size = new System.Drawing.Size(212, 157);
            this.treeListView1.SmallImageList = this.typesImageList;
            this.treeListView1.Sorting = System.Windows.Forms.SortOrder.None;
            this.treeListView1.TabIndex = 3;
            this.treeListView1.UseCompatibleStateImageBehavior = false;
            this.treeListView1.UseXPHighlightStyle = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 108;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 97;
            // 
            // typesImageList
            // 
            this.typesImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("typesImageList.ImageStream")));
            this.typesImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.typesImageList.Images.SetKeyName(0, "pubclass.png");
            this.typesImageList.Images.SetKeyName(1, "pubfield.png");
            this.typesImageList.Images.SetKeyName(2, "pubmethod.png");
            this.typesImageList.Images.SetKeyName(3, "pubproperty.png");
            // 
            // DataTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(212, 157);
            this.Controls.Add(this.treeListView1);
            this.Controls.Add(this.localsListView);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DataTip";
            this.Text = "DataTip";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeListView localsListView;
        private System.Windows.Forms.ColumnHeader localsNameHeader;
        private System.Windows.Forms.ColumnHeader typeLocalHeader;
        private System.Windows.Forms.ColumnHeader localValueHeader;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ImageList typesImageList;
        public System.Windows.Forms.TreeListView treeListView1;
    }
}