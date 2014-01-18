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
    public class SpriteSheetPropertyConverter
    {
         CoronaSpriteSheet sheet;
         SpriteSheetManagerPanel sheetManager;
         AssetManagerForm MainForm;
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public SpriteSheetPropertyConverter() { }
        public SpriteSheetPropertyConverter(CoronaSpriteSheet sheet, AssetManagerForm MainForm,SpriteSheetManagerPanel sheetManager)
        {
            this.sheet = sheet;
            this.MainForm = MainForm;
            this.sheetManager = sheetManager;
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
                return sheet.Name;
            }

            set
            {

                sheet.Name = value;
            }
         }

         [Category("Size")]
         [DescriptionAttribute("The image width of the sprite sheet."), ReadOnly(true)]
         public int Width
         {
             get
             {
                 return sheetManager.imagePictBx.Width;
             }

             set
             {

                 sheetManager.imagePictBx.Width = value;
             }
         }

         [Category("Size")]
         [DescriptionAttribute("The image height of the sprite sheet."),ReadOnly(true)]
         public int Height
         {
             get
             {
                 return sheetManager.imagePictBx.Height;
             }

             set
             {

                 sheetManager.imagePictBx.Height = value;
             }
         }

         [Category("FRAME FACTOR")]
         [DescriptionAttribute("The frames factor compared to a 320x480 resolution.")]
         public float Factor
         {
             get
             {
                 return this.sheet.FramesFactor;
             }

             set
             {

                this.sheet.FramesFactor = value;
                this.sheetManager.calculateSheetSize();
             }
         }
    }
}
