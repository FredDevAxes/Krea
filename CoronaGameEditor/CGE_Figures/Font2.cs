using System.Drawing;
using System;
using Krea.GameEditor.FontManager;

//Classe permettant de serializer la police d'un texte (Font ayant du mal a se serializer en XML correctement)

namespace Krea.CGE_Figures
{
    [Serializable]
    public class Font2

    {
        String familyName;
        float size;
        FontStyle style;
        public FontItem FontItem;
        public Font2() { }

        public Font2(String familyName,float size,FontStyle style)
        {
            this.familyName = familyName;
            this.size = size;
            this.style = style;
            style = new FontStyle();

        }

        public String FamilyName
        {
            get { return this.familyName; }
            set { this.familyName = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public FontStyle Style
        {
            get { return style; }
            set { style = value; }
        }

        public string getFontNameForAndroid()
        {
            return this.familyName.Replace(" ", "").ToLower() ;
        }
        public string getFontNameForIPhone()
        {
            return "";
        }

        public string GenerateCode(string text, Int32 x, Int32 y, Int32 size)
        {
            string androidName = this.getFontNameForAndroid();
            string iPhoneName = this.getFontNameForIPhone();

            string resultCode = "\nif system.getInfo(\"platformName\") == \"Android\" \n" +
                   "then Font =\"" + androidName + "\" \n" +
                   "else Font = \"" + iPhoneName + "\" \n" +
                   "end\n\n" +
                   "display.newText( \"" + text + "\", " + x.ToString() + ", " + y.ToString() + ", Font, " + size.ToString() + " )";

            return resultCode;
        }

    
    }
}
