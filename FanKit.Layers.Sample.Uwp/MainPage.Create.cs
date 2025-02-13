using FanKit.Layers.Demo;
using FanKit.Layers.DragDrop;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        public Rect RandomRect()
        {
            int r = this.Random.Next(16 + 32, 32 + 64);
            int cx = this.Random.Next(0, 512);
            int cy = this.Random.Next(0, 512);
            return new Rect
            {
                X = cx - r,
                Y = cy - r,
                Width = r + r,
                Height = r + r,
            };
        }

        public Color RandomColor()
        {
            int index = this.Random.Next(8, 123);

            return Color.FromArgb(255,
                this.RandomColors[index, 0],
                this.RandomColors[index, 1],
                this.RandomColors[index, 2]);
        }

        //------------------------ Drop ----------------------------//

        private void DropItems()
        {
            switch (this.DropIndexer.Placement)
            {
                case DropPlacement.None:
                    break;
                default:
                    Dropper dropper = new Dropper(this.List, this.DropIndexer);

                    this.Invalidate(this.TryDrop(dropper, new BitmapLayer
                    {
                        Depth = dropper.Depth,

                        SelectMode = SelectMode.Selected,

                        Rect = this.RandomRect(),

                        BitmapThumbnail = new Thumbnail(this.CanvasControl),

                        BitmapWidth = this.BitmapWidth,
                        BitmapHeight = this.BitmapHeight,
                        Bitmap = this.Bitmap,
                    }));
                    break;
            }
        }

        //------------------------ Create ----------------------------//

        public ILayer CreateAndLoad(CustomStruct item)
        {
            if (item.IsGroup)
            {
                return new GroupLayer
                {
                    Depth = item.Depth
                };
            }
            else
            {
                return new FillLayer
                {
                    Depth = item.Depth,

                    Fill = this.RandomColor(),
                    Rect = this.RandomRect(),
                };
            }
        }

        public ILayer CreateAndLoadWithDepth(CustomClass item, int depth)
        {
            if (item.IsGroup)
            {
                return new GroupLayer
                {
                    Depth = depth,
                    IsExpanded = true,
                };
            }
            else
            {
                return new FillLayer
                {
                    Depth = depth,

                    Fill = this.RandomColor(),
                    Rect = this.RandomRect(),
                };
            }
        }

        public ILayer Create(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "Bitmap":
                    return new BitmapLayer
                    {
                        BitmapThumbnail = new Thumbnail(this.CanvasControl),

                        BitmapWidth = this.BitmapWidth,
                        BitmapHeight = this.BitmapHeight,
                        Bitmap = this.Bitmap,
                    };
                case "Fill":
                    return new FillLayer();
                default:
                    return new GroupLayer();
            }
        }

        public ILayer CreateWithDepth(XElement element, int depth)
        {
            switch (element.Name.LocalName)
            {
                case "Bitmap":
                    return new BitmapLayer
                    {
                        Depth = depth,

                        BitmapThumbnail = new Thumbnail(this.CanvasControl),

                        BitmapWidth = this.BitmapWidth,
                        BitmapHeight = this.BitmapHeight,
                        Bitmap = this.Bitmap,
                    };
                case "Fill":
                    return new FillLayer
                    {
                        Depth = depth,
                    };
                default:
                    return new GroupLayer
                    {
                        Depth = depth
                    };
            }
        }
    }
}