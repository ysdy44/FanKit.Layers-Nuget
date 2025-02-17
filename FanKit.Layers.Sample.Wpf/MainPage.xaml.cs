using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FanKit.Layers.Sample
{
    public partial class MainPage : Page
    {
        private static bool IsKeyDown(Key key) => Keyboard.IsKeyDown(key);
        private static bool IsCtrl => IsKeyDown(Key.LeftCtrl) || IsKeyDown(Key.RightCtrl);
        private static bool IsShift => IsKeyDown(Key.LeftShift) || IsKeyDown(Key.RightShift);

        // Tool
        bool IsMode;

        System.Drawing.Point Point;
        System.Drawing.Point StartingPoint;

        Transformer Transformer = System.Drawing.Rectangle.Empty;
        System.Drawing.Rectangle StartingTransformer;

        float BitmapWidth;
        float BitmapHeight;
        System.Drawing.Bitmap Bitmap;

        Point TransformPoint;

        readonly Popup Popup = new Popup
        {
            AllowsTransparency = true,
            Placement = PlacementMode.Absolute,
            Focusable = false,

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
            base.DataContext = this;
            base.FlowDirection = CultureInfoCollection.FlowDirection;
            this.UndoCommand = new UndoCommand(this);
            this.RedoCommand = new RedoCommand(this);
            this.InitializeComponent();

            foreach (OptionTypeMenu item in this.Menus)
            {
                this.MenuFlyout.Items.Add(this.ToMenu(item));
            }

            foreach (OptionCatalogMenu catalog in this.CatalogMenus)
            {
                MenuItem bar = new MenuItem { Header = catalog.Catalog.GetString() };

                foreach (OptionTypeMenu item in catalog.Items)
                {
                    bar.Items.Add(this.ToMenu(item));
                    foreach (InputBinding inputBinding in this.ToInputBindings(item))
                    {
                        base.InputBindings.Add(inputBinding);
                    }
                }

                this.MenuBar.Items.Add(bar);
            }

            this.MenuBar.Items.Add(this.LanguageCommand.ToMenuBarItem());

            this.CanvasControl.Loaded += (s, e) => this.CreateResources();
            this.CanvasControl.Draw += (s, e) => this.Draw(e);

            this.DragButton.MouseLeftButtonDown += delegate
            {
                DataObject data = new DataObject(typeof(string), "Drag");

                this.DragSourceType = DragSourceType.Others;
                this.TransformPoint = this.PointToWindow();
                this.CacheDragOverGuide();

                System.Windows.DragDrop.DoDragDrop(this.DragButton, data, DragDropEffects.Move);
            };

            this.DropButton.DragOver += (s, e) =>
            {
                this.Popup.IsOpen = false;
                e.Effects = DragDropEffects.Copy;
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

            this.DADListView.PreviewMouseLeftButtonDown += delegate
            {
                DataObject data = new DataObject(typeof(string), "Drag");

                this.DragSourceType = DragSourceType.Others;
                this.TransformPoint = this.PointToWindow();
                this.CacheDragOverGuide();

                System.Windows.DragDrop.DoDragDrop(this.DADListView, data, DragDropEffects.Move);
            };

            this.DADListView.DragOver += (s, e) =>
            {
                this.Popup.IsOpen = false;
                e.Effects = DragDropEffects.Copy;
            };
            this.DADListView.Drop += delegate
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

            this.LayerScrollViewer.Drop += (s, e) =>
            {
                this.Popup.IsOpen = false;

                switch (this.DragSourceType)
                {
                    case DragSourceType.None:
                        break;
                    case DragSourceType.Others:
                        this.DropItems();
                        break;
                    case DragSourceType.UnselectedItems:
                    case DragSourceType.SelectedItems:
                        this.Invalidate(this.ReorderItems(this.DropIndexer, this.DragSelection));
                        break;
                    default:
                        break;
                }
            };

            this.LayerScrollViewer.DragItemsStarting += (s, e) =>
            {
                this.Popup.IsOpen = true;

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

                    this.TransformPoint = this.PointToWindow();
                    this.CacheDragOverGuide();
                    break;
                }
            };

            this.LayerScrollViewer.DragEnter += (s, e) =>
            {
            };
            this.LayerScrollViewer.DragLeave += (s, e) =>
            {
                this.Popup.IsOpen = false;

                e.Effects = DragDropEffects.Copy;
            };
            this.LayerScrollViewer.DragOver += (s, e) =>
            {
                switch (this.DragSourceType)
                {
                    case DragSourceType.None:
                        e.Effects = DragDropEffects.None;
                        break;
                    default:
                        this.UIPoint = new DragOverUIPoint
                        {
                            PositionY = e.GetPosition(this.LayerScrollViewer).Y,
                            HorizontalOffset = this.LayerScrollViewer.HorizontalOffset,
                            VerticalOffset = this.LayerScrollViewer.VerticalOffset,
                        };

                        this.DropIndexer = this.DragUI.GetIndexer(this.UIPoint, this.DragSourceType);
                        this.UIRect = this.DragUI.GetUIRect(this.UIPoint, this.DropIndexer);

                        if (this.UIRect.IsEmpty)
                        {
                            this.Popup.IsOpen = false;
                            e.Effects = DragDropEffects.Copy;
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
                                e.Effects = DragDropEffects.Copy;
                                break;
                            case DragSourceType.UnselectedItems:
                            case DragSourceType.SelectedItems:
                                bool canReorder = this.Collection.CanReorderItems(this.DropIndexer, this.DragSelection);
                                e.Effects = canReorder ? DragDropEffects.Move : DragDropEffects.None;
                                break;
                            default:
                                break;
                        }
                        break;
                }
            };

            this.LayerScrollViewer.MouseRightButtonDown += (s, e) =>
            {
                if (e.OriginalSource is FrameworkElement element)
                {
                    if (element.DataContext is ILayer item)
                    {
                        if (item.SelectMode is SelectMode.Deselected)
                            this.Invalidate(this.Click(ClickOptions.SelectOnly, item));
                        this.MenuFlyout.IsOpen = true;
                    }
                }
            };

            this.HistoryScrollViewer.PreviewKeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.Left:
                    case Key.Up:
                        this.Invalidate(this.TryUndo());
                        e.Handled = true;
                        break;
                    case Key.Right:
                    case Key.Down:
                        this.Invalidate(this.TryRedo());
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            };

            this.CanvasControl.MouseDown += (s, e) =>
            {
                Point pp = e.GetPosition(this.CanvasControl);
                this.StartingPoint = this.Point = new System.Drawing.Point((int)pp.X, (int)pp.Y);

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
                        this.CanvasControl.CaptureMouse();
                        break;
                    default:
                        break;
                }
            };
            this.CanvasControl.MouseMove += (s, e) =>
            {
                if (this.CanvasControl.IsMouseCaptured is false)
                    return;

                Point pp = e.GetPosition(this.CanvasControl);
                this.Point = new System.Drawing.Point((int)pp.X, (int)pp.Y);

                if (this.IsMode == false)
                    return;

                this.Transformer = new System.Drawing.Rectangle
                {
                    X = this.StartingTransformer.X + this.Point.X - this.StartingPoint.X,
                    Y = this.StartingTransformer.Y + this.Point.Y - this.StartingPoint.Y,
                    Width = this.StartingTransformer.Width,
                    Height = this.StartingTransformer.Height
                };

                this.Move(this.Point.X - this.StartingPoint.X, this.Point.Y - this.StartingPoint.Y);

                this.Invalidate(InvalidateModes.ValueChangeDelta);
            };
            this.CanvasControl.MouseUp += delegate
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

                this.CanvasControl.ReleaseMouseCapture();
            };
        }

        public void PushHistory(Undo undo)
        {
            int gcs = this.History.Push(undo, true);
            switch (gcs)
            {
                case 0:
                    break;
                default:
                    MessageBox.Show(
                        string.Format(UIType.UIDispose.GetString(), gcs),
                        UIType.UIGCSuccess.GetString()
                        );
                    break;
            }
        }

        private void HistoryListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is Undo item)
                this.Invalidate(this.TryNavigate(item));
        }

        private void LayerScrollViewer_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Popup.IsOpen = false;

            if (e.ClickedItem is ILayer item)
                this.Invalidate(this.Click(item.CanSelect(IsShift, IsCtrl), item));
        }

        private void InfoCommand_Invoked(object sender, object e)
        {
            if (e is ILayer item)
                new InfoDialog(item) { Owner = this.Parent as Window }.ShowDialog();
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