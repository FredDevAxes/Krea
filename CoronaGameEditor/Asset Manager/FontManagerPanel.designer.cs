namespace Krea.Asset_Manager
{
    partial class FontManagerPanel
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.saveBt = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nameTb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.importBt);
            this.groupBox1.Location = new System.Drawing.Point(6, 13);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.richTextBox1);
            this.groupBox2.Location = new System.Drawing.Point(6, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 202);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Font Preview";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(408, 183);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // saveBt
            // 
            this.saveBt.BackColor = System.Drawing.Color.Transparent;
            this.saveBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.saveBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveBt.FlatAppearance.BorderSize = 0;
            this.saveBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBt.ForeColor = System.Drawing.Color.Transparent;
            this.saveBt.Location = new System.Drawing.Point(434, 3);
            this.saveBt.Name = "saveBt";
            this.saveBt.Size = new System.Drawing.Size(25, 25);
            this.saveBt.TabIndex = 4;
            this.saveBt.Text = " ";
            this.saveBt.UseVisualStyleBackColor = false;
            this.saveBt.Click += new System.EventHandler(this.saveBt_Click);
            // 
            // FontManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.saveBt);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FontManagerPanel";
            this.Size = new System.Drawing.Size(462, 318);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox nameTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button importBt;
        private System.Windows.Forms.Button saveBt;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
