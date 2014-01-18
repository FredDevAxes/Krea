using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.Corona_Classes.Widgets;

namespace Krea.GameEditor.Panel_Widgets
{
    public partial class PanelPickerWheel : UserControl
    {
        private WidgetPickerWheel pickerWheel;
        private WidgetPickerWheel.PickerWheelColumn columnSelected;
        public PanelPickerWheel(WidgetPickerWheel pickerWheel)
        {
            InitializeComponent();
            this.pickerWheel = pickerWheel;


            if (this.pickerWheel != null)
            {
                this.init();
            }
        }

        private void init()
        {
            this.columnSelected = null;
            this.columnsListBx.Items.Clear();
            this.rowsListBx.Items.Clear();

            this.widgetNameTxtBx.Text = this.pickerWheel.Name;
            
            for(int i = 0;i<this.pickerWheel.Columns.Count;i++)
            {
                this.columnsListBx.Items.Add(this.pickerWheel.Columns[i]);
            }
                
        }

        private void setColumnSelected(WidgetPickerWheel.PickerWheelColumn column)
        {
            this.columnSelected = column;

            if (this.columnSelected.Width < 999)
            {
                this.isAutoSizeChkBx.Checked = false;
                this.columWidthUpDw.Enabled = true;
                this.columWidthUpDw.Value = this.columnSelected.Width;
            }

            this.startIndexUpDw.Value = this.columnSelected.StartIndex;

            if (this.columnSelected.Alignement == WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.center)
                this.alignementCmbBx.SelectedItem = "center";
            else if (this.columnSelected.Alignement == WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.right)
                this.alignementCmbBx.SelectedItem = "right";
            else if (this.columnSelected.Alignement == WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.left)
                this.alignementCmbBx.SelectedItem = "left";

            this.rowsListBx.Items.Clear();
            for (int i = 0; i < this.columnSelected.Datas.Count; i++)
            {
                this.rowsListBx.Items.Add(this.columnSelected.Datas[i]);
            }
        }

        private void setRowSelected(int index)
        {
            this.rowValueTxtBx.Text = this.columnSelected.Datas[index];
        }

        private void addColumn_Click(object sender, EventArgs e)
        {
            string name = this.columnNameTxtBx.Text;
            int width =999;
            int startIndex = (int)this.startIndexUpDw.Value;

            WidgetPickerWheel.PickerWheelColumn.ColumnAlignement alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.left;

            if ( this.alignementCmbBx.SelectedItem.ToString().Equals("center"))
               alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.center;
            else if (this.alignementCmbBx.SelectedItem.ToString().Equals("left"))
               alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.left;
            else if (this.alignementCmbBx.SelectedItem.ToString().Equals("right"))
               alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.right;

            if(!this.isAutoSizeChkBx.Checked)
                width = (int)this.columWidthUpDw.Value;

            WidgetPickerWheel.PickerWheelColumn col = new WidgetPickerWheel.PickerWheelColumn(name, startIndex, width, alignement);

            this.pickerWheel.Columns.Add(col);
            this.init();
         
        }

        private void removeBt_Click(object sender, EventArgs e)
        {
            if (this.columnsListBx.SelectedItem != null)
            {
                WidgetPickerWheel.PickerWheelColumn col = (WidgetPickerWheel.PickerWheelColumn)this.columnsListBx.SelectedItem;
                this.pickerWheel.Columns.Remove(col);
                this.init();
            }
        }

        private void upColumn_Click(object sender, EventArgs e)
        {
            if (this.pickerWheel.Columns.Count > 2)
            {
                if (this.columnsListBx.SelectedIndex > 0)
                {
                    int selectedIndex = this.columnsListBx.SelectedIndex;
                    this.pickerWheel.Columns.Reverse(selectedIndex - 1, 2);

                    this.init();
                }
            }
        }

        private void downColumn_Click(object sender, EventArgs e)
        {
            if (this.pickerWheel.Columns.Count > 2)
            {
                if (this.columnsListBx.SelectedIndex < pickerWheel.Columns.Count - 1)
                {
                    int selectedIndex = this.columnsListBx.SelectedIndex;
                    this.pickerWheel.Columns.Reverse(selectedIndex, 2);

                    this.init();
                }
            }
        }

        private void validerBt_Click(object sender, EventArgs e)
        {
            this.Parent.Dispose();
        }

        private void saveColumn_Click(object sender, EventArgs e)
        {
            if (this.columnSelected != null)
            {
                string name = this.columnNameTxtBx.Text;
                int width = 999;
                int startIndex = (int)this.startIndexUpDw.Value;

                WidgetPickerWheel.PickerWheelColumn.ColumnAlignement alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.left;

                if (this.alignementCmbBx.SelectedItem.Equals("center"))
                    alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.center;
                else if (this.alignementCmbBx.SelectedItem.Equals("left"))
                    alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.left;
                else if (this.alignementCmbBx.SelectedItem.Equals("right"))
                    alignement = WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.right;

                if (!this.isAutoSizeChkBx.Checked)
                    width = (int)this.columWidthUpDw.Value;

                this.columnSelected.Alignement = alignement;
                this.columnSelected.Width = width;
                this.columnSelected.StartIndex = startIndex;
                this.columnSelected.ColumnName = name;

                this.init();
            }
        }

        private void columnsListBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.columnsListBx.SelectedItem!= null)
            {
                WidgetPickerWheel.PickerWheelColumn col = (WidgetPickerWheel.PickerWheelColumn) this.columnsListBx.SelectedItem;
                this.setColumnSelected(col);
            }
            
        }

        private void addRow_Click(object sender, EventArgs e)
        {
            if (this.columnSelected != null)
            {
                string value = this.rowValueTxtBx.Text;
                this.columnSelected.Datas.Add(value);
                this.rowsListBx.Items.Add(value);
            }
        }

        private void saveRow_Click(object sender, EventArgs e)
        {
            if (this.rowsListBx.SelectedItem != null && this.columnSelected != null)
            {
                string value = this.rowValueTxtBx.Text;
                this.columnSelected.Datas[this.rowsListBx.SelectedIndex] = value;
                this.rowsListBx.Items[this.rowsListBx.SelectedIndex] = value;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.rowsListBx.SelectedItem != null && this.columnSelected != null)
            {
                this.columnSelected.Datas.RemoveAt(this.rowsListBx.SelectedIndex);
                this.rowsListBx.Items.RemoveAt(this.rowsListBx.SelectedIndex);
            }
        }

        private void upRow_Click(object sender, EventArgs e)
        {
            if (this.columnSelected != null)
            {
                if (columnSelected.Datas.Count > 2)
                {
                    if (this.rowsListBx.SelectedIndex > 0)
                    {
                        int selectedIndex = this.rowsListBx.SelectedIndex;
                        columnSelected.Datas.Reverse(selectedIndex - 1, 2);
                        this.setColumnSelected(this.columnSelected);
                    }
                }
            }
            
        }

        private void downRow_Click(object sender, EventArgs e)
        {
            if (this.columnSelected != null)
            {
                if (columnSelected.Datas.Count > 2)
                {
                    if (this.rowsListBx.SelectedIndex < columnSelected.Datas.Count -1)
                    {
                        int selectedIndex = this.rowsListBx.SelectedIndex;
                        columnSelected.Datas.Reverse(selectedIndex, 2);
                        this.setColumnSelected(this.columnSelected);
                    }
                }
            }

        }
    }
}
