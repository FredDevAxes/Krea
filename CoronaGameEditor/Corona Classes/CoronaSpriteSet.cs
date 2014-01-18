using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Krea.CoronaClasses
{
    [Serializable()]
    public class CoronaSpriteSet
    {
        //---------------------------------------------------
        //-------------------Attributs----------------------
        //---------------------------------------------------
        public List<CoronaSpriteSetSequence> Sequences;
        public CoronaSpriteSetSequence SequenceSelected;
        public int indexFrameDep;
        public int frameCount;
        public List<SpriteFrame> Frames;
        public DisplayObject DisplayObjectParent;
        public int CurrentFrame;
        private int sequenceLoopCount;
        public  String Name;

        [NonSerialized()]
        private Timer timerRefesh;
        [NonSerialized()]
        private PictureBox pictToDrawOn;
        [NonSerialized()]
        bool isAnimationInversed = false;

        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public CoronaSpriteSet(string name)
        {
            Sequences = new List<CoronaSpriteSetSequence>();
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

        public void addSequence(String name, int frameDep, int frameCount, int time, int iteration)
        {
            CoronaSpriteSetSequence sequence = new CoronaSpriteSetSequence(name, frameDep, frameCount, time, iteration);
            this.Sequences.Add(sequence);
        }


        public List<SpriteFrame> checkFramesIntegrity()
        {
            List<SpriteFrame> framesToReturn = new List<SpriteFrame>();
            for (int i = 0; i < this.Frames.Count; i++)
            {
                SpriteFrame frame = this.Frames[i];
                if (!frame.SpriteSheetParent.Frames.Contains(frame))
                    framesToReturn.Add(frame);
                    
            }
            if (framesToReturn.Count > 0)
                return framesToReturn;
            else
            {
                framesToReturn = null;
                return null;
            }
               

        }

        public List<CoronaSpriteSetSequence> checkSequencesIntegrity()
        {
            List<CoronaSpriteSetSequence> sequencesToReturn = new List<CoronaSpriteSetSequence>();
            for (int i = 0; i < this.Sequences.Count; i++)
            {
                CoronaSpriteSetSequence sequence = this.Sequences[i];
                if (sequence != null)
                {
                    if (sequence.FrameDepart - 1 + sequence.FrameCount - 1 >= this.Frames.Count)
                        sequencesToReturn.Add(sequence);
                }
            }

            if (sequencesToReturn.Count > 0)
                return sequencesToReturn;
            else
            {
                sequencesToReturn = null;
                return null;
            }
               
        }

        public void playAnimation(PictureBox pict)
        {
            stopAnimation();
            isAnimationInversed = false;

            this.pictToDrawOn = pict;
            if (SequenceSelected != null)
            {
                this.CurrentFrame = SequenceSelected.FrameDepart - 1;
                sequenceLoopCount = 0;
                this.timerRefesh = new Timer();
                try
                {
                    this.timerRefesh.Interval = this.SequenceSelected.SequenceLenght / Math.Abs(this.SequenceSelected.FrameCount);
                    this.timerRefesh.Tick += new System.EventHandler(TimerRefresh_Tick);
                    this.timerRefesh.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sequence lenght must be greater than " + Math.Abs(this.SequenceSelected.FrameCount), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
        }

        public void stopAnimation()
        {
            if (this.timerRefesh != null)
            {
                this.frameCount = 0;
                this.sequenceLoopCount = 0;
                this.timerRefesh.Stop();
                this.timerRefesh.Dispose();
                this.timerRefesh = null;
            }
            this.pictToDrawOn = null;
        }

        public void dessineCurrentFrame(Graphics g)
        {
            if (this.CurrentFrame <= this.Frames.Count && this.CurrentFrame >= 0)
            {
                if (this.pictToDrawOn != null)
                {
                    Point pDest = new Point((int)(this.pictToDrawOn.Width / 2) - (int)(this.Frames[this.CurrentFrame].Image.Width / 2),
                        (int)(this.pictToDrawOn.Height / 2) - (int)(this.Frames[this.CurrentFrame].Image.Height / 2));

                    g.DrawImage(this.Frames[this.CurrentFrame].Image, new Rectangle(pDest, this.Frames[this.CurrentFrame].Image.Size));
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        //-----------------EVENTS--------------------------------------
        private void TimerRefresh_Tick(object sender, System.EventArgs e)
        {
            if(SequenceSelected != null)
            {
                 
                //Si la sequence est inversée
             /*   if(SequenceSelected.FrameCount < 0)
                {
                    CurrentFrame = CurrentFrame - 1;
                    if (CurrentFrame < 0)
                    {
                        this.CurrentFrame = this.Frames.Count - 1;
                    }

                }

                //Si la sequence est mode normal
                else   
                if(SequenceSelected.FrameCount >0)
                {
                    CurrentFrame = CurrentFrame + 1;
                    if (CurrentFrame > SequenceSelected.FrameDepart -1 + SequenceSelected.FrameCount -1)
                        CurrentFrame = SequenceSelected.FrameDepart -1;
                }*/

                if (SequenceSelected.Iteration > -1)
                {
                    CurrentFrame = CurrentFrame + 1;
                    if (CurrentFrame > SequenceSelected.FrameDepart - 1 + SequenceSelected.FrameCount - 1)
                        CurrentFrame = SequenceSelected.FrameDepart - 1;
                }
                else if (SequenceSelected.Iteration <=-1)
                {
                    if (this.isAnimationInversed == false)
                    {
                        CurrentFrame = CurrentFrame + 1;
                        if (CurrentFrame > SequenceSelected.FrameDepart - 1 + SequenceSelected.FrameCount - 1)
                        {
                            CurrentFrame = CurrentFrame - 2;
                            isAnimationInversed = true;
                        }
                    }
                    else
                    {
                        CurrentFrame = CurrentFrame - 1;
                        if (CurrentFrame < SequenceSelected.FrameDepart - 1)
                        {
                            CurrentFrame = CurrentFrame + 2;
                            isAnimationInversed = false;
                        }
                    }
                }

                bool hasBeenDraw = false;
                if (this.DisplayObjectParent != null)
                {
                    if (this.DisplayObjectParent.AnimSpritePictBxParent != null)
                    {
                        hasBeenDraw = true;
                        this.DisplayObjectParent.AnimSpritePictBxParent.Refresh();
                    }
                }

                if (this.pictToDrawOn != null && hasBeenDraw == false)
                {
                    try
                    {
                        if (this.CurrentFrame >= 0 && this.CurrentFrame < this.Frames.Count)
                        {

                            this.pictToDrawOn.Size = this.Frames[this.CurrentFrame].Image.Size;
                            this.pictToDrawOn.Refresh();
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("A Frame does not exist any more !\nThe animation will stop!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.stopAnimation();
                        return;
                    }
                    
                    
                }

                
                if (frameCount == Math.Abs(SequenceSelected.FrameCount)-1)
                {
                 
                    frameCount = 0;



                    if (SequenceSelected.Iteration > 0)
                    {
                        if (this.sequenceLoopCount == SequenceSelected.Iteration - 1)
                            this.stopAnimation();
                        else
                            this.sequenceLoopCount += 1;
                    }
                    else if(SequenceSelected.Iteration == -1)
                    {

                        if (this.sequenceLoopCount == 1)
                            this.stopAnimation();
                        else
                            this.sequenceLoopCount += 1;
                    }
                    else if (SequenceSelected.Iteration == 0)
                    {
                        this.CurrentFrame = SequenceSelected.FrameDepart - 1;
                    }
                    
                }
                else
                    frameCount += 1;

               

              
            }
        }

        public CoronaSpriteSet CloneInstance()
        {
            CoronaSpriteSet newSet = new CoronaSpriteSet(this.Name);

            List<CoronaSpriteSheet> sheetUsed = new List<CoronaSpriteSheet>();

            for (int i = 0; i < this.Frames.Count; i++)
            {
                if (!sheetUsed.Contains(this.Frames[i].SpriteSheetParent))
                    sheetUsed.Add(this.Frames[i].SpriteSheetParent);
            }

            for (int i = 0; i < sheetUsed.Count; i++)
            {
                CoronaSpriteSheet sheet = new CoronaSpriteSheet(sheetUsed[i].Name);
                sheet.FramesFactor = sheetUsed[i].FramesFactor;

                for (int j = 0; j < sheetUsed[i].Frames.Count; j++)
                {
                    SpriteFrame frame = new SpriteFrame(sheetUsed[i].Frames[j].NomFrame, j, sheetUsed[i].Frames[j].Image, sheet);
                    sheet.addFrame(frame);
                }

            }

            return null;
        }
    }



    [Serializable()]
    public class CoronaSpriteSetSequence
    {
         //---------------------------------------------------
        //-------------------Attributs----------------------
        //---------------------------------------------------
        public String Name;
        public int FrameDepart;
        public int FrameCount;
        public int SequenceLenght;
        public int Iteration;

        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public CoronaSpriteSetSequence(String name, int frameDep, int frameCount, int time, int iteration)
        {
            this.Name = name;
            this.FrameDepart = frameDep;
            this.FrameCount = frameCount;
            this.SequenceLenght = time;
            this.Iteration = iteration;
        }

        public override string ToString()
        {
            /*
            return this.Name + " - Start at frame " + this.FrameDepart + " to " +
                (this.FrameDepart + this.FrameCount -1) + " in " + this.SequenceLenght + " mlls, " + this.Iteration + " time";*/
            return this.Name;
        }

    }
}
