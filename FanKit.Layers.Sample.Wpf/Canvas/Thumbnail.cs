using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FanKit.Layers.Sample
{
    public class Thumbnail : IDisposable
    {
        const int W = 50;
        const int H = 50;

        static readonly CanvasSize PixelSize = new CanvasSize(W, H);
        static readonly System.Windows.Int32Rect Int32Rect = PixelSize.ToRect();

        static readonly System.Drawing.Rectangle Clip = new System.Drawing.Rectangle(0, 0, W, H);
        static readonly System.Drawing.PointF[] DestPoints = new System.Drawing.PointF[]
        {
            new System.Drawing.PointF(0, 0),
            new System.Drawing.PointF(W, 0),
            //new System.Drawing.PointF(W,H),
            new System.Drawing.PointF(0, H),
        };

        public readonly WriteableBitmap WriteableBitmap;
        public readonly System.Drawing.Bitmap RenderTarget;
        public readonly ImageBrush ImageBrush;

        public Thumbnail(CanvasControl canvasControl)
        {
            this.WriteableBitmap = PixelSize.ToWriteableBitmap();
            this.RenderTarget = PixelSize.ToBitmap(this.WriteableBitmap);

            this.ImageBrush = new ImageBrush(this.WriteableBitmap);
        }

        private Thumbnail(Thumbnail thumbnail)
        {
            this.WriteableBitmap = thumbnail.WriteableBitmap?.Clone();

            this.RenderTarget = thumbnail.RenderTarget?.Clone(Clip, thumbnail.RenderTarget.PixelFormat);

            this.ImageBrush = new ImageBrush(this.WriteableBitmap);
        }

        public Thumbnail Clone() => new Thumbnail(this);

        public void Invalidate(System.Drawing.Bitmap bitmap, float w, float h, System.Drawing.RectangleF rect)
        {
            this.WriteableBitmap.Lock();

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(this.RenderTarget))
            {
                graphics.Clear(System.Drawing.Color.Black);

                graphics.DrawImage(bitmap, DestPoints);
            }

            this.WriteableBitmap.AddDirtyRect(Int32Rect);
            this.WriteableBitmap.Unlock();
        }

        public void Dispose() => this.RenderTarget?.Dispose();
    }
}