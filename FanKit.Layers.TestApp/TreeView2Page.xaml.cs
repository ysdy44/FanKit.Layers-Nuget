using FanKit.Layers.Demo;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.TestApp
{
    public sealed partial class TreeView2Page : Page
    {
        private readonly LayerManager2<DemoLayer> M2 = new LayerManager2<DemoLayer>();

        public LayerList<DemoLayer> List => this.M2.List;
        public LayerCollection<DemoLayer> Collection => this.M2.Collection;

        public DragUI<DemoLayer> DragUI => this.M2.DragUI;

        public ObservableCollection<DemoLayer> UILayers => this.M2.UILayers;

        public TreeView2Page()
        {
            this.InitializeComponent();
            this.Collection.ResetByList(new DemoLayerCollection());

            //this.UILayers.Clear();
            this.Collection.UISyncTo(this.UILayers);

            this.ListView.ItemsSource = this.UILayers;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DemoLayer item = (DemoLayer)e.ClickedItem;
            this.Collection.ApplySelects(this.List.SelectOnly(item));

            this.Run0.Text = item.SelectMode.ToString();
        }

        private void ExpandCommand_Invoked(object sender, DemoLayer e)
        {
            DemoLayer item = e;
            switch (item.CanExpand())
            {
                case ClickOptions.Collapse:
                    item.IsExpanded = false;
                    this.Collection.SyncToVisualTree();
                    this.Collection.UISyncTo(this.UILayers);

                    this.Run0.Text = item.SelectMode.ToString();
                    this.Run1.Text = bool.FalseString;
                    break;
                case ClickOptions.Expand:
                    item.IsExpanded = true;
                    this.Collection.SyncToVisualTree();
                    this.Collection.UISyncTo(this.UILayers);

                    this.Run0.Text = item.SelectMode.ToString();
                    this.Run1.Text = bool.TrueString;
                    break;
                default:
                    break;
            }
        }
    }
}