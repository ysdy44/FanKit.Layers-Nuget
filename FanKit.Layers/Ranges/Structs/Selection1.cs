using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Ranges
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Selection1
    {
        public readonly int SelectedItemsCount;

        public readonly int SelectedRangesCount;

        public readonly IndexRange Source;

        public bool IsEmpty => this.ToIsEmpty();

        public Selection1(IReadOnlyList<ILayerBase> items)
        {
            this.SelectedItemsCount = 0;
            this.SelectedRangesCount = 0;
            this.Source = IndexRange.NegativeUnit;

            Relation relate = Relation.Empty;

            for (int i = 0; i < items.Count; i++)
            {
                ILayerBase item = items[i];
                switch (relate.Relate(item))
                {
                    case Relp.None:
                        relate = Relation.Empty;
                        break;
                    case Relp.Parent:
                        relate = new Relation(item);

                        this.SelectedItemsCount++;
                        this.SelectedRangesCount++;

                        if (this.Source.IsNegative)
                        {
                            this.Source = new IndexRange(i);
                        }
                        break;
                    case Relp.Child:
                        this.SelectedItemsCount++;

                        if (this.Source.IsNegative)
                            break;

                        this.Source = new IndexRange(this.Source.StartIndex, i);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}