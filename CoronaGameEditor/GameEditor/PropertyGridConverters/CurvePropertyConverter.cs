using System.ComponentModel;
using Krea.CoronaClasses;
using Krea.CGE_Figures;
using System.Reflection;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
  [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The shape representing the Corona Display Object")]
    public class CurvePropertyConverter : FigurePropertyConverter
    {
        CoronaObject objSelected;
        CourbeBezier curve;
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public CurvePropertyConverter() { }
        public CurvePropertyConverter(CoronaObject obj, Form1 MainForm) :
            base(obj,MainForm)
        {
            objSelected = obj;
            curve = (CourbeBezier)obj.DisplayObject.Figure;
        }
        public CoronaObject GetObjectSelected()
        {
            return this.objSelected;
        }
         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
   /*      [Category("ACCURACY")]
        [DescriptionAttribute("An accuracy ratio for the curve.")]
        public Double AccuracyRatio
        {
         get
            {
                return curve.Ratio;
            }

            set
            {

                curve.Ratio = value;

            }
         }*/


        

    }
}
