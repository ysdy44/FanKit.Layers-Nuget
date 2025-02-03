using System.Runtime.InteropServices;

namespace FanKit.Layers.Core
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct DraggingRect
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Width;
        public readonly double Height;

        public static DraggingRect Empty { get; } = new DraggingRect(0, 0, 0, 0);
        public bool IsEmpty => this.X == 0 && this.Y == 0 && this.Width == 0 && this.Height == 0;

        public DraggingRect(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
    }
}