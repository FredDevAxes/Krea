using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using Krea.Corona_Classes;
using System.Drawing;
using Krea.Asset_Manager;
using System.Reflection;
using System.Runtime.Serialization;
using Krea.GameEditor.FontManager;
using System.Drawing.Text;


namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaGameProject
    {
        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum OrientationScreen
        {
            Landscape = 1,
            Portrait = 2
        }

        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum XScreenAlignment
        {
            center = 1,
            left = 2,
            right = 3,
        }

        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum YScreenAlignment
        {
            center = 1,
            top = 2,
            bottom = 3,
        }

        //---------------------------------------------------
        //---------------Attributs Generaux------------------
        //---------------------------------------------------

        public String ProjectName { get; set; }
        public String ProjectPath { get; set; }
        public String BuildFolderPath { get; set; }
        public String SourceFolderPath { get; set; }
        public Boolean isPhysicsEngine { get; set; }
        public Boolean isSoundEngine { get; set; }
        public Boolean isMultiTouch { get; set; }
        public Boolean isMultiScene { get; set; }
        public Boolean isMultiLangue { get; set; }
        public Boolean IncludeTilesMapEditorMobile { get; set; }
        public List<AudioObject> AudioObjects;
        public List<Snippet> Snippets;
        public List<LangueObject> Langues;
        public String DefaultLanguage;
        public List<FontItem> AvailableFont { get; set; }

        //---------------------------------------------------
        //-------------Attributs Build.settings -------------
        //---------------------------------------------------
        public OrientationScreen Orientation { get; set; }
        public List<String> SupportedOrientation { get; set; }
        public String AndroidVersionCode { get; set; }
        public List<String> AndroidPermissions { get; set; }
        public String CustomBuildName { get; set; }
        public List<ConfigField> CustomBuildFields;
        //---------------------------------------------------
        //--------------Attributs Config Lua-----------------
        //---------------------------------------------------
        public int width { get; set; }
        public int height { get; set; }
        public String scale { get; set; }

        public bool isEnabled = true;
        public List<String> ImageSuffix { get; set; }
        public int fps { get; set; }
        public Boolean antialias { get; set; }
        public Image Icone { get; set; }
        public string CgeProjectFilename{ get; set; }
        public List<ConfigField> CustomConfigFields;
        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        public List<Scene> Scenes;
        public KreaProjectVersion KreaProjectVersion;
        [OptionalFieldAttribute()]
        public XScreenAlignment ScreenXAlign = XScreenAlignment.center;
        [OptionalFieldAttribute()]
        public YScreenAlignment ScreenYAlign = YScreenAlignment.center;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------

        //Private constructor
        public  CoronaGameProject()
        {

            Scenes = new List<Scene>();
            AndroidPermissions = new List<String>();
            ImageSuffix = new List<String>();
            SupportedOrientation = new List<String>();
            AudioObjects = new List<AudioObject>();
            Langues = new List<LangueObject>();
            Snippets = new List<Snippet>();
            AvailableFont = new List<FontItem>();

            CustomBuildFields = new List<ConfigField>();
            CustomConfigFields = new List<ConfigField>();
        }

      

        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        public Boolean Init(String _ProjectName, String _ProjectPath,
 CoronaGameProject.OrientationScreen _orientation,
        int _width, int _height, String _scale, String _xAlign, String _yAlign, System.Windows.Forms.ListBox.ObjectCollection _imageSuffix, int _fps, Boolean _antialias,
        String _AndroidVersionCode, System.Windows.Forms.ListBox.ObjectCollection _SupportedOrientation, System.Windows.Forms.ListBox.ObjectCollection _AndroidPermissions, String _CustomBuildName, Image _icon)
        {
            if (_ProjectName == "" || _ProjectPath == "") return false;

            if (this.ProjectPath != null && this.ProjectName != null)
            {
                if (!(this.ProjectPath + "\\" + this.ProjectName + ".krp").Equals(this.ProjectPath + "\\" + _ProjectName.Replace(" ", "_") + ".krp"))
                {
                    if (File.Exists(this.ProjectPath + "\\" + this.ProjectName + ".krp"))
                        File.Move(this.ProjectPath + "\\" + this.ProjectName + ".krp", this.ProjectPath + "\\" + _ProjectName.Replace(" ", "_") + ".krp");

                    //Renommer le projet de ressource correspondant
                    string assetProjectsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software\\Asset Manager";
                    if (!Directory.Exists(assetProjectsDirectory))
                        Directory.CreateDirectory(assetProjectsDirectory);


                    if (File.Exists(assetProjectsDirectory + "\\" + this.ProjectName + "\\" + this.ProjectName + ".kres"))
                        File.Move(assetProjectsDirectory + "\\" + this.ProjectName + "\\" + this.ProjectName + ".kres",
                            assetProjectsDirectory+"\\" + this.ProjectName + "\\" + _ProjectName.Replace(" ", "_") + ".kres");

                    if (Directory.Exists(assetProjectsDirectory+"\\" + this.ProjectName))
                        Directory.Move(assetProjectsDirectory +"\\" + this.ProjectName, assetProjectsDirectory+"\\" + _ProjectName.Replace(" ", "_"));
                }
                
            }

            this.ProjectName = _ProjectName.Replace(" ","_");

            if (this.BuildFolderPath != null)
            {
                if(!this.BuildFolderPath.Equals( this.ProjectPath + "\\" + this.ProjectName))
                    if (Directory.Exists(this.BuildFolderPath))
                        Directory.Move(this.BuildFolderPath, this.ProjectPath + "\\" + this.ProjectName);
            }

            if (this.ProjectPath != null)
            {
                if (!this.ProjectPath.Equals(_ProjectPath))
                    if (Directory.Exists(this.ProjectPath))
                        Directory.Move(this.ProjectPath, _ProjectPath);
            }

            

            
            this.ProjectPath = _ProjectPath;
            this.BuildFolderPath = this.ProjectPath + "\\" + this.ProjectName;
            this.SourceFolderPath = this.ProjectPath + "\\Sources";
            this.CgeProjectFilename = this.ProjectPath + "\\" + this.ProjectName + ".krp";
            this.Orientation = _orientation;
            
            this.Icone = _icon;
            if (this.Icone == null) this.Icone = Properties.Resources.Icon;

            this.width = _width;
            this.height = _height;
            this.scale = _scale;

            if (_xAlign.Equals("center"))
                this.ScreenXAlign = XScreenAlignment.center;
            else if (_xAlign.Equals("left"))
                this.ScreenXAlign = XScreenAlignment.left;
            else if (_xAlign.Equals("right"))
                this.ScreenXAlign = XScreenAlignment.right;

            if (_yAlign.Equals("center"))
                this.ScreenYAlign = YScreenAlignment.center;
            else if (_yAlign.Equals("top"))
                this.ScreenYAlign = YScreenAlignment.top;
            else if (_yAlign.Equals("bottom"))
                this.ScreenYAlign = YScreenAlignment.bottom;

            if (_imageSuffix != null)
            {
                this.ImageSuffix.Clear();
                for (int i = 0; i < _imageSuffix.Count; i++)
                {
                    this.ImageSuffix.Add(_imageSuffix[i].ToString());
                }
            }

            this.fps = _fps;
            this.antialias = _antialias;
            this.AndroidVersionCode = _AndroidVersionCode;

            if (_SupportedOrientation != null)
            {
                this.SupportedOrientation.Clear();
                for (int i = 0; i < _SupportedOrientation.Count; i++)
                {
                    this.SupportedOrientation.Add(_SupportedOrientation[i].ToString());
                }
            }

            if (_AndroidPermissions != null)
            {
                this.AndroidPermissions.Clear();
                for (int i = 0; i < _AndroidPermissions.Count; i++)
                {
                    this.AndroidPermissions.Add(_AndroidPermissions[i].ToString());
                }
            }

            this.CustomBuildName = _CustomBuildName;

            
            return true;
        }



        public void createProjectFiles()
        {
            //Creer un repertoire principal du projet
            if (!Directory.Exists(this.ProjectPath))
                Directory.CreateDirectory(this.ProjectPath);


            //Creer un repertoire du projet a builder
            if (Directory.Exists(this.BuildFolderPath))
                Directory.Delete(this.BuildFolderPath,true);

             Directory.CreateDirectory(this.BuildFolderPath);

            //Creer un dossier de fichier lua source pour les scenes
             if (Directory.Exists(this.SourceFolderPath))
                 Directory.Delete(this.SourceFolderPath, true);

             Directory.CreateDirectory(this.SourceFolderPath);

            this.CgeProjectFilename = this.ProjectPath + "\\" + this.ProjectName + ".krp";
        }

        public void saveProject(String filename, BackgroundWorker worker )
        {
            
            worker.ReportProgress(20);
            //Ajouter l'extension si el ne l'es pas
            if (!filename.EndsWith(".krp"))
                filename += ".krp";

            //---Get current version
            this.KreaProjectVersion = new KreaProjectVersion(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\Version.txt");

            worker.ReportProgress(40);

            MemoryStream ms;
            try
            {
                ms = SerializerHelper.SerializeBinary(this);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error during project saving!\n" + ex.Message, "ERROR", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
           
            CgeProjectFilename = filename;

            

            try
            {

                FileStream fs = File.Create(filename.Replace(".krp","_temp.krp"));

                fs.Write(ms.GetBuffer(), 0, (int)ms.Length);
                ms.Flush();

                fs.Close();
                worker.ReportProgress(60);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error during project saving!\n" + ex.Message, "ERROR", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (File.Exists(filename))
                File.Delete(filename);

            File.Move(filename.Replace(".krp", "_temp.krp"), filename);

        }

        public void saveProject(String filename)
        {
            try
            {

                //Ajouter l'extension si el ne l'es pas
                if (!filename.EndsWith(".krp"))
                    filename += ".krp";

                if (File.Exists(filename))
                    File.Delete(filename);


                CgeProjectFilename = filename;
                FileStream fs = File.Create(filename);

                MemoryStream ms = SerializerHelper.SerializeBinary(this);
                fs.Write(ms.GetBuffer(), 0, (int)ms.Length);
                ms.Flush();

                fs.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public void clearProject()
        {
            //Relacher tous les composants graphiques
            for (int i = 0; i < this.Scenes.Count; i++)
            {
                Scene scene = this.Scenes[i];

                for (int j = 0; j < scene.Layers.Count; j++)
                {
                    CoronaLayer layer = scene.Layers[j];

                    for (int h = 0; h < layer.CoronaObjects.Count; h++)
                    {
                        CoronaObject obj = layer.CoronaObjects[h];
       
                        obj = null;
                    }

                    layer = null;
                }

                scene = null;
            }
        }


        public bool IsNameExistsInProject(string objName)
        {

            for (int i = 0; i < this.Scenes.Count; i++)
            {
                Scene scene = this.Scenes[i];
                if (scene.Name.Equals(objName))
                    return true;

                for (int j = 0; j < scene.Layers.Count; j++)
                {
                    CoronaLayer layer = scene.Layers[j];
                    if (layer.Name.Equals(objName))
                        return true;

                    if (layer.TilesMap != null)
                    {
                        if (layer.TilesMap.TilesMapName.Equals(objName))
                            return true;
                    }

                    for (int k = 0; k < layer.CoronaObjects.Count;k++)
                    {
                        CoronaObject obj = layer.CoronaObjects[k];
                        if (obj.isEntity == false)
                        {
                            if (obj.DisplayObject.Name.Equals(objName))
                                return true;
                        }
                        else
                        {
                            if (obj.Entity.Name.Equals(objName))
                                return true;

                            for (int l = 0; l < obj.Entity.CoronaObjects.Count; l++)
                            {
                                CoronaObject child = obj.Entity.CoronaObjects[l];
                                if (child.DisplayObject.Name.Equals(objName))
                                    return true;
                            }
                        }


                    }

                    for (int k = 0; k < layer.Controls.Count; k++)
                    {
                        if (layer.Controls[k].ControlName.Equals(objName))
                            return true;

                    }

                }

            }

            

            return false;
        }

        public string IncrementObjectName(string objName)
        {
            bool alreadyExists = this.IsNameExistsInProject(objName);

            if (alreadyExists == true)
            {
                int res = -1;

                if (int.TryParse(objName.Substring(objName.Length - 3), out res) == true)
                    objName = objName.Substring(0, objName.Length - 3) + (res + 1).ToString();
                else
                {
                    if (int.TryParse(objName.Substring(objName.Length - 2), out res) == true)
                        objName = objName.Substring(0, objName.Length - 2) + (res + 1).ToString();
                    else
                    {
                        if (int.TryParse(objName.Substring(objName.Length - 1), out res) == true)
                            objName = objName.Substring(0, objName.Length - 1) + (res + 1).ToString();
                        else
                            objName = objName + 1;
                    }
                }

                if (IsNameExistsInProject(objName) == true)
                    return IncrementObjectName(objName);
                else
                    return objName;

            }
            else
            {
                return objName;
            }

        }



        public void updateConfigFields(float XRatio, float YRatio)
        {
           
            if (this.CustomConfigFields == null)
                this.CustomConfigFields = new List<ConfigField>();

            ConfigField applicationField = this.getFieldByName(this.CustomConfigFields, "application");
            if (applicationField == null)
            {
                applicationField = new ConfigField("application",true,true);
                this.CustomConfigFields.Add(applicationField);
            }

            ConfigField contentField = this.getFieldByName(applicationField.Children, "content");
            if (contentField == null)
            {
                contentField = new ConfigField("content", true, true);
                applicationField.Children.Add(contentField);
            }

            //CONTENT FIELDS
            ConfigField screenWidthField = this.getFieldByName(contentField.Children, "width");
            if (screenWidthField == null)
            {
                screenWidthField = new ConfigField("width", "NUMBER", (this.width * XRatio).ToString().Replace(",", "."), true, true);
                contentField.Children.Add(screenWidthField);
            }
            else
            {
                screenWidthField.Value = (this.width * XRatio).ToString().Replace(",", ".");
                screenWidthField.IsNamedField = true;
                screenWidthField.IsAutomaticField = true;
            }

            ConfigField screenHeightField = this.getFieldByName(contentField.Children, "height");
            if (screenHeightField == null)
            {
                screenHeightField = new ConfigField("height", "NUMBER", (this.height * YRatio).ToString().Replace(",", "."), true, true);
                contentField.Children.Add(screenHeightField);
            }
            else
            {
                screenHeightField.Value = (this.height * YRatio).ToString().Replace(",", ".");
                screenHeightField.IsNamedField = true;
                screenHeightField.IsAutomaticField = true;
            }


            ConfigField scaleField = this.getFieldByName(contentField.Children, "scale");
            if (scaleField == null)
            {
                scaleField = new ConfigField("scale", "STRING", this.scale, true, true);
                contentField.Children.Add(scaleField);
            }
            else
            {
                scaleField.Value = this.scale;
                scaleField.IsNamedField = true;
                scaleField.IsAutomaticField = true;
            }

            ConfigField xAlignField = this.getFieldByName(contentField.Children, "xAlign");
            if (xAlignField == null)
            {
                xAlignField = new ConfigField("xAlign", "STRING", this.ScreenXAlign.ToString(), true, true);
                contentField.Children.Add(xAlignField);
            }
            else
            {
                xAlignField.Value = this.ScreenXAlign.ToString();
                xAlignField.IsNamedField = true;
                xAlignField.IsAutomaticField = true;
            }

            ConfigField yAlignField = this.getFieldByName(contentField.Children, "yAlign");
            if (yAlignField == null)
            {
                yAlignField = new ConfigField("yAlign", "STRING", this.ScreenYAlign.ToString(), true, true);
                contentField.Children.Add(yAlignField);
            }
            else
            {
                yAlignField.Value = this.ScreenYAlign.ToString();
                yAlignField.IsNamedField = true;
                yAlignField.IsAutomaticField = true;
            }

            ConfigField fpsField = this.getFieldByName(contentField.Children, "fps");
            if (fpsField == null)
            {
                fpsField = new ConfigField("fps", "NUMBER", this.fps.ToString(), true, true);
                contentField.Children.Add(fpsField);
            }
            else
            {
                fpsField.Value = this.fps.ToString();
                fpsField.IsNamedField = true;
                fpsField.IsAutomaticField = true;
            }

            ConfigField antialiasField = this.getFieldByName(contentField.Children, "antialias");
            if (antialiasField == null)
            {
                antialiasField = new ConfigField("antialias", "BOOLEAN", this.antialias.ToString().ToLower(), true, true);
                contentField.Children.Add(antialiasField);
            }
            else
            {
                antialiasField.Value = this.antialias.ToString().ToLower();
                antialiasField.IsNamedField = true;
                antialiasField.IsAutomaticField = true;
            }


                
        }


        public void updateBuildFields()
        {

            if (this.CustomBuildFields == null)
                this.CustomBuildFields = new List<ConfigField>();

            ConfigField settingsField = this.getFieldByName(this.CustomBuildFields, "settings");
            if (settingsField == null)
            {
                settingsField = new ConfigField("settings", true, true);
                this.CustomBuildFields.Add(settingsField);
            }

            ConfigField iPhoneField = this.getFieldByName(settingsField.Children, "iphone");
            if (iPhoneField == null)
            {
                iPhoneField = new ConfigField("iphone", true, true);
                settingsField.Children.Add(iPhoneField);
            }

            ConfigField componentsField = this.getFieldByName(iPhoneField.Children, "components");
            if (componentsField == null)
            {
                componentsField = new ConfigField("components", true, true);
                iPhoneField.Children.Add(componentsField);
            }

            ConfigField pListField = this.getFieldByName(iPhoneField.Children, "plist");
            if (pListField == null)
            {
                pListField = new ConfigField("plist", true, true);
                iPhoneField.Children.Add(pListField);
            }

            ConfigField UIAppFontsField = this.getFieldByName(this.CustomBuildFields, "UIAppFonts");
            if (UIAppFontsField == null)
            {
                UIAppFontsField = new ConfigField("UIAppFonts", true, true);
                pListField.Children.Add(UIAppFontsField);
            }

            for (int i = 0; i < this.AvailableFont.Count; i++)
            {
                string name = this.AvailableFont[i].NameForAndroid + ".ttf";
                ConfigField fontField = this.getFieldByName(UIAppFontsField.Children, name);
                if (fontField == null)
                {
                    fontField = new ConfigField(name, "STRING", name, true, false);
                    UIAppFontsField.Children.Add(fontField);
                }
                else
                {
                    fontField.Value = name;
                    fontField.IsNamedField = false;
                    fontField.IsAutomaticField = true;
                }
            }


            //--------
            ConfigField androidField = this.getFieldByName(this.CustomBuildFields, "android");
            if (androidField == null)
            {
                androidField = new ConfigField("android", true, true);
                settingsField.Children.Add(androidField);
            }

            if (this.AndroidVersionCode != null)
            {
                ConfigField VersionCodeField = this.getFieldByName(androidField.Children, "VersionCode");
                if (VersionCodeField == null)
                {
                    VersionCodeField = new ConfigField("VersionCode", "STRING", this.AndroidVersionCode, true, true);
                    androidField.Children.Add(VersionCodeField);
                }
                else
                {
                    VersionCodeField.Value = this.AndroidVersionCode;
                    VersionCodeField.IsNamedField = true;
                    VersionCodeField.IsAutomaticField = true;
                }
            }

            ConfigField androidPermissionsField = this.getFieldByName(this.CustomBuildFields, "androidPermissions");
            if (androidPermissionsField == null)
            {
                androidPermissionsField = new ConfigField("androidPermissions", true, true);
                settingsField.Children.Add(androidPermissionsField);
            }

            //androidPermissions SubSection

            for (int i = 0; i < this.AndroidPermissions.Count; i++)
            {
                string permissionStr = this.AndroidPermissions[i];
                ConfigField permField = this.getFieldByName(androidPermissionsField.Children, permissionStr);
                if (permField == null)
                {
                    permField = new ConfigField(permissionStr, "STRING", permissionStr, true, false);
                    androidPermissionsField.Children.Add(permField);
                }
                else
                {
                    permField.Value = permissionStr;
                    permField.IsNamedField = false;
                    permField.IsAutomaticField = true;
                }
            }

            
            //orientation SubSection

            ConfigField orientationField = this.getFieldByName(settingsField.Children, "orientation");
            if (orientationField == null)
            {
                orientationField = new ConfigField("orientation", true, true);
                settingsField.Children.Add(orientationField);
            }

            ConfigField defaultOrientationField = this.getFieldByName(orientationField.Children, "default");
            if (defaultOrientationField == null)
            {
                defaultOrientationField = new ConfigField("default", "STRING", this.Orientation.ToString().ToLower(), true, true);
                orientationField.Children.Add(defaultOrientationField);
            }
            else
            {
                defaultOrientationField.Value = this.Orientation.ToString().ToLower();
                defaultOrientationField.IsNamedField = true;
                defaultOrientationField.IsAutomaticField = true;
            }


            ConfigField contentOrientationField = this.getFieldByName(orientationField.Children, "content");
            if (contentOrientationField == null)
            {
                contentOrientationField = new ConfigField("content", "STRING", this.Orientation.ToString().ToLower(), true, true);
                orientationField.Children.Add(contentOrientationField);
            }
            else
            {
                contentOrientationField.Value = this.Orientation.ToString().ToLower();
                contentOrientationField.IsNamedField = true;
                contentOrientationField.IsAutomaticField = true;
            }

            if (this.SupportedOrientation.Count > 0)
            {
                ConfigField supportedOrientationField = this.getFieldByName(orientationField.Children, "supported");
                if (supportedOrientationField == null)
                {
                    supportedOrientationField = new ConfigField("supported", true, true);
                    orientationField.Children.Add(supportedOrientationField);
                }

                for (int i = 0; i < this.SupportedOrientation.Count; i++)
                {
                    string supportedOrientationStr = this.SupportedOrientation[i];
                    ConfigField supportedOrientationChild = this.getFieldByName(supportedOrientationField.Children, supportedOrientationStr);
                    if (supportedOrientationChild == null)
                    {
                        supportedOrientationChild = new ConfigField(supportedOrientationStr, "STRING", supportedOrientationStr, true, false);
                        supportedOrientationField.Children.Add(supportedOrientationChild);
                    }
                    else
                    {
                        supportedOrientationChild.Value = supportedOrientationStr;

                        supportedOrientationChild.IsNamedField = true;
                        supportedOrientationChild.IsAutomaticField = true;
                    }
                }
            }

            //build  SubSection
            if (this.CustomBuildName != null)
            {


                ConfigField buildField = this.getFieldByName(settingsField.Children, "build");
                if (buildField == null)
                {
                    buildField = new ConfigField("build", true, true);
                    settingsField.Children.Add(buildField);
                }


                ConfigField customBuildField = this.getFieldByName(buildField.Children, "custom");
                if (customBuildField == null)
                {
                    customBuildField = new ConfigField("custom", "STRING", this.CustomBuildName, true, true);
                    buildField.Children.Add(customBuildField);
                }
                else
                {
                    customBuildField.Value = this.CustomBuildName;
                    customBuildField.IsNamedField = true;
                    customBuildField.IsAutomaticField = true;
                }
             
            }

        }


        public bool doesFieldAlreadyExist(string name, List<ConfigField> fields, bool recursive)
        {
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    ConfigField child = fields[i];
                    if (child.Name.Equals(name))
                    {
                        return true;
                    }
                    else if (child.Type.Equals("TABLE") && recursive == true)
                    {

                        ConfigField childConfig = this.getFieldByName(child.Children, name);
                        if (childConfig != null)
                            return true;
                    }
                }
            }
            return false;
        }

        public ConfigField getFieldByName(List<ConfigField> fields,string name)
        {
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    ConfigField child = fields[i];
                    if (child.Name.Equals(name))
                    {
                        return child;
                    }
                    else if (child.Type.Equals("TABLE"))
                    {
                        ConfigField childConfig = this.getFieldByName(child.Children, name);
                        if (childConfig != null)
                            return childConfig;
                    }
                }
            }
            return null;
        }

       
    }
}
