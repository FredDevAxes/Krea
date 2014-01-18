using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Krea.CoronaClasses;

namespace Krea.CGE_Figures
{
    /// <summary>
    /// Figure est la classe de base abstraite, à partir de laquelle
    /// vont dériver des classes comme Cercle, Boite, Triangle, Pentagone
    /// qui elles serviront à représenter des primitives géométriques
    /// </summary>
    /// 

     [Serializable]
   

    public abstract class Figure
    {
        
        //Champs protégés (accessibles seulement par les classes dérivées)
        
        protected Point m_ptPosition = new Point(0, 0);
        protected Color m_colTrait = Color.Empty;
        protected Color m_FillColor = Color.Empty;
        protected bool m_bRempli = false;
        protected int m_nEpaisseur = 1;
        protected Byte m_A = new Byte();
        protected Byte m_B = new Byte();
        protected Byte m_R = new Byte();
        protected Byte m_G = new Byte();
        protected Point lastPos;
        protected String typeFigure;
        public DisplayObject DisplayObjectParent;

        //Constructeurs
        public Figure() {
         
        }

        public Figure(Point pt, Color fillColor, Color strokeColor, bool b, int Ep,DisplayObject objParent)
        {
            this.m_ptPosition = pt;
            this.DisplayObjectParent = objParent;

            m_FillColor = fillColor;

            this.m_colTrait = strokeColor;

            this.m_bRempli = b;
            this.m_nEpaisseur = Ep;
        }
        public Figure(Point pt, Byte coul_A, Byte coul_R, Byte coul_G, Byte coul_B, bool b, int Ep,DisplayObject objParent)
        {
            this.m_ptPosition = pt;
            this.DisplayObjectParent = objParent;
            m_A = coul_A;
            m_R = coul_R;
            m_G = coul_G;
            m_B = coul_B;
            this.m_bRempli = b;
            this.m_nEpaisseur = Ep;
        }
            
        public Figure(Point pt,DisplayObject objParent)
        {
            this.m_ptPosition = pt;
            this.DisplayObjectParent = objParent;
        }



        //Propriétés
        public Point Position
        {
            get { return this.m_ptPosition; }
            set { this.m_ptPosition = value; }
        }

        public Point LastPos
        {
            get { return this.lastPos; }
            set { this.lastPos = value; }
        }

        public Color StrokeColor
        {
            get { return this.m_colTrait; }
            set { this.m_colTrait = value; }
        }

        public Color FillColor
        {
            get { return this.m_FillColor; }
            set { this.m_FillColor = value; }
        }

        public String ShapeType
        {
            get { return this.typeFigure; }
            set { this.typeFigure = value; }
        }

        public bool Fill
        {
            get { return this.m_bRempli; }
            set { this.m_bRempli = value; }
        }

        public int StrokeSize
        {
            get { return this.m_nEpaisseur; }
            set { this.m_nEpaisseur = value; }
        }


        public Byte G
        {
            get { return m_G; }
            set { m_G = value; }
        }


        public Byte A
        {
            get { return m_A; }
            set { m_A = value; }
        }

        [Browsable(false)]
        public Byte B
        {
            get { return m_B; }
            set { m_B = value; }
        }

        public Byte R
        {
            get { return m_R; }
            set { m_R = value; }
        }

        //méthodes abstraites qui doivent être surchargées
        public abstract void Dessine(Graphics g,int alpha,Point offsetPoint);
        public abstract void DessineAt(Graphics g,Point pDest,int alpha);
        public abstract bool Clic(Point pt,Matrix matrix);
        public abstract void SetSizeFromPoint(Point pt);
        public abstract Figure CloneInstance(bool keepLocation);
        public abstract Rectangle getBounds(Matrix matrix);
        public abstract void DrawGorgon(Point pointDest, int alpha, float worldScale);
        public abstract Bitmap DrawInBitmap(int alpha, float worldScale);
    }

      
        }
    