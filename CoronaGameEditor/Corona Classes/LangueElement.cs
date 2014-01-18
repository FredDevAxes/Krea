using System;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class LangueElement
    {
        public String Key { set; get; }
        public String Translation {set; get;}

        public LangueElement() { }

        public LangueElement(String _Key, String _Translation) {
            this.Key = _Key;
            this.Translation = _Translation;
        }

        public override string ToString()
        {
            return Key + " => " + Translation;
        }
    }
}
