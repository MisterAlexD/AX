using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AX.WPF.Extensions
{
    public static class PointExtensions
    {
        public static Point Subtract(this Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point SubtractX(this Point p, double x)
        {
            return new Point(p.X - x, p.Y);
        }

        public static Point SubtractY(this Point p, double y)
        {
            return new Point(p.X, p.Y - y);
        }

        public static Point AbsSubtract(this Point p1, Point p2)
        {
            return new Point(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }

        public static Point Add(this Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point Add(this Point p1, double x, double y)
        {
            return new Point(p1.X + x, p1.Y + y);
        }

        public static Point AddX(this Point p, double x)
        {
            return new Point(p.X + x, p.Y);
        }

        public static Point AddY(this Point p, double y)
        {
            return new Point(p.X, p.Y + y);
        }

        public static Point Round(this Point p1, int decimals = 0)
        {
            return new Point(Math.Round(p1.X, decimals), Math.Round(p1.Y, decimals));
        }

        public static Point Multiply(this Point p, double a)
        {
            return new Point(p.X * a, p.Y * a);
        }

        public static double DistanceTo(this Point a, Point p)
        {
            return Math.Sqrt(Math.Pow(p.X - a.X, 2) + Math.Pow(p.Y - a.Y, 2));
        }

        public static Point GetMiddle(this Point a, Point b)
        {
            return new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }

        public static Size ToSize(this Point p1)
        {
            return new Size(p1.X, p1.Y);
        }

        public static Rect RectAround(this Point center, double halfWidth, double halfHeight)
        {
            return new Rect(new Point(center.X - halfWidth, center.Y - halfHeight), new Size(halfWidth * 2, halfHeight * 2));
        }

    }
}
