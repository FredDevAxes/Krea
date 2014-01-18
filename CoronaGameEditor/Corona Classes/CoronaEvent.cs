using System;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaEvent
    {

        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        public String Name;
        public String Type;

        public String Handle;
        //---------------------------------------------------
        //-------------------Constructeurs------------------------
        //---------------------------------------------------
        public CoronaEvent() { }

        public CoronaEvent(String Name, String Type,String Handle)
        {
            this.Name = Name;
            this.Type = Type;
            this.Handle = Handle;
        }


        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public override string ToString()
        {
            return this.Name + " -- " + this.Type;
        }
    }
}
