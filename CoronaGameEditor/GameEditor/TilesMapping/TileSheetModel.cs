using System.Collections.Generic;

namespace Krea.GameEditor.TilesMapping
{
    public class TileSheetModel
    {
        //---------------------------------------------------
        //-------------------Attributes--------------------
        //---------------------------------------------------
        public List<TileModel> TileModels;
        public bool IsTextureSheet = false;
        public string Name;
        public string filename;
        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public TileSheetModel(string name,string filename, bool isTextureSheet)
        {
            this.TileModels = new List<TileModel>();
            this.IsTextureSheet = isTextureSheet;
            this.Name = name;
            this.filename = filename;
        }

        public override string ToString()
        {
            return this.Name.Replace(".png", "");
        }
    }
}
