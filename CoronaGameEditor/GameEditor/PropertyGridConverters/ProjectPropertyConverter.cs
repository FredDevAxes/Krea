using System;
using System.ComponentModel;
using Krea.CoronaClasses;
using System.IO;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The current project properties.")]
    class ProjectPropertyConverter
    {
        CoronaGameProject project;
        Form1 mainForm;

        [Flags]
        public enum  ScreenScale
        {
            letterbox = 1,
            zoomEven = 2,
            zoomStretch = 3,
            none = 4,
        }

        public ProjectPropertyConverter(CoronaGameProject project, Form1 mainForm)
        {
            this.project = project;
            this.mainForm = mainForm;

        }

        public CoronaGameProject GetProjectSelected()
        {
            return this.project;
        }
        [Category("GENERAL")]
        public String ProjectName
        {
            get
            {
                return this.project.ProjectName;
            }
            set
            {
                if (!this.project.ProjectName.Equals(value.Replace(" ", "_")))
                {
                    
                    System.Windows.Forms.MessageBox.Show("The Lua files need to be closed before continuing! \n You can reload them from the new project path after changes have been applied!", "Information",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    this.mainForm.cgEeditor1.closeAll(true);


                    if (this.project.ProjectPath != null && this.project.ProjectName != null)
                    {
                        if (File.Exists(this.project.ProjectPath + "\\" + this.project.ProjectName + ".krp"))
                            File.Move(this.project.ProjectPath + "\\" + this.project.ProjectName + ".krp", this.project.ProjectPath + "\\" + value.Replace(" ", "_") + ".krp");
                    }

                    if (this.project.BuildFolderPath != null)
                    {
                        if (Directory.Exists(this.BuildFolderPath))
                            Directory.Move(this.project.BuildFolderPath, this.project.ProjectPath + "\\" + value.Replace(" ", "_"));
                    }

                    if (this.project.ProjectPath != null)
                    {
                        if (Directory.Exists(this.project.ProjectPath))
                            Directory.Move(this.project.ProjectPath, this.project.ProjectPath.Replace(this.project.ProjectName, value.Replace(" ", "_")));
                    }

                    //Renommer le projet de ressource correspondant
                    //Renommer le projet de ressource correspondant
                    string assetProjectsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software\\Asset Manager";
                    if (!Directory.Exists(assetProjectsDirectory))
                        Directory.CreateDirectory(assetProjectsDirectory);


                    if (File.Exists(assetProjectsDirectory + "\\" + this.ProjectName + "\\" + this.ProjectName + ".kres"))
                        File.Move(assetProjectsDirectory + "\\" + this.ProjectName + "\\" + this.ProjectName + ".kres",
                            assetProjectsDirectory + "\\" + this.ProjectName + "\\" + value.Replace(" ", "_") + ".kres");

                    if (Directory.Exists(assetProjectsDirectory + "\\" + this.ProjectName))
                        Directory.Move(assetProjectsDirectory + "\\" + this.ProjectName, assetProjectsDirectory + "\\" + value.Replace(" ", "_"));

                    this.project.ProjectPath = this.project.ProjectPath.Replace(this.project.ProjectName, value.Replace(" ", "_"));

                    this.project.BuildFolderPath = this.project.ProjectPath + "\\" + value.Replace(" ", "_");
                    this.project.SourceFolderPath = this.project.ProjectPath + "\\Sources";

                    this.project.ProjectName = value.Replace(" ", "_");
                    this.project.CgeProjectFilename = this.project.ProjectPath + "\\" + this.project.ProjectName + ".krp";

                    this.mainForm.getElementTreeView().ProjectRootNodeSelected.Text = this.project.ProjectName;
                    this.mainForm.saveProject(false);
                }
               
            }
        }

        [Category("BUILD SETTINGS")]
        public String CustomBuildName
        {
            get
            {
                return this.project.CustomBuildName;
            }
            set
            {
                this.project.CustomBuildName = value;
            }
        }



         [Category("BUILD SETTINGS")]
        public String AndroidVersionCode
        {
            get
            {
                return this.project.AndroidVersionCode;
            }
            set
            {
                this.project.AndroidVersionCode = value;
            }
        }

        [Category("GENERAL"),ReadOnly(true)]
        public String ProjectPath
        {
            get
            {
                return this.project.ProjectPath;
            }
            set
            {
                this.project.ProjectPath = value;
            }
        }

         [Category("GENERAL"),ReadOnly(true)]
        public String BuildFolderPath
        {
            get
            {
                return this.project.BuildFolderPath;
            }
            set
            {
                this.project.BuildFolderPath = value;
            }
        }

       

        [Category("DISPLAY"),ReadOnly(false)]
        public int FPS
        {
            get
            {
                return this.project.fps;
            }
            set
            {
                this.project.fps = value;
            }
        }

        [Category("DISPLAY"), ReadOnly(false)]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool Antialias
        {
            get
            {
                return this.project.antialias;
            }
            set
            {
                this.project.antialias = value;
            }
        }

         [Category("SCREEN"),ReadOnly(false)]
         [Editor(typeof(ScreenOrientationEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public CoronaGameProject.OrientationScreen Orientation
        {
            get
            {
                return this.project.Orientation;
            }
            set
            {
                this.project.Orientation = value;
            }
        }

         [Category("SCREEN"),ReadOnly(false)]
         public ScreenScale Scale
        {
            get
            {
                if (this.project.scale.Equals("letterbox"))
                    return ScreenScale.letterbox;
                else if (this.project.scale.Equals("zoomStretch"))
                    return ScreenScale.zoomStretch;
                else if (this.project.scale.Equals("zoomEven"))
                    return ScreenScale.zoomEven;
                
                else
                    return ScreenScale.none;
            }
            set
            {
                if (value == ScreenScale.letterbox)
                    this.project.scale = "letterbox";
                else if (value == ScreenScale.zoomStretch)
                    this.project.scale = "zoomStretch";
                else if (value == ScreenScale.zoomEven)
                    this.project.scale = "zoomEven";

                else
                    this.project.scale ="none";
            }
        }

         [Category("SCREEN"), ReadOnly(false)]
         [Editor(typeof(XAlignEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Krea.CoronaClasses.CoronaGameProject.XScreenAlignment XAlign
        {
            get
            {
                return this.project.ScreenXAlign;
            }
            set
            {
                this.project.ScreenXAlign = value;
            }
        }

         [Category("SCREEN"), ReadOnly(false)]
         [Editor(typeof(YAlignEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public Krea.CoronaClasses.CoronaGameProject.YScreenAlignment YAlign
        {
            get
            {
                return this.project.ScreenYAlign;
            }
            set
            {
                this.project.ScreenYAlign = value;
            }
        }

        [Category("SCREEN"), ReadOnly(false)]
        public int ScreenWidth
        {
            get
            {
                return this.project.width;
            }
            set
            {
                this.project.width = value;
            }
        }

         [Category("SCREEN"), ReadOnly(false)]
        public int ScreenHeight
        {
            get
            {
                return this.project.height;
            }
            set
            {
                this.project.height = value;
            }
        }

         [Category("TILESMAP EDITOR MOBILE"), ReadOnly(false)]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool MobileEditorIncluded
         {
             get
             {
                 return this.project.IncludeTilesMapEditorMobile;
             }
             set
             {
                 this.project.IncludeTilesMapEditorMobile = value;
             }
         }

    }
}
