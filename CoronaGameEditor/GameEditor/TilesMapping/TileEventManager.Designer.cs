namespace Krea.GameEditor.TilesMapping
{
    partial class TileEventManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TileEventManager));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.eventTypeCmbBx = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.eventNameTxtBx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.eventListView = new System.Windows.Forms.ListView();
            this.eventImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip6.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.eventTypeCmbBx);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.eventNameTxtBx);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.eventListView);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip6);
            this.splitContainer1.Size = new System.Drawing.Size(400, 215);
            this.splitContainer1.SplitterDistance = 195;
            this.splitContainer1.TabIndex = 0;
            // 
            // eventTypeCmbBx
            // 
            this.eventTypeCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.eventTypeCmbBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.eventTypeCmbBx.FormattingEnabled = true;
            this.eventTypeCmbBx.Items.AddRange(new object[] {
            "preCollision",
            "collision",
            "postCollision",
            "touch"});
            this.eventTypeCmbBx.Location = new System.Drawing.Point(0, 46);
            this.eventTypeCmbBx.Name = "eventTypeCmbBx";
            this.eventTypeCmbBx.Size = new System.Drawing.Size(195, 21);
            this.eventTypeCmbBx.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Event Type:";
            // 
            // eventNameTxtBx
            // 
            this.eventNameTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.eventNameTxtBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.eventNameTxtBx.Location = new System.Drawing.Point(0, 13);
            this.eventNameTxtBx.Name = "eventNameTxtBx";
            this.eventNameTxtBx.Size = new System.Drawing.Size(195, 20);
            this.eventNameTxtBx.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Event Name:";
            // 
            // eventListView
            // 
            this.eventListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventListView.HideSelection = false;
            this.eventListView.LargeImageList = this.eventImageList;
            this.eventListView.Location = new System.Drawing.Point(0, 31);
            this.eventListView.Name = "eventListView";
            this.eventListView.Size = new System.Drawing.Size(201, 184);
            this.eventListView.TabIndex = 28;
            this.eventListView.UseCompatibleStateImageBehavior = false;
            this.eventListView.ItemActivate += new System.EventHandler(this.eventListView_ItemActivate);
            this.eventListView.SelectedIndexChanged += new System.EventHandler(this.eventListView_SelectedIndexChanged);
            // 
            // eventImageList
            // 
            this.eventImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("eventImageList.ImageStream")));
            this.eventImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.eventImageList.Images.SetKeyName(0, "collisionIcon2.png");
            this.eventImageList.Images.SetKeyName(1, "postCollisionIcon.png");
            this.eventImageList.Images.SetKeyName(2, "preCollisionIcon.png");
            this.eventImageList.Images.SetKeyName(3, "touchIcon.png");
            // 
            // toolStrip6
            // 
            this.toolStrip6.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip6.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip6.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripButton7});
            this.toolStrip6.Location = new System.Drawing.Point(0, 0);
            this.toolStrip6.Name = "toolStrip6";
            this.toolStrip6.Size = new System.Drawing.Size(201, 31);
            this.toolStrip6.TabIndex = 27;
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.BackColor = System.Drawing.Color.White;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Krea.Properties.Resources.addItem;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton3.Text = "Add a new tile event";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.BackColor = System.Drawing.Color.White;
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::Krea.Properties.Resources.removeIcon;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton7.Text = "Remove the selected event selected in the list above.";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // TileEventManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TileEventManager";
            this.Size = new System.Drawing.Size(400, 215);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox eventTypeCmbBx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox eventNameTxtBx;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListView eventListView;
        private System.Windows.Forms.ImageList eventImageList;
        private System.Windows.Forms.ToolStrip toolStrip6;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton7;

    }
}
