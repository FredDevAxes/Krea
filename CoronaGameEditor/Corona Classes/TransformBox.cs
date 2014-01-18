using System.Collections.Generic;
using System.Drawing;
using Krea.CoronaClasses;
using System.Drawing.Drawing2D;
using Krea.CGE_Figures;

namespace Krea.Corona_Classes
{
    
    public class TransformBox
    {
        private Rectangle btResizeWidth;
        private Rectangle btResizeHeight;
        private Rectangle btResizeBoth;

        List<Rectangle> hotSpotsLine;
        bool isLine = false;
        bool isCurve = false;
        CourbeBezier currentCurve;
        Line currentLine;
        int currentIndexToMove = -1;

        private Rectangle btRotation;
        private CoronaObject objectParent;

        public TransformBox(CoronaObject obj)
        {
            this.objectParent = obj;

            if (obj.isEntity == false)
            {
                if (obj.DisplayObject.Type.Equals("FIGURE"))
                {
                    Figure fig = obj.DisplayObject.Figure;
                    if (fig.ShapeType.Equals("LINE"))
                    {
                        currentLine = (Line)fig;
                        this.hotSpotsLine = new List<Rectangle>();
                        isLine = true;
                    }
                    else if (fig.ShapeType.Equals("CURVE"))
                    {
                        currentCurve = (CourbeBezier)fig;
                        this.hotSpotsLine = new List<Rectangle>();
                        isCurve = true;
                    }
                }
            }
            
            

        }

        public void refreshBtsLocation(Rectangle surfaceRectObject,float xScale, float yScale)
        {
            int hotSpotWidth = (int)((float)8 );
            if (hotSpotWidth % 2 != 0)
                hotSpotWidth = hotSpotWidth + 1;
            int halhHotSpotWidth = hotSpotWidth / 2;
            if (halhHotSpotWidth < 1)
            {
                halhHotSpotWidth = 2;
                hotSpotWidth = 4;
            }
            Size hotSpotSize = new Size(hotSpotWidth, hotSpotWidth);

            int halhHotSpotWidthScaledDiff = 0;
            if (xScale > 1)
                halhHotSpotWidthScaledDiff = (int)((float)halhHotSpotWidth * (1/xScale));

            if (this.isCurve == true)
            {
                this.hotSpotsLine.Clear();
  
                for (int i = 0; i < this.currentCurve.UserPoints.Count; i++)
                {
                    Point p = this.currentCurve.UserPoints[i];
                    Rectangle rectPoint = new Rectangle(new Point(p.X - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                        p.Y - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                    
                    this.hotSpotsLine.Add(rectPoint);
                }
            }
            else if (this.isLine == true)
            {
                this.hotSpotsLine.Clear();
                for (int i = 0; i < this.currentLine.Points.Count; i++)
                {
                    Point p = this.currentLine.Points[i];
                    Rectangle rectPoint = new Rectangle(new Point(p.X - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                        p.Y - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                    this.hotSpotsLine.Add(rectPoint);
                }
            }
            else
            {
                if (this.objectParent.isEntity == false)
                {
                    if (this.objectParent.DisplayObject.Type.Equals("SPRITE"))
                    {
                        //CoronaSpriteSet set = this.objectParent.DisplayObject.SpriteSet;
                        //if (set != null && set.Frames.Count > 0)
                        //{
                        //    float factor = set.Frames[this.objectParent.DisplayObject.CurrentFrame].SpriteSheetParent.FramesFactor;
                        //    if (factor <= 0)
                        //        factor = 1;

                        //    int width = (int)((float)set.Frames[this.objectParent.DisplayObject.CurrentFrame].ImageSize.Width / factor);
                        //    int height = (int)((float)set.Frames[this.objectParent.DisplayObject.CurrentFrame].ImageSize.Height / factor);
                        //    this.objectParent.DisplayObject.SurfaceRect = new Rectangle(this.objectParent.DisplayObject.SurfaceRect.Location, new Size(width, height));
                        //}
                       

                        btResizeWidth = Rectangle.Empty;
                        btResizeHeight = Rectangle.Empty;
                        btResizeBoth = Rectangle.Empty;
                    }
                    else
                    {
                        if (this.objectParent.DisplayObject.Figure != null)
                        {
                            if (this.objectParent.DisplayObject.Figure.ShapeType.Equals("CIRCLE"))
                                btResizeBoth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                    surfaceRectObject.Bottom - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                            else if (this.objectParent.DisplayObject.Figure.ShapeType.Equals("TEXT"))
                                btResizeWidth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                    surfaceRectObject.Y + surfaceRectObject.Height / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                            else
                            {
                                btResizeWidth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                    surfaceRectObject.Y + surfaceRectObject.Height / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);

                                btResizeHeight = new Rectangle(new Point(surfaceRectObject.X + surfaceRectObject.Width / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                    surfaceRectObject.Bottom - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);

                                btResizeBoth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                    surfaceRectObject.Bottom - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                            }
                        }
                        else
                        {
                            btResizeWidth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                surfaceRectObject.Y + surfaceRectObject.Height / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);

                            btResizeHeight = new Rectangle(new Point(surfaceRectObject.X + surfaceRectObject.Width / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                surfaceRectObject.Bottom - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);

                            btResizeBoth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                                surfaceRectObject.Bottom - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                        }

                    }
                    btRotation = new Rectangle(new Point(surfaceRectObject.X + surfaceRectObject.Width / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                        surfaceRectObject.Y + surfaceRectObject.Height / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                }
                else
                {
                    btResizeWidth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                        surfaceRectObject.Y + surfaceRectObject.Height / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);

                    btResizeHeight = new Rectangle(new Point(surfaceRectObject.X + surfaceRectObject.Width / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                        surfaceRectObject.Bottom - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);

                    btResizeBoth = new Rectangle(new Point(surfaceRectObject.Right - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                        surfaceRectObject.Bottom - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);

                    btRotation = new Rectangle(new Point(surfaceRectObject.X + surfaceRectObject.Width / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff,
                        surfaceRectObject.Y + surfaceRectObject.Height / 2 - halhHotSpotWidth + halhHotSpotWidthScaledDiff), hotSpotSize);
                }
                
            }
        
        }

        public void drawGorgon(Point offSetPoint, float worldScale)
        {
            if (this.objectParent.isEntity == false)
            { 
                
                Rectangle surfaceRectObject = objectParent.DisplayObject.SurfaceRect;

                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(surfaceRectObject);
                Matrix mRotate = this.objectParent.DisplayObject.getMatrixForDrawing(surfaceRectObject, 1, 1);
                RectangleF boundsPure = gp.GetBounds(mRotate);
                gp.Dispose();
                Rectangle boundsPureInt = new Rectangle((int)boundsPure.X, (int)boundsPure.Y, (int)boundsPure.Width, (int)boundsPure.Height);
                this.refreshBtsLocation(boundsPureInt, worldScale, worldScale);

                if (this.isCurve == true || this.isLine == true)
                {
                   
                    for (int i = 0; i < this.hotSpotsLine.Count; i++)
                    {
                        Rectangle rect = this.hotSpotsLine[i];
                        Rectangle finalRect = new Rectangle(new Point(rect.X + offSetPoint.X, rect.Y + offSetPoint.Y), rect.Size);

                        GorgonGraphicsHelper.Instance.FillRectangle(finalRect, 2, Color.YellowGreen, worldScale, true);
                         

                        //Draw Location 
                        Rectangle locationInfoRect = new Rectangle(finalRect.X, finalRect.Y - 15, 70, 15);
                        GorgonGraphicsHelper.Instance.FillRectangle(locationInfoRect, 1, Color.FromArgb(150, Color.YellowGreen), worldScale,false);

                        string locationInfoText = (rect.X + rect.Width /2) + ";" + (rect.Y+ rect.Height /2);
                        GorgonGraphicsHelper.Instance.DrawText(locationInfoText, "DEFAULT", 8, Point.Empty, Color.DarkBlue, 0, true, locationInfoRect, worldScale);

                    }
                }
                else
                {
                    Rectangle btRotationrect = new Rectangle(new Point(btRotation.X + offSetPoint.X, btRotation.Y + offSetPoint.Y), btRotation.Size);

                    Rectangle finalRect = new Rectangle((int)((float)(boundsPureInt.X + offSetPoint.X) * worldScale),
                                                                        (int)((float)(boundsPureInt.Y + offSetPoint.Y) * worldScale),
                                                                        (int)((float)(boundsPureInt.Width) * worldScale),
                                                                        (int)((float)(boundsPureInt.Height) * worldScale));
                    
                    GorgonGraphicsHelper.Instance.DrawSelectionBox(finalRect,2,Color.YellowGreen,1,5,worldScale);

                    //Draw Location 
                    Rectangle locationInfoRect = new Rectangle(boundsPureInt.X + offSetPoint.X, boundsPureInt.Y + offSetPoint.Y - 15, 70, 15);
                    GorgonGraphicsHelper.Instance.FillRectangle(locationInfoRect, 1, Color.FromArgb(150, Color.YellowGreen), worldScale, false);

                    string locationInfoText = surfaceRectObject.X + ";" + surfaceRectObject.Y;
                    GorgonGraphicsHelper.Instance.DrawText(locationInfoText, "DEFAULT", 8, Point.Empty, Color.DarkBlue, 0, true, locationInfoRect, worldScale);


                    if (!this.objectParent.DisplayObject.Type.Equals("SPRITE"))
                    {
                        if (this.objectParent.DisplayObject.Figure != null)
                        {
                            if (this.objectParent.DisplayObject.Figure.ShapeType.Equals("TEXT"))
                            {
                                Rectangle btwidth = new Rectangle(new Point(btResizeWidth.X + offSetPoint.X, btResizeWidth.Y + offSetPoint.Y), btResizeWidth.Size);
                                GorgonGraphicsHelper.Instance.FillRectangle(btwidth, 0, Color.YellowGreen, worldScale, true);
                            }
                            else if (this.objectParent.DisplayObject.Figure.ShapeType.Equals("CIRCLE"))
                            {
                                Rectangle btBoth = new Rectangle(new Point(btResizeBoth.X + offSetPoint.X, btResizeBoth.Y + offSetPoint.Y), btResizeBoth.Size);
                                GorgonGraphicsHelper.Instance.FillRectangle(btBoth, 0, Color.YellowGreen, worldScale,true);
                            }
                            else
                            {

                                Rectangle btwidth = new Rectangle(new Point(btResizeWidth.X + offSetPoint.X, btResizeWidth.Y + offSetPoint.Y), btResizeWidth.Size);
                                Rectangle btHeight = new Rectangle(new Point(btResizeHeight.X + offSetPoint.X, btResizeHeight.Y + offSetPoint.Y), btResizeHeight.Size);
                                Rectangle btBoth = new Rectangle(new Point(btResizeBoth.X + offSetPoint.X, btResizeBoth.Y + offSetPoint.Y), btResizeBoth.Size);
                                
                                GorgonGraphicsHelper.Instance.FillRectangle(btwidth, 0, Color.YellowGreen, worldScale, true);
                                //GorgonGraphicsHelper.Instance.DrawRectangle(btwidth, 2, Color.YellowGreen, worldScale);

                                GorgonGraphicsHelper.Instance.FillRectangle(btHeight, 0, Color.YellowGreen, worldScale, true);
                                //GorgonGraphicsHelper.Instance.DrawRectangle(btHeight, 2, Color.YellowGreen, worldScale);

                                GorgonGraphicsHelper.Instance.FillRectangle(btBoth, 0, Color.YellowGreen, worldScale, true);
                                //GorgonGraphicsHelper.Instance.DrawRectangle(btBoth, 2, Color.YellowGreen, worldScale);
                            }
                        }
                        else
                        {

                            Rectangle btwidth = new Rectangle(new Point(btResizeWidth.X + offSetPoint.X, btResizeWidth.Y + offSetPoint.Y), btResizeWidth.Size);
                            Rectangle btHeight = new Rectangle(new Point(btResizeHeight.X + offSetPoint.X, btResizeHeight.Y + offSetPoint.Y), btResizeHeight.Size);
                            Rectangle btBoth = new Rectangle(new Point(btResizeBoth.X + offSetPoint.X, btResizeBoth.Y + offSetPoint.Y), btResizeBoth.Size);
                            
                            GorgonGraphicsHelper.Instance.FillRectangle(btwidth, 0, Color.YellowGreen, worldScale, true);
                            //GorgonGraphicsHelper.Instance.DrawRectangle(btwidth, 2, Color.YellowGreen, worldScale);

                            GorgonGraphicsHelper.Instance.FillRectangle(btHeight, 0, Color.YellowGreen, worldScale, true);
                            //GorgonGraphicsHelper.Instance.DrawRectangle(btHeight, 2, Color.YellowGreen, worldScale);

                            GorgonGraphicsHelper.Instance.FillRectangle(btBoth, 0, Color.YellowGreen, worldScale, true);
                            //GorgonGraphicsHelper.Instance.DrawRectangle(btBoth, 2, Color.YellowGreen, worldScale);
                        }
                       


                    }

                    if (this.objectParent.DisplayObject.Type.Equals("FIGURE"))
                    {
                        if (!this.objectParent.DisplayObject.Figure.ShapeType.Equals("CIRCLE"))
                        {
                            GorgonGraphicsHelper.Instance.FillCircle(btRotationrect.X, btRotationrect.Y, btRotationrect.Width / 2, Color.YellowGreen, worldScale, true);
                        }
                    }
                    else
                    {
                        GorgonGraphicsHelper.Instance.FillCircle(btRotationrect.X, btRotationrect.Y, btRotationrect.Width / 2,
                            Color.YellowGreen, worldScale,true);
                        
                    }
                       

                    //GorgonGraphicsHelper.Instance.DrawCircle(btRotationrect.X, btRotationrect.Y, btRotationrect.Width / 2, Color.YellowGreen, 2, worldScale);
              
                }

            }
            else
            {
                GraphicsPath gpGeneral = new GraphicsPath();
                for (int i = 0; i < this.objectParent.Entity.CoronaObjects.Count; i++)
                {
                    GraphicsPath gp = new GraphicsPath();
                    CoronaObject objChild = this.objectParent.Entity.CoronaObjects[i];
                    gp.AddRectangle(objChild.DisplayObject.SurfaceRect);
                    Matrix mRotate = objChild.DisplayObject.getMatrixForDrawing(objChild.DisplayObject.SurfaceRect, 1, 1);
                    gp.Transform(mRotate);
                    gpGeneral.AddRectangle(gp.GetBounds());
                    gp.Dispose();

                }
                RectangleF boundsPure = gpGeneral.GetBounds();
                gpGeneral.Dispose();
                Rectangle boundsPureInt = new Rectangle((int)boundsPure.X, (int)boundsPure.Y, (int)boundsPure.Width, (int)boundsPure.Height);
                this.refreshBtsLocation(boundsPureInt, worldScale, worldScale);

                GorgonGraphicsHelper.Instance.DrawSelectionBox(new Rectangle((int)((float)(boundsPureInt.X + offSetPoint.X) * worldScale),
                                                                          (int)((float)(boundsPureInt.Y + offSetPoint.Y) * worldScale),
                                                                          (int)((float)(boundsPureInt.Width) * worldScale),
                                                                          (int)((float)(boundsPureInt.Height) * worldScale)),
                                                                          2, Color.YellowGreen, 1, 5, worldScale);

            }
        }

        public void drawTransformBox(Graphics g, Point offSetPoint,float xScale, float yScale)
        {
            if (this.objectParent.isEntity == false)
            {

                Rectangle surfaceRectObject = objectParent.DisplayObject.SurfaceRect;

                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(surfaceRectObject);
                Matrix mRotate = this.objectParent.DisplayObject.getMatrixForDrawing(surfaceRectObject, 1, 1);
                RectangleF boundsPure = gp.GetBounds(mRotate);
                gp.Dispose();
                Rectangle boundsPureInt = new Rectangle((int)boundsPure.X, (int)boundsPure.Y, (int)boundsPure.Width, (int)boundsPure.Height);
                this.refreshBtsLocation(boundsPureInt, xScale, yScale);
                Pen pen = new Pen(Brushes.YellowGreen, 2);
                float[] dashValues = { 2, 1 };
                pen.DashPattern = dashValues;


                if (this.isCurve == true || this.isLine == true)
                {
                    for (int i = 0; i < this.hotSpotsLine.Count; i++)
                    {
                        Rectangle rect = this.hotSpotsLine[i];
                        Rectangle finalRect = new Rectangle(new Point(rect.X + offSetPoint.X, rect.Y + offSetPoint.Y), rect.Size);
                        g.FillRectangle(Brushes.LightGoldenrodYellow, finalRect);
                        g.DrawRectangle(new Pen(Color.YellowGreen,2), finalRect);
                    }
                }
                else
                {
                    Rectangle btRotationrect = new Rectangle(new Point(btRotation.X + offSetPoint.X, btRotation.Y + offSetPoint.Y), btRotation.Size);
                    g.DrawRectangle(pen, new Rectangle(boundsPureInt.X + offSetPoint.X, boundsPureInt.Y + offSetPoint.Y, boundsPureInt.Width, boundsPureInt.Height));

                    if (!this.objectParent.DisplayObject.Type.Equals("SPRITE"))
                    {
                        Pen pen2 = new Pen(Color.YellowGreen, 2);
                        //pen2.DashPattern = dashValues;

                        Rectangle btwidth = new Rectangle(new Point(btResizeWidth.X + offSetPoint.X, btResizeWidth.Y + offSetPoint.Y), btResizeWidth.Size);
                        Rectangle btHeight = new Rectangle(new Point(btResizeHeight.X + offSetPoint.X, btResizeHeight.Y + offSetPoint.Y), btResizeHeight.Size);
                        Rectangle btBoth = new Rectangle(new Point(btResizeBoth.X + offSetPoint.X, btResizeBoth.Y + offSetPoint.Y), btResizeBoth.Size);

                        g.FillRectangle(Brushes.Gray, btwidth);
                        g.DrawRectangle(pen2, btwidth);

                        g.FillRectangle(Brushes.Gray, btHeight);
                        g.DrawRectangle(pen2, btHeight);

                        g.FillRectangle(Brushes.Gray, btBoth);
                        g.DrawRectangle(pen2, btBoth);

                    }
                    
                    g.FillEllipse(Brushes.Gray, btRotationrect);
                    g.DrawEllipse(new Pen(Color.YellowGreen, 2), btRotationrect);
                }

            }
            else
            {
                GraphicsPath gpGeneral = new GraphicsPath();
                for (int i = 0; i < this.objectParent.Entity.CoronaObjects.Count; i++)
                {
                    GraphicsPath gp = new GraphicsPath();
                    CoronaObject objChild = this.objectParent.Entity.CoronaObjects[i];
                    gp.AddRectangle(objChild.DisplayObject.SurfaceRect);
                    Matrix mRotate = objChild.DisplayObject.getMatrixForDrawing(objChild.DisplayObject.SurfaceRect, 1, 1);
                    gp.Transform(mRotate);
                    gpGeneral.AddRectangle(gp.GetBounds());
                    gp.Dispose();

                }
                RectangleF boundsPure = gpGeneral.GetBounds();
                gpGeneral.Dispose();
                Rectangle boundsPureInt = new Rectangle((int)boundsPure.X, (int)boundsPure.Y, (int)boundsPure.Width, (int)boundsPure.Height);
                this.refreshBtsLocation(boundsPureInt, xScale, yScale);

                Pen pen = new Pen(Brushes.YellowGreen, 2);
                float[] dashValues = { 2, 1 };
                pen.DashPattern = dashValues;

                g.DrawRectangle(pen, new Rectangle(boundsPureInt.X + offSetPoint.X, boundsPureInt.Y + offSetPoint.Y, boundsPureInt.Width, boundsPureInt.Height));
            }
         
        }

        public string getModeFromPointTouched(Point p)
        {
            if (this.objectParent.isEntity == false)
            {
                if (this.isCurve == true || this.isLine == true)
                {
                    int indexHotSpotTouched = this.getIndexLinePointTouched(p);
                    if (indexHotSpotTouched < 0)
                    {
                        if (this.objectParent.DisplayObject.containsPoint(p))
                            return "MOVE";

                        return "NONE";
                    }
                    else
                    {
                        currentIndexToMove = indexHotSpotTouched;
                        return "MOVE_LINE_POINT";
                    }
                }
                else
                {
                    if (!this.objectParent.DisplayObject.Type.Equals("SPRITE"))
                    {
                        if (this.btResizeBoth.Contains(p))
                            return "RESIZE_BOTH";
                        else if (this.btResizeHeight.Contains(p))
                            return "RESIZE_HEIGHT";
                        else if (this.btResizeWidth.Contains(p))
                            return "RESIZE_WIDTH";
                        else if (this.btRotation.Contains(p))
                            return "ROTATION";
                        else if (this.objectParent.DisplayObject.containsPoint(p))
                            return "MOVE";
                        else
                            return "NONE";
                    }
                    else
                    {

                        if (this.objectParent.DisplayObject.Type.Equals("FIGURE"))
                        {
                            if (this.objectParent.DisplayObject.Figure.ShapeType.Equals("CIRCLE"))
                            {
                                if (this.objectParent.DisplayObject.containsPoint(p))
                                    return "MOVE";
                                else
                                    return "NONE";
                            }
                            else
                            {
                                if (this.btRotation.Contains(p))
                                    return "ROTATION";
                                else if (this.objectParent.DisplayObject.containsPoint(p))
                                    return "MOVE";
                                else
                                    return "NONE";
                            }
                        }
                        else
                        {
                            if (this.btRotation.Contains(p))
                                return "ROTATION";
                            else if (this.objectParent.DisplayObject.containsPoint(p))
                                return "MOVE";
                            else
                                return "NONE";
                        }
                       
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.objectParent.Entity.CoronaObjects.Count; i++)
                {
                    if (this.objectParent.Entity.CoronaObjects[i].DisplayObject.containsPoint(p))
                        return "MOVE";
                }
            }
            
            return "NONE";
        }

        public int getIndexLinePointTouched(Point p)
        {
            for (int i = 0; i < this.hotSpotsLine.Count; i++)
            {
                Rectangle rect = this.hotSpotsLine[i];
                if(rect.Contains(p))
                {
                    return i;
                }
            }

            return -1;
        }

        public void moveLinePoint(Point newPoint)
        {
            if (this.isCurve == true)
            {
                this.currentCurve.UserPoints[currentIndexToMove] =  newPoint ;
                this.objectParent.DisplayObject.SurfaceRect = this.currentCurve.getBounds(new Matrix());
                this.objectParent.PhysicsBody.updateBody();
            }
            else if (this.isLine == true)
            {
                this.currentLine.Points[currentIndexToMove] = newPoint;
                this.objectParent.DisplayObject.SurfaceRect = this.currentLine.getBounds(new Matrix());
                this.objectParent.PhysicsBody.updateBody();
            }
        }
    }
}
