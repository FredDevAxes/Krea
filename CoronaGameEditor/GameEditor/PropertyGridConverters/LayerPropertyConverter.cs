using System;
using Krea.CoronaClasses;
using System.ComponentModel;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    class LayerPropertyConverter
    {

        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private CoronaLayer layerSelected;
        private Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public LayerPropertyConverter(CoronaLayer layer, Form1 MainForm)
        {
            this.layerSelected = layer;
            this.MainForm = MainForm;
        }

        public CoronaLayer GetLayerSelected()
        {
            return this.layerSelected;
        }
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
        [Category("GENERAL")]
        [DescriptionAttribute("The name of the layer.")]
        public String Name
        {
            get { return this.layerSelected.Name; }
            set
            {
               // this.MainForm.UndoRedo.DO(this.layerSelected.SceneParent);

                this.layerSelected.Name = value;
                GameElement elem = this.MainForm.getElementTreeView().getNodeFromObjectInstance(this.MainForm.getElementTreeView().ProjectRootNodeSelected.Nodes, this.layerSelected);
                if (elem != null)
                    elem.Text = value;
            }
        }

        [Category("CAMERA DRAGGING")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DescriptionAttribute("Does the layer should move when camera is dragged.")]
        public bool LayerDragged
        {
            get { return this.layerSelected.isDraggableByCamera; }
            set
            {
                this.layerSelected.isDraggableByCamera = value;
            }
        }


    }
}
