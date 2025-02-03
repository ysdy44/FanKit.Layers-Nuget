using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Options
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Remover']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Remover
    {
        internal readonly int ItemsCount;

        internal readonly Selection1 Source;

        internal bool IsEmpty => this.Source.ToIsEmpty();

        /// <include file="doc/docs.xml" path="docs/doc[@for='Remover.Count']/*" />
        public RemovalCount Count => this.Source.ToRemovalCount(this.ItemsCount);

        /// <include file="doc/docs.xml" path="docs/doc[@for='Remover.Remover']/*" />
        public Remover(IReadOnlyList<ILayerBase> items)
        {
            this.ItemsCount = items.Count;
            this.Source = new Selection1(items);
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Remover: StartIndex = {this.Source.Source.StartIndex}, EndIndex = {this.Source.Source.EndIndex}, Length = {this.Source.Source.Length}, SelectedItemsCount = {this.Source.SelectedItemsCount}, SelectedRangesCount = {this.Source.SelectedRangesCount}]";
    }
}