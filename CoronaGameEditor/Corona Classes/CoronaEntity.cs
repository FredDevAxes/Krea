using System.Collections.Generic;
using System.Reflection;
using System;
namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaEntity
    {

        //---------------------------------------------------
        //-------------------Attributes--------------------
        //---------------------------------------------------
        public List<CoronaJointure> Jointures;
        public List<CoronaObject> CoronaObjects;
        public string Name;
        public CoronaObject objectParent;
        public bool isEnabled = true;
        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public CoronaEntity(string name, CoronaObject objectParent)
        {
            this.Name = name;
            this.objectParent = objectParent;
            Jointures = new List<CoronaJointure>();
            CoronaObjects = new List<CoronaObject>();
        }

        //---------------------------------------------------
        //-------------------Mthodes--------------------
        //---------------------------------------------------
        public void addObject(CoronaObject obj)
        {
            obj.LayerParent = this.objectParent.LayerParent;
            obj.EntityParent = this;


            if (obj.isEntity == false)
            {
                string objName = this.objectParent.LayerParent.SceneParent.projectParent.IncrementObjectName(obj.DisplayObject.Name);
                obj.DisplayObject.Name = objName;
            }
            else
            {
                string objName = this.objectParent.LayerParent.SceneParent.projectParent.IncrementObjectName(obj.Entity.Name);
                obj.Entity.Name = objName;
            }


            this.CoronaObjects.Add(obj);
        }

        public CoronaEntity cloneEntity(CoronaObject objectParent)
        {
            string newName = objectParent.LayerParent.SceneParent.projectParent.IncrementObjectName(this.Name);

            CoronaEntity newEntity = new CoronaEntity(newName,objectParent);
            objectParent.Entity = newEntity;
            ////Copier toutes les objets
            for (int i = 0; i < this.CoronaObjects.Count; i++)
            {
                CoronaObject obj = this.CoronaObjects[i].cloneObject(this.CoronaObjects[i].LayerParent,true,true);

                newEntity.addObject(obj);
            }

            //Recreer toutes les jointures
            for (int i = 0; i < this.Jointures.Count; i++)
            {
                CoronaJointure currentJoint = this.Jointures[i];

                int indexOfOBJA = -1;
                int indexOfOBJB = -1;

                if(currentJoint.coronaObjA != null)
                    indexOfOBJA = this.CoronaObjects.IndexOf(currentJoint.coronaObjA);

                if(currentJoint.coronaObjB != null)
                    indexOfOBJB = this.CoronaObjects.IndexOf(currentJoint.coronaObjB);

                CoronaObject newOBJA = null;
                CoronaObject newOBJB = null;

                if(indexOfOBJA>-1) 
                    newOBJA = newEntity.CoronaObjects[indexOfOBJA];

                if(indexOfOBJB>-1) 
                    newOBJB = newEntity.CoronaObjects[indexOfOBJB];

                CoronaJointure newJoint = currentJoint.clone(newOBJA, newOBJB, currentJoint.layerParent);
                newEntity.Jointures.Add(newJoint);

            }


            return newEntity;
        }

    }
}
