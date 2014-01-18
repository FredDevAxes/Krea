using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.GameEditor.CollisionManager
{
    [Serializable()]
    public class CollisionFilterGroup
    {
        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        public string GroupName;
        public List<int> CollisionWithCategorieBits;
        public int CategorieBit;
        public int IndexGroup;
        //---------------------------------------------------
         //---------------------------------------------------
        //-------------------Constructeurs-----------------------
        //---------------------------------------------------
        public CollisionFilterGroup(string name, int indexGroup)
        {
            this.GroupName = name;
            this.IndexGroup = indexGroup;
            CollisionWithCategorieBits = new List<int>();
        }

        public int getMaskBits()
        {
            int sum = 0;
            for (int i = 0; i < CollisionWithCategorieBits.Count; i++)
            {
                sum += CollisionWithCategorieBits[i];
            }
            return sum;
        }

        public override string ToString()
        {
            return this.GroupName;
        }
    }
}
