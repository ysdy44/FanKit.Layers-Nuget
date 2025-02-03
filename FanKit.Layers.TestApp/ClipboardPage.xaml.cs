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
    public sealed partial class ClipboardPage : Page
    {
        //@Key
        private static bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        private static bool IsCtrl => IsKeyDown(VirtualKey.Control);
        private static bool IsShift => IsKeyDown(VirtualKey.Shift);

        SelectIndexer Indexer;
        readonly LayerManager3<DemoLayer> M3 = new LayerManager3<DemoLayer>();

        LayerList<DemoLayer> List => this.M3.List;
        LayerCollection<DemoLayer> Collection => this.M3.Collection;
        Clipboard<DemoLayer> Clipboard => this.M3.Clipboard;

        public ObservableCollection<DemoLayer> UILayers => this.M3.UILayers;

        public ClipboardPage()
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

            #region Clipboard

            this.CutButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.Control, Key = VirtualKey.X });
            this.CutButton.Click += (s, e) =>
            {
                this.Clipboard.Copy();
                this.Remove();
            };

            this.CopyButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.Control, Key = VirtualKey.C });
            this.CopyButton.Click += (s, e) =>
            {
                this.Clipboard.Copy();
            };

            this.PasteButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.Control, Key = VirtualKey.V });
            this.PasteButton.Click += (s, e) =>
            {
                switch (this.Clipboard.Count)
                {
                    case SelectionCount.None:
                        return;
                    case SelectionCount.Single:
                        {
                            Inserter inserter = new Inserter(this.List);

                            DemoLayer clone = this.Clipboard.CloneSingle(inserter.Depth);
                            this.Insert(inserter, clone);
                            break;
                        }
                    default:
                        {
                            Inserter inserter = new Inserter(this.List);

                            SelectChange[] selects = this.List.DeselectAll();
                            InvalidateModes modes = this.Clipboard.Paste(inserter, selects);
                            this.Invalidate(modes);
                        }
                        break;
                }
            };

            this.DuplicateButton.Click += (s, e) =>
            {
                Duplicator duplicator = new Duplicator(this.List);

                if (duplicator.Count is SelectionCount.None)
                    return;

                InvalidateModes modes = this.Clipboard.Duplicate(duplicator);
                this.Invalidate(modes);
            };

            #endregion
        }

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.LayersCleared)) this.UILayers.Clear();
            if (modes.HasFlag(InvalidateModes.LayersChanged)) this.Collection.UISyncTo(this.UILayers);
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