using Microsoft.Maui.Graphics;

namespace FanKit.Layers.Sample
{
    public abstract class Layer2 : Layer1
    {
        public bool IsGroup => false;

        public RectF Rect { get; set; } = RectF.Zero;
        public RectF StartingRect { get; private set; }

        public bool IsGroupVisibility => this.IsGroup;
        public bool NotGroupVisibility => this.IsGroup is false;

        public bool FillContainsPointRecursion(PointF point)
        {
            return point.X > this.Rect.Left == point.X < this.Rect.Right && point.Y > this.Rect.Top == point.Y < this.Rect.Bottom;
        }

        public void Cache() => this.StartingRect = this.Rect;

        public void Move(float offsetX, float offsetY)
        {
            if (this.StartingRect.IsEmpty) return;

            this.Rect = new RectF
            {
                X = this.StartingRect.X + offsetX,
                Y = this.StartingRect.Y + offsetY,
                Width = this.StartingRect.Width,
                Height = this.StartingRect.Height,
            };
        }
    }
}