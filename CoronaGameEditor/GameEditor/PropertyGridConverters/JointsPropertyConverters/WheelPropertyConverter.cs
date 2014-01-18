using System;
using System.ComponentModel;
using System.Drawing;
using Krea.CoronaClasses;
using System.Reflection;

namespace Krea.GameEditor.PropertyGridConverters.JointsPropertyConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    class WheelPropertyConverter
    {
          //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private CoronaJointure jointSelected;
        private Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public WheelPropertyConverter() { }
        public WheelPropertyConverter(CoronaJointure joint, Form1 MainForm)
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

        [Category("Motor Properties")]
        [DescriptionAttribute("Is the motor enabled.")]
        public bool IsMotorEnabled
        {
            get
            {
                return this.jointSelected.isMotorEnable;
            }

            set
            {
                this.jointSelected.isMotorEnable = value;
            }
        }

        [Category("Motor Properties")]
        [DescriptionAttribute("The motor speed.")]
        public double MotorSpeed
        {
            get
            {
                return this.jointSelected.motorSpeed;
            }

            set
            {
                this.jointSelected.motorSpeed = value;
            }
        }

      

        [Category("Motor Properties")]
        [DescriptionAttribute("The max motor force.")]
        public double MaxMotorTorque
        {
            get
            {
                return this.jointSelected.maxMotorForce;
            }

            set
            {
                this.jointSelected.maxMotorForce = value;
            }
        }

        

        [Category("Spring")]
        [DescriptionAttribute("The spring frequency in Hz.")]
        public int SpringFrequency
        {
            get
            {
                return this.jointSelected.springFrequency;
            }

            set
            {
                this.jointSelected.springFrequency = value;
            }
        }

        [Category("Spring")]
        [DescriptionAttribute("The spring damping ratio (0 to 1).")]
        public double SpringDampingRatio
        {
            get
            {
                return this.jointSelected.springDampingRatio;
            }

            set
            {
                this.jointSelected.springDampingRatio = value;
            }
        }


        //[Category("Limit Properties")]
        //[DescriptionAttribute("Is limit enabled.")]
        //public bool IsLimitEnabled
        //{
        //    get
        //    {
        //        return this.jointSelected.isLimitEnabled;
        //    }

        //    set
        //    {
        //        this.jointSelected.isLimitEnabled = value;
        //    }
        //}

        //[Category("Limit Properties")]
        //[DescriptionAttribute("The lower limit.")]
        //public double LowerLimit
        //{
        //    get
        //    {
        //        return this.jointSelected.lowerLimit;
        //    }

        //    set
        //    {
        //        this.jointSelected.lowerLimit = value;
        //    }
        //}

        //[Category("Limit Properties")]
        //[DescriptionAttribute("The upper limit.")]
        //public double UpperLimit
        //{
        //    get
        //    {
        //        return this.jointSelected.upperLimit;
        //    }

        //    set
        //    {
        //        this.jointSelected.upperLimit = value;
        //    }
        //}

        [Category("Anchors")]
        [DescriptionAttribute("The anchor point.")]
        public Point AnchorPoint
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
        [DescriptionAttribute("The axis distance point.")]
        public Point AxisDistance
        {
            get
            {
                return this.jointSelected.axisDistance;
            }

            set
            {
                this.jointSelected.axisDistance = value;
            }
        }
    }
}
