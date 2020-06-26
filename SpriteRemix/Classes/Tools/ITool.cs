using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SpriteRemix.Classes.Tools
{
    public interface ITool
    {
        string Name { get; }
        string IconSource { get; }

        Color PrimaryColor { get; set; }

        event EventHandler<ToolPaintEventArgs> SurfaceColored;
        event EventHandler<ToolPaintEventArgs> PreviewColored;

        void OnClick(Point origin, WriteableBitmap surface);
        void OnPreview(Point origin, WriteableBitmap surface);
    }
}
