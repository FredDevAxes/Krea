using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
using Krea.GameEditor.PropertyGridConverters;

namespace Krea.GameEditor
{
    public partial class PhysicsBodySettings : UserControl
    {
         //---------------------------------------------------
        //-------------------Attributs--------------------
        //---------------------------------------------------
        private Form1 MainForm;
        private CoronaObject CoronaObject;

        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------

        public PhysicsBodySettings()
        {
            InitializeComponent();
        }


        //---------------------------------------------------
        //-------------------Methodes--------------------
        //---------------------------------------------------
        public void init(Form1 mainForm, CoronaObject objectSelected)
        {
            this.MainForm = mainForm;
            this.CoronaObject = objectSelected;
            reloadPanel();
        }


        public void reloadPanel()
        {
            if (this.CoronaObject != null)
            {
                //Reinit entierement le panneau
               
                this.bodyElementsListView.Items.Clear();

                this.bodyElementsPropertyGrid.SelectedObjects = null;
                for (int i = 0; i < this.CoronaObject.PhysicsBody.BodyElements.Count; i++)
                {
                    BodyElement bodyElement = this.CoronaObject.PhysicsBody.BodyElements[i];
                    ListViewItem bodyItem = new ListViewItem();
                    bodyItem.Name = bodyElement.Name;
                    bodyItem.Text = bodyElement.Name;
                    bodyItem.Tag = bodyElement;
                    
                    if (bodyElement.Type.Equals("CIRCLE"))
                    {
                        bodyItem.ImageIndex = 0;
                    }
                    else
                    {
                        bodyItem.ImageIndex = 1;
                    }


                    this.bodyElementsListView.Items.Add(bodyItem);
                }

                if (this.CoronaObject.PhysicsBody.BodyElements.Count > 0)
                    this.CoronaObject.PhysicsBody.isCustomizedBody = true;
                else
                    this.CoronaObject.PhysicsBody.isCustomizedBody = false;

            }

            if (this.MainForm.isFormLocked == false)
                GorgonLibrary.Gorgon.Go();
        }

        public void addBodyElements(List<BodyElement> elements)
        {
            if (this.CoronaObject != null && elements != null)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    this.CoronaObject.PhysicsBody.BodyElements.Add(elements[i]);
                }
                this.reloadPanel();
            }
           
        }

        public void validerBodyShape()
        {
            if (this.MainForm.physicBodyEditorView1.DrawMode.Equals("SHAPE"))
            {
                //Recuperer la shape convertie

                List<BodyElement> elements = this.MainForm.physicBodyEditorView1.getTriangulatedShapes();
                if (elements != null)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        BodyElement elem = elements[i];


                        this.CoronaObject.PhysicsBody.BodyElements.Add(elements[i]);
                    }
                }
                else
                {
                    MessageBox.Show("The polygon cannot be created! Please try again.", "Invalid Polygon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else if (this.MainForm.physicBodyEditorView1.DrawMode.Equals("HAND"))
            {
                //Recuperer la shape convertie
                List<BodyElement> elements = this.MainForm.physicBodyEditorView1.getBodyLinesFromPoints();
                if (elements != null)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        BodyElement elem = elements[i];


                        this.CoronaObject.PhysicsBody.BodyElements.Add(elements[i]);
                    }
                }

            }

            this.MainForm.physicBodyEditorView1.setModeNormal();
            this.reloadPanel();


        }

        private void bodyElementsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bodyElementsPropertyGrid.SelectedObjects = null;
            if (this.bodyElementsListView.SelectedIndices.Count > 0)
            {
                BodyElementPropertyConverter[] tabConverter =
                    new BodyElementPropertyConverter[this.bodyElementsListView.SelectedIndices.Count];

                for (int i = 0; i < this.bodyElementsListView.SelectedItems.Count; i++)
                {
                    ListViewItem viewItem = this.bodyElementsListView.SelectedItems[i];
                    BodyElement elem = viewItem.Tag as BodyElement;
                    BodyElementPropertyConverter converter = new BodyElementPropertyConverter(elem, viewItem,this.CoronaObject,this.MainForm);
                    tabConverter[i] = converter;
                }

                this.bodyElementsPropertyGrid.SelectedObjects = tabConverter;
            }

            GorgonLibrary.Gorgon.Go();
        }

        public List<BodyElement> GetSelectedBodyElements()
        {
            List<BodyElement> bodyElements = new List<BodyElement>();

            for (int i = 0; i < this.bodyElementsListView.SelectedItems.Count; i++)
            {
                BodyElement elem = this.bodyElementsListView.SelectedItems[i].Tag as BodyElement;
                bodyElements.Add(elem);
            }

            return bodyElements;
        }

        private void removeBodyELementBt_Click(object sender, EventArgs e)
        {
            if (this.bodyElementsListView.SelectedItems.Count > 0)
            {
                for (int i = 0; i < this.bodyElementsListView.SelectedItems.Count; i++)
                {
                    this.CoronaObject.PhysicsBody.BodyElements.Remove((BodyElement)this.bodyElementsListView.SelectedItems[i].Tag);

                }
                this.reloadPanel();

            }
        }
    }
}
