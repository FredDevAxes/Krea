using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using Krea.CoronaClasses;
using System.Drawing.Drawing2D;

namespace Krea.Corona_Classes.Widgets
{
    [Serializable()]
    public class WidgetPickerWheel : CoronaWidget
    {
        //----------------------------------------------------------
        //--------------------- ATTIBUTS --------------------------
        //----------------------------------------------------------
        private Point location = new Point(0, 0);
        private Size size = new Size(296, 222);
        private Image backgroundImage;
        private Size backgroundSize;
        private int selectionHeight = 46;
        private string fontName ="";
        private int fontSize = 22;
        private Color fontColor = Color.Black;
        private int fontAlpha = 255;
        private Color columnColor = Color.White;
        private int columnAlpha = 255;
        private int totalWidth = 296;
        public List<PickerWheelColumn> Columns;
        
        //----------------------------------------------------------
        //--------------------- CONSTRUCTEURS ----------------------
        //----------------------------------------------------------
        public WidgetPickerWheel(string name, CoronaLayer layerParent)
            :base(name,WidgetType.pickerWheel,layerParent)
        {
            this.Columns = new List<PickerWheelColumn>();
        }

        //----------------------------------------------------------
        //--------------------- METHODES --------------------------
        //----------------------------------------------------------
        public override void Dessine(Graphics g, Point offsetPoint, float XScale, float YScale)
        {
            Matrix m = new Matrix();
            m.Scale(XScale, YScale);
            g.Transform = m;

            
        }

       
        //----------------------------------------------------------
        //--------------------- ACCESSEURS --------------------------
        //----------------------------------------------------------
        [Category("GENERAL")]
        [DescriptionAttribute("The location of the widget.")]
        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        [Category("GENERAL")]
        [DescriptionAttribute("The size of the widget.")]
        public Size Size 
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        [Category("GENERAL")]
        [DescriptionAttribute("The selection height of the widget.")]
        public int SelectionHeight
        {
            get
            {
                return this.selectionHeight;
            }
            set
            {
                this.selectionHeight = value;
            }
        }

        [Category("BACKGROUND")]
        [DescriptionAttribute("The background image of the widget.")]
        public Image BackgroundImage
        {
            get
            {
                return this.backgroundImage;
            }
            set
            {
                this.backgroundImage = value;
                if(value != null)
                    this.backgroundSize =this.BackgroundImage.Size;
            }
        }

        [Category("BACKGROUND")]
        [DescriptionAttribute("The background size of the widget.")]
        public Size BackgroundSize
        {
            get
            {
                return this.backgroundSize;
            }
            set
            {
                this.backgroundSize = value;
             
            }
        }

        [Category("BACKGROUND")]
        [DescriptionAttribute("The total width you want the entire background to be stretched to.")]
        public int TotalWidth
        {
            get
            {
                return this.totalWidth;
            }
            set
            {
                this.totalWidth = value;

            }
        }

        [Category("FONT")]
        [DescriptionAttribute("The font name of the widget.")]
        public string FontName
        {
            get
            {
                return this.fontName;
            }
            set
            {
                this.fontName = value;
                
            }
        }
        [Category("FONT")]
        [DescriptionAttribute("The font size of the widget.")]
        public int FontSize
        {
            get
            {
                return this.fontSize;
            }
            set
            {
                this.fontSize = value;

            }
        }

        [Category("FONT")]
        [DescriptionAttribute("The font color of the widget.")]
        public Color FontColor
        {
            get
            {
                return this.fontColor;
            }
            set
            {
                this.fontColor = value;

            }
        }
        [Category("FONT")]
        [DescriptionAttribute("The font alpha of the widget.")]
        public int FontAlpha
        {
            get
            {
                return this.fontAlpha;
            }
            set
            {
                this.fontAlpha = value;

            }
        }

        [Category("COLUMNS")]
        [DescriptionAttribute("The columns color of the widget.")]
        public Color ColumnColor
        {
            get
            {
                return this.columnColor;
            }
            set
            {
                this.columnColor = value;

            }
        }
        [Category("COLUMNS")]
        [DescriptionAttribute("The columns alpha of the widget.")]
        public int ColumnAlpha
        {
            get
            {
                return this.columnAlpha;
            }
            set
            {
                this.columnAlpha = value;

            }
        }
      
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //--------------------- SUB_CLASS TabBarButton -----------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable()]
        public class PickerWheelColumn
        {
            public enum ColumnAlignement
            {
                center = 1,
                right = 2,
                left = 3,
                
            }
            //----------------------------------------------------------
            //--------------------- ATTIBUTS --------------------------
            //----------------------------------------------------------
            public string ColumnName;
            public List<string> Datas;
            public int Width;
            public int StartIndex = 0;
            public ColumnAlignement Alignement;

            //----------------------------------------------------------
            //--------------------- CONSTRUCTEUR -----------------------
            //----------------------------------------------------------

            public PickerWheelColumn(string columnName,int startIndex, int width,ColumnAlignement alignement)
            {
                this.Datas = new List<string>();
                this.StartIndex = startIndex;
                this.Width = width;
                this.Alignement = alignement;
                this.ColumnName = columnName;
            }

            public override string ToString()
            {
                return this.ColumnName;
            }
        }
    }
}
