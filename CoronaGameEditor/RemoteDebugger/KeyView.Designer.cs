namespace Krea.RemoteDebugger
{
    partial class KeyView
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.volumeUpKeyBt = new System.Windows.Forms.Button();
            this.menuKeyBt = new System.Windows.Forms.Button();
            this.searchKeyBt = new System.Windows.Forms.Button();
            this.backKeyBt = new System.Windows.Forms.Button();
            this.volumeDownKeyBt = new System.Windows.Forms.Button();
            this.leftPadKeyBt = new System.Windows.Forms.Button();
            this.upPadKeyBt = new System.Windows.Forms.Button();
            this.rightPadKeyBt = new System.Windows.Forms.Button();
            this.downPadKeyBt = new System.Windows.Forms.Button();
            this.centerPadKeyBt = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.volumeDownKeyBt);
            this.groupBox1.Controls.Add(this.volumeUpKeyBt);
            this.groupBox1.Controls.Add(this.menuKeyBt);
            this.groupBox1.Controls.Add(this.searchKeyBt);
            this.groupBox1.Controls.Add(this.backKeyBt);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 190);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Navigation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.centerPadKeyBt);
            this.groupBox2.Controls.Add(this.downPadKeyBt);
            this.groupBox2.Controls.Add(this.rightPadKeyBt);
            this.groupBox2.Controls.Add(this.upPadKeyBt);
            this.groupBox2.Controls.Add(this.leftPadKeyBt);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 190);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(421, 293);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "D-Pad/Trackball";
            // 
            // volumeUpKeyBt
            // 
            this.volumeUpKeyBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.volumeUpKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volumeUpKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.volumeUpKeyBt.Image = global::Krea.Properties.Resources.volumeUp;
            this.volumeUpKeyBt.Location = new System.Drawing.Point(290, 21);
            this.volumeUpKeyBt.Name = "volumeUpKeyBt";
            this.volumeUpKeyBt.Size = new System.Drawing.Size(72, 73);
            this.volumeUpKeyBt.TabIndex = 3;
            this.volumeUpKeyBt.Text = "Volume +";
            this.volumeUpKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.volumeUpKeyBt.UseVisualStyleBackColor = true;
            this.volumeUpKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.volumeUpKeyBt_MouseDown);
            this.volumeUpKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.volumeUpKeyBt_MouseUp);
            // 
            // menuKeyBt
            // 
            this.menuKeyBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.menuKeyBt.Image = global::Krea.Properties.Resources.menuIcon;
            this.menuKeyBt.Location = new System.Drawing.Point(178, 60);
            this.menuKeyBt.Name = "menuKeyBt";
            this.menuKeyBt.Size = new System.Drawing.Size(72, 73);
            this.menuKeyBt.TabIndex = 2;
            this.menuKeyBt.Text = "Menu";
            this.menuKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.menuKeyBt.UseVisualStyleBackColor = true;
            this.menuKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuKeyBt_MouseDown);
            this.menuKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.menuKeyBt_MouseUp);
            // 
            // searchKeyBt
            // 
            this.searchKeyBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.searchKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.searchKeyBt.Image = global::Krea.Properties.Resources.search;
            this.searchKeyBt.Location = new System.Drawing.Point(99, 60);
            this.searchKeyBt.Name = "searchKeyBt";
            this.searchKeyBt.Size = new System.Drawing.Size(73, 73);
            this.searchKeyBt.TabIndex = 1;
            this.searchKeyBt.Text = "Search";
            this.searchKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.searchKeyBt.UseVisualStyleBackColor = true;
            this.searchKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.searchKeyBt_MouseDown);
            this.searchKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.searchKeyBt_MouseUp);
            // 
            // backKeyBt
            // 
            this.backKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.backKeyBt.Image = global::Krea.Properties.Resources.flecheGaucheIcon;
            this.backKeyBt.Location = new System.Drawing.Point(22, 60);
            this.backKeyBt.Name = "backKeyBt";
            this.backKeyBt.Size = new System.Drawing.Size(71, 73);
            this.backKeyBt.TabIndex = 0;
            this.backKeyBt.Text = "Back";
            this.backKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.backKeyBt.UseVisualStyleBackColor = true;
            this.backKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backKeyBt_MouseDown);
            this.backKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backKeyBt_MouseUp);
            // 
            // volumeDownKeyBt
            // 
            this.volumeDownKeyBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.volumeDownKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volumeDownKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.volumeDownKeyBt.Image = global::Krea.Properties.Resources.volumeDown;
            this.volumeDownKeyBt.Location = new System.Drawing.Point(290, 99);
            this.volumeDownKeyBt.Name = "volumeDownKeyBt";
            this.volumeDownKeyBt.Size = new System.Drawing.Size(72, 73);
            this.volumeDownKeyBt.TabIndex = 4;
            this.volumeDownKeyBt.Text = "Volume -";
            this.volumeDownKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.volumeDownKeyBt.UseVisualStyleBackColor = true;
            this.volumeDownKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.volumeDownKeyBt_MouseDown);
            this.volumeDownKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.volumeDownKeyBt_MouseUp);
            // 
            // leftPadKeyBt
            // 
            this.leftPadKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftPadKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.leftPadKeyBt.Image = global::Krea.Properties.Resources.flecheGaucheIcon;
            this.leftPadKeyBt.Location = new System.Drawing.Point(102, 106);
            this.leftPadKeyBt.Name = "leftPadKeyBt";
            this.leftPadKeyBt.Size = new System.Drawing.Size(70, 73);
            this.leftPadKeyBt.TabIndex = 5;
            this.leftPadKeyBt.Text = "Left";
            this.leftPadKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.leftPadKeyBt.UseVisualStyleBackColor = true;
            this.leftPadKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.leftPadKeyBt_MouseDown);
            this.leftPadKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.leftPadKeyBt_MouseUp);
            // 
            // upPadKeyBt
            // 
            this.upPadKeyBt.BackColor = System.Drawing.Color.Transparent;
            this.upPadKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upPadKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.upPadKeyBt.Image = global::Krea.Properties.Resources.flecheHautIcon;
            this.upPadKeyBt.Location = new System.Drawing.Point(178, 27);
            this.upPadKeyBt.Name = "upPadKeyBt";
            this.upPadKeyBt.Size = new System.Drawing.Size(71, 73);
            this.upPadKeyBt.TabIndex = 6;
            this.upPadKeyBt.Text = "Up";
            this.upPadKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.upPadKeyBt.UseVisualStyleBackColor = false;
            this.upPadKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.upPadKeyBt_MouseDown);
            this.upPadKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.upPadKeyBt_MouseUp);
            // 
            // rightPadKeyBt
            // 
            this.rightPadKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightPadKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rightPadKeyBt.Image = global::Krea.Properties.Resources.flecheDroiteIcon;
            this.rightPadKeyBt.Location = new System.Drawing.Point(255, 106);
            this.rightPadKeyBt.Name = "rightPadKeyBt";
            this.rightPadKeyBt.Size = new System.Drawing.Size(71, 73);
            this.rightPadKeyBt.TabIndex = 7;
            this.rightPadKeyBt.Text = "Right";
            this.rightPadKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rightPadKeyBt.UseVisualStyleBackColor = true;
            this.rightPadKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rightPadKeyBt_MouseDown);
            this.rightPadKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rightPadKeyBt_MouseUp);
            // 
            // downPadKeyBt
            // 
            this.downPadKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downPadKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.downPadKeyBt.Image = global::Krea.Properties.Resources.flecheBasIcon;
            this.downPadKeyBt.Location = new System.Drawing.Point(178, 185);
            this.downPadKeyBt.Name = "downPadKeyBt";
            this.downPadKeyBt.Size = new System.Drawing.Size(71, 73);
            this.downPadKeyBt.TabIndex = 8;
            this.downPadKeyBt.Text = "Left";
            this.downPadKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.downPadKeyBt.UseVisualStyleBackColor = true;
            this.downPadKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.downPadKeyBt_MouseDown);
            this.downPadKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.downPadKeyBt_MouseUp);
            // 
            // centerPadKeyBt
            // 
            this.centerPadKeyBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.centerPadKeyBt.ForeColor = System.Drawing.SystemColors.ControlText;
            this.centerPadKeyBt.Image = global::Krea.Properties.Resources.circleIcon;
            this.centerPadKeyBt.Location = new System.Drawing.Point(178, 106);
            this.centerPadKeyBt.Name = "centerPadKeyBt";
            this.centerPadKeyBt.Size = new System.Drawing.Size(71, 73);
            this.centerPadKeyBt.TabIndex = 9;
            this.centerPadKeyBt.Text = "Center";
            this.centerPadKeyBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.centerPadKeyBt.UseVisualStyleBackColor = true;
            this.centerPadKeyBt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.centerPadKeyBt_MouseDown);
            this.centerPadKeyBt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.centerPadKeyBt_MouseUp);
            // 
            // KeyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "KeyView";
            this.Size = new System.Drawing.Size(421, 483);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyView_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyView_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button searchKeyBt;
        private System.Windows.Forms.Button backKeyBt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button menuKeyBt;
        private System.Windows.Forms.Button volumeUpKeyBt;
        private System.Windows.Forms.Button volumeDownKeyBt;
        private System.Windows.Forms.Button centerPadKeyBt;
        private System.Windows.Forms.Button downPadKeyBt;
        private System.Windows.Forms.Button rightPadKeyBt;
        private System.Windows.Forms.Button upPadKeyBt;
        private System.Windows.Forms.Button leftPadKeyBt;

    }
}
