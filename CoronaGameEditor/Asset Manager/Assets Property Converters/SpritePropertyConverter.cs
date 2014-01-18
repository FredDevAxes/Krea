using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Krea.CoronaClasses;
using System.Drawing;
using System.Reflection;

namespace Krea.Asset_Manager.Assets_Property_Converters
{
    [ObfuscationAttribute(Exclude = true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SpritePropertyConverter
    {
        DisplayObject objSelected;

         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public SpritePropertyConverter() { }
        public SpritePropertyConverter(DisplayObject obj, AssetManagerForm MainForm)
        {
            objSelected = obj;
        }

         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
         [Category("Name")]
        [DescriptionAttribute("The name of the sprite.")]
        public string Name
        {
         get
            {
                return objSelected.Name;
            }

            set
            {

                objSelected.Name = value;
            }
         }

       
    }
}
