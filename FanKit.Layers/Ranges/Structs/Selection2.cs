using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Ranges
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Selection2
    {
        public readonly int ItemsCount;

        public readonly Selection1 Source;

        public readonly SelTyp2 Type2;

        public SelectionCount Count => this.Type2.ToSelectionCount();

        public Selection2(IReadOnlyList<ILayerBase> items)
        {
            this.ItemsCount = items.Count;
            this.Source = new Selection1(items);
            this.Type2 = Source.ToSelTyp2(ItemsCount);
        }
    }
}