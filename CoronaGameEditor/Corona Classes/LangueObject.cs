using System;
using System.Collections.Generic;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class LangueObject 
    {
        public String Langue { get; set; }
        public List<LangueElement> TranslationElement { get; set; }

        public LangueObject() {
            this.Langue = "English";
            TranslationElement = new List<LangueElement>();
        }

        public LangueObject(String _Langue)
        {
            this.Langue = _Langue;
            TranslationElement = new List<LangueElement>();
        }

        public LangueObject(String _Langue, List<LangueElement> _TranslationElement)
        {
            this.Langue = _Langue;
            this.TranslationElement = _TranslationElement;
        }

        public Boolean CheckForDuplicateElement(String _Key) {
            for (int i = 0; i < TranslationElement.Count; i++) {
                if (TranslationElement[i].Key == _Key) return true;
            }
            return false;
        }
        public override string ToString()
        {
            return Langue;
        }
    }
}
