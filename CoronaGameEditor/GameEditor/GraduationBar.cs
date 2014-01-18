using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace Krea.GameEditor
{
    public partial class GraduationBar : UserControl
    {
        [ObfuscationAttribute(Exclude = true)]
        public enum BarOrientation
        {
            vertical = 1,
            horizontal = 2
        }

        public BarOrientation Orientation;
        public Point offSet;
        private int marge;
        private int MouseLocation;
        private float scale;

        public GraduationBar()
        {
            this.marge = 0;
            this.scale = 1;

            InitializeComponent();

            if (this.Size.Width > this.Size.Height)
                this.Orientation = BarOrientation.horizontal;
            else
                Orientation = BarOrientation.vertical;

            
        }

        public void setDefaultOffset(int marge)
        {
            this.marge = marge;

        }

        public void setScale(float scale)
        {
            this.scale = scale;
            this.Refresh();
        }
        public void reportMouseLocation(int location)
        {
            this.MouseLocation = location;
            this.Refresh();
        }

        public void reportOffSetScrolling(Point offSet)
        {
            this.offSet = offSet;
            this.Refresh();
        }

        private void GraduationBar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            int offSetX = (int) (this.offSet.X * this.scale);
            int offSetY = (int)(this.offSet.Y * this.scale);
            Font font = new Font(new FontFamily("ARIAL"),6.0f);

            //int negativeOffSetX = 0;
            //int negativeOffSetY = 0;
            //if (offSetX < 0)
            //    negativeOffSetX = Math.Abs(offSetX) / 100 % 10;

            //if (offSetY < 0)
            //    negativeOffSetY = Math.Abs(offSetY) / 100 % 10;


            //int uniteOffSetX = Math.Abs(offSetX) % 10;
            //int dizaineOffSetX = Math.Abs(offSetX) / 10 % 10;
            //int centaineOffSetX = Math.Abs(offSetX) / 100 % 10;


            if (this.Orientation == BarOrientation.horizontal)
            {

                int nbLignesDix = (this.Size.Width - this.marge + Math.Abs(offSetX)) / (int)(10 * this.scale);
                int nbLignesCinquante = (this.Size.Width - this.marge + Math.Abs(offSetX)) / (int)(50 * this.scale);
                int nbLignesCent = (this.Size.Width - this.marge + Math.Abs(offSetX)) / (int)(100 * this.scale);

                
                if(this.scale >=0.5f)
                {
                    //DRaw 10
                    Pen pen10 = new Pen(Brushes.Blue);

                    for (int i = 0; i < nbLignesDix + 1; i++)
                    {
                        e.Graphics.DrawLine(pen10, new PointF(i * (10 * this.scale) + this.marge  -offSetX, 0), new PointF(i * (10 * this.scale) + this.marge - offSetX, this.Size.Height / 4));
                    }
                }
                
                
                //DRaw 50
                Pen pen50 = new Pen(Brushes.Green);
                if (this.scale < 0.5f)
                {
                    pen50 = new Pen(Brushes.Blue);
                }

                for (int i = 0; i < nbLignesCinquante + 1; i++)
                {
                    e.Graphics.DrawLine(pen50, new PointF(i * (50 * this.scale) + this.marge - offSetX, 0), new PointF(i * (50 * this.scale) + this.marge - offSetX, this.Size.Height / 2));

                }

                //DRaw 100
                Pen pen100 = new Pen(Brushes.Red);

                for (int i = 0; i < nbLignesCent + 1; i++)
                {
                    e.Graphics.DrawLine(pen100, new PointF(i * (100 * this.scale) + this.marge - offSetX, 0), new PointF(i * (100 * this.scale) + this.marge - offSetX, this.Size.Height));
                    e.Graphics.DrawString((i* 100).ToString(), font, Brushes.Red, i * (100 * this.scale) + this.marge - offSetX, this.Size.Height * 0.5f);
                }

                //Draw Mouse location
                float[] dashValues = { 2, 2 };
                Pen pen = new Pen(Color.Black, 1);
                pen.DashPattern = dashValues;

                e.Graphics.DrawLine(pen, new PointF(((this.MouseLocation * this.scale)) + this.marge - offSetX, 0), new PointF(((this.MouseLocation * this.scale)) + this.marge - offSetX, this.Size.Height));   
            }
            else
            {
                int nbLignesDix = (this.Size.Height - this.marge + Math.Abs(offSetY)) / (int)(10 * this.scale);
                int nbLignesCinquante = (this.Size.Height - this.marge + Math.Abs(offSetY)) / (int)(50 * this.scale);
                int nbLignesCent = (this.Size.Height - this.marge + Math.Abs(offSetY)) / (int)(100 * this.scale);

                

                if (this.scale >= 0.5f)
                {
                    //DRaw 10
                    Pen pen10 = new Pen(Brushes.Blue);
                    for (int i = 0; i < nbLignesDix + 1; i++)
                    {
                        e.Graphics.DrawLine(pen10, new PointF(0, i * (10 * this.scale) + this.marge - offSetY), new PointF(this.Size.Width / 4, i * (10 * this.scale) + this.marge - offSetY));

                    }
                }

                //DRaw 50
                Pen pen50 = new Pen(Brushes.Green);
                if (this.scale < 0.5f)
                {
                    pen50 = new Pen(Brushes.Blue);
                }

                for (int i = 0; i < nbLignesCinquante + 1; i++)
                {
                    e.Graphics.DrawLine(pen50, new PointF(0, i * (50 * this.scale) + this.marge - offSetY), new PointF(this.Size.Width / 2, i * (50 * this.scale) + this.marge - offSetY));

                }

                //DRaw 100
                Pen pen100 = new Pen(Brushes.Red);

                for (int i = 0; i < nbLignesCent + 1; i++)
                {
                    e.Graphics.DrawLine(pen100, new PointF(0, i * (100 * this.scale) + this.marge - offSetY), new PointF(this.Size.Width, i * (100 * this.scale) + this.marge - offSetY));
                    e.Graphics.DrawString((i * 100).ToString(), font, Brushes.Red, (float)this.Size.Width * 0.3f, i * (float)(100.0f * this.scale) + (float)this.marge - (float)offSetY);
                }

                //Draw Mouse location
                float[] dashValues = { 2, 2 };
                Pen pen = new Pen(Color.Black, 1);
                pen.DashPattern = dashValues;

                e.Graphics.DrawLine(pen, new PointF(0, ((this.MouseLocation * this.scale)) + this.marge - offSetY), new PointF(this.Size.Width, ((this.MouseLocation * this.scale)) + this.marge - offSetY));

            }

        }

        private void GraduationBar_SizeChanged(object sender, EventArgs e)
        {
            if (this.Size.Width > this.Size.Height)
                this.Orientation = BarOrientation.horizontal;
            else
                Orientation = BarOrientation.vertical;

            this.Refresh();
        }
    }
}
