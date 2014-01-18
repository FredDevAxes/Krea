using System.ComponentModel;
using System.Drawing;
using Krea.Corona_Classes.Controls;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;
using Krea.CoronaClasses;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The joystick object.")]
    public class JoystickPropertyConverter
    {

        Form1 mainForm;
        JoystickControl joy;
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public JoystickPropertyConverter() { }
        public JoystickPropertyConverter(JoystickControl joy, Form1 MainForm)
        {
            this.joy = joy;
            this.mainForm = MainForm;
        }

        //---------------------------------------------------
        //-------------------Accesseurs--------------------
        //---------------------------------------------------
        public JoystickControl GetJoystickSelected()
        {
            return this.joy;
        }

        [Category("GENERAL")]
        [DescriptionAttribute("The name of the joystick.")]
        public string JoystickName
        {
            get
            {
                return this.joy.joystickName;
            }
            set
            {
                //value = value.Replace("_", "");
                value = value.Replace(" ", "");

                this.joy.joystickName = value;
                this.joy.ControlName = value;
                GameElement elem = this.mainForm.getElementTreeView().getNodeFromObjectInstance(this.mainForm.getElementTreeView().ProjectRootNodeSelected.Nodes, this.joy);
                if (elem != null)
                    elem.Text = value;

            }
        }

        [Category("DISPLAY")]
        [DescriptionAttribute("The outer image of the joystick.")]
        public Image OuterImage
        {
            get
            {
                return this.joy.outerImage;
            }
            set
            {
                if (value.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                {

                    this.joy.outerImage = value;
                }
                else
                {
                    System.Windows.MessageBox.Show("Only PNG image format is allowed!", "Loading Image failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }

            }
        }



        [Category("DISPLAY")]
        [DescriptionAttribute("The inner image of the joystick.")]
        public Image InnerImage
        {
            get
            {
                return this.joy.innerImage;
            }
            set
            {
                if (value.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                {

                    this.joy.innerImage = value;
                }
                else
                {
                    System.Windows.MessageBox.Show("Only PNG image format is allowed!", "Loading Image failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
            }
        }

        [Category("DISPLAY")]
        [DescriptionAttribute("The outer radius of the joystick.")]
        [DefaultValue(30)]
        public int OuterRadius
        {
            get
            {
                return this.joy.outerRadius;
            }
            set
            {
                this.joy.outerRadius = value;
            }
        }

        [Category("DISPLAY")]
        [DefaultValue(15)]
        [DescriptionAttribute("The inner radius of the joystick.")]
        public int InnerRadius
        {
            get
            {
                return this.joy.innerRadius;
            }
            set
            {
                this.joy.innerRadius = value;
            }
        }

        [Category("DISPLAY")]
        [DescriptionAttribute("The outer alpha of the joystick.")]
        public float OuterAlpha
        {
            get
            {
                return this.joy.outerAlpha;
            }
            set
            {
                this.joy.outerAlpha = value;
            }
        }

        [Category("DISPLAY")]
        [DescriptionAttribute("The inner alpha of the joystick.")]
        public float InnerAlpha
        {
            get
            {
                return this.joy.innerAlpha;
            }
            set
            {
                this.joy.innerAlpha = value;
            }
        }



        [Category("DISPLAY")]
        [DescriptionAttribute("The background alpha of the joystick.")]
        public float JoystickAlpha
        {
            get
            {
                return this.joy.joystickAlpha;
            }
            set
            {
                this.joy.joystickAlpha = value;
            }
        }

        [Category("DISPLAY")]
        [DescriptionAttribute("The alpha of the joystick ghost on touching.")]
        public int Ghost
        {
            get
            {
                return this.joy.ghost;
            }
            set
            {
                this.joy.ghost = value;
            }
        }


        [Category("LOCATION")]
        [DescriptionAttribute("The location of the joystick.")]
        public Point JoystickLocation
        {
            get
            {
                return this.joy.joystickLocation;
            }
            set
            {
                this.joy.joystickLocation = value;
            }
        }

        [Category("FADING")]
        [DescriptionAttribute("The fade delay the joystick.")]
        public int FadeDelay
        {
            get
            {
                return this.joy.joystickFadeDelay;
            }
            set
            {
                this.joy.joystickFadeDelay = value;
            }
        }

        [Category("FADING")]
        [DescriptionAttribute("Is joystick fading enabled.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsFading
        {
            get
            {
                return this.joy.joystickFade;
            }
            set
            {
                this.joy.joystickFade = value;
            }
        }
      

    }


}
