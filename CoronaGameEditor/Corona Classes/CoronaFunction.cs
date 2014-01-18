using System;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaFunction
    {
        public String Name { get; set; }
        public String Code { get; set; }

        public CoronaFunction() { }

        public CoronaFunction(String _Name, String _Code)
        {
            
            Name = _Name;
            Code = _Code;

        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
