using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AX.WPF.Extensions
{
    public static class MediaExtensions
    {
        public static Color ToColor(this string hexColor)
        {
            return (Color)ColorConverter.ConvertFromString(hexColor);
        }

        public static Color ChangeAlpha(this Color color, byte newAplha)
        {
            return Color.FromArgb(newAplha, color.R, color.G, color.B);
        }

        public static SolidColorBrush ToSolidBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }

        public static Pen ToSolidPen(this Color color, double width = 1)
        {
            return new Pen(color.ToSolidBrush(), width);
        }

        public static Pen SetDash(this Pen pen, DashStyle dashStyle)
        {
            pen.DashStyle = dashStyle;
            return pen;
        }

        public static Pen ToPen(this Brush brush, double width = 1)
        {
            return new Pen(brush, width);
        }

        public static Brush ChangeColor(this Brush brush, Color newColor)
        {
            var solidBrush = brush as SolidColorBrush;
            if (solidBrush != null && !solidBrush.IsFrozen)
            {
                solidBrush.Color = newColor;
            }
            return brush;
        }

        public static Pen ChangeColor(this Pen pen, Color newColor)
        {
            pen.Brush.ChangeColor(newColor);
            return pen;
        }

        public static Brush ChangeOpacity(this Brush brush, double newOpacity)
        {
            brush.Opacity = newOpacity;
            return brush;
        }

        public static Pen ChangeOpacity(this Pen pen, double newOpacity)
        {
            pen.Brush.ChangeOpacity(newOpacity);
            return pen;
        }

        public static Pen ChangeThickness(this Pen pen, double newThickness)
        {
            pen.Thickness = newThickness;
            return pen;
        }

        public static Brush FreezeBrush(this Brush brush)
        {
            brush.Freeze();
            return brush;
        }

        public static Pen FreezePen(this Pen pen)
        {
            pen.Freeze();
            return pen;
        }
    }
}
