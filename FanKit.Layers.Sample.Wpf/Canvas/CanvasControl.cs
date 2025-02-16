using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FanKit.Layers.Sample
{
    public sealed class CanvasControl : Image, IDisposable
    {
        public event EventHandler<System.Drawing.Graphics> Draw;

        readonly System.Drawing.Drawing2D.Matrix Matrix = new System.Drawing.Drawing2D.Matrix();

        CanvasDpi Dpis = new CanvasDpi(1, 1, CanvasDpi.Dpi);
        CanvasDpi DpisScale = new CanvasDpi(1, 1);
        CanvasSize PixelSize = new CanvasSize(512, 512);
        System.Drawing.Color Clear = System.Drawing.Color.White;

        WriteableBitmap WriteableBitmap;
        System.Drawing.Bitmap Bitmap;

        public CanvasUnits Units { get; set; }

        public float Dpi => this.Dpis.Value;
        public float DpiScale => this.DpisScale.Value;

        public int PixelWidth => this.PixelSize.Width;
        public int PixelHeight => this.PixelSize.Height;

        public Color ClearColor
        {
            get => Color.FromArgb(this.Clear.A, this.Clear.R, this.Clear.G, this.Clear.B);
            set => this.Clear = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
        }

        public CanvasControl()
        {
            this.WriteableBitmap = this.PixelSize.ToWriteableBitmap();
            base.Source = this.WriteableBitmap;

            //this.Bitmap?.Dispose();
            this.Bitmap = this.PixelSize.ToBitmap(this.WriteableBitmap);
            this.Drawing();

            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                if (PresentationSource.FromVisual(this) is PresentationSource source && source.CompositionTarget != null)
                {
                    double m11 = source.CompositionTarget.TransformToDevice.M11;
                    double m22 = source.CompositionTarget.TransformToDevice.M22;

                    this.Dpis = new CanvasDpi((float)m11, (float)m22, CanvasDpi.Dpi);
                    this.DpisScale = new CanvasDpi((float)m11, (float)m22);
                    this.PixelSize = new CanvasSize(e.NewSize.Width * m11, e.NewSize.Height * m22);
                    this.Matrix.Scale(this.DpisScale.ValueX, this.DpisScale.ValueY);

                    this.WriteableBitmap = this.PixelSize.ToWriteableBitmap(this.Dpis);
                    base.Source = this.WriteableBitmap;
                }
                else
                {
                    this.Dpis = new CanvasDpi(1, 1, CanvasDpi.Dpi);
                    this.DpisScale = new CanvasDpi(1, 1);
                    this.PixelSize = new CanvasSize(e.NewSize.Width, e.NewSize.Height);
                    this.Matrix.Reset();

                    this.WriteableBitmap = this.PixelSize.ToWriteableBitmap();
                    base.Source = this.WriteableBitmap;
                }

                this.Bitmap?.Dispose();
                this.Bitmap = this.PixelSize.ToBitmap(this.WriteableBitmap);
                this.Drawing();
            };
        }

        public void Invalidate()
        {
            if (this.WriteableBitmap is null) return;
            if (this.Bitmap is null) return;

            this.Drawing();
        }
        private void Drawing()
        {
            if (this.Draw is null) return;

            this.WriteableBitmap.Lock();

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(this.Bitmap))
            {
                graphics.Clear(this.Clear);
                switch (this.Units)
                {
                    case CanvasUnits.Dips:
                        graphics.Transform = this.Matrix;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        break;
                    default:
                        break;
                }
                this.Draw.Invoke(this, graphics);
            }

            this.WriteableBitmap.AddDirtyRect(this.PixelSize.ToRect());
            this.WriteableBitmap.Unlock();
        }

        public void Dispose()
        {
            this.WriteableBitmap = null;
            this.Bitmap.Dispose();
            this.Bitmap = null;
        }
    }
}