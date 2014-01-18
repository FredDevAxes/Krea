using System;
using System.Drawing;

namespace Krea.CoronaClasses
{
        //---------------------------------------------------
        //-------------------Classe FRAME------------------------
        //---------------------------------------------------
         [Serializable()]
        public class SpriteFrame
        {

            //---------------------------------------------------
            //-------------------Attributs--------------------
            //---------------------------------------------------
            public int Index;
            public Image Image;
            public String NomFrame;
            public Point Position;
            public CoronaSpriteSheet SpriteSheetParent;
            private String pathImage;
            public Size ImageSize;

            //---------------------------------------------------
            //-------------------Constructeurs--------------------
            //---------------------------------------------------

            public SpriteFrame(String nom, int index, Image img,CoronaSpriteSheet parent)
            {
                this.SpriteSheetParent = parent;
                this.Index = index;
                int id = nom.LastIndexOf(@"\") + 1;
                this.NomFrame = nom.Substring(id);
                this.Image = img;
                this.ImageSize = img.Size;
            }

            public SpriteFrame(String nom, int index, String pathImage,CoronaSpriteSheet parent)
            {
                this.SpriteSheetParent = parent;
                this.Index = index;
                int id = nom.LastIndexOf(@"\") + 1;
                this.NomFrame = nom.Substring(id);
                this.pathImage = pathImage;



            }


            //---------------------------------------------------
            //-------------------Methodes--------------------
            //---------------------------------------------------
            public void setPosition(Point position)
            {
                this.Position = position;
            }

            public override string ToString()
            {
                return this.NomFrame;

            }

            public SpriteFrame clone()
            {
                return new SpriteFrame(this.NomFrame, this.Index, this.Image,this.SpriteSheetParent);
            }

            public void dessineFrame(Graphics g, float XRatio, float YRatio,float framesFactor,string sheetPath)
            {
                try
                {
                    int indexFrameInSheet = this.SpriteSheetParent.Frames.IndexOf(this);
                    float moyenneRatio = (XRatio + YRatio) / 2;
                    SizeF size;

                    bool res = true;
                    try
                    {
                        int width = this.Image.Width;
                    }
                    catch (Exception x)
                    {
                        res = false;
                    }

                    if(this.Image != null && res == true)
                       size  = new SizeF(this.Image.Width / framesFactor * moyenneRatio, this.Image.Height / framesFactor * moyenneRatio);
                    else
                        size = new SizeF(this.ImageSize.Width / framesFactor * moyenneRatio, this.ImageSize.Height / framesFactor * moyenneRatio);

                    Image finalImg = null;
                    if (this.Image != null && res == true)
                        finalImg = this.Image;
                    else if (!sheetPath.Equals(""))
                    {
                        string framePath = sheetPath + "\\" + this.SpriteSheetParent.Name + "_frame" + indexFrameInSheet + ".png";
                        if (System.IO.File.Exists(framePath))
                        {
                            finalImg = Image.FromFile(framePath);

                        }
                    }

                    if (finalImg != null)
                    {
                        g.DrawImage(finalImg, new RectangleF(new PointF(this.Position.X / framesFactor * XRatio, this.Position.Y / framesFactor * YRatio), size));

                        if (!sheetPath.Equals(""))
                        {
                            finalImg.Dispose();
                            finalImg = null;
                        }
                    }
                    
                }
                catch (Exception ex)
                {

                }

               
            }

            public void dessineFrame(Graphics g, float XRatio, float YRatio)
            {
                float moyenneRatio = (XRatio + YRatio) / 2;
                SizeF size = new SizeF(this.Image.Width * moyenneRatio, this.Image.Height  * moyenneRatio);

                g.DrawImage(this.Image, new RectangleF(new PointF(this.Position.X * XRatio, this.Position.Y  * YRatio), size));
            }
        }
    
}
