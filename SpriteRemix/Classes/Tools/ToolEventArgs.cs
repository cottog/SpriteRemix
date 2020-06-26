using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SpriteRemix.Classes.Tools
{
    public struct ColoredPixel
    {
        public Point Point;
        public Color Color;
    }

    public class ToolPaintEventArgs
    {
        public List<ColoredPixel> Pixels { get; set; }
    }

}