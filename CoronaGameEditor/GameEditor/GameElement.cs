using System;
using System.Windows.Forms;
using System.Drawing;

namespace Krea
{
    public class GameElement : TreeNode
    {
        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        public String NodeType;
        public Object InstanceObjet;
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public GameElement(String nodeType, String name, Object instanceObjet)
        {
            InstanceObjet = instanceObjet;
            NodeType = nodeType;
            this.Name = "GAME_ELEMENT";
            this.Text = name;

            if(this.NodeType.Equals("STAGE"))
            {
                this.ForeColor = Color.Teal;
                this.ImageIndex = 3;
                this.SelectedImageIndex = 3;
            }

            else if (this.NodeType.Equals("LAYER"))
            {
                this.ForeColor = Color.DarkSlateGray;
                this.ImageIndex = 2;
                this.SelectedImageIndex = 2;
            }

            else if (this.NodeType.Equals("OBJECT"))
            {
                this.ForeColor = Color.DarkCyan;
                this.ImageIndex = 7;
                this.SelectedImageIndex = 7;
            }

            else if (this.NodeType.Equals("ENTITY"))
            {
                this.ForeColor = Color.RoyalBlue;
                this.ImageIndex = 14;
                this.SelectedImageIndex =14;
            }

            else if (this.NodeType.Equals("TILESMAP"))
            {
                this.ImageIndex = 12;
                this.SelectedImageIndex = 12;
                this.ForeColor = Color.DarkCyan;
            }
            
            else if (this.NodeType.Equals("JOINT"))
            {
                this.ImageIndex = 8;
                this.SelectedImageIndex = 8;

                this.ForeColor = Color.CadetBlue;
            }
            else if (this.NodeType.Equals("SPRITESHEET"))
            {
                this.ImageIndex = 5;
                this.SelectedImageIndex = 5;
                this.ForeColor = Color.DeepSkyBlue;
            }

            else if (this.NodeType.Equals("SPRITESET"))
            {
                this.ImageIndex = 4;
                this.SelectedImageIndex = 4;
                
                this.ForeColor = Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("AUDIO"))
            {
                this.ImageIndex = 0;
                this.SelectedImageIndex = 0;
                this.ForeColor = Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("CONTROL"))
            {
                this.ImageIndex = 10;
                this.SelectedImageIndex = 10;
                this.ForeColor = Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("WIDGET"))
            {
                this.ImageIndex = 9;
                this.SelectedImageIndex = 9;
                this.ForeColor = Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("SNIPPET"))
            {
                this.ImageIndex = 11;
                this.SelectedImageIndex = 11;
                this.ForeColor = Color.DodgerBlue;
            }

            else if (this.NodeType.Equals("FONT"))
            {
                this.ImageIndex = 13;
                this.SelectedImageIndex = 13;
                this.ForeColor = Color.DodgerBlue;
            }

                
        }

        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public void refresh()
        {
            if (this.InstanceObjet != null)
            {
                if (this.NodeType.Equals("OBJECT"))
                {
                    this.Text = this.Name;
                }
            }
        }

        public Color getFontColor()
        {
            if (this.NodeType.Equals("STAGE"))
            {
                return Color.Teal;
               
            }
            else if (this.NodeType.Equals("LAYER"))
            {
              
                return Color.DarkSlateGray;
            }

            else if (this.NodeType.Equals("OBJECT"))
            {
            
                return Color.DarkCyan;
            }
            else if (this.NodeType.Equals("ENTITY"))
            {

                return Color.RoyalBlue;
            }
            else if (this.NodeType.Equals("TILESMAP"))
            {
                return Color.DarkCyan;
            }

            else if (this.NodeType.Equals("JOINT"))
            {
                return Color.CadetBlue;
            }
            else if (this.NodeType.Equals("SPRITESHEET"))
            {
                return Color.DeepSkyBlue;
            }

            else if (this.NodeType.Equals("SPRITESET"))
            {
                return Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("AUDIO"))
            {
                return Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("CONTROL"))
            {
                return Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("WIDGET"))
            {
                return Color.DodgerBlue;
            }
            else if (this.NodeType.Equals("SNIPPET"))
            {
                return Color.DodgerBlue;
            }

            return Color.Black;
        }

        
        
    }
}
