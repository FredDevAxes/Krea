using System;
using System.Text;
using System.Drawing;
using Krea.CoronaClasses;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;


namespace Krea.Corona_Classes.Controls
{
    [Serializable()]
    [ObfuscationAttribute(Exclude = true)]
    public class JoystickControl:CoronaControl
    {
        //---------------------------------------------------
        //-------------------Attributs--------------------
        //---------------------------------------------------
        public CoronaLayer layerParent;
        public string joystickName;
        public Image outerImage;
        public Image innerImage;

        public int outerRadius;
        public float outerAlpha;
        public int innerRadius;
        public float innerAlpha;
        public int ghost;

        public Point joystickLocation;
        public float joystickAlpha;
        public bool joystickFade;
        public int joystickFadeDelay;

        

        [NonSerialized()]
        public bool isSelected = false;
        [NonSerialized()]
        public GorgonLibrary.Graphics.Sprite InnerGorgonSprite;
        [NonSerialized()]
        public GorgonLibrary.Graphics.Sprite OuterGorgonSprite;
        [NonSerialized()]
        public Point lastPos;

        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public JoystickControl(CoronaLayer layerParent)
            : base(ControlType.joystick, layerParent.Name + "_joystick"+layerParent.Controls.Count)
        {
            this.joystickName = layerParent.Name + "_joystick" + layerParent.Controls.Count;
            this.layerParent = layerParent;

            this.outerImage = null;
            this.innerImage = null;

            this.outerRadius = 30;
            this.outerAlpha = 1;
            this.innerRadius = 15;
            this.innerAlpha = 1;

            this.joystickLocation = new Point(0,0);
            this.joystickAlpha = 1;
            this.joystickFade = true;
            this.joystickFadeDelay = 2000;
            this.ghost = 150;
        }

        //---------------------------------------------------
        //-------------------Methodes--------------------
        //---------------------------------------------------

        public void setSelected(bool isSelected)
        {
            this.isSelected = isSelected;
        }

        public override void DrawGorgon(Point offsetPoint, float worldScale)
        {
            if (this.OuterGorgonSprite != null)
            {
                this.OuterGorgonSprite.Color = Color.FromArgb((int)(this.outerAlpha * 255.0f), Color.White);


                Point pDest = new Point(this.joystickLocation.X + offsetPoint.X,
                        this.joystickLocation.Y + offsetPoint.Y);

                float imgScaleX = (float)this.outerRadius * 2 / (float)this.OuterGorgonSprite.Image.Width;
                float imgScaleY = (float)this.outerRadius * 2 / (float)this.OuterGorgonSprite.Image.Height;

                float finalXScale = worldScale * imgScaleX;
                float finalYScale = worldScale * imgScaleY;
                this.OuterGorgonSprite.SetScale(finalXScale, finalYScale);



                this.OuterGorgonSprite.SetPosition((float)pDest.X * worldScale + (this.OuterGorgonSprite.Axis.X * finalXScale),
                                              (float)pDest.Y * worldScale + (this.OuterGorgonSprite.Axis.Y * finalYScale));
                this.OuterGorgonSprite.Draw();

            }
            else
            {
                Point pDest = new Point(this.joystickLocation.X + offsetPoint.X,
                    this.joystickLocation.Y + offsetPoint.Y);

                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, this.outerRadius,
                        Color.FromArgb((int)(this.outerAlpha * 255.0f), Color.Gray), worldScale, false);
            }

            //-----
            if (this.InnerGorgonSprite != null)
            {
                this.InnerGorgonSprite.Color = Color.FromArgb((int)(this.innerAlpha * 255.0f), Color.White);


                Point pDest = new Point(this.joystickLocation.X + this.outerRadius + offsetPoint.X - this.innerRadius,
                       this.joystickLocation.Y + offsetPoint.Y + this.outerRadius - this.innerRadius);

         

                float imgScaleX = (float)innerRadius * 2 / (float)this.InnerGorgonSprite.Image.Width;
                float imgScaleY = (float)innerRadius * 2 / (float)this.InnerGorgonSprite.Image.Height;

                float finalXScale = worldScale * imgScaleX;
                float finalYScale = worldScale * imgScaleY;
                this.InnerGorgonSprite.SetScale(finalXScale, finalYScale);



                this.InnerGorgonSprite.SetPosition((float)pDest.X * worldScale + (this.InnerGorgonSprite.Axis.X * finalXScale),
                                              (float)pDest.Y * worldScale + (this.InnerGorgonSprite.Axis.Y * finalYScale));
                this.InnerGorgonSprite.Draw();

            }
            else
            {
                Point pDest = new Point(this.joystickLocation.X + this.outerRadius + offsetPoint.X - this.innerRadius,
                       this.joystickLocation.Y + offsetPoint.Y + this.outerRadius - this.innerRadius);

                GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, this.innerRadius,
                        Color.FromArgb((int)(this.outerAlpha * 255.0f), Color.Black), worldScale, false);
            }

             
        }

        public override void Dessine(Graphics g, Point offsetPoint, float xScale, float yScale)
        {
            Matrix m = new Matrix();
            m.Scale(xScale, yScale);
            g.Transform = m;

           
            if (this.outerImage != null)
            {
                float[][] matrixItems ={ 
                       new float[] {1, 0, 0, 0, 0},
                       new float[] {0, 1, 0, 0, 0},
                       new float[] {0, 0, 1, 0, 0},
                       new float[] {0, 0, 0, this.outerAlpha, 0}, 
                       new float[] {0, 0, 0, 0, 1}};
                ColorMatrix colorMatrix = new ColorMatrix();

                // Create an ImageAttributes object and set its color matrix.
                ImageAttributes imageAtt = new ImageAttributes();
                imageAtt.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

               
                Point pDest = new Point(this.joystickLocation.X + offsetPoint.X ,
                     this.joystickLocation.Y + offsetPoint.Y );


                g.DrawImage(
                   this.outerImage,
                   new Rectangle(pDest.X
                        , pDest.Y, this.outerRadius * 2, this.outerRadius * 2),  // destination rectangle
                   0.0f,                          // source rectangle x 
                   0.0f,                          // source rectangle y
                  this.outerImage.Width,                        // source rectangle width
                   this.outerImage.Height,                       // source rectangle height
                   GraphicsUnit.Pixel,
                   imageAtt);
            }
            else
            {
                SolidBrush br = new SolidBrush(Color.FromArgb(Convert.ToInt32(255 * this.outerAlpha), Color.Gray));
                Point pDest = new Point(this.joystickLocation.X + offsetPoint.X,
                 this.joystickLocation.Y + offsetPoint.Y);

                g.FillEllipse(br, new Rectangle(pDest, new Size(this.outerRadius * 2, this.outerRadius * 2)));
            }

            if (this.innerImage != null)
            {
               /* float[][] matrixItems ={ 
                       new float[] {1, 0, 0, 0, 0},
                       new float[] {0, 1, 0, 0, 0},
                       new float[] {0, 0, 1, 0, 0},
                       new float[] {0, 0, 0, this.innerAlpha, 0}, 
                       new float[] {0, 0, 0, 0, 1}};
                ColorMatrix colorMatrix = new ColorMatrix();

                // Create an ImageAttributes object and set its color matrix.
                ImageAttributes imageAtt = new ImageAttributes();
                imageAtt.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);


                Point pDest = new Point(this.joystickLocation.X + this.outerRadius + offsetPoint.X - this.innerRadius,
                   this.joystickLocation.Y + offsetPoint.Y + this.outerRadius - this.innerRadius);

                g.DrawImage(
                   this.innerImage,
                   new Rectangle(pDest.X,
                         pDest.Y, this.innerRadius * 2, this.innerRadius * 2),  // destination rectangle
                   0.0f,                          // source rectangle x 
                   0.0f,                          // source rectangle y
                  this.innerRadius * 2,                        // source rectangle width
                   this.innerRadius *2,                       // source rectangle height
                   GraphicsUnit.Pixel,
                   imageAtt);*/


                Point pDest = new Point(this.joystickLocation.X + this.outerRadius + offsetPoint.X - this.innerRadius,
                   this.joystickLocation.Y + offsetPoint.Y + this.outerRadius - this.innerRadius);

                g.DrawImage(
                   this.innerImage,
                   new Rectangle(pDest.X,
                         pDest.Y, this.innerRadius * 2, this.innerRadius * 2));  // destination rectangle
            }
            else
            {
                SolidBrush br = new SolidBrush(Color.FromArgb(Convert.ToInt32(255 * this.innerAlpha), Color.Black));
                Point pDest = new Point(this.joystickLocation.X + offsetPoint.X + this.outerRadius - this.innerRadius,
                  this.joystickLocation.Y + offsetPoint.Y + this.outerRadius - this.innerRadius);

                g.FillEllipse(br, new Rectangle(pDest, new Size(this.innerRadius * 2, this.innerRadius * 2)));
            }

            g.ResetTransform();
        }


        public string buildLuaCode(float XRatio,float YRatio)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t" + this.layerParent.Name + "." + this.joystickName + " = joystickClass.newJoystick{");

            float moyenneRatio = (XRatio + YRatio) / 2;

            if(this.outerImage != null)
                sb.AppendLine("\t\touterImage = \"" + this.joystickName + "Outer.png\",");
            
            sb.AppendLine("\t\touterRadius = " + ((float)this.outerRadius * moyenneRatio).ToString().Replace(",", ".") + ",");

            sb.AppendLine("\t\touterAlpha = " + this.outerAlpha.ToString().Replace(",", ".") + ",");


            if(this.innerImage != null)
                sb.AppendLine("\t\tinnerImage = \"" + this.joystickName + "Inner.png\",");
            
            sb.AppendLine("\t\tinnerRadius = " + ((float)this.innerRadius * moyenneRatio).ToString().Replace(",", ".") + ",");

            sb.AppendLine("\t\tinnerAlpha = " + this.innerAlpha.ToString().Replace(",", ".") + ",");


            sb.AppendLine("\t\tposition_x = " + ((float)this.joystickLocation.X  * XRatio).ToString().Replace(",", ".")+ ",");
            sb.AppendLine("\t\tposition_y = " + ((float)this.joystickLocation.Y * YRatio).ToString().Replace(",", ".") + ",");

            sb.AppendLine("\t\tghost = " + this.ghost + ",");

            sb.AppendLine("\t\tjoystickAlpha = " + this.joystickAlpha.ToString().Replace(",", ".") + ",");
            sb.AppendLine("\t\tjoystickFade = " + this.joystickFade.ToString().ToLower() + ",");

            sb.AppendLine("\t\tjoystickFadeDelay = " + this.joystickFadeDelay + ",");

            sb.AppendLine("\t\tonMove = " + this.joystickName + "_onMove }");


            return sb.ToString();
        }

        public override CoronaControl Clone(CoronaLayer layerDest,bool incrementName)
        {
            JoystickControl newControl = new JoystickControl(layerDest);
            newControl.isEnabled = this.isEnabled;
            if(incrementName ==true)
            {
                int res = -1;
                if (int.TryParse(this.joystickName.Substring(this.joystickName.Length - 1), out res) == true)
                    newControl.joystickName = joystickName.Substring(0, joystickName.Length - 1) + (res + 1).ToString();
                else
                    newControl.joystickName = this.joystickName + 1;

                newControl.ControlName = this.ControlName;
            }
            else
            {
                newControl.joystickName = this.joystickName;
                newControl.ControlName = this.ControlName;
            }

            newControl.ghost = this.ghost;
            newControl.innerAlpha = this.innerAlpha;
            newControl.outerAlpha = this.outerAlpha;
            newControl.outerRadius = this.outerRadius;
            newControl.innerRadius = this.innerRadius;

            if(this.innerImage != null)
                 newControl.innerImage = new Bitmap(this.innerImage);

            if (this.outerImage != null)
                newControl.outerImage = new Bitmap(this.outerImage);

            newControl.joystickLocation = new Point(this.joystickLocation.X,this.joystickLocation.Y);
            newControl.joystickAlpha = this.joystickAlpha;
            newControl.joystickFade = this.joystickFade;
            newControl.joystickFadeDelay = this.joystickFadeDelay;

            return newControl;
        }
       
    }
}
