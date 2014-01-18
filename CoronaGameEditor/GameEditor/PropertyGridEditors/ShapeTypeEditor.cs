using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace Krea.GameEditor.PropertyGridEditors
{

    public class ShapeTypeEditor : UITypeEditor
    {
        public bool Value = false;
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;

        }


        public override void PaintValue(PaintValueEventArgs e)
        {

            if (e.Value != null)
            {
                if (e.Value.ToString().Equals("CIRCLE"))
                    e.Graphics.DrawImage(Properties.Resources.circleIcon, e.Bounds);
                else if (e.Value.ToString().Equals("TEXT"))
                    e.Graphics.DrawImage(Properties.Resources.textIcon, e.Bounds);
                else if (e.Value.ToString().Equals("LINE"))
                    e.Graphics.DrawImage(Properties.Resources.polylineIcon, e.Bounds);
                else if (e.Value.ToString().Equals("CURVE"))
                    e.Graphics.DrawImage(Properties.Resources.curveIcon, e.Bounds);
                else if (e.Value.ToString().Equals("RECTANGLE"))
                    e.Graphics.DrawImage(Properties.Resources.rectangleIcon, e.Bounds);
            }
           

        }


        public override object EditValue(
                ITypeDescriptorContext context,
                IServiceProvider provider,
                object value)
        {


            bool boolValue = (bool)value;
            Value = !boolValue;


            return Value;
        }


    }
}

