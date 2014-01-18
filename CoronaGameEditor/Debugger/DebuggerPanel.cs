using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
using Krea.GameEditor.Debugger;
using ScintillaNet;
using System.Text.RegularExpressions;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Net;

namespace Krea.Debugger
{
    public partial class DebuggerPanel : UserControl
    {
        //////////////////////////////////////////
        /// PROPERTIES
        //////////////////////////////////////////
        public static ASyncSocket server;
        public ASyncSocket AcceptedEmulator;
        public List<BreakPoint> BreakPoints;
        public List<Watch> Watchs;
        public List<Local> Locals;
        public String LastCommandReceive;
        public String CurrentCommandReceive;
        public String CurrentCommandSend;
        public ResponseBuilder BuilderState;

        private CoronaGameProject project;
        private Form1 mainForm;
        private List<string> listCommands;

        //////////////////////////////////////////
        /// CONSTRUCTOR
        //////////////////////////////////////////
        public DebuggerPanel()
        {
            InitializeComponent();

            this.splitContainer2.SplitterWidth = 15;
            this.splitContainer3.SplitterWidth = 15;
        }

        //////////////////////////////////////////
        /// INIT METHODS
        //////////////////////////////////////////
        public void initServer()
        {
            if (this.project != null)
            {
                this.setHandLostWithSimulator(true);
                this.mainForm.workspaceViewToolStripMenuItem.Enabled = false;
                if (server == null)
                {
                    LastCommandReceive = "";
                    CurrentCommandSend = "";
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
                    
                    server.Listen("127.0.0.1", 8171);
                }

                BreakPoints.Clear();
                Watchs.Clear();
                this.outPutTxtBx.Clear();
                this.watchesListView.Items.Clear();
                this.localsListView.Items.Clear();
                this.backtraceListView.Items.Clear();

                try
                {

                    this.closeAllTempFilesFromEditor();

                    //Create a debug temp directory
                    string tempFolderPath = this.project.BuildFolderPath + "\\..\\DebugTemp";
                    if (Directory.Exists(tempFolderPath))
                        Directory.Delete(tempFolderPath, true);

                    DirectoryInfo info = Directory.CreateDirectory(tempFolderPath);
                    //Copy all Lua source of the build directory
                    foreach (String filePath in Directory.GetFiles(this.project.BuildFolderPath, "*.lua"))
                    {
                        string name = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                        File.Copy(filePath, info.FullName + "\\" + name, true);

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during creation of debug temp files\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                   
                }

                DocumentForm doc = this.mainForm.cgEeditor1.ActiveDocument;
                if (doc != null)
                {
                    foreach (Line line in doc.Scintilla.Lines)
                    {
                        int mask = doc.Scintilla.Markers.GetMarkerMask(line);
                        if (mask != 0)
                        {
                            BreakPoints.Add(new BreakPoint(doc.Text, (line.Number + 1).ToString()));
                        }

                    }
                }


               

            }

        }

     

        public void Init(CoronaGameProject cProject, Form1 mainForm)
        {
            this.project = cProject;
            this.mainForm = mainForm;
            listCommands = new List<string>();
            LastCommandReceive = "";
            CurrentCommandSend = "";

        }

        //////////////////////////////////////////
        /// CODES EDITORS METHODS
        //////////////////////////////////////////
        public void closeAllTempFilesFromEditor()
        {
            if (this.project != null)
            {
                string tempFolderPath = this.project.BuildFolderPath + "\\..\\DebugTemp";
                List<IDockContent> forms = this.mainForm.cgEeditor1.dockPanel.Documents.ToList();

                foreach (DocumentForm documentForm in forms)
                {
                    FileInfo info = new FileInfo(documentForm.FilePath);
                    if (info.Directory.Name.Equals("DebugTemp"))
                    {
                        documentForm.Close();
                        documentForm.Dispose();
                    }

                }

                forms.Clear();
            }

        }

        //////////////////////////////////////////
        /// COMMANDS SERVER METHODS
        //////////////////////////////////////////
        private void sendCommandFromString(string command)
        {
            if (command.Equals("LOCALS"))
            {
                this.localsListView.Items.Clear();
                this.sendLocalsCommand();
            }

            else if (command.Equals("BACKTRACE"))
            {
                this.backtraceListView.Items.Clear();
                this.sendBacktraceCommand();
            }

        }

        public void sendAddBreakPointCommand(BreakPoint breakPoint)
        {
            if (AcceptedEmulator != null)
            {
                if (AcceptedEmulator.IsConnected)
                {

                    String message = "SETB " + breakPoint.File + " " + breakPoint.LineNumber + "\n";
                    AcceptedEmulator.Send(message);

                }
            }
        }

        public void sendDeleteBreakPointCommand(BreakPoint breakPoint)
        {
            if (AcceptedEmulator != null)
            {
                if (AcceptedEmulator.IsConnected)
                {
                    AcceptedEmulator.Send("DELB " + breakPoint.File + " " + breakPoint.LineNumber + "\n");
                }
            }
        }

        public void sendLocalsCommand()
        {
            if (AcceptedEmulator != null)
            {
                if (AcceptedEmulator.IsConnected)
                {
                    AcceptedEmulator.Send("LOCALS\n");
                }
            }
        }

        public void sendBacktraceCommand()
        {
            if (AcceptedEmulator != null)
            {
                if (AcceptedEmulator.IsConnected)
                {
                    AcceptedEmulator.Send("BACKTRACE\n");
                }
            }
        }

        public void sendDumpCommand(string localName)
        {
            if (AcceptedEmulator != null)
            {
                if (AcceptedEmulator.IsConnected)
                {
                    if (!localName.Equals(""))
                    {
                       
                        string[] splittedlocalName = localName.Split('.');
                        if (splittedlocalName.Length > 1)
                        {
                            string finalLocalName = splittedlocalName[0];
                            for (int i = 1; i < splittedlocalName.Length; i++)
                            {
                                int indexTab = -1;
                                if (int.TryParse(splittedlocalName[i], out indexTab))
                                {
                                    finalLocalName += "[" + indexTab + "]";
                                }
                                else
                                {
                                    finalLocalName += "." + splittedlocalName[i];
                                }
                            }


                            String message = "DUMP return (" + finalLocalName + ") \n";
                            AcceptedEmulator.Send(message);
                            
                        }
                        else
                        {
                            String message = "DUMP return (" + localName + ") \n";
                            AcceptedEmulator.Send(message);
                        }
                       
                    }

                }
            }
        }

        public void AnalyzeStatus(String debug_response)
        {

            if (debug_response.Equals("")) return;
            String[] breakpoint = debug_response.Split('\n');
            breakpoint = CleanDuplicate(breakpoint);
            if (Regex.IsMatch(breakpoint[0], "^200 OK$"))
            {
                this.BuilderState = ResponseBuilder.Instance;
                this.BuilderState.Init(breakpoint[0]);
                this.BuilderState.Analyse();

            }
            else if (Regex.IsMatch(breakpoint[0], "^200 OK(\\s?\\d+)$"))
            {
                string[] responseTab = Regex.Split(breakpoint[0], " ");

                if (LastCommandReceive != null)
                {
                    if (Regex.IsMatch(LastCommandReceive, "^setb(\\s+\\s+\\d+)$"))
                    {
                        string[] lastResponseTab = Regex.Split(LastCommandReceive, "\\s");
                        if (Convert.ToInt32(lastResponseTab[2]) > 0)
                        {
                            string message = debug_response.Substring(0, Convert.ToInt32(lastResponseTab[2]));
                            this.BuilderState = ResponseBuilder.Instance;
                            this.BuilderState.Init(message);
                            this.BuilderState.Analyse();
                            // MessageBox.Show(BuilderState.ToString());
                        }
                    }
                    else if (breakpoint.Length > 1)
                    {
                        this.BuilderState = ResponseBuilder.Instance;
                        this.BuilderState.Init(debug_response);
                        this.BuilderState.Analyse();
                    }
                }

            }
            else if (Regex.IsMatch(breakpoint[0], "^202 Paused\\s+([\\w\\W]+)\\s+(\\d+)$"))
            {


                string[] response = Regex.Split(breakpoint[0], "\\s");
                DocumentForm activeDoc = this.mainForm.cgEeditor1.ActiveDocument;
                if (activeDoc != null)
                    activeDoc.Scintilla.FindReplace.ClearAllHighlights();

                if (!response[2].Equals("=?"))
                {
                    this.BuilderState.CurrentBreakPointFile = response[2];
                    this.BuilderState.CurrentBreakPointLine = Convert.ToInt32(response[3]);

                    bool isOpen = false;
                    foreach (DocumentForm documentForm in this.mainForm.cgEeditor1.dockPanel.Documents)
                    {
                        if (response[2].Equals(documentForm.Text.Replace(" *", ""), StringComparison.OrdinalIgnoreCase))
                        {
                            documentForm.Select();
                            documentForm.Show(this.mainForm.cgEeditor1.dockPanel);
                            isOpen = true;
                            break;
                        }
                    }


                    // Open the files
                    if (!isOpen)
                    {
                        DirectoryInfo dir = new DirectoryInfo(this.project.BuildFolderPath);
                        String FilePath = dir.Parent.FullName + "\\DebugTemp\\" + response[2];
                        if (File.Exists(FilePath))
                            this.mainForm.cgEeditor1.OpenFileInEditor(FilePath);
                    }


                    //-----SELECT LINE IN EDITOR
                    try
                    {


                        if (this.mainForm.cgEeditor1.ActiveDocument != null)
                        {
                            if (!this.mainForm.cgEeditor1.ActiveDocument.Scintilla.Text.Equals(""))
                            {

                                int lineNumber = -1;
                                int.TryParse(response[3], out lineNumber);
                                if (lineNumber > -1)
                                {
                                    if (this.mainForm.cgEeditor1.ActiveDocument.Scintilla.Lines.Count > lineNumber - 1)
                                    {
                                        this.mainForm.cgEeditor1.SetDebuggerAtLine(lineNumber - 1);
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    // this.outPutTxtBx.AppendText(BuilderState.ToString());

                    if (CurrentCommandSend != null && this.listCommands.Count == 0)
                    {
                        if (CurrentCommandSend.Contains("STEP"))
                        {
                            this.listCommands.Add("LOCALS");
                            this.listCommands.Add("BACKTRACE");
                        }
                        this.listCommands = new List<String>(this.listCommands.Distinct().ToArray());
                    }
                }
                else
                {
                    this.BuilderState.CurrentBreakPointFile = "?";
                    this.BuilderState.CurrentBreakPointLine = 0;
                    if (activeDoc != null)
                    {
                        activeDoc.Scintilla.Markers.DeleteAll(2);
                    }

                    // this.outPutTxtBx.AppendText(BuilderState.ToString());
                }


            }
            else if (Regex.IsMatch(breakpoint[0], "^203 Paused\\s+([\\w\\W]+)\\s+(\\d+)\\s+(\\d+)$"))
            {
                string[] response = Regex.Split(breakpoint[0], "\\s");
                this.BuilderState.CurrentBreakPointFile = response[2];
                this.BuilderState.CurrentBreakPointLine = Convert.ToInt32(response[3]);
                //   String FilePath = cProject.BuildFolderPath + "\\" + response[2];
                //  cgEeditor1.OpenFileInEditor(FilePath);
                //  cgEeditor1.SetDebuggerAtLine(Convert.ToInt32(response[3]) - 1);


                if (CurrentCommandSend != null)
                {
                    if (!CurrentCommandSend.Equals("LOCALS\n"))
                    {
                        this.listCommands.Add("LOCALS");
                    }
                    if (!CurrentCommandSend.Equals("BACKTRACE\n"))
                    {
                        this.listCommands.Add("BACKTRACE");
                    }
                    this.listCommands = new List<String>(this.listCommands.Distinct().ToArray());
                }
            }
            else if (Regex.IsMatch(breakpoint[0], "^401 Error in Expression (\\d+)$"))
            {
                this.BuilderState.ErrorFound = true;
                this.BuilderState.ErrorDescription = "401 Error In Expression";
            }
            else if (Regex.IsMatch(breakpoint[0], "^401 Error in Execution (\\d+)$"))
            {
                this.BuilderState.ErrorFound = true;
                this.BuilderState.ErrorDescription = "401 Error In Expression";
            }
            else if (Regex.IsMatch(breakpoint[0], "^(%s*)$"))
            {
                if (this.BuilderState.ErrorFound == true)
                {
                    string[] response = Regex.Split(breakpoint[0], "\\s");
                    this.BuilderState.ErrorDescription = breakpoint[0];
                    // MessageBox.Show(BuilderState.ToString());
                }

            }
            if (LastCommandReceive != null)
            {
                if (Regex.IsMatch(LastCommandReceive, "^200 OK(\\s?\\d+)$"))
                {
                    string[] lastResponseTab = Regex.Split(LastCommandReceive, "\\s");
                    int bytesNumber = -1;
                    int.TryParse(lastResponseTab[2], out bytesNumber);
                    if (bytesNumber > debug_response.Length) bytesNumber = debug_response.Length - 1;
                    if (bytesNumber > 0)
                    {
                        string message = debug_response.Substring(0, bytesNumber);
                        this.BuilderState = ResponseBuilder.Instance;
                        this.BuilderState.Init(message);
                        this.BuilderState.Analyse();

                    }
                }

                if (Regex.IsMatch(LastCommandReceive, "^401 Error in Expression (\\d+)$"))
                {
                    string[] lastResponseTab = Regex.Split(LastCommandReceive, "\\s");
                    int bytesNumber = -1;
                    int.TryParse(lastResponseTab[4], out bytesNumber);
                    if (bytesNumber > debug_response.Length) bytesNumber = debug_response.Length - 1;
                    if (bytesNumber > 0)
                    {
                        string message = debug_response.Substring(0, bytesNumber);
                        this.BuilderState = ResponseBuilder.Instance;
                        this.BuilderState.Init(message);
                        this.BuilderState.Analyse();

                        //  MessageBox.Show(BuilderState.ToString());
                    }
                }
                if (Regex.IsMatch(LastCommandReceive, "^401 Error in Execution (\\d+)$"))
                {
                    string[] lastResponseTab = Regex.Split(LastCommandReceive, "\\s");
                    int bytesNumber = -1;
                    int.TryParse(lastResponseTab[4], out bytesNumber);
                    if (bytesNumber > debug_response.Length) bytesNumber = debug_response.Length - 1;
                    if (bytesNumber > 0)
                    {
                        string message = debug_response.Substring(0, bytesNumber);
                        this.BuilderState = ResponseBuilder.Instance;
                        this.BuilderState.Init(message);
                        this.BuilderState.Analyse();

                        //  MessageBox.Show(BuilderState.ToString());
                    }
                }
            }
            LastCommandReceive = breakpoint[0];

            refreshCurrentBuildValues();
        }

        public void SyncDebuggerState()
        {
            //Sync BreakPoints and Watchs 
            this.BuilderState.syncWithDebugger(this.BreakPoints, this.Watchs, this.LastCommandReceive, this.CurrentCommandReceive, this.CurrentCommandSend);
        }

        public void SendAutomaticBackTraceAndLocalsCommand()
        {
            //Check if receive message is (200 OK) or (200 OK Number_of_byte)
            if (this.CurrentCommandReceive.Contains("Paused"))
            {
                for (int i = 0; i < this.listCommands.Count; i++)
                {
                    this.sendCommandFromString(this.listCommands[i]);

                }
                this.listCommands.Clear();
            }
        }

        //////////////////////////////////////////
        /// UTILITIES METHODS
        //////////////////////////////////////////
        public String[] CleanDuplicate(String[] Input)
        {
            List<String> tmp = new List<string>(Input.Distinct().ToArray());
            if (tmp.Count > 1)
            {
                if (Regex.IsMatch(tmp[0], "^200 OK$") && Regex.IsMatch(tmp[1], "^200 OK(\\s?\\d+)$"))
                {
                    tmp.RemoveAt(0);
                    return CleanDuplicate(tmp.ToArray());
                }
                else if (Regex.IsMatch(tmp[0], "^200 OK$") && Regex.IsMatch(tmp[1], "^200 OK$"))
                {
                    tmp.RemoveAt(0);
                    return CleanDuplicate(tmp.ToArray());
                }
                else
                {
                    return tmp.ToArray();
                }
            }
            else
            {
                return tmp.ToArray();
            }
        }

        public void refreshCurrentBuildValues()
        {
            this.BuilderState = ResponseBuilder.Instance;
            this.localsListView.BeginUpdate();
            TreeListViewItem itemParent = null;
            if (this.localsListView.SelectedItems.Count > 0)
            {
                itemParent = this.localsListView.SelectedItems[0];
            }

            //Copy all var
            if (this.BuilderState.Locals != null)
            {
                for (int i = 0; i < this.BuilderState.Locals.Count; i++)
                {
                    Local local = this.BuilderState.Locals[i];
                    TreeListViewItem localItem = new TreeListViewItem();
                    if (itemParent == null)
                    {
                        this.localsListView.Items.Add(localItem);
                    }
                    else
                    {
                        itemParent.Items.Add(localItem);
                    }


                    localItem.Text = local.Name;
                    if (local.Type == null) continue;
                    if (local.Type.Equals("TABLE"))
                        localItem.ImageIndex = 0;
                    else if (local.Type.Equals("FIELD"))
                        localItem.ImageIndex = 3;
                    else if (local.Type.Equals("FUNCTION"))
                        localItem.ImageIndex = 2;
                    else if (local.Type.Equals("USERDATA"))
                        localItem.ImageIndex = 1;

                    localItem.SubItems.Add(local.Type);
                    localItem.SubItems.Add(local.Value);
                    localItem.SetIndentation();
                    localItem.Tag = local;
                }

            }
            
            this.localsListView.EndUpdate();

            //The Watches
            this.watchesListView.Items.Clear();

            if (this.BuilderState.Watchs != null)
            {
                for (int i = 0; i < this.BuilderState.Watchs.Count; i++)
                {
                    Watch watch = this.BuilderState.Watchs[i];
                    if (watch == null) continue;
                    TreeListViewItem localItem = new TreeListViewItem();
                    localItem.Text = watch.Number;
                    localItem.SubItems.Add(watch.Expression);
                    this.watchesListView.Items.Add(localItem);

                }
            }

            if (this.BuilderState.Backtraces != null)
            {
                for (int i = 0; i < this.BuilderState.Backtraces.Count; i++)
                {
                    Backtrace trace = this.BuilderState.Backtraces[i];
                    if (trace == null) continue;
                    TreeListViewItem localItem = new TreeListViewItem();
                    localItem.Text = trace.Number.ToString();
                    localItem.SubItems.Add(trace.Kind);

                    int indexFileStart = 0;
                    if (trace.File.Contains("\\"))
                        indexFileStart = trace.File.LastIndexOf("\\") + 1;

                    localItem.SubItems.Add(trace.File.Substring(indexFileStart, trace.File.Length - indexFileStart));
                    localItem.SubItems.Add(trace.Line.ToString());
                    this.backtraceListView.Items.Add(localItem);
                    localItem.ImageIndex = 2;

                }
            }
        }

        delegate void dPrintReceiveMessageToConsole(String ReceiveMessage);
        private void PrintReceiveMessageToConsole(String ReceiveMessage)
        {
            if (this.InvokeRequired)
            {
                dserver_OnReceive d = new dserver_OnReceive(server_OnReceive);
                object[] args = { ReceiveMessage };
                this.Invoke(d, args);
            }
            else
            {
                ResponseBuilder response = ResponseBuilder.Instance;
                //TODO : Print dans la console en parcourant les liste du ResponseBuilder.

                //outPutTxtBx.AppendText("\n------------------\n");
                outPutTxtBx.AppendText(response.ToString());
               // outPutTxtBx.AppendText("\n------------------\n");
                /* String[] cutReceive = ReceiveMessage.Split('\n');
                 for (int i = 0; i < cutReceive.Length; i++)
                 {
                     if (!cutReceive[i].Contains("200 OK"))
                     {
                         outPutTxtBx.AppendText(cutReceive[i] + '\n');
                        
                     }
                 }*/
                outPutTxtBx.ScrollToCaret();
            }
        }


        //////////////////////////////////////////
        /// UI LIST AND TREEVIEW EVENT
        //////////////////////////////////////////
        private StringBuilder getLocalItemHierachy(TreeListViewItem item, StringBuilder hierarchy)
        {
            if (hierarchy == null) hierarchy = new StringBuilder();
            if (item.Parent != null)
            {
                hierarchy.Insert(0, item.Parent.Text + ".");
                return this.getLocalItemHierachy(item.Parent, hierarchy);
            }
            else
            {
                return hierarchy;
            }

            /* TreeListViewItem[] parents = item.ParentsInHierarch ;
           
             for (int i = parents.Length-1; i >= 0; i--)
             {
                 Local local = parents[i].Tag as Local;
                 strHierarch += local.Name + ".";
             }

             return strHierarch;*/
        }

        private void localsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.localsListView.SelectedItems.Count > 0)
            {
                TreeListViewItem item = this.localsListView.SelectedItems[0];
                Local local = item.Tag as Local;
                if (local.Type.Equals("TABLE"))
                {
                    item.Items.Clear();
                    StringBuilder localHierarchie = getLocalItemHierachy(item, null);
                    localHierarchie.Append(local.Name);
                    this.sendDumpCommand(localHierarchie.ToString());
                }

            }
        }

        //////////////////////////////////////////
        /// UI BUTTON EVENT
        //////////////////////////////////////////
        delegate void drunBt_Click(object sender, EventArgs e);
        private void runBt_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                drunBt_Click d = new drunBt_Click(runBt_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {
                        this.mainForm.cgEeditor1.SetDebuggerAtLine(-1);

                        this.backtraceListView.Items.Clear();
                        this.localsListView.Items.Clear();

                        this.setHandLostWithSimulator(false);
                        AcceptedEmulator.Send("RUN\n");
                    }
                }
            }
        }

        delegate void dstepBt_Click(object sender, EventArgs e);
        private void stepBt_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dstepBt_Click d = new dstepBt_Click(stepBt_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {

                        this.mainForm.cgEeditor1.SetDebuggerAtLine(-1);
                        AcceptedEmulator.Send("STEP\n");


                    }
                }
            }
        }

        delegate void doverBt_Click(object sender, EventArgs e);
        private void overBt_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                doverBt_Click d = new doverBt_Click(overBt_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {
                        this.mainForm.cgEeditor1.SetDebuggerAtLine(-1);
                        AcceptedEmulator.Send("OVER\n");
                    }
                }
            }
        }

        delegate void dsendCommandBt_Click(object sender, EventArgs e);
        private void sendCommandBt_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                dsendCommandBt_Click d = new dsendCommandBt_Click(sendCommandBt_Click);
                object[] args = { sender, e };
                this.Invoke(d, args);
            }
            else
            {
                if (AcceptedEmulator != null)
                {
                    if (AcceptedEmulator.IsConnected)
                    {
                        if (!this.commandTxtBx.Text.Equals(""))
                        {
                            AcceptedEmulator.Send(this.commandTxtBx.Text + "\n");
                        }
                    }
                }
            }
        }

        //////////////////////////////////////////
        /// ASyncSocket SERVER EVENT
        //////////////////////////////////////////
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
                String command = "\n> Send Failed: " + Exception.Message + "\n";
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();
                this.closeDebuggerSession(true);
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
                
                
                String command = "\n> Receive Failed: " + Exception.Message + "\n";
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();
                this.closeDebuggerSession(true);
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
                String command = "\n> Listen Failed: " + Exception.Message + "\n";
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();

                closeDebuggerSession(true);
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
                String command = "\n> Accept Failed : " + Exception.Message + "\n";
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();

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
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();
            }
        }

        delegate void dserver_OnSend(String SendStream);
        void server_OnSend(String SendStream)
        {
            if (this.InvokeRequired)
            {
                dserver_OnSend d = new dserver_OnSend(server_OnSend);
                object[] args = { SendStream };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\n> " + SendStream + "\n";
                this.CurrentCommandSend = SendStream;
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();
            }
        }

        delegate void dserver_OnReceive(string Stream, ASyncSocket AcceptedSocket);
        void server_OnReceive(string Stream, ASyncSocket AcceptedSocket)
        {
            if (this.InvokeRequired)
            {
                dserver_OnReceive d = new dserver_OnReceive(server_OnReceive);
                object[] args = { Stream, AcceptedSocket };
                this.Invoke(d, args);
            }
            else
            {
                this.CurrentCommandReceive = "";

                if (!this.CurrentCommandSend.Contains("RUN"))
                {
                    this.setHandLostWithSimulator(true);

                    //Receive All waiting message from the sockets.
                    this.CurrentCommandReceive = new String(Stream.ToCharArray());

                    //Analyse Receive Message
                    AnalyzeStatus(CurrentCommandReceive);

                    //Sync Watch and BreakPoints inside debugState
                    SyncDebuggerState();

                    //Print Receive Message
                    PrintReceiveMessageToConsole(CurrentCommandReceive);

                    //Send LOCALS AND BACKTRACE COMMANDS
                    SendAutomaticBackTraceAndLocalsCommand();
                }
                else
                {
                    this.CurrentCommandSend = "";

                    this.listCommands.Add("LOCALS");
                    this.listCommands.Add("BACKTRACE");
                    //Send LOCALS AND BACKTRACE COMMANDS
                    SendAutomaticBackTraceAndLocalsCommand();

                    
                }
            }

        }

        delegate void dserver_OnDisconnect(ASyncSocket AcceptedSocket);
        void server_OnDisconnect(ASyncSocket AcceptedSocket)
        {
            if (this.InvokeRequired)
            {
                dserver_OnDisconnect d = new dserver_OnDisconnect(server_OnDisconnect);
                object[] args = {AcceptedSocket };
                this.Invoke(d, args);
            }
            else
            {
                String command = "Simulator disconnected";
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();
                this.closeDebuggerSession(true);
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

                AcceptedSocket.OnReceive += new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                AcceptedSocket.OnSend += new ASyncSocket.OnSendEventHandler(server_OnSend);
                AcceptedSocket.OnDisconnect += new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);
                AcceptedSocket.OnReceiveFailed += new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                AcceptedSocket.OnSendFailed += new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);

                String command = "\n> Emulator Ready to receive order.\n";
                outPutTxtBx.AppendText(command);
                outPutTxtBx.ScrollToCaret();


                foreach (DocumentForm doc in this.mainForm.cgEeditor1.dockPanel.Documents)
                {
                    //--- Get all breakPoints
                    foreach (Line line in doc.Scintilla.Lines)
                    {
                        int mask = doc.Scintilla.Markers.GetMarkerMask(line);
                        if (mask != 0)
                        {
                            BreakPoint breakpoint = new BreakPoint(doc.Text, (line.Number + 1).ToString());
                            this.sendAddBreakPointCommand(breakpoint);
                        }

                    }
                }

            }
        }

        private void setHandLostWithSimulator(bool hasHand)
        {

            this.sendCommandToolStrip.Enabled = hasHand;
            this.runBt.Enabled = hasHand;
            this.stepBt.Enabled = hasHand;
            this.overBt.Enabled = hasHand;
            this.mainForm.cgEeditor1.Enabled = hasHand;
            this.splitContainer1.Enabled = hasHand;
            
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            closeDebuggerSession(true);
        }

        public void closeDebuggerSession(bool changeWorkspace)
        {
            if (AcceptedEmulator != null)
            {
               
                AcceptedEmulator.Send("EXIT\n");
                AcceptedEmulator.Disconnect();

                //AcceptedEmulator.OnReceive -= new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                //AcceptedEmulator.OnSend -= new ASyncSocket.OnSendEventHandler(server_OnSend);
                //AcceptedEmulator.OnDisconnect -= new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);
                //AcceptedEmulator.OnReceiveFailed -= new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                //AcceptedEmulator.OnSendFailed -= new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);

            }

            if (server != null)
            {
                server.OnAccept -= new ASyncSocket.OnAcceptEventHandler(server_OnAccept);
                server.OnReceive -= new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                server.OnSend -= new ASyncSocket.OnSendEventHandler(server_OnSend);
                server.OnListen -= new ASyncSocket.OnListenEventHandler(server_OnListen);
                server.OnAcceptFailed -= new ASyncSocket.OnAcceptFailedEventHandler(server_OnAcceptFailed);
                server.OnListenFailed -= new ASyncSocket.OnListenFailedEventHandler(server_OnListenFailed);
                server.OnReceiveFailed -= new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                server.OnSendFailed -= new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);
                server.StopListenSocket();
            }
                
            server = null;

            if (changeWorkspace == true)
            {
                this.closeAllTempFilesFromEditor();
                this.mainForm.cgEeditor1.setCurrentMode("EDITOR");
                this.mainForm.setWorkSpace("GLOBAL");
            }

            if (this.mainForm != null)
            {
                this.mainForm.cgEeditor1.SetDebuggerAtLine(-1);
                setHandLostWithSimulator(true);
                this.mainForm.workspaceViewToolStripMenuItem.Enabled = true;
            }
            
        }
    }
}
