using Krea.GameEditor.TilesMapping;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;
using Krea.CoronaClasses;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    class TilesMapPropertyConverter
    {

         //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private TilesMap tilesMapSelected;
        private Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public TilesMapPropertyConverter(TilesMap tilesMapSelected, Form1 MainForm)
        {
            this.tilesMapSelected = tilesMapSelected;
            this.MainForm = MainForm;
        }

        public TilesMap GetTileMapSelected()
        {
            return this.tilesMapSelected;
        }
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
        [Category("GENERAL")]
        public string Name
        {
            get { return this.tilesMapSelected.TilesMapName; }
            set
            {
                value = value.Replace("_", "");
                value = value.Replace(" ", "");

                 
                string newName = this.tilesMapSelected.LayerParent.SceneParent.projectParent.IncrementObjectName(value);

                string directoryToMoveResources = this.tilesMapSelected.LayerParent.SceneParent.projectParent.ProjectPath + "\\Resources\\TileMaps";
                if (System.IO.Directory.Exists(directoryToMoveResources + "\\" + this.tilesMapSelected.TilesMapName))
                    System.IO.Directory.Move(directoryToMoveResources + "\\" + this.tilesMapSelected.TilesMapName,
                        directoryToMoveResources + "\\" + newName);

                this.tilesMapSelected.TilesMapName = newName;
                GameElement elem = this.MainForm.getElementTreeView().getNodeFromObjectInstance(this.MainForm.getElementTreeView().ProjectRootNodeSelected.Nodes, this.tilesMapSelected);
                if (elem != null)
                    elem.Text = value;
            }
        }

        [Category("SIZE")]
        public int LinesCount
        {
            get { return this.tilesMapSelected.NbLines; }
            set
            {
                if (value > 3000)
                {
                    MessageBox.Show("The maximum size of a tiles map is 3000x3000 !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    value = 3000;
                }

                this.tilesMapSelected.NbLines = value;

                this.tilesMapSelected.createTilesTab();

                this.MainForm.tilesMapEditor1.setTilesMap(this.tilesMapSelected);
            }
        }

        [Category("SIZE")]
        public int ColumnsCount
        {
            get { return this.tilesMapSelected.NbColumns; }
            set
            {
                if (value > 3000)
                {
                    MessageBox.Show("The maximum size of a tiles map is 3000x3000 !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    value = 3000;
                }
                this.tilesMapSelected.NbColumns = value;

                this.tilesMapSelected.createTilesTab();
                
                this.MainForm.tilesMapEditor1.setTilesMap(this.tilesMapSelected);
                
            }
        }

        //[Category("GENERAL")]
        //public Point Location
        //{
        //    get { return this.tilesMapSelected.Location; }
        //    set
        //    {
        //        this.tilesMapSelected.Location = value;
        //    }
        //}

        [Category("SIZE")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsInfinite
        {
            get { return this.tilesMapSelected.isInfinite; }
            set
            {
                this.tilesMapSelected.isInfinite = value;
            }
        }

        [Category("PHYSICS")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool PhysicsEnabled
        {
            get { return this.tilesMapSelected.isPhysicsEnabled; }
            set { this.tilesMapSelected.isPhysicsEnabled = value; }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The bounce of the tiles.")]
        public decimal Bounce
        {
            get
            {
                return this.tilesMapSelected.Bounce;
            }
            set
            {

                this.tilesMapSelected.Bounce = value;

            }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The name of the collision filter group."), Editor(typeof(CollisionGroupEditor),
          typeof(System.Drawing.Design.UITypeEditor))]
        public string CollisionFilterGroup
        {

            get
            {
                Scene sceneParent = this.tilesMapSelected.LayerParent.SceneParent;
                if (sceneParent.CollisionFilterGroups.Count <= this.tilesMapSelected.CollisionFilterGroupIndex)
                {
                    this.tilesMapSelected.CollisionFilterGroupIndex = 0;
                    return "Default";
                }
                else
                {
                    string collisionGroupName = sceneParent.CollisionFilterGroups[this.tilesMapSelected.CollisionFilterGroupIndex].GroupName;
                    return collisionGroupName;
                }



            }
            set
            {
                Scene sceneParent = this.MainForm.getElementTreeView().SceneSelected;
                if (sceneParent != null)
                {
                    string collisionGroupName = "Default";
                    for (int i = 0; i < sceneParent.CollisionFilterGroups.Count; i++)
                    {
                        if (sceneParent.CollisionFilterGroups[i].GroupName.Equals(value))
                        {
                            this.tilesMapSelected.CollisionFilterGroupIndex = i;
                            collisionGroupName = sceneParent.CollisionFilterGroups[i].GroupName;
                            break;
                        }
                    }

                    if (collisionGroupName.Equals("Default"))
                        this.tilesMapSelected.CollisionFilterGroupIndex = 0;
                }
               
            }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The density of the tiles.")]
        public decimal Density
        {
            get
            {
                return this.tilesMapSelected.Density;
            }
            set
            {

                this.tilesMapSelected.Density = value;

            }
        }


        [Category("PHYSICS")]
        [DescriptionAttribute("The friction of the tiles.")]
        public decimal Friction
        {
            get
            {
                return this.tilesMapSelected.Friction;
            }
            set
            {

                this.tilesMapSelected.Friction = value;

            }
        }

        [Category("PHYSICS")]
        [DescriptionAttribute("The radius of the tiles.")]
        public int Radius
        {
            get
            {
                return this.tilesMapSelected.Radius;
            }
            set
            {

                this.tilesMapSelected.Radius = value;

            }
        }

        [Category("PATH FINDING")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool PathFindingEnabled
        {
            get { return this.tilesMapSelected.IsPathFindingEnabled; }
            set { this.tilesMapSelected.IsPathFindingEnabled = value; }
        }

        [Category("TILE SIZE")]
        public int TilesHeight
        {
            get { return this.tilesMapSelected.TilesHeight; }
            set
            {
                this.tilesMapSelected.TilesHeight = value;

                this.tilesMapSelected.setTilesSize(new Size(this.tilesMapSelected.TilesWidth,this.tilesMapSelected.TilesHeight));

                this.MainForm.tilesMapEditor1.setTilesMap(this.tilesMapSelected);
            }
        }

        [Category("TILE SIZE")]
        public int TilesWidth
        {
            get { return this.tilesMapSelected.TilesWidth; }
            set
            {
                this.tilesMapSelected.TilesWidth = value;

                this.tilesMapSelected.setTilesSize(new Size(this.tilesMapSelected.TilesWidth, this.tilesMapSelected.TilesHeight));

                this.MainForm.tilesMapEditor1.setTilesMap(this.tilesMapSelected);
            }
        }

    }
}
