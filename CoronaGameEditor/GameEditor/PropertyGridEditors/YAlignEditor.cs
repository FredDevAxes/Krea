using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace Krea.GameEditor.PropertyGridEditors
{

    public class YAlignEditor : UITypeEditor
    {

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;

        }


        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {
                Krea.CoronaClasses.CoronaGameProject.YScreenAlignment value = (Krea.CoronaClasses.CoronaGameProject.YScreenAlignment)e.Value;
                if (value == CoronaClasses.CoronaGameProject.YScreenAlignment.center)
                    e.Graphics.DrawImage(Properties.Resources.yAlignCenter, e.Bounds);
                else if (value == CoronaClasses.CoronaGameProject.YScreenAlignment.top)
                    e.Graphics.DrawImage(Properties.Resources.yAlignTop, e.Bounds);
                else if (value == CoronaClasses.CoronaGameProject.YScreenAlignment.bottom)
                    e.Graphics.DrawImage(Properties.Resources.yAlignBottom, e.Bounds);
            }
          
        }


    }
}

