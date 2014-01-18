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
        
    public class ImagePropertyConverter
    {
        DisplayObject objSelected;
            AssetManagerForm MainForm;
            ImageManagerPanel imagePanel;
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public ImagePropertyConverter() { }
        public ImagePropertyConverter(DisplayObject obj, AssetManagerForm MainForm,ImageManagerPanel imagePanel)
        {
            objSelected = obj;
            this.MainForm = MainForm;
            this.imagePanel = imagePanel;
        }

         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
         [Category("Name")]
        [DescriptionAttribute("The name of the display object.")]
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

            [Category("Image")]
        [DescriptionAttribute("The Image of the display object.")]
        public Image Image
        {
         get
            {
                return objSelected.Image;
            }

            set
            {

                objSelected.Image = value;
                objSelected.SurfaceRect = new Rectangle(0,0,value.Width,value.Height);
                imagePanel.imagePictBx.BackgroundImage = objSelected.Image;
                
            }
         }

            [Category("Size")]
        [DescriptionAttribute("The image width of the display object.")]
        public int Width
        {
         get
            {
                return objSelected.SurfaceRect.Width;
            }

            set
            {

                objSelected.SurfaceRect = new Rectangle(0,0,value,objSelected.SurfaceRect.Height);
            }
         }

                [Category("Size")]
        [DescriptionAttribute("The image height of the display object.")]
        public int Height
        {
         get
            {
                return objSelected.SurfaceRect.Height;
            }

            set
            {

                objSelected.SurfaceRect = new Rectangle(0,0,objSelected.SurfaceRect.Width,value);
            }
         }


    }
}
