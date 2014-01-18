using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using Krea.CoronaClasses;

namespace Krea.Corona_Classes.Widgets
{
    [Serializable()]
    public abstract class CoronaWidget
    {
       [Flags]
        public enum WidgetType
        {
            button = 1,
            tableView = 2,
            scrollView = 3,
            pickerWheel = 4,
            tabBar = 5,
        }
        //----------------------------------------------------------
        //--------------------- ATTIBUTS --------------------------
        //----------------------------------------------------------
        public string Name;
        public WidgetType Type;
        public CoronaLayer LayerParent;

        //----------------------------------------------------------
        //--------------------- CONSTRUCTEURS ----------------------
        //----------------------------------------------------------
        public CoronaWidget(string name, WidgetType type, CoronaLayer layerParent)
        {
            this.Name = name;
            this.Type = type;
            this.LayerParent = layerParent;
        }

        //----------------------------------------------------------
        //--------------------- METHODES --------------------------
        //----------------------------------------------------------
        public abstract void Dessine(Graphics g, Point offsetPoint, float XScale, float YScale);

        

    }
}
