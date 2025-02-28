using Microsoft.Maui.Graphics;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public class FillLayer : Layer2, ILayer, IDrawable
    {
        public LayerType Type => LayerType.Fill;
        public string Title { get; } = UIType.LayerFill.GetString();

        public IDrawable Thumbnail => this;

        public Color Fill { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = this.Fill;
            canvas.FillRectangle(dirtyRect);
        }

        public void DrawRecursion(ICanvas canvas)
        {
            canvas.FillColor = this.Fill;
            canvas.FillRectangle(
                x: System.MathF.Min(this.Rect.Left, this.Rect.Right),
                y: System.MathF.Min(this.Rect.Top, this.Rect.Bottom),
                width: System.MathF.Abs(this.Rect.Left - this.Rect.Right),
                height: System.MathF.Abs(this.Rect.Top - this.Rect.Bottom)
            );
        }

        public void RenderThumbnail()
        {
            this.OnPropertyChanged(nameof(Thumbnail));
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

            this.Fill = Color.FromRgb(r, g, b);
            this.Rect = new Rect(left, top, right - left, bottom - top);
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

                new XAttribute("R", (int)(this.Fill.Red * 256f)), // Maui.Color.Red is 0.0 ~ 1.0
                new XAttribute("G", (int)(this.Fill.Green * 256f)), // Maui.Color.Green is 0.0 ~ 1.0
                new XAttribute("B", (int)(this.Fill.Blue * 256f)), // Maui.Color.Blue is 0.0 ~ 1.0

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