namespace Krea.GameEditor.TilesMapping
{
    partial class TilesManagerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TilesManagerPanel));
            this.tilesTabIcons = new System.Windows.Forms.ToolStrip();
            this.loadObjectSet = new System.Windows.Forms.ToolStripButton();
            this.loadTextureSet = new System.Windows.Forms.ToolStripButton();
            this.tilesSizeCombBx = new System.Windows.Forms.ToolStripComboBox();
            this.removeCurrentPalette = new System.Windows.Forms.ToolStripButton();
            this.texturesTabControl = new System.Windows.Forms.TabControl();
            this.tileTexturesPage = new System.Windows.Forms.TabPage();
            this.tileTexturesPictBx = new System.Windows.Forms.PictureBox();
            this.texturesTileScollBar = new System.Windows.Forms.TrackBar();
            this.textureSheetCombBx = new System.Windows.Forms.ComboBox();
            this.tileObjectsPage = new System.Windows.Forms.TabPage();
            this.tileObjectsPictBx = new System.Windows.Forms.PictureBox();
            this.objectTilesScrollBar = new System.Windows.Forms.TrackBar();
            this.objectSheetsCombBx = new System.Windows.Forms.ComboBox();
            this.eventImageList = new System.Windows.Forms.ImageList(this.components);
            this.textureMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToPaletteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createObjectSequenceBt = new System.Windows.Forms.ToolStripMenuItem();
            this.tilesTabIcons.SuspendLayout();
            this.texturesTabControl.SuspendLayout();
            this.tileTexturesPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileTexturesPictBx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texturesTileScollBar)).BeginInit();
            this.tileObjectsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tileObjectsPictBx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectTilesScrollBar)).BeginInit();
            this.textureMenuStrip.SuspendLayout();
            this.objectMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tilesTabIcons
            // 
            this.tilesTabIcons.BackColor = System.Drawing.Color.Transparent;
            this.tilesTabIcons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tilesTabIcons.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tilesTabIcons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadObjectSet,
            this.loadTextureSet,
            this.tilesSizeCombBx,
            this.removeCurrentPalette});
            this.tilesTabIcons.Location = new System.Drawing.Point(0, 0);
            this.tilesTabIcons.Name = "tilesTabIcons";
            this.tilesTabIcons.Size = new System.Drawing.Size(163, 31);
            this.tilesTabIcons.TabIndex = 0;
            // 
            // loadObjectSet
            // 
            this.loadObjectSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadObjectSet.Image = global::Krea.Properties.Resources.uploadObjTileIcon;
            this.loadObjectSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadObjectSet.Name = "loadObjectSet";
            this.loadObjectSet.Size = new System.Drawing.Size(28, 28);
            this.loadObjectSet.Text = "Load Object TilesSheet";
            this.loadObjectSet.ToolTipText = "Load an image with tiles having exactly the following size selected. The tiles on" +
    "ce cutted will be set as OBJECT tiles, that is the top layer of a tile.";
            this.loadObjectSet.Click += new System.EventHandler(this.loadObjectSet_Click);
            // 
            // loadTextureSet
            // 
            this.loadTextureSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadTextureSet.Image = global::Krea.Properties.Resources.uploadTextTileIcon;
            this.loadTextureSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadTextureSet.Name = "loadTextureSet";
            this.loadTextureSet.Size = new System.Drawing.Size(28, 28);
            this.loadTextureSet.Text = "Load Texture TilesSheet";
            this.loadTextureSet.ToolTipText = "Load an image with tiles having exactly the following size selected. The tiles on" +
    "ce cutted will be set as TEXTURE tiles, that is the back layer of a tile.";
            this.loadTextureSet.Click += new System.EventHandler(this.loadTextureSet_Click);
            // 
            // tilesSizeCombBx
            // 
            this.tilesSizeCombBx.BackColor = System.Drawing.SystemColors.Info;
            this.tilesSizeCombBx.Items.AddRange(new object[] {
            "32x32",
            "64x64",
            "128x128",
            "256x256",
            "512x512"});
            this.tilesSizeCombBx.Name = "tilesSizeCombBx";
            this.tilesSizeCombBx.Size = new System.Drawing.Size(100, 23);
            this.tilesSizeCombBx.Text = "Select tiles size";
            // 
            // removeCurrentPalette
            // 
            this.removeCurrentPalette.BackColor = System.Drawing.Color.White;
            this.removeCurrentPalette.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeCurrentPalette.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeCurrentPalette.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeCurrentPalette.Name = "removeCurrentPalette";
            this.removeCurrentPalette.Size = new System.Drawing.Size(28, 28);
            this.removeCurrentPalette.Text = "Remove current palette";
            this.removeCurrentPalette.Click += new System.EventHandler(this.removeCurrentPalette_Click);
            // 
            // texturesTabControl
            // 
            this.texturesTabControl.Controls.Add(this.tileTexturesPage);
            this.texturesTabControl.Controls.Add(this.tileObjectsPage);
            this.texturesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texturesTabControl.Location = new System.Drawing.Point(0, 31);
            this.texturesTabControl.Multiline = true;
            this.texturesTabControl.Name = "texturesTabControl";
            this.texturesTabControl.SelectedIndex = 0;
            this.texturesTabControl.Size = new System.Drawing.Size(163, 388);
            this.texturesTabControl.TabIndex = 1;
            this.texturesTabControl.SelectedIndexChanged += new System.EventHandler(this.texturesTabControl_SelectedIndexChanged);
            this.texturesTabControl.SizeChanged += new System.EventHandler(this.texturesTabControl_SizeChanged);
            // 
            // tileTexturesPage
            // 
            this.tileTexturesPage.AutoScroll = true;
            this.tileTexturesPage.BackColor = System.Drawing.Color.White;
            this.tileTexturesPage.Controls.Add(this.tileTexturesPictBx);
            this.tileTexturesPage.Controls.Add(this.texturesTileScollBar);
            this.tileTexturesPage.Controls.Add(this.textureSheetCombBx);
            this.tileTexturesPage.Location = new System.Drawing.Point(4, 22);
            this.tileTexturesPage.Name = "tileTexturesPage";
            this.tileTexturesPage.Padding = new System.Windows.Forms.Padding(3);
            this.tileTexturesPage.Size = new System.Drawing.Size(155, 362);
            this.tileTexturesPage.TabIndex = 0;
            this.tileTexturesPage.Text = "Textures";
            // 
            // tileTexturesPictBx
            // 
            this.tileTexturesPictBx.BackColor = System.Drawing.Color.Transparent;
            this.tileTexturesPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileTexturesPictBx.Location = new System.Drawing.Point(3, 24);
            this.tileTexturesPictBx.Name = "tileTexturesPictBx";
            this.tileTexturesPictBx.Size = new System.Drawing.Size(129, 335);
            this.tileTexturesPictBx.TabIndex = 6;
            this.tileTexturesPictBx.TabStop = false;
            this.tileTexturesPictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.tileTexturesPictBx_Paint);
            this.tileTexturesPictBx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tileTexturesList_MouseClick);
            // 
            // texturesTileScollBar
            // 
            this.texturesTileScollBar.AutoSize = false;
            this.texturesTileScollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.texturesTileScollBar.Location = new System.Drawing.Point(132, 24);
            this.texturesTileScollBar.Maximum = 0;
            this.texturesTileScollBar.Name = "texturesTileScollBar";
            this.texturesTileScollBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.texturesTileScollBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.texturesTileScollBar.Size = new System.Drawing.Size(20, 335);
            this.texturesTileScollBar.TabIndex = 5;
            this.texturesTileScollBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.texturesTileScollBar.Scroll += new System.EventHandler(this.texturesTileScollBar_Scroll);
            // 
            // textureSheetCombBx
            // 
            this.textureSheetCombBx.BackColor = System.Drawing.SystemColors.Info;
            this.textureSheetCombBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.textureSheetCombBx.FormattingEnabled = true;
            this.textureSheetCombBx.Location = new System.Drawing.Point(3, 3);
            this.textureSheetCombBx.Name = "textureSheetCombBx";
            this.textureSheetCombBx.Size = new System.Drawing.Size(149, 21);
            this.textureSheetCombBx.TabIndex = 1;
            this.textureSheetCombBx.Tag = "false";
            this.textureSheetCombBx.SelectedIndexChanged += new System.EventHandler(this.textureSheetCombBx_SelectedIndexChanged);
            // 
            // tileObjectsPage
            // 
            this.tileObjectsPage.AutoScroll = true;
            this.tileObjectsPage.Controls.Add(this.tileObjectsPictBx);
            this.tileObjectsPage.Controls.Add(this.objectTilesScrollBar);
            this.tileObjectsPage.Controls.Add(this.objectSheetsCombBx);
            this.tileObjectsPage.Location = new System.Drawing.Point(4, 22);
            this.tileObjectsPage.Name = "tileObjectsPage";
            this.tileObjectsPage.Padding = new System.Windows.Forms.Padding(3);
            this.tileObjectsPage.Size = new System.Drawing.Size(155, 362);
            this.tileObjectsPage.TabIndex = 1;
            this.tileObjectsPage.Text = "Objects";
            this.tileObjectsPage.UseVisualStyleBackColor = true;
            // 
            // tileObjectsPictBx
            // 
            this.tileObjectsPictBx.BackColor = System.Drawing.Color.Transparent;
            this.tileObjectsPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileObjectsPictBx.Location = new System.Drawing.Point(3, 24);
            this.tileObjectsPictBx.Name = "tileObjectsPictBx";
            this.tileObjectsPictBx.Size = new System.Drawing.Size(129, 335);
            this.tileObjectsPictBx.TabIndex = 7;
            this.tileObjectsPictBx.TabStop = false;
            this.tileObjectsPictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.tileObjectsPictBx_Paint);
            this.tileObjectsPictBx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tileObjectsSurface_MouseClick);
            // 
            // objectTilesScrollBar
            // 
            this.objectTilesScrollBar.AutoSize = false;
            this.objectTilesScrollBar.BackColor = System.Drawing.Color.White;
            this.objectTilesScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.objectTilesScrollBar.Location = new System.Drawing.Point(132, 24);
            this.objectTilesScrollBar.Maximum = 0;
            this.objectTilesScrollBar.Name = "objectTilesScrollBar";
            this.objectTilesScrollBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.objectTilesScrollBar.Size = new System.Drawing.Size(20, 335);
            this.objectTilesScrollBar.TabIndex = 6;
            this.objectTilesScrollBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.objectTilesScrollBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // objectSheetsCombBx
            // 
            this.objectSheetsCombBx.BackColor = System.Drawing.SystemColors.Info;
            this.objectSheetsCombBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.objectSheetsCombBx.FormattingEnabled = true;
            this.objectSheetsCombBx.Location = new System.Drawing.Point(3, 3);
            this.objectSheetsCombBx.Name = "objectSheetsCombBx";
            this.objectSheetsCombBx.Size = new System.Drawing.Size(149, 21);
            this.objectSheetsCombBx.TabIndex = 1;
            this.objectSheetsCombBx.Tag = "false";
            this.objectSheetsCombBx.SelectedIndexChanged += new System.EventHandler(this.objectSheetsCombBx_SelectedIndexChanged);
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
            // textureMenuStrip
            // 
            this.textureMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToPaletteToolStripMenuItem1,
            this.createSequenceToolStripMenuItem});
            this.textureMenuStrip.Name = "textureMenuStrip";
            this.textureMenuStrip.Size = new System.Drawing.Size(163, 48);
            // 
            // addToPaletteToolStripMenuItem1
            // 
            this.addToPaletteToolStripMenuItem1.Name = "addToPaletteToolStripMenuItem1";
            this.addToPaletteToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.addToPaletteToolStripMenuItem1.Text = "Add To Palette";
            this.addToPaletteToolStripMenuItem1.Click += new System.EventHandler(this.addToPaletteToolStripMenuItem_Click);
            // 
            // createSequenceToolStripMenuItem
            // 
            this.createSequenceToolStripMenuItem.Name = "createSequenceToolStripMenuItem";
            this.createSequenceToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.createSequenceToolStripMenuItem.Text = "Create Sequence";
            this.createSequenceToolStripMenuItem.Click += new System.EventHandler(this.createSequenceToolStripMenuItem_Click);
            // 
            // objectMenuStrip
            // 
            this.objectMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToPaletteToolStripMenuItem,
            this.createObjectSequenceBt});
            this.objectMenuStrip.Name = "textureMenuStrip";
            this.objectMenuStrip.Size = new System.Drawing.Size(163, 48);
            // 
            // addToPaletteToolStripMenuItem
            // 
            this.addToPaletteToolStripMenuItem.Name = "addToPaletteToolStripMenuItem";
            this.addToPaletteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addToPaletteToolStripMenuItem.Text = "Add To Palette";
            this.addToPaletteToolStripMenuItem.Click += new System.EventHandler(this.addToPaletteToolStripMenuItem_Click);
            // 
            // createObjectSequenceBt
            // 
            this.createObjectSequenceBt.Name = "createObjectSequenceBt";
            this.createObjectSequenceBt.Size = new System.Drawing.Size(162, 22);
            this.createObjectSequenceBt.Text = "Create Sequence";
            this.createObjectSequenceBt.Click += new System.EventHandler(this.createObjectSequenceBt_Click);
            // 
            // TilesManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.texturesTabControl);
            this.Controls.Add(this.tilesTabIcons);
            this.Name = "TilesManagerPanel";
            this.Size = new System.Drawing.Size(163, 419);
            this.tilesTabIcons.ResumeLayout(false);
            this.tilesTabIcons.PerformLayout();
            this.texturesTabControl.ResumeLayout(false);
            this.tileTexturesPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tileTexturesPictBx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texturesTileScollBar)).EndInit();
            this.tileObjectsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tileObjectsPictBx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectTilesScrollBar)).EndInit();
            this.textureMenuStrip.ResumeLayout(false);
            this.objectMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tilesTabIcons;
        private System.Windows.Forms.ToolStripButton loadTextureSet;
        private System.Windows.Forms.ToolStripButton loadObjectSet;
        private System.Windows.Forms.TabControl texturesTabControl;
        private System.Windows.Forms.TabPage tileTexturesPage;
        private System.Windows.Forms.TabPage tileObjectsPage;
        private System.Windows.Forms.ToolStripComboBox tilesSizeCombBx;
        private System.Windows.Forms.ComboBox textureSheetCombBx;
        private System.Windows.Forms.ComboBox objectSheetsCombBx;
        private System.Windows.Forms.ToolStripButton removeCurrentPalette;
        private System.Windows.Forms.ImageList eventImageList;
        private System.Windows.Forms.ContextMenuStrip textureMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem createSequenceToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip objectMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem createObjectSequenceBt;
        public System.Windows.Forms.PictureBox tileTexturesPictBx;
        private System.Windows.Forms.TrackBar texturesTileScollBar;
        public System.Windows.Forms.PictureBox tileObjectsPictBx;
        private System.Windows.Forms.TrackBar objectTilesScrollBar;
        private System.Windows.Forms.ToolStripMenuItem addToPaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToPaletteToolStripMenuItem1;
    }
}
