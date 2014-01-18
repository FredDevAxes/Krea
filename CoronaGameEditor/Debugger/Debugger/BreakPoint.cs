using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.GameEditor.Debugger
{
    public class BreakPoint
    {
        public String File { get; set; }
        public String LineNumber { get; set; }
        public BreakPoint(String file, String lineNumber)
        {
            File = file;
            LineNumber = lineNumber;
        }

        public override string ToString()
        {
            return "In " + File + " Line " + LineNumber;
        }
    }
}
