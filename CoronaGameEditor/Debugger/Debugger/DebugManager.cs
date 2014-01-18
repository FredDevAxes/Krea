using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
using System.Threading;

namespace Krea.GameEditor.Debugger
{
    public partial class DebugManager : UserControl
    {
        private CoronaGameProject project;
        public static ASyncSocket server;
        public ASyncSocket AcceptedEmulator;

        public List<BreakPoint> BreakPoints;
        public List<Watch> Watchs;
        public DebugManager()
        {
            InitializeComponent();
            BreakPoints = new List<BreakPoint>();
            Watchs = new List<Watch>();
            server = new ASyncSocket();
            server.OnAccept += new ASyncSocket.OnAcceptEventHandler(server_OnAccept);
            server.OnReceive += new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
            server.OnSend += new ASyncSocket.OnSendEventHandler(server_OnSend);
            server.OnListen += new ASyncSocket.OnListenEventHandler(server_OnListen);
            server.OnAcceptFailed += new ASyncSocket.OnAcceptFailedEventHandler(server_OnAcceptFailed);
            server.OnListenFailed += new ASyncSocket.OnListenFailedEventHandler(server_OnListenFailed);
            server.OnReceiveFailed += new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
            server.OnSendFailed += new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);

            server.Listen(8171);
            
        }
        public void Init(CoronaGameProject cProject) {
            this.project = cProject;

        }
        delegate void dsendBt_Click(object sender, EventArgs e);
        private void sendBt_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dsendBt_Click d = new dsendBt_Click(sendBt_Click);
                object[] args = { sender,e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {
                        if (!SendTb.Text.Equals(""))
                        {
                            AcceptedEmulator.Send(SendTb.Text);
                        }

                    }
                }
            }

        }


        delegate void dserver_OnSendFailed(Exception Exception);
        void server_OnSendFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnSendFailed d = new dserver_OnSendFailed(server_OnSendFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {

                String command = "\nException OnSendFailled: " + Exception.Message + "\n";
                commandtb.AppendText(command);
                commandtb.ScrollToCaret();
            }
        }
        delegate void dserver_OnReceiveFailed(Exception Exception);
        void server_OnReceiveFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnReceiveFailed d = new dserver_OnReceiveFailed(server_OnReceiveFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\nException OnReceiveFailed: " + Exception.Message + "\n";
                commandtb.AppendText(command);
                commandtb.ScrollToCaret();
            
            }
        }
        delegate void dserver_OnListenFailed(Exception Exception);
        void server_OnListenFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnListenFailed d = new dserver_OnListenFailed(server_OnListenFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\nException OnListenFailed: " + Exception.Message + "\n";
                commandtb.AppendText(command);
                commandtb.ScrollToCaret();
                
            }
        }
        delegate void dserver_OnAcceptFailed(Exception Exception);
        void server_OnAcceptFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnAcceptFailed d = new dserver_OnAcceptFailed(server_OnAcceptFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\nEXCEPTION OnAcceptFailed : " + Exception.Message + "\n";
                commandtb.AppendText(command);
                commandtb.ScrollToCaret();
       
            }
        }
        delegate void dserver_OnListen();
        void server_OnListen()
        {
            if (this.InvokeRequired)
            {
                dserver_OnListen d = new dserver_OnListen(server_OnListen);
                this.Invoke(d);
            }
            else
            {
                String command = "\n> Debugger ready.\n";
                       command += "\n> Waiting for the emulator.\n";
                commandtb.AppendText(command);
                commandtb.ScrollToCaret();
            }
        }
        delegate void dserver_OnSend();
        void server_OnSend()
        {
            if (this.InvokeRequired)
            {
                dserver_OnSend d = new dserver_OnSend(server_OnSend);
                this.Invoke(d);
            }
            else
            {
                String command = "\n> Command has been send to the emulator.\n";
                commandtb.AppendText(command);
                commandtb.ScrollToCaret();
            }
        }
        delegate void dserver_OnReceive(string Stream);
        void server_OnReceive(string Stream)
        {
            if (this.InvokeRequired)
            {
                dserver_OnReceive d = new dserver_OnReceive(server_OnReceive);
                object[] args = { Stream };
                this.Invoke(d, args);
            }
            else
            {
                String receive = new String(Stream.ToCharArray());
                commandtb.AppendText(receive);
                commandtb.ScrollToCaret();
                //              Console.WriteLine(Stream);
            }

        }
        delegate void dserver_OnAccept(ASyncSocket AcceptedSocket);
        void server_OnAccept(ASyncSocket AcceptedSocket)
        {
            if (this.InvokeRequired)
            {
                dserver_OnAccept d = new dserver_OnAccept(server_OnAccept);
                object[] args = { AcceptedSocket };
                this.Invoke(d, args);
            }
            else
            {
               
                AcceptedEmulator = AcceptedSocket;
                AcceptedSocket.OnAccept += new ASyncSocket.OnAcceptEventHandler(server_OnAccept);
                AcceptedSocket.OnReceive += new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                AcceptedSocket.OnSend += new ASyncSocket.OnSendEventHandler(server_OnSend);
                AcceptedSocket.OnListen += new ASyncSocket.OnListenEventHandler(server_OnListen);
                AcceptedSocket.OnAcceptFailed += new ASyncSocket.OnAcceptFailedEventHandler(server_OnAcceptFailed);
                //server.OnConnect += new ASyncSocket.OnConnectEventHandler(server_OnConnect);
                //server.OnConnectFailed += new ASyncSocket.OnConnectFailedEventHandler(server_OnConnectFailed);
                //server.OnDisconnect += new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);
                //server.OnDisconnectFailed += new ASyncSocket.OnDisconnectFailedEventHandler(server_OnDisconnectFailed);
                AcceptedSocket.OnListenFailed += new ASyncSocket.OnListenFailedEventHandler(server_OnListenFailed);
                AcceptedSocket.OnReceiveFailed += new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                AcceptedSocket.OnSendFailed += new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);
               // AcceptedSocket.Send("STEP\n");
                String command = "\n> Emulator Ready to receive order.\n";
                commandtb.AppendText(command);
                commandtb.ScrollToCaret();
            }
        }

        delegate void dbutton5_Click(object sender, EventArgs e);
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton5_Click d = new dbutton5_Click(button5_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        AcceptedEmulator.Send("LOCALS\n");


                    }
                }
            }
        }
        delegate void dbutton8_Click(object sender, EventArgs e);
        private void button8_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton8_Click d = new dbutton8_Click(button8_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        AcceptedEmulator.Send("BACKTRACE\n");


                    }
                }
            }
        }

        delegate void dbutton11_Click(object sender, EventArgs e);
        private void button11_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton11_Click d = new dbutton11_Click(button11_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        AcceptedEmulator.Send("RUN\n");


                    }
                }
            }
        }

        delegate void dbutton3_Click(object sender, EventArgs e);
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton3_Click d = new dbutton3_Click(button3_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        AcceptedEmulator.Send("STEP\n");


                    }
                }
            }
        }

        delegate void dbutton2_Click(object sender, EventArgs e);
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton2_Click d = new dbutton2_Click(button2_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        AcceptedEmulator.Send("OVER\n");


                    }
                }
            }
        }

        delegate void ddumpBt_Click(object sender, EventArgs e);
        private void dumpBt_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                ddumpBt_Click d = new ddumpBt_Click(dumpBt_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {
                        if (!dumptb.Text.Equals(""))
                        {
                            String message = "DUMP return (" + dumptb.Text + ") \n";
                            AcceptedEmulator.Send(message);
                        }

                    }
                }
            }
        }

        delegate void dbutton9_Click(object sender, EventArgs e);
        private void button9_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton9_Click d = new dbutton9_Click(button9_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        for (int i = 0; i < BreakPoints.Count; i++)
                        {
                            commandtb.AppendText("\nBreackpoint on file " + BreakPoints[i].File + " on line " + BreakPoints[i].LineNumber + "\n");
                        }


                    }
                }
            }
        }

        delegate void dbutton1_Click(object sender, EventArgs e);
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton1_Click d = new dbutton1_Click(button1_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {
                        for (int i = 0; i < BreakPoints.Count; i++)
                        {
                            AcceptedEmulator.Send("DELB "+BreakPoints[i].File+" "+BreakPoints[i].LineNumber+"\n");
                        }
                        BreakPoints.Clear();
                    }
                }
            }
        }

        delegate void dbutton10_Click(object sender, EventArgs e);
        private void button10_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton10_Click d = new dbutton10_Click(button10_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                for (int i = 0; i < Watchs.Count; i++) {
                    commandtb.AppendText("\nWatch n°"+Watchs[i].Number+" exp: "+Watchs[i].Expression+"\n");
                }
            }
        }

        delegate void dbutton12_Click(object sender, EventArgs e);
        private void button12_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton12_Click d = new dbutton12_Click(button12_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        for (int i = 0; i < Watchs.Count; i++)
                        {
                            //TODO : COMMAND A VERIF (Désasembler avec SocketSpy)
                            AcceptedEmulator.Send("DELW " + Watchs[i].Number+ "\n");
                           
                        }
                        Watchs.Clear();
                    }
                }
            }
        }

        delegate void dbutton14_Click(object sender, EventArgs e);
        private void button14_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton14_Click d = new dbutton14_Click(button14_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        if (!filetb.Text.Equals("") && !linetb.Text.Equals(""))
                        {
                            String message = "SETB " + filetb.Text.ToLower() + " " + linetb.Text + "\n";
                            BreakPoint bt = new BreakPoint(filetb.Text, linetb.Text);
                            BreakPoints.Add(bt);
                            AcceptedEmulator.Send(message);
                        }

                    }
                }
            }
        }

        delegate void dbutton13_Click(object sender, EventArgs e);
        private void button13_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton13_Click d = new dbutton13_Click(button13_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        if (!expressiontb.Equals(""))
                        {
                            //TODO : COMMAND A TROUVER
                            Watch wt = new Watch(Convert.ToString(Watchs.Count + 1), expressiontb.Text);
                            AcceptedEmulator.Send("SETW " + expressiontb.Text +"\n");
                            Watchs.Add(wt);
                        }

                    }
                }
            }
        }

        delegate void dbutton6_Click(object sender, EventArgs e);
        private void button6_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton6_Click d = new dbutton6_Click(button6_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        if (!filetb.Text.Equals("") && !linetb.Text.Equals(""))
                        {
                            AcceptedEmulator.Send("EXIT\n");
                        }

                    }
                }
            }
        }

        delegate void dbutton15_Click(object sender, EventArgs e);
        private void button15_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton15_Click d = new dbutton15_Click(button15_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        if (!dumptb.Text.Equals(""))
                        {
                            AcceptedEmulator.Send("EXEC " + dumptb.Text + "\n");
                        }

                    }
                }
            }
        }

        delegate void dbutton6_Click_1(object sender, EventArgs e);
        private void button6_Click_1(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton6_Click_1 d = new dbutton6_Click_1(button6_Click_1);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        if (!dumptb.Text.Equals(""))
                        {
                            AcceptedEmulator.Send("FRAME" + dumptb.Text + "\n");
                        }

                    }
                }
            }
        }

        delegate void dbutton7_Click(object sender, EventArgs e);
        private void button7_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dbutton7_Click d = new dbutton7_Click(button7_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        if (!dumptb.Text.Equals(""))
                        {
                            AcceptedEmulator.Send("EXEC return (" + dumptb.Text + ")\n");
                        }

                    }
                }
            }
        }


    

    }
}
