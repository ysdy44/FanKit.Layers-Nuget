using FanKit.Layers.Changes;
using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using FanKit.Layers.Reorders;
using System.Linq;

namespace FanKit.Layers
{
    partial class LayerCollection<T>
    {
        #region Reorder

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.CanReorderItems']/*" />
        public bool CanReorderItems(DropIndexer indexer) => this.CanReorderItems(indexer, new IndexSelection(this));

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.CanReorderItems2']/*" />
        public bool CanReorderItems(DropIndexer indexer, IndexSelection selection)
        {
            switch (indexer.Placement)
            {
                case DropPlacement.None: return false;
                case DropPlacement.InsertAtTop: return MoveTo.Non != Mover.BringToFront.To(this.LogicalTree, selection);
                case DropPlacement.InsertAtBottom: return MoveTo.Non != Mover.SendToBack.To(this.LogicalTree, selection);
                case DropPlacement.InsertAbove: return MoveTo.Non != Mover.MoveBefore(indexer.Index).To(this.LogicalTree, selection);
                case DropPlacement.InsertBelow: return MoveTo.Non != Mover.MoveAfter(indexer.Index).To(this.LogicalTree, selection);
                default: return false;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.CanArrange']/*" />
        public bool CanArrange(ArrangeType type) => this.CanArrange(type, new IndexSelection(this));

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.CanArrange2']/*" />
        public bool CanArrange(ArrangeType type, IndexSelection selection)
        {
            switch (type)
            {
                case ArrangeType.BringToFront: return MoveTo.Non != Mover.BringToFront.To(this.LogicalTree, selection);
                case ArrangeType.SendToBack: return MoveTo.Non != Mover.SendToBack.To(this.LogicalTree, selection);
                case ArrangeType.BringForward: return MoveTo.Non != Mover.BringForward.To(this.LogicalTree, selection);
                case ArrangeType.SendBackward: return MoveTo.Non != Mover.SendBackward.To(this.LogicalTree, selection);
                default: return false;
            }
        }

        #endregion

        #region Move

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.MoveAboveSibling']/*" />
        public InvalidateModes MoveAboveSibling(Reorder reorder)
        {
            switch (reorder.Location)
            {
                case ReoLoc.Non:
                    return default;
                case ReoLoc.SSX:
                    this.LogicalTree.RemoveAt(reorder.RemoveIndex);
                    this.LogicalTree.Insert(reorder.InsertIndex, this.Pool[reorder.DepthOfSingle.Id]);

                    this.ApplyDepth(reorder.DepthOfSingle);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SSL:
                    this.LogicalTree.RemoveAt(reorder.RemoveIndex);
                    this.LogicalTree.Add(this.Pool[reorder.DepthOfSingle.Id]);

                    this.ApplyDepth(reorder.DepthOfSingle);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SSF:
                    this.LogicalTree.RemoveAt(reorder.RemoveIndex);
                    this.LogicalTree.Insert(0, this.Pool[reorder.DepthOfSingle.Id]);

                    this.ApplyDepth(reorder.DepthOfSingle);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SSRX:
                    this.LogicalTree.MoveRange(reorder.RemoveRange, reorder.InsertIndex);

                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SSRL:
                    this.LogicalTree.MoveRangeToLast(reorder.RemoveRange);

                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SSRF:
                    this.LogicalTree.MoveRangeToFirst(reorder.RemoveRange);

                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;

                case ReoLoc.NSM:
                case ReoLoc.PSM:
                    IndexRange[] selectedRanges = this.LogicalTree.GetSelectedRanges().ToArray();

                    MoveDirect direct =
                        reorder.Location == ReoLoc.PSM ?
                        MoveDirect.Previous :
                        MoveDirect.Next;

                    foreach (IndexRange selectedRange in selectedRanges)
                    {
                        switch (selectedRange.Length)
                        {
                            case 0:
                                break;
                            case 1:
                                {
                                    MoveIndex move = new MoveIndex(this.LogicalTree, selectedRange.StartIndex, direct, -1);

                                    switch (move.Location)
                                    {
                                        case IdxLoc.Non:
                                            break;
                                        case IdxLoc.Xof:
                                            this.LogicalTree.RemoveAt(move.RemoveIndex);
                                            this.LogicalTree.Insert(move.InsertIndex, this.Pool[move.Depth.Id]);
                                            break;
                                        case IdxLoc.Lst:
                                            this.LogicalTree.RemoveAt(move.RemoveIndex);
                                            this.LogicalTree.Add(this.Pool[move.Depth.Id]);
                                            break;
                                        case IdxLoc.Fst:
                                            this.LogicalTree.RemoveAt(move.RemoveIndex);
                                            this.LogicalTree.Insert(0, this.Pool[move.Depth.Id]);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                {
                                    MoveRange move = new MoveRange(this.LogicalTree, selectedRange, direct, -1);

                                    switch (move.Location)
                                    {
                                        case IdxLoc.Non:
                                            break;
                                        case IdxLoc.Xof:
                                            this.LogicalTree.MoveRange(move.RemoveRange, move.InsertIndex);
                                            break;
                                        case IdxLoc.Lst:
                                            this.LogicalTree.MoveRangeToLast(move.RemoveRange);
                                            break;
                                        case IdxLoc.Fst:
                                            this.LogicalTree.MoveRangeToFirst(move.RemoveRange);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        #endregion

        #region Reorder

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ReorderSingle']/*" />
        public InvalidateModes ReorderSingle(Reorder reorder)
        {
            switch (reorder.Location)
            {
                case ReoLoc.Non:
                    return default;
                case ReoLoc.SX:
                    this.LogicalTree.RemoveAt(reorder.RemoveIndex);
                    this.LogicalTree.Insert(reorder.InsertIndex, this.Pool[reorder.DepthOfSingle.Id]);

                    this.ApplyDepth(reorder.DepthOfSingle);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SL:
                    this.LogicalTree.RemoveAt(reorder.RemoveIndex);
                    this.LogicalTree.Add(this.Pool[reorder.DepthOfSingle.Id]);

                    this.ApplyDepth(reorder.DepthOfSingle);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SF:
                    this.LogicalTree.RemoveAt(reorder.RemoveIndex);
                    this.LogicalTree.Insert(0, this.Pool[reorder.DepthOfSingle.Id]);

                    this.ApplyDepth(reorder.DepthOfSingle);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ReorderSingleRange']/*" />
        public InvalidateModes ReorderSingleRange(Reorder reorder, Int32Change[] depths)
        {
            switch (reorder.Location)
            {
                case ReoLoc.Non:
                    return default;
                case ReoLoc.SRX:
                    this.LogicalTree.MoveRange(reorder.RemoveRange, reorder.InsertIndex);

                    this.ApplyDepths(depths);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SRL:
                    this.LogicalTree.MoveRangeToLast(reorder.RemoveRange);

                    this.ApplyDepths(depths);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.SRF:
                    this.LogicalTree.MoveRangeToFirst(reorder.RemoveRange);

                    this.ApplyDepths(depths);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ReorderMultiple']/*" />
        public InvalidateModes ReorderMultiple(Reorder reorder, Int32Change[] depths, IndexRange[] selectedRanges)
        {
            switch (reorder.Location)
            {
                case ReoLoc.Non:
                    return default;
                case ReoLoc.MX:
                    T drop = this.Pool[reorder.Id];
                    int descendant = reorder.GetDescendantCount();

                    this.LogicalTree.MoveRanges(selectedRanges, drop, descendant);

                    this.ApplyDepths(depths);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.ML:
                    this.LogicalTree.MoveRangesToLast(selectedRanges);

                    this.ApplyDepths(depths);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case ReoLoc.MF:
                    this.LogicalTree.MoveRangesToFirst(selectedRanges);

                    this.ApplyDepths(depths);
                    this.AssignChild2();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        #endregion
    }
}