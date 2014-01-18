using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
using Krea.Asset_Manager.Assets_Property_Converters;
using System.IO;

namespace Krea.GameEditor
{
    public partial class AnimationManager : Form
    {
        private CoronaSpriteSet set;
        private Form1 mainForm;
        public AnimationManager()
        {
            InitializeComponent();
        }

        public void init(Form1 mainForm, CoronaSpriteSet set)
        {
            this.mainForm = mainForm;

            initFromExistingSpriteSet(set);

           
        }

        private void reloadFramesBitmap()
        {
            if (this.set != null)
            {
                CoronaGameProject projectParent = this.mainForm.CurrentProject;

                for (int i = 0; i < set.Frames.Count; i++)
                {
                    SpriteFrame obj = set.Frames[i];
                    if (obj.Image == null)
                    {
                        string sheetDirectory = Path.Combine(projectParent.ProjectPath + "\\Resources\\SpriteSheets", obj.SpriteSheetParent.Name);
                        string frameFileName = Path.Combine(sheetDirectory, obj.SpriteSheetParent.Name + "_frame" + i+".png");
                        if (File.Exists(frameFileName))
                        {
                            obj.Image = Bitmap.FromFile(frameFileName);
                        }
                        
                    }
                }
            }
        }

        private void initFromExistingSpriteSet(CoronaSpriteSet set)
        {
            this.set = set;
            reloadFramesBitmap();

            refreshSpriteSetFramesListView();
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
                spriteSetFrames.ImageSize = new Size(32, 32);
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
                   

                    this.set.SequenceSelected = null;
                   
                    this.animPictBx.Refresh();
                }
            }
            this.framesSetListView.EndUpdate();
        }

        public void Clean()
        {
            stopAnimCurrentSequence();

            this.sequencesCmbBx.Items.Clear();

          
            this.framesSetListView.LargeImageList = null;

            if (this.framesSetListView.LargeImageList != null)
                this.framesSetListView.LargeImageList.Dispose();

            this.framesSetListView.LargeImageList = null;

            this.animPictBx.Dispose();
            this.Dispose();
        }

        private void animPictBx_Paint(object sender, PaintEventArgs e)
        {
            if (this.set != null)
            {
                if (set.SequenceSelected != null)
                {
                    try
                    {
                        set.dessineCurrentFrame(e.Graphics);
                    }
                    catch (Exception ex)
                    {

                    }


                }
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
                        CoronaSpriteSetSequence sequence = new CoronaSpriteSetSequence("sequence" + set.Sequences.Count, indexDepart + 1, this.framesSetListView.SelectedIndices.Count, 1000, 0);
                        this.set.Sequences.Add(sequence);


                        this.sequencesCmbBx.Items.Clear();


                        for (int i = 0; i < set.Sequences.Count; i++)
                        {
                            this.sequencesCmbBx.Items.Add(set.Sequences[i]);
                        }

                        this.sequencesCmbBx.SelectedIndex = set.Sequences.Count - 1;

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

                this.set.SequenceSelected = null;

                this.set.Sequences.Remove(sequence);
                this.refreshSpriteSetFramesListView();
              
            }
        }

        private void sequencesCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.stopAnimCurrentSequence();

          
            if (this.set != null)
            {
                if (this.sequencesCmbBx.SelectedItem != null)
                {
                    CoronaSpriteSetSequence sequence = (CoronaSpriteSetSequence)this.sequencesCmbBx.SelectedItem;
                    if (sequence != null)
                    {
                        SequencePropertyConverter seqProp = new SequencePropertyConverter(sequence);
                        this.sequencePropGrid.SelectedObject = seqProp;
                        this.set.SequenceSelected = sequence;

                    }
                }

            }

            this.playAnimCurrentSequence();
        }

        private void AnimationManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Clean();
        }

        private void playAnimBt_Click(object sender, EventArgs e)
        {
            this.playAnimCurrentSequence();
        }

        private void stopAnimBt_Click(object sender, EventArgs e)
        {
            stopAnimCurrentSequence();
        }

    }
}
