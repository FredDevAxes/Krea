using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Krea.GameEditor.CodeEditor
{
    public partial class DataTip : Form
    {
        public DataTip()
        {
            InitializeComponent();
        }

        public void init()
        {
            this.treeListView1.LostFocus +=new EventHandler(DataTip_LostFocus);
        }

        private void DataTip_LostFocus(object sender, EventArgs args)
        {
            this.Dispose();
        }
    }
}
