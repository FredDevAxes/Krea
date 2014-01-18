using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Krea.RemoteDebugger
{
    public partial class RemoteInfo : UserControl
    {
        public RemoteInfo()
        {
            InitializeComponent();
        }

        public void init(string ipAddress)
        {
            this.currentIpLabel.Text = ipAddress;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string target = "https://www.native-software.com/index.php/news";

            try
            {
                System.Diagnostics.Process.Start(target);
            }

            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);

            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);

            }
        }
    }
}
