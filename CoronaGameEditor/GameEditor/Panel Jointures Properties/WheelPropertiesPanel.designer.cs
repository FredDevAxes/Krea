namespace Krea.GameEditor.Panel_Jointures_Properties
{
    partial class WheelPropertiesPanel
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
            this.validateBt = new System.Windows.Forms.Button();
            this.obj2Tb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.obj1Tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.axisDistanceTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.isMotorEnableCb = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.motorForceNup = new System.Windows.Forms.NumericUpDown();
            this.motorSpeedNup = new System.Windows.Forms.NumericUpDown();
            this.anchorPointTxtBx = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.motorForceNup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedNup)).BeginInit();
            this.SuspendLayout();
            // 
            // validateBt
            // 
            this.validateBt.BackColor = System.Drawing.Color.Transparent;
            this.validateBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.validateBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.validateBt.FlatAppearance.BorderSize = 0;
            this.validateBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.validateBt.Location = new System.Drawing.Point(575, 10);
            this.validateBt.Name = "validateBt";
            this.validateBt.Size = new System.Drawing.Size(25, 25);
            this.validateBt.TabIndex = 5;
            this.validateBt.UseVisualStyleBackColor = false;
            this.validateBt.Click += new System.EventHandler(this.validateBt_Click);
            // 
            // obj2Tb
            // 
            this.obj2Tb.Enabled = false;
            this.obj2Tb.Location = new System.Drawing.Point(57, 24);
            this.obj2Tb.Name = "obj2Tb";
            this.obj2Tb.ReadOnly = true;
            this.obj2Tb.Size = new System.Drawing.Size(116, 22);
            this.obj2Tb.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Obj 2 :";
            // 
            // obj1Tb
            // 
            this.obj1Tb.Enabled = false;
            this.obj1Tb.Location = new System.Drawing.Point(57, 0);
            this.obj1Tb.Name = "obj1Tb";
            this.obj1Tb.ReadOnly = true;
            this.obj1Tb.Size = new System.Drawing.Size(116, 22);
            this.obj1Tb.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Obj 1 :";
            // 
            // axisDistanceTb
            // 
            this.axisDistanceTb.Enabled = false;
            this.axisDistanceTb.Location = new System.Drawing.Point(276, 51);
            this.axisDistanceTb.Name = "axisDistanceTb";
            this.axisDistanceTb.ReadOnly = true;
            this.axisDistanceTb.Size = new System.Drawing.Size(71, 22);
            this.axisDistanceTb.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Axis distance :";
            // 
            // isMotorEnableCb
            // 
            this.isMotorEnableCb.AutoSize = true;
            this.isMotorEnableCb.Location = new System.Drawing.Point(204, 16);
            this.isMotorEnableCb.Name = "isMotorEnableCb";
            this.isMotorEnableCb.Size = new System.Drawing.Size(121, 19);
            this.isMotorEnableCb.TabIndex = 19;
            this.isMotorEnableCb.Text = "Is motor enabled";
            this.isMotorEnableCb.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 15);
            this.label4.TabIndex = 20;
            this.label4.Text = "Motor Speed :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 15);
            this.label5.TabIndex = 22;
            this.label5.Text = "Max motor torque :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.motorForceNup);
            this.groupBox1.Controls.Add(this.motorSpeedNup);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(353, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 72);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Motor";
            // 
            // motorForceNup
            // 
            this.motorForceNup.BackColor = System.Drawing.SystemColors.Info;
            this.motorForceNup.Location = new System.Drawing.Point(133, 44);
            this.motorForceNup.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.motorForceNup.Name = "motorForceNup";
            this.motorForceNup.Size = new System.Drawing.Size(77, 22);
            this.motorForceNup.TabIndex = 25;
            // 
            // motorSpeedNup
            // 
            this.motorSpeedNup.BackColor = System.Drawing.SystemColors.Info;
            this.motorSpeedNup.Location = new System.Drawing.Point(133, 14);
            this.motorSpeedNup.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.motorSpeedNup.Name = "motorSpeedNup";
            this.motorSpeedNup.Size = new System.Drawing.Size(77, 22);
            this.motorSpeedNup.TabIndex = 24;
            // 
            // anchorPointTxtBx
            // 
            this.anchorPointTxtBx.Enabled = false;
            this.anchorPointTxtBx.Location = new System.Drawing.Point(99, 51);
            this.anchorPointTxtBx.Name = "anchorPointTxtBx";
            this.anchorPointTxtBx.ReadOnly = true;
            this.anchorPointTxtBx.Size = new System.Drawing.Size(71, 22);
            this.anchorPointTxtBx.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 15);
            this.label8.TabIndex = 30;
            this.label8.Text = "Anchor Point:";
            // 
            // WheelPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.anchorPointTxtBx);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.isMotorEnableCb);
            this.Controls.Add(this.axisDistanceTb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.obj2Tb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.obj1Tb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.validateBt);
            this.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WheelPropertiesPanel";
            this.Size = new System.Drawing.Size(600, 79);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.motorForceNup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedNup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button validateBt;
        private System.Windows.Forms.TextBox obj2Tb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox obj1Tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox axisDistanceTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox isMotorEnableCb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown motorSpeedNup;
        private System.Windows.Forms.NumericUpDown motorForceNup;
        private System.Windows.Forms.TextBox anchorPointTxtBx;
        private System.Windows.Forms.Label label8;
    }
}
