using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace Krea.GameEditor.PropertyGridEditors
{

    public class XAlignEditor : UITypeEditor
    {

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;

        }


        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {
                Krea.CoronaClasses.CoronaGameProject.XScreenAlignment value = (Krea.CoronaClasses.CoronaGameProject.XScreenAlignment)e.Value;
                if (value == CoronaClasses.CoronaGameProject.XScreenAlignment.center)
                    e.Graphics.DrawImage(Properties.Resources.xAlignCenter, e.Bounds);
                else if (value == CoronaClasses.CoronaGameProject.XScreenAlignment.left)
                    e.Graphics.DrawImage(Properties.Resources.xAlignLeft, e.Bounds);
                else if (value == CoronaClasses.CoronaGameProject.XScreenAlignment.right)
                    e.Graphics.DrawImage(Properties.Resources.xAlignRight, e.Bounds);
            }
          
        }


    }
}

