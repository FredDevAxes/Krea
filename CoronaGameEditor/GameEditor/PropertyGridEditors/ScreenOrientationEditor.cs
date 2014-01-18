using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using Krea.CoronaClasses;

namespace Krea.GameEditor.PropertyGridEditors
{

    public class ScreenOrientationEditor : UITypeEditor
    {

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;

        }


        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {
                CoronaGameProject.OrientationScreen value = (CoronaGameProject.OrientationScreen)e.Value;
                if (value == CoronaGameProject.OrientationScreen.Landscape)
                    e.Graphics.DrawImage(Properties.Resources.paysageIcon, e.Bounds);
                else if (value == CoronaGameProject.OrientationScreen.Portrait)
                    e.Graphics.DrawImage(Properties.Resources.portraitIcon, e.Bounds);
            }
          
        }


    }
}

