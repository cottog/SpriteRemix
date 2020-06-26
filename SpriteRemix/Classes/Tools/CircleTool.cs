using System.Collections.Generic;
using System.Windows;
using SpriteRemix.Classes.Helpers;
using System.Windows.Media.Imaging;

namespace SpriteRemix.Classes.Tools
{
    public class CircleTool : ToolBase
    {
        public override string Name
        {
            get { return "Circle Tool"; }
        }

        public override string IconSource
        {
            get { return "/Images/circle.png"; }
        }

        public int Radius { get; set; } = 15;

        protected override ToolPaintEventArgs GetPaintEventArgs(Point origin, WriteableBitmap surface)
        {
            ToolPaintEventArgs args = null;
            if (origin != null && surface != null)
            {
                var circlePoints = new List<ColoredPixel>();
                for (int i = -Radius; i <= Radius; ++i)
                {
                    for (int j = -Radius; j <= Radius; ++j)
                    {
                        var point = new Point(origin.X + j, origin.Y + i);
                        if (PointHelper.PointOnSurface(point, surface.PixelWidth, surface.PixelHeight))
                        {
                            if (PointHelper.Distance(origin, point) <= Radius)
                                circlePoints.Add(new ColoredPixel() { Point = point, Color = PrimaryColor });
                        }
                    }
                }
                args = new ToolPaintEventArgs() { Pixels = circlePoints };
            }

            return args;
        }

    }
}

