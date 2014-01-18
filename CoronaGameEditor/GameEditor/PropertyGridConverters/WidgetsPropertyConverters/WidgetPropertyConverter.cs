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
    class WidgetPropertyConverter
    {

         //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private CoronaWidget widget;
        private Form1 MainForm;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public WidgetPropertyConverter(CoronaWidget widget, Form1 MainForm)
        {
            this.widget = widget;
            this.MainForm = MainForm;
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
            get { return this.widget.Name; }
            set
            {
                value = value.Replace("_", "");
                value = value.Replace(" ", "");

                this.widget.Name = value;
                GameElement elem = this.MainForm.getElementTreeView().getNodeFromObjectInstance(this.MainForm.getElementTreeView().ProjectRootNodeSelected.Nodes, this.widget);
                if (elem != null)
                    elem.Text = value;
            }
        }

        [Category("GENERAL")]
        [DescriptionAttribute("The type of the widget."), ReadOnly(true)]
        public Krea.Corona_Classes.Widgets.CoronaWidget.WidgetType Type
        {
            get
            {
                return this.widget.Type;
            }
            set
            {
                this.widget.Type = value;
            }
        }

    }
}
