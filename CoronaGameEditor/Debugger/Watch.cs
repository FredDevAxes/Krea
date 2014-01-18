using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.GameEditor.Debugger
{
    public class Watch
    {
        public String Number { get; set; }
        public String Expression { get; set; }

        public Watch(String number, String expression)
        {
            this.Number = number;
            this.Expression = expression;
        }
        public override string ToString()
        {
            return "Watch N°" + Number + "Expression : " + Expression;
        }
    }
}
