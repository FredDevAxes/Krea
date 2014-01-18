namespace Krea.GameEditor.Panel_Jointures_Properties
{
    partial class TouchPropertiesPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.objTb = new System.Windows.Forms.TextBox();
            this.anchorPointTb = new System.Windows.Forms.TextBox();
            this.validateBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "obj :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Anchor Point :";
            // 
            // objTb
            // 
            this.objTb.Enabled = false;
            this.objTb.Location = new System.Drawing.Point(100, 2);
            this.objTb.Name = "objTb";
            this.objTb.ReadOnly = true;
            this.objTb.Size = new System.Drawing.Size(116, 22);
            this.objTb.TabIndex = 2;
            // 
            // anchorPointTb
            // 
            this.anchorPointTb.Enabled = false;
            this.anchorPointTb.Location = new System.Drawing.Point(100, 30);
            this.anchorPointTb.Name = "anchorPointTb";
            this.anchorPointTb.ReadOnly = true;
            this.anchorPointTb.Size = new System.Drawing.Size(116, 22);
            this.anchorPointTb.TabIndex = 3;
            // 
            // validateBt
            // 
            this.validateBt.BackColor = System.Drawing.Color.Transparent;
            this.validateBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.validateBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.validateBt.FlatAppearance.BorderSize = 0;
            this.validateBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.validateBt.Location = new System.Drawing.Point(236, 0);
            this.validateBt.Name = "validateBt";
            this.validateBt.Size = new System.Drawing.Size(25, 25);
            this.validateBt.TabIndex = 4;
            this.validateBt.UseVisualStyleBackColor = false;
            this.validateBt.Click += new System.EventHandler(this.validateBt_Click);
            // 
            // TouchPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.validateBt);
            this.Controls.Add(this.anchorPointTb);
            this.Controls.Add(this.objTb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tempus Sans ITC", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TouchPropertiesPanel";
            this.Size = new System.Drawing.Size(275, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox objTb;
        private System.Windows.Forms.TextBox anchorPointTb;
        private System.Windows.Forms.Button validateBt;
    }
}
