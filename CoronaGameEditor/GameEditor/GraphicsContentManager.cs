using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary;
using GorgonLibrary.Graphics;
using System.Drawing;
using Krea.CoronaClasses;
using System.IO;
using Krea.Corona_Classes;
using Krea.GameEditor.TilesMapping;
using Krea.Corona_Classes.Controls;
using Krea.GameEditor.FontManager;

namespace Krea.GameEditor
{
    public class GraphicsContentManager
    {
        public List<RenderWindow> RendererWindows;
        CoronaGameProject currentProject;

        Form1 mainForm;
        public bool AutomaticStop = true;
        
        public GraphicsContentManager(Form1 mainForm)
        {
            this.mainForm = mainForm;

            this.RendererWindows = new List<GorgonLibrary.Graphics.RenderWindow>();

           
            InizializeGorgon();

            this.AddRenderWindow(new RenderWindow("ScenePreview", this.mainForm.scenePreview1.previewPictBx, false));
           
        }

        public void AddRenderWindow(RenderWindow window)
        {
            if(window != null)
                if (!this.RendererWindows.Contains(window))
                    this.RendererWindows.Add(window);
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------ GORGON RENDERER ------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        // <summary>
        /// Handles the Idle event of the Gorgon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GorgonLibrary.Graphics.FrameEventArgs"/> instance containing the event data.</param>
        private void Gorgon_Idle(object sender, FrameEventArgs e)
        {
            try
            {
                // Do nothing here.  When we need to update, we will.
                Gorgon.CurrentRenderTarget = null;
                Gorgon.CurrentRenderTarget.BlendingMode = BlendingModes.Modulated;
           
                Gorgon.CurrentRenderTarget.Clear(Color.Black);
                if (this.mainForm != null)
                {
                    SceneEditorView sceneEditorView = this.mainForm.sceneEditorView1;
                    if (sceneEditorView.surfacePictBx.Visible == true)
                    {
                        bool showFocus = false;
                        Scene currentScene = this.mainForm.getElementTreeView().SceneSelected;
                        if (currentScene != null)
                        {
                            showFocus = currentScene.Camera.isSurfaceFocusVisible;

                        }
                        sceneEditorView.DrawGorgon(sceneEditorView.CurrentScale, sceneEditorView.getOffsetPoint(),Settings1.Default.ShowGrid,
                            showFocus);
                    }


                    for (int i = 0; i < this.RendererWindows.Count; i++)
                    {
                        RenderWindow window = this.RendererWindows[i];
                        Gorgon.CurrentRenderTarget = window;
                        Gorgon.CurrentRenderTarget.BlendingMode = BlendingModes.Modulated;
                        window.Clear(Color.Black);
                        if (window.Owner.Visible == true)
                        {
                            
                            if (window.Name.Equals("ScenePreview"))
                            {
                                ScenePreview scenePreview = this.mainForm.scenePreview1;

                                Scene scene = this.mainForm.getElementTreeView().SceneSelected;
                                if (scene != null)
                                {
                                    float currentScale = 10;
                                    bool nextScale = true;

                                    float previewWidth = (float)scenePreview.previewPictBx.Width;
                                    float previewHeight = (float)scenePreview.previewPictBx.Height;

                                    while (nextScale == true)
                                    {
                                        float sceneWidthScaled = scene.Size.Width * currentScale;
                                        float sceneHeightScaled = scene.Size.Height * currentScale;
                                        if (sceneWidthScaled <= previewWidth && sceneHeightScaled <= previewHeight)
                                        {
                                            nextScale = false;
                                        }
                                        else
                                        {
                                            currentScale = currentScale / 2;
                                        }
                                    }
                                    scenePreview.CurrentScale = currentScale;
                                    sceneEditorView.DrawGorgon(currentScale, Point.Empty,false,false);

                                    //Draw current view shader
                                    Color color = Color.FromArgb(125, Color.DarkGray);
                                    Point offSet = sceneEditorView.getOffsetPoint();

                                    Point pDestFocus = new Point((int)(-offSet.X * currentScale), (int)(-offSet.Y * currentScale));
                                    Size sizeFocus =  new Size((int)(sceneEditorView.surfacePictBx.Width * currentScale * (1 / this.mainForm.sceneEditorView1.CurrentScale)),
                                        (int)(sceneEditorView.surfacePictBx.Height * currentScale * (1 / this.mainForm.sceneEditorView1.CurrentScale)));

                                    int yTopDest = pDestFocus.Y;
                                    Rectangle topRect = new Rectangle(0, 0,
                                        GorgonLibrary.Gorgon.CurrentRenderTarget.Width, yTopDest);

                                    int yBottomStart = pDestFocus.Y + sizeFocus.Height;
                                    Rectangle bottomRect = new Rectangle(0, yBottomStart,
                                      GorgonLibrary.Gorgon.CurrentRenderTarget.Width, GorgonLibrary.Gorgon.CurrentRenderTarget.Height - yBottomStart);

                                    int xLeftDest = pDestFocus.X;
                                    Rectangle leftRect = new Rectangle(0, yTopDest, xLeftDest, sizeFocus.Height);

                                    int xRightStart = sizeFocus.Width + pDestFocus.X;
                                    int xRightDest = GorgonLibrary.Gorgon.CurrentRenderTarget.Width - xRightStart;
                                    Rectangle rightRect = new Rectangle(xRightStart, yTopDest, xRightDest, sizeFocus.Height);

                                    Gorgon.CurrentRenderTarget.BeginDrawing();
                                    GorgonGraphicsHelper.Instance.FillRectangle(topRect, 1, color, 1, false);
                                    GorgonGraphicsHelper.Instance.FillRectangle(bottomRect, 1, color, 1, false);
                                    GorgonGraphicsHelper.Instance.FillRectangle(leftRect, 1, color, 1, false);
                                    GorgonGraphicsHelper.Instance.FillRectangle(rightRect, 1, color, 1, false);
                                    Gorgon.CurrentRenderTarget.EndDrawing();
                                }
                            }

                            else if (window.Name.Equals("PhysicsBodyEditor"))
                            {
                                PhysicBodyEditorView physicsEditor = this.mainForm.physicBodyEditorView1;
                                physicsEditor.DrawGorgon();
                            }
                            else if (window.Name.Equals("TileMapEditor"))
                            {
                                TilesMapEditor tileMapEditor = this.mainForm.tilesMapEditor1;
                                tileMapEditor.DrawGorgon();
                            }
                            else if (window.Name.Equals("TileMapEditor_TextureModels"))
                            {
                                TilesMapEditor tileMapEditor = this.mainForm.tilesMapEditor1;
                                tileMapEditor.DrawTextureModelsGorgon();
                            }
                            else if (window.Name.Equals("TileMapEditor_ObjectModels"))
                            {
                                TilesMapEditor tileMapEditor = this.mainForm.tilesMapEditor1;
                                tileMapEditor.DrawObjectModelsGorgon();
                            }
                            else if (window.Name.Equals("TileManager_TEXTURES"))
                            {
                                TilesManagerPanel managerPanel = this.mainForm.tilesManagerPanel1;
                                managerPanel.DrawTextureTilesGorgon();
                            }
                            else if (window.Name.Equals("TileManager_OBJECTS"))
                            {
                                TilesManagerPanel managerPanel = this.mainForm.tilesManagerPanel1;
                                managerPanel.DrawObjectTilesGorgon();
                            }
                           

                        }
                        Gorgon.CurrentRenderTarget.Update();
                        Gorgon.CurrentRenderTarget = null;
                    }
                }

                if(AutomaticStop == true)
                    Gorgon.Stop();
            }
            catch (Exception ex)
            {
                Gorgon.Stop();
                AutomaticStop = true;
            }
        }




        /// <summary>
        /// Handles the DeviceReset event of the Gorgon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Gorgon_DeviceReset(object sender, EventArgs e)
        {

            // Restart drawing.
            Gorgon.Go();
        }

        private void InizializeGorgon()
        {
            // Initialize Gorgon
            // Set it up so that we won't be rendering in the background, but allow the screensaver to activate.
            Gorgon.Initialize(false, true);

            Gorgon.GlobalStateSettings.GlobalSmoothing = Smoothing.Smooth;

            // Display the logo.
            Gorgon.LogoVisible = false;
            Gorgon.FrameStatsVisible = false;

            Gorgon.SetMode(this.mainForm.sceneEditorView1.surfacePictBx);

            GorgonGraphicsHelper.Instance.Init();

            // Set an ugly background color.
            Gorgon.Screen.BackgroundColor = Color.FromArgb(0, 0, 64);

            // Assign idle event.
            Gorgon.Idle += new FrameEventHandler(Gorgon_Idle);

            // Assign a device reset event so our image will be resized.
            Gorgon.DeviceReset += new EventHandler(Gorgon_DeviceReset);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------ CONTENTMANAGER ------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetCurrentProject(CoronaGameProject currentProject, float worldScale,Point offSetPoint)
        {
            if (this.currentProject != null)
            {
                this.CleanProjectGraphics(false,false);

            }

            this.currentProject = currentProject;

            if (!Directory.Exists(this.currentProject.ProjectPath + "\\Resources"))
                Directory.CreateDirectory(this.currentProject.ProjectPath + "\\Resources");


            this.LoadProjectGraphics(worldScale, offSetPoint);
        }

        public void LoadProjectGraphics(float worldScale, Point offSetPoint)
        {
            if (this.currentProject != null)
            {
                for (int i = 0; i < this.currentProject.Scenes.Count; i++)
                {
                    Scene scene = this.currentProject.Scenes[i];
                    for (int j = 0; j < scene.Layers.Count; j++)
                    {
                        CoronaLayer layer = scene.Layers[j];
                        for (int k = 0; k < layer.CoronaObjects.Count; k++)
                        {
                            this.UpdateSpriteContent(layer.CoronaObjects[k], worldScale,offSetPoint);
                           
                        }

                        for (int k = 0; k < layer.Controls.Count; k++)
                        {
                            this.UpdateSpriteContent(layer.Controls[k], worldScale, offSetPoint);

                        }

                    }
                }

                this.CleanProjectBitmaps();
                
            }

            
        }

        public void CleanSceneGraphics(Scene scene, bool removeAsset, bool checkIntegrity)
        {
            for (int i = 0; i < scene.Layers.Count; i++)
            {
                this.CleanLayerGraphics(scene.Layers[i], removeAsset, checkIntegrity);
            }
        }


        public void CleanTileMap(TilesMap map, bool removeAsset, bool checkIntegrity)
        {
            map.CleanAllTileModelsUsed(true, removeAsset, checkIntegrity);

            if (removeAsset == true)
            {
                //get textures content
                string folderDest = this.currentProject.SourceFolderPath;
                string textureContentFilePath = folderDest + "\\" + map.TilesMapName + "textures.json";
                if (File.Exists(textureContentFilePath))
                    File.Delete(textureContentFilePath);

                //get objects content
                string objectContentFilePath = folderDest + "\\" + map.TilesMapName + "objects.json";
                if (File.Exists(objectContentFilePath))
                    File.Delete(objectContentFilePath);

                //get collisions content
                string objectCollisionFilePath = folderDest + "\\" + map.TilesMapName + "collisions.json";
                if (File.Exists(objectCollisionFilePath))
                    File.Delete(objectCollisionFilePath);

                //get texture sequences content
                string textureSequenceContentFilePath = folderDest + "\\" + map.TilesMapName + "texturesequences.json";
                if (File.Exists(textureSequenceContentFilePath))
                    File.Delete(textureSequenceContentFilePath);

                //get object sequences content          
                string objectSequenceContentFilePath = folderDest + "\\" + map.TilesMapName + "objectsequences.json";
                if (File.Exists(objectSequenceContentFilePath))
                    File.Delete(objectSequenceContentFilePath);


                //get events content          
                string eventsContentFilePath = folderDest + "\\" + map.TilesMapName + "events.json";
                if (File.Exists(eventsContentFilePath))
                    File.Delete(eventsContentFilePath);
            }
        }

        public void CleanLayerGraphics(CoronaLayer layer, bool removeAsset, bool checkIntegrity)
        {
            if (layer.TilesMap != null)
                this.CleanTileMap(layer.TilesMap, removeAsset, checkIntegrity);

            for (int k = 0; k < layer.CoronaObjects.Count; k++)
            {
                CoronaObject obj = layer.CoronaObjects[k];
                if (obj.isEntity == true)
                {
                    for (int l = 0; l < obj.Entity.CoronaObjects.Count; l++)
                    {
                        CoronaObject child = obj.Entity.CoronaObjects[l];
                        this.CleanSprite(child, removeAsset, checkIntegrity);
                    }
                }
                else
                {
                    this.CleanSprite(obj, removeAsset, checkIntegrity);
                }

            }

            for (int k = 0; k < layer.Controls.Count; k++)
            {
                CoronaControl control = layer.Controls[k];
                this.CleanSprite(control, removeAsset);
            }
        }

        public void CleanProjectGraphics(bool removeAsset, bool checkIntegrity)
        {
            if (this.currentProject != null)
            {
                for (int i = 0; i < this.currentProject.Scenes.Count; i++)
                {
                    Scene scene = this.currentProject.Scenes[i];
                    this.CleanSceneGraphics(scene, removeAsset, checkIntegrity);
                }
            }
        }




        public void CleanProjectBitmaps()
        {
            if (this.currentProject != null)
            {
                bool shouldResfreshAssetLib = false;

                for (int i = 0; i < this.currentProject.Scenes.Count; i++)
                {
                    Scene scene = this.currentProject.Scenes[i];
                    for (int j = 0; j < scene.Layers.Count; j++)
                    {
                        CoronaLayer layer = scene.Layers[j];
                        for (int k = 0; k < layer.CoronaObjects.Count; k++)
                        {
                            CoronaObject obj = layer.CoronaObjects[k];
                            if (obj.isEntity == true)
                            {
                                for (int l = 0; l < obj.Entity.CoronaObjects.Count; l++)
                                {
                                      CoronaObject child = obj.Entity.CoronaObjects[l];
                                      if (child.DisplayObject != null)
                                      {
                                          if (child.DisplayObject.Image != null)
                                          {
                                              child.DisplayObject.Image.Dispose();
                                              child.DisplayObject.Image = null;
                                          }
                                      }
                                      else if (child.DisplayObject.SpriteSet != null)
                                      {
                                          bool shouldResfreshLib = this.mainForm.imageObjectsPanel1.IsSpriteSetInCurrentAssetLib(child.DisplayObject.SpriteSet);
                                          if (shouldResfreshLib == true)
                                              shouldResfreshAssetLib = true;

                                          List<CoronaSpriteSheet> sheetsUsed = this.GetSpriteSheetsUsedFromSpriteSet(child.DisplayObject.SpriteSet);
                                          for (int o = 0; o < sheetsUsed.Count; o++)
                                          {
                                              CoronaSpriteSheet sheet = sheetsUsed[o];
                                              for (int m = 0; m < sheet.Frames.Count; m++)
                                              {
                                                  if (sheet.Frames[m].Image != null)
                                                  {
                                                      sheet.Frames[m].Image.Dispose();
                                                      sheet.Frames[m].Image = null;
                                                  }
                                              }
                                          }
                                      }
                                }
                            }
                            else
                            {
                                if (obj.DisplayObject != null)
                                {
                                    if (obj.DisplayObject.Image != null)
                                    {
                                        obj.DisplayObject.Image.Dispose();
                                        obj.DisplayObject.Image = null;
                                    }
                                    else if (obj.DisplayObject.SpriteSet != null)
                                    {
                                        bool shouldResfreshLib = this.mainForm.imageObjectsPanel1.IsSpriteSetInCurrentAssetLib(obj.DisplayObject.SpriteSet);
                                        if (shouldResfreshLib == true)
                                            shouldResfreshAssetLib = true;

                                        List<CoronaSpriteSheet> sheetsUsed = this.GetSpriteSheetsUsedFromSpriteSet(obj.DisplayObject.SpriteSet);
                                        for (int l = 0; l < sheetsUsed.Count; l++)
                                        {
                                            CoronaSpriteSheet sheet = sheetsUsed[l];
                                            for (int m = 0; m < sheet.Frames.Count; m++)
                                            {
                                                if (sheet.Frames[m].Image != null)
                                                {
                                                    sheet.Frames[m].Image.Dispose();
                                                    sheet.Frames[m].Image = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        for (int k = 0; k < layer.Controls.Count; k++)
                        {

                            CoronaControl control = layer.Controls[k];
                            if (control.Type == CoronaControl.ControlType.joystick)
                            {
                                JoystickControl joy = control as JoystickControl;

                                if (joy.innerImage != null)
                                {
                                    joy.innerImage.Dispose();
                                    joy.innerImage = null;
                                }

                                if (joy.outerImage != null)
                                {
                                    joy.outerImage.Dispose();
                                    joy.outerImage = null;
                                }
                            }
                        }


                    }
                }

                this.mainForm.imageObjectsPanel1.ShouldBeRefreshed = shouldResfreshAssetLib;
            }
        }



        public void UpdateSpriteStates(CoronaObject obj, float worldScale, Point offsetPoint)
        {
            if (this.currentProject != null)
            {
                if (obj.isEntity == true)
                {
                    for (int i = 0; i < obj.Entity.CoronaObjects.Count; i++)
                    {
                        this.UpdateSpriteStates(obj.Entity.CoronaObjects[i], worldScale,offsetPoint);
                    }
                }
                else
                {
                    if (obj.DisplayObject != null)
                    {
                        Sprite sprite = null;
                        if (obj.DisplayObject.Type.Equals("IMAGE"))
                        {
  
                           sprite =  obj.DisplayObject.GorgonSprite;

                           if (sprite == null)
                           {
                               this.UpdateSpriteContent(obj, worldScale, offsetPoint);
                               return;
                           }

                           if (obj.DisplayObject.ImageFillColor == Color.Empty)
                               obj.DisplayObject.ImageFillColor = Color.White;

                           sprite.SetPosition((float)obj.DisplayObject.SurfaceRect.X + sprite.Axis.X + offsetPoint.X,
                               (float)obj.DisplayObject.SurfaceRect.Y + sprite.Axis.Y + offsetPoint.Y);
                           sprite.Rotation = obj.DisplayObject.Rotation;

                           sprite.Color = Color.FromArgb((int)(obj.DisplayObject.Alpha * 255.0f), obj.DisplayObject.ImageFillColor);

                           float imgScaleX = (float)obj.DisplayObject.SurfaceRect.Width / (float)sprite.Image.Width;
                           float imgScaleY = (float)obj.DisplayObject.SurfaceRect.Height / (float)sprite.Image.Height;

                           float finalXScale = worldScale * imgScaleX;
                           float finalYScale = worldScale * imgScaleY;
                           sprite.SetScale(finalXScale, finalYScale);

                           sprite.SetAxis((float)sprite.Image.Width / 2, (float)sprite.Image.Height / 2);
                           sprite.Rotation = obj.DisplayObject.Rotation;

                          
                        }
                        else if (obj.DisplayObject.Type.Equals("SPRITE"))
                        {
                            CoronaSpriteSet spriteSetParent = obj.DisplayObject.SpriteSet;
                            sprite = obj.DisplayObject.GorgonSprite;

                            if (sprite == null)
                            {
                                this.UpdateSpriteContent(obj, worldScale, offsetPoint);
                                return;
                            }

                            if (obj.DisplayObject.CurrentSequence == "DEFAULT")
                            {
                                if (obj.DisplayObject.SpriteSet.Frames.Count > 0)
                                {
                                    //Charger le sprite avec la current frame
                                    SpriteFrame spriteFrame = spriteSetParent.Frames[obj.DisplayObject.CurrentFrame];

                                    float factor = obj.DisplayObject.SpriteSet.Frames[obj.DisplayObject.CurrentFrame].SpriteSheetParent.FramesFactor;
                                    if (factor <= 0)
                                        factor = 1;
                                    int width = Convert.ToInt32((float)sprite.Image.Width / factor);
                                    int height = Convert.ToInt32((float)sprite.Image.Height / factor);
                                    obj.DisplayObject.SurfaceRect = new Rectangle(obj.DisplayObject.SurfaceRect.Location, new Size(width, height));
               
                                }
                            }
                            else
                            {
                                CoronaSpriteSetSequence sequence = obj.DisplayObject.getSequenceByName(obj.DisplayObject.CurrentSequence);
                                if (sequence != null)
                                {

                                    int convertedCurrentFrame = obj.DisplayObject.CurrentFrame + sequence.FrameDepart - 1;

                                    if (obj.DisplayObject.SpriteSet.Frames.Count > 0 && obj.DisplayObject.SpriteSet.Frames.Count > convertedCurrentFrame)
                                    {
                                        //Charger le sprite avec la current frame
                                        SpriteFrame spriteFrame = spriteSetParent.Frames[convertedCurrentFrame];

                                        float factor = obj.DisplayObject.SpriteSet.Frames[convertedCurrentFrame].SpriteSheetParent.FramesFactor;
                                        if (factor <= 0)
                                            factor = 1;
                                        int width = Convert.ToInt32((float)sprite.Image.Width / factor);
                                        int height = Convert.ToInt32((float)sprite.Image.Height / factor);
                                        obj.DisplayObject.SurfaceRect = new Rectangle(obj.DisplayObject.SurfaceRect.Location, new Size(width, height));
  
                                    }
                                    else
                                    {
                                        obj.DisplayObject.CurrentFrame = sequence.FrameCount - 1;
                                        this.UpdateSpriteContent(obj, worldScale,offsetPoint);
                                    }
                                }
                                else
                                {
                                    obj.DisplayObject.setSequence("");
                                    this.UpdateSpriteContent(obj, worldScale, offsetPoint);
                                   
                                }
                            }
                            sprite.Color = Color.FromArgb((int)(obj.DisplayObject.Alpha * 255.0f), Color.White);
                            
                            sprite.SetPosition((float)obj.DisplayObject.SurfaceRect.X + sprite.Axis.X + offsetPoint.X,
                                (float)obj.DisplayObject.SurfaceRect.Y + sprite.Axis.Y + offsetPoint.Y);
                          

                            float imgScaleX = (float)obj.DisplayObject.SurfaceRect.Width / (float)sprite.Image.Width;
                            float imgScaleY = (float)obj.DisplayObject.SurfaceRect.Height / (float)sprite.Image.Height;

                            float finalXScale = worldScale * imgScaleX;
                            float finalYScale = worldScale * imgScaleY;
                            sprite.SetScale(finalXScale, finalYScale);

                            sprite.SetAxis((float)sprite.Image.Width / 2, (float)sprite.Image.Height / 2);
                            sprite.Rotation = obj.DisplayObject.Rotation;

                        }

                        else if (obj.DisplayObject.Type.Equals("FIGURE"))
                        {
                            Krea.CGE_Figures.Figure fig = obj.DisplayObject.Figure;

                            if (fig.ShapeType.Equals("RECTANGLE"))
                            {
                                this.CleanSprite(obj,false,false);

                                Bitmap textureFigure = fig.DrawInBitmap((int)(255.0f * obj.DisplayObject.Alpha),worldScale);

                                sprite = new Sprite(obj.DisplayObject.Name, GorgonLibrary.Graphics.Image.FromBitmap("shape_" + obj.DisplayObject.Name, textureFigure));
                                obj.DisplayObject.GorgonSprite = sprite;

                                textureFigure.Dispose();
                                textureFigure = null;
                                
                                sprite.SetAxis((float)sprite.Image.Width / 2, (float)sprite.Image.Height / 2);
                                sprite.SetPosition((float)obj.DisplayObject.SurfaceRect.X + sprite.Axis.X + offsetPoint.X,
                                    (float)obj.DisplayObject.SurfaceRect.Y + sprite.Axis.Y + offsetPoint.Y);
                                sprite.Rotation = obj.DisplayObject.Rotation;


                            }
                            else if (fig.ShapeType.Equals("TEXT"))
                            {
                                //this.CleanSprite(obj, false, false);

                                //Bitmap textureFigure = fig.DrawInBitmap((int)(255.0f * obj.DisplayObject.Alpha),1);

                                //sprite = new Sprite(obj.DisplayObject.Name, GorgonLibrary.Graphics.Image.FromBitmap("text_" + obj.DisplayObject.Name, textureFigure));
                                //obj.DisplayObject.GorgonSprite = sprite;

                                //sprite.SetAxis((float)sprite.Image.Width / 2, (float)sprite.Image.Height / 2);
                                //sprite.SetPosition((float)obj.DisplayObject.SurfaceRect.X + sprite.Axis.X + offsetPoint.X,
                                //    (float)obj.DisplayObject.SurfaceRect.Y + sprite.Axis.Y + offsetPoint.Y);
                                //sprite.Rotation = obj.DisplayObject.Rotation;

                                //Krea.CGE_Figures.Texte textObject = fig as Krea.CGE_Figures.Texte;

                                //TextSprite textSprite = textObject.TextSprite;

                                //if (textSprite != null)
                                //{
                                //    textSprite.Text = textObject.txt;
                                //    textSprite.Color = Color.FromArgb((int)(obj.DisplayObject.Alpha * 255.0f),
                                //      textObject.FillColor.R, textObject.FillColor.G, textObject.FillColor.B);

                                //    textSprite.SetAxis((float)textSprite.Width / 2, (float)textSprite.Height / 2);

                                //    textSprite.SetPosition((float)obj.DisplayObject.SurfaceRect.X + textSprite.Axis.X + offsetPoint.X,
                                //        (float)obj.DisplayObject.SurfaceRect.Y + textSprite.Axis.Y + offsetPoint.Y);

                                //    textSprite.Rotation = obj.DisplayObject.Rotation;

                                //    textSprite.SetScale(worldScale, worldScale);

                                //}
                            }

                        }
                    }
                }
            }

        }


        public void UpdateSpriteContent(CoronaControl control, float worldScale, Point offSetPoint)
        {
            try
            {
                if (this.currentProject != null)
                {
                    if (control != null)
                    {
                        if (control.Type == CoronaControl.ControlType.joystick)
                        {
                            JoystickControl joy = control as JoystickControl;

                            if (joy.innerImage != null || joy.outerImage != null)
                            {
                                this.ExportControlToProjectResources(joy);
                            }

                            this.CleanSprite(joy, false);

                            //Charger le sprite
                            string innerPath = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images", joy.joystickName + "_inner.png");
                            if(File.Exists(innerPath))
                                joy.InnerGorgonSprite = new Sprite( joy.joystickName + "_inner", GorgonLibrary.Graphics.Image.FromFile(innerPath));

                            string outerPath = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images", joy.joystickName + "_outer.png");
                            if (File.Exists(outerPath))
                                 joy.OuterGorgonSprite = new Sprite(joy.joystickName + "_outer", GorgonLibrary.Graphics.Image.FromFile(outerPath));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateSpriteContent(CoronaObject obj,float worldScale,Point offSetPoint)
        {
            try
            {
                if (this.currentProject != null)
                {
                    if (obj.isEntity == true)
                    {
                        for (int i = 0; i < obj.Entity.CoronaObjects.Count; i++)
                        {
                            this.UpdateSpriteContent(obj.Entity.CoronaObjects[i], worldScale, offSetPoint);
                        }
                    }
                    else
                    {
                        if (obj.DisplayObject != null)
                        {
                            Sprite sprite = null;
                            if (obj.DisplayObject.Type.Equals("IMAGE"))
                            {
                                if (obj.DisplayObject.Image != null)
                                {
                                    this.ExportAssetToProjectResources(obj);
                                }

                                this.CleanSprite(obj, false, true);

                                //Charger le sprite
                                string filename = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images", obj.DisplayObject.OriginalAssetName + ".png");
                                sprite = new Sprite(obj.DisplayObject.OriginalAssetName, GorgonLibrary.Graphics.Image.FromFile(filename));
                                obj.DisplayObject.GorgonSprite = sprite;

                            }
                            else if (obj.DisplayObject.Type.Equals("SPRITE"))
                            {
                                this.CleanSprite(obj, false, true);

                                bool needExport = false;
                                CoronaSpriteSet spriteSetParent = obj.DisplayObject.SpriteSet;
                                for (int i = 0; i < spriteSetParent.Frames.Count; i++)
                                {
                                    SpriteFrame frame = spriteSetParent.Frames[i];
                                    if (frame.Image != null)
                                    {
                                        needExport = true;
                                        frame.ImageSize = frame.Image.Size;
                                        
                                    }
                                }

                                if (needExport == true)
                                {
                                    this.ExportAssetToProjectResources(obj);
                                }


                                if (obj.DisplayObject.CurrentSequence == "DEFAULT")
                                {
                                    if (obj.DisplayObject.SpriteSet.Frames.Count > 0)
                                    {
                                        //Charger le sprite avec la current frame
                                        SpriteFrame spriteFrame = spriteSetParent.Frames[obj.DisplayObject.CurrentFrame];

                                        string sheetDirectory = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\SpriteSheets", spriteFrame.SpriteSheetParent.Name);
                                        string frameFileName = Path.Combine(sheetDirectory, spriteFrame.SpriteSheetParent.Name + "_frame" + obj.DisplayObject.CurrentFrame+".png");
                                        if (File.Exists(frameFileName))
                                        {
                                            sprite = new Sprite(obj.DisplayObject.OriginalAssetName, GorgonLibrary.Graphics.Image.FromFile(frameFileName));
                                            obj.DisplayObject.GorgonSprite = sprite;

                                        }
                                    }
                                }
                                else
                                {
                                    CoronaSpriteSetSequence sequence = obj.DisplayObject.getSequenceByName(obj.DisplayObject.CurrentSequence);
                                    if (sequence != null)
                                    {

                                        int convertedCurrentFrame = obj.DisplayObject.CurrentFrame + sequence.FrameDepart - 1;

                                        if (obj.DisplayObject.SpriteSet.Frames.Count > 0 && obj.DisplayObject.SpriteSet.Frames.Count > convertedCurrentFrame)
                                        {
                                            //Charger le sprite avec la current frame
                                            SpriteFrame spriteFrame = spriteSetParent.Frames[convertedCurrentFrame];

                                            string sheetDirectory = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\SpriteSheets", spriteFrame.SpriteSheetParent.Name);
                                            string frameFileName = Path.Combine(sheetDirectory, spriteFrame.SpriteSheetParent.Name + "_frame" + convertedCurrentFrame+".png");
                                            if (File.Exists(frameFileName))
                                            {
                                                sprite = new Sprite(obj.DisplayObject.OriginalAssetName, GorgonLibrary.Graphics.Image.FromFile(frameFileName));
                                                obj.DisplayObject.GorgonSprite = sprite;

                                            }
                                        }
                                        else
                                        {
                                            obj.DisplayObject.CurrentFrame = sequence.FrameCount - 1;
                                            this.UpdateSpriteContent(obj, worldScale, offSetPoint);
                                        }
                                    }
                                    else
                                    {
                                        obj.DisplayObject.setSequence("");

                                        if (obj.DisplayObject.SpriteSet.Frames.Count > 0)
                                        {
                                            //Charger le sprite avec la current frame
                                            SpriteFrame spriteFrame = spriteSetParent.Frames[obj.DisplayObject.CurrentFrame];

                                            string sheetDirectory = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\SpriteSheets", spriteFrame.SpriteSheetParent.Name);
                                            string frameFileName = Path.Combine(sheetDirectory, spriteFrame.NomFrame);
                                            if (File.Exists(frameFileName))
                                            {
                                                sprite = new Sprite(obj.DisplayObject.OriginalAssetName, GorgonLibrary.Graphics.Image.FromFile(frameFileName));
                                                obj.DisplayObject.GorgonSprite = sprite;

                                            }
                                        }
                                    }
                                }

                            }

                            else if (obj.DisplayObject.Type.Equals("FIGURE"))
                            {
                                Krea.CGE_Figures.Figure fig = obj.DisplayObject.Figure;

                                if (fig.ShapeType.Equals("RECTANGLE"))
                                {
                                    this.CleanSprite(obj, false, false);

                                    Bitmap textureFigure = fig.DrawInBitmap((int)(255.0f * obj.DisplayObject.Alpha), worldScale);

                                    sprite = new Sprite(obj.DisplayObject.Name, GorgonLibrary.Graphics.Image.FromBitmap("shape_" + obj.DisplayObject.Name, textureFigure));
                                    obj.DisplayObject.GorgonSprite = sprite;

                                    textureFigure.Dispose();
                                    textureFigure = null;

                                }
                                else if (fig.ShapeType.Equals("TEXT"))
                                {
                                    //this.CleanSprite(obj, false, false);

                                    //Krea.CGE_Figures.Texte texteObject = fig as Krea.CGE_Figures.Texte;

                                    
                                    //TextSprite textSprite = null;
                                    //if (texteObject.font2.FontItem == null)
                                    //    texteObject.font2.FontItem = new FontItem("DEFAULT", this.currentProject);


                                    //if (texteObject.font2.FontItem.NameForIphone.Equals("DEFAULT"))
                                    //{
                                        
                                    //    GorgonLibrary.Graphics.Font textFont = this.GetGorgonFont(texteObject.font2.FontItem.NameForIphone);
                                    //    if (textFont != null)
                                    //    {
                                    //        textSprite = new TextSprite(fig.DisplayObjectParent.Name, texteObject.txt, textFont);
                                    //    }
                                    //    else
                                    //    {
                                    //        textSprite = new TextSprite(fig.DisplayObjectParent.Name, texteObject.txt,
                                    //            new GorgonLibrary.Graphics.Font(texteObject.font2.FontItem.NameForIphone,
                                    //                new System.Drawing.Font(SystemFonts.DefaultFont.FontFamily, texteObject.font2.Size * worldScale)));
                                    //    }

                                    //    //textSprite = new TextSprite(fig.DisplayObjectParent.Name, texteObject.txt,
                                    //    //    new GorgonLibrary.Graphics.Font(texteObject.font2.FontItem.NameForIphone,
                                    //    //        new System.Drawing.Font( SystemFonts.DefaultFont.FontFamily,texteObject.font2.Size * worldScale)));
                                    //}
                                    //else
                                    //{
                                       
                                    //    GorgonLibrary.Graphics.Font textFont = this.GetGorgonFont(texteObject.font2.FontItem.NameForIphone);
                                    //    if (textFont != null)
                                    //    {
                                    //        textSprite = new TextSprite(fig.DisplayObjectParent.Name, texteObject.txt, textFont);
                                    //    }
                                    //    else
                                    //    {
                                    //        textSprite = new TextSprite(fig.DisplayObjectParent.Name, texteObject.txt,
                                    //        new GorgonLibrary.Graphics.Font(texteObject.font2.FontItem.NameForIphone,
                                    //            new System.Drawing.Font(texteObject.font2.FontItem.NameForIphone, texteObject.font2.Size * worldScale)));
                                    //    }
                                      
                                    //}

                                    //texteObject.TextSprite = textSprite;

                                    //Bitmap textureFigure = fig.DrawInBitmap((int)(255.0f * obj.DisplayObject.Alpha), worldScale);

                                    //sprite = new Sprite(obj.DisplayObject.Name, GorgonLibrary.Graphics.Image.FromBitmap("text_" + obj.DisplayObject.Name, textureFigure));
                                    //obj.DisplayObject.GorgonSprite = sprite;

                                }



                            }

                            if (sprite != null)
                            {
                                this.UpdateSpriteStates(obj, worldScale, offSetPoint);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }


      

        public List<CoronaSpriteSheet> GetSpriteSheetsUsedFromSpriteSet(CoronaSpriteSet set)
        {
            List<CoronaSpriteSheet> listSheets = new List<CoronaSpriteSheet>();

            for (int i = 0; i < set.Frames.Count; i++)
            {
                SpriteFrame frame = set.Frames[i];
                if (!listSheets.Contains(frame.SpriteSheetParent))
                    listSheets.Add(frame.SpriteSheetParent);

            }
            return listSheets;
        }

        public bool IsSpriteSheetUsedByProject(CoronaSpriteSheet sheet, CoronaObject sourceObject)
        {
            if (this.currentProject != null)
            {
                for (int i = 0; i < this.currentProject.Scenes.Count; i++)
                {
                    Scene scene = this.currentProject.Scenes[i];
                    for (int j = 0; j < scene.Layers.Count; j++)
                    {
                        CoronaLayer layer = scene.Layers[j];
                        for (int k = 0; k < layer.CoronaObjects.Count; k++)
                        {

                            bool res = this.IsSpriteSheetUsedByObject(layer.CoronaObjects[k], sheet, sourceObject);
                            if (res == true)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsSpriteSheetUsedByObject(CoronaObject obj, CoronaSpriteSheet sheet, CoronaObject sourceObject)
        {
            if (obj != null)
            {
                if (obj.isEntity == true)
                {
                    CoronaEntity entity = obj.Entity;
                    for (int l = 0; l < entity.CoronaObjects.Count; l++)
                    {
                        CoronaObject child = entity.CoronaObjects[l];
                        bool res = this.IsSpriteSheetUsedByObject(child, sheet, sourceObject);
                        if (res == true)
                            return true;
                    }
                }
                else
                {
                    if (obj == sourceObject) return false;
                    if (obj.DisplayObject != null)
                    {

                        if (obj.DisplayObject.SpriteSet != null)
                        {
                            List<CoronaSpriteSheet> sheetUsedByObject = this.GetSpriteSheetsUsedFromSpriteSet(obj.DisplayObject.SpriteSet);
                            for (int i = 0; i < sheetUsedByObject.Count; i++)
                            {
                                CoronaSpriteSheet sheetUsed = sheetUsedByObject[i];
                                if (sheet.Name.Equals(sheetUsed.Name))
                                    return true;
                            }

                        }
                    }
                }
            }

            return false;
        }


        public void CleanSprite(CoronaControl control, bool removeAsset)
        {
            if (this.currentProject != null)
            {
                if (control != null)
                {
                    if (control.Type == CoronaControl.ControlType.joystick)
                    {
                        JoystickControl joy = control as JoystickControl;

                        if (joy.InnerGorgonSprite != null)
                        {
                            joy.InnerGorgonSprite.Image.Dispose();
                            joy.InnerGorgonSprite.Image = null;

                            joy.InnerGorgonSprite = null;
                        }


                        if (joy.OuterGorgonSprite != null)
                        {
                            joy.OuterGorgonSprite.Image.Dispose();
                            joy.OuterGorgonSprite.Image = null;

                            joy.OuterGorgonSprite = null;
                        }


                        if (removeAsset == true)
                        {
                            string innerPath = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images",
                                                            joy.joystickName + "_inner.png");
                            if (File.Exists(innerPath))
                            {
                                File.Delete(innerPath);
                            }

                            string outerPath = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images",
                                                            joy.joystickName + "_outer.png");
                            if (File.Exists(outerPath))
                            {
                                File.Delete(outerPath);
                            }
                        }
                    }
                }
            }

        }

        public void CleanSprite(CoronaObject obj,bool removeAsset, bool checkIntegrity)
        {
            if (this.currentProject != null)
            {
                if (obj.DisplayObject != null)
                {
                    Sprite sprite = null;
                    if (obj.DisplayObject.Type.Equals("IMAGE"))
                    {
                        sprite = obj.DisplayObject.GorgonSprite;
                        if(sprite != null)
                        {
                            if (checkIntegrity == true)
                            {
                                bool isImageUsed = this.IsImageUsedByProject(sprite.Image, obj);
                                if (isImageUsed == false)
                                {
                                    sprite.Image.Dispose();


                                    if (removeAsset == true)
                                    {
                                        string assetPath = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images",
                                                                        obj.DisplayObject.OriginalAssetName + ".png");
                                        if (File.Exists(assetPath))
                                        {
                                            File.Delete(assetPath);
                                        }
                                    }
                                }


                            }
                            else
                            {
                                sprite.Image.Dispose();

                                if (removeAsset == true)
                                {
                                    string assetPath = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images",
                                                                    obj.DisplayObject.OriginalAssetName + ".png");
                                    if (File.Exists(assetPath))
                                    {
                                        File.Delete(assetPath);
                                    }
                                }
                            }


                        }
                       
                    }
                    else if (obj.DisplayObject.Type.Equals("SPRITE"))
                    {
                        sprite = obj.DisplayObject.GorgonSprite;
                        if (sprite != null)
                        {
                            if (checkIntegrity == true)
                            {
                                bool isImageUsed = this.IsImageUsedByProject(sprite.Image, obj);
                                if (isImageUsed == false)
                                {
                                    sprite.Image.Dispose();

                                    if (removeAsset == true)
                                    {
                                        List<CoronaSpriteSheet> sheetUsedbyObject = this.GetSpriteSheetsUsedFromSpriteSet(obj.DisplayObject.SpriteSet);
                                        for (int i = 0; i < sheetUsedbyObject.Count; i++)
                                        {
                                            CoronaSpriteSheet sheetUsed = sheetUsedbyObject[i];
                                            bool isUsed = this.IsSpriteSheetUsedByProject(sheetUsed, obj);
                                            if (isUsed == false)
                                            {
                                                string sheetDirectory = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\SpriteSheets",
                                                                    sheetUsed.Name);

                                                if (Directory.Exists(sheetDirectory))
                                                {
                                                    Directory.Delete(sheetDirectory, true);
                                                }
                                            }
                                        }

                                    }
                                }

                               

                            }
                            else
                            {
                                sprite.Image.Dispose();

                                if (removeAsset == true)
                                {
                                    List<CoronaSpriteSheet> sheetUsedbyObject = this.GetSpriteSheetsUsedFromSpriteSet(obj.DisplayObject.SpriteSet);
                                    for (int i = 0; i < sheetUsedbyObject.Count; i++)
                                    {
                                        CoronaSpriteSheet sheetUsed = sheetUsedbyObject[i];

                                        string sheetDirectory = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\SpriteSheets",
                                                            sheetUsed.Name);

                                        if (Directory.Exists(sheetDirectory))
                                        {
                                            Directory.Delete(sheetDirectory, true);
                                        }
                                   
                                    }

                                }
                            }
                        }
                    }
                    else if (obj.DisplayObject.Type.Equals("FIGURE"))
                    {
                        Krea.CGE_Figures.Figure fig = obj.DisplayObject.Figure;

                        if (fig.ShapeType.Equals("RECTANGLE"))
                        {
                            sprite = obj.DisplayObject.GorgonSprite;
                            if (sprite != null && sprite.Image != null)
                            {
                                
                                sprite.Image.Dispose();
                                sprite.Image = null;
                            }
                        }
                        else if (fig.ShapeType.Equals("TEXT"))
                        {
                            Krea.CGE_Figures.Texte textObject = fig as Krea.CGE_Figures.Texte;

                           
                        }
                    }


                    sprite = null;
                    obj.DisplayObject.GorgonSprite = null;
                }
            }
        }

        public void ExportControlToProjectResources(CoronaControl control)
        {
            if (this.currentProject != null)
            {
                if (!Directory.Exists(this.currentProject.ProjectPath + "\\Resources\\Images"))
                    Directory.CreateDirectory(this.currentProject.ProjectPath + "\\Resources\\Images");

                if (control != null)
                {
                    if (control.Type == CoronaControl.ControlType.joystick)
                    {
                        JoystickControl joy = control as JoystickControl;

                        if (joy.innerImage != null)
                        {
                            string filename = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images", joy.joystickName+"_inner.png");
                            if (File.Exists(filename))
                                File.Delete(filename);

                            joy.innerImage.Save(filename);
                        }

                        if (joy.outerImage != null)
                        {
                            string filename = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images", joy.joystickName + "_outer.png");
                            if (File.Exists(filename))
                                File.Delete(filename);

                            joy.outerImage.Save(filename);
                        }
                    }
                }
            }
        }
        public void ExportAssetToProjectResources(CoronaObject obj)
        {
            if (this.currentProject != null)
            {
                if (!Directory.Exists(this.currentProject.ProjectPath + "\\Resources\\Images"))
                    Directory.CreateDirectory(this.currentProject.ProjectPath + "\\Resources\\Images");

                if (obj.DisplayObject != null)
                {
                    if (obj.DisplayObject.Type.Equals("IMAGE"))
                    {
                        if (obj.DisplayObject.Image != null)
                        {
                            string filename = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\Images", obj.DisplayObject.OriginalAssetName + ".png");
                            if (File.Exists(filename))
                                File.Delete(filename);

                            obj.DisplayObject.Image.Save(filename);
                        }
                    }
                    else if (obj.DisplayObject.Type.Equals("SPRITE"))
                    {
                        CoronaSpriteSet set = obj.DisplayObject.SpriteSet;
                        if (set != null)
                        {
                            List<CoronaSpriteSheet> spriteSheetsUsed = new List<CoronaSpriteSheet>();
                            for (int i = 0; i < set.Frames.Count; i++)
                            {
                                if (!spriteSheetsUsed.Contains(set.Frames[i].SpriteSheetParent))
                                    spriteSheetsUsed.Add(set.Frames[i].SpriteSheetParent);

                            }

                            for (int i = 0; i < spriteSheetsUsed.Count; i++)
                            {
                                CoronaSpriteSheet sheet = spriteSheetsUsed[i];
                                string sheetDirectory = Path.Combine(this.currentProject.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                                if (Directory.Exists(sheetDirectory))
                                    Directory.Delete(sheetDirectory, true);

                                Directory.CreateDirectory(sheetDirectory);


                                for (int j = 0; j < sheet.Frames.Count; j++)
                                {
                                    SpriteFrame frame = sheet.Frames[j];
                                    if (frame.Image != null)
                                    {
                                        frame.ImageSize = frame.Image.Size;
                                        string framePath = Path.Combine(sheetDirectory, sheet.Name + "_frame" + j + ".png");

                                        if (!File.Exists(framePath))
                                            frame.Image.Save(framePath);
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        public bool IsImageUsedByProject(GorgonLibrary.Graphics.Image image,CoronaObject sourceObject)
        {
            if (this.currentProject != null)
            {
                for (int i = 0; i < this.currentProject.Scenes.Count; i++)
                {
                    Scene scene = this.currentProject.Scenes[i];
                    for (int j = 0; j < scene.Layers.Count; j++)
                    {
                        CoronaLayer layer = scene.Layers[j];
                        for (int k = 0; k < layer.CoronaObjects.Count; k++)
                        {
                            bool res = this.IsImageUsedByObject(layer.CoronaObjects[k], image, sourceObject);
                            if (res == true)
                                return true;
                        }

                       
                    }
                }
            }

            return false;
        }


     
        public bool IsImageUsedByObject(CoronaObject obj, GorgonLibrary.Graphics.Image image,CoronaObject sourceObject)
        {
            if (obj != null)
            {
                if (obj.isEntity == true)
                {
                    CoronaEntity entity = obj.Entity;
                    for (int l = 0; l < entity.CoronaObjects.Count; l++)
                    {
                        CoronaObject child = entity.CoronaObjects[l];
                        bool res = this.IsImageUsedByObject(child, image, sourceObject);
                        if (res == true)
                            return true;
                    }
                }
                else
                {
                    if (obj == sourceObject) return false;
                    if (obj.DisplayObject != null)
                    {
                        
                        //if (obj.DisplayObject.Type.Equals("IMAGE"))
                        //{
                        //    if (obj == sourceObject) return false;
                        //    else if (obj.DisplayObject.OriginalAssetName.Equals(image.Name))
                        //        return true;
                        //}
                        //else if (obj.DisplayObject.Type.Equals("SPRITE"))
                        //{

                        //}

                        if (obj.DisplayObject.GorgonSprite != null)
                        {
                            if (obj.DisplayObject.GorgonSprite.Image == image)
                                return true;

                        }
                    }
                }
            }

            return false;
        }

        //public GorgonLibrary.Graphics.Image GetImage(string filename)
        //{
        //    for (int i = 0; i < this.LoadedImages.Count; i++)
        //    {
        //        if (this.LoadedImages[i].Name.Equals(Path.GetFileNameWithoutExtension(filename)))
        //        {
        //            return this.LoadedImages[i];
        //        }
        //    }

        //    //Try to Load image
        //    if (this.currentProject != null)
        //    {
        //        GorgonLibrary.Graphics.Image image = this.LoadImage(filename);
        //        return image;
        //    }

        //    return null;
        //}

        //public GorgonLibrary.Graphics.Image LoadImage(string filename)
        //{
        //    //Try to Load image
        //    if (this.currentProject != null)
        //    {
        //        string finalPath = Path.Combine(this.currentProject.ProjectPath, filename);
        //        if(File.Exists(finalPath))
        //        {
        //            GorgonLibrary.Graphics.Image image = GorgonLibrary.Graphics.Image.FromFile(finalPath);
        //            if (!this.LoadedImages.Contains(image))
        //            {
        //                this.LoadedImages.Add(image);
        //            }

        //            return image;
        //        }
        //    }

        //    return null;
        //}

        //public void CleanProjectGraphics()
        //{
        //    if (this.currentProject != null)
        //    {

        //        for (int i = 0; i < this.LoadedImages.Count; i++)
        //        {
        //            this.LoadedImages[i].Dispose();
        //        }

        //        this.LoadedImages.Clear();
        //        this.currentProject = null;
        //    }

            
        //}

        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------ TILE MAP CONTENT MANAGER ------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
       

       


    }
}
