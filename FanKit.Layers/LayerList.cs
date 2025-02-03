using FanKit.Layers.Changes;
using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using FanKit.Layers.Options;
using FanKit.Layers.Ranges;
using FanKit.Layers.Reorders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList']/*" />
    public partial class LayerList<T> :
        IReadOnlyList<ILayerBase>,
        IReadOnlyCollection<ILayerBase>
        where T : class, ILayerBase
    {
        private readonly LogicalTreeList<T> LogicalTree;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.LayerList']/*" />
        public LayerList(LogicalTreeList<T> logicalTree)
        {
            this.LogicalTree = logicalTree;
        }

        /// <inheritdoc cref="IReadOnlyList{T}.this"/>
        public ILayerBase this[int index] => this.LogicalTree[index];

        /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
        public int Count => this.LogicalTree.Count;

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => this.LogicalTree.GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<ILayerBase> GetEnumerator() => this.LogicalTree.GetEnumerator();

        #region ILayerList

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.IndexerOf']/*" />
        public SelectIndexer IndexerOf(T item) => this.LogicalTree.IndexerOf(item);

        //------------------------ Range ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.IndexRangeOf']/*" />
        public IndexSelection IndexRangeOf(T item)
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    return IndexSelection.Empty;
                case 1:
                    return new IndexSelection(SelTyp3.AS);
                default:
                    int index = this.LogicalTree.IndexOf(item);
                    return new IndexSelection(this.LogicalTree, item.Depth, index);
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.IndexRangeOf2']/*" />
        public IndexRange IndexRangeOf(T newItem, SelectIndexer oldIndexer) => this.LogicalTree.IndexRangeOf(newItem, oldIndexer);

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.IndexRangeOf3']/*" />
        public IndexRange IndexRangeOf(SelectIndexer newIndexer, SelectIndexer oldIndexer) => this.LogicalTree.IndexRangeOf(newIndexer, oldIndexer);

        //------------------------ Ranges ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetSelectedRanges']/*" />
        public IndexRange[] GetSelectedRanges() => this.LogicalTree.GetSelectedRanges().ToArray();

        //------------------------ IDs ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetIds']/*" />
        public Guid[] GetIds() => this.LogicalTree.GetIds();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetNodes']/*" />
        public XmlTreeNode[] GetNodes() => this.LogicalTree.GetNodes();

        //------------------------ Reset ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.ResetByList']/*" />
        public void ResetByList(IEnumerable<T> list)
        {
            this.LogicalTree.Clear();
            foreach (T item in list)
            {
                this.LogicalTree.Add(item);
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.ResetByTree']/*" />
        public void ResetByTree<C>(IEnumerable<C> tree)
            where C : class, IComposite<C>, T
        {
            this.LogicalTree.Clear();

            this.AddRecursion(tree, 0);
        }

        private void AddRecursion<C>(IEnumerable<C> children, int depth)
            where C : class, IComposite<C>, T
        {
            foreach (C item in children)
            {
                item.Depth = depth;
                this.LogicalTree.Add(item);

                if (item.Children != null)
                {
                    this.AddRecursion(item.Children, depth + 1);
                }
            }
        }

        #endregion

        #region ILayerList2

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.Deselect']/*" />
        public SelectChange[] Deselect(T item) => this.LogicalTree.Deselect(item).ToArray();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.Deselect2']/*" />
        public SelectChange[] Deselect(SelectIndexer indexer) => this.LogicalTree.Deselect(indexer).ToArray();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.Select']/*" />
        public SelectChange[] Select(T item) => this.LogicalTree.Select(item).ToArray();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.Select2']/*" />
        public SelectChange[] Select(SelectIndexer indexer) => this.LogicalTree.Select(indexer).ToArray();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.SelectOnly']/*" />
        public SelectChange[] SelectOnly(T item) => this.LogicalTree.SelectOnly(item).ToArray();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.SelectOnly2']/*" />
        public SelectChange[] SelectOnly(SelectIndexer indexer) => this.LogicalTree.SelectOnly(indexer).ToArray();

        //------------------------ Range ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.SelectRangeOnly']/*" />
        public SelectChange[] SelectRangeOnly(IndexRange range) => this.LogicalTree.SelectRangeOnly(range).ToArray();

        #endregion

        #region ILayerList3

        // LayerLogicalTree.List.cs
        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.DeselectAll']/*" />
        public SelectChange[] DeselectAll() => this.LogicalTree.DeselectAll().ToArray();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.SelectAll']/*" />
        public SelectChange[] SelectAll() => this.LogicalTree.SelectAll().ToArray();

        //------------------------ Visibility ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.HideAll']/*" />
        public BooleanChange[] HideAll() => this.LogicalTree.HideAll().ToArray();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.ShowAll']/*" />
        public BooleanChange[] ShowAll() => this.LogicalTree.ShowAll().ToArray();

        #endregion

        #region ILayerList4

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.CanRelease']/*" />
        public bool CanRelease(IndexSelection selection) => !selection.IsEmpty && this.LogicalTree.CanRelease();

        //------------------------ Ungroup ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetDepthsForUngroupMultiple']/*" />
        public Int32Change[] GetDepthsForUngroupMultiple(Ungrouper ungrouper)
        {
            switch (ungrouper.Mode)
            {
                case UngroupMode.None:
                    return default;
                case UngroupMode.SingleAtLast:
                case UngroupMode.SingleWithoutChild:
                    return default;
                case UngroupMode.SingleRangeExpand:
                case UngroupMode.SingleRangeUnexpand:
                    IndexRange selectedRange = ungrouper.Source.Source;
                    return this.LogicalTree[selectedRange].Select(ChangesExtensions.Decrease).ToArray();
                case UngroupMode.MultipleRanges:
                    return this.LogicalTree.DepthUngroup().ToArray();
                default:
                    return default;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetSelectsForUngroupMultiple']/*" />
        public SelectChange[] GetSelectsForUngroupMultiple(Ungrouper ungrouper)
        {
            switch (ungrouper.Mode)
            {
                case UngroupMode.None:
                    return default;
                case UngroupMode.SingleAtLast:
                case UngroupMode.SingleWithoutChild:
                    return default;
                case UngroupMode.SingleRangeExpand:
                case UngroupMode.SingleRangeUnexpand:
                    IndexRange selectedRange = ungrouper.Source.Source;
                    return this.LogicalTree.DeselectChildren(selectedRange).ToArray();
                case UngroupMode.MultipleRanges:
                    return this.LogicalTree.DeselectAllGroupChild().ToArray();
                default:
                    return default;
            }
        }

        //------------------------ Group ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetDepthsForGroupMultiple']/*" />
        public Int32Change[] GetDepthsForGroupMultiple(Grouper grouper) // DepthGroupMultiple
        {
            switch (grouper.Source.Source.ToSelTyp2(this.LogicalTree.Count))
            {
                case SelTyp2.Non:
                    return default;
                case SelTyp2.S:
                    return default;
                case SelTyp2.SR:
                    IndexRange selectedRange = grouper.Source.Source.Source;
                    return this.LogicalTree[selectedRange].Select(ChangesExtensions.Increase).ToArray();
                case SelTyp2.M:
                    return this.LogicalTree.DepthGroup(grouper.Depth + 1).ToArray();
                case SelTyp2.AS:
                case SelTyp2.ASR:
                case SelTyp2.AM:
                    return this.LogicalTree.Select(ChangesExtensions.Increase).ToArray();
                default:
                    return default;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetSelectsForGroupMultiple']/*" />
        public SelectChange[] GetSelectsForGroupMultiple(Grouper grouper)
        {
            switch (grouper.Source.Source.ToSelTyp2(this.LogicalTree.Count))
            {
                case SelTyp2.Non:
                    return default;
                case SelTyp2.S:
                    return default;
                case SelTyp2.SR:
                    IndexRange selectedRange = grouper.Source.Source.Source;
                    return this.LogicalTree.DeselectParentSelect(selectedRange).ToArray();
                case SelTyp2.M:
                    return this.LogicalTree.DeselectParentSelectAllDeselect().ToArray();
                case SelTyp2.AS:
                case SelTyp2.ASR:
                case SelTyp2.AM:
                    return this.LogicalTree.DeselectParentSelectAll().ToArray();
                default:
                    return default;
            }
        }

        //------------------------ Release ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetDepthsForRelease']/*" />
        public Int32Change[] GetDepthsForRelease() => this.LogicalTree.DepthRelease().ToArray(); // GetReleaseDepths

        //------------------------ Package ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetDepthsForPackage']/*" />
        public Int32Change[] GetDepthsForPackage() => this.LogicalTree.Select(ChangesExtensions.Increase).ToArray(); // DepthPackage

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetSelectsForPackage']/*" />
        public SelectChange[] GetSelectsForPackage() => this.LogicalTree.DeselectParentSelectAll().ToArray();

        #endregion

        #region ILayerList5

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetDepthsForReorderMultiple']/*" />
        public Int32Change[] GetDepthsForReorderMultiple(Reorder reorder) // DepthReorderSingleRange
        {
            switch (reorder.Location)
            {
                case ReoLoc.Non:
                    return null;
                case ReoLoc.SRX:
                    if (reorder.DropDepth is 0)
                        return this.LogicalTree.ResetDepths(reorder.RemoveRange).ToArray();
                    else
                        return this.LogicalTree.ResetDepths(reorder.RemoveRange, reorder.DropDepth).ToArray();
                case ReoLoc.SRL:
                case ReoLoc.SRF:
                    return this.LogicalTree.ResetDepths(reorder.RemoveRange).ToArray();
                default:
                    return null;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerList.GetDepthsForReorderMultiple2']/*" />
        public Int32Change[] GetDepthsForReorderMultiple(Reorder reorder, IndexRange[] selectedRanges) // DepthReorderMultiple
        {
            switch (reorder.Location)
            {
                case ReoLoc.Non:
                    return null;
                case ReoLoc.MX:
                    switch (reorder.DropDepth)
                    {
                        case 0:
                            return this.LogicalTree.ResetDepths(selectedRanges).ToArray();
                        default:
                            return this.LogicalTree.ResetDepths(selectedRanges, reorder.DropDepth).ToArray();
                    }
                case ReoLoc.ML:
                    return this.LogicalTree.ResetDepths(selectedRanges).ToArray();
                case ReoLoc.MF:
                    return this.LogicalTree.ResetDepths(selectedRanges).ToArray();
                default:
                    return null;
            }
        }

        #endregion

        public override string ToString() => this.LogicalTree.ToString();
    }
}