using FanKit.Layers.Core;
using System.Runtime.InteropServices;

namespace FanKit.Layers.DragDrop
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='DragOverUIRect']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public struct DragOverUIRect
    {
        internal static DragOverUIRect Empty { get; } = new DragOverUIRect
        {
            IsEmpty = true,
            X = 0,
            Y = 0,
            Width = 0,
            Height = 0,
        };

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragOverUIRect.IsEmpty']/*" />
        public bool IsEmpty;

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragOverUIRect.X']/*" />
        public double X;

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragOverUIRect.Y']/*" />
        public double Y;

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragOverUIRect.Width']/*" />
        public double Width;

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragOverUIRect.Height']/*" />
        public double Height;

        internal DragOverUIRect(DraggingRect rect)
        {
            this.IsEmpty = false;
            this.X = rect.X;
            this.Y = rect.Y;
            this.Width = rect.Width;
            this.Height = rect.Height;
        }

        internal DragOverUIRect(DragOverUIPoint point, DraggingRect rect)
        {
            this.IsEmpty = false;
            this.X = rect.X - point.HorizontalOffset;
            this.Y = rect.Y - point.VerticalOffset + point.HeaderHeight;
            this.Width = rect.Width;
            this.Height = rect.Height;
        }

        /// <inheritdoc/>
        public override string ToString() => $"[DragOverUIRect: X = {this.X}, Y = {this.Y}, Width = {this.Width}, Height = {this.Height}]";
    }
}