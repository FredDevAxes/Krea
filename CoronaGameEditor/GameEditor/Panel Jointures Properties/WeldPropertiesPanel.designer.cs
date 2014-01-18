namespace Krea.GameEditor.Panel_Jointures_Properties
{
    partial class WeldPropertiesPanel
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
            this.anchorPointTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.validateBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // obj2Tb
            // 
            this.obj2Tb.Enabled = false;
            this.obj2Tb.Location = new System.Drawing.Point(61, 30);
            this.obj2Tb.Name = "obj2Tb";
            this.obj2Tb.ReadOnly = true;
            this.obj2Tb.Size = new System.Drawing.Size(116, 22);
            this.obj2Tb.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "Obj 2 :";
            // 
            // obj1Tb
            // 
            this.obj1Tb.Enabled = false;
            this.obj1Tb.Location = new System.Drawing.Point(59, 3);
            this.obj1Tb.Name = "obj1Tb";
            this.obj1Tb.ReadOnly = true;
            this.obj1Tb.Size = new System.Drawing.Size(116, 22);
            this.obj1Tb.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "Obj 1 :";
            // 
            // anchorPointTb
            // 
            this.anchorPointTb.Enabled = false;
            this.anchorPointTb.Location = new System.Drawing.Point(279, 3);
            this.anchorPointTb.Name = "anchorPointTb";
            this.anchorPointTb.ReadOnly = true;
            this.anchorPointTb.Size = new System.Drawing.Size(116, 22);
            this.anchorPointTb.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "Anchor Point :";
            // 
            // validateBt
            // 
            this.validateBt.BackColor = System.Drawing.Color.Transparent;
            this.validateBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.validateBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.validateBt.FlatAppearance.BorderSize = 0;
            this.validateBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.validateBt.Location = new System.Drawing.Point(403, 6);
            this.validateBt.Name = "validateBt";
            this.validateBt.Size = new System.Drawing.Size(25, 25);
            this.validateBt.TabIndex = 22;
            this.validateBt.UseVisualStyleBackColor = false;
            this.validateBt.Click += new System.EventHandler(this.validateBt_Click);
            // 
            // WeldPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.validateBt);
            this.Controls.Add(this.anchorPointTb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.obj2Tb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.obj1Tb);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WeldPropertiesPanel";
            this.Size = new System.Drawing.Size(439, 57);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox obj2Tb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox obj1Tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox anchorPointTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button validateBt;
    }
}
