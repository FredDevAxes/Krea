namespace Krea.GameEditor
{
    partial class PhysicsBodySettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhysicsBodySettings));
            this.bodyElementsToolStrip = new System.Windows.Forms.ToolStrip();
            this.removeBodyELementBt = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bodyElementsListView = new System.Windows.Forms.ListView();
            this.bodyElementsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.bodyElementsImageList = new System.Windows.Forms.ImageList(this.components);
            this.bodyElementsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bodyElementsToolStrip
            // 
            this.bodyElementsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeBodyELementBt});
            this.bodyElementsToolStrip.Location = new System.Drawing.Point(0, 0);
            this.bodyElementsToolStrip.Name = "bodyElementsToolStrip";
            this.bodyElementsToolStrip.Size = new System.Drawing.Size(468, 25);
            this.bodyElementsToolStrip.TabIndex = 1;
            this.bodyElementsToolStrip.Text = "toolStrip1";
            // 
            // removeBodyELementBt
            // 
            this.removeBodyELementBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeBodyELementBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeBodyELementBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeBodyELementBt.Name = "removeBodyELementBt";
            this.removeBodyELementBt.Size = new System.Drawing.Size(23, 22);
            this.removeBodyELementBt.Text = "Remove Selected Body Elements";
            this.removeBodyELementBt.Click += new System.EventHandler(this.removeBodyELementBt_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bodyElementsListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.bodyElementsPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(468, 218);
            this.splitContainer1.SplitterDistance = 218;
            this.splitContainer1.TabIndex = 2;
            // 
            // bodyElementsListView
            // 
            this.bodyElementsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyElementsListView.HideSelection = false;
            this.bodyElementsListView.LargeImageList = this.bodyElementsImageList;
            this.bodyElementsListView.Location = new System.Drawing.Point(0, 0);
            this.bodyElementsListView.Name = "bodyElementsListView";
            this.bodyElementsListView.Size = new System.Drawing.Size(218, 218);
            this.bodyElementsListView.TabIndex = 4;
            this.bodyElementsListView.UseCompatibleStateImageBehavior = false;
            this.bodyElementsListView.View = System.Windows.Forms.View.Tile;
            this.bodyElementsListView.SelectedIndexChanged += new System.EventHandler(this.bodyElementsListView_SelectedIndexChanged);
            // 
            // bodyElementsPropertyGrid
            // 
            this.bodyElementsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyElementsPropertyGrid.HelpVisible = false;
            this.bodyElementsPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.bodyElementsPropertyGrid.Name = "bodyElementsPropertyGrid";
            this.bodyElementsPropertyGrid.Size = new System.Drawing.Size(246, 218);
            this.bodyElementsPropertyGrid.TabIndex = 6;
            // 
            // bodyElementsImageList
            // 
            this.bodyElementsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("bodyElementsImageList.ImageStream")));
            this.bodyElementsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.bodyElementsImageList.Images.SetKeyName(0, "circleIcon.png");
            this.bodyElementsImageList.Images.SetKeyName(1, "triangleIcon.png");
            // 
            // PhysicsBodySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.bodyElementsToolStrip);
            this.Name = "PhysicsBodySettings";
            this.Size = new System.Drawing.Size(468, 243);
            this.bodyElementsToolStrip.ResumeLayout(false);
            this.bodyElementsToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip bodyElementsToolStrip;
        private System.Windows.Forms.ToolStripButton removeBodyELementBt;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView bodyElementsListView;
        private System.Windows.Forms.PropertyGrid bodyElementsPropertyGrid;
        private System.Windows.Forms.ImageList bodyElementsImageList;
    }
}
