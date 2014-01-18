using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Krea.GameEditor.TilesMapping
{
    [Serializable()]
    public class TileEvent
    {
        [ObfuscationAttribute(Exclude = true)]
        public enum TileEventType
        {
            preCollision = 1,
            collision = 2,
            postCollision = 3,
            touch = 4
        }

        public TileEventType Type;
        public string Name;

        public TileEvent(string name, TileEventType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
