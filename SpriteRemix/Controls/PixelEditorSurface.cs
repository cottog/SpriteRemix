using SpriteRemix.Classes.Helpers;
using SpriteRemix.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpriteRemix.Controls
{
    public class PixelEditorSurface : FrameworkElement
    {
        private readonly BitmapSurface drawingSurface;
        private readonly Visual gridLines;
        private readonly BitmapSurface previewSurface;

        public int PixelWidth { get; } = 32;
        public int PixelHeight { get; } = 32;
        public int Magnification { get; } = 20;

        public static readonly DependencyProperty ToolProperty =
            DependencyProperty.Register(
            "Tool", typeof(ITool),
            typeof(PixelEditorSurface),
            new FrameworkPropertyMetadata(
                default(ITool),
                new PropertyChangedCallback(OnFirstPropertyChanged))
        );

        private static void OnFirstPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var surfaceControl = d as PixelEditorSurface;
            if (surfaceControl != null)
            {
                surfaceControl.HandleEventSubscriptions(e.OldValue as ITool, e.NewValue as ITool);
            }
        }

        private void HandleEventSubscriptions(ITool oldTool, ITool newTool)
        {
            if (oldTool != null)           
            {
                oldTool.PreviewColored -= OnPreviewColored;
                oldTool.SurfaceColored -= OnDrawingColored;
            }

            if (newTool != null)
            {
                newTool.PreviewColored += OnPreviewColored;
                newTool.SurfaceColored += OnDrawingColored;
            }
        }

        public ITool Tool
        {
            get { return (ITool)GetValue(ToolProperty); }
            set 
            {
                var prevTool = (ITool)GetValue(ToolProperty);
                if (prevTool != null)
                {
                    prevTool.PreviewColored -= OnPreviewColored;
                    prevTool.SurfaceColored -= OnDrawingColored;
                }

                value.PreviewColored += OnPreviewColored;
                value.SurfaceColored += OnDrawingColored;

                SetValue(ToolProperty, value); 
            }
        }

        public PixelEditorSurface()
        {
            drawingSurface = new BitmapSurface(this);
            gridLines = CreateGridLines();
            previewSurface = new BitmapSurface(this);

            Cursor = Cursors.Pen;

            AddVisualChild(drawingSurface);
            AddVisualChild(previewSurface);
            AddVisualChild(gridLines);
        }

        protected override int VisualChildrenCount => 3;

        protected override Visual GetVisualChild(int index)
        {
            Visual returnValue = gridLines;
            switch(index)
            {
                case 0: 
                    returnValue = drawingSurface;
                    break;
                case 1:
                    returnValue = previewSurface;
                    break;
                case 2:
                    returnValue = gridLines;
                    break;
            }

            return returnValue;
        }

        private void PutColorAtPosition(Point pixel, Color color, BitmapSurface surface)
        {                        
            surface.SetColor(
                (int)pixel.X,
                (int)pixel.Y,
                color);

            surface.InvalidateVisual();            
        }

        private Point GetMousePixel(BitmapSurface surface)
        {
            Point returnValue = new Point(-1, -1);
            
            var mousePosition = Mouse.GetPosition(surface);
            var foo = surface.DesiredSize;
            var surfaceWidth = PixelWidth * Magnification;
            var surfaceHeight = PixelHeight * Magnification;

            if (PointHelper.PointOnSurface(mousePosition, surfaceWidth, surfaceHeight))
                returnValue = new Point((int)(mousePosition.X / Magnification), (int)(mousePosition.Y / Magnification));

            return returnValue;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
                DoClick();
            else if (e.LeftButton != MouseButtonState.Pressed)
                DoPreview();
        }

        private void DoClick()
        {
            var mousePixel = GetMousePixel(drawingSurface);
            var drawingBitmap = drawingSurface.GetBitmap();
            Tool.OnClick(mousePixel, drawingBitmap);
        }

        private void DoPreview()
        {
            previewSurface.DeleteAll();
            var mousePixel = GetMousePixel(previewSurface);
            var drawingBitmap = drawingSurface.GetBitmap();
            Tool.OnPreview(mousePixel, drawingBitmap);           
        }

        private void OnDrawingColored(object sender, ToolPaintEventArgs e)
        {
            if (e != null && e.Pixels != null)
            {
                ColorPixelsOnSurface(e.Pixels, drawingSurface);
            }
        }

        private void OnPreviewColored(object sender, ToolPaintEventArgs e)
        {
            if (e != null && e.Pixels != null)
            {
                ColorPixelsOnSurface(e.Pixels, previewSurface);
            }
        }

        private void ColorPixelsOnSurface(List<ColoredPixel> pixels, BitmapSurface surface)
        {
            if (pixels != null)
            {
                foreach (var pixel in pixels)
                {
                    if (pixel.Point != null && PointHelper.PointOnSurface(pixel.Point, PixelWidth, PixelHeight))
                    {                        
                        surface.SetColor(
                            (int)pixel.Point.X,
                            (int)pixel.Point.Y,
                            pixel.Color);
                    }
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            CaptureMouse();
            DoClick();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ReleaseMouseCapture();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            previewSurface.DeleteAll();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size(PixelWidth * Magnification, PixelHeight * Magnification);

            drawingSurface.Measure(size);
            previewSurface.Measure(size);

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var rect = new Rect(finalSize);
            drawingSurface.Arrange(rect);
            previewSurface.Arrange(rect);
            return finalSize;
        }

        private Visual CreateGridLines()
        {
            var dv = new DrawingVisual();
            var dc = dv.RenderOpen();

            var lineThickness = 1d; // snap gridlines to device pixels

            var pen = new Pen(new SolidColorBrush(Color.FromArgb(128, 63, 63, 63)), lineThickness);

            pen.Freeze();

            for (var x = 1; x < PixelWidth; x++)
                dc.DrawLine(pen, new Point(x * Magnification - (lineThickness / 2), 0), new Point(x * Magnification - (lineThickness / 2), PixelHeight * Magnification));

            for (var y = 1; y < PixelHeight; y++)
                dc.DrawLine(pen, new Point(0, y * Magnification - (lineThickness / 2)), new Point(PixelWidth * Magnification, y * Magnification - (lineThickness / 2)));

            dc.Close();

            return dv;
        }

        private sealed class BitmapSurface : FrameworkElement
        {
            private readonly PixelEditorSurface owner;
            private readonly WriteableBitmap bitmap;

            public BitmapSurface(PixelEditorSurface owner)
            {
                this.owner = owner;
                bitmap = BitmapFactory.New(owner.PixelWidth, owner.PixelHeight);
                bitmap.Clear(Colors.Transparent);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            }

            protected override void OnRender(DrawingContext dc)
            {
                base.OnRender(dc);
                
                var width = bitmap.PixelWidth * owner.Magnification;
                var height = bitmap.PixelHeight * owner.Magnification;

                dc.DrawImage(bitmap, new Rect(0, 0, width, height));
            }

            internal void SetColor(int x, int y, Color color)
            {                
                bitmap.SetPixel(x, y, color);
            }

            internal void FillColor(Color color, byte alpha = 255)
            {                
                bitmap.Clear(color);
            }

            internal void DeleteAll()
            {
                bitmap.Clear(Colors.Transparent);
            }

            internal WriteableBitmap GetBitmap()
            {
                return bitmap.Clone();
            }
        }
    }
}
