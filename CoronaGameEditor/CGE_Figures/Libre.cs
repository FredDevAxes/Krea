using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
using Krea.CoronaClasses;



namespace Krea.CGE_Figures
{
    [Serializable]
    public class Libre : Figure
    {
        List<Point> tabPoints;
        

        public Libre() { }

        public Libre(Point position, List<Point> tabPoints, Color couleur, int epaisseur, bool rempli,DisplayObject objParent)
            : base(position, Color.Empty,couleur,rempli, epaisseur,objParent)
        {
        
            this.tabPoints = tabPoints;
        }

        public Libre(Point position, List<Point> tabPoints, Byte coul_A, Byte coul_R, Byte coul_G, Byte coul_B, int epaisseur, bool rempli,DisplayObject objParent)
            : base(position, coul_A, coul_R, coul_G, coul_B, rempli, epaisseur,objParent)
        {

            this.tabPoints = tabPoints;
            
        }


        public List<Point> TabPoints
        {
            get { return tabPoints; }
            set { tabPoints = value; }
        }

        public override Rectangle getBounds(Matrix matrix)
        {
            GraphicsPath g = new GraphicsPath();
            g.AddLines(tabPoints.ToArray());

            RectangleF rectF = g.GetBounds(matrix);
            return new Rectangle(new Point((int)rectF.Location.X, (int)rectF.Location.Y), new Size((int)rectF.Width, (int)rectF.Height));
        }

        public override void DrawGorgon(Point pointDest, int alpha, float worldScale)
        {

        }
        public override Bitmap DrawInBitmap(int alpha, float worldScale)
        {
            return null;
        }
        public override void Dessine(Graphics g, int alpha, Point offsetPoint)
        {
            //Définir les couleurs en ARGB
            m_colTrait = Color.FromArgb(alpha, R, G, B);

            Point[] tabPAjust = new Point[this.tabPoints.Count]; 
            for (int i = 0; i < this.tabPoints.Count; i++)
            {
                tabPAjust[i] = new Point(offsetPoint.X + this.tabPoints[i].X, offsetPoint.Y + this.tabPoints[i].Y);
            }
            Pen p = new Pen(m_colTrait, StrokeSize);
            if(this.tabPoints.Count>2)
                g.DrawLines(p, tabPAjust);

        }

        public override void DessineAt(Graphics g, Point pointDest,int alpha)
        {
          /*  //Définir les couleurs en ARGB
            m_colTrait = Color.FromArgb(A, R, G, B);

            Pen p = new Pen(m_colTrait, Epaisseur);
            if (this.tabPoints.Count > 2)
                g.DrawLines(p, tabPoints.ToArray());*/
        }

        public override bool Clic(Point p, Matrix matrix)
        {
            Rectangle rect = this.getBounds(matrix);
            return rect.Contains(p);
        }

        public override void SetSizeFromPoint(Point pt)
        {
            
        }

        public override Figure CloneInstance(bool keepLocation)
        {
            return null;
        }
    }
}


