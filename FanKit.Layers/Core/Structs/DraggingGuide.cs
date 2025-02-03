using System.Runtime.InteropServices;

namespace FanKit.Layers.Core
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct DraggingGuide
    {
        public static DraggingGuide Empty { get; } = new DraggingGuide(-1, 0, 0, 0, 0, 0);

        public readonly int Index;

        public readonly double Top;
        public readonly double CenterY;
        public readonly double Bottom;
        public readonly double Width;

        public readonly DraggingRect InsertAbove;
        public readonly DraggingRect InsertBelow;
        public readonly DraggingRect InsertAtBottom;

        public bool IsEmpty => this.Index < 0;

        public DraggingGuide(int index, double top, double bottom, double marginX, double width, double height)
        {
            this.Index = index;

            this.Top = top;
            this.CenterY = (top + bottom) / 2;
            this.Bottom = bottom;
            this.Width = width;

            double m = width - marginX;
            double h = height / 2;
            this.InsertAbove = new DraggingRect(marginX, top - h, m, height);

            double b = bottom - h;
            this.InsertBelow = new DraggingRect(marginX, b, m, height);
            this.InsertAtBottom = new DraggingRect(0, b, width, height);
        }
    }
}