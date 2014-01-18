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
    public partial class TileEventManager : UserControl
    {

         private TilesMap currentTilesMap;
        private Form1 mainForm;

        public TileEventManager()
        {
            InitializeComponent();
        }

        public void init(TilesMap map, Form1 mainForm)
        {
            this.currentTilesMap = map;
            this.mainForm = mainForm;

            this.eventListView.Items.Clear();
            this.eventNameTxtBx.Text = "";
            this.eventTypeCmbBx.SelectedIndex = 0;

            if (this.currentTilesMap != null)
            {
                if (this.currentTilesMap.TileEvents == null)
                    this.currentTilesMap.TileEvents = new List<TileEvent>();

                for (int i = 0; i < this.currentTilesMap.TileEvents.Count; i++)
                {
                    this.addEventToListView(this.currentTilesMap.TileEvents[i]);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.currentTilesMap != null)
            {
                string eventName = this.eventNameTxtBx.Text.Replace(" ", "");

                TileEvent.TileEventType eventType = TileEvent.TileEventType.collision;
                if (this.eventTypeCmbBx.SelectedText.Equals("collision"))
                    eventType = TileEvent.TileEventType.collision;
                else if (this.eventTypeCmbBx.SelectedText.Equals("preCollision"))
                    eventType = TileEvent.TileEventType.preCollision;
                else if (this.eventTypeCmbBx.SelectedText.Equals("postCollision"))
                    eventType = TileEvent.TileEventType.postCollision;
                else if (this.eventTypeCmbBx.SelectedText.Equals("touch"))
                    eventType = TileEvent.TileEventType.touch;

                TileEvent tileEvent = new TileEvent(eventName,eventType);
                this.addEventToListView(tileEvent);
                this.currentTilesMap.TileEvents.Add(tileEvent);

            }
        }

        private void addEventToListView(TileEvent tileEvent)
        {
            if (tileEvent != null)
            {
                ListViewItem itemEvent = new ListViewItem();
                itemEvent.Text = tileEvent.Name;
                itemEvent.Tag = tileEvent;

                if (tileEvent.Type == TileEvent.TileEventType.collision)
                    itemEvent.ImageIndex = 0;
                else if (tileEvent.Type == TileEvent.TileEventType.postCollision)
                    itemEvent.ImageIndex = 1;
                else if (tileEvent.Type == TileEvent.TileEventType.preCollision)
                    itemEvent.ImageIndex = 2;
                else if (tileEvent.Type == TileEvent.TileEventType.touch)
                    itemEvent.ImageIndex = 3;

                this.eventListView.Items.Add(itemEvent);


            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (this.currentTilesMap != null)
            {
                ListViewItem[] list = new ListViewItem[this.eventListView.SelectedItems.Count];
                this.eventListView.SelectedItems.CopyTo(list, 0);
                for (int i = 0; i < list.Length; i++)
                {
                    ListViewItem item = list[i];
                    TileEvent eventSelected = item.Tag as TileEvent;
                    if (this.currentTilesMap.TileEvents.Contains(eventSelected))
                        this.currentTilesMap.TileEvents.Remove(eventSelected);

                    this.eventListView.Items.Remove(item);
                }
            }
        }

        private void eventListView_ItemActivate(object sender, EventArgs e)
        {
            this.mainForm.tilesMapEditor1.CreationMode = "CREATING_EVENT";
        }

        private void eventListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mainForm.tilesMapEditor1.CreationMode = "CREATING_EVENT";
        }
    }
}
