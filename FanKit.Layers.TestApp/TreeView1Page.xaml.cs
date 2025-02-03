using FanKit.Layers.Changes;
using FanKit.Layers.Demo;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.TestApp
{
    public sealed partial class TreeView1Page : Page
    {
        private readonly LayerManager1<DemoLayer> M1 = new LayerManager1<DemoLayer>();

        public LayerList<DemoLayer> List => this.M1.List;

        public TreeView1Page()
        {
            this.InitializeComponent();
            this.List.ResetByList(new DemoLayerCollection());

            this.ListView.ItemsSource = this.List;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DemoLayer item = (DemoLayer)e.ClickedItem;
            this.ApplySelects(this.List.SelectOnly(item));

            this.Run0.Text = item.SelectMode.ToString();
        }

        public void ApplySelects(IEnumerable<SelectChange> changes)
        {
            foreach (ILayerBase item in this.List)
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