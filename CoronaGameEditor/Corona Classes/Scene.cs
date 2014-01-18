using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using System.Drawing.Drawing2D;
using System.IO;
using Krea.Corona_Classes;
using Krea.GameEditor.TilesMapping;
using Krea.Corona_Classes.Controls;
using Krea.Corona_Classes.Widgets;
using Krea.GameEditor.CollisionManager;
using System.Reflection;
using System.Windows.Forms;
using Krea.GameEditor;
namespace Krea.CoronaClasses
{

    [Serializable()]
    public class Scene
    {

        //---------------------------------------------------
        //-------------------Enums---------------------------
        //---------------------------------------------------
        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum PhysicsDrawMode
        {
            normal = 1,
            hybrid = 2,
            debug = 3
        }

        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum StatusBarDisplayMode
        {
            Hidden = 1,
            Default = 2,
            Translucent = 3,
            Dark = 4
        }

        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------

        public int Zoom;
        public int LastScale;
        public Point lastPos;
        public Point CurrentSceneViewLocation;
        public List<CoronaLayer> Layers;
        public List<CoronaSpriteSet> SpriteSets;
        public List<CoronaSpriteSheet> SpriteSheets;
        public List<CollisionFilterGroup> CollisionFilterGroups;
        public String Name;

        public Camera Camera;
        //---- OLD PROPERTIES
        public Rectangle SurfaceFocus;
        public Rectangle CameraFollowLimitRectangle;
        public CoronaObject objectFocusedByCamera;
        public bool isSurfaceFocusVisible;
        //-----

        public bool isMultiTouchActive = false;
        
        public bool IsFPSVisible;
        public Size InitialSize;
        public Size Size;
       
        private CoronaGameProject.OrientationScreen orientation;
        public CoronaGameProject projectParent;
        public List<CoronaVariable> vars { get; set; }
        public List<CoronaFunction> functions { get; set; }

        public CoronaAds Ad;

        //----Physics Properties
        public bool isPhysicsActive = false;
        public PhysicsDrawMode PhysDrawMode = PhysicsDrawMode.normal;
        public double physicsXGravity = 0;
        public double physicsYGravity = 9.8;

        public int PhysicScale = 30;
        public int PhysicPositionIterations = 8;
        public int PhysicVelocityIterations = 3;
        public bool isEnabled = true;
        public Color DefaultColor = Color.Black;
        public StatusBarDisplayMode StatusBarMode = StatusBarDisplayMode.Hidden;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public Scene(Size sceneSize, CoronaGameProject.OrientationScreen or, CoronaGameProject projectParent)
        {
            this.projectParent = projectParent;
            this.Layers = new List<CoronaLayer>();
            this.SpriteSets = new List<CoronaSpriteSet>();
            this.SpriteSheets = new List<CoronaSpriteSheet>();

            this.functions = new List<CoronaFunction>();
            this.vars = new List<CoronaVariable>();

            this.InitialSize = sceneSize;
            this.Size = sceneSize;

           
            Rectangle cameraFollowLimitRectangle = new Rectangle(0, 0, this.projectParent.width, this.projectParent.height);
            Rectangle surfaceFocusCamera = new Rectangle(new Point(this.Size.Width / 2, this.Size.Height / 2),Size.Empty);
            this.Camera = new Camera(this, surfaceFocusCamera, cameraFollowLimitRectangle);
            this.setOrientation(or);
           
            
            this.Ad = new CoronaAds();
            this.initCollisionGroupFilter();
        }

        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        public void initCollisionGroupFilter()
        {
            this.CollisionFilterGroups = new List<CollisionFilterGroup>();
        }

        public void setOrientation(CoronaGameProject.OrientationScreen or)
        {
            this.orientation = or;

            if (this.orientation == CoronaGameProject.OrientationScreen.Landscape)
                this.Camera.SurfaceFocus.Size = new Size(480, 320);
            else if (this.orientation == CoronaGameProject.OrientationScreen.Portrait)
                this.Camera.SurfaceFocus.Size = new Size(320, 480);
        }

        

        public void createLuaFile()
        {
            //Coper le template d'une scene
            if(this.Name.Equals("mapeditormobile"))
                File.Copy(Path.GetDirectoryName(Application.ExecutablePath) + "\\Lua Template\\scene_editormobile.lua", this.projectParent.SourceFolderPath + "\\" + this.Name + ".lua", true);
            else
                File.Copy(Path.GetDirectoryName(Application.ExecutablePath) + "\\Lua Template\\scene.lua", this.projectParent.SourceFolderPath + "\\" + this.Name + ".lua", true);
        }

        public CoronaLayer newLayer()
        {
            CoronaLayer layer = new CoronaLayer(this.Layers.Count, this);

            this.Layers.Add(layer);

            return layer;

        }



        public void moveAdSurface(Point p)
        {

            int xMove = this.Ad.location.X - (this.lastPos.X - p.X);
            int yMove = this.Ad.location.Y - (this.lastPos.Y - p.Y);


            this.lastPos.X = p.X;
            this.lastPos.Y = p.Y;


            this.Ad.location = new Point(xMove, yMove);
        }

        public void deselectAllObjects()
        {
            for (int i = 0; i < this.Layers.Count; i++)
            {
                this.Layers[i].deselectAllObjects();
            }
        }

        public void deselectAllControls()
        {
            for (int i = 0; i < this.Layers.Count; i++)
            {
                this.Layers[i].deselectAllControls();
            }
        }

        public void dessineGorgonScene(float xScale, float yScale, Point offsetPoint, bool drawFocusShader)
        {

            for (int i = 0; i < this.Layers.Count; i++)
            {
                if (this.Layers[i].isEnabled == true)
                    this.Layers[i].dessineGorgonLayer(xScale, yScale, offsetPoint);

            }

           
            //Afficher la fenetre de l'ad si elle existe
            if (this.Ad.isActive == true)
            {
                SolidBrush brAd = new SolidBrush(Color.FromArgb(180, 0, 0, 0));


                Point pDestAd = new Point(offsetPoint.X + this.Ad.location.X + this.Camera.SurfaceFocus.Location.X, offsetPoint.Y + this.Ad.location.Y + this.Camera.SurfaceFocus.Location.Y);
                GorgonGraphicsHelper.Instance.FillRectangle(new Rectangle(pDestAd, this.Ad.size), 1, Color.FromArgb(180, 0, 0, 0), xScale, false);
               
                
                SizeF stringSize = TextRenderer.MeasureText("AD BANNER", SystemFonts.DefaultFont);

                GorgonGraphicsHelper.Instance.DrawText("AD BANNER", "DEFAULT", SystemFonts.DefaultFont.Size, new Point(pDestAd.X + this.Ad.size.Width / 2 - (int)stringSize.Width / 2,
                            pDestAd.Y + this.Ad.size.Height / 2 - (int)stringSize.Height / 2), Color.White, 0, false, Rectangle.Empty,xScale);


            }

            if (drawFocusShader == true)
            {

                Color color = Color.FromArgb(125, Color.DarkGray);
                
                //GraphicsPath path = new GraphicsPath();
                //path.AddRectangle(new Rectangle(new Point(offsetPoint.X, offsetPoint.Y),
                //    new Size(GorgonLibrary.Gorgon.Screen.Width - offsetPoint.X, (int)GorgonLibrary.Gorgon.Screen.Height - offsetPoint.Y)));


                if (this.projectParent.Orientation == CoronaGameProject.OrientationScreen.Portrait)
                    this.Camera.SurfaceFocus = new Rectangle(this.Camera.SurfaceFocus.Location, new Size(this.projectParent.width, this.projectParent.height));
                else
                    this.Camera.SurfaceFocus = new Rectangle(this.Camera.SurfaceFocus.Location, new Size(this.projectParent.height, this.projectParent.width));


                //Point pDestFocus = new Point(offsetPoint.X + this.Camera.SurfaceFocus.Location.X, offsetPoint.Y + this.Camera.SurfaceFocus.Location.Y);
                //path.AddRectangle(new Rectangle(pDestFocus, this.Camera.SurfaceFocus.Size));

                //path.Dispose();

                int yTopDest = (int)((float)(this.Camera.SurfaceFocus.Y+ offsetPoint.Y) *yScale);
                Rectangle topRect = new Rectangle(0, 0,
                    GorgonLibrary.Gorgon.CurrentRenderTarget.Width, yTopDest);

                int yBottomStart = (int)((float)(this.Camera.SurfaceFocus.Y+this.Camera.SurfaceFocus.Height + offsetPoint.Y) * yScale);
                Rectangle bottomRect = new Rectangle(0, yBottomStart,
                  GorgonLibrary.Gorgon.CurrentRenderTarget.Width, GorgonLibrary.Gorgon.CurrentRenderTarget.Height - yBottomStart);

                int xLeftDest = (int)((float)(this.Camera.SurfaceFocus.X + offsetPoint.X) * xScale);
                Rectangle leftRect = new Rectangle(0, yTopDest, xLeftDest, (int)((float)this.Camera.SurfaceFocus.Height * yScale));

                int xRightStart = (int)((float)(this.Camera.SurfaceFocus.X +this.Camera.SurfaceFocus.Width+ offsetPoint.X) * xScale);
                int xRightDest = GorgonLibrary.Gorgon.CurrentRenderTarget.Width - xRightStart;
                Rectangle rightRect = new Rectangle(xRightStart, yTopDest, xRightDest, (int)((float)this.Camera.SurfaceFocus.Height * yScale));

                GorgonGraphicsHelper.Instance.FillRectangle(topRect, 1, color, 1, false);
                GorgonGraphicsHelper.Instance.FillRectangle(bottomRect, 1, color, 1, false);
                GorgonGraphicsHelper.Instance.FillRectangle(leftRect, 1, color, 1, false);
                GorgonGraphicsHelper.Instance.FillRectangle(rightRect, 1, color, 1, false);
            }
        }

        public void dessineScene(Graphics g, float xScale, float yScale, Point offsetPoint)
        {

            for (int i = 0; i < this.Layers.Count; i++)
            {
                if(this.Layers[i].isEnabled == true) 
                     this.Layers[i].dessineLayer(g, xScale, yScale, offsetPoint);

            }

            Matrix m = new Matrix();
            m.Scale(xScale, yScale);
            g.Transform = m;

            //Afficher la fenetre de l'ad si elle existe
            if (this.Ad.isActive == true)
            {
                SolidBrush brAd = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
                Point pDestAd = new Point(offsetPoint.X + this.Ad.location.X + this.Camera.SurfaceFocus.Location.X, offsetPoint.Y + this.Ad.location.Y + this.Camera.SurfaceFocus.Location.Y);
                g.FillRectangle(brAd, new Rectangle(pDestAd, this.Ad.size));
                SizeF stringSize = g.MeasureString("AD BANNER", new Font("ARIAL", 12));

                g.DrawString("AD BANNER", new Font("ARIAL", 12), Brushes.White, new Point(pDestAd.X + this.Ad.size.Width / 2 - (int)stringSize.Width /2,
                            pDestAd.Y + this.Ad.size.Height / 2 - (int)stringSize.Height / 2));
            }

            if (this.Camera.isSurfaceFocusVisible == true)
            {

                SolidBrush br = new SolidBrush(Color.FromArgb(80, Color.DarkGray));

                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(new Rectangle(new Point(offsetPoint.X, offsetPoint.Y),
                    new Size((int)g.ClipBounds.Width - offsetPoint.X, (int)g.ClipBounds.Height - offsetPoint.Y)));


                if (this.projectParent.Orientation == CoronaGameProject.OrientationScreen.Portrait)
                    this.Camera.SurfaceFocus = new Rectangle(this.Camera.SurfaceFocus.Location, new Size(this.projectParent.width, this.projectParent.height));
                else
                    this.Camera.SurfaceFocus = new Rectangle(this.Camera.SurfaceFocus.Location, new Size(this.projectParent.height, this.projectParent.width));


                Point pDestFocus = new Point(offsetPoint.X + this.Camera.SurfaceFocus.Location.X, offsetPoint.Y + this.Camera.SurfaceFocus.Location.Y);
                path.AddRectangle(new Rectangle(pDestFocus, this.Camera.SurfaceFocus.Size));

                g.FillPath(br, path);

            }


        }

        public CoronaObject getObjectTouched(Point p)
        {
            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                CoronaLayer layer = this.Layers[i];
                if (layer.isEnabled == true)
                {
                    CoronaObject obj = layer.getObjTouched(p);

                    if (obj != null)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        public CoronaObject getEntityTouched(Point p)
        {
            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                CoronaLayer layer = this.Layers[i];
                CoronaObject obj = layer.getEntityTouched(p);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        public JoystickControl getJoystickTouched(Point p)
        {
            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                CoronaLayer layer = this.Layers[i];
                JoystickControl obj = layer.getJoystickTouched(p);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        //--------------------------------------------------------------------------
        //------------------------ GENERATE CODE LUA METHOD------------------------
        //--------------------------------------------------------------------------

        public String generateLocalVarsLua()
        {
            StringBuilder sb = new StringBuilder();


            //Create accesseur to resources
            sb.AppendLine("-- Declaring Camera Instance ------");
            sb.AppendLine("local "+this.Name+"_camera = nil");
            sb.AppendLine("---------- INITIALIZE KREA ASSET RESOURCES HIERARCHY: " + this.Name + " ----------");
            sb.AppendLine("if(not storyboard.resources) then storyboard.resources = {} end");
            sb.AppendLine("storyboard.resources." + this.Name + " = {}");
            sb.AppendLine("local " + this.Name + " = storyboard.resources." + this.Name+"\n");

             //Pour tous les layers de la scene
            for (int i = 0; i < this.Layers.Count; i++)
            {
                CoronaLayer layerParent = this.Layers[i];

                string name = layerParent.Name.Replace(" ", "");

                //Ajouter au content un Display group pour l'objet
                sb.AppendLine(this.Name + "." + name + " = {}");
                sb.AppendLine("local " + name + " = " + this.Name + "." + name+"\n");
            }


            for (int j = 0; j < this.vars.Count; j++)
            {
                CoronaVariable cVars = this.vars[j];
                if (cVars.isLocal)
                {
                    //Construction of variable lua code
                    String buf = "";
                    buf += "local ";
                    buf += cVars.Name;
                    if (!cVars.InitValue.Equals("")) buf += " = " + cVars.InitValue;

                    sb.AppendLine(buf);
                }

            }

            //Activé le FPS Si desiré
            if (this.IsFPSVisible == true)
            {
                sb.AppendLine("local performance = nil");

            }


            return sb.ToString();
        }



        public String generateGlobalVarsLua()
        {
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < this.vars.Count; j++)
            {
                CoronaVariable cVars = this.vars[j];
                if (!cVars.isLocal)
                {
                    //Construction of variable lua code
                    String buf = "";
                    buf += cVars.Name;
                    if (!cVars.InitValue.Equals("")) buf += " = " + cVars.InitValue;

                    sb.AppendLine(buf);
                }

            }

            return sb.ToString();
        }

        public void prepareForBuild()
        {
            this.SpriteSets.Clear();
            this.SpriteSheets.Clear();

            for (int i = 0; i < this.Layers.Count; i++)
            {
                if(this.Layers[i].isEnabled == true)
                {
                    for (int j = 0; j < this.Layers[i].CoronaObjects.Count; j++)
                    {
                        CoronaObject obj = this.Layers[i].CoronaObjects[j];
                        if (obj.isEnabled == true)
                        {
                            if (obj.isEntity == false)
                            {

                                if (obj.DisplayObject.Type.Equals("SPRITE"))
                                {
                                    CoronaSpriteSet set = obj.DisplayObject.SpriteSet;
                                    if (!this.SpriteSets.Contains(set))
                                    {
                                        bool alreadyExist = false;
                                        for (int l = 0; l < this.SpriteSets.Count; l++)
                                        {
                                            if (this.SpriteSets[l].Name.Equals(set.Name))
                                            {
                                                alreadyExist = true;
                                                break;
                                            }
                                        }

                                        if (alreadyExist == false)
                                            this.SpriteSets.Add(set);

                                        
                                    }

                                    for (int k = 0; k < set.Frames.Count; k++)
                                    {
                                        CoronaSpriteSheet sheet = set.Frames[k].SpriteSheetParent;
                                        if (!this.SpriteSheets.Contains(sheet))
                                        {
                                            bool alreadyExist = false;
                                            for (int l = 0; l < this.SpriteSheets.Count; l++)
                                            {
                                                if (this.SpriteSheets[l].Name.Equals(sheet.Name))
                                                {
                                                    alreadyExist = true;
                                                    break;
                                                }
                                            }

                                            if (alreadyExist == false)
                                                this.SpriteSheets.Add(sheet);

                                            
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int k = 0; k < obj.Entity.CoronaObjects.Count; k++)
                                {
                                    CoronaObject child = obj.Entity.CoronaObjects[k];
                                    if (child.isEnabled == true)
                                    {
                                        if (child.DisplayObject.Type.Equals("SPRITE"))
                                        {
                                            CoronaSpriteSet set = child.DisplayObject.SpriteSet;
                                            if (!this.SpriteSets.Contains(set))
                                            {
                                                bool alreadyExist = false;
                                                for (int l = 0; l < this.SpriteSets.Count; l++)
                                                {
                                                    if (this.SpriteSets[l].Name.Equals(set.Name))
                                                    {
                                                        alreadyExist = true;
                                                        break;
                                                    }
                                                }

                                                if (alreadyExist == false)
                                                    this.SpriteSets.Add(set);
                                            }

                                            for (int l = 0; l < set.Frames.Count; l++)
                                            {
                                                CoronaSpriteSheet sheet = set.Frames[l].SpriteSheetParent;
                                                if (!this.SpriteSheets.Contains(sheet))
                                                {

                                                    bool alreadyExist = false;
                                                    for (int m = 0; m < this.SpriteSheets.Count; m++)
                                                    {
                                                        if (this.SpriteSheets[m].Name.Equals(sheet.Name))
                                                        {
                                                            alreadyExist = true;
                                                            break;
                                                        }
                                                    }

                                                    if (alreadyExist == false)
                                                        this.SpriteSheets.Add(sheet);
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
        }

        public String generateRequiresLua()
        {
            StringBuilder sb = new StringBuilder();

            bool hasTilesMap = false;
            bool hasPathFollow = false;
            bool adsActive = this.Ad.isActive;
            bool hasGenerator = false;
            bool hasJoystick = false;
            bool hasEntity = false;

            for (int i = 0; i < this.Layers.Count; i++)
            {
                if (this.Layers[i].isEnabled == true)
                {
                    if (this.Layers[i].TilesMap != null && this.Layers[i].TilesMap.isEnabled == true)
                        hasTilesMap = true;

                    CoronaLayer layer = this.Layers[i];
                    for (int k = 0; k < layer.CoronaObjects.Count; k++)
                    {
                        if (layer.CoronaObjects[k].isEnabled == true)
                        {
                            if (layer.CoronaObjects[k].isEntity == false)
                            {
                                if (layer.CoronaObjects[k].PathFollow.isEnabled == true)
                                {
                                    hasPathFollow = true;
                                }

                                if (layer.CoronaObjects[k].isGenerator == true)
                                {
                                    hasGenerator = true;
                                }
                            }
                            else
                            {
                                hasEntity = true;

                                if (layer.CoronaObjects[k].isGenerator == true)
                                {
                                    hasGenerator = true;
                                }

                                for (int l = 0; l < layer.CoronaObjects[k].Entity.CoronaObjects.Count; l++)
                                {
                                    CoronaObject child = layer.CoronaObjects[k].Entity.CoronaObjects[l];
                                    if (child.isEnabled == true)
                                    {
                                        if (child.PathFollow.isEnabled == true)
                                        {
                                            hasPathFollow = true;
                                        }

                                        if (child.isGenerator == true)
                                        {
                                            hasGenerator = true;
                                        }
                                    }
                                }

                            }
                        }

                    }

                    for (int j = 0; j < this.Layers[i].Controls.Count; j++)
                    {
                        if (this.Layers[i].Controls[j].isEnabled == true)
                        {
                            hasJoystick = true;
                            break;
                        }
                    }

                }
            }

            if (this.isPhysicsActive == true)
                sb.AppendLine("local physics = require(\"physics\")");

            sb.AppendLine("local cameraInstance = require(\"camera\")");
            sb.AppendLine("local objectInstance = require(\"object\")");
            if (hasEntity == true)
                sb.AppendLine("local entityInstance = require(\"entity\")");

            if (this.SpriteSheets.Count > 0)
                sb.AppendLine("local sprite = require(\"sprite\")");

            if(hasJoystick == true)
                sb.AppendLine("local joystickClass = require(\"joystick\")\n");

            if (hasGenerator == true)
                sb.AppendLine("local generatorInstance = require (\"generator\")");

            if (adsActive == true)
                sb.AppendLine("local ads = require (\"ads\")");
            if (hasTilesMap == true)
                sb.AppendLine("local tilesMapInstance = require(\"tilesmap\")");
            if (hasPathFollow == true)
                sb.AppendLine("local pathFollowInstance = require(\"pathfollow\")");

            if (this.projectParent.Snippets.Count > 0)
                sb.AppendLine("local snippets = require(\"snippets\")");


               

            return sb.ToString();
        }

        public String generateDeclarationsLua(float XRatio, float YRatio)
        {
            StringBuilder sb = new StringBuilder();
            

            //Check for touch joint and declare the dragBody Function if necessary
            for (int i = 0; i < this.Layers.Count; i++)
            {
                bool dragObjectFound = false;
                CoronaLayer layer = this.Layers[i];
                if (layer.isEnabled == true)
                {
                    for (int j = 0; j < layer.CoronaObjects.Count; j++)
                    {
                        CoronaObject obj = layer.CoronaObjects[j];
                        if (obj.isEnabled == true)
                        {
                            if (obj.isEntity == false)
                            {
                                if (obj.isDraggable == true)
                                {
                                    dragObjectFound = true;
                                }
                            }
                            else
                            {
                                for (int l = 0; l < obj.Entity.CoronaObjects.Count; l++)
                                {
                                    CoronaObject child = obj.Entity.CoronaObjects[l];
                                    if (child.isEnabled == true)
                                    {
                                        if (child.isDraggable == true)
                                        {
                                            dragObjectFound = true;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                if (dragObjectFound == true)
                {
                    sb.AppendLine("-- A general function for dragging physics bodies");
                    sb.AppendLine("local function dragBody( event )");
                    sb.AppendLine("\tlocal body = event.target");
                    sb.AppendLine("\tlocal phase = event.phase");
                    sb.AppendLine("\tlocal stage = display.getCurrentStage()");
                    sb.AppendLine("\tif \"began\" == phase then");
                    sb.AppendLine("\t\tstage:setFocus( body, event.id )");
                    sb.AppendLine("\t\tbody.isFocus = true");
                    sb.AppendLine("\t\tlocal layerXOffset = 0");
                    sb.AppendLine("\t\tlocal layerYOffset = 0");
                    sb.AppendLine("\t\tlocal layerParent = body.getInstance().displayGroupParent");
                    sb.AppendLine("\t\tlocal instanceObject = body.getInstance()");
                    sb.AppendLine("\t\tif(instanceObject) then");
                    sb.AppendLine("\t\t\tlayerXOffset = layerParent.x");
                    sb.AppendLine("\t\t\tlayerYOffset = layerParent.y");
                    sb.AppendLine("\t\tend");
                    sb.AppendLine("\t\tlocal stageX,stageY = scene.view.xOrigin,scene.view.yOrigin");
                    sb.AppendLine("\t\tlocal xDest = event.x - stageX - layerXOffset");
                    sb.AppendLine("\t\tlocal yDest = event.y - stageY - layerYOffset");
                    sb.AppendLine("\t\tbody.tempJoint = physics.newJoint( \"touch\", body, xDest*(1/layerParent.xScale), yDest*(1/layerParent.yScale) )");
                    sb.AppendLine("\t\tbody.tempJoint.maxForce = instanceObject.dragMaxForce");
                    sb.AppendLine("\t\tbody.tempJoint.frequency = instanceObject.dragFrequency");
                    sb.AppendLine("\t\tbody.tempJoint.dampingRatio = instanceObject.dragDamping");
                    sb.AppendLine("\telseif body.isFocus then");
                    sb.AppendLine("\t\tif \"moved\" == phase then");
                    sb.AppendLine("\t\tlocal layerXOffset = 0");
                    sb.AppendLine("\t\tlocal layerYOffset = 0");
                    sb.AppendLine("\t\tlocal layerParent = body.getInstance().displayGroupParent");
                    sb.AppendLine("\t\tif(body.getInstance) then");
                    sb.AppendLine("\t\t\tlayerXOffset = layerParent.x");
                    sb.AppendLine("\t\t\tlayerYOffset = layerParent.y");
                    sb.AppendLine("\t\tend");
                    sb.AppendLine("\t\tlocal stageX,stageY = scene.view.xOrigin,scene.view.yOrigin");
                    sb.AppendLine("\t\tlocal xDest = event.x - stageX - layerXOffset");
                    sb.AppendLine("\t\tlocal yDest = event.y - stageY - layerYOffset");
                    sb.AppendLine("\t\t\tbody.tempJoint:setTarget( xDest*(1/layerParent.xScale), yDest*(1/layerParent.yScale) )");
                    sb.AppendLine("\t\telseif \"ended\" == phase or \"cancelled\" == phase then");
                    sb.AppendLine("\t\t\tstage:setFocus( body, nil )");
                    sb.AppendLine("\t\t\tbody.isFocus = false");
                    sb.AppendLine("\t\t\tbody.tempJoint:removeSelf()");
                    sb.AppendLine("\t\tend");
                    sb.AppendLine("\tend");
                    sb.AppendLine("\treturn true");
                    sb.AppendLine("end");

                    break;
                }
            }

            
            sb.AppendLine("\nlocal deviceXRatio = 0");
            sb.AppendLine("local deviceYRatio = 0");


            for (int i = 0; i < this.SpriteSheets.Count; i++)
            {
                CoronaSpriteSheet sheet = this.SpriteSheets[i];
                sb.AppendLine("local data_" + sheet.Name + " = nil");
                sb.AppendLine("local sheet_" + sheet.Name + " = nil");
            }

            for (int i = 0; i < this.SpriteSets.Count; i++)
            {
                CoronaSpriteSet set = this.SpriteSets[i];

                //Ecrire dans le code le chargement de la sprite sheet
                sb.AppendLine("local " + set.Name + " = nil");
            }

           

            for (int i = 0; i < this.Layers.Count; i++)
            {
                CoronaLayer layer = this.Layers[i];

                /*if (i == 0)
                {
                    sb.AppendLine("-----------------------------------------------------------------------------");
                    sb.AppendLine("------------------ Layers & associated objects declarations -----------------");
                    sb.AppendLine("-----------------------------------------------------------------------------");
                }
               
                sb.AppendLine("\n------ Layer "+layer.Name+" -------");

                //Declarer le layer
                sb.AppendLine("local layer_" + layer.Name + " = nil");

                if (layer.TilesMap != null)
                {
                    sb.AppendLine("local " + layer.TilesMap.TilesMapName.Replace(" ", "_") + " = nil");
                }

                for (int j = 0; j < layer.CoronaObjects.Count; j++)
                {
                    CoronaObject obj = layer.CoronaObjects[j];
                    sb.AppendLine("local " + obj.DisplayObject.Name + " = nil");

                    if (obj.isGenerator == true)
                    {
                        sb.AppendLine("local " + obj.DisplayObject.Name + "_generator = nil");
                    }

                }
           */

                if (layer.isEnabled == true)
                {
                    for (int j = 0; j < layer.Controls.Count; j++)
                    {
                        if (layer.Controls[j].type == CoronaControl.ControlType.joystick)
                        {
                            JoystickControl joy = layer.Controls[j] as JoystickControl;
                            if (j == 0)
                            {
                                sb.AppendLine("\n------ CONTROLS : " + layer.Name + " -------");
                                sb.AppendLine("---- You should initialize these Control handlers in the New function, just before the initializeComponents function ! -----");

                            }

                            sb.AppendLine("local " + joy.joystickName + "_onMove = nil\n");
                        }

                    }

                    //Gerer les Widgets
                    for (int j = 0; j < layer.Widgets.Count; j++)
                    {
                        if (j == 0)
                        {
                            sb.AppendLine("------ WIDGETS -------");
                            sb.AppendLine("---- You should initialize these widgets handlers in the New function, just before the initializeComponents function ! -----");
                            sb.AppendLine("local widget = require(\"widget\")\n");
                        }

                        CoronaWidget widget = layer.Widgets[j];

                        if (widget.Type == CoronaWidget.WidgetType.tabBar)
                        {
                            sb.AppendLine("local " + widget.Name + "_onBtPress = nil\n");
                        }


                    }
                }
            }

            
            return sb.ToString();
        }

        public String generateInitializeComponentLua(bool isCustomBuild,float XRatio, float YRatio)
        {

           

            //Verifier si la physic doit etre activer
            if (this.isPhysicsActive == false)
            {
                for (int i = 0; i < this.Layers.Count; i++)
                {
                    if (this.Layers[i].isEnabled == true)
                    {

                        bool found = false;
                        if (this.Layers[i].TilesMap != null)
                        {
                            if (this.Layers[i].TilesMap.isEnabled == true)
                            {
                                if (this.Layers[i].TilesMap.isPhysicsEnabled == true)
                                {
                                    this.isPhysicsActive = true;
                                    found = true;
                                    break;
                                }
                            }
                        }

                        for (int j = 0; j < this.Layers[i].CoronaObjects.Count; j++)
                        {
                            CoronaObject obj = this.Layers[i].CoronaObjects[j];
                            if (obj.isEnabled == true)
                            {
                                if (obj.isEntity == false)
                                {
                                    if (obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                    {
                                        this.isPhysicsActive = true;
                                        found = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < obj.Entity.CoronaObjects.Count; k++)
                                    {
                                        CoronaObject child = obj.Entity.CoronaObjects[k];
                                        if (child.isEnabled == true)
                                        {
                                            if (child.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                            {
                                                this.isPhysicsActive = true;
                                                found = true;
                                                break;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        if (found == true)
                            break;
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\tdeviceXRatio = " + XRatio.ToString().Replace(",", "."));
            sb.AppendLine("\tdeviceYRatio = " + YRatio.ToString().Replace(",", "."));

            sb.AppendLine(createAllSpriteSheetsLua(isCustomBuild));
            sb.AppendLine(this.createAllSpriteSets());

            float moyenneRatio = (XRatio + YRatio) / 2;

            //Ajouter le strat de la physics si desiré
            if (this.isPhysicsActive == true)
            {
                sb.AppendLine("\tphysics.start()");
                sb.AppendLine("\tphysics.setDrawMode(\"" + this.PhysDrawMode + "\")");
                sb.AppendLine("\tphysics.setGravity(" + this.physicsXGravity.ToString().Replace(",", ".") +
                                "," + this.physicsYGravity.ToString().Replace(",", ".") + ")");

                sb.AppendLine("\tphysics.setScale(" + this.PhysicScale + ")");
                sb.AppendLine("\tphysics.setPositionIterations(" + this.PhysicPositionIterations + ")");
                sb.AppendLine("\tphysics.setVelocityIterations(" + this.PhysicVelocityIterations + ")");

            }

           


           

            this.buildAllLayers(sb, XRatio, YRatio);

            //for (int i = 0; i < this.Layers.Count; i++)
            //{
            //    sb.AppendLine("\t\t"+this.Layers[i].Name+".displayGroup:translate(" + ((int)((float)-this.SurfaceFocus.X * XRatio)).ToString().Replace(",", ".") + "," +
            //                              ((int)((float)-this.SurfaceFocus.Y * YRatio)).ToString().Replace(",", ".") + ")");
            //}


            sb.AppendLine("\t---- Initialize Camera instance -----");
            sb.AppendLine("\t" + this.Name + "_camera = cameraInstance.Camera.create()");
            //Si le focus est defini sur un objet
            if (this.Camera.objectFocusedByCamera != null)
            {
                if (this.Camera.objectFocusedByCamera.isEnabled == true)
                {
                    String nameObj = this.Camera.objectFocusedByCamera.DisplayObject.Name;
                    String nameLayer = this.Camera.objectFocusedByCamera.LayerParent.Name;

                    float xLeft = (this.Camera.CameraFollowLimitRectangle.Left - this.Camera.SurfaceFocus.Left) * XRatio;
                    float xRight = (this.Camera.CameraFollowLimitRectangle.Right - this.Camera.SurfaceFocus.Left) * XRatio;
                    float yTop = (this.Camera.CameraFollowLimitRectangle.Top - this.Camera.SurfaceFocus.Top) * YRatio;
                    float yBottom = (this.Camera.CameraFollowLimitRectangle.Bottom - this.Camera.SurfaceFocus.Top) * YRatio;

                    sb.AppendLine("\t" + this.Name + "_camera.scrollWindowLeft= " + xLeft.ToString().Replace(",", "."));
                    sb.AppendLine("\t" + this.Name + "_camera.scrollWindowTop =  " + yTop.ToString().Replace(",", "."));
                    sb.AppendLine("\t" + this.Name + "_camera.scrollWindowRight =  " + xRight.ToString().Replace(",", "."));
                    sb.AppendLine("\t" + this.Name + "_camera.scrollWindowBottom =  " + yBottom.ToString().Replace(",", "."));


                }
            }

         

            for (int i = 0; i < this.Layers.Count; i++)
            {
                CoronaLayer layerParent = this.Layers[i];
                if (layerParent.isEnabled == true)
                {
                    //Creer la tilesMap du layer
                    TilesMap map = layerParent.TilesMap;
                    if (map != null)
                    {
                        if(map.isEnabled == true)
                            sb.AppendLine("\t" + layerParent.Name + "." + map.TilesMapName + ":setAutoScrollActive()");
                    }

                    //Translate la position du group d'objet general (scene)
                    //sb.AppendLine("\t" + layerParent.Name + ".displayGroup:translate(" + ((int)((float)-this.SurfaceFocus.X * XRatio)).ToString().Replace(",", ".") + "," +
                    //    ((int)((float)-this.SurfaceFocus.Y * YRatio)).ToString().Replace(",", ".") + ")");
                }
            }

         
            //Translate la position du group d'objet general (scene)
            //sb.AppendLine("\tlocalGroup:translate(" + ((int)((float)-this.SurfaceFocus.X * XRatio)).ToString().Replace(",", ".") + "," + 
            //    ((int)((float)-this.SurfaceFocus.Y * YRatio)).ToString().Replace(",", ".") + ")");



            //Creer la baniere de pub 
            if (this.Ad.isActive == true)
            {
                sb.AppendLine("\tads.init(\"" + this.Ad.provider.ToString() + "\",\"" + this.Ad.appId + "\")");

                sb.AppendLine("\tads.show(\"" + this.Ad.bannerType.ToString() +
                        "\",{x =" + ((float)this.Ad.location.X * XRatio).ToString().Replace(",", ".") + ",y=" + ((float)this.Ad.location.Y * YRatio).ToString().Replace(",", ".") +
                        ",interval =" + this.Ad.interval + ",testMode=" + this.Ad.isTestMode.ToString().ToLower() + "})");


            }

            //Ajouter le FPS Si desiré
            if (this.IsFPSVisible == true)
            {

                sb.AppendLine("\t------------------ INIT PERFORMANCE ANALYSER -----------------------");
                sb.AppendLine("\tperformance = require(\"perfanalyser\").PerfAnalyser.create({group = localGroup})");

                sb.AppendLine("\t--------------------------------------------------------------------");

            }


            return sb.ToString();

        }

        private String createAllSpriteSheetsLua(bool isCustomBuild)
        {
            StringBuilder sb = new StringBuilder();

            List<CoronaSpriteSheet> sheetsToBuild = new List<CoronaSpriteSheet>();

            for (int i = 0; i < this.Layers.Count; i++)
            {
                if(this.Layers[i].isEnabled == true)
                {
                    for (int j = 0; j < this.Layers[i].CoronaObjects.Count; j++)
                    {
                        CoronaObject obj = this.Layers[i].CoronaObjects[j];
                        if (obj.isEnabled == true)
                        {
                            if (obj.isEntity == false)
                            {
                                if (obj.DisplayObject.Type.Equals("SPRITE"))
                                {
                                    CoronaSpriteSet set = obj.DisplayObject.SpriteSet;
                                    for (int k = 0; k < set.Frames.Count; k++)
                                    {
                                        CoronaSpriteSheet sheet = set.Frames[k].SpriteSheetParent;
                                        if (!sheetsToBuild.Contains(sheet))
                                        {
                                            bool alreadyExist = false;
                                            for (int l = 0; l < sheetsToBuild.Count; l++)
                                            {
                                                if (sheetsToBuild[l].Name.Equals(sheet.Name))
                                                {
                                                    alreadyExist = true;
                                                    break;
                                                }
                                            }

                                            if(alreadyExist == false)
                                                sheetsToBuild.Add(sheet);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int k = 0; k < obj.Entity.CoronaObjects.Count; k++)
                                {
                                    CoronaObject child = obj.Entity.CoronaObjects[k];
                                    if (child.isEnabled == true)
                                    {
                                        if (child.DisplayObject.Type.Equals("SPRITE"))
                                        {
                                            CoronaSpriteSet set = child.DisplayObject.SpriteSet;
                                            for (int l = 0; l < set.Frames.Count; l++)
                                            {
                                                CoronaSpriteSheet sheet = set.Frames[l].SpriteSheetParent;
                                                if (!sheetsToBuild.Contains(sheet))
                                                {
                                                    bool alreadyExist = false;
                                                    for (int m = 0; m < sheetsToBuild.Count; m++)
                                                    {
                                                        if (sheetsToBuild[m].Name.Equals(sheet.Name))
                                                        {
                                                            alreadyExist = true;
                                                            break;
                                                        }
                                                    }

                                                    if (alreadyExist == false)
                                                        sheetsToBuild.Add(sheet);
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

            if (isCustomBuild == false)
            {
                sb.AppendLine("\tlocal dynamicXScale = 1/display.contentScaleX");
                sb.AppendLine("\tlocal dynamicYScale= 1/display.contentScaleY");
                sb.AppendLine("\tlocal averageScale  = (dynamicXScale+dynamicYScale)/2");
                sb.AppendLine("\tlocal tabSuffixes = {");
                //Creer les sprites sheets
                List<ImageSuffix> ListImgSuffix = new List<ImageSuffix>();

                // Fill All Image Suffix String in an Object
                for (int i = 0; i < this.projectParent.ImageSuffix.Count; i++)
                {
                    ListImgSuffix.Add(new ImageSuffix(projectParent.ImageSuffix[i]));
                }

                for (int i = 0; i < this.projectParent.ImageSuffix.Count; i++)
                {

                    sb.Append("\t\t{suffix = \"" + ListImgSuffix[i].suffix + "\",value = " + ListImgSuffix[i].ratio.ToString().Replace(",", ".") + "}");
                    if (i < this.projectParent.ImageSuffix.Count - 1)
                        sb.Append(",\n");

                }

                sb.AppendLine("}");
                sb.AppendLine("\tlocal currentSuffix = {suffix = \"\",value = 1}");
                sb.AppendLine("\tfor i = 1,#tabSuffixes do ");
                sb.AppendLine("\t\tlocal minValue = math.min(math.abs(averageScale-currentSuffix.value),math.abs(averageScale- tabSuffixes[i].value))");
                sb.AppendLine("\t\tif(minValue == math.abs(averageScale- tabSuffixes[i].value) ) then ");
                sb.AppendLine("\t\t\tcurrentSuffix =  tabSuffixes[i]");

                sb.AppendLine("\t\tend");
                sb.AppendLine("\tend");
                for (int i = 0; i < sheetsToBuild.Count; i++)
                {

                    CoronaSpriteSheet sheet = sheetsToBuild[i];



                    //Ecrire dans le code le chargement de la sprite sheet
                    sb.AppendLine("\tdata_" + sheet.Name + " = require(\"sprite" + sheet.Name.Replace(" ", "").ToLower() + "anim\"..currentSuffix.suffix).getSpriteSheetData()");
                    sb.AppendLine("\tsheet_" + sheet.Name + " = sprite.newSpriteSheetFromData(\"" +
                                                                                sheet.Name.Replace(" ", "_").ToLower() + "\"..currentSuffix.suffix..\".png\",data_" + sheet.Name + ")\n");
                }
            }
            else
            {
                sb.AppendLine("\tlocal currentSuffix = {suffix = \"\",value = 1}");
                for (int i = 0; i < sheetsToBuild.Count; i++)
                {

                    CoronaSpriteSheet sheet = sheetsToBuild[i];


                   
                    //Ecrire dans le code le chargement de la sprite sheet
                    sb.AppendLine("\tdata_" + sheet.Name + " = require(\"sprite" + sheet.Name.Replace(" ", "").ToLower() + "anim\").getSpriteSheetData()");
                    sb.AppendLine("\tsheet_" + sheet.Name + " = sprite.newSpriteSheetFromData(\"" +
                                                                                sheet.Name.Replace(" ", "_").ToLower() + ".png\",data_" + sheet.Name + ")\n");
                }
            }
            
            return sb.ToString();
        }

        public void createAllSpriteSheetsImages(string suffix, float XRatio, float YRatio)
        {

            for (int i = 0; i < this.SpriteSheets.Count; i++)
            {
                  CoronaSpriteSheet sheet = this.SpriteSheets[i];
                
                  

                    //Creer l'image de la sprite sheet
                    SizeF size = sheet.calculateSize(XRatio, YRatio, true);
                try
                {
                    //if (size.Width > 2048)
                    //    size.Width = 2048;
                    //else
                    //    size.Width = (int)Math.Ceiling(size.Width);

                    //if (size.Height > 2048)
                    //    size.Height = 2048;
                    //else
                    //    size.Height = (int)Math.Ceiling(size.Height);

                    sheet.ImageSpriteSheet = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
                    string sheetDirectory = Path.Combine(this.projectParent.ProjectPath + "\\Resources\\SpriteSheets", sheet.Name);
                    using (Graphics g = Graphics.FromImage(sheet.ImageSpriteSheet))
                    {
                        sheet.dessineAllFrame(g, XRatio, YRatio, sheetDirectory);
                    }
                    sheet.ImageSpriteSheet.Save(this.projectParent.BuildFolderPath + "\\" + sheet.Name.Replace(" ", "_").ToLower() + suffix + ".png", System.Drawing.Imaging.ImageFormat.Png);

                    sheet.ImageSpriteSheet.Dispose();
                    sheet.ImageSpriteSheet = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The bitmap size " + size.Width + "x" + size.Height + " does not seem to be supported by your system. If you are running Krea on a virtual machine, please try to increase the memory allocated for your Windows OS.",
                        "Fail to create sprite sheet \"" + sheet.Name + "\"!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private String createAllSpriteSets()
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.SpriteSets.Count; i++)
            {
                CoronaSpriteSet set = this.SpriteSets[i];

                //Ecrire dans le code le chargement de la sprite sheet
                sb.AppendLine("\t"+set.Name + " = sprite.newSpriteMultiSet(\n{");

                for (int j = 0; j < set.Frames.Count; j++)
                {
                    SpriteFrame frame = set.Frames[j];

                    int indexInSheet = frame.SpriteSheetParent.Frames.IndexOf(frame);

                    sb.AppendLine("\t\t{ sheet = sheet_" + frame.SpriteSheetParent.Name.Replace(" ", "_") + ", frames = {" + (indexInSheet + 1) + "}},");
                }

                sb.AppendLine("}) \n\n");

                sb.AppendLine(" ------ ADD ANIMATION SEQUENCES TO THE SPRITE SET----------\n\n");
                //Creer toutes les sequences
                for (int j = 0; j < set.Sequences.Count; j++)
                {
                    CoronaSpriteSetSequence seq = set.Sequences[j];

                    sb.AppendLine("\tsprite.add(" + set.Name + ",\"" + seq.Name + "\"," + seq.FrameDepart + "," + seq.FrameCount + "," + seq.SequenceLenght + "," + seq.Iteration + ")");
                }
            }

            return sb.ToString();

        }

        
        private void buildAllLayers(StringBuilder sb,float XRatio, float YRatio)
        {

            //Pour tous les layers de la scene
            for (int i = 0; i < this.Layers.Count; i++)
            {
                CoronaLayer layerParent = this.Layers[i];
                if (layerParent.isEnabled == true)
                {
                    string name = layerParent.Name.Replace(" ", "");

                    //Ajouter au content un Display group pour l'objet
                    sb.AppendLine("\t" + name + ".displayGroup = display.newGroup()");

                    //Ajouter le layer au display group de la scene
                    sb.AppendLine("\tlocalGroup:insert(" + name + ".displayGroup)");


                    layerParent.buildLayerToLua(sb, XRatio, YRatio);



                    layerParent.buildToLuaAllJointures(sb, XRatio, YRatio);

                    sb.AppendLine("\t" + this.Layers[i].Name + ".displayGroup:translate(" + ((int)((float)-this.Camera.SurfaceFocus.X * XRatio)).ToString().Replace(",", ".") + "," +
                                              ((int)((float)-this.Camera.SurfaceFocus.Y * YRatio)).ToString().Replace(",", ".") + ")");
                }
              
            }

        }


        public String buildWillEnterScene(float XRatio, float YRatio)
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("\t\tgroup:translate(" + ((int)((float)-this.SurfaceFocus.X * XRatio)).ToString().Replace(",", ".") + "," +
            //                               ((int)((float)-this.SurfaceFocus.Y * YRatio)).ToString().Replace(",", ".") + ")");

            sb.AppendLine("\tdisplay.setDefault(\"background\"," + this.DefaultColor.R + "," + this.DefaultColor.G + "," + this.DefaultColor.B + ",255)");
            if (this.StatusBarMode == 0)
                this.StatusBarMode = Scene.StatusBarDisplayMode.Hidden;

            if (this.StatusBarMode == StatusBarDisplayMode.Hidden)
                sb.AppendLine("\tdisplay.setStatusBar(display.HiddenStatusBar)");
            else if (this.StatusBarMode == StatusBarDisplayMode.Default)
                sb.AppendLine("\tdisplay.setStatusBar(display.DefaultStatusBar)");
            else if (this.StatusBarMode == StatusBarDisplayMode.Dark)
                sb.AppendLine("\tdisplay.setStatusBar(display.DarkStatusBar)");
            else if (this.StatusBarMode == StatusBarDisplayMode.Translucent)
                sb.AppendLine("\tdisplay.setStatusBar(display.TranslucentStatusBar)");

            if (this.isPhysicsActive == true)
            {
                sb.AppendLine("\tphysics.setDrawMode(\"" + this.PhysDrawMode + "\")");
                sb.AppendLine("\tphysics.setGravity(" + this.physicsXGravity.ToString().Replace(",", ".") +
                                "," + this.physicsYGravity.ToString().Replace(",", ".") + ")");

                sb.AppendLine("\tphysics.setScale(" + this.PhysicScale + ")");
                sb.AppendLine("\tphysics.setPositionIterations(" + this.PhysicPositionIterations + ")");
                sb.AppendLine("\tphysics.setVelocityIterations(" + this.PhysicVelocityIterations + ")");
            }

            if (this.isMultiTouchActive)
            {
                sb.AppendLine("\tsystem.activate(\"multitouch\")");
            }

            sb.AppendLine("\t---------- ACTIVATE SCENE: " + this.Name + " ----------");

            for (int i = 0; i < this.Layers.Count; i++)
            {
                CoronaLayer layerParent = this.Layers[i];
                if (layerParent.isEnabled == true)
                {
                    if (layerParent.TilesMap != null)
                    {
                        if (layerParent.TilesMap.isEnabled == true)
                        {
                            string tilesMapName = layerParent.TilesMap.TilesMapName.ToLower();
                            sb.AppendLine("\t-- Starting tilemap --");
                            sb.AppendLine("\t if(" + this.Name + "." + layerParent.Name + "." + tilesMapName + ") then  " + this.Name + "." + layerParent.Name + "." + tilesMapName + ":start() end");

                        }
                    }
                }
              
            }

            //Si le focus est defini sur un objet
            if (this.Camera.objectFocusedByCamera != null)
            {
                if (this.Camera.objectFocusedByCamera.isEnabled == true)
                {
                    String nameObj = this.Camera.objectFocusedByCamera.DisplayObject.Name;

                    sb.AppendLine("\n\t ------------ START FOCUS ON OBJECT : " + nameObj + " --------------");
                    sb.AppendLine("\t" + this.Name + "_camera:startAutoFocusOnObject(" + this.Camera.objectFocusedByCamera.LayerParent.Name + "." + nameObj + ")");
                }
            }
            else 
            {
                
                for (int i = 0; i < this.Layers.Count; i++)
                {
                    if(i == 0)
                    {
                         sb.AppendLine("\n\t ------------ START DRAG FOCUS --------------");
                         sb.AppendLine("\tlocal displayGroupsToMove = {}");
                    }
                      

                    CoronaLayer layerParent = this.Layers[i];
                    if (layerParent.isEnabled == true)
                    {
                        if (layerParent.isDraggableByCamera == true)
                        {
                            sb.AppendLine("\ttable.insert(displayGroupsToMove," + layerParent.Name + ".displayGroup)");

                        }
 
                    }

                    if (i == this.Layers.Count - 1)
                        sb.AppendLine("\t" + this.Name + "_camera:setDragEnabled(displayGroupsToMove)");
                }

             
            }

            for (int i = 0; i < this.Layers.Count; i++)
            {

                CoronaLayer layerParent = this.Layers[i];
                if (layerParent.isEnabled == true)
                {

                    //Mettre en pause les generateurs

                    for (int j = 0; j < layerParent.CoronaObjects.Count; j++)
                    {

                        CoronaObject obj = layerParent.CoronaObjects[j];
                        if (obj.isEnabled == true)
                        {
                            if (obj.isEntity == false)
                            {
                                string objName = obj.DisplayObject.Name.Replace(" ", "");

                                if (obj.isGenerator == true)
                                {
                                    if (obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object.isBodyActive = false");
                                    }

                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + ":pauseInteractions()");
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + ".isLocked = true");
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object.alpha = 0");
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator:startGeneration()");
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator:startObjectsInteractions()");
                                }
                                else
                                {
                                    if (obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object.isBodyActive = true");
                                    }

                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + ":startInteractions()");

                                }


                                //Creer les events associés pour l'editeur mobile
                                if (this.Name.Equals("mapeditormobile"))
                                {

                                    string otherAttribute = obj.otherAttribute;
                                    if (otherAttribute.Equals("LOAD"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",loadTilesMap)");
                                    }
                                    if (otherAttribute.Equals("SAVE"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",saveTilesMap)");
                                    }
                                    else if (otherAttribute.Equals("MODE_TEXTURE"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",setTargetTextures)");
                                    }
                                    else if (otherAttribute.Equals("MODE_OBJECT"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",setTargetObjects)");
                                    }
                                    else if (otherAttribute.Equals("MODE_APPLY"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",setModeApply)");
                                    }
                                    else if (otherAttribute.Equals("MODE_REMOVE"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",setModeRemove)");
                                    }
                                    else if (otherAttribute.Equals("MODE_SCROLLING"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",setModeScroll)");

                                    }
                                    else if (otherAttribute.Equals("MODE_EDITION"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",setModeEdition)");
                                    }
                                    else if (otherAttribute.Equals("MODE_COLLISIONS"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:addEventListener(\"tap\",setTargetCollisions)");
                                    }

                                }
                            }
                            else
                            {
                                for (int k = 0; k < obj.Entity.CoronaObjects.Count; k++)
                                {
                                    CoronaObject child = obj.Entity.CoronaObjects[k];
                                    if (child.isEnabled == true)
                                    {
                                        string objNameChild = child.DisplayObject.Name.Replace(" ", "");

                                        if (obj.isGenerator == true || child.isGenerator == true)
                                        {
                                            if (child.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object.isBodyActive = false");
                                            }

                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ":pauseInteractions()");
                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".isLocked = true");
                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object.alpha = 0");


                                        }

                                        if (child.isGenerator == true)
                                        {

                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + "_generator:startGeneration()");
                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + "_generator:startObjectsInteractions()");
                                        }
                                        else if (obj.isGenerator == false)
                                        {
                                            if (child.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object.isBodyActive = true");
                                            }

                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ":startInteractions()");

                                        }


                                        //Creer les events associés pour l'editeur mobile
                                        if (this.Name.Equals("mapeditormobile"))
                                        {

                                            string otherAttribute = child.otherAttribute;
                                            if (otherAttribute.Equals("LOAD"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",loadTilesMap)");
                                            }
                                            if (otherAttribute.Equals("SAVE"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",saveTilesMap)");
                                            }
                                            else if (otherAttribute.Equals("MODE_TEXTURE"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",setTargetTextures)");
                                            }
                                            else if (otherAttribute.Equals("MODE_OBJECT"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",setTargetObjects)");
                                            }
                                            else if (otherAttribute.Equals("MODE_APPLY"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",setModeApply)");
                                            }
                                            else if (otherAttribute.Equals("MODE_REMOVE"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",setModeRemove)");
                                            }
                                            else if (otherAttribute.Equals("MODE_SCROLLING"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",setModeScroll)");

                                            }
                                            else if (otherAttribute.Equals("MODE_EDITION"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",setModeEdition)");
                                            }
                                            else if (otherAttribute.Equals("MODE_COLLISIONS"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:addEventListener(\"tap\",setTargetCollisions)");
                                            }

                                        }
                                    }
                                }

                                if (obj.isGenerator == true)
                                {
                                    sb.AppendLine("\t" + layerParent.Name + "." + obj.Entity.Name + "_generator:startGeneration()");
                                }

                            }
                        }

                    }


                    for (int j = 0; j < layerParent.Controls.Count; j++)
                    {
                        CoronaControl control = layerParent.Controls[j];
                        if (control.isEnabled == true)
                        {
                            if (control.type == CoronaControl.ControlType.joystick)
                            {
                                JoystickControl joy = control as JoystickControl;
                                string controlName = joy.joystickName;
                                sb.AppendLine("\t" + layerParent.Name + "." + controlName + ".joystickStart()");
                            }
                        }
                       
                    }

                    //sb.AppendLine("\tif(isInit == false) then");
                    //sb.AppendLine("print(isInit)");
                    //sb.AppendLine("\t\tisInit = true");
                    //sb.AppendLine("\t\tgroup:translate(" + ((int)((float)-this.SurfaceFocus.X * XRatio)).ToString().Replace(",", ".") + "," +
                    //                        ((int)((float)-this.SurfaceFocus.Y * YRatio)).ToString().Replace(",", ".") + ")");
                    //sb.AppendLine("\tend");

                }
            }

             //Activé le FPS Si desiré
            if (this.IsFPSVisible == true)
            {
                sb.AppendLine("\tperformance:start()");

            }

            return sb.ToString();
        }
             
        public String buildDidExitScene()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\t---------- DEACTIVATE SCENE: " + this.Name + " ----------");
       
            for (int i = 0; i < this.Layers.Count; i++)
            {

                CoronaLayer layerParent = this.Layers[i];
                //Mettre en pause les generateurs
                if (layerParent.isEnabled == true)
                {
                    for (int j = 0; j < layerParent.CoronaObjects.Count; j++)
                    {
                        CoronaObject obj = layerParent.CoronaObjects[j];
                        if (obj.isEnabled == true)
                        {
                            if (obj.isEntity == false)
                            {
                                string objName = obj.DisplayObject.Name.Replace(" ", "");


                                if (obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                {
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object.isBodyActive = false");
                                }


                                if (obj.isGenerator == true)
                                {
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator:stopGeneration()");
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator:stopObjectsInteractions()");
                                }

                                sb.AppendLine("\t" + layerParent.Name + "." + objName + ":pauseInteractions()");

                                //Creer les events associés pour l'editeur mobile
                                if (this.Name.Equals("mapeditormobile"))
                                {

                                    string otherAttribute = obj.otherAttribute;
                                    if (otherAttribute.Equals("LOAD"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",loadTilesMap)");
                                    }
                                    if (otherAttribute.Equals("SAVE"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",saveTilesMap)");
                                    }
                                    else if (otherAttribute.Equals("MODE_TEXTURE"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",setTargetTextures)");
                                    }
                                    else if (otherAttribute.Equals("MODE_OBJECT"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",setTargetObjects)");
                                    }
                                    else if (otherAttribute.Equals("MODE_APPLY"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",setModeApply)");
                                    }
                                    else if (otherAttribute.Equals("MODE_REMOVE"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",setModeRemove)");
                                    }
                                    else if (otherAttribute.Equals("MODE_SCROLLING"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",setModeScroll)");

                                    }
                                    else if (otherAttribute.Equals("MODE_EDITION"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",setModeEdition)");
                                    }
                                    else if (otherAttribute.Equals("MODE_COLLISIONS"))
                                    {
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ".object:removeEventListener(\"tap\",setTargetCollisions)");
                                    }

                                }
                            }
                            else
                            {
                                for (int k = 0; k < obj.Entity.CoronaObjects.Count; k++)
                                {
                                    CoronaObject child = obj.Entity.CoronaObjects[k];
                                    if (child.isEnabled == true)
                                    {
                                        string objNameChild = child.DisplayObject.Name.Replace(" ", "");

                                        if (obj.isGenerator == true || child.isGenerator == true)
                                        {
                                            if (child.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object.isBodyActive = false");
                                            }

                                        }

                                        if (child.isGenerator == true)
                                        {
                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + "_generator:stopGeneration()");
                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + "_generator:stopObjectsInteractions()");
                                        }
                                        else if (obj.isGenerator == false)
                                        {
                                            sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ":pauseInteractions()");
                                        }


                                        //Creer les events associés pour l'editeur mobile
                                        if (this.Name.Equals("mapeditormobile"))
                                        {

                                            string otherAttribute = child.otherAttribute;
                                            if (otherAttribute.Equals("LOAD"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",loadTilesMap)");
                                            }
                                            if (otherAttribute.Equals("SAVE"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",saveTilesMap)");
                                            }
                                            else if (otherAttribute.Equals("MODE_TEXTURE"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",setTargetTextures)");
                                            }
                                            else if (otherAttribute.Equals("MODE_OBJECT"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",setTargetObjects)");
                                            }
                                            else if (otherAttribute.Equals("MODE_APPLY"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",setModeApply)");
                                            }
                                            else if (otherAttribute.Equals("MODE_REMOVE"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",setModeRemove)");
                                            }
                                            else if (otherAttribute.Equals("MODE_SCROLLING"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",setModeScroll)");

                                            }
                                            else if (otherAttribute.Equals("MODE_EDITION"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",setModeEdition)");
                                            }
                                            else if (otherAttribute.Equals("MODE_COLLISIONS"))
                                            {
                                                sb.AppendLine("\t" + layerParent.Name + "." + objNameChild + ".object:removeEventListener(\"tap\",setTargetCollisions)");
                                            }


                                        }
                                    }
                                }

                                if (obj.isGenerator == true)
                                {
                                    sb.AppendLine("\t" + layerParent.Name + "." + obj.Entity.Name + "_generator:stopGeneration()");
                                }

                            }
                        }

                    }




                    for (int j = 0; j < layerParent.Controls.Count; j++)
                    {
                        CoronaControl control = layerParent.Controls[j];
                        if (control.isEnabled == true)
                        {
                            if (control.type == CoronaControl.ControlType.joystick)
                            {
                                JoystickControl joy = control as JoystickControl;
                                string controlName = joy.joystickName;
                                sb.AppendLine("\t" + layerParent.Name + "." + controlName + ".joystickStop()");
                            }
                        }
                    }
                }

            }

          //Si le focus est defini sur un objet
            if (this.Camera.objectFocusedByCamera != null)
            {
                if (this.Camera.objectFocusedByCamera.isEnabled == true)
                {
                    String nameObj = this.Camera.objectFocusedByCamera.DisplayObject.Name;

                    sb.AppendLine("\n\t ------------ STOP AUTO FOCUS ON OBJECT : " + nameObj + " --------------");
                    sb.AppendLine("\t" + this.Name + "_camera:stopAutoFocus()");
                }
            }
            else
            {
                 sb.AppendLine("\n\t ------------ STOP DRAG FOCUS --------------");
                sb.AppendLine("\t"+this.Name+"_camera:setDragDisable()");
            }

            for (int i = 0; i < this.Layers.Count; i++)
            {

                CoronaLayer layerParent = this.Layers[i];
                if (layerParent.isEnabled == true)
                {
                    //Desactiver les tilesmap
                    if (layerParent.TilesMap != null)
                    {
                        if (layerParent.TilesMap.isEnabled == true)
                        {
                            string tilesMapName = layerParent.TilesMap.TilesMapName.ToLower();
                            sb.AppendLine("\t-- Pausing tilemap --");
                            sb.AppendLine("\t if(" + layerParent.Name + "." + tilesMapName + ") then  " + layerParent.Name + "." + tilesMapName + ":pause() end");
                        }
                    }
                }
               
            }


            //Activé le FPS Si desiré
            if (this.IsFPSVisible == true)
            {
                sb.AppendLine("\tperformance:stop()");

            }

            return sb.ToString();

        }

        public String buildCleanScene()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\t---------- CLEAN SCENE: " + this.Name + " ----------");
            //Si le focus est defini sur un objet
            if (this.Camera.objectFocusedByCamera != null)
            {
                if (this.Camera.objectFocusedByCamera.isEnabled == true)
                {
                    String nameObj = this.Camera.objectFocusedByCamera.DisplayObject.Name;

                    //Creer une function
                    sb.AppendLine("\tRuntime:removeEventListener(\"enterFrame\",onObject" + nameObj + "_Move)");
                }
            }


            for (int i = 0; i < this.Layers.Count; i++)
            {

                CoronaLayer layerParent = this.Layers[i];
                if (layerParent.isEnabled == true)
                {
                    //Supprimer la tilesMap du layer
                    TilesMap map = layerParent.TilesMap;
                    if (map != null)
                    {
                        if (map.isEnabled == true)
                        {
                            sb.AppendLine("\t-- Cleaning tilemap --");
                            sb.AppendLine("\t" + layerParent.Name + "." + map.TilesMapName + ":clean()");
                        }
                    }

                    //Supprimer tous les controls si existant

                    for (int j = 0; j < layerParent.Controls.Count; j++)
                    {
                        CoronaControl control = layerParent.Controls[j];
                        if (control.isEnabled == true)
                        {
                            if (control.type == CoronaControl.ControlType.joystick)
                            {
                                JoystickControl joy = control as JoystickControl;
                                string controlName = joy.joystickName;
                                sb.AppendLine("\t" + layerParent.Name + "." + controlName + ".joystickStop()");
                                sb.AppendLine("\t" + layerParent.Name + "." + controlName + ":removeSelf()");
                                sb.AppendLine("\t" + layerParent.Name + "." + controlName + " = nil");
                                sb.AppendLine("\t" + layerParent.Name + "." + controlName + "_onMove = nil");
                            }

                        }
                    }

                    //Supprimer tous les objets
                    for (int j = 0; j < layerParent.CoronaObjects.Count; j++)
                    {
                        if (layerParent.CoronaObjects[j].isEnabled == true)
                        {
                            if (layerParent.CoronaObjects[j].isEntity == false)
                            {
                                DisplayObject obj = layerParent.CoronaObjects[j].DisplayObject;

                                string objName = obj.Name.Replace(" ", "");

                                if (obj.CoronaObjectParent.isGenerator == true)
                                {
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator:clean()");
                                    sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator = nil");
                                }
                                sb.AppendLine("\t" + layerParent.Name + "." + objName + ":clean()");
                                sb.AppendLine("\t" + layerParent.Name + "." + objName + " = nil");
                            }
                            else
                            {
                                for (int k = 0; k < layerParent.CoronaObjects[j].Entity.CoronaObjects.Count; k++)
                                {
                                    if (layerParent.CoronaObjects[j].Entity.CoronaObjects[k].isEnabled == true)
                                    {
                                        DisplayObject obj = layerParent.CoronaObjects[j].Entity.CoronaObjects[k].DisplayObject;

                                        string objName = obj.Name.Replace(" ", "");

                                        if (obj.CoronaObjectParent.isGenerator == true)
                                        {
                                            sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator:clean()");
                                            sb.AppendLine("\t" + layerParent.Name + "." + objName + "_generator = nil");
                                        }
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + ":clean()");
                                        sb.AppendLine("\t" + layerParent.Name + "." + objName + " = nil");
                                    }
                                }

                            }
                        }


                    }
                }

            }

            sb.AppendLine("\t" + "collectgarbage(\"collect\")");
            return sb.ToString();
        }
    }
   
}