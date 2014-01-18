namespace Krea
{
    partial class LanguageManagerV2
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
            this.listLangueLb = new System.Windows.Forms.ListBox();
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.langueNameCb = new System.Windows.Forms.ToolStripComboBox();
            this.AddBt = new System.Windows.Forms.ToolStripButton();
            this.removeBt = new System.Windows.Forms.ToolStripButton();
            this.defaultLangueCb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TranslationLb = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btAddTranslation = new System.Windows.Forms.ToolStripButton();
            this.btRemoveTranslation = new System.Windows.Forms.ToolStripButton();
            this.TranslationTb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.KeyTb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.translateBt = new System.Windows.Forms.Button();
            this.GTranslateLanguageCb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.GSelectedLanguageCb = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.defaultLangueCb);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(405, 359);
            this.splitContainer1.SplitterDistance = 174;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listLangueLb);
            this.groupBox1.Controls.Add(this.toolStrip6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 325);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project Languages:";
            // 
            // listLangueLb
            // 
            this.listLangueLb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLangueLb.FormattingEnabled = true;
            this.listLangueLb.Location = new System.Drawing.Point(3, 47);
            this.listLangueLb.Name = "listLangueLb";
            this.listLangueLb.Size = new System.Drawing.Size(168, 275);
            this.listLangueLb.TabIndex = 24;
            this.listLangueLb.Tag = "false";
            this.listLangueLb.SelectedIndexChanged += new System.EventHandler(this.listLangueLb_SelectedIndexChanged);
            // 
            // toolStrip6
            // 
            this.toolStrip6.BackColor = System.Drawing.Color.White;
            this.toolStrip6.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip6.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.langueNameCb,
            this.AddBt,
            this.removeBt});
            this.toolStrip6.Location = new System.Drawing.Point(3, 16);
            this.toolStrip6.Name = "toolStrip6";
            this.toolStrip6.Size = new System.Drawing.Size(168, 31);
            this.toolStrip6.TabIndex = 16;
            // 
            // langueNameCb
            // 
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
            this.langueNameCb.Size = new System.Drawing.Size(121, 31);
            this.langueNameCb.Text = "Select new ...";
            // 
            // AddBt
            // 
            this.AddBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddBt.Image = global::Krea.Properties.Resources.addItem;
            this.AddBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddBt.Name = "AddBt";
            this.AddBt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AddBt.Size = new System.Drawing.Size(28, 28);
            this.AddBt.Text = "toolStripButton1";
            this.AddBt.Click += new System.EventHandler(this.AddBt_Click);
            // 
            // removeBt
            // 
            this.removeBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeBt.Name = "removeBt";
            this.removeBt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.removeBt.Size = new System.Drawing.Size(28, 28);
            this.removeBt.Text = "toolStripButton1";
            this.removeBt.Click += new System.EventHandler(this.removeBt_Click);
            // 
            // defaultLangueCb
            // 
            this.defaultLangueCb.Dock = System.Windows.Forms.DockStyle.Top;
            this.defaultLangueCb.FormattingEnabled = true;
            this.defaultLangueCb.Location = new System.Drawing.Point(0, 13);
            this.defaultLangueCb.Name = "defaultLangueCb";
            this.defaultLangueCb.Size = new System.Drawing.Size(174, 21);
            this.defaultLangueCb.TabIndex = 32;
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
            this.label2.TabIndex = 30;
            this.label2.Text = "Default project language :";
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
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(221, 359);
            this.splitContainer2.SplitterDistance = 221;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TranslationLb);
            this.groupBox3.Controls.Add(this.toolStrip1);
            this.groupBox3.Controls.Add(this.TranslationTb);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.KeyTb);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(221, 221);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Translations";
            // 
            // TranslationLb
            // 
            this.TranslationLb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TranslationLb.FormattingEnabled = true;
            this.TranslationLb.HorizontalScrollbar = true;
            this.TranslationLb.Location = new System.Drawing.Point(3, 113);
            this.TranslationLb.Name = "TranslationLb";
            this.TranslationLb.Size = new System.Drawing.Size(215, 105);
            this.TranslationLb.TabIndex = 27;
            this.TranslationLb.Tag = "false";
            this.TranslationLb.SelectedIndexChanged += new System.EventHandler(this.TranslationLb_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btAddTranslation,
            this.btRemoveTranslation});
            this.toolStrip1.Location = new System.Drawing.Point(3, 82);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(215, 31);
            this.toolStrip1.TabIndex = 26;
            // 
            // btAddTranslation
            // 
            this.btAddTranslation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAddTranslation.Image = global::Krea.Properties.Resources.addItem;
            this.btAddTranslation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAddTranslation.Name = "btAddTranslation";
            this.btAddTranslation.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btAddTranslation.Size = new System.Drawing.Size(28, 28);
            this.btAddTranslation.Text = "toolStripButton1";
            this.btAddTranslation.Click += new System.EventHandler(this.btAddTranslation_Click);
            // 
            // btRemoveTranslation
            // 
            this.btRemoveTranslation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRemoveTranslation.Image = global::Krea.Properties.Resources.removeIcon;
            this.btRemoveTranslation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRemoveTranslation.Name = "btRemoveTranslation";
            this.btRemoveTranslation.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btRemoveTranslation.Size = new System.Drawing.Size(28, 28);
            this.btRemoveTranslation.Text = "toolStripButton1";
            this.btRemoveTranslation.Click += new System.EventHandler(this.btRemoveTranslation_Click);
            // 
            // TranslationTb
            // 
            this.TranslationTb.Dock = System.Windows.Forms.DockStyle.Top;
            this.TranslationTb.Location = new System.Drawing.Point(3, 62);
            this.TranslationTb.Name = "TranslationTb";
            this.TranslationTb.Size = new System.Drawing.Size(215, 20);
            this.TranslationTb.TabIndex = 25;
            this.TranslationTb.Tag = "false";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Translation : ";
            // 
            // KeyTb
            // 
            this.KeyTb.Dock = System.Windows.Forms.DockStyle.Top;
            this.KeyTb.Location = new System.Drawing.Point(3, 29);
            this.KeyTb.Name = "KeyTb";
            this.KeyTb.Size = new System.Drawing.Size(215, 20);
            this.KeyTb.TabIndex = 12;
            this.KeyTb.Tag = "false";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Key :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.translateBt);
            this.groupBox2.Controls.Add(this.GTranslateLanguageCb);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.GSelectedLanguageCb);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 128);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Translate using Microsoft Bing";
            // 
            // translateBt
            // 
            this.translateBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.translateBt.Dock = System.Windows.Forms.DockStyle.Top;
            this.translateBt.FlatAppearance.BorderSize = 0;
            this.translateBt.ForeColor = System.Drawing.Color.Black;
            this.translateBt.Location = new System.Drawing.Point(3, 84);
            this.translateBt.Name = "translateBt";
            this.translateBt.Size = new System.Drawing.Size(215, 25);
            this.translateBt.TabIndex = 37;
            this.translateBt.Text = "Translate Now!";
            this.translateBt.UseVisualStyleBackColor = true;
            this.translateBt.Click += new System.EventHandler(this.GTranslateBt_Click);
            // 
            // GTranslateLanguageCb
            // 
            this.GTranslateLanguageCb.Dock = System.Windows.Forms.DockStyle.Top;
            this.GTranslateLanguageCb.FormattingEnabled = true;
            this.GTranslateLanguageCb.Items.AddRange(new object[] {
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
            this.GTranslateLanguageCb.Location = new System.Drawing.Point(3, 63);
            this.GTranslateLanguageCb.Name = "GTranslateLanguageCb";
            this.GTranslateLanguageCb.Size = new System.Drawing.Size(215, 21);
            this.GTranslateLanguageCb.TabIndex = 36;
            this.GTranslateLanguageCb.Tag = "false";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(3, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Translate language in :";
            // 
            // GSelectedLanguageCb
            // 
            this.GSelectedLanguageCb.Dock = System.Windows.Forms.DockStyle.Top;
            this.GSelectedLanguageCb.FormattingEnabled = true;
            this.GSelectedLanguageCb.Location = new System.Drawing.Point(3, 29);
            this.GSelectedLanguageCb.Name = "GSelectedLanguageCb";
            this.GSelectedLanguageCb.Size = new System.Drawing.Size(215, 21);
            this.GSelectedLanguageCb.TabIndex = 32;
            this.GSelectedLanguageCb.Tag = "false";
            this.GSelectedLanguageCb.SelectedIndexChanged += new System.EventHandler(this.GSelectedLanguageCb_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(3, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Select a language";
            // 
            // LanguageManagerV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Name = "LanguageManagerV2";
            this.Size = new System.Drawing.Size(405, 359);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox defaultLangueCb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip6;
        private System.Windows.Forms.ToolStripComboBox langueNameCb;
        private System.Windows.Forms.ToolStripButton AddBt;
        private System.Windows.Forms.ToolStripButton removeBt;
        private System.Windows.Forms.ListBox listLangueLb;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox GSelectedLanguageCb;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox KeyTb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox TranslationLb;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btAddTranslation;
        private System.Windows.Forms.ToolStripButton btRemoveTranslation;
        private System.Windows.Forms.TextBox TranslationTb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button translateBt;
        private System.Windows.Forms.ComboBox GTranslateLanguageCb;
    }
}
