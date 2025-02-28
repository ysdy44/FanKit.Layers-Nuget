using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using System.Linq;
using System.Text;

namespace FanKit.Layers.Sample
{
    public sealed partial class MainPage : ContentPage
    {
        //private static bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        private static bool IsCtrl => false;// IsKeyDown(VirtualKey.Control);
        private static bool IsShift => false;//  IsKeyDown(VirtualKey.Shift);

        bool IsMode;

        PointF Point;
        PointF StartingPoint;

        Transformer Transformer = Rect.Zero;
        Rect StartingTransformer;

        float BitmapWidth;
        float BitmapHeight;
        Microsoft.Maui.Graphics.IImage Bitmap;

        PointF TransformPoint;
        bool CanReorder;

        /*
        readonly Popup Popup = new Popup
        {
            IsHitTestVisible = false,
            Width = 200,
            Height = GuideHeight,
            Child = new Grid
            {
                IsHitTestVisible = false,
                Width = 200,
                Children =
                {
                    new Rectangle
                    {
                        Width = GuideHeight,
                        Height = GuideHeight,
                        StrokeThickness = 2,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Stroke =  new SolidColorBrush(Colors.DodgerBlue),
                        Fill = new SolidColorBrush(Colors.DodgerBlue)
                        {
                            Opacity = 0.4
                        },
                    },
                    new Rectangle
                    {
                        Height = 2,
                        Margin = new Thickness(GuideHeight, 0, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Center,
                        Fill =  new SolidColorBrush(Colors.DodgerBlue),
                    }
                }
            }
        };
         */

        public MainPage()
        {
            base.BindingContext = this;
            base.FlowDirection = CultureInfoCollection.FlowDirection;
            this.UndoCommand = new UndoCommand(this);
            this.RedoCommand = new RedoCommand(this);
            this.InitializeComponent();

            this.LanguageCommand.XamlRoot = this;

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                foreach (OptionCatalogMenu catalog in this.CatalogMenus)
                {
                    MenuBarItem bar = new MenuBarItem { Text = catalog.Catalog.GetString() };

                    foreach (OptionTypeMenu item in catalog.Items)
                    {
                        bar.Add(this.ToMenu(item));
                    }

                    base.MenuBarItems.Add(bar);
                }

                base.MenuBarItems.Add(this.LanguageCommand.ToMenuBarItem());
            }
            else
            {
                foreach (OptionCatalogMenu catalog in this.CatalogMenus)
                {
                    base.ToolbarItems.Add(new CatalogToolbarItem(catalog)
                    {
                        Order = ToolbarItemOrder.Secondary,
                        XamlRoot = this,
                    });
                }

                base.ToolbarItems.Add(new LanguageToolbarItem(this.LanguageCommand)
                {
                    Order = ToolbarItemOrder.Secondary,
                    XamlRoot = this,
                });
            }

            this.TapGestureRecognizer0.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 0); };
            this.TapGestureRecognizer1.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 1); };
            //this.TapGestureRecognizer2.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 2); };
            this.TapGestureRecognizer3.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 3); };
            this.TapGestureRecognizer4.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 4); };
            this.TapGestureRecognizer5.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 5); };
            this.TapGestureRecognizer6.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 6); };
            this.TapGestureRecognizer7.Tapped += delegate { for (int i = 0; i < 8; i++) this.Pivot(i, i == 7); };

            this.CanvasControl.Loaded += (s, e) => this.CreateResourcesAsync();
            //this.CanvasControl.Draw += (s, e) => this.Draw(e.DrawingSession);

            this.DragButton.DragStarting += delegate
            {
                this.DragSourceType = DragSourceType.Others;
                this.LayerScrollView.AllowDrop = true;
                this.TransformPoint = this.PointToWindow();
                this.CacheDragOverGuide();
            };
            this.DragButton.DropCompleted += delegate
            {
                this.LayerScrollView.AllowDrop = false;
                this.PopupHost.IsVisible = false;

                this.DropItems();
            };

            this.DropButton.DragOver += (s, e) =>
            {
                this.PopupHost.IsVisible = false;
                e.AcceptedOperation = DataPackageOperation.Copy;
            };
            this.DropButton.Drop += delegate
            {
                switch (this.DragSourceType)
                {
                    case DragSourceType.None:
                        break;
                    case DragSourceType.Others:
                        break;
                    case DragSourceType.UnselectedItems:
                    case DragSourceType.SelectedItems:
                        this.Invalidate(this.TryRemove(this.DragSelection));
                        break;
                    default:
                        break;
                }
            };

            /*
            this.DADListView.DragItemsStarting += delegate
            {
                this.DragSourceType = DragSourceType.Others;
                this.LayerScrollView.Tag = this.FindDescendant();
                this.TransformPoint = this.PointToWindow();
                this.CacheDragOverGuide();
            };
            this.DADListView.DragItemsCompleted += delegate
            {
                this.LayerScrollView.Tag = null;
                this.Popup.IsOpen = false;

                this.DropItems();
            };

            this.DADListView.DragOver += (s, e) =>
            {
                this.Popup.IsOpen = false;
                e.AcceptedOperation = DataPackageOperation.Copy;
            };
            this.DADListView.Drop += delegate
            {
                this.Invalidate(this.TryRemove(this.DragSelection));
            };
             */

            this.DragStarting.Invoked += (s, e) =>
            {
                if (e is ILayer item)
                {
                    switch (item.SelectMode)
                    {
                        case SelectMode.Deselected:
                            this.DragSelection = this.List.IndexRangeOf(item);

                            this.DragSourceType = DragSourceType.UnselectedItems;
                            break;
                        default:
                            this.DragSelection = new IndexSelection(this.List);

                            this.DragSourceType = DragSourceType.SelectedItems;
                            break;
                    }

                    this.LayerScrollView.AllowDrop = true;
                    this.TransformPoint = this.PointToWindow();
                    this.CacheDragOverGuide();
                    return;
                }
            };
            this.DropCompleted.Invoked += delegate
            {
                this.LayerScrollView.AllowDrop = false;
                this.PopupHost.IsVisible = false;

                switch (this.CanReorder)
                {
                    case true:
                        this.Invalidate(this.ReorderItems(this.DropIndexer, this.DragSelection));
                        break;
                    default:
                        break;
                }
            };
            this.LayerScrollView.DragOver += (s, e) =>
            {
                switch (this.DragSourceType)
                {
                    case DragSourceType.None:
                        e.AcceptedOperation = DataPackageOperation.None;
                        break;
                    default:
                        this.UIPoint = new DragOverUIPoint
                        {
                            PositionY = e.GetPosition(this.LayerScrollView).Value.Y,
                            HorizontalOffset = this.LayerScrollView.ScrollX,
                            VerticalOffset = this.LayerScrollView.ScrollY,
                        };

                        this.DropIndexer = this.DragUI.GetIndexer(this.UIPoint, this.DragSourceType);
                        this.UIRect = this.DragUI.GetUIRect(this.UIPoint, this.DropIndexer);

                        if (this.UIRect.IsEmpty)
                        {
                            this.PopupHost.IsVisible = false;
                            e.AcceptedOperation = DataPackageOperation.Copy;
                            break;
                        }

                        AbsoluteLayout.SetLayoutBounds(this.Popup, new Rect
                        {
                            X = this.TransformPoint.X + this.UIRect.X,
                            Y = this.TransformPoint.Y + this.UIRect.Y,
                            Width = this.UIRect.Width,
                            Height = GuideHeight,
                        });
                        this.PopupHost.IsVisible = true;

                        switch (this.DragSourceType)
                        {
                            case DragSourceType.Others:
                                e.AcceptedOperation = DataPackageOperation.Copy;
                                break;
                            case DragSourceType.UnselectedItems:
                            case DragSourceType.SelectedItems:
                                this.CanReorder = this.Collection.CanReorderItems(this.DropIndexer, this.DragSelection);
                                e.AcceptedOperation = this.CanReorder ? DataPackageOperation.Copy : DataPackageOperation.None;
                                break;
                            default:
                                break;
                        }
                        break;
                }
            };

            /*
            this.LayerScrollView.RightTapped += (s, e) =>
            {
                if (e.OriginalSource is FrameworkElement element)
                {
                    if (element.DataContext is ILayer item)
                    {
                        if (item.SelectMode is SelectMode.Deselected)
                            this.Invalidate(this.Click(ClickOptions.SelectOnly, item));
                        this.MenuFlyout.ShowAt(this, e.GetPosition(this));
                    }
                }
            };

            this.HistoryListView.PreviewKeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case VirtualKey.Left:
                    case VirtualKey.Up:
                        this.Invalidate(this.TryUndo());
                        e.Handled = true;
                        break;
                    case VirtualKey.Right:
                    case VirtualKey.Down:
                        this.Invalidate(this.TryRedo());
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            };
             */

            this.CanvasControl.StartInteraction += (s, e) =>
            {
                PointF[] pp = e.Touches;
                this.StartingPoint = this.Point = pp.Last();

                switch (this.Transformer.Rect.IsEmpty is false && this.Transformer.Rect.Contains(this.StartingPoint))
                {
                    case false:
                        foreach (ILayer item in this.FillContainsPointRoot(this.StartingPoint))
                        {
                            this.Invalidate(this.Click(ClickOptions.SelectOnly, item));

                            goto case true;
                        }

                        this.IsMode = false;
                        this.Click(OptionType.DeselectAll);
                        break;
                    case true:
                        this.IsMode = true;
                        this.Invalidate(InvalidateModes.ValueChangeStarted);

                        this.StartingTransformer = this.Transformer.Rect;

                        this.Cache();
                        //this.CanvasControl.CapturePointer(e.Pointer);
                        break;
                    default:
                        break;
                }
            };
            this.CanvasControl.DragInteraction += (s, e) =>
            {
                if (e.Touches is null || e.Touches.Length == 0)
                    return;

                PointF[] pp = e.Touches;
                this.Point = pp.Last();

                if (this.IsMode == false)
                    return;

                this.Transformer = new Rect
                {
                    X = this.StartingTransformer.X + this.Point.X - this.StartingPoint.X,
                    Y = this.StartingTransformer.Y + this.Point.Y - this.StartingPoint.Y,
                    Width = this.StartingTransformer.Width,
                    Height = this.StartingTransformer.Height
                };

                this.Move(this.Point.X - this.StartingPoint.X, this.Point.Y - this.StartingPoint.Y);

                this.Invalidate(InvalidateModes.ValueChangeDelta);
            };
            this.CanvasControl.EndInteraction += delegate
            {
                if (this.IsMode == false)
                    return;

                RectChange[] move = this.GetRectChanges().ToArray();

                switch (move.Length)
                {
                    case 0:
                        break;
                    case 1:
                        this.PushHistory(new Undo
                        {
                            Type = HistoryType.Rect,
                            Description = UIType.ToMove.GetString(),
                            Change = move.Single(),
                        });

                        this.Invalidate(InvalidateModes.ValueChangeCompleted);
                        break;
                    default:
                        this.PushHistory(new Undo
                        {
                            Type = HistoryType.Rects,
                            Description = UIType.ToMoves.GetString(),
                            Change = new RectChanges
                            {
                                Rects = move,
                            }
                        });

                        this.Invalidate(InvalidateModes.ValueChangeCompleted);
                        break;
                }

                //this.CanvasControl.ReleasePointerCaptures();
            };
        }

        public async void PushHistory(Undo undo)
        {
            int gcs = this.History.Push(undo, true);
            switch (gcs)
            {
                case 0:
                    break;
                default:
                    await base.DisplayAlert(
                        UIType.UIGCSuccess.GetString(),
                        string.Format(UIType.UIDispose.GetString(), gcs),
                        UIType.UIBack.GetString()
                        );
                    break;
            }
        }

        private void HistoryCommand_Invoked(object sender, object e)
        {
            if (e is Undo item)
                this.Invalidate(this.TryNavigate(item));
        }

        private void LayersCommand_Invoked(object sender, object e)
        {
            if (e is ILayer item)
                this.Invalidate(this.Click(item.CanSelect(IsShift, IsCtrl), item));
        }

        private async void InfoCommand_Invoked(object sender, object e)
        {
            if (e is ILayer layer)
            {
                string title = layer.Title.ToString();

                StringBuilder sb = new StringBuilder();

                sb.Append(UIType.InfoDepth.GetString());
                sb.AppendLine(layer.Depth.ToString());

                sb.Append(UIType.InfoIsExpanded.GetString());
                sb.AppendLine(layer.IsExpanded.ToString());

                sb.Append(UIType.InfoIsLocked.GetString());
                sb.AppendLine(layer.IsLocked.ToString());

                sb.Append(UIType.InfoIsVisible.GetString());
                sb.AppendLine(layer.IsVisible.ToString());

                sb.Append(UIType.InfoSelectMode.GetString());
                sb.AppendLine(layer.SelectMode.ToString());

                sb.AppendLine(UIType.InfoChildren.GetString());

                foreach (ILayer item in layer.Children)
                {
                    sb.AppendLine(item.ToString());
                }

                string message = sb.ToString();

                await base.DisplayAlert(title, message, UIType.UIBack.GetString());
            }
        }

        private void ExpandCommand_Invoked(object sender, object e)
        {
            if (e is ILayer item)
                this.Invalidate(this.Click(item.CanExpand(), item));
        }

        private void LockCommand_Invoked(object sender, object e)
        {
            if (e is ILayer item)
                this.Invalidate(this.Click(item.CanLock(), item));
        }

        private void VisibleCommand_Invoked(object sender, object e)
        {
            if (e is ILayer item)
                this.Invalidate(this.Click(item.CanVisible(), item));
        }

        private void SelectCommand_Invoked(object sender, object e)
        {
            if (e is ILayer item)
                this.Invalidate(this.Click(item.CanSelect(), item));
        }
    }
}