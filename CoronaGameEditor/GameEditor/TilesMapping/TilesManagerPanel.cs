using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Krea.Corona_Classes;

namespace Krea.GameEditor.TilesMapping
{
    public partial class TilesManagerPanel : UserControl
    {
        //---------------------------------------------------
        //-------------------Attributes--------------------
        //---------------------------------------------------
        private Form1 mainForm;

        public TileModel ModelSelected;

        public TilesSelection TilesSelection;
        //---------------------------------------------------
        //-------------------Constructors--------------------
        //---------------------------------------------------
        public TilesManagerPanel()
        {
            InitializeComponent();
        }

        //---------------------------------------------------
        //-------------------Methods--------------------
        //---------------------------------------------------
        public void init(Form1 mainForm)
        {
            this.mainForm = mainForm;


            SceneEditorView sceneEditorView = this.mainForm.sceneEditorView1;
            //------ RENDER WINDOWS TEXTURES
            bool exists = false;
            for (int i = 0; i < sceneEditorView.GraphicsContentManager.RendererWindows.Count; i++)
            {
                if (sceneEditorView.GraphicsContentManager.RendererWindows[i].Name.Equals("TileManager_TEXTURES"))
                {
                    exists = true;
                    break;
                }
            }
            if (exists == false)
                this.mainForm.sceneEditorView1.GraphicsContentManager.AddRenderWindow(
                 new GorgonLibrary.Graphics.RenderWindow("TileManager_TEXTURES", this.tileTexturesPictBx, false));

            //------ RENDER WINDOWS OBJECTS
            for (int i = 0; i < sceneEditorView.GraphicsContentManager.RendererWindows.Count; i++)
            {
                if (sceneEditorView.GraphicsContentManager.RendererWindows[i].Name.Equals("TileManager_OBJECTS"))
                {
                    exists = true;
                    break;
                }
            }
            if (exists == false)
                this.mainForm.sceneEditorView1.GraphicsContentManager.AddRenderWindow(
                 new GorgonLibrary.Graphics.RenderWindow("TileManager_OBJECTS", this.tileObjectsPictBx, false));

            //Load textures
            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);

            string pathTextures = Path.Combine(documentsDirectory + "\\TilesMap\\TextureSheets\\");
            if (!Directory.Exists(pathTextures))
                Directory.CreateDirectory(pathTextures);

            this.loadTexturesModel(pathTextures);

            //Load Objects
            string pathObjects = Path.Combine(documentsDirectory + "\\TilesMap\\ObjectSheets\\");
            if (!Directory.Exists(pathObjects))
                Directory.CreateDirectory(pathObjects);

            this.loadObjectsModel(pathObjects);

            if (this.textureSheetCombBx.Items.Count > 0)
                this.textureSheetCombBx.SelectedIndex = 0;

            if (this.objectSheetsCombBx.Items.Count > 0)
                this.objectSheetsCombBx.SelectedIndex = 0;
            //Definir les positions 
            this.refreshTextureTileModelLocations();
            this.refreshObjectTileModelLocations();

        }

        private void loadTexturesModel(string directoryParentPath)
        {
            DirectoryInfo imagesDirectory = new DirectoryInfo(directoryParentPath);
            FileInfo[] tabFilesInfo = imagesDirectory.GetFiles();

            for (int i = 0; i < tabFilesInfo.Length; i++)
            {
                string filename = tabFilesInfo[i].Name;
                if (filename.Contains("32x32"))
                {
                    this.addTextureSetToList(tabFilesInfo[i].FullName, filename, new Size(32, 32));
                }
                else if (filename.Contains("64x64"))
                {
                    this.addTextureSetToList(tabFilesInfo[i].FullName,filename, new Size(64, 64));
                }
                else if(filename.Contains("128x128"))
                {
                    this.addTextureSetToList(tabFilesInfo[i].FullName,filename, new Size(128, 128));
                }
                else if (filename.Contains("256x256"))
                {
                    this.addTextureSetToList(tabFilesInfo[i].FullName,filename, new Size(256, 256));

                }
                else if (filename.Contains("512x512"))
                {
                    this.addTextureSetToList(tabFilesInfo[i].FullName,filename, new Size(512, 512));
                }
              

            }

            //Repositionner les textures
            this.refreshTextureTileModelLocations();
        }

        public void loadObjectsModel(string directoryParentPath)
        {
            DirectoryInfo imagesDirectory = new DirectoryInfo(directoryParentPath);
            FileInfo[] tabFilesInfo = imagesDirectory.GetFiles();

            for (int i = 0; i < tabFilesInfo.Length; i++)
            {
                string filename = tabFilesInfo[i].Name;
                if (filename.Contains("32x32"))
                {
                    this.addObjectSetToList(tabFilesInfo[i].FullName,filename, new Size(32, 32));
                }
                else if (filename.Contains("64x64"))
                {
                    this.addObjectSetToList(tabFilesInfo[i].FullName,filename, new Size(64, 64));
                }
                else if (filename.Contains("128x128"))
                {
                    this.addObjectSetToList(tabFilesInfo[i].FullName,filename, new Size(128, 128));
                }
                else if (filename.Contains("256x256"))
                {
                    this.addObjectSetToList(tabFilesInfo[i].FullName,filename, new Size(256, 256));

                }
                else if (filename.Contains("512x512"))
                {
                    this.addObjectSetToList(tabFilesInfo[i].FullName,filename, new Size(512, 512));
                }
            }
            //Repositionner les textures
            this.refreshObjectTileModelLocations();
        }

        public List<TileModel> getTextureSheetSelected()
        {
            if (this.textureSheetCombBx.SelectedItem != null)
            {
                TileSheetModel sheetModel = (TileSheetModel)this.textureSheetCombBx.SelectedItem;
                return sheetModel.TileModels;
            }
            else
            {
                if (this.textureSheetCombBx.Items.Count > 0)
                {
                    this.textureSheetCombBx.SelectedIndex = 0;
                    TileSheetModel sheetModel = (TileSheetModel)this.textureSheetCombBx.SelectedItem;
                    return sheetModel.TileModels;
                }

            }
            return null;
        }

        public void drawTileTexturesModel(Point offsetPoint)
        {
            List<TileModel> modelsSelected = getTextureSheetSelected();
            if (modelsSelected != null)
            {
                for (int i = 0; i < modelsSelected.Count; i++)
                {
                    modelsSelected[i].DrawGorgon(offsetPoint);

                }
            }
        }

        public List<TileModel> getObjectsSheetSelected()
        {
            if (this.objectSheetsCombBx.SelectedItem != null)
            {
                TileSheetModel sheetModel = (TileSheetModel)this.objectSheetsCombBx.SelectedItem;
                return sheetModel.TileModels;
            }
            else
            {
                if (this.objectSheetsCombBx.Items.Count > 0)
                {
                    this.objectSheetsCombBx.SelectedIndex = 0;
                    TileSheetModel sheetModel = (TileSheetModel)this.objectSheetsCombBx.SelectedItem;
                    return sheetModel.TileModels;
                }

            }
            return null;
        }

        public void drawTileObjectsModel(Point offsetPoint)
        {
            List<TileModel> modelsSelected = getObjectsSheetSelected();
            if (modelsSelected != null)
            {
                
                for (int i = 0; i < modelsSelected.Count; i++)
                {
                    modelsSelected[i].DrawGorgon(offsetPoint);

                }
            }
        }

        public TileModel getTextureTileModelTouched(Point p)
        {
            List<TileModel> modelsSelected = getTextureSheetSelected();
            if (modelsSelected != null)
            {
                for (int i = 0; i < modelsSelected.Count; i++)
                {
                    if (modelsSelected[i].isTouched(p) == true)
                        return modelsSelected[i];
                }
            }
            return null;
        }

       /* public List<TileModel> getTextureTileModelsTouched(Rectangle selectionRect)
        {

        }*/

        public TileModel getObjectTileModelTouched(Point p)
        {
            List<TileModel> modelsSelected = getObjectsSheetSelected();
            if (modelsSelected != null)
            {
                for (int i = 0; i < modelsSelected.Count; i++)
                {
                    if (modelsSelected[i].isTouched(p) == true)
                        return modelsSelected[i];
                }
            }
            return null;
        }

        public void refreshTextureTileModelLocations()
        {
            List<TileModel> modelsSelected = getTextureSheetSelected();
            if (modelsSelected != null)
            {
                int xDest = 0;
                int yDest = 0;
                Size size = new Size(32, 32);

                for (int i = 0; i < modelsSelected.Count; i++)
                {
                    TileModel model = modelsSelected[i];
                    if (i == 0)
                        model.surfaceRect.Location = new Point(0, 0);
                    else
                    {
                        xDest = modelsSelected[i - 1].surfaceRect.Location.X + 32;
                        yDest = modelsSelected[i - 1].surfaceRect.Location.Y;

                        if (xDest +32 >= this.tileTexturesPage.Size.Width)
                        {
                            xDest = 0;
                            yDest += 32;
                            size = new Size(32, yDest + 32);
                        }

                        model.surfaceRect.Location = new Point(xDest, yDest);
                    }
                }

                if (size.Height > this.tileTexturesPictBx.Height)
                {
                    this.texturesTileScollBar.Visible = true;
                    int maxScrollValue = size.Height - this.tileTexturesPictBx.Height;
                    this.refreshTextureScrollBarValues(maxScrollValue, this.texturesTileScollBar.Value);
                }
                else
                {
                    this.refreshTextureScrollBarValues(0, 0);
                    this.texturesTileScollBar.Visible = false;
                
                }
                
            }

        }

        public void refreshObjectTileModelLocations()
        {
            List<TileModel> modelsSelected = getObjectsSheetSelected();
            if (modelsSelected != null)
            {
                int xDest = 0;
                int yDest = 0;
                 Size size = new Size(32, 32);
                for (int i = 0; i < modelsSelected.Count; i++)
                {
                    TileModel model = modelsSelected[i];
                    if (i == 0)
                        model.surfaceRect.Location = new Point(0, 0);
                    else
                    {
                        xDest = modelsSelected[i - 1].surfaceRect.Location.X + 32;
                        yDest = modelsSelected[i - 1].surfaceRect.Location.Y;

                        if (xDest + 32 >= this.tileObjectsPage.Size.Width)
                        {
                            xDest = 0;
                            yDest += 32;
                            size = new Size(32, yDest + 32);
                        }

                        model.surfaceRect.Location = new Point(xDest, yDest);
                    }
                }



                if (size.Height > this.tileObjectsPictBx.Height)
                {
                    this.objectTilesScrollBar.Visible = true;
                    int maxScrollValue = size.Height - this.tileObjectsPictBx.Height;
                    this.refreshObjectScrollBarValues(maxScrollValue, this.objectTilesScrollBar.Value);
                }
                else
                {
                    this.refreshObjectScrollBarValues(0, 0);
                    this.objectTilesScrollBar.Visible = false;

                }
             
            }

        }

        private void addTextureSetToList(string pathImage,string name,Size tilesSize)
        {
            TileSheetModel sheetModel = new TileSheetModel(name,pathImage, true);

            //Charger la texture set
            Bitmap textureSet = new Bitmap(Image.FromFile(pathImage));

            //Recuperer le nombre de texture par ligne et colonne
            int nbTexturesLigne = textureSet.Width / tilesSize.Width;
            int nbTexturesColonne = textureSet.Height / tilesSize.Height;


            //Creer et ajouter les textures
            for (int i = 0; i < nbTexturesColonne; i++)
            {
                for (int j = 0; j < nbTexturesLigne; j++)
                {
                    //Recuperer l'image de la texture
                    Bitmap texture = textureSet.Clone(new Rectangle(new Point(j * tilesSize.Width, i * tilesSize.Height), new Size(tilesSize.Width, tilesSize.Height)), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    
                    //Creer une texture model et l'ajouter a la liste
                    int index = i * nbTexturesLigne + j;
                    string modelName = "TEXTURE_" + sheetModel.ToString() + "_" + index + "_MODEL";
                    GorgonLibrary.Graphics.Sprite sprite = new GorgonLibrary.Graphics.Sprite(modelName, GorgonLibrary.Graphics.Image.FromBitmap(modelName,texture));
                    texture.Dispose();
                    
                    TileModel model = new TileModel(modelName, new Point(0, 0), tilesSize, sprite, true, true);
                    
                    sheetModel.TileModels.Add(model);

                }
            }

            //Relacher la texture Set principale
            textureSet.Dispose();

            this.textureSheetCombBx.Items.Add(sheetModel);
        }

        private void addObjectSetToList(string pathImage, string name, Size tilesSize)
        {
            TileSheetModel sheetModel = new TileSheetModel(name,pathImage, false);

            //Charger la texture set
            Bitmap objectSet = new Bitmap(Image.FromFile(pathImage));

        
            //Recuperer le nombre de texture par ligne et colonne
            int nbTexturesLigne = objectSet.Width / tilesSize.Width;
            int nbTexturesColonne = objectSet.Height / tilesSize.Height;

            //Creer et ajouter les textures
            for (int i = 0; i < nbTexturesColonne; i++)
            {
                for (int j = 0; j < nbTexturesLigne; j++)
                {
                    //Recuperer l'image de la texture
                    Bitmap texture = objectSet.Clone(new Rectangle(new Point(j * tilesSize.Width, i * tilesSize.Height), new Size(tilesSize.Width, tilesSize.Height)), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    //Creer une texture model et l'ajouter a la liste
                    int index = i * nbTexturesLigne + j;
                    string modelName = "OBJECT_" + sheetModel.ToString() + "_" + index + "_MODEL";
                    GorgonLibrary.Graphics.Sprite sprite = new GorgonLibrary.Graphics.Sprite(modelName, GorgonLibrary.Graphics.Image.FromBitmap(modelName, texture));
                    texture.Dispose();
                    
                    TileModel model = new TileModel(modelName, new Point(0, 0), tilesSize, sprite, true, false);
                    sheetModel.TileModels.Add(model);

                }
            }

            //Relacher la texture Set principale
            objectSet.Dispose();
            objectSet = null;
            this.objectSheetsCombBx.Items.Add(sheetModel);
        }

        //---------------------------------------------------
        //-------------------Events--------------------
        //---------------------------------------------------
  
       public void setCurrentTilesMap(TilesMap map)
       {
           //Creer une selecteur de tilesmodel
            TilesSelection = new TilesSelection(map);
       }

        private void loadTextureSet_Click(object sender, EventArgs e)
        {
            try
            {
                Size tilesSize = getTilesSizeToLoad();
                if(tilesSize.Width ==0)
                {
                    MessageBox.Show("Please select a tile size first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    OpenFileDialog openF = new OpenFileDialog();
                    openF.Filter = "PNG Files (*.png)|*.png";
                    if (openF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (openF.FileName.ToLower().EndsWith(".png"))
                        {
                            //Charger l'image et verifier la taille
                            Bitmap bmp = new Bitmap(Image.FromFile(openF.FileName));

                            //Si la hauteur ou la largeur ne sont pas des multiple de 32
                           if (bmp.Width % tilesSize.Width != 0 || bmp.Height % tilesSize.Height != 0)
                            {
                                //Refuser le chargement
                               MessageBox.Show("Sorry ! Only "+tilesSize.Width+"x"+tilesSize.Height+" images are expected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                bmp.Dispose();
                                bmp = null;
                                return;
                            }
                            else
                            {
                                //Relacher le fichier image
                                bmp.Dispose();
                                bmp = null;

                                //Copier le fichier dans les ressources Textures

                                string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                                if (!Directory.Exists(documentsDirectory))
                                    Directory.CreateDirectory(documentsDirectory);

                                string pathTextures = Path.Combine(documentsDirectory + "\\TilesMap\\TextureSheets\\");
                                string pathDest = pathTextures + "\\" + openF.SafeFileName.Replace(".png", "") + "_" + tilesSize.Width + "x" + tilesSize.Height + ".png";
                                File.Copy(openF.FileName, pathDest, true);

                                //Ajouter aux Textures
                                this.addTextureSetToList(pathDest, openF.SafeFileName.Replace(".png", "") + "_" + tilesSize.Width + "x" + tilesSize.Height, tilesSize);

                                //Repositionner les textures
                                this.refreshTextureTileModelLocations();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Texture Set loading! \n "+ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
            }
        }

        //--------------------------------------------------------------------
        //--------------- EVENTS TEXTURE PICTURE BOX -------------------------
        //--------------------------------------------------------------------

       
        private void tileTexturesList_MouseClick(object sender, MouseEventArgs e)
        {
            this.mainForm.tilesMapEditor1.CreationMode = "CREATING_MODELS";
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point pTouched = new Point(e.Location.X, e.Location.Y - this.texturesTileScollBar.Value);
                this.ModelSelected = getTextureTileModelTouched(pTouched);
                if (this.ModelSelected != null)
                {
                   
                    if(this.TilesSelection != null)
                    {
                        //Verifier si le control est actif : si oui ajouter a la liste 
                        if (Control.ModifierKeys == Keys.Control)
                            this.TilesSelection.ListModelsSelected.Add(this.ModelSelected);
                        else if (Control.ModifierKeys == Keys.Shift && this.TilesSelection.ListModelsSelected.Count > 0)
                        {
                            List<TileModel> models = this.getTextureSheetSelected();
                            if (models != null)
                            {
                                int indexLastModelSelected = models.IndexOf(this.TilesSelection.ListModelsSelected[this.TilesSelection.ListModelsSelected.Count - 1]);
                                int indexTileModelTouched = models.IndexOf(this.ModelSelected);

                                if (indexLastModelSelected >= 0 && indexTileModelTouched >= 0)
                                {
                                    if (indexTileModelTouched > indexLastModelSelected)
                                    {
                                        this.TilesSelection.ListModelsSelected.Clear();
                                        for (int i = indexLastModelSelected; i <= indexTileModelTouched; i++)
                                        {
                                            TileModel model = models[i];
                                            this.TilesSelection.ListModelsSelected.Add(model);
                                        }
                                    }

                                }

                            }

                        }
                        else
                        {
                            //SInn effacer la liste et ajouter le model
                            this.TilesSelection.ListModelsSelected.Clear();
                            this.TilesSelection.ListModelsSelected.Add(this.ModelSelected);

                        }
                    }

                }
            }
            else
            {
                this.textureMenuStrip.Show(Cursor.Position);
            }

            GorgonLibrary.Gorgon.Go();
        }

        //--------------------------------------------------------------------
        //--------------- EVENTS OBJECTS PICTURE BOX -------------------------
        //--------------------------------------------------------------------

        private void tileObjectsSurface_MouseClick(object sender, MouseEventArgs e)
        {
            this.mainForm.tilesMapEditor1.CreationMode = "CREATING_MODELS";
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point pTouched = new Point(e.Location.X,e.Location.Y - this.objectTilesScrollBar.Value);
                this.ModelSelected = getObjectTileModelTouched(pTouched);
                if (this.ModelSelected != null)
                {
                    if (this.TilesSelection != null)
                    {
                        //Verifier si le control est actif : si oui ajouter a la liste 
                        if (Control.ModifierKeys == Keys.Control)
                            this.TilesSelection.ListModelsSelected.Add(this.ModelSelected);
                        else if (Control.ModifierKeys == Keys.Shift && this.TilesSelection.ListModelsSelected.Count > 0)
                        {
                            List<TileModel> models = this.getObjectsSheetSelected();
                            if (models != null)
                            {
                                int indexLastModelSelected = models.IndexOf(this.TilesSelection.ListModelsSelected[this.TilesSelection.ListModelsSelected.Count - 1]);
                                int indexTileModelTouched = models.IndexOf(this.ModelSelected);

                                if (indexLastModelSelected >= 0 && indexTileModelTouched >= 0)
                                {
                                    if (indexTileModelTouched > indexLastModelSelected)
                                    {
                                        this.TilesSelection.ListModelsSelected.Clear();
                                        for (int i = indexLastModelSelected; i <= indexTileModelTouched; i++)
                                        {
                                            TileModel model = models[i];
                                            this.TilesSelection.ListModelsSelected.Add(model);
                                        }
                                    }

                                }

                            }

                        }
                        else
                        {
                            //SInn effacer la liste et ajouter le model
                            this.TilesSelection.ListModelsSelected.Clear();
                            this.TilesSelection.ListModelsSelected.Add(this.ModelSelected);

                        }
                    }
                }
               
            }
            else
            {
                this.objectMenuStrip.Show(Cursor.Position);
            }

            GorgonLibrary.Gorgon.Go();
        }



        public void DrawObjectTilesGorgon()
        {
            GorgonLibrary.Gorgon.CurrentRenderTarget.Clear(Color.Black);
            GorgonLibrary.Gorgon.CurrentRenderTarget.BeginDrawing();

            Point offsetPoint = new Point(0, this.objectTilesScrollBar.Value);
            drawTileObjectsModel(offsetPoint);

           
            if (this.TilesSelection != null)
            {
                for (int i = 0; i < this.TilesSelection.ListModelsSelected.Count; i++)
                {
                    Rectangle finalRect = new Rectangle(offsetPoint.X + this.TilesSelection.ListModelsSelected[i].surfaceRect.X,
                        offsetPoint.Y + this.TilesSelection.ListModelsSelected[i].surfaceRect.Y,
                        this.TilesSelection.ListModelsSelected[i].surfaceRect.Width,
                        this.TilesSelection.ListModelsSelected[i].surfaceRect.Height);

                    GorgonGraphicsHelper.Instance.DrawRectangle(finalRect, 2, Color.White, 1);
                  
                }


            }

            GorgonLibrary.Gorgon.CurrentRenderTarget.EndDrawing();
        }

        public void DrawTextureTilesGorgon()
        {
            GorgonLibrary.Gorgon.CurrentRenderTarget.Clear(Color.Black);
            GorgonLibrary.Gorgon.CurrentRenderTarget.BeginDrawing();

            Point offsetPoint = new Point(0,this.texturesTileScollBar.Value);
            this.drawTileTexturesModel(offsetPoint);


            if (this.TilesSelection != null)
            {
                for (int i = 0; i < this.TilesSelection.ListModelsSelected.Count; i++)
                {
                    Rectangle finalRect = new Rectangle(offsetPoint.X + this.TilesSelection.ListModelsSelected[i].surfaceRect.X,
                        offsetPoint.Y + this.TilesSelection.ListModelsSelected[i].surfaceRect.Y,
                        this.TilesSelection.ListModelsSelected[i].surfaceRect.Width,
                        this.TilesSelection.ListModelsSelected[i].surfaceRect.Height);

                    GorgonGraphicsHelper.Instance.DrawRectangle(finalRect, 2, Color.White, 1);

                }


            }

            GorgonLibrary.Gorgon.CurrentRenderTarget.EndDrawing();
        }

      
        private Size getTilesSizeToLoad()
        {
            if (this.tilesSizeCombBx.SelectedItem != null)
            {
                if (this.tilesSizeCombBx.SelectedItem.Equals("32x32"))
                {
                    return new Size(32, 32);
                }
                else if (this.tilesSizeCombBx.SelectedItem.Equals("64x64"))
                {
                    return new Size(64, 64);
                }
                else if (this.tilesSizeCombBx.SelectedItem.Equals("128x128"))
                {
                    return new Size(128, 128);
                }
                else if (this.tilesSizeCombBx.SelectedItem.Equals("256x256"))
                {
                    return new Size(256, 256);
                }
                else if (this.tilesSizeCombBx.SelectedItem.Equals("512x512"))
                {
                    return new Size(512, 512);
                }
            }
           

            return new Size(0,0);

        }
        private void loadObjectSet_Click(object sender, EventArgs e)
        {
            try
            {
                Size tilesSize = getTilesSizeToLoad();
                if(tilesSize.Width ==0)
                {
                    MessageBox.Show("Please select a tile size first !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    OpenFileDialog openF = new OpenFileDialog();
                    openF.Filter = "PNG Files (*.png)|*.png";
                    if (openF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (openF.FileName.ToLower().EndsWith(".png"))
                        {
                            //Charger l'image et verifier la taille
                            Bitmap bmp = new Bitmap(Image.FromFile(openF.FileName));

                            //Si la hauteur ou la largeur ne sont pas des multiple de 32
                            if (bmp.Width % tilesSize.Width != 0 || bmp.Height % tilesSize.Height != 0)
                            {
                                //Refuser le chargement
                                MessageBox.Show("Sorry ! Only "+tilesSize.Width+"x"+tilesSize.Height+" images are expected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                bmp.Dispose();
                                bmp = null;
                                return;
                            }
                            else
                            {
                                bmp.Dispose();
                                bmp = null;

                                //Copier le fichier dans les ressources Textures
                                string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                                if (!Directory.Exists(documentsDirectory))
                                    Directory.CreateDirectory(documentsDirectory);

                                string pathObjects = Path.Combine(documentsDirectory + "\\TilesMap\\ObjectSheets\\");
                                string pathDest = pathObjects + "\\" + openF.SafeFileName.Replace(".png","") + "_" + tilesSize.Width + "x" + tilesSize.Height+".png";
                                File.Copy(openF.FileName, pathDest,true);
                               

                                //Ajouter aux Textures
                                this.addObjectSetToList(pathDest, openF.SafeFileName.Replace(".png", "") + "_" + tilesSize.Width + "x" + tilesSize.Height, tilesSize);

                                //Repositionner les textures
                                this.refreshObjectTileModelLocations();

                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Object Set loading! \n " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
            }
        }

        private void objectSheetsCombBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.TilesSelection != null)
                this.TilesSelection.ListModelsSelected.Clear(); 

            this.refreshObjectTileModelLocations();
            GorgonLibrary.Gorgon.Go();
        }

        private void textureSheetCombBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.TilesSelection != null)
                this.TilesSelection.ListModelsSelected.Clear();
            this.refreshTextureTileModelLocations();

        }

        private void texturesTabControl_SizeChanged(object sender, EventArgs e)
        {
            this.refreshObjectTileModelLocations();
            this.refreshTextureTileModelLocations();
        }

        private void removeCurrentPalette_Click(object sender, EventArgs e)
        {
            if(this.texturesTabControl.SelectedTab != null)
            {
                if (this.texturesTabControl.SelectedTab.Text.Equals("Textures"))
                {
                    if (this.textureSheetCombBx.SelectedItem != null)
                    {
                        TileSheetModel sheetModel = (TileSheetModel)this.textureSheetCombBx.SelectedItem;
                        if (sheetModel != null)
                        {
                            if (MessageBox.Show("Are you sure to remove the \"" + sheetModel.ToString() + "\" tile models?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    == DialogResult.Yes)
                            {
                                this.textureSheetCombBx.Items.Remove(sheetModel);

              
                                if (File.Exists(sheetModel.filename))
                                {
                                    File.Delete(sheetModel.filename);
                                }

                                this.refreshTextureTileModelLocations();
                            }
                        }
                    }
                }
                else if (this.texturesTabControl.SelectedTab.Text.Equals("Objects"))
                {
                    if (this.objectSheetsCombBx.SelectedItem != null)
                    {
                        TileSheetModel sheetModel = (TileSheetModel)this.objectSheetsCombBx.SelectedItem;
                        if (sheetModel != null)
                        {
                            if (MessageBox.Show("Are you sure to remove the \"" + sheetModel.ToString() + "\" tile models?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    == DialogResult.Yes)
                            {
                                this.objectSheetsCombBx.Items.Remove(sheetModel);

                                if (File.Exists(sheetModel.filename))
                                {
                                    File.Delete(sheetModel.filename);
                                }

                                this.refreshObjectTileModelLocations();
                            }
                        }
                    }
                }
            }
            
        }

        private void createSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TilesSelection.ListModelsSelected.Count > 1 && this.mainForm.tilesMapEditor1.CurrentTilesMap != null)
            {

                TilesMap map = this.mainForm.tilesMapEditor1.CurrentTilesMap;
                if (map.TextureSequences == null)
                    map.TextureSequences = new List<TileSequence>();

                
                TileSequence seq = new TileSequence("textureSequence" + map.TextureSequences.Count, new List<TileModel>(this.TilesSelection.ListModelsSelected));

                this.mainForm.tilesMapEditor1.AddTextureSequence(seq);
            }
        }

        private void createObjectSequenceBt_Click(object sender, EventArgs e)
        {
            if (this.TilesSelection.ListModelsSelected.Count > 1 && this.mainForm.tilesMapEditor1.CurrentTilesMap != null)
            {

                TilesMap map = this.mainForm.tilesMapEditor1.CurrentTilesMap;
                if (map.ObjectSequences == null)
                    map.ObjectSequences = new List<TileSequence>();


                TileSequence seq = new TileSequence("objectSequence" + map.ObjectSequences.Count, new List<TileModel>(this.TilesSelection.ListModelsSelected));

                this.mainForm.tilesMapEditor1.AddObjectSequence(seq);
            }
        }


        private void tileObjectsPictBx_Paint(object sender, PaintEventArgs e)
        {
            if(GorgonLibrary.Gorgon.IsInitialized == true)
                 GorgonLibrary.Gorgon.Go();
        }

        private void tileTexturesPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (GorgonLibrary.Gorgon.IsInitialized == true)
                GorgonLibrary.Gorgon.Go();
        }



        private void refreshTextureScrollBarValues(int maxVal, int currentValue)
        {
            this.texturesTileScollBar.Minimum = -maxVal;
            this.texturesTileScollBar.Maximum = 0;

            if (currentValue > 0)
                currentValue = 0;

            if (currentValue < this.texturesTileScollBar.Minimum)
                currentValue = this.texturesTileScollBar.Minimum;

            this.texturesTileScollBar.Value = -currentValue;

            GorgonLibrary.Gorgon.Go();
        }

        private void refreshObjectScrollBarValues(int maxVal, int currentValue)
        {
            this.objectTilesScrollBar.Minimum = -maxVal;
            this.objectTilesScrollBar.Maximum = 0;

            if (currentValue > 0)
                currentValue = 0;

            if (currentValue < this.objectTilesScrollBar.Minimum)
                currentValue = this.objectTilesScrollBar.Minimum;

           
            this.objectTilesScrollBar.Value = -currentValue;

            GorgonLibrary.Gorgon.Go();
        }

        private void texturesTileScollBar_Scroll(object sender, EventArgs e)
        {
            GorgonLibrary.Gorgon.Go();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            GorgonLibrary.Gorgon.Go();
        }

        private void addToPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TilesSelection != null)
            {
                TilesMap currentMap = this.TilesSelection.TilesMapParent;
                if (currentMap != null)
                {
                    for (int i = 0; i < this.TilesSelection.ListModelsSelected.Count; i++)
                    {
                        TileModel model = this.TilesSelection.ListModelsSelected[i];
                    
                        if (model != null)
                        {
                            string finalName = model.Name.Replace("_MODEL", "");
                            if (model.IsTexture == true)
                            {
                                if (!currentMap.IsTileModelExists(currentMap.TileModelsTextureUsed, finalName))
                                {
                                    //Clone gorgon image 
                                    Image img = model.GorgonSprite.Image.SaveBitmap();
                                    GorgonLibrary.Graphics.Image newImage = GorgonLibrary.Graphics.Image.FromBitmap(finalName, img);
                                    GorgonLibrary.Graphics.Sprite sprite = new GorgonLibrary.Graphics.Sprite(finalName, newImage);
                                    TileModel newModel = new TileModel(finalName, model.surfaceRect.Location, model.surfaceRect.Size,
                                            sprite, false, true);
                                    img.Dispose();
                                    img = null;

                                    currentMap.TileModelsTextureUsed.Add(newModel);
                                    currentMap.UpdateGorgonTileModel(newModel, true);


                                }
                            }
                            else
                            {
                                if (!currentMap.IsTileModelExists(currentMap.TileModelsObjectsUsed, finalName))
                                {
                                    //Clone gorgon image 
                                    Image img = model.GorgonSprite.Image.SaveBitmap();
                                    GorgonLibrary.Graphics.Image newImage = GorgonLibrary.Graphics.Image.FromBitmap(finalName, img);
                                    GorgonLibrary.Graphics.Sprite sprite = new GorgonLibrary.Graphics.Sprite(finalName, newImage);
                                    TileModel newModel = new TileModel(finalName, model.surfaceRect.Location, model.surfaceRect.Size,
                                            sprite, false, false);
                                    img.Dispose();
                                    img = null;

                                    currentMap.TileModelsObjectsUsed.Add(newModel);
                                    currentMap.UpdateGorgonTileModel(newModel, true);

                                   
                                }
                            }

                        }
                    }
                    GorgonLibrary.Gorgon.Go();
                }
            }
        }

        private void texturesTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.TilesSelection != null)
                 this.TilesSelection.ListModelsSelected.Clear();
        }

    }
}
