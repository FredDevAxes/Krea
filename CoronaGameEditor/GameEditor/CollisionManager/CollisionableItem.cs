using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Krea.GameEditor.CollisionManager
{
    public class CollisionableItem
    {
        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        [ObfuscationAttribute(Exclude = true)]
        public enum CollisionableObjectType
        {
            DisplayObject = 1,
            TilesMap = 2,
          
        }

        public object Tag;
        public CollisionableObjectType ObjectType;
        public List<int> CollisionWithCategorieBits;
        public int CategorieBit;

        //---------------------------------------------------
        //-------------------Constructeurs-----------------------
        //---------------------------------------------------
        public CollisionableItem(CollisionableObjectType ObjectType, object tag)
        {
            this.ObjectType = ObjectType;
            this.Tag = tag;
            CollisionWithCategorieBits = new List<int>();
        }


        //---------------------------------------------------
        //-------------------Methodes-----------------------
        //---------------------------------------------------

    }
}
