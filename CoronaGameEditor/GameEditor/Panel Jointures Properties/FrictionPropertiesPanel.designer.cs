namespace Krea.GameEditor.Panel_Jointures_Properties
{
    partial class FrictionPropertiesPanel
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
            this.obj2Tb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.obj1Tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.validateBt = new System.Windows.Forms.Button();
            this.maxTorqueNup = new System.Windows.Forms.NumericUpDown();
            this.maxForceNup = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.maxTorqueNup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxForceNup)).BeginInit();
            this.SuspendLayout();
            // 
            // obj2Tb
            // 
            this.obj2Tb.Enabled = false;
            this.obj2Tb.Location = new System.Drawing.Point(46, 37);
            this.obj2Tb.Name = "obj2Tb";
            this.obj2Tb.ReadOnly = true;
            this.obj2Tb.Size = new System.Drawing.Size(100, 20);
            this.obj2Tb.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "Obj 2 :";
            // 
            // obj1Tb
            // 
            this.obj1Tb.Enabled = false;
            this.obj1Tb.Location = new System.Drawing.Point(46, 11);
            this.obj1Tb.Name = "obj1Tb";
            this.obj1Tb.ReadOnly = true;
            this.obj1Tb.Size = new System.Drawing.Size(100, 20);
            this.obj1Tb.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "Obj 1 :";
            // 
            // validateBt
            // 
            this.validateBt.BackColor = System.Drawing.Color.Transparent;
            this.validateBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.validateBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.validateBt.FlatAppearance.BorderSize = 0;
            this.validateBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.validateBt.Location = new System.Drawing.Point(325, 8);
            this.validateBt.Name = "validateBt";
            this.validateBt.Size = new System.Drawing.Size(25, 25);
            this.validateBt.TabIndex = 29;
            this.validateBt.UseVisualStyleBackColor = false;
            this.validateBt.Click += new System.EventHandler(this.validateBt_Click);
            // 
            // maxTorqueNup
            // 
            this.maxTorqueNup.BackColor = System.Drawing.SystemColors.Info;
            this.maxTorqueNup.Location = new System.Drawing.Point(237, 35);
            this.maxTorqueNup.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.maxTorqueNup.Name = "maxTorqueNup";
            this.maxTorqueNup.Size = new System.Drawing.Size(64, 20);
            this.maxTorqueNup.TabIndex = 33;
            // 
            // maxForceNup
            // 
            this.maxForceNup.BackColor = System.Drawing.SystemColors.Info;
            this.maxForceNup.Location = new System.Drawing.Point(237, 9);
            this.maxForceNup.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.maxForceNup.Name = "maxForceNup";
            this.maxForceNup.Size = new System.Drawing.Size(64, 20);
            this.maxForceNup.TabIndex = 32;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(151, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 30;
            this.label4.Text = "Max force :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(152, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 15);
            this.label5.TabIndex = 31;
            this.label5.Text = "Max torque :";
            // 
            // FrictionPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.maxTorqueNup);
            this.Controls.Add(this.maxForceNup);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.validateBt);
            this.Controls.Add(this.obj2Tb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.obj1Tb);
            this.Controls.Add(this.label1);
            this.Name = "FrictionPropertiesPanel";
            this.Size = new System.Drawing.Size(353, 67);
            ((System.ComponentModel.ISupportInitialize)(this.maxTorqueNup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxForceNup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button validateBt;
        private System.Windows.Forms.TextBox obj2Tb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox obj1Tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown maxTorqueNup;
        private System.Windows.Forms.NumericUpDown maxForceNup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
