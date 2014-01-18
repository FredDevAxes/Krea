using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace Krea.GameEditor.PropertyGridEditors
{

    public class GeneratorEmissionTypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;

        }


        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {

                Krea.CoronaClasses.CoronaObject.GeneratorEmissionType value = (Krea.CoronaClasses.CoronaObject.GeneratorEmissionType)e.Value;
                if (value == CoronaClasses.CoronaObject.GeneratorEmissionType.POINT)
                    e.Graphics.DrawImage(Properties.Resources.generatorTypePoint, e.Bounds);
                else if (value == CoronaClasses.CoronaObject.GeneratorEmissionType.LINE)
                    e.Graphics.DrawImage(Properties.Resources.generatorTypeLine, e.Bounds);
                else if (value == CoronaClasses.CoronaObject.GeneratorEmissionType.DISC)
                    e.Graphics.DrawImage(Properties.Resources.generatorTypeDisc, e.Bounds);
            }

        }



    }
}

