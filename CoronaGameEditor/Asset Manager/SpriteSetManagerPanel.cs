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
    public partial class SpriteSetManagerPanel : UserControl
    {

        public SpriteSetManagerPanel()
        {
            InitializeComponent();

            this.splitContainer1.SplitterWidth = 15;
            this.splitContainer2.SplitterWidth = 15;
            this.splitContainer3.SplitterWidth = 15;
            this.splitContainer4.SplitterWidth = 15;
        }
        private AssetManagerForm mainForm;
        private CoronaSpriteSet set;

        public void setManagerParent(AssetManagerForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public void init(AssetManagerForm mainForm, CoronaSpriteSet set)
        {
            this.mainForm = mainForm;

            initFromExistingSpriteSet(set);

            DisplayObjectProperties();
        }

        private void initFromExistingSpriteSet(CoronaSpriteSet set)
        {
            this.set = set;

            this.sheetCmbBx.Items.Clear();
            if (this.mainForm.CurrentAssetProject != null)
            {
                for (int i = 0; i < this.mainForm.CurrentAssetProject.SpriteSheets.Count; i++)
                {
                    this.sheetCmbBx.Items.Add(this.mainForm.CurrentAssetProject.SpriteSheets[i]);
                }

                if (this.sheetCmbBx.Items.Count > 0)
                    this.sheetCmbBx.SelectedIndex = 0;
            }

            refreshSpriteSetFramesListView();
        }

        public void DisplayObjectProperties()
        {
            if (this.set != null)
            {
                SpriteSetPropertyConverter spriteProp = new SpriteSetPropertyConverter(this.set, this.mainForm);
                this.mainForm.propertyGrid1.SelectedObject = spriteProp;

            }
        }

        private void playAnimCurrentSequence()
        {
            stopAnimCurrentSequence();

            if (this.set != null)
            {
                if (this.sequencesCmbBx.SelectedItem != null)
                {
                    CoronaSpriteSetSequence sequence = (CoronaSpriteSetSequence)this.sequencesCmbBx.SelectedItem;
                    this.set.SequenceSelected = sequence;
                    this.set.playAnimation(this.animPictBx);
                }
            }



        }

        private void stopAnimCurrentSequence()
        {
            if (this.set != null)
            {
                this.set.stopAnimation();
            }
        }

        private void stopAnimBt_Click(object sender, EventArgs e)
        {
            stopAnimCurrentSequence();
        }

        private void refreshSpriteSetFramesListView()
        {

            this.framesSetListView.Items.Clear();
            this.sequencesCmbBx.Items.Clear();
            this.sequencesCmbBx.Text = "";
            this.sequencePropGrid.SelectedObject = null;
            this.animPictBx.Refresh();

            if (this.framesSetListView.LargeImageList != null)
                this.framesSetListView.LargeImageList.Dispose();

            this.framesSetListView.LargeImageList = null;
            this.framesSetListView.BeginUpdate();
            if (this.set != null)
            {
                ImageList spriteSetFrames = new ImageList();
                spriteSetFrames.ImageSize = new Size(32,32);
                this.framesSetListView.LargeImageList = spriteSetFrames;

                for (int i = 0; i < set.Frames.Count; i++)
                {
                    SpriteFrame obj = set.Frames[i];
                    if (obj.Image != null)
                    {
                        spriteSetFrames.Images.Add(obj.Image);
                        ListViewItem item = new ListViewItem((i + 1).ToString(), spriteSetFrames.Images.Count - 1);
                        item.Tag = obj;
                        this.framesSetListView.Items.Add(item);
                    }
                }


                for (int i = 0; i < set.Sequences.Count; i++)
                {
                    this.sequencesCmbBx.Items.Add(set.Sequences[i]);
                }

                if (this.sequencesCmbBx.Items.Count > 0)
                    this.sequencesCmbBx.SelectedIndex = 0;
                else
                {
                    if (this.sequenceFrameListView.LargeImageList != null)
                        this.sequenceFrameListView.LargeImageList.Dispose();

                    this.sequenceFrameListView.LargeImageList = null;

                    this.sequenceFrameListView.BeginUpdate();
                    this.sequenceFrameListView.Items.Clear();

                    this.set.SequenceSelected = null;
                    this.sequenceFrameListView.EndUpdate();

                    this.animPictBx.Refresh();
                }
            }
            this.framesSetListView.EndUpdate();
        }

        public void upIndexFrame()
        {

            if (this.framesSetListView.SelectedIndices.Count > 0)
            {
                int index = this.framesSetListView.SelectedIndices[0];
                if (index == -1 || index == 0)
                    return;


                SpriteFrame select, previous, temp;

                select = this.set.Frames[index];

                previous = this.set.Frames[index - 1];

                temp = select;

                select = previous;

                previous = temp;

                this.set.Frames[index] = select;

                this.set.Frames[index - 1] = previous;

                this.refreshSpriteSetFramesListView();
                this.framesSetListView.SelectedIndices.Clear();
                this.framesSetListView.SelectedIndices.Add(index - 1);
            }
        }

        public void downIndexFrame()
        {
            if (this.framesSetListView.SelectedIndices.Count > 0)
            {
                int index = this.framesSetListView.SelectedIndices[0];
                if (index == -1 || index == this.set.Frames.Count - 1)
                    return;


                SpriteFrame select, next, temp;

                select = this.set.Frames[index];

                next = this.set.Frames[index + 1];

                temp = select;

                select = next;

                next = temp;

                this.set.Frames[index] = select;

                this.set.Frames[index + 1] = next;

                this.refreshSpriteSetFramesListView();
                this.framesSetListView.SelectedIndices.Clear();
                this.framesSetListView.SelectedIndices.Add(index + 1);
            }
        }

        private void sheetCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.framesSheetListView.Items.Clear();

            if (this.framesSheetListView.LargeImageList != null)
                this.framesSheetListView.LargeImageList.Dispose();

            this.framesSheetListView.LargeImageList = null;
            this.framesSheetListView.BeginUpdate();

            if (this.sheetCmbBx.SelectedItem != null)
            {
                CoronaSpriteSheet sheet = (CoronaSpriteSheet)this.sheetCmbBx.SelectedItem;

                if (sheet != null)
                {
                    ImageList spriteSetFrames = new ImageList();
                    spriteSetFrames.ImageSize = new Size(32,32);
                    this.framesSheetListView.LargeImageList = spriteSetFrames;

                    for (int i = 0; i < sheet.Frames.Count; i++)
                    {
                        SpriteFrame obj = sheet.Frames[i];
                        if (obj.Image != null)
                        {
                            spriteSetFrames.Images.Add(obj.Image);
                            ListViewItem item = new ListViewItem((i + 1).ToString(), spriteSetFrames.Images.Count - 1);
                            this.framesSheetListView.Items.Add(item);
                        }
                    }
                }

            }

            this.framesSheetListView.EndUpdate();
        }

        private void validBt_Click(object sender, EventArgs e)
        {
            this.Clean();
        }

        public void Clean()
        {
            stopAnimCurrentSequence();

            this.sequencesCmbBx.Items.Clear();

            sequenceFrameListView.Items.Clear();
            if (this.sequenceFrameListView.LargeImageList != null)
                this.sequenceFrameListView.LargeImageList.Dispose();

            this.sequenceFrameListView.LargeImageList = null;

            this.framesSheetListView.Items.Clear();

            if (this.framesSheetListView.LargeImageList != null)
                this.framesSheetListView.LargeImageList.Dispose();

            this.framesSetListView.LargeImageList = null;

            if (this.framesSetListView.LargeImageList != null)
                this.framesSetListView.LargeImageList.Dispose();

            this.framesSetListView.LargeImageList = null;

            this.animPictBx.Dispose();
            this.mainForm.RemoveControlFromObjectsPanel(this);
            this.mainForm.RefreshAssetListView();
            this.Dispose();
        }

        private void playAnimBt_Click(object sender, EventArgs e)
        {
            this.playAnimCurrentSequence();
        }

        private void stopAnimBt_Click_1(object sender, EventArgs e)
        {
            this.stopAnimCurrentSequence();
        }

        private void sequencesCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.stopAnimCurrentSequence();

            if (this.sequenceFrameListView.LargeImageList != null)
                this.sequenceFrameListView.LargeImageList.Dispose();

            this.sequenceFrameListView.LargeImageList = null;

            this.sequenceFrameListView.BeginUpdate();
            this.sequenceFrameListView.Items.Clear();

            if (this.set != null)
            {
                if (this.sequencesCmbBx.SelectedItem != null)
                {
                    CoronaSpriteSetSequence sequence = (CoronaSpriteSetSequence)this.sequencesCmbBx.SelectedItem;
                    if (sequence != null)
                    {
                        ImageList sequenceFrames = new ImageList();
                        sequenceFrames.ImageSize = new Size(32,32);
                        this.sequenceFrameListView.LargeImageList = sequenceFrames;

                        if (sequence.FrameDepart - 1 + sequence.FrameCount - 1 < set.Frames.Count)
                        {
                            for (int i = sequence.FrameDepart - 1; i < sequence.FrameDepart - 1 + sequence.FrameCount; i++)
                            {
                                SpriteFrame obj = set.Frames[i];
                                if (obj.Image != null)
                                {
                                    sequenceFrames.Images.Add(obj.Image);
                                    ListViewItem item = new ListViewItem((i + 1).ToString(), sequenceFrames.Images.Count - 1);
                                    this.sequenceFrameListView.Items.Add(item);
                                }
                            }
                        }

                        SequencePropertyConverter seqProp = new SequencePropertyConverter(sequence);
                        this.sequencePropGrid.SelectedObject = seqProp;
                        this.set.SequenceSelected = sequence;
                        
                    }
                }

            }

            this.sequenceFrameListView.EndUpdate();

            this.playAnimCurrentSequence();
        }

        private void animPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (this.set != null)
            {
                if (set.SequenceSelected != null)
                {
                    try
                    {
                        this.sequenceFrameListView.SelectedIndices.Clear();
                        this.sequenceFrameListView.SelectedIndices.Add(set.CurrentFrame+1 - set.SequenceSelected.FrameDepart );
                        set.dessineCurrentFrame(e.Graphics);
                    }
                    catch (Exception ex)
                    {

                    }
                   

                }
            }
        }

        private void importAllFramesBt_Click(object sender, EventArgs e)
        {
            if (this.sheetCmbBx.SelectedItem != null && this.set != null)
            {
                CoronaSpriteSheet sheetSelected = (CoronaSpriteSheet)this.sheetCmbBx.SelectedItem;
                for (int i = 0; i<sheetSelected.Frames.Count; i++)
                {
                    this.set.addFrame(sheetSelected.Frames[i]);
                }

                this.refreshSpriteSetFramesListView();
            }
        }

        private void importFrameBt_Click(object sender, EventArgs e)
        {
            if (this.sheetCmbBx.SelectedItem != null && this.set != null)
            {
                if (this.framesSheetListView.SelectedIndices.Count > 0)
                {
                    CoronaSpriteSheet sheetSelected = (CoronaSpriteSheet)this.sheetCmbBx.SelectedItem;
                    for (int i = 0; i < this.framesSheetListView.SelectedIndices.Count; i++)
                    {
                        this.set.Frames.Add(sheetSelected.Frames[this.framesSheetListView.SelectedIndices[i]]);
                    }

                    this.refreshSpriteSetFramesListView();
                }

            }
          

        }

        private void monterFrameBt_Click(object sender, EventArgs e)
        {
            this.upIndexFrame();
        }

        private void descendreFrameBt_Click(object sender, EventArgs e)
        {
            this.downIndexFrame();
        }


       
        private void removeFrameBt_Click(object sender, EventArgs e)
        {
            if (this.set != null)
            {
               
                this.stopAnimCurrentSequence();

                for (int i = 0; i < this.framesSetListView.SelectedItems.Count; i++)
                {
                    SpriteFrame frame = (SpriteFrame)this.framesSetListView.SelectedItems[i].Tag;
                    this.removeFrame(frame);

                    List<CoronaSpriteSetSequence> sequencesToDelete = this.set.checkSequencesIntegrity();
                    if (sequencesToDelete != null)
                    {
                        for (int j = 0; j < sequencesToDelete.Count; j++)
                        {
                            CoronaSpriteSetSequence sequenceToDelete = sequencesToDelete[j];
                            this.removeSequence(sequenceToDelete);
                            MessageBox.Show("The sequence named \"" + sequenceToDelete.Name + "\" has been automatically removed because it used a frame index that does not exist anymore!", "Sequence Integrity Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sequenceToDelete = null;
                        }

                        sequencesToDelete = null;
                    }
                }


                this.refreshSpriteSetFramesListView();

                //-----
                //DialogResult res = MessageBox.Show("Warning: All sequences of that sprite set need to be removed before be allowed to remove a frame!\n Continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (res == DialogResult.Yes)
                //{
                //    this.stopAnimCurrentSequence();
                //    this.set.Sequences.Clear();

                //    for (int i = 0; i < this.framesSetListView.SelectedItems.Count; i++)
                //    {
                //        SpriteFrame frame = (SpriteFrame)this.framesSetListView.SelectedItems[i].Tag;
                //        this.removeFrame(frame);

                //    }
                //    this.refreshSpriteSetFramesListView();
                //}
                

            }
        }

        public void removeFrame(SpriteFrame frame)
        {
            if (this.set != null)
            {
                if(this.set.Frames.Contains(frame))
                   this.set.Frames.Remove(frame);

            }
        }

        private void sequencePropGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.playAnimCurrentSequence();
        }

        private void addSequenceBt_Click(object sender, EventArgs e)
        {
            if (this.set != null)
            {
                if (this.framesSetListView.SelectedIndices.Count > 0)
                {
                    bool canCreate = true;
                    int indexDepart = this.framesSetListView.SelectedIndices[0];
                    for (int i = 0; i < this.framesSetListView.SelectedIndices.Count; i++)
                    {
                        if (i != 0)
                        {
                            int indexPrevious = this.framesSetListView.SelectedIndices[i - 1];
                            int index = this.framesSetListView.SelectedIndices[i];
                            if (index - indexPrevious != 1)
                            {
                                canCreate = false;
                                break;
                            }
                        }
                    }

                    if (canCreate == true)
                    {
                        this.stopAnimCurrentSequence();
                        CoronaSpriteSetSequence sequence = new CoronaSpriteSetSequence("sequence" + set.Sequences.Count, indexDepart+1, this.framesSetListView.SelectedIndices.Count, 1000, 0);
                        this.set.Sequences.Add(sequence);

                        
                        this.sequencesCmbBx.Items.Clear();


                        for (int i = 0; i < set.Sequences.Count; i++)
                        {
                            this.sequencesCmbBx.Items.Add(set.Sequences[i]);
                        }

                        this.sequencesCmbBx.SelectedIndex = set.Sequences.Count-1;

                    }
                    else
                    {
                        MessageBox.Show("Can not create sequences with non-consecutive frames!", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select Sprite set frame to create a new sequence!", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


             }
        }

        private void removeCurrentSequenceBt_Click(object sender, EventArgs e)
        {

            if (this.sequencesCmbBx.SelectedItem != null)
            {
                this.removeSequence((CoronaSpriteSetSequence)this.sequencesCmbBx.SelectedItem);
            }

        }

        private void removeSequence(CoronaSpriteSetSequence sequence)
        {
            this.stopAnimCurrentSequence();

            if (sequence != null)
            {
                this.stopAnimCurrentSequence();

                if (this.sequenceFrameListView.LargeImageList != null)
                    this.sequenceFrameListView.LargeImageList.Dispose();

                this.sequenceFrameListView.LargeImageList = null;

                this.sequenceFrameListView.BeginUpdate();
                this.sequenceFrameListView.Items.Clear();

                this.set.SequenceSelected = null;

                this.set.Sequences.Remove(sequence);
                this.refreshSpriteSetFramesListView();
                this.sequenceFrameListView.EndUpdate();
            }
        }

    }
}
