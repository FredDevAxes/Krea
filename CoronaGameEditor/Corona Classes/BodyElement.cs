using System;
using System.Collections.Generic;
using System.Drawing;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class BodyElement
    {


        //---------------------------------------------------
        //-------------------Attributs--------------------
        //---------------------------------------------------
        public decimal Bounce;
        public decimal Density;
        public decimal Friction;
        public int Radius;
        public int Index;
        public string Name;
        public String Type ;
        public List<Point> BodyShape;
        public Point LocationCircle;
        public Rectangle SurfaceCircle;
        public bool IsSensor;

        public int CollisionGroupIndex = 0;
        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public BodyElement( int index,string name, decimal Bounce, decimal Density, decimal Friction, Point location,int Radius)
        {
            this.Index = index;
            this.Bounce = Bounce;
            this.Density = Density;
            this.Friction = Friction;
            this.Radius = Radius;
            this.Name = name;
            this.LocationCircle = location;
            SurfaceCircle = new Rectangle(this.LocationCircle,new Size( this.Radius*2, this.Radius *2));
            this.Type = "CIRCLE";
        }

        public BodyElement(int index, string name, decimal Bounce, decimal Density, decimal Friction,  List<Point> bodyShape)
        {
            this.Index = index;
            this.Bounce = Bounce;
            this.Density = Density;
            this.Friction = Friction;
            this.Name = name;

            BodyShape = bodyShape;

            if (bodyShape.Count > 2)
            {
                this.Type = "SHAPE";
            }
            else
                this.Type = "LINE";
           
        }


        //---------------------------------------------------
        //-------------------Methodes--------------------
        //---------------------------------------------------

        public override string ToString()
        {
            return this.Index + " -- " + this.Name;
        }
    }
}
