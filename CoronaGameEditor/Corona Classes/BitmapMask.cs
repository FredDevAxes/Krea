using System;
using System.Drawing;

namespace Krea.Corona_Classes
{
    [Serializable()]
    public class BitmapMask
    {
        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        public Image MaskImage;
        public bool IsMaskEnabled = false;
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public BitmapMask(Image maskImage)
        {
            this.MaskImage = maskImage;
            if (this.MaskImage != null)
                this.IsMaskEnabled = true;
        }
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
    }
}
