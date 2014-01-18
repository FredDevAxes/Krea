using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Krea.RemoteDebugger
{
    public partial class CompassView : UserControl
    {
        public CompassView()
        {
            InitializeComponent();
        }

        private float compassRotation = 0;
        private bool isMouseDown = false;
        private float receivedValue = 0;
        private AppRemoteController mainController;

        public void init(AppRemoteController mainController)
        {
          this.mainController = mainController;
        }
        private void CompassView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Matrix matrix = new Matrix();
            matrix.RotateAt(compassRotation,new PointF(this.surfaceDessin.Width/2,this.surfaceDessin.Height/2));

            g.Transform = matrix;

            float xFactor = (float)this.surfaceDessin.Size.Width / (float)Properties.Resources.compass.Width;
            float yFactor = (float)this.surfaceDessin.Size.Height / (float)Properties.Resources.compass.Height;

            float finalFactor = 1;
            if (xFactor > yFactor)
                finalFactor = yFactor;
            else
                finalFactor = xFactor;

            SizeF size = new SizeF((float)Properties.Resources.compass.Width * finalFactor,(float)Properties.Resources.compass.Height * finalFactor);
            float xDest = this.surfaceDessin.Width / 2 - size.Width / 2;
            float yDest = this.surfaceDessin.Height / 2 - size.Height / 2;
            
            g.DrawImage(Properties.Resources.compass, new RectangleF(new PointF(xDest, yDest), size));
        }

        private void CompassView_MouseDown(object sender, MouseEventArgs e)
        {
            this.isMouseDown = true;

            double angle = this.Angle((double)this.surfaceDessin.Size.Width / 2, (double)this.surfaceDessin.Size.Height / 2,
                       (double)e.Location.X, (double)e.Location.Y);

            this.compassRotation = (float)angle;

            this.refreshCompassValues();

            this.mainController.sendCompassEvent();

            this.surfaceDessin.Refresh();
        }

        private void refreshCompassValues()
        {

            string text = "";
            float angle = this.receivedValue;

            if (angle <= 22 || angle > 337)
                text = "N";
            else if (angle > 22 && angle <= 67)
                text = "NE";
            else if (angle > 67 && angle <= 112)
                text = "E";
            else if (angle > 112 && angle <= 157)
                text = "SE";
            else if (angle > 157 && angle <= 202)
                text = "S";
            else if (angle > 202 && angle <= 247)
                text = "SW";
            else if (angle > 247 && angle <= 292)
                text = "W";
            else if (angle > 292 && angle <= 337)
                text = "NW";


            this.angleTxtBx.Text = angle.ToString() + "°";
            this.directionLabel.Text = text;
        }

        private void surfaceDessin_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isMouseDown == true)
            {
                double angle = this.Angle((double)this.surfaceDessin.Size.Width / 2, (double)this.surfaceDessin.Size.Height / 2,
                        (double)e.Location.X, (double)e.Location.Y);

                this.compassRotation = (float)angle;
                receivedValue = 360 - this.compassRotation;

                this.refreshCompassValues();

                this.mainController.sendCompassEvent();

                this.surfaceDessin.Refresh();


            }
        }

        private void surfaceDessin_MouseUp(object sender, MouseEventArgs e)
        {
            this.isMouseDown = false;
        }


        public double Angle(double px1, double py1, double px2, double py2)
        {

            // Negate X and Y values
            double pxRes = px2 - px1;

            double pyRes = py2 - py1;
            double angle = 0.0;

            // Calculate the angle
            if (pxRes == 0.0)
            {
                if (pxRes == 0.0)

                    angle = 0.0;
                else if (pyRes > 0.0) angle = System.Math.PI / 2.0;

                else
                    angle = System.Math.PI * 3.0 / 2.0;

            }
            else if (pyRes == 0.0)
            {
                if (pxRes > 0.0)

                    angle = 0.0;

                else
                    angle = System.Math.PI;

            }

            else
            {
                if (pxRes < 0.0)

                    angle = System.Math.Atan(pyRes / pxRes) + System.Math.PI;
                else if (pyRes < 0.0) angle = System.Math.Atan(pyRes / pxRes) + (2 * System.Math.PI);

                else
                    angle = System.Math.Atan(pyRes / pxRes);

            }

            // Convert to degrees
            angle = angle * 180 / System.Math.PI; return angle;



        }

        private void CompassView_SizeChanged(object sender, EventArgs e)
        {
            this.surfaceDessin.Refresh();
        }

        public void applyAngleFromCompassValues(float compassValue)
        {
            receivedValue = compassValue;

            float currentRotation = this.compassRotation;
            float delta = -compassValue - currentRotation;
            if(Math.Abs(delta) >=180)
            {
                if(delta< -180) delta = delta +360;
                else if( delta> 180) delta = delta -360;
            }


            this.compassRotation = currentRotation + delta * 0.3f;
          

        }

        public float getLastValue()
        {
            return this.receivedValue;
        }

        public void refreshGUI()
        {
            this.refreshCompassValues();
            this.surfaceDessin.Refresh();
        }
    }
}
