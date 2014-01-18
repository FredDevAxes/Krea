namespace Krea.Debugger
{
    partial class DebuggerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebuggerPanel));
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer2 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer3 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            this.navigationToolStrip = new System.Windows.Forms.ToolStrip();
            this.runBt = new System.Windows.Forms.ToolStripButton();
            this.stepBt = new System.Windows.Forms.ToolStripButton();
            this.overBt = new System.Windows.Forms.ToolStripButton();
            this.closeDebuggerSessionBt = new System.Windows.Forms.ToolStripButton();
            this.typesImageList = new System.Windows.Forms.ImageList(this.components);
            this.sendCommandToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.commandTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.sendCommandBt = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.OutputTabPage = new System.Windows.Forms.TabPage();
            this.outPutTxtBx = new System.Windows.Forms.RichTextBox();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.backtraceListView = new System.Windows.Forms.TreeListView();
            this.numberbacktraceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.kindHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.filebacktraceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lineBackTraceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.localsListView = new System.Windows.Forms.TreeListView();
            this.localsNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeLocalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.localValueHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.watchesListView = new System.Windows.Forms.TreeListView();
            this.numberWatchHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.expressionHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.navigationToolStrip.SuspendLayout();
            this.sendCommandToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.OutputTabPage.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationToolStrip
            // 
            this.navigationToolStrip.BackColor = System.Drawing.Color.White;
            this.navigationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.navigationToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.navigationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runBt,
            this.stepBt,
            this.overBt,
            this.closeDebuggerSessionBt});
            this.navigationToolStrip.Location = new System.Drawing.Point(0, 0);
            this.navigationToolStrip.Name = "navigationToolStrip";
            this.navigationToolStrip.ShowItemToolTips = false;
            this.navigationToolStrip.Size = new System.Drawing.Size(515, 31);
            this.navigationToolStrip.TabIndex = 1;
            this.navigationToolStrip.Text = "toolStrip1";
            // 
            // runBt
            // 
            this.runBt.Image = global::Krea.Properties.Resources.playItem;
            this.runBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runBt.Name = "runBt";
            this.runBt.Size = new System.Drawing.Size(56, 28);
            this.runBt.Text = "Run";
            this.runBt.ToolTipText = "Once having loaded the main.lua of your project into the Corona simulator and add" +
    "ed all the breakpoints needed, run the app in debug mode.";
            this.runBt.Click += new System.EventHandler(this.runBt_Click);
            // 
            // stepBt
            // 
            this.stepBt.Image = global::Krea.Properties.Resources.flecheBasIcon;
            this.stepBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stepBt.Name = "stepBt";
            this.stepBt.Size = new System.Drawing.Size(58, 28);
            this.stepBt.Text = "Step";
            this.stepBt.ToolTipText = "Execute the next step";
            this.stepBt.Click += new System.EventHandler(this.stepBt_Click);
            // 
            // overBt
            // 
            this.overBt.Image = global::Krea.Properties.Resources.flecheHautIcon;
            this.overBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.overBt.Name = "overBt";
            this.overBt.Size = new System.Drawing.Size(60, 28);
            this.overBt.Text = "Over";
            this.overBt.ToolTipText = "Go over the current function";
            this.overBt.Click += new System.EventHandler(this.overBt_Click);
            // 
            // closeDebuggerSessionBt
            // 
            this.closeDebuggerSessionBt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.closeDebuggerSessionBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.closeDebuggerSessionBt.Image = global::Krea.Properties.Resources.deleteIcon;
            this.closeDebuggerSessionBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeDebuggerSessionBt.Name = "closeDebuggerSessionBt";
            this.closeDebuggerSessionBt.Size = new System.Drawing.Size(28, 28);
            this.closeDebuggerSessionBt.Text = "close";
            this.closeDebuggerSessionBt.ToolTipText = "Close the debugging session and then the debugger panel.";
            this.closeDebuggerSessionBt.Click += new System.EventHandler(this.toolStripButton1_Click);
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
            // sendCommandToolStrip
            // 
            this.sendCommandToolStrip.BackColor = System.Drawing.Color.White;
            this.sendCommandToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.sendCommandToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.sendCommandToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.commandTxtBx,
            this.sendCommandBt});
            this.sendCommandToolStrip.Location = new System.Drawing.Point(0, 31);
            this.sendCommandToolStrip.Name = "sendCommandToolStrip";
            this.sendCommandToolStrip.ShowItemToolTips = false;
            this.sendCommandToolStrip.Size = new System.Drawing.Size(515, 31);
            this.sendCommandToolStrip.TabIndex = 4;
            this.sendCommandToolStrip.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(96, 28);
            this.toolStripLabel2.Text = "Send Command:";
            // 
            // commandTxtBx
            // 
            this.commandTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.commandTxtBx.Name = "commandTxtBx";
            this.commandTxtBx.Size = new System.Drawing.Size(150, 31);
            // 
            // sendCommandBt
            // 
            this.sendCommandBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sendCommandBt.Image = global::Krea.Properties.Resources.tickIcon;
            this.sendCommandBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sendCommandBt.Name = "sendCommandBt";
            this.sendCommandBt.Size = new System.Drawing.Size(28, 28);
            this.sendCommandBt.Text = "Send";
            this.sendCommandBt.ToolTipText = "Send a custom commant to the Corona debugger";
            this.sendCommandBt.Click += new System.EventHandler(this.sendCommandBt_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 62);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(515, 512);
            this.splitContainer1.SplitterDistance = 294;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer3.Size = new System.Drawing.Size(515, 294);
            this.splitContainer3.SplitterDistance = 105;
            this.splitContainer3.SplitterWidth = 10;
            this.splitContainer3.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.OutputTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(515, 105);
            this.tabControl1.TabIndex = 3;
            // 
            // OutputTabPage
            // 
            this.OutputTabPage.AutoScroll = true;
            this.OutputTabPage.Controls.Add(this.outPutTxtBx);
            this.OutputTabPage.Location = new System.Drawing.Point(4, 22);
            this.OutputTabPage.Name = "OutputTabPage";
            this.OutputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.OutputTabPage.Size = new System.Drawing.Size(507, 79);
            this.OutputTabPage.TabIndex = 1;
            this.OutputTabPage.Text = "Output";
            this.OutputTabPage.UseVisualStyleBackColor = true;
            // 
            // outPutTxtBx
            // 
            this.outPutTxtBx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outPutTxtBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outPutTxtBx.Location = new System.Drawing.Point(3, 3);
            this.outPutTxtBx.Name = "outPutTxtBx";
            this.outPutTxtBx.Size = new System.Drawing.Size(501, 73);
            this.outPutTxtBx.TabIndex = 32;
            this.outPutTxtBx.Text = "";
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(515, 179);
            this.tabControl3.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.backtraceListView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(507, 153);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Backtrace";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // backtraceListView
            // 
            this.backtraceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.numberbacktraceHeader,
            this.kindHeader,
            this.filebacktraceHeader,
            this.lineBackTraceHeader});
            treeListViewItemCollectionComparer1.Column = 0;
            treeListViewItemCollectionComparer1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.backtraceListView.Comparer = treeListViewItemCollectionComparer1;
            this.backtraceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backtraceListView.GridLines = true;
            this.backtraceListView.HideSelection = false;
            this.backtraceListView.LargeImageList = this.typesImageList;
            this.backtraceListView.Location = new System.Drawing.Point(3, 3);
            this.backtraceListView.MultiSelect = false;
            this.backtraceListView.Name = "backtraceListView";
            this.backtraceListView.Size = new System.Drawing.Size(501, 147);
            this.backtraceListView.SmallImageList = this.typesImageList;
            this.backtraceListView.Sorting = System.Windows.Forms.SortOrder.None;
            this.backtraceListView.TabIndex = 2;
            this.backtraceListView.UseCompatibleStateImageBehavior = false;
            // 
            // numberbacktraceHeader
            // 
            this.numberbacktraceHeader.Text = "Number";
            this.numberbacktraceHeader.Width = 58;
            // 
            // kindHeader
            // 
            this.kindHeader.Text = "Kind";
            this.kindHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.kindHeader.Width = 100;
            // 
            // filebacktraceHeader
            // 
            this.filebacktraceHeader.Text = "File";
            this.filebacktraceHeader.Width = 93;
            // 
            // lineBackTraceHeader
            // 
            this.lineBackTraceHeader.Text = "Line";
            this.lineBackTraceHeader.Width = 48;
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
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl4);
            this.splitContainer2.Panel2Collapsed = true;
            this.splitContainer2.Size = new System.Drawing.Size(515, 208);
            this.splitContainer2.SplitterDistance = 106;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(515, 208);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.localsListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(507, 182);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Locals";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // localsListView
            // 
            this.localsListView.AutoArrange = false;
            this.localsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.localsNameHeader,
            this.typeLocalHeader,
            this.localValueHeader});
            treeListViewItemCollectionComparer2.Column = 0;
            treeListViewItemCollectionComparer2.SortOrder = System.Windows.Forms.SortOrder.None;
            this.localsListView.Comparer = treeListViewItemCollectionComparer2;
            this.localsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localsListView.GridLines = true;
            this.localsListView.HideSelection = false;
            this.localsListView.LargeImageList = this.typesImageList;
            this.localsListView.Location = new System.Drawing.Point(3, 3);
            this.localsListView.MultiSelect = false;
            this.localsListView.Name = "localsListView";
            this.localsListView.Size = new System.Drawing.Size(501, 176);
            this.localsListView.SmallImageList = this.typesImageList;
            this.localsListView.Sorting = System.Windows.Forms.SortOrder.None;
            this.localsListView.TabIndex = 1;
            this.localsListView.UseCompatibleStateImageBehavior = false;
            this.localsListView.SelectedIndexChanged += new System.EventHandler(this.localsListView_SelectedIndexChanged);
            // 
            // localsNameHeader
            // 
            this.localsNameHeader.Text = "Name";
            this.localsNameHeader.Width = 107;
            // 
            // typeLocalHeader
            // 
            this.typeLocalHeader.Text = "Type";
            this.typeLocalHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.typeLocalHeader.Width = 64;
            // 
            // localValueHeader
            // 
            this.localValueHeader.Text = "Value";
            this.localValueHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.localValueHeader.Width = 70;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage4);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(0, 0);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(150, 46);
            this.tabControl4.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.watchesListView);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(142, 20);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Watches";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // watchesListView
            // 
            this.watchesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.numberWatchHeader,
            this.expressionHeader});
            treeListViewItemCollectionComparer3.Column = 0;
            treeListViewItemCollectionComparer3.SortOrder = System.Windows.Forms.SortOrder.None;
            this.watchesListView.Comparer = treeListViewItemCollectionComparer3;
            this.watchesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.watchesListView.GridLines = true;
            this.watchesListView.Location = new System.Drawing.Point(3, 3);
            this.watchesListView.MultiSelect = false;
            this.watchesListView.Name = "watchesListView";
            this.watchesListView.Size = new System.Drawing.Size(136, 14);
            this.watchesListView.Sorting = System.Windows.Forms.SortOrder.None;
            this.watchesListView.TabIndex = 0;
            this.watchesListView.UseCompatibleStateImageBehavior = false;
            // 
            // numberWatchHeader
            // 
            this.numberWatchHeader.Text = "Number";
            // 
            // expressionHeader
            // 
            this.expressionHeader.Text = "Expression";
            this.expressionHeader.Width = 132;
            // 
            // DebuggerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.sendCommandToolStrip);
            this.Controls.Add(this.navigationToolStrip);
            this.Name = "DebuggerPanel";
            this.Size = new System.Drawing.Size(515, 574);
            this.navigationToolStrip.ResumeLayout(false);
            this.navigationToolStrip.PerformLayout();
            this.sendCommandToolStrip.ResumeLayout(false);
            this.sendCommandToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.OutputTabPage.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip navigationToolStrip;
        private System.Windows.Forms.ToolStripButton runBt;
        private System.Windows.Forms.ToolStripButton stepBt;
        private System.Windows.Forms.ToolStripButton overBt;
        private System.Windows.Forms.ImageList typesImageList;
        private System.Windows.Forms.ToolStrip sendCommandToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox commandTxtBx;
        private System.Windows.Forms.ToolStripButton sendCommandBt;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage OutputTabPage;
        private System.Windows.Forms.RichTextBox outPutTxtBx;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeListView backtraceListView;
        private System.Windows.Forms.ColumnHeader numberbacktraceHeader;
        private System.Windows.Forms.ColumnHeader kindHeader;
        private System.Windows.Forms.ColumnHeader filebacktraceHeader;
        private System.Windows.Forms.ColumnHeader lineBackTraceHeader;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeListView localsListView;
        private System.Windows.Forms.ColumnHeader localsNameHeader;
        private System.Windows.Forms.ColumnHeader typeLocalHeader;
        private System.Windows.Forms.ColumnHeader localValueHeader;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TreeListView watchesListView;
        private System.Windows.Forms.ColumnHeader numberWatchHeader;
        private System.Windows.Forms.ColumnHeader expressionHeader;
        private System.Windows.Forms.ToolStripButton closeDebuggerSessionBt;
    }
}
