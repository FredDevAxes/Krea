
using System;
using System.Collections.Generic;
using System.Drawing;
using Krea.CoronaClasses;
using System.IO;
using System.Windows.Forms;
using Krea.Corona_Classes.Controls;
using Krea.Corona_Classes.Widgets;
using Krea.Asset_Manager;
namespace Krea.GameEditor
{
    class ImageSuffix
    {
        public String suffix;
        public Double ratio;

        public ImageSuffix() { }
        public ImageSuffix(String ImageSuffix) { 
          //  ["@2x"] = 2;
            try
            {
                String stringtmp = ImageSuffix.Replace("[\"", "");
                stringtmp = stringtmp.Replace("\"] ", "");
                stringtmp = stringtmp.Replace(" ", "");
                String[] result = stringtmp.Split('=');
                this.suffix = result[0];
                this.ratio = Convert.ToDouble(result[1].Replace('.',','));
            }
            catch {
                return;
            }
        }
    }
    class ImageResizer
    {
        /*  
         *  for iPad, it is 72x72 px (icon-72.png)
         *  for iPhone 3G/3GS, it is 57x57 px (icon.png)
         *  for iPhone 4/Retina screen, it is 114x114px (icon@2x.png)
         *  for iTunes connect, it is 512x512 px
         */

        public CoronaGameProject Project { get; set; }

        public ImageResizer(){}
        public ImageResizer(CoronaGameProject CurrentProject)
        {

            if (CurrentProject == null) return;
         
            Project = CurrentProject;
            
        }

        public Boolean generateImageFilesWithRatio(float XRatio,float YRatio)
        {
            if (this.Project == null) return false;
            if (this.Project.ImageSuffix == null) return false;

            float moyenneRatio = (XRatio + YRatio) / 2;

            try
            {
                for (int i = 0; i < this.Project.Scenes.Count; i++)
                {
                    if (this.Project.Scenes[i].isEnabled == true)
                    {

                        for (int j = 0; j < this.Project.Scenes[i].Layers.Count; j++)
                        {
                            if (this.Project.Scenes[i].Layers[j].isEnabled == true)
                            {
                                for (int k = 0; k < this.Project.Scenes[i].Layers[j].CoronaObjects.Count; k++)
                                {
                                    CoronaObject coronaObject = this.Project.Scenes[i].Layers[j].CoronaObjects[k];
                                    if (coronaObject.isEnabled == true)
                                    {
                                        if (coronaObject.isEntity == false)
                                        {
                                            DisplayObject obj = coronaObject.DisplayObject;
                                            if (obj.Type.Equals("IMAGE"))
                                            {

                                                if (obj.GorgonSprite != null)
                                                {
                                                      string imagePath =  Path.Combine(this.Project.ProjectPath + "\\Resources\\Images",
                                                            obj.OriginalAssetName+".png");

                                                      Size finalSize = new Size(Convert.ToInt32(obj.SurfaceRect.Size.Width * XRatio), Convert.ToInt32(obj.SurfaceRect.Size.Height * YRatio));
                                                      string finalImageName = Project.BuildFolderPath + "\\" + obj.OriginalAssetName.ToLower() + finalSize.Width + "x" + finalSize.Height;
                                                      if (File.Exists(imagePath))
                                                      {
                                                          if (!File.Exists(finalImageName + ".png"))
                                                          {
                                                              Bitmap originalImage = (Bitmap)Bitmap.FromFile(imagePath);
                                                              //Bitmap originalImage = (Bitmap)obj.GorgonSprite.Image.SaveBitmap();

                                                              Image finalImage = new Bitmap(originalImage, finalSize);
                                                              finalImage.Save(finalImageName + ".png",
                                                                  System.Drawing.Imaging.ImageFormat.Png);
                                                              finalImage.Dispose();
                                                              finalImage = null;

                                                              originalImage.Dispose();
                                                              originalImage = null;
                                                          }
                                                      }
                                                }

                                            }

                                            if (coronaObject.BitmapMask != null)
                                            {
                                                if (coronaObject.BitmapMask.MaskImage != null && coronaObject.BitmapMask.IsMaskEnabled == true)
                                                {
                                                    Image finalImage = new Bitmap(coronaObject.BitmapMask.MaskImage,
                                                        new Size(Convert.ToInt32(coronaObject.BitmapMask.MaskImage.Size.Width * moyenneRatio),
                                                                    Convert.ToInt32(coronaObject.BitmapMask.MaskImage.Size.Height * moyenneRatio)));
                                                    finalImage.Save(Project.BuildFolderPath + "\\" + obj.Name.ToLower() + "_mask.png", System.Drawing.Imaging.ImageFormat.Png);
                                                    finalImage.Dispose();
                                                    finalImage = null;
                                                
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int e = 0; e < coronaObject.Entity.CoronaObjects.Count; e++)
                                            {
                                                CoronaObject child = coronaObject.Entity.CoronaObjects[e];
                                                if (child.isEnabled == true)
                                                {
                                                    DisplayObject obj = child.DisplayObject;
                                                    if (obj.Type.Equals("IMAGE"))
                                                    {

                                                        if (obj.GorgonSprite != null)
                                                        {
                                                            string imagePath = Path.Combine(this.Project.ProjectPath + "\\Resources\\Images",
                                                          obj.OriginalAssetName + ".png");

                                                            Size finalSize = new Size(Convert.ToInt32(obj.SurfaceRect.Size.Width * XRatio), Convert.ToInt32(obj.SurfaceRect.Size.Height * YRatio));
                                                            string finalImageName = Project.BuildFolderPath + "\\" + obj.OriginalAssetName.ToLower() + finalSize.Width + "x" + finalSize.Height;
                                                            if (File.Exists(imagePath))
                                                            {
                                                                if (!File.Exists(finalImageName + ".png"))
                                                                {
                                                                    Bitmap originalImage = (Bitmap)Bitmap.FromFile(imagePath);
                                                                    //Bitmap originalImage = (Bitmap)obj.GorgonSprite.Image.SaveBitmap();
                                                                   
                                                                    Image finalImage = new Bitmap(originalImage, finalSize);
                                                                    finalImage.Save(finalImageName + ".png",
                                                                        System.Drawing.Imaging.ImageFormat.Png);
                                                                    finalImage.Dispose();
                                                                    finalImage = null;
                                                                    originalImage.Dispose();
                                                                    originalImage = null;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (coronaObject.BitmapMask != null)
                                                    {
                                                        if (coronaObject.BitmapMask.MaskImage != null && coronaObject.BitmapMask.IsMaskEnabled == true)
                                                        {
                                                            Image finalImage = new Bitmap(coronaObject.BitmapMask.MaskImage,
                                                                new Size(Convert.ToInt32(coronaObject.BitmapMask.MaskImage.Size.Width * moyenneRatio),
                                                                            Convert.ToInt32(coronaObject.BitmapMask.MaskImage.Size.Height * moyenneRatio)));
                                                            finalImage.Save(Project.BuildFolderPath + "\\" + obj.Name.ToLower() + "_mask.png", System.Drawing.Imaging.ImageFormat.Png);

                                                            finalImage.Dispose();
                                                            finalImage = null;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            Scene scene = this.Project.Scenes[i];


                            //scene.createAllSpriteSheetsImages("", moyenneRatio, moyenneRatio);

                            for (int l = 0; l < scene.SpriteSheets.Count; l++)
                            {

                                CoronaSpriteSheet sheet = scene.SpriteSheets[l];

                             
                                //if (size.Width >= 4096 || size.Height >= 4096)
                                //{
                                //    MessageBox.Show("The size of the " + sheet.Name + " sprite sheet image seems to be too big (4096 max)!\n Please go to the asset manager and increase the frames factor for this sheet!",
                                //        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //}

                                //Creer le fichier lua associé
                                SpriteSheetLuaGenerator gen = new SpriteSheetLuaGenerator(sheet,this.Project);
                                gen.writeToLua(new DirectoryInfo(this.Project.BuildFolderPath), "", moyenneRatio, moyenneRatio);
                            }


                            for (int l = 0; l < scene.Layers.Count; l++)
                            {
                                CoronaLayer layer = scene.Layers[l];
                                if (layer.isEnabled == true)
                                {
                                    if (layer.TilesMap != null)
                                    {
                                        if (layer.TilesMap.isEnabled == true)
                                        {
                                            string texturesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "textsheet";
                                            string objectsDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "objsheet";
                                            string textureSequencesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "textsequencessheet";
                                            string objectSequencesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "objsequencessheet";

                                            float orientedXRatio = 0;
                                            float orientedYRatio = 0;
                                            if (this.Project.Orientation == CoronaGameProject.OrientationScreen.Portrait)
                                            {
                                                orientedXRatio = XRatio;
                                                orientedYRatio = YRatio;
                                            }
                                            else
                                            {
                                                orientedXRatio = YRatio;
                                                orientedYRatio = XRatio;
                                            }

                                            layer.TilesMap.refreshTilesMapContent();
                                            layer.TilesMap.createTextureSet(this.Project.BuildFolderPath, texturesDataSheetName, "", orientedXRatio, orientedYRatio);
                                            layer.TilesMap.createObjectsSet(this.Project.BuildFolderPath, objectsDataSheetName, "", orientedXRatio, orientedYRatio);
                                            layer.TilesMap.creatTextureSequencesSet(this.Project.BuildFolderPath, textureSequencesDataSheetName, "", orientedXRatio, orientedYRatio);
                                            layer.TilesMap.createObjectSequencesSet(this.Project.BuildFolderPath, objectSequencesDataSheetName, "", orientedXRatio, orientedYRatio);


                                            string fileNameDest = layer.TilesMap.createJSONConfigFile(scene, this.Project.SourceFolderPath, orientedXRatio, orientedYRatio);
                                        }
                                    }
                                }

                            }

                            for (int k = 0; k < this.Project.Scenes[i].Layers[j].Controls.Count; k++)
                            {
                                CoronaControl control = this.Project.Scenes[i].Layers[j].Controls[k];
                                if (control.isEnabled == true)
                                {
                                    if (control.Type == CoronaControl.ControlType.joystick)
                                    {

                                        JoystickControl joy = control as JoystickControl;

                                        //Pour l'image OUTER
                                        if (joy.outerImage != null)
                                        {
                                            //Enregistrer l'image en size *ratio
                                            Size sizeBack = new Size(Convert.ToInt32(joy.outerRadius * 2 * XRatio), Convert.ToInt32(joy.outerRadius * 2 * moyenneRatio));
                                            Bitmap OuterImage = new Bitmap(joy.outerImage, sizeBack);
                                            OuterImage.Save(Project.BuildFolderPath + "\\" + joy.joystickName + "Outer.png", System.Drawing.Imaging.ImageFormat.Png);
                                            OuterImage.Dispose();

                                        }

                                        //Pour l'image Inner
                                        if (joy.innerImage != null)
                                        {
                                            //Enregistrer l'image en size *ratio
                                            Size sizeBack = new Size(Convert.ToInt32(joy.innerRadius * 2 * moyenneRatio), Convert.ToInt32(joy.innerRadius * 2 * moyenneRatio));
                                            Bitmap InnerImage = new Bitmap(joy.innerImage, sizeBack);
                                            InnerImage.Save(Project.BuildFolderPath + "\\" + joy.joystickName + "Inner.png", System.Drawing.Imaging.ImageFormat.Png);
                                            InnerImage.Dispose();
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean GenerateImageFile() {
            if (this.Project == null) return false;
            if (this.Project.ImageSuffix == null) return false;

            List<ImageSuffix> ListImgSuffix = new List<ImageSuffix>();

            // Fill All Image Suffix String in an Object
            for (int i = 0; i < this.Project.ImageSuffix.Count; i++) {
                ListImgSuffix.Add(new ImageSuffix(Project.ImageSuffix[i]));
            }

            try
            {
                for (int i = 0; i < this.Project.Scenes.Count; i++)
                {
                    if (this.Project.Scenes[i].isEnabled == true)
                    {
                        for (int j = 0; j < this.Project.Scenes[i].Layers.Count; j++)
                        {
                            if (this.Project.Scenes[i].Layers[j].isEnabled == true)
                            {
                                for (int k = 0; k < this.Project.Scenes[i].Layers[j].CoronaObjects.Count; k++)
                                {
                                    CoronaObject coronaObject = this.Project.Scenes[i].Layers[j].CoronaObjects[k];
                                    if (coronaObject.isEnabled == true)
                                    {
                                        if (coronaObject.isEntity == false)
                                        {
                                            DisplayObject obj = coronaObject.DisplayObject;
                                            if (obj.Type.Equals("IMAGE"))
                                            {

                                                //Enregistrer l'image en size *1
                                                if (!File.Exists(Project.BuildFolderPath + "\\" + obj.Name.ToLower() + ".png"))
                                                {
                                                    
                                                    if (obj.GorgonSprite != null)
                                                    {
                                                        //Bitmap originalImage = (Bitmap)obj.GorgonSprite.Image.SaveBitmap();
                                                        string imagePath =  Path.Combine(this.Project.ProjectPath + "\\Resources\\Images",
                                                            obj.OriginalAssetName+".png");

                                                        string finalImageName = Project.BuildFolderPath + "\\" + obj.OriginalAssetName.ToLower()+obj.SurfaceRect.Width+"x"+obj.SurfaceRect.Height ;
                                                        
                                                        if (File.Exists(imagePath))
                                                        {
                                                            if (!File.Exists(finalImageName+".png"))
                                                            {
                                                                Bitmap originalImage = (Bitmap)Bitmap.FromFile(imagePath);
                                                                Bitmap finalImage = new Bitmap(originalImage, obj.SurfaceRect.Size);
                                                                finalImage.Save(finalImageName + ".png",
                                                                    System.Drawing.Imaging.ImageFormat.Png);
                                                                finalImage.Dispose();

                                                                for (int l = 0; l < this.Project.ImageSuffix.Count; l++)
                                                                {
                                                                    Bitmap ImageResized = new Bitmap(originalImage, Convert.ToInt32(obj.SurfaceRect.Size.Width * ListImgSuffix[l].ratio),
                                                                        Convert.ToInt32(obj.SurfaceRect.Size.Height * ListImgSuffix[l].ratio));

                                                                    ImageResized.Save(finalImageName + ListImgSuffix[l].suffix + ".png",
                                                                        System.Drawing.Imaging.ImageFormat.Png);

                                                                    ImageResized.Dispose();

                                                                }

                                                                originalImage.Dispose();
                                                            }
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    finalImage = new Bitmap(obj.Image, obj.SurfaceRect.Size);
                                                    //    finalImage.Save(Project.BuildFolderPath + "\\" + obj.Name.ToLower() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                                    //    finalImage.Dispose();
                                                    //}

                                                  

                                                }

                                            }

                                            if (coronaObject.BitmapMask != null)
                                            {
                                                if (coronaObject.BitmapMask.MaskImage != null && coronaObject.BitmapMask.IsMaskEnabled == true)
                                                {
                                                    coronaObject.BitmapMask.MaskImage.Save(Project.BuildFolderPath + "\\" + obj.Name.ToLower() + "_mask.png", System.Drawing.Imaging.ImageFormat.Png);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int e = 0; e < coronaObject.Entity.CoronaObjects.Count; e++)
                                            {
                                                CoronaObject child = coronaObject.Entity.CoronaObjects[e];
                                                if (child.isEnabled == true)
                                                {
                                                    DisplayObject obj = child.DisplayObject;
                                                    if (obj.Type.Equals("IMAGE"))
                                                    {

                                                        //Enregistrer l'image en size *1
                                                        if (!File.Exists(Project.BuildFolderPath + "\\" + obj.Name.ToLower() + ".png"))
                                                        {
                                                            if (obj.GorgonSprite != null)
                                                            {
                                                                string imagePath = Path.Combine(this.Project.ProjectPath + "\\Resources\\Images",
                                                                  obj.OriginalAssetName + ".png");

                                                                string finalImageName = Project.BuildFolderPath + "\\" + obj.OriginalAssetName.ToLower() + obj.SurfaceRect.Width + "x" + obj.SurfaceRect.Height;

                                                                if (File.Exists(imagePath))
                                                                {
                                                                    if (!File.Exists(finalImageName + ".png"))
                                                                    {
                                                                        Bitmap originalImage = (Bitmap)Bitmap.FromFile(imagePath);
                                                                        //Bitmap originalImage = (Bitmap)obj.GorgonSprite.Image.SaveBitmap();

                                                                        Image finalImage = new Bitmap(originalImage, obj.SurfaceRect.Size);
                                                                        finalImage.Save(finalImageName + ".png",
                                                                            System.Drawing.Imaging.ImageFormat.Png);
                                                                        finalImage.Dispose();

                                                                        for (int l = 0; l < this.Project.ImageSuffix.Count; l++)
                                                                        {
                                                                            Bitmap ImageResized = new Bitmap(originalImage, Convert.ToInt32(obj.SurfaceRect.Size.Width * ListImgSuffix[l].ratio), Convert.ToInt32(obj.SurfaceRect.Size.Height * ListImgSuffix[l].ratio));
                                                                            ImageResized.Save(finalImageName + ListImgSuffix[l].suffix + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                                                            ImageResized.Dispose();

                                                                        }

                                                                        originalImage.Dispose();
                                                                    }
                                                                }
                                                            }

                                                        }


                                                    }



                                                    if (child.BitmapMask != null)
                                                    {
                                                        if (child.BitmapMask.MaskImage != null && child.BitmapMask.IsMaskEnabled == true)
                                                        {
                                                            child.BitmapMask.MaskImage.Save(Project.BuildFolderPath + "\\" + obj.Name.ToLower() + "_mask.png", System.Drawing.Imaging.ImageFormat.Png);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                            }

                            Scene scene = this.Project.Scenes[i];


                           // scene.createAllSpriteSheetsImages("", 1, 1);

                            for (int l = 0; l < scene.SpriteSheets.Count; l++)
                            {

                                CoronaSpriteSheet sheet = scene.SpriteSheets[l];
                             
                                //if (size.Width >= 4096 || size.Height >= 4096)
                                //{
                                //    MessageBox.Show("The size of the " + sheet.Name + " sprite sheet image seems to be too big (4096 max)!\n Please go to the asset manager and increase the frames factor for this sheet!",
                                //        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //}

                                //Creer le fichier lua associé
                                SpriteSheetLuaGenerator gen = new SpriteSheetLuaGenerator(sheet,this.Project);
                                gen.writeToLua(new DirectoryInfo(this.Project.BuildFolderPath), "", 1, 1);
                            }

                            for (int l = 0; l < scene.Layers.Count; l++)
                            {
                                CoronaLayer layer = scene.Layers[l];
                                if (layer.isEnabled == true)
                                {
                                    if (layer.TilesMap != null)
                                    {
                                        if (layer.TilesMap.isEnabled == true)
                                        {
                                            string texturesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "textsheet";
                                            string objectsDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "objsheet";
                                            string textureSequencesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "textsequencessheet";
                                            string objectSequencesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "objsequencessheet";

                                           
                                            layer.TilesMap.createTextureSet(this.Project.BuildFolderPath, texturesDataSheetName, "", 1, 1);
                                            layer.TilesMap.createObjectsSet(this.Project.BuildFolderPath, objectsDataSheetName, "", 1, 1);
                                            layer.TilesMap.creatTextureSequencesSet(this.Project.BuildFolderPath, textureSequencesDataSheetName, "", 1, 1);
                                            layer.TilesMap.createObjectSequencesSet(this.Project.BuildFolderPath, objectSequencesDataSheetName, "", 1, 1);
                                            layer.TilesMap.refreshTilesMapContent();

                                            string fileNameDest = layer.TilesMap.createJSONConfigFile(scene, this.Project.SourceFolderPath, 1, 1);
                                        }
                                    }
                                }

                            }

                            //Creer les sprites sheets
                            for (int k = 0; k < this.Project.ImageSuffix.Count; k++)
                            {
                               // scene.createAllSpriteSheetsImages(ListImgSuffix[k].suffix, (float)ListImgSuffix[k].ratio, (float)ListImgSuffix[k].ratio);

                                for (int l = 0; l < scene.SpriteSheets.Count; l++)
                                {

                                    CoronaSpriteSheet sheet = scene.SpriteSheets[l];
                                 

                                    //if (size.Width >= 4096 || size.Height >= 4096)
                                    //{
                                    //    MessageBox.Show("The size of the " + sheet.Name + " sprite sheet image seems to be too big (4096 max)!\n Please go to the asset manager and increase the frames factor for this sheet!",
                                    //        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //}

                                    //Creer le fichier lua associé
                                    SpriteSheetLuaGenerator gen = new SpriteSheetLuaGenerator(sheet,this.Project);
                                    gen.writeToLua(new DirectoryInfo(this.Project.BuildFolderPath), ListImgSuffix[k].suffix, (float)ListImgSuffix[k].ratio, (float)ListImgSuffix[k].ratio);
                                }

                                for (int l = 0; l < scene.Layers.Count; l++)
                                {
                                    CoronaLayer layer = scene.Layers[l];
                                    if (layer.isEnabled == true)
                                    {
                                        if (layer.TilesMap != null)
                                        {
                                            if (layer.TilesMap.isEnabled == true)
                                            {
                                                string texturesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "textsheet";
                                                string objectsDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "objsheet";
                                                string textureSequencesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "textsequencessheet";
                                                string objectSequencesDataSheetName = layer.TilesMap.TilesMapName.ToLower() + "objsequencessheet";

                                                layer.TilesMap.refreshTilesMapContent();
                                                layer.TilesMap.createTextureSet(this.Project.BuildFolderPath, texturesDataSheetName, ListImgSuffix[k].suffix, (float)ListImgSuffix[k].ratio, (float)ListImgSuffix[k].ratio);
                                                layer.TilesMap.createObjectsSet(this.Project.BuildFolderPath, objectsDataSheetName, ListImgSuffix[k].suffix, (float)ListImgSuffix[k].ratio, (float)ListImgSuffix[k].ratio);
                                                layer.TilesMap.creatTextureSequencesSet(this.Project.BuildFolderPath, textureSequencesDataSheetName, ListImgSuffix[k].suffix, (float)ListImgSuffix[k].ratio, (float)ListImgSuffix[k].ratio);
                                                layer.TilesMap.createObjectSequencesSet(this.Project.BuildFolderPath, objectSequencesDataSheetName, ListImgSuffix[k].suffix, (float)ListImgSuffix[k].ratio, (float)ListImgSuffix[k].ratio);


                                                //string fileNameDest = layer.TilesMap.createJSONConfigFile(scene, this.Project.SourceFolderPath, (float)ListImgSuffix[k].ratio, (float)ListImgSuffix[k].ratio);
                                            }
                                        }
                                    }


                                }
                            }

                            for (int k = 0; k < this.Project.Scenes[i].Layers[j].Controls.Count; k++)
                            {
                                CoronaControl control = this.Project.Scenes[i].Layers[j].Controls[k];
                                if (control.isEnabled == true)
                                {
                                    if (control.Type == CoronaControl.ControlType.joystick)
                                    {

                                        JoystickControl joy = control as JoystickControl;

                                        //Pour l'image OUTER
                                        if (joy.outerImage != null)
                                        {
                                            //Enregistrer l'image en size *1
                                            Size sizeBack = new Size(joy.outerRadius * 2, joy.outerRadius * 2);
                                            Bitmap OuterImage = new Bitmap(joy.outerImage, sizeBack);
                                            OuterImage.Save(Project.BuildFolderPath + "\\" + joy.joystickName + "Outer.png", System.Drawing.Imaging.ImageFormat.Png);

                                            for (int l = 0; l < this.Project.ImageSuffix.Count; l++)
                                            {
                                                Bitmap ImageResized = new Bitmap(joy.outerImage, Convert.ToInt32(sizeBack.Width * ListImgSuffix[l].ratio), Convert.ToInt32(sizeBack.Height * ListImgSuffix[l].ratio));
                                                ImageResized.Save(Project.BuildFolderPath + "\\" + joy.joystickName + "Outer" + ListImgSuffix[l].suffix + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                                ImageResized.Dispose();
                                                ImageResized = null;
                                            }

                                            OuterImage.Dispose();
                                            OuterImage = null;
                                        }

                                        //Pour l'image Inner
                                        if (joy.innerImage != null)
                                        {
                                            //Enregistrer l'image en size *1
                                            Size sizeBack = new Size(joy.innerRadius * 2, joy.innerRadius * 2);
                                            Bitmap InnerImage = new Bitmap(joy.innerImage, sizeBack);
                                            InnerImage.Save(Project.BuildFolderPath + "\\" + joy.joystickName + "Inner.png", System.Drawing.Imaging.ImageFormat.Png);

                                            for (int l = 0; l < this.Project.ImageSuffix.Count; l++)
                                            {
                                                Bitmap ImageResized = new Bitmap(joy.innerImage, Convert.ToInt32(sizeBack.Width * ListImgSuffix[l].ratio), Convert.ToInt32(sizeBack.Height * ListImgSuffix[l].ratio));
                                                ImageResized.Save(Project.BuildFolderPath + "\\" + joy.joystickName + "Inner" + ListImgSuffix[l].suffix + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                                ImageResized.Dispose();
                                                ImageResized = null;

                                            }

                                            InnerImage.Dispose();
                                            InnerImage = null;
                                        }

                                    }
                                }
                            }

                            for (int k = 0; k < this.Project.Scenes[i].Layers[j].Widgets.Count; k++)
                            {
                                CoronaWidget widget = this.Project.Scenes[i].Layers[j].Widgets[k];
                                if (widget.Type == CoronaWidget.WidgetType.tabBar)
                                {
                                    WidgetTabBar tabBar = (WidgetTabBar)widget;

                                    //Pour tous les bouttons
                                    for (int e = 0; e < tabBar.Buttons.Count; e++)
                                    {
                                        WidgetTabBar.TabBarButton bt = tabBar.Buttons[e];
                                        if (bt.ImagePressedState != null)
                                        {
                                            bt.ImagePressedState.Save(Project.BuildFolderPath + "\\" + tabBar.Name + bt.Id + "down.png");
                                        }
                                        if (bt.ImageUnPressedState != null)
                                        {
                                            bt.ImageUnPressedState.Save(Project.BuildFolderPath + "\\" + tabBar.Name + bt.Id + "up.png");
                                        }
                                    }

                                    /*if (tabBar.BackgroundImage != null)
                                    {
                                        Bitmap bmp = new Bitmap(tabBar.BackgroundImage, tabBar.Size);
                                        bmp.Save(Project.BuildFolderPath + "\\" + tabBar.Name + "background.png");
                                    }*/


                                }
                                else if (widget.Type == CoronaWidget.WidgetType.pickerWheel)
                                {
                                    WidgetPickerWheel pickW = (WidgetPickerWheel)widget;
                                    if (pickW.BackgroundImage != null)
                                    {
                                        Bitmap background = new Bitmap(pickW.BackgroundImage, pickW.BackgroundSize);
                                        background.Save(Project.BuildFolderPath + "\\" + pickW.Name + "background.png");
                                    }

                                }

                            }


                        }
                    }
                }
                return true;
            }
            catch (Exception ex){
                MessageBox.Show("Error during image files generation !\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// GenerateIconFile()
        /// Generate Multiple size of Icon file from the Icone object specified in the constructor.
        /// </summary>
        /// <returns>true/false</returns>
        public Boolean GenerateIconFile()
        {
            if (this.Project.Icone == null) this.Project.Icone = Properties.Resources.Icon;

            if (this.Project.BuildFolderPath.Equals("")) return false;

            try
            {
                //Get Original Icone File
                Image Icone = new Bitmap(this.Project.Icone);

                //Resize to the Correct Size
                Bitmap Icon57x57 = new Bitmap(Icone, 57, 57);
                Bitmap Icon36x36 = new Bitmap(Icone, 36, 36);
                Bitmap Icon48x48 = new Bitmap(Icone, 48, 48);
                Bitmap Icon72x72 = new Bitmap(Icone, 72, 72);
                Bitmap Icon114x114 = new Bitmap(Icone, 114, 114);
                Bitmap Icon512x512 = new Bitmap(Icone, 512, 512);
                //Save File

                //Iphone Icone File
                Icon57x57.Save(this.Project.BuildFolderPath + "\\Icon.png", System.Drawing.Imaging.ImageFormat.Png);
                Icon72x72.Save(this.Project.BuildFolderPath + "\\Icon-72.png", System.Drawing.Imaging.ImageFormat.Png);
                Icon114x114.Save(this.Project.BuildFolderPath + "\\Icon@2x.png", System.Drawing.Imaging.ImageFormat.Png);

                // Itune Store and Android Store Iphone
                Icon512x512.Save(this.Project.BuildFolderPath + "\\IconItunes.png", System.Drawing.Imaging.ImageFormat.Png);

                //Android Icone File
                Icon36x36.Save(this.Project.BuildFolderPath + "\\Icon-ldpi.png", System.Drawing.Imaging.ImageFormat.Png);
                Icon48x48.Save(this.Project.BuildFolderPath + "\\Icon-mdpi.png", System.Drawing.Imaging.ImageFormat.Png);
                Icon72x72.Save(this.Project.BuildFolderPath + "\\Icon-hdpi.png", System.Drawing.Imaging.ImageFormat.Png);

                Icon57x57.Dispose();
                Icon36x36.Dispose();
                Icon48x48.Dispose();
                Icon72x72.Dispose();
                Icon114x114.Dispose();
                Icon512x512.Dispose();

                Icone.Dispose();

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error during image resizing ! \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
        
    }
}

