using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Ranges
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='IndexSelection']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct IndexSelection // Selection3
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexSelection.Empty']/*" />
        public static IndexSelection Empty { get; } = new IndexSelection(SelTyp3.Non);

        internal readonly SelTyp3 Type3;

        internal readonly IndexRange Source;

        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexSelection.IsEmpty']/*" />
        public bool IsEmpty => this.Type3 == SelTyp3.Non;

        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexSelection.RemovalCount']/*" />
        public RemovalCount RemovalCount => this.Type3.ToRemovalCount();

        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexSelection.SelectionCount']/*" />
        public SelectionCount SelectionCount => this.Type3.ToSelectionCount();

        internal IndexSelection(SelTyp3 type)
        {
            this.Type3 = type;
            this.Source = IndexRange.NegativeUnit;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexSelection.IndexSelection']/*" />
        public IndexSelection(IReadOnlyList<ILayerBase> items)
        {
            switch (items.Count)
            {
                case 0:
                    this.Type3 = SelTyp3.Non;
                    this.Source = IndexRange.NegativeUnit;
                    break;
                case 1:
                    this.Type3 = SelTyp3.AS;
                    this.Source = IndexRange.NegativeUnit;
                    break;
                default:
                    Selection1 selection = new Selection1(items);

                    switch (selection.ToSelTyp1())
                    {
                        case SelTyp1.Non:
                            this.Type3 = SelTyp3.Non;
                            this.Source = IndexRange.NegativeUnit;
                            break;
                        case SelTyp1.S:
                            if (items.First().SelectMode.IsSelected())
                            {
                                this.Type3 = SelTyp3.FS;
                                this.Source = IndexRange.Zero;
                            }
                            else if (items.Last().SelectMode is SelectMode.Deselected)
                            {
                                this.Type3 = SelTyp3.S;
                                this.Source = selection.Source;
                            }
                            else
                            {
                                this.Type3 = SelTyp3.BS;
                                this.Source = new IndexRange(items.Count - 1);
                            }
                            break;
                        case SelTyp1.SR:
                            if (selection.SelectedItemsCount >= items.Count)
                            {
                                this.Type3 = SelTyp3.ASR;
                                this.Source = new IndexRange(0, items.Count - 1);
                            }
                            else if (items.First().SelectMode.IsSelected())
                            {
                                this.Type3 = SelTyp3.FSR;
                                this.Source = new IndexRange(0, selection.Source.EndIndex);
                            }
                            else if (items.Last().SelectMode is SelectMode.Deselected)
                            {
                                this.Type3 = SelTyp3.SR;
                                this.Source = selection.Source;
                            }
                            else
                            {
                                this.Type3 = SelTyp3.BSR;
                                this.Source = new IndexRange(selection.Source.StartIndex, items.Count - 1);
                            }
                            break;
                        case SelTyp1.M:
                            if (selection.SelectedItemsCount < items.Count)
                            {
                                this.Type3 = SelTyp3.M;
                                this.Source = IndexRange.NegativeUnit;
                            }
                            else
                            {
                                this.Type3 = SelTyp3.AM;
                                this.Source = new IndexRange(0, items.Count - 1);
                            }
                            break;
                        default:
                            this.Type3 = SelTyp3.Non;
                            this.Source = IndexRange.NegativeUnit;
                            break;
                    }
                    break;
            }
        }

        internal IndexSelection(IReadOnlyList<ILayerBase> items, int depth, int index)
        {
            switch (index)
            {
                case 0:
                    {
                        ILayerBase item1 = items[1];

                        if (depth >= item1.Depth)
                        {
                            this.Type3 = SelTyp3.FS;
                            this.Source = IndexRange.Zero;
                            return;
                        }

                        for (int i = 1 + 1; i < items.Count; i++)
                        {
                            ILayerBase item = items[i];

                            if (depth >= item.Depth)
                            {
                                this.Type3 = SelTyp3.FSR;
                                this.Source = new IndexRange(0, i - 1);
                                return;
                            }
                        }

                        this.Type3 = SelTyp3.ASR;
                        this.Source = new IndexRange(0, items.Count - 1);
                    }
                    break;
                default:
                    {
                        if (index >= items.Count - 1)
                        {
                            this.Type3 = SelTyp3.BS;
                            this.Source = new IndexRange(index);
                            return;
                        }

                        ILayerBase item1 = items[index + 1];

                        if (depth >= item1.Depth)
                        {
                            this.Type3 = SelTyp3.S;
                            this.Source = new IndexRange(index);
                            return;
                        }

                        for (int i = index + 1 + 1; i < items.Count; i++)
                        {
                            ILayerBase item = items[i];

                            if (depth >= item.Depth)
                            {
                                this.Type3 = SelTyp3.SR;
                                this.Source = new IndexRange(index, i - 1);
                                return;
                            }
                        }

                        this.Type3 = SelTyp3.BSR;
                        this.Source = new IndexRange(index, items.Count - 1);
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[IndexSelection: Type = {this.Type3}, StartIndex = {this.Source.StartIndex}, EndIndex = {this.Source.EndIndex}, Length = {this.Source.Length}]";
    }
}