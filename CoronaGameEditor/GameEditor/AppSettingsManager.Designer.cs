namespace Krea.GameEditor
{
    partial class AppSettingsManager
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
            this.label6 = new System.Windows.Forms.Label();
            this.removeResolutionBt = new System.Windows.Forms.Button();
            this.addResolutionBt = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.heightNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.widthNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.deviceNameTxtBx = new System.Windows.Forms.TextBox();
            this.customResolutionListBx = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.coronaPathTxtBx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.autoFindCoronaSDKBt = new System.Windows.Forms.Button();
            this.browseCoronaSDKFolder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumUpDw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumUpDw)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(172, 26);
            this.label6.TabIndex = 21;
            this.label6.Text = "Resolution values must be \r\ndefined in Portrait orientation";
            // 
            // removeResolutionBt
            // 
            this.removeResolutionBt.BackgroundImage = global::Krea.Properties.Resources.removeIcon;
            this.removeResolutionBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.removeResolutionBt.FlatAppearance.BorderSize = 0;
            this.removeResolutionBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeResolutionBt.ForeColor = System.Drawing.Color.Transparent;
            this.removeResolutionBt.Location = new System.Drawing.Point(337, 10);
            this.removeResolutionBt.Name = "removeResolutionBt";
            this.removeResolutionBt.Size = new System.Drawing.Size(25, 25);
            this.removeResolutionBt.TabIndex = 20;
            this.removeResolutionBt.UseVisualStyleBackColor = true;
            this.removeResolutionBt.Click += new System.EventHandler(this.removeResolutionBt_Click);
            // 
            // addResolutionBt
            // 
            this.addResolutionBt.BackgroundImage = global::Krea.Properties.Resources.addItem;
            this.addResolutionBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.addResolutionBt.FlatAppearance.BorderSize = 0;
            this.addResolutionBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addResolutionBt.ForeColor = System.Drawing.Color.Transparent;
            this.addResolutionBt.Location = new System.Drawing.Point(253, 8);
            this.addResolutionBt.Name = "addResolutionBt";
            this.addResolutionBt.Size = new System.Drawing.Size(25, 25);
            this.addResolutionBt.TabIndex = 19;
            this.addResolutionBt.UseVisualStyleBackColor = true;
            this.addResolutionBt.Click += new System.EventHandler(this.addResolutionBt_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Screen height:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Screen width:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Device Name:";
            // 
            // heightNumUpDw
            // 
            this.heightNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.heightNumUpDw.Location = new System.Drawing.Point(105, 72);
            this.heightNumUpDw.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.heightNumUpDw.Name = "heightNumUpDw";
            this.heightNumUpDw.Size = new System.Drawing.Size(74, 20);
            this.heightNumUpDw.TabIndex = 3;
            this.heightNumUpDw.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            // 
            // widthNumUpDw
            // 
            this.widthNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.widthNumUpDw.Location = new System.Drawing.Point(105, 46);
            this.widthNumUpDw.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.widthNumUpDw.Minimum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.widthNumUpDw.Name = "widthNumUpDw";
            this.widthNumUpDw.Size = new System.Drawing.Size(74, 20);
            this.widthNumUpDw.TabIndex = 2;
            this.widthNumUpDw.Value = new decimal(new int[] {
            320,
            0,
            0,
            0});
            // 
            // deviceNameTxtBx
            // 
            this.deviceNameTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.deviceNameTxtBx.Location = new System.Drawing.Point(105, 10);
            this.deviceNameTxtBx.Name = "deviceNameTxtBx";
            this.deviceNameTxtBx.Size = new System.Drawing.Size(142, 20);
            this.deviceNameTxtBx.TabIndex = 1;
            // 
            // customResolutionListBx
            // 
            this.customResolutionListBx.FormattingEnabled = true;
            this.customResolutionListBx.Location = new System.Drawing.Point(191, 46);
            this.customResolutionListBx.Name = "customResolutionListBx";
            this.customResolutionListBx.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.customResolutionListBx.Size = new System.Drawing.Size(171, 173);
            this.customResolutionListBx.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(387, 270);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.removeResolutionBt);
            this.tabPage2.Controls.Add(this.customResolutionListBx);
            this.tabPage2.Controls.Add(this.addResolutionBt);
            this.tabPage2.Controls.Add(this.deviceNameTxtBx);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.widthNumUpDw);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.heightNumUpDw);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(379, 244);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Custom Resolutions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.browseCoronaSDKFolder);
            this.tabPage1.Controls.Add(this.autoFindCoronaSDKBt);
            this.tabPage1.Controls.Add(this.coronaPathTxtBx);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(379, 244);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Corona";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // coronaPathTxtBx
            // 
            this.coronaPathTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.coronaPathTxtBx.Location = new System.Drawing.Point(9, 29);
            this.coronaPathTxtBx.Name = "coronaPathTxtBx";
            this.coronaPathTxtBx.Size = new System.Drawing.Size(364, 20);
            this.coronaPathTxtBx.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Corona SDK folder:";
            // 
            // autoFindCoronaSDKBt
            // 
            this.autoFindCoronaSDKBt.Location = new System.Drawing.Point(9, 55);
            this.autoFindCoronaSDKBt.Name = "autoFindCoronaSDKBt";
            this.autoFindCoronaSDKBt.Size = new System.Drawing.Size(75, 23);
            this.autoFindCoronaSDKBt.TabIndex = 9;
            this.autoFindCoronaSDKBt.Text = "Try locate it!";
            this.autoFindCoronaSDKBt.UseVisualStyleBackColor = true;
            this.autoFindCoronaSDKBt.Click += new System.EventHandler(this.autoFindCoronaSDKBt_Click);
            // 
            // browseCoronaSDKFolder
            // 
            this.browseCoronaSDKFolder.Location = new System.Drawing.Point(298, 55);
            this.browseCoronaSDKFolder.Name = "browseCoronaSDKFolder";
            this.browseCoronaSDKFolder.Size = new System.Drawing.Size(75, 23);
            this.browseCoronaSDKFolder.TabIndex = 10;
            this.browseCoronaSDKFolder.Text = "Browse...";
            this.browseCoronaSDKFolder.UseVisualStyleBackColor = true;
            this.browseCoronaSDKFolder.Click += new System.EventHandler(this.browseCoronaSDKFolder_Click);
            // 
            // AppSettingsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabControl1);
            this.Name = "AppSettingsManager";
            this.Size = new System.Drawing.Size(387, 270);
            ((System.ComponentModel.ISupportInitialize)(this.heightNumUpDw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumUpDw)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown heightNumUpDw;
        private System.Windows.Forms.NumericUpDown widthNumUpDw;
        private System.Windows.Forms.TextBox deviceNameTxtBx;
        private System.Windows.Forms.ListBox customResolutionListBx;
        private System.Windows.Forms.Button removeResolutionBt;
        private System.Windows.Forms.Button addResolutionBt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button browseCoronaSDKFolder;
        private System.Windows.Forms.Button autoFindCoronaSDKBt;
        private System.Windows.Forms.TextBox coronaPathTxtBx;
        private System.Windows.Forms.Label label1;
    }
}
