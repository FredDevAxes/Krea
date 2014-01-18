using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krea.GameEditor.TilesMapping;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Krea.Corona_Classes.Widgets;
using System.Reflection;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    class TabBarPropertyConverter :WidgetPropertyConverter
    {

         //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private WidgetTabBar tabBar;
        private Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public TabBarPropertyConverter(CoronaWidget widget, Form1 MainForm):
            base(widget,MainForm)
        {
            this.tabBar = (WidgetTabBar)widget;
        }

      
        //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------

        //----------------------------------------------------------
        //--------------------- ACCESSEURS --------------------------
        //----------------------------------------------------------
        [Category("GENERAL")]
        [DescriptionAttribute("The location of the widget.")]
        public Point Location
        {
            get
            {
                return this.tabBar.Location;
            }
            set
            {
                this.tabBar.Location = value;
            }
        }

        [Category("GENERAL")]
        [DescriptionAttribute("The size of the widget.")]
        public Size Size
        {
            get
            {
                return this.tabBar.Size;
            }
            set
            {
                this.tabBar.Size = value;
            }
        }

        [Category("COLOR")]
        [DescriptionAttribute("The bottom fill color of the widget.")]
        public Color BottomFillColor
        {
            get
            {
                return this.tabBar.BottomFillColor;
            }
            set
            {
                this.tabBar.BottomFillColor = value;
            }
        }

        [Category("COLOR")]
        [DescriptionAttribute("The bottom fill alpha of the widget.")]
        public int BottomFillAlpha
        {
            get
            {
                return this.tabBar.BottomFillAlpha;
            }
            set
            {
                this.tabBar.BottomFillAlpha = value;
            }
        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The gradient direction.")]
        public Krea.Corona_Classes.GradientColor.Direction Direction
        {
            get
            {
                return this.tabBar.GradientForTup.direction;
            }

            set
            {
                this.tabBar.GradientForTup.direction = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("Is the gradient enabled for the top half part.")]
        public bool GradientEnabled
        {
            get
            {
                return this.tabBar.GradientForTup.isEnabled;
            }

            set
            {

                this.tabBar.GradientForTup.isEnabled = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The first color of the gradient.")]
        public Color Color1
        {
            get
            {
                return this.tabBar.GradientForTup.color1;
            }

            set
            {
                this.tabBar.GradientForTup.color1 = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The second color of the gradient.")]
        public Color Color2
        {
            get
            {
                return this.tabBar.GradientForTup.color2;
            }

            set
            {
                this.tabBar.GradientForTup.color2 = value;
            }

        }

    }
}
