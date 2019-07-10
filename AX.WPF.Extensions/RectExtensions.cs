using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AX.WPF.Extensions
{
    public static class RectExtensions
    {
        public static Point GetNW(this Rect rect)
        {
            return rect.Location;
        }

        public static Point GetNE(this Rect rect)
        {
            return rect.Location.AddX(rect.Width);
        }

        public static Point GetSE(this Rect rect)
        {
            return rect.Location.Add(rect.Width, rect.Height);
        }

        public static Point GetSW(this Rect rect)
        {
            return rect.Location.AddY(rect.Height);
        }

        public static Point GetCenter(this Rect rect)
        {
            return rect.Location.Add(new Point(rect.Width / 2, rect.Height / 2));
        }
    }
}
