using System.Collections.Generic;

namespace FanKit.Layers.Sample
{
    public sealed class DragItemsEventArgs
    {
        private readonly object item;

        public DragItemsEventArgs(object item)
        {
            this.item = item;
        }

        public IEnumerable<object> Items
        {
            get
            {
                yield return this.item;
            }
        }
    }
}