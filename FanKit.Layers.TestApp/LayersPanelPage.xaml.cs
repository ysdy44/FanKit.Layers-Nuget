using FanKit.Layers.Changes;
using FanKit.Layers.Demo;
using FanKit.Layers.Options;
using FanKit.Layers.Ranges;
using System.Collections.ObjectModel;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FanKit.Layers.TestApp
{
    public sealed partial class LayersPanelPage : Page
    {
        //@Key
        private static bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        private static bool IsCtrl => IsKeyDown(VirtualKey.Control);
        private static bool IsShift => IsKeyDown(VirtualKey.Shift);

        SelectIndexer Indexer;
        readonly LayerManager2<DemoLayer> M2 = new LayerManager2<DemoLayer>();

        LayerList<DemoLayer> List => this.M2.List;
        LayerCollection<DemoLayer> Collection => this.M2.Collection;

        public ObservableCollection<DemoLayer> UILayers => this.M2.UILayers;

        public LayersPanelPage()
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

                const InvalidateModes modes = InvalidateModes.Select;
                this.Invalidate(modes);
            };

            #endregion

            #region View

            this.HideAllButton.Click += (s, e) =>
            {
                BooleanChange[] visibles = this.List.HideAll();

                this.Collection.ApplyVisibles(visibles);
            };
            this.ShowAllButton.Click += (s, e) =>
            {
                BooleanChange[] visibles = this.List.ShowAll();

                this.Collection.ApplyVisibles(visibles);
            };

            this.DeselectAllButton.Click += (s, e) =>
            {
                SelectChange[] selects = this.List.DeselectAll();

                this.Collection.ApplySelects(selects);

                const InvalidateModes modes = InvalidateModes.Select;
                this.Invalidate(modes);
            };
            this.SelectAllButton.Click += (s, e) =>
            {
                SelectChange[] selects = this.List.SelectAll();

                this.Collection.ApplySelects(selects);

                const InvalidateModes modes = InvalidateModes.Select;
                this.Invalidate(modes);
            };

            this.CollapseAllButton.Click += (s, e) =>
            {
                this.Collection.CollapseAll();

                const InvalidateModes modes = InvalidateModes.Expand;
                this.Invalidate(modes);
            };
            this.ExpandAllButton.Click += (s, e) =>
            {
                this.Collection.ExpandAll();

                const InvalidateModes modes = InvalidateModes.Expand;
                this.Invalidate(modes);
            };

            #endregion

            #region Collection

            this.RemoveButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.None, Key = VirtualKey.Delete });
            this.RemoveButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.None, Key = VirtualKey.Back });
            this.RemoveButton.Click += (s, e) => this.Remove();

            this.InsertButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.None, Key = VirtualKey.Insert });
            this.InsertButton.Click += (s, e) =>
            {
                Inserter inserter = new Inserter(this.List);

                this.Insert(inserter, new DemoLayer
                {
                    Title = "?",

                    Depth = inserter.Depth,

                    SelectMode = SelectMode.Selected,
                });
            };

            this.GroupButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.Shift, Key = VirtualKey.G });
            this.GroupButton.Click += (s, e) =>
            {
                Grouper grouper = new Grouper(this.List);

                switch (grouper.Count)
                {
                    case SelectionCount.None:
                        break;
                    case SelectionCount.Single:
                        {
                            DemoLayer group = new DemoLayer
                            {
                                IsGroup = true,
                                Title = "?",

                                Depth = grouper.Depth,
                                IsExpanded = false,

                                SelectMode = SelectMode.Selected,
                            };

                            group.RenderThumbnail();

                            Int32Change depth = grouper.DepthOfSingle;
                            SelectChange select = grouper.SelectingOfSingle;
                            InvalidateModes modes = this.Collection.GroupSingle(grouper, group, depth, select);
                            this.Invalidate(modes);
                        }
                        break;
                    case SelectionCount.Multiple:
                        {
                            DemoLayer group = new DemoLayer
                            {
                                IsGroup = true,
                                Title = "?",

                                Depth = grouper.Depth,
                                IsExpanded = false,

                                SelectMode = SelectMode.Selected,
                            };

                            group.RenderThumbnail();

                            Int32Change[] depths = this.List.GetDepthsForGroupMultiple(grouper);
                            SelectChange[] selects = this.List.GetSelectsForGroupMultiple(grouper);
                            InvalidateModes modes = this.Collection.GroupMultiple(grouper, group, depths, selects);
                            this.Invalidate(modes);
                        }
                        break;
                    default:
                        break;
                }
            };

            #endregion
        }

        #region Command

        private void SelectCommand_Invoked(object sender, DemoLayer e)
        {
            switch (e.CanSelect())
            {
                case ClickOptions.Deselect:
                    this.Collection.ApplySelects(this.List.Deselect(e));
                    break;
                case ClickOptions.Select:
                    this.Indexer = this.List.IndexerOf(e);
                    this.Collection.ApplySelects(this.List.Select(this.Indexer));
                    break;
                default:
                    break;
            }
        }

        private void VisibleCommand_Invoked(object sender, DemoLayer e)
        {
            switch (e.CanVisible())
            {
                case ClickOptions.Hide:
                    this.Collection.ApplyVisible(e.ToFalse());
                    break;
                case ClickOptions.Show:
                    this.Collection.ApplyVisible(e.ToTrue());
                    break;
                default:
                    break;
            }
        }

        private void LockCommand_Invoked(object sender, DemoLayer e)
        {
            switch (e.CanLock())
            {
                case ClickOptions.Lock:
                    this.Collection.ApplyLock(e.ToTrue());
                    break;
                case ClickOptions.Unlock:
                    this.Collection.ApplyLock(e.ToFalse());
                    break;
                default:
                    break;
            }
        }

        private void ExpandCommand_Invoked(object sender, DemoLayer e)
        {
            switch (e.CanExpand())
            {
                case ClickOptions.Collapse:
                    e.IsExpanded = false;
                    this.Collection.SyncToVisualTree();
                    this.Collection.UISyncTo(this.UILayers);
                    break;
                case ClickOptions.Expand:
                    e.IsExpanded = true;
                    this.Collection.SyncToVisualTree();
                    this.Collection.UISyncTo(this.UILayers);
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.LayersCleared)) this.UILayers.Clear();
            if (modes.HasFlag(InvalidateModes.LayersChanged)) this.Collection.UISyncTo(this.UILayers);
            if (modes.HasFlag(InvalidateModes.LayerCanExecuteChanged))
            {
                IndexSelection selection = new IndexSelection(this.List);

                this.RemoveButton.IsEnabled = selection.IsEmpty is false;
                this.InsertButton.IsEnabled = selection.IsEmpty is false;
            }
        }

        private void Remove()
        {
            Remover remover = new Remover(this.List);

            switch (remover.Count)
            {
                case RemovalCount.None:
                    break;
                case RemovalCount.Remove:
                    {
                        InvalidateModes modes = this.Collection.Remove(remover);
                        this.Invalidate(modes);
                    }
                    break;
                case RemovalCount.RemoveAll:
                    {
                        InvalidateModes modes = this.Collection.Clear();
                        this.Invalidate(modes);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Insert(Inserter inserter, DemoLayer add)
        {
            switch (inserter.HasSelected)
            {
                case false:
                    {
                        InvalidateModes modes = this.Collection.InsertAtTop(add);
                        this.Invalidate(modes);
                    }
                    break;
                case true:
                    {
                        SelectChange[] selects = this.List.DeselectAll();
                        InvalidateModes modes = this.Collection.Insert(inserter, add, selects);
                        this.Invalidate(modes);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}