using System.Windows.Media;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public class FillLayer : Layer2, ILayer
    {
        private readonly SolidColorBrush SolidColorBrush = new SolidColorBrush();

        public LayerType Type => LayerType.Fill;
        public string Title { get; } = UIType.LayerFill.GetString();

        public Brush Thumbnail => this.SolidColorBrush;

        public System.Drawing.SolidBrush Fill { get; set; }

        private Color FillColor
        {
            get => this.SolidColorBrush.Color;
            set => this.SolidColorBrush.Color = value;
        }

        public void DrawRecursion(System.Drawing.Graphics drawingSession)
        {
            drawingSession.FillRectangle(this.Fill, new System.Drawing.Rectangle
            {
                X = System.Math.Min(this.Rect.Left, this.Rect.Right),
                Y = System.Math.Min(this.Rect.Top, this.Rect.Bottom),
                Width = System.Math.Abs(this.Rect.Left - this.Rect.Right),
                Height = System.Math.Abs(this.Rect.Top - this.Rect.Bottom),
            });
        }

        public void RenderThumbnail()
        {
            this.FillColor = Color.FromArgb(this.Fill.Color.A, this.Fill.Color.R, this.Fill.Color.G, this.Fill.Color.B);
        }

        public ILayer Clone() => this.CloneSelf(null);
        public ILayer Clone(int depth) => this.CloneSelf(depth);
        private ILayer CloneSelf(int? depth) => new FillLayer
        {
            depth = depth is null ? this.depth : depth.Value,
            isExpanded = this.isExpanded,
            isLocked = this.isLocked,

            isVisible = this.isVisible,
            selectMode = this.selectMode,

            Fill = this.Fill,
            Rect = this.Rect,

            FillColor = this.FillColor,
        };

        public void LoadFromXml(XElement content)
        {
            byte r = 255;
            byte g = 255;
            byte b = 255;

            double left = 0;
            double top = 0;
            double right = 1024;
            double bottom = 1024;

            foreach (XAttribute attribute in content.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "Depth": this.Depth = (int)attribute; break;

                    case "IsExpanded": this.IsExpanded = (bool)attribute; break;
                    case "IsLocked": this.IsLocked = (bool)attribute; break;
                    case "IsVisible": this.IsVisible = (bool)attribute; break;
                    case "IsSelected": this.SelectMode = ((bool)attribute).ToSelectMode(); break;

                    case "R": r = (byte)(int)attribute; break;
                    case "G": g = (byte)(int)attribute; break;
                    case "B": b = (byte)(int)attribute; break;

                    case "Left": left = (double)attribute; break;
                    case "Top": top = (double)attribute; break;
                    case "Right": right = (double)attribute; break;
                    case "Bottom": bottom = (double)attribute; break;
                    default: break;
                }
            }

            this.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, r, g, b));
            this.Rect = new System.Drawing.Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top));
        }

        public XElement SaveToXml(XmlStructure structure, XObject children)
        {
            return new XElement
            (
                $"{this.Type}",
                this.SaveXmlStructure(structure, children),

                new XAttribute("IsExpanded", this.IsExpanded),
                new XAttribute("IsLocked", this.IsLocked),
                new XAttribute("IsVisible", this.IsVisible),
                new XAttribute("IsSelected", this.SelectMode.IsSelected()),

                new XAttribute("R", this.Fill.Color.R),
                new XAttribute("G", this.Fill.Color.G),
                new XAttribute("B", this.Fill.Color.B),

                new XAttribute("Left", this.Rect.Left),
                new XAttribute("Top", this.Rect.Top),
                new XAttribute("Right", this.Rect.Right),
                new XAttribute("Bottom", this.Rect.Bottom)
             );
        }

        public void Dispose()
        {
            this.Children.Clear();
        }

        public override string ToString() => $"[{this.Type}: Depth={this.Depth}]";
    }
}