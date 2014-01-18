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
    public partial class SpriteManagerPanel : UserControl
    {
        public SpriteManagerPanel()
        {
            InitializeComponent();

            this.splitContainer1.SplitterWidth = 15;
            this.splitContainer2.SplitterWidth = 15;
        }

        private AssetManagerForm mainForm;
        private DisplayObject obj;
        private CoronaSpriteSet lastSet;

        public void init(AssetManagerForm mainForm, DisplayObject obj)
        {
            this.mainForm = mainForm;
            initFromExistingSprite(obj);

            DisplayObjectProperties();
        }

        public void initFromExistingSprite(DisplayObject obj)
        {
            this.obj = obj;
            initSpriteSetListAvailable();

            this.DisplayObjectProperties();

            

        }
        public void DisplayObjectProperties()
        {
            if (this.obj != null)
            {
                SpritePropertyConverter spriteProp = new SpritePropertyConverter(this.obj, this.mainForm);
                this.mainForm.propertyGrid1.SelectedObject = spriteProp;

            }
        }
        private void initSpriteSetListAvailable()
        {
            this.spriteSetParentCmbBx.Items.Clear();

            if (this.mainForm != null && this.mainForm.CurrentAssetProject != null)
            {
                //Init le name de l'objet selected
                //Init la liste des spriteset dispo
                List<CoronaSpriteSet> spriteSets = this.mainForm.CurrentAssetProject.SpriteSets;
                int indexCurrent = -1;
                if (spriteSets != null)
                {
                    for (int i = 0; i < spriteSets.Count; i++)
                    {
                        this.spriteSetParentCmbBx.Items.Add(spriteSets[i]);
                        if (this.obj.SpriteSet == spriteSets[i])
                            indexCurrent = i;

                    }
                }

                if (indexCurrent != -1)
                    this.spriteSetParentCmbBx.SelectedIndex = indexCurrent;
            }
        }

        private void spriteSetParentCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.stopAnimCurrentSequence();

            this.sequencesCmbBx.Items.Clear();
            this.sequencesCmbBx.Text = "";
            this.sequenceFramesListView.Items.Clear();

            if (this.frameSpriteSetListView.LargeImageList != null)
                this.frameSpriteSetListView.LargeImageList.Dispose();

            this.frameSpriteSetListView.LargeImageList = null;

            if (this.sequenceFramesListView.LargeImageList != null)
                this.sequenceFramesListView.LargeImageList.Dispose();

            this.sequenceFramesListView.LargeImageList = null;


            this.frameSpriteSetListView.BeginUpdate();
            this.frameSpriteSetListView.Items.Clear();

            if (this.spriteSetParentCmbBx.SelectedItem != null)
            {
                CoronaSpriteSet set = (CoronaSpriteSet)this.spriteSetParentCmbBx.SelectedItem;
                
                if (set != null)
                {
                    this.obj.SpriteSet = set;

                    ImageList spriteSetFrames = new ImageList();
                    spriteSetFrames.ImageSize = new Size(64,64);
                    this.frameSpriteSetListView.LargeImageList = spriteSetFrames;

                    for (int i = 0; i < set.Frames.Count; i++)
                    {
                        SpriteFrame obj = set.Frames[i];
                        if (obj.Image != null)
                        {
                            spriteSetFrames.Images.Add(obj.Image);
                            ListViewItem item = new ListViewItem((i + 1).ToString(), spriteSetFrames.Images.Count - 1);
                            this.frameSpriteSetListView.Items.Add(item);
                        }
                    }


                    for (int i = 0; i < set.Sequences.Count; i++)
                    {
                        this.sequencesCmbBx.Items.Add(set.Sequences[i]);
                    }

                    if (this.sequencesCmbBx.Items.Count > 0)
                        this.sequencesCmbBx.SelectedIndex = 0;
                }
            }

            this.frameSpriteSetListView.EndUpdate();

            this.animPictBx.Refresh();
        }

        private void sequencesCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (this.sequenceFramesListView.LargeImageList != null)
                this.sequenceFramesListView.LargeImageList.Dispose();

            this.sequenceFramesListView.LargeImageList = null;

            this.sequenceFramesListView.BeginUpdate();
            this.sequenceFramesListView.Items.Clear();

            this.stopAnimCurrentSequence();
            if (this.spriteSetParentCmbBx.SelectedItem != null)
            {
                CoronaSpriteSet set = (CoronaSpriteSet)this.spriteSetParentCmbBx.SelectedItem;
                if (set != null)
                {
                    if (this.sequencesCmbBx.SelectedItem != null)
                    {
                        CoronaSpriteSetSequence sequence = (CoronaSpriteSetSequence)this.sequencesCmbBx.SelectedItem;
                        if (sequence != null)
                        {
                            
                            ImageList sequenceFrames = new ImageList();
                            sequenceFrames.ImageSize = new Size(64,64);
                            this.sequenceFramesListView.LargeImageList = sequenceFrames;

                            if(sequence.FrameDepart-1 + sequence.FrameCount-1 <=set.Frames.Count)
                            {
                                for (int i = sequence.FrameDepart - 1; i < sequence.FrameDepart - 1+sequence.FrameCount; i++)
                                 {
                                    SpriteFrame obj = set.Frames[i];
                                    if (obj.Image != null)
                                    {
                                        sequenceFrames.Images.Add(obj.Image);
                                        ListViewItem item = new ListViewItem((i + 1).ToString(), sequenceFrames.Images.Count - 1);
                                        this.sequenceFramesListView.Items.Add(item);
                                    }
                                 }
                                set.SequenceSelected = sequence;
                                
                            }

                            
                           
                        }
                    }
                   
                }
            }

            this.sequenceFramesListView.EndUpdate();
            playAnimCurrentSequence();
        }

        private void animPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (lastSet != null)
            {
                if (lastSet.SequenceSelected != null)
                {

                    this.sequenceFramesListView.SelectedIndices.Clear();
                    this.sequenceFramesListView.SelectedIndices.Add(lastSet.CurrentFrame + 1 - lastSet.SequenceSelected.FrameDepart);

                    lastSet.dessineCurrentFrame(e.Graphics);
                   
                }
            }
        }

        private void playAnimBt_Click(object sender, EventArgs e)
        {
            playAnimCurrentSequence();
        }

        private void playAnimCurrentSequence()
        {
            stopAnimCurrentSequence();
            if (this.spriteSetParentCmbBx.SelectedItem != null)
            {
                CoronaSpriteSet set = (CoronaSpriteSet)this.spriteSetParentCmbBx.SelectedItem;
                if (this.sequencesCmbBx.SelectedItem != null)
                {
                    CoronaSpriteSetSequence sequence = (CoronaSpriteSetSequence)this.sequencesCmbBx.SelectedItem;
                    set.SequenceSelected = sequence;
                    lastSet = set;
                    set.playAnimation(this.animPictBx);
                }
            }
        }

        private void stopAnimCurrentSequence()
        {
            if (lastSet != null)
            {
                lastSet.stopAnimation();
                lastSet = null;
            }
        }

        private void stopAnimBt_Click(object sender, EventArgs e)
        {
            stopAnimCurrentSequence();
        }

        private void validBt_Click(object sender, EventArgs e)
        {
            this.Clean();
        }

        public void Clean()
        {
            stopAnimCurrentSequence();

            this.sequencesCmbBx.Items.Clear();
            this.sequenceFramesListView.Items.Clear();

            if (this.frameSpriteSetListView.LargeImageList != null)
                this.frameSpriteSetListView.LargeImageList.Dispose();

            this.frameSpriteSetListView.LargeImageList = null;

            if (this.sequenceFramesListView.LargeImageList != null)
                this.sequenceFramesListView.LargeImageList.Dispose();

            this.sequenceFramesListView.LargeImageList = null;

            this.animPictBx.Dispose();
            this.mainForm.RemoveControlFromObjectsPanel(this);
            this.mainForm.RefreshAssetListView();
            this.Dispose();
        }
    }
}
