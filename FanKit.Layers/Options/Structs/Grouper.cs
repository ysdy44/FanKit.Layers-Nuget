using FanKit.Layers.Changes;
using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Options
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Grouper']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Grouper
    {
        internal readonly Selection2 Source;

        internal readonly int Index;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Grouper.Depth']/*" />
        public readonly int Depth;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Grouper.DepthOfSingle']/*" />
        public readonly Int32Change DepthOfSingle;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Grouper.SelectingOfSingle']/*" />
        public readonly SelectChange SelectingOfSingle;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Grouper.Count']/*" />
        public SelectionCount Count => this.Source.Type2.ToSelectionCount();

        /// <include file="doc/docs.xml" path="docs/doc[@for='Grouper.Grouper']/*" />
        public Grouper(IReadOnlyList<ILayerBase> items)
        {
            this.Source = new Selection2(items);

            switch (this.Source.Type2)
            {
                case SelTyp2.S:
                    this.Index = this.Source.Source.Source.StartIndex;

                    ILayerBase single = items[this.Index];
                    this.Depth = single.Depth;

                    this.DepthOfSingle = single.Increase();
                    this.SelectingOfSingle = single.ToParent();
                    break;
                case SelTyp2.SR:
                case SelTyp2.M:
                    this.Index = this.Source.Source.Source.StartIndex;
                    this.Depth = items[this.Index].Depth;

                    this.DepthOfSingle = default;
                    this.SelectingOfSingle = default;
                    break;
                default:
                    this.Index = -1;
                    this.Depth = 0;

                    this.DepthOfSingle = default;
                    this.SelectingOfSingle = default;
                    break;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Grouper: Type2 = {this.Source.Type2}, Index = {this.Index}, Depth = {this.Depth}, StartIndex = {this.Source.Source.Source.StartIndex}, EndIndex = {this.Source.Source.Source.EndIndex}, Length = {this.Source.Source.Source.Length}, SelectedItemsCount = {this.Source.Source.SelectedItemsCount}, SelectedRangesCount = {this.Source.Source.SelectedRangesCount}]";
    }
}