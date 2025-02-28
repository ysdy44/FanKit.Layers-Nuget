using Microsoft.Maui.Graphics;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public class GroupLayer : Layer1, ILayer
    {
        public bool IsGroup => true;
        public LayerType Type => LayerType.Group;
        public string Title { get; } = UIType.LayerGroup.GetString();

        public IDrawable Thumbnail => null;

        public RectF Rect { get => RectF.Zero; set { } }
        public RectF StartingRect => RectF.Zero;

        public bool IsGroupVisibility => this.IsGroup;
        public bool NotGroupVisibility => this.IsGroup is false;

        public bool FillContainsPointRecursion(PointF point)
        {
            foreach (ILayer item in this.Children)
            {
                if (item.IsLocked)
                    continue;

                if (item.IsVisible)
                {
                    if (item.FillContainsPointRecursion(point))
                        return true;
                }
            }
            return false;
        }

        public void Cache()
        {
        }

        public void Move(float offsetX, float offsetY)
        {
        }

        public void DrawRecursion(ICanvas canvas)
        {
            foreach (ILayer item in this.Children)
            {
                if (item.IsVisible)
                {
                    item.DrawRecursion(canvas);
                }
            }
        }

        public void RenderThumbnail()
        {
        }

        public ILayer Clone() => this.CloneSelf(null);
        public ILayer Clone(int depth) => this.CloneSelf(depth);
        private ILayer CloneSelf(int? depth) => new GroupLayer
        {
            depth = depth is null ? this.depth : depth.Value,
            isExpanded = this.isExpanded,
            isLocked = this.isLocked,

            isVisible = this.isVisible,
            selectMode = this.selectMode,
        };

        public void LoadFromXml(XElement content)
        {
            foreach (XAttribute attribute in content.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "Depth": this.Depth = (int)attribute; break;

                    case "IsExpanded": this.IsExpanded = (bool)attribute; break;
                    case "IsLocked": this.IsLocked = (bool)attribute; break;
                    case "IsVisible": this.IsVisible = (bool)attribute; break;
                    case "IsSelected": this.SelectMode = ((bool)attribute).ToSelectMode(); break;
                    default: break;
                }
            }
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
                new XAttribute("IsSelected", this.SelectMode.IsSelected())
             );
        }

        public void Dispose()
        {
            this.Children.Clear();
        }

        public override string ToString() => $"[{this.Type}: Depth={this.Depth}]";
    }
}