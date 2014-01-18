using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace Krea.GameEditor.PropertyGridEditors
{

    public class GradientDirectionEditor : UITypeEditor
    {

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;

        }


        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {
                Krea.Corona_Classes.GradientColor.Direction value = (Krea.Corona_Classes.GradientColor.Direction)e.Value;
                if (value == Corona_Classes.GradientColor.Direction.down)
                    e.Graphics.DrawImage(Properties.Resources.flecheBasIcon, e.Bounds);
                else if (value == Corona_Classes.GradientColor.Direction.up)
                    e.Graphics.DrawImage(Properties.Resources.flecheHautIcon, e.Bounds);
                else if (value == Corona_Classes.GradientColor.Direction.right)
                    e.Graphics.DrawImage(Properties.Resources.flecheDroiteIcon, e.Bounds);
                else if (value == Corona_Classes.GradientColor.Direction.left)
                    e.Graphics.DrawImage(Properties.Resources.flecheGaucheIcon, e.Bounds);
            }
        }


    }
}

