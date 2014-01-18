using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Krea.GameEditor;
using System.ComponentModel;
using Krea.Corona_Classes.Controls;
using Krea.Asset_Manager;
using System.Drawing;
using Krea.CGE_Figures;
using System.Drawing.Text;
using Krea.Corona_Classes;
using System.Net;
using Krea.GameEditor.FontManager;

namespace Krea.CoronaClasses
{
    public class CoronaGameBuilder
    {

        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        private String PATH_TEMPLATES_DIR = Path.GetDirectoryName(Application.ExecutablePath)+ "\\Lua Template";
        private String PATH_REPOSITORY_DIR = Path.GetDirectoryName(Application.ExecutablePath) + "\\Lua Repository";

        private CoronaGameProject project;

        //---------------------------------------------------
        //-------------------Constructeur------------------------
        //---------------------------------------------------
        public CoronaGameBuilder(CoronaGameProject project)
        {
            this.project = project;
        }


        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public List<string> getLuaFilesUsed(bool isDebug)
        {
            if (this.project != null)
            {
                bool fpsAnalyser = false;
                bool fStarPathFinding = false;
                bool generator = false;
                bool joystick = false;
                bool mobileEditorEngine = false ;
                bool pathFollow = false;
                bool rosetta = true;
                bool tilesmap = false;
                bool entity = false;


                List<string> fileNames = new List<string>();

                for (int i = 0; i < this.project.Scenes.Count; i++)
                {
                    Scene scene = this.project.Scenes[i];
                    if (scene.isEnabled == true)
                    {
                        if (scene.Name.Equals("mapeditormobile"))
                            mobileEditorEngine = true;

                        if (scene.IsFPSVisible == true)
                            fpsAnalyser = true;

                        for (int j = 0; j < scene.Layers.Count; j++)
                        {
                            CoronaLayer layer = scene.Layers[j];
                            if(layer.isEnabled == true)
                            {
                                if (layer.TilesMap != null)
                                {
                                    if (layer.TilesMap.isEnabled == true)
                                    {
                                        tilesmap = true;

                                        if (layer.TilesMap.IsPathFindingEnabled == true)
                                        {
                                            fStarPathFinding = true;
                                        }
                                    }
                                }
                            }

                            for (int k = 0; k < layer.Controls.Count; k++)
                            {
                                CoronaControl control = layer.Controls[k];
                                if (control.isEnabled == true)
                                {
                                    if (control.Type == CoronaControl.ControlType.joystick)
                                    {
                                        joystick = true;
                                        break;
                                    }
                                }
                            }

                            for (int k = 0; k < layer.CoronaObjects.Count; k++)
                            {
                                CoronaObject obj = layer.CoronaObjects[k];
                                if (obj.isEnabled == true)
                                {
                                    if (obj.isEntity == false)
                                    {
                                        if (obj.PathFollow.isEnabled == true)
                                        {
                                            pathFollow = true;
                                        }

                                        if (obj.isGenerator == true)
                                        {
                                            generator = true;
                                        }
                                    }
                                    else
                                    {
                                        entity = true;

                                        if (obj.isGenerator == true)
                                        {
                                            generator = true;
                                        }

                                        for (int l = 0; l < obj.Entity.CoronaObjects.Count; l++)
                                        {
                                            CoronaObject child = obj.Entity.CoronaObjects[l];
                                            if (child.isEnabled == true)
                                            {
                                                if (child.PathFollow.isEnabled == true)
                                                {
                                                    pathFollow = true;
                                                }

                                                if (child.isGenerator == true)
                                                {
                                                    generator = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                fileNames.Add("object.lua");
                fileNames.Add("camera.lua");
                fileNames.Add("soundengine.lua");
                if (fpsAnalyser == true)
                {
                    fileNames.Add("graph.lua");
                    fileNames.Add("perfanalyser.lua");
                }

                if (fStarPathFinding == true)
                    fileNames.Add("fstar.lua");
                if (generator == true)
                    fileNames.Add("generator.lua");
                if (joystick == true)
                    fileNames.Add("joystick.lua");
                if (mobileEditorEngine == true)
                    fileNames.Add("mobileeditorengine.lua");
                if (pathFollow == true)
                    fileNames.Add("pathfollow.lua");
                if (rosetta == true)
                    fileNames.Add("rosetta.lua");
                if (tilesmap == true)
                    fileNames.Add("tilesmap.lua");
                if (entity == true)
                    fileNames.Add("entity.lua");

                if (Settings1.Default.RemoteControlEnabled == true && isDebug == false)
                {
                    fileNames.Add("Krea Remote\\remotecontroler.lua");
                   
                }

                //if (isDebug == true)
                //{
                //    fileNames.Add("mobdebug.lua");
                //}


               
                return fileNames;
            }
            return null;
        }

        public void buildToLua(BackgroundWorker worker, bool isCustomBuild, float XRatio, float YRatio,bool isDebug)
        {
            if (this.project != null)
            {
                try
                {
                    float moyenneRatio = (XRatio + YRatio) / 2;

                    //Report progress
                    worker.ReportProgress(10);

                    if (Directory.Exists(this.project.BuildFolderPath))
                        Directory.Delete(this.project.BuildFolderPath,true);

                    Directory.CreateDirectory(this.project.BuildFolderPath);

                    //Report progress
                    worker.ReportProgress(20);

                    CreateConfigLua(isCustomBuild,XRatio,YRatio);

                    CreateBuildSettings();

                    //Report progress
                    worker.ReportProgress(30);

                    createMain(isDebug);

                    //Report progress
                    worker.ReportProgress(40);

                    ImageResizer ImageResizer = new ImageResizer(this.project);
                    if (!ImageResizer.GenerateIconFile()) MessageBox.Show("can't create Icon!");

                    if (isCustomBuild == false)
                    {
                        if (!ImageResizer.GenerateImageFile()) MessageBox.Show("can't create Images!");
                    }
                    else
                    {
                        if (!ImageResizer.generateImageFilesWithRatio(XRatio,YRatio)) MessageBox.Show("can't create Images!");
                    }
                   
                    //Report progress
                    worker.ReportProgress(50);

                    CreateLuaLangue(this.project);

                    //Report progress
                    worker.ReportProgress(60);

                    

                    //Importer les fichiers necessaires
                    List<string> filenames = this.getLuaFilesUsed(isDebug);
                    if (filenames != null)
                    {
                        for (int i = 0; i < filenames.Count; i++)
                        {
                            File.Copy(this.PATH_REPOSITORY_DIR + "\\" + filenames[i], this.project.BuildFolderPath + "\\" + filenames[i].Replace("Krea Remote\\",""), true);
                        }
                    }

                    for (int i = 0; i < project.Scenes.Count; i++)
                    {
                        Scene scene = project.Scenes[i];
                        if (scene.isEnabled == true)
                        {
                            for (int j = 0; j < scene.Layers.Count; j++)
                            {
                                CoronaLayer layer = scene.Layers[j];
                                if (layer.isEnabled == true)
                                {
                                    for (int k = 0; k < layer.CoronaObjects.Count; k++)
                                    {
                                        CoronaObject objectToBuild = layer.CoronaObjects[k];
                                        if (objectToBuild.isEnabled == true)
                                        {
                                            if (objectToBuild.isEntity == false)
                                            {
                                                PhysicsBody ph = objectToBuild.PhysicsBody;
                                                if (ph.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                                {
                                                    string objName = objectToBuild.DisplayObject.Name.Replace(" ", "");
                                                    //Creer le fichier lua associé
                                                    PhysicBodyLuaGenerator gen = new PhysicBodyLuaGenerator(ph);
                                                    gen.writeToLua(new DirectoryInfo(project.BuildFolderPath), XRatio, YRatio);

                                                    if (File.Exists(project.SourceFolderPath + "\\body" + objName.ToLower() + ".lua"))
                                                        File.Delete(project.SourceFolderPath + "\\body" + objName.ToLower() + ".lua");


                                                }
                                            }
                                            else
                                            {

                                                for (int l = 0; l < objectToBuild.Entity.CoronaObjects.Count; l++)
                                                {
                                                    CoronaObject child = objectToBuild.Entity.CoronaObjects[l];
                                                    if (child.isEnabled == true)
                                                    {

                                                        PhysicsBody ph = child.PhysicsBody;
                                                        if (ph.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                                        {
                                                            string objName = child.DisplayObject.Name.Replace(" ", "");
                                                            //Creer le fichier lua associé
                                                            PhysicBodyLuaGenerator gen = new PhysicBodyLuaGenerator(ph);
                                                            gen.writeToLua(new DirectoryInfo(project.BuildFolderPath), XRatio, YRatio);

                                                            if (File.Exists(project.SourceFolderPath + "\\body" + objName.ToLower() + ".lua"))
                                                                File.Delete(project.SourceFolderPath + "\\body" + objName.ToLower() + ".lua");


                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    CopyFolder(this.project.SourceFolderPath, this.project.BuildFolderPath);
                 

                    

                  
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Build Failed!\n"+ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    
            }
            else
                MessageBox.Show("Build Failed ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void createMain(bool isDebug)
        {
             string path = this.project.BuildFolderPath + "\\main.lua";

            //Copier le fichier template main.lua et l'editer
            File.Copy(this.PATH_TEMPLATES_DIR + "\\main.lua", path,true);

            StringBuilder sb = new StringBuilder();
           

            //Ouvrir le fichier et recuperer son contenu
            sb.Append(File.ReadAllText(path));

            //if (isDebug == true)
            //{

            //    sb.Insert(0, "require(\"mobdebug\").on()\n");
                

            //}

            //Se deplacer a l'index de ----BODY:START
            int indexToWrite = sb.ToString().IndexOf("----BODY:START") + "----BODY:START".Length;


            String contentToWrite = "\n\n";

            //bool containsTextObject = false;
            //for (int i = 0; i < this.project.Scenes.Count; i++)
            //{
            //    if (this.project.Scenes[i].isEnabled == true)
            //    {
            //        for (int j = 0; j < this.project.Scenes[i].Layers.Count; j++)
            //        {

            //            CoronaLayer layer = this.project.Scenes[i].Layers[j];
            //            if (layer.isEnabled == true)
            //            {
            //                for (int k = 0; k < layer.CoronaObjects.Count; k++)
            //                {
            //                    if (layer.CoronaObjects[k].isEnabled == true)
            //                    {
            //                        if (layer.CoronaObjects[k].isEntity == false)
            //                        {
            //                            if (layer.CoronaObjects[k].DisplayObject.Type.Equals("FIGURE"))
            //                            {
            //                                if (layer.CoronaObjects[k].DisplayObject.Figure.ShapeType.Equals("TEXT"))
            //                                {
            //                                    containsTextObject = true;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            for (int l = 0; l < layer.CoronaObjects[k].Entity.CoronaObjects.Count; l++)
            //                            {
            //                                if (layer.CoronaObjects[k].Entity.CoronaObjects[l].DisplayObject.Type.Equals("FIGURE"))
            //                                {
            //                                    if (layer.CoronaObjects[k].Entity.CoronaObjects[l].isEnabled == true)
            //                                    {
            //                                        if (layer.CoronaObjects[k].Entity.CoronaObjects[l].DisplayObject.Figure.ShapeType.Equals("TEXT"))
            //                                        {
            //                                            containsTextObject = true;
            //                                            break;
            //                                        }
            //                                    }
            //                                }
            //                            }

            //                            if (containsTextObject == true)
            //                                break;

            //                        }
            //                    }
            //                }
            //            }
            //            if (containsTextObject == true)
            //                break;
            //        }
            //    }
            //    if (containsTextObject == true)
            //        break;
            //}

            //if (containsTextObject == true)
            //    contentToWrite += "rosetta:initiate()\n";

            //Set the default language 
            contentToWrite += "rosetta:initiate()\n";
            contentToWrite += "rosetta:setCurrentLanguage(\"" + this.project.DefaultLanguage.ToLower() +"\")\n\n";
            //Creer les sons
            contentToWrite += this.CreateAudioParamsLua(this.project.AudioObjects);

            //Copier toutes les ressources necessaires aux tilesmap du resources directory au documents directory
            for(int i = 0;i<this.project.Scenes.Count;i++)
            {
                if (this.project.Scenes[i].isEnabled == true)
                {
                    for (int j = 0; j < this.project.Scenes[i].Layers.Count; j++)
                    {
                        CoronaLayer layer = this.project.Scenes[i].Layers[j];
                        if (layer.isEnabled == true)
                        {
                            if (layer.TilesMap != null)
                            {
                                if (layer.TilesMap.isEnabled == true)
                                {
                                    string mapName = layer.TilesMap.TilesMapName.ToLower();

                                    contentToWrite += "\t local override = false\n";
                                    contentToWrite += "\t local path1 = system.pathForFile(\"" + mapName + "lastmodifiedtime.txt\", system.ResourceDirectory)\n";
                                    contentToWrite += "\t local path2 = system.pathForFile(\"" + mapName + "lastmodifiedtime.txt\",  system.DocumentsDirectory)\n";
                                    contentToWrite += "\t local result = getLastestFileTime(path1,path2)\n";
                                    contentToWrite += "\t if(result) then \n";
                                    contentToWrite += "\t\t if(result == path1) then\n";
                                    contentToWrite += "\t\t\t override = true\n";
                                    contentToWrite += "\t\t elseif(result == path2) then \n";
                                    contentToWrite += "\t\t\t override = false\n";
                                    contentToWrite += "\t\t end\n";
                                    contentToWrite += "\t else\n";
                                    contentToWrite += "\t\t override = true\n";
                                    contentToWrite += "\t end\n";

                                    contentToWrite += "\tcopyFile( \"" + mapName + "lastmodifiedtime.txt\", system.ResourceDirectory, \"" + mapName + "lastmodifiedtime.txt\", system.DocumentsDirectory,override )\n";
                                    contentToWrite += "\tcopyFile( \"" + mapName + "textures.json\", system.ResourceDirectory, \"" + mapName + "textures.json\", system.DocumentsDirectory,override )\n";
                                    contentToWrite += "\tcopyFile( \"" + mapName + "objects.json\", system.ResourceDirectory, \"" + mapName + "objects.json\", system.DocumentsDirectory,override)\n";
                                    contentToWrite += "\tcopyFile( \"" + mapName + "collisions.json\", system.ResourceDirectory, \"" + mapName + "collisions.json\", system.DocumentsDirectory,override )\n";
                                    contentToWrite += "\n";
                                }
                            }
                        }
                    }
                }
            }



            for (int i = 0; i < this.project.Scenes.Count; i++)
            {
                if (this.project.Scenes[i].isEnabled == true)
                {
                    contentToWrite += "storyboard.gotoScene(\"" + this.project.Scenes[i].Name.ToLower() + "\")\n";
                    break;
                }
              
            }
           

            if (Settings1.Default.RemoteControlEnabled == true && isDebug == false)
            {
                string ip = this.GetIpAdresse();
                if (!ip.Equals(""))
                {
                    //Copy Remote Lua File
                    contentToWrite += "local remoteParams= {ipAddress = \"" + ip + "\", port = 8200}\n";
                    contentToWrite += "local remoteConnection = function()\n\tlocal remoteControler,errorsLog = require(\"remotecontroler\").RemoteControler.create(remoteParams)\n";

                    contentToWrite += "\tfor i = 1,#errorsLog do\n";
                    contentToWrite += "\t\tprint(errorsLog[i])\n";
                    contentToWrite += "\tend\n";
                    contentToWrite += "end\n";
                    contentToWrite += "timer.performWithDelay(1000,remoteConnection,1)";

                }

            }
           
          
            sb.Insert(indexToWrite, contentToWrite);

            File.Delete(path);
            FileStream fs = File.Create(path);
            fs.Close();
            
            File.AppendAllText(path, sb.ToString());
            sb.Clear();
            sb = null;
         
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
       
        private String createAllEvents(Scene scene, String contentToWrite)
        {
             //Pour tous les layers
            for (int i = 0; i < scene.Layers.Count; i++)
            {
                for (int j = 0; j < scene.Layers[i].CoronaObjects.Count; j++)
                {
                    CoronaObject obj = scene.Layers[i].CoronaObjects[j];

                    //Creer les events associé à chaque object
                    for (int h = 0; h < obj.Events.Count; h++)
                    {
                        CoronaEvent cEvent = obj.Events[h];
                        string objName = obj.DisplayObject.Name.Replace(" ", "");


                        contentToWrite += "local " + objName + "_" + cEvent.Name +
                             " = {eventName = \"" + cEvent.Name + "\",\n" +
                              " eventType = \"" + cEvent.Type + "\",\n" +
                             " handle = " + cEvent.Handle + "\n" +
                             "}";

                        //Lancer l'event
                        contentToWrite += objName + ":createEventListener("+ objName + "_" + cEvent.Name+")";
                    }
                }
            }

            return contentToWrite;
        }

        private String createAllTimers(Scene scene, String contentToWrite)
        {
            //Pour tous les layers
            for (int i = 0; i < scene.Layers.Count; i++)
            {
                for (int j = 0; j < scene.Layers[i].CoronaObjects.Count; j++)
                {
                    CoronaObject obj = scene.Layers[i].CoronaObjects[j];

                    //Creer les timers associés à chaque object
                    for (int h = 0; h < obj.Timers.Count; h++)
                    {
                        CoronaTimer timer = obj.Timers[h];
                        string objName = obj.DisplayObject.Name;

                        //Creer la function et la rattacher a l'objet
                        contentToWrite +=  objName + "." + timer.Name + "Handle = " + timer.Handle + "\n";

                        //Lancer l'event
                        contentToWrite +=  objName + "." + timer.Name + "= timer.performWithDelay("+timer.Delay+","+
                                                                                                       objName + "." + timer.Name + "Handle,"+
                                                                                                      timer.Iteration+")\n";
                    }
                }
            }

            return contentToWrite;
        }


        public void CreateBuildSettings()
        {
            string path = this.project.BuildFolderPath + "\\build.settings";

            this.project.updateBuildFields();


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("settings  =");
            sb.AppendLine("{\n");

            ConfigField settingsField = this.project.getFieldByName(this.project.CustomBuildFields, "settings");

            int indentCount = 1;
            for (int i = 0; i < settingsField.Children.Count; i++)
            {
                ConfigField settingsChild = settingsField.Children[i];
                sb.Append(settingsChild.ToLua(indentCount));
            }

            sb.AppendLine("}");
            ////settings Section
            //String contentToWrite = "settings  = \n";
            //contentToWrite += "{\n";

            //contentToWrite += "\tiphone = \n";
            //contentToWrite += "\t{\n";
            //     contentToWrite += "\t\tcomponents = {}\n";
            //contentToWrite += "\t},\n";

            ////android SubSection
            //if (this.project.AndroidVersionCode != null)
            //{
            //    contentToWrite += "\tandroid = \n";
            //    contentToWrite += "\t{\n";

            //    contentToWrite += "\t\tVersionCode = \"" + this.project.AndroidVersionCode + "\"\n";

            //    //close android SubSection
            //    contentToWrite += "\t},\n";
            //}

            ////androidPermissions SubSection
            //if (this.project.AndroidPermissions.Count > 0)
            //{
            //    contentToWrite += "\tandroidPermissions  = " + "\n";
            //    contentToWrite += "\t{\n";

            //    for (int i = 0; i < this.project.AndroidPermissions.Count; i++)
            //    {
            //        contentToWrite += "\t\t\"" + this.project.AndroidPermissions[i] + "\"";
            //        if (i != this.project.AndroidPermissions.Count - 1)
            //        {
            //            contentToWrite += ",";
            //        }
            //        contentToWrite += "\n";
            //    }
            //    //close androidPermissions SubSection
            //    contentToWrite += "\t},\n";
            //}

            ////orientation SubSection
            //contentToWrite += "\torientation  = " + "\n";
            //contentToWrite += "\t{\n";

            //contentToWrite += "\t\tdefault = \"" + this.project.Orientation.ToString().ToLower() + "\",\n";
            //contentToWrite += "\t\tcontent = \"" + this.project.Orientation.ToString().ToLower() + "\",\n";

            ////orientation supported SubSection
            //if (this.project.SupportedOrientation.Count > 0)
            //{
            //    contentToWrite += "\t\tsupported   = " + "\n";
            //    contentToWrite += "\t\t{\n";

            //    for (int i = 0; i < this.project.SupportedOrientation.Count; i++)
            //    {
            //        contentToWrite += "\t\t\t\"" + this.project.SupportedOrientation[i] + "\",";
            //    }
            //    contentToWrite += "\n";
            //    // close orientation supported SubSection
            //    contentToWrite += "\t\t},\n";
            //}
            ////close orientation SubSection
            //contentToWrite += "\t},\n";

            ////Builds Fonts
            //contentToWrite += this.GenerateIphoneFontsProperties(this.project.AvailableFont);

            ////build  SubSection
            //if (this.project.CustomBuildName != null)
            //{
            //    contentToWrite += "\tbuild  = \n";
            //    contentToWrite += "\t{\n";

            //    contentToWrite += "\t\tcustom  = \"" + this.project.CustomBuildName + "\",\n";

            //    //Close build  SubSection
            //    contentToWrite += "\t},\n";
            //}
            ////settings Content SubSection
            //contentToWrite += "}\n";


            FileStream fs = File.Create(path);
            fs.Close();

            File.AppendAllText(path, sb.ToString());
            sb.Clear();
            sb = null;
        }
        public void CreateConfigLua(bool isCustomBuild, float XRatio, float YRatio)
        {
            string path = this.project.BuildFolderPath + "\\config.lua";

            this.project.updateConfigFields(XRatio, YRatio);

            StringBuilder sb = new StringBuilder();

            //Application Section
            String contentToWrite = "application =\n";
            contentToWrite += "{\n";
            //Content SubSection
            contentToWrite += "\tcontent =\n";
            contentToWrite += "\t{\n";

            ////Write Parameters
            //contentToWrite += "\t\twidth = " + (this.project.width * XRatio).ToString().Replace(",", ".") + ",\n";
            //contentToWrite += "\t\theight = " + (this.project.height * YRatio).ToString().Replace(",", ".") + ",\n";
            //contentToWrite += "\t\tscale = \"" + this.project.scale + "\",\n";
            //contentToWrite += "\t\txAlign =  \"" + this.project.ScreenXAlign.ToString() + "\",\n";
            //contentToWrite += "\t\tyAlign = \"" + this.project.ScreenYAlign.ToString() + "\",\n";
            //contentToWrite += "\t\tfps = " + this.project.fps.ToString() + ",\n";
            //contentToWrite += "\t\tantialias = " + this.project.antialias.ToString().ToLower() + ",\n";
            ConfigField contentField = this.project.getFieldByName(this.project.CustomConfigFields, "content");
          
            int indentCount = 2;
            for (int i = 0; i < contentField.Children.Count; i++)
            {
                ConfigField contentChild = contentField.Children[i];
                contentToWrite += contentChild.ToLua(indentCount);
            }

            if (isCustomBuild == false)
            {
                //ImageSuffix SubSection
                if (this.project.ImageSuffix.Count > 0)
                {
                    contentToWrite += "\t\timageSuffix =\n";
                    contentToWrite += "\t\t{\n";
                    for (int i = 0; i < this.project.ImageSuffix.Count; i++)
                    {
                        contentToWrite += "\t\t\t" + this.project.ImageSuffix[i].ToString() + ",\n";
                    }
                    //Close ImageSuffix SubSection
                    contentToWrite += "\t\t},\n";
                }
            }

          

            //Close Content SubSection
            contentToWrite += "\t},\n";

            indentCount = 1;
            ConfigField applicationField = this.project.getFieldByName(this.project.CustomConfigFields, "application");
            for (int i = 0; i < applicationField.Children.Count; i++)
            {
                ConfigField applicationChild = applicationField.Children[i];

                if(!applicationChild.Name.Equals("content"))
                    contentToWrite += applicationChild.ToLua(indentCount);
            }
            //Application Content SubSection
            contentToWrite += "}\n";

            sb.Append(contentToWrite);

            FileStream fs = File.Create(path);
            fs.Close();

            File.AppendAllText(path, sb.ToString());
            sb.Clear();
            sb = null;
        }

        //FILE UTILS---------------------------
        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Directory.CreateDirectory(sourceFolder);
            }

            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest,true);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        /////////////////////////////////////////
        // Create Lua Langue
        // Generate language.settings and all <LangueName>.langue
        /////////////////////////////////////////
        public void CreateLuaLangue(CoronaGameProject project)
        {

            if (project.Langues == null) return;
            if (project.DefaultLanguage == null) return;

            string path = this.project.BuildFolderPath + "\\lang.set";

            StringBuilder sb = new StringBuilder();

            String contentToWrite = "{\n";

            if (!project.DefaultLanguage.Equals("")) contentToWrite += "\"default\":\"" + project.DefaultLanguage.ToLower() + "\",\n\n";


            if (project.Langues.Count > 0)
            {
                contentToWrite += "\"languages\":\n[\n";
                for (int i = 0; i < project.Langues.Count; i++)
                {
                    if (i == project.Langues.Count) contentToWrite += "\"" + project.Langues[i].Langue.ToLower() + "\"\n";
                    else contentToWrite += "\"" + project.Langues[i].Langue.ToLower() + "\"\n";

                    if (i < project.Langues.Count - 1) contentToWrite += ",";
                    CreateLuaLangueFile(project.Langues[i]);
                }
                contentToWrite += "]";
            }
           

            contentToWrite += "}\n";

            //Save to File
            sb.Append(contentToWrite);

            FileStream fs = File.Create(path);
            fs.Close();

            File.AppendAllText(path, sb.ToString());
            sb.Clear();
            sb = null;
        }

        private void CreateLuaLangueFile(LangueObject Langue)
        {
            if (Langue == null) return;
            string path = this.project.BuildFolderPath + "\\" + Langue.Langue.ToLower() + ".set";

            StringBuilder sb = new StringBuilder();

            //Application Section

            String contentToWrite = "";
            contentToWrite += "{\n";

            if (Langue.TranslationElement.Count > 0)
            {
                for (int i = 0; i < Langue.TranslationElement.Count; i++)
                {
                    contentToWrite += "\"" + Langue.TranslationElement[i].Key.Replace(System.Environment.NewLine, "!NL!").Replace("\r", "").Replace("\n", "!NL!").Replace("\"", "") +
                        "\":\"" + Langue.TranslationElement[i].Translation.Replace(System.Environment.NewLine, "!NL!").Replace("\r", "").Replace("\n","!NL!").Replace("\"", "") + "\"\n";
                    if (i < Langue.TranslationElement.Count - 1)
                        contentToWrite += ",";
                }
            }
            else
            {
                contentToWrite += "\"" + "N/A" + "\":\"" + "N/A" + "\"\n";
            }
           
            contentToWrite += "}\n";

            sb.Append(contentToWrite);

            FileStream fs = File.Create(path);
            fs.Close();

            File.AppendAllText(path, sb.ToString());
            sb.Clear();
            sb = null;
        }


        private String createFunctions(Scene scene,String contentToWrite)
        {

            for (int j = 0; j < scene.functions.Count; j++)
            {
                contentToWrite += "-- User Function : " + scene.functions[j].Name + "\n--\n";
                contentToWrite += scene.functions[j].Code + "\n";
            }

            return contentToWrite;
        }

        private String createVars(Scene scene,String contentToWrite)
        {

            for (int j = 0; j < scene.vars.Count; j++)
            {
                CoronaVariable cVars = scene.vars[j];

                //Construction of variable lua code
                String buf = "";
                if (cVars.isLocal) buf += "local ";
                buf += cVars.Name;
                if (!cVars.InitValue.Equals("")) buf += " = " + cVars.InitValue;

                contentToWrite += buf + "\n";
            }
            
            return contentToWrite;
        }

        public String CreateAudioParamsLua(List<AudioObject> inputList)
        {

            StringBuilder s = new StringBuilder();

            s.Append("local params = {\n");
            s.Append("StreamList = {\n");
            for (int i = 0; i < inputList.Count; i++)
            {
                if (inputList[i].type == "STREAM")
                {
                    AudioObject obj = inputList[i];
                    s.Append("{\n");
                    s.Append("Path=\"" + obj.name + "\",\n");
                    s.Append("Volume=" + obj.volume.ToString().Replace(",",".") + ",\n");
                    s.Append("LoadOnInit=" + obj.isPreloaded.ToString().ToLower() + ",\n");
                    s.Append("Loops=" + obj.loops + ",\n");
                    s.Append("Instance=nil,\n");
                    s.Append("},");
                }
            }
            s.Append("},");
            s.Append("SoundList  = {\n");
            for (int i = 0; i < inputList.Count; i++)
            {
                if (inputList[i].type == "SOUND")
                {
                    AudioObject obj = inputList[i];
                    s.Append("{\n");
                    s.Append("Path=\"" + obj.name + "\",\n");
                    s.Append("Volume=" + obj.volume.ToString().Replace(",", ".") + ",\n");
                    s.Append("LoadOnInit=" + obj.isPreloaded.ToString().ToLower() + ",\n");
                    s.Append("Loops=" + obj.loops + ",\n");
                    s.Append("Instance=nil,\n");
                    s.Append("},");
                }
            }
            s.Append("},");
            s.Append("}");

            s.Append("\n\n");

            s.Append("soundEngineInstance = require(\"soundengine\")\n");
            s.Append("audioEngine = soundEngineInstance.SoundEngine.create(params)\n");

            return s.ToString();
        }

        private string GenerateIphoneFontsProperties(List<FontItem> FontItemList)
        {

            String resultCode = "\tiphone = {\n" +
                                "\t\tplist = {\n" +
                                "\t\t\tUIAppFonts =\n" +
                                "\t\t\t{\n";
            for (int i = 0; i < FontItemList.Count; i++)
            {
                string name = FontItemList[i].NameForAndroid + ".ttf";
                resultCode += "\t\t\t\"" + name + "\",\n";
            }
            resultCode += "\t\t\t}\n" +
                          "\t\t}\n" +
                          "\t},\n";

            return resultCode;
        }


        private void copyAllFontFiles()
        {
            if (project != null)
            {
                for (int i = 0; i < project.AvailableFont.Count; i++)
                {
                    FontItem item = project.AvailableFont[i];
                    item.SanityzeFontAndMoveToProjectDirectory(item.FileName, project.BuildFolderPath);
                    
                }

            }
        }
        private List<string> getAllFontsUsed(CoronaGameProject project)
        {
            if (project != null)
            {
                List<string> list = new List<string>();
                for (int i = 0; i < project.Scenes.Count; i++)
                {
                    Scene scene = this.project.Scenes[i];
                    for (int j = 0; j < scene.Layers.Count; j++)
                    {
                        CoronaLayer layer = scene.Layers[j];
                        for (int k = 0; k < layer.CoronaObjects.Count; k++)
                        {
                            CoronaObject obj = layer.CoronaObjects[k];
                            if (obj.isEntity == false)
                            {
                                if (obj.DisplayObject.Figure != null)
                                {
                                    Figure fig = obj.DisplayObject.Figure;
                                    if (fig.ShapeType.Equals("TEXT"))
                                    {
                                        Texte txt = fig as Texte;
                                        string fontName = txt.font2.getFontNameForAndroid();
                                        if (!list.Contains(fontName))
                                        {
                                            list.Add(fontName);

                                            string fontsfolder = System.Environment.GetFolderPath(
                                                System.Environment.SpecialFolder.Fonts);

                                            FontNameGetter fontNameGetter = new FontNameGetter();
                                            string res = fontNameGetter.GetFontFileName(new DirectoryInfo(fontsfolder), txt.font2);
                                            if (File.Exists(res))
                                            {
                                                string fileDest = project.BuildFolderPath + "\\" + fontName + ".ttf";
                                                File.Copy(res, fileDest, true);
                                            }

                                        }


                                    }
                                }
                            }
                            else
                            {

                                for (int l = 0; l < obj.Entity.CoronaObjects.Count; l++)
                                {
                                    CoronaObject child = obj.Entity.CoronaObjects[l];

                                    if (child.DisplayObject.Figure != null)
                                    {
                                        Figure fig = child.DisplayObject.Figure;
                                        if (fig.ShapeType.Equals("TEXT"))
                                        {
                                            Texte txt = fig as Texte;
                                            string fontName = txt.font2.getFontNameForAndroid();
                                            if (!list.Contains(fontName))
                                            {
                                                list.Add(fontName);

                                                string fontsfolder = System.Environment.GetFolderPath(
                                                    System.Environment.SpecialFolder.Fonts);

                                                FontNameGetter fontNameGetter = new FontNameGetter();
                                                string res = fontNameGetter.GetFontFileName(new DirectoryInfo(fontsfolder), txt.font2);
                                                if (File.Exists(res))
                                                {
                                                    string fileDest = project.BuildFolderPath + "\\" + fontName + ".ttf";
                                                    File.Copy(res, fileDest, true);
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return list;
            }
            return null;
        }

        public Boolean SanityzeFontAndMoveToProjectDirectory(string PathToFile, string projectPathDirectory)
        {
            if (File.Exists(PathToFile))
            {
                if (Directory.Exists(projectPathDirectory))
                {

                    //Load FileName as Android compatibility
                    FileInfo fi = new FileInfo(PathToFile);
                    string finalFileName = fi.Name.ToLower().Replace(" ", "");

                    File.Copy(PathToFile, projectPathDirectory + "\\" + finalFileName, true);
                    return true;
                }
            }
            return false;
        }

        public void copyAudioFiles(List<AudioObject> audios)
        {
            if (audios != null)
            {
                for (int i = 0; i < audios.Count; i++)
                {
                    File.Copy(this.project.SourceFolderPath+"\\"+audios[i].path, this.project.BuildFolderPath + "\\" + audios[i].name);

                }
            }
        }
    }
}
