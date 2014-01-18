using System;
using System.Text;
using System.Drawing;
using Krea.Corona_Classes;
using System.Collections.Generic;
namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaJointure
    {

        /// <summary>
        /// Attribut
        /// </summary>
        public String name { get; set; }
        public String type {get; set;}
        public CoronaLayer layerParent { get; set; }
        public CoronaObject coronaObjA { get; set; }
        public CoronaObject coronaObjB { get; set; }
        public Point AnchorPointA { get; set; }
        public Point AnchorPointB { get; set; }

        public bool isEnabled = true;
        public Point ObjectAnchorPointA;
        public Point ObjectAnchorPointB;
        public Point axisDistance { get; set; }


   

        //Attribute for Pivot Joints
        //
        public Boolean isMotorEnable {get;set;}
        public Double motorSpeed { get; set; }
        public Double maxMotorTorque { get; set; }
        public Double maxMotorForce { get; set; }
        public Boolean isLimitEnabled { get; set; }
        public Double lowerLimit { get; set; }
        public Double upperLimit { get; set; }


        //Distance Joints
        //
        public Double frequency { get; set; }
        public Double dampingRatio { get; set; }

        //Friction Joints
        //
        public Double maxForce { get; set; }
        public Double maxTorque { get; set; }

        public int springFrequency { get; set; }
        public double springDampingRatio { get; set; }

        public Point CurrentPointToMove;
        //***********************************
        // Constructor
        //***********************************
        public CoronaJointure(CoronaLayer layerParent)
        {
            this.type = "NONE";
            this.layerParent = layerParent;
        }

        //***********************************
        // Init Jointure
        //***********************************

        // Pivot Init
        //
        public void InitPivotJointure (CoronaObject _coronaObjA, CoronaObject _coronaObjB,
            Point _AnchorPoint, bool _isMotorEnabled, bool _isLimitEnabled, Double _motorSpeed, Double _maxMotorTorque,
            Double _upperLimit, Double _lowerLimit)
        {
               
                type = "PIVOT";
                coronaObjA = _coronaObjA;
                coronaObjB = _coronaObjB;
                isMotorEnable = _isMotorEnabled;
                isLimitEnabled = _isLimitEnabled;
                motorSpeed = _motorSpeed;
                maxMotorTorque = _maxMotorTorque;
                upperLimit = _upperLimit;
                lowerLimit = _lowerLimit;
                AnchorPointA = new Point(_AnchorPoint.X, _AnchorPoint.Y);
                this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;
        }
        // Distance Init
        //
        public void InitDistanceJointure(CoronaObject _coronaObjA, CoronaObject _coronaObjB,Point anchorA,Point anchorB,
             Double _dampingRatio, Double _frequency)
        {
            //crateA, crateB, crateA.x,crateA.y, crateB.x,crateB.y
            type = "DISTANCE";
            coronaObjA = _coronaObjA;
            coronaObjB = _coronaObjB;
            AnchorPointA = anchorA;
            AnchorPointB = anchorB;
            dampingRatio = _dampingRatio;
            frequency = _frequency;

            this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;
            
        }
        // Piston Init
        //
        public void InitPistonJointure( CoronaObject _coronaObjA, CoronaObject _coronaObjB, Point _axisDistance,Point anchorA,
             bool _isMotorEnabled, bool _isLimitEnabled, Double _motorSpeed, Double _maxMotorForce,
            Double _upperLimit, Double _lowerLimit)
        {
            type = "PISTON";
            coronaObjA = _coronaObjA;
            coronaObjB = _coronaObjB;
            AnchorPointA = anchorA;
            axisDistance = _axisDistance;
            isMotorEnable = _isMotorEnabled;
            isLimitEnabled = _isLimitEnabled;
            motorSpeed = _motorSpeed;
            maxMotorForce = _maxMotorForce;
            upperLimit = _upperLimit;
            lowerLimit = _lowerLimit;

            this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;
        }
        // Friction Init
        //
        public void InitFrictionJointure(CoronaObject _coronaObjA, CoronaObject _coronaObjB,Point _AnchorPoint,
     Double _maxForce, Double _maxTorque)
        {
            type = "FRICTION";
            coronaObjA = _coronaObjA;
            coronaObjB = _coronaObjB;
            AnchorPointA = _AnchorPoint;

            maxForce = _maxForce;
            maxTorque = _maxTorque;
            this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;
        }
        // Weld Init
        //
        public void InitWeldJointure(CoronaObject _coronaObjA, CoronaObject _coronaObjB, Point _AnchorPoint)
        {
            type = "WELD";
            coronaObjA = _coronaObjA;
            coronaObjB = _coronaObjB;
            AnchorPointA = _AnchorPoint;
            this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;
        
        }
        // Wheel Init (WAIT FOR CORONA SDK PATH BEFORE USE IT!) ==> bug with maxMotorForce
        //
        public void InitWheelJointure( CoronaObject _coronaObjA, CoronaObject _coronaObjB, Point _axisDistance,Point anchorA,
            bool _isMotorEnabled, Double _motorSpeed, Double _maxMotorTorque)
        {
            type = "WHEEL";
            coronaObjA = _coronaObjA;
            coronaObjB = _coronaObjB;
            AnchorPointA = anchorA;
           
            axisDistance = _axisDistance;
            
            isMotorEnable = _isMotorEnabled;

            motorSpeed = _motorSpeed;
            maxMotorTorque = _maxMotorTorque;

            this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;
        }
        // Wheel Init
        //
        public void InitWheelJointure(CoronaObject _coronaObjA, CoronaObject _coronaObjB, Point anchorPoint,Point _axisDistance,
    bool _isMotorEnabled, bool _isLimitEnabled, Double _motorSpeed, Double _motorForce,
    Double _upperLimit, Double _lowerLimit)
        {
            type = "WHEEL";
            this.AnchorPointA = anchorPoint;
            coronaObjA = _coronaObjA;
            coronaObjB = _coronaObjB;
            

            axisDistance = _axisDistance;

            isMotorEnable = _isMotorEnabled;
            isLimitEnabled = _isLimitEnabled;
            this.maxMotorForce = _motorForce;
            motorSpeed = _motorSpeed;
            upperLimit = _upperLimit;
            lowerLimit = _lowerLimit;
            this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;
        }
        // Pulley Init
        //
        public void InitPulleyJointure( CoronaObject _coronaObjA, CoronaObject _coronaObjB,Point objAnchorA,Point objAnchorB,
                                Point _AnchorPointA, Point _AnchorPointB)
        {
           
            type = "PULLEY";
            this.ObjectAnchorPointA = objAnchorA;
            this.ObjectAnchorPointB = objAnchorB;
            coronaObjA = _coronaObjA;
            coronaObjB = _coronaObjB;
            AnchorPointA = _AnchorPointA;
            AnchorPointB = _AnchorPointB;
            this.name = this.type + "_" + _coronaObjA.DisplayObject.Name + "_" + _coronaObjB.DisplayObject.Name;

        }
        // Touch Init
        public void InitTouchJointure( CoronaObject _coronaObj, Point _Target)
        {
            //myJoint = physics.newJoint( "touch", crate, targetX,targetY )
            type = "TOUCH";
            coronaObjA = _coronaObj;
            AnchorPointA = _Target;
            this.name = this.type + "_" + _coronaObj.DisplayObject.Name;

        }

        //***********************************
        // Methodes 
        //***********************************
        public Point getAnchorTouched(Point pTouched,bool setValue)
        {
            if (this.AnchorPointA != Point.Empty)
            {
                Point pDest = new Point(this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X - 5,
                    this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y- 5);
                Rectangle rectAnchorA = new Rectangle(pDest, new Size(10, 10));
                if (rectAnchorA.Contains(pTouched))
                {
                    if(setValue == true)
                        this.AnchorPointA = new Point(pTouched.X - this.coronaObjA.DisplayObject.SurfaceRect.X,
                            pTouched.Y - this.coronaObjA.DisplayObject.SurfaceRect.Y) ;

                    return this.AnchorPointA;
                }
                    
            }

            if (this.AnchorPointB != Point.Empty)
            {
                Point pDest = new Point(this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X - 5,
                    this.AnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                Rectangle rectAnchorB = new Rectangle(pDest, new Size(10, 10));
                if (rectAnchorB.Contains(pTouched))
                {
                    if (setValue == true)
                        this.AnchorPointB = new Point(pTouched.X - this.coronaObjB.DisplayObject.SurfaceRect.X,
                            pTouched.Y - this.coronaObjB.DisplayObject.SurfaceRect.Y);

                    return this.AnchorPointB;
                }
                   
            }

            if (this.axisDistance != Point.Empty)
            {
                Point pDest = new Point(this.axisDistance.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                    this.axisDistance.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                Rectangle rectAxisDistance = new Rectangle(pDest, new Size(10, 10));
                if (rectAxisDistance.Contains(pTouched))
                {
                    if (setValue == true)
                        this.axisDistance = new Point(pTouched.X - this.coronaObjA.DisplayObject.SurfaceRect.X,
                            pTouched.Y - this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    return this.axisDistance;
                }
                   
            }

            if (this.ObjectAnchorPointA != Point.Empty)
            {
                Point pDest = new Point(this.ObjectAnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                    this.ObjectAnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                Rectangle rectObjAnchorA = new Rectangle(pDest, new Size(10, 10));
                if (rectObjAnchorA.Contains(pTouched))
                {
                    if (setValue == true)
                        this.ObjectAnchorPointA = new Point(pTouched.X - this.coronaObjA.DisplayObject.SurfaceRect.X,
                            pTouched.Y - this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    return this.ObjectAnchorPointA;
                }
                  
            }

            if (this.ObjectAnchorPointB != Point.Empty)
            {
                Point pDest = new Point(this.ObjectAnchorPointB.X - 5 + this.coronaObjB.DisplayObject.SurfaceRect.X,
                    this.ObjectAnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                Rectangle rectObjAnchorB = new Rectangle(pDest, new Size(10, 10));
                if (rectObjAnchorB.Contains(pTouched))
                {
                    if (setValue == true)
                        this.ObjectAnchorPointB = new Point(pTouched.X - this.coronaObjB.DisplayObject.SurfaceRect.X,
                            pTouched.Y - this.coronaObjB.DisplayObject.SurfaceRect.Y);

                    return this.ObjectAnchorPointB;
                }
                    
            }

            return Point.Empty;
        }


        public void drawGorgon(Point offsetPoint,float worldScale)
        {
            if (this.type.Equals("DISTANCE"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X - 5,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor A","DEFAULT",SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                }

                if (this.AnchorPointB != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointB.X - 5 + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);


                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.LightBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor B", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                }

                if (this.AnchorPointA != Point.Empty && this.AnchorPointB != Point.Empty)
                {
                    List<Point> points = new List<Point>();
                    
                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    points.Add(pDest1);
                    points.Add(pDest2);

                    GorgonGraphicsHelper.Instance.DrawLines(points, Color.FromArgb(150, Color.Blue), 2, worldScale);

                    points.Clear();
                    points = null;
                }
            }
            else if (this.type.Equals("FRICTION"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                
                }
            }
            else if (this.type.Equals("PISTON"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                   
                }

                if (this.axisDistance != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.axisDistance.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.LightBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Axis Distance", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                  
                }

                if (this.AnchorPointA != Point.Empty && this.axisDistance != Point.Empty)
                {
                   
                    List<Point> points = new List<Point>();

                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X
                         , offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.axisDistance.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    points.Add(pDest1);
                    points.Add(pDest2);

                    GorgonGraphicsHelper.Instance.DrawLines(points, Color.FromArgb(150, Color.Red), 2, worldScale);

                    points.Clear();
                    points = null;
                }
            }
            else if (this.type.Equals("PIVOT"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                }
            }
            else if (this.type.Equals("PULLEY"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Rope Anchor A", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                }

                if (this.AnchorPointB != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointB.X - 5 + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Rope Anchor B", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                }

                if (this.AnchorPointA != Point.Empty && this.AnchorPointB != Point.Empty)
                {
                  
                    List<Point> points = new List<Point>();

                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                                           offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    points.Add(pDest1);
                    points.Add(pDest2);

                    GorgonGraphicsHelper.Instance.DrawLines(points, Color.FromArgb(150, Color.Green), 2, worldScale);

                    points.Clear();
                    points = null;
                }

                if (this.ObjectAnchorPointA != Point.Empty && this.AnchorPointA != Point.Empty)
                {

                    Point pDest = new Point(offsetPoint.X + this.ObjectAnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.ObjectAnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.LightBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor A",  "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);


                    List<Point> points = new List<Point>();


                    pDest = new Point(offsetPoint.X + this.ObjectAnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.ObjectAnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDestRopeA = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                   
                    points.Add(pDestRopeA);
                    points.Add(pDest);
                    GorgonGraphicsHelper.Instance.DrawLines(points, Color.FromArgb(150, Color.Green), 2, worldScale);

                    points.Clear();
                    points = null;

                }

                if (this.ObjectAnchorPointB != Point.Empty && this.AnchorPointB != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.ObjectAnchorPointB.X - 5 + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.ObjectAnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.LightBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor B","DEFAULT",SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);


                    List<Point> points = new List<Point>();

                    pDest = new Point(offsetPoint.X + this.ObjectAnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.ObjectAnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    Point pDestRopeB = new Point(offsetPoint.X + this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                   
                    points.Add(pDestRopeB);
                    points.Add(pDest);

                    GorgonGraphicsHelper.Instance.DrawLines(points, Color.FromArgb(150, Color.Green), 2, worldScale);

                    points.Clear();
                    points = null;

                
                }
            }
            else if (this.type.Equals("WELD"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                }
            }
            else if (this.type.Equals("WHEEL"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.DarkBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Anchor", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);

                }

                if (this.axisDistance != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.axisDistance.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    GorgonGraphicsHelper.Instance.FillCircle(pDest.X, pDest.Y, 5, Color.FromArgb(150, Color.LightBlue), worldScale, true);
                    GorgonGraphicsHelper.Instance.DrawText("Axis Distance", "DEFAULT", SystemFonts.DefaultFont.Size, pDest, Color.Red, 0, false, Rectangle.Empty, worldScale);
                   
                }

                if (this.AnchorPointA != Point.Empty && this.axisDistance != Point.Empty)
                {
                 
                    List<Point> points = new List<Point>();


                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.axisDistance.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    points.Add(pDest1);
                    points.Add(pDest2);

                    GorgonGraphicsHelper.Instance.DrawLines(points, Color.FromArgb(150, Color.Red), 2, worldScale);

                    points.Clear();
                    points = null;
                }
            }
        }

        public void dessineJointure(Graphics g, Point offsetPoint)
        {

            if (this.type.Equals("DISTANCE"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X - 5,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);

                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor A", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.AnchorPointB != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointB.X - 5 + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor B", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.AnchorPointA != Point.Empty && this.AnchorPointB != Point.Empty)
                {
                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    g.DrawLine(Pens.Blue, pDest1, pDest2);
                }
            }
            else if (this.type.Equals("FRICTION"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor", new Font("ARIAL", 10), Brushes.Red, pDest);
                }
            }
            else if (this.type.Equals("PISTON"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.axisDistance != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.axisDistance.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(6, 6)));
                    g.DrawString("Axis Distance", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.AnchorPointA != Point.Empty && this.axisDistance != Point.Empty)
                {
                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.axisDistance.X + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    g.DrawLine(Pens.Red, pDest1, pDest2);
                }
            }
            else if (this.type.Equals("PIVOT"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor", new Font("ARIAL", 10), Brushes.Red, pDest);
                }
            }
            else if (this.type.Equals("PULLEY"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(6, 6)));
                    g.DrawString("Rope Anchor A", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.AnchorPointB != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointB.X - 5 + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Rope Anchor B", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.AnchorPointA != Point.Empty && this.AnchorPointB != Point.Empty)
                {
                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    g.DrawLine(Pens.Green, pDest1, pDest2);
                }

                if (this.ObjectAnchorPointA != Point.Empty && this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.ObjectAnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.ObjectAnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor A", new Font("ARIAL", 10), Brushes.Red, pDest);

                    pDest = new Point(offsetPoint.X + this.ObjectAnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.ObjectAnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDestRopeA = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.DrawLine(Pens.DarkBlue, pDestRopeA, pDest);
                }

                if (this.ObjectAnchorPointB != Point.Empty && this.AnchorPointB != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.ObjectAnchorPointB.X - 5 + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.ObjectAnchorPointB.Y - 5 + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest,new Size(10, 10)));
                    g.DrawString("Anchor B", new Font("ARIAL", 10), Brushes.Red, pDest);

                    pDest = new Point(offsetPoint.X + this.ObjectAnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.ObjectAnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    Point pDestRopeB = new Point(offsetPoint.X + this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X
                        , offsetPoint.Y + this.AnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y);
                    g.DrawLine(Pens.DarkBlue, pDestRopeB, pDest);
                }
            }
            else if (this.type.Equals("WELD"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor", new Font("ARIAL", 10), Brushes.Red, pDest);
                }
            }
            else if (this.type.Equals("WHEEL"))
            {
                if (this.AnchorPointA != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.AnchorPointA.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.DarkBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Anchor", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.axisDistance != Point.Empty)
                {
                    Point pDest = new Point(offsetPoint.X + this.axisDistance.X - 5 + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y - 5 + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.FillEllipse(Brushes.LightBlue, new Rectangle(pDest, new Size(10, 10)));
                    g.DrawString("Axis Distance", new Font("ARIAL", 10), Brushes.Red, pDest);
                }

                if (this.AnchorPointA != Point.Empty && this.axisDistance != Point.Empty)
                {
                    Point pDest1 = new Point(offsetPoint.X + this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    Point pDest2 = new Point(offsetPoint.X + this.axisDistance.X + this.coronaObjA.DisplayObject.SurfaceRect.X,
                        offsetPoint.Y + this.axisDistance.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y);
                    g.DrawLine(Pens.Red, pDest1, pDest2);
                }
            }


        }

        //Override ToString()
        //
        public override string ToString()
        {
            return this.name;
        }

        //***********************************
        // Generate Lua Code
        //***********************************

        public void toCodeLua(StringBuilder sb, float XRatio, float YRatio)
        {


            Point offSetFocus = this.layerParent.SceneParent.Camera.SurfaceFocus.Location;
            if (this.type.ToLower().Equals("pivot") || this.type.ToLower().Equals("friction") || this.type.ToLower().Equals("weld"))
            {
                // Lua ->> myJoint = physics.newJoint( "pivot", crateA, crateB, 200,300 )
                // Lua ->> myJoint = physics.newJoint( "friction", crateA, crateB, 200,300 )
                // Lua ->> myJoint = physics.newJoint( "weld", crateA, crateB, 200,300 )
                sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + " = physics.newJoint( \"" + this.type.ToLower() + "\"," + this.layerParent.Name + "." + this.coronaObjA.DisplayObject.Name + ".object," + this.layerParent.Name + "." + this.coronaObjB.DisplayObject.Name + ".object,"
                                                        + ((float)(this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.AnchorPointA.Y+ this.coronaObjA.DisplayObject.SurfaceRect.Y) * YRatio).ToString().Replace(",", ".") + ")");
                if (!this.type.ToLower().Equals("weld"))
                {
                    if (!this.type.ToLower().Equals("friction"))
                    {
                        sb.AppendLine("\t"+this.layerParent.Name+"." + this.name + ".isMotorEnabled=" + this.isMotorEnable.ToString().ToLower());
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".isLimitEnabled=" + this.isLimitEnabled.ToString().ToLower());
                    }
                    else
                    {
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".maxForce=" + this.maxForce.ToString());
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".maxTorque=" + this.maxTorque.ToString());
                    }

                    if (this.isMotorEnable && !this.type.ToLower().Equals("friction"))
                    {
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".motorSpeed=" + this.motorSpeed.ToString());
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".maxMotorTorque=" + this.maxMotorTorque.ToString());
                    }

                    if (this.isLimitEnabled && !this.type.ToLower().Equals("friction"))
                    {
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ":setRotationLimits(" + this.lowerLimit.ToString() + "," + this.upperLimit.ToString() + ")");
                    }
                }

            }
            else if (this.type.ToLower().Equals("distance")){
                // Lua ->> myJoint = physics.newJoint( "distance", crateA, crateB, crateA.x,crateA.y, crateB.x,crateB.y )
                sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + " = physics.newJoint( \"" + this.type.ToLower() + "\"," + this.layerParent.Name + "." + this.coronaObjA.DisplayObject.Name + ".object," + this.layerParent.Name + "." + this.coronaObjB.DisplayObject.Name + ".object,"
                                                + ((float)(this.AnchorPointA.X+ this.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y)* YRatio).ToString().Replace(",", ".") + ","
                                                + ((float)(this.AnchorPointB.X+ this.coronaObjB.DisplayObject.SurfaceRect.X)* XRatio).ToString().Replace(",", ".") + "," + ((float)(this.AnchorPointB.Y+ this.coronaObjB.DisplayObject.SurfaceRect.Y) * YRatio).ToString().Replace(",", ".") + ")");

                sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".frequency=" + this.frequency.ToString());
                sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".dampingRatio=" + this.dampingRatio.ToString().Replace(",","."));
             
            }
            else if (this.type.ToLower().Equals("piston") || this.type.ToLower().Equals("wheel"))
            {
                // Lua ->> myJoint = physics.newJoint( "piston", crateA, crateB, crateA.x,crateA.y, axisDistanceX,axisDistanceY )
                // Lua ->> myJoint = physics.newJoint( "wheel", crateA, crateB, crateA.x,crateA.y, axisDistanceX,axisDistanceY )
                sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + " = physics.newJoint( \"" + this.type.ToLower() + "\"," + this.layerParent.Name + "." + this.coronaObjA.DisplayObject.Name + ".object," + this.layerParent.Name + "." + this.coronaObjB.DisplayObject.Name + ".object,"
                                                + ((float)(this.AnchorPointA.X+ this.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.AnchorPointA.Y+ this.coronaObjA.DisplayObject.SurfaceRect.Y) * YRatio).ToString().Replace(",", ".") + ","
                                                + ((float)(this.axisDistance.X+ this.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.axisDistance.Y+ this.coronaObjA.DisplayObject.SurfaceRect.Y) * YRatio).ToString().Replace(",", ".") + ")");

                sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".isMotorEnabled=" + this.isMotorEnable.ToString().ToLower());

                if (this.type.ToLower().Equals("wheel"))
                {
                    sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".springFrequency=" + this.springFrequency.ToString());
                    sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".springDampingRatio=" + this.springDampingRatio.ToString().Replace(",","."));
                }

              
               
                if (this.isMotorEnable)
                {
                    if (this.type.ToLower().Equals("wheel"))
                    {
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".motorSpeed=" + this.motorSpeed.ToString());
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".maxMotorTorque =" + this.maxMotorTorque.ToString());
                    }
                    else
                    {
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".motorSpeed=" + this.motorSpeed.ToString());
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".maxMotorForce =" + this.maxMotorForce.ToString());
                    }
                    
    
                }


                if (this.type.ToLower().Equals("piston"))
                {
                    sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ".isLimitEnabled=" + this.isLimitEnabled.ToString().ToLower());
                    if (this.isLimitEnabled)
                    {
                        sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + ":setLimits(" + this.lowerLimit.ToString() + "," + this.upperLimit.ToString() + ")");
                    }
                }
            }
            else if (this.type.ToLower().Equals("pulley"))
            {
                // Lua ->> myJoint = physics.newJoint( "pulley", crateA, crateB, anchorA_x,anchorA_y, anchorB_x,anchorB_y, crateA.x,crateA.y, crateB.x,crateB.y, 1.0 )
                sb.AppendLine("\t" + this.layerParent.Name + "." + this.name + " = physics.newJoint( \"" + this.type.ToLower() + "\"," + this.layerParent.Name + "." + this.coronaObjA.DisplayObject.Name + ".object," + this.layerParent.Name + "." + this.coronaObjB.DisplayObject.Name + ".object,"
                    + ((float)(this.AnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.AnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y ) * YRatio).ToString().Replace(",", ".") + ","
                    + ((float)(this.AnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.AnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y ) * YRatio).ToString().Replace(",", ".") + ","
                    + ((float)(this.ObjectAnchorPointA.X + this.coronaObjA.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.ObjectAnchorPointA.Y + this.coronaObjA.DisplayObject.SurfaceRect.Y) * YRatio).ToString().Replace(",", ".") + ","
                    + ((float)(this.ObjectAnchorPointB.X + this.coronaObjB.DisplayObject.SurfaceRect.X) * XRatio).ToString().Replace(",", ".") + "," + ((float)(this.ObjectAnchorPointB.Y + this.coronaObjB.DisplayObject.SurfaceRect.Y) * YRatio).ToString().Replace(",", ".") + "," + "1.0)");
            }
            else if (this.type.ToLower().Equals("touch"))
            {        
                // Lua ->> myJoint = physics.newJoint( "touch", crate, targetX,targetY )
                sb.AppendLine("\t" + this.layerParent.Name + "." + this.coronaObjA.DisplayObject.Name + ".object:addEventListener(\"touch\",dragBody)");
            }
        }

        public CoronaJointure clone(CoronaObject objA, CoronaObject objB,CoronaLayer layerParent)
        {

            CoronaJointure newJoint = new CoronaJointure(layerParent);
            newJoint.isEnabled = this.isEnabled;
            newJoint.coronaObjA = objA;
            newJoint.coronaObjB = objB;
            newJoint.type = this.type;

            if(objB != null)
                newJoint.name = newJoint.type + "_" + objA.DisplayObject.Name + "_" + objB.DisplayObject.Name;
            else
                newJoint.name = newJoint.type + "_" + objA.DisplayObject.Name;

            newJoint.AnchorPointA = new Point(this.AnchorPointA.X, this.AnchorPointA.Y);
            newJoint.AnchorPointB = new Point(this.AnchorPointB.X, this.AnchorPointB.Y);

            newJoint.ObjectAnchorPointA = new Point(this.ObjectAnchorPointA.X, this.ObjectAnchorPointA.Y);
            newJoint.ObjectAnchorPointB = new Point(this.ObjectAnchorPointB.X, this.ObjectAnchorPointB.Y);

            newJoint.axisDistance = new Point(this.axisDistance.X, this.axisDistance.Y);

            newJoint.motorSpeed = this.motorSpeed;
            newJoint.maxMotorForce = this.maxMotorForce;
            newJoint.maxMotorTorque = this.maxMotorTorque;
            newJoint.isMotorEnable = this.isMotorEnable;
            newJoint.isLimitEnabled = this.isLimitEnabled;
            newJoint.upperLimit = this.upperLimit;
            newJoint.lowerLimit = this.lowerLimit;

            newJoint.frequency = this.frequency;
            newJoint.maxForce = this.maxForce;
            newJoint.maxTorque = this.maxTorque;

            return newJoint ;

        }
       
    }
}
