using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace Krea.GameEditor.PropertyGridEditors
{

    public class AdsProviderEditor : UITypeEditor
    {

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;

        }


        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {
                Krea.Corona_Classes.CoronaAds.AdsProvider value = (Krea.Corona_Classes.CoronaAds.AdsProvider)e.Value;
                if (value == Krea.Corona_Classes.CoronaAds.AdsProvider.inmobi)
                    e.Graphics.DrawImage(Properties.Resources.inmobi, e.Bounds);
                else if (value == Krea.Corona_Classes.CoronaAds.AdsProvider.inneractive)
                    e.Graphics.DrawImage(Properties.Resources.inneractive, e.Bounds);
                else if (value == Krea.Corona_Classes.CoronaAds.AdsProvider.none)
                    e.Graphics.DrawImage(Properties.Resources.none, e.Bounds);
            }
         
          
        }


    }
}

