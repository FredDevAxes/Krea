using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Krea.GameEditor.TilesMapping
{
    public partial class TileSequenceManager : UserControl
    {
        private TilesMap currentTilesMap;
        private Form1 mainForm;
        public TileSequenceManager()
        {
            InitializeComponent();
        }

        public void init(TilesMap map, Form1 mainForm)
        {
            this.currentTilesMap = map;
            this.mainForm = mainForm;

            this.textureSequenceImageList.Images.Clear();
            this.textureSequenceListView.Items.Clear();
            this.textureSequencePropGrid.SelectedObject = null;

            this.objectSequenceImageList.Images.Clear();
            this.objectSequenceListView.Items.Clear();
            this.objectSequencePropGrid.SelectedObject = null;

            if (map.TextureSequences == null)
                map.TextureSequences = new List<TileSequence>();

            if (map.ObjectSequences == null)
                map.ObjectSequences = new List<TileSequence>();

            for (int i = 0; i < map.TextureSequences.Count; i++)
            {
                TileSequence seq = map.TextureSequences[i];
                int indexImage = this.textureSequenceImageList.Images.Count;
                this.textureSequenceImageList.Images.Add(seq.Frames[0].GorgonSprite.Image.SaveBitmap());

                
                ListViewItem seqItem = new ListViewItem();
                seqItem.Text = seq.Name;
                seqItem.ImageIndex = indexImage;
                seqItem.Tag = seq;
                this.textureSequenceListView.Items.Add(seqItem);

            }

            for (int i = 0; i < map.ObjectSequences.Count; i++)
            {
                TileSequence seq = map.ObjectSequences[i];
                int indexImage = this.objectSequenceImageList.Images.Count;
                this.objectSequenceImageList.Images.Add(seq.Frames[0].GorgonSprite.Image.SaveBitmap());

               
                ListViewItem seqItem = new ListViewItem();
                seqItem.Text = seq.Name;
                seqItem.ImageIndex = indexImage;
                seqItem.Tag = seq;
                this.objectSequenceListView.Items.Add(seqItem);

            }
        }

        public void AddTextureSequence(TileSequence seq)
        {
            if (this.currentTilesMap != null && seq.Frames.Count>0)
            {
                this.currentTilesMap.TextureSequences.Add(seq);
                int indexImage = this.textureSequenceImageList.Images.Count;
                this.textureSequenceImageList.Images.Add(seq.Frames[0].Image);

               
                ListViewItem seqItem = new ListViewItem();
                seqItem.Text = seq.Name;
                seqItem.ImageIndex = indexImage;
                seqItem.Tag = seq;
                this.textureSequenceListView.Items.Add(seqItem);

                
            }
        }

        public void AddObjectSequence(TileSequence seq)
        {
            if (this.currentTilesMap != null && seq.Frames.Count > 0)
            {
                this.currentTilesMap.ObjectSequences.Add(seq);
                int indexImage = this.objectSequenceImageList.Images.Count;
                this.objectSequenceImageList.Images.Add(seq.Frames[0].Image);

               
                ListViewItem seqItem = new ListViewItem();
                seqItem.Text = seq.Name;
                seqItem.ImageIndex = indexImage;
                seqItem.Tag = seq;
                this.objectSequenceListView.Items.Add(seqItem);


            }
        }

        private void textureSequenceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.textureSequenceListView.SelectedItems.Count > 0)
            {
                this.mainForm.tilesMapEditor1.CreationMode = "CREATING_TEXTURE_SEQUENCE";
                TileSequence seqSelected = (TileSequence)this.textureSequenceListView.SelectedItems[0].Tag;
                this.textureSequencePropGrid.SelectedObject = seqSelected;
            }
        }

        private void objectSequenceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.objectSequenceListView.SelectedItems.Count > 0)
            {
                this.mainForm.tilesMapEditor1.CreationMode = "CREATING_OBJECT_SEQUENCE";
                TileSequence seqSelected = (TileSequence)this.objectSequenceListView.SelectedItems[0].Tag;
                this.objectSequencePropGrid.SelectedObject = seqSelected;
            }
        }

        private void removeTextureSequenceBt_Click(object sender, EventArgs e)
        {
            if (this.textureSequenceListView.SelectedItems.Count > 0 && this.currentTilesMap != null)
            {
                for (int i = 0; i < this.textureSequenceListView.SelectedIndices.Count; i++)
                {
                    int index = this.textureSequenceListView.SelectedIndices[i];
                    this.currentTilesMap.TextureSequences.RemoveAt(index);
                    this.textureSequenceListView.Items.RemoveAt(index);
                    this.textureSequenceImageList.Images.RemoveAt(index);
                }
            }
        }

        private void removeObjectSequenceBt_Click(object sender, EventArgs e)
        {

            if (this.objectSequenceListView.SelectedItems.Count > 0 && this.currentTilesMap != null)
            {
                for (int i = 0; i < this.objectSequenceListView.SelectedIndices.Count; i++)
                {
                    int index = this.objectSequenceListView.SelectedIndices[i];
                    this.currentTilesMap.ObjectSequences.RemoveAt(index);
                    this.objectSequenceListView.Items.RemoveAt(index);
                    this.objectSequenceImageList.Images.RemoveAt(index);
                }
            }
        }

        private void objectSequenceListView_ItemActivate(object sender, EventArgs e)
        {
            this.mainForm.tilesMapEditor1.CreationMode = "CREATING_OBJECT_SEQUENCE";
        }

        private void textureSequenceListView_ItemActivate(object sender, EventArgs e)
        {
            this.mainForm.tilesMapEditor1.CreationMode = "CREATING_TEXTURE_SEQUENCE";
        }
    }
}
