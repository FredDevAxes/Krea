using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Krea.GameEditor;
using Krea.GameEditor.Panel_Jointures_Properties;
using Krea.CoronaClasses;
using Krea.GameEditor.TilesMapping;
using Krea.Corona_Classes.Widgets;
using Krea.GameEditor.Panel_Widgets;
using Krea.GameEditor.CollisionManager;
using System.Runtime.InteropServices;
using Utility.ModifyRegistry;
using System.Drawing.Drawing2D;
using Krea.Asset_Manager;
using Microsoft.VisualBasic;
using System.Reflection;
using Krea.GameEditor.FontManager;
using Krea.RemoteDebugger;

using Krea.Team;
using Krea.Corona_Classes;
using System.Collections.Specialized;


namespace Krea
{
    public partial class Form1 : Form
    {
        //------------------Attributs d'ui instances--------------
        public PhysicBodyEditorView CurrentObjectPhysicEditorPanel;
        public UserControl CurrentJointPanel;

        String substringDirectory;
       // public UndoRedo UndoRedo;
        public CoronaGameProject CurrentProject ;
        public CoronaObject CoronaObjectSelected;
        public Emulator CoronaEmulator;
        public string currentWorkerAction;
        private string loadingProjectFilename;
        public int ShapeType;
        public bool isFormLocked = false;
        public string directorySelectedDest;
        public string filenameSelected;


        public TargetResolution currentTargetResolution;
        public bool IsCustomBuild = false;
        private AssetManagerForm currentAssetManager;
       
        private RemoteControllerForm currentRemoteControllerForm;
        public bool IsDebugMode = false;
        public string CurrentWorkspace = "GLOBAL";


        private List<TabPage> tabPages;
        private StringDictionary tabPageLocationsDico;
        public Form1()
        {

            InitializeComponent();

          

            this.tabControlArea1.MouseDoubleClick += new MouseEventHandler(page_MouseDoubleClick);
            this.tabControlArea2.MouseDoubleClick += new MouseEventHandler(page_MouseDoubleClick);
            this.tabControlArea3.MouseDoubleClick += new MouseEventHandler(page_MouseDoubleClick);
            this.tabControlArea4.MouseDoubleClick += new MouseEventHandler(page_MouseDoubleClick);
            this.tabControlArea5.MouseDoubleClick += new MouseEventHandler(page_MouseDoubleClick);
            this.tabControlArea6.MouseDoubleClick += new MouseEventHandler(page_MouseDoubleClick);
        }

        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //    // Getting the graphics object
        //    Graphics g = pevent.Graphics;

        //    // Creating the rectangle for the gradient
        //    Rectangle rBackground = new Rectangle(0, 0,
        //                              this.Width, this.Height);

        //    // Creating the lineargradient
        //    Color colorGradient1 = Settings1.Default.Gradient1;
        //    Color colorGradient2 = Settings1.Default.Gradient2;
        //    LinearGradientMode direction = Settings1.Default.GradientDirection;
        //    LinearGradientBrush bBackground = new LinearGradientBrush(rBackground, colorGradient1, colorGradient2,direction);

        //    // Draw the gradient onto the form
        //    g.FillRectangle(bBackground, rBackground);

        //    // Disposing of the resources held by the brush
        //    bBackground.Dispose();
        //}

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }
      


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);
       

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }


        //---------------------------------------------------
        //-------------------Methodes-------------------
        //---------------------------------------------------

        private void recentProjectItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripItem itemSender = (ToolStripItem) sender;

                if (File.Exists(itemSender.Text) && itemSender.Text.EndsWith(".krp"))
                {
                    if (this.CurrentProject != null)
                    {

                        System.Windows.Forms.DialogResult result = MessageBox.Show("The current project needs to be closed before continuing !\n" +
                                          "Save ?", "A Project is already open !", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {

                            this.loadingProjectFilename = itemSender.Text;

                            if (this.mainBackWorker.IsBusy == true)
                                this.mainBackWorker.CancelAsync();

                            if (this.mainBackWorker.IsBusy == false)
                            {


                                //BUILD AND SAVE
                                this.closeAllTabPage();
                                this.currentWorkerAction = "ACTION_SAVE_LOAD";
                                this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                            }
                            return;

                        }
                        else if (result == System.Windows.Forms.DialogResult.No)
                        {
                            this.loadingProjectFilename = itemSender.Text;

                            if (this.mainBackWorker.IsBusy == true)
                                this.mainBackWorker.CancelAsync();

                            if (this.mainBackWorker.IsBusy == false)
                            {

                                this.clearCurrentProject();

                                //ONL
                                 this.closeAllTabPage(); 
                                this.currentWorkerAction = "ACTION_LOAD";
                                this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                            }
                        }
                        else if (result == System.Windows.Forms.DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                    {
                        this.loadingProjectFilename = itemSender.Text;

                        if (this.mainBackWorker.IsBusy == true)
                            this.mainBackWorker.CancelAsync();

                        if (this.mainBackWorker.IsBusy == false)
                        {
                            this.closeAllTabPage();
                            this.currentWorkerAction = "ACTION_LOAD";
                            this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The path \"" + itemSender.Text + "\" does not seem to exist anymore!", "Cannot open specified path", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.loadRecentProjectPathItems();
                }
                   
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error during loading recent project!\n"+ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void clearCurrentRecentProjectItems()
        {
            if(this.openRecentTooltStripMenuItem.DropDownItems.Count>0)
            {
                ToolStripItem[] items = new ToolStripItem[this.openRecentTooltStripMenuItem.DropDownItems.Count];
                this.openRecentTooltStripMenuItem.DropDownItems.CopyTo(items,0);

                for (int i = 0; i < items.Length; i++)
                {
                    ToolStripItem item = items[i];
                    item.Dispose();
                    item = null;
                }
                this.openRecentTooltStripMenuItem.DropDownItems.Clear();
            }
            

        }

        private void addRecentProjectPath(string path)
        {
            if (File.Exists(path))
            {
                if (!Settings1.Default.RecentProjectPaths.Contains(path))
                {
                    Settings1.Default.RecentProjectPaths.Insert(0, path);
                    if (Settings1.Default.RecentProjectPaths.Count > 10)
                        Settings1.Default.RecentProjectPaths.RemoveAt(10);

                    Settings1.Default.Save();
                   
                }

                
            }
        }
        private void loadRecentProjectPathItems()
        {
            clearCurrentRecentProjectItems();

            //Load recent projects
            if (Settings1.Default.RecentProjectPaths != null)
            {
                string[] paths = new string[Settings1.Default.RecentProjectPaths.Count];
                Settings1.Default.RecentProjectPaths.CopyTo(paths, 0);

                for (int i = 0; i < paths.Length; i++)
                {
                    string projectPath = paths[i];
                    if (File.Exists(projectPath))
                    {
                        ToolStripMenuItem newItem = new ToolStripMenuItem(projectPath);
                        newItem.Click += new EventHandler(recentProjectItem_Click);
                        this.openRecentTooltStripMenuItem.DropDownItems.Add(newItem);
                    }
                    else
                    {
                        Settings1.Default.RecentProjectPaths.Remove(projectPath);

                    }
                }
            }
            else
            {
                Settings1.Default.RecentProjectPaths = new System.Collections.Specialized.StringCollection();
                Settings1.Default.Save();
            }
        }

        public void Init()
        {


             loadRecentProjectPathItems();
             this.gameElementTreeView1.init(this);
             this.fileExplorer1.Init(this);

             this.imageObjectsPanel1.init(this);
             this.cgEeditor1.Init(this);
             this.sceneEditorView1.init(this);
             this.sceneEditorView1.UniqueInit();

             this.tilesManagerPanel1.init(this);
             this.scenePreview1.init(this);
             this.coronaAPIPanel1.init(this);
            
            //Init property grid toolbar
             ToolStrip mobjToolbar =

                    (ToolStrip)typeof(PropertyGrid).InvokeMember("toolStrip",

                                                        BindingFlags.GetField | BindingFlags.NonPublic |

                                                        BindingFlags.Instance,

                                                        null,

                                                        propertyGrid1,

                                                        null);

             ToolStripItem expandBt = mobjToolbar.Items.Add("Expand All",Properties.Resources.flecheBasIcon, expandAllPropertyGridCategories);
             expandBt.DisplayStyle = ToolStripItemDisplayStyle.Image;
             ToolStripItem collapseBt = mobjToolbar.Items.Add("Collapse All", Properties.Resources.flecheHautIcon, collapseAllPropertyGridCategories);
             collapseBt.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //------

             this.ShapeType = 0;

             this.graduationBarY.setDefaultOffset(this.graduationBarX.Size.Height);
             currentWorkerAction = "NONE";
            //Retirer les page inutiles

             refreshTabPagesLocation();

             closeAllTabPage();

           

             if (Settings1.Default.RemoteControlEnabled == false)
                 this.remoteModeCmbBx.SelectedIndex = 0;
             else if (Settings1.Default.RemoteControlEnabled == true)
                 this.remoteModeCmbBx.SelectedIndex = 1;

             //Resolutions
             this.initResolutions();
           
             
        }

        public void closeAllTabPage()
        {
            if (this.collisionManagerPage.Parent != null)
                this.collisionManagerPage.Parent.Controls.Remove(this.collisionManagerPage);

            if (this.languageManagerPage.Parent != null)
                this.languageManagerPage.Parent.Controls.Remove(this.languageManagerPage);

            if (this.physicsBodyManagerPage.Parent != null)
                this.physicsBodyManagerPage.Parent.Controls.Remove(this.physicsBodyManagerPage);

            if (this.projectSettingsPage.Parent != null)
                this.projectSettingsPage.Parent.Controls.Remove(this.projectSettingsPage);

            if (this.tileMapEditorPage.Parent != null)
                this.tileMapEditorPage.Parent.Controls.Remove(this.tileMapEditorPage);

            if (this.userSettingsPage.Parent != null)
                this.userSettingsPage.Parent.Controls.Remove(this.userSettingsPage);

            if (this.jointManagerPage.Parent != null)
                this.jointManagerPage.Parent.Controls.Remove(this.jointManagerPage);

            if (this.debuggerPage.Parent != null)
                this.debuggerPage.Parent.Controls.Remove(this.debuggerPage);
           
            //this.tabControlArea2.Controls.Remove(this.collisionManagerPage);
            //this.tabControlArea2.Controls.Remove(this.languageManagerPage);
            //this.tabControlArea2.Controls.Remove(this.physicsBodyManagerPage);
            //this.tabControlArea2.Controls.Remove(this.projectSettingsPage);
            //this.tabControlArea2.Controls.Remove(this.tileMapEditorPage);
            //this.tabControlArea2.Controls.Remove(this.userSettingsPage);

            //this.tabControlArea5.Controls.Remove(this.jointManagerPage);
            //this.tabControlArea5.Controls.Remove(this.debuggerPage);

            this.checkAllTabControlEmpty();
        }

        public void expandAllPropertyGridCategories(object sender, System.EventArgs e)
        {
            if (this.propertyGrid1.SelectedObjects != null)
                this.propertyGrid1.ExpandAllGridItems();

        }

        private void collapseAllPropertyGridCategories(object sender, System.EventArgs e)
        {
            if (this.propertyGrid1.SelectedObjects != null)
                this.propertyGrid1.CollapseAllGridItems();

        }

        public void setShowGrid(bool show)
        {
            Settings1.Default.ShowGrid = show;
            this.showGridBt.Checked = show;

            GorgonLibrary.Gorgon.Go();
        }

        public void initResolutions()
        {
            this.resolutionsCmbBx.Items.Clear();

            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);


            string filename = documentsDirectory + "\\resolutions.txt";
            if (File.Exists(filename))
            {
                List<string> listRes = new List<string>(File.ReadAllLines(filename));
            

                //Check for basic corona presets resolutions (NEWS)
                string[] newPresets = new string[14];
                newPresets[0] = "iPhone : 320x480";
                newPresets[1] = "iPhone 4 : 640x960";
                newPresets[2] = "iPad : 768x1024";
                newPresets[3] = "Moto Droid : 480x854";
                newPresets[4] = "Galaxy Tab : 600x1024";
                newPresets[5] = "Nexus One : 480x800";
                newPresets[6] = "Htc Sensation : 540x960";
                newPresets[7] = "Kindle Fire : 600x1024";
                newPresets[8] = "Nook Color : 600x1024";

                newPresets[9] = "iPhone 5 : 640x1136";
                newPresets[10] = "Galaxy S3 : 720x1280";
                newPresets[11] = "iPad Retina : 1536x2048";
                newPresets[12] = "Kindle Fire HD 7: 800x1280";
                newPresets[13] = "Kindle Fire HD 9: 1200x1920";

                for (int i = 0; i < newPresets.Length; i++)
                {
                    if (!listRes.Contains(newPresets[i]))
                        listRes.Add(newPresets[i]);
                }

                for (int i = 0; i < listRes.Count; i++)
                {
                    string str = listRes[i];
                    if(!str.Equals("") && !str.Contains("\0") )
                    {
                        string[] splittedStr = str.Split(':');
                        if(splittedStr.Length<2) continue;

                        string targetDevice = splittedStr[0];
                        string resolution = splittedStr[1];
                        int width = 0;
                        int height = 0;
                        int.TryParse((resolution.Split('x'))[0],out width);
                        int.TryParse((resolution.Split('x'))[1],out height);
                        if(width >0 && height >0) 
                        {
                            TargetResolution res = new TargetResolution(targetDevice, new Size(width, height));
                            this.resolutionsCmbBx.Items.Add(res);
                        }
                         
                    }
                   
                }

                if(this.resolutionsCmbBx.Items.Count>0)
                    this.resolutionsCmbBx.SelectedIndex =0;
            }
            else
            {
                string[] lines = new string[15];
                lines[0] = "iPhone : 320x480";
                lines[1] = "iPhone 4 : 640x960";
                lines[2] = "iPad : 768x1024";
                lines[3] = "Moto Droid : 480x854";
                lines[4] = "Galaxy Tab : 600x1024";
                lines[5] = "Nexus One : 480x800";
                lines[6] = "Htc Sensation : 540x960";
                lines[8] = "Kindle Fire : 600x1024";
                lines[9] = "Nook Color : 600x1024";

                lines[10] = "iPhone 5 : 640x1136";
                lines[11] = "Galaxy S3 : 720x1280";
                lines[12] = "iPad Retina : 1536x2048";
                lines[13] = "Kindle Fire HD 7: 800x1280";
                lines[14] = "Kindle Fire HD 9: 1200x1920";
                File.WriteAllLines(filename, lines);
                initResolutions();
            }
        }

        public void saveResolutions()
        {
          
            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);

            string filename = documentsDirectory + "\\resolutions.txt";

             if (File.Exists(filename))
                 File.Delete(filename);

             string[] lines = new string[this.resolutionsCmbBx.Items.Count];
            for (int i = 0; i < this.resolutionsCmbBx.Items.Count; i++)
            {
                TargetResolution res = (TargetResolution)this.resolutionsCmbBx.Items[i];
                lines[i] = res.ToString();
     
            }

            File.WriteAllLines(filename, lines);


        }
        public void PopulateTreeView(string directoryValue, TreeNode parentNode)
        {
            string[] directoryArray =
             Directory.GetDirectories(directoryValue);

            try
            {
                if (directoryArray.Length != 0)
                {
                    foreach (string directory in directoryArray)
                    {
                        substringDirectory = directory.Substring(
                        directory.LastIndexOf('\\') + 1,
                        directory.Length - directory.LastIndexOf('\\') - 1);

                        TreeNode myNode = new TreeNode(substringDirectory);

                        parentNode.Nodes.Add(myNode);

                        PopulateTreeView(directory, myNode);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                parentNode.Nodes.Add("Access denied");
            } // end catch
        }

        public Scene addSceneToProject(string sceneName)
        {
            if (CurrentProject != null)
            {

                Scene scene1 = new Scene(new Size(2000,2000), this.CurrentProject.Orientation,this.CurrentProject);
                if (sceneName != null && !sceneName.Equals(""))
                    scene1.Name = sceneName.ToLower();
                else
                    scene1.Name = "scene" + this.CurrentProject.Scenes.Count;

                scene1.Name = this.CurrentProject.IncrementObjectName(scene1.Name);
                scene1.createLuaFile();

                //Ouvrir le fichier dans l'editeur de code
                this.cgEeditor1.OpenFileInEditor(this.CurrentProject.SourceFolderPath + "\\" + scene1.Name + ".lua");

                scene1.CurrentSceneViewLocation = new Point(scene1.Camera.SurfaceFocus.Location.X - (this.sceneEditorView1.surfacePictBx.Width / 3),
                    scene1.Camera.SurfaceFocus.Location.Y - (this.sceneEditorView1.surfacePictBx.Height / 3));
                this.gameElementTreeView1.newScene(scene1);
                this.CurrentProject.Scenes.Add(scene1);

                this.newLayerBt_Click(null, null);

                
                return scene1;
            }
            return null;
        }

        public void addAudioToProject(AudioObject audio)
        {
            if (CurrentProject != null)
            {
                this.gameElementTreeView1.newAudioObject(audio);
                this.CurrentProject.AudioObjects.Add(audio);

            }

        }
       

        public static string ExecuteCmd(string arguments)
        {
            // Create the Process Info object with the overloaded constructor
            // This takes in two parameters, the program to start and the
            // command line arguments.
            // The arguments parm is prefixed with "@" to eliminate the need
            // to escape special characters (i.e. backslashes) in the
            // arguments string and has "/C" prior to the command to tell
            // the process to execute the command quickly without feedback.
            ProcessStartInfo _info =
                new ProcessStartInfo("cmd", @"/C " + arguments);

            // The following commands are needed to redirect the
            // standard output.  This means that it will be redirected
            // to the Process.StandardOutput StreamReader.
            _info.RedirectStandardOutput = true;

            // Set UseShellExecute to false.  This tells the process to run
            // as a child of the invoking program, instead of on its own.
            // This allows us to intercept and redirect the standard output.
            _info.UseShellExecute = false;

            // Set CreateNoWindow to true, to supress the creation of
            // a new window
            _info.CreateNoWindow = true;

            // Create a process, assign its ProcessStartInfo and start it
            Process _p = new Process();
            _p.StartInfo = _info;
            _p.Start();

            // Capture the results in a string
            string _processResults = _p.StandardOutput.ReadToEnd();

            // Close the process to release system resources
            _p.Close();

            // Return the output stream to the caller
            return _processResults;
        }




     


        //---------------------------------------------------
        //-------------------Accesseurs-------------------
        //---------------------------------------------------

        public GameElementTreeView getElementTreeView()
        {
            return this.gameElementTreeView1;
        }

        public TabPage getMapEditorPage()
        {
            return this.sceneEditorPage;
        }

        public TabControl getGameEditorTabControl()
        {
            return this.tabControlArea2;
        }

        public TabPage getCollisionMaskTabPage()
        {
            return this.collisionManagerPage;
        }

        public CollisionManager getCollisionMask()
        {
            return this.collisionManager1;
        }

        public TabPage getRosetaPage()
        {
            return this.languageManagerPage;
        }
        ///---------------------------------------------------------------------------------------------------------
        ///---------------------------------------------------------------------------------------------------------
        //-------------------Events-------------------
        ///---------------------------------------------------------------------------------------------------------
        ///---------------------------------------------------------------------------------------------------------

        //DELETE
        public void removeObjectsSelected_Click(object sender, EventArgs e)
        {
          
                List<CoronaObject> listObj = this.sceneEditorView1.objectsSelected;
                if (listObj != null)
                {
                    for (int i = 0; i < listObj.Count; i++)
                    {
                        this.gameElementTreeView1.removeCoronaObject(listObj[i]);
                    }

                    listObj.Clear();
                }

        }

     

        private void zoomIn_Click(object sender, EventArgs e)
        {
            if (this.gameElementTreeView1.SceneSelected != null)
            {
                this.sceneEditorView1.zoomAvant();
                this.graduationBarX.setScale(this.sceneEditorView1.CurrentScale);
                this.graduationBarY.setScale(this.sceneEditorView1.CurrentScale);

                this.sceneEditorView1.RefreshScrollView();
            }
        }

        private void zoomBackBt_Click(object sender, EventArgs e)
        {
            if (this.gameElementTreeView1.SceneSelected != null)
            {
                this.sceneEditorView1.zoomArriere();
                this.graduationBarX.setScale(this.sceneEditorView1.CurrentScale);
                this.graduationBarY.setScale(this.sceneEditorView1.CurrentScale);

                this.sceneEditorView1.RefreshScrollView();
            }
               
            

        }

        private void startEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Try to build
            if (this.CurrentProject != null)
            {

                Emulator.Instance.Dispose();

                this.IsCustomBuild = false;
                string filePathSelectedInEditor = "";
               
                if (this.cgEeditor1.ActiveDocument != null)
                {
                    filePathSelectedInEditor = this.cgEeditor1.ActiveDocument.FilePath;
                }

                //Refresh sceneLuaCodes
                for (int i = 0; i < this.CurrentProject.Scenes.Count; i++)
                {
                    Scene scene = this.CurrentProject.Scenes[i];
                    this.cgEeditor1.RefreshSceneLuaCode(scene,false,1,1);
                }

                if (filePathSelectedInEditor != null && !filePathSelectedInEditor.Equals(""))
                    this.cgEeditor1.OpenFileInEditor(filePathSelectedInEditor);

               

                if (this.mainBackWorker.IsBusy == true)
                    this.mainBackWorker.CancelAsync();

                bool res = this.cgEeditor1.CheckLuaSyntaxBeforeBuild();
                if (res == false)
                    return;

                if (this.mainBackWorker.IsBusy == false)
                {
                   // this.Enabled = false;
                    this.isFormLocked = true;
                    this.currentWorkerAction = "ACTION_BUILD_PLAY";
                    this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                }

               

            }
           
        }

        private void customBuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Try to build
            if (this.CurrentProject != null)
            {
                Emulator.Instance.Dispose();

                this.IsCustomBuild = true;

                float XRatio = 0;
                float YRatio = 0;
                if (this.CurrentProject.Orientation == CoronaGameProject.OrientationScreen.Portrait)
                {
                    XRatio = (float)this.currentTargetResolution.Resolution.Width / (float)this.CurrentProject.width;
                    YRatio = (float)this.currentTargetResolution.Resolution.Height / (float)this.CurrentProject.height;
                }
                else
                {
                    YRatio = (float)this.currentTargetResolution.Resolution.Width / (float)this.CurrentProject.width;
                    XRatio = (float)this.currentTargetResolution.Resolution.Height / (float)this.CurrentProject.height;
                }
               
                //Refresh sceneLuaCodes
                string filePathSelectedInEditor = "";
                if (this.cgEeditor1.ActiveDocument != null)
                {
                    filePathSelectedInEditor = this.cgEeditor1.ActiveDocument.FilePath;
                }

                for (int i = 0; i < this.CurrentProject.Scenes.Count; i++)
                {
                    Scene scene = this.CurrentProject.Scenes[i];
                    this.cgEeditor1.RefreshSceneLuaCode(scene, true, XRatio, YRatio);
                }

                if(!filePathSelectedInEditor.Equals(""))
                    this.cgEeditor1.OpenFileInEditor(filePathSelectedInEditor);


                if (this.mainBackWorker.IsBusy == true)
                    this.mainBackWorker.CancelAsync();

                if (this.mainBackWorker.IsBusy == false)
                {
                    //this.Enabled = false;
                    this.isFormLocked = true;
                    string device = this.currentTargetResolution.TargetDevice.Replace(" ", "");
                    ModifyRegistry myRegistry = new ModifyRegistry();
                    myRegistry.SubKey = "Software\\Ansca Corona\\Corona Simulator\\Preferences";
                    myRegistry.ShowError = true;

                    myRegistry.Write("Device", device);

                    if(this.CurrentProject.Orientation == CoronaGameProject.OrientationScreen.Landscape)
                    {

                        //regKey.SetValue("Zoom", unchecked((int)0xf0000000u), Re.DWord);
                                                                                


                        //string hex = "fffffffe";
                        //var result = new byte[hex.Length/2];
                        //for (int i = 0; i < hex.Length; i += 2)
                        //{
                        //    result[i / 2] = byte.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                        //}

                        myRegistry.Write("Zoom", unchecked((int)0xfffffffdu));
                    }
                        
                   
                    
                    this.currentWorkerAction = "ACTION_BUILD_PLAY";
                    this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                }



            }
        }

       

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            checkAndOpenNewProjectPanel();
        }

        public void checkAndOpenNewProjectPanel()
        {
            if (this.CurrentProject != null)
            {
                DialogResult res = MessageBox.Show("Do you want to save the current project ?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    this.saveProject(true);
                    return;
                }
                else if (res == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else
                {
                    this.clearCurrentProject();
                    this.openNewProjectPanel();
                    return;
                }
            }

            this.openNewProjectPanel();
        }
        public void openNewProjectPanel()
        {
            this.projectSettings1.init(this);
           
            if (this.projectSettingsPage.Parent == null)
            {
                this.checkAndOpenTabPage(this.projectSettingsPage);
            }

            TabControl controlParent = this.projectSettingsPage.Parent as TabControl;

            controlParent.SelectedTab = this.projectSettingsPage;

            this.checkAllTabControlEmpty();

            controlParent.Refresh();
        }

        public void closeProjectPage()
        {

            TabControl controlParent = this.projectSettingsPage.Parent as TabControl;
            if (controlParent != null)
            {
                controlParent.TabPages.Remove(this.projectSettingsPage);
            }

            this.checkAllTabControlEmpty();

            if(controlParent != null)
            if (controlParent.TabPages.Count > 0)
                controlParent.SelectedTab = controlParent.TabPages[0];
        }


       
        



        
        ///---------------------------------------------------------------------------------------------------------
        ///--------------------- METHODES ET EVENTS Collision MANAGER-------------------------------
        ///---------------------------------------------------------------------------------------------------------
        private void closeCollisionMaskManagerBt_Click(object sender, EventArgs e)
        {

            TabControl controlParent = this.collisionManagerPage.Parent as TabControl;
            if (controlParent != null)
            {
                controlParent.TabPages.Remove(this.collisionManagerPage);

                if (controlParent.TabPages.Count > 0)
                    controlParent.SelectedTab = controlParent.TabPages[0];
            }

            this.checkAllTabControlEmpty();
        }

        private void collisionningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.gameElementTreeView1.ProjectSelected != null)
            {
                if (this.gameElementTreeView1.SceneSelected != null)
                {

                    if (this.collisionManagerPage.Parent == null)
                    {
                        this.checkAndOpenTabPage(this.collisionManagerPage);
                    }

                    TabControl controlParent = this.collisionManagerPage.Parent as TabControl;
                    controlParent.SelectedTab = this.collisionManagerPage;

                    this.checkAllTabControlEmpty();
                    
                    this.collisionManager1.Init(this.gameElementTreeView1.SceneSelected);
                    
                }
                else
                    MessageBox.Show("No stage selected !\n Please select a stage first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("No project loaded !\n Please create/load a project first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        ///---------------------------------------------------------------------------------------------------------
        ///--------------------- METHODES ET EVENTS LANGAGE MANAGER-------------------------------
        ///---------------------------------------------------------------------------------------------------------
        public void initLangageManager()
        {
            if (this.gameElementTreeView1.ProjectSelected != null)
            {
                if (this.gameElementTreeView1.SceneSelected != null)
                {

                    if (this.languageManagerPage.Parent == null)
                    {
                        this.checkAndOpenTabPage(this.languageManagerPage);
                    }

                    TabControl controlParent = this.languageManagerPage.Parent as TabControl;
                    controlParent.SelectedTab = this.languageManagerPage;

                    this.checkAllTabControlEmpty();

                    this.languageManager1.Init(this, this.CurrentProject);

                  

                }
                else
                    MessageBox.Show("No stage selected !\n Please select a stage first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("No project loaded !\n Please create/load a project first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void languageManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.initLangageManager();
           
        }


        ///---------------------------------------------------------------------------------------------------------
        ///--------------------- METHODES ET EVENTS TILESMAP MANAGER-------------------------------
        ///---------------------------------------------------------------------------------------------------------
        public void openTilesMapEditor(TilesMap map)
        {
            //Init le manager
            this.tilesManagerPanel1.setCurrentTilesMap(map);

            this.tilesMapEditor1.init(this);
            this.tilesMapEditor1.setTilesMap(map);

            if (this.tileMapEditorPage.Parent == null)
            {
                this.checkAndOpenTabPage(this.tileMapEditorPage);
            }

            TabControl controlParent = this.tileMapEditorPage.Parent as TabControl;
            controlParent.SelectedTab = this.tileMapEditorPage;

            this.checkAllTabControlEmpty();

            //------
            bool exists = false;
            for (int i = 0; i < this.sceneEditorView1.GraphicsContentManager.RendererWindows.Count; i++)
            {
                if (this.sceneEditorView1.GraphicsContentManager.RendererWindows[i].Name.Equals("TileMapEditor"))
                {
                    exists = true;
                    break;
                }
            }
            if (exists == false)
                this.sceneEditorView1.GraphicsContentManager.AddRenderWindow(
                 new GorgonLibrary.Graphics.RenderWindow("TileMapEditor", this.tilesMapEditor1.surfaceDessin, false));
            //------
            exists = false;
            for (int i = 0; i < this.sceneEditorView1.GraphicsContentManager.RendererWindows.Count; i++)
            {
                if (this.sceneEditorView1.GraphicsContentManager.RendererWindows[i].Name.Equals("TileMapEditor_TextureModels"))
                {
                    exists = true;
                    break;
                }
            }
            if (exists == false)
                this.sceneEditorView1.GraphicsContentManager.AddRenderWindow(
                 new GorgonLibrary.Graphics.RenderWindow("TileMapEditor_TextureModels", this.tilesMapEditor1.textureModelsPictBx, false));

            //------
            exists = false;
            for (int i = 0; i < this.sceneEditorView1.GraphicsContentManager.RendererWindows.Count; i++)
            {
                if (this.sceneEditorView1.GraphicsContentManager.RendererWindows[i].Name.Equals("TileMapEditor_ObjectModels"))
                {
                    exists = true;
                    break;
                }
            }
            if (exists == false)
                this.sceneEditorView1.GraphicsContentManager.AddRenderWindow(
                 new GorgonLibrary.Graphics.RenderWindow("TileMapEditor_ObjectModels", this.tilesMapEditor1.objectModelsPictBx, false));


         


        }

        public void closeTilesMapEditor()
        {
            //Init le manager
            this.tilesManagerPanel1.setCurrentTilesMap(null);

            this.tilesMapEditor1.init(this);
            this.tilesMapEditor1.setTilesMap(null);

            TabControl controlParent = this.tileMapEditorPage.Parent as TabControl;
            if (controlParent != null)
            {
                controlParent.TabPages.Remove(this.tileMapEditorPage);

                
                if (controlParent.TabPages.Count > 0)
                    controlParent.SelectedTab = controlParent.TabPages[0];
            }

            this.checkAllTabControlEmpty();

        }

        ///---------------------------------------------------------------------------------------------------------
        ///---------------------------------------------------------------------------------------------------------
        ///--------------------- METHODES ET EVENTS PHYSIC BODY MANAGER -------------------------------
        ///---------------------------------------------------------------------------------------------------------
        ///---------------------------------------------------------------------------------------------------------

        public void initPhysicBodyManager(CoronaObject obj)
        {
           

            this.physicBodyEditorView1.init(this, obj);
            this.physicsBodySettings1.init(this, obj);

            if (!this.tabControlArea2.Controls.Contains(this.physicsBodyManagerPage))
                this.tabControlArea2.Controls.Add(this.physicsBodyManagerPage);

            bool exists = false;
            for (int i = 0; i < this.sceneEditorView1.GraphicsContentManager.RendererWindows.Count; i++)
            {
                if (this.sceneEditorView1.GraphicsContentManager.RendererWindows[i].Name.Equals("PhysicsBodyEditor"))
                {
                    exists = true;
                    break;
                }
            }
            if(exists == false)
                this.sceneEditorView1.GraphicsContentManager.AddRenderWindow(
                 new GorgonLibrary.Graphics.RenderWindow("PhysicsBodyEditor", this.physicBodyEditorView1.surfacePictBx, false));

            this.tabControlArea2.SelectedTab = this.physicsBodyManagerPage;

            this.setWorkSpace("GLOBAL");
        }


        private void newShapeBt_Click(object sender, EventArgs e)
        {
            this.physicBodyEditorView1.newShape();
        }

        private void newCircleBodyBt_Click(object sender, EventArgs e)
        {
            this.physicBodyEditorView1.newCircle();
        }


        private void validBodyBt_Click(object sender, EventArgs e)
        {
            this.physicsBodySettings1.validerBodyShape();
        }

        private void newHandShape_Click(object sender, EventArgs e)
        {
            this.physicBodyEditorView1.newHandShape();
        }


        ///---------------------------------------------------------------------------------------------------------
        ///---------------------------------------------------------------------------------------------------------
        ///---------------------GEsTION DE  CREATION DE JOINTURES -------------------------------
        ///---------------------------------------------------------------------------------------------------------
        ///---------------------------------------------------------------------------------------------------------


        ///---------------------------------------------------------------------------------------------------------
        // ------------  SERIALISATION DU PROJET EN COURs-------------
        //---------------------------------------------------
        //-------------------Methodes-------------------
        ///---------------------------------------------------------------------------------------------------------
        public void clearCurrentProject()
        {
            this.gameElementTreeView1.clearTreeView();

            this.cgEeditor1.closeAll(false);

            if(this.CurrentProject != null)
            {
                this.sceneEditorView1.GraphicsContentManager.CleanProjectGraphics(false, false);
                this.CurrentProject.clearProject();
            }
            
            this.CurrentProject = null;
            this.sceneEditorView1.CurentCalque = "NONE";

            this.CoronaObjectSelected = null;
            this.propertyGrid1.SelectedObject = null;

            if (this.isFormLocked == false)
            {
                this.scenePreview1.Refresh();
                GorgonLibrary.Gorgon.Go();
            }
          
        }

       

        public void loadProject(string filename, BackgroundWorker worker)
        {
            worker.ReportProgress(10);

            //DialogResult rs = MessageBox.Show("Do you want to create a project back up before continuing ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (rs == System.Windows.Forms.DialogResult.Yes)
            //{
            //    //Make a backUp
            //    File.Copy(filename, filename.Replace(".krp", "_backup.krp"),true);
            //}

        
            //do back up up
            if(File.Exists(filename))
                File.Copy(filename, filename.Replace(".krp", "_backup.krp"),true);

            FileStream fs = File.OpenRead(filename);
            if (fs.Length > 0)
            {
                MemoryStream ms = new MemoryStream();
                ms.SetLength(fs.Length);

                fs.Read(ms.GetBuffer(), 0, (int)ms.Length);

                this.CurrentProject = (CoronaGameProject)SerializerHelper.DeSerializeBinary(ms);
                ms.Close();
                ms.Dispose();
               
                
                
                this.CurrentProject.updateBuildFields();
                this.CurrentProject.updateConfigFields(1,1);

                this.CurrentProject.BuildFolderPath = filename.Substring(0, filename.LastIndexOf("\\"))+"\\"+ this.CurrentProject.ProjectName;
                this.CurrentProject.ProjectPath = filename.Substring(0, filename.LastIndexOf("\\"));
                this.CurrentProject.SourceFolderPath = filename.Substring(0, filename.LastIndexOf("\\")) + "\\Sources";
                this.CurrentProject.CgeProjectFilename = this.CurrentProject.ProjectPath + "\\" + this.CurrentProject.ProjectName + ".krp";

                this.sceneEditorView1.GraphicsContentManager.SetCurrentProject(this.CurrentProject, this.sceneEditorView1.CurrentScale, Point.Empty);

                this.debuggerPanel1.Init(this.CurrentProject, this);
                worker.ReportProgress(40);
                if (this.CurrentProject != null)
                {

                    worker.ReportProgress(70);

                    //Recharger toutes les tilesMap existantes
                    //Create all files needed for tilesmaps
                    for (int i = 0; i < this.CurrentProject.Scenes.Count; i++)
                    {
                        Scene scene = this.CurrentProject.Scenes[i];
                        scene.projectParent = this.CurrentProject;


                        for (int j = 0; j < scene.Layers.Count; j++)
                        {
                            CoronaLayer layer = scene.Layers[j];

                            if (layer.TilesMap != null)
                            {
                                layer.TilesMap.reloadMapContent(CurrentProject.SourceFolderPath);
                                //layer.TilesMap.reloadFromFile(this.CurrentProject.SourceFolderPath);
                            }
                        }

                        
                    }

                    //Gerer les fonts

                    if (this.CurrentProject.AvailableFont == null)
                        this.CurrentProject.AvailableFont = new List<FontItem>();


                }
            }
            fs.Close();
            fs.Dispose();
            fs = null;
            //this.UndoRedo.clearBuffers();
        }
 
        public void saveProject(bool closeAfter)
        {
            try
            {
                if (this.CurrentProject == null)
                {
                    MessageBox.Show("No Project open !\n", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                
                 if (this.mainBackWorker.IsBusy == true)
                        this.mainBackWorker.CancelAsync();

                 if (this.mainBackWorker.IsBusy == false)
                 {
                     this.isFormLocked = true;
                     if (closeAfter == true)
                     {
                         //BUILD AND SAVE AND CLOSE
                         this.currentWorkerAction = "ACTION_BUILD_SAVE_CLEAN";
                     }
                     else
                     {
                         if (closeAfter == false)
                         {
                             //BUILD AND SAVE ONLY
                             this.currentWorkerAction = "ACTION_BUILD_SAVE";
                         }
                     }
                    
                     this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                 }

            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR DURING PROJECT SAVING !\n"+ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }

        public void checkAndOpenNewExistingProject()
        {
            if (this.CurrentProject != null)
            {

                System.Windows.Forms.DialogResult result = MessageBox.Show("The current project needs to be closed before continuing !\n" +
                                  "Save ?", "A Project is already open !", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    OpenFileDialog openF = new OpenFileDialog();
                    openF.Filter = "Krea Project Files (*.krp)|*.krp";
                    if (openF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (openF.FileName.EndsWith(".krp"))
                        {

                            this.loadingProjectFilename = openF.FileName;

                            if (this.mainBackWorker.IsBusy == true)
                                this.mainBackWorker.CancelAsync();

                            if (this.mainBackWorker.IsBusy == false)
                            {


                                //BUILD AND SAVE
                                this.closeAllTabPage();
                                this.currentWorkerAction = "ACTION_SAVE_LOAD";
                                this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                            }
                            return;
                        }
                    }
                }
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog openF2 = new OpenFileDialog();
            openF2.Filter = "Krea Project Files (*.krp)|*.krp";
            if (openF2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openF2.FileName.EndsWith(".krp"))
                {

                    this.loadingProjectFilename = openF2.FileName;

                    if (this.mainBackWorker.IsBusy == true)
                        this.mainBackWorker.CancelAsync();

                    if (this.mainBackWorker.IsBusy == false)
                    {

                        this.clearCurrentProject();

                        this.closeAllTabPage();
                        //ONLY 
                        this.currentWorkerAction = "ACTION_LOAD";
                        this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                    }
                }
                else
                {
                    MessageBox.Show("Only KRP files can be opened!", "File Format Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.closeProjectPage();
            checkAndOpenNewExistingProject();
        }

        private void SaveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (this.CurrentProject != null)
            {
                 this.saveProject(false);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentProject != null)
            {
                SaveFileDialog saveDial = new SaveFileDialog();
                saveDial.Filter = "Krea Project Files (*.krp)|*.krp";
                if (saveDial.ShowDialog() == DialogResult.OK)
                {
                    this.CurrentProject.CgeProjectFilename = saveDial.FileName;
                    this.saveProject(false);
                }
            }
        }



        private void openAssetManager_Click(object sender, EventArgs e)
        {
            //Create a form 
            if(currentAssetManager == null)
                currentAssetManager = new AssetManagerForm();

            DialogResult dr = currentAssetManager.ShowDialog(this);
            this.refreshAssetsProjectsBt_Click(null, null);
        }

        private void sceneEditorView1_MouseEnter(object sender, EventArgs e)
        {
            this.sceneEditorView1.Focus();

        }

       
        private void showHideCamera_Click(object sender, EventArgs e)
        {
            if (this.gameElementTreeView1.SceneSelected != null)
            {
                this.gameElementTreeView1.SceneSelected.Camera.isSurfaceFocusVisible = !this.gameElementTreeView1.SceneSelected.Camera.isSurfaceFocusVisible;

                if(this.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();
                
            }
        }

        //------------------------------------------------------------------------------
        //---------------------CREATION DES JOINTURES---------------------------------------
        //-----------------------------------------------------------------------------------
        public void openPanelJointure(String nameJoint, CoronaJointure joint)
        {
            //Fermer un panel si existant

            //if (!this.tabControlArea5.Controls.Contains(this.jointManagerPage))
            //    this.tabControlArea5.Controls.Add(this.jointManagerPage);

            if (this.jointManagerPage.Parent == null)
            {
                this.checkAndOpenTabPage(this.jointManagerPage);
            }

            TabControl controlParent = this.jointManagerPage.Parent as TabControl;
            controlParent.SelectedTab = this.jointManagerPage;

            this.checkAllTabControlEmpty();

            if (nameJoint.Equals("PIVOT"))
            {
                PivotPropertiesPanel pivotPanel = new PivotPropertiesPanel();
                pivotPanel.init(this, joint);

                this.CurrentJointPanel = pivotPanel;
                this.jointManagerPage.Controls.Add(pivotPanel);
            }
            else if (nameJoint.Equals("DISTANCE"))
            {
                DistancePropertiesPanel distancePanel = new DistancePropertiesPanel();
                distancePanel.init(this, joint);

                this.CurrentJointPanel = distancePanel;
                this.jointManagerPage.Controls.Add(distancePanel);
            }
            else if (nameJoint.Equals("FRICTION"))
            {
                FrictionPropertiesPanel panel = new FrictionPropertiesPanel();
                panel.Init(this, joint);

                this.CurrentJointPanel = panel;
                this.jointManagerPage.Controls.Add(panel);
            }
            else if (nameJoint.Equals("PISTON"))
            {
                PistonPropertiesPanel panel = new PistonPropertiesPanel();
                panel.init(this, joint);

                this.CurrentJointPanel = panel;
                this.jointManagerPage.Controls.Add(panel);
            }
            else if (nameJoint.Equals("PULLEY"))
            {
                PulleyPropertiesPanel panel = new PulleyPropertiesPanel();
                panel.Init(this, joint);

                this.CurrentJointPanel = panel;
                this.jointManagerPage.Controls.Add(panel);
            }
            else if (nameJoint.Equals("WHEEL"))
            {
                WheelPropertiesPanel panel = new WheelPropertiesPanel();
                panel.Init(this, joint);

                this.CurrentJointPanel = panel;
                this.jointManagerPage.Controls.Add(panel);
            }
            else if (nameJoint.Equals("WELD"))
            {
                WeldPropertiesPanel panel = new WeldPropertiesPanel();
                panel.Init(this, joint);

                this.CurrentJointPanel = panel;
                this.jointManagerPage.Controls.Add(panel);
            }
            else if (nameJoint.Equals("TOUCH"))
            {
                TouchPropertiesPanel panel = new TouchPropertiesPanel();
                panel.Init(this, joint);

                this.CurrentJointPanel = panel;
                this.jointManagerPage.Controls.Add(panel);
            }

            this.setWorkSpace("GLOBAL");
        }

        private void startJointCreationBt_Click(object sender, EventArgs e)
        {
            closeJointPage();

            //Verifier qu'on est bien sur un layer seulement
            if (this.sceneEditorView1.CurentCalque.Equals("LAYER"))
            {
               
                //Creer une jointure vide
                CoronaJointure joint = new CoronaJointure(this.gameElementTreeView1.LayerSelected);

                //Pour les pivots
                if (this.typeJointCmbBx.SelectedIndex == 0)
                {
                    this.openPanelJointure("PIVOT", joint);
                    ((PivotPropertiesPanel)this.CurrentJointPanel).startCreationJoint();

                }
                //Pour les Distance
                else if (this.typeJointCmbBx.SelectedIndex == 1)
                {
                    this.openPanelJointure("DISTANCE", joint);
                    ((DistancePropertiesPanel)this.CurrentJointPanel).startCreationJoint();
                }
                //Pour les Piston
                else if (this.typeJointCmbBx.SelectedIndex == 2)
                {
                    this.openPanelJointure("PISTON", joint);
                    ((PistonPropertiesPanel)this.CurrentJointPanel).startCreationJoint();
                }
                //Pour les Friction
                else if (this.typeJointCmbBx.SelectedIndex == 3)
                {
                    this.openPanelJointure("FRICTION", joint);
                    ((FrictionPropertiesPanel)this.CurrentJointPanel).startCreationJoint();
                }
                //Pour les Weld
                else if (this.typeJointCmbBx.SelectedIndex == 4)
                {
                    this.openPanelJointure("WELD", joint);
                    ((WeldPropertiesPanel)this.CurrentJointPanel).startCreationJoint();
                }
                //Pour les Wheel
                else if (this.typeJointCmbBx.SelectedIndex == 5)
                {
                    this.openPanelJointure("WHEEL", joint);
                    ((WheelPropertiesPanel)this.CurrentJointPanel).startCreationJoint();
                }
                //Pour les Pulley
                else if (this.typeJointCmbBx.SelectedIndex == 6)
                {
                    this.openPanelJointure("PULLEY", joint);
                    ((PulleyPropertiesPanel)this.CurrentJointPanel).startCreationJoint();
                }
                //Pour les Touch
                else if (this.typeJointCmbBx.SelectedIndex == 7)
                {
                    this.openPanelJointure("TOUCH", joint);
                    ((TouchPropertiesPanel)this.CurrentJointPanel).startCreationJoint();
                }

                this.setWorkSpace("GLOBAL");
            }
            else
            {
                MessageBox.Show("Please select a Layer to create joints !", "Select a layer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

      
        private void cancelCreationJointBt_Click(object sender, EventArgs e)
        {
            closeJointPage();

            if (this.isFormLocked == false)
                GorgonLibrary.Gorgon.Go();
        }

        public void closeJointPage()
        {
            TabControl controlParent = this.jointManagerPage.Parent as TabControl ;
            if (controlParent != null)
            {
                controlParent.TabPages.Remove(this.jointManagerPage);
            }
            
           
            //Fermer un panel si existant
            if (this.jointManagerPage != null)
            {
                for (int i = 0; i < this.jointManagerPage.Controls.Count; i++)
                {
                    this.jointManagerPage.Controls[i].Dispose();
                }

                this.jointManagerPage.Controls.Clear();

            }


            this.CurrentJointPanel = null;

            this.guideCreationJoint.Text = "";
        }
        
        
        //------------------------------------------------------------------------------
        //---------------------CREATION DES SHAPES---------------------------------------
        //-----------------------------------------------------------------------------------
        private void newRectBt_Click(object sender, EventArgs e)
        {
            this.SetModeObject();

            this.sceneEditorView1.Mode = "CREATE_SHAPE";

            this.newLineBt.Checked = false;
            this.newCircleBt.Checked = false;
            this.newRectBt.Checked = true;
            this.newTextBt.Checked = false;
            this.newBezierBt.Checked = false;

            this.ShapeType = 1;
            this.sceneEditorView1.isBezierCreated = false;
            this.sceneEditorView1.isLineCreated = false;
           
        }

        private void newLineBt_Click(object sender, EventArgs e)
        {
            this.SetModeObject();
            this.sceneEditorView1.Mode = "CREATE_SHAPE";

            this.newLineBt.Checked = true;
            this.newCircleBt.Checked = false;
            this.newRectBt.Checked = false;
            this.newTextBt.Checked = false;
            this.newBezierBt.Checked = false;


            this.ShapeType = 2;
            this.sceneEditorView1.isBezierCreated = false;
            this.sceneEditorView1.isLineCreated = false;


        }
        private void newBezierBt_Click(object sender, EventArgs e)
        {
            this.SetModeObject();
            this.sceneEditorView1.Mode = "CREATE_SHAPE";

            this.newLineBt.Checked = false;
            this.newCircleBt.Checked = false;
            this.newRectBt.Checked = false;
            this.newTextBt.Checked = false;
            this.newBezierBt.Checked = true;

            this.ShapeType = 5;
            this.sceneEditorView1.isBezierCreated = false;
            this.sceneEditorView1.isLineCreated = false;

        }

        private void newTextBt_Click(object sender, EventArgs e)
        {
            this.SetModeObject();
            this.sceneEditorView1.Mode = "CREATE_SHAPE";

            this.newLineBt.Checked = false;
            this.newCircleBt.Checked = false;
            this.newRectBt.Checked = false;
            this.newTextBt.Checked = true;
            this.newBezierBt.Checked = false;

            this.ShapeType = 4;
            this.sceneEditorView1.isBezierCreated = false;
            this.sceneEditorView1.isLineCreated = false;

        }

        private void newCircleBt_Click(object sender, EventArgs e)
        {
            this.SetModeObject();
            this.sceneEditorView1.Mode = "CREATE_SHAPE";

            this.newLineBt.Checked = false;
            this.newCircleBt.Checked = true;
            this.newRectBt.Checked = false;
            this.newTextBt.Checked = false;
            this.newBezierBt.Checked = false;

            this.ShapeType = 3;
            this.sceneEditorView1.isBezierCreated = false;
            this.sceneEditorView1.isLineCreated = false;



        }

        public void addFigureBt_Click(object sender, EventArgs e)
        {
            if (this.sceneEditorView1.FigActive != null &&
                (this.gameElementTreeView1.LayerSelected != null || 
                (this.gameElementTreeView1.CoronaObjectSelected !=null && this.gameElementTreeView1.CoronaObjectSelected.isEntity == true)))
            {

                this.sceneEditorView1.FigActive.FillColor = Color.Gray;
                this.sceneEditorView1.FigActive.Fill = true;
                if (this.sceneEditorView1.FigActive.ShapeType.Equals("CURVE"))
                {
                    CGE_Figures.CourbeBezier courbe = this.sceneEditorView1.FigActive as CGE_Figures.CourbeBezier;
                    if (courbe.UserPoints.Count < 2)
                        return;
                }

                if (this.sceneEditorView1.FigActive.ShapeType.Equals("LINE"))
                {
                    CGE_Figures.Line line = this.sceneEditorView1.FigActive as CGE_Figures.Line;
                    
                    if (line.Points.Count < 2)
                        return;
                }

                //Creer un displayObject
                DisplayObject dispObject = new DisplayObject(this.sceneEditorView1.FigActive,null);
                if (dispObject.Type.Equals("FIGURE"))
                {
                    if (dispObject.Figure.ShapeType.Equals("TEXT"))
                    {
                        CGE_Figures.Texte text = dispObject.Figure as CGE_Figures.Texte;

                        //int width = Convert.ToInt32(text.font2.Size * (text.txt.Length / 1.5));
                        //int height = Convert.ToInt32(text.font2.Size * 1.5);
                        Point pDest = this.sceneEditorView1.surfaceTextTemp.Location;
                        Size size = this.sceneEditorView1.surfaceTextTemp.Size;
                        dispObject.SurfaceRect = new Rectangle(pDest, size);
                        this.sceneEditorView1.surfaceTextTemp = Rectangle.Empty ;
                        //Recuperer la clé du translate
                        bool isFound = false;
                        for (int i = 0; i < this.CurrentProject.Langues.Count; i++)
                        {
                            LangueObject langue = this.CurrentProject.Langues[i];
                            if (langue.Langue.Equals(this.CurrentProject.DefaultLanguage))
                            {
                                for (int j = 0; j < langue.TranslationElement.Count; j++)
                                {
                                    LangueElement elem = langue.TranslationElement[j];
                                    if (elem.Key.Equals(text.txt))
                                    {
                                        elem.Key = text.txt;
                                        elem.Translation = text.txt;
                                        isFound = true;
                                        break;
                                    }

                                }

                                if (isFound == false)
                                {
                                    LangueElement elem = new LangueElement(text.txt, text.txt);
                                    langue.TranslationElement.Add(elem);
                                }

                                break;
                            }
                        }

                    }
                }

                if (this.gameElementTreeView1.CoronaObjectSelected != null && 
                    (this.gameElementTreeView1.CoronaObjectSelected.isEntity == true ||this.gameElementTreeView1.CoronaObjectSelected.EntityParent != null))
                {
                    dispObject.Name = this.gameElementTreeView1.ProjectSelected.IncrementObjectName("shape" + this.gameElementTreeView1.CoronaObjectSelected.LayerParent.CoronaObjects.Count);
                    //Creer un corona object
                    CoronaObject obj = new CoronaObject(dispObject);

                    this.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj, this.sceneEditorView1.CurrentScale, this.sceneEditorView1.getOffsetPoint());

                    if (this.gameElementTreeView1.CoronaObjectSelected.isEntity == true)
                    {
                        //Ajouter l'objet au projet
                        this.gameElementTreeView1.CoronaObjectSelected.Entity.addObject(obj);
                    }
                    else
                    {
                        //Ajouter l'objet au projet
                        this.gameElementTreeView1.CoronaObjectSelected.EntityParent.addObject(obj);
                    }
                    
                    this.gameElementTreeView1.newCoronaObject(obj);
                }
                else
                {
                    dispObject.Name = this.gameElementTreeView1.ProjectSelected.IncrementObjectName("shape" + this.gameElementTreeView1.LayerSelected.CoronaObjects.Count);

                    //Creer un corona object
                    CoronaObject obj = new CoronaObject(dispObject);

                 

                    //Ajouter l'objet au projet
                    this.gameElementTreeView1.LayerSelected.addCoronaObject(obj, true);
                    this.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj,this.sceneEditorView1.CurrentScale,this.sceneEditorView1.getOffsetPoint());
                    this.gameElementTreeView1.newCoronaObject(obj);
                }
                
                this.sceneEditorView1.FigActive = null;
                this.sceneEditorView1.isBezierCreated = false;
                this.sceneEditorView1.isLineCreated = false;
                this.SetModeObject();
            }

            
        }


        //----------- PROPERTY GRID EVENTS --------------------
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            for (int i = 0; i<this.propertyGrid1.SelectedObjects.Length; i++)
            {
                object obj = this.propertyGrid1.SelectedObjects[i];
                if (obj != null)
                {
                    if (obj is GameEditor.PropertyGridConverters.TextPropertyConverter)
                    {
                        continue;
                    }
                    else if (obj is GameEditor.PropertyGridConverters.ObjectPropertyConverter)
                    {
                        GameEditor.PropertyGridConverters.ObjectPropertyConverter objConverter = obj as GameEditor.PropertyGridConverters.ObjectPropertyConverter;
                        CoronaObject coronaObj = objConverter.GetObjectSelected();
                        this.sceneEditorView1.GraphicsContentManager.UpdateSpriteStates(coronaObj, this.sceneEditorView1.CurrentScale, this.sceneEditorView1.getOffsetPoint());
                    }
                    else if (obj is GameEditor.PropertyGridConverters.SpritePropertyConverter)
                    {
                        GameEditor.PropertyGridConverters.SpritePropertyConverter objConverter = obj as GameEditor.PropertyGridConverters.SpritePropertyConverter;
                        CoronaObject coronaObj = objConverter.GetObjectSelected();
                        this.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(coronaObj, this.sceneEditorView1.CurrentScale, this.sceneEditorView1.getOffsetPoint());
                    }
                   
                    else if (obj is GameEditor.PropertyGridConverters.JoystickPropertyConverter)
                    {
                        if (e.ChangedItem.Label.Equals("JoystickName"))
                        {

                            GameEditor.PropertyGridConverters.JoystickPropertyConverter objConverter = obj as GameEditor.PropertyGridConverters.JoystickPropertyConverter;
                            Krea.Corona_Classes.Controls.JoystickControl joy = objConverter.GetJoystickSelected();

                            //Move the asset name if exists
                            
                            string oldInnerPath = Path.Combine(this.CurrentProject.ProjectPath + "\\Resources\\Images", e.OldValue + "_inner.png");
                            string newInnerPath = Path.Combine(this.CurrentProject.ProjectPath + "\\Resources\\Images", joy.joystickName + "_inner.png");
                            if (File.Exists(oldInnerPath) && !oldInnerPath.Equals(newInnerPath))
                                File.Move(oldInnerPath, newInnerPath);


                            string oldOuterPath = Path.Combine(this.CurrentProject.ProjectPath + "\\Resources\\Images", e.OldValue + "_outer.png");
                            string newOuterPath = Path.Combine(this.CurrentProject.ProjectPath + "\\Resources\\Images", joy.joystickName + "_outer.png");
                            if (File.Exists(oldOuterPath) && !oldOuterPath.Equals(newOuterPath))
                                File.Move(oldOuterPath, newOuterPath);

                            this.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(joy,
                                this.sceneEditorView1.CurrentScale, this.sceneEditorView1.getOffsetPoint());
                        }
                        else if (e.ChangedItem.Label.Equals("InnerImage") ||e.ChangedItem.Label.Equals("OuterImage") )
                        {
                            GameEditor.PropertyGridConverters.JoystickPropertyConverter objConverter = obj as GameEditor.PropertyGridConverters.JoystickPropertyConverter;
                            Krea.Corona_Classes.Controls.JoystickControl joy = objConverter.GetJoystickSelected();

                            this.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(joy,
                                this.sceneEditorView1.CurrentScale, this.sceneEditorView1.getOffsetPoint());
                        }

                    }
                   
                }
            }

            Scene sceneSelected = this.gameElementTreeView1.SceneSelected;
            if (sceneSelected != null)
            {
                
                if (this.IsCustomBuild == true)
                {
                    float XRatio = (float)this.currentTargetResolution.Resolution.Width / (float)this.CurrentProject.width;
                    float YRatio = (float)this.currentTargetResolution.Resolution.Height / (float)this.CurrentProject.height;
                    this.cgEeditor1.RefreshSceneLuaCode(sceneSelected, true, XRatio,YRatio);
                }
                else
                    this.cgEeditor1.RefreshSceneLuaCode(sceneSelected, false, 1, 1);


            }

            if (this.isFormLocked == false)
            {
                GorgonLibrary.Gorgon.Go();
                GorgonLibrary.Gorgon.Go();
                this.tilesMapEditor1.Refresh();
            }
           
        }


        //-------------------- Action Backworker ---------------------------
        private void mainBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                string actionToExecute = e.Argument.ToString();

                if (actionToExecute.Equals("ACTION_SAVE") || actionToExecute.Equals("ACTION_SAVE_LOAD")|| actionToExecute.Equals("ACTION_SAVE_CLEAN")
                    || actionToExecute.Equals("ACTION_SAVE_CLOSE"))
                {

                    string path = this.CurrentProject.CgeProjectFilename;
                    this.addRecentProjectPath(path);
                    this.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();
                    this.CurrentProject.saveProject(path, sender as BackgroundWorker);

                }
                else if (this.currentWorkerAction.Equals("ACTION_BUILD_SAVE") ||
                   this.currentWorkerAction.Equals("ACTION_BUILD_PLAY") ||
                   this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_LOAD")||
                      this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_CLEAN") ||
                    this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_CLOSE"))
                {
                    Emulator.Instance.Dispose();

                    //Builder le projet
                    CoronaGameBuilder builder = new CoronaGameBuilder(this.CurrentProject);
                    if (this.IsCustomBuild == true)
                    {
                        float XRatio = (float)this.currentTargetResolution.Resolution.Width / (float)this.CurrentProject.width;
                        float YRatio = (float)this.currentTargetResolution.Resolution.Height / (float)this.CurrentProject.height;
                        builder.buildToLua(worker, true, XRatio, YRatio, this.IsDebugMode);
                       
                    }
                    else
                        builder.buildToLua(worker, false, 1, 1, this.IsDebugMode);
                }
                else if (actionToExecute.Equals("ACTION_LOAD"))
                {
                    this.loadProject(this.loadingProjectFilename, worker);
                }
                else if (actionToExecute.Equals("ACTION_IMPORTSTAGE"))
                {
                    if (this.CurrentProject != null)
                    {
                        ProjectSerializer.importStage(this, this.filenameSelected, worker);
                    }
                }
                else if (actionToExecute.Equals("ACTION_EXPORTSTAGE"))
                {
                    if (this.gameElementTreeView1.SceneSelected != null)
                    {
                        ProjectSerializer.exportStage(this, this.gameElementTreeView1.SceneSelected, this.directorySelectedDest, worker);
                    }
                   
                }
                else if (actionToExecute.Equals("ACTION_IMPORTLAYER"))
                {
                    if (this.gameElementTreeView1.SceneSelected != null)
                    {
                        ProjectSerializer.importLayer(this, this.filenameSelected, worker);
                    }
                }
                else if (actionToExecute.Equals("ACTION_EXPORTLAYER"))
                {
                    if (this.gameElementTreeView1.LayerSelected != null)
                    {
                        ProjectSerializer.exportLayer(this, this.gameElementTreeView1.LayerSelected, this.directorySelectedDest, worker);
                    }
                }
                else if (actionToExecute.Equals("ACTION_IMPORTENTITY"))
                {
                    if (this.gameElementTreeView1.LayerSelected != null)
                    {
                        ProjectSerializer.importEntity(this, this.filenameSelected, worker);
                    }
                }
                else if (actionToExecute.Equals("ACTION_EXPORTENTITY"))
                {
                    if (this.gameElementTreeView1.CoronaObjectSelected != null)
                    {
                        ProjectSerializer.exportEntity(this, this.gameElementTreeView1.CoronaObjectSelected, this.directorySelectedDest, worker);
                    }
                }
                else if (actionToExecute.Equals("ACTION_IMPORTOBJECT"))
                {
                    if (this.gameElementTreeView1.LayerSelected != null)
                    {
                        ProjectSerializer.importObject(this, this.filenameSelected, worker);
                    }
                }
                else if (actionToExecute.Equals("ACTION_EXPORTOBJECT"))
                {
                    if (this.gameElementTreeView1.CoronaObjectSelected != null)
                    {
                        ProjectSerializer.exportObject(this, this.gameElementTreeView1.CoronaObjectSelected, this.directorySelectedDest, worker);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during executing background task : "+this.currentWorkerAction+" !\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                
            }
        }

        private void mainBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            this.workerProgressBar.Value = progress;

             if (this.currentWorkerAction.Equals("ACTION_BUILD_SAVE") ||
                this.currentWorkerAction.Equals("ACTION_BUILD_PLAY")||
                this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_LOAD") ||
                this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_CLEAN") ||
                    this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_CLOSE"))
            {
                if(progress == 10)
                    this.statusWorkerProgressBar.Text = "Copying Ressource Files";
                else if (progress == 20)
                    this.statusWorkerProgressBar.Text = "Creating build settings";
                else if (progress == 30)
                    this.statusWorkerProgressBar.Text = "Building application content";
                else if (progress == 40)
                    this.statusWorkerProgressBar.Text = "Resizing images";
                else if (progress == 50)
                    this.statusWorkerProgressBar.Text = "Creating languages files";
                else if (progress == 60)
                    this.statusWorkerProgressBar.Text = "Creating tile maps files";
            }
             else if (this.currentWorkerAction.Equals("ACTION_SAVE") || 
                 currentWorkerAction.Equals("ACTION_SAVE_LOAD") ||
                 currentWorkerAction.Equals("ACTION_SAVE_CLOSE") || 
                 currentWorkerAction.Equals("ACTION_SAVE_CLEAN"))
            {
                if (progress > 10)
                    this.statusWorkerProgressBar.Text = "Saving...";
                                
            }
            else if (this.currentWorkerAction.Equals("ACTION_LOAD"))
            {
                if (progress < 40)
                    this.statusWorkerProgressBar.Text = "Loading project file...";
                else if (progress >= 40)
                {
                    this.statusWorkerProgressBar.Text = "Initializing world...";
                    
                }
            }
             else if (this.currentWorkerAction.Equals("ACTION_IMPORTSTAGE"))
             {
                 this.statusWorkerProgressBar.Text = "Importing stage to project...";
             }
             else if (this.currentWorkerAction.Equals("ACTION_EXPORTSTAGE"))
             {
                 this.statusWorkerProgressBar.Text = "Exporting scene...";
             }
        }

        private void mainBackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int progress = 100;
           
            this.workerProgressBar.Value = progress;

            if (this.currentWorkerAction.Equals("ACTION_SAVE") || this.currentWorkerAction.Equals("ACTION_SAVE_LOAD")
                || this.currentWorkerAction.Equals("ACTION_SAVE_CLOSE") || this.currentWorkerAction.Equals("ACTION_SAVE_CLEAN"))
            {
                if (this.imageObjectsPanel1.ShouldBeRefreshed == true)
                {
                    this.imageObjectsPanel1.RefreshCurrentAssetProject();
                    this.imageObjectsPanel1.ShouldBeRefreshed = false;
                }

                this.statusWorkerProgressBar.Text = "Saving success!";

                this.loadRecentProjectPathItems();

                //START LOADING
                this.currentWorkerAction = this.currentWorkerAction.Replace("_SAVE", "");

                if( this.currentWorkerAction.Equals("ACTION_LOAD"))
                    this.clearCurrentProject();


                this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);

                if (this.currentWorkerAction.Equals("ACTION_CLOSE"))
                {
                    this.clearCurrentProject();
                    //openNewProjectPanel();

                    Application.Exit();
                }
                else if (this.currentWorkerAction.Equals("ACTION_CLEAN"))
                {
                    this.clearCurrentProject();
                   
                }

                GorgonLibrary.Gorgon.Go();
                
            }
            else if (this.currentWorkerAction.Equals("ACTION_BUILD_SAVE") ||
                this.currentWorkerAction.Equals("ACTION_BUILD_PLAY")||
                this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_LOAD")||
                this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_CLOSE") || this.currentWorkerAction.Equals("ACTION_BUILD_SAVE_CLEAN"))
            {
                this.statusWorkerProgressBar.Text = "Build succeeded!";

                //START SAVING
                if(this.currentWorkerAction.Equals("ACTION_BUILD_PLAY"))
                {
                    this.statusWorkerProgressBar.Text = "Starting simulator...";

                    this.debuggerPanel1.closeDebuggerSession(false);

                    if (this.IsDebugMode == false && this.CurrentWorkspace.Equals("DEBUG"))
                        this.setWorkSpace("GLOBAL");

                    if (Settings1.Default.RemoteControlEnabled == true && IsDebugMode == false)
                    {
                        if (currentRemoteControllerForm == null)
                        {
                            currentRemoteControllerForm = new RemoteControllerForm();
                            currentRemoteControllerForm.Show(this);
                        }
                        else
                        {
                            if (currentRemoteControllerForm.IsDisposed == true)
                            {
                                currentRemoteControllerForm = new RemoteControllerForm();
                                currentRemoteControllerForm.Show(this);
                               
                            }
                        }

                        currentRemoteControllerForm.init();

                    }
                    else if (Settings1.Default.RemoteControlEnabled == true && IsDebugMode == true)
                    {
                        MessageBox.Show("You cannot launch in the same time the visual debugger and the remote controller!","Disabling remote controller", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.remoteModeCmbBx.SelectedIndex = 0;
                    }

                   
                    this.cgEeditor1.startEmulator();

                    this.currentWorkerAction = "ACTION_SAVE";
                    this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                }
                else
                {
                    this.currentWorkerAction = this.currentWorkerAction.Replace("_BUILD", "");
                    this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                }

                GorgonLibrary.Gorgon.Go();
                
            }
            else if (this.currentWorkerAction.Equals("ACTION_LOAD"))
            {
                if (this.imageObjectsPanel1.ShouldBeRefreshed == true)
                {
                    this.imageObjectsPanel1.RefreshCurrentAssetProject();
                    this.imageObjectsPanel1.ShouldBeRefreshed = false;
                }

                this.statusWorkerProgressBar.Text = "Loading sucess !";
                this.refreshAssetsProjectsBt_Click(null, null);

                //Creer la hierarchie des nodes
                this.gameElementTreeView1.loadProject(this.CurrentProject);

                if (this.CurrentProject != null)
                {
                    this.fileExplorer1.PopulateTreeView();

                    float XRatio = 1;
                    float YRatio = 1;
                    if (this.IsCustomBuild == true)
                    {
                        XRatio = (float)this.currentTargetResolution.Resolution.Width / (float)this.CurrentProject.width;
                        YRatio = (float)this.currentTargetResolution.Resolution.Height / (float)this.CurrentProject.height;
                     
                    }

                    for (int i = 0; i < this.CurrentProject.Scenes.Count; i++)
                    {

                        Scene scene = this.CurrentProject.Scenes[i];
                        if (scene.Camera == null)
                        {
                            scene.Camera = new Corona_Classes.Camera(scene, scene.SurfaceFocus, scene.CameraFollowLimitRectangle);
                            if (scene.objectFocusedByCamera != null)
                                scene.Camera.setObjectFocusedByCamera(scene.objectFocusedByCamera);
                            scene.Camera.isSurfaceFocusVisible = scene.isSurfaceFocusVisible;

                        }

                        this.cgEeditor1.RefreshSceneLuaCode(scene, IsCustomBuild,XRatio,YRatio);
                    }

                    if(this.CurrentProject.Scenes.Count > 0)
                        this.sceneEditorView1.setModeSceneEditor(this.CurrentProject.Scenes[0]);

                    GorgonLibrary.Gorgon.Go();
                }
                    
            }
            else if (this.currentWorkerAction.Equals("ACTION_IMPORTSTAGE")
                 || this.currentWorkerAction.Equals("ACTION_IMPORTLAYER")
                || this.currentWorkerAction.Equals("ACTION_IMPORTENTITY")
                || this.currentWorkerAction.Equals("ACTION_IMPORTOBJECT"))
            {
                if (this.imageObjectsPanel1.ShouldBeRefreshed == true)
                {
                    this.imageObjectsPanel1.RefreshCurrentAssetProject();
                    this.imageObjectsPanel1.ShouldBeRefreshed = false;
                }

                this.statusWorkerProgressBar.Text = "Import has completed successfully!";

                //Creer la hierarchie des nodes
                this.gameElementTreeView1.loadProject(this.CurrentProject);

                if (this.CurrentProject != null)
                {
                    float XRatio = 1;
                    float YRatio = 1;
                    if (this.IsCustomBuild == true)
                    {
                        XRatio = (float)this.currentTargetResolution.Resolution.Width / (float)this.CurrentProject.width;
                        YRatio = (float)this.currentTargetResolution.Resolution.Height / (float)this.CurrentProject.height;

                    }

                    for (int i = 0; i < this.CurrentProject.Scenes.Count; i++)
                    {

                        Scene scene = this.CurrentProject.Scenes[i];

                        if (scene.Camera == null)
                        {
                            scene.Camera = new Corona_Classes.Camera(scene, scene.SurfaceFocus, scene.CameraFollowLimitRectangle);
                            if (scene.objectFocusedByCamera != null)
                                scene.Camera.setObjectFocusedByCamera(scene.objectFocusedByCamera);
                            scene.Camera.isSurfaceFocusVisible = scene.isSurfaceFocusVisible;

                        }

                        this.cgEeditor1.RefreshSceneLuaCode(scene, IsCustomBuild, XRatio, YRatio);
                    }

                    if (this.CurrentProject.Scenes.Count > 0)
                        this.sceneEditorView1.setModeSceneEditor(this.CurrentProject.Scenes[0]);
                }
            }
            else if (this.currentWorkerAction.Equals("ACTION_EXPORTSTAGE")
                || this.currentWorkerAction.Equals("ACTION_EXPORTLAYER")
                || this.currentWorkerAction.Equals("ACTION_EXPORTENTITY")
                || this.currentWorkerAction.Equals("ACTION_EXPORTOBJECT"))
            {
                this.statusWorkerProgressBar.Text = "Export has completed successfully!";
            }
            
            else
            {
                this.statusWorkerProgressBar.Text = "Ready !";
                this.workerProgressBar.Value = 100;

                //this.Enabled = true;
                this.isFormLocked = false;
                this.fileExplorer1.PopulateTreeView();
            }
        }

        private void closePhysicBodyManagerBt_Click(object sender, EventArgs e)
        {
            TabControl controlParent = this.physicsBodyManagerPage.Parent as TabControl;
            if (controlParent != null)
            {
                controlParent.TabPages.Remove(this.physicsBodyManagerPage);

                if (controlParent.TabPages.Count > 0)
                    controlParent.SelectedTab = controlParent.TabPages[0];
            }

            this.checkAllTabControlEmpty();
        }

      
        public void SetModeFocus()
        {
            if (this.sceneEditorView1.Mode.Equals("FOCUS"))
            {
                if (this.gameElementTreeView1.SceneSelected != null)
                    this.gameElementTreeView1.SceneSelected.Camera.isSurfaceFocusVisible = !this.gameElementTreeView1.SceneSelected.Camera.isSurfaceFocusVisible;

                
            }
            else if (this.sceneEditorView1.Mode.Equals("CAMERA_RECTANGLE"))
            {
                if (this.gameElementTreeView1.SceneSelected != null)
                {
                    this.sceneEditorView1.setModeMovingFocus();
                    this.gameElementTreeView1.SceneSelected.Camera.isSurfaceFocusVisible = true;
                    this.sceneEditorView1.Mode = "CAMERA_RECTANGLE";
                }
                    
            }



            if (this.gameElementTreeView1.LayerSelected != null)
                this.gameElementTreeView1.LayerSelected.JointureSelected = null;


            this.sceneEditorView1.setModeMovingFocus();
            this.objModeBt.Checked = false;
            this.adModeBt.Checked = false;
            this.cameraModeBt.Checked = true;
            this.controlsModeBt.Checked = false;
            GorgonLibrary.Gorgon.Go();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SetModeFocus();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SetModeObject();

        }

        private void modeEntitiesBt_Click(object sender, EventArgs e)
        {
            this.SetModeEntity();
        }

        public void SetModeAds()
        {
            if (this.gameElementTreeView1.LayerSelected != null)
                this.gameElementTreeView1.LayerSelected.JointureSelected = null;

            this.sceneEditorView1.setModeMovingAddSurface();
            this.objModeBt.Checked = false;
            this.modeEntitiesBt.Checked = false;
            this.adModeBt.Checked = true;
            this.cameraModeBt.Checked = false;
            this.controlsModeBt.Checked = false;

        }
        public void SetModeObject()
        {
            if (this.gameElementTreeView1.LayerSelected != null)
                this.gameElementTreeView1.LayerSelected.JointureSelected = null;

            this.sceneEditorView1.setModeMovingObjects();
            this.objModeBt.Checked = true;
            this.modeEntitiesBt.Checked = false;
            this.adModeBt.Checked = false;
            this.cameraModeBt.Checked = false;
            this.controlsModeBt.Checked = false;
        }

        public void SetModeEntity()
        {
            if (this.gameElementTreeView1.LayerSelected != null)
                this.gameElementTreeView1.LayerSelected.JointureSelected = null;

            this.sceneEditorView1.setModeMovingEntities();
            this.objModeBt.Checked = false;
            this.modeEntitiesBt.Checked = true;
            this.adModeBt.Checked = false;
            this.cameraModeBt.Checked = false;
            this.controlsModeBt.Checked = false;
        }

        public void setModeJoint()
        {
            this.sceneEditorView1.setModeMovingJoint();
            this.objModeBt.Checked = false;
            this.modeEntitiesBt.Checked = false;
            this.adModeBt.Checked = false;
            this.cameraModeBt.Checked = false;
            this.controlsModeBt.Checked = false;
        }
        public void SetModeControl()
        {
            if (this.gameElementTreeView1.LayerSelected != null)
                this.gameElementTreeView1.LayerSelected.JointureSelected = null;

            this.sceneEditorView1.setModeMovingControls();
            this.objModeBt.Checked = false;
            this.modeEntitiesBt.Checked = false;
            this.adModeBt.Checked = false;
            this.cameraModeBt.Checked = false;
            this.controlsModeBt.Checked = true;
        }
        private void controlsModeBt_Click(object sender, EventArgs e)
        {
            this.SetModeControl();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.SetModeAds();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                this.Init();

                //si le chemin du fichier est passé en argument
                string[] Args = Environment.GetCommandLineArgs();
                if (Args.Length == 2)
                {

                    this.loadingProjectFilename = Args[1];

                    if (this.mainBackWorker.IsBusy == true)
                        this.mainBackWorker.CancelAsync();

                    if (this.mainBackWorker.IsBusy == false)
                    {

                        //BUILD AND SAVE
                        this.closeAllTabPage();
                        this.currentWorkerAction = "ACTION_LOAD";
                        this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                    }
                }
                


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during project loading: " + ex.Message);

              
            }

        }

        private void validateShape_Click(object sender, EventArgs e)
        {
            if (this.ShapeType == 5 || this.ShapeType == 2)
            {
                this.addFigureBt_Click(null, null);

                if (this.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();
            }
        }

        public void validShapeBuilding()
        {
            this.validateShape_Click(null, null);
        }
       /* private void addWidgetBt_Click(object sender, EventArgs e)
        {
            if (this.widgetTypeCmbBx.SelectedItem != null && this.gameElementTreeView1.LayerSelected != null)
            {
                string widgetType = this.widgetTypeCmbBx.SelectedItem.ToString();
                CoronaLayer layer = this.gameElementTreeView1.LayerSelected;

                if(widgetType.Equals("TabBar"))
                {

                    WidgetTabBar tabBar = new WidgetTabBar("WidgetTabBar" + layer.Widgets.Count, layer);
                    layer.Widgets.Add(tabBar);
                    this.gameElementTreeView1.newCoronaWidget(tabBar);
                }
                else if (widgetType.Equals("PickerWheel"))
                {

                    WidgetPickerWheel pickerW = new WidgetPickerWheel("WidgetPickerWheel" + layer.Widgets.Count, layer);
                    layer.Widgets.Add(pickerW);
                    this.gameElementTreeView1.newCoronaWidget(pickerW);
                }
            }
        }*/


        //--------------------------------------------------------------------------------------------
        //-------------------- WIDGET CONTROL PANELS SECTION -----------------------------------------
        //--------------------------------------------------------------------------------------------
        //--- For the TABBAR WIDGET ----
        public void openTabBarWidgetPanel(WidgetTabBar tabBar)
        {
            if (tabBar != null)
            {
                TabPage tabBarPage = new TabPage(tabBar.Name +" buttons");
                tabBarPage.AutoScroll = true;
                PanelTabBar panel = new PanelTabBar(tabBar);

                tabBarPage.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                this.tabControlArea2.Controls.Add(tabBarPage);
            }
        }
        //--- For the PICKERWHEEL WIDGET ----
        public void openPickerWheelWidgetPanel(WidgetPickerWheel pickW)
        {
            if (pickW != null)
            {
                TabPage PickerWheelPage = new TabPage(pickW.Name + " Columns");
                PickerWheelPage.AutoScroll = true;
                PanelPickerWheel panel = new PanelPickerWheel(pickW);

                PickerWheelPage.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                this.tabControlArea2.Controls.Add(PickerWheelPage);
            }
        }

        private void gameElementsTabControl_SizeChanged(object sender, EventArgs e)
        {
            this.tilesManagerPanel1.refreshTextureTileModelLocations();
            this.tilesManagerPanel1.refreshObjectTileModelLocations();
        }

        private void bodyZoomInBt_Click(object sender, EventArgs e)
        {
            this.physicBodyEditorView1.zoomAvant();
           
        }

        private void bodyZoomOutBt_Click(object sender, EventArgs e)
        {
            this.physicBodyEditorView1.zoomArriere();
        }

        private void refreshAssetsProjectsBt_Click(object sender, EventArgs e)
        {
            this.imageObjectsPanel1.laodAssetsProjectDirectories();
        }

        private void newSceneBt_Click(object sender, EventArgs e)
        {
            this.addSceneToProject(null);
        }

        private void newLayerBt_Click(object sender, EventArgs e)
        {
            Scene sceneSelected = this.gameElementTreeView1.SceneSelected;
            if (sceneSelected != null)
            {
                CoronaLayer layer = sceneSelected.newLayer();
                this.gameElementTreeView1.newLayer(sceneSelected, layer);
            }
           
        }

        private void newEntityBt_Click(object sender, EventArgs e)
        {
            CoronaLayer layerSelected = this.gameElementTreeView1.LayerSelected;
            if (layerSelected != null)
            {

                CoronaObject entity = layerSelected.newEntity();
                this.gameElementTreeView1.newCoronaObject(entity);
            }


        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void designerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setWorkSpace("GLOBAL");
        }

        private void codeEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setWorkSpace("GLOBAL");
        }

        private void bothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setWorkSpace("GLOBAL");
        }

        public void setWorkSpace(string mode)
        {
           // if (mode.Equals(this.CurrentWorkspace)) return;

            Form1.Suspend(this);
            if (mode.Equals("GLOBAL"))
            {
                CurrentWorkspace = mode;
               // voletCentral.Panel2Collapsed = true;
               // voletCentral.Panel1Collapsed = false;
               // verticalSplitterDroit.Panel2Collapsed = false;
               // splitContainer4.Panel2Collapsed = true;
               // this.tabControlArea4.SelectedIndex = 0;
            
               //verticalSplitterGauche.Panel1Collapsed = false;
               //verticalSplitterGauche.Panel2Collapsed = false;

                //splitContainer1.SplitterDistance = this.Size.Width / 2;

                for (int i = 0; i < this.tabPages.Count; i++)
                {
                    for (int j = 0; j < this.tabPages[i].Controls.Count; j++)
                        this.tabPages[i].Controls[j].Enabled = true;
                }

               this.cgEeditor1.setDocumentsReadOnlyState(false);

               TabControl parent = null;
               if (this.debuggerPage.Parent != null)
               {
                    parent = this.debuggerPage.Parent as TabControl;
                   parent.TabPages.Remove(this.debuggerPage);
               }

               if (parent != null)
               {
                   if (parent.TabPages.Count > 0)
                       parent.SelectedTab = parent.TabPages[0];
               }

               this.checkAllTabControlEmpty();
            }
          
            else if (mode.Equals("DEBUG"))
            {
                CurrentWorkspace = mode;
                //voletCentral.Panel1Collapsed = true;
                //voletCentral.Panel2Collapsed = false;

                //splitContainer4.Panel2Collapsed = false;
                //this.tabControlArea4.SelectedIndex = 1;

                //verticalSplitterDroit.Panel2Collapsed = true;

                ////splitContainer1.SplitterDistance = this.Size.Width / 2;
                //verticalSplitterGauche.Panel1Collapsed = true;
                //verticalSplitterGauche.Panel2Collapsed = false;

                for (int i = 0; i < this.tabPages.Count; i++)
                {
                    for (int j = 0; j < this.tabPages[i].Controls.Count; j++)
                        this.tabPages[i].Controls[j].Enabled = false;
                }

                this.checkAndOpenTabPage(this.debuggerPage);

                this.cgEeditor1.setDocumentsReadOnlyState(true);
                this.cgEeditor1.Enabled = true;
                this.debuggerPanel1.Enabled = true;

                this.checkAllTabControlEmpty();
            }

            //else if (mode.Equals("GLOBAL"))
            //{
            //    CurrentWorkspace = mode;
            //    voletCentral.Panel1Collapsed = false;
            //    splitContainer4.Panel2Collapsed = true;
            //    voletCentral.Panel2Collapsed = false;
            //    this.tabControlArea4.SelectedIndex = 1;

            //    verticalSplitterDroit.Panel2Collapsed = false;

               
            //    verticalSplitterGauche.Panel1Collapsed = false;
            //    verticalSplitterGauche.Panel2Collapsed = false;

            //    //splitContainer1.SplitterDistance = this.Size.Width / 2;

            //    this.cgEeditor1.setDocumentsReadOnlyState(false);
            //}
            Form1.Resume(this);
            GorgonLibrary.Gorgon.Go();
        }

        private void resolutionsCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.resolutionsCmbBx.SelectedItem != null)
            {
                this.currentTargetResolution = (TargetResolution)this.resolutionsCmbBx.SelectedItem;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------APPLICATION SETTINGS SECTION------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void openAppSettingsPage()
        {
            this.appSettingsManager1.init(this);

            if (this.userSettingsPage.Parent == null)
            {
                this.checkAndOpenTabPage(this.userSettingsPage);
            }

            TabControl controlParent = this.userSettingsPage.Parent as TabControl;

            controlParent.SelectedTab = this.userSettingsPage;

            this.checkAllTabControlEmpty();
        }

        public void closeUserSettingsPage()
        {

            TabControl controlParent = this.userSettingsPage.Parent as TabControl;
            if (controlParent != null)
            {
                controlParent.TabPages.Remove(this.userSettingsPage);

                if (controlParent.TabPages.Count > 0)
                    controlParent.SelectedTab = controlParent.TabPages[0];
            }

            this.checkAllTabControlEmpty();

            
        }

        private void setBackColorToControl(Control control,Color color)
        {
            if (control.Tag != null)
            {
                if (!control.Tag.Equals("false"))
                {
                    for (int i = 0; i < control.Controls.Count; i++)
                    {
                        Control childControl = control.Controls[i];

                        setBackColorToControl(childControl, color);

                    }

                    control.BackColor = color;
                }
            }
           
           
        }

        public void loadUserSettings()
        {
           

            Size applicationSize = Settings1.Default.ApplicationSize;

            if (Settings1.Default.IsFullScreen == true)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.Size = applicationSize;
               
                this.Location = Settings1.Default.ApplicationLocation;
            }

        }

        public void saveUserSettings()
        {
            Size applicationSize =  this.Size;

            this.serializeTabPageLocations();
           
            Settings1.Default.ApplicationSize = applicationSize;
            Settings1.Default.ApplicationLocation = this.Location;
            if (this.WindowState == FormWindowState.Maximized)
                Settings1.Default.IsFullScreen = true;
            else
                Settings1.Default.IsFullScreen = false;

            Settings1.Default.Save();
        }

        private void cGESettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openAppSettingsPage();
        }

        private void closeCGESettingsPage_Click(object sender, EventArgs e)
        {
            this.closeUserSettingsPage();
        }

        private void gameElementsTabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.tabControlArea3.SelectedIndex == 1)
            {
                this.tilesManagerPanel1.refreshObjectTileModelLocations();
                this.tilesManagerPanel1.refreshTextureTileModelLocations();


            }
        }

        private void aboutCGEToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AboutKrea about = new AboutKrea();
            about.ShowDialog();


        }

        private void preferencesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
           
        }

        private void setCameraRectangle_Click(object sender, EventArgs e)
        {
            this.sceneEditorView1.Mode = "CAMERA_RECTANGLE";
            this.SetModeFocus();
            this.sceneEditorView1.Mode = "CAMERA_RECTANGLE";
            

            
        }

        private void goWebSiteBt_Click(object sender, EventArgs e)
        {
            //string target = "http://www.native-software.com/index.php/krea/docs";

            //try
            //{
            //    System.Diagnostics.Process.Start(target);
            //}

            //catch(System.ComponentModel.Win32Exception noBrowser)
            //{
            //    if (noBrowser.ErrorCode == -2147467259)
            //        MessageBox.Show(noBrowser.Message);

            //}
            //catch (System.Exception other)
            //{
            //    MessageBox.Show(other.Message);

            //}

            Help.ShowHelp(this, Path.GetDirectoryName(Application.ExecutablePath) + "\\Help\\HowTo_ENGLISH.chm");
        }

        private void cgEeditor1_Enter(object sender, EventArgs e)
        {
            this.cgEeditor1.IsFocused = true;
        }

        private void cgEeditor1_Leave(object sender, EventArgs e)
        {
            this.cgEeditor1.IsFocused = false;
        }

        private void sceneEditorView1_Enter(object sender, EventArgs e)
        {
            this.sceneEditorView1.IsFocused = true;
            this.sceneEditorView1.surfacePictBx.Focus();
        }

        private void sceneEditorView1_Leave(object sender, EventArgs e)
        {
            this.sceneEditorView1.IsFocused = false;
        }

        private void duplicateObjectBt_Click(object sender, EventArgs e)
        {
            this.gameElementTreeView1.DupplicateSelectedObjects();
        }


        private const int WM_SETREDRAW = 0x000B;

        public static void Suspend(Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero,
                IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        public static void Resume(Control control)
        {
            // Create a C "true" boolean as an IntPtr
            IntPtr wparam = new IntPtr(1);
            Message msgResumeUpdate = Message.Create(control.Handle, WM_SETREDRAW, wparam,
                IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgResumeUpdate);

            control.Invalidate();
            control.Refresh();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            Form1.Suspend(this);
        }

        private void EmulatorModeCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.EmulatorModeCmbBx.SelectedIndex == 0)
            {
                this.buildModeBt.Image = Properties.Resources.releaseModeIcon;
                this.IsDebugMode = false;
            }

            else if (this.EmulatorModeCmbBx.SelectedIndex == 1)
            {
                this.buildModeBt.Image = Properties.Resources.debugModeIcon;
                this.IsDebugMode = true;
            }
        }

        private void buildModeBt_Click(object sender, EventArgs e)
        {

            if (this.EmulatorModeCmbBx.SelectedIndex == 0)
                this.EmulatorModeCmbBx.SelectedIndex = 1;
            else if (this.EmulatorModeCmbBx.SelectedIndex == 1)
                this.EmulatorModeCmbBx.SelectedIndex = 0;
            


            //MessageBox.Show("The Debug mode providing a full Visual Debugger is coming soon!\n We are actually testing and improving this feature to give you the best of Krea!",
            //        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void remoteModeBt_Click(object sender, EventArgs e)
        {
            if (this.remoteModeCmbBx.SelectedIndex == 0)
                this.remoteModeCmbBx.SelectedIndex = 1;
            else if (this.remoteModeCmbBx.SelectedIndex == 1)
                this.remoteModeCmbBx.SelectedIndex = 0;
        }

        private void remoteModeCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.remoteModeCmbBx.SelectedIndex == 0)
            {
                this.remoteModeBt.Image = Properties.Resources.remoteDisabled;
                Settings1.Default.RemoteControlEnabled = false;
            }

            else if (this.remoteModeCmbBx.SelectedIndex == 1)
            {
                this.remoteModeBt.Image = Properties.Resources.remoteEnabled;
                Settings1.Default.RemoteControlEnabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.mainBackWorker.IsBusy == false)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (MessageBox.Show("Do you really want to quit?", "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }

                   
                }

                saveUserSettings();
                this.saveResolutions();

                if (this.CurrentProject != null)
                {
                    //ONLY 
                    e.Cancel = true;
                    this.currentWorkerAction = "ACTION_BUILD_SAVE_CLOSE";
                    this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                }

            }
            
        }

        private void showQridBt_Click(object sender, EventArgs e)
        {
            this.setShowGrid(!Settings1.Default.ShowGrid);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            TabControl controlParent = this.languageManagerPage.Parent as TabControl;
            if (controlParent != null)
            {
                controlParent.TabPages.Remove(this.languageManagerPage);

                if (controlParent.TabPages.Count > 0)
                    controlParent.SelectedTab = controlParent.TabPages[0];
            }

            this.checkAllTabControlEmpty();
        }

        private void collapseAllBt_Click(object sender, EventArgs e)
        {
            if (this.gameElementTreeView1.ProjectRootNodeSelected != null)
            {
                this.gameElementTreeView1.ProjectRootNodeSelected.Collapse(false);
                this.gameElementTreeView1.ProjectRootNodeSelected.Expand();
            }
                
        }

        private void ExpandAllBt_Click(object sender, EventArgs e)
        {
            if (this.gameElementTreeView1.ProjectRootNodeSelected != null)
                this.gameElementTreeView1.ProjectRootNodeSelected.ExpandAll();
        }

        private void preferencesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.openAppSettingsPage();
        }


        private void gameElementTreeView1_Paint(object sender, PaintEventArgs e)
        {
            if (this.CurrentProject == null)
            {
                e.Graphics.DrawString("Open or create a Krea project to see the scenes and the layers content",
                    SystemFonts.DefaultFont, Brushes.CadetBlue, new RectangleF(Point.Empty, this.Bounds.Size));
            }
        }

        private void closeCurrentProjectMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentProject != null)
            {
                this.loadingProjectFilename = "";

                if (this.mainBackWorker.IsBusy == true)
                    this.mainBackWorker.CancelAsync();

                if (this.mainBackWorker.IsBusy == false)
                {

                

                    this.closeAllTabPage();
                    //ONLY 
                    this.currentWorkerAction = "ACTION_BUILD_SAVE_CLEAN";
                    this.mainBackWorker.RunWorkerAsync(this.currentWorkerAction);
                }
            }
        }

       

     //--------------------------------------------------------------- DOCKING TABPAGES ------------------------------------------------------------------------------
        private void tc_MouseDown(object sender, MouseEventArgs e)
        {
            // store clicked tab
            TabControl tc = (TabControl)sender;
            int hover_index = this.getHoverTabIndex(tc);
            if (hover_index >= 0) { tc.Tag = tc.TabPages[hover_index]; }
        }
        private void tc_MouseUp(object sender, MouseEventArgs e)
        {
            // clear stored tab
            TabControl tc = (TabControl)sender;
            tc.Tag = null;
        }
        private void tc_MouseMove(object sender, MouseEventArgs e)
        {
            // mouse button down? tab was clicked?
            TabControl tc = (TabControl)sender;
            if ((e.Button != MouseButtons.Left) || (tc.Tag == null)) return;
            TabPage clickedTab = (TabPage)tc.Tag;
            int clicked_index = tc.TabPages.IndexOf(clickedTab);

            // start drag n drop
            tc.DoDragDrop(clickedTab, DragDropEffects.All);
        }
        private void tc_DragOver(object sender, DragEventArgs e)
        {
            TabControl tabControlDest = (TabControl)sender;

            // a tab is draged?
            if (e.Data.GetData(typeof(TabPage)) == null) return;
            TabPage dragTab = (TabPage)e.Data.GetData(typeof(TabPage));


            TabControl tabControlSrc = dragTab.Parent as TabControl;

            if (tabControlDest == tabControlSrc)
            {
                int dragTab_index = tabControlDest.TabPages.IndexOf(dragTab);

                // hover over a tab?
                int hoverTab_index = this.getHoverTabIndex(tabControlDest);
                if (hoverTab_index < 0) { e.Effect = DragDropEffects.None; return; }
                TabPage hoverTab = tabControlDest.TabPages[hoverTab_index];
                e.Effect = DragDropEffects.Move;

                // start of drag?
                if (dragTab == hoverTab) return;

                // swap dragTab & hoverTab - avoids toggeling
                Rectangle dragTabRect = tabControlDest.GetTabRect(dragTab_index);
                Rectangle hoverTabRect = tabControlDest.GetTabRect(hoverTab_index);

                if (dragTabRect.Width < hoverTabRect.Width)
                {
                    Point tcLocation = tabControlDest.PointToScreen(tabControlDest.Location);

                    if (dragTab_index < hoverTab_index)
                    {
                        if ((e.X - tcLocation.X) > ((hoverTabRect.X + hoverTabRect.Width) - dragTabRect.Width))
                            this.swapTabPages(tabControlDest, dragTab, hoverTab);
                    }
                    else if (dragTab_index > hoverTab_index)
                    {
                        if ((e.X - tcLocation.X) < (hoverTabRect.X + dragTabRect.Width))
                            this.swapTabPages(tabControlDest, dragTab, hoverTab);
                    }
                }
                else this.swapTabPages(tabControlDest, dragTab, hoverTab);

                // select new pos of dragTab
                tabControlDest.SelectedIndex = tabControlDest.TabPages.IndexOf(dragTab);
            }
            else
            {
                this.swapTabPageOverTabControls(dragTab, tabControlSrc, tabControlDest);
            }
        }

        private int getHoverTabIndex(TabControl tc)
        {
            for (int i = 0; i < tc.TabPages.Count; i++)
            {
                if (tc.GetTabRect(i).Contains(tc.PointToClient(Cursor.Position)))
                    return i;
            }

            return -1;
        }

        private void swapTabPages(TabControl tc, TabPage src, TabPage dst)
        {
            int index_src = tc.TabPages.IndexOf(src);
            int index_dst = tc.TabPages.IndexOf(dst);
            tc.TabPages[index_dst] = src;
            tc.TabPages[index_src] = dst;
            tc.Refresh();
        }

        private void swapTabPageOverTabControls(TabPage page, TabControl src, TabControl dst)
        {
            if (src == dst) return;
          
            src.TabPages.Remove(page);
            dst.TabPages.Add(page);

            int indexDestArea = this.getIndexTabControlArea(dst);
            if (indexDestArea != -1)
            {
                StringDictionary dico = this.tabPageLocationsDico;
                if (dico.ContainsKey(page.Name))
                {
                    dico[page.Name] = indexDestArea.ToString() ;
                }
            }

            checkAllTabControlEmpty();

            if (src.TabPages.Count > 0)
                src.SelectedTab = src.TabPages[0];

            //src.Refresh();
            //dst.Refresh();

        }

        private void checkAllTabControlEmpty()
        {
            //this.checkTabControlEmpty(this.tabControlArea1);
            //this.checkTabControlEmpty(this.tabControlArea2);
            //this.checkTabControlEmpty(this.tabControlArea3);
            //this.checkTabControlEmpty(this.tabControlArea4);
            //this.checkTabControlEmpty(this.tabControlArea5);
            //this.checkTabControlEmpty(this.tabControlArea6);

            bool voletGaucheVisible = true;
            bool voletCentralVisible = true;
            bool voletDroitVisible = true;
            //--- VOLET GAUCHE --------------------------------------------------------------------
            bool isUpVoletGaucheEmpty = (this.tabControlArea1.TabPages.Count == 0);
            bool isDownVoletGaucheEmpty = (this.tabControlArea4.TabPages.Count == 0);
            if (isUpVoletGaucheEmpty == true && isDownVoletGaucheEmpty == false)
            {
                this.voletGauche.Visible = true;
               
                this.voletGauche.Panel1Collapsed = true;
                this.voletGauche.Panel2Collapsed = false;

            
            }
            else if (isUpVoletGaucheEmpty == false && isDownVoletGaucheEmpty == true)
            {
                this.voletGauche.Visible = true;

                this.voletGauche.Panel1Collapsed = false;
                this.voletGauche.Panel2Collapsed = true;

              
            }
            else if (isUpVoletGaucheEmpty == true && isDownVoletGaucheEmpty == true)
            {
                this.voletGauche.Visible = false;
                voletGaucheVisible = false;
                this.voletGauche.Panel1Collapsed = true;
                this.voletGauche.Panel2Collapsed = true;
             
            }
            else if (isUpVoletGaucheEmpty == false && isDownVoletGaucheEmpty == false)
            {
                this.voletGauche.Visible = true;

                this.voletGauche.Panel1Collapsed = false;
                this.voletGauche.Panel2Collapsed = false;

            }


            //--- VOLET CENTRAL --------------------------------------------------------------------
            bool isUpVoletCentralEmpty = (this.tabControlArea2.TabPages.Count == 0);
            bool isDownVoletCentralEmpty = (this.tabControlArea5.TabPages.Count == 0);
            if (isUpVoletCentralEmpty == true && isDownVoletCentralEmpty == false)
            {
                this.voletCentral.Visible = true;

                this.voletCentral.Panel1Collapsed = true;
                this.voletCentral.Panel2Collapsed = false;

             
            }
            else if (isUpVoletCentralEmpty == false && isDownVoletCentralEmpty == true)
            {
                this.voletCentral.Visible = true;

                this.voletCentral.Panel1Collapsed = false;
                this.voletCentral.Panel2Collapsed = true;

            
            }
            else if (isUpVoletCentralEmpty == true && isDownVoletCentralEmpty == true)
            {
                this.voletCentral.Visible = false;
                voletCentralVisible = false;
                this.voletCentral.Panel1Collapsed = true;
                this.voletCentral.Panel2Collapsed = true;

              
            }
            else if (isUpVoletCentralEmpty == false && isDownVoletCentralEmpty == false)
            {
                this.voletCentral.Visible = true;

                this.voletCentral.Panel1Collapsed = false;
                this.voletCentral.Panel2Collapsed = false;

            }

            //--- VOLET DROIT --------------------------------------------------------------------
            bool isUpVoletDroitEmpty = (this.tabControlArea3.TabPages.Count == 0);
            bool isDownVoletDroitEmpty = (this.tabControlArea6.TabPages.Count == 0);
            if (isUpVoletDroitEmpty == true && isDownVoletDroitEmpty == false)
            {
                this.voletDroit.Visible = true;

                this.voletDroit.Panel1Collapsed = true;
                this.voletDroit.Panel2Collapsed = false;


            }
            else if (isUpVoletDroitEmpty == false && isDownVoletDroitEmpty == true)
            {
                this.voletDroit.Visible = true;

                this.voletDroit.Panel1Collapsed = false;
                this.voletDroit.Panel2Collapsed = true;


            }
            else if (isUpVoletDroitEmpty == true && isDownVoletDroitEmpty == true)
            {
                this.voletDroit.Visible = false;
                voletDroitVisible = false;
                this.voletDroit.Panel1Collapsed = true;
                this.voletDroit.Panel2Collapsed = true;


            }
            else if (isUpVoletDroitEmpty == false && isDownVoletDroitEmpty == false)
            {
                this.voletDroit.Visible = true;

                this.voletDroit.Panel1Collapsed = false;
                this.voletDroit.Panel2Collapsed = false;

            }


            //-- SPLITTER VISIBILITY ----------------------
            if (voletGaucheVisible == true && 
                voletCentralVisible == false &&
                voletDroitVisible == false)
            {
                this.verticalSplitterGauche.Panel1Collapsed = false;
                this.verticalSplitterGauche.Panel2Collapsed = true;

                this.verticalSplitterDroit.Panel1Collapsed = true;
                this.verticalSplitterDroit.Panel2Collapsed = true;
            }
            else if (voletGaucheVisible == true &&
                     voletCentralVisible == true &&
                     voletDroitVisible == false)
            {
                this.verticalSplitterGauche.Panel1Collapsed = false;
                this.verticalSplitterGauche.Panel2Collapsed = false;

                this.verticalSplitterDroit.Panel1Collapsed = false;
                this.verticalSplitterDroit.Panel2Collapsed = true;
            }

            else if (voletGaucheVisible == false &&
                voletCentralVisible == true &&
                voletDroitVisible == false)
            {
                this.verticalSplitterGauche.Panel1Collapsed = true;
                this.verticalSplitterGauche.Panel2Collapsed = false;

                this.verticalSplitterDroit.Panel1Collapsed = false;
                this.verticalSplitterDroit.Panel2Collapsed = true;
            }
            else if (voletGaucheVisible== false &&
                      voletCentralVisible == true &&
                      voletDroitVisible == true)
            {
                this.verticalSplitterGauche.Panel1Collapsed = true;
                this.verticalSplitterGauche.Panel2Collapsed = false;

                this.verticalSplitterDroit.Panel1Collapsed = false;
                this.verticalSplitterDroit.Panel2Collapsed = false;
            }
            else if (voletGaucheVisible == false &&
                 voletCentralVisible == false &&
                 voletDroitVisible == true)
            {
                this.verticalSplitterGauche.Panel1Collapsed = true;
                this.verticalSplitterGauche.Panel2Collapsed = false;

                this.verticalSplitterDroit.Panel1Collapsed = true;
                this.verticalSplitterDroit.Panel2Collapsed = false;
            }
            else if (voletGaucheVisible == true &&
                voletCentralVisible == false &&
               voletDroitVisible == true)
            {
                this.verticalSplitterGauche.Panel1Collapsed = false;
                this.verticalSplitterGauche.Panel2Collapsed = false;

                this.verticalSplitterDroit.Panel1Collapsed = true;
                this.verticalSplitterDroit.Panel2Collapsed = false;
            }
            else if (voletGaucheVisible == true &&
                      voletCentralVisible == true &&
                      voletDroitVisible == true)
            {
                this.verticalSplitterGauche.Panel1Collapsed = false;
                this.verticalSplitterGauche.Panel2Collapsed = false;

                this.verticalSplitterDroit.Panel1Collapsed = false;
                this.verticalSplitterDroit.Panel2Collapsed = false;
            }
            else if (voletGaucheVisible == false &&
                     voletCentralVisible == false &&
                    voletDroitVisible == false)
            {
                this.verticalSplitterGauche.Panel1Collapsed = true;
                this.verticalSplitterGauche.Panel2Collapsed = true;

                this.verticalSplitterDroit.Panel1Collapsed = true;
                this.verticalSplitterDroit.Panel2Collapsed = true;
            }

           
        }

        private int getIndexTabControlArea(TabControl control)
        {
            int res = -1;

            string indexSTR = control.Name.Replace("tabControlArea", "");
            int.TryParse(indexSTR, out res);

            return res;
        }

        private int getTabPageArea(TabPage page)
        {
            StringDictionary dico = this.tabPageLocationsDico;

            if (dico.ContainsKey(page.Name))
            {
                int res = -1;
                int.TryParse(dico[page.Name], out res);

                return res;
            }

            return -1;
        }


        private void deserializeTabPageLocations()
        {
            StringCollection stringCollection = Settings1.Default.TabPagesPositions;
            if (stringCollection == null)
            {
                stringCollection = new StringCollection();
            }

            if (stringCollection.Count < 18)
            {
                stringCollection.Clear();
                stringCollection.Add("projectTreePage=1");
                stringCollection.Add("filesTreePage=1");

                stringCollection.Add("projectSettingsPage=2");
                stringCollection.Add("tileMapEditorPage=2");
                stringCollection.Add("userSettingsPage=2");
                stringCollection.Add("sceneEditorPage=2");
                stringCollection.Add("collisionManagerPage=2");
                stringCollection.Add("languageManagerPage=2");
                stringCollection.Add("physicsBodyManagerPage=2");

                stringCollection.Add("assetsPage=3");
                stringCollection.Add("tilesManagerPage=3");

                stringCollection.Add("overviewPage=4");
                stringCollection.Add("apiReferencePage=4");
                stringCollection.Add("ircPage=4");

                stringCollection.Add("codeEditorPage=5");
                stringCollection.Add("jointManagerPage=5");
                stringCollection.Add("debuggerPage=5");

                stringCollection.Add("propertiesPage=6");
            }

            this.tabPageLocationsDico = new StringDictionary();
            for(int i =0;i<stringCollection.Count;i++)
            {
                string row = stringCollection[i];
                string key = row.Split('=')[0];
                string value = row.Split('=')[1];
                this.tabPageLocationsDico.Add(key, value);

            }
        }

        private void serializeTabPageLocations()
        {

            StringCollection stringCollection = new StringCollection();
            foreach (string key in this.tabPageLocationsDico.Keys)
            {
                stringCollection.Add(key + "=" + this.tabPageLocationsDico[key]);

            }

            Settings1.Default.TabPagesPositions = stringCollection;
            
        }

        private void refreshTabPagesLocation()
        {
            this.deserializeTabPageLocations();

            tabPages = new List<TabPage>();
            tabPages.Add(this.projectTreePage);
            tabPages.Add(this.filesTreePage);

            tabPages.Add(this.projectSettingsPage);
            tabPages.Add(this.tileMapEditorPage);
            tabPages.Add(this.userSettingsPage);
            tabPages.Add(this.sceneEditorPage);
            tabPages.Add(this.collisionManagerPage);
            tabPages.Add(this.languageManagerPage);
            tabPages.Add(this.physicsBodyManagerPage);

            tabPages.Add(this.assetsPage);
            tabPages.Add(this.tilesManagerPage);

            tabPages.Add(this.overviewPage);
            tabPages.Add(this.apiReferencePage);
        
            tabPages.Add(this.codeEditorPage);
            tabPages.Add(this.jointManagerPage);
            tabPages.Add(this.debuggerPage);

            tabPages.Add(this.propertiesPage);

            for (int i = 0; i < tabPages.Count; i++)
            {
                TabPage page = tabPages[i];
                if (page.Parent != null)
                {
                    TabControl controlParent = page.Parent as TabControl;
                    controlParent.TabPages.Remove(page);

                 
                    controlParent.Refresh();
                }

                this.checkAndOpenTabPage(page);
            }


            

            Settings1.Default.Save();

            this.checkAllTabControlEmpty();
        }


        private void page_MouseDoubleClick(object sender, MouseEventArgs args)
        {
            TabControl controlParent = sender as TabControl;

            TabPage page = controlParent.SelectedTab;
            if (page != null)
            {
               
                controlParent.TabPages.Remove(page);

                if (controlParent.TabPages.Count > 0)
                    controlParent.SelectedTab = controlParent.TabPages[0];

                this.resetTabPageLocation(page);
            }

        }


        private void resetTabPageLocation(TabPage page)
        {
            StringDictionary dico = this.tabPageLocationsDico;


            if (page.Name.Equals("projectTreePage"))
            {
                dico["projectTreePage"] = "1";
            }
            else if (page.Name.Equals("filesTreePage"))
            {
                dico["filesTreePage"] = "1";
            }
           

            else if (page.Name.Equals("projectSettingsPage"))
            {
                dico["projectSettingsPage"] = "2";
            }
            else if (page.Name.Equals("tileMapEditorPage"))
            {
                dico["tileMapEditorPage"] = "2";
            }
            else if (page.Name.Equals("userSettingsPage"))
            {
                dico["userSettingsPage"] = "2";
            }
            else if (page.Name.Equals("sceneEditorPage"))
            {
                dico["sceneEditorPage"] = "2";
            }
            else if (page.Name.Equals("collisionManagerPage"))
            {
                dico["collisionManagerPage"] = "2";
            }
            else if (page.Name.Equals("languageManagerPage"))
            {
                dico["languageManagerPage"] = "2";
            }
            else if (page.Name.Equals("physicsBodyManagerPage"))
            {
                dico["physicsBodyManagerPage"] = "2";
            }
                
        
             
            else if (page.Name.Equals("assetsPage"))
            {
                dico["assetsPage"] = "3";
            }
            else if (page.Name.Equals("tilesManagerPage"))
            {
                dico["tilesManagerPage"] = "3";
            }

            else if (page.Name.Equals("overviewPage"))
            {
                dico["overviewPage"] = "4";
            }
            else if (page.Name.Equals("apiReferencePage"))
            {
                dico["apiReferencePage"] = "4";
            }
            else if (page.Name.Equals("ircPage"))
            {
                dico["ircPage"] = "4";
            }

            else if (page.Name.Equals("codeEditorPage"))
            {
                dico["codeEditorPage"] = "5";
            }
            else if (page.Name.Equals("jointManagerPage"))
            {
                dico["jointManagerPage"] = "5";
            }
            else if (page.Name.Equals("debuggerPage"))
            {
                dico["debuggerPage"] = "5";
            }

            else if (page.Name.Equals("propertiesPage"))
            {
                dico["propertiesPage"] = "6";
            }

            this.checkAndOpenTabPage(page);

            this.checkAllTabControlEmpty();
        }

        private void checkAndOpenTabPage(TabPage page)
        {
            StringDictionary dico = this.tabPageLocationsDico;

            int destArea = this.getTabPageArea(page);

            TabControl controlDest = null;
            if (destArea == 1)
                controlDest = this.tabControlArea1;
            else if (destArea == 2)
                controlDest = this.tabControlArea2;
            else if (destArea == 3)
                controlDest = this.tabControlArea3;
            else if (destArea == 4)
                controlDest = this.tabControlArea4;
            else if (destArea == 5)
                controlDest = this.tabControlArea5;
            else if (destArea == 6)
                controlDest = this.tabControlArea6;

            if (controlDest != null)
            {
                if (!controlDest.TabPages.Contains(page))
                {
                    controlDest.TabPages.Add(page);
                }

                controlDest.Refresh();
            }

         
          
        }

        private void checkTabControlEmpty(TabControl ctrl)
        {
            if (ctrl.TabPages.Count > 0)
            {
                if (ctrl.Name.Contains("1"))
                {
                    voletGauche.Panel1Collapsed = false;
                }
                else if (ctrl.Name.Contains("2"))
                {
                    voletCentral.Panel1Collapsed = false;
                }
                else if (ctrl.Name.Contains("3"))
                {
                    voletDroit.Panel1Collapsed = false;
                }
                else if (ctrl.Name.Contains("4"))
                {
                    voletGauche.Panel2Collapsed = false;
                }
                else if (ctrl.Name.Contains("5"))
                {
                    voletCentral.Panel2Collapsed = false;
                }
                else if (ctrl.Name.Contains("6"))
                {
                    voletDroit.Panel2Collapsed = false;
                }
            }
            else
            {
                if (ctrl.Name.Contains("1"))
                {
                    voletGauche.Panel1Collapsed = true;
                }
                else if (ctrl.Name.Contains("2"))
                {
                    voletCentral.Panel1Collapsed = true;
                }
                else if (ctrl.Name.Contains("3"))
                {
                    voletDroit.Panel1Collapsed = true;
                }
                else if (ctrl.Name.Contains("4"))
                {
                    voletGauche.Panel2Collapsed = true;
                }
                else if (ctrl.Name.Contains("5"))
                {
                    voletCentral.Panel2Collapsed = true;
                }
                else if (ctrl.Name.Contains("6"))
                {
                    voletDroit.Panel2Collapsed = true;
                }
            }

         
        }

        private void resetViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Suspend(this);
            Settings1.Default.TabPagesPositions = null;

            this.refreshTabPagesLocation();
            this.closeAllTabPage();

            Resume(this);

            GorgonLibrary.Gorgon.Go();
        }

      

        private void appSettingsManager1_Load(object sender, EventArgs e)
        {

        }

        

       

       


        

    }
}
