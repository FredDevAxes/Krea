using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.ComponentModel;
using Krea.CoronaClasses;
using Krea.Corona_Classes;


namespace Krea.CGE_Figures
{
    [Serializable]
    public class Rect : Figure
    {
        
        private int largeur;
        private int hauteur;
        public bool isRounded = false;
        public int cornerRadius = 2;
        public Rect() {
 
        }

        public Rect(Point position, int largeur, int hauteur,bool isRounded, Color fillColor, Color strokeColor, int epaisseur, bool rempli, DisplayObject objParent)
            : base(position, fillColor,strokeColor, rempli, epaisseur,objParent)
        {

            this.typeFigure = "RECTANGLE";
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.isRounded = isRounded;
        }

        [DescriptionAttribute("The height of the shape.")]
        public int Height
        {
            get { return hauteur; }
            set { hauteur = value; }
        }

        [DescriptionAttribute("The width of the shape.")]
        public int Width
        {
            get { return largeur; }
            set { largeur = value; }
        }

        public override Bitmap DrawInBitmap(int alpha, float worldScale)
        {
            int width = (int)(this.largeur*worldScale);
            int height = (int)(this.hauteur * worldScale);

            if (width <=0) width = 10;
            if (height <= 0) height = 10;

            if (width > 4096) width = 4096;
            if (height > 4096) height = 4096;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            if (this.isRounded == false)
            {
                drawRect(g, alpha, Point.Empty,worldScale);
            }
            else
            {
                drawRoundedRect(g, alpha, Point.Empty,worldScale);
            }
            

            g.Dispose();

            return bitmap;
        }

        public override void DrawGorgon(Point pointDest, int alpha, float worldScale)
        {
            if (this.isRounded == false)
            {
                if (this.DisplayObjectParent != null && this.DisplayObjectParent.GradientColor.isEnabled == true)
                {
                    //LinearGradientBrush br = this.DisplayObjectParent.GradientColor.getBrushForDrawing(new Rectangle(pointDest, new Size(this.largeur, this.hauteur)), Convert.ToInt32(this.DisplayObjectParent.Alpha * 255));
                    //g.FillRectangle(br, pointDest.X, pointDest.Y, largeur, hauteur);

                    if (this.DisplayObjectParent.GorgonSprite != null)
                    {
                        this.DisplayObjectParent.GorgonSprite.Draw();
                    }
                }
                else if (m_bRempli == true)
                {
                  
                    GorgonGraphicsHelper.Instance.FillRectangle(new Rectangle(pointDest.X, pointDest.Y, largeur, hauteur), 1,
                        Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B), worldScale, false);
                }

                if (this.StrokeSize > 0)
                {

                    GorgonGraphicsHelper.Instance.DrawRectangle(new Rectangle(this.m_ptPosition.X + pointDest.X, this.m_ptPosition.Y + pointDest.Y, largeur, hauteur), m_nEpaisseur,
                        Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), worldScale);
                }
            }
            else
            {
                if (this.DisplayObjectParent != null)
                {
                    if (this.DisplayObjectParent.GorgonSprite != null)
                    {
                        this.DisplayObjectParent.GorgonSprite.Draw();
                    }
                }
            }

        }

        public override void Dessine(Graphics g,int alpha,Point offsetPoint)
        {
           if(this.isRounded == false)
               drawRect(g, alpha, new Point(offsetPoint.X + this.Position.X,offsetPoint.Y + this.Position.Y),1);
           else
               drawRoundedRect(g, alpha, new Point(offsetPoint.X + this.Position.X, offsetPoint.Y + this.Position.Y), 1);
        }


        private void drawRect(Graphics g, int alpha, Point pointDest,float worldScale)
        {
            Point finalPoint = new Point((int)(pointDest.X * worldScale), (int)(pointDest.Y * worldScale));
            Size finalSize = new Size((int)(this.largeur * worldScale), (int)(this.hauteur * worldScale));
            if (this.DisplayObjectParent != null && this.DisplayObjectParent.GradientColor.isEnabled == true)
            {
                LinearGradientBrush br = this.DisplayObjectParent.GradientColor.getBrushForDrawing(
                    new Rectangle(finalPoint, finalSize),Convert.ToInt32(this.DisplayObjectParent.Alpha * 255));

                g.FillRectangle(br, finalPoint.X, finalPoint.Y, finalSize.Width, finalSize.Height);
                br.Dispose();
            }
            else if (m_bRempli == true)
            {
                Brush brosse = new SolidBrush(Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));
                g.FillRectangle(brosse, finalPoint.X, finalPoint.Y, finalSize.Width, finalSize.Height);
                brosse.Dispose();
            }

            if (this.StrokeSize > 0)
            {
                Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), m_nEpaisseur);
                g.DrawRectangle(p, finalPoint.X, finalPoint.Y, finalSize.Width, finalSize.Height);
                p.Dispose();
            }
            
        }

        private void drawRoundedRect(Graphics g, int alpha, Point pDest,float worldScale)
        {
            try
            {
                int radius = this.cornerRadius;
                GraphicsPath gp = new GraphicsPath();

                Point finalPoint = new Point((int)(pDest.X * worldScale), (int)(pDest.Y * worldScale));
                Size finalSize = new Size((int)(this.largeur * worldScale), (int)(this.hauteur * worldScale));

                int x = finalPoint.X;
                int y = finalPoint.Y;
                int width = finalSize.Width;
                int height = finalSize.Height;

                gp.AddLine(x + radius, y, x + width - (radius * 2), y); // Line
                gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90); // Corner
                gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2)); // Line
                gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90); // Corner
                gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height); // Line
                gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90); // Corner
                gp.AddLine(x, y + height - (radius * 2), x, y + radius); // Line
                gp.AddArc(x, y, radius * 2, radius * 2, 180, 90); // Corner
                gp.CloseFigure();

                if (this.DisplayObjectParent.GradientColor.isEnabled == true)
                {
                    LinearGradientBrush br = this.DisplayObjectParent.GradientColor.getBrushForDrawing(new Rectangle(finalPoint, finalSize), Convert.ToInt32(this.DisplayObjectParent.Alpha * 255));
                    g.FillPath(br, gp);

                    br.Dispose();
                }
                else if (m_bRempli == true)
                {
                    Brush brosse = new SolidBrush(Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));
                    g.FillPath(brosse, gp);
                    brosse.Dispose();
                }

                Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), m_nEpaisseur);
                g.DrawPath(p, gp);
                gp.Dispose();

                p.Dispose();
            }
            catch (Exception ex)
            {
            }
        

     }
        public override void DessineAt(Graphics g, Point pointDest,int alpha)
        {
            try
            {
                if (this.isRounded == false)
                    drawRect(g, alpha, pointDest,1);
                else
                    drawRoundedRect(g, alpha, pointDest,1);
            }
            catch (Exception ex)
            {

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
            g.AddRectangle(new Rectangle(this.Position.X, this.Position.Y, largeur, hauteur));

            RectangleF rectF = g.GetBounds(matrix);
            return new Rectangle(new Point((int)rectF.Location.X, (int)rectF.Location.Y), new Size((int)rectF.Width, (int)rectF.Height));
        }

        public override void SetSizeFromPoint(Point pt)
        {
            int width = this.largeur - (this.lastPos.X - pt.X);
            int height = this.hauteur - (this.lastPos.Y - pt.Y);

            if (width < 2) width = 2;
            if (height < 2) height = 2;
            this.lastPos.X = pt.X;
            this.lastPos.Y = pt.Y;
            this.largeur = width;
            this.hauteur = height;

            if (this.DisplayObjectParent != null)
            {
                this.DisplayObjectParent.SurfaceRect = new Rectangle(this.DisplayObjectParent.SurfaceRect.Location, new Size(this.largeur, this.hauteur));
            }
        }

        public override Figure CloneInstance(bool keepLocation)
        {

            Point pos ;
            if(keepLocation==true)
            {
                pos = new Point(this.m_ptPosition.X, this.m_ptPosition.Y);
            }
            else
            {
                pos = new Point(this.m_ptPosition.X  +20, this.m_ptPosition.Y);
            }
            
            Rect fig = new Rect(pos, this.largeur, this.hauteur,this.isRounded,this.FillColor, this.m_colTrait, this.m_nEpaisseur, this.m_bRempli,null);
            fig.cornerRadius = this.cornerRadius;
            return fig;
        }
    }

}
