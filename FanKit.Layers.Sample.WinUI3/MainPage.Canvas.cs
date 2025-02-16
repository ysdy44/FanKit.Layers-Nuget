using FanKit.Layers.Ranges;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        private async Task CreateResourcesAsync(CanvasControl sender)
        {
            string installedPath = Windows.ApplicationModel.Package.Current.InstalledPath;
            string fullPath = System.IO.Path.Combine(installedPath, "Images", "avatar.jpg");
            this.Bitmap = await CanvasBitmap.LoadAsync(sender, fullPath);
            this.BitmapWidth = (float)this.Bitmap.Size.Width;
            this.BitmapHeight = (float)this.Bitmap.Size.Height;
            this.Click(OptionType.NewByList);
        }

        //------------------------ Draw ----------------------------//

        public void Draw(CanvasDrawingSession drawingSession)
        {
            for (int i = this.Collection.Count - 1; i >= 0; i--)
            {
                ILayer item = this.Collection[i];
                if (item.IsVisible)
                {
                    switch (item.Depth)
                    {
                        case 0:
                            item.DrawRecursion(drawingSession);
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (ILayer item in this.Collection)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        break;
                    default:
                        if (item.Rect.IsEmpty)
                            break;

                        drawingSession.DrawRectangle(item.Rect, Colors.DodgerBlue, 2f);
                        break;
                }
            }

            this.Transformer.Draw(drawingSession);
        }

        //------------------------ Invalidate ----------------------------//

        public void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.SelectionCleared))
            {
                this.Transformer = Rect.Empty;
                this.Selection = IndexSelection.Empty;
            }

            if (modes.HasFlag(InvalidateModes.SelectionChanged))
            {
                this.Transformer = this.GetRect();
                this.Selection = new IndexSelection(this.List);
            }

            if (modes.HasFlag(InvalidateModes.CanvasControlInvalidate))
                this.CanvasControl.Invalidate(); // Invalidate

            if (modes.HasFlag(InvalidateModes.AllThumbnailInvalidate))
                foreach (ILayerBase item in this.List)
                {
                    item.RenderThumbnail();
                }

            if (modes.HasFlag(InvalidateModes.LayersCleared))
                this.UILayers.Clear();

            if (modes.HasFlag(InvalidateModes.LayersChanged))
                this.Collection.UISyncTo(this.UILayers);

            if (modes.HasFlag(InvalidateModes.LayerCanExecuteChanged))
                this.CanExecuteChanged?.Invoke(this, null);

            if (modes.HasFlag(InvalidateModes.HistoryCleared))
                this.UIHistory.Clear();

            if (modes.HasFlag(InvalidateModes.HistoryChanged))
                this.History.UISyncTo(this.UIHistory);

            if (modes.HasFlag(InvalidateModes.HistoryCanExecuteChanged))
            {
                this.UndoCommand.RaiseCanExecuteChanged();
                this.RedoCommand.RaiseCanExecuteChanged();
            }

            if (modes.HasFlag(InvalidateModes.HistorySelectionChanged))
                this.History.UISyncTime();

            if (modes.HasFlag(InvalidateModes.Output))
            {
                this.LayerTextBlock.Text = this.List.ToString();

                XmlTreeNode[] nodes = this.List.GetNodes();
                if (nodes is null)
                {
                    this.NodeTextBlock.Text = string.Empty;
                    this.Xml0TextBlock.Text = "<Root/>";
                    this.Xml1TextBlock.Text = "<Root/>";
                    this.Xml2TextBlock.Text = "<Root/>";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (XmlTreeNode item in nodes)
                    {
                        item.AppendTo(sb);
                    }
                    this.NodeTextBlock.Text = sb.ToString();
                    this.Xml0TextBlock.Text = this.List.ToXmlListString();
                    this.Xml1TextBlock.Text = this.Collection.ToXmlTreeString(nodes);
                    this.Xml2TextBlock.Text = this.List.ToXmlTreeNodes01String(nodes);
                }
            }
        }

        //------------------------ Rect ----------------------------//

        public Rect GetRect()
        {
            Rect rects = Rect.Empty;
            foreach (ILayer item in this.Collection)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        break;
                    default:
                        if (item.Rect.IsEmpty)
                            break;
                        else if (rects.IsEmpty)
                            rects = item.Rect;
                        else
                            rects.Union(item.Rect);
                        break;
                }
            }

            return rects;
        }

        public IEnumerable<ILayer> FillContainsPointRoot(Point point)
        {
            foreach (ILayer item in this.Collection)
            {
                if (item.IsLocked)
                    continue;

                if (item.IsVisible)
                {
                    switch (item.Depth)
                    {
                        case 0:
                            if (item.FillContainsPointRecursion(point))
                                yield return item;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void Cache()
        {
            foreach (ILayer item in this.Collection)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        break;
                    default:
                        item.Cache();
                        break;
                }
            }
        }

        public void Move(double offsetX, double offsetY)
        {
            foreach (ILayer item in this.Collection)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        break;
                    default:
                        item.Move(offsetX, offsetY);
                        item.RenderThumbnail();
                        break;
                }
            }
        }
    }
}