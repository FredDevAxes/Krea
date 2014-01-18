using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Krea.CGE_Figures;
using System.Drawing.Drawing2D;
using Krea.Corona_Classes;
using System.Reflection;
using System.Drawing.Imaging;


namespace Krea.CoronaClasses
{
    [Serializable()]
    public class DisplayObject
    {
        //---------------------------------------------------
        //-------------------Enums-----------------------
        //---------------------------------------------------
        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum AlphaBlendingMode
        {
            normal = 1,
            add = 2,
        }


        //---------------------------------------------------
        //-------------------Attributes-----------------------
        //---------------------------------------------------
        
        private Image image;
        public CoronaSpriteSet SpriteSet;
        private int currentFrame;
        public String Name;
        public int Rotation = 0;
        private Point lastPos;
        private Rectangle surfaceRect;
        private const double VERS_DEGRE = 180 / 3.141592;
        private String type;
        private float scale;
        public  Rectangle InitialRect;
        public float Alpha;
        public AlphaBlendingMode blendMode = AlphaBlendingMode.normal;
        public GradientColor GradientColor;
        public CoronaObject CoronaObjectParent;
        public String OriginalAssetName;

        public Color ImageFillColor = Color.Transparent;
       
        public int CurrentSpriteFrame = 0;
        public bool AutoPlay = false;
        public string CurrentSequence = "DEFAULT";
        //Si c'est une figure
        public Figure Figure;

        [NonSerialized]
        public GorgonLibrary.Graphics.Sprite GorgonSprite;
        [NonSerialized]
        public GorgonLibrary.Graphics.TextSprite GorgonTextSprite;
        [NonSerialized]
        public PictureBox AnimSpritePictBxParent;

       

        public DisplayObject(Image img, Point location, CoronaObject coronaObjectParent)
        {
            this.surfaceRect = new Rectangle();
            this.surfaceRect.Size = img.Size;
            
            this.surfaceRect.Location = location;
            InitialRect = this.surfaceRect;

            this.Name = "";
            this.type = "IMAGE";
            this.image = img;
           
            CurrentFrame = 0;
            Alpha = 1.0F;

            this.GradientColor = new GradientColor();
            this.CoronaObjectParent = coronaObjectParent;
           
            
        }

        public DisplayObject(GorgonLibrary.Graphics.Sprite gorgonSprite, Point location, CoronaObject coronaObjectParent)
        {
            this.surfaceRect = new Rectangle();
            this.GorgonSprite = gorgonSprite;

            this.surfaceRect.Size = new Size(GorgonSprite.Image.Width, GorgonSprite.Image.Height) ;

            this.surfaceRect.Location = location;
            InitialRect = this.surfaceRect;

            this.Name = "";
            this.type = "IMAGE";
            

            CurrentFrame = 0;
            Alpha = 1.0F;

            this.GradientColor = new GradientColor();
            this.CoronaObjectParent = coronaObjectParent;


        }

        public DisplayObject(Figure fig,CoronaObject coronaObjectParent)
        {
            this.Name = "Figure";
            this.type = "FIGURE";
            this.Figure = fig;
            this.Figure.DisplayObjectParent = this;

            if (this.Figure.ShapeType.Equals("RECTANGLE"))
            {
                Rect rect = this.Figure as Rect;
                this.surfaceRect = rect.getBounds(new Matrix());
            }
            else if (this.Figure.ShapeType.Equals("CIRCLE"))
            {
                Cercle cercle = this.Figure as Cercle;
                this.surfaceRect = cercle.getBounds(new Matrix());
            }
            else if (this.Figure.ShapeType.Equals("LINE"))
            {
                Line line = this.Figure as Line;

                this.surfaceRect = line.getBounds(new Matrix());
            }
            else if (this.Figure.ShapeType.Equals("TEXT"))
            {
                Texte text = this.Figure as Texte;
                this.surfaceRect = new Rectangle(text.Position,
                    System.Windows.Forms.TextRenderer.MeasureText(text.txt,
                    new Font(text.font2.FamilyName,text.font2.Size, text.font2.Style)));
            }
            else if (this.Figure.ShapeType.Equals("CURVE"))
            {
                CourbeBezier courbe = this.Figure as CourbeBezier;
                this.surfaceRect = courbe.getBounds(new Matrix());
            }

            Alpha = 1.0F;
            this.GradientColor = new GradientColor();
            this.CoronaObjectParent = coronaObjectParent;

        }


        public DisplayObject(CoronaSpriteSet spriteSet, Point location, CoronaObject coronaObjectParent)
        {

            this.type = "SPRITE";
            Alpha = 1.0F;
            this.SpriteSet = spriteSet;
            this.SpriteSet.DisplayObjectParent = this;

           this.surfaceRect = new Rectangle(location,new Size(50,50));

            
            InitialRect = this.surfaceRect;
            this.CoronaObjectParent = coronaObjectParent;
            this.GradientColor = new GradientColor();

            this.CurrentFrame = SpriteSet.indexFrameDep;

        
          
        }


        public void setSequence(string sequenceName)
        {
            for (int i = 0; i < this.SpriteSet.Sequences.Count; i++)
            {
                if (this.SpriteSet.Sequences[i].Name == sequenceName)
                {
                    this.CurrentSequence = sequenceName;
                    int frame = this.currentFrame;
                    this.CurrentFrame = frame;
                    return;
                }
            }

            this.CurrentSequence = "DEFAULT";
            int framedefault = this.currentFrame;
            this.CurrentFrame = framedefault;

        }

        public CoronaSpriteSetSequence getSequenceByName(string sequenceName)
        {
            for (int i = 0; i < this.SpriteSet.Sequences.Count; i++)
            {
                if (this.SpriteSet.Sequences[i].Name == sequenceName)
                {
                    return this.SpriteSet.Sequences[i];
                }
            }

            return null;
        }

        public bool containsPoint(Point p)
        {
  
            GraphicsPath gp = new GraphicsPath();
            Matrix m = this.getMatrixForDrawing(this.surfaceRect, 1.0f, 1.0f);
            gp.AddRectangle(this.surfaceRect);

            return gp.GetBounds(m).Contains(p);
                
            
        }

        public void move(Point pEvent)
        {
  
            int xMove =  this.lastPos.X - pEvent.X;
            int yMove =this.lastPos.Y - pEvent.Y;

            this.surfaceRect.Location = new Point(this.SurfaceRect.Location.X - xMove,  this.SurfaceRect.Location.Y - yMove);
            InitialRect.X = this.surfaceRect.X;
            InitialRect.Y = this.surfaceRect.Y;

            //Si le displayObject est une figure
            //if (this.type.Equals("IMAGE") || this.type.Equals("SPRITE"))
            //{
            //    if (this.GorgonSprite != null)
            //        this.GorgonSprite.SetPosition((float)this.SurfaceRect.X + this.GorgonSprite.Axis.X, (float)this.SurfaceRect.Y + this.GorgonSprite.Axis.Y);
            //}
            //else
                if (this.type.Equals("FIGURE"))
            {
                this.Figure.Position = this.surfaceRect.Location;

                if (this.Figure.ShapeType.Equals("CURVE"))
                {
                    CourbeBezier courbe = this.Figure as CourbeBezier;
                    for (int i = 0; i < courbe.UserPoints.Count; i++)
                    {
                        int X = courbe.UserPoints[i].X - xMove;
                        int Y = courbe.UserPoints[i].Y - yMove;

                        courbe.UserPoints[i] = new Point(X, Y);
                    }
                }
                else if (this.Figure.ShapeType.Equals("LINE"))
                {
                    Line line = this.Figure as Line;
                    for (int i = 0; i < line.Points.Count; i++)
                    {
                        int X = line.Points[i].X - xMove;
                        int Y = line.Points[i].Y - yMove;

                        line.Points[i] = new Point(X, Y);
                    }
                }
                //else if(this.Figure.ShapeType.Equals("RECTANGLE") || this.Figure.ShapeType.Equals("TEXT"))
                //{
                //    if(this.GorgonSprite != null)
                //         this.GorgonSprite.SetPosition((float)this.SurfaceRect.X + this.GorgonSprite.Axis.X, (float)this.SurfaceRect.Y + this.GorgonSprite.Axis.Y);
                //}
            }

            PathFollow pathFollow = this.CoronaObjectParent.PathFollow;
            if ( pathFollow!= null)
            {
                for (int i = 0; i < pathFollow.Path.Count; i++)
                {
                    int X = pathFollow.Path[i].X - xMove;
                    int Y = pathFollow.Path[i].Y - yMove;
                    pathFollow.Path[i]  = new Point(X, Y);
                }
            }
            this.lastPos.X = pEvent.X;
            this.lastPos.Y = pEvent.Y;
           
        }


      
        public void setSizeFromPoint(Point pEvent)
        {
            //Si le displayObject n'est pas une figure
            if (!this.type.Equals("FIGURE"))
            {
                int width = this.SurfaceRect.Width - (this.lastPos.X - pEvent.X);
                int height = this.SurfaceRect.Height - (this.lastPos.Y - pEvent.Y);

                this.lastPos.X = pEvent.X;
                this.lastPos.Y = pEvent.Y;
                if (width < 5) width = 5;
                if (height < 5) height = 5;
                this.surfaceRect.Size = new Size(width, height);

            }
            else
            {
                this.Figure.SetSizeFromPoint(pEvent);

                
            }
           
        }
      

        public void setAnimPictureBoxParent(PictureBox pictBx)
        {
            this.AnimSpritePictBxParent = pictBx;
        }

        public void dessineAt(Graphics g,Point offsetPoint,bool showSelection, Matrix matrixToApply,float xScale,float yScale)
        {


            try
            {
                Rectangle rectDest = new Rectangle(new Point(offsetPoint.X + this.surfaceRect.Location.X,
                                                         offsetPoint.Y + this.surfaceRect.Location.Y), this.surfaceRect.Size);
                Matrix m = null;
                if (matrixToApply != null)
                    m = matrixToApply;
                else
                    m = this.getMatrixForDrawing(rectDest, xScale, yScale);

                //Si c'est un sprite  -----------
                if (this.type.Equals("SPRITE"))
                {
                    if (this.CoronaObjectParent != null)
                    {
                        if (this.SpriteSet != null)
                        {
                            if (this.currentFrame < this.SpriteSet.Frames.Count)
                            {

                                g.Transform = m;


                                if (this.CurrentSequence == "") this.setSequence("");

                                if (this.CurrentSequence == "DEFAULT")
                                {
                                    Image image = this.SpriteSet.Frames[this.currentFrame].Image;

                                    float factor = this.SpriteSet.Frames[this.currentFrame].SpriteSheetParent.FramesFactor;
                                    if (factor <= 0)
                                        factor = 1;

                                    int width = Convert.ToInt32((float)image.Size.Width / factor);
                                    int height = Convert.ToInt32((float)image.Size.Height / factor);
                                    this.surfaceRect = new Rectangle(this.surfaceRect.Location, new Size(width, height));

                                    g.DrawImage(image, rectDest.X, rectDest.Y, this.surfaceRect.Width, this.surfaceRect.Height);

                                }
                                else
                                {
                                    CoronaSpriteSetSequence sequence = this.getSequenceByName(this.CurrentSequence);
                                    if (sequence == null)
                                    {
                                        this.setSequence("");

                                        Image image = this.SpriteSet.Frames[this.currentFrame].Image;

                                        float factor = this.SpriteSet.Frames[this.currentFrame].SpriteSheetParent.FramesFactor;
                                        if (factor <= 0)
                                            factor = 1;

                                        int width = Convert.ToInt32((float)image.Size.Width / factor);
                                        int height = Convert.ToInt32((float)image.Size.Height / factor);
                                        this.surfaceRect = new Rectangle(this.surfaceRect.Location, new Size(width, height));

                                        g.DrawImage(image, rectDest.X, rectDest.Y, this.surfaceRect.Width, this.surfaceRect.Height);

                                    }
                                    else
                                    {
                                        int convertedFrame = this.currentFrame + sequence.FrameDepart - 1;
                                        Image image = this.SpriteSet.Frames[convertedFrame].Image;

                                        float factor = this.SpriteSet.Frames[convertedFrame].SpriteSheetParent.FramesFactor;
                                        if (factor <= 0)
                                            factor = 1;

                                        int width = Convert.ToInt32((float)image.Size.Width / factor);
                                        int height = Convert.ToInt32((float)image.Size.Height / factor);
                                        this.surfaceRect = new Rectangle(this.surfaceRect.Location, new Size(width, height));

                                        g.DrawImage(image, rectDest.X, rectDest.Y, this.surfaceRect.Width, this.surfaceRect.Height);
                

                                    }
                                }

                                

                            }
                        }
                    }
                    else if (this.AnimSpritePictBxParent != null)
                    {
                        if (this.SpriteSet.Frames.Count > 0)
                        {
                            Image img = this.SpriteSet.Frames[CurrentFrame].Image;
                            if (img.Height > this.AnimSpritePictBxParent.Height || img.Height > this.AnimSpritePictBxParent.Height)
                            {
                                g.DrawImage(img, 0, 0, this.AnimSpritePictBxParent.Width, this.AnimSpritePictBxParent.Height);
                            }
                            else
                            {
                                int xDest = System.Convert.ToInt32(this.AnimSpritePictBxParent.Size.Width * 0.5 - rectDest.Size.Width * 0.5);
                                int yDest = System.Convert.ToInt32(this.AnimSpritePictBxParent.Size.Height * 0.5 - rectDest.Size.Height * 0.5);
                                Point pDest = new Point(xDest, yDest);
                                g.DrawImage(img, pDest);
                            }

                        }
                    }
                }
                //Si c'est une image simple -----------
                else if (this.type.Equals("IMAGE"))
                {
                    if (this.CoronaObjectParent != null)
                    {
                        g.Transform = m;

                        if (this.ImageFillColor.IsEmpty)
                            this.ImageFillColor = Color.White;

                        float tR = (float)ImageFillColor.R / 255;
                        float tG = (float)ImageFillColor.G / 255;
                        float tB = (float)ImageFillColor.B / 255;
                        float tA = this.Alpha;
                        ColorMatrix cm = new ColorMatrix(new float[][]
                    {
                        new float[] {tR, 0, 0, 0, 0},
                        new float[] {0, tG, 0, 0, 0},
                        new float[] {0, 0, tB, 0, 0},
                        new float[] {0, 0, 0,tA , 0},
                        new float[] {0, 0, 0, 0, 1}
                    });

                        // Create ImageAttributes
                        ImageAttributes imgAttribs = new ImageAttributes();
                        // Set color matrix
                        imgAttribs.SetColorMatrix(cm);

                        try
                        {
                            // Draw image with ImageAttributes
                            g.DrawImage(image,
                            rectDest,
                            0, 0, image.Width, image.Height,
                            GraphicsUnit.Pixel, imgAttribs);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }


                //Si c'est une figure -----------
                else if (this.type.Equals("FIGURE"))
                {
                    int alphaARGB = System.Convert.ToInt32(this.Alpha * 255);
                    if (this.Figure.ShapeType.Equals("RECTANGLE"))
                    {
                        Rect rect = this.Figure as Rect;
                        g.Transform = m;

                        rect.DessineAt(g, rectDest.Location, alphaARGB);


                    }
                    else if (this.Figure.ShapeType.Equals("CIRCLE"))
                    {

                        Cercle circ = this.Figure as Cercle;
                        g.Transform = m;

                        circ.DessineAt(g, rectDest.Location, alphaARGB);

                    }
                    else if (this.Figure.ShapeType.Equals("CURVE"))
                    {

                        CourbeBezier bezier = this.Figure as CourbeBezier;
                        g.Transform = m;

                        bezier.Dessine(g, alphaARGB, offsetPoint);

                    }
                    else if (this.Figure.ShapeType.Equals("LINE"))
                    {
                        Line line = this.Figure as Line;
                        g.Transform = m;


                        line.Dessine(g, alphaARGB, offsetPoint);


                    }
                    else if (this.Figure.ShapeType.Equals("TEXT"))
                    {
                        Texte txt = this.Figure as Texte;
                        g.Transform = m;

                        txt.DessineAt(g, rectDest.Location, alphaARGB);
                        rectDest = new Rectangle(rectDest.Location, new Size((int)txt.stringSize.Width, (int)txt.stringSize.Height));

                    }


                }



                //DRAW SELECTION
                if (showSelection == true)
                {
                    if (this.CoronaObjectParent.isSelected == true)
                    {
                        /* float[] dashValues = { 2, 2 };
                         Pen pen = new Pen(Color.FromArgb(150, Color.Blue), 3);
                         pen.DashPattern = dashValues;

                         g.ResetTransform();*/

                        Matrix matrixPath = new Matrix();
                        matrixPath.Scale(xScale, yScale);
                        g.Transform = matrixPath;


                        if (this.CoronaObjectParent.PathFollow != null)
                        {
                            if (this.CoronaObjectParent.PathFollow.isEnabled == true)
                            {
                                this.CoronaObjectParent.PathFollow.dessine(g, offsetPoint);
                            }
                        }


                        if (this.CoronaObjectParent.TransformBox != null)
                            this.CoronaObjectParent.TransformBox.drawTransformBox(g, offsetPoint,xScale,yScale);
                        /* GraphicsPath gp = new GraphicsPath();

                         gp.AddRectangle(rectDest);

                         RectangleF rectF = gp.GetBounds(m);
                         g.DrawRectangle(pen, rectF.X, rectF.Y, rectF.Width, rectF.Height);*/

                        g.Transform = m;
                    }
                }
            }
            catch (Exception Exception) 
            {
            }
           

            
        }

        public void DrawGorgon(Point offsetPoint, bool showSelection, float xScale, float yScale, bool applyRotation)
        {
            try
            {
                Rectangle rectDest = new Rectangle(new Point(offsetPoint.X + this.surfaceRect.Location.X,
                                                         offsetPoint.Y + this.surfaceRect.Location.Y), this.surfaceRect.Size);
               

                //Si c'est un sprite  -----------
                if (this.type.Equals("SPRITE"))
                {
                    if (this.GorgonSprite != null)
                    {
                        this.GorgonSprite.Color = Color.FromArgb((int)(this.Alpha * 255.0f), Color.White);

                       
                        float imgScaleX = (float)this.SurfaceRect.Width / (float)this.GorgonSprite.Image.Width;
                        float imgScaleY = (float)this.SurfaceRect.Height / (float)this.GorgonSprite.Image.Height;

                        float finalXScale = xScale * imgScaleX;
                        float finalYScale = yScale * imgScaleY;
                        this.GorgonSprite.SetScale(finalXScale, finalYScale);

                        this.GorgonSprite.SetAxis((float)this.GorgonSprite.Image.Width / 2, (float)this.GorgonSprite.Image.Height / 2);

                        this.GorgonSprite.SetPosition((float)rectDest.X * xScale + (this.GorgonSprite.Axis.X * finalXScale),
                                                      (float)rectDest.Y * yScale + (this.GorgonSprite.Axis.Y * finalYScale));


                        if (applyRotation == true)
                            this.GorgonSprite.Rotation = this.Rotation;
                        else
                            this.GorgonSprite.Rotation = 0;
                        
                        this.GorgonSprite.Draw();
                    }
                    
                }
                //Si c'est une image simple -----------
                else if (this.type.Equals("IMAGE"))
                {
                    if (this.GorgonSprite != null)
                    {


                        if (this.ImageFillColor.IsEmpty)
                            this.ImageFillColor = Color.White;

                       
                        this.GorgonSprite.Color = Color.FromArgb((int)(this.Alpha * 255.0f), this.ImageFillColor);

                        float imgScaleX = (float)this.SurfaceRect.Width / (float)this.GorgonSprite.Image.Width;
                        float imgScaleY = (float)this.SurfaceRect.Height / (float)this.GorgonSprite.Image.Height;

                        float finalXScale = xScale * imgScaleX;
                        float finalYScale = yScale * imgScaleY;
                        this.GorgonSprite.SetScale(finalXScale, finalYScale);


                        this.GorgonSprite.SetAxis((float)this.GorgonSprite.Image.Width / 2, (float)this.GorgonSprite.Image.Height / 2);

                        if (applyRotation == true)
                            this.GorgonSprite.Rotation = this.Rotation;
                        else
                            this.GorgonSprite.Rotation = 0;

                        this.GorgonSprite.SetPosition((float)rectDest.X * xScale + (this.GorgonSprite.Axis.X * finalXScale),
                                                      (float)rectDest.Y * yScale + (this.GorgonSprite.Axis.Y * finalYScale));
                        this.GorgonSprite.Draw();

                    }
                }


                //Si c'est une figure -----------
                else if (this.type.Equals("FIGURE"))
                {
                    int alphaARGB = System.Convert.ToInt32(this.Alpha * 255);
                    if (this.Figure.ShapeType.Equals("RECTANGLE"))
                    {

                        if (this.GorgonSprite != null)
                        {
                           
                            float imgScaleX = (float)this.SurfaceRect.Width / (float)this.GorgonSprite.Image.Width;
                            float imgScaleY = (float)this.SurfaceRect.Height / (float)this.GorgonSprite.Image.Height;

                            float finalXScale = xScale * imgScaleX;
                            float finalYScale = yScale * imgScaleY;

                            this.GorgonSprite.SetScale(finalXScale, finalYScale);

                            this.GorgonSprite.SetAxis((float)this.GorgonSprite.Image.Width / 2, (float)this.GorgonSprite.Image.Height / 2);
                            this.GorgonSprite.Rotation = this.Rotation;


                            this.GorgonSprite.SetPosition((float)rectDest.X * xScale + (this.GorgonSprite.Axis.X * finalXScale),
                                                                                 (float)rectDest.Y * yScale + (this.GorgonSprite.Axis.Y * finalYScale));
                            this.GorgonSprite.Draw();
                        }
                       

                    }
                    else if (this.Figure.ShapeType.Equals("CIRCLE"))
                    {

                        Cercle circ = this.Figure as Cercle;

                        circ.DrawGorgon(offsetPoint, alphaARGB, xScale);
                       

                    }
                    else if (this.Figure.ShapeType.Equals("CURVE"))
                    {

                        CourbeBezier bezier = this.Figure as CourbeBezier;

                        bezier.DrawGorgon(offsetPoint, alphaARGB, xScale);
                       

                    }
                    else if (this.Figure.ShapeType.Equals("LINE"))
                    {
                        Line line = this.Figure as Line;
                        line.DrawGorgon(offsetPoint, alphaARGB, xScale);


                    }
                    else if (this.Figure.ShapeType.Equals("TEXT"))
                    {



                        Krea.CGE_Figures.Texte textObject = this.Figure as Krea.CGE_Figures.Texte;

                        string fontName = "DEFAULT";
                        if (textObject.font2.FontItem != null)
                            fontName = textObject.font2.FontItem.NameForIphone;

                        GorgonGraphicsHelper.Instance.DrawText(textObject.txt, fontName, textObject.font2.Size,
                            Point.Empty, Color.FromArgb((int)(this.Alpha * 255.0f), textObject.FillColor), this.Rotation, true,
                            new Rectangle(offsetPoint.X + this.SurfaceRect.X,offsetPoint.Y + this.SurfaceRect.Y,this.surfaceRect.Width,this.surfaceRect.Height), xScale);



                        //if (textObject.TextSprite != null)
                        //{
                        //    textObject.TextSprite.Text = textObject.txt;
                        //    textObject.TextSprite.Font.FontSize = textObject.font2.Size * xScale;

                        //    textObject.TextSprite.Color = Color.FromArgb((int)(this.Alpha * 255.0f),
                        //      textObject.FillColor.R, textObject.FillColor.G, textObject.FillColor.B);

                        //    textObject.TextSprite.SetAxis((float)textObject.TextSprite.Width / 2, (float)textObject.TextSprite.Height / 2);

                        //    textObject.TextSprite.SetPosition((float)this.SurfaceRect.X + textObject.TextSprite.Axis.X + offsetPoint.X,
                        //        (float)this.SurfaceRect.Y + textObject.TextSprite.Axis.Y + offsetPoint.Y);

                        //    textObject.TextSprite.Rotation = this.Rotation;

                        //    textObject.TextSprite.SetScale(xScale, yScale);

                        //    textObject.TextSprite.Draw();

                        //}
                        //if (this.GorgonSprite != null)
                        //{
                          

                        //    this.GorgonSprite.SetScale(xScale, yScale);
                        //    this.GorgonSprite.SetAxis((float)this.GorgonSprite.Image.Width / 2, (float)this.GorgonSprite.Image.Height / 2);
                        //    this.GorgonSprite.Rotation = this.Rotation;

                        //    this.GorgonSprite.SetPosition((float)rectDest.X * xScale + (this.GorgonSprite.Axis.X * xScale),
                        //                                                         (float)rectDest.Y * yScale + (this.GorgonSprite.Axis.Y * yScale));
                        //    this.GorgonSprite.Draw();
                        //}

                    }


                }



                //DRAW SELECTION
                if (showSelection == true)
                {
                    if (this.CoronaObjectParent.isSelected == true)
                    {

                        if (this.CoronaObjectParent.PathFollow != null)
                        {
                            if (this.CoronaObjectParent.PathFollow.isEnabled == true)
                            {
                                this.CoronaObjectParent.PathFollow.drawGorgon(offsetPoint, xScale);
                            }
                        }


                        if (this.CoronaObjectParent.TransformBox != null)
                            this.CoronaObjectParent.TransformBox.drawGorgon(offsetPoint, xScale);

                    }
                }
            }
            catch (Exception Exception)
            {
            }
        }
       
        public Matrix getMatrixForDrawing(Rectangle rectDest, float xScale, float yScale)
        {
            //Si c'est un sprite  -----------
            if (this.type.Equals("SPRITE"))
            {
                if (this.CoronaObjectParent != null)
                {
                    Matrix m = new Matrix();
                    m.RotateAt(this.Rotation, new Point(Convert.ToInt32((rectDest.X + rectDest.Width / 2) * xScale),
                Convert.ToInt32((rectDest.Y + rectDest.Height / 2) * yScale)));

                    m.Scale(xScale, yScale);
                    return m;
                }

            }
            else if (this.type.Equals("IMAGE"))
            {
                if (this.CoronaObjectParent != null)
                {
                    Matrix m = new Matrix();
                    m.RotateAt(this.Rotation, new Point(Convert.ToInt32((rectDest.X + rectDest.Width / 2) * xScale),
                         Convert.ToInt32((rectDest.Y + rectDest.Height / 2) * yScale)));

                    m.Scale(xScale, yScale);
                    return m;
                }
            }
            //Si c'est une figure -----------
            else if (this.type.Equals("FIGURE"))
            {
                if (this.Figure.ShapeType.Equals("RECTANGLE"))
                {

                    Matrix m = new Matrix();
                    m.RotateAt(this.Rotation, new Point(Convert.ToInt32((rectDest.X + rectDest.Width / 2) * xScale),
                        Convert.ToInt32((rectDest.Y + rectDest.Height / 2) * yScale)));

                    m.Scale(xScale, yScale);
                    return m;
                }
                else if (this.Figure.ShapeType.Equals("CIRCLE"))
                {
                    Matrix m = new Matrix();
                    m.RotateAt(this.Rotation, new Point(Convert.ToInt32((rectDest.X + rectDest.Width / 2) * xScale),
                       Convert.ToInt32((rectDest.Y + rectDest.Height / 2) * yScale)));
                    m.Scale(xScale, yScale);
                    return m;
                }
                else if (this.Figure.ShapeType.Equals("LINE"))
                {
                    Matrix m = new Matrix();
                    m.RotateAt(this.Rotation, new Point(Convert.ToInt32((rectDest.X + rectDest.Width / 2) * xScale),
                        Convert.ToInt32((rectDest.Y + rectDest.Height / 2) * yScale)));
                    m.Scale(xScale, yScale);

                    return m;
                }
                else if (this.Figure.ShapeType.Equals("CURVE"))
                {
                    Matrix m = new Matrix();
                    m.RotateAt(this.Rotation, new Point(Convert.ToInt32((rectDest.X + rectDest.Width / 2) * xScale),
                        Convert.ToInt32((rectDest.Y + rectDest.Height / 2) * yScale)));
                    m.Scale(xScale, yScale);

                    return m;
                }
                else if (this.Figure.ShapeType.Equals("TEXT"))
                {
                    Matrix m = new Matrix();
                    m.RotateAt(this.Rotation, new Point(Convert.ToInt32((rectDest.X + rectDest.Width / 2) * xScale),
                         Convert.ToInt32((rectDest.Y + rectDest.Height / 2) * yScale)));

                    m.Scale(xScale, yScale);
                    return m;
                }
                return null;
            }
            return null;
        }

        public void SetScale(float scale)
        {
            int newWidth = Convert.ToInt32(this.InitialRect.Size.Width * scale);
            int newHeight = Convert.ToInt32(this.InitialRect.Size.Height * scale);

            this.surfaceRect.Width = newWidth;
            this.surfaceRect.Height = newHeight;

            this.surfaceRect.X = Convert.ToInt32(this.InitialRect.X * scale);
            this.surfaceRect.Y = Convert.ToInt32(this.InitialRect.Y * scale);
            this.scale = scale;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public DisplayObject cloneInstance(bool keepLocation)
        {
           
            if (this.type.Equals("SPRITE"))
            {
                Point pDest;
                if (keepLocation == true)
                {
                    pDest = new Point(this.SurfaceRect.X,
                                      this.SurfaceRect.Y);
                }
                else
                {
                    pDest = new Point(this.SurfaceRect.X + 20, this.SurfaceRect.Y );
                }

                DisplayObject obj = new DisplayObject(this.SpriteSet, pDest, this.CoronaObjectParent);
                obj.OriginalAssetName = this.OriginalAssetName;
                obj.Name = this.Name;

                obj.setSequence(this.CurrentSequence);
                obj.CurrentFrame = this.CurrentFrame;
                obj.AutoPlay = this.AutoPlay;
                
                return obj;
            }
            else if (this.type.Equals("IMAGE"))
            {
                //Creer une nouvelle Image
                Image newImage = null;
                DisplayObject newDisplayObject = null;

                if (this.GorgonSprite != null)
                {
                    GorgonLibrary.Graphics.Sprite newGorgonSprite = (GorgonLibrary.Graphics.Sprite)this.GorgonSprite.Clone();
                    newDisplayObject = new DisplayObject(newGorgonSprite, new Point(0, 0), this.CoronaObjectParent);
                }
                else if (this.image != null)
                {
                    newImage = new Bitmap(this.image);
                    newDisplayObject = new DisplayObject(newImage, new Point(0, 0), this.CoronaObjectParent);
                }
                else return null;

                
                newDisplayObject.OriginalAssetName = this.OriginalAssetName;
                newDisplayObject.Name = this.Name;
                //Changer la position de l'objet
                 Point pDest;
                 if (keepLocation == true)
                 {
                     pDest = new Point(this.SurfaceRect.X,
                                       this.SurfaceRect.Y);
                 }
                 else
                 {
                     pDest = new Point(this.SurfaceRect.X + 20, this.SurfaceRect.Y);
                 }

                newDisplayObject.surfaceRect = new Rectangle(pDest, this.surfaceRect.Size);
                newDisplayObject.InitialRect = new Rectangle(this.InitialRect.X, this.InitialRect.Y,
                                                        this.InitialRect.Width, this.InitialRect.Height);

                newDisplayObject.ImageFillColor = this.ImageFillColor;

                
                return newDisplayObject;
            }
            else if (this.type.Equals("FIGURE"))
            {
                //Recuperer le clone de la figure
                Figure newFig = this.Figure.CloneInstance(keepLocation);

                DisplayObject newDisplayObject = new DisplayObject(newFig, this.CoronaObjectParent);
                newFig.DisplayObjectParent = newDisplayObject;
                newDisplayObject.OriginalAssetName = this.OriginalAssetName;
                newDisplayObject.Name = this.Name;

                //Copy the gradient color
                newDisplayObject.GradientColor.color1 = this.GradientColor.color1;
                newDisplayObject.GradientColor.color2 = this.GradientColor.color2;
                newDisplayObject.GradientColor.isEnabled = this.GradientColor.isEnabled;
                newDisplayObject.GradientColor.direction = this.GradientColor.direction;

                if (newFig.ShapeType.Equals("TEXT"))
                {
                    Texte text = (Texte)newFig;

                    if (this.GorgonSprite != null)
                    {
                        newDisplayObject.GorgonSprite = (GorgonLibrary.Graphics.Sprite)this.GorgonSprite.Clone();

                    }
                     Point pDest;
                     if (keepLocation == true)
                     {
                         pDest = new Point(text.Position.X,
                                          text.Position.Y);
                     }
                     else
                     {
                         pDest = new Point(text.Position.X + 20,
                                           text.Position.Y);
                     }
                     newDisplayObject.SurfaceRect = new Rectangle(pDest, this.surfaceRect.Size);
                }

                return newDisplayObject;
            }

            return null;

        }

      

        //-----------------Accesseurs--------------------------------------
        public int CurrentFrame {
            get {

                return this.currentFrame; 
            }
            set 
            {
                if (this.type.Equals("SPRITE"))
                {
                    if (this.CurrentSequence == "DEFAULT")
                    {
                        if (value >= this.SpriteSet.Frames.Count)
                            value = 0;
                        else if (value < 0)
                            value = this.SpriteSet.Frames.Count - 1;

                        this.currentFrame = value;

                    }
                    else
                    {
                        CoronaSpriteSetSequence sequence = this.getSequenceByName(this.CurrentSequence);
                        if (sequence != null)
                        {
                            int sequenceFrameCount = sequence.FrameCount;
                          
                            if (value > sequenceFrameCount - 1)
                                value = 0;
                            else if (value < 0)
                                value = sequenceFrameCount - 1;

                            int convertedCurrentFrame = value + sequence.FrameDepart - 1;
                            this.currentFrame = value;


                            if (!(this.SpriteSet.Frames.Count > 0 && this.SpriteSet.Frames.Count > convertedCurrentFrame))
                            {
                                value = sequenceFrameCount - 1;
                                this.currentFrame = value;
                            }
                           
                        }
                        else
                        {
                            this.setSequence("");

                            if (value > this.SpriteSet.Frames.Count)
                                value = 0;
                            else if (value < 0)
                                value = this.SpriteSet.Frames.Count - 1;

                            this.currentFrame = value;

                        }

                    }

                }
            }
        }

        public Rectangle SurfaceRect { get { return this.surfaceRect; } set { this.surfaceRect = value; } }

        public Image Image { get { return this.image; } set { this.image = value; } }

        public String Type { get { return this.type; } set { this.type = value; } }

        public Point LastPos { get { return this.lastPos; } set { this.lastPos = value; } }






        //------------------- MATH HELPER ------------------------------
        public double Angle(double px1, double py1, double px2, double py2)
        {

            // Negate X and Y values
            double pxRes = px2 - px1;

            double pyRes = py2 - py1;
            double angle = 0.0;

            // Calculate the angle
            if (pxRes == 0.0)
            {
                if (pxRes == 0.0)

                    angle = 0.0;
                else if (pyRes > 0.0) angle = System.Math.PI / 2.0;

                else
                    angle = System.Math.PI * 3.0 / 2.0;

            }
            else if (pyRes == 0.0)
            {
                if (pxRes > 0.0)

                    angle = 0.0;

                else
                    angle = System.Math.PI;

            }

            else
            {
                if (pxRes < 0.0)

                    angle = System.Math.Atan(pyRes / pxRes) + System.Math.PI;
                else if (pyRes < 0.0) angle = System.Math.Atan(pyRes / pxRes) + (2 * System.Math.PI);

                else
                    angle = System.Math.Atan(pyRes / pxRes);

            }

            // Convert to degrees
            angle = angle * 180 / System.Math.PI; return angle;



        }

    }
}
