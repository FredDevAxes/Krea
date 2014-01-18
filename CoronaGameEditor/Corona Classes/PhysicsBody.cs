using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using System.Drawing.Drawing2D;
using Krea.CGE_Figures;
using Krea.GameEditor.CollisionManager;
using System.Reflection;
using Krea.Corona_Classes;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class PhysicsBody
    {
        //---------------------------------------------------
        //-------------------Enums-----------------------
        //---------------------------------------------------
        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum PhysicBodyMode
        {
            Inactive = 1,
            Static = 2,
            Dynamic = 3,
            kinematic = 4,
        }

         //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        public List<BodyElement> BodyElements;
        public int CollisionGroupIndex = 0;
        public PhysicBodyMode Mode;
        public bool isCustomizedBody;
        public CoronaObject objectParent;
        public bool isFixedRotation = false;
        public decimal Bounce = 0;
        public decimal Density = 0;
        public decimal Friction = 0;
        public int Radius = -1;

        public float AngularDamping = 0;
        public float AngularVelocity = 0;
        public bool IsAwake = true;
        public bool IsBodyActive = true;
        public bool IsSensor = false;
        public bool IsBullet = false;
        public bool IsSleepingAllowed = true;
        public float LinearDamping = 0;
        public Point LinearVelocity = new Point(0, 0);

        public Size OriginSize;
        //---------------------------------------------------
        //-------------------Constructeurs------------------------
        //---------------------------------------------------

        public PhysicsBody(CoronaObject objectParent)
        {
            
            BodyElements = new List<BodyElement>();
            this.objectParent = objectParent;

            this.Mode = PhysicBodyMode.Inactive;
            isCustomizedBody = false;

           
        }

        public void updateBody()
        {
            DisplayObject dispObject = this.objectParent.DisplayObject;
            Figure fig = dispObject.Figure;
            if (fig != null)
            {
                if (fig.ShapeType.Equals("LINE"))
                {
                    Line line = fig as Line;
                    this.BodyElements.Clear();


                    //Creer un bodyElement tout les deux points
                    for (int i = 0; i < line.Points.Count - 1; i++)
                    {
                        List<Point> listePointsLigne = new List<Point>();

                        Point p1 = new Point(line.Points[i].X - dispObject.SurfaceRect.X,
                                                line.Points[i].Y - dispObject.SurfaceRect.Y);

                        Point p2 = new Point(line.Points[i + 1].X - dispObject.SurfaceRect.X,
                                                line.Points[i + 1].Y - dispObject.SurfaceRect.Y);


                        listePointsLigne.Add(p1);
                        listePointsLigne.Add(p2);

                        BodyElement elem = new BodyElement(this.BodyElements.Count, "Line" + this.BodyElements.Count, 0, 0, 0, listePointsLigne);
                        this.BodyElements.Add(elem);


                    }

                    this.isCustomizedBody = true;
                }
                else if (fig.ShapeType.Equals("CURVE"))
                {
                    //Recuperer tous les points de la curve
                    CourbeBezier courbe = fig as CourbeBezier;
                    this.BodyElements.Clear();

                    GraphicsPath path = new GraphicsPath(FillMode.Winding);
                    path.AddCurve(courbe.UserPoints.ToArray());
                    path.Flatten();
                    PointF[] finalPoints = path.PathPoints;

                    //Creer un bodyElement tout les deux points
                    for (int i = 0; i < finalPoints.Length - 1; i++)
                    {
                        List<Point> listePointsLigne = new List<Point>();

                        Point p1 = new Point((int)finalPoints[i].X - dispObject.SurfaceRect.X,
                                                (int)finalPoints[i].Y - dispObject.SurfaceRect.Y);
                        Point p2 = new Point((int)finalPoints[i + 1].X - dispObject.SurfaceRect.X
                                            , (int)finalPoints[i + 1].Y - dispObject.SurfaceRect.Y);
                        listePointsLigne.Add(p1);
                        listePointsLigne.Add(p2);

                        BodyElement elem = new BodyElement(this.BodyElements.Count, "Line" + this.BodyElements.Count, 0, 0, 0, listePointsLigne);
                        this.BodyElements.Add(elem);


                    }

                    this.isCustomizedBody = true;
                }


            }
            else
            {
                if (dispObject.Type.Equals("SPRITE") || dispObject.Type.Equals("IMAGE"))
                {
                    if (this.OriginSize.Width > 0 && this.OriginSize.Height > 0)
                    {
                        double scaleX = Convert.ToDouble(dispObject.SurfaceRect.Width) / Convert.ToDouble(this.OriginSize.Width);
                        double scaleY = Convert.ToDouble(dispObject.SurfaceRect.Height) / Convert.ToDouble(this.OriginSize.Height);
                        for (int i = 0; i < this.BodyElements.Count; i++)
                        {
                            BodyElement elem = this.BodyElements[i];
                            if (elem.Type.Equals("CIRCLE"))
                            {
                                elem.Radius = Convert.ToInt32(Convert.ToDouble(elem.Radius) * scaleX);

                            }
                            else if (elem.Type.Equals("SHAPE"))
                            {
                                for (int j = 0; j < elem.BodyShape.Count; j++)
                                {
                                    Point p = elem.BodyShape[j];

                                    p.X = Convert.ToInt32(Convert.ToDouble(p.X) * scaleX);
                                    p.Y = Convert.ToInt32(Convert.ToDouble(p.Y) * scaleY);
                                    elem.BodyShape[j] = p;
                                }


                            }
                        }
                    }
                    

                }
                this.OriginSize = dispObject.SurfaceRect.Size;
            }

        }


        public String getPhysicsParams(CoronaObject objParent,float XRatio,float YRatio)
        {
            string paramsPhysics = "";

            float moyenneRatio = (XRatio + YRatio) / 2;
            if (this.isCustomizedBody == true)
            {
                
                for (int i = 0; i < this.BodyElements.Count; i++)
                {
                    BodyElement elem = this.BodyElements[i];

                    if (i > 0) paramsPhysics += ",\n";

                    paramsPhysics += "{ ";

                    //Recuperer les properties
                    
                  /*  paramsPhysics += ("bounce =" + ((float)elem.Bounce / moyenneRatio)).Replace(",", ".") + ",";
                    paramsPhysics += ("density =" + ((float)elem.Density/ moyenneRatio)).Replace(",", ".") + ",";
                    paramsPhysics += ("friction =" + ((float)elem.Friction / moyenneRatio)).Replace(",", ".");
                    paramsPhysics += ",filter = " + this.getCollisionFilter();*/

                    paramsPhysics += "bounce =" + elem.Bounce.ToString().Replace(",", ".") + ",";
                    paramsPhysics += "density =" + elem.Density.ToString().Replace(",", ".") + ",";
                    paramsPhysics += "friction =" + elem.Friction.ToString().Replace(",", ".");
                    paramsPhysics += ",isSensor =" + elem.IsSensor.ToString().ToLower() + ",";

                    if(elem.CollisionGroupIndex >0)
                        paramsPhysics += "filter = " + this.getCollisionFilter(elem.CollisionGroupIndex);
                    else
                        paramsPhysics += "filter = " + this.getCollisionFilter(this.CollisionGroupIndex);

                    if (elem.Type.Equals("CIRCLE"))
                    {

                        if (elem.Radius > 0)
                            paramsPhysics += ",radius =" + (elem.Radius * moyenneRatio).ToString().Replace(",", ".");
                    }
                    else
                    {
                        if (objParent.DisplayObject.Type.Equals("FIGURE") && (objParent.DisplayObject.Figure.ShapeType.Equals("CURVE") || objParent.DisplayObject.Figure.ShapeType.Equals("LINE")))
                        {
                            //Recuperer la shape du body si elle existe
                            if (elem.BodyShape.Count > 1)
                            {
                                paramsPhysics += ",shape = {";
                                for (int j = 0; j < elem.BodyShape.Count; j++)
                                {
                                    if (j != 0) paramsPhysics += ",\n";

                                    Point p = elem.BodyShape[j];

                                    paramsPhysics += ((float)(p.X - BodyElements[0].BodyShape[0].X) * XRatio).ToString().Replace(",", ".") + "," +
                                                    ((float)(p.Y - BodyElements[0].BodyShape[0].Y) * YRatio).ToString().Replace(",", ".");
                                }

                                paramsPhysics += "}";
                            }
                        }
                        else
                        {
                           
                            //Recuperer la shape du body si elle existe
                            if (elem.BodyShape.Count > 1)
                            {
                                paramsPhysics += ",shape = {";
                                for (int j = 0; j < elem.BodyShape.Count; j++)
                                {
                                    if (j != 0) paramsPhysics += ",\n";

                                    Point p = elem.BodyShape[j];

                                    paramsPhysics += ((float)((p.X - objParent.DisplayObject.SurfaceRect.Width / 2) * moyenneRatio)).ToString().Replace(",", ".") + "," +
                                                    ((float)((p.Y - objParent.DisplayObject.SurfaceRect.Height / 2) * moyenneRatio)).ToString().Replace(",", ".");
                                   
                                }

                                paramsPhysics += "}";
                            }
                        }
                       
                    }

                    paramsPhysics += "}";
                }
            }
            else
            {

                paramsPhysics += "{ ";

                //Recuperer les properties
                paramsPhysics += ("bounce =" + Bounce).Replace(",", ".") + ",";
                paramsPhysics += ("density =" + Density).Replace(",", ".") + ",";
                paramsPhysics += ("friction =" + Friction).Replace(",", ".");
                paramsPhysics += ",filter = " + this.getCollisionFilter(this.CollisionGroupIndex);

                if (objParent.DisplayObject.Type.Equals("FIGURE") && objParent.DisplayObject.Figure.ShapeType.Equals("CIRCLE"))
                {
                    Cercle cercle = objParent.DisplayObject.Figure as Cercle;
                    paramsPhysics += ",radius =" + ((float)cercle.Rayon * moyenneRatio).ToString().Replace(",", ".");
                }
                else if (this.Radius > 0)
                {
                    paramsPhysics += ",radius =" + ((float)this.Radius * XRatio).ToString().Replace(",", ".");
                }
                else
                {
                    float halfContentWidth = (float)this.objectParent.DisplayObject.SurfaceRect.Width / (float)2 * (float)XRatio;
                    float halfContentHeight = (float)this.objectParent.DisplayObject.SurfaceRect.Height / (float)2 * (float)YRatio;
                    paramsPhysics += ",shape = {" + (-halfContentWidth).ToString().Replace(",", ".") + "," + (-halfContentHeight).ToString().Replace(",", ".") + "," +
                        (halfContentWidth).ToString().Replace(",", ".") + "," + (-halfContentHeight).ToString().Replace(",", ".") + "," +
                        (halfContentWidth).ToString().Replace(",", ".") + "," + (halfContentHeight).ToString().Replace(",", ".") + "," +
                        (-halfContentWidth).ToString().Replace(",", ".") + "," + (halfContentHeight).ToString().Replace(",", ".") + "}";

                }

                paramsPhysics += "}";


            }
            
            

            return paramsPhysics;
        }

        public void drawGorgonBodyElements(Color color, bool applyPointsAjustingWithParent, Point offsetPoint, float worldScale,bool applyRotation)
        {
            if (this.BodyElements.Count == 0) this.isCustomizedBody = false;

            Color colorElements = Color.FromArgb(150, Color.GreenYellow);
            if (this.isCustomizedBody == true)
            {
               
                for (int i = 0; i < this.BodyElements.Count; i++)
                {
                    BodyElement elem = this.BodyElements[i];

                    if (elem.Type.Equals("SHAPE"))
                    {
                        if (applyPointsAjustingWithParent == true)
                        {
                            //Creer un tableau de points ajustés
                            List<Point> tabPointsAjust = new List<Point>();

                            Point middlePoint = new Point(objectParent.DisplayObject.SurfaceRect.X + objectParent.DisplayObject.SurfaceRect.Width / 2 + offsetPoint.X,
                                                objectParent.DisplayObject.SurfaceRect.Y + objectParent.DisplayObject.SurfaceRect.Height / 2 + offsetPoint.Y);
                            
                            float angle = this.objectParent.DisplayObject.Rotation;
                            double cosRotation = Math.Cos(Math.PI * angle / 180.0);
                            double sinRotation = Math.Sin(Math.PI * angle / 180.0);
                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {

                                Point pAjust = new Point(elem.BodyShape[j].X + this.objectParent.DisplayObject.SurfaceRect.X + offsetPoint.X,
                                                      elem.BodyShape[j].Y + this.objectParent.DisplayObject.SurfaceRect.Y + offsetPoint.Y);

                                if (applyRotation == true)
                                {
                                    int rotatedX = (int)(middlePoint.X + cosRotation * (pAjust.X - middlePoint.X) -
                                                 Math.Sin(Math.PI * angle / 180.0) * (pAjust.Y - middlePoint.Y));

                                    int rotatedY = (int)(middlePoint.Y + sinRotation * (pAjust.X - middlePoint.X) +
                                   cosRotation * (pAjust.Y - middlePoint.Y));

                                    Point pRotated = new Point(rotatedX, rotatedY);

                                    tabPointsAjust.Insert(j, pRotated);
                                }
                                else
                                {
                                    tabPointsAjust.Insert(j, pAjust);
                                }
                                //Close the figure
                                 if (j == elem.BodyShape.Count - 1)
                                {
                                   tabPointsAjust.Add(tabPointsAjust[0]);
                                }


                            }
                          
                            GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 1, worldScale);

                        }
                        else
                        {
                            //Creer un tableau de points ajustés
                            List<Point> tabPointsAjust = new List<Point>();

                            Point middlePoint = new Point( objectParent.DisplayObject.SurfaceRect.Width / 2,
                                             objectParent.DisplayObject.SurfaceRect.Height / 2 );

                            float angle = this.objectParent.DisplayObject.Rotation;
                            double cosRotation = Math.Cos(Math.PI * angle / 180.0);
                            double sinRotation = Math.Sin(Math.PI * angle / 180.0);

                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {

                                Point pAjust = new Point((int)((float)(elem.BodyShape[j].X  + offsetPoint.X)) ,
                                                      (int)((float)(elem.BodyShape[j].Y + offsetPoint.Y) ));

                                if (applyRotation == true)
                                {
                                    int rotatedX = (int)(middlePoint.X + cosRotation * (pAjust.X - middlePoint.X) -
                                                 Math.Sin(Math.PI * angle / 180.0) * (pAjust.Y - middlePoint.Y));

                                    int rotatedY = (int)(middlePoint.Y + sinRotation * (pAjust.X - middlePoint.X) +
                                   cosRotation * (pAjust.Y - middlePoint.Y));

                                    Point pRotated = new Point(rotatedX, rotatedY);

                                    tabPointsAjust.Insert(j, pRotated);
                                }
                                else
                                {
                                    tabPointsAjust.Insert(j, pAjust);
                                }
                                //Close the figure
                                if (j == elem.BodyShape.Count - 1)
                                {
                                    tabPointsAjust.Add(tabPointsAjust[0]);
                                }

                            }
                          
                            GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 1, worldScale);
                        }

                    }
                    else if (elem.Type.Equals("CIRCLE"))
                    {
                        int radius = elem.Radius;

                        if (applyPointsAjustingWithParent == true)
                        {
                            GorgonGraphicsHelper.Instance.FillCircle(elem.SurfaceCircle.X + this.objectParent.DisplayObject.SurfaceRect.X + offsetPoint.X,
                                            elem.SurfaceCircle.Y + this.objectParent.DisplayObject.SurfaceRect.Y + offsetPoint.Y,
                                            elem.Radius, colorElements, worldScale, false);
                          
                        }
                        else
                        {
                            GorgonGraphicsHelper.Instance.FillCircle(elem.SurfaceCircle.X + offsetPoint.X,
                                             elem.SurfaceCircle.Y + offsetPoint.Y,
                                             elem.Radius, colorElements, worldScale, false);
                        }

                    }
                    else if (elem.Type.Equals("LINE"))
                    {

                        if (applyPointsAjustingWithParent == true)
                        {
                            //Creer un tableau de points ajustés
                            List<Point> tabPointsAjust = new List<Point>();
                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {

                                Point pAjust = new Point(elem.BodyShape[j].X + this.objectParent.DisplayObject.SurfaceRect.X + offsetPoint.X,
                                                      elem.BodyShape[j].Y + this.objectParent.DisplayObject.SurfaceRect.Y + offsetPoint.Y);

                                tabPointsAjust.Insert(j, pAjust);
                            }
                            GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 2, worldScale);
                        }
                        else
                        {
                            //Creer un tableau de points ajustés
                            List<Point> tabPointsAjust = new List<Point>();
                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {

                                Point pAjust = new Point(elem.BodyShape[j].X + offsetPoint.X,
                                                      elem.BodyShape[j].Y + offsetPoint.Y);

                                tabPointsAjust.Insert(j, pAjust);
                            }
                            GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 2, worldScale);
                        }
                    }
                }


            }
            else
            {
                DisplayObject dispObj = this.objectParent.DisplayObject;
                if (dispObj.Type.Equals("FIGURE"))
                {
                    Figure fig = this.objectParent.DisplayObject.Figure;
                    if (fig != null)
                    {
                        if (fig.ShapeType.Equals("CIRCLE"))
                        {
                            Cercle circ = fig as Cercle;
                         
                            GorgonGraphicsHelper.Instance.FillCircle(circ.Position.X + offsetPoint.X,
                                            circ.Position.Y + offsetPoint.Y,
                                            circ.Rayon, colorElements, worldScale, false);
                        }
                        else if (fig.ShapeType.Equals("RECTANGLE"))
                        {
                            Rect rect = fig as Rect;
                          
                            //GorgonGraphicsHelper.Instance.FillRectangle(new Rectangle(rect.Position.X + offsetPoint.X,
                            //        rect.Position.Y + offsetPoint.Y, rect.Width, rect.Height),
                            //    2, colorElements, worldScale);
                            //Creer un tableau de points ajustés
                            List<Point> tabPointsAjust = new List<Point>();

                            Point middlePoint = new Point(objectParent.DisplayObject.SurfaceRect.X + objectParent.DisplayObject.SurfaceRect.Width / 2 + offsetPoint.X,
                                                   objectParent.DisplayObject.SurfaceRect.Y + objectParent.DisplayObject.SurfaceRect.Height / 2 + offsetPoint.Y);


                            float angle = this.objectParent.DisplayObject.Rotation;
                            double cosRotation = Math.Cos(Math.PI * angle / 180.0);
                            double sinRotation = Math.Sin(Math.PI * angle / 180.0);

                            List<Point> rectPoints = new List<Point>();
                            rectPoints.Add(rect.Position);
                            rectPoints.Add(new Point(rect.Position.X + rect.Width, rect.Position.Y));
                            rectPoints.Add(new Point(rect.Position.X + rect.Width, dispObj.SurfaceRect.Bottom));
                            rectPoints.Add(new Point(rect.Position.X, dispObj.SurfaceRect.Bottom));


                            for (int j = 0; j < rectPoints.Count; j++)
                            {

                                Point pAjust = new Point((int)((float)(rectPoints[j].X + offsetPoint.X)),
                                                      (int)((float)(rectPoints[j].Y + offsetPoint.Y)));

                                if (applyRotation == true)
                                {
                                    int rotatedX = (int)(middlePoint.X + cosRotation * (pAjust.X - middlePoint.X) -
                                                 Math.Sin(Math.PI * angle / 180.0) * (pAjust.Y - middlePoint.Y));

                                    int rotatedY = (int)(middlePoint.Y + sinRotation * (pAjust.X - middlePoint.X) +
                                   cosRotation * (pAjust.Y - middlePoint.Y));

                                    Point pRotated = new Point(rotatedX, rotatedY);

                                    tabPointsAjust.Insert(j, pRotated);
                                }
                                else
                                {
                                    tabPointsAjust.Insert(j, pAjust);
                                }
                                //Close the figure
                                if (j == rectPoints.Count - 1)
                                {
                                    tabPointsAjust.Add(tabPointsAjust[0]);
                                }

                            }

                            GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 1, worldScale);


                        }
                        else if (fig.ShapeType.Equals("LINE"))
                        {
                            Krea.CGE_Figures.Line line = fig as Krea.CGE_Figures.Line;
                            //Convertir tous les points et leur ajouter un offset
                            //Creer un tableau de points ajustés
                            List<Point> tabPointsAjust = new List<Point>();
                            for (int j = 0; j < line.Points.Count; j++)
                            {

                                Point pAjust = new Point(line.Points[j].X + offsetPoint.X,
                                                      line.Points[j].Y + offsetPoint.Y);

                                tabPointsAjust.Insert(j, pAjust);
                            }
                            GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 2, worldScale);

                        }
                        else if (fig.ShapeType.Equals("TEXT"))
                        {

                            GorgonGraphicsHelper.Instance.FillRectangle(dispObj.SurfaceRect, 2, colorElements, worldScale, false);
                        }
                    }
                }
                else
                {
                    if (applyPointsAjustingWithParent == true)
                    {
                        //GorgonGraphicsHelper.Instance.FillRectangle(new Rectangle(dispObj.SurfaceRect.X + offsetPoint.X, dispObj.SurfaceRect.Y + offsetPoint.Y,
                        //    dispObj.SurfaceRect.Width, dispObj.SurfaceRect.Height), 2, colorElements, worldScale);

                        //Creer un tableau de points ajustés
                        List<Point> tabPointsAjust = new List<Point>();

                        Point middlePoint = new Point(objectParent.DisplayObject.SurfaceRect.X + objectParent.DisplayObject.SurfaceRect.Width / 2 + offsetPoint.X,
                                               objectParent.DisplayObject.SurfaceRect.Y + objectParent.DisplayObject.SurfaceRect.Height / 2 + offsetPoint.Y);
                            

                        float angle = this.objectParent.DisplayObject.Rotation;
                        double cosRotation = Math.Cos(Math.PI * angle / 180.0);
                        double sinRotation = Math.Sin(Math.PI * angle / 180.0);

                        List<Point> rectPoints = new List<Point>();
                        rectPoints.Add(dispObj.SurfaceRect.Location);
                        rectPoints.Add(new Point(dispObj.SurfaceRect.Right, dispObj.SurfaceRect.Top));
                        rectPoints.Add(new Point(dispObj.SurfaceRect.Right, dispObj.SurfaceRect.Bottom));
                        rectPoints.Add(new Point(dispObj.SurfaceRect.Left, dispObj.SurfaceRect.Bottom));


                        for (int j = 0; j < rectPoints.Count; j++)
                        {

                            Point pAjust = new Point((int)((float)(rectPoints[j].X + offsetPoint.X)),
                                                  (int)((float)(rectPoints[j].Y + offsetPoint.Y)));

                            if (applyRotation == true)
                            {
                                int rotatedX = (int)(middlePoint.X + cosRotation * (pAjust.X - middlePoint.X) -
                                             Math.Sin(Math.PI * angle / 180.0) * (pAjust.Y - middlePoint.Y));

                                int rotatedY = (int)(middlePoint.Y + sinRotation * (pAjust.X - middlePoint.X) +
                               cosRotation * (pAjust.Y - middlePoint.Y));

                                Point pRotated = new Point(rotatedX, rotatedY);

                                tabPointsAjust.Insert(j, pRotated);
                            }
                            else
                            {
                                tabPointsAjust.Insert(j, pAjust);
                            }
                            //Close the figure
                            if (j == rectPoints.Count - 1)
                            {
                                tabPointsAjust.Add(tabPointsAjust[0]);
                            }

                        }

                        GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 1, worldScale);
                    }

                    else
                    {
                         //GorgonGraphicsHelper.FillRectangle(new Rectangle(offsetPoint.X, offsetPoint.Y, dispObj.SurfaceRect.Width, dispObj.SurfaceRect.Height),
                         //                       2, colorElements, worldScale);
                        //Creer un tableau de points ajustés
                        List<Point> tabPointsAjust = new List<Point>();

                        Point middlePoint = new Point(objectParent.DisplayObject.SurfaceRect.Width / 2 + offsetPoint.X,
                                               objectParent.DisplayObject.SurfaceRect.Height / 2 + offsetPoint.Y);
                            

                        float angle = this.objectParent.DisplayObject.Rotation;
                        double cosRotation = Math.Cos(Math.PI * angle / 180.0);
                        double sinRotation = Math.Sin(Math.PI * angle / 180.0);

                        List<Point> rectPoints = new List<Point>();
                        rectPoints.Add(offsetPoint);
                        rectPoints.Add(new Point(dispObj.SurfaceRect.Width + offsetPoint.X, offsetPoint.Y));
                        rectPoints.Add(new Point(dispObj.SurfaceRect.Width + offsetPoint.X, dispObj.SurfaceRect.Height + offsetPoint.Y));
                        rectPoints.Add(new Point(offsetPoint.X, dispObj.SurfaceRect.Height + offsetPoint.Y));


                        for (int j = 0; j < rectPoints.Count; j++)
                        {

                            Point pAjust = new Point((int)((float)(rectPoints[j].X + offsetPoint.X)),
                                                  (int)((float)(rectPoints[j].Y + offsetPoint.Y)));

                            if (applyRotation == true)
                            {
                                int rotatedX = (int)(middlePoint.X + cosRotation * (pAjust.X - middlePoint.X) -
                                             Math.Sin(Math.PI * angle / 180.0) * (pAjust.Y - middlePoint.Y));

                                int rotatedY = (int)(middlePoint.Y + sinRotation * (pAjust.X - middlePoint.X) +
                               cosRotation * (pAjust.Y - middlePoint.Y));

                                Point pRotated = new Point(rotatedX, rotatedY);

                                tabPointsAjust.Insert(j, pRotated);
                            }
                            else
                            {
                                tabPointsAjust.Insert(j, pAjust);
                            }
                            //Close the figure
                            if (j == rectPoints.Count - 1)
                            {
                                tabPointsAjust.Add(tabPointsAjust[0]);
                            }

                        }

                        GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, colorElements, 1, worldScale);
                    }
                       
                        
                }
            }
            
            
        }

        public void dessineAllBodyELements(Graphics g,Brush brush, bool applyPointsAjustingWithParent,Point offsetPoint)
        {
            if (this.BodyElements.Count == 0) this.isCustomizedBody = false;
            
            if (this.isCustomizedBody == true)
            {

                for (int i = 0; i < this.BodyElements.Count; i++)
                {
                    BodyElement elem = this.BodyElements[i];

                    if (elem.Type.Equals("SHAPE"))
                    {
                        if (applyPointsAjustingWithParent == true)
                        {
                            //Creer un tableau de points ajustés
                            Point[] tabPointsAjust = new Point[elem.BodyShape.Count];
                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {

                                Point pAjust = new Point(elem.BodyShape[j].X + this.objectParent.DisplayObject.SurfaceRect.X + offsetPoint.X,
                                                      elem.BodyShape[j].Y + this.objectParent.DisplayObject.SurfaceRect.Y + offsetPoint.Y);

                                tabPointsAjust[j] = pAjust;
                            }
                            g.FillPolygon(brush, tabPointsAjust);
                        }
                        else
                        {
                            //Creer un tableau de points ajustés
                            Point[] tabPointsAjust = new Point[elem.BodyShape.Count];
                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {

                                Point pAjust = new Point(elem.BodyShape[j].X + offsetPoint.X,
                                                      elem.BodyShape[j].Y + offsetPoint.Y);

                                tabPointsAjust[j] = pAjust;
                            }
                            g.FillPolygon(brush, tabPointsAjust);
                        }

                    }
                    else if (elem.Type.Equals("CIRCLE"))
                    {
                        int radius = elem.Radius;

                        if (applyPointsAjustingWithParent == true)
                        {
                            g.FillEllipse(brush, new Rectangle(new Point(elem.SurfaceCircle.X + this.objectParent.DisplayObject.SurfaceRect.X + offsetPoint.X,
                                                                   elem.SurfaceCircle.Y + this.objectParent.DisplayObject.SurfaceRect.Y + offsetPoint.Y)
                                                         , new Size(elem.Radius*2, elem.Radius*2)));
                        }
                        else
                        {
                            g.FillEllipse(brush, new Rectangle(new Point(elem.SurfaceCircle.X + offsetPoint.X,
                                                                   elem.SurfaceCircle.Y + offsetPoint.Y)
                                                         , new Size(elem.Radius * 2, elem.Radius * 2)));
                        }

                    }
                    else if (elem.Type.Equals("LINE"))
                    {

                        if (applyPointsAjustingWithParent == true)
                        {
                            //Creer un tableau de points ajustés
                            Point[] tabPointsAjust = new Point[elem.BodyShape.Count];
                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {
                                Point pAjust;
                              
                                 pAjust = new Point(elem.BodyShape[j].X + this.objectParent.DisplayObject.SurfaceRect.X + offsetPoint.X,
                                                        elem.BodyShape[j].Y + this.objectParent.DisplayObject.SurfaceRect.Y + offsetPoint.Y);

                                tabPointsAjust[j] = pAjust;
                            }


                            g.DrawLines(Pens.GreenYellow, tabPointsAjust);
                        }
                        else
                        {
                            //Creer un tableau de points ajustés
                            Point[] tabPointsAjust = new Point[elem.BodyShape.Count];
                            for (int j = 0; j < elem.BodyShape.Count; j++)
                            {

                                Point pAjust = new Point(elem.BodyShape[j].X + offsetPoint.X,
                                                      elem.BodyShape[j].Y + offsetPoint.Y);

                                tabPointsAjust[j] = pAjust;
                            }
                            g.DrawLines(Pens.GreenYellow, tabPointsAjust);
                        }
                    }
                }
                

            }
            else
            {
                DisplayObject dispObj = this.objectParent.DisplayObject;
                if (dispObj.Type.Equals("FIGURE"))
                {
                    Figure fig = this.objectParent.DisplayObject.Figure;
                    if (fig != null)
                    {
                        if (fig.ShapeType.Equals("CIRCLE"))
                        {
                            Cercle circ = fig as Cercle;
                            g.FillEllipse(brush, circ.Position.X + offsetPoint.X, circ.Position.Y + offsetPoint.Y, circ.Rayon * 2, circ.Rayon * 2);
                        }
                        else if (fig.ShapeType.Equals("RECTANGLE"))
                        {
                            Rect rect = fig as Rect;
                            g.FillRectangle(brush, rect.Position.X + offsetPoint.X, rect.Position.Y + offsetPoint.Y, rect.Width, rect.Height);
                        }
                        else if (fig.ShapeType.Equals("LINE"))
                        {
                            Krea.CGE_Figures.Line line = fig as Krea.CGE_Figures.Line;
                            //Convertir tous les points et leur ajouter un offset
                            Point[] tabPoints = new Point[line.Points.Count];
                            for (int i = 0; i < line.Points.Count; i++)
                            {
                                tabPoints[i] = new Point(line.Points[i].X + offsetPoint.X, line.Points[i].Y + offsetPoint.Y);
                            }

                            if (line.Points.Count > 2)
                            {
                                g.DrawLines(Pens.GreenYellow, tabPoints);

                            }
                            else if (line.Points.Count == 2)
                            {
                                g.DrawLine(Pens.GreenYellow, tabPoints[0], tabPoints[1]);
                            }
                        }
                        else if (fig.ShapeType.Equals("TEXT"))
                        {
                            Texte txt = fig as Texte;
                            SizeF size = g.MeasureString(txt.txt, new Font(txt.font2.FamilyName,txt.font2.Size));
                            g.FillRectangle(brush, txt.Position.X + offsetPoint.X, txt.Position.Y + offsetPoint.Y, size.Width, size.Height);
                        }
                    }
                }
                else
                {
                    if(applyPointsAjustingWithParent == true)
                         g.FillRectangle(brush, dispObj.SurfaceRect.X + offsetPoint.X, dispObj.SurfaceRect.Y + offsetPoint.Y, dispObj.SurfaceRect.Width, dispObj.SurfaceRect.Height);
                    else
                        g.FillRectangle(brush, offsetPoint.X, offsetPoint.Y, dispObj.SurfaceRect.Width, dispObj.SurfaceRect.Height);
                }
            }
            
            

        }


        public bool isPointIsInBody(Point p)
        {
            if (this.isCustomizedBody == true)
            {
                for (int i = 0; i < this.BodyElements.Count; i++)
                {
                    BodyElement elem = this.BodyElements[i];
                    if (elem.Type.Equals("SHAPE"))
                    {
                        if (this.IsInPolygon(elem.BodyShape.ToArray(), p) == true)
                            return true;
                    }
                    else if(elem.Type.Equals("CIRCLE"))
                    {
                        Rectangle rectSurface = new Rectangle(new Point(this.objectParent.DisplayObject.SurfaceRect.X + elem.SurfaceCircle.X,
                                                                        this.objectParent.DisplayObject.SurfaceRect.Y + elem.SurfaceCircle.Y),
                                                                        elem.SurfaceCircle.Size);
                        if (rectSurface.Contains(p))
                            return true;
                        else return false;
                    }

                }
                return false;
            }
            else
            {
                return this.objectParent.DisplayObject.SurfaceRect.Contains(p);
            }
                 
            
        }

        private bool IsInPolygon(Point[] poly, Point point)
        {
            var coef = poly.Skip(1).Select((p, i) => (point.Y - poly[i].Y) * (p.X - poly[i].X) - (point.X - poly[i].X) * (p.Y - poly[i].Y)).ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }


        public String getCollisionFilter(int groupIndex)
        {
            if (groupIndex + 1 <= this.objectParent.LayerParent.SceneParent.CollisionFilterGroups.Count)
            {
                CollisionFilterGroup groupFilter = this.objectParent.LayerParent.SceneParent.CollisionFilterGroups[groupIndex];
                string collisionFilter = "{ ";
                collisionFilter += "categoryBits = " + groupFilter.CategorieBit + ", maskBits = " + groupFilter.getMaskBits();
                collisionFilter += "}";
                return collisionFilter;
            }
            else
            {
                //string collisionFilter = "{ ";
                //collisionFilter += "categoryBits = 0, maskBits = 0" ;
                //collisionFilter += "}";
                //return collisionFilter;

                return "nil";
            }
               

           
        }

       


        public PhysicsBody cloneBody(CoronaObject objectParent)
        {
            PhysicsBody newBody = new PhysicsBody(objectParent);

            newBody.Mode = this.Mode;
            newBody.CollisionGroupIndex = this.CollisionGroupIndex;

            if (this.isCustomizedBody == true)
            {
                for (int i = 0; i < this.BodyElements.Count; i++)
                {
                    BodyElement elem = this.BodyElements[i];
                    if (elem.Type.Equals("CIRCLE"))
                    {

                        BodyElement newElem = new BodyElement(i, elem.Name, elem.Bounce, elem.Density, elem.Friction, elem.LocationCircle, elem.Radius);
                        newElem.CollisionGroupIndex = elem.CollisionGroupIndex;
                        newBody.BodyElements.Add(newElem);
                    }
                    else
                    {
                        //Create a new bodyElement
                        List<Point> bodyShape = elem.BodyShape.ToList();

                        BodyElement newElem = new BodyElement(i, elem.Name, elem.Bounce, elem.Density, elem.Friction, bodyShape);
                        newElem.CollisionGroupIndex = elem.CollisionGroupIndex;
                        newBody.BodyElements.Add(newElem);

                    }
                   
                }
                newBody.isCustomizedBody = true;
            }
            else
            {
                newBody.Bounce = this.Bounce;
                newBody.Density = this.Density;
                newBody.Friction = this.Friction;
                newBody.isCustomizedBody = false;
            }

            
            newBody.isFixedRotation = this.isFixedRotation;
            return newBody;

        }
    }
}
