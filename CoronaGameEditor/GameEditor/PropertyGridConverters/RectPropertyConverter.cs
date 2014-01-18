using System.ComponentModel;
using Krea.CoronaClasses;
using Krea.CGE_Figures;
using System.Drawing;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
  [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The shape representing the Corona Display Object")]
    public class RectPropertyConverter : FigurePropertyConverter
    {
        CoronaObject objSelected;
        Rect rect;
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public RectPropertyConverter() { }
        public RectPropertyConverter(CoronaObject obj, Form1 MainForm) :
            base(obj,MainForm)
        {
            objSelected = obj;
            rect = (Rect) obj.DisplayObject.Figure;
        }
        public CoronaObject GetObjectSelected()
        {
            return this.objSelected;
        }
         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
         [Category("SHAPE PROPERTIES")]
        [DescriptionAttribute("Is a rounded rectangle.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsRounded
        {
         get
            {
                return rect.isRounded;
            }

            set
            {

                rect.isRounded = value;
            }
         }

        [Category("SHAPE PROPERTIES")]
        [DescriptionAttribute("The corners radius of the rounded rectangle.")]
        public int CornerRadius
        {
         get
            {
                return rect.cornerRadius;
            }

            set
            {

                rect.cornerRadius = value;
            }
         }

        [Category("GRADIENT")]
        [DescriptionAttribute("The gradient direction of the object.")]
        [Editor(typeof(GradientDirectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Krea.Corona_Classes.GradientColor.Direction Direction
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.direction;
            }

            set
            {

                this.objSelected.DisplayObject.GradientColor.direction = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("Is the gradient is enabled for this object.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool GradientEnabled
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.isEnabled;
            }

            set
            {

                this.objSelected.DisplayObject.GradientColor.isEnabled = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The first color of the gradient.")]
        public Color Color1
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.color1;
            }

            set
            {


                this.objSelected.DisplayObject.GradientColor.color1 = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The second color of the gradient.")]
        public Color Color2
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.color2;
            }

            set
            {

                this.objSelected.DisplayObject.GradientColor.color2 = value;
            }

        }

    }
}
