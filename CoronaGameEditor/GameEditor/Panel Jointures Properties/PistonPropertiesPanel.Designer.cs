namespace Krea.GameEditor.Panel_Jointures_Properties
{
    partial class PistonPropertiesPanel
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
            System.Windows.Forms.Label label7;
            this.saveBt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameObj2TxtBx = new System.Windows.Forms.TextBox();
            this.nameObj1TxtBx = new System.Windows.Forms.TextBox();
            this.motorGrpBx = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.motorSpeedNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.maxMotorForceNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lowerLimitNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.upperLimitNumUpDw = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.isLimitedChkBx = new System.Windows.Forms.CheckBox();
            this.isMotorEnabledChkBx = new System.Windows.Forms.CheckBox();
            this.axisDistanceTxtBx = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            this.motorGrpBx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedNumUpDw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxMotorForceNumUpDw)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lowerLimitNumUpDw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperLimitNumUpDw)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label7.Location = new System.Drawing.Point(72, 59);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(86, 15);
            label7.TabIndex = 24;
            label7.Text = "Axis Distance:";
            // 
            // saveBt
            // 
            this.saveBt.BackColor = System.Drawing.Color.Transparent;
            this.saveBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.saveBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveBt.FlatAppearance.BorderSize = 0;
            this.saveBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBt.ForeColor = System.Drawing.Color.Transparent;
            this.saveBt.Location = new System.Drawing.Point(623, 13);
            this.saveBt.Name = "saveBt";
            this.saveBt.Size = new System.Drawing.Size(25, 25);
            this.saveBt.TabIndex = 14;
            this.saveBt.UseVisualStyleBackColor = false;
            this.saveBt.Click += new System.EventHandler(this.saveBt_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Obj 2:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Obj 1:";
            // 
            // nameObj2TxtBx
            // 
            this.nameObj2TxtBx.Location = new System.Drawing.Point(50, 32);
            this.nameObj2TxtBx.Name = "nameObj2TxtBx";
            this.nameObj2TxtBx.ReadOnly = true;
            this.nameObj2TxtBx.Size = new System.Drawing.Size(100, 20);
            this.nameObj2TxtBx.TabIndex = 16;
            // 
            // nameObj1TxtBx
            // 
            this.nameObj1TxtBx.Location = new System.Drawing.Point(50, 6);
            this.nameObj1TxtBx.Name = "nameObj1TxtBx";
            this.nameObj1TxtBx.ReadOnly = true;
            this.nameObj1TxtBx.Size = new System.Drawing.Size(100, 20);
            this.nameObj1TxtBx.TabIndex = 15;
            // 
            // motorGrpBx
            // 
            this.motorGrpBx.Controls.Add(this.label3);
            this.motorGrpBx.Controls.Add(this.motorSpeedNumUpDw);
            this.motorGrpBx.Controls.Add(this.label4);
            this.motorGrpBx.Controls.Add(this.maxMotorForceNumUpDw);
            this.motorGrpBx.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motorGrpBx.Location = new System.Drawing.Point(266, 6);
            this.motorGrpBx.Name = "motorGrpBx";
            this.motorGrpBx.Size = new System.Drawing.Size(195, 68);
            this.motorGrpBx.TabIndex = 22;
            this.motorGrpBx.TabStop = false;
            this.motorGrpBx.Text = "Motor Properties";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Motor Speed:";
            // 
            // motorSpeedNumUpDw
            // 
            this.motorSpeedNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.motorSpeedNumUpDw.Location = new System.Drawing.Point(123, 13);
            this.motorSpeedNumUpDw.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.motorSpeedNumUpDw.Name = "motorSpeedNumUpDw";
            this.motorSpeedNumUpDw.Size = new System.Drawing.Size(59, 22);
            this.motorSpeedNumUpDw.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Max Motor Force:";
            // 
            // maxMotorForceNumUpDw
            // 
            this.maxMotorForceNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.maxMotorForceNumUpDw.Location = new System.Drawing.Point(123, 39);
            this.maxMotorForceNumUpDw.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.maxMotorForceNumUpDw.Name = "maxMotorForceNumUpDw";
            this.maxMotorForceNumUpDw.Size = new System.Drawing.Size(59, 22);
            this.maxMotorForceNumUpDw.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lowerLimitNumUpDw);
            this.groupBox1.Controls.Add(this.upperLimitNumUpDw);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(467, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 68);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Limit Properties";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Lower Limit:";
            // 
            // lowerLimitNumUpDw
            // 
            this.lowerLimitNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.lowerLimitNumUpDw.Location = new System.Drawing.Point(84, 39);
            this.lowerLimitNumUpDw.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.lowerLimitNumUpDw.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.lowerLimitNumUpDw.Name = "lowerLimitNumUpDw";
            this.lowerLimitNumUpDw.Size = new System.Drawing.Size(59, 22);
            this.lowerLimitNumUpDw.TabIndex = 13;
            // 
            // upperLimitNumUpDw
            // 
            this.upperLimitNumUpDw.BackColor = System.Drawing.SystemColors.Info;
            this.upperLimitNumUpDw.Location = new System.Drawing.Point(84, 13);
            this.upperLimitNumUpDw.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.upperLimitNumUpDw.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.upperLimitNumUpDw.Name = "upperLimitNumUpDw";
            this.upperLimitNumUpDw.Size = new System.Drawing.Size(59, 22);
            this.upperLimitNumUpDw.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Upper Limit:";
            // 
            // isLimitedChkBx
            // 
            this.isLimitedChkBx.AutoSize = true;
            this.isLimitedChkBx.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isLimitedChkBx.Location = new System.Drawing.Point(160, 26);
            this.isLimitedChkBx.Name = "isLimitedChkBx";
            this.isLimitedChkBx.Size = new System.Drawing.Size(104, 19);
            this.isLimitedChkBx.TabIndex = 20;
            this.isLimitedChkBx.Text = "Limit Enabled";
            this.isLimitedChkBx.UseVisualStyleBackColor = true;
            // 
            // isMotorEnabledChkBx
            // 
            this.isMotorEnabledChkBx.AutoSize = true;
            this.isMotorEnabledChkBx.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isMotorEnabledChkBx.Location = new System.Drawing.Point(160, 3);
            this.isMotorEnabledChkBx.Name = "isMotorEnabledChkBx";
            this.isMotorEnabledChkBx.Size = new System.Drawing.Size(109, 19);
            this.isMotorEnabledChkBx.TabIndex = 19;
            this.isMotorEnabledChkBx.Text = "Motor Enabled";
            this.isMotorEnabledChkBx.UseVisualStyleBackColor = true;
            // 
            // axisDistanceTxtBx
            // 
            this.axisDistanceTxtBx.Location = new System.Drawing.Point(160, 54);
            this.axisDistanceTxtBx.Name = "axisDistanceTxtBx";
            this.axisDistanceTxtBx.ReadOnly = true;
            this.axisDistanceTxtBx.Size = new System.Drawing.Size(100, 20);
            this.axisDistanceTxtBx.TabIndex = 23;
            // 
            // PistonPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(label7);
            this.Controls.Add(this.axisDistanceTxtBx);
            this.Controls.Add(this.motorGrpBx);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.isLimitedChkBx);
            this.Controls.Add(this.isMotorEnabledChkBx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameObj2TxtBx);
            this.Controls.Add(this.nameObj1TxtBx);
            this.Controls.Add(this.saveBt);
            this.Name = "PistonPropertiesPanel";
            this.Size = new System.Drawing.Size(667, 80);
            this.motorGrpBx.ResumeLayout(false);
            this.motorGrpBx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedNumUpDw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxMotorForceNumUpDw)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lowerLimitNumUpDw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperLimitNumUpDw)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveBt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameObj2TxtBx;
        private System.Windows.Forms.TextBox nameObj1TxtBx;
        private System.Windows.Forms.GroupBox motorGrpBx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown motorSpeedNumUpDw;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown maxMotorForceNumUpDw;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown lowerLimitNumUpDw;
        private System.Windows.Forms.NumericUpDown upperLimitNumUpDw;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox isLimitedChkBx;
        private System.Windows.Forms.CheckBox isMotorEnabledChkBx;
        private System.Windows.Forms.TextBox axisDistanceTxtBx;
    }
}
