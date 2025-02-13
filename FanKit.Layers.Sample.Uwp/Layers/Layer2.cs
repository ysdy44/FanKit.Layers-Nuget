using Windows.Foundation;
using Windows.UI.Xaml;

namespace FanKit.Layers.Sample
{
    public abstract class Layer2 : Layer1
    {
        public bool IsGroup => false;

        public Rect Rect { get; set; } = Rect.Empty;
        public Rect StartingRect { get; private set; }

        public Visibility IsGroupVisibility => this.IsGroup ? Visibility.Visible : Visibility.Collapsed;
        public Visibility NotGroupVisibility => this.IsGroup ? Visibility.Collapsed : Visibility.Visible;

        public bool FillContainsPointRecursion(Point point)
        {
            return point.X > this.Rect.Left == point.X < this.Rect.Right && point.Y > this.Rect.Top == point.Y < this.Rect.Bottom;
        }

        public void Cache() => this.StartingRect = this.Rect;

        public void Move(double offsetX, double offsetY)
        {
            if (this.StartingRect.IsEmpty) return;

            this.Rect = new Rect
            {
                X = this.StartingRect.X + offsetX,
                Y = this.StartingRect.Y + offsetY,
                Width = this.StartingRect.Width,
                Height = this.StartingRect.Height,
            };
        }
    }
}