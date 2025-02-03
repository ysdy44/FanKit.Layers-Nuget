using FanKit.Layers.Changes;
using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Options
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Releaser']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Releaser
    {
        internal readonly Selection2 Source;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Releaser.DepthOfSingle']/*" />
        public readonly Int32Change DepthOfSingle;

        internal bool IsEmpty => this.Source.Source.ToIsEmpty();

        /// <include file="doc/docs.xml" path="docs/doc[@for='Releaser.Count']/*" />
        public SelectionCount Count => this.Source.Type2.ToSelectionCount();

        /// <include file="doc/docs.xml" path="docs/doc[@for='Releaser.Releaser']/*" />
        public Releaser(IReadOnlyList<ILayerBase> items)
        {
            this.Source = new Selection2(items);

            switch (this.Source.Type2)
            {
                case SelTyp2.S:
                    ILayerBase single = items[this.Source.Source.Source.StartIndex];
                    this.DepthOfSingle = single.Decrease();
                    break;
                default:
                    this.DepthOfSingle = default;
                    break;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Releaser: Type2 = {this.Source.Type2}, StartIndex = {this.Source.Source.Source.StartIndex}, EndIndex = {this.Source.Source.Source.EndIndex}, Length = {this.Source.Source.Source.Length}, SelectedItemsCount = {this.Source.Source.SelectedItemsCount}, SelectedRangesCount = {this.Source.Source.SelectedRangesCount}]";
    }
}