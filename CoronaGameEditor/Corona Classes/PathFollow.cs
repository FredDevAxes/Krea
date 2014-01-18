using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Krea.CoronaClasses;
using System.Drawing.Drawing2D;

namespace Krea.Corona_Classes
{
    [Serializable()]
    public class PathFollow
    {
        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        public List<Point> Path;
        public bool isCyclic = false;
        public int speed = 500;
        public bool isCurve = false;
        public bool isEnabled = false;
        public bool Rotate = false;
        public int Iteration = 1;
        private CoronaObject objectParent;
        public bool removeOnComplete = false;
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public PathFollow(CoronaObject objectParent)
        {
            this.Path = new List<Point>();
            this.objectParent = objectParent;
            
        }
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        public void drawGorgon(Point offsetPoint, float worldScale)
        {
            if (this.isEnabled == true)
            {
                List<Point> tabPoints = new List<Point>();

                for (int i = 0; i < this.Path.Count; i++)
                {
                    int x = this.Path[i].X + offsetPoint.X;
                    int y = this.Path[i].Y + offsetPoint.Y;
                    tabPoints.Insert(i,new Point(x, y));
                }

                Color penColor = Color.FromArgb(150, Color.Blue);
                Color brushColor = Color.FromArgb(155, Color.Green);

                if (tabPoints.Count > 1)
                {
                    if (this.isCurve == true)
                    {
                        GorgonGraphicsHelper.Instance.DrawCurve(tabPoints, penColor, 1, worldScale);
                       
                    }
                    else
                    {
                        GorgonGraphicsHelper.Instance.DrawLines(tabPoints, penColor, 1, worldScale);
                    }
                }

                for (int i = 0; i < tabPoints.Count; i++)
                {
                    GorgonGraphicsHelper.Instance.FillCircle(tabPoints[i].X - 3, tabPoints[i].Y - 3, 3, brushColor, worldScale, false);
                 
                }
            }
        }
        public void dessine(Graphics g,Point offsetPoint)
        {
            if (this.isEnabled == true)
            {
                Point[] tabPoints = new Point[this.Path.Count];
                
                for (int i = 0; i < this.Path.Count; i++)
                {
                    int x = this.Path[i].X + offsetPoint.X;
                    int y = this.Path[i].Y + offsetPoint.Y;
                    tabPoints[i] = new Point(x, y);
                }

                float[] dashValues = { 1, 1 };
                Pen pen = new Pen(Color.FromArgb(150, Color.Blue), 2);
                SolidBrush br = new SolidBrush(Color.FromArgb(155, Color.Green));
                pen.DashPattern = dashValues;

                if (tabPoints.Length > 1)
                {
                    if (this.isCurve == true)
                    {
                        g.DrawCurve(pen, tabPoints);
                    }
                    else
                    {
                        g.DrawLines(pen, tabPoints);
                    }
                }

                for (int i = 0; i < tabPoints.Length; i++)
                {
                    
                    g.FillEllipse(br, new Rectangle(tabPoints[i].X - 3, tabPoints[i].Y - 3, 6, 6));
                    
                }

                pen.Dispose();
                br.Dispose();
            }
           
            
        }

        public PathFollow cloneInstance(CoronaObject parent)
        {
            PathFollow path = new PathFollow(parent);
            path.Path.AddRange(this.Path);

            path.isCurve = this.isCurve;
            path.isCyclic = this.isCyclic;
            path.isEnabled = this.isEnabled;
            path.removeOnComplete = this.removeOnComplete;
            return path;
        }

        public string getPointsTableLua(float XRatio,float YRatio)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");

            //Always add the first point
            if (this.isCurve == true)
            {
                GraphicsPath path = new GraphicsPath(FillMode.Winding);
                path.AddCurve(this.Path.ToArray());
                path.Flatten();
                PointF[] finalPoints = path.PathPoints;

                for (int i = 0; i < finalPoints.Length; i++)
                {
                    if (i % 10 == 0) sb.Append("\n");

                    sb.Append("{ x = " + ((float)finalPoints[i].X * XRatio).ToString().Replace(",", ".") + ", y = " + ((float)finalPoints[i].Y * YRatio).ToString().Replace(",", ".") + "}");

                    if (i < finalPoints.Length - 1) sb.Append(",");

                }
            }
            else
            {
                for (int i = 0; i < this.Path.Count; i++)
                {
                    if (i % 10 == 0) sb.Append("\n");

                    sb.Append("{ x = " + ((float)this.Path[i].X * XRatio).ToString().Replace(",", ".") + ", y = " + ((float)this.Path[i].Y * YRatio).ToString().Replace(",", ".") + "}");

                    if (i < this.Path.Count - 1) sb.Append(",");

                }

            }

            sb.AppendLine("}\n");
            return sb.ToString();
        }
    }
}
