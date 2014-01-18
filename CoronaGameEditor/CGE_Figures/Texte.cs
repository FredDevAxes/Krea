using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.ComponentModel;
using Krea.CoronaClasses;
using Krea.GameEditor.FontManager;


namespace Krea.CGE_Figures
{
    [Serializable]
    public class Texte : Figure
    {
        public String txt;
        public Font2 font2;
        public SizeF stringSize;


        public Texte() { }

        public Texte(Point position, String txt, Font2 font2, Color couleur, int epaisseur, bool rempli, DisplayObject objParent)
            : base(position, couleur,Color.Empty, rempli, epaisseur,objParent)
        {

            this.typeFigure = "TEXT";
            this.txt = txt;
            this.font2 = font2;

            
        }

        public Texte(Point position, String txt, Font2 font2, Byte coul_A, Byte coul_R, Byte coul_G, Byte coul_B, int epaisseur, bool rempli, DisplayObject objParent)
            : base(position, coul_A, coul_R, coul_G, coul_B, rempli, epaisseur,objParent)
        {
            this.typeFigure = "TEXT";
            this.txt = txt;
            this.font2 = font2 ;

            

        }

        [DescriptionAttribute("The text that should be displayed.")]
        public String Txt
        {
            get { return txt; }
            set { txt = value; }
        }

        [DescriptionAttribute("The font used for the text.")]
        public string FontFamily
        {
            get { return font2.FamilyName; }
            set { font2.FamilyName = value; }
        }

        [DescriptionAttribute("The font size of the text.")]
        public float FontSize
        {
            get { return font2.Size; }
            set { font2.Size = value; }
        }


        public override void Dessine(Graphics g,int alpha,Point offsetPoint)
        {
            //Définir les couleurs en ARGB
            Brush brosse = new SolidBrush(Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));

           
            if (this.DisplayObjectParent != null)
                g.DrawString(txt, new Font(font2.FamilyName, font2.Size, font2.Style), brosse, this.DisplayObjectParent.SurfaceRect);
            else
            {
                Point pDest = new Point(offsetPoint.X + this.Position.X, offsetPoint.Y + this.Position.Y);
                g.DrawString(txt, new Font(font2.FamilyName, font2.Size, font2.Style), brosse, pDest);
            }
            
            brosse.Dispose();

        }


        public override Bitmap DrawInBitmap(int alpha, float worldScale)
        {
         
            Rectangle surfaceText = this.DisplayObjectParent.SurfaceRect;
            int width = (int)Math.Round((surfaceText.Width / (double)4)) * 4;
            int height = (int)Math.Round((surfaceText.Height / (double)4)) * 4;

            this.DisplayObjectParent.SurfaceRect = new Rectangle(surfaceText.Location, new Size(width, height));

            Size finalSize = new Size((int)(width * worldScale), (int)(height * worldScale));
            Bitmap bitmap = new Bitmap(finalSize.Width,finalSize.Height);
            Graphics g = Graphics.FromImage(bitmap);

            //stringSize = this.getBounds(new Matrix()).Size;
            if (font2.FontItem == null)
                font2.FontItem = new FontItem("DEFAULT", this.DisplayObjectParent.CoronaObjectParent.LayerParent.SceneParent.projectParent);

            if (DisplayObjectParent.GradientColor.isEnabled == true)
            {

                Rectangle destRect = new Rectangle(Point.Empty, finalSize);
                LinearGradientBrush br = DisplayObjectParent.GradientColor.getBrushForDrawing(destRect, Convert.ToInt32(this.DisplayObjectParent.Alpha * 255));
                if (font2.FontItem.NameForIphone.Equals("DEFAULT"))
                {
                    g.DrawString(txt, new Font(SystemFonts.DefaultFont.FontFamily, font2.Size*worldScale), br, destRect);
                }
                else
                {
                    g.DrawString(txt, new Font(font2.FontItem.NameForIphone, font2.Size * worldScale), br, destRect);
                }
                br.Dispose();
            }
            else
            {
                //Définir les couleurs en ARGB
                Rectangle destRect = new Rectangle(Point.Empty, finalSize);
                Brush brosse = new SolidBrush(Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));

                if (font2.FontItem.NameForIphone.Equals("DEFAULT"))
                {
                    g.DrawString(txt, new Font(SystemFonts.DefaultFont.FontFamily, font2.Size * worldScale), brosse, destRect);
                }
                else
                {
                    g.DrawString(txt, new Font(font2.FontItem.NameForIphone, font2.Size* worldScale), brosse, destRect);
                }
                brosse.Dispose();
            }

            g.Dispose();
            return bitmap;
        }

        public override void DrawGorgon(Point pointDest, int alpha, float worldScale)
        {
            if (this.DisplayObjectParent != null)
            {
                if (this.DisplayObjectParent.GorgonSprite != null)
                {
                    this.DisplayObjectParent.GorgonSprite.Draw();
                }
            }
        }

        public override void DessineAt(Graphics g, Point pointDest,int alpha)
        {
            //Check size
            Rectangle surfaceText = this.DisplayObjectParent.SurfaceRect;
            int width = (int)Math.Round((surfaceText.Width / (double)4)) * 4;
            int height = (int)Math.Round((surfaceText.Height / (double)4)) * 4;

            this.DisplayObjectParent.SurfaceRect = new Rectangle(surfaceText.Location, new Size(width, height));

            //stringSize = this.getBounds(new Matrix()).Size;
            if (font2.FontItem == null)
                font2.FontItem = new FontItem("DEFAULT", this.DisplayObjectParent.CoronaObjectParent.LayerParent.SceneParent.projectParent);

            if (DisplayObjectParent.GradientColor.isEnabled == true)
            {
                
                Rectangle destRect = new Rectangle(pointDest, this.DisplayObjectParent.SurfaceRect.Size);
                LinearGradientBrush br = DisplayObjectParent.GradientColor.getBrushForDrawing(destRect, Convert.ToInt32(this.DisplayObjectParent.Alpha * 255));
                if (font2.FontItem.NameForIphone.Equals("DEFAULT"))
                {
                    g.DrawString(txt, new Font(SystemFonts.DefaultFont.FontFamily, font2.Size), br, destRect);
                }
                else
                {
                    g.DrawString(txt, new Font(font2.FontItem.NameForIphone, font2.Size), br, destRect);
                }
                br.Dispose();
            }
            else
            {
                //Définir les couleurs en ARGB
                Rectangle destRect = new Rectangle(pointDest, this.DisplayObjectParent.SurfaceRect.Size);
                Brush brosse = new SolidBrush(Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));

                if (font2.FontItem.NameForIphone.Equals("DEFAULT"))
                {
                    g.DrawString(txt, new Font(SystemFonts.DefaultFont.FontFamily, font2.Size), brosse, destRect);
                }
                else
                {
                    g.DrawString(txt, new Font(font2.FontItem.NameForIphone, font2.Size), brosse, destRect);
                }
                brosse.Dispose();
            }

            
        }

        public override bool Clic(Point p,Matrix matrix)
        {
            Rectangle rect = this.getBounds(matrix);
            

            return rect.Contains(p);
        }
        
        public override Rectangle getBounds(Matrix matrix)
        {
            GraphicsPath g = new GraphicsPath();

            g.AddString(this.txt, new FontFamily(font2.FamilyName), (int)font2.Style, font2.Size, this.Position, StringFormat.GenericDefault);


            RectangleF rectF = g.GetBounds(matrix);
            return new Rectangle(new Point((int)rectF.Location.X, (int)rectF.Location.Y), new Size((int)rectF.Width, (int)rectF.Height));

        }

        public override void SetSizeFromPoint(Point pt)
        {

            //if (this.DisplayObjectParent != null)
            //{
            //    this.font2.Size = this.font2.Size - (float)(this.lastPos.X - pt.X)/10;
            //    if (this.font2.Size < 1) this.font2.Size = 1;

            //    this.lastPos.X = pt.X;

            //    Size textSize = System.Windows.Forms.TextRenderer.MeasureText(this.txt,
            //            new Font(this.font2.FamilyName,this.font2.Size, this.font2.Style));
            //    this.DisplayObjectParent.SurfaceRect = new Rectangle(this.DisplayObjectParent.SurfaceRect.Location, textSize);

            
            //}
            if (this.DisplayObjectParent != null)
            {
                Rectangle surfaceText = this.DisplayObjectParent.SurfaceRect;
                float xMove = (this.lastPos.X - pt.X);
                float yMove = (this.lastPos.Y - pt.Y);
                float width = surfaceText.Width - xMove*2;
                Console.WriteLine("XMOVE = " + xMove + " ------ WIDTH = " + width);

                //width = (int)Math.Round((width / (double)4)) * 4;
               
                
                //if (width < 4) width = 4;
                //if (height < 4) height = 4;
                this.lastPos.X = pt.X;
                this.lastPos.Y = pt.Y;

                string fontName = "DEFAULT";
                if (this.font2.FontItem != null)
                    fontName = this.font2.FontItem.NameForIphone;
                SizeF result = Krea.Corona_Classes.GorgonGraphicsHelper.Instance.GetTextSize(this.txt, fontName, this.font2.Size,
                            Point.Empty, true, new Rectangle(surfaceText.Location, new Size((int)width, 0)));



                int nearestMultipleWidth = (int)Math.Round((result.Width / (double)4)) * 4;
                int nearestMultipleHeight = (int)Math.Round((result.Height / (double)4)) * 4;
                this.DisplayObjectParent.SurfaceRect = new Rectangle(surfaceText.Location, new Size(nearestMultipleWidth, nearestMultipleHeight));

                //SizeF result;
                //using (var image = new Bitmap(1, 1))
                //{
                //    using (var g = Graphics.FromImage(image))
                //    {
                //        result = g.MeasureString(this.txt, new Font(SystemFonts.DefaultFont.FontFamily, font2.Size), (int)width);

                //    }

                //    image.Dispose();
                //}


                //GraphicsPath path = new GraphicsPath();
                //path.AddString(this.txt, new FontFamily(font2.FamilyName), (int)font2.Style, font2.Size, new Rectangle(surfaceText.Location,new Size((int)width,Int32.MaxValue)), StringFormat.GenericDefault);
                //RectangleF size = path.GetBounds();
                //height = size.Height*3f;

                //path.Dispose();
                //path = null;

               

             
            }
          
        }
        public override Figure CloneInstance(bool keepLocation)
        {
            int offSet = 0;
            if (keepLocation == false)
                offSet = 20;

            Point pos = new Point(this.DisplayObjectParent.SurfaceRect.X + offSet, this.DisplayObjectParent.SurfaceRect.Y);
            float size = this.font2.Size;
            Texte fig = new Texte(pos, this.txt, new Font2(this.font2.FontItem.NameForIphone, size, this.font2.Style), this.m_FillColor, this.m_nEpaisseur, this.m_bRempli, null);
            fig.font2.FontItem = this.font2.FontItem;

            return fig;
        }
    }
}


