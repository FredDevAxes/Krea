using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Krea.CoronaClasses;
using System.Drawing;
using Krea.Corona_Classes;
using System.Reflection;

namespace Krea.Asset_Manager.Assets_Property_Converters
{
    [ObfuscationAttribute(Exclude = true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SnippetPropertyConverter
    {
            Snippet objSelected;
            AssetManagerForm mainForm;

         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public SnippetPropertyConverter() { }
        public SnippetPropertyConverter(Snippet obj, AssetManagerForm mainForm)
        {
            objSelected = obj;
            this.mainForm = mainForm;
        }

         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
        [DescriptionAttribute("The name of the snippet.")]
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

        [DescriptionAttribute("The category of the snippet.")]
        public string Category
        {
            get
            {
                return objSelected.Category;
            }

            set
            {

                objSelected.Category = value;
            }
        }

        [DescriptionAttribute("The Author of the snippet.")]
        public string Author
        {
            get
            {
                return objSelected.Author;
            }

            set
            {

                objSelected.Author = value;
            }
        }

        [DescriptionAttribute("The Version of the snippet.")]
        public float Version
        {
            get
            {
                return objSelected.Version;
            }

            set
            {

                objSelected.Version = value;
            }
        }
        [DescriptionAttribute("The Description of the snippet.")]
        public string Description
        {
            get
            {
                return objSelected.Description;
            }

            set
            {

                objSelected.Description = value;
            }
        }
    }
}
