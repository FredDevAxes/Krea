using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
using Krea.Asset_Manager.Assets_Property_Converters;

namespace Krea.Asset_Manager
{
    public partial class ImageManagerPanel : UserControl
    {
        public ImageManagerPanel()
        {
            InitializeComponent();
            this.imagePictBx.AllowDrop = true;
            
        }

        private AssetManagerForm mainForm;
        private DisplayObject DisplayObject;


        public void init(DisplayObject obj,AssetManagerForm mainForm)
        {
            this.mainForm = mainForm;
            initFromExistingSprite(obj);
           
        }

        public void initFromExistingSprite(DisplayObject obj)
        {
            this.DisplayObject = obj;
            this.DisplayObjectProperties();

            if (this.DisplayObject.Image != null)
            {
                this.imagePictBx.BackgroundImage = this.DisplayObject.Image;
                if (this.DisplayObject.Image.Size.Width > this.imagePictBx.Size.Width ||
                    this.DisplayObject.Image.Size.Height > this.imagePictBx.Size.Height)
                {
                    this.imagePictBx.BackgroundImageLayout = ImageLayout.Zoom;
                }
                else
                {
                    this.imagePictBx.BackgroundImageLayout = ImageLayout.Center;
                }

                
            }
        }

        public void DisplayObjectProperties()
        {
            if (this.DisplayObject != null)
            {
                ImagePropertyConverter spriteProp = new ImagePropertyConverter(this.DisplayObject, this.mainForm,this);
                this.mainForm.propertyGrid1.SelectedObject = null;
                this.mainForm.propertyGrid1.SelectedObject = spriteProp;

            }
        }
        private void rotateCurrentImage(RotateFlipType type)
        {
            if (this.DisplayObject != null)
            {
                if (this.DisplayObject.Image != null)
                {

                    this.DisplayObject.Image.RotateFlip(type);
                    GraphicsUnit graph = GraphicsUnit.Pixel;

                    RectangleF rect = this.DisplayObject.Image.GetBounds(ref graph);
                    this.DisplayObject.SurfaceRect = new Rectangle(0, 0, (int)rect.Width, (int)rect.Height);

                    this.imagePictBx.Refresh();
                }
            }
        }

        private void rotateLeftBt_Click(object sender, EventArgs e)
        {
            rotateCurrentImage(RotateFlipType.Rotate270FlipNone);
        }

        private void rotateRightBt_Click(object sender, EventArgs e)
        {
            rotateCurrentImage(RotateFlipType.Rotate90FlipNone);
        }

        private void changeBackColorBt_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.imagePictBx.BackColor = colorDialog1.Color;
            }
        }

        private void validBt_Click(object sender, EventArgs e)
        {
            this.Clean();
        }

        public void Clean()
        {
            this.mainForm.RemoveControlFromObjectsPanel(this);
            this.imagePictBx.Dispose();
            this.mainForm.RefreshAssetListView();
            this.Dispose();
        }

        private void imagePictBx_DragEnter(object sender, DragEventArgs e)
        {
            string filename = String.Empty;
            bool ret = false;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = System.IO.Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                        {
                            ret = true;
                        }
                    }
                }
            }

            if (ret == true)  
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void imagePictBx_DragDrop(object sender, DragEventArgs e)
        {
            string filename = String.Empty;
            bool ret = false;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = System.IO.Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                        {
                            ret = true;
                        }
                    }
                }
            }

            if (ret == true)
            {
                this.DisplayObject.Image = new Bitmap(Image.FromFile(filename));
                this.DisplayObject.SurfaceRect = new Rectangle(0, 0, this.DisplayObject.Image.Width, this.DisplayObject.Image.Height);
                initFromExistingSprite(this.DisplayObject);
            }
        }

        private void imagePictBx_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.imagePictBx.DoDragDrop(this.imagePictBx.BackgroundImage, DragDropEffects.All);  
        }
    }
}
