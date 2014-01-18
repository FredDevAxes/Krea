using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
using System.Drawing.Imaging;
using Krea.Asset_Manager.Assets_Property_Converters;
using Krea.Asset_Manager;
using System.IO;

namespace Krea.Asset_Manager
{
    public partial class SpriteSheetManagerPanel : UserControl
    {
        public SpriteSheetManagerPanel()
        {
            InitializeComponent();

            this.splitContainer1.SplitterWidth = 15;
        }

        private AssetManagerForm mainForm;
        private CoronaSpriteSheet sheet;
        private int maxHeight;
        public void init(AssetManagerForm mainForm,CoronaSpriteSheet sheet)
        {
            this.mainForm = mainForm;
            this.sheet = sheet;

            if (this.sheet != null)
            {

                this.calculateSizeBt_Click(null, null);
                refreshFramesListView();
                this.DisplayObjectProperties();
            }
           
        }

        public void DisplayObjectProperties()
        {
            if (this.sheet != null)
            {
                SpriteSheetPropertyConverter spriteProp = new SpriteSheetPropertyConverter(this.sheet, this.mainForm,this);
                this.mainForm.propertyGrid1.SelectedObject = spriteProp;

            }
        }
      
        private void refreshFramesListView()
        {
            this.framesListView.Items.Clear();

            if (this.framesListView.LargeImageList != null)
                this.framesListView.LargeImageList.Dispose();

            this.framesListView.LargeImageList = null;
            this.framesListView.BeginUpdate();
            if (this.sheet != null)
            {
                ImageList sheetFrames = new ImageList();
                sheetFrames.ImageSize = new Size(32,32);
                this.framesListView.LargeImageList = sheetFrames;


                for (int i = 0; i < this.sheet.Frames.Count; i++)
                {
                    SpriteFrame obj = this.sheet.Frames[i];
                    if (obj.Image != null)
                    {
                        sheetFrames.Images.Add(obj.Image);
                        ListViewItem item = new ListViewItem((i+1).ToString(), sheetFrames.Images.Count - 1);
                        item.Tag = obj;
                        this.framesListView.Items.Add(item);
                    }
                }
                this.calculateSheetSize();
            }
            this.framesListView.EndUpdate();
        }

        public bool addFrameToSpriteSheet(String filename)
        {
            //Create Frame from filename
            Image img1 = Image.FromFile(filename);

            //
            //Trick to allow Annimated Gif importation
            //          
            FrameDimension dimension = new FrameDimension(img1.FrameDimensionsList[0]);
            // Get Frame count of Image File
            int frameCount = img1.GetFrameCount(dimension);
            // Browse frame list
            for (int i = 0; i < frameCount; i++)
            {
                // Select current frame
                img1.SelectActiveFrame(dimension, i);

                // Add normaly the image to the spritesheet
                Image img = new Bitmap(img1);
                SpriteFrame newFrame = new SpriteFrame(filename, this.sheet.Frames.Count, img, sheet);
                this.sheet.Frames.Add(newFrame);
                

            }

            //Clean
            img1.Dispose();

            
   
            return true;

        }
        private void refreshPositionFrames()
        {
            Point pDest = new Point(5, 5);
            this.maxHeight = 0;
            for (int i = 0; i < this.sheet.Frames.Count; i++)
            {
                SpriteFrame frame = (SpriteFrame)this.sheet.Frames[i];
                

                //Recuperer le point de la derniere frame de la liste si il y en a une
                if (i >= 1)
                {
                    SpriteFrame lastFrame = (SpriteFrame)sheet.Frames[i - 1];

                    //Traiter le point 
                    pDest.X = lastFrame.Position.X + lastFrame.Image.Width + 5;
                    pDest.Y = lastFrame.Position.Y;

                    //Si le point en x est superieur a la largeur totale
                    if (pDest.X + frame.Image.Width > 1024)
                    {
                        //Positionner les points une ligne en dessous
                        pDest.X = 5;
                        pDest.Y = lastFrame.Position.Y + this.maxHeight+5;
                        this.maxHeight = frame.Image.Height;

                    }
                    //Sinon 
                    else
                    {
                        //Enregistrer la nouvelle valeur du maxHeight de la ligne du sprite set
                        if (frame.Image.Height > this.maxHeight)
                        {
                            this.maxHeight = frame.Image.Height;
                        }
                    }

                }

                //Modifier la position a la frame et l'ajouter a la liste
                frame.setPosition(pDest);

            }


        }

        public void removeFrame(int index)
        {
            if (this.sheet != null && this.mainForm.CurrentAssetProject != null)
            {
                AssetsToSerialize assetProject = this.mainForm.CurrentAssetProject;
                SpriteFrame frame = this.sheet.Frames[index];
                this.sheet.Frames.Remove(frame);

                //Chercher toutes lespsriteSet utilisant cette frame
                for (int i = 0; i < assetProject.SpriteSets.Count; i++)
                {
                    CoronaSpriteSet set = assetProject.SpriteSets[i];
                    List<SpriteFrame> framesToDelete = set.checkFramesIntegrity();
                    if (framesToDelete != null)
                    {
                        for (int j = 0; j < framesToDelete.Count; j++)
                        {
                            set.Frames.Remove(framesToDelete[j]);
                        }
                        framesToDelete = null;

                        List<CoronaSpriteSetSequence> sequencesToDelete = set.checkSequencesIntegrity();
                        if (sequencesToDelete != null)
                        {
                            for (int j = 0; j < sequencesToDelete.Count; j++)
                            {
                                CoronaSpriteSetSequence sequenceToDelete = sequencesToDelete[j];
                                set.Sequences.Remove(sequenceToDelete);
                                MessageBox.Show("The sequence named \"" + sequenceToDelete.Name + "\", located in the sprite set named \"" + set.Name + "\", has been automatically removed because it used a frame index that does not exist anymore!", "Sequence Integrity Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                sequenceToDelete = null;
                            }

                            sequencesToDelete = null;
                        }
                    }
                }
                frame.Image.Dispose();

                
                frame = null;


                //Chercher toutes lespsriteSet utilisant cette frame
                //for (int i = 0; i < assetProject.SpriteSets.Count; i++)
                //{
                //    bool hasFound = false;
                //    CoronaSpriteSet set = assetProject.SpriteSets[i];
                //    for (int j = 0; j < set.Frames.Count; j++)
                //    {
                //        if (set.Frames[j] == frame)
                //        {
                //            hasFound = true;
                //            break;
                //        }
                //    }

                //    if (hasFound == true)
                //    {
                //        this.mainForm.removeSpriteSet(set);
                //    }
                //}
                //frame.Image.Dispose();

                //this.sheet.Frames.Remove(frame);
                //frame = null;

               
            }
           
        }

        public void upIndexFrame()
        {

            if (this.framesListView.SelectedIndices.Count > 0)
            {
                int index = this.framesListView.SelectedIndices[0];
                if (index == -1 || index == 0)
                    return;


                SpriteFrame select, previous, temp;

                select = this.sheet.Frames[index];

                previous = this.sheet.Frames[index - 1];

                temp = select;

                select = previous;

                previous = temp;

                this.sheet.Frames[index] = select;

                this.sheet.Frames[index - 1] = previous;

                this.refreshFramesListView();
                this.framesListView.SelectedIndices.Clear();
                this.framesListView.SelectedIndices.Add(index - 1);
            }
        }

        public void downIndexFrame()
        {
            if (this.framesListView.SelectedIndices.Count > 0)
            {
                int index = this.framesListView.SelectedIndices[0];
                if (index == -1 || index == this.sheet.Frames.Count - 1)
                    return;

                
                SpriteFrame select, next, temp;

                select = this.sheet.Frames[index];

                next = this.sheet.Frames[index + 1];

                temp = select;

                select = next;

                next = temp;

                this.sheet.Frames[index] = select;

                this.sheet.Frames[index + 1] = next;

                this.refreshFramesListView();
                this.framesListView.SelectedIndices.Clear();
                this.framesListView.SelectedIndices.Add(index + 1);
            }
        }

        public void dessineAllFrame(Graphics g,float xRatio, float yRatio,bool applyRatio)
        {
            if (this.sheet != null)
            {
                if (applyRatio == true)
                {
                    for (int i = 0; i < this.sheet.Frames.Count; i++)
                    {
                        SpriteFrame frame = (SpriteFrame)this.sheet.Frames[i];
                        frame.dessineFrame(g, xRatio, yRatio,this.sheet.FramesFactor,"");
                    }
                }
                else
                {
                    for (int i = 0; i < this.sheet.Frames.Count; i++)
                    {
                        SpriteFrame frame = (SpriteFrame)this.sheet.Frames[i];
                        frame.dessineFrame(g, xRatio, yRatio);
                    }
                }
                
            }
       
        }


        private void addFrameBt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileD = new OpenFileDialog();
            openFileD.Multiselect = true;

            openFileD.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif" ;
            openFileD.AddExtension = false;

            if (openFileD.ShowDialog() == DialogResult.OK)
            {
                String[] filenames = openFileD.FileNames;
                for (int i = 0; i < filenames.Length; i++)
                {
                    try
                    {
                        addFrameToSpriteSheet(filenames[i]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during image loading ! \n\n " + ex.Message);
                    }


                }

                this.refreshFramesListView();

            }
        }

        private void imagePictBx_Paint(object sender, PaintEventArgs e)
        {
            if (this.sheet != null)
            {
                
                dessineAllFrame(e.Graphics,1,1,true);
            }
        }

        private void validBt_Click(object sender, EventArgs e)
        {
            this.Clean();
        }


        public void Clean()
        {
            if (this.sheet != null)
            {
                try
                {

                    if (this.sheet.ImageSpriteSheet != null)
                    {
                        this.sheet.ImageSpriteSheet.Dispose();
                        this.sheet.ImageSpriteSheet = null;
                    }

                    if (this.imagePictBx.Size.Width > 0 && this.imagePictBx.Size.Height > 0)
                    {
                        //Creer une image de l'ensemble des frames
                        this.sheet.ImageSpriteSheet = new Bitmap(Convert.ToInt32(this.imagePictBx.Size.Width), Convert.ToInt32(this.imagePictBx.Size.Height));

                        using (Graphics g = Graphics.FromImage(this.sheet.ImageSpriteSheet))
                        {
                            dessineAllFrame(g, 1, 1, true);
                        }
                    }
                   


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: The size of the sheet seems to be too big !\n You should create different smaller sheets and only one sprite set using all of them.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }


            this.framesListView.Items.Clear();

            if (this.framesListView.LargeImageList != null)
                this.framesListView.LargeImageList.Dispose();

            this.framesListView.LargeImageList = null;

            this.mainForm.RemoveControlFromObjectsPanel(this);
            this.mainForm.RefreshAssetListView();
            this.Dispose();
        }

        private void removeFrameBt_Click(object sender, EventArgs e)
        {
            if (this.sheet != null)
            {
                ListViewItem[] items = new ListViewItem[this.framesListView.SelectedItems.Count];
                this.framesListView.SelectedItems.CopyTo(items, 0);
                for (int i = 0; i < items.Length; i++)
                {
                    int index = this.sheet.Frames.IndexOf(items[i].Tag as SpriteFrame);
                    if(index>-1)
                        this.removeFrame(index);
                }
                this.refreshFramesListView();

            }
        }


        private void monterFrameBt_Click(object sender, EventArgs e)
        {
            upIndexFrame();
        }

        private void descendreFrameBt_Click(object sender, EventArgs e)
        {
            this.downIndexFrame();
        }

        private void calculateSizeBt_Click(object sender, EventArgs e)
        {
            calculateSheetSize();
        }

        public void calculateSheetSize()
        {
            if (this.sheet != null)
            {
                refreshPositionFrames();
                SizeF sizeSheet = this.sheet.calculateSize(1, 1, true);

                if (sizeSheet.Width > 2048)
                    this.imagePictBx.Width = 2048;
                else
                    this.imagePictBx.Width = (int)Math.Ceiling(sizeSheet.Width);

                if (sizeSheet.Height > 2048)
                    this.imagePictBx.Height = 2048;
                else
                    this.imagePictBx.Height = (int)Math.Ceiling(sizeSheet.Height);

                
                this.imagePictBx.Refresh();
            }
        }

        private void spriteSheetSplitterBt_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            form.Icon = this.mainForm.Icon;
            form.Text = "Sprite Sheet Splitter";
            
            SpriteSheetSplitter splitterControl = new SpriteSheetSplitter();
            form.Controls.Add(splitterControl);
            splitterControl.Dock = DockStyle.Fill;

            form.Size = this.mainForm.Size;
            form.WindowState = this.mainForm.WindowState;
            form.Location = this.mainForm.Location;

            DialogResult rs = form.ShowDialog(this.mainForm);

            if (rs == DialogResult.OK)
            {
                for (int i = 0; i < splitterControl.Frames.Count; i++)
                {
                    SpriteFrame newFrame = new SpriteFrame(this.sheet.Name, this.sheet.Frames.Count, splitterControl.Frames[i], sheet);
                    this.sheet.Frames.Add(newFrame);
                }

                this.refreshFramesListView();
            }

            splitterControl.Dispose();
            form.Dispose();
            splitterControl = null;
            form = null;
        }
    }
}
