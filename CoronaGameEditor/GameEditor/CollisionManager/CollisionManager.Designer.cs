namespace Krea.GameEditor.CollisionManager
{
    partial class CollisionManager
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.filterGroupNameTxtBx = new System.Windows.Forms.ToolStripTextBox();
            this.addFilterGroupBt = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeFilterGroupBt = new System.Windows.Forms.ToolStripButton();
            this.filterGroupsListBx = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.filterGroupNameTxtBx,
            this.addFilterGroupBt,
            this.toolStripSeparator1,
            this.removeFilterGroupBt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(593, 31);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(42, 28);
            this.toolStripLabel2.Text = "Name:";
            // 
            // filterGroupNameTxtBx
            // 
            this.filterGroupNameTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.filterGroupNameTxtBx.Name = "filterGroupNameTxtBx";
            this.filterGroupNameTxtBx.Size = new System.Drawing.Size(150, 31);
            // 
            // addFilterGroupBt
            // 
            this.addFilterGroupBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addFilterGroupBt.Image = global::Krea.Properties.Resources.addItem;
            this.addFilterGroupBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addFilterGroupBt.Name = "addFilterGroupBt";
            this.addFilterGroupBt.Size = new System.Drawing.Size(28, 28);
            this.addFilterGroupBt.Text = "Add";
            this.addFilterGroupBt.ToolTipText = "Add a new filter group having the name entered in the text box.";
            this.addFilterGroupBt.Click += new System.EventHandler(this.addFilterGroupBt_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // removeFilterGroupBt
            // 
            this.removeFilterGroupBt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeFilterGroupBt.Image = global::Krea.Properties.Resources.removeIcon;
            this.removeFilterGroupBt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeFilterGroupBt.Name = "removeFilterGroupBt";
            this.removeFilterGroupBt.Size = new System.Drawing.Size(28, 28);
            this.removeFilterGroupBt.Text = "Remove selected filter group";
            this.removeFilterGroupBt.ToolTipText = "Remove the selected filter group selected in the list above.";
            this.removeFilterGroupBt.Click += new System.EventHandler(this.removeFilterGroup_Click);
            // 
            // filterGroupsListBx
            // 
            this.filterGroupsListBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterGroupsListBx.FormattingEnabled = true;
            this.filterGroupsListBx.Location = new System.Drawing.Point(0, 31);
            this.filterGroupsListBx.Name = "filterGroupsListBx";
            this.filterGroupsListBx.Size = new System.Drawing.Size(593, 56);
            this.filterGroupsListBx.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 87);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(593, 401);
            this.dataGridView1.StandardTab = true;
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            // 
            // CollisionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.filterGroupsListBx);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CollisionManager";
            this.Size = new System.Drawing.Size(593, 488);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox filterGroupNameTxtBx;
        private System.Windows.Forms.ToolStripButton addFilterGroupBt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton removeFilterGroupBt;
        private System.Windows.Forms.ListBox filterGroupsListBx;
        private System.Windows.Forms.DataGridView dataGridView1;

    }
}
