using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SpriteRemix.Classes.Tools
{
    public class SinglePointTool : ToolBase
    {
        public override string Name
        {
            get { return "Single Point Tool"; }
        }

        public override string IconSource
        {
            get { return "/Images/paint-brush.png"; }
        }

       

        protected override ToolPaintEventArgs GetPaintEventArgs(Point origin, WriteableBitmap surface)
        {
            ToolPaintEventArgs args = null;
            if (origin != null)
            {
                var list = new List<ColoredPixel>
                {
                    new ColoredPixel() { Point = origin, Color = PrimaryColor }
                };
                args = new ToolPaintEventArgs() { Pixels = list };
            }
            return args;
        }
    }
}