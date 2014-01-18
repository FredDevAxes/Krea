namespace Krea.GameEditor.Panel_Widgets
{
    partial class PanelPickerWheel
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
            this.validerBt = new System.Windows.Forms.Button();
            this.widgetNameTxtBx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gpRows = new System.Windows.Forms.GroupBox();
            this.rowValueTxtBx = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.saveRow = new System.Windows.Forms.Button();
            this.rowsListBx = new System.Windows.Forms.ListBox();
            this.downRow = new System.Windows.Forms.Button();
            this.addRow = new System.Windows.Forms.Button();
            this.upRow = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.saveColumn = new System.Windows.Forms.Button();
            this.downColumn = new System.Windows.Forms.Button();
            this.upColumn = new System.Windows.Forms.Button();
            this.removeBt = new System.Windows.Forms.Button();
            this.addColumn = new System.Windows.Forms.Button();
            this.columnsListBx = new System.Windows.Forms.ListBox();
            this.alignementCmbBx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.isAutoSizeChkBx = new System.Windows.Forms.CheckBox();
            this.startIndexUpDw = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.columWidthUpDw = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.columnNameTxtBx = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gpRows.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startIndexUpDw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columWidthUpDw)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.validerBt);
            this.groupBox1.Controls.Add(this.widgetNameTxtBx);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 62);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Properties";
            // 
            // validerBt
            // 
            this.validerBt.BackColor = System.Drawing.Color.Transparent;
            this.validerBt.BackgroundImage = global::Krea.Properties.Resources.tickIcon;
            this.validerBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.validerBt.FlatAppearance.BorderSize = 0;
            this.validerBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.validerBt.ForeColor = System.Drawing.Color.Transparent;
            this.validerBt.Location = new System.Drawing.Point(222, 24);
            this.validerBt.Name = "validerBt";
            this.validerBt.Size = new System.Drawing.Size(25, 25);
            this.validerBt.TabIndex = 27;
            this.validerBt.UseVisualStyleBackColor = false;
            this.validerBt.Click += new System.EventHandler(this.validerBt_Click);
            // 
            // widgetNameTxtBx
            // 
            this.widgetNameTxtBx.Location = new System.Drawing.Point(116, 27);
            this.widgetNameTxtBx.Name = "widgetNameTxtBx";
            this.widgetNameTxtBx.ReadOnly = true;
            this.widgetNameTxtBx.Size = new System.Drawing.Size(100, 20);
            this.widgetNameTxtBx.TabIndex = 1;
            this.widgetNameTxtBx.Tag = "false";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PickerWheel Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.gpRows);
            this.groupBox2.Controls.Add(this.saveColumn);
            this.groupBox2.Controls.Add(this.downColumn);
            this.groupBox2.Controls.Add(this.upColumn);
            this.groupBox2.Controls.Add(this.removeBt);
            this.groupBox2.Controls.Add(this.addColumn);
            this.groupBox2.Controls.Add(this.columnsListBx);
            this.groupBox2.Controls.Add(this.alignementCmbBx);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.isAutoSizeChkBx);
            this.groupBox2.Controls.Add(this.startIndexUpDw);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.columWidthUpDw);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.columnNameTxtBx);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(554, 369);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Columns";
            // 
            // gpRows
            // 
            this.gpRows.Controls.Add(this.rowValueTxtBx);
            this.gpRows.Controls.Add(this.label5);
            this.gpRows.Controls.Add(this.saveRow);
            this.gpRows.Controls.Add(this.rowsListBx);
            this.gpRows.Controls.Add(this.downRow);
            this.gpRows.Controls.Add(this.addRow);
            this.gpRows.Controls.Add(this.upRow);
            this.gpRows.Controls.Add(this.button4);
            this.gpRows.Location = new System.Drawing.Point(11, 172);
            this.gpRows.Name = "gpRows";
            this.gpRows.Size = new System.Drawing.Size(489, 177);
            this.gpRows.TabIndex = 62;
            this.gpRows.TabStop = false;
            this.gpRows.Text = "Rows";
            // 
            // rowValueTxtBx
            // 
            this.rowValueTxtBx.Location = new System.Drawing.Point(10, 47);
            this.rowValueTxtBx.Name = "rowValueTxtBx";
            this.rowValueTxtBx.Size = new System.Drawing.Size(142, 20);
            this.rowValueTxtBx.TabIndex = 64;
            this.rowValueTxtBx.Tag = "false";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 63;
            this.label5.Text = "Row value:";
            // 
            // saveRow
            // 
            this.saveRow.BackColor = System.Drawing.Color.Transparent;
            this.saveRow.BackgroundImage = global::Krea.Properties.Resources.saveIcon;
            this.saveRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveRow.FlatAppearance.BorderSize = 0;
            this.saveRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveRow.ForeColor = System.Drawing.Color.Transparent;
            this.saveRow.Location = new System.Drawing.Point(198, 141);
            this.saveRow.Name = "saveRow";
            this.saveRow.Size = new System.Drawing.Size(25, 25);
            this.saveRow.TabIndex = 68;
            this.saveRow.UseVisualStyleBackColor = false;
            this.saveRow.Click += new System.EventHandler(this.saveRow_Click);
            // 
            // rowsListBx
            // 
            this.rowsListBx.FormattingEnabled = true;
            this.rowsListBx.Location = new System.Drawing.Point(229, 19);
            this.rowsListBx.Name = "rowsListBx";
            this.rowsListBx.Size = new System.Drawing.Size(190, 147);
            this.rowsListBx.TabIndex = 63;
            this.rowsListBx.Tag = "false";
            // 
            // downRow
            // 
            this.downRow.BackColor = System.Drawing.Color.Transparent;
            this.downRow.BackgroundImage = global::Krea.Properties.Resources.flecheBasIcon;
            this.downRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.downRow.FlatAppearance.BorderSize = 0;
            this.downRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downRow.ForeColor = System.Drawing.Color.Transparent;
            this.downRow.Location = new System.Drawing.Point(425, 111);
            this.downRow.Name = "downRow";
            this.downRow.Size = new System.Drawing.Size(25, 25);
            this.downRow.TabIndex = 67;
            this.downRow.UseVisualStyleBackColor = false;
            this.downRow.Click += new System.EventHandler(this.downRow_Click);
            // 
            // addRow
            // 
            this.addRow.BackgroundImage = global::Krea.Properties.Resources.addItem;
            this.addRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.addRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addRow.ForeColor = System.Drawing.Color.Transparent;
            this.addRow.Location = new System.Drawing.Point(164, 44);
            this.addRow.Name = "addRow";
            this.addRow.Size = new System.Drawing.Size(25, 25);
            this.addRow.TabIndex = 64;
            this.addRow.UseVisualStyleBackColor = true;
            this.addRow.Click += new System.EventHandler(this.addRow_Click);
            // 
            // upRow
            // 
            this.upRow.BackColor = System.Drawing.Color.Transparent;
            this.upRow.BackgroundImage = global::Krea.Properties.Resources.flecheHautIcon;
            this.upRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.upRow.FlatAppearance.BorderSize = 0;
            this.upRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upRow.ForeColor = System.Drawing.Color.Transparent;
            this.upRow.Location = new System.Drawing.Point(425, 66);
            this.upRow.Name = "upRow";
            this.upRow.Size = new System.Drawing.Size(25, 25);
            this.upRow.TabIndex = 66;
            this.upRow.UseVisualStyleBackColor = false;
            this.upRow.Click += new System.EventHandler(this.upRow_Click);
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::Krea.Properties.Resources.removeIcon;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.Transparent;
            this.button4.Location = new System.Drawing.Point(425, 19);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(25, 25);
            this.button4.TabIndex = 65;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // saveColumn
            // 
            this.saveColumn.BackColor = System.Drawing.Color.Transparent;
            this.saveColumn.BackgroundImage = global::Krea.Properties.Resources.saveIcon;
            this.saveColumn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveColumn.FlatAppearance.BorderSize = 0;
            this.saveColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveColumn.ForeColor = System.Drawing.Color.Transparent;
            this.saveColumn.Location = new System.Drawing.Point(279, 141);
            this.saveColumn.Name = "saveColumn";
            this.saveColumn.Size = new System.Drawing.Size(25, 25);
            this.saveColumn.TabIndex = 61;
            this.saveColumn.UseVisualStyleBackColor = false;
            this.saveColumn.Click += new System.EventHandler(this.saveColumn_Click);
            // 
            // downColumn
            // 
            this.downColumn.BackColor = System.Drawing.Color.Transparent;
            this.downColumn.BackgroundImage = global::Krea.Properties.Resources.flecheBasIcon;
            this.downColumn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.downColumn.FlatAppearance.BorderSize = 0;
            this.downColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downColumn.ForeColor = System.Drawing.Color.Transparent;
            this.downColumn.Location = new System.Drawing.Point(506, 111);
            this.downColumn.Name = "downColumn";
            this.downColumn.Size = new System.Drawing.Size(25, 25);
            this.downColumn.TabIndex = 60;
            this.downColumn.UseVisualStyleBackColor = false;
            this.downColumn.Click += new System.EventHandler(this.downColumn_Click);
            // 
            // upColumn
            // 
            this.upColumn.BackColor = System.Drawing.Color.Transparent;
            this.upColumn.BackgroundImage = global::Krea.Properties.Resources.flecheHautIcon;
            this.upColumn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.upColumn.FlatAppearance.BorderSize = 0;
            this.upColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upColumn.ForeColor = System.Drawing.Color.Transparent;
            this.upColumn.Location = new System.Drawing.Point(506, 66);
            this.upColumn.Name = "upColumn";
            this.upColumn.Size = new System.Drawing.Size(25, 25);
            this.upColumn.TabIndex = 59;
            this.upColumn.UseVisualStyleBackColor = false;
            this.upColumn.Click += new System.EventHandler(this.upColumn_Click);
            // 
            // removeBt
            // 
            this.removeBt.BackgroundImage = global::Krea.Properties.Resources.removeIcon;
            this.removeBt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.removeBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeBt.ForeColor = System.Drawing.Color.Transparent;
            this.removeBt.Location = new System.Drawing.Point(506, 19);
            this.removeBt.Name = "removeBt";
            this.removeBt.Size = new System.Drawing.Size(25, 25);
            this.removeBt.TabIndex = 58;
            this.removeBt.UseVisualStyleBackColor = true;
            this.removeBt.Click += new System.EventHandler(this.removeBt_Click);
            // 
            // addColumn
            // 
            this.addColumn.BackgroundImage = global::Krea.Properties.Resources.addItem;
            this.addColumn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.addColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addColumn.ForeColor = System.Drawing.Color.Transparent;
            this.addColumn.Location = new System.Drawing.Point(175, 41);
            this.addColumn.Name = "addColumn";
            this.addColumn.Size = new System.Drawing.Size(25, 25);
            this.addColumn.TabIndex = 57;
            this.addColumn.UseVisualStyleBackColor = true;
            this.addColumn.Click += new System.EventHandler(this.addColumn_Click);
            // 
            // columnsListBx
            // 
            this.columnsListBx.FormattingEnabled = true;
            this.columnsListBx.Location = new System.Drawing.Point(310, 19);
            this.columnsListBx.Name = "columnsListBx";
            this.columnsListBx.Size = new System.Drawing.Size(190, 147);
            this.columnsListBx.TabIndex = 56;
            this.columnsListBx.Tag = "false";
            this.columnsListBx.SelectedIndexChanged += new System.EventHandler(this.columnsListBx_SelectedIndexChanged);
            // 
            // alignementCmbBx
            // 
            this.alignementCmbBx.FormattingEnabled = true;
            this.alignementCmbBx.Items.AddRange(new object[] {
            "center",
            "right",
            "left"});
            this.alignementCmbBx.Location = new System.Drawing.Point(79, 129);
            this.alignementCmbBx.Name = "alignementCmbBx";
            this.alignementCmbBx.Size = new System.Drawing.Size(121, 21);
            this.alignementCmbBx.TabIndex = 55;
            this.alignementCmbBx.Tag = "false";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Alignement:";
            // 
            // isAutoSizeChkBx
            // 
            this.isAutoSizeChkBx.AutoSize = true;
            this.isAutoSizeChkBx.Checked = true;
            this.isAutoSizeChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isAutoSizeChkBx.Location = new System.Drawing.Point(51, 70);
            this.isAutoSizeChkBx.Name = "isAutoSizeChkBx";
            this.isAutoSizeChkBx.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.isAutoSizeChkBx.Size = new System.Drawing.Size(48, 17);
            this.isAutoSizeChkBx.TabIndex = 53;
            this.isAutoSizeChkBx.Text = "Auto";
            this.isAutoSizeChkBx.UseVisualStyleBackColor = true;
            // 
            // startIndexUpDw
            // 
            this.startIndexUpDw.Location = new System.Drawing.Point(74, 96);
            this.startIndexUpDw.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.startIndexUpDw.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.startIndexUpDw.Name = "startIndexUpDw";
            this.startIndexUpDw.Size = new System.Drawing.Size(47, 20);
            this.startIndexUpDw.TabIndex = 52;
            this.startIndexUpDw.Tag = "false";
            this.startIndexUpDw.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Start Index:";
            // 
            // columWidthUpDw
            // 
            this.columWidthUpDw.Enabled = false;
            this.columWidthUpDw.Location = new System.Drawing.Point(116, 69);
            this.columWidthUpDw.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.columWidthUpDw.Name = "columWidthUpDw";
            this.columWidthUpDw.Size = new System.Drawing.Size(47, 20);
            this.columWidthUpDw.TabIndex = 50;
            this.columWidthUpDw.Tag = "false";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 49;
            this.label10.Text = "Width:";
            // 
            // columnNameTxtBx
            // 
            this.columnNameTxtBx.Location = new System.Drawing.Point(51, 44);
            this.columnNameTxtBx.Name = "columnNameTxtBx";
            this.columnNameTxtBx.Size = new System.Drawing.Size(100, 20);
            this.columnNameTxtBx.TabIndex = 3;
            this.columnNameTxtBx.Tag = "false";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // PanelPickerWheel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PanelPickerWheel";
            this.Size = new System.Drawing.Size(554, 434);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gpRows.ResumeLayout(false);
            this.gpRows.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startIndexUpDw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columWidthUpDw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button validerBt;
        private System.Windows.Forms.TextBox widgetNameTxtBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox columnNameTxtBx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown columWidthUpDw;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown startIndexUpDw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox alignementCmbBx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox isAutoSizeChkBx;
        private System.Windows.Forms.GroupBox gpRows;
        private System.Windows.Forms.TextBox rowValueTxtBx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button saveRow;
        private System.Windows.Forms.ListBox rowsListBx;
        private System.Windows.Forms.Button downRow;
        private System.Windows.Forms.Button addRow;
        private System.Windows.Forms.Button upRow;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button saveColumn;
        private System.Windows.Forms.Button downColumn;
        private System.Windows.Forms.Button upColumn;
        private System.Windows.Forms.Button removeBt;
        private System.Windows.Forms.Button addColumn;
        private System.Windows.Forms.ListBox columnsListBx;
    }
}
