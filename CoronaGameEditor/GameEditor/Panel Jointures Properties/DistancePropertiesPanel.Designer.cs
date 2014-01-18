namespace Krea.GameEditor.Panel_Jointures_Properties
{
    partial class DistancePropertiesPanel
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
            this.saveBt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameObj2TxtBx = new System.Windows.Forms.TextBox();
            this.nameObj1TxtBx = new System.Windows.Forms.TextBox();
            this.dampingRatioNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.frequencyNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dampingRatioNumUpDw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumUpDw)).BeginInit();
            this.SuspendLayout();
            // 
            // saveBt
            // 
            this.saveBt.BackColor = System.Drawing.Color.Transparent;
            this.saveBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.saveBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveBt.FlatAppearance.BorderSize = 0;
            this.saveBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBt.ForeColor = System.Drawing.Color.Transparent;
            this.saveBt.Location = new System.Drawing.Point(344, 10);
            this.saveBt.Name = "saveBt";
            this.saveBt.Size = new System.Drawing.Size(25, 25);
            this.saveBt.TabIndex = 13;
            this.saveBt.UseVisualStyleBackColor = false;
            this.saveBt.Click += new System.EventHandler(this.saveBt_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-1, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Obj 2:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "Obj 1:";
            // 
            // nameObj2TxtBx
            // 
            this.nameObj2TxtBx.Location = new System.Drawing.Point(55, 39);
            this.nameObj2TxtBx.Name = "nameObj2TxtBx";
            this.nameObj2TxtBx.ReadOnly = true;
            this.nameObj2TxtBx.Size = new System.Drawing.Size(100, 20);
            this.nameObj2TxtBx.TabIndex = 15;
            // 
            // nameObj1TxtBx
            // 
            this.nameObj1TxtBx.Location = new System.Drawing.Point(55, 13);
            this.nameObj1TxtBx.Name = "nameObj1TxtBx";
            this.nameObj1TxtBx.ReadOnly = true;
            this.nameObj1TxtBx.Size = new System.Drawing.Size(100, 20);
            this.nameObj1TxtBx.TabIndex = 14;
            // 
            // dampingRatioNumUpDw
            // 
            this.dampingRatioNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.dampingRatioNumUpDw.DecimalPlaces = 1;
            this.dampingRatioNumUpDw.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.dampingRatioNumUpDw.Location = new System.Drawing.Point(268, 10);
            this.dampingRatioNumUpDw.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dampingRatioNumUpDw.Name = "dampingRatioNumUpDw";
            this.dampingRatioNumUpDw.Size = new System.Drawing.Size(59, 20);
            this.dampingRatioNumUpDw.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(167, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 22;
            this.label6.Text = "Damping Ratio:";
            // 
            // frequencyNumUpDw
            // 
            this.frequencyNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.frequencyNumUpDw.Location = new System.Drawing.Point(268, 40);
            this.frequencyNumUpDw.Name = "frequencyNumUpDw";
            this.frequencyNumUpDw.Size = new System.Drawing.Size(59, 20);
            this.frequencyNumUpDw.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(177, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 24;
            this.label3.Text = "Frequency:";
            // 
            // DistancePropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.frequencyNumUpDw);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dampingRatioNumUpDw);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameObj2TxtBx);
            this.Controls.Add(this.nameObj1TxtBx);
            this.Controls.Add(this.saveBt);
            this.Name = "DistancePropertiesPanel";
            this.Size = new System.Drawing.Size(389, 73);
            ((System.ComponentModel.ISupportInitialize)(this.dampingRatioNumUpDw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumUpDw)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveBt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameObj2TxtBx;
        private System.Windows.Forms.TextBox nameObj1TxtBx;
        private System.Windows.Forms.NumericUpDown dampingRatioNumUpDw;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown frequencyNumUpDw;
        private System.Windows.Forms.Label label3;
    }
}
