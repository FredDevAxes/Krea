using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System;
using Krea.CoronaClasses;



namespace Krea.CGE_Figures
{
    [Serializable]
    public class Polygone : Figure
    {
          List<Point> tabPoints;
           
          public Polygone() {}

          public Polygone(Point position, List<Point> tabPoints, Color fillColor, Color strokeColor, int epaisseur, bool rempli,DisplayObject objParent)
              : base(position, fillColor, strokeColor, rempli, epaisseur,objParent)
           { 
           
               this.tabPoints = tabPoints;
              
           }

          public Polygone(Point position, List<Point> tabPoints, Byte coul_A, Byte coul_R, Byte coul_G, Byte coul_B, int epaisseur, bool rempli,DisplayObject objParent)
              : base(position, coul_A, coul_R,coul_G, coul_B, rempli, epaisseur, objParent)
          {
               this.tabPoints = tabPoints;
          }


         public List<Point> TabPoints
          {
              get { return tabPoints; }
              set { tabPoints = value; }
          }

          public override void Dessine(Graphics g,int alpha,Point offsetPoint)
          {
              //Définir les couleurs en ARGB
              Pen p = new Pen(Color.FromArgb(alpha, this.m_colTrait.R, this.m_colTrait.G, this.m_colTrait.B), this.StrokeSize);
              Brush brosse = new SolidBrush(Color.FromArgb(alpha, this.m_FillColor.R, this.m_FillColor.G, this.m_FillColor.B));


              if (tabPoints.Count > 2)
              {
                  Point[] tabPAjust = new Point[this.tabPoints.Count];
                  for (int i = 0; i < this.tabPoints.Count; i++)
                  {
                      tabPAjust[i] = new Point(offsetPoint.X + this.tabPoints[i].X, offsetPoint.Y + this.tabPoints[i].Y);
                  }

                  if (m_bRempli == true)
                  {
                      g.FillPolygon(brosse, tabPAjust);
                  }
                  g.DrawPolygon(p, tabPAjust);
              }
          }

          public override void DessineAt(Graphics g, Point pointDest,int alpha)
          {
          }

          public override void DrawGorgon(Point pointDest, int alpha, float worldScale)
          {

          }
          public override Bitmap DrawInBitmap(int alpha, float worldScale)
          {
              return null;
          }
          public override bool Clic(Point p, Matrix matrix)
          {
              Rectangle rect = this.getBounds(matrix);
              return rect.Contains(p);
          }

          public override Rectangle getBounds(Matrix matrix)
          {
              GraphicsPath g = new GraphicsPath();

              if (tabPoints.Count > 2)
                  g.AddPolygon(tabPoints.ToArray());

              RectangleF rectF = g.GetBounds(matrix);
              return new Rectangle(new Point((int)rectF.Location.X, (int)rectF.Location.Y), new Size((int)rectF.Width, (int)rectF.Height));
          }

          public override void SetSizeFromPoint(Point pt)
          {
          

          }

          public override Figure CloneInstance(bool keepLocation)
          {
              return null;
          }
         
       }
    }


