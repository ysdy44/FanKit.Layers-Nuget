namespace FanKit.Layers.Sample
{
    public sealed class ItemClickEventArgs
    {
        private readonly object Item;

        public ItemClickEventArgs(object item)
        {
            this.Item = item;
        }

        public object ClickedItem => this.Item;
    }
}