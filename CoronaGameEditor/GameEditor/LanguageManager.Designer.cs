namespace Krea.GameEditor
{
    partial class LanguageManager
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.languagesListBx = new System.Windows.Forms.ListView();
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.langueNameCb = new System.Windows.Forms.ToolStripComboBox();
            this.removeBt = new System.Windows.Forms.ToolStripButton();
            this.AddBt = new System.Windows.Forms.ToolStripButton();
            this.LoadtBt = new System.Windows.Forms.ToolStripButton();
            this.SavetBt = new System.Windows.Forms.ToolStripButton();
            this.defaultLangueCb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tranlationGridView = new System.Windows.Forms.DataGridView();
            this.keyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.translationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.translateFromLanguageCmbBx = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.translateToLanguageCmbBx = new System.Windows.Forms.ToolStripComboBox();
            this.translateNowBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.translationStatusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.appConfigToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.newKeyTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.newValueTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.removeFieldBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addFieldBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.translationBackWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tranlationGridView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.appConfigToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.defaultLangueCb);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tranlationGridView);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.appConfigToolStrip);
            this.splitContainer1.Size = new System.Drawing.Size(739, 513);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.languagesListBx);
            this.groupBox1.Controls.Add(this.toolStrip6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(739, 157);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project Languages:";
            // 
            // languagesListBx
            // 
            this.languagesListBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.languagesListBx.Location = new System.Drawing.Point(3, 47);
            this.languagesListBx.Name = "languagesListBx";
            this.languagesListBx.Size = new System.Drawing.Size(733, 107);
            this.languagesListBx.TabIndex = 17;
            this.languagesListBx.UseCompatibleStateImageBehavior = false;
            this.languagesListBx.View = System.Windows.Forms.View.List;
            this.languagesListBx.SelectedIndexChanged += new System.EventHandler(this.languagesListBx_SelectedIndexChanged);
            // 
            // toolStrip6
            // 
            this.toolStrip6.BackColor = System.Drawing.Color.White;
            this.toolStrip6.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip6.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.langueNameCb,
            this.removeBt,
            this.AddBt,
            this.LoadtBt,
            this.SavetBt});
            this.toolStrip6.Location = new System.Drawing.Point(3, 16);
            this.toolStrip6.Name = "toolStrip6";
            this.toolStrip6.Size = new System.Drawing.Size(733, 31);
            this.toolStrip6.TabIndex = 16;
            // 
            // langueNameCb
            // 
            this.langueNameCb.BackColor = System.Drawing.SystemColors.Info;
            this.langueNameCb.Items.AddRange(new object[] {
            "Arabic",
            "Bulgarian",
            "Catalan",
            "Chinese (Simplified)",
            "Chinese (Traditional)",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Estonian",
            "Finnish",
            "French",
            "German",
            "Greek",
            "Haitian Creole",
            "Hebrew",
            "Hindi",
            "Hungarian",
            "Indonesian",
            "Italian",
            "Japanese",
            "Korean",
            "Latvian",
            "Lithuanian",
            "Norwegian",
            "Polish",
            "Portuguese",
            "Romanian",
            "Russian",
            "Slovak",
            "Slovenian",
            "Spanish",
            "Swedish",
            "Thai",
            "Turkish",
            "Ukrainian",
            "Vietnamese"});
            this.langueNameCb.Name = "langueNameCb";
            this.langueNameCb.Size = new System.Drawing.Size(200, 31);
            this.langueNameCb.Text = "Select new ...";
            // 
            // removeBt
            // 
            this.removeBt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.removeBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeBt.Name = "removeBt";
            this.removeBt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.removeBt.Size = new System.Drawing.Size(28, 28);
            this.removeBt.Text = "Remove the  selected language of the list below";
            this.removeBt.Click += new System.EventHandler(this.removeBt_Click);
            // 
            // AddBt
            // 
            this.AddBt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.AddBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddBt.Image = global::Krea.Properties.Resources.addItem;
            this.AddBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddBt.Name = "AddBt";
            this.AddBt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AddBt.Size = new System.Drawing.Size(28, 28);
            this.AddBt.Text = "Add the language selected in the combo box into the project.";
            this.AddBt.Click += new System.EventHandler(this.AddBt_Click);
            // 
            // LoadtBt
            // 
            this.LoadtBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LoadtBt.Image = global::Krea.Properties.Resources.uploadIcon;
            this.LoadtBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadtBt.Name = "LoadtBt";
            this.LoadtBt.Size = new System.Drawing.Size(28, 28);
            this.LoadtBt.Text = "Import a Comma-separated values (CSV) file";
            this.LoadtBt.Click += new System.EventHandler(this.LoadtBt_Click);
            // 
            // SavetBt
            // 
            this.SavetBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SavetBt.Image = global::Krea.Properties.Resources.downloadFileIcon;
            this.SavetBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SavetBt.Name = "SavetBt";
            this.SavetBt.Size = new System.Drawing.Size(28, 28);
            this.SavetBt.Text = "Export selected language to a Comma-separated values (CSV) file.";
            this.SavetBt.Click += new System.EventHandler(this.SavetBt_Click);
            // 
            // defaultLangueCb
            // 
            this.defaultLangueCb.BackColor = System.Drawing.SystemColors.Info;
            this.defaultLangueCb.Dock = System.Windows.Forms.DockStyle.Top;
            this.defaultLangueCb.FormattingEnabled = true;
            this.defaultLangueCb.Location = new System.Drawing.Point(0, 13);
            this.defaultLangueCb.Name = "defaultLangueCb";
            this.defaultLangueCb.Size = new System.Drawing.Size(739, 21);
            this.defaultLangueCb.TabIndex = 34;
            this.defaultLangueCb.Tag = "false";
            this.defaultLangueCb.SelectedIndexChanged += new System.EventHandler(this.defaultLangueCb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Default project language :";
            // 
            // tranlationGridView
            // 
            this.tranlationGridView.AllowUserToAddRows = false;
            this.tranlationGridView.AllowUserToDeleteRows = false;
            this.tranlationGridView.AllowUserToOrderColumns = true;
            this.tranlationGridView.BackgroundColor = System.Drawing.Color.White;
            this.tranlationGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tranlationGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyColumn,
            this.translationColumn});
            this.tranlationGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tranlationGridView.Location = new System.Drawing.Point(0, 31);
            this.tranlationGridView.MultiSelect = false;
            this.tranlationGridView.Name = "tranlationGridView";
            this.tranlationGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tranlationGridView.Size = new System.Drawing.Size(739, 250);
            this.tranlationGridView.TabIndex = 30;
            this.tranlationGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.tranlationGridView_CellEndEdit);
            // 
            // keyColumn
            // 
            this.keyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.keyColumn.HeaderText = "Key Name";
            this.keyColumn.Name = "keyColumn";
            // 
            // translationColumn
            // 
            this.translationColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.translationColumn.HeaderText = "Translation";
            this.translationColumn.Name = "translationColumn";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.translateFromLanguageCmbBx,
            this.toolStripLabel3,
            this.translateToLanguageCmbBx,
            this.translateNowBt,
            this.toolStripSeparator3,
            this.translationStatusProgressBar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 281);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(739, 31);
            this.toolStrip1.TabIndex = 29;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(87, 28);
            this.toolStripLabel2.Text = "Translate from:";
            // 
            // translateFromLanguageCmbBx
            // 
            this.translateFromLanguageCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.translateFromLanguageCmbBx.Name = "translateFromLanguageCmbBx";
            this.translateFromLanguageCmbBx.Size = new System.Drawing.Size(121, 31);
            this.translateFromLanguageCmbBx.SelectedIndexChanged += new System.EventHandler(this.translateFromLanguageCmbBx_SelectedIndexChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(21, 28);
            this.toolStripLabel3.Text = "to ";
            // 
            // translateToLanguageCmbBx
            // 
            this.translateToLanguageCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.translateToLanguageCmbBx.Items.AddRange(new object[] {
            "All",
            "Arabic",
            "Bulgarian",
            "Catalan",
            "Chinese (Simplified)",
            "Chinese (Traditional)",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Estonian",
            "Finnish",
            "French",
            "German",
            "Greek",
            "Haitian Creole",
            "Hebrew",
            "Hindi",
            "Hungarian",
            "Indonesian",
            "Italian",
            "Japanese",
            "Korean",
            "Latvian",
            "Lithuanian",
            "Norwegian",
            "Polish",
            "Portuguese",
            "Romanian",
            "Russian",
            "Slovak",
            "Slovenian",
            "Spanish",
            "Swedish",
            "Thai",
            "Turkish",
            "Ukrainian",
            "Vietnamese"});
            this.translateToLanguageCmbBx.Name = "translateToLanguageCmbBx";
            this.translateToLanguageCmbBx.Size = new System.Drawing.Size(121, 31);
            this.translateToLanguageCmbBx.SelectedIndexChanged += new System.EventHandler(this.translateToLanguageCmbBx_SelectedIndexChanged);
            // 
            // translateNowBt
            // 
            this.translateNowBt.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.translateNowBt.Image = global::Krea.Properties.Resources.settingsIcon;
            this.translateNowBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.translateNowBt.Name = "translateNowBt";
            this.translateNowBt.Size = new System.Drawing.Size(112, 28);
            this.translateNowBt.Text = "Translate now!";
            this.translateNowBt.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // translationStatusProgressBar
            // 
            this.translationStatusProgressBar.Name = "translationStatusProgressBar";
            this.translationStatusProgressBar.Size = new System.Drawing.Size(100, 28);
            // 
            // appConfigToolStrip
            // 
            this.appConfigToolStrip.BackColor = System.Drawing.Color.White;
            this.appConfigToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.appConfigToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.appConfigToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.newKeyTxtBx,
            this.toolStripSeparator2,
            this.newValueTxtBx,
            this.removeFieldBt,
            this.toolStripSeparator1,
            this.addFieldBt,
            this.toolStripSeparator4,
            this.toolStripLabel4});
            this.appConfigToolStrip.Location = new System.Drawing.Point(0, 0);
            this.appConfigToolStrip.Name = "appConfigToolStrip";
            this.appConfigToolStrip.Size = new System.Drawing.Size(739, 31);
            this.appConfigToolStrip.TabIndex = 27;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 28);
            this.toolStripLabel1.Text = "New:";
            // 
            // newKeyTxtBx
            // 
            this.newKeyTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.newKeyTxtBx.Name = "newKeyTxtBx";
            this.newKeyTxtBx.Size = new System.Drawing.Size(100, 31);
            this.newKeyTxtBx.Text = "keyName";
            this.newKeyTxtBx.ToolTipText = "Type the name of your new field";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // newValueTxtBx
            // 
            this.newValueTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.newValueTxtBx.Name = "newValueTxtBx";
            this.newValueTxtBx.Size = new System.Drawing.Size(200, 31);
            this.newValueTxtBx.Text = "Value";
            this.newValueTxtBx.ToolTipText = "Type the value of your field here";
            // 
            // removeFieldBt
            // 
            this.removeFieldBt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.removeFieldBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeFieldBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeFieldBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeFieldBt.Name = "removeFieldBt";
            this.removeFieldBt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.removeFieldBt.Size = new System.Drawing.Size(28, 28);
            this.removeFieldBt.Text = "Remove selected translation row.";
            this.removeFieldBt.Click += new System.EventHandler(this.removeFieldBt_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // addFieldBt
            // 
            this.addFieldBt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.addFieldBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addFieldBt.Image = global::Krea.Properties.Resources.addItem;
            this.addFieldBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addFieldBt.Name = "addFieldBt";
            this.addFieldBt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.addFieldBt.Size = new System.Drawing.Size(28, 28);
            this.addFieldBt.Text = "Add a new translation row.";
            this.addFieldBt.ToolTipText = "Add a new translation row.";
            this.addFieldBt.Click += new System.EventHandler(this.addFieldBt_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(189, 28);
            this.toolStripLabel4.Text = "Note: Add \"!NL!\" tag for a new line";
            // 
            // translationBackWorker
            // 
            this.translationBackWorker.WorkerReportsProgress = true;
            this.translationBackWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.translationBackWorker_DoWork);
            this.translationBackWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.translationBackWorker_ProgressChanged);
            this.translationBackWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.translationBackWorker_RunWorkerCompleted);
            // 
            // LanguageManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Name = "LanguageManager";
            this.Size = new System.Drawing.Size(739, 513);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tranlationGridView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.appConfigToolStrip.ResumeLayout(false);
            this.appConfigToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip appConfigToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox newKeyTxtBx;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox newValueTxtBx;
        private System.Windows.Forms.ToolStripButton removeFieldBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton addFieldBt;
        private System.Windows.Forms.ComboBox defaultLangueCb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip6;
        private System.Windows.Forms.ToolStripComboBox langueNameCb;
        private System.Windows.Forms.ToolStripButton removeBt;
        private System.Windows.Forms.ToolStripButton AddBt;
        private System.Windows.Forms.ListView languagesListBx;
        private System.Windows.Forms.DataGridView tranlationGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn translationColumn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox translateFromLanguageCmbBx;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox translateToLanguageCmbBx;
        private System.Windows.Forms.ToolStripButton translateNowBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripProgressBar translationStatusProgressBar;
        private System.ComponentModel.BackgroundWorker translationBackWorker;
        private System.Windows.Forms.ToolStripButton LoadtBt;
        private System.Windows.Forms.ToolStripButton SavetBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
    }
}
