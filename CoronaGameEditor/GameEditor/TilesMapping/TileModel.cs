using System;
using System.Drawing;

namespace Krea.GameEditor.TilesMapping
{
    [Serializable()]
    public class TileModel
    {

        //---------------------------------------------------
        //-------------------Attributes----------------------
        //---------------------------------------------------
        public Bitmap Image;
        public bool IsCrossable;
        public Rectangle surfaceRect;
        public bool IsTexture;
        public Size OriginalSize;
        public string Name;

        [NonSerialized()]
        public GorgonLibrary.Graphics.Sprite GorgonSprite;
        [NonSerialized()]
        public bool ShouldBeExported = false;
        //---------------------------------------------------
        //-------------------Constructors--------------------
        //---------------------------------------------------
        public TileModel(string name,Point location, Size size, GorgonLibrary.Graphics.Sprite sprite, bool isCrossable, bool isTexture)
        {
            this.surfaceRect = new Rectangle(location, new Size(32,32));

            this.OriginalSize = size;
            this.GorgonSprite = sprite;
            this.IsCrossable = isCrossable;
            this.IsTexture = isTexture;
            this.Name = name;
        }

        //---------------------------------------------------
        //-------------------Methods-------------------------
        //---------------------------------------------------
        public bool isTouched(Point p)
        {
            return this.surfaceRect.Contains(p);
        }

        public bool isTouched(Rectangle rect)
        {
            return rect.IntersectsWith(this.surfaceRect);
        }

        public void drawTileModel(Graphics g)
        {
            
            g.DrawImage(this.Image, this.surfaceRect);

        }

        public void DrawGorgon(Point offsetPoint)
        {
            if (this.GorgonSprite != null)
            {
                float imgScaleX = (float)this.surfaceRect.Width / (float)this.GorgonSprite.Image.Width;
                float imgScaleY = (float)this.surfaceRect.Height / (float)this.GorgonSprite.Image.Height;

                this.GorgonSprite.SetScale(imgScaleX, imgScaleY);

                this.GorgonSprite.SetPosition(this.surfaceRect.X + offsetPoint.X, this.surfaceRect.Y + offsetPoint.Y);
                this.GorgonSprite.Draw();
            }
        }
    }
}
