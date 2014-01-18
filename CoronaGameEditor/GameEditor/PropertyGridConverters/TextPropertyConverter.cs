using System;
using Krea.CoronaClasses;
using System.ComponentModel;
using Krea.CGE_Figures;
using System.Drawing;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;
using Krea.GameEditor.FontManager;
using System.ComponentModel.Design;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The shape representing the Corona Display Object")]
    public class TextPropertyConverter : ObjectPropertyConverter
    {
        CoronaObject objSelected;
        Texte text ;

         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public TextPropertyConverter() { }
        public TextPropertyConverter(CoronaObject obj, Form1 MainForm) :
            base(obj,MainForm)
        {
            objSelected = obj;
            this.text = obj.DisplayObject.Figure as Texte;
        }

        public CoronaObject GetObjectSelected()
        {
            return this.objSelected;
        }
         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------


        [Category("TEXT PROPERTIES")]
        [DescriptionAttribute("The object type."),ReadOnly(true)]
        [Editor(typeof(ShapeTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String ShapeType
        {
            get { return this.text.ShapeType; }
        }

        [Category("TEXT PROPERTIES")]
        [DescriptionAttribute("The current text of the object.")]
        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String Text
        {
            get { return this.text.Txt; }
            set {

 

                //Recuperer la clé du translate
                bool isFound = false;
                CoronaGameProject project = this.objSelected.LayerParent.SceneParent.projectParent;
                for(int i = 0; i< project.Langues.Count;i++)
                {
                    LangueObject langue = project.Langues[i];
                    if (langue.Langue.Equals(project.DefaultLanguage))
                    {
                        for (int j = 0; j < langue.TranslationElement.Count; j++)
                        {
                            LangueElement elem = langue.TranslationElement[j];
                            if (elem.Key.Equals(this.text.txt))
                            {
                                elem.Key = value;
                                elem.Translation = value;
                                isFound = true;
                                break;
                            }

                        }

                        if (isFound == false)
                        {
                            LangueElement elem = new LangueElement(value, value);
                            langue.TranslationElement.Add(elem);
                        }

                        break;
                    }
                }
                
                this.text.Txt = value;

                this.text.LastPos = Point.Empty;
                this.text.SetSizeFromPoint(Point.Empty);
            }
        }

        [Category("TEXT PROPERTIES")]
        [DefaultValue(12)]
        [AmbientValue(typeof(string), "int/6,78")]
        //[Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DescriptionAttribute("The Text size.")]
        public float TextSize
        {
            get { return this.text.font2.Size; }
            set
            {
                float finalValue = (float)Convert.ToInt32(value);
                if(finalValue < 6) finalValue = 6;

                this.text.font2.Size = finalValue;
                this.text.LastPos = Point.Empty;
                this.text.SetSizeFromPoint(Point.Empty);
            }

        }


        [Category("TEXT PROPERTIES")]
        [DescriptionAttribute("The Text Font."),Editor(typeof(FontEditor),
          typeof(System.Drawing.Design.UITypeEditor))]
        public FontItem Font
        {
            get {

                if (this.text.font2.FontItem == null)
                    this.text.font2.FontItem = new FontItem("DEFAULT", this.text.DisplayObjectParent.CoronaObjectParent.LayerParent.SceneParent.projectParent);

                return this.text.font2.FontItem; }
            set
            {
                if (value != null)
                {
                    this.text.font2.FontItem = value;
                    this.text.font2.FamilyName = value.FontFamilyName;

                    
                    this.text.LastPos = Point.Empty;
                    this.text.SetSizeFromPoint(Point.Empty);
                }
                    
            }
        }

       [Category("TEXT PROPERTIES")]
        [DescriptionAttribute("The Text color.")]
        public Color TextColor
        {
            get { return this.text.FillColor; }
            set {

                this.text.FillColor = value;
            }
        }


       /* [Category("TEXT PROPERTIES")]
        [DescriptionAttribute("The text size.")]
        public float TextSize
        {
            get { return this.text.font2.Size; }
            set {

                this.text.font2.Size = value;
                int width = Convert.ToInt32(text.font2.Size * (text.txt.Length / 1.5));
                int height = Convert.ToInt32(text.font2.Size * 1.5);
                this.objSelected.DisplayObject.SurfaceRect = new Rectangle(text.Position.X, text.Position.Y, width, height); 
            }
        }*/

        [Category("PHYSIC BODY")]
       [DefaultValue(0)]
       [AmbientValue(typeof(string), "decimal/0,1000")]
        [DescriptionAttribute("The bounce of the body.")]
        public decimal Bounce
        {
            get
            {
                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.objSelected.PhysicsBody.isCustomizedBody == true)
                        return this.objSelected.PhysicsBody.BodyElements[0].Bounce;
                    else return this.objSelected.PhysicsBody.Bounce;

                }
                else
                    return 0;
            }
            set
            {

                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.objSelected.PhysicsBody.isCustomizedBody == true)
                        this.objSelected.PhysicsBody.BodyElements[0].Bounce = value;
                    else this.objSelected.PhysicsBody.Bounce = value;

                }


            }
        }

        [Category("PHYSIC BODY")]
        [DescriptionAttribute("The density of the body.")]
        public decimal Density
        {
            get
            {
                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.objSelected.PhysicsBody.isCustomizedBody == true)
                        return this.objSelected.PhysicsBody.BodyElements[0].Density;
                    else return this.objSelected.PhysicsBody.Density;

                }

                else
                    return 0;
            }
            set
            {

                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.objSelected.PhysicsBody.isCustomizedBody == true)
                        this.objSelected.PhysicsBody.BodyElements[0].Density = value;
                    else this.objSelected.PhysicsBody.Density = value;
                }

            }
        }

       [Category("PHYSIC BODY")]
        [DescriptionAttribute("The friction of the body.")]
        public decimal Friction
        {
            get
            {
                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.objSelected.PhysicsBody.isCustomizedBody == true)
                        return this.objSelected.PhysicsBody.BodyElements[0].Friction;
                    else return this.objSelected.PhysicsBody.Friction;
                }
                else
                    return 0;
            }
            set
            {

                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    if (this.objSelected.PhysicsBody.isCustomizedBody == true)
                        this.objSelected.PhysicsBody.BodyElements[0].Friction = value;
                    else this.objSelected.PhysicsBody.Friction = value;
                }

            }
        }

        [Category("PHYSIC BODY")]
        [DescriptionAttribute("The radius of the body.")]
        public int Radius
        {
            get
            {
                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    return this.objSelected.PhysicsBody.Radius;
                }

                else
                    return 0;
            }
            set
            {

                if (this.objSelected.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Inactive)
                    this.objSelected.PhysicsBody.Radius = value;

            }
        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The gradient direction of the object.")]
        [Editor(typeof(GradientDirectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Krea.Corona_Classes.GradientColor.Direction Direction
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.direction;
            }

            set
            {

                this.objSelected.DisplayObject.GradientColor.direction = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("Is the gradient is enabled for this object.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool GradientEnabled
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.isEnabled;
            }

            set
            {

                this.objSelected.DisplayObject.GradientColor.isEnabled = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The first color of the gradient.")]
        public Color Color1
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.color1;
            }

            set
            {


                this.objSelected.DisplayObject.GradientColor.color1 = value;
            }

        }

        [Category("GRADIENT")]
        [DescriptionAttribute("The second color of the gradient.")]
        public Color Color2
        {
            get
            {
                if (this.objSelected.DisplayObject.GradientColor == null)
                    this.objSelected.DisplayObject.GradientColor = new Corona_Classes.GradientColor();
                return this.objSelected.DisplayObject.GradientColor.color2;
            }

            set
            {

                this.objSelected.DisplayObject.GradientColor.color2 = value;
            }

        }

    }
}
