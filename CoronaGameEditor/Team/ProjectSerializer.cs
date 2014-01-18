using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using Krea.Asset_Manager;
using Krea.CoronaClasses;
using System.Windows.Forms;

namespace Krea.Team
{
    public class ProjectSerializer
    {
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------SCENE ----------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static void importStage(Form1 mainForm,string filename, BackgroundWorker worker)
        {
            worker.ReportProgress(10);
            string directoryName = Path.GetDirectoryName(filename);

            FileStream fs = File.OpenRead(filename);
            if (fs.Length > 0)
            {
                MemoryStream ms = new MemoryStream();
                ms.SetLength(fs.Length);

                fs.Read(ms.GetBuffer(), 0, (int)ms.Length);
                worker.ReportProgress(30);
                Scene scene = (Scene)SerializerHelper.DeSerializeBinary(ms);
                fs.Close();
                ms.Close();
                worker.ReportProgress(50);
                if (scene != null)
                {
                    scene.projectParent = mainForm.CurrentProject;
                    //Ajouter la scene au projet
                    mainForm.CurrentProject.Scenes.Add(scene);

                    //Import the lua file si existant
                    if (File.Exists(filename.Replace(".krs", ".lua")))
                    {
                        string currentSceneName = scene.Name;
                        File.Copy(filename.Replace(".krs", ".lua"), mainForm.CurrentProject.SourceFolderPath + "\\" + currentSceneName + ".lua", true);


                        scene.Name = scene.projectParent.IncrementObjectName(scene.Name);
                        if(!scene.Name.Equals(currentSceneName))
                            File.Move(mainForm.CurrentProject.SourceFolderPath + "\\" + currentSceneName + ".lua", mainForm.CurrentProject.SourceFolderPath + "\\" + scene.Name + ".lua");

                    }
                    else
                    {

                        scene.Name = scene.projectParent.IncrementObjectName(scene.Name);
                        scene.createLuaFile();
                    }

                    if (Directory.Exists(directoryName + "\\Images"))
                    {
                        if (!Directory.Exists(scene.projectParent.ProjectPath + "\\Resources\\Images"))
                            Directory.CreateDirectory(scene.projectParent.ProjectPath + "\\Resources\\Images");

                        string[] files = Directory.GetFiles(directoryName + "\\Images");
                        for (int i = 0; i < files.Length; i++)
                        {
                            string fileNameOnly = Path.GetFileName(files[i]);
                            File.Copy(files[i], scene.projectParent.ProjectPath + "\\Resources\\Images\\" + fileNameOnly, true);
                        }
                    }

                    if (Directory.Exists(directoryName + "\\SpriteSheets"))
                    {
                        if (!Directory.Exists(scene.projectParent.ProjectPath + "\\Resources\\SpriteSheets"))
                            Directory.CreateDirectory(scene.projectParent.ProjectPath + "\\Resources\\SpriteSheets");

                        string[] directories = Directory.GetDirectories(directoryName + "\\SpriteSheets");
                        for (int i = 0; i < directories.Length; i++)
                        {
                            string directoryNameOnly = Path.GetFileName(directories[i]);

                            if (!Directory.Exists(scene.projectParent.ProjectPath + "\\Resources\\SpriteSheets\\" + directoryNameOnly))
                                Directory.CreateDirectory(scene.projectParent.ProjectPath + "\\Resources\\SpriteSheets\\" + directoryNameOnly);

                            string[] files = Directory.GetFiles(directories[i]);
                            for (int j = 0; j < files.Length; j++)
                            {
                                string fileNameOnly = Path.GetFileName(files[j]);
                                File.Copy(files[j], scene.projectParent.ProjectPath + "\\Resources\\SpriteSheets\\" +
                                    directoryNameOnly + "\\" + fileNameOnly, true);
                            }
                        }

                    }


                    //Traiter les fichiers necessaires aux tilesmap
                    worker.ReportProgress(60);
                    int prog = 60;
                    for (int i = 0; i < scene.Layers.Count; i++)
                    {
                        CoronaLayer layer = scene.Layers[i];
                        layer.SceneParent = scene;

                       
                        layer.Name = scene.projectParent.IncrementObjectName(layer.Name);

                        for (int k = 0; k < layer.CoronaObjects.Count; k++)
                        {
                            CoronaObject obj = layer.CoronaObjects[k];
                            if (obj.isEntity == false)
                            {
                                obj.DisplayObject.Name = scene.projectParent.IncrementObjectName(obj.DisplayObject.Name);
                            }
                            else
                            {
                                obj.Entity.Name = scene.projectParent.IncrementObjectName(obj.Entity.Name);
                                for (int j = 0; j < obj.Entity.CoronaObjects.Count; j++)
                                {
                                    obj.Entity.CoronaObjects[j].DisplayObject.Name = scene.projectParent.IncrementObjectName(obj.Entity.CoronaObjects[j].DisplayObject.Name);
                                }
                            }
                        }

                        if (layer.TilesMap != null)
                        {
                            worker.ReportProgress(prog + 5);

                            string oldMapName = layer.TilesMap.TilesMapName;
                            layer.TilesMap.TilesMapName = scene.projectParent.IncrementObjectName(layer.TilesMap.TilesMapName);

                            if (Directory.Exists(directoryName + "\\TileMaps"))
                            {
                                if (!Directory.Exists(scene.projectParent.ProjectPath + "\\Resources\\TileMaps\\" + layer.TilesMap.TilesMapName))
                                    Directory.CreateDirectory(scene.projectParent.ProjectPath + "\\Resources\\TileMaps\\" + layer.TilesMap.TilesMapName);

                                string[] directories = Directory.GetDirectories(directoryName + "\\TileMaps");
                                for (int m= 0; m < directories.Length; m++)
                                {
                                    string directoryNameOnly = Path.GetFileName(directories[m]);

                                    string[] files = Directory.GetFiles(directories[m]);
                                    for (int j = 0; j < files.Length; j++)
                                    {
                                        string fileNameOnly = Path.GetFileName(files[j]);
                                        File.Copy(files[j], scene.projectParent.ProjectPath + "\\Resources\\TileMaps\\" +
                                            layer.TilesMap.TilesMapName + "\\" + fileNameOnly, true);
                                    }
                                }

                                //Copy all JSON Config files of the tile map
                                string[] jsonFiles = Directory.GetFiles(directoryName, "*.json");
                                for (int j = 0; j < jsonFiles.Length; j++)
                                {
                                    string fileNameOnly = Path.GetFileName(jsonFiles[j]);
                                    if (fileNameOnly.Contains(oldMapName))
                                    {
                                        string newFileName = scene.projectParent.SourceFolderPath + "\\" + fileNameOnly.Replace(oldMapName, layer.TilesMap.TilesMapName);
                                        File.Copy(jsonFiles[j], newFileName, true);
                                    }
                                }
                            }

                            layer.TilesMap.reloadMapContent(scene.projectParent.SourceFolderPath);
                        


                        }

                        for (int j = 0; j < layer.CoronaObjects.Count; j++)
                        {
                            CoronaObject obj = layer.CoronaObjects[j];
                            mainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj, 1, System.Drawing.Point.Empty);
                        }

                        for (int j = 0; j < layer.Controls.Count; j++)
                        {
                            Corona_Classes.Controls.CoronaControl obj = layer.Controls[j];
                            mainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj, 1, System.Drawing.Point.Empty);
                        }

                        mainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();
                    }

                    worker.ReportProgress(90);
                    
                }
                else
                {
                    MessageBox.Show("Error during stage import !\n The file seems to be corrupted !", "Stage Loading Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    mainForm.currentWorkerAction = "NONE";
                }


            }

        }

        public static void exportStage(Form1 mainForm,Scene scene, string directoryName, BackgroundWorker worker)
        {
            worker.ReportProgress(20);
            string sceneSerializedDirectory = directoryName + "\\" + scene.Name;

            if (Directory.Exists(sceneSerializedDirectory))
                Directory.Delete(sceneSerializedDirectory, true);

            Directory.CreateDirectory(sceneSerializedDirectory);

            FileStream fs = File.Create(sceneSerializedDirectory + "\\" + scene.Name + ".krs");
            MemoryStream ms = SerializerHelper.SerializeBinary(scene);

            worker.ReportProgress(40);
            fs.Write(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Flush();
            fs.Close();
            worker.ReportProgress(60);

            if (File.Exists(mainForm.CurrentProject.SourceFolderPath + "\\" + scene.Name + ".lua"))
            {
                File.Copy(mainForm.CurrentProject.SourceFolderPath + "\\" + scene.Name + ".lua", sceneSerializedDirectory + "\\" + scene.Name + ".lua", true);
            }

            for (int i = 0; i < scene.Layers.Count; i++)
            {
                CoronaLayer layer = scene.Layers[i];
                if (layer.TilesMap != null)
                {
                    string mapResourcesDirectory = mainForm.CurrentProject.ProjectPath + "\\Resources\\TileMaps\\" + layer.TilesMap.TilesMapName;
                    if (Directory.Exists(mapResourcesDirectory))
                    {
                        Directory.CreateDirectory(sceneSerializedDirectory + "\\TileMaps\\" + layer.TilesMap.TilesMapName);

                        string[] files = Directory.GetFiles(mapResourcesDirectory);
                        for (int j = 0; j < files.Length; j++)
                        {
                            string fileNameOnly = Path.GetFileName(files[j]);
                            File.Copy(files[j], sceneSerializedDirectory + "\\TileMaps\\" + layer.TilesMap.TilesMapName + "\\" + fileNameOnly);
                        }
                    }

                    layer.TilesMap.createJSONConfigFile(layer.SceneParent, mainForm.CurrentProject.SourceFolderPath, 1, 1);

                    string path1 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "textures.json";
                    string path2 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objects.json";
                    string path3 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "collisions.json";
                    string path4 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "texturesequences.json";
                    string path5 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objectsequences.json";
                    string path6 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "events.json";

                    if (File.Exists(path1))
                        File.Copy(path1, sceneSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "textures.json", true);

                    if (File.Exists(path2))
                        File.Copy(path2, sceneSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objects.json", true);

                    if (File.Exists(path3))
                        File.Copy(path3, sceneSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "collisions.json", true);

                    if (File.Exists(path4))
                        File.Copy(path4, sceneSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "texturesequences.json", true);

                    if (File.Exists(path5))
                        File.Copy(path5, sceneSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objectsequences.json", true);

                    if (File.Exists(path6))
                        File.Copy(path6, sceneSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "events.json", true);



                }

                for (int m = 0; m < layer.CoronaObjects.Count; m++)
                {
                    CoronaObject obj = layer.CoronaObjects[m];
                    if (obj.isEntity == true)
                    {
                        CoronaEntity entity = obj.Entity;
                        for (int j = 0; j < entity.CoronaObjects.Count; j++)
                        {
                            CoronaObject child = entity.CoronaObjects[j];
                            if (child.DisplayObject != null)
                            {
                                if (child.DisplayObject.Type.Equals("IMAGE"))
                                {

                                    string filename = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\Images", child.DisplayObject.OriginalAssetName + ".png");
                                    if (File.Exists(filename))
                                    {
                                        if (!Directory.Exists(sceneSerializedDirectory + "\\Images"))
                                            Directory.CreateDirectory(sceneSerializedDirectory + "\\Images");

                                        File.Copy(filename, sceneSerializedDirectory + "\\Images\\" + child.DisplayObject.OriginalAssetName + ".png", true);
                                    }



                                }
                                else if (child.DisplayObject.Type.Equals("SPRITE"))
                                {
                                    CoronaSpriteSet set = child.DisplayObject.SpriteSet;
                                    if (set != null)
                                    {
                                        List<CoronaSpriteSheet> spriteSheetsUsed = new List<CoronaSpriteSheet>();
                                        for (int k = 0; k < set.Frames.Count; k++)
                                        {
                                            if (!spriteSheetsUsed.Contains(set.Frames[k].SpriteSheetParent))
                                                spriteSheetsUsed.Add(set.Frames[k].SpriteSheetParent);

                                        }

                                        for (int k = 0; k < spriteSheetsUsed.Count; k++)
                                        {
                                            CoronaSpriteSheet sheet = spriteSheetsUsed[k];
                                            string sheetDirectory = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                                            if (!Directory.Exists(sceneSerializedDirectory + "\\SpriteSheets\\" + sheet.Name))
                                            {
                                                Directory.CreateDirectory(sceneSerializedDirectory + "\\SpriteSheets\\" + sheet.Name);
                                            }

                                            string[] files = Directory.GetFiles(sheetDirectory);
                                            for (int l = 0; l < files.Length; l++)
                                            {
                                                string fileNameOnly = Path.GetFileName(files[l]);
                                                File.Copy(files[l], sceneSerializedDirectory + "\\SpriteSheets\\" + sheet.Name + "\\" + fileNameOnly, true);
                                            }



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
                            if (obj.DisplayObject.Type.Equals("IMAGE"))
                            {

                                string filename = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\Images", obj.DisplayObject.OriginalAssetName + ".png");
                                if (File.Exists(filename))
                                {
                                    if (!Directory.Exists(sceneSerializedDirectory + "\\Images"))
                                        Directory.CreateDirectory(sceneSerializedDirectory + "\\Images");

                                    File.Copy(filename, sceneSerializedDirectory + "\\Images\\" + obj.DisplayObject.OriginalAssetName + ".png", true);
                                }



                            }
                            else if (obj.DisplayObject.Type.Equals("SPRITE"))
                            {
                                CoronaSpriteSet set = obj.DisplayObject.SpriteSet;
                                if (set != null)
                                {
                                    List<CoronaSpriteSheet> spriteSheetsUsed = new List<CoronaSpriteSheet>();
                                    for (int j = 0; j < set.Frames.Count; j++)
                                    {
                                        if (!spriteSheetsUsed.Contains(set.Frames[j].SpriteSheetParent))
                                            spriteSheetsUsed.Add(set.Frames[j].SpriteSheetParent);

                                    }

                                    for (int j = 0; j < spriteSheetsUsed.Count; j++)
                                    {
                                        CoronaSpriteSheet sheet = spriteSheetsUsed[j];
                                        string sheetDirectory = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                                        if (!Directory.Exists(sceneSerializedDirectory + "\\SpriteSheets\\" + sheet.Name))
                                        {
                                            Directory.CreateDirectory(sceneSerializedDirectory + "\\SpriteSheets\\" + sheet.Name);
                                        }

                                        string[] files = Directory.GetFiles(sheetDirectory);
                                        for (int k = 0; k < files.Length; k++)
                                        {
                                            string fileNameOnly = Path.GetFileName(files[k]);
                                            File.Copy(files[k], sceneSerializedDirectory + "\\SpriteSheets\\" + sheet.Name + "\\" + fileNameOnly, true);
                                        }



                                    }
                                }
                            }
                        }
                    }
                }


                for (int m = 0; m < layer.Controls.Count; m++)
                {
                    Corona_Classes.Controls.CoronaControl control = layer.Controls[m];
                    if (control.type == Corona_Classes.Controls.CoronaControl.ControlType.joystick)
                    {
                        Corona_Classes.Controls.JoystickControl joy = control as Corona_Classes.Controls.JoystickControl;

                        string innerImage = mainForm.CurrentProject.ProjectPath + "\\Resources\\Images\\" + joy.joystickName + "_inner.png";
                        if (File.Exists(innerImage))
                            File.Copy(innerImage, sceneSerializedDirectory + "\\Images\\" + joy.joystickName + "_inner.png");

                        string outerImage = mainForm.CurrentProject.ProjectPath + "\\Resources\\Images\\" + joy.joystickName + "_outer.png";
                        if (File.Exists(outerImage))
                            File.Copy(outerImage, sceneSerializedDirectory + "\\Images\\" + joy.joystickName + "_outer.png");


                    }
                }
            }


            worker.ReportProgress(80);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------- LAYER ----------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
       
        public static void importLayer(Form1 mainForm, string filename, BackgroundWorker worker)
        {
            Scene sceneParent = mainForm.getElementTreeView().SceneSelected;
            if (sceneParent != null)
            {
                string directoryName = Path.GetDirectoryName(filename);

                worker.ReportProgress(10);
                FileStream fs = File.OpenRead(filename);
                if (fs.Length > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    ms.SetLength(fs.Length);

                    fs.Read(ms.GetBuffer(), 0, (int)ms.Length);
                    worker.ReportProgress(30);
                    CoronaLayer layer = (CoronaLayer)SerializerHelper.DeSerializeBinary(ms);
                    fs.Close();
                    ms.Close();
                    worker.ReportProgress(50);
                    if (layer != null)
                    {
                        layer.SceneParent = sceneParent;
                        //Ajouter la scene au projet
                        sceneParent.Layers.Add(layer);

                        layer.Name = sceneParent.projectParent.IncrementObjectName(layer.Name);


                        string oldMapName = "";
                        if (layer.TilesMap != null)
                        {
                            oldMapName = layer.TilesMap.TilesMapName;
                            layer.TilesMap.TilesMapName = sceneParent.projectParent.IncrementObjectName(layer.TilesMap.TilesMapName);
                        }

                        for (int i = 0; i < layer.CoronaObjects.Count; i++)
                        {
                            CoronaObject obj = layer.CoronaObjects[i];
                            if (obj.isEntity == false)
                            {
                                obj.DisplayObject.Name = sceneParent.projectParent.IncrementObjectName(obj.DisplayObject.Name);
                            }
                            else
                            {
                                obj.Entity.Name = sceneParent.projectParent.IncrementObjectName(obj.Entity.Name);
                                for (int j = 0; j < obj.Entity.CoronaObjects.Count; j++)
                                {
                                    obj.Entity.CoronaObjects[j].DisplayObject.Name = sceneParent.projectParent.IncrementObjectName(obj.Entity.CoronaObjects[j].DisplayObject.Name);
                                }
                            }
                        }

                        //Traiter les fichiers necessaires aux tilesmap
                        worker.ReportProgress(60);
                        int prog = 60;


                        if (Directory.Exists(directoryName + "\\Images"))
                        {
                            if (!Directory.Exists(sceneParent.projectParent.ProjectPath + "\\Resources\\Images"))
                                Directory.CreateDirectory(sceneParent.projectParent.ProjectPath + "\\Resources\\Images");

                            string[] files = Directory.GetFiles(directoryName + "\\Images");
                            for (int i = 0; i < files.Length; i++)
                            {
                                string fileNameOnly = Path.GetFileName(files[i]);
                                File.Copy(files[i], sceneParent.projectParent.ProjectPath + "\\Resources\\Images\\" + fileNameOnly, true);
                            }
                        }

                        if (Directory.Exists(directoryName + "\\SpriteSheets"))
                        {
                            if (!Directory.Exists(sceneParent.projectParent.ProjectPath + "\\Resources\\SpriteSheets"))
                                Directory.CreateDirectory(sceneParent.projectParent.ProjectPath + "\\Resources\\SpriteSheets");

                            string[] directories = Directory.GetDirectories(directoryName + "\\SpriteSheets");
                            for (int i = 0; i < directories.Length; i++)
                            {
                                string directoryNameOnly = Path.GetFileName(directories[i]);

                                if (!Directory.Exists(sceneParent.projectParent.ProjectPath + "\\Resources\\SpriteSheets\\" + directoryNameOnly))
                                    Directory.CreateDirectory(sceneParent.projectParent.ProjectPath + "\\Resources\\SpriteSheets\\" + directoryNameOnly);

                                string[] files = Directory.GetFiles(directories[i]);
                                for (int j = 0; j < files.Length; j++)
                                {
                                    string fileNameOnly = Path.GetFileName(files[j]);
                                    File.Copy(files[j], sceneParent.projectParent.ProjectPath + "\\Resources\\SpriteSheets\\" +
                                        directoryNameOnly + "\\" + fileNameOnly, true);
                                }
                            }

                        }

                        if (Directory.Exists(directoryName + "\\TileMaps") && layer.TilesMap!=null)
                        {
                            if (!Directory.Exists(sceneParent.projectParent.ProjectPath + "\\Resources\\TileMaps\\"+layer.TilesMap.TilesMapName))
                                Directory.CreateDirectory(sceneParent.projectParent.ProjectPath + "\\Resources\\TileMaps\\"+layer.TilesMap.TilesMapName);

                            string[] directories = Directory.GetDirectories(directoryName + "\\TileMaps");
                            for (int i = 0; i < directories.Length; i++)
                            {
                                string directoryNameOnly = Path.GetFileName(directories[i]);

                                string[] files = Directory.GetFiles(directories[i]);
                                for (int j = 0; j < files.Length; j++)
                                {
                                    string fileNameOnly = Path.GetFileName(files[j]);
                                    File.Copy(files[j], sceneParent.projectParent.ProjectPath + "\\Resources\\TileMaps\\" +
                                        layer.TilesMap.TilesMapName + "\\" + fileNameOnly, true);
                                }
                            }

                            //Copy all JSON Config files of the tile map
                            string[] jsonFiles = Directory.GetFiles(directoryName,"*.json");
                            for (int j = 0; j < jsonFiles.Length; j++)
                            {
                                string fileNameOnly = Path.GetFileName(jsonFiles[j]);
                                string newFileName = sceneParent.projectParent.SourceFolderPath + "\\" + fileNameOnly.Replace(oldMapName,layer.TilesMap.TilesMapName);
                                File.Copy(jsonFiles[j], newFileName, true);
                            }
                        }

                        if (layer.TilesMap != null)
                        {
                            worker.ReportProgress(prog + 5);
                           
                            layer.TilesMap.reloadMapContent(sceneParent.projectParent.SourceFolderPath);

                        }

                        for (int i = 0; i < layer.CoronaObjects.Count; i++)
                        {
                            CoronaObject obj = layer.CoronaObjects[i];
                            mainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj, 1, System.Drawing.Point.Empty);
                        }

                        for (int i = 0; i < layer.Controls.Count; i++)
                        {
                            Corona_Classes.Controls.CoronaControl obj = layer.Controls[i];
                            mainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj, 1, System.Drawing.Point.Empty);
                        }

                        mainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();
                        worker.ReportProgress(90);
                       
                    }
                    else
                    {
                        MessageBox.Show("Error during layer importation!\n The file seems to be corrupted!", "Layer Importation Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        mainForm.currentWorkerAction = "NONE";
                    }

                }
            }

        }

        public static void exportLayer(Form1 mainForm, CoronaLayer layer, string directoryName, BackgroundWorker worker)
        {
            worker.ReportProgress(20);

            string layerSerializedDirectory = directoryName+"\\"+layer.Name;

            if (Directory.Exists(layerSerializedDirectory))
                Directory.Delete(layerSerializedDirectory, true);

             Directory.CreateDirectory(layerSerializedDirectory);

            FileStream fs = File.Create(layerSerializedDirectory + "\\" + layer.Name + ".krl");
            MemoryStream ms = SerializerHelper.SerializeBinary(layer);

            worker.ReportProgress(40);
            fs.Write(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Flush();
            fs.Close();
            worker.ReportProgress(60);

            if (layer.TilesMap != null)
            {
                string mapResourcesDirectory = mainForm.CurrentProject.ProjectPath + "\\Resources\\TileMaps\\" + layer.TilesMap.TilesMapName;
                if (Directory.Exists(mapResourcesDirectory))
                {
                    Directory.CreateDirectory(layerSerializedDirectory+"\\TileMaps\\"+ layer.TilesMap.TilesMapName);

                    string[] files = Directory.GetFiles(mapResourcesDirectory);
                    for (int i = 0; i < files.Length; i++)
                    {
                        string fileNameOnly = Path.GetFileName(files[i]);
                        File.Copy(files[i], layerSerializedDirectory + "\\TileMaps\\" + layer.TilesMap.TilesMapName + "\\" + fileNameOnly);
                    }
                }

                layer.TilesMap.createJSONConfigFile(layer.SceneParent, mainForm.CurrentProject.SourceFolderPath, 1, 1);

                string path1 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "textures.json";
                string path2 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objects.json";
                string path3 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "collisions.json";
                string path4 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "texturesequences.json";
                string path5 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objectsequences.json";
                string path6 = mainForm.CurrentProject.SourceFolderPath + "\\" + layer.TilesMap.TilesMapName.ToLower() + "events.json";

                if (File.Exists(path1))
                    File.Copy(path1, layerSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "textures.json", true);

                if (File.Exists(path2))
                    File.Copy(path2, layerSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objects.json", true);

                if (File.Exists(path3))
                    File.Copy(path3, layerSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "collisions.json", true);

                if (File.Exists(path4))
                    File.Copy(path4, layerSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "texturesequences.json", true);

                if (File.Exists(path5))
                    File.Copy(path5, layerSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "objectsequences.json", true);

                if (File.Exists(path6))
                    File.Copy(path6, layerSerializedDirectory + "\\" + layer.TilesMap.TilesMapName.ToLower() + "events.json", true);


                
            }

            for (int i = 0; i < layer.CoronaObjects.Count; i++)
            {
                CoronaObject obj = layer.CoronaObjects[i];
                if (obj.isEntity == true)
                {
                    CoronaEntity entity = obj.Entity;
                    for (int j = 0; j < entity.CoronaObjects.Count; j++)
                    {
                        CoronaObject child = entity.CoronaObjects[j];
                        if (child.DisplayObject != null)
                        {
                            if (child.DisplayObject.Type.Equals("IMAGE"))
                            {

                                string filename = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\Images", child.DisplayObject.OriginalAssetName + ".png");
                                if (File.Exists(filename))
                                {
                                    if (!Directory.Exists(layerSerializedDirectory + "\\Images"))
                                        Directory.CreateDirectory(layerSerializedDirectory + "\\Images");

                                    File.Copy(filename, layerSerializedDirectory + "\\Images\\" + child.DisplayObject.OriginalAssetName + ".png", true);
                                }



                            }
                            else if (child.DisplayObject.Type.Equals("SPRITE"))
                            {
                                CoronaSpriteSet set = child.DisplayObject.SpriteSet;
                                if (set != null)
                                {
                                    List<CoronaSpriteSheet> spriteSheetsUsed = new List<CoronaSpriteSheet>();
                                    for (int k = 0; k < set.Frames.Count; k++)
                                    {
                                        if (!spriteSheetsUsed.Contains(set.Frames[k].SpriteSheetParent))
                                            spriteSheetsUsed.Add(set.Frames[k].SpriteSheetParent);

                                    }

                                    for (int k = 0; k < spriteSheetsUsed.Count; k++)
                                    {
                                        CoronaSpriteSheet sheet = spriteSheetsUsed[k];
                                        string sheetDirectory = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                                        if (!Directory.Exists(layerSerializedDirectory + "\\SpriteSheets\\" + sheet.Name))
                                        {
                                            Directory.CreateDirectory(layerSerializedDirectory + "\\SpriteSheets\\" + sheet.Name);
                                        }

                                        string[] files = Directory.GetFiles(sheetDirectory);
                                        for (int l = 0; l < files.Length; l++)
                                        {
                                            string fileNameOnly = Path.GetFileName(files[l]);
                                            File.Copy(files[l], layerSerializedDirectory + "\\SpriteSheets\\" + sheet.Name + "\\" + fileNameOnly, true);
                                        }



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
                        if (obj.DisplayObject.Type.Equals("IMAGE"))
                        {

                            string filename = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\Images", obj.DisplayObject.OriginalAssetName + ".png");
                            if (File.Exists(filename))
                            {
                                if (!Directory.Exists(layerSerializedDirectory + "\\Images"))
                                    Directory.CreateDirectory(layerSerializedDirectory + "\\Images");

                                File.Copy(filename, layerSerializedDirectory + "\\Images\\" + obj.DisplayObject.OriginalAssetName + ".png", true);
                            }



                        }
                        else if (obj.DisplayObject.Type.Equals("SPRITE"))
                        {
                            CoronaSpriteSet set = obj.DisplayObject.SpriteSet;
                            if (set != null)
                            {
                                List<CoronaSpriteSheet> spriteSheetsUsed = new List<CoronaSpriteSheet>();
                                for (int j = 0; j < set.Frames.Count; j++)
                                {
                                    if (!spriteSheetsUsed.Contains(set.Frames[j].SpriteSheetParent))
                                        spriteSheetsUsed.Add(set.Frames[j].SpriteSheetParent);

                                }

                                for (int j = 0; j < spriteSheetsUsed.Count; j++)
                                {
                                    CoronaSpriteSheet sheet = spriteSheetsUsed[j];
                                    string sheetDirectory = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                                    if (!Directory.Exists(layerSerializedDirectory + "\\SpriteSheets\\" + sheet.Name))
                                    {
                                        Directory.CreateDirectory(layerSerializedDirectory + "\\SpriteSheets\\" + sheet.Name);
                                    }

                                    string[] files = Directory.GetFiles(sheetDirectory);
                                    for (int k = 0; k < files.Length; k++)
                                    {
                                        string fileNameOnly = Path.GetFileName(files[k]);
                                        File.Copy(files[k], layerSerializedDirectory + "\\SpriteSheets\\" + sheet.Name + "\\" + fileNameOnly, true);
                                    }



                                }
                            }
                        }
                    }
                }
            }


            for (int i = 0; i < layer.Controls.Count; i++)
            {
                Corona_Classes.Controls.CoronaControl control = layer.Controls[i];
                if (control.type == Corona_Classes.Controls.CoronaControl.ControlType.joystick)
                {
                    Corona_Classes.Controls.JoystickControl joy = control as Corona_Classes.Controls.JoystickControl;

                    string innerImage = mainForm.CurrentProject.ProjectPath + "\\Resources\\Images\\" + joy.joystickName + "_inner.png";
                    if (File.Exists(innerImage))
                        File.Copy(innerImage, layerSerializedDirectory + "\\Images\\" + joy.joystickName + "_inner.png");

                    string outerImage = mainForm.CurrentProject.ProjectPath + "\\Resources\\Images\\" + joy.joystickName + "_outer.png";
                    if (File.Exists(outerImage))
                        File.Copy(outerImage, layerSerializedDirectory + "\\Images\\" + joy.joystickName + "_outer.png");


                }
            }

            worker.ReportProgress(80);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------- ENTITY ----------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static void importEntity(Form1 mainForm, string filename, BackgroundWorker worker)
        {
            CoronaLayer layerParent = mainForm.getElementTreeView().LayerSelected;
            string projectPath = layerParent.SceneParent.projectParent.ProjectPath;
            if (layerParent != null)
            {
                worker.ReportProgress(10);
                FileStream fs = File.OpenRead(filename);
                if (fs.Length > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    ms.SetLength(fs.Length);

                    fs.Read(ms.GetBuffer(), 0, (int)ms.Length);
                    worker.ReportProgress(30);
                    CoronaObject entity = (CoronaObject)SerializerHelper.DeSerializeBinary(ms);
                    fs.Close();
                    ms.Close();
                    worker.ReportProgress(50);
                    if (entity != null)
                    {
                        entity.LayerParent = layerParent;
                        entity.Entity.Name = layerParent.SceneParent.projectParent.IncrementObjectName(entity.Entity.Name);
                        //Ajouter l'entité ay layer
                        layerParent.CoronaObjects.Add(entity);

                        string currentDir = Path.GetDirectoryName(filename);
                        if (Directory.Exists(currentDir + "\\Images"))
                        {
                            string[] files = Directory.GetFiles(currentDir + "\\Images");
                            for (int i = 0; i < files.Length; i++)
                            {
                                string fileNameOnly = Path.GetFileName(files[i]);
                                File.Copy(files[i],
                                     projectPath + "\\Resources\\Images\\" + fileNameOnly, true);
                            }
                        }

                        if (Directory.Exists(currentDir + "\\SpriteSheets"))
                        {
                            string[] sheetDirectories = Directory.GetDirectories(currentDir + "\\SpriteSheets");
                            for (int i = 0; i < sheetDirectories.Length; i++)
                            {
                                string finalDirectoryDestNameOnly = Path.GetFileName(sheetDirectories[i]);
                                if (!Directory.Exists(projectPath + "\\Resources\\SpriteSheets\\" + finalDirectoryDestNameOnly))
                                {
                                    Directory.CreateDirectory(projectPath + "\\Resources\\SpriteSheets\\" + finalDirectoryDestNameOnly);
                                }

                                string[] files = Directory.GetFiles(sheetDirectories[i]);
                                for (int j = 0; j < files.Length; j++)
                                {
                                    string fileNameOnly = Path.GetFileName(files[j]);
                                    File.Copy(files[j],
                                         projectPath + "\\Resources\\SpriteSheets\\" + finalDirectoryDestNameOnly + "\\" + fileNameOnly, true);
                                }

                            }
                        }

                        for (int i = 0; i < entity.Entity.CoronaObjects.Count; i++)
                        {
                            entity.Entity.CoronaObjects[i].LayerParent = layerParent;
                            entity.Entity.CoronaObjects[i].objectAttachedToGenerator = null;
                            entity.Entity.CoronaObjects[i].DisplayObject.Name = layerParent.SceneParent.projectParent.IncrementObjectName(entity.Entity.CoronaObjects[i].DisplayObject.Name);

                            mainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(entity.Entity.CoronaObjects[i], 1, System.Drawing.Point.Empty);
                        }

                        mainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();


                        //Traiter les fichiers necessaires aux tilesmap
                        worker.ReportProgress(60);

                        worker.ReportProgress(90);

                    }
                    else
                    {
                        MessageBox.Show("Error during entity importation!\n The file seems to be corrupted!", "Entity Importation Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        mainForm.currentWorkerAction = "NONE";
                    }

                }
            }

        }

        public static void exportEntity(Form1 mainForm, CoronaObject entity, string directoryName, BackgroundWorker worker)
        {
            worker.ReportProgress(20);
            string entitySerialzedDirectory = directoryName + "\\" + entity.Entity.Name;

            if (Directory.Exists(entitySerialzedDirectory))
                Directory.Delete(entitySerialzedDirectory, true);

            Directory.CreateDirectory(entitySerialzedDirectory);

            FileStream fs = File.Create(directoryName + "\\" + entity.Entity.Name + ".kre");
            MemoryStream ms = SerializerHelper.SerializeBinary(entity);

            worker.ReportProgress(40);
            fs.Write(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Flush();
            fs.Close();

            CoronaEntity entityObject = entity.Entity;
            for (int k = 0; k < entityObject.CoronaObjects.Count; k++)
            {
                CoronaObject child = entityObject.CoronaObjects[k];
                if (child.DisplayObject != null)
                {
                    if (child.DisplayObject.Type.Equals("IMAGE"))
                    {

                        string filename = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\Images", child.DisplayObject.OriginalAssetName + ".png");
                        if (File.Exists(filename))
                        {
                            if (!Directory.Exists(entitySerialzedDirectory + "\\Images"))
                                Directory.CreateDirectory(entitySerialzedDirectory + "\\Images");

                            File.Copy(filename, entitySerialzedDirectory + "\\Images\\" + child.DisplayObject.OriginalAssetName + ".png", true);
                        }



                    }
                    else if (child.DisplayObject.Type.Equals("SPRITE"))
                    {
                        CoronaSpriteSet set = child.DisplayObject.SpriteSet;
                        if (set != null)
                        {
                            List<CoronaSpriteSheet> spriteSheetsUsed = new List<CoronaSpriteSheet>();
                            for (int l = 0; l < set.Frames.Count; l++)
                            {
                                if (!spriteSheetsUsed.Contains(set.Frames[l].SpriteSheetParent))
                                    spriteSheetsUsed.Add(set.Frames[l].SpriteSheetParent);

                            }

                            for (int l = 0; l < spriteSheetsUsed.Count; l++)
                            {
                                CoronaSpriteSheet sheet = spriteSheetsUsed[l];
                                string sheetDirectory = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                                if (!Directory.Exists(entitySerialzedDirectory + "\\SpriteSheets\\" + sheet.Name))
                                {
                                    Directory.CreateDirectory(entitySerialzedDirectory + "\\SpriteSheets\\" + sheet.Name);
                                }

                                string[] files = Directory.GetFiles(sheetDirectory);
                                for (int m = 0; m < files.Length; m++)
                                {
                                    string fileNameOnly = Path.GetFileName(files[m]);
                                    File.Copy(files[m], entitySerialzedDirectory + "\\SpriteSheets\\" + sheet.Name + "\\" + fileNameOnly, true);
                                }



                            }
                        }
                    }
                }
            }

            worker.ReportProgress(80);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------- OBJECT ----------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static void importObject(Form1 mainForm, string filename, BackgroundWorker worker)
        {
            CoronaLayer layerParent = mainForm.getElementTreeView().LayerSelected;
            if (layerParent != null)
            {
                string projectPath = layerParent.SceneParent.projectParent.ProjectPath;
                worker.ReportProgress(10);
                FileStream fs = File.OpenRead(filename);
                if (fs.Length > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    ms.SetLength(fs.Length);

                    fs.Read(ms.GetBuffer(), 0, (int)ms.Length);
                    worker.ReportProgress(30);
                    CoronaObject obj = (CoronaObject)SerializerHelper.DeSerializeBinary(ms);
                    fs.Close();
                    ms.Close();
                    worker.ReportProgress(50);
                    if (obj != null)
                    {
                        obj.LayerParent = layerParent;
                        obj.DisplayObject.Name = layerParent.SceneParent.projectParent.IncrementObjectName(obj.DisplayObject.Name);
                        //Ajouter l'entité ay layer
                        layerParent.CoronaObjects.Add(obj);

                        string currentDir = Path.GetDirectoryName(filename);
                        if (Directory.Exists(currentDir + "\\Images"))
                        {
                            string[] files = Directory.GetFiles(currentDir + "\\Images");
                            for (int i = 0; i < files.Length; i++)
                            {
                                string fileNameOnly = Path.GetFileName(files[i]);
                                File.Copy(files[i],
                                     projectPath + "\\Resources\\Images\\" + fileNameOnly, true);
                            }
                        }

                        if (Directory.Exists(currentDir + "\\SpriteSheets"))
                        {
                            string[] sheetDirectories = Directory.GetDirectories(currentDir + "\\SpriteSheets");
                            for (int i = 0; i < sheetDirectories.Length; i++)
                            {
                                string finalDirectoryDestNameOnly = Path.GetFileName(sheetDirectories[i]);
                                if (!Directory.Exists(projectPath + "\\Resources\\SpriteSheets\\" + finalDirectoryDestNameOnly))
                                {
                                    Directory.CreateDirectory(projectPath + "\\Resources\\SpriteSheets\\" + finalDirectoryDestNameOnly);
                                }

                                string[] files = Directory.GetFiles(sheetDirectories[i]);
                                for (int j = 0; j < files.Length; j++)
                                {
                                    string fileNameOnly = Path.GetFileName(files[j]);
                                    File.Copy(files[j],
                                         projectPath + "\\Resources\\SpriteSheets\\" + finalDirectoryDestNameOnly + "\\" + fileNameOnly, true);
                                }
                              
                            }
                        }

                      
                        mainForm.sceneEditorView1.GraphicsContentManager.UpdateSpriteContent(obj, 1, System.Drawing.Point.Empty);
                        worker.ReportProgress(90);
                        mainForm.sceneEditorView1.GraphicsContentManager.CleanProjectBitmaps();

                        obj.objectAttachedToGenerator = null;

                    }
                    else
                    {
                        MessageBox.Show("Error during object importation!\n The file seems to be corrupted!", "Object Importation Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        mainForm.currentWorkerAction = "NONE";
                    }

                }
            }

        }

        public static void exportObject(Form1 mainForm, CoronaObject obj, string directoryName, BackgroundWorker worker)
        {
            worker.ReportProgress(20);
           
            //Create FIrst directory
            string finalDirectory = directoryName + "\\" + obj.DisplayObject.Name;
            if (Directory.Exists(finalDirectory))
                Directory.Delete(finalDirectory, true);

            Directory.CreateDirectory(finalDirectory);

            if (obj.DisplayObject != null)
            {
                if (obj.DisplayObject.Type.Equals("IMAGE"))
                {
                   
                    string filename = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\Images", obj.DisplayObject.OriginalAssetName + ".png");
                    if (File.Exists(filename))
                    {
                        if (!Directory.Exists(finalDirectory + "\\Images"))
                            Directory.CreateDirectory(finalDirectory + "\\Images");

                        File.Copy(filename, finalDirectory + "\\Images\\" + obj.DisplayObject.OriginalAssetName + ".png", true);
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
                            string sheetDirectory = Path.Combine(mainForm.CurrentProject.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                            if (!Directory.Exists(finalDirectory + "\\SpriteSheets\\" + sheet.Name))
                            {
                                Directory.CreateDirectory(finalDirectory + "\\SpriteSheets\\" + sheet.Name);
                            }

                            string[] files = Directory.GetFiles(sheetDirectory);
                            for(int j = 0;j<files.Length;j++)
                            {
                                string fileNameOnly = Path.GetFileName(files[j]);
                                File.Copy(files[j], finalDirectory + "\\SpriteSheets\\" + sheet.Name + "\\" + fileNameOnly, true);
                            }

                            

                        }
                    }
                }
            }

            FileStream fs = File.Create(finalDirectory + "\\" + obj.DisplayObject.Name + ".kro");

           
            MemoryStream ms = SerializerHelper.SerializeBinary(obj);

            worker.ReportProgress(40);
            fs.Write(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Flush();
            fs.Close();

            worker.ReportProgress(80);
        }
    
    }
}
