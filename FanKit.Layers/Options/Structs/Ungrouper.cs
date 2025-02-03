using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Options
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Ungrouper']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Ungrouper
    {
        internal readonly Selection1 Source;

        private readonly SelTyp1 Type1;

        internal readonly UngroupMode Mode;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Ungrouper.Count']/*" />
        public SelectionCount Count
        {
            get
            {
                switch (this.Mode)
                {
                    case UngroupMode.None:
                        return SelectionCount.None;
                    case UngroupMode.SingleAtLast:
                    case UngroupMode.SingleWithoutChild:
                        return SelectionCount.Single;
                    case UngroupMode.SingleRangeExpand:
                    case UngroupMode.SingleRangeUnexpand:
                    case UngroupMode.MultipleRanges:
                        return SelectionCount.Multiple;
                    default:
                        return default;
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Ungrouper.Ungrouper']/*" />
        public Ungrouper(IReadOnlyList<ILayerBase> items)
        {
            this.Source = new Selection1(items);
            this.Type1 = this.Source.ToSelTyp1();

            switch (this.Type1)
            {
                case SelTyp1.Non:
                    this.Mode = UngroupMode.None;
                    break;
                case SelTyp1.S:
                    int index = this.Source.Source.StartIndex;
                    ILayerBase single = items[index];

                    if (single.IsGroup)
                    {
                        this.Mode = index + 1 < items.Count - 1 ?
                            UngroupMode.SingleWithoutChild :
                            UngroupMode.SingleAtLast;
                        break;
                    }

                    goto case SelTyp1.Non;
                case SelTyp1.SR:
                    ILayerBase first = items[this.Source.Source.StartIndex];

                    this.Mode = first.IsExpanded ? UngroupMode.SingleRangeExpand : UngroupMode.SingleRangeUnexpand;
                    break;
                case SelTyp1.M:
                    foreach (ILayerBase item in items)
                    {
                        if (item.SelectMode != SelectMode.Deselected)
                        {
                            if (item.IsGroup)
                            {
                                this.Mode = UngroupMode.MultipleRanges;
                                return;
                            }
                        }
                    }

                    goto case SelTyp1.Non;
                default:
                    goto case SelTyp1.Non;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Ungrouper: Type1 = {this.Type1}, StartIndex = {this.Source.Source.StartIndex}, EndIndex = {this.Source.Source.EndIndex}, Length = {this.Source.Source.Length}, SelectedItemsCount = {this.Source.SelectedItemsCount}, SelectedRangesCount = {this.Source.SelectedRangesCount}]";
    }
}