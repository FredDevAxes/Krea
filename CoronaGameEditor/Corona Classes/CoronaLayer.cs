using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Krea.GameEditor.TilesMapping;
using System.Drawing.Drawing2D;
using Krea.Corona_Classes.Controls;
using Krea.Corona_Classes.Widgets;
using Krea.CGE_Figures;
using Krea.Corona_Classes;
using System.IO;
using System.Windows.Forms;



namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaLayer
    {
        

        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        public int IndexLayer;
        public List<CoronaObject> CoronaObjects;
        public List<CoronaJointure> Jointures;
        public List<CoronaControl> Controls;
        public List<CoronaWidget> Widgets;
        public List<CoronaEntity> Entities;

        public TilesMap TilesMap;
        public String Name;

        public CoronaJointure JointureSelected;

        public Scene SceneParent;
        public bool isDraggableByCamera = false;
        public bool isEnabled = true;
        [NonSerialized()]
        public Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public CoronaLayer(int index,Scene sceneParent)
        {
            SceneParent = sceneParent;

            this.IndexLayer = index;

            this.Name = "Layer" + sceneParent.Layers.Count;
            this.Name = this.SceneParent.projectParent.IncrementObjectName(this.Name);
            this.CoronaObjects = new List<CoronaObject>();
            this.Controls = new List<CoronaControl>();
            this.Jointures = new List<CoronaJointure>();
            this.Widgets = new List<CoronaWidget>();
            this.Entities = new List<CoronaEntity>();

        }

        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        public void addCoronaObject(CoronaObject obj,bool incrementName)
        {
            obj.setSelected(false);
            obj.LayerParent = this;

            if (incrementName == true)
            {
                if (obj.isEntity == false)
                {
                    string objName = this.SceneParent.projectParent.IncrementObjectName(obj.DisplayObject.Name);
                    obj.DisplayObject.Name = objName;
                }
                else
                {
                    string objName = this.SceneParent.projectParent.IncrementObjectName(obj.Entity.Name);
                    obj.Entity.Name = objName;
                }
            }
           

            this.CoronaObjects.Add(obj);
            
        }

        

        
        public void newTilesMap()
        {
            //Count all tile maps in the project
            int count = 0;
            for (int i = 0; i < this.SceneParent.projectParent.Scenes.Count; i++)
            {
                for (int j = 0; j < this.SceneParent.projectParent.Scenes[i].Layers.Count; j++)
                {
                    if (this.SceneParent.projectParent.Scenes[i].Layers[j].TilesMap != null)
                    {
                        count++;
                    }
                }
            }

            this.TilesMap = new TilesMap("map" + count, new Point(0, 0), 10, 10, 64, 64,this);

        }

        public CoronaObject newEntity()
        {
            //Creer le corona object parent
            CoronaObject objectEntity = new CoronaObject(true);
            this.addCoronaObject(objectEntity, true);

            return objectEntity;
        }

        public void removeCoronaObject(CoronaObject obj)
        {
            if (obj != null)
            {
                this.CoronaObjects.Remove(obj);

                if (this.SceneParent.Camera.objectFocusedByCamera == obj)
                    this.SceneParent.Camera.objectFocusedByCamera = null;

                obj = null;

                
            }
        }

    
        public void showAllObjects()
        {
            if (this.CoronaObjects != null)
            {
                for (int i = 0;i<this.CoronaObjects.Count;i++)
                {
                    CoronaObject obj = this.CoronaObjects[i];
                    obj.isVisible = true;
                }

            }
        }

        public void hideAllObjects()
        {
            if (this.CoronaObjects != null)
            {
                for (int i = 0; i < this.CoronaObjects.Count; i++)
                {
                    CoronaObject obj = this.CoronaObjects[i];
                    obj.isVisible = false;
                }

            }
        }

        public void dessineGorgonLayer(float scaleX, float scaleY, Point offsetPoint)
        {
            //Dessiner la tilesMap si elle existe
            if (this.TilesMap != null)
            {
                if (!GorgonLibrary.Gorgon.CurrentRenderTarget.Name.Equals("ScenePreview"))
                    if (this.TilesMap.isEnabled == true)
                    {
                        //Point offSetWithCameraLocation = new Point(offsetPoint.X + this.SceneParent.SurfaceFocus.Location.X,
                        //                                    offsetPoint.Y + this.SceneParent.SurfaceFocus.Location.Y);
                        this.TilesMap.DrawGorgon(scaleX, scaleY, offsetPoint, "ALL", false);
                    
                    }

            }

            for (int i = 0; i < this.CoronaObjects.Count; i++)
            {
                CoronaObject obj = this.CoronaObjects[i];

                if (obj.isEntity == false)
                {
                    if (obj.isEnabled == true)
                    {

                        obj.DisplayObject.DrawGorgon(offsetPoint, true, scaleX, scaleY,true);
                        if (this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.hybrid || this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.debug)
                        {
                            if (this.CoronaObjects[i].PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                obj.PhysicsBody.drawGorgonBodyElements(Color.FromArgb(150, Color.GreenYellow), true, offsetPoint, scaleX,true);

                        }
                    }
                }
                else
                {
                    if (obj.isEnabled == true)
                    {
                        for (int j = 0; j < obj.Entity.CoronaObjects.Count; j++)
                        {
                            CoronaObject child = obj.Entity.CoronaObjects[j];
                            if (child.isEnabled == true)
                            {
                                child.DisplayObject.DrawGorgon(offsetPoint, true, scaleX, scaleY, true);
                                
                                if (this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.hybrid || this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.debug)
                                {
                                    if (child.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                    {
                                        child.PhysicsBody.drawGorgonBodyElements(Color.FromArgb(150, Color.GreenYellow), true, offsetPoint, scaleX,true);

                                    }

                                }
                            }

                        }


                        if (obj.isSelected == true)
                        {
                            
                            if (obj.TransformBox != null)
                                obj.TransformBox.drawGorgon(offsetPoint, scaleX);

                        }
                    }
                }
            }

            //Dessiner les controls
            for (int i = 0; i < this.Controls.Count; i++)
            {
                CoronaControl control = this.Controls[i];
                if (control.isEnabled == true)
                {
                  
                    control.DrawGorgon(offsetPoint, scaleX);

                    if (control.type == CoronaControl.ControlType.joystick)
                    {
                        JoystickControl joy = control as JoystickControl;
                        if (joy.isSelected == true)
                        {
                            Rectangle surface = new Rectangle(joy.joystickLocation.X + offsetPoint.X, joy.joystickLocation.Y + offsetPoint.Y,
                                    joy.outerRadius * 2, joy.outerRadius * 2);

                            GorgonGraphicsHelper.Instance.DrawSelectionBox(surface, 2, Color.YellowGreen, 2, 2, scaleX);
                        }
                    }
                }

            }

            ////Dessiner les widgets
            //for (int i = 0; i < this.Widgets.Count; i++)
            //{
            //    CoronaWidget widget = this.Widgets[i];
            //    widget.Dessine(g, offsetPoint, scaleX, scaleY);
            //    g.ResetTransform();
            //}


          

            if (this.JointureSelected != null)
            {
                if (this.JointureSelected.isEnabled == true)
                    this.JointureSelected.drawGorgon(offsetPoint, scaleX);
            }

           
        }

        public void dessineLayer(Graphics g,float scaleX,float scaleY,Point offsetPoint)
        {
             //Dessiner la tilesMap si elle existe
            if (this.TilesMap != null)
            {
                if (this.TilesMap.isEnabled == true)
                {
                    //Point offSetWithCameraLocation = new Point(offsetPoint.X + this.SceneParent.SurfaceFocus.Location.X,
                    //                                    offsetPoint.Y + this.SceneParent.SurfaceFocus.Location.Y);


                    bool res = this.TilesMap.DrawTilesInEditor(g, scaleX, scaleY, offsetPoint, "ALL", false);
                    if (res == false)
                    {
                        this.TilesMap.createTilesTab();
                    }
                }
               
            }

             //Dessiner tous les objets
              float[] dashValues = { 2, 2 };
                    Pen penSelection = new Pen(Color.FromArgb(150, Color.Blue), 3);
                    penSelection.DashPattern = dashValues;

            for (int i = 0; i < this.CoronaObjects.Count; i++)
            {
                CoronaObject obj = this.CoronaObjects[i];
                
                if (obj.isEntity == false)
                {
                    if (obj.isEnabled == true)
                    {
                        obj.DisplayObject.dessineAt(g, offsetPoint, true, null, scaleX, scaleY);

                        if (this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.hybrid || this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.debug)
                        {
                            if (this.CoronaObjects[i].PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                            {
                                SolidBrush br = new SolidBrush(Color.FromArgb(150, Color.GreenYellow));


                                obj.PhysicsBody.dessineAllBodyELements(g, br, true, offsetPoint);

                            }

                        }
                    }
                }
                else
                {
                    if (obj.isEnabled == true)
                    {
                        for (int j = 0; j < obj.Entity.CoronaObjects.Count; j++)
                        {
                            CoronaObject child = obj.Entity.CoronaObjects[j];
                            if (child.isEnabled == true)
                            {
                                child.DisplayObject.dessineAt(g, offsetPoint, true, null, scaleX, scaleY);

                                if (this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.hybrid || this.SceneParent.PhysDrawMode == Scene.PhysicsDrawMode.debug)
                                {
                                    if (child.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                    {
                                        SolidBrush br = new SolidBrush(Color.FromArgb(150, Color.GreenYellow));
                                        child.PhysicsBody.dessineAllBodyELements(g, br, true, offsetPoint);

                                    }

                                }
                            }
                           
                        }


                        if (obj.isSelected == true)
                        {
                            Matrix matrixPath = new Matrix();
                            matrixPath.Scale(scaleX, scaleY);
                            g.Transform = matrixPath;

                            if (obj.TransformBox != null)
                                obj.TransformBox.drawTransformBox(g, offsetPoint,scaleX,scaleY);

                        }
                    }
                    
                }
                
                g.ResetTransform();
            }

            Matrix m = new Matrix();
            m.Scale(scaleX, scaleY);
           
            //Dessiner les controls
            for (int i = 0; i < this.Controls.Count; i++)
            {
                CoronaControl control = this.Controls[i];
                if (control.isEnabled == true)
                {
                    control.Dessine(g, offsetPoint, scaleX, scaleY);


                    if (control.type == CoronaControl.ControlType.joystick)
                    {
                        JoystickControl joy = control as JoystickControl;
                        if (joy.isSelected == true)
                        {
                            Pen pen = new Pen(Brushes.Blue, 3);
                            Rectangle surface = new Rectangle(joy.joystickLocation.X + offsetPoint.X, joy.joystickLocation.Y + offsetPoint.Y,
                                    joy.outerRadius * 2, joy.outerRadius * 2);

                            g.ResetTransform();
                            g.Transform = m;
                            g.DrawRectangle(pen, surface);
                        }
                    }
                }
                g.ResetTransform();
            }

            //Dessiner les widgets
            for (int i = 0; i < this.Widgets.Count; i++)
            {
                CoronaWidget widget = this.Widgets[i];
                widget.Dessine(g, offsetPoint, scaleX, scaleY);
                g.ResetTransform();
            }

           
            g.Transform = m;

            if (this.JointureSelected != null)
            {
                if(this.JointureSelected.isEnabled == true)
                    this.JointureSelected.dessineJointure(g, offsetPoint);
            }

            g.ResetTransform();

        }


        public CoronaObject getObjTouched(Point p)
        {
            for (int i = this.CoronaObjects.Count -1; i>=0 ; i--)
            {
                if (this.CoronaObjects[i].isEnabled == true)
                {
                    if (this.CoronaObjects[i].isEntity == false)
                    {
                        if (this.CoronaObjects[i].DisplayObject.containsPoint(p) == true)
                            return this.CoronaObjects[i];
                    }
                    else
                    {
                        CoronaEntity entity = this.CoronaObjects[i].Entity;
                        for (int j = entity.CoronaObjects.Count - 1; j >= 0; j--)
                        {
                            if (entity.CoronaObjects[j].isEnabled == true)
                            {
                                if (entity.CoronaObjects[j].DisplayObject.containsPoint(p) == true)
                                    return entity.CoronaObjects[j];
                            }
                        }


                    }
                }
                
            }
            return null;
        }

        public CoronaObject getEntityTouched(Point p)
        {
            for (int i = this.CoronaObjects.Count - 1; i >= 0; i--)
            {
                if (this.CoronaObjects[i].isEnabled == true)
                {
                    if (this.CoronaObjects[i].isEntity == true)
                    {
                        CoronaEntity entity = this.CoronaObjects[i].Entity;
                        for (int j = entity.CoronaObjects.Count - 1; j >= 0; j--)
                        {
                            if (entity.CoronaObjects[j].isEnabled == true)
                            {
                                if (entity.CoronaObjects[j].DisplayObject.containsPoint(p) == true)
                                    return this.CoronaObjects[i];
                            }
                        }
                    }
                }
                
            }
            return null;
        }
 
        public JoystickControl getJoystickTouched(Point p)
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                if (this.Controls[i].type == CoronaControl.ControlType.joystick)
                {
                    JoystickControl joy = this.Controls[i] as JoystickControl;
                    if (joy.isEnabled == true)
                    {
                        Rectangle surfaceJoy = new Rectangle(joy.joystickLocation, new Size(joy.outerRadius * 2, joy.outerRadius * 2));
                        if (surfaceJoy.Contains(p) == true)
                            return joy;
                    }
                }
               
            }
            return null;
        }

        public void deselectAllObjects()
        {
            for (int i = 0; i < this.CoronaObjects.Count; i++)
            {
                this.CoronaObjects[i].setSelected(false);

                if (this.CoronaObjects[i].isEntity == true)
                {
                    for (int k = 0; k < this.CoronaObjects[i].Entity.CoronaObjects.Count; k++)
                    {
                        this.CoronaObjects[i].Entity.CoronaObjects[k].isSelected = false;
                    }
                }
            }

        }

        public void deselectAllControls()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].type == CoronaControl.ControlType.joystick)
                {
                    JoystickControl joy = this.Controls[i] as JoystickControl;
                    joy.setSelected(false);
                }
              
            }
        }


        public void moveObjects(List<CoronaObject> list, Point p)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].DisplayObject.move(p);
            }
        }


        public void ScaleLayer(float scale)
        {

            //Repercuter le scale sur tout les corona object du layer
            for (int i = 0; i < this.CoronaObjects.Count; i++)
            {
                this.CoronaObjects[i].DisplayObject.SetScale(scale);
            }

        }

        public CoronaLayer Clone()
        {
            try
            {
                CoronaLayer newLayer = new CoronaLayer(this.SceneParent.Layers.Count,this.SceneParent);

                //Clone all Objects
                for (int i = 0;i<this.CoronaObjects.Count;i++)
                {
                    CoronaObject currentObj = this.CoronaObjects[i];
                    CoronaObject newObj = currentObj.cloneObject(newLayer,true,true);
                }

                //Clone all Joints
                for (int i = 0;i<this.Jointures.Count;i++)
                {
                    CoronaJointure currentJoint = this.Jointures[i];

                    //FindCoronaObjects
                    string objNameA = currentJoint.coronaObjA.DisplayObject.Name;
                    string objNameB ="";
                    CoronaObject objA = null;
                    CoronaObject objB = null;
                    if(currentJoint.coronaObjB != null)
                    {
                        objNameB=currentJoint.coronaObjB.DisplayObject.Name;
                    }

                    for (int j = 0; j < newLayer.CoronaObjects.Count; j++)
                    {
                        CoronaObject currentObj = newLayer.CoronaObjects[j];
                        if(currentObj.DisplayObject.Name.Equals(objNameA))
                            objA = currentObj;
                        else if(currentObj.DisplayObject.Name.Equals(objNameB))
                            objB = currentObj;
                    }

                    CoronaJointure newJoint = currentJoint.clone(objA,objB,newLayer);
                    newLayer.Jointures.Add(newJoint);
                }

                //Clone all controls
                for (int i = 0;i<this.Controls.Count;i++)
                {
                    CoronaControl currentControl = this.Controls[i];
                    CoronaControl newControl = currentControl.Clone(newLayer,false);
                    newLayer.Controls.Add(newControl);
                }

                //-------------- WARNING: CLONE WIDGETS TO DO ----------------------------
                //-------------- WARNING: CLONE TILESMAP TO DO ----------------------------

                return newLayer;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
            
        }


        public void buildLayerToLua(StringBuilder sb,float XRatio, float YRatio)
        {
           
            this.buildAllObjects(sb, XRatio, YRatio);
            //this.buildToLuaAllJointures(sb, XRatio, YRatio);

        }

        private void buildAllObjects(StringBuilder sb, float XRatio, float YRatio)
        {
            float moyenneRatio = (XRatio + YRatio) / 2;

            //Creer la tilesMap du layer
            TilesMap map = this.TilesMap;
            if (map != null)
            {
                if (map.isEnabled == true)
                {
                    string folderDest = this.SceneParent.projectParent.BuildFolderPath;

                    //Creer le tableaux des evenements
                    sb.AppendLine("\tlocal events = {}");
                    for (int i = 0; i < map.TileEvents.Count; i++)
                    {
                        TileEvent ev = map.TileEvents[i];
                        sb.AppendLine("\tevents[" + (i + 1) + "] = {name = \"" + ev.Name + "\",type = \"" + ev.Type.ToString() + "\",listener =" + ev.Name + "}");

                    }
                    string paramsName = "params_" + map.TilesMapName;
                    sb.AppendLine("\tlocal " + paramsName + " = {}");

                    //------------OLD VERSION :SINCE Corona version 2012.840 ---------------------
                    String paramsTilesMap = "\t" + paramsName + ".xPos = " + ((int)((float)(this.SceneParent.Camera.SurfaceFocus.X) * XRatio)).ToString().Replace(",", ".") + "\n" +
                                                 "\t" + paramsName + ".name = \"" + map.TilesMapName.ToLower() + "\"\n" +
                                                 "\t" + paramsName + ".isInfinite = " + map.isInfinite.ToString().ToLower() + "\n" +
                                                 "\t" + paramsName + ".events = events\n" +
                                                 "\t" + paramsName + ".dynamicScale = 1/currentSuffix.value\n" +
                                                 "\t" + paramsName + ".scaleSuffix = currentSuffix.suffix\n" +
                                                   "\t" + paramsName + ".yPos = " + ((int)((float)(this.SceneParent.Camera.SurfaceFocus.Y) * YRatio)).ToString().Replace(",", ".") + "\n" +
                                                   "\t" + paramsName + ".xScroll = " + ((int)((float)(this.SceneParent.Camera.SurfaceFocus.X) * XRatio)).ToString().Replace(",", ".") + "\n" +
                                                   "\t" + paramsName + ".yScroll = " + ((int)((float)(this.SceneParent.Camera.SurfaceFocus.Y) * YRatio)).ToString().Replace(",", ".") + "\n" +
                                                  "\t" + paramsName + ".displayGroup= " + this.Name + ".displayGroup\n";

                    //String paramsTilesMap = "\t" + paramsName + ".xPos = " + ((int)((float)(this.SceneParent.SurfaceFocus.X) * XRatio)).ToString().Replace(",", ".") + "\n" +
                    //                            "\t" + paramsName + ".name = \"" + map.TilesMapName.ToLower() + "\"\n" +
                    //                            "\t" + paramsName + ".isInfinite = " + map.isInfinite.ToString().ToLower() + "\n" +
                    //                            "\t" + paramsName + ".events = events\n" +
                    //                              "\t" + paramsName + ".yPos = " + ((int)((float)(this.SceneParent.SurfaceFocus.Y) * YRatio)).ToString().Replace(",", ".") + "\n" +
                    //                              "\t" + paramsName + ".xScroll = " + ((int)((float)(this.SceneParent.SurfaceFocus.X) * XRatio)).ToString().Replace(",", ".") + "\n" +
                    //                              "\t" + paramsName + ".yScroll = " + ((int)((float)(this.SceneParent.SurfaceFocus.Y) * YRatio)).ToString().Replace(",", ".") + "\n" +
                    //                             "\t" + paramsName + ".displayGroup= " + this.Name + ".displayGroup\n";


                    //String paramsTilesMap = "\t" + paramsName + ".xPos = " + ((int)((float)(map.Location.X) * XRatio)).ToString().Replace(",", ".") + "\n" +
                    //                             "\t" + paramsName + ".name = \"" + map.TilesMapName.ToLower() + "\"\n" +
                    //                             "\t" + paramsName + ".isInfinite = " + map.isInfinite.ToString().ToLower() + "\n" +
                    //                               "\t" + paramsName + ".yPos = " + ((int)((float)(map.Location.Y) * YRatio)).ToString().Replace(",", ".") + "\n" +
                    //                              "\t" + paramsName + ".displayGroup= " + this.Name + ".displayGroup\n";
                    sb.Append(paramsTilesMap);

                    String createTilesMap = "\t" + this.Name + "." + map.TilesMapName +
                                        " = tilesMapInstance.TilesMap.create(" + paramsName + ")";

                    sb.AppendLine("\t" + createTilesMap);

                }
            }

            //Creer tous les objets
            for (int j = 0; j < this.CoronaObjects.Count; j++)
            {
                CoronaObject objectToBuild = this.CoronaObjects[j];
                if (objectToBuild.isEnabled == true)
                {
                    if (objectToBuild.isEntity == false)
                    {
                        this.buildObjectToLua(sb, XRatio, YRatio, objectToBuild);
                    }
                    else
                    {
                        sb.AppendLine("\tlocal paramsEntity_" + objectToBuild.Entity.Name + " = {}");
                        sb.AppendLine("\tparamsEntity_" + objectToBuild.Entity.Name + ".name = \"" + objectToBuild.Entity.Name + "\"");

                        sb.AppendLine("\t" + this.Name.Replace(" ", "") + "." + objectToBuild.Entity.Name + "= entityInstance.Entity.create(paramsEntity_" + objectToBuild.Entity.Name + ")");

                        for (int l = 0; l < objectToBuild.Entity.CoronaObjects.Count; l++)
                        {
                            CoronaObject childToBuild = objectToBuild.Entity.CoronaObjects[l];
                            if (childToBuild.isEnabled == true)
                            {
                                if (childToBuild.isEntity == false)
                                {
                                    this.buildObjectToLua(sb, XRatio, YRatio, childToBuild);

                                    string objName = childToBuild.DisplayObject.Name.Replace(" ", "");
                                    sb.AppendLine("\t" + this.Name.Replace(" ", "") + "." + objectToBuild.Entity.Name + ":addObject(" + this.Name + "." + objName + ")");
                                }
                            }
                        }


                    }
                }
            }

            //Gerer les controls
            for (int j = 0; j < this.Controls.Count; j++)
            {
                CoronaControl control = this.Controls[j];
                if (control.isEnabled == true)
                {
                    if (control.type == CoronaControl.ControlType.joystick)
                    {
                        JoystickControl joy = control as JoystickControl;
                        string luaJoystick = joy.buildLuaCode(XRatio, YRatio);
                        sb.Append(luaJoystick);

                        sb.AppendLine("\t" + this.Name.Replace(" ", "") + ".displayGroup:insert(" + this.Name + "." + joy.joystickName + ")");
                    }
                }
               
            }

            //Gerer les Widgets
            for (int j = 0; j < this.Widgets.Count; j++)
            {
                CoronaWidget widget = this.Widgets[j];
                //For the TABBAR-----------------
                if (widget.Type == CoronaWidget.WidgetType.tabBar)
                {
                    WidgetTabBar tabBar = (WidgetTabBar)widget;
                    //Creer un tableau de buttons
                    sb.AppendLine("\tlocal tabButtons_" + tabBar.Name + " = {");

                    for (int e = 0; e < tabBar.Buttons.Count; e++)
                    {
                        WidgetTabBar.TabBarButton bt = tabBar.Buttons[e];
                        sb.AppendLine("\t{");
                        sb.AppendLine("\t\t id = \"" + bt.Id + "\",");
                        sb.AppendLine("\t\t label = \"" + bt.Label + "\",");
                        sb.AppendLine("\t\t up = \"" + tabBar.Name + "" + bt.Id + "up.png\",");
                        sb.AppendLine("\t\t down = \"" + tabBar.Name + "" + bt.Id + "down.png\",");
                        sb.AppendLine("\t\t upWidth = " + ((float)bt.ImageUnPressedState.Size.Width * XRatio).ToString().Replace(",", ".") + ",");
                        sb.AppendLine("\t\t upHeight = " + ((float)bt.ImageUnPressedState.Size.Height * YRatio).ToString().Replace(",", ".") + ",");
                        sb.AppendLine("\t\t downWidth = " + ((float)bt.ImagePressedState.Size.Width * XRatio).ToString().Replace(",", ".") + ",");
                        sb.AppendLine("\t\t downHeight = " + ((float)bt.ImagePressedState.Size.Height * YRatio).ToString().Replace(",", ".") + ",");
                        sb.AppendLine("\t\t size = " + bt.FontSize + ",");

                        if (!bt.FontName.Equals(""))
                            sb.AppendLine("\t\t font = \"" + bt.FontName + "\",");

                        sb.AppendLine("\t\t labelColor = {" + bt.LabelColor.R + "," + bt.LabelColor.G + "," + bt.LabelColor.B + "," + bt.LabelAlpha + "},");
                        sb.AppendLine("\t\t cornerRadius = " + ((float)bt.CornerRadius * XRatio).ToString().Replace(",", ".") + ",");
                        sb.AppendLine("\t\t selected = " + bt.Selected.ToString().ToLower() + ",");
                        sb.AppendLine("\t\t onPress = " + widget.Name + "_onBtPress");
                        sb.AppendLine("\t},");
                    }
                    sb.AppendLine("\t}");

                    //Creer le gradient si existant
                    if (tabBar.GradientForTup.isEnabled == true)
                    {
                        //Create a gradient object
                        string color1 = "{" + tabBar.GradientForTup.color1.R + ","
                            + tabBar.GradientForTup.color1.G + ","
                            + tabBar.GradientForTup.color1.B + "}";

                        string color2 = "{" + tabBar.GradientForTup.color2.R + ","
                            + tabBar.GradientForTup.color2.G + ","
                            + tabBar.GradientForTup.color2.B + "}";

                        string direction = "\"" + tabBar.GradientForTup.direction.ToString() + "\"";
                        string gradient = color1 + "," + color2 + "," + direction;
                        sb.AppendLine("\tlocal " + tabBar.Name + "_gradient = graphics.newGradient(" + gradient + ")");

                    }

                    //Creer les parametres
                    sb.AppendLine("\t" + this.Name + "" + tabBar.Name + " = widget.newTabBar{");
                    sb.AppendLine("\t\t width = " + ((float)tabBar.Size.Width * XRatio).ToString().Replace(",", ".") + ",");
                    sb.AppendLine("\t\t height = " + ((float)tabBar.Size.Height * YRatio).ToString().Replace(",", ".") + ",");
                    sb.AppendLine("\t\t left = " + ((float)tabBar.Location.X * XRatio).ToString().Replace(",", ".") + ",");
                    sb.AppendLine("\t\t top = " + ((float)tabBar.Location.Y * YRatio).ToString().Replace(",", ".") + ",");

                    /*if (tabBar.BackgroundImage != null)
                      sb.AppendLine("\t background = \"" + tabBar.Name + "background.png\",");
                    else*/
                    if (tabBar.GradientForTup.isEnabled == true)
                    {
                        sb.AppendLine("\t\t topGradient = " + tabBar.Name + "_gradient,");
                        sb.AppendLine("\t\t bottomFill = {" + tabBar.BottomFillColor.R + "," + tabBar.BottomFillColor.G + ","
                            + tabBar.BottomFillColor.B + ","
                            + tabBar.BottomFillAlpha + "},");

                    }
                    else
                    {
                        sb.AppendLine("\t\t bottomFill = {" + tabBar.BottomFillColor.R + "," + tabBar.BottomFillColor.G + ","
                            + tabBar.BottomFillColor.B + ","
                            + tabBar.BottomFillAlpha + "},");
                    }

                    sb.AppendLine("\t\t buttons = tabButtons_" + tabBar.Name);
                    sb.AppendLine("\t}");


                    sb.AppendLine("\t" + this.Name + ".displayGroup:insert(" + this.Name + "." + tabBar.Name + ".view)");
                }

                //For the PICKERWHEEL-----------------
                else if (widget.Type == CoronaWidget.WidgetType.pickerWheel)
                {
                    WidgetPickerWheel pickerW = (WidgetPickerWheel)widget;

                    //Create the column data
                    sb.AppendLine("\tlocal " + widget.Name + "_columnsData = {}");

                    for (int e = 0; e < pickerW.Columns.Count; e++)
                    {
                        //Create rows 
                        string rows = "{";
                        for (int h = 0; h < pickerW.Columns[e].Datas.Count; h++)
                        {
                            rows += "\"" + pickerW.Columns[e].Datas[h] + "\"";

                            if (h < pickerW.Columns[e].Datas.Count - 1)
                                rows += ",";
                        }
                        rows += "}";

                        sb.AppendLine("\t" + widget.Name + "_columnsData[" + (e + 1) + "] = " + rows);

                        if (pickerW.Columns[e].Alignement != WidgetPickerWheel.PickerWheelColumn.ColumnAlignement.left)
                            sb.AppendLine("\t" + widget.Name + "_columnsData[" + (e + 1) + "].alignement =\"" + pickerW.Columns[e].Alignement.ToString() + "\"");
                        if (pickerW.Columns[e].Width != 999)
                            sb.AppendLine("\t" + widget.Name + "_columnsData[" + (e + 1) + "].width = " + pickerW.Columns[e].Width);

                        sb.AppendLine("\t" + widget.Name + "_columnsData[" + (e + 1) + "].startIndex = " + pickerW.Columns[e].StartIndex);

                        sb.AppendLine("\n");

                    }

                    //Create the PickerWheel
                    sb.AppendLine("\t" + this.Name + "" + widget.Name + " = widget.newPickerWheel{");
                    sb.AppendLine("\t\t id = \"" + widget.Name + "\",");
                    sb.AppendLine("\t\t width = " + ((float)pickerW.Size.Width * XRatio).ToString().Replace(",", ".") + ",");
                    sb.AppendLine("\t\t height = " + ((float)pickerW.Size.Height * YRatio).ToString().Replace(",", ".") + ",");

                    if (pickerW.TotalWidth > 0)
                        sb.AppendLine("\t\t totalWidth = " + ((float)pickerW.TotalWidth * XRatio).ToString().Replace(",", ".") + ",");

                    sb.AppendLine("\t\t left = " + ((float)pickerW.Location.X * XRatio).ToString().Replace(",", ".") + ",");
                    sb.AppendLine("\t\t top = " + ((float)pickerW.Location.Y * YRatio).ToString().Replace(",", ".") + ",");
                    sb.AppendLine("\t selectionHeight = " + pickerW.SelectionHeight + ",");

                    if (!pickerW.FontName.Equals(""))
                        sb.AppendLine("\t\t font = \"" + pickerW.FontName + "\",");

                    sb.AppendLine("\t\t fontSize = " + ((float)pickerW.FontSize * XRatio).ToString().Replace(",", ".") + ",");

                    string fontColorStr = "{" + pickerW.FontColor.R + ","
                            + pickerW.FontColor.G + ","
                            + pickerW.FontColor.B + "," + pickerW.FontAlpha + "}";
                    sb.AppendLine("\t\t fontColor = " + fontColorStr + ",");

                    string ColumnColorStr = "{" + pickerW.ColumnColor.R + ","
                             + pickerW.ColumnColor.G + ","
                             + pickerW.ColumnColor.B + "," + pickerW.ColumnAlpha + "}";
                    sb.AppendLine("\t\t columnColor = " + ColumnColorStr + ",");

                    if (pickerW.BackgroundImage != null)
                    {
                        sb.AppendLine("\t\t background = \"" + widget.Name + "background.png\",");
                        sb.AppendLine("\t\t backgroundWidth = " + ((float)pickerW.BackgroundSize.Width * XRatio).ToString().Replace(",", ".") + ",");
                        sb.AppendLine("\t\t backgroundHeight = " + ((float)pickerW.BackgroundSize.Height * YRatio).ToString().Replace(",", ".") + ",");
                    }

                    sb.AppendLine("\t\t columns = " + widget.Name + "_columnsData");
                    sb.AppendLine("\t}");

                    sb.AppendLine("\t" + this.Name + ".displayGroup:insert(" + this.Name + "." + pickerW.Name + ".view)");
                    sb.AppendLine("\t" + this.Name + "." + widget.Name + ".x = " + ((float)pickerW.Location.X * XRatio).ToString().Replace(",", "."));
                    sb.AppendLine("\t" + this.Name + "." + widget.Name + ".y = " + ((float)pickerW.Location.Y * YRatio).ToString().Replace(",", "."));
                }
            }

            for (int j = 0; j < this.CoronaObjects.Count; j++)
            {
                CoronaObject obj = this.CoronaObjects[j];
                if (obj.isEnabled == true)
                {
                    string objName = "";
                    if (obj.isEntity == false)
                        objName = obj.DisplayObject.Name.Replace(" ", "");
                    else
                        objName = obj.Entity.Name.Replace(" ", "");
                    //Definir si l'object est un generateur
                    if (obj.isGenerator == true)
                    {
                        //bool copyPhysic = false;
                        //if (obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                        //    copyPhysic = true;

                        string mapName = "nil";
                        if (this.TilesMap != null && this.TilesMap.isEnabled == true)
                            mapName = this.Name + "." + this.TilesMap.TilesMapName;

                        sb.AppendLine("\tlocal paramsGenerator_" + objName + " = {}");
                        sb.AppendLine("\tparamsGenerator_" + objName + ".isEntity = true");
                        sb.AppendLine("\tparamsGenerator_" + objName + ".name =\"" + objName + "_generator\"");
                        sb.AppendLine("\tparamsGenerator_" + objName + ".iteration =" + obj.generatorIteration);
                        sb.AppendLine("\tparamsGenerator_" + objName + ".objectToDuplicate = " + this.Name + "." + objName);
                        sb.AppendLine("\tparamsGenerator_" + objName + ".map = " + mapName);
                        sb.AppendLine("\tparamsGenerator_" + objName + ".fadeInSpeed = " + obj.FadeInSpeed);
                        sb.AppendLine("\tparamsGenerator_" + objName + ".fadeOutSpeed = " + obj.FadeOutSpeed);
                        sb.AppendLine("\tparamsGenerator_" + objName + ".fadeInEnabled = " + obj.FadeInEnabled.ToString().ToLower());
                        sb.AppendLine("\tparamsGenerator_" + objName + ".fadeOutEnabled = " + obj.FadeOutEnabled.ToString().ToLower());
                        sb.AppendLine("\tparamsGenerator_" + objName + ".removeOnCompleteFadeOut = " + obj.RemoveOnCompleteFadeOut.ToString().ToLower());
                        sb.AppendLine("\tparamsGenerator_" + objName + ".emissionType = \"" + obj.generatorEmissionType.ToString() + "\"");
                        sb.AppendLine("\tparamsGenerator_" + objName + ".delayBetweenFades = " + obj.DelayBetweenFades);
                        sb.AppendLine("\tparamsGenerator_" + objName + ".xImpulse = " + (obj.GeneratorXImpulse * XRatio).ToString().Replace(",", "."));
                        sb.AppendLine("\tparamsGenerator_" + objName + ".yImpulse = " + (obj.GeneratorYImpulse * YRatio).ToString().Replace(",", "."));
                        sb.AppendLine("\tparamsGenerator_" + objName + ".angularImpulse = " + obj.GeneratorAngularImpulse);
                        sb.AppendLine("\tparamsGenerator_" + objName + ".insertCloneAtEndOfGroup = " + obj.insertCloneAtEndOfGroup.ToString().ToLower());
                        if (obj.objectAttachedToGenerator != null)
                        {
                            if (obj.objectAttachedToGenerator.isEnabled == true)
                            {
                                string objAttachName = obj.objectAttachedToGenerator.DisplayObject.Name.Replace(" ", "");
                                sb.AppendLine("\tparamsGenerator_" + objName + ".objectAttached = " + this.Name + "." + objAttachName);
                            }
                        }

                        sb.AppendLine("\tparamsGenerator_" + objName + ".copyPhysicBody  =" + true.ToString().ToLower());
                        sb.AppendLine("\tparamsGenerator_" + objName + ".delay = " + obj.generatorDelay);

                        sb.AppendLine("\t" + this.Name + "." + objName + "_generator = generatorInstance.Generator.create(paramsGenerator_" + objName + ")");

                    }

                    if (obj.isEntity == true)
                    {
                        for (int k = 0; k < obj.Entity.CoronaObjects.Count; k++)
                        {
                            CoronaObject child = obj.Entity.CoronaObjects[k];
                            if (child.isEnabled == true)
                            {
                                string objNameChild = child.DisplayObject.Name.Replace(" ", "");
                                //Definir si l'object est un generateur
                                if (child.isGenerator == true)
                                {
                                    bool copyPhysic = false;
                                    if (child.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                                        copyPhysic = true;

                                    string mapName = "nil";
                                    if (this.TilesMap != null && this.TilesMap.isEnabled == true)
                                        mapName = this.Name + "." + this.TilesMap.TilesMapName;

                                    sb.AppendLine("\tlocal paramsGenerator_" + objNameChild + " = {}");
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".name =\"" + objNameChild + "_generator\"");
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".iteration =" + child.generatorIteration);
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".objectToDuplicate = " + this.Name + "." + objNameChild);
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".map = " + mapName);
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".fadeInSpeed = " + child.FadeInSpeed);
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".fadeOutSpeed = " + child.FadeOutSpeed);
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".fadeInEnabled = " + child.FadeInEnabled.ToString().ToLower());
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".fadeOutEnabled = " + child.FadeOutEnabled.ToString().ToLower());
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".removeOnCompleteFadeOut = " + child.RemoveOnCompleteFadeOut.ToString().ToLower());
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".emissionType = \"" + child.generatorEmissionType.ToString() + "\"");
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".delayBetweenFades = " + child.DelayBetweenFades);
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".xImpulse = " + (child.GeneratorXImpulse * XRatio).ToString().Replace(",", "."));
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".yImpulse = " + (child.GeneratorYImpulse * YRatio).ToString().Replace(",", "."));
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".angularImpulse = " + child.GeneratorAngularImpulse);
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".insertCloneAtEndOfGroup = " + child.insertCloneAtEndOfGroup.ToString().ToLower());
                                    if (child.objectAttachedToGenerator != null)
                                    {
                                        if (child.isEnabled == true)
                                        {
                                            string objAttachName = child.objectAttachedToGenerator.DisplayObject.Name.Replace(" ", "");
                                            sb.AppendLine("\tparamsGenerator_" + objNameChild + ".objectAttached = " + this.Name + "." + objAttachName);
                                        }
                                    }

                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".copyPhysicBody  =" + copyPhysic.ToString().ToLower());
                                    sb.AppendLine("\tparamsGenerator_" + objNameChild + ".delay = " + child.generatorDelay);

                                    sb.AppendLine("\t" + this.Name + "." + objNameChild + "_generator = generatorInstance.Generator.create(paramsGenerator_" + objNameChild + ")");

                                }
                            }
                        }

                    }
                }
                
            }
        }
       
        public void buildObjectToLua(StringBuilder sb, float XRatio, float YRatio, CoronaObject objectToBuild)
        {
            float moyenneRatio = (XRatio + YRatio) / 2;

            if (objectToBuild.isEntity == false)
            {
                DisplayObject obj = objectToBuild.DisplayObject;


                string objName = obj.Name.Replace(" ", "");
                string paramsName = "params_" + objName;

                //Create Gradient color if enabled
                string gradient = "nil";


                if (obj.GradientColor.isEnabled == true)
                {
                    string color1 = "{" + obj.GradientColor.color1.R + ","
                        + obj.GradientColor.color1.G + ","
                        + obj.GradientColor.color1.B + "}";

                    string color2 = "{" + obj.GradientColor.color2.R + ","
                        + obj.GradientColor.color2.G + ","
                        + obj.GradientColor.color2.B + "}";

                    string direction = "\"" + obj.GradientColor.direction.ToString() + "\"";
                    gradient = "{" + color1 + "," + color2 + "," + direction + "}";
                }

                sb.AppendLine("\tlocal " + paramsName + " = {}");

                if (objectToBuild.EntityParent != null)
                {
                    sb.AppendLine("\t" + paramsName + ".entityParent = " + this.Name.Replace(" ", "") + "." + objectToBuild.EntityParent.Name);
                }

                if (obj.Type.Equals("IMAGE"))
                {

                    Size finalSize = new Size(Convert.ToInt32(obj.SurfaceRect.Size.Width * XRatio), Convert.ToInt32(obj.SurfaceRect.Size.Height * YRatio));
                    String paramsObj = "\t" + paramsName + ".name = \"" + objName + "\"\n" +
                                              "\t" + paramsName + ".type = \"IMAGE\" \n" +
                                              "\t" + paramsName + ".isHandledByTilesMap = " + obj.CoronaObjectParent.IsHandledByTilesMap.ToString().ToLower() + "\n" +
                                             "\t" + paramsName + ".x = " + ((float)obj.SurfaceRect.X * XRatio).ToString().Replace(",", ".") + "\n" +
                                             "\t" + paramsName + ".y = " + ((float)obj.SurfaceRect.Y * YRatio).ToString().Replace(",", ".") + "\n" +
                                             "\t" + paramsName + ".width = " + finalSize.Width.ToString() + "\n" +
                                             "\t" + paramsName + ".height = " + finalSize.Height.ToString() + "\n" +
                                             "\t" + paramsName + ".blendMode = \"" + obj.blendMode + "\"\n" +
                                              "\t" + paramsName + ".rotation = " + obj.Rotation + "\n" +
                                              "\t" + paramsName + ".alpha = " + obj.Alpha.ToString().Replace(",", ".") + "\n" +
                                            "\t" + paramsName + ".gradient = " + gradient + "\n" +
                                             "\t" + paramsName + ".pathImage = \"" + obj.OriginalAssetName.ToLower() + finalSize.Width + "x" + finalSize.Height + ".png\"\n" +
                                             "\t" + paramsName + ".displayGroupParent = " + this.Name.Replace(" ", "") + ".displayGroup\n\n";

                    sb.Append(paramsObj);
                    sb.AppendLine("\t" + paramsName + ".isDraggable = " + obj.CoronaObjectParent.isDraggable.ToString().ToLower());

                    if (obj.CoronaObjectParent.isDraggable == true)
                    {
                        sb.AppendLine("\t" + paramsName + ".dragDamping = " + obj.CoronaObjectParent.DragDamping.ToString().Replace(",", "."));
                        sb.AppendLine("\t" + paramsName + ".dragMaxForce = " + obj.CoronaObjectParent.DragMaxForce);
                        sb.AppendLine("\t" + paramsName + ".dragFrequency = " + obj.CoronaObjectParent.DragFrequency);
                    }

                    if (obj.CoronaObjectParent.isDraggable == true)
                        sb.AppendLine("\t" + paramsName + ".dragBody = dragBody");


                    String createObj = "\t" + this.Name + "." + objName + " = objectInstance.Object.create(" + paramsName + ")";
                    sb.AppendLine(createObj);



                    if (obj.ImageFillColor.IsEmpty == false)
                    {
                        sb.AppendLine("\t" + this.Name + "." + objName + ".object:setFillColor(" +
                            obj.ImageFillColor.R + "," + obj.ImageFillColor.G + "," + obj.ImageFillColor.B + ")");
                    }
                }

                //Sinon si c'est un SPRITE
                else if (obj.Type.Equals("SPRITE"))
                {
                    string sequenceName = obj.CurrentSequence;
                    if (sequenceName == "DEFAULT" || sequenceName == "") sequenceName = "nil";
                    else
                    {
                        sequenceName = "\"" + sequenceName + "\"";
                    }

                    //Creer les params de l'objet
                    String paramsSprite = "\t" + paramsName + ".name = \"" + objName + "\"\n" +
                                             "\t" + paramsName + ".type = \"SPRITE\" \n" +
                                             "\t" + paramsName + ".isHandledByTilesMap = " + obj.CoronaObjectParent.IsHandledByTilesMap.ToString().ToLower() + "\n" +
                                            "\t" + paramsName + ".x = " + ((float)obj.SurfaceRect.X * XRatio).ToString().Replace(",", ".") + "\n" +
                                             "\t" + paramsName + ".y = " + ((float)obj.SurfaceRect.Y * YRatio).ToString().Replace(",", ".") + "\n" +
                                             "\t" + paramsName + ".spriteSet = " + obj.SpriteSet.Name + "\n" +
                                             "\t" + paramsName + ".rotation = " + obj.Rotation + "\n" +
                                             "\t" + paramsName + ".blendMode = \"" + obj.blendMode + "\"\n" +
                                             "\t" + paramsName + ".sequenceName = " + sequenceName+ "\n" +
                                             "\t" + paramsName + ".firstFrameIndex = " + (obj.CurrentFrame +1) + "\n" +
                                              "\t" + paramsName + ".autoPlay = " + obj.AutoPlay.ToString().ToLower() + "\n" +
                                             "\t" + paramsName + ".alpha = " + obj.Alpha.ToString().Replace(",", ".") + "\n" +
                                            "\t" + paramsName + ".gradient = " + gradient + "\n" +
                                            "\t" + paramsName + ".scale = 1/currentSuffix.value\n" +
                                             "\t" + paramsName + ".displayGroupParent = " + this.Name.Replace(" ", "") + ".displayGroup\n\n";


                    //Ajouter la sprite set de l'objet 
                    sb.Append(paramsSprite);

                    sb.AppendLine("\t" + paramsName + ".isDraggable = " + obj.CoronaObjectParent.isDraggable.ToString().ToLower());

                    if (obj.CoronaObjectParent.isDraggable == true)
                    {
                        sb.AppendLine("\t" + paramsName + ".dragBody = dragBody");
                        sb.AppendLine("\t" + paramsName + ".dragDamping = " + obj.CoronaObjectParent.DragDamping.ToString().Replace(",", "."));
                        sb.AppendLine("\t" + paramsName + ".dragMaxForce = " + obj.CoronaObjectParent.DragMaxForce);
                        sb.AppendLine("\t" + paramsName + ".dragFrequency = " + obj.CoronaObjectParent.DragFrequency);
                    }

                    sb.AppendLine("\t" + this.Name + "." + objName + " = objectInstance.Object.create(" + paramsName + ")");
                    //if (obj.SpriteSet.Sequences.Count > 0)
                    //    sb.AppendLine("\t" + this.Name + "." + objName + ".currentSequence = \"" + obj.SpriteSet.Sequences[0].Name + "\"");

                }
                else if (obj.Type.Equals("FIGURE"))
                {
                    //Si c'est un rectangle 
                    if (obj.Figure.ShapeType.Equals("RECTANGLE"))
                    {
                        //Recuperer le rectangle 
                        Rect rect = obj.Figure as Rect;

                        String paramsObj = "\t" + paramsName + ".name = \"" + objName + "\"\n" +
                                             "\t" + paramsName + ".type = \"RECTANGLE\" \n" +
                                             "\t" + paramsName + ".isHandledByTilesMap = " + obj.CoronaObjectParent.IsHandledByTilesMap.ToString().ToLower() + "\n" +
                                             "\t" + paramsName + ".isRounded = " + rect.isRounded.ToString().ToLower() + "\n" +
                                              "\t" + paramsName + ".x = " + ((float)rect.Position.X * XRatio).ToString().Replace(",", ".") + "\n" +
                                              "\t" + paramsName + ".y = " + ((float)rect.Position.Y * YRatio).ToString().Replace(",", ".") + "\n" +
                                              "\t" + paramsName + ".width = " + ((float)rect.Width * XRatio).ToString().Replace(",", ".") + "\n" +
                                              "\t" + paramsName + ".height = " + ((float)rect.Height * YRatio).ToString().Replace(",", ".") + "\n" +
                                             "\t" + paramsName + ".rotation = " + obj.Rotation + "\n" +
                                             "\t" + paramsName + ".blendMode = \"" + obj.blendMode + "\"\n" +
                                            "\t" + paramsName + ".gradient = " + gradient + "\n" +
                                            "\t" + paramsName + ".alpha = " + obj.Alpha.ToString().Replace(",", ".") + "\n";
                        if (rect.isRounded == true)
                        {
                            paramsObj += "\t" + paramsName + ".cornerRadius = " + rect.cornerRadius + "\n";
                        }

                        if (obj.Figure.Fill == true)
                            paramsObj += "\t" + paramsName + ".backColor = { R = " + rect.FillColor.R + ",G = " + rect.FillColor.G + ", B = " + rect.FillColor.B + "}\n";
                        else
                            paramsObj += "\t" + paramsName + ".backColor = { R = 0, G = 0, B= 0}\n";

                        paramsObj += "\t" + paramsName + ".strokeColor = { R = " + rect.StrokeColor.R + ",G = " + rect.StrokeColor.G + ", B = " + rect.StrokeColor.B + "}\n" +
                                                "\t" + paramsName + ".strokeWidth = " + rect.StrokeSize + "\n" +
                                                "\t" + paramsName + ".displayGroupParent = " + this.Name.Replace(" ", "") + ".displayGroup\n\n";


                        sb.Append(paramsObj);

                        sb.AppendLine("\t" + paramsName + ".isDraggable = " + obj.CoronaObjectParent.isDraggable.ToString().ToLower());
                        if (obj.CoronaObjectParent.isDraggable == true)
                        {
                            sb.AppendLine("\t" + paramsName + ".dragBody = dragBody");
                            sb.AppendLine("\t" + paramsName + ".dragDamping = " + obj.CoronaObjectParent.DragDamping.ToString().Replace(",", "."));
                            sb.AppendLine("\t" + paramsName + ".dragMaxForce = " + obj.CoronaObjectParent.DragMaxForce);
                            sb.AppendLine("\t" + paramsName + ".dragFrequency = " + obj.CoronaObjectParent.DragFrequency);
                        }

                        String createObj = "\t" + this.Name + "." + objName + " = objectInstance.Object.create(" + paramsName + ")";
                        sb.AppendLine(createObj);

                    }
                    else if (obj.Figure.ShapeType.Equals("CIRCLE"))
                    {
                        //Recuperer le cercle 
                        Cercle cercle = obj.Figure as Cercle;

                        String paramsObj = "\t" + paramsName + ".name = \"" + objName + "\"\n" +
                                                 "\t" + paramsName + ".type = \"CIRCLE\" \n" +
                                                 "\t" + paramsName + ".isHandledByTilesMap = " + obj.CoronaObjectParent.IsHandledByTilesMap.ToString().ToLower() + "\n" +
                                                 "\t" + paramsName + ".x = " + ((float)cercle.Position.X * XRatio).ToString().Replace(",", ".") + "\n" +
                                                 "\t" + paramsName + ".y = " + ((float)cercle.Position.Y * YRatio).ToString().Replace(",", ".") + "\n" +
                                                 "\t" + paramsName + ".radius = " + ((float)cercle.Rayon * moyenneRatio).ToString().Replace(",", ".") + "\n" +
                                                 "\t" + paramsName + ".rotation = " + obj.Rotation + "\n" +
                                                "\t" + paramsName + ".blendMode = \"" + obj.blendMode + "\"\n" +
                                                "\t" + paramsName + ".alpha = " + obj.Alpha.ToString().Replace(",", ".") + "\n";
                        if (obj.Figure.Fill == true)
                            paramsObj += "\t" + paramsName + ".backColor = { R = " + cercle.FillColor.R + ",G = " + cercle.FillColor.G + ", B = " + cercle.FillColor.B + "}\n";
                        else
                            paramsObj += "\t" + paramsName + ".backColor = { R = 0, G = 0, B= 0}\n";

                        paramsObj += "\t" + paramsName + ".strokeColor = { R = " + cercle.StrokeColor.R + ",G = " + cercle.StrokeColor.G + ", B = " + cercle.StrokeColor.B + "}\n" +
                                       "\t" + paramsName + ".strokeWidth = " + cercle.StrokeSize + "\n" +
                                          "\t" + paramsName + ".displayGroupParent = " + this.Name.Replace(" ", "") + ".displayGroup\n\n";

                        sb.Append(paramsObj);

                        sb.AppendLine("\t" + paramsName + ".isDraggable = " + obj.CoronaObjectParent.isDraggable.ToString().ToLower());
                        if (obj.CoronaObjectParent.isDraggable == true)
                        {
                            sb.AppendLine("\t" + paramsName + ".dragBody = dragBody");
                            sb.AppendLine("\t" + paramsName + ".dragDamping = " + obj.CoronaObjectParent.DragDamping.ToString().Replace(",", "."));
                            sb.AppendLine("\t" + paramsName + ".dragMaxForce = " + obj.CoronaObjectParent.DragMaxForce);
                            sb.AppendLine("\t" + paramsName + ".dragFrequency = " + obj.CoronaObjectParent.DragFrequency);
                        }

                        String createObj = "\t" + this.Name + "." + objName + " = objectInstance.Object.create(" + paramsName + ")";
                        sb.AppendLine(createObj);


                    }
                    else if (obj.Figure.ShapeType.Equals("LINE"))
                    {
                        //Recuperer le cercle 
                        Line line = obj.Figure as Line;

                        //Create table of points
                        string tabPoints = "\tlocal points_" + objName + "= {";
                        for (int k = 0; k < line.Points.Count; k++)
                        {
                            if (k % 50 == 0)
                                tabPoints += "\n\t\t";

                            tabPoints += (((float)line.Points[k].X * XRatio).ToString().Replace(",", ".")) + "," + (((float)line.Points[k].Y * YRatio).ToString().Replace(",", "."));
                            if (k != line.Points.Count - 1)
                                tabPoints += ",";

                        }
                        tabPoints += "\t}";

                        String paramsObj = "\t" + paramsName + ".name = \"" + objName + "\"\n" +
                                                "\t" + paramsName + ".type = \"LINE\"\n" +
                                                "\t" + paramsName + ".isHandledByTilesMap = " + obj.CoronaObjectParent.IsHandledByTilesMap.ToString().ToLower() + "\n" +
                                                 "\t" + paramsName + ".tabPoints = points_" + objName + "\n" +
                                                 "\t" + paramsName + ".rotation = " + obj.Rotation + "\n" +
                                                 "\t" + paramsName + ".blendMode = \"" + obj.blendMode + "\"\n" +
                                                 "\t" + paramsName + ".alpha = " + obj.Alpha.ToString().Replace(",", ".") + "\n" +
                                                  "\t" + paramsName + ".gradient = " + gradient + "\n" +
                                                 "\t" + paramsName + ".backColor = { R = " + line.StrokeColor.R + ",G = " + line.StrokeColor.G + ", B = " + line.StrokeColor.B + "}\n" +
                                                 "\t" + paramsName + ".strokeWidth = " + line.StrokeSize + "\n" +
                                                 "\t" + paramsName + ".displayGroupParent = " + this.Name.Replace(" ", "") + ".displayGroup\n\n";

                        sb.Append(tabPoints);
                        sb.Append(paramsObj);

                        sb.AppendLine("\t" + paramsName + ".isDraggable = " + obj.CoronaObjectParent.isDraggable.ToString().ToLower());
                        if (obj.CoronaObjectParent.isDraggable == true)
                        {
                            sb.AppendLine("\t" + paramsName + ".dragBody = dragBody");
                            sb.AppendLine("\t" + paramsName + ".dragDamping = " + obj.CoronaObjectParent.DragDamping.ToString().Replace(",", "."));
                            sb.AppendLine("\t" + paramsName + ".dragMaxForce = " + obj.CoronaObjectParent.DragMaxForce);
                            sb.AppendLine("\t" + paramsName + ".dragFrequency = " + obj.CoronaObjectParent.DragFrequency);
                        }

                        String createObj = "\t" + this.Name + "." + objName + " = objectInstance.Object.create(" + paramsName + ")";
                        sb.AppendLine(createObj);

                    }
                    else if (obj.Figure.ShapeType.Equals("CURVE"))
                    {
                        //Recuperer le cercle 
                        CourbeBezier courbe = obj.Figure as CourbeBezier;

                        GraphicsPath path = new GraphicsPath(FillMode.Winding);
                        path.AddCurve(courbe.UserPoints.ToArray());
                        path.Flatten();
                        PointF[] finalPoints = path.PathPoints;

                        //Create table of points
                        string tabPoints = "\tlocal points_" + objName + "= {";
                        for (int k = 0; k < finalPoints.Length; k++)
                        {
                            if (k % 10 == 0)
                                tabPoints += "\n\t\t";

                            tabPoints += ((float)finalPoints[k].X * XRatio).ToString().Replace(",", ".") + "," + ((float)finalPoints[k].Y * YRatio).ToString().Replace(",", ".");
                            if (k != finalPoints.Length - 1)
                                tabPoints += ",";

                        }
                        tabPoints += "\t}\n";

                        String paramsObj = "\t" + paramsName + ".name = \"" + objName + "\"\n" +
                                              "\t" + paramsName + ".type = \"CURVE\"\n" +
                                              "\t" + paramsName + ".isHandledByTilesMap = " + obj.CoronaObjectParent.IsHandledByTilesMap.ToString().ToLower() + "\n" +
                                               "\t" + paramsName + ".tabPoints = points_" + objName + "\n" +
                                                 "\t" + paramsName + ".rotation = " + obj.Rotation + "\n" +
                                                 "\t" + paramsName + ".blendMode = \"" + obj.blendMode + "\"\n" +
                                                 "\t" + paramsName + ".alpha = " + obj.Alpha.ToString().Replace(",", ".") + "\n" +
                                                  "\t" + paramsName + ".gradient = " + gradient + "\n" +
                                                 "\t" + paramsName + ".backColor = { R = " + courbe.StrokeColor.R + ",G = " + courbe.StrokeColor.G + ", B = " + courbe.StrokeColor.B + "}\n" +
                                                  "\t" + paramsName + ".strokeWidth = " + courbe.StrokeSize + "\n" +
                                                  "\t" + paramsName + ".displayGroupParent = " + this.Name.Replace(" ", "") + ".displayGroup\n\n";

                        sb.Append(tabPoints);

                        sb.Append(paramsObj);


                        sb.AppendLine("\t" + paramsName + ".isDraggable = " + obj.CoronaObjectParent.isDraggable.ToString().ToLower());
                        if (obj.CoronaObjectParent.isDraggable == true)
                        {
                            sb.AppendLine("\t" + paramsName + ".dragBody = dragBody");
                            sb.AppendLine("\t" + paramsName + ".dragDamping = " + obj.CoronaObjectParent.DragDamping.ToString().Replace(",", "."));
                            sb.AppendLine("\t" + paramsName + ".dragMaxForce = " + obj.CoronaObjectParent.DragMaxForce);
                            sb.AppendLine("\t" + paramsName + ".dragFrequency = " + obj.CoronaObjectParent.DragFrequency);
                        }

                        String createObj = "\t" + this.Name + "." + objName + " = objectInstance.Object.create(" + paramsName + ")";
                        sb.AppendLine(createObj);

                    }
                    else if (obj.Figure.ShapeType.Equals("TEXT"))
                    {
                        //Recuperer le cercle 
                        Texte text = obj.Figure as Texte;
                        if (text.font2.FontItem == null) text.font2.FontItem = new GameEditor.FontManager.FontItem("DEFAULT", this.SceneParent.projectParent);
                        if (!text.font2.FontItem.NameForAndroid.Equals("DEFAULT"))
                        {
                            string fontDeclaration = text.font2.FontItem.GenerateCode();
                            sb.AppendLine(fontDeclaration);
                        }
                        else
                        {
                            sb.AppendLine("\tlocal font = nil");
                        }

                        String paramsObj = "\t" + paramsName + ".name = \"" + objName + "\"\n" +
                                               "\t" + paramsName + ".type = \"TEXT\" \n" +
                                               "\t" + paramsName + ".isHandledByTilesMap = " + obj.CoronaObjectParent.IsHandledByTilesMap.ToString().ToLower() + "\n" +
                                               "\t" + paramsName + ".x = " + ((float)text.Position.X * XRatio).ToString().Replace(",", ".") + "\n" +
                                               "\t" + paramsName + ".y = " + ((float)text.Position.Y * YRatio).ToString().Replace(",", ".") + "\n" +
                                               "\t" + paramsName + ".width = " + ((float)obj.SurfaceRect.Width * XRatio).ToString().Replace(",", ".") + "\n" +
                                               "\t" + paramsName + ".height = "+((float)0 * YRatio).ToString().Replace(",", ".") + "\n" +
                                               "\t" + paramsName + ".text = \"" + @text.Txt.Replace("\r\n",@"\n").Replace("\"","\\\"") + "\"\n" +
                                                 "\t" + paramsName + ".rotation = " + obj.Rotation + "\n" +
                                                 "\t" + paramsName + ".blendMode = \"" + obj.blendMode + "\"\n" +
                                                 "\t" + paramsName + ".alpha = " + obj.Alpha.ToString().Replace(",", ".") + "\n" +
                                                 "\t" + paramsName + ".gradient = " + gradient + "\n" +
                                                "\t" + paramsName + ".textColor = { R = " + text.FillColor.R + ",G = " + text.FillColor.G + ", B = " + text.FillColor.B + "}\n" +
                                                "\t" + paramsName + ".textSize = " + (((float)text.font2.Size * 1.4f) * XRatio).ToString().Replace(",", ".") + "\n" +
                                                "\t" + paramsName + ".textFont = font\n" +
                                                "\t" + paramsName + ".displayGroupParent = " + this.Name.Replace(" ", "") + ".displayGroup\n\n";

                        sb.Append(paramsObj);


                        sb.AppendLine("\t" + paramsName + ".isDraggable = " + obj.CoronaObjectParent.isDraggable.ToString().ToLower());
                        if (obj.CoronaObjectParent.isDraggable == true)
                        {
                            sb.AppendLine("\t" + paramsName + ".dragBody = dragBody");
                            sb.AppendLine("\t" + paramsName + ".dragDamping = " + obj.CoronaObjectParent.DragDamping.ToString().Replace(",", "."));
                            sb.AppendLine("\t" + paramsName + ".dragMaxForce = " + obj.CoronaObjectParent.DragMaxForce);
                            sb.AppendLine("\t" + paramsName + ".dragFrequency = " + obj.CoronaObjectParent.DragFrequency);
                        }

                        String createObj = "\t" + this.Name + "." + objName + " = objectInstance.Object.create(" + paramsName + ")";
                        sb.AppendLine(createObj);

                    }

                    
                }

                //Ajouter l'object dans les interactions de la tilesmap si elle existe dans le layer
                if (this.TilesMap != null && obj.CoronaObjectParent.IsHandledByTilesMap == true)
                {
                    if (this.TilesMap.isEnabled == true)
                    {
                        sb.AppendLine("\t" + this.Name + "." + this.TilesMap.TilesMapName + ":addExternalObjectToInteractWith(" + objName + ")");
                    }
                }

                //CREER Le mask DE L OBJET SI ELLE EXISTE -----------------------------------------
                if (obj.CoronaObjectParent.BitmapMask != null && obj.CoronaObjectParent.BitmapMask.MaskImage != null && obj.CoronaObjectParent.BitmapMask.IsMaskEnabled == true)
                {
                    sb.AppendLine("\tlocal " + objName + "_mask = graphics.newMask(\"" + objName.ToLower() + "_mask.png\")");
                    sb.AppendLine("\t" + this.Name + "." + objName + ".object:setMask(" + objName + "_mask)");
                }

                //CREER LA PHYSIC DE L OBJET SI ELLE EXISTE -----------------------------------------
                if (obj.CoronaObjectParent.PhysicsBody != null)
                {
                    PhysicsBody ph = obj.CoronaObjectParent.PhysicsBody;
                    if (ph.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                    {
                        CoronaObject coronaObject = obj.CoronaObjectParent;

                        ////Creer le fichier lua associé
                        //PhysicBodyLuaGenerator gen = new PhysicBodyLuaGenerator(coronaObject.PhysicsBody);
                        //gen.writeToLua(new DirectoryInfo(this.SceneParent.projectParent.BuildFolderPath), XRatio, YRatio);

                        //if(File.Exists(this.SceneParent.projectParent.SourceFolderPath+"\\body" + objName.ToLower()+".lua"))
                        //    File.Delete(this.SceneParent.projectParent.SourceFolderPath+"\\body" + objName.ToLower()+".lua");

                        //Faire le require correspondant
                        sb.AppendLine("\tlocal body_" + objName + " = require(\"body" + objName.ToLower() + "\")" + ".startBody(" + this.Name + "." + objName + ")");


                    }

                }

                //CREER Le PATH FOLLOW DE L OBJET SI ELLE EXISTE -----------------------------------------
                if (obj.CoronaObjectParent.PathFollow != null)
                {
                    PathFollow path = obj.CoronaObjectParent.PathFollow;
                    if (path.isEnabled == true)
                    {
                        CoronaObject coronaObject = obj.CoronaObjectParent;

                        sb.AppendLine("\tlocal " + objName + "_pathFollow = " + path.getPointsTableLua(XRatio, YRatio));
                        sb.AppendLine("\tlocal " + objName + "_pathFollow_params = {}");
                        sb.AppendLine("\t" + objName + "_pathFollow_params.isCyclic = " + coronaObject.PathFollow.isCyclic.ToString().ToLower());
                        sb.AppendLine("\t" + objName + "_pathFollow_params.removeOnComplete = " + coronaObject.PathFollow.removeOnComplete.ToString().ToLower());
                        sb.AppendLine("\t" + objName + "_pathFollow_params.rotate = " + coronaObject.PathFollow.Rotate.ToString().ToLower());
                        sb.AppendLine("\t" + objName + "_pathFollow_params.targetObject = " + this.Name + "." + objName);
                        sb.AppendLine("\t" + objName + "_pathFollow_params.path = " + objName + "_pathFollow");
                        sb.AppendLine("\t" + objName + "_pathFollow_params.speed = " + coronaObject.PathFollow.speed);
                        sb.AppendLine("\t" + objName + "_pathFollow_params.iteration = " + coronaObject.PathFollow.Iteration);
                        sb.AppendLine("\t" + this.Name + "." + objName + ".pathFollow = pathFollowInstance.PathFollow.create(" + objName + "_pathFollow_params)");


                    }

                }

                /*  sb.AppendLine("\t" + this.Name + "." + objName + ":setOnStartBehaviour(" + obj.CoronaObjectParent.onStartFunction + ")");
                  sb.AppendLine("\t" + this.Name + "." + objName + ":setOnPauseBehaviour(" + obj.CoronaObjectParent.onPauseFunction + ")");
                  sb.AppendLine("\t" + this.Name + "." + objName + ":setOnDeleteBehaviour(" + obj.CoronaObjectParent.onDeleteFunction + ")");*/


            }
        
        }

        public void buildToLuaAllJointures(StringBuilder sb, float XRatio, float YRatio)
        {
            for (int j = 0; j < this.Jointures.Count; j++)
            {
                CoronaJointure joint = this.Jointures[j];
                if (joint.isEnabled == true)
                {
                    if (!joint.type.Equals(""))
                        joint.toCodeLua(sb, XRatio, YRatio);
                }
            }

            for (int i = 0; i < this.CoronaObjects.Count; i++)
            {
                CoronaObject objectToBuild = this.CoronaObjects[i];
                if (objectToBuild.isEnabled == true)
                {
                    if (objectToBuild.isEntity == true)
                    {
                        for (int l = 0; l < objectToBuild.Entity.Jointures.Count; l++)
                        {
                            CoronaJointure jointToBuild = objectToBuild.Entity.Jointures[l];
                            if (jointToBuild.isEnabled == true)
                            {
                                sb.AppendLine("\tlocal paramsJoint_" + jointToBuild.name + "= {}");
                                sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".name = \"" + jointToBuild.name + "\"");
                                sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".type = \"" + jointToBuild.type.ToLower() + "\"");
                                if (jointToBuild.coronaObjA != null)
                                {
                                    string objName = jointToBuild.coronaObjA.DisplayObject.Name.Replace(" ", "");
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".obj1 = \"" + objName + "\"");
                                }

                                if (jointToBuild.coronaObjB != null)
                                {
                                    string objName = jointToBuild.coronaObjB.DisplayObject.Name.Replace(" ", "");
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".obj2 = \"" + objName + "\"");
                                }

                                Point offSetFocus = this.SceneParent.Camera.SurfaceFocus.Location;
                                if (jointToBuild.AnchorPointA != Point.Empty)
                                {
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".anchorX1 = " + (int)((float)(jointToBuild.AnchorPointA.X + jointToBuild.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio));
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".anchorY1 = " + (int)((float)(jointToBuild.AnchorPointA.Y + jointToBuild.coronaObjA.DisplayObject.SurfaceRect.Y) * YRatio));
                                }
                                if (jointToBuild.AnchorPointB != Point.Empty)
                                {
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".anchorX2 = " + (int)((float)(jointToBuild.AnchorPointB.X + jointToBuild.coronaObjB.DisplayObject.SurfaceRect.X) * XRatio));
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".anchorY2 = " + (int)((float)(jointToBuild.AnchorPointB.Y + jointToBuild.coronaObjB.DisplayObject.SurfaceRect.Y) * YRatio));
                                }

                                if (jointToBuild.ObjectAnchorPointA != Point.Empty)
                                {
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".objX1 = " + (int)((float)(jointToBuild.ObjectAnchorPointA.X + jointToBuild.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio));
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".objY1 = " + (int)((float)(jointToBuild.ObjectAnchorPointA.Y + jointToBuild.coronaObjA.DisplayObject.SurfaceRect.Y) * YRatio));
                                }

                                if (jointToBuild.ObjectAnchorPointB != Point.Empty)
                                {
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".objX2 = " + (int)((float)(jointToBuild.ObjectAnchorPointB.X + jointToBuild.coronaObjB.DisplayObject.SurfaceRect.X) * XRatio));
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".objY2 = " + (int)((float)(jointToBuild.ObjectAnchorPointB.Y + jointToBuild.coronaObjB.DisplayObject.SurfaceRect.Y) * YRatio));
                                }

                                if (jointToBuild.axisDistance != Point.Empty)
                                {
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".axisDistanceX = " + (int)((float)(jointToBuild.axisDistance.X + jointToBuild.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio));
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".axisDistanceY = " + (int)((float)(jointToBuild.axisDistance.Y + jointToBuild.coronaObjA.DisplayObject.SurfaceRect.Y) * YRatio));
                                }

                                //Set Params

                                if (jointToBuild.type.ToLower().Equals("pivot") || jointToBuild.type.ToLower().Equals("friction") || jointToBuild.type.ToLower().Equals("weld"))
                                {
                                    if (!jointToBuild.type.Equals("weld"))
                                    {
                                        if (!jointToBuild.type.Equals("friction"))
                                        {
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".isMotorEnabled=" + jointToBuild.isMotorEnable.ToString().ToLower());
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".isLimitEnabled=" + jointToBuild.isLimitEnabled.ToString().ToLower());
                                        }
                                        else
                                        {
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".maxForce=" + jointToBuild.maxForce.ToString());
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".maxTorque=" + jointToBuild.maxTorque.ToString());
                                        }

                                        if (jointToBuild.isMotorEnable && !jointToBuild.type.Equals("friction"))
                                        {
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".motorSpeed=" + jointToBuild.motorSpeed.ToString());
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".maxMotorTorque=" + jointToBuild.maxMotorTorque.ToString());
                                        }

                                        if (jointToBuild.isLimitEnabled && !jointToBuild.type.Equals("friction"))
                                        {
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".lowerLimit=" + jointToBuild.lowerLimit.ToString());
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".upperLimit=" + jointToBuild.upperLimit.ToString());
                                        }

                                    }

                                }
                                else if (jointToBuild.type.ToLower().Equals("distance"))
                                {

                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".frequency=" + jointToBuild.frequency.ToString());
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".dampingRatio=" + jointToBuild.frequency.ToString());

                                }
                                else if (jointToBuild.type.ToLower().Equals("piston") || jointToBuild.type.ToLower().Equals("wheel"))
                                {
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".isMotorEnabled=" + jointToBuild.isMotorEnable.ToString().ToLower());
                                    sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".isLimitEnabled=" + jointToBuild.isLimitEnabled.ToString().ToLower());

                                    if (jointToBuild.type.ToLower().Equals("piston"))
                                    {
                                        if (jointToBuild.isMotorEnable)
                                        {
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".motorSpeed=" + jointToBuild.motorSpeed.ToString());
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".maxMotorForce =" + jointToBuild.maxMotorForce.ToString());

                                        }



                                    }
                                    if (jointToBuild.type.ToLower().Equals("Wheel"))
                                    {
                                        if (jointToBuild.isMotorEnable)
                                        {
                                            sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".motorSpeed=" + jointToBuild.motorSpeed.ToString());

                                        }

                                    }

                                    if (jointToBuild.isLimitEnabled)
                                    {
                                        sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".lowerLimit=" + jointToBuild.lowerLimit.ToString());
                                        sb.AppendLine("\tparamsJoint_" + jointToBuild.name + ".upperLimit=" + jointToBuild.upperLimit.ToString());
                                    }
                                }


                                sb.AppendLine("\t" + this.Name.Replace(" ", "") + "." + objectToBuild.Entity.Name + ":createJoint(paramsJoint_" + jointToBuild.name + ")");
                            }
                        }
                    }
                }
            }
            
            
        }
    }
}
