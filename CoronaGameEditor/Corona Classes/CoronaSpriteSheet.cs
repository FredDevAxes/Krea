using System;
using System.Collections.Generic;
using System.Drawing;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaSpriteSheet
    {

        //---------------------------------------------------
        //-------------------Attributs----------------------
        //---------------------------------------------------
      
        public List<SpriteFrame> Frames;
        public String Name;
        public float FramesFactor = 1;

        [NonSerialized()]
        public Image ImageSpriteSheet;
        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public CoronaSpriteSheet(string name)
        {
            this.Frames = new List<SpriteFrame>();
            this.Name = name;
        }
        //---------------------------------------------------
        //-------------------Methodes------------------------
        //---------------------------------------------------

        public void addFrame(SpriteFrame frame)
        {
            this.Frames.Add(frame);
           
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void dessineAllFrame(Graphics g,float XRatio,float YRatio, string sheetDirectory)
        {
            for (int i = 0; i < this.Frames.Count; i++)
            {
                SpriteFrame frame =this.Frames[i];
                frame.dessineFrame(g, XRatio, YRatio, this.FramesFactor, sheetDirectory);
            }

        }

        public SizeF calculateSize(float XRatio,float YRatio,bool applyFrameFactor)
        {
            try
            {
                float maxX = 0;
                float maxY = 0;

                if (FramesFactor == 0)
                    FramesFactor = 1;

                float factor = 1;
                if (applyFrameFactor == true)
                {
                    factor = FramesFactor;
                }

                //Parcourir les frames
                for (int i = 0; i < this.Frames.Count; i++)
                {
                    SpriteFrame frame = this.Frames[i];
                    float imageWidth = (float)frame.ImageSize.Width;
                    if (imageWidth == 0)
                    {
                        if (frame.Image != null)
                            imageWidth = frame.Image.Width;
                    }

                    float width = (float)frame.Position.X / factor * XRatio + imageWidth / factor * XRatio;
                    if (width > maxX)
                        maxX = width;


                    float imageHeight = (float)frame.ImageSize.Height;
                    if (imageHeight == 0)
                    {
                        if (frame.Image != null)
                            imageHeight = frame.Image.Height;
                    }
                    float height = (float)frame.Position.Y / factor * YRatio + imageHeight / factor * YRatio;
                    if (height > maxY)
                        maxY = height;

                }
                return new SizeF(maxX, maxY);
            }
            catch (Exception ex)
            {
                return SizeF.Empty;
            }
        }

    }
}
