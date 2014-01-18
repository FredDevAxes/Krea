using System;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaTimer
    {

        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        public String Name;
        public decimal Iteration;
        public decimal Delay;

        public String Handle;
        //---------------------------------------------------
        //-------------------Constructeurs------------------------
        //---------------------------------------------------
        public CoronaTimer() { }

        public CoronaTimer(String Name, decimal delay, decimal iteration, String Handle)
        {
            this.Name = Name;
            this.Delay = delay;
            this.Iteration = iteration;
            this.Handle = Handle;
        }


        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------
        public override string ToString()
        {
            return this.Name + " -- Delay: "+this.Delay +" X "+this.Iteration;
        }
    }
}
