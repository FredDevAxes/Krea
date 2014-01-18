namespace Krea.Asset_Manager
{
    partial class AudioManagerPanel
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.importBt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.preloadCB = new System.Windows.Forms.CheckBox();
            this.repeatNUD = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.stopBt = new System.Windows.Forms.Button();
            this.PlayBt = new System.Windows.Forms.Button();
            this.CompteurVolumeTb = new System.Windows.Forms.TextBox();
            this.VolumeLb = new System.Windows.Forms.Label();
            this.VolumeTb = new System.Windows.Forms.TrackBar();
            this.typeStreamRb = new System.Windows.Forms.RadioButton();
            this.typeSoundRb = new System.Windows.Forms.RadioButton();
            this.saveBt = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repeatNUD)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTb)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nameTb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.importBt);
            this.groupBox1.Location = new System.Drawing.Point(25, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 75);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Information";
            // 
            // nameTb
            // 
            this.nameTb.BackColor = System.Drawing.SystemColors.Info;
            this.nameTb.Enabled = false;
            this.nameTb.Location = new System.Drawing.Point(69, 29);
            this.nameTb.Name = "nameTb";
            this.nameTb.Size = new System.Drawing.Size(284, 20);
            this.nameTb.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name : ";
            // 
            // importBt
            // 
            this.importBt.BackColor = System.Drawing.Color.Transparent;
            this.importBt.BackgroundImage = global::Krea.Properties.Resources.uploadIcon;
            this.importBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.importBt.FlatAppearance.BorderSize = 0;
            this.importBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.importBt.ForeColor = System.Drawing.Color.Transparent;
            this.importBt.Location = new System.Drawing.Point(359, 19);
            this.importBt.Name = "importBt";
            this.importBt.Size = new System.Drawing.Size(37, 39);
            this.importBt.TabIndex = 2;
            this.importBt.UseVisualStyleBackColor = false;
            this.importBt.Click += new System.EventHandler(this.importBt_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Volume :";
            // 
            // preloadCB
            // 
            this.preloadCB.AutoSize = true;
            this.preloadCB.Location = new System.Drawing.Point(256, 25);
            this.preloadCB.Name = "preloadCB";
            this.preloadCB.Size = new System.Drawing.Size(135, 17);
            this.preloadCB.TabIndex = 7;
            this.preloadCB.Text = "Play at application start";
            this.preloadCB.UseVisualStyleBackColor = true;
            // 
            // repeatNUD
            // 
            this.repeatNUD.BackColor = System.Drawing.SystemColors.Info;
            this.repeatNUD.Location = new System.Drawing.Point(93, 76);
            this.repeatNUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.repeatNUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.repeatNUD.Name = "repeatNUD";
            this.repeatNUD.Size = new System.Drawing.Size(136, 20);
            this.repeatNUD.TabIndex = 8;
            this.repeatNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.repeatNUD.ValueChanged += new System.EventHandler(this.repeatNUD_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Repeat :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.stopBt);
            this.groupBox2.Controls.Add(this.PlayBt);
            this.groupBox2.Controls.Add(this.CompteurVolumeTb);
            this.groupBox2.Controls.Add(this.VolumeLb);
            this.groupBox2.Controls.Add(this.VolumeTb);
            this.groupBox2.Controls.Add(this.typeStreamRb);
            this.groupBox2.Controls.Add(this.typeSoundRb);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.repeatNUD);
            this.groupBox2.Controls.Add(this.preloadCB);
            this.groupBox2.Location = new System.Drawing.Point(25, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 190);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Audio Configuration";
            // 
            // stopBt
            // 
            this.stopBt.BackColor = System.Drawing.Color.Transparent;
            this.stopBt.BackgroundImage = global::Krea.Properties.Resources.stopIcon;
            this.stopBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.stopBt.FlatAppearance.BorderSize = 0;
            this.stopBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopBt.ForeColor = System.Drawing.Color.Transparent;
            this.stopBt.Location = new System.Drawing.Point(121, 102);
            this.stopBt.Name = "stopBt";
            this.stopBt.Size = new System.Drawing.Size(25, 25);
            this.stopBt.TabIndex = 6;
            this.stopBt.UseVisualStyleBackColor = false;
            this.stopBt.Click += new System.EventHandler(this.stopBt_Click);
            // 
            // PlayBt
            // 
            this.PlayBt.BackColor = System.Drawing.Color.Transparent;
            this.PlayBt.BackgroundImage = global::Krea.Properties.Resources.playItem;
            this.PlayBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PlayBt.FlatAppearance.BorderSize = 0;
            this.PlayBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayBt.ForeColor = System.Drawing.Color.Transparent;
            this.PlayBt.Location = new System.Drawing.Point(90, 102);
            this.PlayBt.Name = "PlayBt";
            this.PlayBt.Size = new System.Drawing.Size(25, 25);
            this.PlayBt.TabIndex = 5;
            this.PlayBt.UseVisualStyleBackColor = false;
            this.PlayBt.Click += new System.EventHandler(this.PlayBt_Click);
            // 
            // CompteurVolumeTb
            // 
            this.CompteurVolumeTb.BackColor = System.Drawing.SystemColors.Info;
            this.CompteurVolumeTb.Location = new System.Drawing.Point(199, 28);
            this.CompteurVolumeTb.Name = "CompteurVolumeTb";
            this.CompteurVolumeTb.Size = new System.Drawing.Size(30, 20);
            this.CompteurVolumeTb.TabIndex = 13;
            this.CompteurVolumeTb.Text = "0.5";
            // 
            // VolumeLb
            // 
            this.VolumeLb.AutoSize = true;
            this.VolumeLb.Location = new System.Drawing.Point(196, 35);
            this.VolumeLb.Name = "VolumeLb";
            this.VolumeLb.Size = new System.Drawing.Size(0, 13);
            this.VolumeLb.TabIndex = 12;
            // 
            // VolumeTb
            // 
            this.VolumeTb.BackColor = System.Drawing.Color.White;
            this.VolumeTb.Location = new System.Drawing.Point(93, 25);
            this.VolumeTb.Maximum = 100;
            this.VolumeTb.Name = "VolumeTb";
            this.VolumeTb.Size = new System.Drawing.Size(104, 45);
            this.VolumeTb.TabIndex = 5;
            this.VolumeTb.TickFrequency = 10;
            this.VolumeTb.Value = 50;
            this.VolumeTb.Scroll += new System.EventHandler(this.VolumeTb_Scroll);
            // 
            // typeStreamRb
            // 
            this.typeStreamRb.AutoSize = true;
            this.typeStreamRb.Location = new System.Drawing.Point(256, 94);
            this.typeStreamRb.Name = "typeStreamRb";
            this.typeStreamRb.Size = new System.Drawing.Size(99, 17);
            this.typeStreamRb.TabIndex = 11;
            this.typeStreamRb.Text = "Load as Stream";
            this.typeStreamRb.UseVisualStyleBackColor = true;
            // 
            // typeSoundRb
            // 
            this.typeSoundRb.AutoSize = true;
            this.typeSoundRb.Checked = true;
            this.typeSoundRb.Location = new System.Drawing.Point(256, 71);
            this.typeSoundRb.Name = "typeSoundRb";
            this.typeSoundRb.Size = new System.Drawing.Size(97, 17);
            this.typeSoundRb.TabIndex = 10;
            this.typeSoundRb.TabStop = true;
            this.typeSoundRb.Text = "Load as Sound";
            this.typeSoundRb.UseVisualStyleBackColor = true;
            // 
            // saveBt
            // 
            this.saveBt.BackColor = System.Drawing.Color.Transparent;
            this.saveBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.saveBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveBt.FlatAppearance.BorderSize = 0;
            this.saveBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBt.ForeColor = System.Drawing.Color.Transparent;
            this.saveBt.Location = new System.Drawing.Point(414, 3);
            this.saveBt.Name = "saveBt";
            this.saveBt.Size = new System.Drawing.Size(25, 25);
            this.saveBt.TabIndex = 4;
            this.saveBt.UseVisualStyleBackColor = false;
            this.saveBt.Click += new System.EventHandler(this.saveBt_Click);
            // 
            // AudioManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.saveBt);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AudioManagerPanel";
            this.Size = new System.Drawing.Size(560, 332);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repeatNUD)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox preloadCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown repeatNUD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button importBt;
        private System.Windows.Forms.Button saveBt;
        private System.Windows.Forms.RadioButton typeStreamRb;
        private System.Windows.Forms.RadioButton typeSoundRb;
        private System.Windows.Forms.Button PlayBt;
        private System.Windows.Forms.Button stopBt;
        private System.Windows.Forms.Label VolumeLb;
        private System.Windows.Forms.TrackBar VolumeTb;
        private System.Windows.Forms.TextBox CompteurVolumeTb;
    }
}
