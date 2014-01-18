using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Krea.CoronaClasses;
using System.Drawing.Drawing2D;
using Krea.Corona_Classes;
using Krea.Asset_Manager;
using Krea.GameEditor.FontManager;
using System.Collections.Generic;

namespace Krea
{
    public partial class ImageObjectsPanel : UserControl
    {
       

         //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        

        public CoronaObject ObjectToCreate ;
        public Form1 MainForm;
        private DisplayObject displayObjectSelected;
        private AudioObject audioObjectSelected;
        private FontItem fontItemSelected;
        private Snippet snippetSelected;
        private AssetsToSerialize assetsToSerialize;
        private DirectoryInfo[] tabDirAssetsProjects;
        private Cursor currentCursorDrag;

        public bool ShouldBeRefreshed = false;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public ImageObjectsPanel()
        {
            InitializeComponent();

            this.assetsListView.GiveFeedback += new GiveFeedbackEventHandler(this.UpdateCursor);

           
        }

        //---------------------------------------------------
        //-------------------Methodes ----------------------
        //---------------------------------------------------
        public void init(Form1 mainForm)
        {
            this.MainForm = mainForm;


            laodAssetsProjectDirectories();

        }

        public void updateAssets(List<object> assetsToUpdate)
        {
            //Recuperer tous les repertoires
            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);

            if (!Directory.Exists(documentsDirectory + "\\Asset Manager"))
                Directory.CreateDirectory(documentsDirectory + "\\Asset Manager");

            String[] tabString = Directory.GetDirectories(documentsDirectory + "\\Asset Manager\\");

            for (int i = 0; i < tabString.Length; i++)
            {
                string pathDirToload = tabString[i];

                //Recuperer les assets du repertoire selectioné
                DirectoryInfo dirProjectSelected = new DirectoryInfo(pathDirToload);
                string pathFileSerialized = pathDirToload + "\\" + dirProjectSelected.Name.Replace(" ", "_") + ".kres";
                if (File.Exists(pathFileSerialized))
                {
                    AssetsToSerialize assetsSerialized = null;

                    FileStream fs = File.OpenRead(pathFileSerialized);
                    if (fs.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream();
                        ms.SetLength(fs.Length);

                        fs.Read(ms.GetBuffer(), 0, (int)ms.Length);

                        assetsSerialized = (AssetsToSerialize)SerializerHelper.DeSerializeBinary(ms); 
                        ms.Close();

                    }

                    fs.Close();

                    if (assetsSerialized != null)
                    {
                        List<object> assetsUpdated = new List<object>();
                        //Verifier si le projet contient les assets
                        for (int j = 0; j < assetsToUpdate.Count; j++)
                        {
                            object assetOBJ = assetsToUpdate[j];
                            bool isUpdated = false;
                            if (assetOBJ is DisplayObject)
                            {
                                DisplayObject assetDisplayObject = assetOBJ as DisplayObject;
                                
                                if (assetDisplayObject.Type.Equals("IMAGE"))
                                {
                                    //Chercher dans le projet d'assets si il contient une image du même nom
                                    for (int k = 0; k < assetsSerialized.ListObjects.Count; k++)
                                    {
                                        DisplayObject serializedDisplayObject = assetsSerialized.ListObjects[k];
                                        if (serializedDisplayObject.Type.Equals("IMAGE"))
                                        {
                                            if (serializedDisplayObject.Name.Equals(assetDisplayObject.OriginalAssetName))
                                            {
                                                //Update the asset
                                                assetDisplayObject.Image = new Bitmap(serializedDisplayObject.Image);
                                                isUpdated = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (assetDisplayObject.Type.Equals("SPRITE"))
                                {
                                    //Chercher dans le projet d'assets si il contient une image du même nom
                                    for (int k = 0; k < assetsSerialized.ListObjects.Count; k++)
                                    {
                                        DisplayObject serializedDisplayObject = assetsSerialized.ListObjects[k];
                                        if (serializedDisplayObject.Type.Equals("SPRITE"))
                                        {
                                            if (serializedDisplayObject.Name.Equals(assetDisplayObject.OriginalAssetName))
                                            {
                                                //Update the asset
                                                assetDisplayObject.SpriteSet = serializedDisplayObject.SpriteSet;
                                                string currentSeq = assetDisplayObject.CurrentSequence;
                                                assetDisplayObject.CurrentSequence = currentSeq;
                                                int frame = assetDisplayObject.CurrentFrame;
                                                assetDisplayObject.CurrentFrame = frame;
                                           
                                                assetDisplayObject.InitialRect = assetDisplayObject.SurfaceRect;
                                                
                                                isUpdated = true;
                                                break;
                                            }
                                        }
                                    }
                                } 
                            }

                            if (isUpdated == true)
                            {
                                assetsUpdated.Add(assetOBJ);
                            }
                        }

                        for (int j = 0; j < assetsUpdated.Count; j++)
                        {
                            assetsToUpdate.Remove(assetsUpdated[j]);
                        }

                    }

                    assetsSerialized = null;
                }

                if (assetsToUpdate.Count == 0)
                    return;
            }
            

        }

        public void laodAssetsProjectDirectories()
        {
            int selectIndex = 0;
            if (this.assetsProjectsCmbBx.Items.Count > 0)
                selectIndex = this.assetsProjectsCmbBx.SelectedIndex;

            this.assetsProjectsCmbBx.Items.Clear();

              //Recuperer tous les repertoires


            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);

            if (!Directory.Exists(documentsDirectory + "\\Asset Manager"))
                Directory.CreateDirectory(documentsDirectory + "\\Asset Manager");

            String[] tabString = Directory.GetDirectories(documentsDirectory + "\\Asset Manager\\");

            tabDirAssetsProjects = new DirectoryInfo[tabString.Length];

            for (int i = 0; i < tabString.Length; i++)
            {
                tabDirAssetsProjects[i] = new DirectoryInfo(tabString[i]);

                //Remplir la combobox des projects assets
                this.assetsProjectsCmbBx.Items.Add(tabDirAssetsProjects[i].Name);
            }

            if (this.assetsProjectsCmbBx.Items.Count > selectIndex)
                this.assetsProjectsCmbBx.SelectedIndex = selectIndex;
            else if (this.assetsProjectsCmbBx.Items.Count > 0)
                this.assetsProjectsCmbBx.SelectedIndex = 0;
            
        }

        public void cleanCurrentAssetToSerialize()
        {
            if (this.assetsToSerialize != null)
            {
               /* for (int i = 0; i<this.assetsToSerialize.ListObjects.Count; i++)
                {
                    DisplayObject obj = this.assetsToSerialize.ListObjects[i];
                    if (obj.Image != null)
                        obj.Image.Dispose();

                    if (obj.SpriteSet != null)
                    {
                        for (int j = 0; j < obj.SpriteSet.Frames.Count; j++)
                        {
                            obj.SpriteSet.Frames[j].Image.Dispose();
                        }
                    }
                }*/


                this.assetsToSerialize = null;
            }
        }
        public void loadAssetsFromProject( String pathDirToload)
        {
            if(this.MainForm.CurrentProject != null)
            {
                cleanCurrentAssetToSerialize();

                string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                if (!Directory.Exists(documentsDirectory))
                    Directory.CreateDirectory(documentsDirectory);

                //Creer d'abord le repertoire du project en cours si il n'existe pas !
                DirectoryInfo dirCurrentProject = null;
                string pathDirCurrentProject = documentsDirectory + "\\Asset Manager\\" + this.MainForm.CurrentProject.ProjectName;
                if (!Directory.Exists(pathDirCurrentProject))
                {
                    dirCurrentProject = Directory.CreateDirectory(pathDirCurrentProject);
                }

                
                //Recuperer les assets du repertoire selectioné
                DirectoryInfo dirProjectSelected = new DirectoryInfo(pathDirToload);
                string pathFileSerialized = pathDirToload + "\\" + dirProjectSelected.Name.Replace(" ", "_") + ".kres";
                if (File.Exists(pathFileSerialized))
                {

                    FileStream fs = File.OpenRead(pathFileSerialized);
                    if (fs.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream();
                        ms.SetLength(fs.Length);

                        fs.Read(ms.GetBuffer(), 0, (int)ms.Length);

                        assetsToSerialize = new AssetsToSerialize();
                        
                        assetsToSerialize = (AssetsToSerialize)SerializerHelper.DeSerializeBinary(ms);
                        ms.Close();

                        //Remplir la Assets List bx avec la liste 
                        loadAssetsListBxFromSerialized(assetsToSerialize);
                    }

                    fs.Close();

                }
                else
                {
                    MessageBox.Show("NO ASSETS FOUND FOR THIS PROJECT !\n You should open the Asset manager to create your assets for this project !",
                                                                           "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
           /* else
            {
                MessageBox.Show("NO PROJECT OPENED!\n Please open or create a project before continuing !",
                                                                           "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
           

        }

        public void loadAssetsListBxFromSerialized(AssetsToSerialize assetsToSerialize)
        {
            if (assetsToSerialize != null)
            {
                this.objectCategoryCmbBx.SelectedIndex = 0;
            }
        }


        public bool IsSpriteSetInCurrentAssetLib(CoronaSpriteSet set)
        {
            if (this.assetsToSerialize != null)
            {
                for (int i = 0; i < this.assetsToSerialize.ListObjects.Count; i++)
                {
                    DisplayObject obj = this.assetsToSerialize.ListObjects[i];
                    if (obj != null)
                    {
                        if (obj.Type.Equals("SPRITE"))
                        {
                            if (obj.SpriteSet == set)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        public void RefreshCurrentAssetProject()
        {
            int indexTypeObject = this.objectCategoryCmbBx.SelectedIndex;
            assetsProjectsCmbBx_SelectedIndexChanged(null, null);

            this.objectCategoryCmbBx.SelectedIndex = indexTypeObject;
        }

        private void assetsProjectsCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.assetsProjectsCmbBx.SelectedItem != null)
            {
                if (this.tabDirAssetsProjects != null)
                {
                   loadAssetsFromProject(this.tabDirAssetsProjects[this.assetsProjectsCmbBx.SelectedIndex].FullName);
                   objectCategoryCmbBx_SelectedIndexChanged(null,null);
                }
               
            }

        }

        private void assetPreviewPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (this.displayObjectSelected != null)
            {
                Matrix m = new Matrix();
                e.Graphics.Transform = m;
                if (this.displayObjectSelected.Type.Equals("IMAGE"))
                    this.displayObjectSelected.dessineAt(e.Graphics, new Point(0, 0), false, null, 1, 1);

                else if (this.displayObjectSelected.Type.Equals("SPRITE"))
                {
                    e.Graphics.DrawImage(this.displayObjectSelected.SpriteSet.Frames[0].Image, new Point(0, 0));
                }
                    
            }
            
        }

       

        private void objectCategoryCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.assetsListView.Items.Clear();
            this.assetsImageList.Images.Clear();
            this.assetsImageList.ImageSize = new Size(64,64);
            if (this.assetsToSerialize != null)
            {
                //Si c'est les images
                if (this.objectCategoryCmbBx.SelectedIndex == 0)
                {
                    for (int i = 0; i < this.assetsToSerialize.ListObjects.Count; i++)
                    {
                        DisplayObject obj = this.assetsToSerialize.ListObjects[i];
                        if (obj.Type.Equals("IMAGE"))
                        {
                            this.assetsImageList.Images.Add(obj.Image);

                            ListViewItem item = new ListViewItem(obj.Name, this.assetsImageList.Images.Count -1);
                            
                            item.Tag = obj;
                            this.assetsListView.Items.Add(item);
                        }
                           

                    }
                }

                //Si c'est les sprites
                else if (this.objectCategoryCmbBx.SelectedIndex == 1)
                {
                    for (int i = 0; i < this.assetsToSerialize.ListObjects.Count; i++)
                    {
                        DisplayObject obj = this.assetsToSerialize.ListObjects[i];
                        if (obj.Type.Equals("SPRITE"))
                        {
                            this.assetsImageList.Images.Add(obj.SpriteSet.Frames[0].Image);

                            ListViewItem item = new ListViewItem(obj.Name, this.assetsImageList.Images.Count -1);

                            item.Tag = obj;
                            this.assetsListView.Items.Add(item);
                        }

                    }
                }

                else if (this.objectCategoryCmbBx.SelectedIndex == 2)
                {
                    this.assetsImageList.Images.Add(Properties.Resources.audioIcon);
                    for (int i = 0; i < this.assetsToSerialize.Audios.Count; i++)
                    {
                        AudioObject obj = this.assetsToSerialize.Audios[i];
                        ListViewItem item = new ListViewItem(obj.name, 0);
                        item.Tag = obj;
                        this.assetsListView.Items.Add(item);


                    }
                }
                else if (this.objectCategoryCmbBx.SelectedIndex == 3)
                {
                    this.assetsImageList.Images.Add(Properties.Resources.snippetIcon);
                    for (int i = 0; i < this.assetsToSerialize.Snippets.Count; i++)
                    {
                        Snippet snippet = this.assetsToSerialize.Snippets[i];


                        ListViewItem item = new ListViewItem(snippet.Name, 0);
                        item.Tag = snippet;
                        this.assetsListView.Items.Add(item);


                    }
                }

                else if (this.objectCategoryCmbBx.SelectedIndex == 4)
                {
                    this.assetsImageList.Images.Add(Properties.Resources.ttf);
                    if (this.assetsToSerialize.Fonts == null) this.assetsToSerialize.Fonts = new List<FontItem>();
                    for (int i = 0; i < this.assetsToSerialize.Fonts.Count; i++)
                    {
                        FontItem font = this.assetsToSerialize.Fonts[i];


                        ListViewItem item = new ListViewItem(font.NameForIphone, 0);
                        item.Tag = font;
                        this.assetsListView.Items.Add(item);


                    }
                }
            }

        }


       
        private void assetsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.assetsListView.FocusedItem != null)
            {
                if (this.objectCategoryCmbBx.SelectedIndex == 0 || this.objectCategoryCmbBx.SelectedIndex == 1)
                {

                    this.displayObjectSelected = (DisplayObject)this.assetsListView.FocusedItem.Tag;

                }

                else if (this.objectCategoryCmbBx.SelectedIndex == 2)
                {
                    this.audioObjectSelected = (AudioObject)this.assetsListView.FocusedItem.Tag;
                }
                else if (this.objectCategoryCmbBx.SelectedIndex == 3)
                {
                    this.snippetSelected = (Snippet)this.assetsListView.FocusedItem.Tag;
                }
                else if (this.objectCategoryCmbBx.SelectedIndex == 4)
                {
                    this.fontItemSelected = (FontItem)this.assetsListView.FocusedItem.Tag;
                }
            }
            else
            {
              
                this.audioObjectSelected = null;
                this.displayObjectSelected = null;
                this.snippetSelected = null;
                this.fontItemSelected = null;
            }
        }

        private void assetsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.MainForm.CurrentProject != null)
            {
                GameElementTreeView treeView = this.MainForm.getElementTreeView();
                if (this.objectCategoryCmbBx.SelectedIndex == 0 || this.objectCategoryCmbBx.SelectedIndex == 1)
                {
                    if (this.displayObjectSelected != null && treeView.LayerSelected != null)
                    {

                        DisplayObject dispObjTemp = this.displayObjectSelected.cloneInstance(true);
                        dispObjTemp.Name = this.displayObjectSelected.Name.ToLower().Replace(" ", "").Replace("-", ""); 
                        dispObjTemp.OriginalAssetName = this.displayObjectSelected.Name;

                        //Creer un corona object et l'ajouter 
                        CoronaObject obj = new CoronaObject(dispObjTemp);
                        if (obj.DisplayObject != null)
                        {
                            obj.DisplayObject.SurfaceRect = new Rectangle(treeView.LayerSelected.SceneParent.SurfaceFocus.Location, obj.DisplayObject.SurfaceRect.Size);
                            this.MainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj,
                                                    this.MainForm.sceneEditorView1.CurrentScale, this.MainForm.sceneEditorView1.getOffsetPoint());
                            this.MainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();
                        }
                        
                        CoronaObject objectSelectedInTreeView = treeView.CoronaObjectSelected;
                        if (objectSelectedInTreeView != null)
                        {
                            if (objectSelectedInTreeView.EntityParent != null)
                            {
                                objectSelectedInTreeView.EntityParent.addObject(obj);
                            }
                            else if (objectSelectedInTreeView.isEntity == true)
                            {
                                objectSelectedInTreeView.Entity.addObject(obj);
                            }
                            else
                            {
                                treeView.LayerSelected.addCoronaObject(obj, true);
                            }

                        }
                        else
                        {

                            treeView.LayerSelected.addCoronaObject(obj, true);
                        }


                        treeView.newCoronaObject(obj);

                        if (this.MainForm.isFormLocked == false)
                            GorgonLibrary.Gorgon.Go();
                    }
                }
                else if (this.objectCategoryCmbBx.SelectedIndex == 2)
                {
                    if (audioObjectSelected != null)
                    {
                        try
                        {
                            AudioObject obj = audioObjectSelected.cloneInstance();
                            obj.name = obj.name.ToLower().Replace(" ", "").Replace("_", "").Replace("-", "");
                            string assetProjectsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software\\Asset Manager";
                            if (!Directory.Exists(assetProjectsDirectory))
                                Directory.CreateDirectory(assetProjectsDirectory);

                            File.Copy(assetProjectsDirectory +"\\" + this.assetsToSerialize.ProjectName + "\\" + audioObjectSelected.name,
                                   this.MainForm.CurrentProject.SourceFolderPath + "\\" + obj.name, true);

                            this.MainForm.getElementTreeView().newAudioObject(obj);
                            this.MainForm.CurrentProject.AudioObjects.Add(obj);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error during audio file transfert ! \n" + ex.Message);
                        }
                    }
                }
                else if (this.objectCategoryCmbBx.SelectedIndex == 3)
                {
                    if (snippetSelected != null)
                    {
                        Snippet snippet = snippetSelected.cloneInstance();
                        CoronaGameProject project = this.MainForm.CurrentProject;
                        if ( project != null)
                        {
                            bool res = false;
                            for (int i = 0; i < project.Snippets.Count; i++)
                            {
                                if (project.Snippets[i].Name.Equals(snippet.Name))
                                {
                                    res = true;
                                    break;
                                }

                            }

                            if (res == false)
                            {
                                this.MainForm.getElementTreeView().newSnippet(snippet);
                                project.Snippets.Add(snippet);
                                this.MainForm.cgEeditor1.RefreshSnippetLuaCode(project);
                            }
                           
                        }
                        
                    }
                }
                else if (this.objectCategoryCmbBx.SelectedIndex == 4)
                {
                    if (this.fontItemSelected != null)
                    {
                        FontItem font = fontItemSelected.cloneInstance();
                        if (File.Exists(font.OriginalPath))
                        {
                            string destFile =  this.MainForm.CurrentProject.SourceFolderPath + "\\" + font.NameForAndroid + ".ttf";
                            File.Copy(font.OriginalPath, destFile, true);
                            font.FileName = destFile;
                            font.OriginalPath = destFile;
                        }
                           



                        CoronaGameProject project = this.MainForm.CurrentProject;
                        font.projectParent = project;
                        if (project != null)
                        {
                            bool res = false;
                            for (int i = 0; i < project.AvailableFont.Count; i++)
                            {
                                if (project.AvailableFont[i].NameForIphone.Equals(font.NameForIphone))
                                {
                                    res = true;
                                    break;
                                }

                            }

                            if (res == false)
                            {
                                this.MainForm.getElementTreeView().newFontItem(font);
                                project.AvailableFont.Add(font);
                            }

                        }
                    }
                }

            }
        }

        private void assetsListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                if (currentCursorDrag != null)
                {
                    currentCursorDrag.Dispose();
                    currentCursorDrag = null;
                }

                if(this.assetsListView.SelectedIndices.Count>0)
                {
                    if (this.objectCategoryCmbBx.SelectedIndex < 2)
                    {
                        int index = this.assetsListView.SelectedIndices[0];
                        if (index < this.assetsImageList.Images.Count)
                        {
                            Bitmap bmp = new Bitmap(this.assetsImageList.Images[index]);
                            currentCursorDrag = Form1.CreateCursor(bmp, bmp.Width / 2, bmp.Height / 2);
                            this.assetsListView.DoDragDrop(e.Item, DragDropEffects.Move);
                        }
                    }
                     
                    
                }
               
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateCursor(object sender, GiveFeedbackEventArgs fea)
        {
            //Debug.WriteLine(MainForm.MousePosition);
            
            //Debug.WriteLine("effect = " + fea.Effect);
            if (fea.Effect == DragDropEffects.Move && currentCursorDrag != null)
            {
                fea.UseDefaultCursors = false;
                Cursor.Current = currentCursorDrag;

            }
            else
            {

                fea.UseDefaultCursors = true;
            }
                
        }

        private void assetsListView_MouseEnter(object sender, EventArgs e)
        {
            this.assetsListView.Focus();
        }

    

    }
}
