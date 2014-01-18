using System;

namespace Krea.GameEditor.TilesMapping
{
    [Serializable()]
    public class TileProperties
    {

        //---------------------------------------------------
        //-------------------Attributes----------------------
        //---------------------------------------------------
        public String Type;
        public bool IsBreackable;

        //---------------------------------------------------
        //-------------------Constructors--------------------
        //---------------------------------------------------
        public TileProperties(String type, bool isBreackable)
        {
            this.Type = type;
            this.IsBreackable = isBreackable;
        }
        //---------------------------------------------------
        //-------------------Methods-------------------------
        //---------------------------------------------------
    }
}
