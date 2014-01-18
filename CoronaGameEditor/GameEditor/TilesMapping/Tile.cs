using System;
using System.Drawing;
using Krea.Corona_Classes;

namespace Krea.GameEditor.TilesMapping
{
  
    public class Tile
    {
        //---------------------------------------------------
        //-------------------Attributes----------------------
        //---------------------------------------------------
        public int Width;
        public int Height;
        public Point Location;
        public int LineIndex;
        public int ColumnIndex;
        public bool IsUsed;
        public bool isSelected;
        public bool IsVisible;
        public bool IsCrossable;
        private TilesMap mapParent;

        
        public TileModel TileModelTexture;
      
        public TileModel TileModelImageObject;
      
        public TileSequence TileTextureSequence;
     
        public TileSequence TileObjectSequence;

        public TileEvent TileEvent;
        

        //---------------------------------------------------
        //-------------------Constructors--------------------
        //---------------------------------------------------
        public Tile(int lineIndex, int columnsIndex, int width, int height, bool isVisible,TilesMap mapParent)
        {
            
            this.LineIndex = lineIndex;
            this.ColumnIndex = columnsIndex;
            this.Width = width;
            this.Height = height;
            this.IsVisible = isVisible;
            this.IsUsed = false;
            this.isSelected = false;
            IsCrossable = false;
            this.mapParent = mapParent;

            //Definirla position de la tile
            this.Location = new Point(this.Width * this.ColumnIndex, this.Height * this.LineIndex);

        }

        //---------------------------------------------------
        //-------------------Methods-------------------------
        //---------------------------------------------------

        public void setTexture(TileModel model)
        {
 
            TileModelTexture = model;
            if(model != null)
                this.TileTextureSequence = null;
        }

        public void setObjectImage(TileModel model)
        {
            this.TileModelImageObject = model;
            if (model != null)
                this.TileObjectSequence = null;
        }

        public void setTextureSequence(TileSequence sequence)
        {
            TileTextureSequence = sequence;
            if (sequence != null)
                 this.TileModelTexture = null;
        }

        public void setObjectSequence(TileSequence sequence)
        {
            this.TileObjectSequence = sequence;
            if (sequence != null)
                this.TileModelImageObject = null;
        }

        public void setEvent(TileEvent ev)
        {
            this.TileEvent = ev;

        }
        public bool isTouched(Point p)
        {
            Rectangle rect = new Rectangle(this.Location, new Size(this.Width, this.Height));
            return rect.Contains(p);
        }

        public bool isTouched(Rectangle rect)
        {
            return rect.IntersectsWith(new Rectangle(this.Location, new Size(this.Width, this.Height)));
        }


        public void DrawGorgon(Point offsetPoint, string layerToShow, bool showCollision,float worldScale)
        {
             Point pDest = new Point((int)((this.mapParent.Location.X + this.Location.X + offsetPoint.X)*worldScale),
                 (int)((this.mapParent.Location.Y + this.Location.Y + offsetPoint.Y)*worldScale));

            Rectangle rectDest = new Rectangle(pDest,
                new Size((int)((this.mapParent.TilesWidth*worldScale)), 
                    (int)((this.mapParent.TilesHeight)*worldScale)));

            if (layerToShow.Equals("ALL") || layerToShow.Equals("TEXTURE"))
            {
                //Dessiner l'image associée
                if (this.TileModelTexture != null)
                {
                    GorgonLibrary.Graphics.Sprite sprite = this.TileModelTexture.GorgonSprite;
                    if (sprite != null && sprite.Image != null)
                    {
                        float imgScaleX = (float)this.mapParent.TilesWidth / (float)sprite.Image.Width;
                        float imgScaleY = (float)this.mapParent.TilesHeight / (float)sprite.Image.Height;

                        float finalXScale = worldScale * imgScaleX;
                        float finalYScale = worldScale * imgScaleY;
                        sprite.SetScale(finalXScale, finalYScale);

                        sprite.SetPosition(pDest.X, pDest.Y);
                        sprite.Draw();
                    }
                    else
                    {
                        this.TileModelTexture = null;
                    }
                         
                    
                }


                if (this.TileTextureSequence != null)
                {
                    if (this.mapParent.TextureSequences.Contains(this.TileTextureSequence) && this.TileTextureSequence.Frames.Count > 0)
                    {
                        GorgonLibrary.Graphics.Sprite sprite = this.TileTextureSequence.Frames[0].GorgonSprite;
                        if (sprite != null && sprite.Image != null)
                        {
                            float imgScaleX = (float)this.mapParent.TilesWidth / (float)sprite.Image.Width;
                            float imgScaleY = (float)this.mapParent.TilesHeight / (float)sprite.Image.Height;

                            float finalXScale = worldScale * imgScaleX;
                            float finalYScale = worldScale * imgScaleY;
                            sprite.SetScale(finalXScale, finalYScale);

                            sprite.SetPosition(pDest.X, pDest.Y);
                            sprite.Draw();
                        }
                    }
                    else
                    {
                        this.TileTextureSequence = null;
                    }
                }
            }


            if (layerToShow.Equals("ALL") || layerToShow.Equals("OBJECT"))
            {
                if (this.TileModelImageObject != null)
                {
                     GorgonLibrary.Graphics.Sprite sprite = this.TileModelImageObject.GorgonSprite;
                     if (sprite != null && sprite.Image != null)
                     {
                         float imgScaleX = (float)this.mapParent.TilesWidth / (float)sprite.Image.Width;
                         float imgScaleY = (float)this.mapParent.TilesHeight / (float)sprite.Image.Height;

                         float finalXScale = worldScale * imgScaleX;
                         float finalYScale = worldScale * imgScaleY;
                         sprite.SetScale(finalXScale, finalYScale);

                         sprite.SetPosition(pDest.X, pDest.Y);
                         sprite.Draw();
                     }
                     else
                     {
                         this.TileModelImageObject = null;
                     }
                   
                }


                if (this.TileObjectSequence != null)
                {
                    if (this.mapParent.ObjectSequences.Contains(this.TileObjectSequence) && this.TileObjectSequence.Frames.Count > 0)
                    {
                     
                        GorgonLibrary.Graphics.Sprite sprite = this.TileObjectSequence.Frames[0].GorgonSprite;
                        if (sprite != null && sprite.Image != null)
                        {
                            float imgScaleX = (float)this.mapParent.TilesWidth / (float)sprite.Image.Width;
                            float imgScaleY = (float)this.mapParent.TilesHeight / (float)sprite.Image.Height;

                            float finalXScale = worldScale * imgScaleX;
                            float finalYScale = worldScale * imgScaleY;
                            sprite.SetScale(finalXScale, finalYScale);

                            sprite.SetPosition(pDest.X, pDest.Y);
                            sprite.Draw();
                        }
                        else
                        {
                            this.TileObjectSequence = null;
                        }
                   
                    }
                    else
                    {
                        this.TileObjectSequence = null;
                    }
                }
            }

            if (layerToShow.Equals("ALL") || layerToShow.Equals("EVENT"))
            {
                if (this.TileEvent != null)
                {
                    if (this.mapParent.TileEvents.Contains(this.TileEvent))
                    {
                        GorgonLibrary.Graphics.Sprite sprite = null;
                        if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.collision)
                            sprite = this.mapParent.CollisionEventSprite;  
                        else if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.preCollision)
                             sprite = this.mapParent.PreCollisionEventSprite;  
                        else if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.postCollision)
                             sprite = this.mapParent.PostCollisionEventSprite;  
                        else if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.touch)
                             sprite = this.mapParent.TouchEventSprite;  
                    
                        if(sprite != null)
                        {
                            float imgScaleX = (float)this.mapParent.TilesWidth/2 / (float)sprite.Image.Width;
                            float imgScaleY = (float)this.mapParent.TilesHeight/2 / (float)sprite.Image.Height;

                            float finalXScale = worldScale * imgScaleX;
                            float finalYScale = worldScale * imgScaleY;
                            sprite.SetScale(finalXScale, finalYScale);

                            sprite.SetPosition(pDest.X,pDest.Y);
                            sprite.Draw();
                        }
                    }
                    else
                        this.TileEvent = null;
                   
                }
            }

            if (showCollision == true)
            {
                Color color;
                if (this.IsCrossable == true)    
                    color = Color.FromArgb(50, Color.LightBlue);
                else
                  color = Color.FromArgb(100, Color.Red);


                Rectangle rectCollision = new Rectangle(this.mapParent.Location.X + this.Location.X + offsetPoint.X,
                    this.mapParent.Location.Y + this.Location.Y + offsetPoint.Y,
                            this.mapParent.TilesWidth,this.mapParent.TilesHeight);

                GorgonGraphicsHelper.Instance.FillRectangle(rectCollision, 1, color, worldScale, false);
            }
             
        }
        
        public void Draw(Graphics g, Point offsetPoint,string layerToShow, bool showCollision)
        {
            Point pDest = new Point(this.mapParent.Location.X + this.Location.X + offsetPoint.X, this.mapParent.Location.Y + this.Location.Y + offsetPoint.Y);

            if (layerToShow.Equals("ALL") || layerToShow.Equals("TEXTURE"))
            {
                //Dessiner l'image associée
                if (this.TileModelTexture != null)
                {
                    g.DrawImage(this.TileModelTexture.Image, new Rectangle(pDest, new Size(this.mapParent.TilesWidth, this.mapParent.TilesHeight)));
                }


                if (this.TileTextureSequence != null)
                {
                    if (this.mapParent.TextureSequences.Contains(this.TileTextureSequence) && this.TileTextureSequence.Frames.Count > 0)
                        g.DrawImage(this.TileTextureSequence.Frames[0].Image, new Rectangle(pDest, new Size(this.mapParent.TilesWidth, this.mapParent.TilesHeight)));
                    else
                    {
                        this.TileTextureSequence = null;
                    }
                }
            }


            if (layerToShow.Equals("ALL") || layerToShow.Equals("OBJECT"))
            {
                if (this.TileModelImageObject != null)
                {
                    g.DrawImage(this.TileModelImageObject.Image, new Rectangle(pDest, new Size(this.mapParent.TilesWidth, this.mapParent.TilesHeight)));
                }


                if (this.TileObjectSequence != null)
                {
                    if (this.mapParent.ObjectSequences.Contains(this.TileObjectSequence) && this.TileObjectSequence.Frames.Count > 0)
                    {
                        g.DrawImage(this.TileObjectSequence.Frames[0].Image, new Rectangle(pDest, new Size(this.mapParent.TilesWidth, this.mapParent.TilesHeight)));


                    }
                    else
                    {
                        this.TileObjectSequence = null;
                    }
                }
            }

            if (layerToShow.Equals("ALL") || layerToShow.Equals("EVENT"))
            {
                if (this.TileEvent != null)
                {
                    if (this.mapParent.TileEvents.Contains(this.TileEvent))
                    {
                        if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.collision)
                            g.DrawImage(Properties.Resources.collisionIcon2, new Rectangle(pDest, new Size(this.mapParent.TilesWidth/2, this.mapParent.TilesHeight/2)));
                        else if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.preCollision)
                            g.DrawImage(Properties.Resources.preCollisionIcon, new Rectangle(pDest, new Size(this.mapParent.TilesWidth / 2, this.mapParent.TilesHeight / 2)));
                        else if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.postCollision)
                            g.DrawImage(Properties.Resources.postCollisionIcon, new Rectangle(pDest, new Size(this.mapParent.TilesWidth / 2, this.mapParent.TilesHeight / 2)));
                        else if (this.TileEvent.Type == TilesMapping.TileEvent.TileEventType.touch)
                            g.DrawImage(Properties.Resources.touchIcon, new Rectangle(pDest, new Size(this.mapParent.TilesWidth / 2, this.mapParent.TilesHeight / 2)));
                    }
                    else
                        this.TileEvent = null;
                   
                }
            }

            if (showCollision == true)
            {
                if (this.IsCrossable == true)
                {
                    Brush brosse = new SolidBrush(Color.FromArgb(50, Color.LightBlue));

                    g.FillRectangle(brosse, pDest.X, pDest.Y, this.Width, this.Height);
                }
                else
                {
                    Brush brosse = new SolidBrush(Color.FromArgb(50, Color.Red));
                    g.FillRectangle(brosse, pDest.X, pDest.Y, this.Width, this.Height);
                }
            }
             
        }

    }
}
