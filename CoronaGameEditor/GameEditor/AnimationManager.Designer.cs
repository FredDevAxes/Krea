namespace Krea.GameEditor
{
    partial class AnimationManager
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationManager));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.framesSetListView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.sequencePropGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.addSequenceBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.sequencesCmbBx = new System.Windows.Forms.ToolStripComboBox();
            this.removeCurrentSequenceBt = new System.Windows.Forms.ToolStripButton();
            this.animPictBx = new System.Windows.Forms.PictureBox();
            this.tabIconsPrincipal = new System.Windows.Forms.ToolStrip();
            this.playAnimBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.stopAnimBt = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animPictBx)).BeginInit();
            this.tabIconsPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.framesSetListView);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(637, 429);
            this.splitContainer1.SplitterDistance = 131;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 0;
            // 
            // framesSetListView
            // 
            this.framesSetListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.framesSetListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.framesSetListView.FullRowSelect = true;
            this.framesSetListView.HideSelection = false;
            this.framesSetListView.Location = new System.Drawing.Point(0, 13);
            this.framesSetListView.Name = "framesSetListView";
            this.framesSetListView.Size = new System.Drawing.Size(637, 118);
            this.framesSetListView.TabIndex = 32;
            this.framesSetListView.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sprite Set Frames:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.sequencePropGrid);
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.animPictBx);
            this.splitContainer2.Panel2.Controls.Add(this.tabIconsPrincipal);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(637, 290);
            this.splitContainer2.SplitterDistance = 297;
            this.splitContainer2.SplitterWidth = 8;
            this.splitContainer2.TabIndex = 0;
            // 
            // sequencePropGrid
            // 
            this.sequencePropGrid.BackColor = System.Drawing.Color.LightBlue;
            this.sequencePropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequencePropGrid.HelpBackColor = System.Drawing.Color.LightBlue;
            this.sequencePropGrid.Location = new System.Drawing.Point(0, 31);
            this.sequencePropGrid.Margin = new System.Windows.Forms.Padding(0);
            this.sequencePropGrid.Name = "sequencePropGrid";
            this.sequencePropGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.sequencePropGrid.Size = new System.Drawing.Size(297, 259);
            this.sequencePropGrid.TabIndex = 32;
            this.sequencePropGrid.Tag = "false";
            this.sequencePropGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.sequencePropGrid_PropertyValueChanged);
            // 
            // toolStrip4
            // 
            this.toolStrip4.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip4.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSequenceBt,
            this.toolStripLabel3,
            this.sequencesCmbBx,
            this.removeCurrentSequenceBt});
            this.toolStrip4.Location = new System.Drawing.Point(0, 0);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(297, 31);
            this.toolStrip4.TabIndex = 31;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // addSequenceBt
            // 
            this.addSequenceBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addSequenceBt.Image = global::Krea.Properties.Resources.addItem;
            this.addSequenceBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addSequenceBt.Name = "addSequenceBt";
            this.addSequenceBt.Size = new System.Drawing.Size(28, 28);
            this.addSequenceBt.Text = "Add Sequence";
            this.addSequenceBt.ToolTipText = "Add a new sequence with frames selected in the list above.";
            this.addSequenceBt.Click += new System.EventHandler(this.addSequenceBt_Click);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(66, 28);
            this.toolStripLabel3.Text = "Sequences:";
            // 
            // sequencesCmbBx
            // 
            this.sequencesCmbBx.BackColor = System.Drawing.SystemColors.Info;
            this.sequencesCmbBx.DropDownWidth = 200;
            this.sequencesCmbBx.Name = "sequencesCmbBx";
            this.sequencesCmbBx.Size = new System.Drawing.Size(121, 31);
            this.sequencesCmbBx.SelectedIndexChanged += new System.EventHandler(this.sequencesCmbBx_SelectedIndexChanged);
            // 
            // removeCurrentSequenceBt
            // 
            this.removeCurrentSequenceBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeCurrentSequenceBt.Image = ((System.Drawing.Image)(resources.GetObject("removeCurrentSequenceBt.Image")));
            this.removeCurrentSequenceBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeCurrentSequenceBt.Name = "removeCurrentSequenceBt";
            this.removeCurrentSequenceBt.Size = new System.Drawing.Size(28, 28);
            this.removeCurrentSequenceBt.Text = "Remove Sequence";
            this.removeCurrentSequenceBt.Click += new System.EventHandler(this.removeCurrentSequenceBt_Click);
            // 
            // animPictBx
            // 
            this.animPictBx.BackColor = System.Drawing.Color.Transparent;
            this.animPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animPictBx.Location = new System.Drawing.Point(0, 18);
            this.animPictBx.Name = "animPictBx";
            this.animPictBx.Size = new System.Drawing.Size(332, 237);
            this.animPictBx.TabIndex = 38;
            this.animPictBx.TabStop = false;
            this.animPictBx.Paint += new System.Windows.Forms.PaintEventHandler(this.animPictBx_Paint);
            // 
            // tabIconsPrincipal
            // 
            this.tabIconsPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.tabIconsPrincipal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabIconsPrincipal.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tabIconsPrincipal.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tabIconsPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playAnimBt,
            this.toolStripSeparator4,
            this.stopAnimBt});
            this.tabIconsPrincipal.Location = new System.Drawing.Point(0, 255);
            this.tabIconsPrincipal.Name = "tabIconsPrincipal";
            this.tabIconsPrincipal.Padding = new System.Windows.Forms.Padding(0);
            this.tabIconsPrincipal.Size = new System.Drawing.Size(332, 35);
            this.tabIconsPrincipal.Stretch = true;
            this.tabIconsPrincipal.TabIndex = 37;
            // 
            // playAnimBt
            // 
            this.playAnimBt.AutoSize = false;
            this.playAnimBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.playAnimBt.Image = global::Krea.Properties.Resources.playItem;
            this.playAnimBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playAnimBt.Name = "playAnimBt";
            this.playAnimBt.Size = new System.Drawing.Size(32, 32);
            this.playAnimBt.Text = "Text";
            this.playAnimBt.Click += new System.EventHandler(this.playAnimBt_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 35);
            // 
            // stopAnimBt
            // 
            this.stopAnimBt.AutoSize = false;
            this.stopAnimBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopAnimBt.Image = global::Krea.Properties.Resources.stopIcon;
            this.stopAnimBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopAnimBt.Name = "stopAnimBt";
            this.stopAnimBt.Size = new System.Drawing.Size(32, 32);
            this.stopAnimBt.Text = "Text";
            this.stopAnimBt.Click += new System.EventHandler(this.stopAnimBt_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 18);
            this.label2.TabIndex = 36;
            this.label2.Text = "Sequence Preview:";
            // 
            // AnimationManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(637, 429);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AnimationManager";
            this.Text = "Animation Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnimationManager_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animPictBx)).EndInit();
            this.tabIconsPrincipal.ResumeLayout(false);
            this.tabIconsPrincipal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView framesSetListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid sequencePropGrid;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox sequencesCmbBx;
        private System.Windows.Forms.ToolStripButton removeCurrentSequenceBt;
        private System.Windows.Forms.PictureBox animPictBx;
        private System.Windows.Forms.ToolStrip tabIconsPrincipal;
        private System.Windows.Forms.ToolStripButton playAnimBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton stopAnimBt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripButton addSequenceBt;
    }
}