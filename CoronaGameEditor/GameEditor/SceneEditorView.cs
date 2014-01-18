using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Krea.GameEditor.Panel_Jointures_Properties;
using Krea.CoronaClasses;
using Krea.CGE_Figures;
using Krea.GameEditor.PropertyGridConverters;
using System.Drawing.Drawing2D;
using Krea.Corona_Classes;
using Krea.Corona_Classes.Controls;
using GorgonLibrary.Graphics;
using GorgonLibrary;

namespace Krea.GameEditor
{
    public partial class SceneEditorView : UserControl
    {

        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------

        public String CurentCalque;
        public int indexLayerSelected;
        public int indexSceneSelected;
        public String Mode;

        public float CurrentScale = 1.0f;
        private bool isMousePressed;
        public List<CoronaObject> objectsSelected;
        private List<JoystickControl> joysticksSelected;

        private Form1 mainForm;

        private bool moveFocusSceneActive;
        private bool moveObjectActive;
        private bool moveSurfaceAdActive;
        private bool moveSurfaceControlActive;

        private String movingMode;

        private Point currentMousePos;

        public bool isScrolling = false;
        public Figure FigActive;
        public bool isBezierCreated = false;
        public Rectangle surfaceTextTemp;
        public bool isLineCreated = false;
        public Point CurrentJointAnchor;
        private Point pDepart;

        //-----------Cursors
        private Cursor rotationCursor;
        private Cursor createShapeCursor;
        private Cursor openHandCursor;
        private TransformBox transformBoxSelected;

        private Point jointAnchorSelected = Point.Empty;

        public GraphicsContentManager GraphicsContentManager;
        public bool IsFocused = false;
        //---------------------------------------------------
        //-------------------Constructeurs------------------------
        //---------------------------------------------------
        public SceneEditorView()
        {
            InitializeComponent();


        }
        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (e.Delta > 0)
                        this.zoomAvant();
                    else
                        this.zoomArriere();

                    this.mainForm.graduationBarX.setScale(this.CurrentScale);
                    this.mainForm.graduationBarY.setScale(this.CurrentScale);
                }
                else
                {


                    Point value = this.getOffsetPoint();
                    if (e.Delta > 0)
                        value.Y = value.Y + (int)(50 * (1 / this.CurrentScale));
                    else
                        value.Y = value.Y - (int)(50 * (1 / this.CurrentScale));

                    this.scrollView(-value.X, -value.Y);
                }

            }
        }


        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------


        public void UniqueInit()
        {
            try
            {

                // Initialize Gorgon

                CurentCalque = "NONE";
                moveFocusSceneActive = false;
                moveObjectActive = false;
                moveSurfaceAdActive = false;
                moveSurfaceControlActive = false;
                movingMode = "NONE";
                Mode = "NONE";

                CurrentScale = 1;

                this.indexLayerSelected = -1;
                this.indexSceneSelected = -1;

                objectsSelected = new List<CoronaObject>();
                joysticksSelected = new List<JoystickControl>();

                this.GraphicsContentManager = new GraphicsContentManager(this.mainForm);

                this.surfacePictBx.AllowDrop = true;

                this.MouseWheel += new MouseEventHandler(panel1_MouseWheel);


                this.mainForm.SetModeObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during initializaton of the graphics engine.\n\n Please be sure to have SlimDX installed on this computer. If not, please uninstall Krea and reinstall it entirely. The SlimDX installer should be launched automatically.",
                    "SlimDX does not seem to be installed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Application.Exit();
            }
        }
        public void init(Form1 mainForm)
        {
            isMousePressed = false;
            this.mainForm = mainForm;

            this.openHandCursor = Form1.CreateCursor(Properties.Resources.handcursor, 0, 0);


            this.rotationCursor = Form1.CreateCursor(new Bitmap(Properties.Resources.refreshIcon,
                new Size(Properties.Resources.refreshIcon.Width / 2, Properties.Resources.refreshIcon.Height / 2)),
                Properties.Resources.refreshIcon.Width / 4, Properties.Resources.refreshIcon.Height / 4);

            this.createShapeCursor = Form1.CreateCursor(new Bitmap(Properties.Resources.editIcon,
                new Size(Properties.Resources.editIcon.Width / 2, Properties.Resources.editIcon.Height / 2)),
                0, Properties.Resources.editIcon.Height / 2);
        }


        public void SetDefaultCursor()
        {
            this.Cursor = this.openHandCursor;
        }

        public void SetRotationCursor()
        {
            this.Cursor = this.rotationCursor;
        }

        public void SetEditCursor()
        {
            this.Cursor = this.createShapeCursor;
        }

        public void setModeMovingFocus()
        {
            movingMode = "FOCUS";
            this.Mode = "FOCUS";
            Scene sceneSelected = this.mainForm.getElementTreeView().SceneSelected;
            if (sceneSelected != null)
            {
                sceneSelected.deselectAllControls();
                sceneSelected.deselectAllObjects();

                this.setModeSceneEditor(sceneSelected);
            }


            this.objectsSelected.Clear();
            this.joysticksSelected.Clear();
            this.mainForm.propertyGrid1.SelectedObjects = null;

            //this.surfacePictBx.Refresh();
            Gorgon.Go();
        }

        public void setModeMovingObjects()
        {
            if (!movingMode.Equals("OBJECT"))
            {

                Scene scene = this.mainForm.getElementTreeView().SceneSelected;
                if (scene != null)
                {
                    scene.deselectAllObjects();
                }

                this.objectsSelected.Clear();
            }

            movingMode = "OBJECT";
            this.Mode = "NONE";

            Scene sceneSelected = this.mainForm.getElementTreeView().SceneSelected;
            if (sceneSelected != null)
            {
                sceneSelected.deselectAllControls();

            }


            this.joysticksSelected.Clear();
            this.mainForm.propertyGrid1.SelectedObjects = null;
            GorgonLibrary.Gorgon.Go();
            
            this.mainForm.getElementTreeView().refreshNodesSelectedSceneEditor();
        }

        public void setModeMovingEntities()
        {
            movingMode = "ENTITY";
            this.Mode = "NONE";

            Scene sceneSelected = this.mainForm.getElementTreeView().SceneSelected;
            if (sceneSelected != null)
            {
                sceneSelected.deselectAllControls();
                sceneSelected.deselectAllObjects();
            }

          
            this.joysticksSelected.Clear();
            this.mainForm.propertyGrid1.SelectedObjects = null;
            GorgonLibrary.Gorgon.Go();

            this.mainForm.getElementTreeView().refreshNodesSelectedSceneEditor();
        }

        public void setModeMovingJoint()
        {
            movingMode = "JOINT";
            this.Mode = "JOINT";

            Scene sceneSelected = this.mainForm.getElementTreeView().SceneSelected;
            if (sceneSelected != null)
            {
                sceneSelected.deselectAllControls();
                sceneSelected.deselectAllObjects();
            }

            this.objectsSelected.Clear();
            this.joysticksSelected.Clear();
            this.mainForm.propertyGrid1.SelectedObjects = null;
            GorgonLibrary.Gorgon.Go();
            this.mainForm.getElementTreeView().refreshNodesSelectedSceneEditor();
        }

        public void setModeMovingAddSurface()
        {
            movingMode = "AD";
            this.Mode = "MOVE";
            Scene sceneSelected = this.mainForm.getElementTreeView().SceneSelected;
            if (sceneSelected != null)
            {
                sceneSelected.deselectAllControls();
                sceneSelected.deselectAllObjects();
            }

            this.objectsSelected.Clear();
            this.joysticksSelected.Clear();
            this.mainForm.propertyGrid1.SelectedObjects = null;
            GorgonLibrary.Gorgon.Go();
            this.mainForm.getElementTreeView().refreshNodesSelectedSceneEditor();
        }

        public void setModeMovingControls()
        {
            movingMode = "CONTROL";
            this.Mode = "MOVE";
            Scene sceneSelected = this.mainForm.getElementTreeView().SceneSelected;
            if (sceneSelected != null)
            {
                sceneSelected.deselectAllControls();
                sceneSelected.deselectAllObjects();
            }

            this.objectsSelected.Clear();
            this.joysticksSelected.Clear();
            this.mainForm.propertyGrid1.SelectedObjects = null;
            GorgonLibrary.Gorgon.Go();
            this.mainForm.getElementTreeView().refreshNodesSelectedSceneEditor();
        }

        public void SetScrollMax()
        {
            Scene scene = this.mainForm.getElementTreeView().SceneSelected;
            if (scene != null)
            {

                this.vScrollBar.Maximum = 0;
                this.hScrollBar.Maximum = (int)(Math.Abs(scene.Size.Width));

                this.vScrollBar.Minimum = -(int)(Math.Abs(scene.Size.Height));
                this.hScrollBar.Minimum = 0;
            }
        }

        public Point GetScrollMaxNormalValues()
        {
            return new Point(this.hScrollBar.Maximum, -this.vScrollBar.Minimum);
        }

        public void zoomAvant()
        {
            Scene scene = this.mainForm.getElementTreeView().SceneSelected;
            if (scene != null)
            {
                this.CurrentScale = this.CurrentScale * 2;
                if (this.CurrentScale > 4) this.CurrentScale = 4;

                this.SetScrollMax();


                //Get mouse location 
                //Point currentOffsetpoint = this.getOffsetPoint();
                //Point ControlCenterTranslated = new Point(
                //    (this.surfacePictBx.Width / 2) - currentOffsetpoint.X,
                //    (this.surfacePictBx.Height / 2) - currentOffsetpoint.Y);

                Point finalScrollPoint = new Point(currentMousePos.X - (int)((this.surfacePictBx.Width / 2) * (1 / this.CurrentScale)),
                    currentMousePos.Y - (int)((this.surfacePictBx.Height / 2) * (1 / this.CurrentScale)));

                this.scrollView(finalScrollPoint.X, finalScrollPoint.Y);

                if (this.mainForm.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();
            }
        }

        public void zoomArriere()
        {
            Scene scene = this.mainForm.getElementTreeView().SceneSelected;
            if (scene != null)
            {
                this.CurrentScale = this.CurrentScale / 2;
                if (this.CurrentScale < 0.25f) this.CurrentScale = 0.25f;

                this.SetScrollMax();

                //Get mouse location 
                //Point currentOffsetpoint = this.getOffsetPoint();
                //Point ControlCenterTranslated = new Point(
                //    (this.surfacePictBx.Width / 2) - currentOffsetpoint.X,
                //    (this.surfacePictBx.Height / 2) - currentOffsetpoint.Y);

                Point finalScrollPoint = new Point(currentMousePos.X - (int)((this.surfacePictBx.Width / 2)*(1/this.CurrentScale)),
                    currentMousePos.Y - (int)((this.surfacePictBx.Height / 2) * (1 / this.CurrentScale)));

                this.scrollView(finalScrollPoint.X, finalScrollPoint.Y);

                
                if (this.mainForm.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();
            }
        }

        public void resizeScene(Size size)
        {
            Scene scene = this.mainForm.getElementTreeView().SceneSelected;
            if (scene != null)
            {

                scene.InitialSize = size;
                scene.Size = size;

                this.SetScrollMax();

                GorgonLibrary.Gorgon.Go();
            }


        }

        public void setModeNormal()
        {
            this.objectsSelected.Clear();

            this.CurentCalque = "NONE";

            //Mettre la picture de la size de l'objet
            this.surfacePictBx.Size = this.Size;

            if (this.mainForm.isFormLocked == false)
                GorgonLibrary.Gorgon.Go(); 

        }


        
        public void setModeSceneEditor(Scene scene)
        {
            ////Afficher ou non la check box d'affichage de la tilesmap si necessaire
            //bool res = false;
            //for (int i = 0; i < scene.Layers.Count; i++)
            //{
            //    if (scene.Layers[i].TilesMap != null)
            //    {
            //        res = true;
            //        break;
            //    }
            //}

            //this.mainForm.scenePreview1.showTilesMapChkBx.Visible = res;


            this.objectsSelected.Clear();

            this.mainForm.getElementTreeView().SceneSelected = scene;
            this.CurentCalque = "STAGE";

            this.SetScrollMax();
            

            this.scrollView(scene.CurrentSceneViewLocation.X, scene.CurrentSceneViewLocation.Y);
            for (int i = 0; i < scene.projectParent.Scenes.Count; i++)
            {
                if (scene == scene.projectParent.Scenes[i])
                {
                    this.indexSceneSelected = i;
                    break;
                }

            }

            this.indexLayerSelected = -1;
            GorgonLibrary.Gorgon.Go();
        }

        public void setModeLayerEditor(CoronaLayer layer)
        {
            this.objectsSelected.Clear();
            layer.deselectAllObjects();
            layer.deselectAllControls();
            this.mainForm.getElementTreeView().SceneSelected = layer.SceneParent;
            this.CurentCalque = "LAYER";

            this.indexLayerSelected = layer.IndexLayer;
            GorgonLibrary.Gorgon.Go();
        }

        private void dessineQuadrillage(Graphics g, Point offSetPoint)
        {
            int largeurCellule = 50;
            int hauteurCellule = 50;
            if (largeurCellule <= 0) largeurCellule = 1;
            if (hauteurCellule <= 0) hauteurCellule = 1;

            int offSetX = offSetPoint.X % 50;
            int offSetY = offSetPoint.Y % 50;
            if (offSetX > 50) offSetX = offSetX - 50;
            if (offSetY > 50) offSetY = offSetY - 50;

            int nbLignes = (int)(this.surfacePictBx.Size.Width * (1 / CurrentScale) / largeurCellule);
            int nbColonnes = (int)(this.surfacePictBx.Size.Height * (1 / CurrentScale) / hauteurCellule);

            float[] dashValues = { 1, 1 };
            Pen pen = new Pen(Color.FromArgb(150, Color.LightGray), 2);
            pen.DashPattern = dashValues;

            int viewHeight = (int)(this.surfacePictBx.Size.Height * (1 / CurrentScale));
            int viewWidth = (int)(this.surfacePictBx.Size.Width * (1 / CurrentScale));
            //Creer les collones
            for (int i = 0; i < nbLignes + 2; i++)
            {
                g.DrawLine(pen, new Point(i * largeurCellule + offSetX, 0), new Point(i * largeurCellule + offSetX, viewHeight));
            }

            //Creer les lignes
            for (int i = 0; i < nbColonnes + 2; i++)
            {
                g.DrawLine(pen, new Point(0, i * hauteurCellule + offSetY), new Point(viewWidth, i * hauteurCellule + offSetY));
            }
        }

        private void dessineGorgonQuadrillage(Point offSetPoint, float worldScale)
        {
            int largeurCellule = (int)(50.0f * worldScale);
            int hauteurCellule = (int)(50.0f * worldScale);

            int offSetX = (int)((float)offSetPoint.X * worldScale) % largeurCellule;
            int offSetY = (int)((float)offSetPoint.Y * worldScale) % hauteurCellule;
            if (offSetX > largeurCellule) offSetX = offSetX - largeurCellule;
            if (offSetY > hauteurCellule) offSetY = offSetY - hauteurCellule;

            int nbLignes = (int)((float)this.surfacePictBx.Size.Width / largeurCellule);
            int nbColonnes = (int)(this.surfacePictBx.Size.Height / hauteurCellule);

            int viewHeight = this.surfacePictBx.Size.Height;
            int viewWidth = this.surfacePictBx.Size.Width;
            //Creer les collones
            for (int i = 0; i < nbLignes + 2; i++)
            {
                GorgonGraphicsHelper.Instance.DrawVerticalDottedLine(new Rectangle(i * largeurCellule + offSetX, 0, 5, viewHeight),
                    Color.FromArgb(150,  Color.LightGray),1, 2, this.CurrentScale);
            }

            //Creer les lignes
            for (int i = 0; i < nbColonnes + 2; i++)
            {
                GorgonGraphicsHelper.Instance.DrawHorizontalDottedLine(new Rectangle(0, i * hauteurCellule + offSetY, viewWidth, i * hauteurCellule + offSetY),
                    Color.FromArgb(150, Color.LightGray), 2,1, this.CurrentScale);
               
            }
        }

        public Point getOffsetPoint()
        {
            Point p = new Point(-this.hScrollBar.Value, this.vScrollBar.Value);
            return p;
        }

        //---------------------------------------------------
        //-------------------Events------------------------
        //---------------------------------------------------
        private void surfacePictBx_Paint(object sender, PaintEventArgs e)
        {
            if (GorgonLibrary.Gorgon.IsInitialized == true)
                 Gorgon.Go();

            //try
            //{

                //e.Graphics.CompositingMode = CompositingMode.SourceOver;
                //e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                //e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
                //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                //e.Graphics.SmoothingMode = SmoothingMode.None;

            //    System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            //    m.Scale(this.CurrentScale, this.CurrentScale);

            //    e.Graphics.Transform = m;
            //    if (this.CurentCalque.Equals("STAGE"))
            //    {
            //        Scene scene = this.mainForm.getElementTreeView().SceneSelected;

            //        if (scene != null)
            //        {

            //            if (scene.DefaultColor == Color.Empty)
            //                scene.DefaultColor = Color.Black;

            //            e.Graphics.Clear(scene.DefaultColor);
            //        }
            //    }
            //    else if (this.CurentCalque.Equals("LAYER"))
            //    {
            //        CoronaLayer layer = this.mainForm.getElementTreeView().LayerSelected;
            //        if (layer != null)
            //        {
            //            Scene scene = layer.SceneParent;
            //            if (scene.DefaultColor == Color.Empty)
            //                scene.DefaultColor = Color.Black;

            //            e.Graphics.Clear(scene.DefaultColor);
            //        }
            //    }

            //    Point offsetPoint = this.getOffsetPoint();

            //    if (Settings1.Default.ShowGrid == true)
            //    {
            //        dessineQuadrillage(e.Graphics, offsetPoint);
            //    }
               

            //    if (this.CurentCalque.Equals("STAGE"))
            //    {
            //        Scene scene = this.mainForm.getElementTreeView().SceneSelected;

            //        if (scene != null)
            //        {

            //            for (int i = 0; i < scene.Layers.Count; i++)
            //            {
            //                CoronaLayer layer = scene.Layers[i];
            //                if (layer.isEnabled == true)
            //                {
            //                    if (layer.TilesMap != null)
            //                    {
            //                        if (layer.TilesMap.isEnabled == true)
            //                        {
            //                            Rectangle rect = new Rectangle(new Point(-offsetPoint.X, -offsetPoint.Y), this.surfacePictBx.Size);
            //                            layer.TilesMap.setSurfaceVisible(rect, this.CurrentScale, this.CurrentScale);
            //                        }
            //                    }
            //                }
            //            }
            //            scene.dessineScene(e.Graphics, this.CurrentScale, this.CurrentScale, offsetPoint);



            //            //Dessiner la shape en construction 
            //            if (this.FigActive != null)
            //            {
            //                e.Graphics.Transform = m;
            //                this.FigActive.Dessine(e.Graphics, 255, offsetPoint);

            //            }

            //            if (this.Mode.Equals("CAMERA_RECTANGLE"))
            //            {
            //                Rectangle rect = new Rectangle(new Point(scene.Camera.CameraFollowLimitRectangle.X + offsetPoint.X, scene.Camera.CameraFollowLimitRectangle.Y + offsetPoint.Y),
            //                                                scene.Camera.CameraFollowLimitRectangle.Size);
            //                e.Graphics.DrawRectangle(Pens.DarkGoldenrod, rect);
            //            }
            //        }

            //    }
            //    else if (this.CurentCalque.Equals("LAYER"))
            //    {
            //        CoronaLayer layer = this.mainForm.getElementTreeView().LayerSelected;
            //        if (layer != null)
            //        {
                        
            //            //Definir la zone d'affichage des tiles
            //            if (layer.TilesMap != null)
            //            {
            //                if (layer.TilesMap.isEnabled == true)
            //                {
            //                    Rectangle rect = new Rectangle(new Point(-offsetPoint.X, -offsetPoint.Y), this.surfacePictBx.Size);

            //                    layer.TilesMap.setSurfaceVisible(rect, this.CurrentScale, this.CurrentScale);
            //                }
            //            }


            //            layer.dessineLayer(e.Graphics, this.CurrentScale, this.CurrentScale, offsetPoint);

            //            //Dessiner la shape en construction 
            //            if (this.FigActive != null)
            //            {
            //                e.Graphics.Transform = m;
            //                this.FigActive.Dessine(e.Graphics, 255, offsetPoint);
            //            }

            //            //Dessiner la surface du text temporaire
            //            if (this.surfaceTextTemp != Rectangle.Empty)
            //            {
            //                e.Graphics.Transform = m;
            //                e.Graphics.DrawRectangle(Pens.YellowGreen,new Rectangle(new Point(this.surfaceTextTemp.X + offsetPoint.X,
            //                                                                                    this.surfaceTextTemp.Y + offsetPoint.Y),
            //                                                                        this.surfaceTextTemp.Size));
                                                        
            //            }

            //            if (layer.SceneParent.Camera.isSurfaceFocusVisible == true)
            //            {
            //                e.Graphics.Transform = m;
            //                SolidBrush br = new SolidBrush(Color.FromArgb(80, Color.DarkGray));

            //                GraphicsPath path = new GraphicsPath();
            //                path.AddRectangle(new Rectangle(new Point(offsetPoint.X, offsetPoint.Y),
            //                    new Size((int)e.Graphics.ClipBounds.Width - offsetPoint.X, (int)e.Graphics.ClipBounds.Height - offsetPoint.Y)));


            //                if (layer.SceneParent.projectParent.Orientation == CoronaGameProject.OrientationScreen.Portrait)
            //                    layer.SceneParent.Camera.SurfaceFocus = new Rectangle(layer.SceneParent.Camera.SurfaceFocus.Location, new Size(layer.SceneParent.projectParent.width, layer.SceneParent.projectParent.height));
            //                else
            //                    layer.SceneParent.Camera.SurfaceFocus = new Rectangle(layer.SceneParent.Camera.SurfaceFocus.Location, new Size(layer.SceneParent.projectParent.height, layer.SceneParent.projectParent.width));


            //                Point pDestFocus = new Point(offsetPoint.X + layer.SceneParent.Camera.SurfaceFocus.Location.X, offsetPoint.Y + layer.SceneParent.Camera.SurfaceFocus.Location.Y);
            //                path.AddRectangle(new Rectangle(pDestFocus, layer.SceneParent.Camera.SurfaceFocus.Size));

            //                e.Graphics.FillPath(br, path);

            //            }
            //        }

            //    }

            //    if (this.mainForm != null)
            //    {
            //        UserControl jointPanel = this.mainForm.CurrentJointPanel;

            //        //Verifier que le panel n'est pas null
            //        if (jointPanel != null)
            //        {
            //            Graphics g = e.Graphics;
            //            g.Transform = m;

            //            //Recuperer le panel correspondant au joint
            //            if (jointPanel.Name.Equals("DISTANCE"))
            //            {
            //                DistancePropertiesPanel panelDistance = (DistancePropertiesPanel)jointPanel;

            //                if (panelDistance.anchorPointA != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelDistance.anchorPointA.X - 3, offsetPoint.Y + panelDistance.anchorPointA.Y - 3);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor A", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelDistance.anchorPointB != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelDistance.anchorPointB.X - 3, offsetPoint.Y + panelDistance.anchorPointB.Y - 3);
            //                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor B", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelDistance.anchorPointA != Point.Empty && panelDistance.anchorPointB != Point.Empty)
            //                {
            //                    Point pDest1 = new Point(offsetPoint.X + panelDistance.anchorPointA.X, offsetPoint.Y + panelDistance.anchorPointA.Y);
            //                    Point pDest2 = new Point(offsetPoint.X + panelDistance.anchorPointB.X, offsetPoint.Y + panelDistance.anchorPointB.Y);
            //                    g.DrawLine(Pens.Blue, pDest1, pDest2);
            //                }
            //            }
            //            else if (jointPanel.Name.Equals("FRICTION"))
            //            {
            //                FrictionPropertiesPanel panelFriction = (FrictionPropertiesPanel)jointPanel;

            //                if (panelFriction.anchorPoint != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelFriction.anchorPoint.X - 3, offsetPoint.Y + panelFriction.anchorPoint.Y - 3);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }
            //            }
            //            else if (jointPanel.Name.Equals("PISTON"))
            //            {
            //                PistonPropertiesPanel panelPiston = (PistonPropertiesPanel)jointPanel;

            //                if (panelPiston.anchorPoint != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelPiston.anchorPoint.X - 3, offsetPoint.Y + panelPiston.anchorPoint.Y - 3);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelPiston.axisDistance != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelPiston.axisDistance.X - 3, offsetPoint.Y + panelPiston.axisDistance.Y - 3);
            //                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Axis Distance", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelPiston.anchorPoint != Point.Empty && panelPiston.axisDistance != Point.Empty)
            //                {
            //                    Point pDest1 = new Point(offsetPoint.X + panelPiston.anchorPoint.X, offsetPoint.Y + panelPiston.anchorPoint.Y);
            //                    Point pDest2 = new Point(offsetPoint.X + panelPiston.axisDistance.X, offsetPoint.Y + panelPiston.axisDistance.Y);
            //                    g.DrawLine(Pens.Red, pDest1, pDest2);
            //                }
            //            }
            //            else if (jointPanel.Name.Equals("PIVOT"))
            //            {
            //                PivotPropertiesPanel panelPivot = (PivotPropertiesPanel)jointPanel;

            //                if (panelPivot.anchorPoint != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelPivot.anchorPoint.X - 3, offsetPoint.Y + panelPivot.anchorPoint.Y - 3);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }
            //            }
            //            else if (jointPanel.Name.Equals("PULLEY"))
            //            {
            //                PulleyPropertiesPanel panelPulley = jointPanel as PulleyPropertiesPanel;

            //                if (panelPulley.anchorPointA != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelPulley.anchorPointA.X - 3 + panelPulley.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.anchorPointA.Y - 3 + panelPulley.objA.DisplayObject.SurfaceRect.Y);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Rope Anchor A", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelPulley.anchorPointB != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelPulley.anchorPointB.X - 3 + panelPulley.objB.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.anchorPointB.Y - 3 + panelPulley.objB.DisplayObject.SurfaceRect.Y);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Rope Anchor B", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelPulley.anchorPointA != Point.Empty && panelPulley.anchorPointB != Point.Empty)
            //                {
            //                    Point pDest1 = new Point(offsetPoint.X + panelPulley.anchorPointA.X + panelPulley.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.anchorPointA.Y + panelPulley.objA.DisplayObject.SurfaceRect.Y);
            //                    Point pDest2 = new Point(offsetPoint.X + panelPulley.anchorPointB.X + panelPulley.objB.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.anchorPointB.Y + panelPulley.objB.DisplayObject.SurfaceRect.Y);
            //                    g.DrawLine(Pens.Green, pDest1, pDest2);
            //                }

            //                if (panelPulley.objAnchorPointA != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelPulley.objAnchorPointA.X - 3 + panelPulley.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.objAnchorPointA.Y - 3 + panelPulley.objA.DisplayObject.SurfaceRect.Y);
            //                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor A", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);

            //                    pDest = new Point(offsetPoint.X + panelPulley.objAnchorPointA.X + panelPulley.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.objAnchorPointA.Y + panelPulley.objA.DisplayObject.SurfaceRect.Y);
            //                    Point pDestRopeA = new Point(offsetPoint.X + panelPulley.anchorPointA.X + panelPulley.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.anchorPointA.Y + panelPulley.objA.DisplayObject.SurfaceRect.Y);
            //                    g.DrawLine(Pens.DarkBlue, pDestRopeA, pDest);
            //                }

            //                if (panelPulley.objAnchorPointB != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelPulley.objAnchorPointB.X - 3 + panelPulley.objB.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.objAnchorPointB.Y - 3 + panelPulley.objB.DisplayObject.SurfaceRect.Y);
            //                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor B", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);

            //                    pDest = new Point(offsetPoint.X + panelPulley.objAnchorPointB.X + panelPulley.objB.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelPulley.objAnchorPointB.Y + panelPulley.objB.DisplayObject.SurfaceRect.Y);
            //                    Point pDestRopeB = new Point(offsetPoint.X + panelPulley.anchorPointB.X + panelPulley.objB.DisplayObject.SurfaceRect.X
            //                        , offsetPoint.Y + panelPulley.anchorPointB.Y + panelPulley.objB.DisplayObject.SurfaceRect.Y);
            //                    g.DrawLine(Pens.DarkBlue, pDestRopeB, pDest);
            //                }

            //            }
            //            else if (jointPanel.Name.Equals("WELD"))
            //            {
            //                WeldPropertiesPanel panelWeld = (WeldPropertiesPanel)jointPanel;

            //                if (panelWeld.anchorPoint != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelWeld.anchorPoint.X - 3 + panelWeld.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelWeld.anchorPoint.Y - 3 + panelWeld.objA.DisplayObject.SurfaceRect.Y);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }
            //            }
            //            else if (jointPanel.Name.Equals("WHEEL"))
            //            {
            //                WheelPropertiesPanel panelWheel = (WheelPropertiesPanel)jointPanel;

            //                if (panelWheel.anchorPoint != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelWheel.anchorPoint.X - 3 + panelWheel.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelWheel.anchorPoint.Y - 3 +panelWheel.objA.DisplayObject.SurfaceRect.Y);
            //                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Anchor", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelWheel.axisDistance != Point.Empty)
            //                {
            //                    Point pDest = new Point(offsetPoint.X + panelWheel.axisDistance.X - 3 + panelWheel.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelWheel.axisDistance.Y - 3 + panelWheel.objA.DisplayObject.SurfaceRect.Y);
            //                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(6, 6)));
            //                    g.DrawString("Axis Distance", new System.Drawing.Font("ARIAL", 10), Brushes.Red, pDest);
            //                }

            //                if (panelWheel.anchorPoint != Point.Empty && panelWheel.axisDistance != Point.Empty)
            //                {
            //                    Point pDest1 = new Point(offsetPoint.X + panelWheel.anchorPoint.X + panelWheel.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelWheel.anchorPoint.Y + panelWheel.objA.DisplayObject.SurfaceRect.Y);
            //                    Point pDest2 = new Point(offsetPoint.X + panelWheel.axisDistance.X + panelWheel.objA.DisplayObject.SurfaceRect.X,
            //                        offsetPoint.Y + panelWheel.axisDistance.Y + panelWheel.objA.DisplayObject.SurfaceRect.Y);
            //                    g.DrawLine(Pens.Red, pDest1, pDest2);
            //                }
            //            }
            //            g.ResetTransform();
            //        }


            //        if (this.isMousePressed == false && this.isScrolling == false)
            //            this.mainForm.scenePreview1.Refresh();
            //    }


            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error stage painting !\n" + ex.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //    //----------------------------------------------------------------------------------------------------
            //    //-------------------------------------- EVENT REPORTING ---------------------------------------------
            //    //----------------------------------------------------------------------------------------------------
            //    Krea.KreaEventReports.NativeKreaEvent kreaEvent = new Krea.KreaEventReports.NativeKreaEvent(ex);
            //    if (Settings1.Default.KreaEventReportAutomaticSend == true)
            //    {
            //        KreaEventReports.KreaEventReportSender.ReportEvent(kreaEvent);
            //    }
            //    else
            //    {
            //        KreaEventReports.KreaEventReporterForm reportForm = new KreaEventReports.KreaEventReporterForm();
            //        reportForm.init(kreaEvent);
            //        reportForm.Show();
            //    }
            //    //----------------------------------------------------------------------------------------------------
            //    //----------------------------------------------------------------------------------------------------
            //}



        }


        //Mousse pressed --------------------------------------------------------------------------------------------------
        private void surfacePictBx_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePressed = true;
            Point offSetPoint = this.getOffsetPoint();
            Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.CurrentScale)),
                    Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.CurrentScale)));


            GameElementTreeView treeView = this.mainForm.getElementTreeView();
            Scene sceneSelected = treeView.SceneSelected;

            this.pDepart = pTouched;
            if (CurentCalque.Equals("LAYER"))
            {
                CoronaLayer layer = treeView.LayerSelected;
                if (layer != null)
                {


                    if (this.Mode.Equals("MOVE") || this.Mode.Contains("RESIZE") || this.Mode.Equals("ROTATION") || this.Mode.Equals("DELETE"))
                    {

                        if (this.movingMode.Equals("OBJECT"))
                        {
                            for (int i = 0; i < this.objectsSelected.Count; i++)
                            {
                                CoronaObject coronaObject = this.objectsSelected[i];
                                if (coronaObject.isEnabled == true)
                                {
                                    if (coronaObject.isEntity == false)
                                    {
                                        coronaObject.setSelected(true);
                                        coronaObject.DisplayObject.LastPos = pTouched;
                                        if (coronaObject.DisplayObject.Type.Equals("FIGURE"))
                                        {
                                            coronaObject.DisplayObject.Figure.LastPos = pTouched;
                                        }
                                    }
                                }

                            }

                            moveObjectActive = true;
                        }
                        else if (this.movingMode.Equals("ENTITY"))
                        {
                            for (int i = 0; i < this.objectsSelected.Count; i++)
                            {
                                CoronaObject entity = this.objectsSelected[i];
                                if (entity.isEnabled == true)
                                {
                                    if (entity.isEntity == true)
                                    {
                                        for (int j = 0; j < entity.Entity.CoronaObjects.Count; j++)
                                        {
                                            CoronaObject coronaObject = entity.Entity.CoronaObjects[j];

                                            coronaObject.DisplayObject.LastPos = pTouched;
                                            if (coronaObject.DisplayObject.Type.Equals("FIGURE"))
                                            {
                                                coronaObject.DisplayObject.Figure.LastPos = pTouched;
                                            }
                                        }


                                    }
                                }

                            }

                            moveObjectActive = true;
                        }
                        else if (this.movingMode.Equals("CONTROL"))
                        {
                            this.moveSurfaceControlActive = true;
                            for (int i = 0; i < this.joysticksSelected.Count; i++)
                            {
                                JoystickControl joy = this.joysticksSelected[i];
                                if(joy.isEnabled == true)
                                    joy.lastPos = pTouched;
                            }
                        }



                    }
                    else if (this.Mode.Equals("CREATE_SHAPE"))
                    {
                        pDepart = pTouched;
                        //Si un rectangle
                        if (this.mainForm.ShapeType == 1)
                        {
                            this.FigActive = new Rect(pTouched, 10, 10, false, Color.Empty, Color.White, 2, false, null);

                        }
                        //Si une Ligne
                        else if (this.mainForm.ShapeType == 2)
                        {

                            //Creer la ligne si elle ne l'est pas
                            if (isLineCreated == false)
                            {
                                this.FigActive = new Line(pTouched, Color.White, 2, false, null);
                                isLineCreated = true;


                            }
                            else
                            {

                                //Sinon ajouter seulement le point au tableau
                                Line line = this.FigActive as Line;
                                line.Points.Add(pTouched);

                            }
                        }
                        //Si un Cercle
                        else if (this.mainForm.ShapeType == 3)
                        {
                            this.FigActive = new Cercle(pTouched, 2, Color.Empty, Color.White, 2, false, null);

                        }
                        //Si un Texte
                        else if (this.mainForm.ShapeType == 4)
                        {
                            Font2 font = new Font2("ARIAL", 16, FontStyle.Regular);
                            //this.FigActive = new Texte(pDepart, "New Text", font, Color.Blue, 1, false, null);

                            this.surfaceTextTemp = new Rectangle(pTouched, TextRenderer.MeasureText("New Text", new System.Drawing.Font(font.FamilyName, font.Size)));
                            font = null;
                        }
                        //Si un une courbe de bezier
                        else if (this.mainForm.ShapeType == 5)
                        {
                            //Creer le bezier s'il ne l'est pas
                            if (isBezierCreated == false)
                            {
                                List<Point> tabP = new List<Point>();
                                tabP.Add(pTouched);

                                this.FigActive = new CourbeBezier(pTouched, tabP, Color.Empty, Color.White, 2, false, null);
                                isBezierCreated = true;


                            }
                            else
                            {

                                //Sinon ajouter seulement le point au tableau
                                CourbeBezier bezier = this.FigActive as CourbeBezier;
                                bezier.UserPoints.Add(pTouched);

                            }




                        }
                        //Si un rectangle arrondi
                        else if (this.mainForm.ShapeType == 6)
                        {
                            this.FigActive = new Rect(pTouched, 10, 10, true, Color.Empty, Color.White, 2, false, null);
                        }
                    }
                    else if (this.Mode.Equals("PATH_FOLLOW"))
                    {
                        if (this.objectsSelected.Count > 0)
                        {
                            CoronaObject obj = objectsSelected[0];
                            if (obj != null)
                            {
                                if (obj.isEnabled == true)
                                {
                                    if (obj.isEntity == false)
                                        obj.PathFollow.Path.Add(pTouched);
                                }

                            }

                        }
                    }


                }
            }
            else if (CurentCalque.Equals("STAGE"))
            {
                if (sceneSelected != null)
                {
                    if (this.Mode.Equals("CAMERA_RECTANGLE"))
                    {
                        pDepart = pTouched;
                        sceneSelected.Camera.CameraFollowLimitRectangle = new Rectangle(pTouched, new Size(10, 10));
                    }
                    else if (this.Mode.Equals("CREATE_SHAPE"))
                    {
                        if (treeView.CoronaObjectSelected != null)
                        {
                            if (treeView.CoronaObjectSelected.isEntity == true)
                            {
                                pDepart = pTouched;
                                //Si un rectangle
                                if (this.mainForm.ShapeType == 1)
                                {
                                    this.FigActive = new Rect(pTouched, 10, 10, false, Color.Empty, Color.White, 2, false, null);

                                }
                                //Si une Ligne
                                else if (this.mainForm.ShapeType == 2)
                                {

                                    //Creer la ligne si elle ne l'est pas
                                    if (isLineCreated == false)
                                    {
                                        this.FigActive = new Line(pTouched, Color.White, 2, false, null);
                                        isLineCreated = true;


                                    }
                                    else
                                    {

                                        //Sinon ajouter seulement le point au tableau
                                        Line line = this.FigActive as Line;
                                        line.Points.Add(pTouched);

                                    }
                                }
                                //Si un Cercle
                                else if (this.mainForm.ShapeType == 3)
                                {
                                    this.FigActive = new Cercle(pTouched, 2, Color.Empty, Color.White, 2, false, null);

                                }
                                //Si un Texte
                                else if (this.mainForm.ShapeType == 4)
                                {
                                    Font2 font = new Font2("ARIAL", 16, FontStyle.Regular);
                                    //this.FigActive = new Texte(pDepart, "New Text", font, Color.Blue, 1, false, null);

                                    this.surfaceTextTemp = new Rectangle(pTouched, TextRenderer.MeasureText("New Text", new System.Drawing.Font(font.FamilyName, font.Size)));
                                    font = null;
                                }
                                //Si un une courbe de bezier
                                else if (this.mainForm.ShapeType == 5)
                                {
                                    //Creer le bezier s'il ne l'est pas
                                    if (isBezierCreated == false)
                                    {
                                        List<Point> tabP = new List<Point>();
                                        tabP.Add(pTouched);

                                        this.FigActive = new CourbeBezier(pTouched, tabP, Color.Empty, Color.White, 2, false, null);
                                        isBezierCreated = true;


                                    }
                                    else
                                    {

                                        //Sinon ajouter seulement le point au tableau
                                        CourbeBezier bezier = this.FigActive as CourbeBezier;
                                        bezier.UserPoints.Add(pTouched);

                                    }

                                }
                                //Si un rectangle arrondi
                                else if (this.mainForm.ShapeType == 6)
                                {
                                    this.FigActive = new Rect(pTouched, 10, 10, true, Color.Empty, Color.White, 2, false, null);
                                }
                            }
                            else
                            {
                                this.Mode = "NONE";
                                this.isMousePressed = false;
                                MessageBox.Show("Please select a layer before adding a new display object!", "Select a layer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                        else
                        {
                            this.Mode = "NONE";
                            this.isMousePressed = false;
                            MessageBox.Show("Please select a layer before adding a new display object!", "Select a layer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else if (this.Mode.Equals("MOVE") || this.Mode.Contains("RESIZE") || this.Mode.Equals("ROTATION"))
                    {
                        if (this.movingMode.Equals("FOCUS") && this.Mode.Equals("MOVE"))
                        {
                            if (sceneSelected.Camera.SurfaceFocus.Contains(pTouched))
                            {
                                sceneSelected.lastPos = pTouched;
                                this.moveFocusSceneActive = true;
                            }
                        }
                        else if (this.movingMode.Equals("OBJECT"))
                        {
                            for (int i = 0; i < this.objectsSelected.Count; i++)
                            {
                                CoronaObject coronaObject = this.objectsSelected[i];
                                if (coronaObject.isEnabled == true)
                                {
                                    if (coronaObject.isEntity == false)
                                    {
                                        coronaObject.DisplayObject.LastPos = pTouched;
                                        coronaObject.PhysicsBody.OriginSize = coronaObject.DisplayObject.SurfaceRect.Size;
                                        if (coronaObject.DisplayObject.Type.Equals("FIGURE"))
                                        {
                                            coronaObject.DisplayObject.Figure.LastPos = pTouched;
                                        }
                                    }
                                }
                            }

                            moveObjectActive = true;


                        }
                        else if (this.movingMode.Equals("ENTITY"))
                        {
                            for (int i = 0; i < this.objectsSelected.Count; i++)
                            {
                                CoronaObject entity = this.objectsSelected[i];
                                if (entity.isEnabled == true)
                                {

                                    if (entity.isEntity == true)
                                    {
                                        for (int j = 0; j < entity.Entity.CoronaObjects.Count; j++)
                                        {
                                            CoronaObject coronaObject = entity.Entity.CoronaObjects[j];
                                            coronaObject.DisplayObject.LastPos = pTouched;
                                            if (coronaObject.DisplayObject.Type.Equals("FIGURE"))
                                            {
                                                coronaObject.DisplayObject.Figure.LastPos = pTouched;
                                            }
                                        }


                                    }
                                }
                            }

                            moveObjectActive = true;
                        }
                        else if (this.movingMode.Equals("AD"))
                        {

                            if (sceneSelected.Ad.isActive == true)
                            {
                                Rectangle rect = new Rectangle(
                                    new Point(sceneSelected.Ad.location.X + offSetPoint.X + sceneSelected.Camera.SurfaceFocus.Location.X,
                                        sceneSelected.Ad.location.Y+ offSetPoint.Y + sceneSelected.Camera.SurfaceFocus.Location.Y),
                                    sceneSelected.Ad.size);

                                if (rect.Contains(e.Location))
                                {
                                    sceneSelected.lastPos = pTouched;
                                    this.moveSurfaceAdActive = true;
                                }
                            }

                        }
                        else if (this.movingMode.Equals("CONTROL"))
                        {
                            this.moveSurfaceControlActive = true;
                            for (int i = 0; i < this.joysticksSelected.Count; i++)
                            {
                                JoystickControl joy = this.joysticksSelected[i];
                                if(joy.isEnabled == true)
                                    joy.lastPos = pTouched;
                            }
                        }


                    }
                    else if (this.Mode.Equals("PATH_FOLLOW"))
                    {
                        if (this.objectsSelected.Count > 0)
                        {
                            CoronaObject obj = objectsSelected[0];
                            if (obj != null)
                            {
                                if(obj.isEnabled == true)
                                 obj.PathFollow.Path.Add(pTouched);


                            }

                        }
                    }

                }
            }

            if (this.mainForm.isFormLocked == false)
                GorgonLibrary.Gorgon.Go();


        }


        //Mouse MOve--------------------------------------------------------------------------------------------------
        private void surfacePictBx_MouseMove(object sender, MouseEventArgs e)
        {
            this.surfacePictBx.Focus();

            Point offSetPoint = this.getOffsetPoint();
            Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.CurrentScale)),
                     Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.CurrentScale)));


            //Afficher en permanence la position de la souris
            this.mainForm.mouseXPos.Text = "X: " + pTouched.X.ToString();
            this.mainForm.mouseYPos.Text = "Y: " + pTouched.Y.ToString();
            this.mainForm.graduationBarX.reportMouseLocation(pTouched.X);
            this.mainForm.graduationBarY.reportMouseLocation(pTouched.Y);

            currentMousePos = pTouched;

            if (this.movingMode.Equals("FOCUS") && !this.Mode.Equals("CAMERA_RECTANGLE") && this.CurentCalque.Equals("STAGE"))
            {
                Scene sceneSelected = this.mainForm.getElementTreeView().SceneSelected;
                if (sceneSelected != null)
                {
                    if (sceneSelected.Camera.SurfaceFocus.Contains(pTouched))
                    {
                        this.Mode = "MOVE";
                        Cursor = Cursors.SizeAll;
                    }
                    else
                    {
                        this.Mode = "NONE";
                        Cursor = this.openHandCursor;
                    }
                }
                else
                {
                    this.Mode = "NONE";
                    Cursor = this.openHandCursor;
                }

            }

            else if (isMousePressed == false && this.Mode.Equals("CAMERA_RECTANGLE"))
            {
                Cursor = this.createShapeCursor;
            }
            /*else if (isMousePressed == false && this.mainForm.getElementTreeView().JointureSelected != null)
            {
                if (this.indexSceneSelected >= 0)
                {
                    CoronaJointure joint = this.mainForm.getElementTreeView().JointureSelected;
                    if (joint != null)
                    {
                        Point anchorTouched = joint.getAnchorTouched(pTouched);
                        if (anchorTouched != Point.Empty)
                        {
                            Cursor = this.createShapeCursor;
                            this.Mode = "MOVE_JOINT";
                            this.jointAnchorSelected = anchorTouched;
                        }
                        else
                        {
                            this.Mode = "NONE";
                            this.jointAnchorSelected = Point.Empty;
                        }
                    }

                }
            }*/
            else if (isMousePressed == false && !this.Mode.Equals("CREATE_SHAPE") && !this.Mode.Equals("PATH_FOLLOW") && !this.Mode.Equals("JOINT")
                && !this.Mode.Equals("GENERATOR_ATTACH") && !this.movingMode.Equals("AD"))
            {
                string modeTargeted = "NONE";
                if (this.movingMode.Equals("OBJECT"))
                {

                    for (int i = 0; i < this.objectsSelected.Count; i++)
                    {
                        if (this.objectsSelected[i].isEnabled == true)
                        {
                            TransformBox box = this.objectsSelected[i].TransformBox;
                            if (box != null)
                            {
                                modeTargeted = box.getModeFromPointTouched(pTouched);
                                if (!modeTargeted.Equals("NONE"))
                                {
                                    if (modeTargeted.Equals("MOVE_LINE_POINT"))
                                    {
                                        transformBoxSelected = box;
                                    }
                                    else
                                        transformBoxSelected = null;

                                    break;
                                }

                            }
                        }


                    }
                }
                else if (this.movingMode.Equals("ENTITY"))
                {

                    for (int i = 0; i < this.objectsSelected.Count; i++)
                    {
                        if (this.objectsSelected[i].isEnabled == true)
                        {
                            TransformBox box = this.objectsSelected[i].TransformBox;
                            if (box != null)
                            {
                                modeTargeted = box.getModeFromPointTouched(pTouched);
                                if (!modeTargeted.Equals("NONE"))
                                {
                                    transformBoxSelected = box;
                                    break;
                                }

                            }
                        }

                    }
                }
                else if (this.movingMode.Equals("CONTROL"))
                {
                    for (int i = 0; i < this.joysticksSelected.Count; i++)
                    {
                        JoystickControl joy = joysticksSelected[i];
                        if (joy.isEnabled == true)
                        {
                            Rectangle rect = new Rectangle(joy.joystickLocation, new Size(joy.outerRadius * 2, joy.outerRadius * 2));
                            if (rect.Contains(pTouched))
                            {
                                modeTargeted = "MOVE";
                                break;
                            }
                        }

                    }
                }



                if (modeTargeted.Equals("NONE"))
                {
                    if (this.isMousePressed == false)
                        Cursor = this.openHandCursor;

                }
                else if (modeTargeted.Equals("RESIZE_BOTH"))
                {
                    Cursor = Cursors.SizeNWSE;
                }
                else if (modeTargeted.Equals("RESIZE_WIDTH"))
                {
                    Cursor = Cursors.SizeWE;
                }
                else if (modeTargeted.Equals("RESIZE_HEIGHT"))
                {
                    Cursor = Cursors.SizeNS;
                }
                else if (modeTargeted.Equals("MOVE"))
                {
                    Cursor = Cursors.SizeAll;
                }
                else if (modeTargeted.Equals("MOVE_LINE_POINT"))
                {
                    Cursor = this.createShapeCursor;
                }
                else if (modeTargeted.Equals("ROTATION"))
                {
                    Cursor = this.rotationCursor;
                }


                this.Mode = modeTargeted;
            }
            else if ((this.Mode.Equals("CREATE_SHAPE") || this.Mode.Equals("PATH_FOLLOW")) && isMousePressed == false)
            {
                Cursor = this.createShapeCursor;

            }
            else if (this.Mode.Equals("JOINT") && isMousePressed == false)
            {
                CoronaLayer layerSelected = this.mainForm.getElementTreeView().LayerSelected;
                if (layerSelected != null)
                {
                    if (layerSelected.JointureSelected != null && this.mainForm.CurrentJointPanel == null)
                    {
                        if (layerSelected.JointureSelected.isEnabled == true)
                        {
                            Point pTouchedJointure = layerSelected.JointureSelected.getAnchorTouched(pTouched, false);
                            if (pTouchedJointure != Point.Empty)
                            {

                                Cursor = this.createShapeCursor;
                            }
                            else
                                Cursor = this.openHandCursor;
                        }
                    }

                }

            }
            
            else if (this.movingMode.Equals("AD"))
            {
                if (this.moveSurfaceAdActive == true)
                    Cursor = Cursors.SizeAll;
            }




            if (isMousePressed == true)
            {

                // -----MODE LAYER --------------
                if (CurentCalque.Equals("LAYER"))
                {
                    if (this.Mode.Contains("MOVE") || this.Mode.Contains("RESIZE") || this.Mode.Equals("ROTATION"))
                    {
                        if (!this.movingMode.Equals("CONTROL"))
                        {
                            for (int i = 0; i < this.objectsSelected.Count; i++)
                            {
                                CoronaObject obj = this.objectsSelected[i];
                                DisplayObject dispObject = obj.DisplayObject;
                                if (obj.isEnabled == true)
                                {
                                    if (this.Mode.Equals("MOVE"))
                                    {
                                        if (obj.isEntity == false)
                                            dispObject.move(pTouched);
                                        else
                                        {
                                            for (int j = 0; j < obj.Entity.CoronaObjects.Count; j++)
                                            {
                                                CoronaObject objChild = obj.Entity.CoronaObjects[j];
                                                if (objChild.isEnabled == true)
                                                {
                                                    objChild.DisplayObject.move(pTouched);
                                                }
                                              
                                            }
                                        }



                                    }
                                    else if (this.Mode.Equals("MOVE_LINE_POINT"))
                                    {
                                        if (this.transformBoxSelected != null)
                                        {
                                            this.transformBoxSelected.moveLinePoint(pTouched);
                                        }
                                    }
                                    else if (this.Mode.Contains("RESIZE"))
                                    {
                                        if (this.Mode.Equals("RESIZE_WIDTH"))
                                        {
                                            Point finalPoint = new Point(pTouched.X, dispObject.LastPos.Y);
                                            dispObject.setSizeFromPoint(finalPoint);
                                            obj.PhysicsBody.updateBody();
                                        }
                                        else if (this.Mode.Equals("RESIZE_HEIGHT"))
                                        {
                                            Point finalPoint = new Point(dispObject.LastPos.X, pTouched.Y);
                                            dispObject.setSizeFromPoint(finalPoint);
                                            obj.PhysicsBody.updateBody();
                                        }
                                        else if (this.Mode.Equals("RESIZE_BOTH"))
                                        {
                                            dispObject.setSizeFromPoint(pTouched);
                                            obj.PhysicsBody.updateBody();
                                        }

                                        if (dispObject.Type.Equals("IMAGE"))
                                        {
                                            if (dispObject.GorgonSprite != null)
                                            {
                                                float imgScaleX = (float)dispObject.SurfaceRect.Width / (float)dispObject.GorgonSprite.Image.Width;
                                                float imgScaleY = (float)dispObject.SurfaceRect.Height / (float)dispObject.GorgonSprite.Image.Height;

                                                float finalXScale = this.CurrentScale * imgScaleX;
                                                float finalYScale = this.CurrentScale * imgScaleY;
                                                dispObject.GorgonSprite.SetScale(finalXScale, finalYScale);
                                            }
                                           
                                        }
                                        else if (dispObject.Type.Equals("FIGURE"))
                                        {
                                            if (dispObject.Figure.ShapeType.Equals("RECTANGLE") || dispObject.Figure.ShapeType.Equals("TEXT"))
                                            {
                                                if (dispObject.GorgonSprite != null)
                                                {
                                                    float imgScaleX = (float)dispObject.SurfaceRect.Width / (float)dispObject.GorgonSprite.Image.Width;
                                                    float imgScaleY = (float)dispObject.SurfaceRect.Height / (float)dispObject.GorgonSprite.Image.Height;

                                                    float finalXScale = this.CurrentScale * imgScaleX;
                                                    float finalYScale = this.CurrentScale * imgScaleY;
                                                    dispObject.GorgonSprite.SetScale(finalXScale, finalYScale);
                                                }
                                            }
                                        }
                                       
                                    }
                                    else if (this.Mode.Equals("ROTATION"))
                                    {

                                        int angle = pTouched.X - dispObject.SurfaceRect.X - dispObject.SurfaceRect.Width / 2;
                                        if (angle < -360) angle = -360;
                                        if (angle > 360) angle = 360;
                                        dispObject.Rotation = angle;

                                        if (dispObject.Type.Equals("IMAGE")|| dispObject.Type.Equals("SPRITE"))
                                        {
                                            if (dispObject.GorgonSprite != null)
                                            {
                                                dispObject.GorgonSprite.Rotation = angle;
                                            }

                                        }
                                        else if (dispObject.Type.Equals("FIGURE"))
                                        {
                                            if (dispObject.Figure.ShapeType.Equals("RECTANGLE") || dispObject.Figure.ShapeType.Equals("TEXT"))
                                            {
                                                if (dispObject.GorgonSprite != null)
                                                {
                                                    dispObject.GorgonSprite.Rotation = angle;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < this.joysticksSelected.Count; i++)
                            {
                                JoystickControl joy = this.joysticksSelected[i];
                                if (joy.isEnabled == true)
                                {
                                    int xMove = joy.lastPos.X - pTouched.X;
                                    int yMove = joy.lastPos.Y - pTouched.Y;

                                    joy.joystickLocation = new Point(joy.joystickLocation.X - xMove, joy.joystickLocation.Y - yMove);

                                    joy.lastPos.X = pTouched.X;
                                    joy.lastPos.Y = pTouched.Y;
                                }

                            }
                        }


                        if (this.mainForm.isFormLocked == false)
                            GorgonLibrary.Gorgon.Go();

                       
                        return;
                    }

                    else if (this.Mode.Equals("CREATE_SHAPE"))
                    {
                        if (this.FigActive != null)
                        {
                            //Si un rectangle
                            if (this.mainForm.ShapeType == 1 || this.mainForm.ShapeType == 6)
                            {
                                //Recuperer le carre de la figure active
                                Rect c = this.FigActive as Rect;

                                //Definition de la geometrie du carre 
                                if (pTouched.X < pDepart.X && pTouched.Y < pDepart.Y)
                                {
                                    c.Position = pTouched;

                                    c.Width = pDepart.X - pTouched.X;
                                    c.Height = pDepart.Y - pTouched.Y;
                                }


                                else if (pTouched.X > pDepart.X && pTouched.Y > pDepart.Y)
                                {
                                    c.Width = (int)Math.Sqrt(Math.Pow((pTouched.X - pDepart.X), 2));
                                    c.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }
                                else if (e.X < pDepart.X && e.Y > pDepart.Y)
                                {

                                    c.Width = pDepart.X - pTouched.X;
                                    c.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                                return;
                            }

                            //Si un Cercle
                            else if (this.mainForm.ShapeType == 3)
                            {
                                //Recuperer le cercle de la figure active
                                Cercle c = this.FigActive as Cercle;

                                if (c != null)
                                {
                                    //Redefinir la taille du rayon en fonction de la position courante de la souris
                                    c.Rayon = (int)(Math.Sqrt(Math.Pow((pDepart.X - pTouched.X), 2) + Math.Pow((pDepart.Y - pTouched.Y), 2))) / 2;

                                    if (this.mainForm.isFormLocked == false)
                                        GorgonLibrary.Gorgon.Go();

                                }
                                return;
                            }
                            //si un texte
                            else if (this.mainForm.ShapeType == 4 && this.surfaceTextTemp != null)
                            {
                                //Definition de la geometrie du carre 
                                if (pTouched.X < pDepart.X && pTouched.Y < pDepart.Y)
                                {
                                    this.surfaceTextTemp.Location = pTouched;

                                    this.surfaceTextTemp.Width = pDepart.X - pTouched.X;
                                    this.surfaceTextTemp.Height = pDepart.Y - pTouched.Y;
                                }


                                else if (pTouched.X > pDepart.X && pTouched.Y > pDepart.Y)
                                {
                                    this.surfaceTextTemp.Width = (int)Math.Sqrt(Math.Pow((pTouched.X - pDepart.X), 2));
                                    this.surfaceTextTemp.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }
                                else if (e.X < pDepart.X && e.Y > pDepart.Y)
                                {

                                    this.surfaceTextTemp.Width = pDepart.X - pTouched.X;
                                    this.surfaceTextTemp.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                                return;
                            }
                        }
                    }
                    else if (this.Mode.Equals("JOINT"))
                    {
                        CoronaLayer layerSelected = this.mainForm.getElementTreeView().LayerSelected;
                        if (layerSelected != null)
                        {
                            if (layerSelected.JointureSelected != null)
                            {
                                if (layerSelected.JointureSelected.isEnabled == true)
                                {
                                    Point pTouchedJointure = layerSelected.JointureSelected.getAnchorTouched(pTouched, true);
                                    if (pTouchedJointure != Point.Empty)
                                    {
                                        if (this.mainForm.isFormLocked == false)
                                            GorgonLibrary.Gorgon.Go();
                                    }
                                    else
                                    {
                                        //Move the camera lcoation
                                        int offSetX = pDepart.X - pTouched.X;
                                        int offSetY = pDepart.Y - pTouched.Y;

                                        Point currentOffset = this.getOffsetPoint();
                                        int Yvalue = -currentOffset.Y + offSetY;
                                        int Xvalue = -currentOffset.X + offSetX;

                                        this.scrollView(Xvalue, Yvalue);

                                       
                                        return;
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        //Move the camera lcoation
                       
                        int offSetX = pDepart.X - pTouched.X;
                        int offSetY = pDepart.Y - pTouched.Y;

                        Point currentOffset = this.getOffsetPoint();
                        int Yvalue = -currentOffset.Y + offSetY;
                        int Xvalue = -currentOffset.X + offSetX;

                        this.scrollView(Xvalue, Yvalue);
                        return;
                    }

                }
                else if (CurentCalque.Equals("STAGE"))
                {
                    GameElementTreeView treeView = this.mainForm.getElementTreeView();

                    if (this.Mode.Equals("CAMERA_RECTANGLE"))
                    {
                        Scene sceneSelected = treeView.SceneSelected;
                        if (sceneSelected != null)
                        {
                            int width = pTouched.X - pDepart.X;
                            if (width < 10) width = 10;
                            int height = pTouched.Y - pDepart.Y;
                            if (height < 10) height = 10;
                            sceneSelected.Camera.CameraFollowLimitRectangle = new Rectangle(pDepart, new Size(width, height));

                            if (this.mainForm.isFormLocked == false)
                                GorgonLibrary.Gorgon.Go();
                        }
                        return;
                    }
                    else if (this.Mode.Equals("MOVE"))
                    {
                        if (treeView.SceneSelected != null)
                        {
                            if (moveFocusSceneActive == true)
                            {
                                treeView.SceneSelected.Camera.moveFocusScene(pTouched);

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                            }
                            else if (moveObjectActive == true)
                            {
                                for (int i = 0; i < this.objectsSelected.Count; i++)
                                {
                                    if (this.objectsSelected[i].isEnabled == true)
                                    {
                                        if (this.objectsSelected[i].isEntity == false)
                                        {
                                            this.objectsSelected[i].DisplayObject.move(pTouched);
                                        }
                                        else
                                        {
                                            for (int j = 0; j < this.objectsSelected[i].Entity.CoronaObjects.Count; j++)
                                            {
                                                CoronaObject child = this.objectsSelected[i].Entity.CoronaObjects[j];
                                                if (child.isEnabled == true)
                                                {
                                                    child.DisplayObject.move(pTouched);
                                                }
                                            }
                                        }
                                    }


                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                            }
                            else if (moveSurfaceAdActive == true)
                            {
                                treeView.SceneSelected.moveAdSurface(pTouched);

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                            }
                            else if (moveSurfaceControlActive == true)
                            {
                                for (int i = 0; i < this.joysticksSelected.Count; i++)
                                {
                                    JoystickControl joy = this.joysticksSelected[i];
                                    if (joy.isEnabled == true)
                                    {
                                        int xMove = joy.lastPos.X - pTouched.X;
                                        int yMove = joy.lastPos.Y - pTouched.Y;

                                        joy.joystickLocation = new Point(joy.joystickLocation.X - xMove, joy.joystickLocation.Y - yMove);

                                        joy.lastPos.X = pTouched.X;
                                        joy.lastPos.Y = pTouched.Y;
                                    }
                                }

                                GorgonLibrary.Gorgon.Go();
                            }
                        }
                    }
                    else if (this.Mode.Equals("MOVE_LINE_POINT"))
                    {
                        if (this.transformBoxSelected != null)
                        {
                            this.transformBoxSelected.moveLinePoint(pTouched);

                            if (this.mainForm.isFormLocked == false)
                                GorgonLibrary.Gorgon.Go();
                        }
                    }
                    else if (this.Mode.Contains("RESIZE"))
                    {
                        if (moveObjectActive == true)
                        {
                            if (this.Mode.Equals("RESIZE_WIDTH"))
                            {
                                for (int i = 0; i < this.objectsSelected.Count; i++)
                                {
                                    CoronaObject obj = this.objectsSelected[i];
                                    if (obj.isEnabled == true)
                                    {
                                        Point finalPoint = new Point(pTouched.X, obj.DisplayObject.LastPos.Y);
                                        obj.DisplayObject.setSizeFromPoint(finalPoint);

                                        obj.PhysicsBody.updateBody();
                                    }
                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();


                                return;

                            }
                            else if (this.Mode.Equals("RESIZE_HEIGHT"))
                            {
                                for (int i = 0; i < this.objectsSelected.Count; i++)
                                {
                                    CoronaObject obj = this.objectsSelected[i];
                                    if (obj.isEnabled == true)
                                    {
                                        Point finalPoint = new Point(obj.DisplayObject.LastPos.X, pTouched.Y);
                                        obj.DisplayObject.setSizeFromPoint(finalPoint);

                                        obj.PhysicsBody.updateBody();
                                    }
                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                                return;



                            }
                            else if (this.Mode.Equals("RESIZE_BOTH"))
                            {
                                for (int i = 0; i < this.objectsSelected.Count; i++)
                                {
                                    CoronaObject obj = this.objectsSelected[i];
                                    if (obj.isEnabled == true)
                                    {
                                        obj.DisplayObject.setSizeFromPoint(pTouched);

                                        obj.PhysicsBody.updateBody();
                                    }
                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                                return;
                            }

                        }
                    }
                    else if (this.Mode.Equals("ROTATION"))
                    {
                        if (moveObjectActive == true)
                        {
                            for (int i = 0; i < this.objectsSelected.Count; i++)
                            {
                                CoronaObject obj = this.objectsSelected[i];
                                if (obj.isEnabled == true)
                                {
                                    int angle = pTouched.X - obj.DisplayObject.SurfaceRect.X - obj.DisplayObject.SurfaceRect.Width / 2;
                                    if (angle < -360) angle = -360;
                                    if (angle > 360) angle = 360;
                                    obj.DisplayObject.Rotation = angle;
                                }
                            }

                            if (this.mainForm.isFormLocked == false)
                                GorgonLibrary.Gorgon.Go();
                        }

                    }
                    else if (this.Mode.Equals("CREATE_SHAPE"))
                    {
                        if (this.FigActive != null)
                        {
                            //Si un rectangle
                            if (this.mainForm.ShapeType == 1 || this.mainForm.ShapeType == 6)
                            {
                                //Recuperer le carre de la figure active
                                Rect c = this.FigActive as Rect;

                                //Definition de la geometrie du carre 
                                if (pTouched.X < pDepart.X && pTouched.Y < pDepart.Y)
                                {
                                    c.Position = pTouched;

                                    c.Width = pDepart.X - pTouched.X;
                                    c.Height = pDepart.Y - pTouched.Y;
                                }


                                else if (pTouched.X > pDepart.X && pTouched.Y > pDepart.Y)
                                {
                                    c.Width = (int)Math.Sqrt(Math.Pow((pTouched.X - pDepart.X), 2));
                                    c.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }
                                else if (e.X < pDepart.X && e.Y > pDepart.Y)
                                {

                                    c.Width = pDepart.X - pTouched.X;
                                    c.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                                return;
                            }

                            //Si un Cercle
                            else if (this.mainForm.ShapeType == 3)
                            {
                                //Recuperer le cercle de la figure active
                                Cercle c = this.FigActive as Cercle;

                                if (c != null)
                                {
                                    //Redefinir la taille du rayon en fonction de la position courante de la souris
                                    c.Rayon = (int)(Math.Sqrt(Math.Pow((pDepart.X - pTouched.X), 2) + Math.Pow((pDepart.Y - pTouched.Y), 2))) / 2;

                                    if (this.mainForm.isFormLocked == false)
                                        GorgonLibrary.Gorgon.Go();

                                }
                                return;
                            }
                            //si un texte
                            else if (this.mainForm.ShapeType == 4 && this.surfaceTextTemp != null)
                            {
                                //Definition de la geometrie du carre 
                                if (pTouched.X < pDepart.X && pTouched.Y < pDepart.Y)
                                {
                                    this.surfaceTextTemp.Location = pTouched;

                                    this.surfaceTextTemp.Width = pDepart.X - pTouched.X;
                                    this.surfaceTextTemp.Height = pDepart.Y - pTouched.Y;
                                }


                                else if (pTouched.X > pDepart.X && pTouched.Y > pDepart.Y)
                                {
                                    this.surfaceTextTemp.Width = (int)Math.Sqrt(Math.Pow((pTouched.X - pDepart.X), 2));
                                    this.surfaceTextTemp.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }
                                else if (e.X < pDepart.X && e.Y > pDepart.Y)
                                {

                                    this.surfaceTextTemp.Width = pDepart.X - pTouched.X;
                                    this.surfaceTextTemp.Height = (int)Math.Sqrt(Math.Pow((pTouched.Y - pDepart.Y), 2));
                                }

                                if (this.mainForm.isFormLocked == false)
                                    GorgonLibrary.Gorgon.Go();
                                return;
                            }
                        }
                    }
                    else if (this.Mode.Equals("MOVE_JOINT"))
                    {
                        if (this.jointAnchorSelected != Point.Empty)
                        {
                            this.jointAnchorSelected.X = pTouched.X;
                            this.jointAnchorSelected.Y = pTouched.Y;
                        }
                    }

                    else
                    {
                        
                        //Move the camera lcoation
                        int offSetX = pDepart.X - pTouched.X;
                        int offSetY = pDepart.Y - pTouched.Y;

                        Point currentOffset = this.getOffsetPoint();
                        int Yvalue = -currentOffset.Y + offSetY;
                        int Xvalue = -currentOffset.X + offSetX;

                        this.scrollView(Xvalue, Yvalue);
                        return;
                    }

                }
            }





        }

        public void scrollView(int offSetX, int offSetY)
        {
            //Move the camera lcoation
            Point maxOffset = this.GetScrollMaxNormalValues();

            if (offSetY > maxOffset.Y)
                offSetY = maxOffset.Y;
            else if (offSetY < 0)
                offSetY = 0;
            
            this.vScrollBar.Value = -offSetY;

            if (offSetX > maxOffset.X)
                offSetX = maxOffset.X;
            else if (offSetX < 0)
                offSetX = 0;
            
            this.hScrollBar.Value = offSetX;

            RefreshScrollView();
        }

        //Mouse released ---------------------------------------------------------------------------------------------------
        private void surfacePictBx_MouseUp(object sender, MouseEventArgs e)
        {
            
           
            isMousePressed = false;
            moveFocusSceneActive = false;
            moveObjectActive = false;
            moveSurfaceAdActive = false;
            moveSurfaceControlActive = false;


            Point offSetPoint = this.getOffsetPoint();

            Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.CurrentScale)),
                    Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.CurrentScale)));

            if (this.Mode.Equals("CREATE_SHAPE"))
            {
                if (this.mainForm.ShapeType != 5 && this.mainForm.ShapeType != 2)
                {
                    if (this.mainForm.ShapeType == 4 && this.surfaceTextTemp != null)
                    {
                        Font2 font = new Font2("ARIAL", 16, FontStyle.Regular);
                        this.FigActive = new Texte(this.surfaceTextTemp.Location, "New Text", font, Color.Blue, 1, false, null);
                    }
                    this.mainForm.addFigureBt_Click(null, null);
                }

                if (this.mainForm.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();
            }
            //------------------------------------------------------------------------------
            //--------------------------CREATION DES JOINTURES--------------------------
            //------------------------------------------------------------------------------
            else if (this.Mode.Equals("JOINT"))
            {
                UserControl jointPanel = this.mainForm.CurrentJointPanel;

                //Verifier que le panel n'est pas null
                if (jointPanel != null)
                {

                    //Recuperer le panel correspondant au joint
                    if (jointPanel.Name.Equals("PIVOT"))
                    {
                        PivotPropertiesPanel pivotPanel = (PivotPropertiesPanel)jointPanel;
                        pivotPanel.nextCreationStep(pTouched);

                    }
                    else if (jointPanel.Name.Equals("DISTANCE"))
                    {
                        DistancePropertiesPanel panel = (DistancePropertiesPanel)jointPanel;
                        panel.nextCreationStep(pTouched);

                    }
                    else if (jointPanel.Name.Equals("FRICTION"))
                    {
                        FrictionPropertiesPanel panel = (FrictionPropertiesPanel)jointPanel;
                        panel.nextCreationStep(pTouched);

                    }
                    else if (jointPanel.Name.Equals("PISTON"))
                    {
                        PistonPropertiesPanel panel = (PistonPropertiesPanel)jointPanel;
                        panel.nextCreationStep(pTouched);

                    }
                    else if (jointPanel.Name.Equals("PULLEY"))
                    {
                        PulleyPropertiesPanel panel = (PulleyPropertiesPanel)jointPanel;
                        panel.nextCreationStep(pTouched);

                    }
                    else if (jointPanel.Name.Equals("WHEEL"))
                    {
                        WheelPropertiesPanel panel = (WheelPropertiesPanel)jointPanel;
                        panel.nextCreationStep(pTouched);

                    }
                    else if (jointPanel.Name.Equals("WELD"))
                    {
                        WeldPropertiesPanel panel = (WeldPropertiesPanel)jointPanel;
                        panel.nextCreationStep(pTouched);

                    }
                    else if (jointPanel.Name.Equals("TOUCH"))
                    {
                        TouchPropertiesPanel panel = (TouchPropertiesPanel)jointPanel;
                        panel.nextCreationStep(pTouched);

                    }


                }

                if (this.mainForm.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();

            }


        }


        public void selectObject(CoronaObject obj, bool isControlKeyPressed)
        {
            GameElementTreeView treeView = this.mainForm.getElementTreeView();
            if (obj != null)
            {
                if (obj.isEnabled == true)
                {
                    if (isControlKeyPressed == true)
                    {
                        if (!this.objectsSelected.Contains(obj))
                        {
                            this.objectsSelected.Add(obj);
                            obj.setSelected(true);
                        }
                        else
                        {
                            this.objectsSelected.Remove(obj);
                            obj.setSelected(false);
                        }

                    }

                    else
                    {
                        for (int i = 0; i < this.objectsSelected.Count; i++)
                        {
                            this.objectsSelected[i].setSelected(false);
                        }
                        this.objectsSelected.Clear();
                        this.objectsSelected.Add(obj);
                        obj.setSelected(true);
                    }

                    treeView.CoronaObjectSelected = obj;
                }
            }
            else
            {
                if (!isControlKeyPressed)
                {
                    for (int i = 0; i < this.objectsSelected.Count; i++)
                    {
                        this.objectsSelected[i].setSelected(false);
                    }
                    this.objectsSelected.Clear();
                }
            }

            if (this.objectsSelected.Count > 0)
            {
                object[] tabPropConv = new object[this.objectsSelected.Count];
                for (int i = 0; i < this.objectsSelected.Count; i++)
                {
                    CoronaObject objSelected = this.objectsSelected[i];
                    if (objSelected.isEnabled == true)
                    {
                        if (objSelected.isEntity == false)
                        {
                            if (objSelected.DisplayObject.Figure != null)
                            {


                                if (objSelected.DisplayObject.Figure.ShapeType.Equals("TEXT"))
                                {
                                    //Afficher les proprietes de l'objet dans le property grid
                                    TextPropertyConverter objectConverter = new TextPropertyConverter(objSelected, this.mainForm);
                                    tabPropConv[i] = objectConverter;
                                }
                                else if (objSelected.DisplayObject.Figure.ShapeType.Equals("RECTANGLE"))
                                {
                                    //Afficher les proprietes de l'objet dans le property grid
                                    RectPropertyConverter objectConverter = new RectPropertyConverter(objSelected, this.mainForm);
                                    tabPropConv[i] = objectConverter;
                                }
                                else if (objSelected.DisplayObject.Figure.ShapeType.Equals("CURVE"))
                                {
                                    //Afficher les proprietes de l'objet dans le property grid
                                    CurvePropertyConverter objectConverter = new CurvePropertyConverter(objSelected, this.mainForm);
                                    tabPropConv[i] = objectConverter;
                                }
                                else
                                {
                                    //Afficher les proprietes de l'objet dans le property grid
                                    FigurePropertyConverter objectConverter = new FigurePropertyConverter(objSelected, this.mainForm);
                                    tabPropConv[i] = objectConverter;
                                }
                            }
                            else if (objSelected.DisplayObject.Type.Equals("IMAGE"))
                            {
                                //Afficher les proprietes de l'objet dans le property grid
                                ImagePropertyConverter objectConverter = new ImagePropertyConverter(objSelected, this.mainForm);
                                tabPropConv[i] = objectConverter;
                            }
                            else if (objSelected.DisplayObject.Type.Equals("SPRITE"))
                            {
                                //Afficher les proprietes de l'objet dans le property grid
                                SpritePropertyConverter objectConverter = new SpritePropertyConverter(objSelected, this.mainForm);
                                tabPropConv[i] = objectConverter;
                            }
                            else
                            {
                                //Afficher les proprietes de l'objet dans le property grid
                                ObjectPropertyConverter objectConverter = new ObjectPropertyConverter(objSelected, this.mainForm);
                                tabPropConv[i] = objectConverter;
                            }
                        }
                        else
                        {
                            //Afficher les proprietes de l'objet dans le property grid
                            EntityPropertyConverter objectConverter = new EntityPropertyConverter(objSelected, this.mainForm);
                            tabPropConv[i] = objectConverter;
                        }
                    }

                }

                this.mainForm.propertyGrid1.SelectedObjects = tabPropConv;


            }
            else
            {
                this.mainForm.propertyGrid1.SelectedObjects = null;
            }
        }

        private void surfacePictBx_MouseClick(object sender, MouseEventArgs e)
        {
            this.Focus();
            this.Select();

            Point offSetPoint = this.getOffsetPoint();

            for (int i = 0; i < this.objectsSelected.Count; i++)
            {
                CoronaObject obj = this.objectsSelected[i];
                if(obj.DisplayObject != null)
                if (obj.DisplayObject.Type.Equals("FIGURE"))
                    this.GraphicsContentManager.UpdateSpriteStates(obj, this.CurrentScale, offSetPoint);

                //if (obj.DisplayObject.Type.Equals("SPRITE"))
                //{
                //    this.GraphicsContentManager.UpdateSpriteContent(obj, this.CurrentScale, offSetPoint);
                //}
                //else
                //    this.GraphicsContentManager.UpdateSpriteStates(obj, this.CurrentScale, offSetPoint);
            }

            Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.CurrentScale)),
                     Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.CurrentScale)));

            bool isControlKeyPressed = false;

            //Verifier si le control est actif : si oui ajouter a la liste 
            if (Control.ModifierKeys == Keys.Control)
                isControlKeyPressed = true;

            GameElementTreeView treeView = this.mainForm.getElementTreeView();
            Scene sceneSelected = treeView.SceneSelected;

            if (sceneSelected != null)
            {
                if (this.movingMode == "OBJECT")
                {
                    CoronaObject obj = null;
                    if (CurentCalque.Equals("LAYER"))
                        obj = treeView.LayerSelected.getObjTouched(pTouched);
                    else if (CurentCalque.Equals("STAGE"))
                        obj = sceneSelected.getObjectTouched(pTouched);

                    if (this.objectsSelected.Contains(obj) && isControlKeyPressed == false)
                    {
                        if (this.objectsSelected.Count > 0)
                        {

                            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                            {
                                treeView.activerBoutonsNecessairesMenuObject(this.objectsSelected[0]);
                                treeView.menuObject.Show(Cursor.Position);
                            }
                        }
                        else
                        {
                            this.mainForm.propertyGrid1.SelectedObjects = null;
                        }
                        GorgonLibrary.Gorgon.Go();
                        return;
                    }

                    if (!this.Mode.Equals("PATH_FOLLOW") && !this.movingMode.Equals("AD") && !this.Mode.Equals("GENERATOR_ATTACH"))
                    {
                        this.selectObject(obj, isControlKeyPressed);

                    }
                    else if (this.Mode.Equals("GENERATOR_ATTACH") && CurentCalque.Equals("LAYER"))
                    {
                        if (treeView.CoronaObjectSelected != null && obj != null)
                        {
                            if (treeView.CoronaObjectSelected != obj)
                            {
                                treeView.CoronaObjectSelected.objectAttachedToGenerator = obj;

                                MessageBox.Show("The generator of \"" + treeView.CoronaObjectSelected.DisplayObject.Name + "\" has been correctly fastened on the object \"" + obj.DisplayObject.Name + "\"!",
                                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Mode = "NONE";
                            }
                            else
                            {
                                this.mainForm.propertyGrid1.SelectedObjects = null;
                            }

                        }
                    }
                    else if (this.Mode.Equals("GENERATOR_ATTACH") && CurentCalque.Equals("STAGE"))
                    {
                        MessageBox.Show("Please select the layer where the generator is located to define its fastener!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (this.objectsSelected.Count > 0)
                    {

                        if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        {
                            treeView.activerBoutonsNecessairesMenuObject(this.objectsSelected[0]);
                            treeView.menuObject.Show(Cursor.Position);
                        }
                    }
                    else
                    {
                        this.mainForm.propertyGrid1.SelectedObjects = null;
                    }

                    treeView.refreshNodesSelectedSceneEditor();
                }
                else if (this.movingMode.Equals("ENTITY"))
                {
                    CoronaObject obj = null;
                    if (CurentCalque.Equals("LAYER"))
                        obj = treeView.LayerSelected.getObjTouched(pTouched);
                    else if (CurentCalque.Equals("STAGE"))
                        obj = sceneSelected.getObjectTouched(pTouched);


                    if (obj != null && obj.EntityParent != null)
                    {
                        if (this.objectsSelected.Contains(obj.EntityParent.objectParent) && isControlKeyPressed == false)
                        {
                            if (this.objectsSelected.Count > 0)
                            {
                                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                                {
                                    treeView.activerBoutonsNecessairesMenuObject(this.objectsSelected[0]);
                                    treeView.menuObject.Show(Cursor.Position);
                                }
                            }
                            GorgonLibrary.Gorgon.Go();
                            return;
                        }


                        this.selectObject(obj.EntityParent.objectParent, isControlKeyPressed);

                        if (this.objectsSelected.Count > 0)
                        {

                            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                            {
                                treeView.activerBoutonsNecessairesMenuObject(this.objectsSelected[0]);
                                treeView.menuObject.Show(Cursor.Position);
                            }
                        }
                        else
                        {
                            this.mainForm.propertyGrid1.SelectedObjects = null;
                        }

                        treeView.refreshNodesSelectedSceneEditor();
                    }
                    else
                    {
                        if(treeView.SceneSelected != null)
                            treeView.SceneSelected.deselectAllObjects();

                        this.objectsSelected.Clear();
                        treeView.refreshNodesSelectedSceneEditor();
                    }


                }

                else if (this.movingMode.Equals("CONTROL"))
                {
                    JoystickControl joy = null;
                    if (CurentCalque.Equals("LAYER"))
                        joy = treeView.LayerSelected.getJoystickTouched(pTouched);
                    else if (CurentCalque.Equals("STAGE"))
                        joy = sceneSelected.getJoystickTouched(pTouched);

                    if (joy != null)
                    {
                        if (isControlKeyPressed)
                        {
                            if (!this.joysticksSelected.Contains(joy))
                            {
                                this.joysticksSelected.Add(joy);
                                joy.setSelected(true);
                            }
                            else
                            {
                                this.joysticksSelected.Remove(joy);
                                joy.setSelected(false);
                            }

                        }

                        else
                        {
                            for (int i = 0; i < this.joysticksSelected.Count; i++)
                            {
                                this.joysticksSelected[i].setSelected(false);
                            }
                            this.joysticksSelected.Clear();
                            this.joysticksSelected.Add(joy);
                            joy.setSelected(true);
                        }
                    }
                    else
                    {
                        if (!isControlKeyPressed)
                        {
                            for (int i = 0; i < this.joysticksSelected.Count; i++)
                            {
                                this.joysticksSelected[i].setSelected(false);
                            }
                            this.joysticksSelected.Clear();
                        }
                    }

                    if (this.joysticksSelected.Count > 0)
                    {
                        object[] tabPropConv = new object[this.joysticksSelected.Count];
                        for (int i = 0; i < this.joysticksSelected.Count; i++)
                        {
                            JoystickControl joySelected = this.joysticksSelected[i];
                            JoystickPropertyConverter joyConverter = new JoystickPropertyConverter(joySelected, mainForm);
                            tabPropConv[i] = joyConverter;

                        }

                        this.mainForm.propertyGrid1.SelectedObjects = tabPropConv;
                    }
                    else
                    {
                        this.mainForm.propertyGrid1.SelectedObjects = null;
                    }
                }
            }


            GorgonLibrary.Gorgon.Go();

        }

        public void RefreshScrollView()
        {
            this.vScrollBar_Scroll(null, null);
        }

      
        private void surfacePictBx_SizeChanged(object sender, EventArgs e)
        {
            if (this.mainForm != null)
            {
                this.SetScrollMax();
                GorgonLibrary.Gorgon.Go();
            }

        }

        private void surfacePictBx_DragEnter(object sender, DragEventArgs e)
        {
            GameElementTreeView treeView = this.mainForm.getElementTreeView();
            if (treeView.LayerSelected != null)
            {
                if (e.Data.GetDataPresent(typeof(ListViewItem)))
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    string filename = String.Empty;
                    bool ret = false;
                    if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                    {
                        Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
                        if (data != null)
                        {
                            if ((data.Length == 1) && (data.GetValue(0) is String))
                            {
                                filename = ((string[])data)[0];
                                string ext = System.IO.Path.GetExtension(filename).ToLower();
                                if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                                {
                                    ret = true;
                                }
                            }
                        }
                    }

                    if (ret == true)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
           

        }

        private void surfacePictBx_DragDrop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;

                if (item.Tag is DisplayObject)
                {
                    DisplayObject obj = (DisplayObject)item.Tag;

                    GameElementTreeView treeView = this.mainForm.getElementTreeView();
                    if (treeView.LayerSelected != null)
                    {
                        DisplayObject dispObjTemp = obj.cloneInstance(true);
                        dispObjTemp.Name = obj.Name.ToLower().Replace(" ", "").Replace("_", "").Replace("-", ""); ;
                        dispObjTemp.OriginalAssetName = obj.Name;

                        //Creer un corona object et l'ajouter 
                        CoronaObject finalObj = new CoronaObject(dispObjTemp);
                        this.GraphicsContentManager.UpdateSpriteContent(finalObj,this.CurrentScale,this.getOffsetPoint());
                        this.GraphicsContentManager.CleanProjectBitmaps();
                        if (this.mainForm.imageObjectsPanel1.ShouldBeRefreshed == true)
                        {
                            this.mainForm.imageObjectsPanel1.RefreshCurrentAssetProject();
                            this.mainForm.imageObjectsPanel1.ShouldBeRefreshed = false;
                        }

                        CoronaObject objectSelectedInTreeView = this.mainForm.getElementTreeView().CoronaObjectSelected;
                        if (objectSelectedInTreeView != null)
                        {
                            if (objectSelectedInTreeView.EntityParent != null)
                            {
                                objectSelectedInTreeView.EntityParent.addObject(finalObj);
                            }
                            else if (objectSelectedInTreeView.isEntity == true)
                            {
                                objectSelectedInTreeView.Entity.addObject(finalObj);
                            }
                            else
                            {
                                treeView.LayerSelected.addCoronaObject(finalObj, true);
                            }

                        }
                        else
                        {

                            treeView.LayerSelected.addCoronaObject(finalObj, true);
                        }
                      
                        treeView.newCoronaObject(finalObj);

                        Point offSet = this.getOffsetPoint();
                        Rectangle surfacePictBx = this.surfacePictBx.RectangleToClient(new Rectangle(new Point(e.X, e.Y), new Size(10, 10)));

                        Point dest = new Point(Convert.ToInt32(surfacePictBx.X * (1 / this.CurrentScale)) - offSet.X - finalObj.DisplayObject.SurfaceRect.Width / 2,
                            Convert.ToInt32(surfacePictBx.Y * (1 / this.CurrentScale)) - offSet.Y - finalObj.DisplayObject.SurfaceRect.Height / 2);
                        finalObj.DisplayObject.SurfaceRect = new Rectangle(dest, finalObj.DisplayObject.SurfaceRect.Size);

                        if (this.mainForm.isFormLocked == false)
                            GorgonLibrary.Gorgon.Go();

                    }
                    else
                        MessageBox.Show("Please select a layer before adding a new display object!", "Select a layer", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
            }
            else
            {
                string filename = String.Empty;
                bool ret = false;
                if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                {
                    Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
                    if (data != null)
                    {
                        if ((data.Length == 1) && (data.GetValue(0) is String))
                        {
                            filename = ((string[])data)[0];
                            string ext = System.IO.Path.GetExtension(filename).ToLower();
                            if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                            {
                                ret = true;
                            }
                        }
                    }
                }

                if (ret == true)
                {

                    if (this.mainForm.CurrentProject != null)
                    {
                        GameElementTreeView treeView = this.mainForm.getElementTreeView();

                        if (treeView.LayerSelected != null)
                        {
                            System.Drawing.Image image = new Bitmap(System.Drawing.Image.FromFile(filename));
                            DisplayObject dispObjTemp = new DisplayObject(image, Point.Empty, null);
                            dispObjTemp.Name = System.IO.Path.GetFileNameWithoutExtension(filename).Replace("~","_");
                            dispObjTemp.OriginalAssetName = dispObjTemp.Name;
                          

                            //Creer un corona object et l'ajouter 
                            CoronaObject obj = new CoronaObject(dispObjTemp);
                            if (obj.DisplayObject != null)
                            {
                                obj.DisplayObject.SurfaceRect = new Rectangle(treeView.LayerSelected.SceneParent.SurfaceFocus.Location, obj.DisplayObject.SurfaceRect.Size);
                            }
                            this.GraphicsContentManager.UpdateSpriteContent(obj, this.CurrentScale, this.getOffsetPoint());
                            this.GraphicsContentManager.CleanProjectBitmaps();
                            if (this.mainForm.imageObjectsPanel1.ShouldBeRefreshed == true)
                            {
                                this.mainForm.imageObjectsPanel1.RefreshCurrentAssetProject();
                                this.mainForm.imageObjectsPanel1.ShouldBeRefreshed = false;
                            }

                            CoronaObject objectSelectedInTreeView = treeView.CoronaObjectSelected;
                            if (objectSelectedInTreeView != null)
                            {
                                if (objectSelectedInTreeView.EntityParent != null)
                                {
                                    objectSelectedInTreeView.EntityParent.addObject(obj);
                                }
                                else if (objectSelectedInTreeView.isEntity == true)
                                {
                                    objectSelectedInTreeView.Entity.addObject(obj);
                                }
                                else
                                {
                                    treeView.LayerSelected.addCoronaObject(obj, true);
                                }

                            }
                            else
                            {

                                treeView.LayerSelected.addCoronaObject(obj, true);
                            }


                            treeView.newCoronaObject(obj);

                            Point offSet = this.getOffsetPoint();
                            Rectangle surfacePictBx = this.surfacePictBx.RectangleToClient(new Rectangle(new Point(e.X, e.Y), new Size(10, 10)));

                            Point dest = new Point(Convert.ToInt32(surfacePictBx.X * (1 / this.CurrentScale)) - offSet.X - obj.DisplayObject.SurfaceRect.Width / 2,
                                Convert.ToInt32(surfacePictBx.Y * (1 / this.CurrentScale)) - offSet.Y - obj.DisplayObject.SurfaceRect.Height / 2);
                            obj.DisplayObject.SurfaceRect = new Rectangle(dest, obj.DisplayObject.SurfaceRect.Size);


                            if (this.mainForm.isFormLocked == false)
                                GorgonLibrary.Gorgon.Go();
                        }
                    }
                }


            }

        }

        public void dispatchKeyEvent(KeyEventArgs args)
        {
            SceneEditorView_KeyDown(null, args);
        }

        private void surfacePictBx_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.SceneEditorView_KeyDown(null, new KeyEventArgs(e.Modifiers | e.KeyCode));
        }

        private void SceneEditorView_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.IsFocused == true && this.mainForm.CurrentProject != null)
            {
                bool controlPressed = e.Control;
               
                if (e.KeyCode == Keys.Delete)
                    this.mainForm.removeObjectsSelected_Click(null, null);
                else if (controlPressed == true && e.KeyCode == Keys.D)
                    this.mainForm.getElementTreeView().DupplicateSelectedObjects();
                else if (e.KeyCode == Keys.Escape)
                    this.mainForm.SetModeObject();

                else if (controlPressed == true && e.KeyCode == Keys.D1)
                {
                    this.mainForm.SetModeObject();
                }
                else if (controlPressed == true && e.KeyCode == Keys.D2)
                {
                    this.mainForm.SetModeEntity();
                }
                else if (controlPressed == true && e.KeyCode == Keys.D3)
                {
                    this.mainForm.SetModeControl();
                }
                else if (controlPressed == true && e.KeyCode == Keys.D4)
                {
                    this.mainForm.SetModeAds();
                }
                else if (controlPressed == true && e.KeyCode == Keys.D5)
                {
                    this.mainForm.SetModeFocus();
                }
                else if (controlPressed == true && e.KeyCode == Keys.A)
                {

                    CoronaLayer layerSelected = this.mainForm.getElementTreeView().LayerSelected;
                    Scene sceneScelected = this.mainForm.getElementTreeView().SceneSelected;


                    if (this.movingMode.Equals("OBJECT"))
                    {
                        this.objectsSelected.Clear();
                        if (layerSelected != null)
                        {

                            for (int i = 0; i < layerSelected.CoronaObjects.Count; i++)
                            {
                                CoronaObject obj = layerSelected.CoronaObjects[i];
                                if (obj.isEntity == false)
                                {
                                    this.selectObject(obj, true);
                                }
                                else
                                {
                                    for (int j = 0; j < obj.Entity.CoronaObjects.Count; j++)
                                    {
                                        CoronaObject child = obj.Entity.CoronaObjects[j];
                                        this.selectObject(child, true);
                                    }
                                }
                            }
                        }
                        else if (sceneScelected != null)
                        {
                            for (int k = 0; k < sceneScelected.Layers.Count; k++)
                            {
                                CoronaLayer layer = sceneScelected.Layers[k];
                                for (int i = 0; i < layer.CoronaObjects.Count; i++)
                                {
                                    CoronaObject obj = layer.CoronaObjects[i];
                                    if (obj.isEntity == false)
                                    {
                                        this.selectObject(obj, true);
                                    }
                                    else
                                    {
                                        for (int j = 0; j < obj.Entity.CoronaObjects.Count; j++)
                                        {
                                            CoronaObject child = obj.Entity.CoronaObjects[j];
                                            this.selectObject(child, true);
                                        }
                                    }
                                }
                            }
                        }

                        this.mainForm.getElementTreeView().refreshNodesSelectedSceneEditor();
                        GorgonLibrary.Gorgon.Go();
                    }
                }


            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------GORGON --------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void DrawGorgon(float worldScale, Point offsetPoint,bool drawGrid,bool drawFocusShader)
        {
            Gorgon.CurrentRenderTarget.BeginDrawing();

            try
            {

                // Do nothing here.  When we need to update, we will.
                if (this.CurentCalque.Equals("STAGE"))
                {
                    Scene scene = this.mainForm.getElementTreeView().SceneSelected;

                    if (scene != null)
                    {

                        if (scene.DefaultColor == Color.Empty)
                            scene.DefaultColor = Color.Black;

                        Gorgon.CurrentRenderTarget.Clear(scene.DefaultColor);
                    }
                }
                else if (this.CurentCalque.Equals("LAYER"))
                {
                    CoronaLayer layer = this.mainForm.getElementTreeView().LayerSelected;
                    if (layer != null)
                    {
                        Scene scene = layer.SceneParent;
                        if (scene.DefaultColor == Color.Empty)
                            scene.DefaultColor = Color.Black;

                        Gorgon.CurrentRenderTarget.Clear(scene.DefaultColor);
                    }
                }



                if (!Gorgon.CurrentRenderTarget.Name.Equals("ScenePreview"))
                    if (drawGrid == true)
                    {
                        dessineGorgonQuadrillage(offsetPoint, worldScale);
                    }


                if (this.CurentCalque.Equals("STAGE"))
                {
                    Scene scene = this.mainForm.getElementTreeView().SceneSelected;

                    if (scene != null)
                    {
                        if (!Gorgon.CurrentRenderTarget.Name.Equals("ScenePreview"))
                        {
                            for (int i = 0; i < scene.Layers.Count; i++)
                            {
                                CoronaLayer layer = scene.Layers[i];
                                if (layer.isEnabled == true)
                                {
                                    if (layer.TilesMap != null)
                                    {
                                        if (layer.TilesMap.isEnabled == true)
                                        {
                                            Rectangle rect = new Rectangle(new Point(-offsetPoint.X, -offsetPoint.Y), this.surfacePictBx.Size);
                                            layer.TilesMap.setSurfaceVisible(rect, worldScale, worldScale);
                                        }
                                    }
                                }
                            }
                        }

                        scene.dessineGorgonScene(worldScale, worldScale, offsetPoint, drawFocusShader);


                        //Dessiner la shape en construction 
                        if (this.FigActive != null)
                        {
                            this.FigActive.DrawGorgon(offsetPoint, 255,worldScale);
                        }

                        if (this.Mode.Equals("CAMERA_RECTANGLE"))
                        {
                            Rectangle rect = new Rectangle(new Point(scene.Camera.CameraFollowLimitRectangle.X + offsetPoint.X, scene.Camera.CameraFollowLimitRectangle.Y + offsetPoint.Y),
                                                            scene.Camera.CameraFollowLimitRectangle.Size);

                            GorgonGraphicsHelper.Instance.DrawRectangle(rect, 1, Color.DarkGoldenrod, worldScale);
                        }
                    }

                }
                else if (this.CurentCalque.Equals("LAYER"))
                {
                    CoronaLayer layer = this.mainForm.getElementTreeView().LayerSelected;
                    if (layer != null)
                    {

                        //Definir la zone d'affichage des tiles
                        if (layer.TilesMap != null)
                        {
                            if (layer.TilesMap.isEnabled == true)
                            {
                                Rectangle rect = new Rectangle(new Point(-offsetPoint.X, -offsetPoint.Y), this.surfacePictBx.Size);

                                layer.TilesMap.setSurfaceVisible(rect, worldScale, worldScale);
                            }
                        }

                        layer.dessineGorgonLayer(worldScale, worldScale, offsetPoint);
                       

                        //Dessiner la shape en construction 
                        if (this.FigActive != null)
                        {
                            this.FigActive.DrawGorgon(offsetPoint, 255, worldScale);
                          
                        }

                        //Dessiner la surface du text temporaire
                        if (this.surfaceTextTemp != Rectangle.Empty)
                        {
                            GorgonGraphicsHelper.Instance.DrawRectangle(new Rectangle(new Point(this.surfaceTextTemp.X + offsetPoint.X,
                                                                                                 this.surfaceTextTemp.Y + offsetPoint.Y),
                                                                                     this.surfaceTextTemp.Size), 1, Color.YellowGreen, worldScale);
                        }

                        if (drawFocusShader == true)
                        {

                            Camera camera = layer.SceneParent.Camera;
                            if (layer.SceneParent.projectParent.Orientation == CoronaGameProject.OrientationScreen.Portrait)
                                camera.SurfaceFocus = new Rectangle(camera.SurfaceFocus.Location, new Size(layer.SceneParent.projectParent.width,
                                    layer.SceneParent.projectParent.height));
                            else
                                camera.SurfaceFocus = new Rectangle(camera.SurfaceFocus.Location, new Size(layer.SceneParent.projectParent.height,
                                    layer.SceneParent.projectParent.width));


                            Color color = Color.FromArgb(125, Color.DarkGray);

                            int yTopDest = (int)((float)(camera.SurfaceFocus.Y + offsetPoint.Y) * worldScale);
                            Rectangle topRect = new Rectangle(0, 0,
                                GorgonLibrary.Gorgon.CurrentRenderTarget.Width, yTopDest);

                            int yBottomStart = (int)((float)(camera.SurfaceFocus.Y + camera.SurfaceFocus.Height + offsetPoint.Y) * worldScale);
                            Rectangle bottomRect = new Rectangle(0, yBottomStart,
                              GorgonLibrary.Gorgon.CurrentRenderTarget.Width, GorgonLibrary.Gorgon.CurrentRenderTarget.Height - yBottomStart);

                            int xLeftDest = (int)((float)(camera.SurfaceFocus.X + offsetPoint.X) * worldScale);
                            Rectangle leftRect = new Rectangle(0, yTopDest, xLeftDest, (int)((float)camera.SurfaceFocus.Height * worldScale));

                            int xRightStart = (int)((float)(camera.SurfaceFocus.X + camera.SurfaceFocus.Width + offsetPoint.X) * worldScale);
                            int xRightDest = GorgonLibrary.Gorgon.CurrentRenderTarget.Width - xRightStart;
                            Rectangle rightRect = new Rectangle(xRightStart, yTopDest, xRightDest, (int)((float)camera.SurfaceFocus.Height * worldScale));

                            GorgonGraphicsHelper.Instance.FillRectangle(topRect, 1, color, 1, false);
                            GorgonGraphicsHelper.Instance.FillRectangle(bottomRect, 1, color, 1, false);
                            GorgonGraphicsHelper.Instance.FillRectangle(leftRect, 1, color, 1, false);
                            GorgonGraphicsHelper.Instance.FillRectangle(rightRect, 1, color, 1, false);


                        }


                    }

                }

                if (this.mainForm != null)
                {
                    UserControl jointPanel = this.mainForm.CurrentJointPanel;

                    //Verifier que le panel n'est pas null
                    if (jointPanel != null)
                    {
                       

                        //Recuperer le panel correspondant au joint
                        if (jointPanel.Name.Equals("DISTANCE"))
                        {
                            DistancePropertiesPanel panelDistance = (DistancePropertiesPanel)jointPanel;
                            Point pObjA = Point.Empty;
                            if (panelDistance.objA != null)
                            {
                                pObjA = panelDistance.objA.DisplayObject.SurfaceRect.Location;
                            }

                            Point pObjB = Point.Empty;
                            if (panelDistance.objB != null)
                            {
                                pObjB = panelDistance.objB.DisplayObject.SurfaceRect.Location;
                            }


                            if (panelDistance.anchorPointA != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X +offsetPoint.X + panelDistance.anchorPointA.X - 3,
                                    pObjA.Y + offsetPoint.Y + panelDistance.anchorPointA.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);

                                GorgonGraphicsHelper.Instance.DrawText("Anchor A", "DEFAULT",10, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                            }

                            if (panelDistance.anchorPointB != Point.Empty)
                            {
                                Point pDest = new Point(pObjB.X + offsetPoint.X + panelDistance.anchorPointB.X - 3,
                                    pObjB.Y + offsetPoint.Y + panelDistance.anchorPointB.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.LightBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor B", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                               
                            }

                            if (panelDistance.anchorPointA != Point.Empty && panelDistance.anchorPointB != Point.Empty)
                            {
                                List<Point> points = new List<Point>();

                                Point pDest1 = new Point(pObjA.X + offsetPoint.X + panelDistance.anchorPointA.X,
                                   pObjA.Y + offsetPoint.Y + panelDistance.anchorPointA.Y);
                                Point pDest2 = new Point(pObjB.X + offsetPoint.X + panelDistance.anchorPointB.X,
                                    pObjB.Y + offsetPoint.Y + panelDistance.anchorPointB.Y);
                                points.Add(pDest1);
                                points.Add(pDest2);

                                GorgonGraphicsHelper.Instance.DrawLines(points, Color.Blue, 2, worldScale);

                                points.Clear();
                                points = null;

                              
                               
                          
                            }
                        }
                        else if (jointPanel.Name.Equals("FRICTION"))
                        {
                            FrictionPropertiesPanel panelFriction = (FrictionPropertiesPanel)jointPanel;
                            Point pObjA = Point.Empty;
                            if (panelFriction.objA != null)
                            {
                                pObjA = panelFriction.objA.DisplayObject.SurfaceRect.Location;
                            }

                            if (panelFriction.anchorPoint != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X+offsetPoint.X + panelFriction.anchorPoint.X - 3,
                                    pObjA.Y+offsetPoint.Y + panelFriction.anchorPoint.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                             
                            }
                        }
                        else if (jointPanel.Name.Equals("PISTON"))
                        {
                            PistonPropertiesPanel panelPiston = (PistonPropertiesPanel)jointPanel;
                            Point pObjA = Point.Empty;
                            if (panelPiston.objA != null)
                            {
                                pObjA = panelPiston.objA.DisplayObject.SurfaceRect.Location;
                            }

                            if (panelPiston.anchorPoint != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X + offsetPoint.X + panelPiston.anchorPoint.X - 3,
                                    pObjA.Y + offsetPoint.Y + panelPiston.anchorPoint.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                            }

                            if (panelPiston.axisDistance != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X + offsetPoint.X + panelPiston.axisDistance.X - 3,
                                    pObjA.Y + offsetPoint.Y + panelPiston.axisDistance.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.LightBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Axis Distance", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                          
                            }

                            if (panelPiston.anchorPoint != Point.Empty && panelPiston.axisDistance != Point.Empty)
                            {
                           
                                List<Point> points = new List<Point>();

                                Point pDest1 = new Point(pObjA.X + offsetPoint.X + panelPiston.anchorPoint.X,
                                    pObjA.Y + offsetPoint.Y + panelPiston.anchorPoint.Y);
                                Point pDest2 = new Point(pObjA.X + offsetPoint.X + panelPiston.axisDistance.X,
                                    pObjA.Y + offsetPoint.Y + panelPiston.axisDistance.Y);
                                points.Add(pDest1);
                                points.Add(pDest2);

                                GorgonGraphicsHelper.Instance.DrawLines(points, Color.Red, 2, worldScale);

                                points.Clear();
                                points = null;
                            }
                        }
                        else if (jointPanel.Name.Equals("PIVOT"))
                        {
                            PivotPropertiesPanel panelPivot = (PivotPropertiesPanel)jointPanel;
                            Point pObjA = Point.Empty;
                            if (panelPivot.objA != null)
                            {
                                pObjA = panelPivot.objA.DisplayObject.SurfaceRect.Location;
                            }

                            if (panelPivot.anchorPoint != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X + offsetPoint.X + panelPivot.anchorPoint.X - 3,
                                    pObjA.Y + offsetPoint.Y + panelPivot.anchorPoint.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                            }
                        }
                        else if (jointPanel.Name.Equals("PULLEY"))
                        {
                            PulleyPropertiesPanel panelPulley = jointPanel as PulleyPropertiesPanel;
                            Point pObjA = Point.Empty;
                            if (panelPulley.objA != null)
                            {
                                pObjA = panelPulley.objA.DisplayObject.SurfaceRect.Location;
                            }

                            Point pObjB = Point.Empty;
                            if (panelPulley.objB != null)
                            {
                                pObjB = panelPulley.objB.DisplayObject.SurfaceRect.Location;
                            }

                            if (panelPulley.anchorPointA != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X +offsetPoint.X + panelPulley.anchorPointA.X - 3 ,
                                    pObjA.Y + offsetPoint.Y + panelPulley.anchorPointA.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Rope Anchor A", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                               
                            }

                            if (panelPulley.anchorPointB != Point.Empty)
                            {
                                Point pDest = new Point(pObjB.X + offsetPoint.X + panelPulley.anchorPointB.X - 3 ,
                                    pObjB.Y + offsetPoint.Y + panelPulley.anchorPointB.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Rope Anchor B", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                         
                            }

                            if (panelPulley.anchorPointA != Point.Empty && panelPulley.anchorPointB != Point.Empty)
                            {
   
                                List<Point> points = new List<Point>();

                                Point pDest1 = new Point(pObjA.X + offsetPoint.X + panelPulley.anchorPointA.X ,
                                    pObjA.Y + offsetPoint.Y + panelPulley.anchorPointA.Y);
                                Point pDest2 = new Point(pObjB.X + offsetPoint.X + panelPulley.anchorPointB.X,
                                    pObjB.Y + offsetPoint.Y + panelPulley.anchorPointB.Y);
                                points.Add(pDest1);
                                points.Add(pDest2);

                                GorgonGraphicsHelper.Instance.DrawLines(points, Color.Green, 2, worldScale);

                                points.Clear();
                                points = null;
                            }

                            if (panelPulley.objAnchorPointA != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X + offsetPoint.X + panelPulley.objAnchorPointA.X - 3 ,
                                    pObjA.Y + offsetPoint.Y + panelPulley.objAnchorPointA.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.LightBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor A", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                         
     
                                List<Point> points = new List<Point>();

                                pDest = new Point(pObjA.X + offsetPoint.X + panelPulley.objAnchorPointA.X,
                                    pObjA.Y + offsetPoint.Y + panelPulley.objAnchorPointA.Y );
                                Point pDestRopeA = new Point(pObjA.X + offsetPoint.X + panelPulley.anchorPointA.X,
                                    pObjA.Y + offsetPoint.Y + panelPulley.anchorPointA.Y);
                                points.Add(pDestRopeA);
                                points.Add(pDest);

                                GorgonGraphicsHelper.Instance.DrawLines(points, Color.Green, 2, worldScale);

                                points.Clear();
                                points = null;
                            }

                            if (panelPulley.objAnchorPointB != Point.Empty)
                            {
                                Point pDest = new Point(pObjB.X+offsetPoint.X + panelPulley.objAnchorPointB.X - 3,
                                    pObjB.Y +offsetPoint.Y + panelPulley.objAnchorPointB.Y - 3 );

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.LightBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor B", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                                List<Point> points = new List<Point>();

                                pDest = new Point(pObjB.X + offsetPoint.X + panelPulley.objAnchorPointB.X,
                                   pObjB.Y + offsetPoint.Y + panelPulley.objAnchorPointB.Y);
                                Point pDestRopeB = new Point(pObjB.X + offsetPoint.X + panelPulley.anchorPointB.X
                                    , pObjB.Y + offsetPoint.Y + panelPulley.anchorPointB.Y);
                                points.Add(pDestRopeB);
                                points.Add(pDest);

                                GorgonGraphicsHelper.Instance.DrawLines(points, Color.DarkBlue, 2, worldScale);

                                points.Clear();
                                points = null;
                            }

                        }
                        else if (jointPanel.Name.Equals("WELD"))
                        {
                            WeldPropertiesPanel panelWeld = (WeldPropertiesPanel)jointPanel;
                            Point pObjA = Point.Empty;
                            if (panelWeld.objA != null)
                            {
                                pObjA = panelWeld.objA.DisplayObject.SurfaceRect.Location;
                            }

                          
                            if (panelWeld.anchorPoint != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X+offsetPoint.X + panelWeld.anchorPoint.X - 3,
                                    pObjA.Y + offsetPoint.Y + panelWeld.anchorPoint.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                            }
                        }
                        else if (jointPanel.Name.Equals("WHEEL"))
                        {
                            WheelPropertiesPanel panelWheel = (WheelPropertiesPanel)jointPanel;
                            Point pObjA = Point.Empty;
                            if (panelWheel.objA != null)
                            {
                                pObjA = panelWheel.objA.DisplayObject.SurfaceRect.Location;
                            }
                            if (panelWheel.anchorPoint != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X + offsetPoint.X + panelWheel.anchorPoint.X - 3,
                                    pObjA.Y + offsetPoint.Y + panelWheel.anchorPoint.Y - 3);


                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.DarkBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                            }

                            if (panelWheel.axisDistance != Point.Empty)
                            {
                                Point pDest = new Point(pObjA.X + offsetPoint.X + panelWheel.axisDistance.X - 3,
                                    pObjA.Y + offsetPoint.Y + panelWheel.axisDistance.Y - 3);

                                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 3, Color.LightBlue, worldScale, true);
                                GorgonGraphicsHelper.Instance.DrawText("Axis Distance", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                            }

                            if (panelWheel.anchorPoint != Point.Empty && panelWheel.axisDistance != Point.Empty)
                            {
              
                                List<Point> points = new List<Point>();

                                Point pDest1 = new Point(pObjA.X + offsetPoint.X + panelWheel.anchorPoint.X ,
                                    pObjA.Y + offsetPoint.Y + panelWheel.anchorPoint.Y);
                                Point pDest2 = new Point(pObjA.X + offsetPoint.X + panelWheel.axisDistance.X ,
                                    pObjA.Y + offsetPoint.Y + panelWheel.axisDistance.Y);
                                points.Add(pDest1);
                                points.Add(pDest2);

                                GorgonGraphicsHelper.Instance.DrawLines(points, Color.Red, 2, worldScale);

                                points.Clear();
                                points = null;
                            }
                        }
                      
                    }


                   
                }
             

            }
            catch (Exception ex)
            {
               

                MessageBox.Show("Error stage painting !\n" + ex.StackTrace, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                ////----------------------------------------------------------------------------------------------------
                ////-------------------------------------- EVENT REPORTING ---------------------------------------------
                ////----------------------------------------------------------------------------------------------------
                //Krea.KreaEventReports.NativeKreaEvent kreaEvent = new Krea.KreaEventReports.NativeKreaEvent(ex);
                //if (Settings1.Default.KreaEventReportAutomaticSend == true)
                //{
                //    KreaEventReports.KreaEventReportSender.ReportEvent(kreaEvent);
                //}
                //else
                //{
                //    KreaEventReports.KreaEventReporterForm reportForm = new KreaEventReports.KreaEventReporterForm();
                //    reportForm.init(kreaEvent);
                //    reportForm.Show();
                //}
                ////----------------------------------------------------------------------------------------------------
                ////----------------------------------------------------------------------------------------------------
            }

            Gorgon.CurrentRenderTarget.EndDrawing();
   

        }


       

        private void SceneEditorView_Load(object sender, EventArgs e)
        {
            
        }

        private void vScrollBar_Scroll(object sender, EventArgs e)
        {
            Point offsetInversed = this.getOffsetPoint();
            Point offSetFinal = new Point(-offsetInversed.X, -offsetInversed.Y);

            if (this.mainForm.getElementTreeView().SceneSelected != null)
                this.mainForm.getElementTreeView().SceneSelected.CurrentSceneViewLocation = offSetFinal;

            this.mainForm.graduationBarX.reportOffSetScrolling(offSetFinal);
            this.mainForm.graduationBarY.reportOffSetScrolling(offSetFinal);
            GorgonLibrary.Gorgon.Go();
        }

        private void hScrollBar_MouseLeave(object sender, EventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.None) == MouseButtons.None)
            {
                this.surfacePictBx.Focus();

            }
        }

       
    }
}
