using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Krea.GameEditor.TilesMapping
{
     [ObfuscationAttribute(Exclude = true)]
    public class JSONTileSequence
    {
        public string Name;
        public int StartAtFrame;
        public int FrameCount;
        public int Iteration;
        public int Lenght;

        public JSONTileSequence(string name, int startAtFrame, int frameCount, int lenght,int iteration)
        {
            this.Name = name;
            this.Iteration = iteration;
            this.StartAtFrame = startAtFrame;
            this.Lenght = lenght;
            this.FrameCount = frameCount;
        }

    }
}
