namespace Krea.GameEditor.Debugger
{
    partial class DebugManager
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
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.debugWorker = new System.ComponentModel.BackgroundWorker();
            this.SendTb = new System.Windows.Forms.TextBox();
            this.sendBt = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.filetb = new System.Windows.Forms.TextBox();
            this.filelbl = new System.Windows.Forms.Label();
            this.linetb = new System.Windows.Forms.TextBox();
            this.linelbl = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button15 = new System.Windows.Forms.Button();
            this.dumpBt = new System.Windows.Forms.Button();
            this.dumptb = new System.Windows.Forms.TextBox();
            this.dumplb = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.expressiontb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.frameTb = new System.Windows.Forms.TextBox();
            this.commandtb = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(164, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "over";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(83, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "step";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // debugWorker
            // 
            this.debugWorker.WorkerReportsProgress = true;
            this.debugWorker.WorkerSupportsCancellation = true;
            // 
            // SendTb
            // 
            this.SendTb.Location = new System.Drawing.Point(14, 376);
            this.SendTb.Name = "SendTb";
            this.SendTb.Size = new System.Drawing.Size(453, 20);
            this.SendTb.TabIndex = 5;
            // 
            // sendBt
            // 
            this.sendBt.Location = new System.Drawing.Point(473, 346);
            this.sendBt.Name = "sendBt";
            this.sendBt.Size = new System.Drawing.Size(303, 50);
            this.sendBt.TabIndex = 6;
            this.sendBt.Text = "Send";
            this.sendBt.UseVisualStyleBackColor = true;
            this.sendBt.Click += new System.EventHandler(this.sendBt_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(6, 52);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "locals";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(87, 52);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 10;
            this.button8.Text = "backtrace";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(161, 47);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(127, 23);
            this.button9.TabIndex = 11;
            this.button9.Text = "List Breakpoints";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(137, 49);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(127, 24);
            this.button10.TabIndex = 12;
            this.button10.Text = "List Watch";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(6, 19);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 13;
            this.button11.Text = "run";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(161, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 24);
            this.button1.TabIndex = 14;
            this.button1.Text = "Delete All Breakpoints";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(137, 79);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(127, 21);
            this.button12.TabIndex = 15;
            this.button12.Text = "Delete All Watch";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // filetb
            // 
            this.filetb.Location = new System.Drawing.Point(55, 19);
            this.filetb.Name = "filetb";
            this.filetb.Size = new System.Drawing.Size(100, 20);
            this.filetb.TabIndex = 16;
            // 
            // filelbl
            // 
            this.filelbl.AutoSize = true;
            this.filelbl.Location = new System.Drawing.Point(14, 22);
            this.filelbl.Name = "filelbl";
            this.filelbl.Size = new System.Drawing.Size(35, 13);
            this.filelbl.TabIndex = 17;
            this.filelbl.Text = "fichier";
            // 
            // linetb
            // 
            this.linetb.Location = new System.Drawing.Point(55, 47);
            this.linetb.Name = "linetb";
            this.linetb.Size = new System.Drawing.Size(100, 20);
            this.linetb.TabIndex = 18;
            // 
            // linelbl
            // 
            this.linelbl.AutoSize = true;
            this.linelbl.Location = new System.Drawing.Point(20, 50);
            this.linelbl.Name = "linelbl";
            this.linelbl.Size = new System.Drawing.Size(29, 13);
            this.linelbl.TabIndex = 19;
            this.linelbl.Text = "ligne";
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(137, 19);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(127, 24);
            this.button13.TabIndex = 20;
            this.button13.Text = "Add Watch";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(161, 15);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(127, 26);
            this.button14.TabIndex = 21;
            this.button14.Text = "Add Breakpoint";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.filelbl);
            this.groupBox1.Controls.Add(this.linelbl);
            this.groupBox1.Controls.Add(this.filetb);
            this.groupBox1.Controls.Add(this.linetb);
            this.groupBox1.Controls.Add(this.button14);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Location = new System.Drawing.Point(474, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 106);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Breakpoints";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button11);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(14, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(246, 102);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Control";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button15);
            this.groupBox4.Controls.Add(this.dumpBt);
            this.groupBox4.Controls.Add(this.dumptb);
            this.groupBox4.Controls.Add(this.dumplb);
            this.groupBox4.Location = new System.Drawing.Point(14, 127);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(454, 54);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(233, 19);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 3;
            this.button15.Text = "exec";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // dumpBt
            // 
            this.dumpBt.Location = new System.Drawing.Point(150, 20);
            this.dumpBt.Name = "dumpBt";
            this.dumpBt.Size = new System.Drawing.Size(77, 23);
            this.dumpBt.TabIndex = 2;
            this.dumpBt.Text = "dump";
            this.dumpBt.UseVisualStyleBackColor = true;
            this.dumpBt.Click += new System.EventHandler(this.dumpBt_Click);
            // 
            // dumptb
            // 
            this.dumptb.Location = new System.Drawing.Point(52, 22);
            this.dumptb.Name = "dumptb";
            this.dumptb.Size = new System.Drawing.Size(86, 20);
            this.dumptb.TabIndex = 1;
            // 
            // dumplb
            // 
            this.dumplb.AutoSize = true;
            this.dumplb.Location = new System.Drawing.Point(13, 22);
            this.dumplb.Name = "dumplb";
            this.dumplb.Size = new System.Drawing.Size(34, 13);
            this.dumplb.TabIndex = 0;
            this.dumplb.Text = "target";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button7);
            this.groupBox5.Controls.Add(this.expressiontb);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.button13);
            this.groupBox5.Controls.Add(this.button10);
            this.groupBox5.Controls.Add(this.button12);
            this.groupBox5.Location = new System.Drawing.Point(474, 123);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(302, 217);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Watch";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(137, 106);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 4;
            this.button7.Text = "eval";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // expressiontb
            // 
            this.expressiontb.Location = new System.Drawing.Point(24, 69);
            this.expressiontb.Name = "expressiontb";
            this.expressiontb.Size = new System.Drawing.Size(100, 20);
            this.expressiontb.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "expression";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.button6);
            this.groupBox6.Controls.Add(this.frameTb);
            this.groupBox6.Location = new System.Drawing.Point(268, 11);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 100);
            this.groupBox6.TabIndex = 27;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Frame";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Frame";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(75, 58);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 1;
            this.button6.Text = "frame";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // frameTb
            // 
            this.frameTb.Location = new System.Drawing.Point(50, 32);
            this.frameTb.Name = "frameTb";
            this.frameTb.Size = new System.Drawing.Size(100, 20);
            this.frameTb.TabIndex = 0;
            // 
            // commandtb
            // 
            this.commandtb.Location = new System.Drawing.Point(14, 202);
            this.commandtb.Name = "commandtb";
            this.commandtb.Size = new System.Drawing.Size(453, 168);
            this.commandtb.TabIndex = 28;
            this.commandtb.Text = "";
            // 
            // DebugManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.commandtb);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sendBt);
            this.Controls.Add(this.SendTb);
            this.Name = "DebugManager";
            this.Size = new System.Drawing.Size(786, 408);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.ComponentModel.BackgroundWorker debugWorker;
        private System.Windows.Forms.TextBox SendTb;
        private System.Windows.Forms.Button sendBt;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TextBox filetb;
        private System.Windows.Forms.Label filelbl;
        private System.Windows.Forms.TextBox linetb;
        private System.Windows.Forms.Label linelbl;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button dumpBt;
        private System.Windows.Forms.TextBox dumptb;
        private System.Windows.Forms.Label dumplb;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox expressiontb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox frameTb;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.RichTextBox commandtb;
    }
}
