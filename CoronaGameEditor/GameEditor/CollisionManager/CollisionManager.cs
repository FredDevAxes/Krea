using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;

namespace Krea.GameEditor.CollisionManager
{
    public partial class CollisionManager : UserControl
    {
        public Scene currentScene;
        public List<CollisionFilterGroup> CollisionableGroupsBuffer;
        public int MAX_CATEGORIE_BIT = 32768;

        public CollisionManager()
        {
            InitializeComponent();
            this.CollisionableGroupsBuffer = new List<CollisionFilterGroup>();
            
        }

        public void Init(Scene scene)
        {
            if (scene != null)
            {
                this.filterGroupsListBx.Items.Clear();

                for (int i = 0; i < scene.CollisionFilterGroups.Count; i++)
                {
                    this.filterGroupsListBx.Items.Add(scene.CollisionFilterGroups[i]);
                }

                this.currentScene = scene;
                this.CollisionableGroupsBuffer = GetAllCollisionableItems();

                initGrid();
            }
        }

        private void initGrid()
        {
            this.dataGridView1.SuspendLayout();
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Rows.Clear();

            //Add a new columns for groups name
          /*  DataGridViewTextBoxColumn colGroupsName = new DataGridViewTextBoxColumn();
            colGroupsName.HeaderText = "GROUPS";
            colGroupsName.ReadOnly = true;
            colGroupsName.SortMode = DataGridViewColumnSortMode.NotSortable;

            this.dataGridView1.Columns.Add(colGroupsName);*/

            for (int i = 0; i < this.CollisionableGroupsBuffer.Count; i++)
            {
                CollisionFilterGroup group =  this.CollisionableGroupsBuffer[i];

                //Creer une nouvelle colonne
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                col.HeaderText = i + " : " + group.GroupName;
                this.dataGridView1.Columns.Add(col);

                //Creer une ligne pour le nom du groupe
                this.dataGridView1.Rows.Add();

                string cellText = i + " : " + group.GroupName;
                this.dataGridView1.Rows[i].HeaderCell.Value = cellText;
                

            }

            //Remove Last row
        //    this.dataGridView1.Rows.RemoveAt(this.dataGridView1.Rows.Count - 1);

            this.dataGridView1.ResumeLayout(false);
            updateGridValues();
        }

        private void updateGridValues()
        {
            this.dataGridView1.SuspendLayout();
            for (int i = 0; i < this.CollisionableGroupsBuffer.Count; i++)
            {
                CollisionFilterGroup group = this.CollisionableGroupsBuffer[i];

                for (int j = 0; j < this.CollisionableGroupsBuffer.Count; j++)
                {

                    CollisionFilterGroup groupCol = this.CollisionableGroupsBuffer[j];

                    if (i < this.CollisionableGroupsBuffer.Count && j < this.CollisionableGroupsBuffer.Count)
                    {
                        //Objact A and B are a tiles map
                        if (this.isItemACollisionWithItemB(this.CollisionableGroupsBuffer[i], this.CollisionableGroupsBuffer[j]))
                        {
                            this.dataGridView1.Rows[i].Cells[j].Value = true;
                        }
                        else
                        {
                            this.dataGridView1.Rows[i].Cells[j].Value = false;
                        }
                    }

                }
            }

            this.dataGridView1.ResumeLayout(true);
        }
        private void addFilterGroupBt_Click(object sender, EventArgs e)
        {

            if (!this.filterGroupNameTxtBx.Text.Equals(string.Empty))
            {
                for (int i = 0; i < this.currentScene.CollisionFilterGroups.Count; i++)
                {
                    CollisionFilterGroup group = this.currentScene.CollisionFilterGroups[i];
                    if (group.GroupName.Equals(this.filterGroupNameTxtBx.Text))
                    {
                        MessageBox.Show("The collision filter group \"" + this.filterGroupNameTxtBx.Text + "\" already exists!",
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return;
                    }
                }

                if (this.currentScene.CollisionFilterGroups.Count < 16)
                {
                    CollisionFilterGroup group = new CollisionFilterGroup(this.filterGroupNameTxtBx.Text.Replace(" ", ""), this.currentScene.CollisionFilterGroups.Count);
                    this.currentScene.CollisionFilterGroups.Add(group);
                }
                else
                {
                    MessageBox.Show(" Cannot create more than 16 collision filter groups !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Init(this.currentScene);

            }
            else
                MessageBox.Show("Please give a name to your collision filter group !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public Boolean isItemACollisionWithItemB(CollisionFilterGroup A, CollisionFilterGroup B)
        {
            for (int i = 0; i < A.CollisionWithCategorieBits.Count; i++)
            {
                if (A.CollisionWithCategorieBits[i].Equals(B.CategorieBit))
                {

                    return true;
                }
            }
            return false;
        }

        public List<CollisionFilterGroup> GetAllCollisionableItems()
        {
            // If Scene Exist
            if (this.currentScene != null)
            {
                // Clear Buffer
                this.CollisionableGroupsBuffer.Clear();


                //Start seeking inside all Layers for Corona Objects
                for (int i = 0; i < this.currentScene.CollisionFilterGroups.Count; i++)
                {
                    //Calculate PowValue for checking if the object is inside the collisionable range
                    int powValue = Convert.ToInt32(System.Math.Pow(Convert.ToDouble(2), Convert.ToDouble(this.CollisionableGroupsBuffer.Count)));
                    this.currentScene.CollisionFilterGroups[i].CategorieBit = powValue;
                    CollisionableGroupsBuffer.Add(this.currentScene.CollisionFilterGroups[i]);
                }

            }
            return CollisionableGroupsBuffer;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cellTouched = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Boolean isChecked = (Boolean)cellTouched.Value;
                CollisionFilterGroup ObjectA = CollisionableGroupsBuffer[e.RowIndex];
                CollisionFilterGroup ObjectB = CollisionableGroupsBuffer[e.ColumnIndex];

                if (isChecked == true)
                {
                    if (isItemACollisionWithItemB(ObjectA, ObjectB))
                    {
                        ObjectA.CollisionWithCategorieBits.Remove(ObjectB.CategorieBit);
                    }
                    if (isItemACollisionWithItemB(ObjectB, ObjectA))
                    {
                        ObjectB.CollisionWithCategorieBits.Remove(ObjectA.CategorieBit);
                    }

                }
                else
                {
                    if (!isItemACollisionWithItemB(ObjectA, ObjectB))
                    {
                        ObjectA.CollisionWithCategorieBits.Add(ObjectB.CategorieBit);
                    }
                    if (!isItemACollisionWithItemB(ObjectB, ObjectA))
                    {
                        ObjectB.CollisionWithCategorieBits.Add(ObjectA.CategorieBit);
                    }

                }

                updateGridValues();

            }
           

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cellTouched = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Boolean isChecked = (Boolean)cellTouched.Value;
                CollisionFilterGroup ObjectA = CollisionableGroupsBuffer[e.RowIndex];
                CollisionFilterGroup ObjectB = CollisionableGroupsBuffer[e.ColumnIndex];

                if (isChecked == true)
                {
                    if (isItemACollisionWithItemB(ObjectA, ObjectB))
                    {
                        ObjectA.CollisionWithCategorieBits.Remove(ObjectB.CategorieBit);
                    }
                    if (isItemACollisionWithItemB(ObjectB, ObjectA))
                    {
                        ObjectB.CollisionWithCategorieBits.Remove(ObjectA.CategorieBit);
                    }

                }
                else
                {
                    if (!isItemACollisionWithItemB(ObjectA, ObjectB))
                    {
                        ObjectA.CollisionWithCategorieBits.Add(ObjectB.CategorieBit);
                    }
                    if (!isItemACollisionWithItemB(ObjectB, ObjectA))
                    {
                        ObjectB.CollisionWithCategorieBits.Add(ObjectA.CategorieBit);
                    }

                }

                updateGridValues();

            }
        }

        private void removeFilterGroup_Click(object sender, EventArgs e)
        {
            if (this.filterGroupsListBx.SelectedItem != null)
            {
                CollisionFilterGroup group = (CollisionFilterGroup)filterGroupsListBx.SelectedItem;
                if (this.currentScene.CollisionFilterGroups.Contains(group))
                    this.currentScene.CollisionFilterGroups.Remove(group);


                this.Init(this.currentScene);
            }
        }

    }
}
