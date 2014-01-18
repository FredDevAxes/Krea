using System.ComponentModel;
using Krea.CoronaClasses;
using Krea.CGE_Figures;
using System.Reflection;
using Krea.GameEditor.PropertyGridEditors;

namespace Krea.GameEditor.PropertyGridConverters
{
    [ObfuscationAttribute(Exclude = true)]
  [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The shape representing a physics body element of a Corona Display Object")]
    public class BodyElementPropertyConverter
    {
        private BodyElement bodyElement;
        private System.Windows.Forms.ListViewItem viewItem;
        private CoronaObject selectedObject;
        private Form1 mainForm;

         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public BodyElementPropertyConverter() { }
        public BodyElementPropertyConverter(BodyElement bodyElement, System.Windows.Forms.ListViewItem viewItem,CoronaObject objectParent,Form1 mainForm)
        {
            this.bodyElement = bodyElement;
            this.viewItem = viewItem;
            this.selectedObject = objectParent;
            this.mainForm = mainForm;
        }

        public CoronaObject GetObjectSelected()
        {
            return this.selectedObject;
        }
         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
        [Category("Physics")]
        [DescriptionAttribute("The Bounce of the body element.")]
        public decimal Bounce
        {
         get
            {
                return this.bodyElement.Bounce;
            }

            set
            {

                this.bodyElement.Bounce = value;

            }
         }

         [Category("Physics")]
        [DescriptionAttribute("The Density of the body element.")]
        public decimal Density
        {
         get
            {
                return this.bodyElement.Density;
            }

            set
            {

                this.bodyElement.Density = value;

            }
         }

         [Category("Physics")]
        [DescriptionAttribute("The Friction of the body element.")]
        public decimal Friction
        {
         get
            {
                return this.bodyElement.Friction;
            }

            set
            {

                this.bodyElement.Friction = value;

            }
         }

         [Category("Physics")]
         [DescriptionAttribute("The element is a sensor.")]
         public bool IsSensor
         {
             get
             {
                 return this.bodyElement.IsSensor;
             }

             set
             {

                 this.bodyElement.IsSensor = value;

             }
         }

         [DescriptionAttribute("The name of the collision filter group."), Editor(typeof(CollisionGroupEditor),
           typeof(System.Drawing.Design.UITypeEditor))]
         [Category("Physics")]
         public string CollisionFilterGroup
         {

             get
             {
                 Scene sceneParent = this.selectedObject.LayerParent.SceneParent;
                 if (sceneParent.CollisionFilterGroups.Count <= this.bodyElement.CollisionGroupIndex)
                 {
                     this.bodyElement.CollisionGroupIndex = 0;
                     return "Default";
                 }
                 else
                 {
                     string collisionGroupName = sceneParent.CollisionFilterGroups[this.bodyElement.CollisionGroupIndex].GroupName;
                     return collisionGroupName;
                 }



             }
             set
             {
                 Scene sceneParent = this.selectedObject.LayerParent.SceneParent;
                 string collisionGroupName = "Default";
                 for (int i = 0; i < sceneParent.CollisionFilterGroups.Count; i++)
                 {
                     if (sceneParent.CollisionFilterGroups[i].GroupName.Equals(value))
                     {
                         this.bodyElement.CollisionGroupIndex = i;
                         collisionGroupName = sceneParent.CollisionFilterGroups[i].GroupName;
                         break;
                     }
                 }

                 if (collisionGroupName.Equals("Default"))
                     this.bodyElement.CollisionGroupIndex = 0;
             }
         }
          [Category("General")]
         [DescriptionAttribute("The name of the body element.")]
         public string Name
         {
             get
             {
                 return this.bodyElement.Name;
             }

             set
             {
                 this.bodyElement.Name = value.Replace(" ","_").Replace(".","").Replace(",","");
                 this.viewItem.Name = this.bodyElement.Name;
                 this.viewItem.Text = this.bodyElement.Name;
             }
         }

         [Category("General")]
        
         [DescriptionAttribute("The type of the body element."),ReadOnly(true)]
         public string Type
         {
             get
             {
                 return this.bodyElement.Type;
             }

         }    
        
        

    }
}
