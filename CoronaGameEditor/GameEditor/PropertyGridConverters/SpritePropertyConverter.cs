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
    public class SpritePropertyConverter
    {

        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        private CoronaObject selectedObject;
        public Form1 MainForm;

     
        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public SpritePropertyConverter() { }
        public SpritePropertyConverter(CoronaObject obj, Form1 MainForm)
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
            get { return this.selectedObject.DisplayObject.Name; }
            set
            {

                value = value.Replace("_", "").Replace(" ", "");
                value = value.Replace(" ", "").Replace(" ", "");

                value = this.MainForm.getElementTreeView().ProjectSelected.IncrementObjectName(value);
                this.selectedObject.DisplayObject.Name = value;
                GameElement elem= this.MainForm.getElementTreeView().getNodeFromObjectInstance(this.MainForm.getElementTreeView().ProjectRootNodeSelected.Nodes,this.selectedObject);
                if (elem != null)
                    elem.Text = value;
            }
        }

         [Category("DISPLAY")]
        [DescriptionAttribute("The alpha that should be used for the object.")]
        public float Alpha
        {
            get { return this.selectedObject.DisplayObject.Alpha; }
            set {

                    this.selectedObject.DisplayObject.Alpha = value;

            }
        }

         [Category("SPRITE")]
         [DescriptionAttribute("The sheet factor to apply to deal with a 480x320 device resolution.")]

         public float SheetSizeFactor
         {
             get
             {
                 if( this.selectedObject.DisplayObject.SpriteSet != null && this.selectedObject.DisplayObject.SpriteSet.Frames.Count>0)
                 {
                     return this.selectedObject.DisplayObject.SpriteSet.Frames[0].SpriteSheetParent.FramesFactor;
                 }
                 else
                     return -1;
                 
             }

             set
             {
                 if (this.selectedObject.DisplayObject.SpriteSet != null && this.selectedObject.DisplayObject.SpriteSet.Frames.Count > 0)
                 {
                    this.selectedObject.DisplayObject.SpriteSet.Frames[0].SpriteSheetParent.FramesFactor = value;

                    GorgonLibrary.Gorgon.Go();
                 }
             }
         }

         [Category("SPRITE")]
         [DescriptionAttribute("The current Frame of the animation.")]

         public int CurrentFrame
         {
             get
             {
                 return this.selectedObject.DisplayObject.CurrentFrame +1;

             }

             set
             {

                 this.selectedObject.DisplayObject.CurrentFrame = value-1;

             }
         }

         [Category("SPRITE")]
         [DescriptionAttribute("Does the first animation should be played at scene creation.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool AutoPlay
         {
             get
             {
                 return this.selectedObject.DisplayObject.AutoPlay;

             }

             set
             {
                 this.selectedObject.DisplayObject.AutoPlay = value;

             }
         }


         [Category("SPRITE")]
         [DescriptionAttribute("The current Sequence of the animation.")]
         [Editor(typeof(SpriteSequenceEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public string CurrentSequence
         {
             get
             {
                 if (this.selectedObject.DisplayObject.CurrentSequence == "") this.selectedObject.DisplayObject.setSequence("");
                 return this.selectedObject.DisplayObject.CurrentSequence;

             }

             set
             {
                 this.selectedObject.DisplayObject.setSequence(value);

             }
         }

         [Category("GENERAL")]
        [DescriptionAttribute("The size of the display object.")]
        public Size Size
        {
            get {
               
                    int frame  =  this.selectedObject.DisplayObject.CurrentFrame ;
                    this.selectedObject.DisplayObject.CurrentFrame = frame;

                    return this.selectedObject.DisplayObject.SurfaceRect.Size;
            }
           
        }

         [Category("GENERAL")]
        [DescriptionAttribute("The location of the display object.")]
        public Point Location
        {
            get 
            {
                if (this.selectedObject.DisplayObject.Figure != null)
                    return this.selectedObject.DisplayObject.Figure.Position;
                else    
                    return this.selectedObject.DisplayObject.SurfaceRect.Location;
            }
            set 
            {


                if (this.selectedObject.DisplayObject.Figure != null)
                    this.selectedObject.DisplayObject.Figure.Position = value;

                this.selectedObject.DisplayObject.SurfaceRect = new Rectangle(value, this.selectedObject.DisplayObject.SurfaceRect.Size); 

                
            }
        }

         [Category("DISPLAY")]
        [DescriptionAttribute("The Alpha blending of the object.")]
        public DisplayObject.AlphaBlendingMode  BlendMode
        {
            get {
                return this.selectedObject.DisplayObject.blendMode; }
            set
            {


                this.selectedObject.DisplayObject.blendMode = value;

            }

        }

         [Category("GENERAL")]
        [DescriptionAttribute("The angle of rotation of the object.")]
        public int Rotation
        {
            get
            {
                return this.selectedObject.DisplayObject.Rotation;
            }
            set
            {
                if (this.selectedObject.DisplayObject.Figure != null)
                {
                    Figure fig = this.selectedObject.DisplayObject.Figure;
                    if (fig.ShapeType.Equals("CURVE") || fig.ShapeType.Equals("LINE"))
                    {
                        return;
                    }
                    else
                    {
                        this.selectedObject.DisplayObject.Rotation = value;
                    }


                }
                else
                {
                    this.selectedObject.DisplayObject.Rotation = value;
                }

            }

        }


        



        //------------------EVENTS ---------------------------------
       /* [Category("EVENTS")]
        [DescriptionAttribute("The onStart event of the object."),Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String OnStart
        {
            get
            {
                return this.selectedObject.onStartFunction;
            }

            set
            {
                this.selectedObject.onStartFunction = value;
            }
       
        }

        [Category("EVENTS")]
        [DescriptionAttribute("The onPause event of the object."), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String OnPause
        {
            get
            {
                return this.selectedObject.onPauseFunction;
            }

            set
            {
                this.selectedObject.onPauseFunction = value;
            }

        }

        [Category("EVENTS")]
        [DescriptionAttribute("The onDelete event of the object."), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String OnDelete
        {
            get
            {
                return this.selectedObject.onDeleteFunction;
            }

            set
            {
               
                this.selectedObject.onDeleteFunction = value;
            }

        }*/

        [Category("TILESMAP HANDLING")]
        [DescriptionAttribute(@"Does the object is handled by the tilesmap if existing. If true and if the object is in a layer that contains a tilesmap,
                this object will automatically call the stopInteraction method and then will be disabled!")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsHandledByTilesMap
        {
            get
            {
                return this.selectedObject.IsHandledByTilesMap;
            }

            set
            {


                this.selectedObject.IsHandledByTilesMap = value;
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



        [Category("GENERATOR"),ReadOnly(true)]
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

        

        [Category("PATH FOLLOW")]
        [DescriptionAttribute("Is the path follow enabled for this object.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsEnabled
        {
            get
            {

                 return this.selectedObject.PathFollow.isEnabled;

            }

            set
            {

               this.selectedObject.PathFollow.isEnabled = value;

            }

        }

        [Category("DRAG")]
        [DescriptionAttribute("Is the object draggable by a touch joint.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsDraggable
        {
            get
            {

                return this.selectedObject.isDraggable;

            }

            set
            {

                this.selectedObject.isDraggable = value;
                if (this.selectedObject.isDraggable == true && this.selectedObject.PhysicsBody.Mode != PhysicsBody.PhysicBodyMode.Dynamic)
                    this.selectedObject.PhysicsBody.Mode = PhysicsBody.PhysicBodyMode.Dynamic;
            }

        }

        [Category("DRAG")]
        [DescriptionAttribute("The drag damping ratio.")]
        public float DampingRatio
        {
            get
            {
                if (this.selectedObject.DragDamping <= 0)
                    this.selectedObject.DragDamping = 0.7f;

                return this.selectedObject.DragDamping;

            }

            set
            {
                if (this.selectedObject.DragDamping <= 0)
                    this.selectedObject.DragDamping = 0.7f;

                this.selectedObject.DragDamping = value;

            }

        }

        [Category("DRAG")]
        [DescriptionAttribute("The drag frequency (in Hz).")]
        public int Frequency
        {
            get
            {
                if (this.selectedObject.DragFrequency <= 0)
                    this.selectedObject.DragFrequency = 5;

                return this.selectedObject.DragFrequency;

            }

            set
            {
                if (this.selectedObject.DragFrequency <= 0)
                    this.selectedObject.DragFrequency = 5;

                this.selectedObject.DragFrequency = value;

            }

        }

        [Category("DRAG")]
        [DescriptionAttribute("The drag maximum force.")]
        public int MaxForce
        {
            get
            {
                if (this.selectedObject.DragMaxForce <= 0)
                    this.selectedObject.DragMaxForce = 10;

                return this.selectedObject.DragMaxForce;

            }

            set
            {
                if (this.selectedObject.DragMaxForce <= 0)
                    this.selectedObject.DragMaxForce = 10;

                this.selectedObject.DragMaxForce = value;

            }

        }

        [Category("PATH FOLLOW")]
        [DescriptionAttribute("The total time in milliseconds to transit.")]
        public int Speed
        {
            get
            {

                return this.selectedObject.PathFollow.speed;

            }

            set
            {

                this.selectedObject.PathFollow.speed = value;

            }

        }

        [Category("PATH FOLLOW")]
        [DescriptionAttribute("Is a cyclic transit.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsCyclic
        {
            get
            {

                return this.selectedObject.PathFollow.isCyclic;

            }

            set
            {

                this.selectedObject.PathFollow.isCyclic = value;

            }

        }


        [Category("PATH FOLLOW")]
        [DescriptionAttribute("Is a curved transit.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool IsCurved
        {
            get
            {

                return this.selectedObject.PathFollow.isCurve;

            }

            set
            {

                this.selectedObject.PathFollow.isCurve = value;

            }

        }

        [Category("PATH FOLLOW")]
        [DescriptionAttribute("Does the object rotate during transition.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool Rotate
        {
            get
            {

                return this.selectedObject.PathFollow.Rotate;

            }

            set
            {

                this.selectedObject.PathFollow.Rotate = value;

            }

        }

        [Category("PATH FOLLOW")]
        [DescriptionAttribute("Does the object need to be removed once the transition completed.")]
        [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool RemoveOnComplete
        {
            get
            {

                return this.selectedObject.PathFollow.removeOnComplete;

            }

            set
            {

                this.selectedObject.PathFollow.removeOnComplete = value;

            }

        }

        [Category("PATH FOLLOW")]
        [DescriptionAttribute("The transit iteration.")]
        public int TransitIteration
        {
            get
            {

                return this.selectedObject.PathFollow.Iteration;

            }

            set
            {

                this.selectedObject.PathFollow.Iteration = value;

            }

        }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("Is the rotation of the body fixed.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool IsFixedRotation
         {
              get
            {
                return this.selectedObject.PhysicsBody.isFixedRotation;
            }
             set
             {
                 this.selectedObject.PhysicsBody.isFixedRotation = value;
             }
         }

        [Category("PHYSICS BODY")]
        [DescriptionAttribute("The physic body mode of the object.")]
        public PhysicsBody.PhysicBodyMode BodyMode
        {
            get
            {
                return this.selectedObject.PhysicsBody.Mode;
            }
            set
            {


                this.selectedObject.PhysicsBody.Mode = value;

                if (value != PhysicsBody.PhysicBodyMode.Inactive)
                {
                    this.selectedObject.PhysicsBody.updateBody();
                }
                

            }

        }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("The angular damping of the body.")]
         public float AngularDamping 
         {
            get
            {
                return this.selectedObject.PhysicsBody.AngularDamping;
            }
            set
            {
                this.selectedObject.PhysicsBody.AngularDamping = value;
            }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("The linear damping of the body.")]
         public float LinearDamping 
         {
            get
            {
                return this.selectedObject.PhysicsBody.LinearDamping;
            }
            set
            {
                this.selectedObject.PhysicsBody.LinearDamping = value;
            }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("The angular velocity of the body.")]
         public float AngularVelocity 
         {
            get
            {
                return this.selectedObject.PhysicsBody.AngularVelocity;
            }
            set
            {
                this.selectedObject.PhysicsBody.AngularVelocity = value;
            }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("The angular velocity of the body.")]
         public Point LinearVelocity 
         {
            get
            {
                return this.selectedObject.PhysicsBody.LinearVelocity;
            }
            set
            {
                this.selectedObject.PhysicsBody.LinearVelocity = value;
            }
         }

          [Category("PHYSICS BODY")]
         [DescriptionAttribute("Is the body awake.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool IsAwake 
         {
            get
            {
                return this.selectedObject.PhysicsBody.IsAwake;
            }
            set
            {
                this.selectedObject.PhysicsBody.IsAwake = value;
            }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("Is the body allowed to sleep.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool IsSleepingAllowed 
         {
            get
            {
                return this.selectedObject.PhysicsBody.IsSleepingAllowed;
            }
            set
            {
                this.selectedObject.PhysicsBody.IsSleepingAllowed = value;
            }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("Is a sensor.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool IsSensor 
         {
            get
            {
                return this.selectedObject.PhysicsBody.IsSensor;
            }
            set
            {
                this.selectedObject.PhysicsBody.IsSensor = value;
            }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("The name of the collision filter group."), Editor(typeof(CollisionGroupEditor),
          typeof(System.Drawing.Design.UITypeEditor))]
         public string CollisionFilterGroup
         {

             get
             {
                 Scene sceneParent = this.selectedObject.LayerParent.SceneParent;
                 if (sceneParent.CollisionFilterGroups.Count <= this.selectedObject.PhysicsBody.CollisionGroupIndex)
                 {
                     this.selectedObject.PhysicsBody.CollisionGroupIndex = 0;
                     return "Default";
                 }
                 else
                 {
                     string collisionGroupName = sceneParent.CollisionFilterGroups[this.selectedObject.PhysicsBody.CollisionGroupIndex].GroupName;
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
                         this.selectedObject.PhysicsBody.CollisionGroupIndex = i;
                         collisionGroupName = sceneParent.CollisionFilterGroups[i].GroupName;
                         break;
                     }
                 }
                
                 if(collisionGroupName.Equals("Default"))
                     this.selectedObject.PhysicsBody.CollisionGroupIndex = 0;
             }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("Is the body active.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool IsBodyActive 
         {
            get
            {
                return this.selectedObject.PhysicsBody.IsBodyActive;
            }
            set
            {
                this.selectedObject.PhysicsBody.IsBodyActive = value;
            }
         }

         [Category("PHYSICS BODY")]
         [DescriptionAttribute("Is a bullet.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool IsBullet
         {
             get
             {
                 return this.selectedObject.PhysicsBody.IsBullet;
             }
             set
             {
                 this.selectedObject.PhysicsBody.IsBullet = value;
             }
         }


         [Category("BITMAP MASK")]
         [DescriptionAttribute("The mask image applied to the object.")]
         public System.Drawing.Image BitmapMask
         {
             get
             {
                 if (this.selectedObject.BitmapMask == null)
                     return null;

                 return this.selectedObject.BitmapMask.MaskImage;
             }
             set
             {
                 if (this.selectedObject.BitmapMask == null)
                     this.selectedObject.BitmapMask = new BitmapMask(null);

                 this.selectedObject.BitmapMask.MaskImage = value;
             }

         }

         [Category("BITMAP MASK")]
         [DescriptionAttribute("Is the mask enabled for this object.")]
         [Editor(typeof(CheckBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
         public bool IsMaskEnabled
         {
             get
             {
                 if (this.selectedObject.BitmapMask == null)
                     return false;

                 return this.selectedObject.BitmapMask.IsMaskEnabled;
             }
             set
             {
                 if (this.selectedObject.BitmapMask == null)
                     this.selectedObject.BitmapMask = new BitmapMask(null);


                 if (this.selectedObject.BitmapMask.MaskImage != null)
                     this.selectedObject.BitmapMask.IsMaskEnabled = value;
                 else
                     this.selectedObject.BitmapMask.IsMaskEnabled = false;
             }

         }

    }
}

