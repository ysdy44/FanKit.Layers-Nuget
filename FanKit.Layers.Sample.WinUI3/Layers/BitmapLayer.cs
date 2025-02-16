using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Xaml.Media;
using System.Xml.Linq;
using Windows.Foundation;

namespace FanKit.Layers.Sample
{
    public class BitmapLayer : Layer2, ILayer
    {
        public LayerType Type => LayerType.Bitmap;
        public string Title { get; } = UIType.LayerBitmap.GetString();

        public float BitmapWidth;
        public float BitmapHeight;
        public CanvasBitmap Bitmap;

        public Brush Thumbnail => this.BitmapThumbnail?.ImageBrush;
        public Thumbnail BitmapThumbnail { get; set; }

        public void DrawRecursion(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                Source = this.Bitmap,
                TransformMatrix = System.Numerics.Matrix3x2.CreateScale(
                    (float)(this.Rect.Width / this.BitmapWidth),
                    (float)(this.Rect.Height / this.BitmapHeight)
                    ) * System.Numerics.Matrix3x2.CreateTranslation(
                        (float)this.Rect.X,
                        (float)this.Rect.Y
                        )
            });
        }

        public void RenderThumbnail()
        {
            this.BitmapThumbnail?.Invalidate(this.Bitmap, this.BitmapWidth, this.BitmapHeight, this.Rect);
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
            this.BitmapThumbnail?.Dispose();
        }

        public override string ToString() => $"[{this.Type}: Depth={this.Depth}]";
    }
}