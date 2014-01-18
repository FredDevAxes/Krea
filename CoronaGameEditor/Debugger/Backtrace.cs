using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.Debugger
{
    public class Backtrace
    {
        public Int32 Number;
        public String Kind;
        public String File;
        public Int32 Line;

        public Backtrace() { }

        public Backtrace(Int32 _Number,String _Kind,String _File,Int32 _Line) {
            this.Number= _Number;
            this.Kind = _Kind;
            this.File=_File;
            this.Line = _Line;
        }

        public String ToString() {
            return "[ " + Number + "]" + " " + Kind + " at " + File + ":" + Line;
        }
    }
}
