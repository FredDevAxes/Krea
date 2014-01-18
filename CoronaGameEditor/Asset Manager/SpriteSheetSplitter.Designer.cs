namespace Krea.Asset_Manager
{
    partial class SpriteSheetSplitter
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.framesListView = new System.Windows.Forms.ListView();
            this.framesImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.importAllFramesBt = new System.Windows.Forms.ToolStripButton();
            this.importFrameBt = new System.Windows.Forms.ToolStripButton();
            this.importBt = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.removeSelectedFramesBt = new System.Windows.Forms.ToolStripButton();
            this.sheetPictBx = new System.Windows.Forms.PictureBox();
            this.graduationBarX = new Krea.GameEditor.GraduationBar();
            this.graduationBarY = new Krea.GameEditor.GraduationBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.gridModeToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.columnsCountTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.linesCountTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.xOffsetTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.yOffsetTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tileWidthTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tileHeightTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.gridProcessBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.navigationToolStrip = new System.Windows.Forms.ToolStrip();
            this.loadSheetBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.manualModeBt = new System.Windows.Forms.ToolStripButton();
            this.alphaCutBt = new System.Windows.Forms.ToolStripButton();
            this.gridModeBt = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.colorSelectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectedColorBt = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetPictBx)).BeginInit();
            this.gridModeToolStrip.SuspendLayout();
            this.navigationToolStrip.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
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
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.framesListView);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip3);
            this.splitContainer1.Panel1.Controls.Add(this.importBt);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sheetPictBx);
            this.splitContainer1.Panel2.Controls.Add(this.graduationBarX);
            this.splitContainer1.Panel2.Controls.Add(this.graduationBarY);
            this.splitContainer1.Panel2.Controls.Add(this.hScrollBar1);
            this.splitContainer1.Panel2.Controls.Add(this.vScrollBar1);
            this.splitContainer1.Panel2.Controls.Add(this.gridModeToolStrip);
            this.splitContainer1.Panel2.Controls.Add(this.navigationToolStrip);
            this.splitContainer1.Size = new System.Drawing.Size(778, 566);
            this.splitContainer1.SplitterDistance = 182;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // framesListView
            // 
            this.framesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framesListView.HideSelection = false;
            this.framesListView.LargeImageList = this.framesImageList;
            this.framesListView.Location = new System.Drawing.Point(0, 31);
            this.framesListView.Name = "framesListView";
            this.framesListView.Size = new System.Drawing.Size(146, 485);
            this.framesListView.TabIndex = 31;
            this.framesListView.UseCompatibleStateImageBehavior = false;
            // 
            // framesImageList
            // 
            this.framesImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.framesImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.framesImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip3
            // 
            this.toolStrip3.AutoSize = false;
            this.toolStrip3.BackColor = System.Drawing.Color.LightBlue;
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip3.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importAllFramesBt,
            this.importFrameBt});
            this.toolStrip3.Location = new System.Drawing.Point(146, 31);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 120, 1, 0);
            this.toolStrip3.Size = new System.Drawing.Size(36, 485);
            this.toolStrip3.TabIndex = 30;
            // 
            // importAllFramesBt
            // 
            this.importAllFramesBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.importAllFramesBt.Image = global::Krea.Properties.Resources.doubleFlecheDrotieIcon;
            this.importAllFramesBt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.importAllFramesBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importAllFramesBt.Name = "importAllFramesBt";
            this.importAllFramesBt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.importAllFramesBt.RightToLeftAutoMirrorImage = true;
            this.importAllFramesBt.Size = new System.Drawing.Size(34, 28);
            this.importAllFramesBt.Text = "Import all frames";
            this.importAllFramesBt.Click += new System.EventHandler(this.importAllFramesBt_Click);
            // 
            // importFrameBt
            // 
            this.importFrameBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.importFrameBt.Image = global::Krea.Properties.Resources.flecheDroiteIcon;
            this.importFrameBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importFrameBt.Name = "importFrameBt";
            this.importFrameBt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.importFrameBt.RightToLeftAutoMirrorImage = true;
            this.importFrameBt.Size = new System.Drawing.Size(34, 28);
            this.importFrameBt.Text = "Import only selected frames";
            this.importFrameBt.Click += new System.EventHandler(this.importFrameBt_Click);
            // 
            // importBt
            // 
            this.importBt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.importBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importBt.ForeColor = System.Drawing.Color.Black;
            this.importBt.Image = global::Krea.Properties.Resources.validateBlue;
            this.importBt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.importBt.Location = new System.Drawing.Point(0, 516);
            this.importBt.Name = "importBt";
            this.importBt.Size = new System.Drawing.Size(182, 50);
            this.importBt.TabIndex = 5;
            this.importBt.Text = "Import";
            this.importBt.UseVisualStyleBackColor = true;
            this.importBt.Click += new System.EventHandler(this.importBt_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightBlue;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSelectedFramesBt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(182, 31);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // removeSelectedFramesBt
            // 
            this.removeSelectedFramesBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeSelectedFramesBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeSelectedFramesBt.Name = "removeSelectedFramesBt";
            this.removeSelectedFramesBt.Size = new System.Drawing.Size(119, 28);
            this.removeSelectedFramesBt.Text = "Remove Frames";
            this.removeSelectedFramesBt.Click += new System.EventHandler(this.removeSelectedFramesBt_Click);
            // 
            // sheetPictBx
            // 
            this.sheetPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetPictBx.Location = new System.Drawing.Point(29, 55);
            this.sheetPictBx.Name = "sheetPictBx";
            this.sheetPictBx.Size = new System.Drawing.Size(470, 494);
            this.sheetPictBx.TabIndex = 40;
            this.sheetPictBx.TabStop = false;
            this.sheetPictBx.SizeChanged += new System.EventHandler(this.sheetPictBx_SizeChanged);
            this.sheetPictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.sheetPictBx_Paint);
            this.sheetPictBx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.sheetPictBx_MouseDown);
            this.sheetPictBx.MouseLeave += new System.EventHandler(this.sheetPictBx_MouseLeave);
            this.sheetPictBx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sheetPictBx_MouseMove);
            this.sheetPictBx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sheetPictBx_MouseUp);
            // 
            // graduationBarX
            // 
            this.graduationBarX.BackColor = System.Drawing.Color.White;
            this.graduationBarX.Dock = System.Windows.Forms.DockStyle.Top;
            this.graduationBarX.Location = new System.Drawing.Point(29, 31);
            this.graduationBarX.Name = "graduationBarX";
            this.graduationBarX.Size = new System.Drawing.Size(470, 24);
            this.graduationBarX.TabIndex = 39;
            // 
            // graduationBarY
            // 
            this.graduationBarY.BackColor = System.Drawing.Color.White;
            this.graduationBarY.Dock = System.Windows.Forms.DockStyle.Left;
            this.graduationBarY.Location = new System.Drawing.Point(0, 31);
            this.graduationBarY.Name = "graduationBarY";
            this.graduationBarY.Size = new System.Drawing.Size(29, 518);
            this.graduationBarY.TabIndex = 38;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 549);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(499, 17);
            this.hScrollBar1.TabIndex = 35;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(499, 31);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 535);
            this.vScrollBar1.TabIndex = 33;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // gridModeToolStrip
            // 
            this.gridModeToolStrip.AutoSize = false;
            this.gridModeToolStrip.BackColor = System.Drawing.Color.LightBlue;
            this.gridModeToolStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.gridModeToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.gridModeToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.gridModeToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.gridModeToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.columnsCountTxtBx,
            this.toolStripLabel2,
            this.linesCountTxtBx,
            this.toolStripLabel5,
            this.xOffsetTxtBx,
            this.toolStripLabel6,
            this.yOffsetTxtBx,
            this.toolStripLabel3,
            this.tileWidthTxtBx,
            this.toolStripLabel4,
            this.tileHeightTxtBx,
            this.gridProcessBt,
            this.toolStripSeparator6});
            this.gridModeToolStrip.Location = new System.Drawing.Point(516, 31);
            this.gridModeToolStrip.Name = "gridModeToolStrip";
            this.gridModeToolStrip.Size = new System.Drawing.Size(70, 535);
            this.gridModeToolStrip.TabIndex = 31;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 15);
            this.toolStripLabel1.Text = "Columns:";
            // 
            // columnsCountTxtBx
            // 
            this.columnsCountTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.columnsCountTxtBx.Name = "columnsCountTxtBx";
            this.columnsCountTxtBx.Size = new System.Drawing.Size(66, 23);
            this.columnsCountTxtBx.Text = "2";
            this.columnsCountTxtBx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(68, 15);
            this.toolStripLabel2.Text = "Lines:";
            // 
            // linesCountTxtBx
            // 
            this.linesCountTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.linesCountTxtBx.Name = "linesCountTxtBx";
            this.linesCountTxtBx.Size = new System.Drawing.Size(66, 23);
            this.linesCountTxtBx.Text = "2";
            this.linesCountTxtBx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(68, 15);
            this.toolStripLabel5.Text = "X Offset:";
            // 
            // xOffsetTxtBx
            // 
            this.xOffsetTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.xOffsetTxtBx.Name = "xOffsetTxtBx";
            this.xOffsetTxtBx.Size = new System.Drawing.Size(66, 23);
            this.xOffsetTxtBx.Text = "0";
            this.xOffsetTxtBx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(68, 15);
            this.toolStripLabel6.Text = "Y Offset:";
            // 
            // yOffsetTxtBx
            // 
            this.yOffsetTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.yOffsetTxtBx.Name = "yOffsetTxtBx";
            this.yOffsetTxtBx.Size = new System.Drawing.Size(66, 23);
            this.yOffsetTxtBx.Text = "0";
            this.yOffsetTxtBx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(68, 15);
            this.toolStripLabel3.Text = "Tile Width:";
            // 
            // tileWidthTxtBx
            // 
            this.tileWidthTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.tileWidthTxtBx.Name = "tileWidthTxtBx";
            this.tileWidthTxtBx.Size = new System.Drawing.Size(66, 23);
            this.tileWidthTxtBx.Text = "100";
            this.tileWidthTxtBx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(68, 15);
            this.toolStripLabel4.Text = "Tile Height:";
            // 
            // tileHeightTxtBx
            // 
            this.tileHeightTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.tileHeightTxtBx.Name = "tileHeightTxtBx";
            this.tileHeightTxtBx.Size = new System.Drawing.Size(66, 23);
            this.tileHeightTxtBx.Text = "100";
            this.tileHeightTxtBx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gridProcessBt
            // 
            this.gridProcessBt.Image = global::Krea.Properties.Resources.settingsIcon;
            this.gridProcessBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gridProcessBt.Name = "gridProcessBt";
            this.gridProcessBt.Size = new System.Drawing.Size(68, 28);
            this.gridProcessBt.Text = "GO";
            this.gridProcessBt.Click += new System.EventHandler(this.gridProcessBt_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(68, 6);
            // 
            // navigationToolStrip
            // 
            this.navigationToolStrip.BackColor = System.Drawing.Color.LightBlue;
            this.navigationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.navigationToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.navigationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSheetBt,
            this.toolStripSeparator3,
            this.toolStripLabel7,
            this.manualModeBt,
            this.alphaCutBt,
            this.gridModeBt});
            this.navigationToolStrip.Location = new System.Drawing.Point(0, 0);
            this.navigationToolStrip.Name = "navigationToolStrip";
            this.navigationToolStrip.ShowItemToolTips = false;
            this.navigationToolStrip.Size = new System.Drawing.Size(586, 31);
            this.navigationToolStrip.TabIndex = 3;
            this.navigationToolStrip.Text = "toolStrip1";
            // 
            // loadSheetBt
            // 
            this.loadSheetBt.Image = global::Krea.Properties.Resources.uploadTextTileIcon;
            this.loadSheetBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadSheetBt.Name = "loadSheetBt";
            this.loadSheetBt.Size = new System.Drawing.Size(92, 28);
            this.loadSheetBt.Text = "Load sheet";
            this.loadSheetBt.Click += new System.EventHandler(this.loadSheetBt_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(46, 28);
            this.toolStripLabel7.Text = "Modes:";
            // 
            // manualModeBt
            // 
            this.manualModeBt.Image = global::Krea.Properties.Resources.editIcon;
            this.manualModeBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.manualModeBt.Name = "manualModeBt";
            this.manualModeBt.Size = new System.Drawing.Size(84, 28);
            this.manualModeBt.Text = "Manually";
            this.manualModeBt.Click += new System.EventHandler(this.runBt_Click);
            // 
            // alphaCutBt
            // 
            this.alphaCutBt.Image = global::Krea.Properties.Resources.buildIcon;
            this.alphaCutBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.alphaCutBt.Name = "alphaCutBt";
            this.alphaCutBt.Size = new System.Drawing.Size(109, 28);
            this.alphaCutBt.Text = "Alpha Cutting";
            this.alphaCutBt.Click += new System.EventHandler(this.alphaCutBt_Click);
            // 
            // gridModeBt
            // 
            this.gridModeBt.Image = global::Krea.Properties.Resources.showGridIcon;
            this.gridModeBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gridModeBt.Name = "gridModeBt";
            this.gridModeBt.Size = new System.Drawing.Size(119, 28);
            this.gridModeBt.Text = "Uniformed Tiles";
            this.gridModeBt.Click += new System.EventHandler(this.gridModeBt_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorSelectedMenuItem,
            this.clearSelectedColorBt});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // colorSelectedMenuItem
            // 
            this.colorSelectedMenuItem.BackColor = System.Drawing.Color.White;
            this.colorSelectedMenuItem.Enabled = false;
            this.colorSelectedMenuItem.Name = "colorSelectedMenuItem";
            this.colorSelectedMenuItem.Size = new System.Drawing.Size(152, 22);
            // 
            // clearSelectedColorBt
            // 
            this.clearSelectedColorBt.Name = "clearSelectedColorBt";
            this.clearSelectedColorBt.Size = new System.Drawing.Size(152, 22);
            this.clearSelectedColorBt.Text = "Clear color";
            this.clearSelectedColorBt.Click += new System.EventHandler(this.clearSelectedColorBt_Click);
            // 
            // SpriteSheetSplitter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SpriteSheetSplitter";
            this.Size = new System.Drawing.Size(778, 566);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetPictBx)).EndInit();
            this.gridModeToolStrip.ResumeLayout(false);
            this.gridModeToolStrip.PerformLayout();
            this.navigationToolStrip.ResumeLayout(false);
            this.navigationToolStrip.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button importBt;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton removeSelectedFramesBt;
        private System.Windows.Forms.ToolStrip navigationToolStrip;
        private System.Windows.Forms.ToolStripButton manualModeBt;
        private System.Windows.Forms.ToolStripButton alphaCutBt;
        private System.Windows.Forms.ImageList framesImageList;
        private System.Windows.Forms.ToolStripButton loadSheetBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton gridModeBt;
        private System.Windows.Forms.ListView framesListView;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton importAllFramesBt;
        private System.Windows.Forms.ToolStripButton importFrameBt;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ToolStrip gridModeToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox columnsCountTxtBx;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox linesCountTxtBx;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox tileWidthTxtBx;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox tileHeightTxtBx;
        private System.Windows.Forms.ToolStripButton gridProcessBt;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.PictureBox sheetPictBx;
        private GameEditor.GraduationBar graduationBarX;
        private GameEditor.GraduationBar graduationBarY;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox xOffsetTxtBx;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripTextBox yOffsetTxtBx;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem colorSelectedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectedColorBt;
    }
}
