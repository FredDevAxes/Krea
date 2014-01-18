using System;
using Krea.CoronaClasses;
using System.ComponentModel;
using Krea.CGE_Figures;
using System.Drawing;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The shape representing the Corona Display Object")]
    public class FigurePropertyConverter : ObjectPropertyConverter
    {

        Figure shape ;
        CoronaObject obj;
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public FigurePropertyConverter() { }
        public FigurePropertyConverter(CoronaObject obj, Form1 MainForm) :
            base(obj,MainForm)
        {
            this.shape = obj.DisplayObject.Figure;
            this.obj = obj;
        }

         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------


        [Category("SHAPE PROPERTIES")]
        [DescriptionAttribute("The shape type of the object."),ReadOnly(true)]
        [Editor(typeof(ShapeTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String ShapeType
        {
            get { return this.shape.ShapeType; }
        }

        [Category("SHAPE PROPERTIES")]
        [DescriptionAttribute("The stroke color of the shape.")]
        public Color StrokeColor
        {
            get { return this.shape.StrokeColor; }
            set {

                this.shape.StrokeColor = value;
            }
        }

        [Category("SHAPE PROPERTIES")]
        [DescriptionAttribute("The fill color of the shape.")]
        public Color FillColor
        {
            get { return this.shape.FillColor; }
            set {

                this.shape.FillColor = value;
            }
        }

        [Category("SHAPE PROPERTIES")]
        [DescriptionAttribute("Defines if the shape should be filled by a color.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsFilled
        {
            get { return this.shape.Fill; }
            set {

                this.shape.Fill = value;
            }
        }

        [Category("SHAPE PROPERTIES")]
        [DefaultValue(2)]
        [DescriptionAttribute("The stroke size of the shape.")]
        public int StrokeSize
        {
            get { return this.shape.StrokeSize; }
            set {
                this.shape.StrokeSize = value;
            }
        }

        [Category("PHYSICS BODY")]
        [DefaultValue(0.1f)]
        [DescriptionAttribute("The bounce of the body.")]
        public decimal Bounce
        {
            get {
                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if(this.obj.PhysicsBody.isCustomizedBody == true)
                        return this.obj.PhysicsBody.BodyElements[0].Bounce;
                    else return this.obj.PhysicsBody.Bounce;

                }
                else
                    return 0;
            }
            set {

                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.obj.PhysicsBody.isCustomizedBody == true)
                    {
                        for(int i = 0;i<this.obj.PhysicsBody.BodyElements.Count;i++)
                        {
                            this.obj.PhysicsBody.BodyElements[i].Bounce = value;
                        }
                    }
                        
                    else this.obj.PhysicsBody.Bounce = value;

                }
                    

            }
        }

        [Category("PHYSICS BODY")]
        [DefaultValue(0.1f)]
        [DescriptionAttribute("The density of the body.")]
        public decimal Density
        {
            get
            {
                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.obj.PhysicsBody.isCustomizedBody == true)
                        return this.obj.PhysicsBody.BodyElements[0].Density;
                    else return this.obj.PhysicsBody.Density;

                }
                    
                else
                    return 0;
            }
            set
            {

                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.obj.PhysicsBody.isCustomizedBody == true)
                    {
                        for (int i = 0; i < this.obj.PhysicsBody.BodyElements.Count; i++)
                        {
                            this.obj.PhysicsBody.BodyElements[i].Density = value;
                        }
                    }
                    else this.obj.PhysicsBody.Density = value;
                }

            }
        }

        [Category("PHYSICS BODY")]
        [DefaultValue(0.1f)]
        [DescriptionAttribute("The friction of the body.")]
        public decimal Friction
        {
            get
            {
                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.obj.PhysicsBody.isCustomizedBody == true)
                        return this.obj.PhysicsBody.BodyElements[0].Friction;
                    else return this.obj.PhysicsBody.Friction;
                }
                else
                    return 0;
            }
            set
            {
                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.obj.PhysicsBody.isCustomizedBody == true)
                    {
                        for (int i = 0; i < this.obj.PhysicsBody.BodyElements.Count; i++)
                        {
                            this.obj.PhysicsBody.BodyElements[i].Friction = value;
                        }
                    }
                    else this.obj.PhysicsBody.Friction = value;
                }

            }
        }

        [Category("PHYSICS BODY")]
        [DefaultValue(0)]
        [DescriptionAttribute("The radius of the body.")]
        public int Radius
        {
            get
            {
                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.obj.PhysicsBody.Radius <= 0)
                    {
                        Figure fig = this.obj.DisplayObject.Figure;
                        if (fig != null)
                        {
                            if (fig.ShapeType.Equals("CIRCLE"))
                            {
                                Cercle circ = fig as Cercle;
                                this.obj.PhysicsBody.Radius = circ.Rayon;
                                return circ.Rayon;
                            }

                        }
                    }
                   
                    return this.obj.PhysicsBody.Radius;
                }
                    
                else
                    return 0;
            }
            set
            {

                if (this.obj.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                    this.obj.PhysicsBody.Radius = value;

            }
        }



      

    }


}
