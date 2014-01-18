using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.GameEditor.CodeEditor.APIElements
{
    public class APICategory
    {
        //----------------------------------------------------------
        //--------------------- ATTRIBUTS --------------------------
        //----------------------------------------------------------

        public string name;
        public List<APICategory> SubCategories;
        public List<APIItem> Items;
        //----------------------------------------------------------
        //--------------------- CONSTRUCTEURS ----------------------
        //----------------------------------------------------------
        public APICategory(string name)
        {
            this.name = name;
            this.SubCategories = new List<APICategory>();
            this.Items = new List<APIItem>();

        }

        //----------------------------------------------------------
        //--------------------- METHODE ----------------------------
        //----------------------------------------------------------
    }
}
