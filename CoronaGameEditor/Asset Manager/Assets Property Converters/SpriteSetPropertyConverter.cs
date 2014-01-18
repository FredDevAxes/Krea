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
    public class SpriteSetPropertyConverter
    {
         CoronaSpriteSet set;

         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public SpriteSetPropertyConverter() { }
        public SpriteSetPropertyConverter(CoronaSpriteSet set, AssetManagerForm MainForm)
        {
            this.set = set;
        }

         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
         [Category("Name")]
        [DescriptionAttribute("The name of the Spriteset.")]
        public string Name
        {
         get
            {
                return set.Name;
            }

            set
            {

                set.Name = value;
            }
         }

       
    }
}
