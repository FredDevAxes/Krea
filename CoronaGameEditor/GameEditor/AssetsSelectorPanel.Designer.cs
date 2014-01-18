namespace Krea
{
    partial class ImageObjectsPanel
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
            this.label4 = new System.Windows.Forms.Label();
            this.assetsProjectsCmbBx = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.objectCategoryCmbBx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.assetsListView = new System.Windows.Forms.ListView();
            this.assetsImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Project Source:";
            // 
            // assetsProjectsCmbBx
            // 
            this.assetsProjectsCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.assetsProjectsCmbBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.assetsProjectsCmbBx.FormattingEnabled = true;
            this.assetsProjectsCmbBx.Location = new System.Drawing.Point(0, 15);
            this.assetsProjectsCmbBx.Name = "assetsProjectsCmbBx";
            this.assetsProjectsCmbBx.Size = new System.Drawing.Size(152, 23);
            this.assetsProjectsCmbBx.TabIndex = 13;
            this.assetsProjectsCmbBx.Tag = "false";
            this.assetsProjectsCmbBx.SelectedIndexChanged += new System.EventHandler(this.assetsProjectsCmbBx_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "Asset category:";
            // 
            // objectCategoryCmbBx
            // 
            this.objectCategoryCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.objectCategoryCmbBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.objectCategoryCmbBx.FormattingEnabled = true;
            this.objectCategoryCmbBx.Items.AddRange(new object[] {
            "Images",
            "Sprites",
            "Audios",
            "Snippets",
            "Fonts"});
            this.objectCategoryCmbBx.Location = new System.Drawing.Point(0, 53);
            this.objectCategoryCmbBx.Name = "objectCategoryCmbBx";
            this.objectCategoryCmbBx.Size = new System.Drawing.Size(152, 23);
            this.objectCategoryCmbBx.TabIndex = 16;
            this.objectCategoryCmbBx.Tag = "false";
            this.objectCategoryCmbBx.SelectedIndexChanged += new System.EventHandler(this.objectCategoryCmbBx_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Assets available:";
            // 
            // assetsListView
            // 
            this.assetsListView.AllowDrop = true;
            this.assetsListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.assetsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetsListView.LargeImageList = this.assetsImageList;
            this.assetsListView.Location = new System.Drawing.Point(0, 91);
            this.assetsListView.MultiSelect = false;
            this.assetsListView.Name = "assetsListView";
            this.assetsListView.Size = new System.Drawing.Size(152, 481);
            this.assetsListView.TabIndex = 18;
            this.assetsListView.Tag = "false";
            this.assetsListView.UseCompatibleStateImageBehavior = false;
            this.assetsListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.assetsListView_ItemDrag);
            this.assetsListView.SelectedIndexChanged += new System.EventHandler(this.assetsListView_SelectedIndexChanged);
            this.assetsListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.assetsListView_MouseDoubleClick);
            this.assetsListView.MouseEnter += new System.EventHandler(this.assetsListView_MouseEnter);
            // 
            // assetsImageList
            // 
            this.assetsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.assetsImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.assetsImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ImageObjectsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.assetsListView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.objectCategoryCmbBx);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.assetsProjectsCmbBx);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImageObjectsPanel";
            this.Size = new System.Drawing.Size(152, 572);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox assetsProjectsCmbBx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox objectCategoryCmbBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList assetsImageList;
        private System.Windows.Forms.ListView assetsListView;


    }
}
