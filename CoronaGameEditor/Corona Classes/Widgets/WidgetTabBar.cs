using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using Krea.CoronaClasses;
using System.Drawing.Drawing2D;

namespace Krea.Corona_Classes.Widgets
{
    [Serializable()]
    public class WidgetTabBar : CoronaWidget
    {
        //----------------------------------------------------------
        //--------------------- ATTIBUTS --------------------------
        //----------------------------------------------------------
        public Point Location;
        public Size Size = new Size(320, 480);
        public Image BackgroundImage;
        public GradientColor GradientForTup ;
        public Color BottomFillColor = Color.Black;
        public int BottomFillAlpha = 255;
        public List<TabBarButton> Buttons;
        
        //----------------------------------------------------------
        //--------------------- CONSTRUCTEURS ----------------------
        //----------------------------------------------------------
        public WidgetTabBar(string name,CoronaLayer layerParent)
            :base(name,WidgetType.tabBar,layerParent)
        {
            Buttons = new List<TabBarButton>();
            GradientForTup = new GradientColor();
            GradientForTup.isEnabled = false;
        }

        //----------------------------------------------------------
        //--------------------- METHODES --------------------------
        //----------------------------------------------------------
        public override void Dessine(Graphics g, Point offsetPoint, float XScale, float YScale)
        {
            Matrix m = new Matrix();
            m.Scale(XScale, YScale);
            g.Transform = m;
           
            if (this.GradientForTup.isEnabled== true)
            {

                LinearGradientBrush br = null;

                Rectangle rectDest = new Rectangle(new Point(this.Location.X + offsetPoint.X, this.Location.Y + offsetPoint.Y), this.Size);
                br = this.GradientForTup.getBrushForDrawing(rectDest,255);
                g.FillRectangle(br, rectDest);


            }
            else
            {
            }

            SolidBrush brBottom = new SolidBrush(Color.FromArgb(this.BottomFillAlpha, this.BottomFillColor));
            g.FillRectangle(brBottom, new Rectangle(new Point(this.Location.X + offsetPoint.X, this.Location.Y + this.Size.Height / 2 + offsetPoint.Y), new Size(this.Size.Width, this.Size.Height / 2)));
            
            

           /* if (this.backgroundImage != null)
            {
                g.DrawImage(this.backgroundImage, new Rectangle(new Point(this.location.X+offsetPoint.X,this.location.Y+offsetPoint.Y), this.size));
            }
            else
            {
                

                
            }*/
        }

      
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //--------------------- SUB_CLASS TabBarButton -----------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable()]
        public class TabBarButton
        {
            //----------------------------------------------------------
            //--------------------- ATTIBUTS --------------------------
            //----------------------------------------------------------
            public string Id;
            public Image ImagePressedState;
            public Image ImageUnPressedState;
            public string Label;
            public string FontName;
            public int FontSize = 10;
            public Color LabelColor = Color.Black;
            public int LabelAlpha = 255;
            public int CornerRadius = 4;
            public bool Selected = false;
            //----------------------------------------------------------
            //--------------------- CONSTRUCTEUR -----------------------
            //----------------------------------------------------------

            public TabBarButton(string id, string label)
            {
                this.Id = id;
                this.Label = label;

            }

            public override string ToString()
            {
                return this.Id;
            }
        }
    }
}
