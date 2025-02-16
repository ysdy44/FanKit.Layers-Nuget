using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FanKit.Layers.Sample
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct CanvasSize
    {
        public readonly int Width;
        public readonly int Height;

        public CanvasSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public CanvasSize(double width, double height)
        {
            this.Width = (int)width;
            this.Height = (int)height;
        }

        public Int32Rect ToRect() => new Int32Rect(0, 0, this.Width, this.Height);

        public WriteableBitmap ToWriteableBitmap() => new WriteableBitmap(this.Width, this.Height, CanvasDpi.Dpi, CanvasDpi.Dpi, PixelFormats.Pbgra32, null);

        public WriteableBitmap ToWriteableBitmap(CanvasDpi dpi) => new WriteableBitmap(this.Width, this.Height, dpi.ValueX, dpi.ValueY, PixelFormats.Pbgra32, null);

        public System.Drawing.Bitmap ToBitmap(WriteableBitmap writeableBitmap) => new System.Drawing.Bitmap(this.Width, this.Height, writeableBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, writeableBitmap.BackBuffer);
    }
}