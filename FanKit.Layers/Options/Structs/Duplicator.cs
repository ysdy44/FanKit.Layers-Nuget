using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Options
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Duplicator']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Duplicator
    {
        internal readonly Selection2 Source;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Duplicator.Count']/*" />
        public SelectionCount Count => this.Source.Count;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Duplicator.Duplicator']/*" />
        public Duplicator(IReadOnlyList<ILayerBase> items)
        {
            this.Source = new Selection2(items);
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Duplicator: Type2 = {this.Source.Type2}, StartIndex = {this.Source.Source.Source.StartIndex}, EndIndex = {this.Source.Source.Source.EndIndex}, Length = {this.Source.Source.Source.Length}, SelectedItemsCount = {this.Source.Source.SelectedItemsCount}, SelectedRangesCount = {this.Source.Source.SelectedRangesCount}]";
    }
}