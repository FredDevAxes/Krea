using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.Debugger
{
    public class Local
    {
    

        public String Name {get; set;}
        public String Operator {get; set;}
        public String Type {get; set;}
        public String Value {get; set;}

        public Local() 
        {
        }
        public Local(String _Name, String _Operator, String _Type, String _Value)
        {
            this.Name                = _Name.Replace("\t","");
            this.Operator            = _Operator;
            this.Type                = _Type;
            this.Value               = _Value;
        }


    }
}
