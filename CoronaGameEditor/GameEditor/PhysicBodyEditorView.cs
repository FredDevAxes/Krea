using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using PolygonCuttingEar;
using GeometryUtility;
using System.Drawing.Drawing2D;
using Krea.CoronaClasses;
using Krea.Corona_Classes;
namespace Krea.GameEditor
{
    public partial class PhysicBodyEditorView : UserControl
    {
        //---------------------------------------------------
        //-------------------Attributs--------------------
        //---------------------------------------------------

        public String DrawMode;


        private CoronaObject coronaObject;
        public List<Point> shapeBuilding;
        private Rectangle surfaceCircleBuilding;
        private Form1 mainForm;

        //--Dessinage
        private Boolean isMouseDown;
        private Point lastPos;
        private float currentScale = 1;
        
        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public PhysicBodyEditorView()
        {
            InitializeComponent();

            
        }

        public void init(Form1 mainForm, CoronaObject coronaObject)
        {
            this.mainForm = mainForm;
            this.coronaObject = coronaObject;

         

            this.graduationBarY.setDefaultOffset(this.graduationBarX.Size.Height);
            this.setScrollBarsValues();
            
            setModeNormal();
            isMouseDown = false;

            if (this.mainForm.isFormLocked == false)
                GorgonLibrary.Gorgon.Go();

        }

        public Point getScrollOffsetPoint()
        {
            return new Point(-this.hScrollBar1.Value, -this.vScrollBar1.Value);
        }

        public void setScrollBarsValues()
        {
            if (this.coronaObject != null)
            {
                this.hScrollBar1.Minimum = 0;
                this.vScrollBar1.Minimum = 0;

                int xOffSetMax =  (int)(this.coronaObject.DisplayObject.SurfaceRect.Width *this.currentScale - this.surfacePictBx.Width);
                if (xOffSetMax < 0) xOffSetMax = 0;

                int yOffSetMax = (int)(this.coronaObject.DisplayObject.SurfaceRect.Height * this.currentScale - this.surfacePictBx.Height);
                if (yOffSetMax < 0) yOffSetMax = 0;

                this.hScrollBar1.Maximum = (int)(xOffSetMax *(1/this.currentScale));
                this.vScrollBar1.Maximum = (int)(yOffSetMax * (1 / this.currentScale));
            }
        }
        

        
        public void newShape()
        {
            this.shapeBuilding = new List<Point>();
            this.DrawMode = "SHAPE";
            surfaceCircleBuilding = Rectangle.Empty;
        }

        public void newHandShape()
        {
            this.shapeBuilding = new List<Point>();
            this.DrawMode = "HAND";
            surfaceCircleBuilding = Rectangle.Empty;
        }

        public void newCircle()
        {
            this.DrawMode = "CIRCLE";
            surfaceCircleBuilding = new Rectangle(0,0,10,10);
            this.shapeBuilding = null;
        }

        public void setModeNormal()
        {
            this.DrawMode = "NONE";
            this.shapeBuilding = null;
            surfaceCircleBuilding = Rectangle.Empty;

            if (this.mainForm.isFormLocked == false)
                GorgonLibrary.Gorgon.Go();
        }

        public List<BodyElement> getTriangulatedShapes()
        {
 
            if(this.shapeBuilding != null)
            {
                //Creer une liste de body elements
                if (this.shapeBuilding.Count < 3) return null;

                List<BodyElement> elements = new List<BodyElement>();

                //Create a Cpolygon
                CPoint2D[] tabCpoint2D = getCPoint2D(this.shapeBuilding);

                CPolygonShape polygonShapes = new CPolygonShape(tabCpoint2D);
                //Generated
                bool res = (bool)polygonShapes.CutEar();
                if (res == false) return null;

                //Recuperer le nombre d'elem deja present
                int nbElemExistants = this.coronaObject.PhysicsBody.BodyElements.Count;
                for(int i = 0;i<polygonShapes.NumberOfPolygons;i++)
                {
                    //Recuperer les polygons
                    CPoint2D[] tabCps = polygonShapes.Polygons(i);

                    //Recuperer le tableau de point associé
                    List<Point> pointsConverted = this.convertCPoint2DToPoint(tabCps);

                    //Diviser les points en groupe de 8
                    List<List<Point>> listPolygones = new List<List<Point>>();
                    List<Point> currentList = new List<Point>();
                    for (int j = 0; j < pointsConverted.Count; j++)
                    {
                        if (currentList.Count == 8)
                        {
                            listPolygones.Add(currentList);
                            currentList = new List<Point>();
                            currentList.Add(pointsConverted[0]);
                            currentList.Add(pointsConverted[j - 1]);

                        }

                        currentList.Add(pointsConverted[j]);

                        if (j == pointsConverted.Count - 1)
                        {
                            listPolygones.Add(currentList);
                        }
                            
                    }


                    //Creer un body elem par polygone
                    for (int j = 0; j < listPolygones.Count; j++)
                    {
                        int indexElem = nbElemExistants;
                        nbElemExistants++;
                        string name = "AUTO_SHAPE";
                        BodyElement elem = new BodyElement(indexElem, name, 0, 0, 0, listPolygones[j]);
                        elements.Add(elem);
                    }
                    
                }

                return elements;
            }
           
            return null;
        }

        public List<BodyElement> getTriangulatedShapes(CPolygonShape polygonShapes)
        {
            if (polygonShapes != null)
            {
                //Creer une liste de body elements
                List<BodyElement> elements = new List<BodyElement>();

                bool res = (bool)polygonShapes.CutEar();
                if (res == false) return null;

                //Recuperer le nombre d'elem deja present
                int nbElemExistants = this.coronaObject.PhysicsBody.BodyElements.Count;
                for (int i = 0; i < polygonShapes.NumberOfPolygons; i++)
                {
                    //Recuperer les polygons
                    CPoint2D[] tabCps = polygonShapes.Polygons(i);

                    //Recuperer le tableau de point associé
                    List<Point> pointsConverted = this.convertCPoint2DToPoint(tabCps);

                    //Creer un body elem
                    int indexElem = nbElemExistants;
                    nbElemExistants++;
                    string name = "AUTO_SHAPE";
                    BodyElement elem = new BodyElement(indexElem, name, 0, 0, 0, pointsConverted);
                    elements.Add(elem);
                }

                return elements;
            }

            return null;
        }

        public List<Point> convertCPoint2DToPoint(CPoint2D[] tabPoints2D)
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < tabPoints2D.Length; i++)
            {
                int x = Convert.ToInt32(tabPoints2D[i].X);
                int y = Convert.ToInt32(tabPoints2D[i].Y);
                points.Add(new Point(x,y));
            }

            return points;
        }

        private CPoint2D[] getCPoint2D(List<Point> listPointsDep)
        {
            CPoint2D[] tabCpoint2D = new CPoint2D[listPointsDep.Count];
            for (int i = 0; i < listPointsDep.Count; i++)
            {
                CPoint2D cP = new CPoint2D(Convert.ToDouble(listPointsDep[i].X), Convert.ToDouble(listPointsDep[i].Y));
                tabCpoint2D[i] = cP;
            }

            return tabCpoint2D;
        }

        private CPoint2D[] getCPoint2D(PointF[] tabPointsF )
        {
            CPoint2D[] tabCpoint2D = new CPoint2D[tabPointsF.Length];
            for (int i = 0; i < tabPointsF.Length; i++)
            {
                CPoint2D cP = new CPoint2D(Convert.ToDouble(tabPointsF[i].X), Convert.ToDouble(tabPointsF[i].Y));
                tabCpoint2D[i] = cP;
            }

            return tabCpoint2D;
        }

        public List<BodyElement> getBodyLinesFromPoints()
        {
            if (this.shapeBuilding != null)
            {
                //APpliquer un algo de reduction de points
                List<Point> pointsReduced = this.DouglasPeuckerReduction(shapeBuilding,3);

                //Convertir la liste de points en CPoints2D 
                CPoint2D[] points2D = getCPoint2D(pointsReduced);
                
                //Creer un calculate reduction
                CalculateReduction cr = new CalculateReduction(points2D);
                CPoint2D[] pointsRet = cr.getPointReduce();
                if (pointsRet != null)
                {
                    List<Point> listPoints = convertCPoint2DToPoint(pointsRet);

                    //Creer les bodyelements
                    List<BodyElement> elements = new List<BodyElement>();


                    for (int i = 0; i < listPoints.Count-1; i++)
                    {
                        Point p1 = listPoints[i];
                        Point p2 = listPoints[i + 1];
                        List<Point> pointsLine = new List<Point>();
                        pointsLine.Add(p1);
                        pointsLine.Add(p2);
                        BodyElement elem = new BodyElement(elements.Count, "Line" + elements.Count, 0, 0, 0, pointsLine);
                        elements.Add(elem);

                        if (listPoints.Count % 2 == 1)
                        {
                            if (i == listPoints.Count - 2)
                            {
                                Point p3 = listPoints[i + 1];
                                List<Point> pointsLine2 = new List<Point>();
                                pointsLine2.Add(p2);
                                pointsLine2.Add(p3);

                                BodyElement elem2 = new BodyElement(elements.Count, "Line" + elements.Count, 0, 0, 0, pointsLine2);
                                elements.Add(elem2);
                            }
                        }
                    }
                    return elements;

                    
                    
                }

            }

            return null;
        }


        //-------------------------------------------------------------------------------------
        public List<Point> DouglasPeuckerReduction
    (List<Point> Points, Double Tolerance)
        {
            if (Points == null || Points.Count < 3)
                return Points;

            Int32 firstPoint = 0;
            Int32 lastPoint = Points.Count - 1;
            List<Int32> pointIndexsToKeep = new List<Int32>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            //The first and the last point cannot be the same
            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(Points, firstPoint, lastPoint,
            Tolerance, ref pointIndexsToKeep);

            List<Point> returnPoints = new List<Point>();
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                returnPoints.Add(Points[index]);
            }

            return returnPoints;
        }

        /// <summary>
        /// Douglases the peucker reduction.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstPoint">The first point.</param>
        /// <param name="lastPoint">The last point.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="pointIndexsToKeep">The point index to keep.</param>
        private static void DouglasPeuckerReduction(List<Point>
            points, Int32 firstPoint, Int32 lastPoint, Double tolerance,
            ref List<Int32> pointIndexsToKeep)
        {
            Double maxDistance = 0;
            Int32 indexFarthest = 0;

            for (Int32 index = firstPoint; index < lastPoint; index++)
            {
                Double distance = PerpendicularDistance
                    (points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint,
                indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest,
                lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        /// <summary>
        /// The distance of a point from a line made from point1 and point2.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static Double PerpendicularDistance
            (Point Point1, Point Point2, Point Point)
        {
            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = v((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            Double area = Math.Abs(.5 * (Point1.X * Point2.Y + Point2.X *
            Point.Y + Point.X * Point1.Y - Point2.X * Point1.Y - Point.X *
            Point2.Y - Point1.X * Point.Y));
            Double bottom = Math.Sqrt(Math.Pow(Point1.X - Point2.X, 2) +
            Math.Pow(Point1.Y - Point2.Y, 2));
            Double height = area / bottom * 2;

            return height;

            //Another option
            //Double A = Point.X - Point1.X;
            //Double B = Point.Y - Point1.Y;
            //Double C = Point2.X - Point1.X;
            //Double D = Point2.Y - Point1.Y;

            //Double dot = A * C + B * D;
            //Double len_sq = C * C + D * D;
            //Double param = dot / len_sq;

            //Double xx, yy;

            //if (param < 0)
            //{
            //    xx = Point1.X;
            //    yy = Point1.Y;
            //}
            //else if (param > 1)
            //{
            //    xx = Point2.X;
            //    yy = Point2.Y;
            //}
            //else
            //{
            //    xx = Point1.X + param * C;
            //    yy = Point1.Y + param * D;
            //}

            //Double d = DistanceBetweenOn2DPlane(Point, new Point(xx, yy));
        }

        //---------------------------------------------------
        //-------------------Events--------------------
        //---------------------------------------------------
        private void surfacePictBx_MouseClick(object sender, MouseEventArgs e)
        {
            Point offSetPoint = this.getScrollOffsetPoint();
            Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.currentScale)),
                     Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.currentScale)));


            //Verifier si le mode de construction
            if (this.DrawMode.Equals("SHAPE"))
            {
                this.shapeBuilding.Add(pTouched);
                GorgonLibrary.Gorgon.Go();
            }
        }

        private void surfacePictBx_MouseDown(object sender, MouseEventArgs e)
        {
            Point offSetPoint = this.getScrollOffsetPoint();
            Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.currentScale)),
                     Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.currentScale)));


            if (this.DrawMode.Equals("CIRCLE"))
            {
                Point centerPoint = new Point(Convert.ToInt32(-offSetPoint.X + this.coronaObject.DisplayObject.SurfaceRect.Width/2),
                         Convert.ToInt32(-offSetPoint.Y + (this.coronaObject.DisplayObject.SurfaceRect.Height / 2)));

                this.surfaceCircleBuilding.Location = centerPoint;
                this.lastPos = centerPoint;
                this.isMouseDown = true;
            }
            else if (this.DrawMode.Equals("HAND"))
            {
                this.lastPos = pTouched;

                this.shapeBuilding.Add(pTouched);

                this.isMouseDown = true;

                
            }
            GorgonLibrary.Gorgon.Go();
        }


        private void surfacePictBx_MouseMove(object sender, MouseEventArgs e)
        {
            Point offSetPoint = this.getScrollOffsetPoint();
            Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.currentScale)),
                     Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.currentScale)));

            if (this.DrawMode.Equals("CIRCLE"))
            {
                if (this.isMouseDown == true)
                {
                    
                    this.surfaceCircleBuilding.Width = Math.Abs(pTouched.X - (int)(lastPos.X * (1 / this.currentScale)));
                    this.surfaceCircleBuilding.Height = this.surfaceCircleBuilding.Width;


                    this.surfaceCircleBuilding.Location = new Point(lastPos.X - this.surfaceCircleBuilding.Width / 2,
                                                        lastPos.Y - this.surfaceCircleBuilding.Height / 2);


                    GorgonLibrary.Gorgon.Go();
                }
            }
            else if (this.DrawMode.Equals("HAND"))
            {
                if (this.isMouseDown == true)
                {

                    shapeBuilding.Add(pTouched);

                    if (this.mainForm.isFormLocked == false)
                        GorgonLibrary.Gorgon.Go();


                }
            }

            this.graduationBarX.reportMouseLocation(pTouched.X);
            this.graduationBarY.reportMouseLocation(pTouched.Y);
        }



        private void surfacePictBx_MouseUp(object sender, MouseEventArgs e)
        {
            this.isMouseDown = false;

            if (this.DrawMode.Equals("CIRCLE"))
            {
                //Creer directement l'element Circle 
                int index = this.coronaObject.PhysicsBody.BodyElements.Count;
                string name = "Circle";

                BodyElement elem = new BodyElement(index, name, 0, 0, 0, this.surfaceCircleBuilding.Location, this.surfaceCircleBuilding.Width / 2);
                this.coronaObject.PhysicsBody.BodyElements.Add(elem);

                this.surfaceCircleBuilding = Rectangle.Empty ;

                this.shapeBuilding = null;
                this.mainForm.physicsBodySettings1.reloadPanel();
            }
           else if (this.DrawMode.Equals("HAND"))
            {
                Point offSetPoint = this.getScrollOffsetPoint();
                Point pTouched = new Point(Convert.ToInt32(-offSetPoint.X + e.Location.X * (1 / this.currentScale)),
                     Convert.ToInt32(-offSetPoint.Y + e.Location.Y * (1 / this.currentScale)));

                shapeBuilding.Add(pTouched);

                if (this.mainForm.isFormLocked == false)
                    GorgonLibrary.Gorgon.Go();
            }
        }



        public void DrawGorgon()
        {
            GorgonLibrary.Gorgon.CurrentRenderTarget.Clear(Color.Black);
            GorgonLibrary.Gorgon.CurrentRenderTarget.BeginDrawing();
            //Dessiner les shapes des element existants
            if (this.coronaObject != null)
            {
               
                Point offsetPoint = this.getScrollOffsetPoint();
                Point offsetFinal = new Point(offsetPoint.X - this.coronaObject.DisplayObject.SurfaceRect.X, offsetPoint.Y - this.coronaObject.DisplayObject.SurfaceRect.Y);

                this.coronaObject.DisplayObject.DrawGorgon(offsetFinal
                                                                , false, this.currentScale, this.currentScale,false);
            
                Color color = Color.FromArgb(100, Color.YellowGreen);
                this.coronaObject.PhysicsBody.drawGorgonBodyElements(color, false, offsetPoint, this.currentScale,false);

                //Dessiner d'une autre couleur les elements selectionnés
                List<BodyElement> selectedBodies = this.mainForm.physicsBodySettings1.GetSelectedBodyElements();
                for (int i = 0; i < selectedBodies.Count; i++)
                {
                    BodyElement elem = selectedBodies[i];
                    if (elem.Type.Equals("SHAPE"))
                    {

                        //Creer un tableau de points ajustés
                        List<Point> tabPointsAjust = new List<Point>();

                        Point middlePoint = new Point(this.coronaObject.DisplayObject.SurfaceRect.Width / 2,
                                         this.coronaObject.DisplayObject.SurfaceRect.Height / 2);

                        for (int j = 0; j < elem.BodyShape.Count; j++)
                        {

                            Point pAjust = new Point((int)((float)(elem.BodyShape[j].X + offsetPoint.X)),
                                                  (int)((float)(elem.BodyShape[j].Y + offsetPoint.Y)));


                            tabPointsAjust.Insert(j, pAjust);

                            //Close the figure
                            if (j == elem.BodyShape.Count - 1)
                            {
                                tabPointsAjust.Add(tabPointsAjust[0]);
                            }

                        }

                        GorgonGraphicsHelper.Instance.DrawLines(tabPointsAjust, Color.FromArgb(150, Color.Blue), 1.5f, this.currentScale);


                    }
                    else if (elem.Type.Equals("CIRCLE"))
                    {
                        int radius = elem.Radius;


                            GorgonGraphicsHelper.Instance.FillCircle(elem.SurfaceCircle.X + offsetPoint.X,
                                             elem.SurfaceCircle.Y + offsetPoint.Y,
                                             elem.Radius, Color.FromArgb(150, Color.Blue), this.currentScale, false);
                        

                    }
                }

                //Dessiner le build
                if (this.shapeBuilding != null)
                {
                    if (this.shapeBuilding.Count > 1)
                    {
                        List<Point> tabPoint = new List<Point>();
                        for (int i = 0; i < this.shapeBuilding.Count; i++)
                        {
                            tabPoint.Add(new Point(this.shapeBuilding[i].X + offsetPoint.X, this.shapeBuilding[i].Y + offsetPoint.Y));
                        }

                      
                        GorgonGraphicsHelper.Instance.DrawLines(tabPoint, color, 1, this.currentScale);
                    }


                    if (this.DrawMode == "SHAPE")
                    {
                        Color blue = Color.FromArgb(100, Color.Blue);
                        for (int i = 0; i < this.shapeBuilding.Count; i++)
                        {
                         
                            GorgonGraphicsHelper.Instance.FillCircle(this.shapeBuilding[i].X + offsetPoint.X - 3, this.shapeBuilding[i].Y + offsetPoint.Y - 3,
                                3, blue, this.currentScale, false);
                        }
                    }


                }
                else if (this.surfaceCircleBuilding.IsEmpty == false)
                {
                  
                    GorgonGraphicsHelper.Instance.FillCircle(this.surfaceCircleBuilding.Location.X + offsetPoint.X, this.surfaceCircleBuilding.Location.Y + offsetPoint.Y,
                              this.surfaceCircleBuilding.Size.Width / 2, color, this.currentScale, false);
                }

            }

            GorgonLibrary.Gorgon.CurrentRenderTarget.EndDrawing();
        }

       private void surfacePictBx_Paint(object sender, PaintEventArgs e)
       {
           if (GorgonLibrary.Gorgon.IsInitialized == true)
               GorgonLibrary.Gorgon.Go();

           ////Dessiner les shapes des element existants
           //if (this.coronaObject != null)
           //{
           //     Matrix matrix = new Matrix();
           //     matrix.Scale(this.currentScale,this.currentScale);

           //     Point offsetPoint = this.getScrollOffsetPoint();
           //     Point offsetFinal = new Point(offsetPoint.X - this.coronaObject.DisplayObject.SurfaceRect.X, offsetPoint.Y - this.coronaObject.DisplayObject.SurfaceRect.Y);

           //     this.coronaObject.DisplayObject.dessineAt(e.Graphics, offsetFinal,
           //                                                 false,matrix, this.currentScale, this.currentScale);

           //     e.Graphics.ResetTransform();

           //     Matrix m = new Matrix();
           //     m.Scale(this.currentScale, this.currentScale);
           //     e.Graphics.Transform = m;

           //     SolidBrush brush = new SolidBrush(Color.FromArgb(150, Color.YellowGreen));
           //     this.coronaObject.PhysicsBody.dessineAllBodyELements(e.Graphics, brush, false, offsetPoint);


           //    //Dessiner le build
           //    if (this.shapeBuilding != null)
           //    {
           //        if (this.shapeBuilding.Count > 1)
           //        {
           //            Point[] tabPoint = new Point[this.shapeBuilding.Count];
           //            for (int i = 0; i < this.shapeBuilding.Count; i++)
           //            {
           //                tabPoint[i] = new Point(this.shapeBuilding[i].X + offsetPoint.X, this.shapeBuilding[i].Y + offsetPoint.Y);
           //            }

           //            e.Graphics.DrawLines(Pens.GreenYellow, tabPoint);
           //        }
                  

           //        if (this.DrawMode == "SHAPE")
           //        {
           //            for (int i = 0; i < this.shapeBuilding.Count; i++)
           //            {
           //                //Draw points
           //                SolidBrush br = new SolidBrush(Color.FromArgb(150, Color.Blue));
           //                e.Graphics.FillEllipse(br, new RectangleF(new PointF(this.shapeBuilding[i].X + offsetPoint.X - (3 * (1 / this.currentScale)), this.shapeBuilding[i].Y + offsetPoint.Y - (3 * (1 / this.currentScale))),
           //                                                             new SizeF((6 * (1 / this.currentScale)), (6 * (1 / this.currentScale)))));

           //            }
           //        }
                   

           //    }
           //    else if (this.surfaceCircleBuilding.IsEmpty == false)
           //    {
           //        Rectangle rect = new Rectangle(new Point(this.surfaceCircleBuilding.Location.X + offsetPoint.X,
           //            this.surfaceCircleBuilding.Location.Y +offsetPoint.Y),
           //            this.surfaceCircleBuilding.Size);

           //        e.Graphics.DrawEllipse(Pens.GreenYellow, rect);
           //    }

           //}
       }


       public Point getOffsetPoint()
       {
           Point p = new Point(-this.hScrollBar1.Value, -this.vScrollBar1.Value);
           return p;
       }

       private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
       {
           if (this.mainForm.isFormLocked == false)
             GorgonLibrary.Gorgon.Go();

           Point offsetInversed = getOffsetPoint();
           Point offSetFinal = new Point(-offsetInversed.X, -offsetInversed.Y);
           this.graduationBarX.reportOffSetScrolling(offSetFinal);
           this.graduationBarY.reportOffSetScrolling(offSetFinal);
       }



       public void zoomAvant()
       {
           this.currentScale = this.currentScale * 2;
           if (this.currentScale > 4) this.currentScale = 4;

           this.setScrollBarsValues();
           if (this.mainForm.isFormLocked == false)
               GorgonLibrary.Gorgon.Go();

           this.graduationBarX.setScale(this.currentScale);
           this.graduationBarY.setScale(this.currentScale);
       }

       public void zoomArriere()
       {
           this.currentScale = this.currentScale / 2;
           if (this.currentScale < 0.25f) this.currentScale = 0.25f;

           this.setScrollBarsValues();
           if (this.mainForm.isFormLocked == false)
               GorgonLibrary.Gorgon.Go();
           this.graduationBarX.setScale(this.currentScale);
           this.graduationBarY.setScale(this.currentScale);
       }











    }
}
