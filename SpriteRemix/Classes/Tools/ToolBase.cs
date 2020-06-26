using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpriteRemix.Classes.Tools
{
    public abstract class ToolBase : ITool
    {
        public abstract string Name { get; }

        public abstract string IconSource { get; }

        public Color PrimaryColor { get; set; }

        public event EventHandler<ToolPaintEventArgs> SurfaceColored;
        public event EventHandler<ToolPaintEventArgs> PreviewColored;

        protected abstract ToolPaintEventArgs GetPaintEventArgs(Point origin, WriteableBitmap surface);

        public virtual void OnClick(Point origin, WriteableBitmap surface)
        {
            if (origin != null && surface != null)
            {
                var args = GetPaintEventArgs(origin, surface);
                if (args != null)
                    SurfaceColored?.Invoke(this, args);
            }
        }

        public virtual void OnPreview(Point origin, WriteableBitmap surface)
        {
            if (origin != null && surface != null)
            {
                var args = GetPaintEventArgs(origin, surface);
                if (args != null)
                    PreviewColored?.Invoke(this, args);
            }
        }
    }
}
