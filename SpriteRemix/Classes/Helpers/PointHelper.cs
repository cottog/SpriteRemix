using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;

namespace SpriteRemix.Classes.Helpers
{
    public static class PointHelper
    {
        public static bool PointOnSurface(Point point, int surfaceWidth, int surfaceHeight)
        {
            return (point.X >= 0 && point.X < surfaceWidth && point.Y >= 0 && point.Y < surfaceHeight);
        }

        public static double Distance(Point point1, Point point2)
        {
           return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
