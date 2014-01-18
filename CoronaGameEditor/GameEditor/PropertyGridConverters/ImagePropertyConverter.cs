using System;
using Krea.CoronaClasses;
using System.ComponentModel;
using Krea.CGE_Figures;
using System.Drawing;
using System.Reflection;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The shape representing the Corona Display Object")]
    public class ImagePropertyConverter : ObjectPropertyConverter
    {

        CoronaObject obj;
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public ImagePropertyConverter() { }
        public ImagePropertyConverter(CoronaObject obj, Form1 MainForm) :
            base(obj,MainForm)
        {
            this.obj = obj;
        }

         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------

        public CoronaObject GetObjectSelected()
        {
            return this.obj;
        }
        [Category("IMAGE COLOR")]
        [DescriptionAttribute("The fill color of the image.")]
        public Color FillColor
        {
            get { return this.obj.DisplayObject.ImageFillColor; }
            set {

                this.obj.DisplayObject.ImageFillColor = value;
            }
        }


        /*[Category("IMAGE DISPLAY")]
        [DescriptionAttribute("The alpha fill color of the image. Values between 0.0 and 1.0")]
        public int AlphaFillColor
        {
            get { return this.obj.DisplayObject.ImageFillColorAlpha; }
            set
            {
                if (value > 255)
                    value = 1;
                else if (value < 0)
                    value = 0;

                
                this.obj.DisplayObject.ImageFillColorAlpha = value;
            }
        }*/

    }


}
