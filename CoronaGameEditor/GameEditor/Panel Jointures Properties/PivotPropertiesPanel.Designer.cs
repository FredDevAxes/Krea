namespace Krea.GameEditor.Panel_Jointures_Properties
{
    partial class PivotPropertiesPanel
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
            this.nameObj1TxtBx = new System.Windows.Forms.TextBox();
            this.nameObj2TxtBx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.isMotorEnabledChkBx = new System.Windows.Forms.CheckBox();
            this.isLimitedChkBx = new System.Windows.Forms.CheckBox();
            this.motorSpeedNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.maxTorqueNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lowerLimitNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.upperLimitNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.motorGrpBx = new System.Windows.Forms.GroupBox();
            this.saveBt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedNumUpDw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxTorqueNumUpDw)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lowerLimitNumUpDw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperLimitNumUpDw)).BeginInit();
            this.motorGrpBx.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameObj1TxtBx
            // 
            this.nameObj1TxtBx.Location = new System.Drawing.Point(48, 8);
            this.nameObj1TxtBx.Name = "nameObj1TxtBx";
            this.nameObj1TxtBx.ReadOnly = true;
            this.nameObj1TxtBx.Size = new System.Drawing.Size(116, 22);
            this.nameObj1TxtBx.TabIndex = 0;
            // 
            // nameObj2TxtBx
            // 
            this.nameObj2TxtBx.Location = new System.Drawing.Point(48, 48);
            this.nameObj2TxtBx.Name = "nameObj2TxtBx";
            this.nameObj2TxtBx.ReadOnly = true;
            this.nameObj2TxtBx.Size = new System.Drawing.Size(116, 22);
            this.nameObj2TxtBx.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Obj 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Obj 2:";
            // 
            // isMotorEnabledChkBx
            // 
            this.isMotorEnabledChkBx.AutoSize = true;
            this.isMotorEnabledChkBx.Location = new System.Drawing.Point(170, 3);
            this.isMotorEnabledChkBx.Name = "isMotorEnabledChkBx";
            this.isMotorEnabledChkBx.Size = new System.Drawing.Size(109, 19);
            this.isMotorEnabledChkBx.TabIndex = 4;
            this.isMotorEnabledChkBx.Text = "Motor Enabled";
            this.isMotorEnabledChkBx.UseVisualStyleBackColor = true;
            // 
            // isLimitedChkBx
            // 
            this.isLimitedChkBx.AutoSize = true;
            this.isLimitedChkBx.Location = new System.Drawing.Point(170, 29);
            this.isLimitedChkBx.Name = "isLimitedChkBx";
            this.isLimitedChkBx.Size = new System.Drawing.Size(104, 19);
            this.isLimitedChkBx.TabIndex = 5;
            this.isLimitedChkBx.Text = "Limit Enabled";
            this.isLimitedChkBx.UseVisualStyleBackColor = true;
            // 
            // motorSpeedNumUpDw
            // 
            this.motorSpeedNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.motorSpeedNumUpDw.Location = new System.Drawing.Point(131, 15);
            this.motorSpeedNumUpDw.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.motorSpeedNumUpDw.Name = "motorSpeedNumUpDw";
            this.motorSpeedNumUpDw.Size = new System.Drawing.Size(69, 22);
            this.motorSpeedNumUpDw.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Motor Speed:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Max Motor Torque:";
            // 
            // maxTorqueNumUpDw
            // 
            this.maxTorqueNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.maxTorqueNumUpDw.Location = new System.Drawing.Point(131, 45);
            this.maxTorqueNumUpDw.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.maxTorqueNumUpDw.Name = "maxTorqueNumUpDw";
            this.maxTorqueNumUpDw.Size = new System.Drawing.Size(69, 22);
            this.maxTorqueNumUpDw.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lowerLimitNumUpDw);
            this.groupBox1.Controls.Add(this.upperLimitNumUpDw);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(495, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 73);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Limit Properties";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Lower Limit:";
            // 
            // lowerLimitNumUpDw
            // 
            this.lowerLimitNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.lowerLimitNumUpDw.Location = new System.Drawing.Point(100, 45);
            this.lowerLimitNumUpDw.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.lowerLimitNumUpDw.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.lowerLimitNumUpDw.Name = "lowerLimitNumUpDw";
            this.lowerLimitNumUpDw.Size = new System.Drawing.Size(50, 22);
            this.lowerLimitNumUpDw.TabIndex = 13;
            // 
            // upperLimitNumUpDw
            // 
            this.upperLimitNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.upperLimitNumUpDw.Location = new System.Drawing.Point(100, 15);
            this.upperLimitNumUpDw.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.upperLimitNumUpDw.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.upperLimitNumUpDw.Name = "upperLimitNumUpDw";
            this.upperLimitNumUpDw.Size = new System.Drawing.Size(50, 22);
            this.upperLimitNumUpDw.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Upper Limit:";
            // 
            // motorGrpBx
            // 
            this.motorGrpBx.Controls.Add(this.label3);
            this.motorGrpBx.Controls.Add(this.motorSpeedNumUpDw);
            this.motorGrpBx.Controls.Add(this.label4);
            this.motorGrpBx.Controls.Add(this.maxTorqueNumUpDw);
            this.motorGrpBx.Location = new System.Drawing.Point(281, 3);
            this.motorGrpBx.Name = "motorGrpBx";
            this.motorGrpBx.Size = new System.Drawing.Size(208, 73);
            this.motorGrpBx.TabIndex = 11;
            this.motorGrpBx.TabStop = false;
            this.motorGrpBx.Text = "Motor Properties";
            // 
            // saveBt
            // 
            this.saveBt.BackColor = System.Drawing.Color.Transparent;
            this.saveBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.saveBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveBt.FlatAppearance.BorderSize = 0;
            this.saveBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBt.ForeColor = System.Drawing.Color.Transparent;
            this.saveBt.Location = new System.Drawing.Point(659, 12);
            this.saveBt.Name = "saveBt";
            this.saveBt.Size = new System.Drawing.Size(25, 25);
            this.saveBt.TabIndex = 12;
            this.saveBt.UseVisualStyleBackColor = false;
            this.saveBt.Click += new System.EventHandler(this.saveBt_Click);
            // 
            // PivotPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.saveBt);
            this.Controls.Add(this.motorGrpBx);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.isLimitedChkBx);
            this.Controls.Add(this.isMotorEnabledChkBx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameObj2TxtBx);
            this.Controls.Add(this.nameObj1TxtBx);
            this.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PivotPropertiesPanel";
            this.Size = new System.Drawing.Size(704, 83);
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedNumUpDw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxTorqueNumUpDw)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lowerLimitNumUpDw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperLimitNumUpDw)).EndInit();
            this.motorGrpBx.ResumeLayout(false);
            this.motorGrpBx.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameObj1TxtBx;
        private System.Windows.Forms.TextBox nameObj2TxtBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox isMotorEnabledChkBx;
        private System.Windows.Forms.CheckBox isLimitedChkBx;
        private System.Windows.Forms.NumericUpDown motorSpeedNumUpDw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown maxTorqueNumUpDw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown lowerLimitNumUpDw;
        private System.Windows.Forms.NumericUpDown upperLimitNumUpDw;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox motorGrpBx;
        private System.Windows.Forms.Button saveBt;
    }
}
