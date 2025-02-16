using System.Windows.Media;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public class BitmapLayer : Layer2, ILayer
    {
        public LayerType Type => LayerType.Bitmap;
        public string Title { get; } = UIType.LayerBitmap.GetString();

        public float BitmapWidth;
        public float BitmapHeight;
        public System.Drawing.Bitmap Bitmap;

        public Brush Thumbnail => this.BitmapThumbnail?.ImageBrush;
        public Thumbnail BitmapThumbnail { get; set; }

        public void DrawRecursion(System.Drawing.Graphics drawingSession)
        {
            System.Drawing.PointF t0 = new System.Drawing.PointF(this.Rect.Left, this.Rect.Top);
            System.Drawing.PointF t1 = new System.Drawing.PointF(this.Rect.Right, this.Rect.Top);
            //System.Drawing.PointF t2 = new System.Drawing.PointF(this.Rect.Right, this.Rect.Bottom);
            System.Drawing.PointF t3 = new System.Drawing.PointF(this.Rect.Left, this.Rect.Bottom);
            drawingSession.DrawImage(this.Bitmap, new System.Drawing.PointF[]
            {
                t0,
                t1,
                //t2,
                t3,
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