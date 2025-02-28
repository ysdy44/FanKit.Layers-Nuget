using Microsoft.Maui.Graphics;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public class BitmapLayer : Layer2, ILayer
    {
        public LayerType Type => LayerType.Bitmap;
        public string Title { get; } = UIType.LayerBitmap.GetString();

        public float BitmapWidth;
        public float BitmapHeight;
        public IImage Bitmap;

        public IDrawable Thumbnail => this.Bitmap;
        public Thumbnail BitmapThumbnail { get; set; }

        public void DrawRecursion(ICanvas canvas)
        {
            canvas.DrawImage(this.Bitmap, this.Rect.X, this.Rect.Y, this.Rect.Width, this.Rect.Height);
        }

        public void RenderThumbnail()
        {
            this.BitmapThumbnail?.Invalidate(this.Bitmap, this.BitmapWidth, this.BitmapHeight, this.Rect);
            this.OnPropertyChanged(nameof(Thumbnail));
        }

        public ILayer Clone() => this.CloneSelf(null);
        public ILayer Clone(int depth) => this.CloneSelf(depth);
        private ILayer CloneSelf(int? depth) => new BitmapLayer
        {
            depth = depth is null ? this.depth : depth.Value,
            isExpanded = this.isExpanded,
            isLocked = this.isLocked,

            isVisible = this.isVisible,
            selectMode = this.selectMode,

            Rect = this.Rect,

            BitmapThumbnail = this.BitmapThumbnail?.Clone(),

            BitmapWidth = this.BitmapWidth,
            BitmapHeight = this.BitmapHeight,
            Bitmap = this.Bitmap,
        };

        public void LoadFromXml(XElement content)
        {
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

                    case "Left": left = (double)attribute; break;
                    case "Top": top = (double)attribute; break;
                    case "Right": right = (double)attribute; break;
                    case "Bottom": bottom = (double)attribute; break;
                    default: break;
                }
            }

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

                new XAttribute("Left", this.Rect.Left),
                new XAttribute("Top", this.Rect.Top),
                new XAttribute("Right", this.Rect.Right),
                new XAttribute("Bottom", this.Rect.Bottom)
             );
        }

        public void Dispose()
        {
            this.Children.Clear();
            //this.BitmapThumbnail?.Dispose();
        }

        public override string ToString() => $"[{this.Type}: Depth={this.Depth}]";
    }
}