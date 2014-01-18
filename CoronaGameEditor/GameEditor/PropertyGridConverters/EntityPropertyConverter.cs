using System;
using Krea.CoronaClasses;
using System.ComponentModel;
using System.Drawing;
using Krea.CGE_Figures;
using System.Drawing.Drawing2D;
using Krea.Corona_Classes;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EntityPropertyConverter
    {

        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private CoronaObject selectedObject;
        public Form1 MainForm;

     
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public EntityPropertyConverter() { }
        public EntityPropertyConverter(CoronaObject obj, Form1 MainForm)
        {
            this.selectedObject = obj;
            this.MainForm = MainForm;
        }
        public CoronaObject GetObjectSelected()
        {
            return this.selectedObject;
        }
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------


        //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------


        [Category("GENERAL")]
        [DescriptionAttribute("The name of the object.")]
        public String Name
        {
            get { return this.selectedObject.Entity.Name; }
            set
            {

                value = value.Replace("_", "").Replace(" ", "");
                value = value.Replace(" ", "").Replace(" ", "");

                value = this.MainForm.getElementTreeView().ProjectSelected.IncrementObjectName(value);
                this.selectedObject.Entity.Name = value;
                GameElement elem= this.MainForm.getElementTreeView().getNodeFromObjectInstance(this.MainForm.getElementTreeView().ProjectRootNodeSelected.Nodes,this.selectedObject);
                if (elem != null)
                    elem.Text = value;
            }
        }

        [Category("GENERATOR")]
        [DescriptionAttribute("Is the object is a generator.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsGenerator
        {
            get
            {
                return this.selectedObject.isGenerator;
            }

            set
            {


                this.selectedObject.isGenerator = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("Does the cloned object should to be inserted at the end of the display group parent.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool InsertCloneAtEndOfGroup
        {
            get
            {
                return this.selectedObject.insertCloneAtEndOfGroup;
            }

            set
            {


                this.selectedObject.insertCloneAtEndOfGroup = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("The generator delay between two creations.")]
        public int Delay
        {
            get
            {
                return this.selectedObject.generatorDelay;
            }

            set
            {


                this.selectedObject.generatorDelay = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("The generator iteration.")]
        public int Iteration
        {
            get
            {
                return this.selectedObject.generatorIteration;
            }

            set
            {


                this.selectedObject.generatorIteration = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("The Fade In speed in milliseconds.")]
        public int FadeInSpeed
        {
            get
            {
                return this.selectedObject.FadeInSpeed;
            }

            set
            {


                this.selectedObject.FadeInSpeed = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("The Fade Out speed in milliseconds.")]
        public int FadeOutSpeed
        {
            get
            {
                return this.selectedObject.FadeOutSpeed;
            }

            set
            {


                this.selectedObject.FadeOutSpeed = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("Does the object generated should be removed after fade out.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool RemoveOnCompleteFadeOut
        {
            get
            {
                return this.selectedObject.RemoveOnCompleteFadeOut;
            }

            set
            {


                this.selectedObject.RemoveOnCompleteFadeOut = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("The emission type of the generator.")]
        [Editor(typeof(GeneratorEmissionTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Krea.CoronaClasses.CoronaObject.GeneratorEmissionType EmissionType
        {
            get
            {
                return this.selectedObject.generatorEmissionType;
            }

            set
            {


                this.selectedObject.generatorEmissionType = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("A linear X impulse for the objects generated.")]
        public float XLinearImpulse
        {
            get
            {
                return this.selectedObject.GeneratorXImpulse;
            }

            set
            {


                this.selectedObject.GeneratorXImpulse = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("A linear Y impulse for the objects generated.")]
        public float YLinearImpulse
        {
            get
            {
                return this.selectedObject.GeneratorYImpulse;
            }

            set
            {


                this.selectedObject.GeneratorYImpulse = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("An angular impulse for the objects generated.")]
        public int AngularImpulse
        {
            get
            {
                return this.selectedObject.GeneratorAngularImpulse;
            }

            set
            {


                this.selectedObject.GeneratorAngularImpulse = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("The delay in milliseconds between Fade In and Fade Out.")]
        public int DelayBetweenFades
        {
            get
            {
                return this.selectedObject.DelayBetweenFades;
            }

            set
            {


                this.selectedObject.DelayBetweenFades = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("Is the Fade In enabled.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool FadeInEnabled
        {
            get
            {
                return this.selectedObject.FadeInEnabled;
            }

            set
            {


                this.selectedObject.FadeInEnabled = value;
            }

        }

        [Category("GENERATOR")]
        [DescriptionAttribute("Is the Fade Out enabled.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool FadeOutEnabled
        {
            get
            {
                return this.selectedObject.FadeOutEnabled;
            }

            set
            {


                this.selectedObject.FadeOutEnabled = value;
            }

        }



        [Category("GENERATOR"), ReadOnly(true)]
        [DescriptionAttribute("The object where the generator is fastened.")]
        public string ObjectFastener
        {
            get
            {
                if (this.selectedObject.objectAttachedToGenerator != null)
                    return this.selectedObject.objectAttachedToGenerator.DisplayObject.Name;
                else
                    return "NONE";
            }


        }

    }
}

