using FanKit.Layers.Changes;
using FanKit.Layers.Collections;
using FanKit.Layers.Demo;
using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.TestApp
{
    public sealed partial class SelectedRangesPage : Page
    {
        //@Key
        private static bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        private static bool IsCtrl => IsKeyDown(VirtualKey.Control);
        private static bool IsShift => IsKeyDown(VirtualKey.Shift);

        SelectIndexer Indexer;

        IndexSelection Selection = IndexSelection.Empty;

        readonly LogicalTreeList<DemoLayer> LogicalTree = new LogicalTreeList<DemoLayer>();

        public SelectedRangesPage()
        {
            this.InitializeComponent();

            this.LogicalTree.AddRange(new DemoLayerCollection());
            this.ListView.ItemsSource = this.LogicalTree;

            this.SubtitleTextBlock.Text = this.Selection.ToString();
            this.KeysTextBlock.Text = $"[Ctrl: {IsCtrl}, Shift: {IsShift}]";
        }

        #region ListView

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DemoLayer item = (DemoLayer)e.ClickedItem;

            switch (item.CanSelect(IsShift, IsCtrl))
            {
                case ClickOptions.None:
                    break;
                case ClickOptions.Deselect:
                    this.ApplySelects(this.LogicalTree.Deselect(item));
                    break;
                case ClickOptions.Select:
                    this.Indexer = this.LogicalTree.IndexerOf(item);
                    this.ApplySelects(this.LogicalTree.Select(this.Indexer));
                    break;
                case ClickOptions.SelectOnly:
                    this.Indexer = this.LogicalTree.IndexerOf(item);
                    this.ApplySelects(this.LogicalTree.SelectOnly(this.Indexer));
                    break;
                case ClickOptions.SelectRangeOnly:
                    IndexRange selectRange = this.LogicalTree.IndexRangeOf(item, this.Indexer);
                    if (selectRange.IsNegative)
                        break;

                    this.ApplySelects(this.LogicalTree.SelectRangeOnly(selectRange));
                    break;
                default:
                    break;
            }

            this.Selection = new IndexSelection(this.LogicalTree);

            this.SubtitleTextBlock.Text = this.Selection.ToString();
            this.KeysTextBlock.Text = $"[Ctrl: {IsCtrl}, Shift: {IsShift}]";

            this.ContentListView.ItemsSource = this.LogicalTree.GetSelectedRanges().ToArray();
        }

        #endregion

        public void ApplySelects(IEnumerable<SelectChange> changes)
        {
            foreach (DemoLayer item in this.LogicalTree)
            {
                foreach (SelectChange change in changes)
                {
                    if (item.Id == change.Id)
                    {
                        item.SelectMode = change.NewValue;
                    }
                }
            }
        }
    }
}