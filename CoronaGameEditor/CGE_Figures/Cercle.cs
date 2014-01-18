using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using Krea.CoronaClasses;
using Krea.Corona_Classes;

namespace Krea.CGE_Figures
{
    [Serializable]
    public class Cercle : Figure
    {

        private int m_nRayon;

        public Cercle() {
        }

        public Cercle(Point position, int rayon, Color fillColor, Color strokeColor, int epaisseur, bool rempli,DisplayObject objParent)
            : base(position, fillColor, strokeColor, rempli, epaisseur,objParent)
        {
            this.typeFigure = "CIRCLE";
            this.m_nRayon = rayon;

        }

        public int Rayon
        {
            get { return m_nRayon; }
            set { m_nRayon = value; }
        }

        public override void Dessine(Graphics g,int alpha,Point offsetPoint)
        {
            //Définir les couleurs en ARGB
            Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), m_nEpaisseur);
            Brush brosse = new SolidBrush(Color.FromArgb(alpha,this.m_FillColor.R,this.m_FillColor.G,this.m_FillColor.B));
            Point pDest = new Point(offsetPoint.X + this.Position.X, offsetPoint.Y + this.Position.Y);
            if (m_bRempli == true)
            {
                g.FillEllipse(brosse, pDest.X , pDest.Y , Rayon * 2, Rayon * 2);
            }

            g.DrawEllipse(p, pDest.X , pDest.Y , Rayon * 2, Rayon * 2);

            
        }

        public override void DrawGorgon(Point pointDest, int alpha, float worldScale)
        {
            //Définir les couleurs en ARGB
            Color colorBorder = Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B);
            Color colorFill = Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B);

            if (m_bRempli == true)
            {
                GorgonGraphicsHelper.Instance.FillCircle(this.m_ptPosition.X + pointDest.X, this.m_ptPosition.Y + pointDest.Y, this.Rayon, colorFill, worldScale, false);
            }

            GorgonGraphicsHelper.Instance.DrawCircle(this.m_ptPosition.X + pointDest.X, this.m_ptPosition.Y + pointDest.Y, this.Rayon, colorBorder, m_nEpaisseur, worldScale, false);
            
        }

        public override Bitmap DrawInBitmap(int alpha, float worldScale)
        {
            Bitmap bitmap = new Bitmap(Rayon * 2, Rayon * 2);
            Graphics g = Graphics.FromImage(bitmap);

            //Définir les couleurs en ARGB
            Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), m_nEpaisseur);
           

            if (m_bRempli == true)
            {
                Brush brosse = new SolidBrush(Color.FromArgb((int)(255.0f * this.DisplayObjectParent.Alpha), this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));
                g.FillEllipse(brosse, 0, 0, Rayon * 2, Rayon * 2);
                brosse.Dispose();
            }

            g.DrawEllipse(p, 0, 0, Rayon * 2, Rayon * 2);

            p.Dispose();
            g.Dispose();

            return bitmap;
        }

        public override void DessineAt(Graphics g, Point pointDest, int alpha)
        {
            //Définir les couleurs en ARGB
            Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), m_nEpaisseur);
            Brush brosse = new SolidBrush(Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));

            if (m_bRempli == true)
            {
                g.FillEllipse(brosse, pointDest.X , pointDest.Y , Rayon * 2, Rayon * 2);
            }

            g.DrawEllipse(p, pointDest.X , pointDest.Y , Rayon * 2, Rayon * 2);
        }

        public override bool Clic(Point p, Matrix matrix)
        {
            GraphicsPath g = new GraphicsPath();

            g.AddEllipse(new Rectangle(this.Position.X , this.Position.Y , Rayon * 2, Rayon * 2));

            return g.IsVisible(p);
        }

        public override void SetSizeFromPoint(Point pt)
        {
            this.m_nRayon = this.m_nRayon - (this.lastPos.X - pt.X);
            if (this.m_nRayon < 1) this.m_nRayon = 1;
            this.lastPos.X = pt.X;

            if (this.DisplayObjectParent != null)
            {
                this.DisplayObjectParent.SurfaceRect = new Rectangle(this.DisplayObjectParent.SurfaceRect.Location, new Size(this.m_nRayon * 2, this.m_nRayon * 2));
            }
        }

        public override Rectangle getBounds(Matrix matrix)
        {
            GraphicsPath g = new GraphicsPath();
            g.AddEllipse(new Rectangle(this.Position.X , this.Position.Y, Rayon * 2, Rayon * 2));

            RectangleF rectF = g.GetBounds(matrix);
            return new Rectangle(new Point((int)rectF.Location.X,(int)rectF.Location.Y),new Size((int)rectF.Width,(int)rectF.Height));
        }

        public override Figure CloneInstance(bool keepLocation)
        {
            Point pos;
            if (keepLocation == true)
            {
                pos = new Point(this.m_ptPosition.X, this.m_ptPosition.Y);
            }
            else
            {
                pos = new Point(this.m_ptPosition.X  +20, this.m_ptPosition.Y);
            }

             Color fillColor = Color.Empty;
            try
            {
                fillColor = this.FillColor;
            }
            catch
            {
                fillColor = Color.Empty;
            }
           

            Cercle fig = new Cercle(pos, this.m_nRayon, fillColor, this.m_colTrait, this.m_nEpaisseur, this.m_bRempli,null);

            return fig;
        }
    }

}
