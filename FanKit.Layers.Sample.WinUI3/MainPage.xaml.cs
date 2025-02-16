using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using Microsoft.Graphics.Canvas;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace FanKit.Layers.Sample
{
    public sealed partial class MainPage : Page
    {
        private static bool IsKeyDown(VirtualKey key) => InputKeyboardSource.GetKeyStateForCurrentThread(key).HasFlag(CoreVirtualKeyStates.Down);
        private static bool IsCtrl => IsKeyDown(VirtualKey.Control);
        private static bool IsShift => IsKeyDown(VirtualKey.Shift);

        bool IsMode;

        Point Point;
        Point StartingPoint;

        Transformer Transformer = Rect.Empty;
        Rect StartingTransformer;

        float BitmapWidth;
        float BitmapHeight;
        CanvasBitmap Bitmap;

        Point TransformPoint;

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

        public MainPage()
        {
            this.UndoCommand = new UndoCommand(this);
            this.RedoCommand = new RedoCommand(this);
            this.InitializeComponent();
            this.RootLayout.FlowDirection = CultureInfoCollection.FlowDirection;

            base.Loaded += delegate
            {
                this.LanguageCommand.XamlRoot = this.XamlRoot;
                this.Popup.XamlRoot = this.XamlRoot;
            };

            foreach (OptionTypeMenu item in this.Menus)
            {
                this.MenuFlyout.Items.Add(this.ToMenu(item));
            }

            foreach (OptionCatalogMenu catalog in this.CatalogMenus)
            {
                MenuBarItem bar = new MenuBarItem { Title = catalog.Catalog.GetString() };

                foreach (OptionTypeMenu item in catalog.Items)
                {
                    bar.Items.Add(this.ToMenu(item));
                }

                this.MenuBar.Items.Add(bar);
            }

            this.MenuBar.Items.Add(this.LanguageCommand.ToMenuBarItem());

            this.CanvasControl.CreateResources += (s, e) => e.TrackAsyncAction(this.CreateResourcesAsync(s).AsAsyncAction());
            this.CanvasControl.Draw += (s, e) => this.Draw(e.DrawingSession);

            this.DragButton.DragStarting += delegate
            {
                this.DragSourceType = DragSourceType.Others;
                this.LayerListView.Tag = this.FindDescendant();
                this.TransformPoint = this.PointToWindow();
                this.CacheDragOverGuide();
            };
            this.DragButton.DropCompleted += delegate
            {
                this.LayerListView.Tag = null;
                this.Popup.IsOpen = false;

                this.DropItems();
            };

            this.DropButton.DragOver += (s, e) =>
            {
                this.Popup.IsOpen = false;
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

            this.DADListView.DragItemsStarting += delegate
            {
                this.DragSourceType = DragSourceType.Others;
                this.LayerListView.Tag = this.FindDescendant();
                this.TransformPoint = this.PointToWindow();
                this.CacheDragOverGuide();
            };
            this.DADListView.DragItemsCompleted += delegate
            {
                this.LayerListView.Tag = null;
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

            this.LayerListView.DragItemsStarting += (s, e) =>
            {
                foreach (ILayer item in e.Items)
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

                    this.LayerListView.Tag = this.FindDescendant();
                    this.TransformPoint = this.PointToWindow();
                    this.CacheDragOverGuide();
                    break;
                }
            };
            this.LayerListView.DragItemsCompleted += (s, e) =>
            {
                this.LayerListView.Tag = null;
                this.Popup.IsOpen = false;

                switch (e.DropResult)
                {
                    case DataPackageOperation.Move:
                        this.Invalidate(this.ReorderItems(this.DropIndexer, this.DragSelection));
                        break;
                    default:
                        break;
                }
            };
            this.LayerListView.DragOver += (s, e) =>
            {
                switch (this.DragSourceType)
                {
                    case DragSourceType.None:
                        e.AcceptedOperation = DataPackageOperation.None;
                        break;
                    default:
                        ScrollViewer scrollViewer = (ScrollViewer)this.LayerListView.Tag;
                        if (scrollViewer is null)
                            break;

                        this.UIPoint = new DragOverUIPoint
                        {
                            PositionY = e.GetPosition(this.LayerListView).Y,
                            HorizontalOffset = scrollViewer.HorizontalOffset,
                            VerticalOffset = scrollViewer.VerticalOffset,
                        };

                        this.DropIndexer = this.DragUI.GetIndexer(this.UIPoint, this.DragSourceType);
                        this.UIRect = this.DragUI.GetUIRect(this.UIPoint, this.DropIndexer);

                        if (this.UIRect.IsEmpty)
                        {
                            this.Popup.IsOpen = false;
                            e.AcceptedOperation = DataPackageOperation.Copy;
                            break;
                        }

                        this.Popup.Width = this.UIRect.Width;
                        (this.Popup.Child as FrameworkElement).Width = this.UIRect.Width;
                        this.Popup.HorizontalOffset = this.TransformPoint.X + this.UIRect.X;
                        this.Popup.VerticalOffset = this.TransformPoint.Y + this.UIRect.Y;
                        this.Popup.IsOpen = true;

                        switch (this.DragSourceType)
                        {
                            case DragSourceType.Others:
                                e.AcceptedOperation = DataPackageOperation.Copy;
                                break;
                            case DragSourceType.UnselectedItems:
                            case DragSourceType.SelectedItems:
                                bool canReorder = this.Collection.CanReorderItems(this.DropIndexer, this.DragSelection);
                                e.AcceptedOperation = canReorder ? DataPackageOperation.Move : DataPackageOperation.None;
                                break;
                            default:
                                break;
                        }
                        break;
                }
            };

            this.LayerListView.RightTapped += (s, e) =>
            {
                if (e.OriginalSource is FrameworkElement element)
                {
                    if (element.DataContext is ILayer item)
                    {
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

            this.CanvasControl.PointerPressed += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.CanvasControl);
                this.StartingPoint = this.Point = pp.Position;

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
                        this.CanvasControl.CapturePointer(e.Pointer);
                        break;
                    default:
                        break;
                }
            };
            this.CanvasControl.PointerMoved += (s, e) =>
            {
                if (this.CanvasControl.PointerCaptures is null || this.CanvasControl.PointerCaptures.Count is 0)
                    return;

                PointerPoint pp = e.GetCurrentPoint(this.CanvasControl);
                this.Point = pp.Position;

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
            this.CanvasControl.PointerReleased += delegate
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

                this.CanvasControl.ReleasePointerCaptures();
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
                    await new MessageDialog(
                        string.Format(UIType.UIDispose.GetString(), gcs),
                        UIType.UIGCSuccess.GetString()
                        ).ShowAsync();
                    break;
            }
        }

        private void HistoryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Undo item)
                this.Invalidate(this.TryNavigate(item));
        }

        private void LayerListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ILayer item)
                this.Invalidate(this.Click(item.CanSelect(IsShift, IsCtrl), item));
        }

        private async void InfoCommand_Invoked(object sender, object e)
        {
            if (e is ILayer item)
                await new InfoDialog(item) { XamlRoot = base.XamlRoot }.ShowAsync();
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