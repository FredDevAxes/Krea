using System;
using System.Drawing;
using System.ComponentModel;
using Krea.CoronaClasses;
using System.Reflection;

namespace Krea.Corona_Classes.Controls
{
    [Serializable()]
    [ObfuscationAttribute(Exclude = true)]
    public abstract class CoronaControl
    {

        [Flags]
        public enum ControlType
        {
            joystick = 1,
            accelerometer = 2,
        }

        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        [ObfuscationAttribute(Exclude = true)]
        public ControlType type;
        public String ControlName;
        public bool isEnabled = true;
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public CoronaControl(ControlType type,String name)
        {
            this.type = type;
            this.ControlName = name;
        }

        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public abstract void Dessine(Graphics g, Point offsetPoint, float xScale, float yScale);
        public abstract void DrawGorgon(Point offsetPoint, float worldScale);
        public abstract CoronaControl Clone(CoronaLayer layerDest,bool incrementName);

        [Category("GENERAL")]
        [DescriptionAttribute("The type of the control."), ReadOnly(true)]
        public ControlType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}
