using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Krea.CoronaClasses;
using System.Drawing;
using System.Reflection;

namespace Krea.Asset_Manager.Assets_Property_Converters
{
    [ObfuscationAttribute(Exclude = true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SequencePropertyConverter
    {
            CoronaSpriteSetSequence seq;
          
         //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        
        public SequencePropertyConverter() { }
        public SequencePropertyConverter(CoronaSpriteSetSequence seq)
        {
            this.seq = seq;
          
        }

         //---------------------------------------------------
        //-------------------Assesseurs----------------------
        //---------------------------------------------------
         [Category("GENERAL")]
        [DescriptionAttribute("The name of the animation sequence.")]
        public string Name
        {
         get
            {
                return seq.Name;
            }

            set
            {

                seq.Name = value;
            }
         }

        [Category("ANIMATION")]
        [DescriptionAttribute("The number of loops of the animation:\nValue of -1: bounce back and forth exactly once.\nValue of -2: bounce back and forth forever")]
        public int Iteration
        {
         get
            {
                return seq.Iteration;
            }

            set
            {
                if (value < -2) value = -2;
                seq.Iteration = value;
            }
         }

         [Category("ANIMATION")]
        [DescriptionAttribute("The animation lenght in milliseconds.")]
        public int Lenght
        {
         get
            {
                return seq.SequenceLenght;
            }

            set
            {

                seq.SequenceLenght = value;
                
            }
         }

        [Category("ANIMATION")]
        [DescriptionAttribute("The index of the first frame."),ReadOnly(true)]
        public int StartFrame
        {
         get
            {
                return seq.FrameDepart;
            }
         }

        [Category("ANIMATION")]
        [DescriptionAttribute("The animation frame count."), ReadOnly(true)]
        public int FrameCount
        {
            get
            {
                return seq.FrameCount;
            }
        }


    }
}
