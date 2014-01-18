using System;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace Krea.Corona_Classes
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DescriptionAttribute("The gradient used for this object.")]
    public class GradientColor
    {
        [Flags]
        [ObfuscationAttribute(Exclude = true)]
        public enum Direction
        {
            up = 1,
            down = 2,
            left = 3,
            right = 4,
        }

        public Direction direction;
        public Color color1;
        public Color color2;
        public bool isEnabled = false;

        public GradientColor()
        {
            direction = Direction.down;
        }

        public LinearGradientBrush getBrushForDrawing(Rectangle rect,int alpha)
        {
            LinearGradientBrush br = null;

            if (this.direction == Direction.down)
            {
                br = new LinearGradientBrush(rect, Color.FromArgb(alpha,this.color1), Color.FromArgb(alpha,this.color2), LinearGradientMode.Vertical);
            }
            else if (this.direction == Direction.up)
            {
                Point p1 = new Point(rect.Location.X, rect.Location.Y + rect.Size.Height);
                Point p2 = rect.Location;
                br = new LinearGradientBrush(p1, p2, Color.FromArgb(alpha, this.color1), Color.FromArgb(alpha, this.color2));
            }
            else if (this.direction == Direction.left)
            {
                br = new LinearGradientBrush(rect, Color.FromArgb(alpha, this.color1), Color.FromArgb(alpha, this.color2), LinearGradientMode.Horizontal);
            }
            else if (this.direction == Direction.right)
            {
                Point p1 = new Point(rect.Location.X+rect.Size.Width, rect.Location.Y);
                Point p2 = rect.Location;
                br = new LinearGradientBrush(p1, p2, Color.FromArgb(alpha, this.color1), Color.FromArgb(alpha, this.color2));
            }

            return br;
        }
        

    }
}
