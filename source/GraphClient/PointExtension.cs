using System;
using System.Windows;

namespace GraphClient
{
    static class PointExtension
    {
        public static Point Minus(this Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static Point Plus(this Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static double Size(this Point p1)
        {
            return Math.Sqrt(p1.X * p1.X + p1.Y * p1.Y);
        }
        public static Point Unit(this Point p1)
        {
            return p1.Divide(p1.Size());
        }
        public static Point Multi(this Point p1, double scalar)
        {
            return new Point(p1.X * scalar, p1.Y * scalar);
        }
        public static Point Divide(this Point p1, double scalar)
        {
            return new Point(p1.X / scalar, p1.Y / scalar);
        }
        public static double Scalar(this Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }
    }
}
