﻿using FanKit.Layers.Changes;
using FanKit.Layers.Demo;
using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using FanKit.Layers.Reorders;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace FanKit.Layers.TestApp
{
    public sealed partial class ReorderPage : Page
    {
        //@Key
        private static bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        private static bool IsCtrl => IsKeyDown(VirtualKey.Control);
        private static bool IsShift => IsKeyDown(VirtualKey.Shift);

        // DragItemsStarting
        IndexSelection DragSelection;

        // DragStarting
        DragSourceType DragSourceType;
        Point TransformPoint;

        // DragOver
        DragOverUIPoint UIPoint;
        DragOverUIRect UIRect;
        DropIndexer DropIndexer;

        SelectIndexer Indexer;
        readonly LayerManager2<DemoLayer> M2 = new LayerManager2<DemoLayer>();

        DragUI<DemoLayer> DragUI => this.M2.DragUI;
        LayerList<DemoLayer> List => this.M2.List;
        LayerCollection<DemoLayer> Collection => this.M2.Collection;

        public ObservableCollection<DemoLayer> UILayers => this.M2.UILayers;
        readonly Popup Popup = new Popup
        {
            IsHitTestVisible = false,
            Width = 200,
            Height = 16,
            Child = new Grid
            {
                IsHitTestVisible = false,
                Width = 200,
                Children =
                {
                    new Windows.UI.Xaml.Shapes.Rectangle
                    {
                        Width = 16,
                        Height = 16,
                        StrokeThickness = 2,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Stroke =  new SolidColorBrush(Colors.DodgerBlue),
                        Fill = new SolidColorBrush(Colors.DodgerBlue)
                        {
                            Opacity = 0.4
                        },
                    },
                    new Windows.UI.Xaml.Shapes.Rectangle
                    {
                        Height = 2,
                        Margin = new Thickness(16, 0, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Center,
                        Fill =  new SolidColorBrush(Colors.DodgerBlue),
                    },
                }
            }
        };

        public ReorderPage()
        {
            this.InitializeComponent();

            this.Collection.ResetByList(new DemoLayerCollection());

            this.Collection.UISyncTo(this.UILayers);

            #region ListView

            this.ListView.ItemsSource = this.UILayers;
            this.ListView.ItemClick += (s, e) =>
            {
                DemoLayer item = (DemoLayer)e.ClickedItem;

                switch (item.CanSelect(IsShift, IsCtrl))
                {
                    case ClickOptions.None:
                        break;
                    case ClickOptions.Deselect:
                        this.Collection.ApplySelects(this.List.Deselect(item));
                        break;
                    case ClickOptions.Select:
                        this.Indexer = this.List.IndexerOf(item);
                        this.Collection.ApplySelects(this.List.Select(this.Indexer));
                        break;
                    case ClickOptions.SelectOnly:
                        this.Indexer = this.List.IndexerOf(item);
                        this.Collection.ApplySelects(this.List.SelectOnly(this.Indexer));
                        break;
                    case ClickOptions.SelectRangeOnly:
                        IndexRange selectRange = this.List.IndexRangeOf(item, this.Indexer);
                        if (selectRange.IsNegative)
                            break;

                        this.Collection.ApplySelects(this.List.SelectRangeOnly(selectRange));
                        break;
                    default:
                        break;
                }
            };

            #endregion

            #region ListView & Reorder

            this.ListView.DragItemsStarting += (s, e) =>
            {
                foreach (DemoLayer item in e.Items)
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

                    this.ListView.Tag = this.ListView.FindDescendantScrollViewer();
                    this.TransformPoint = this.ListView.PointToWindow();
                    this.CacheDragOverGuide();
                    break;
                }
            };

            this.ListView.DragItemsCompleted += (s, e) =>
            {
                this.ListView.Tag = null;
                this.Popup.IsOpen = false;

                switch (e.DropResult)
                {
                    case DataPackageOperation.Move:
                        Reorder reorder = new Reorder(this.List, this.DropIndexer, this.DragSelection);

                        if (reorder.IsSibling)
                        {
                            InvalidateModes modes = this.Collection.MoveAboveSibling(reorder);
                            this.Invalidate(modes);
                            break;
                        }

                        switch (reorder.Count)
                        {
                            case ReorderCount.None:
                                break;
                            case ReorderCount.Single:
                                {
                                    InvalidateModes modes = this.Collection.ReorderSingle(reorder);

                                    if (modes.HasFlag(InvalidateModes.LayersCleared)) this.UILayers.Clear();
                                    if (modes.HasFlag(InvalidateModes.LayersChanged)) this.Collection.UISyncTo(this.UILayers);
                                }
                                break;
                            case ReorderCount.SingleRange:
                                {
                                    Int32Change[] depths = this.List.GetDepthsForReorderMultiple(reorder);
                                    InvalidateModes modes = this.Collection.ReorderSingleRange(reorder, depths);
                                    this.Invalidate(modes);
                                }
                                break;
                            case ReorderCount.Multiple:
                                {
                                    IndexRange[] selectedRanges = this.List.GetSelectedRanges();
                                    Int32Change[] depths = this.List.GetDepthsForReorderMultiple(reorder, selectedRanges);
                                    InvalidateModes modes = this.Collection.ReorderMultiple(reorder, depths, selectedRanges);
                                    this.Invalidate(modes);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            };

            this.ListView.DragOver += (s, e) =>
            {
                switch (this.DragSourceType)
                {
                    case DragSourceType.None:
                        e.AcceptedOperation = DataPackageOperation.None;
                        break;
                    default:
                        ScrollViewer scrollViewer = (ScrollViewer)this.ListView.Tag;
                        if (scrollViewer is null)
                            break;

                        this.UIPoint = new DragOverUIPoint
                        {
                            PositionY = e.GetPosition(this.ListView).Y,
                            HorizontalOffset = scrollViewer.HorizontalOffset,
                            VerticalOffset = scrollViewer.VerticalOffset,
                            HeaderHeight = this.TitleStackPanel.ActualHeight,
                        };

                        this.DropIndexer = this.DragUI.GetIndexer(this.UIPoint, this.DragSourceType);
                        this.UIRect = this.DragUI.GetUIRect(this.UIPoint, this.DropIndexer);
                        this.TitleTextBlock.Text = this.DropIndexer.ToString();

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

            #endregion
        }

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.LayersCleared)) this.UILayers.Clear();
            if (modes.HasFlag(InvalidateModes.LayersChanged)) this.Collection.UISyncTo(this.UILayers);
        }

        #region Drag & Drop

        private void CacheDragOverGuide()
        {
            this.DragUI.CacheDragOverGuide(this.ListView.ActualWidth, 16, this.ContainerSize);
        }

        private double ContainerSize(int i)
        {
            return this.ListView.ContainerFromIndex(i) is FrameworkElement element ? element.ActualHeight : 0;
        }

        #endregion
    }
}