namespace Krea.RemoteDebugger
{
    partial class RemoteControllerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteControllerForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.appRemoteController1 = new Krea.RemoteDebugger.AppRemoteController();
            this.remoteInfo1 = new Krea.RemoteDebugger.RemoteInfo();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.appRemoteController1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.remoteInfo1);
            this.splitContainer1.Size = new System.Drawing.Size(424, 715);
            this.splitContainer1.SplitterDistance = 497;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // appRemoteController1
            // 
            this.appRemoteController1.BackColor = System.Drawing.Color.Transparent;
            this.appRemoteController1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appRemoteController1.Location = new System.Drawing.Point(0, 0);
            this.appRemoteController1.Name = "appRemoteController1";
            this.appRemoteController1.Size = new System.Drawing.Size(424, 497);
            this.appRemoteController1.TabIndex = 5;
            // 
            // remoteInfo1
            // 
            this.remoteInfo1.BackColor = System.Drawing.Color.Transparent;
            this.remoteInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remoteInfo1.Location = new System.Drawing.Point(0, 0);
            this.remoteInfo1.Name = "remoteInfo1";
            this.remoteInfo1.Size = new System.Drawing.Size(424, 208);
            this.remoteInfo1.TabIndex = 0;
            // 
            // RemoteControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(424, 715);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoteControllerForm";
            this.Text = "Remote Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteControllerForm_FormClosing);
            this.Load += new System.EventHandler(this.RemoteControllerForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private AppRemoteController appRemoteController1;
        private RemoteInfo remoteInfo1;




    }
}