using System.Drawing;

namespace Krea.GameEditor
{
    public class TargetResolution
    {
        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        public Size Resolution;
        public string TargetDevice;
        //---------------------------------------------------
        //-------------------Constructeurs-----------------------
        //---------------------------------------------------
        public TargetResolution(string targetDevice, Size Resolution)
        {
            this.Resolution = Resolution;
            this.TargetDevice = targetDevice;
        }
        //---------------------------------------------------
        //-------------------Methodes-----------------------
        //---------------------------------------------------

        public override string ToString()
        {
            return this.TargetDevice + ": " + Resolution.Width + "x" + Resolution.Height;
        }
    }
}
