using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Krea.CoronaClasses;
using Krea.Corona_Classes;

namespace Krea.CGE_Figures
{
     [Serializable]
    public class CourbeBezier:Figure
    {

        public List<Point> UserPoints;
        public int PointsOnCurve;
        private Point[] bezierPoints;
        public List<Point> CuttedPoints;
        public Double Ratio = 1;
        public CourbeBezier(Point position,List<Point> tabPoints, Color fillColor, Color strokeColor, int epaisseur, bool rempli,DisplayObject objParent)
              : base(position, fillColor, strokeColor, rempli, epaisseur,objParent)
           { 
           
               this.UserPoints = tabPoints;
               this.typeFigure = "CURVE";
               this.PointsOnCurve = 100;
           }


        //méthodes abstraites qui doivent être surchargées
        public override void Dessine(Graphics g, int alpha, Point offsetPoint)
        {
            Point[] tabPoint = UserPoints.ToArray();
            for (int i = 0; i < tabPoint.Length; i++)
            {
                tabPoint[i].X = tabPoint[i].X + offsetPoint.X;
                tabPoint[i].Y = tabPoint[i].Y + offsetPoint.Y;

            }
            
            if (UserPoints.Count > 1)
            {
                //Définir les couleurs en ARGB
                Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), this.StrokeSize);
                g.DrawCurve(p, tabPoint);
                p.Dispose();
       
            }

        }

        public override void DrawGorgon(Point pointDest, int alpha,float worldScale)
        {
            if (UserPoints.Count > 1)
            {
                //Définir les couleurs en ARGB

                List<Point> tabPoint = new List<Point>();
                for (int i = 0; i < this.UserPoints.Count; i++)
                {
                    tabPoint.Insert(i, new Point(this.UserPoints[i].X + pointDest.X, this.UserPoints[i].Y + pointDest.Y));

                }

                if (tabPoint.Count > 1)
                {
                    Color color = Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B);
                    GorgonGraphicsHelper.Instance.DrawCurve(tabPoint, color, this.StrokeSize, worldScale);

                }

            }

        }
        public override Bitmap DrawInBitmap(int alpha, float worldScale)
        {

            return null;
        }

        public override void DessineAt(Graphics g, Point pDest, int alpha)
        {

            
            if (UserPoints.Count > 1)
            {
                //Définir les couleurs en ARGB
                Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), this.StrokeSize);

                g.DrawCurve(p, this.UserPoints.ToArray());
                p.Dispose();
            }

        }
        public override bool Clic(Point p, Matrix matrix) 
        {
            Rectangle rect = this.getBounds(matrix);
            return rect.Contains(p);
        }

        public override Rectangle getBounds(Matrix matrix)
        {
            GraphicsPath g = new GraphicsPath();
            g.AddCurve(this.UserPoints.ToArray());
            RectangleF rectF = g.GetBounds(matrix);
            return new Rectangle(new Point((int)rectF.Location.X, (int)rectF.Location.Y), new Size((int)rectF.Width, (int)rectF.Height));
        }

        public override void SetSizeFromPoint(Point pt) { }
        public override Figure CloneInstance(bool keepLocation) 
        {
            List<Point> points = new List<Point>();
            for(int i = 0;i<this.UserPoints.Count;i++)
            {
                Point p = new Point(this.UserPoints[i].X,this.UserPoints[i].Y);
                points.Add(p);
            }
            CourbeBezier fig = new CourbeBezier(this.UserPoints[0],points,this.m_colTrait,this.StrokeColor,this.m_nEpaisseur,this.m_bRempli, null);

            return fig;
        }

        private List<Point> DouglasPeuckerReduction
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
        private static Double PerpendicularDistance
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
    }
}
