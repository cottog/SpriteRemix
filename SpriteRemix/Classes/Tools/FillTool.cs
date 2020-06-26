using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SpriteRemix.Classes.Tools
{
    public class FillTool : ToolBase
    {
        public override string Name
        {
            get { return "Fill Tool"; }
        }

        public override string IconSource
        {
            get { return "/Images/paint-can.png"; }
        }

        protected override ToolPaintEventArgs GetPaintEventArgs(Point origin, WriteableBitmap surface)
        {
            ToolPaintEventArgs args = null;
            if (origin != null && surface != null)
            {

                var points = new List<Point>();
                var color = surface.GetPixel((int)origin.X, (int)origin.Y);
                GetContiguousPoint(origin, surface, color, points);

                if (points != null && points.Count > 0)
                {
                    var list = new List<ColoredPixel>();
                    foreach (var point in points)
                    {
                        list.Add(new ColoredPixel() { Point = point, Color = PrimaryColor });
                    }
                    args = new ToolPaintEventArgs() { Pixels = list };
                }
            }
            return args;
        }

        private void GetContiguousPoint(Point point, WriteableBitmap surface, Color matchingColor, List<Point> currentPoints)
        {
            if (point.X < 0 || point.X >= surface.PixelWidth)
                return;
            if (point.Y < 0 || point.Y >= surface.PixelHeight)
                return;

            var color = surface.GetPixel((int)point.X, (int)point.Y);
            if (currentPoints.Contains(point) || color != matchingColor)
                return;

            currentPoints.Add(point);
            GetContiguousPoint(new Point(point.X + 1, point.Y), surface, matchingColor, currentPoints);
            GetContiguousPoint(new Point(point.X - 1, point.Y), surface, matchingColor, currentPoints);
            GetContiguousPoint(new Point(point.X, point.Y + 1), surface, matchingColor, currentPoints);
            GetContiguousPoint(new Point(point.X, point.Y - 1), surface, matchingColor, currentPoints);
        }
    }
}
