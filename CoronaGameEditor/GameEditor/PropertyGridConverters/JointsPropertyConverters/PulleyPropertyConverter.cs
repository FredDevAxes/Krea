using System;
using System.ComponentModel;
using System.Drawing;
using Krea.CoronaClasses;
using System.Reflection;

namespace Krea.GameEditor.PropertyGridConverters.JointsPropertyConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    class PulleyPropertyConverter
    {
        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private CoronaJointure jointSelected;
        private Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public PulleyPropertyConverter() { }
        public PulleyPropertyConverter(CoronaJointure joint, Form1 MainForm)
        {
            this.jointSelected = joint;
            this.MainForm = MainForm;
           
        }
        public CoronaJointure GetJointSelected()
        {
            return this.jointSelected;
        }
        //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------


        [Category("General Properties")]
        [DescriptionAttribute("The joint name.")]
        public String JointName
        {
            get
            {
                return this.jointSelected.name;
            }

            set
            {
                this.jointSelected.name = value;
                GameElement elem = this.MainForm.getElementTreeView().getNodeFromObjectInstance(this.MainForm.getElementTreeView().ProjectRootNodeSelected.Nodes, this.jointSelected);
                if (elem != null)
                    elem.Text = value;
                
            }
        }

        [Category("General Properties")]
        [DescriptionAttribute("The joint type."),ReadOnly(true)]
        public String JointType
        {
            get
            {
                return this.jointSelected.type;
            }
        }

  

        [Category("Targeted Objects")]
        [DescriptionAttribute("The name of the object A"), ReadOnly(true)]
        public String ObjectAName
        {
            get
            {
                return this.jointSelected.coronaObjA.DisplayObject.Name;
            }
        }

        [Category("Targeted Objects")]
        [DescriptionAttribute("The name of the object B"), ReadOnly(true)]
        public String ObjectBName
        {
            get
            {
                return this.jointSelected.coronaObjB.DisplayObject.Name;
            }
        }

        [Category("Anchors")]
        [DescriptionAttribute("The anchor point inside the object A.")]
        public Point AnchorPointObjectA
        {
            get
            {
                return this.jointSelected.ObjectAnchorPointA;
            }

            set
            {
                this.jointSelected.ObjectAnchorPointA = value;
            }
        }
        [Category("Anchors")]
        [DescriptionAttribute("The anchor point inside the object B.")]
        public Point AnchorPointObjectB
        {
            get
            {
                return this.jointSelected.ObjectAnchorPointB;
            }

            set
            {
                this.jointSelected.ObjectAnchorPointB = value;
            }
        }

        [Category("Anchors")]
        [DescriptionAttribute("The rope anchor point A.")]
        public Point RopeAnchorPointA
        {
            get
            {
                return this.jointSelected.AnchorPointA;
            }

            set
            {
                this.jointSelected.AnchorPointA = value;
            }
        }

        [Category("Anchors")]
        [DescriptionAttribute("The rope anchor point B.")]
        public Point RopeAnchorPointB
        {
            get
            {
                return this.jointSelected.AnchorPointB;
            }

            set
            {
                this.jointSelected.AnchorPointB = value;
            }
        }
    }
}
