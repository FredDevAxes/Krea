using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using Krea.CoronaClasses;
using Krea.Corona_Classes;
using System.Reflection;
using Krea.GameEditor.FontManager;
using System.Diagnostics;

namespace Krea.Asset_Manager
{
    public partial class AssetManagerForm : Form
    {
        public AssetsToSerialize CurrentAssetProject;
        private ImageList assetsImageList;
        private List<UserControl> selectedObjectPanels;

        [ObfuscationAttribute(Exclude = true)]
        public enum AssetType
        {
            Image = 1,
            Sprite = 2,
            SpriteSet = 3,
            SpriteSheet = 4,
            Audio = 5,
            Snippet = 6,
            Font = 7,
        }

        private AssetType assetTypeSelected;

        public AssetManagerForm()
        {
            InitializeComponent();

            this.splitContainer2.SplitterWidth = 15;
            this.voletDroitSplitContainer.SplitterWidth = 15;
            this.voletGaucheSplitContainer.SplitterWidth = 15;
            this.principalSplitContainer.SplitterWidth = 15;
            selectedObjectPanels = new List<UserControl>();
            init();
        }

        private void init()
        {
            initAssetProjectsCmbBx();
            RefreshAssetListView();
        }

        private void initAssetProjectsCmbBx()
        {
            try
            {

                
                this.projectsCmbBx.Items.Clear();
                this.projectsCmbBx.Text = "";
                if (this.CurrentAssetProject != null)
                {
                    this.CurrentAssetProject.clean();
                    this.CurrentAssetProject = null;
                }

                //-----------------------------Verifier que le repertoire des assets est bien crée ---------------------------------

                string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                if (!Directory.Exists(documentsDirectory))
                    Directory.CreateDirectory(documentsDirectory);


                if (!Directory.Exists(documentsDirectory + "\\Asset Manager"))
                    Directory.CreateDirectory(documentsDirectory + "\\Asset Manager");

                //Recuperer le repertoire de l'asset manager
                DirectoryInfo dir = new DirectoryInfo(documentsDirectory + "\\Asset Manager\\");
                foreach (DirectoryInfo dirProject in dir.GetDirectories())
                {
                    FileInfo[] tabFileInfo = dirProject.GetFiles("*.kres");
                    if (tabFileInfo.Length == 0)
                    {
                        //Create one
                        AssetsToSerialize assets = new AssetsToSerialize();
                        assets.ProjectName = dirProject.Name;
                        this.CurrentAssetProject = assets;
                        this.saveCurrentAssetProject();
                        this.CurrentAssetProject = null;

                        this.projectsCmbBx.Items.Add(dirProject.Name);
                    }
                    else
                    {
                        for (int i = 0; i < tabFileInfo.Length; i++)
                        {
                            this.projectsCmbBx.Items.Add(dirProject.Name);
                        }
                  
                    }
                
                }

                if (this.projectsCmbBx.Items.Count > 0)
                    this.projectsCmbBx.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR OCCURS DURING LOADING ! \n " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private UserControl getObjectPanelFromObject(object obj)
        {
            for (int i = 0; i < this.selectedObjectPanels.Count; i++)
            {
                if (this.selectedObjectPanels[i].Tag == obj)
                    return this.selectedObjectPanels[i];
            }

            return null;
        }

        private void hideAllObjectsPanel()
        {
            for (int i = 0; i < this.selectedObjectPanels.Count; i++)
            {
                this.selectedObjectPanels[i].Visible = false;

            }
        }

        private void addNewControlToObjectsPanel(UserControl control)
        {
            if (!this.selectedObjectPanels.Contains(control))
            {
                this.selectedObjectPanels.Add(control);
                this.selectedAssetTabPage.Controls.Add(control);

                refreshSelectedObjectListView();
            }

        }

        public void RemoveControlFromObjectsPanel(UserControl control)
        {
            if (this.selectedObjectPanels.Contains(control))
            {
                this.selectedObjectPanels.Remove(control);
                this.selectedAssetTabPage.Controls.Remove(control);

                control.Dispose();
                refreshSelectedObjectListView();
            }
        }

        private void openPanelFromObject(object objectToOpen)
        {

           
            if (objectToOpen is DisplayObject)
            {
                DisplayObject obj = (DisplayObject)objectToOpen;
                if (obj.Type.Equals("IMAGE"))
                {
                    this.openImageManagerFromObject(obj);
                }
                else if (obj.Type.Equals("SPRITE"))
                {
                   this.openSpriteManagerFromObject(obj);
                }
            }
            else if (objectToOpen is CoronaSpriteSheet)
            {

                CoronaSpriteSheet sheet = (CoronaSpriteSheet)objectToOpen;

                //Check if no sprite set or sprite panel is already opened
                bool canOpen = true;
                for (int i = 0; i < this.selectedObjectPanels.Count; i++)
                {
                    object obj = this.selectedObjectPanels[i].Tag;
                    if (obj is CoronaSpriteSet)
                    {
                        CoronaSpriteSet set = (CoronaSpriteSet)obj;
                        for (int j = 0; j < set.Frames.Count; j++)
                        {
                            if (set.Frames[j].SpriteSheetParent == sheet)
                            {
                                canOpen = false;
                                break;
                            }
                        }

                        if (canOpen == false)
                            break;
                    }
                }

                if(canOpen == true)
                    this.openSpriteSheetManagerFromObject(sheet);
                else
                    MessageBox.Show("Please close all Sprite and SpriteSet panels using a frame of "+sheet.Name+" before continuing!", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (objectToOpen is CoronaSpriteSet)
            {
                CoronaSpriteSet set = (CoronaSpriteSet)objectToOpen;

                //Check if no sprite set or sprite panel is already opened
                bool canOpen = true;
                for (int i = 0; i < this.selectedObjectPanels.Count; i++)
                {
                    object obj = this.selectedObjectPanels[i].Tag;
                    if (obj is DisplayObject)
                    {
                        DisplayObject dispObject = (DisplayObject)obj;
                        if (dispObject.Type.Equals("SPRITE"))
                        {
                            if (dispObject.SpriteSet == set)
                            {
                                canOpen = false;
                                break;
                            }
                                
                        } 
                    }
                }

                if (canOpen == true)
                    this.openSpriteSetsManagerFromObject(set);
                else
                    MessageBox.Show("Please close all Sprite panels using " + set.Name + " before continuing!", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (objectToOpen is AudioObject)
            {
                AudioObject audio = (AudioObject)objectToOpen;
                this.openAudioManagerPanelFromObject(audio);
            }
            else if (objectToOpen is Snippet)
            {
                Snippet snippet = (Snippet)objectToOpen;
                this.openSnippetManagerPanelFromObject(snippet);
            }
            else if (objectToOpen is FontItem)
            {
                FontItem font = (FontItem)objectToOpen;
                this.openFontManagerPanelFromObject(font);
            }
        }

        public void openSpriteManagerFromObject(DisplayObject obj)
        {
            hideAllObjectsPanel();

            //Si le tab est deja ouvert
            SpriteManagerPanel panel = null;
            UserControl page = getObjectPanelFromObject(obj);
            if (page != null)
            {
                page.Visible = true;
                panel = (SpriteManagerPanel)page;
            }
            //Sinn
            else
            {
                //Create a spite manager panel
                panel = new SpriteManagerPanel();
                panel.init(this, obj);
                panel.Dock = DockStyle.Fill;
                panel.Tag = obj;

                addNewControlToObjectsPanel(panel);

                panel.Visible = true;
            }

            panel.DisplayObjectProperties();
        }

        public void openSpriteSheetManagerFromObject(CoronaSpriteSheet spriteSheet)
        {
            hideAllObjectsPanel();

            //Si le tab est deja ouvert
            SpriteSheetManagerPanel panel = null;
            UserControl page = getObjectPanelFromObject(spriteSheet);
            if (page != null)
            {
                page.Visible = true;
                panel = (SpriteSheetManagerPanel)page;
            }
            //Sinn
            else
            {
                //Create a spite manager panel
                panel = new SpriteSheetManagerPanel();
                panel.init(this, spriteSheet);
                panel.Dock = DockStyle.Fill;
                panel.Tag = spriteSheet;
                addNewControlToObjectsPanel(panel);
                panel.Visible = true;
            }

            panel.DisplayObjectProperties();
        }

        public void openSpriteSetsManagerFromObject(CoronaSpriteSet spriteSet)
        {
            hideAllObjectsPanel();

            //Si le tab est deja ouvert
            SpriteSetManagerPanel panel = null;
            UserControl page = getObjectPanelFromObject(spriteSet);
            if (page != null)
            {
                page.Visible = true;
                panel = (SpriteSetManagerPanel)page;
            }
            //Sinn
            else
            {
                //Create a spite manager panel
                panel = new SpriteSetManagerPanel();
                panel.init(this, spriteSet);
                panel.Dock = DockStyle.Fill;
                panel.Tag = spriteSet;

                addNewControlToObjectsPanel(panel);
                panel.DisplayObjectProperties();
                panel.Visible = true;
            }

            panel.DisplayObjectProperties();
        }

        public void openImageManagerFromObject(DisplayObject obj)
        {
            //Si le tab est deja ouvert
            hideAllObjectsPanel();

            //Si le tab est deja ouvert
            ImageManagerPanel panel = null;
            UserControl page = getObjectPanelFromObject(obj);
            if (page != null)
            {
                page.Visible = true;
                panel = (ImageManagerPanel)page;
            }
            //Sinn
            else
            {
                //Create a tabPageImage panel
                panel = new ImageManagerPanel();
                panel.init(obj, this);
                panel.Dock = DockStyle.Fill;
                panel.Tag = obj;

                addNewControlToObjectsPanel(panel);
                panel.Visible = true;
            }

            panel.DisplayObjectProperties();

        }

        public void openAudioManagerPanelFromObject(AudioObject obj)
        {
            hideAllObjectsPanel();

            //Si le tab est deja ouvert
            AudioManagerPanel panel = null;
            UserControl page = getObjectPanelFromObject(obj);
            if (page != null)
            {
                page.Visible = true;
                panel = (AudioManagerPanel)page;
            }
            //Sinn
            else
            {
                //Create a tabPageImage panel
                panel = new AudioManagerPanel(obj, this);
                panel.Dock = DockStyle.Fill;
                panel.Tag = obj;
                addNewControlToObjectsPanel(panel);
                panel.Visible = true;
            }
        }

        public void openFontManagerPanelFromObject(FontItem obj)
        {
            hideAllObjectsPanel();

            //Si le tab est deja ouvert
            FontManagerPanel panel = null;
            UserControl page = getObjectPanelFromObject(obj);
            if (page != null)
            {
                page.Visible = true;
                panel = (FontManagerPanel)page;
            }
            //Sinn
            else
            {
                //Create a tabPageImage panel
                panel = new FontManagerPanel(obj, this);
                panel.Dock = DockStyle.Fill;
                panel.Tag = obj;
                addNewControlToObjectsPanel(panel);
                panel.Visible = true;
            }
        }
        public void openSnippetManagerPanelFromObject(Snippet snippet)
        {
            hideAllObjectsPanel();

            //Si le tab est deja ouvert
            SnippetManagerPanel panel = null;
            UserControl page = getObjectPanelFromObject(snippet);
            if (page != null)
            {
                page.Visible = true;
                panel = (SnippetManagerPanel)page;
            }
            //Sinn
            else
            {
                //Create a tabPageImage panel
                panel = new SnippetManagerPanel();
                panel.init(this, snippet);
                panel.Dock = DockStyle.Fill;
                panel.Tag = snippet;
                addNewControlToObjectsPanel(panel);
                panel.Visible = true;
            }

            panel.DisplayObjectProperties();
        }

        private void projectsCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.projectsCmbBx.SelectedItem != null)
            {
                 if (this.CurrentAssetProject != null)
                {
                    this.saveCurrentAssetProject();
                    this.CurrentAssetProject.clean();
                    this.CurrentAssetProject = null;
                }


                string projectName = this.projectsCmbBx.SelectedItem.ToString();

                string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                if (!Directory.Exists(documentsDirectory))
                    Directory.CreateDirectory(documentsDirectory);


                string projectFullPath = documentsDirectory + "\\Asset Manager\\" + projectName;
                if (Directory.Exists(projectFullPath))
                {
                    if (File.Exists(projectFullPath + "\\" + projectName + ".kres"))
                    {
                        try
                        {
                            //Creer une hierarchie meme vide
                            AssetsToSerialize assets = new AssetsToSerialize();

                            //Recuperer les instnces de chaque Asset 
                            FileStream fs = File.OpenRead(projectFullPath + "\\" + projectName + ".kres");
                            if (fs.Length > 0)
                            {
                                MemoryStream ms = new MemoryStream();
                                ms.SetLength(fs.Length);

                                fs.Read(ms.GetBuffer(), 0, (int)ms.Length);

                                assets = (AssetsToSerialize)SerializerHelper.DeSerializeBinary(ms);
                                ms.Close();
                            }
                            fs.Close();
                            assets.ProjectName = projectName;

                            this.CurrentAssetProject = assets;

                            this.RefreshAssetListView();
                        }
                        catch (Exception ex)
                        {
                               MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
            }
        }

        private void refreshSelectedObjectListView()
        {

            this.selectedObjectPanelsListView.BeginUpdate();
            this.selectedObjectPanelsListView.Items.Clear();

            if (selectedObjectPanelsListView.LargeImageList != null)
            {
                selectedObjectPanelsListView.LargeImageList.Dispose();
                selectedObjectPanelsListView.LargeImageList = null;
            }

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            this.selectedObjectPanelsListView.LargeImageList = imageList;

            for (int i = 0; i < this.selectedObjectPanels.Count; i++)
            {
                UserControl control = this.selectedObjectPanels[i];
                if (control.Tag is DisplayObject)
                {
                    DisplayObject obj = (DisplayObject)control.Tag;
                    if (obj.Type.Equals("IMAGE"))
                    {
                        if (obj.Image != null)
                            imageList.Images.Add(obj.Image);
                        else
                            imageList.Images.Add(Properties.Resources.bmpFileIcon);

                        ListViewItem item = new ListViewItem(obj.Name, imageList.Images.Count - 1);
                        item.Tag = obj;
                        this.selectedObjectPanelsListView.Items.Add(item);
                    }
                    else if (obj.Type.Equals("SPRITE"))
                    {
                        if (obj.SpriteSet.Frames.Count > 0)
                            imageList.Images.Add(obj.SpriteSet.Frames[0].Image);
                        else
                            imageList.Images.Add(Properties.Resources.spriteIcon);

                        ListViewItem item = new ListViewItem(obj.Name, imageList.Images.Count - 1);
                        item.Tag = obj;
                        this.selectedObjectPanelsListView.Items.Add(item);
                    }
                }
                else if (control.Tag is CoronaSpriteSheet)
                {

                    CoronaSpriteSheet sheet = (CoronaSpriteSheet)control.Tag ;
                    if (sheet.Frames.Count > 0)
                        imageList.Images.Add(sheet.Frames[0].Image);
                    else
                        imageList.Images.Add(Properties.Resources.spriteSheetIcon);

                    ListViewItem item = new ListViewItem(sheet.Name, imageList.Images.Count - 1);
                    item.Tag = sheet;
                    this.selectedObjectPanelsListView.Items.Add(item);

                }
                else if (control.Tag is CoronaSpriteSet)
                {
                    CoronaSpriteSet set = (CoronaSpriteSet) control.Tag;

                    if (set.Frames.Count > 0)
                        imageList.Images.Add(set.Frames[0].Image);
                    else
                        imageList.Images.Add(Properties.Resources.spriteSetIcon);

                    ListViewItem item = new ListViewItem(set.Name, imageList.Images.Count - 1);
                    item.Tag = set;
                    this.selectedObjectPanelsListView.Items.Add(item);
                }
                else if (control.Tag is AudioObject)
                {
                    AudioObject audio = (AudioObject)control.Tag;

                    imageList.Images.Add(Properties.Resources.audioIcon);

                    ListViewItem item = new ListViewItem(audio.name, imageList.Images.Count - 1);
                    item.Tag = audio;
                    this.selectedObjectPanelsListView.Items.Add(item);
                }
                else if (control.Tag is Snippet)
                {
                    Snippet snippet = (Snippet)control.Tag;
                    imageList.Images.Add(Properties.Resources.snippetIcon);

                    ListViewItem item = new ListViewItem(snippet.Name, imageList.Images.Count - 1);
                    item.Tag = snippet;
                    this.selectedObjectPanelsListView.Items.Add(item);
                }
            }

            this.selectedObjectPanelsListView.EndUpdate();

        }

        public void RefreshAssetListView()
        {
            if (assetsImageList != null)
                assetsImageList.Dispose();

            this.propertyGrid1.SelectedObject = null;

            this.assetsListView.BeginUpdate();
            this.assetsListView.Items.Clear();
            
            if (this.CurrentAssetProject != null && !this.assetTypeSelected.Equals(""))
            {
                assetsImageList = new ImageList();
                assetsImageList.ImageSize = new Size(32,32);
                this.assetsListView.LargeImageList = assetsImageList;
                if (this.assetTypeSelected == AssetType.Image)
                {
                    for (int i = 0; i < this.CurrentAssetProject.ListObjects.Count; i++)
                    {
                        DisplayObject obj = this.CurrentAssetProject.ListObjects[i];
                        if (obj.Type.Equals("IMAGE"))
                        {
                            assetsImageList.Images.Add(obj.Image);
                            ListViewItem item = new ListViewItem(obj.Name, assetsImageList.Images.Count-1);
                            item.Tag = obj;
                            this.assetsListView.Items.Add(item);
                        }
                    }
                }
                else if (this.assetTypeSelected == AssetType.Sprite)
                {
                    for (int i = 0; i < this.CurrentAssetProject.ListObjects.Count; i++)
                    {
                        DisplayObject obj = this.CurrentAssetProject.ListObjects[i];
                        if (obj.Type.Equals("SPRITE"))
                        {
                            if(obj.SpriteSet.Frames.Count>0)
                                assetsImageList.Images.Add(obj.SpriteSet.Frames[0].Image);
                            else
                                assetsImageList.Images.Add(Properties.Resources.spriteIcon);

                            ListViewItem item = new ListViewItem(obj.Name, assetsImageList.Images.Count - 1);
                            item.Tag = obj;
                            this.assetsListView.Items.Add(item);
                        }
                    }
                }
                else if (this.assetTypeSelected == AssetType.SpriteSet)
                {
                    for (int i = 0; i < this.CurrentAssetProject.SpriteSets.Count; i++)
                    {
                        CoronaSpriteSet set = this.CurrentAssetProject.SpriteSets[i];

                        if (set.Frames.Count > 0)
                            assetsImageList.Images.Add(set.Frames[0].Image);
                        else
                            assetsImageList.Images.Add(Properties.Resources.spriteSetIcon);

                        ListViewItem item = new ListViewItem(set.Name, assetsImageList.Images.Count - 1);
                        item.Tag = set;
                        this.assetsListView.Items.Add(item);
                    }
                }
                else if (this.assetTypeSelected == AssetType.SpriteSheet)
                {
                    for (int i = 0; i < this.CurrentAssetProject.SpriteSheets.Count; i++)
                    {
                        CoronaSpriteSheet sheet = this.CurrentAssetProject.SpriteSheets[i];
                        if (sheet.Frames.Count > 0)
                        {
                            Image frame = sheet.Frames[0].Image;
                            assetsImageList.Images.Add(frame);
                        }
                        else
                        {
                            assetsImageList.Images.Add(Properties.Resources.spriteSheetIcon);
                        }


                        ListViewItem item = new ListViewItem(sheet.Name, assetsImageList.Images.Count - 1);
                        item.Tag = sheet;
                        this.assetsListView.Items.Add(item);
                    }
                }
                else if (this.assetTypeSelected == AssetType.Audio)
                {
                    for (int i = 0; i < this.CurrentAssetProject.Audios.Count; i++)
                    {
                        AudioObject audio = this.CurrentAssetProject.Audios[i];

                        assetsImageList.Images.Add(Properties.Resources.audioIcon);

                        ListViewItem item = new ListViewItem(audio.name, assetsImageList.Images.Count - 1);
                        item.Tag = audio;
                        this.assetsListView.Items.Add(item);
                    }
                }
                else if (this.assetTypeSelected == AssetType.Snippet)
                {
                    for (int i = 0; i < this.CurrentAssetProject.Snippets.Count; i++)
                    {
                        Snippet snippet = this.CurrentAssetProject.Snippets[i];
                        assetsImageList.Images.Add(Properties.Resources.snippetIcon);

                        ListViewItem item = new ListViewItem(snippet.Name, assetsImageList.Images.Count - 1);
                        item.Tag = snippet;
                        this.assetsListView.Items.Add(item);
                    }
                }
                else if (this.assetTypeSelected == AssetType.Font)
                {
                    if (this.CurrentAssetProject.Fonts == null) this.CurrentAssetProject.Fonts = new List<FontItem>();
                    for (int i = 0; i < this.CurrentAssetProject.Fonts.Count; i++)
                    {
                        FontItem font = this.CurrentAssetProject.Fonts[i];
                        assetsImageList.Images.Add(Properties.Resources.ttf);

                        ListViewItem item = new ListViewItem(font.NameForAndroid, assetsImageList.Images.Count - 1);
                        item.Tag = font;
                        this.assetsListView.Items.Add(item);
                    }
                }

            }
            this.assetsListView.EndUpdate();
        }

        
        private void removeCurrentAssetProject()
        {
            try
            {
                if (this.CurrentAssetProject != null)
                {
                    DialogResult rs = MessageBox.Show("Are you sure to delete this asset project ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.Yes)
                    {
                        string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                        if (!Directory.Exists(documentsDirectory))
                            Directory.CreateDirectory(documentsDirectory);

                        string pathDirProject = documentsDirectory + "\\Asset Manager\\" + this.CurrentAssetProject.ProjectName;
                        if (Directory.Exists(pathDirProject))
                        {
                            Directory.Delete(pathDirProject, true);

                        }

                        init();
                        MessageBox.Show("Project successfully deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurs during deleting current project! \n " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }


        private void saveCurrentAssetProject()
        {
            try
            {
                if (this.CurrentAssetProject != null)
                {
                    string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                    if (!Directory.Exists(documentsDirectory))
                        Directory.CreateDirectory(documentsDirectory);

                    string pathDirProject = documentsDirectory + "\\Asset Manager\\" + this.CurrentAssetProject.ProjectName;
                    if(!Directory.Exists(pathDirProject))
                    {
                        Directory.CreateDirectory(pathDirProject);
                    }

                    DirectoryInfo dir = new DirectoryInfo(pathDirProject);

                    String pathFileDest = dir.FullName + "\\" + CurrentAssetProject.ProjectName + ".kres";

                    FileStream fs = File.Create(pathFileDest);
                    MemoryStream ms = SerializerHelper.SerializeBinary(CurrentAssetProject);

                    fs.Write(ms.GetBuffer(), 0, (int)ms.Length);
                    ms.Flush();
                    fs.Close();

                }
                else
                {
                    MessageBox.Show("No project selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR OCCURS DURING SAVING ! \n " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

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
        //    LinearGradientBrush bBackground = new LinearGradientBrush(rBackground, colorGradient1, colorGradient2, direction);

        //    // Draw the gradient onto the form
        //    g.FillRectangle(bBackground, rBackground);

        //    // Disposing of the resources held by the brush
        //    bBackground.Dispose();
        //}

        private void assetTypeListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentAssetProject != null && this.assetTypeListView.SelectedItems.Count>0)
            {
                string typeSelected = this.assetTypeListView.SelectedItems[0].ToolTipText;
                if (typeSelected.Equals("Image"))
                    this.assetTypeSelected = AssetType.Image;
                else if (typeSelected.Equals("Sprite"))
                    this.assetTypeSelected = AssetType.Sprite;
                else if (typeSelected.Equals("Sprite Set"))
                    this.assetTypeSelected = AssetType.SpriteSet;
                else if (typeSelected.Equals("Sprite Sheet"))
                    this.assetTypeSelected = AssetType.SpriteSheet;
                else if (typeSelected.Equals("Audio"))
                    this.assetTypeSelected = AssetType.Audio;
                else if (typeSelected.Equals("Snippet"))
                    this.assetTypeSelected = AssetType.Snippet;
                else if (typeSelected.Equals("Font"))
                    this.assetTypeSelected = AssetType.Font;

                this.RefreshAssetListView();
            }
        }

        private void newAssetBt_Click(object sender, EventArgs e)
        {
            if (this.CurrentAssetProject != null)
            {
                if (this.assetTypeListView.SelectedItems.Count > 0)
                {
                    string assetTypeSelected = this.assetTypeListView.SelectedItems[0].ToolTipText;
                    object objectCreated = null;
                    if (assetTypeSelected.Equals("Image"))
                    {
                        DisplayObject obj = new DisplayObject(new Bitmap(Properties.Resources.bmpFileIcon), new Point(0, 0), null);
                        obj.Name = "image" + this.CurrentAssetProject.ListObjects.Count;
                        this.CurrentAssetProject.ListObjects.Add(obj);
                        objectCreated = obj;
                       

                    }
                    else if (assetTypeSelected.Equals("Sprite"))
                    {
                        DisplayObject obj = new DisplayObject(new CoronaSpriteSet(""), new Point(0, 0),null);
                        obj.Name = "sprite" + this.CurrentAssetProject.ListObjects.Count;
                        this.CurrentAssetProject.ListObjects.Add(obj);
                        objectCreated = obj;
                    }
                    else if (assetTypeSelected.Equals("Sprite Set"))
                    {
                        CoronaSpriteSet obj = new CoronaSpriteSet("Spriteset"+this.CurrentAssetProject.SpriteSets.Count);
                        this.CurrentAssetProject.SpriteSets.Add(obj);
                        objectCreated = obj;
                    }
                    else if (assetTypeSelected.Equals("Sprite Sheet"))
                    {
                        CoronaSpriteSheet obj = new CoronaSpriteSheet("Spritesheet" + this.CurrentAssetProject.SpriteSheets.Count);
                        this.CurrentAssetProject.SpriteSheets.Add(obj);
                        objectCreated = obj;
                    }
                    else if (assetTypeSelected.Equals("Audio"))
                    {
                        AudioObject obj = new AudioObject("", "Audio" + this.CurrentAssetProject.SpriteSheets.Count, 1, false, 1, "SOUND");
                        this.CurrentAssetProject.Audios.Add(obj);
                        objectCreated = obj;
                    }
                    else if (assetTypeSelected.Equals("Snippet"))
                    {
                        Snippet obj = new Snippet("snippet" + this.CurrentAssetProject.Snippets.Count,"", "", "", "", 1, "");
                        this.CurrentAssetProject.Snippets.Add(obj);
                        objectCreated = obj;
                    }
                    else if (assetTypeSelected.Equals("Font"))
                    {
                        if(this.CurrentAssetProject.Fonts == null)
                            this.CurrentAssetProject.Fonts = new List<FontItem>();

                        FontItem obj = new FontItem("font" + this.CurrentAssetProject.Fonts.Count, null);
                        this.CurrentAssetProject.Fonts.Add(obj);
                        objectCreated = obj;
                    }

                    if (objectCreated != null)
                    {
                        this.RefreshAssetListView();
                        this.openPanelFromObject(objectCreated);
                    }
                        
                }

                
            }
        }

        private void assetsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.CurrentAssetProject != null)
            {
                if (this.assetsListView.SelectedItems.Count > 0)
                {

                    object obj = this.assetsListView.SelectedItems[0].Tag;
                    this.openPanelFromObject(obj);

                }
            }
        }

        private void saveCurrentProjectBt_Click(object sender, EventArgs e)
        {
            this.saveCurrentAssetProject();
        }

        private void removeCurrentProjectBt_Click(object sender, EventArgs e)
        {
            this.removeCurrentAssetProject();
        }


        private void removeTabFromObject(object obj)
        {
            UserControl[] controls = new UserControl[this.selectedObjectPanels.Count];
            this.selectedObjectPanels.CopyTo(controls);
            for (int i = 0; i < controls.Length; i++)
            {
                if(controls[i].Tag == obj)
                {
                     if (controls[i] is SpriteManagerPanel)
                    {
                        SpriteManagerPanel panel = (SpriteManagerPanel)controls[i];
                        panel.Clean();
                        panel = null;
                    }
                    else if (controls[i] is ImageManagerPanel)
                    {
                        ImageManagerPanel panel = (ImageManagerPanel)controls[i];
                        panel.Clean();
                        panel = null;
                    }
                    else if (controls[i] is AudioManagerPanel)
                    {
                        AudioManagerPanel panel = (AudioManagerPanel)controls[i];
                        panel.Clean();
                        panel = null;
                    }
                    else if (controls[i] is SnippetManagerPanel)
                    {
                        SnippetManagerPanel panel = (SnippetManagerPanel)controls[i];
                        panel.Clean();
                        panel = null;

                    }
                    else if (controls[i] is SpriteSetManagerPanel)
                    {
                        SpriteSetManagerPanel panel = (SpriteSetManagerPanel)controls[i];
                        panel.Clean();
                        panel = null;
                    }

                    else if (controls[i] is SpriteSheetManagerPanel)
                    {
                        SpriteSheetManagerPanel panel = (SpriteSheetManagerPanel)controls[i];
                        panel.Clean();
                        panel = null;
                    }

                     this.RefreshAssetListView();

                     break;
                }
               
            }
        }

        private void removeCurrentAssetBt_Click(object sender, EventArgs e)
        {
            if (this.CurrentAssetProject != null)
            {
                if (this.assetsListView.SelectedItems.Count > 0)
                {
                    
                    string assetTypeSelected = this.assetTypeListView.SelectedItems[0].ToolTipText;
                    if (assetTypeSelected.Equals("Image"))
                    {
                        DisplayObject obj = (DisplayObject)this.assetsListView.SelectedItems[0].Tag;
                        this.removeDisplayObject(obj);

                    }
                    else if (assetTypeSelected.Equals("Sprite"))
                    {
                        DisplayObject obj = (DisplayObject)this.assetsListView.SelectedItems[0].Tag;
                        this.removeDisplayObject(obj);
                    }
                    else if (assetTypeSelected.Equals("Sprite Set"))
                    {
                        CoronaSpriteSet obj = (CoronaSpriteSet)this.assetsListView.SelectedItems[0].Tag;

                        DialogResult rs = MessageBox.Show("Removing this Sprite set will also also remove all resources using it!\nContinue ?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.Yes)
                            this.removeSpriteSet(obj);
                    }
                    else if (assetTypeSelected.Equals("Sprite Sheet"))
                    {
                        CoronaSpriteSheet obj = (CoronaSpriteSheet)this.assetsListView.SelectedItems[0].Tag;

                        DialogResult rs = MessageBox.Show("Removing this Sprite sheet will also also remove all resources using it!\nContinue ?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(rs == System.Windows.Forms.DialogResult.Yes)
                            this.removeSpriteSheet(obj);
                    }
                    else if (assetTypeSelected.Equals("Audio"))
                    {
                        AudioObject obj = (AudioObject)this.assetsListView.SelectedItems[0].Tag;
                        this.removeAudioObject(obj);
                    }
                    else if (assetTypeSelected.Equals("Snippet"))
                    {
                        Snippet obj = (Snippet)this.assetsListView.SelectedItems[0].Tag;
                        this.removeSnippet(obj);
                    }
                    else if (assetTypeSelected.Equals("Font"))
                    {
                        FontItem obj = (FontItem)this.assetsListView.SelectedItems[0].Tag;
                        this.removeFontObject(obj);
                    }


                    this.removeTabFromObject(this.assetsListView.SelectedItems[0].Tag);
                    this.RefreshAssetListView();
                }
            }
        }

        public void removeDisplayObject(DisplayObject obj)
        {
            if (this.CurrentAssetProject != null)
            {
                if (this.CurrentAssetProject.ListObjects.Contains(obj))
                    this.CurrentAssetProject.ListObjects.Remove(obj);

                if (obj.Image != null)
                {
                    obj.Image.Dispose();
                }

                obj = null;
            }
        }

        public void removeAudioObject(AudioObject obj)
        {
            if (this.CurrentAssetProject != null)
            {
                if (this.CurrentAssetProject.Audios.Contains(obj))
                    this.CurrentAssetProject.Audios.Remove(obj);

                obj = null;

            }
        }

        public void removeFontObject(FontItem obj)
        {
            if (this.CurrentAssetProject != null)
            {
                if (this.CurrentAssetProject.Fonts.Contains(obj))
                    this.CurrentAssetProject.Fonts.Remove(obj);

                obj = null;

            }
        }

        public void removeSnippet(Snippet obj)
        {
            if (this.CurrentAssetProject != null)
            {
                if (this.CurrentAssetProject.Snippets.Contains(obj))
                    this.CurrentAssetProject.Snippets.Remove(obj);

                obj = null;  
            }
        }
        public void removeSpriteSet(CoronaSpriteSet set)
        {
            if (this.CurrentAssetProject != null)
            {
                List<DisplayObject> objToRemove = new List<DisplayObject>();
                //Chercher tous les sprite utilisant cette sprite set
                for (int i = 0; i < this.CurrentAssetProject.ListObjects.Count; i++)
                {
                    DisplayObject obj = this.CurrentAssetProject.ListObjects[i];
                    if (obj.Type.Equals("SPRITE"))
                    {
                        if (obj.SpriteSet == set)
                        {
                            if (!objToRemove.Contains(obj))
                             objToRemove.Add(obj);
                        }
                    }
                }

                for (int i = 0; i < objToRemove.Count; i++)
                {
                    DisplayObject obj = objToRemove[i];
                    this.CurrentAssetProject.ListObjects.Remove(obj);
                    obj = null;
                }
                objToRemove.Clear();

                this.CurrentAssetProject.SpriteSets.Remove(set);
            }
        }

        public void removeSpriteSheet(CoronaSpriteSheet sheet)
        {
            if (this.CurrentAssetProject != null)
            {
                List<CoronaSpriteSet> objToRemove = new List<CoronaSpriteSet>();
                //Chercher tous les sprite utilisant cette sprite set
                for (int i = 0; i < this.CurrentAssetProject.SpriteSets.Count; i++)
                {
                    CoronaSpriteSet obj = this.CurrentAssetProject.SpriteSets[i];
                    for (int j = 0; j < obj.Frames.Count; j++)
                    {
                        if (obj.Frames[j].SpriteSheetParent == sheet)
                        {
                            if(!objToRemove.Contains(obj))
                                objToRemove.Add(obj);
                        }
                    }
                }

                for (int i = 0; i < objToRemove.Count; i++)
                {
                    CoronaSpriteSet obj = objToRemove[i];
                    this.removeSpriteSet(obj);
                }

                objToRemove.Clear();


                for (int i = 0; i < sheet.Frames.Count; i++)
                {
                    SpriteFrame obj = sheet.Frames[i];
                    obj.Image.Dispose();
                    obj = null;
                }
                sheet.Frames.Clear();

                this.CurrentAssetProject.SpriteSheets.Remove(sheet);

            }
        }

        private void selectedObjectPanelsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.selectedObjectPanelsListView.SelectedIndices.Count > 0)
            {
                object obj = this.selectedObjectPanelsListView.Items[this.selectedObjectPanelsListView.SelectedIndices[0]].Tag;
                this.openPanelFromObject(obj);


            }
        }

        private void closeAllBt_Click(object sender, EventArgs e)
        {
            UserControl[] controls = new UserControl[this.selectedObjectPanels.Count];
            this.selectedObjectPanels.CopyTo(controls);
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] is SpriteManagerPanel)
                {
                    SpriteManagerPanel panel = (SpriteManagerPanel)controls[i];
                    panel.Clean();
                    panel = null;
                }
                else if (controls[i] is ImageManagerPanel)
                {
                    ImageManagerPanel panel = (ImageManagerPanel)controls[i];
                    panel.Clean();
                    panel = null;
                }
                else if (controls[i] is AudioManagerPanel)
                {
                    AudioManagerPanel panel = (AudioManagerPanel)controls[i];
                    panel.Clean();
                    panel = null;
                }
                else if (controls[i] is SnippetManagerPanel)
                {
                    SnippetManagerPanel panel = (SnippetManagerPanel)controls[i];
                    panel.Clean();
                    panel = null;

                }
                else if (controls[i] is SpriteSetManagerPanel)
                {
                    SpriteSetManagerPanel panel = (SpriteSetManagerPanel)controls[i];
                    panel.Clean();
                    panel = null;
                }

                else if (controls[i] is SpriteSheetManagerPanel)
                {
                    SpriteSheetManagerPanel panel = (SpriteSheetManagerPanel)controls[i];
                    panel.Clean();
                    panel = null;
                }
                else if (controls[i] is FontManagerPanel)
                {
                    FontManagerPanel panel = (FontManagerPanel)controls[i];
                    panel.Clean();
                    panel = null;
                }
            }
        }

        private void AssetManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.CurrentAssetProject != null)
            {
                this.closeAllBt_Click(null, null);
                DialogResult rs = MessageBox.Show("Save current project before closing?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    this.saveCurrentAssetProject();

                  /*  this.CurrentAssetProject.clean();
                    this.CurrentAssetProject = null;
                    this.Dispose(true);*/
                }

                else if (rs == DialogResult.No)
                {
                    /*  this.CurrentAssetProject.clean();
                  this.CurrentAssetProject = null;
                  this.Dispose(true);*/
                }
                  

                else if (rs == DialogResult.Cancel)
                    e.Cancel = true;

            }
            else
            {
                /*  this.CurrentAssetProject.clean();
                  this.CurrentAssetProject = null;
                  this.Dispose(true);*/
            }
            
        }

        private void assetsListView_MouseEnter(object sender, EventArgs e)
        {
            this.assetsListView.Focus();
        }

        private void assetTypeListView_MouseEnter(object sender, EventArgs e)
        {
            this.assetTypeListView.Focus();
        }

        private void selectedObjectPanelsListView_MouseEnter(object sender, EventArgs e)
        {
            this.selectedObjectPanelsListView.Focus();
        }

        private void refreshProjectList_Click(object sender, EventArgs e)
        {
            this.init();
        }

        private void openAssetsFolderBt_Click(object sender, EventArgs e)
        {
            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software\\Asset Manager";
                if (!Directory.Exists(documentsDirectory))
                    Directory.CreateDirectory(documentsDirectory);

            Process.Start(documentsDirectory);
        }

    }
}
