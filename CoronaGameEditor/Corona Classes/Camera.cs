using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krea.CoronaClasses;
using System.Drawing;

namespace Krea.Corona_Classes
{
    [Serializable()]
    public class Camera
    {

        public Scene sceneParent;
        public Rectangle SurfaceFocus;
        public Rectangle CameraFollowLimitRectangle;
        public CoronaObject objectFocusedByCamera;
        public bool isSurfaceFocusVisible;
        public bool isDraggable = false;
        public Camera(Scene sceneParent, Rectangle surfaceFocus, Rectangle cameraFollowLimitRectangle)
        {
            this.sceneParent = sceneParent;

            this.SurfaceFocus = surfaceFocus;
            this.CameraFollowLimitRectangle = cameraFollowLimitRectangle;

            isSurfaceFocusVisible = true;
        }

        public void setObjectFocusedByCamera(CoronaObject obj)
        {
            //Creer deux variables si elles n'existent pas deja
            bool isCreated = false;

            for (int i = 0; i < this.sceneParent.vars.Count; i++)
            {
                CoronaVariable var = this.sceneParent.vars[i];
                if (var.Name.Equals("LastPosX_Focus") || var.Name.Equals("LastPosY_Focus"))
                {
                    isCreated = true;
                    break;
                }
            }

            if (isCreated == false)
            {
                CoronaVariable var_LastPosX = new CoronaVariable("Text", true, "LastPosX_Focus", "nil");
                CoronaVariable var_LastPosY = new CoronaVariable("Text", true, "LastPosY_Focus", "nil");

                this.sceneParent.vars.Add(var_LastPosX);
                this.sceneParent.vars.Add(var_LastPosY);
            }

            this.objectFocusedByCamera = obj;

        }


        public void moveFocusScene(Point p)
        {

            int xMove = this.SurfaceFocus.Location.X - (this.sceneParent.lastPos.X - p.X);
            int yMove = this.SurfaceFocus.Location.Y - (this.sceneParent.lastPos.Y - p.Y);


            this.sceneParent.lastPos.X = p.X;
            this.sceneParent.lastPos.Y = p.Y;


            this.SurfaceFocus.Location = new Point(xMove, yMove);
        }

    }
}
