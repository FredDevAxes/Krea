using System;
using Krea.CoronaClasses;
using System.ComponentModel;
using System.Drawing;
using Krea.Corona_Classes;
using System.IO;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude=true)]
    class ScenePropertyConverter
    {
         //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private Scene sceneSelected;
        private Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public ScenePropertyConverter(Scene scene, Form1 MainForm)
        {
            this.sceneSelected = scene;
            this.MainForm = MainForm;
            
        }
        public Scene GetSceneSelected()
        {
            return this.sceneSelected;
        }
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
        [Category("GENERAL")]
        public String Name
        {
            get { return this.sceneSelected.Name; }
            set
            {
              //  this.MainForm.UndoRedo.clearBuffers();


                value = value.Replace(" ", "").ToLower().Replace("-","_");

                value = this.sceneSelected.projectParent.IncrementObjectName(value);
                try
                {
                    //Fermer le document actif
                    this.MainForm.cgEeditor1.closeFile(this.sceneSelected.projectParent.SourceFolderPath + "\\" + this.sceneSelected.Name + ".lua");
                    File.Move(this.sceneSelected.projectParent.SourceFolderPath + "\\" + this.sceneSelected.Name + ".lua", this.sceneSelected.projectParent.SourceFolderPath + "\\" + value + ".lua");
                }
                catch (Exception ex)
                {
                    
                }
                this.sceneSelected.Name = value;
                this.MainForm.cgEeditor1.OpenFileInEditor(this.sceneSelected.projectParent.SourceFolderPath + "\\" + this.sceneSelected.Name + ".lua");
                GameElement elem = this.MainForm.getElementTreeView().getNodeFromObjectInstance(this.MainForm.getElementTreeView().ProjectRootNodeSelected.Nodes, this.sceneSelected);
                if (elem != null)
                    elem.Text = value;

                this.MainForm.saveProject(false);
            }
        }


        [Category("GENERAL")]
        public Size Size
        {
            get { return this.sceneSelected.Size; }
            set 
            {

                this.sceneSelected.Size = value;
                this.MainForm.sceneEditorView1.resizeScene( this.sceneSelected.Size);
            }
        }

         [Category("MULTITOUCH")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool MultiTouchEnabled
        {
            get { return this.sceneSelected.isMultiTouchActive; }
            set
            {

                this.sceneSelected.isMultiTouchActive = value;
            }
        }


        [Category("PERFORMANCE")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool ShowPerformance
        {
            get { return this.sceneSelected.IsFPSVisible; }
            set
            {

                this.sceneSelected.IsFPSVisible = value;
            }
        }

        [Category("DISPLAY")]
        public Krea.CoronaClasses.Scene.StatusBarDisplayMode StatusBarDisplayMode
        {
            get {
                if (this.sceneSelected.StatusBarMode == null || this.sceneSelected.StatusBarMode == 0)
                    this.sceneSelected.StatusBarMode = Scene.StatusBarDisplayMode.Hidden;

                return this.sceneSelected.StatusBarMode; 
            
            }
            set
            {
                this.sceneSelected.StatusBarMode = value;
            }
        }

        [Category("DISPLAY")]
        public Color DefaultColor
        {
            get
            {
                if (sceneSelected.DefaultColor == Color.Empty)
                    sceneSelected.DefaultColor = Color.Black;

                return sceneSelected.DefaultColor;

            }
            set
            {
                this.sceneSelected.DefaultColor = value;
            }
        }

        [Category("PHYSICS")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool PhysicsEnabled
        {
            get { return this.sceneSelected.isPhysicsActive; }
            set {

                this.sceneSelected.isPhysicsActive = value; 
            }
        }

       [Category("DISPLAY")]
        public Scene.PhysicsDrawMode DrawMode
        {
            get { return this.sceneSelected.PhysDrawMode; }
            set {

                this.sceneSelected.PhysDrawMode = value;
                GorgonLibrary.Gorgon.Go();
            }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The physic gravity on X.")]
        public double XGravity
        {
            get
            {
                return this.sceneSelected.physicsXGravity;
            }
            set
            {

                this.sceneSelected.physicsXGravity = value;
            }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The physic gravity on Y.")]
        public double YGravity
        {
            get
            {
                return this.sceneSelected.physicsYGravity;
            }
            set
            {

                this.sceneSelected.physicsYGravity = value;
            }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The physic scale.")]
        public int PhysicsScale
        {
            get
            {
                return this.sceneSelected.PhysicScale;
            }
            set
            {

                this.sceneSelected.PhysicScale = value;
            }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The physic position iterations.")]
        public int PositionIterations
        {
            get
            {
                return this.sceneSelected.PhysicPositionIterations;
            }
            set
            {

                this.sceneSelected.PhysicPositionIterations = value;
            }
        }


        [Category("PHYSICS")]
        [DescriptionAttribute("The physic velocity iterations.")]
        public int VelocityIterations
        {
            get
            {
                return this.sceneSelected.PhysicVelocityIterations;
            }
            set
            {

                this.sceneSelected.PhysicVelocityIterations = value;
            }
        }


        //--------------- ADVERTISING --------------------------------------
        [Category("ADVERTISING")]
        [DescriptionAttribute("Is the advertising activated.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool AdEnabled
        {
            get
            {
                return this.sceneSelected.Ad.isActive;
            }

            set
            {

                this.sceneSelected.Ad.isActive = value;

            }
        }

       [Category("ADVERTISING")]
        [DescriptionAttribute("The banner type.")]
        public Krea.Corona_Classes.CoronaAds.BannerTypes BannerType
        {
            get
            {
            
                return this.sceneSelected.Ad.bannerType;
               
                
            }

            set
            {

               this.sceneSelected.Ad.bannerType = value;

               if (value == CoronaAds.BannerTypes.banner120x600)
                   this.sceneSelected.Ad.size = new Size(120, 600);
               else if (value == CoronaAds.BannerTypes.banner300x250)
                   this.sceneSelected.Ad.size = new Size(300, 250);
               else if(value == CoronaAds.BannerTypes.banner320x48)
                   this.sceneSelected.Ad.size = new Size(320, 48);
               else if (value == CoronaAds.BannerTypes.banner468x60)
                   this.sceneSelected.Ad.size = new Size(468, 60);
               else if(value == CoronaAds.BannerTypes.banner728x90)
                   this.sceneSelected.Ad.size = new Size(728, 90);
               else if (value == CoronaAds.BannerTypes.fullscreen)
               {
                   if(this.sceneSelected.projectParent.Orientation == CoronaGameProject.OrientationScreen.Landscape)
                       this.sceneSelected.Ad.size = new Size(480, 320);
                   else if(this.sceneSelected.projectParent.Orientation == CoronaGameProject.OrientationScreen.Portrait)
                       this.sceneSelected.Ad.size = new Size(320, 480);
               }
               else if (value == CoronaAds.BannerTypes.banner)
                   this.sceneSelected.Ad.size = new Size(320, 48);
               else if (value == CoronaAds.BannerTypes.text)
                   this.sceneSelected.Ad.size = new Size(320, 48);

            }
        }

        [Category("ADVERTISING")]
        [DescriptionAttribute("The location of the banner.")]
        public Point Location
        {
            get
            {
                return this.sceneSelected.Ad.location;
            }

            set
            {

                this.sceneSelected.Ad.location = value;
            }
        }

       [Category("ADVERTISING")]
        [DescriptionAttribute("The refreshing interval in seconds.")]
        public int RefreshInterval
        {
            get
            {
                return this.sceneSelected.Ad.interval;
            }

            set
            {

                 this.sceneSelected.Ad.interval = value;
            }
        }

        [Category("ADVERTISING")]
        [DescriptionAttribute("Is in test mode.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool TestMode
        {
            get
            {
                return  this.sceneSelected.Ad.isTestMode;
            }

            set
            {

                this.sceneSelected.Ad.isTestMode = value;
            }
        }

        [Category("ADVERTISING")]
        [DescriptionAttribute("The provider of the ad.")]
        [Editor(typeof(AdsProviderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Krea.Corona_Classes.CoronaAds.AdsProvider Provider
        {
            get
            {
                return this.sceneSelected.Ad.provider;
            }

            set
            {



                this.sceneSelected.Ad.provider = value;
            }
        }

       [Category("ADVERTISING")]
        [DescriptionAttribute("The application Id.")]
        public string AppID
        {
            get
            {
                return this.sceneSelected.Ad.appId;
               
            }

            set
            {


                this.sceneSelected.Ad.appId = value;
            }
        }

    }
}
