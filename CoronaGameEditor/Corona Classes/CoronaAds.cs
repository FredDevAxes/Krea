using System;
using System.Drawing;
using System.Reflection;


namespace Krea.Corona_Classes
{
    [Serializable()]

    public class CoronaAds
    {
         [Flags]
         [ObfuscationAttribute(Exclude = true)]
        public enum BannerTypes
        {
            
            banner320x48 = 1,
            banner300x250 = 2,
            banner728x90 = 3,
            banner468x60 = 4,
            banner120x600 = 5,
            banner = 6,
            fullscreen = 7,
            text = 8,
        }
         [Flags]
         [ObfuscationAttribute(Exclude = true)]
        public enum AdsProvider
        {
            none = 1,
            inmobi = 2,
            inneractive = 3,
        }

         public bool isActive;
         public BannerTypes bannerType;
         public Point location;
         public int interval;
         public bool isTestMode;
         public AdsProvider provider;
         public Size size;
         public string appId;

        public CoronaAds()
        {
            isActive = false;
            bannerType = BannerTypes.banner320x48;
            this.size = new Size(320, 48);
            location = new Point(0, 0);
            isTestMode = true;
            provider = AdsProvider.none;
            interval = 10;
            appId = "";
        }

       
    }
}
