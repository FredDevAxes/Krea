using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.GameEditor.CodeEditor.APIElements;
namespace Krea.GameEditor.CodeEditor
{
    public partial class AutoCompleteForm : Form
    {
        public AutoCompleteForm()
        {
            InitializeComponent();
        }


        public void refreshValues(List<APIItem> items)
        {
            this.itemsListView.BeginUpdate();
            this.itemsListView.Items.Clear();

            ColumnHeader nameColumn = this.itemsListView.Columns["Name"];

            for (int i = 0; i < items.Count; i++)
            {
                APIItem item = items[i];
                if (item.isFunction)
                {
                    ListViewItem itemNode = this.itemsListView.Items.Add(item.name);
                    itemNode.Group = this.itemsListView.Groups["functionsGroup"];

                }

                else
                {
                    ListViewItem itemNode = this.itemsListView.Items.Add(item.name);
                    itemNode.Group = this.itemsListView.Groups["fieldsGroup"];
                }
                    
            }

            this.itemsListView.EndUpdate();
        }

        private void itemsListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.Close();
        }

        private void itemsListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r'))
            {
                if (this.itemsListView.SelectedItems.Count > 0)
                {
                    Form1 form = (Form1)this.Owner;
                    form.cgEeditor1.ActiveDocument.Scintilla.InsertText(this.itemsListView.SelectedItems[0].Text);

                    this.Hide();
                }
            }

        }
    }
}
