using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
using Krea.CoronaClasses;
using Krea.Corona_Classes;

namespace Krea.CGE_Figures
{
    [Serializable]
    
    public class Line : Figure
    {
        public List<Point> Points;
      

        public Line() {}

        public Line(Point position, Color couleur, int epaisseur, bool rempli, DisplayObject objParent)
            : base(position, Color.Empty, couleur, rempli, epaisseur,objParent)
        {
            this.typeFigure = "LINE";
            this.Points = new List<Point>();
            this.Points.Add(position);
        }

        public Line(Point position, Byte coul_A, Byte coul_R, Byte coul_G, Byte coul_B, int epaisseur, bool rempli, DisplayObject objParent)
           : base(position, coul_A, coul_R,coul_G, coul_B, rempli, epaisseur,objParent)
       {
           this.typeFigure = "LINE";
           this.Points = new List<Point>();
           this.Points.Add(position);
       }

        public override void DrawGorgon(Point pointDest, int alpha,float worldScale)
        {
            if (this.Points.Count > 1)
            {
                //Définir les couleurs en ARGB

                List<Point> tabPoint = new List<Point>();
                for (int i = 0; i < this.Points.Count; i++)
                {
                    tabPoint.Insert(i, new Point(this.Points[i].X + pointDest.X, this.Points[i].Y + pointDest.Y));

                }

                if (tabPoint.Count > 1)
                {
                    Color color = Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B);
                    GorgonGraphicsHelper.Instance.DrawLines(tabPoint, color, this.StrokeSize, worldScale);

                }

            }
        }
        public override Bitmap DrawInBitmap(int alpha, float worldScale)
        {
            return null;
        }
       public override void Dessine(Graphics g,int alpha,Point offsetPoint)
       {
           //Définir les couleurs en ARGB
           Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), this.StrokeSize);

           //Convertir tous les points et leur ajouter un offset
           Point[] tabPoints = new Point[this.Points.Count];
           for(int i = 0;i<this.Points.Count;i++)
           {
               tabPoints[i] = new Point(this.Points[i].X + offsetPoint.X,this.Points[i].Y + offsetPoint.Y);
           }

           if (this.Points.Count > 2)
           {
               g.DrawLines(p, tabPoints);

           }
           else if (this.Points.Count == 2)
           {
               g.DrawLine(p, tabPoints[0], tabPoints[1]);
           }
           
         
           p.Dispose();

       }

       public override void DessineAt(Graphics g, Point pointDest,int alpha)
       {

       }

       public override bool Clic(Point p,Matrix matrix)
       {
            Rectangle rect = this.getBounds(matrix);
            return rect.Contains(p);
       }

       public override Rectangle getBounds(Matrix matrix)
       {
           GraphicsPath g = new GraphicsPath();
           g.AddLines(this.Points.ToArray());

           RectangleF rectF = g.GetBounds(matrix);
           return new Rectangle(new Point((int)rectF.Location.X, (int)rectF.Location.Y), new Size((int)rectF.Width, (int)rectF.Height));
       }

       public override void SetSizeFromPoint(Point pt)
       {
           
       }

       public override Figure CloneInstance(bool keepLocation)
       {

           List<Point> points = new List<Point>();
           for (int i = 0; i < this.Points.Count; i++)
           {
               Point p = new Point(this.Points[i].X, this.Points[i].Y);
               points.Add(p);
           }

           Line fig = new Line(this.Points[0], this.m_colTrait, this.m_nEpaisseur, this.m_bRempli, null);
           fig.Points = points;

           return fig;
       }
    }
}
