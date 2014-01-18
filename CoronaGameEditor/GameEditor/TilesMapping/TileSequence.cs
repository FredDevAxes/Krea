using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.GameEditor.TilesMapping
{
    [Serializable()]
    public class TileSequence
    {
        public string Name {get;set;}
        public List<TileModel> Frames;
        public int Iteration {get;set;}
        public int Lenght { get; set; }

        public TileSequence(string name, List<TileModel> frames)
        {
            this.Name = name;
            this.Frames = frames;
            this.Lenght = 1000;
            this.Iteration = 0;
        }

    }
}
