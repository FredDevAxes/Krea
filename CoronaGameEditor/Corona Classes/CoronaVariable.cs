using System;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaVariable
    {
        public String Type { get; set; }
        public Boolean isLocal { get; set; }
        public String Name { get; set; }
        public String InitValue { get; set; }

        public CoronaVariable() { }

        public CoronaVariable(String _Type, Boolean _isLocal, String _Name, String _InitValue) {
            Type = _Type;
            isLocal = _isLocal;
            Name = _Name;
            InitValue = _InitValue;
        
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
