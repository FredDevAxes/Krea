using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Krea.RemoteDebugger
{
    public partial class RemoteControllerForm : Form
    {
        public RemoteControllerForm()
        {
            InitializeComponent();
        }

        private void RemoteControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.appRemoteController1.closeDebuggerSession();
        }


        private static IPAddress empty = IPAddress.Parse("0.0.0.0");
        private static IPAddress intranetMask1 = IPAddress.Parse("10.255.255.255");
        private static IPAddress intranetMask2 = IPAddress.Parse("172.16.0.0");
        private static IPAddress intranetMask3 = IPAddress.Parse("172.31.255.255");
        private static IPAddress intranetMask4 = IPAddress.Parse("192.168.255.255");

        public void init()
        {
            string ipAddress = this.GetIpAdresse();
            if (!ipAddress.Equals(""))
            {
                this.appRemoteController1.initServer(ipAddress, 8200, this);
                IPAddress addr = IPAddress.Parse(ipAddress);
                if (!IsOnIntranet(addr))
                {
                    this.remoteInfo1.init("No local network found!");
                }
                else
                    this.remoteInfo1.init(ipAddress);
            }
            else
            {
                MessageBox.Show("Cannot open remote controller because no activated network was found on your computer!\nPlease check your local networks!", "Cannot open remote controller", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }

           
        }

        private void CheckIPVersion(IPAddress ipAddress, IPAddress mask, out byte[] addressBytes, out byte[] maskBytes)
       {
         if (mask == null)
         {
           throw new ArgumentException();
         }
   
         addressBytes = ipAddress.GetAddressBytes();
         maskBytes = mask.GetAddressBytes();
   
         if (addressBytes.Length != maskBytes.Length)
         {
           throw new ArgumentException("The address and mask don't use the same IP standard");
         }
       }

        public IPAddress And(IPAddress ipAddress, IPAddress mask)
        {
            byte[] addressBytes;
            byte[] maskBytes;
            CheckIPVersion(ipAddress, mask, out addressBytes, out maskBytes);

            byte[] resultBytes = new byte[addressBytes.Length];
            for (int i = 0; i < addressBytes.Length; ++i)
            {
                resultBytes[i] = (byte)(addressBytes[i] & maskBytes[i]);
            }

            return new IPAddress(resultBytes);
        }

        public bool IsOnIntranet(IPAddress ipAddress)
        {
            if (empty.Equals(ipAddress))
            {
                return false;
            }
            bool onIntranet = IPAddress.IsLoopback(ipAddress);
            onIntranet = onIntranet ||
              ipAddress.Equals(And(ipAddress,intranetMask1)); //10.255.255.255
            onIntranet = onIntranet ||
              ipAddress.Equals(And(ipAddress,intranetMask4)); ////192.168.255.255

            onIntranet = onIntranet || (intranetMask2.Equals(And(ipAddress,intranetMask2))
              && ipAddress.Equals(And(ipAddress,intranetMask3)));

            return onIntranet;
        }

        private string GetIpAdresse()
        {
            IPAddress[] ipEntry = Dns.GetHostAddresses(Dns.GetHostName());
            //IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            if (ipEntry != null)
            {
                for (int i = 0; i < ipEntry.Length; i++)
                {
                    if (ipEntry[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return ipEntry[i].ToString();
                }

            }
            return "";
        }



        private void RemoteControllerForm_Load(object sender, EventArgs e)
        {
            this.appRemoteController1.initServer(this.GetIpAdresse(), 8200, this);
        }

        
    }
}
