using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Text;
using Krea.Corona_Classes;
using System.Reflection;


namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaObject
    {
        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------

        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum GeneratorEmissionType
        {
            POINT = 1,
            LINE = 2,
            DISC = 3,
        }
       
        public bool isVisible;
        public bool isEnabled = true;
        public bool isDraggable = false;
        public int DragMaxForce = 1000;
        public int DragFrequency = 50;
        public float DragDamping = 0;

        private DisplayObject displayObject;
        public PhysicsBody PhysicsBody;
        public List<CoronaEvent> Events;
        public List<CoronaTimer> Timers;
        public CoronaLayer LayerParent;
        public PathFollow PathFollow;
        public String onStartFunction;
        public String onPauseFunction;
        public String onDeleteFunction;

        public bool IsHandledByTilesMap = false;

        public bool isGenerator = false;
        public bool insertCloneAtEndOfGroup = false;

        public int generatorDelay = 1000;
        public int generatorIteration = -1;
        public string otherAttribute = "";
        public CoronaObject objectAttachedToGenerator = null;
        public BitmapMask BitmapMask;

        public int FadeInSpeed = 1000;
        public bool FadeInEnabled = false;

        public int FadeOutSpeed = 1000;
        public bool FadeOutEnabled = false;

        public bool RemoveOnCompleteFadeOut = true;

        public int DelayBetweenFades = 500;
        public GeneratorEmissionType generatorEmissionType = GeneratorEmissionType.POINT;
        public int GeneratorAngularImpulse = 0;
        public float GeneratorXImpulse = 0;
        public float GeneratorYImpulse = 0;


        //ENTITY
        public bool isEntity = false;
        public CoronaEntity Entity;
        public CoronaEntity EntityParent;
        [NonSerialized()]
        public bool isSelected = false;
        [NonSerialized()]
        public TransformBox TransformBox;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public CoronaObject(bool isEntity)
        {
            this.isEntity = isEntity;
            this.Entity = new CoronaEntity("Entity",this);

            this.isSelected = false;
            isVisible = true;

        }


        //Display Object Image Corona Object
        public CoronaObject(Point location, Image image)
        {
            Events = new List<CoronaEvent>();
            Timers = new List<CoronaTimer>();
            this.PhysicsBody = new PhysicsBody(this);
            this.PathFollow = new PathFollow(this);

            DisplayObject = new DisplayObject(image, location, this);
            this.isSelected = false;
            isVisible = true;

            onStartFunction = "function() \n\n end";
            onPauseFunction = " function() \n\n end";
            onDeleteFunction = "function() \n\n end";

            this.IsHandledByTilesMap = false;
        }

        public CoronaObject(DisplayObject displayObj)
        {
            Events = new List<CoronaEvent>();
            Timers = new List<CoronaTimer>();
            this.PhysicsBody = new PhysicsBody(this);
            this.PathFollow = new PathFollow(this);

            DisplayObject = displayObj;
            this.DisplayObject.CoronaObjectParent = this;

            this.isSelected = false;
            isVisible = true;


            onStartFunction = "function() \n\n end";
            onPauseFunction = "function() \n\n end";
            onDeleteFunction = "function() \n\n end";

            this.IsHandledByTilesMap = false;
        }

        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        public void setEntityParent(CoronaEntity entity)
        {
            this.EntityParent = entity;
        }

        public override string ToString()
        {
            return this.displayObject.Name;
        }

        public CoronaObject cloneObject(CoronaLayer layerDest,bool incrementName,bool keepPosition)
        {

            if (this.isEntity == true)
            {
                CoronaObject newEntity = new CoronaObject(true);
                newEntity.isEnabled = this.isEnabled;
                layerDest.addCoronaObject(newEntity, incrementName);
                newEntity.Entity = this.Entity.cloneEntity(newEntity);

                return newEntity;

            }
            else if(this.EntityParent != null)
            {
                if (this.displayObject != null)
                {

                    //Dupliquer le displayObject
                    DisplayObject newDisplayObject = this.DisplayObject.cloneInstance(keepPosition);

                    CoronaObject newObject = new CoronaObject(newDisplayObject);
                    newObject.displayObject.Name = layerDest.SceneParent.projectParent.IncrementObjectName(newDisplayObject.Name);

                    if (newObject != null)
                    {
                        
                        

                        //Copier le body
                        if (this.PhysicsBody != null)
                        {
                            newObject.PhysicsBody = this.PhysicsBody.cloneBody(newObject);
                        }


                        newObject.isDraggable = this.isDraggable;

                        //Copier les events de l'objet
                        newObject.onStartFunction = this.onStartFunction;
                        newObject.onPauseFunction = this.onPauseFunction;
                        newObject.onDeleteFunction = this.onDeleteFunction;

                        StringBuilder sb = new StringBuilder();
                        //Retirer le nom des functions et les ajuster au nom de l'objet
                        //---POUR START

                        sb.Append(newObject.onStartFunction);
                        newObject.onStartFunction = sb.ToString().Replace(this.displayObject.Name, newObject.displayObject.Name);
                        sb.Clear();

                        //---POUR PAUSE

                        sb.Append(newObject.onPauseFunction);
                        newObject.onPauseFunction = sb.ToString().Replace(this.displayObject.Name, newObject.displayObject.Name);
                        sb.Clear();

                        //---POUR DELETE

                        sb.Append(newObject.onDeleteFunction);
                        newObject.onDeleteFunction = sb.ToString().Replace(this.displayObject.Name, newObject.displayObject.Name);
                        sb.Clear();


                        if (this.isGenerator == true)
                        {
                            newObject.isGenerator = true;
                            newObject.generatorDelay = this.generatorDelay;
                            newObject.generatorIteration = this.generatorIteration;
                            newObject.generatorEmissionType = this.generatorEmissionType;
                            newObject.FadeInEnabled = this.FadeInEnabled;
                            newObject.FadeInSpeed = this.FadeInSpeed;
                            newObject.FadeOutEnabled = this.FadeOutEnabled;
                            newObject.FadeOutSpeed = this.FadeOutSpeed;
                            newObject.RemoveOnCompleteFadeOut = this.RemoveOnCompleteFadeOut;
                            newObject.DelayBetweenFades = this.DelayBetweenFades;
                            newObject.GeneratorXImpulse = this.GeneratorXImpulse;
                            newObject.GeneratorYImpulse = this.GeneratorYImpulse;
                        }

                        newObject.displayObject.GradientColor.isEnabled = this.displayObject.GradientColor.isEnabled;
                        newObject.displayObject.GradientColor.color1 = this.displayObject.GradientColor.color1;
                        newObject.displayObject.GradientColor.color2 = this.displayObject.GradientColor.color2;
                        newObject.displayObject.GradientColor.direction = this.displayObject.GradientColor.direction;
                        newObject.displayObject.Alpha = this.displayObject.Alpha;

                        newObject.PathFollow = this.PathFollow.cloneInstance(newObject);
                        newObject.displayObject.Rotation = this.displayObject.Rotation;


                        newObject.isDraggable = this.isDraggable;

                        return newObject;
                    }

                }
            }
            else
            {
                if (this.displayObject != null)
                {

                    //Dupliquer le displayObject
                    DisplayObject newDisplayObject = this.DisplayObject.cloneInstance(keepPosition);

                    CoronaObject newObject = new CoronaObject(newDisplayObject);
                    

                    if (newObject != null)
                    {
                        newObject.isEnabled = this.isEnabled;
                        //Copier le layer parent
                        layerDest.addCoronaObject(newObject, incrementName);
                        newObject.displayObject.OriginalAssetName = this.displayObject.OriginalAssetName;

                        //Copier le body
                        if (this.PhysicsBody != null)
                        {
                            newObject.PhysicsBody = this.PhysicsBody.cloneBody(newObject);
                        }



                        //Copier les events de l'objet
                        newObject.onStartFunction = this.onStartFunction;
                        newObject.onPauseFunction = this.onPauseFunction;
                        newObject.onDeleteFunction = this.onDeleteFunction;

                        StringBuilder sb = new StringBuilder();
                        //Retirer le nom des functions et les ajuster au nom de l'objet
                        //---POUR START

                        sb.Append(newObject.onStartFunction);
                        newObject.onStartFunction = sb.ToString().Replace(this.displayObject.Name, newObject.displayObject.Name);
                        sb.Clear();

                        //---POUR PAUSE

                        sb.Append(newObject.onPauseFunction);
                        newObject.onPauseFunction = sb.ToString().Replace(this.displayObject.Name, newObject.displayObject.Name);
                        sb.Clear();

                        //---POUR DELETE

                        sb.Append(newObject.onDeleteFunction);
                        newObject.onDeleteFunction = sb.ToString().Replace(this.displayObject.Name, newObject.displayObject.Name);
                        sb.Clear();


                        if (this.isGenerator == true)
                        {
                            newObject.isGenerator = true;
                            newObject.generatorDelay = this.generatorDelay;
                            newObject.generatorIteration = this.generatorIteration;
                            newObject.generatorEmissionType = this.generatorEmissionType;
                            newObject.FadeInEnabled = this.FadeInEnabled;
                            newObject.FadeInSpeed = this.FadeInSpeed;
                            newObject.FadeOutEnabled = this.FadeOutEnabled;
                            newObject.FadeOutSpeed = this.FadeOutSpeed;
                            newObject.RemoveOnCompleteFadeOut = this.RemoveOnCompleteFadeOut;
                            newObject.DelayBetweenFades = this.DelayBetweenFades;
                            newObject.GeneratorXImpulse = this.GeneratorXImpulse;
                            newObject.GeneratorYImpulse = this.GeneratorYImpulse;
                            

                        }

                        newObject.displayObject.GradientColor.isEnabled = this.displayObject.GradientColor.isEnabled;
                        newObject.displayObject.GradientColor.color1 = this.displayObject.GradientColor.color1;
                        newObject.displayObject.GradientColor.color2 = this.displayObject.GradientColor.color2;
                        newObject.displayObject.GradientColor.direction = this.displayObject.GradientColor.direction;
                        newObject.displayObject.Alpha = this.displayObject.Alpha;

                        newObject.PathFollow = this.PathFollow.cloneInstance(newObject);
                        newObject.displayObject.Rotation = this.displayObject.Rotation;

                        newObject.isDraggable = this.isDraggable;
                        return newObject;
                    }

                }
            }
            

            return null;
        }

        public void setSelected(bool isSelected)
        {
            this.isSelected = isSelected;
            if(isSelected == true)
                 this.TransformBox = new TransformBox(this);
            else
                this.TransformBox = null;
        }

         [CategoryAttribute("Display Object"), DescriptionAttribute("The display object associated")]
        public DisplayObject DisplayObject { get { return this.displayObject; } set { this.displayObject = value; } }

    }
}
