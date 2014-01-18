using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary;
using GorgonLibrary.Graphics;
using System.Drawing;
using Krea.CoronaClasses;

namespace Krea.Corona_Classes
{
    public class GorgonGraphicsHelper
    {
        private TextSprite textSprite;


        private static GorgonGraphicsHelper instance;
        public static GorgonGraphicsHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GorgonGraphicsHelper();
                   

                }
                return instance;
            }
        }

        public void Init()
        {
            this.textSprite = new TextSprite("TextSprite", string.Empty, FontCache.Fonts[0]);
        }
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------- RECTANGLES -----------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        public void DrawRectangle(System.Drawing.Rectangle rectangleDest, float thickness, System.Drawing.Color color,float worldScale)
        {
            // VERTICAL LINE LEFT
            //Vector2D vectorThickNess = new Vector2D(thickness * worldScale, thickness * worldScale);

            List<Point> points = new List<Point>();
            int TOP = rectangleDest.Top;
            int BOTTOM = rectangleDest.Bottom;
            int LEFT = rectangleDest.Left;
            int RIGHT = rectangleDest.Right;

            //TOPLEFT
            points.Add(new Point(LEFT, TOP));
            points.Add(new Point(RIGHT, TOP));

            points.Add(new Point(RIGHT, BOTTOM));
            points.Add(new Point(LEFT, BOTTOM));

            points.Add(new Point(LEFT, TOP));

            DrawLines(points, color, thickness, worldScale);
            points = null;
        
        }

        private float checkWorldScale(float scale)
        {
            if (scale > 1) scale = 1;
            return scale;
        }

        public void FillRectangle(System.Drawing.Rectangle rectangleDest, float thickness, System.Drawing.Color color, float worldScale,bool checkworlScale)
        {
            //DrawRectangle(rectangleDest, thickness, color,worldScale);
            float finalScale = worldScale;
            if (checkworlScale == true)
                finalScale = this.checkWorldScale(worldScale);

            Gorgon.CurrentRenderTarget.FilledRectangle((int)((float)rectangleDest.Left * worldScale), (int)((float)rectangleDest.Top * worldScale),
                                        (int)((float)rectangleDest.Width * finalScale), (int)((float)rectangleDest.Height * finalScale), color);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------- DRAWING LINES  ------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public void DrawLines(List<System.Drawing.Point> points, Color color, float thickness, float worldScale)
        {

            //Convert point to vector2
            if (points.Count < 2)
                return;

            float finalThickness = thickness * worldScale;
            if(finalThickness<1)
                finalThickness = 1;

            Vector2D vectorThickNess = new Vector2D(finalThickness, finalThickness);
            for (int i = 1; i < points.Count; i++)
            {
                Gorgon.CurrentRenderTarget.Line((int)((float)points[i - 1].X * worldScale), (int)((float)points[i - 1].Y * worldScale), 
                                    (int)(((float)points[i].X - (float)points[i - 1].X)  * worldScale) ,
                                     (int)(((float)points[i].Y - (float)points[i - 1].Y) * worldScale), color, vectorThickNess);

            }

        }

        public void DrawDottedLines(List<System.Drawing.Point> points,int dotWidth, Color color, float thickness, float worldScale)
        {
            //Convert point to vector2
            if (points.Count < 2)
                return;

            if (dotWidth < 1) dotWidth = 1;

            float finalThickness = thickness * worldScale;
            if (finalThickness < 1)
                finalThickness = 1;

            Vector2D vectorThickNess = new Vector2D(finalThickness, finalThickness);
            for (int i = 1; i < points.Count; i++)
            {
                if(i % dotWidth == 0)
                    Gorgon.CurrentRenderTarget.Line((int)((float)points[i - 1].X * worldScale), (int)((float)points[i - 1].Y * worldScale), 
                                     (int)(((float)points[i].X - (float)points[i - 1].X)  * worldScale),
                                      (int)(((float)points[i].Y - (float)points[i - 1].Y)  * worldScale) , color, vectorThickNess);

            }

        }
           //-------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------- DRAWING CURVE  ------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public  void DrawCurve(List<System.Drawing.Point> points, Color color, float thickness, float worldScale)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);
                                path.AddCurve(points.ToArray());
                                path.Flatten();
            PointF[] finalPoints = path.PathPoints;
            path.Dispose();

            //Convert point to vector2
            if (finalPoints.Length < 2)
                return;

            float finalThickness = thickness * worldScale;
            if (finalThickness < 1)
                finalThickness = 1;

            Vector2D vectorThickNess = new Vector2D(finalThickness, finalThickness);
            for (int i = 1; i < finalPoints.Length; i++)
            {
                Gorgon.CurrentRenderTarget.Line((int)((float)finalPoints[i - 1].X * worldScale), (int)((float)finalPoints[i - 1].Y * worldScale),
                                     (int)(((float)finalPoints[i].X - (float)finalPoints[i - 1].X) * worldScale),
                                    (int)(((float)finalPoints[i].Y - (float)finalPoints[i - 1].Y) * worldScale), color, vectorThickNess);

            }
        }

        public  void DrawDottedCurve(List<System.Drawing.Point> points,int dotWidth, Color color, float thickness, float worldScale)
        {
            if (dotWidth < 1) dotWidth = 1;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);
            path.AddCurve(points.ToArray());
            path.Flatten();
            PointF[] finalPoints = path.PathPoints;
            path.Dispose();

            //Convert point to vector2
            if (finalPoints.Length < 2)
                return;

            float finalThickness = thickness * worldScale;
            if (finalThickness < 1)
                finalThickness = 1;

            Vector2D vectorThickNess = new Vector2D(finalThickness, finalThickness);
            for (int i = 1; i < finalPoints.Length; i++)
            {
                if (i % dotWidth == 0)
                    Gorgon.CurrentRenderTarget.Line((int)((float)finalPoints[i - 1].X * worldScale), (int)((float)finalPoints[i - 1].Y * worldScale),
                                    (int)(((float)finalPoints[i].X - (float)finalPoints[i - 1].X) * worldScale),
                                    (int)(((float)finalPoints[i].Y - (float)finalPoints[i - 1].Y) * worldScale), color, vectorThickNess);

            }

        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------- DRAWING CIRCLE  ------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public void DrawCircle(float x, float y, float radius, Color color, float thickness, float worldScale, bool checkworlScale)
        {
            float finalScale = worldScale;
            if (checkworlScale == true)
                finalScale = this.checkWorldScale(worldScale);

            float finalThickness = thickness * worldScale;
            if (finalThickness < 1)
                finalThickness = 1;

            Vector2D vectorThickNess = new Vector2D(finalThickness, finalThickness);

            Gorgon.CurrentRenderTarget.Circle((int)(((float)x * worldScale) + ((float)radius * worldScale)), (int)(((float)y * worldScale) + ((float)radius * finalScale)),
                radius * finalScale, color, vectorThickNess);
        }

        public  void FillCircle(int x, int y, int radius, Color color, float worldScale,bool checkworlScale)
        {
            float finalScale = worldScale;
            if (checkworlScale == true)
                finalScale = this.checkWorldScale(worldScale);

            Gorgon.CurrentRenderTarget.FilledCircle((int)(((float)x * worldScale) + ((float)radius * worldScale)), (int)(((float)y * worldScale) + ((float)radius * finalScale)),
                (int)((float)radius * finalScale), color);

            
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------- DRAWING TEXT  ------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public  void DrawText(string text, string fontName,float fontSize, Point pDest, Color color, int rotation, bool wordWrap, Rectangle rectDest, float worldScale)
        {
            try
            {
                GorgonLibrary.Graphics.Font textFont = GetGorgonFont(fontName,fontSize);
                if (textFont != null)
                {
                    textSprite.Font = textFont;
                }
                else
                {
                    if (fontName.Equals("DEFAULT"))
                    {
                        textFont = new GorgonLibrary.Graphics.Font(fontName+"_"+fontSize,
                           new System.Drawing.Font(SystemFonts.DefaultFont.FontFamily, fontSize));

                    }
                    else
                    {
                        textFont = new GorgonLibrary.Graphics.Font(fontName + "_" + fontSize,
                           new System.Drawing.Font(fontName, fontSize));

                    }

                    textSprite.Font = textFont;
                }
                textSprite.Bounds = null;
                textSprite.WordWrap = false;
                textSprite.Color = color;
                textSprite.Text = text;
                textSprite.SetAxis((int)((float)rectDest.Width ) / 2, (int)((float)rectDest.Height ) / 2);
                textSprite.Rotation = rotation;
               
                if (wordWrap == false)
                {
                    textSprite.Position = new Vector2D((int)((float)pDest.X * worldScale), (int)((float)pDest.Y * worldScale));
                    
                }
                else
                {
                    textSprite.SetPosition((float)(rectDest.X +textSprite.Axis.X) * worldScale, (float)(rectDest.Y+textSprite.Axis.Y) * worldScale);
                    textSprite.Bounds = new Viewport(0, 0, (int)((float)rectDest.Width ), 0);

                    textSprite.WordWrap = wordWrap;
             
                }
           
               

                textSprite.SetScale(worldScale, worldScale);
                textSprite.Draw();
            }
            catch (Exception ex)
            {

            }
            
        }

        public SizeF GetTextSize(string text, string fontName,float fontSize, Point pDest, bool wordWrap, Rectangle rectDest)
        {
            try
            {
                GorgonLibrary.Graphics.Font textFont = GetGorgonFont(fontName, fontSize);
                if (textFont != null)
                {
                    textSprite.Font = textFont;
                }
                else
                {
                    if (fontName.Equals("DEFAULT"))
                    {
                        textFont = new GorgonLibrary.Graphics.Font(fontName + "_" + fontSize,
                           new System.Drawing.Font(SystemFonts.DefaultFont.FontFamily, fontSize));

                    }
                    else
                    {
                        textFont = new GorgonLibrary.Graphics.Font(fontName + "_" + fontSize,
                           new System.Drawing.Font(fontName, fontSize));

                    }

                    textSprite.Font = textFont;
                }
                textSprite.Bounds = null;
                textSprite.WordWrap = false;
            
                textSprite.Text = text;
                textSprite.SetAxis((int)((float)rectDest.Width) / 2, (int)((float)rectDest.Height) / 2);
                textSprite.Rotation = 0;

                if (wordWrap == false)
                {
                    textSprite.Position = new Vector2D(pDest.X, pDest.Y);

                }
                else
                {
                    textSprite.SetPosition(rectDest.X + textSprite.Axis.X, rectDest.Y + textSprite.Axis.Y);
                    textSprite.Bounds = new Viewport(0, 0, rectDest.Width, 0);

                }
            
                textSprite.WordWrap = wordWrap;
             
                //Get the max width of all lines
                float maxLineWidth = 0;
                for (int i = 0; i < textSprite.LineCount; i++)
                {
                    float lineWidth = textSprite.MeasureLine(i);
                    if (lineWidth > maxLineWidth)
                        maxLineWidth = lineWidth;
                }

                float finalWidth = rectDest.Width;
                if (finalWidth < maxLineWidth)
                    finalWidth = maxLineWidth;

                return new SizeF(finalWidth, textSprite.Height);
            }
            catch (Exception ex)
            {
                return new SizeF(0, 0);
            }
            
        }
        public  GorgonLibrary.Graphics.Font GetGorgonFont(string fontName, float fontSize)
        {
            for (int i = 0; i < FontCache.Fonts.Count; i++)
            {
                GorgonLibrary.Graphics.Font font = FontCache.Fonts[i];
                if (font.Name.Equals(fontName+"_"+fontSize))
                {
                    return font;
                }
            }
            return null;
        }

        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------- SELECTION BOXES ------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        public  void DrawSelectionBox(System.Drawing.Rectangle selectionBoxDRAWING, int width, Color color, int dotWidth, int dotHeight, float worldScale)
        {
            //TOP LINE
            DrawHorizontalDottedLine(new System.Drawing.Rectangle(selectionBoxDRAWING.X, selectionBoxDRAWING.Y
                                                                        , selectionBoxDRAWING.Width, width), color, dotHeight, dotWidth, worldScale);

            //BOTTOM Line 
            DrawHorizontalDottedLine( new System.Drawing.Rectangle(selectionBoxDRAWING.X, selectionBoxDRAWING.Bottom
                                                                        , selectionBoxDRAWING.Width, width), color, dotHeight, dotWidth, worldScale);

            //LEFT LINE
            DrawVerticalDottedLine( new System.Drawing.Rectangle(selectionBoxDRAWING.X, selectionBoxDRAWING.Y
                                                                        , width, selectionBoxDRAWING.Height), color, dotWidth, dotHeight, worldScale);

            //RIGHT Line 
            DrawVerticalDottedLine( new System.Drawing.Rectangle(selectionBoxDRAWING.Right, selectionBoxDRAWING.Y
                                                                        , width, selectionBoxDRAWING.Height), color, dotWidth, dotHeight, worldScale);
        }


        public  void DrawVerticalDottedLine(System.Drawing.Rectangle selectionBoxDRAWING, Color color, int dotWidth, int dotHeight, float worldScale)
        {
            //When the width is greater than 0, the user is selecting an area to the right of the starting point
            
            if (selectionBoxDRAWING.Height > 0)
            {

                //Draw the line starting at the starting location and moving to the right
                int totalDot = selectionBoxDRAWING.Height / dotHeight;
                for (int aCounter = 0; aCounter < totalDot; aCounter++)
                {

                    if (aCounter % 2 == 0)
                    {
                        //System.Drawing.Rectangle dotRect = new System.Drawing.Rectangle(selectionBoxDRAWING.X, selectionBoxDRAWING.Y + (aCounter * dotHeight), dotWidth, dotHeight);
                        //FillRectangle(dotRect, 1.0f/worldScale, color, worldScale);

                        int finalX = selectionBoxDRAWING.X;
                        int finalY = selectionBoxDRAWING.Y + (aCounter * dotHeight);

                        Gorgon.CurrentRenderTarget.Line(finalX, finalY, dotWidth, dotHeight, color);
                    }

                }

            }

        }

        public  void DrawHorizontalDottedLine(System.Drawing.Rectangle selectionBoxDRAWING, Color color, int dotWidth, int dotHeight, float worldScale)
        {

            //When the width is greater than 0, the user is selecting an area to the right of the starting point

            if (selectionBoxDRAWING.Width > 0)
            {

                //Draw the line starting at the starting location and moving to the right
                int totalDot = selectionBoxDRAWING.Width / dotWidth;
                for (int aCounter = 0; aCounter < totalDot; aCounter++)
                {
                    if (aCounter % 2 == 0)
                    {
                        //System.Drawing.Rectangle dotRect = new System.Drawing.Rectangle(selectionBoxDRAWING.X + (aCounter * dotWidth), selectionBoxDRAWING.Y, dotWidth, dotHeight);
                        //FillRectangle(dotRect, 1.0f / worldScale, color, worldScale);


                        int finalX = selectionBoxDRAWING.X + (aCounter * dotWidth);
                        int finalY = selectionBoxDRAWING.Y;

                        Gorgon.CurrentRenderTarget.Line(finalX, finalY, dotWidth, dotHeight, color);
                    }

                }

            }

        }
    }
}
