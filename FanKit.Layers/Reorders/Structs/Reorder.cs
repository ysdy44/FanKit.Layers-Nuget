using FanKit.Layers.Changes;
using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using FanKit.Layers.Reorders;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Reorder
    {
        internal readonly ReoLoc Location;

        internal readonly int InsertIndex;
        internal readonly int DropDepth;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder.DepthOfSingle']/*" />
        public readonly Int32Change DepthOfSingle;
        internal readonly int RemoveIndex;

        internal readonly IndexRange RemoveRange;

        internal readonly Guid Id;
        internal readonly int DescendantCount;

        private bool IsBelow => this.DescendantCount > -1;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder.IsSibling']/*" />
        public bool IsSibling
        {
            get
            {
                switch (this.Location)
                {
                    case ReoLoc.SSX:
                    case ReoLoc.SSL:
                    case ReoLoc.SSF:
                    case ReoLoc.SSRX:
                    case ReoLoc.SSRL:
                    case ReoLoc.SSRF:
                    case ReoLoc.NSM:
                    case ReoLoc.PSM: return true;
                    default: return false;
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder.Count']/*" />
        public ReorderCount Count
        {
            get
            {
                switch (this.Location)
                {
                    case ReoLoc.Non: return ReorderCount.None;

                    case ReoLoc.SX:
                    case ReoLoc.SL:
                    case ReoLoc.SF: return ReorderCount.Single;

                    case ReoLoc.SRX:
                    case ReoLoc.SRL:
                    case ReoLoc.SRF: return ReorderCount.SingleRange;

                    case ReoLoc.MX:
                    case ReoLoc.ML:
                    case ReoLoc.MF: return ReorderCount.Multiple;

                    default: return ReorderCount.None;
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder.ReorderDrop']/*" />
        public Reorder(IReadOnlyList<ILayerBase> items, DropIndexer indexer)
            : this(items, ToMover(indexer), new IndexSelection(items))
        {
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder.ReorderDropSelection']/*" />
        public Reorder(IReadOnlyList<ILayerBase> items, DropIndexer indexer, IndexSelection selection)
            : this(items, ToMover(indexer), selection)
        {
        }

        private static Mover ToMover(DropIndexer indexer)
        {
            switch (indexer.Placement)
            {
                case DropPlacement.None: return default;
                case DropPlacement.InsertAtTop: return Mover.BringToFront;
                case DropPlacement.InsertAtBottom: return Mover.SendToBack;
                case DropPlacement.InsertAbove: return Mover.MoveBefore(indexer.Index);
                case DropPlacement.InsertBelow: return Mover.MoveAfter(indexer.Index);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder.ReorderArrange']/*" />
        public Reorder(IReadOnlyList<ILayerBase> items, ArrangeType type)
            : this(items, ToMover(type), new IndexSelection(items))
        {
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Reorder.ReorderArrangeSelection']/*" />
        public Reorder(IReadOnlyList<ILayerBase> items, ArrangeType type, IndexSelection selection)
            : this(items, ToMover(type), selection)
        {
        }

        private static Mover ToMover(ArrangeType type)
        {
            switch (type)
            {
                case ArrangeType.BringToFront: return Mover.BringToFront;
                case ArrangeType.SendToBack: return Mover.SendToBack;
                case ArrangeType.BringForward: return Mover.BringForward;
                case ArrangeType.SendBackward: return Mover.SendBackward;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        internal Reorder(IReadOnlyList<ILayerBase> items, Mover mover, IndexSelection selection)
        {
            MoveTo to = mover.To(items, selection);

            switch (to)
            {
                case MoveTo.Non:
                    #region Default value
                    this.Location = ReoLoc.Non;

                    this.InsertIndex = -1;
                    this.DropDepth = -1;
                    this.DepthOfSingle = default;
                    this.RemoveIndex = -1;
                    this.RemoveRange = IndexRange.NegativeUnit;
                    this.Id = Guid.Empty;
                    this.DescendantCount = -1;
                    #endregion
                    break;
                case MoveTo.SN:
                case MoveTo.SP:

                case MoveTo.SA:
                case MoveTo.SB:

                case MoveTo.SL:
                case MoveTo.SF:
                    {
                        MoveIndex move = new MoveIndex(items, selection.Source.StartIndex, mover.Direct, mover.Index);

                        if (move.Location == IdxLoc.Non)
                            goto case MoveTo.Non;
                        else
                        {
                            #region Default value
                            this.RemoveRange = IndexRange.NegativeUnit;
                            this.Id = Guid.Empty;
                            this.DescendantCount = -1;
                            #endregion

                            this.Location = move.ToLocation(to);

                            this.InsertIndex = move.InsertIndex;
                            this.DropDepth = -1;
                            this.DepthOfSingle = move.Depth;
                            this.RemoveIndex = move.RemoveIndex;
                            break;
                        }
                    }

                case MoveTo.SRN:
                case MoveTo.SRP:

                case MoveTo.SRA:
                case MoveTo.SRB:

                case MoveTo.SRL:
                case MoveTo.SRF:
                    {
                        MoveRange move = new MoveRange(items, selection.Source, mover.Direct, mover.Index);

                        if (move.Location == IdxLoc.Non)
                            goto case MoveTo.Non;
                        else
                        {
                            #region Default value
                            this.DepthOfSingle = default;
                            this.RemoveIndex = -1;
                            this.Id = Guid.Empty;
                            this.DescendantCount = -1;
                            #endregion

                            this.Location = move.ToLocation(to);

                            this.InsertIndex = move.InsertIndex;
                            this.DropDepth = move.DropDepth;
                            this.RemoveRange = move.RemoveRange;
                            break;
                        }
                    }

                case MoveTo.MN:
                    {
                        #region Default value
                        this.InsertIndex = -1;
                        this.DropDepth = -1;
                        this.DepthOfSingle = default;
                        this.RemoveIndex = -1;
                        this.RemoveRange = IndexRange.NegativeUnit;
                        this.Id = Guid.Empty;
                        this.DescendantCount = -1;
                        #endregion

                        this.Location = ReoLoc.NSM;
                        break;
                    }
                case MoveTo.MP:
                    {
                        #region Default value
                        this.InsertIndex = -1;
                        this.DropDepth = -1;
                        this.DepthOfSingle = default;
                        this.RemoveIndex = -1;
                        this.RemoveRange = IndexRange.NegativeUnit;
                        this.Id = Guid.Empty;
                        this.DescendantCount = -1;
                        #endregion

                        this.Location = ReoLoc.PSM;
                        break;
                    }

                case MoveTo.MA:
                case MoveTo.MB:

                case MoveTo.ML:
                case MoveTo.MF:
                    #region Default value
                    this.InsertIndex = -1;
                    this.DepthOfSingle = default;
                    this.RemoveIndex = -1;
                    this.RemoveRange = IndexRange.NegativeUnit;
                    #endregion

                    switch (mover.Direct)
                    {
                        case MoveDirect.Before:
                            {
                                ILayerBase drop = items[mover.Index];

                                this.Location = ReoLoc.MX;
                                this.DropDepth = drop.Depth;
                                this.Id = drop.Id;
                                this.DescendantCount = -1;
                            }
                            break;
                        case MoveDirect.After:
                            {
                                ILayerBase drop = items[mover.Index];

                                this.Location = ReoLoc.MX;

                                this.DropDepth = drop.Depth;
                                this.Id = drop.Id;
                                this.DescendantCount = drop.Settings.GetDescendantCount(drop);
                            }
                            break;
                        case MoveDirect.First:
                            this.Location = ReoLoc.MF;

                            this.DropDepth = 0;
                            this.Id = Guid.Empty;
                            this.DescendantCount = -1;
                            break;
                        case MoveDirect.Last:
                            this.Location = ReoLoc.ML;

                            this.DropDepth = 0;
                            // Multiple
                            this.Id = Guid.Empty;
                            this.DescendantCount = -1;
                            break;
                        default:
                            #region Default value
                            this.Location = ReoLoc.Non;

                            this.DropDepth = -1;
                            this.Id = Guid.Empty;
                            this.DescendantCount = -1;
                            #endregion
                            break;
                    }
                    break;

                default:
                    goto case MoveTo.Non;
            }
        }

        internal int GetDescendantCount()
        {
            if (this.IsBelow)
            {
                return this.DescendantCount + 1;
            }
            else
            {
                return 0;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Reorder: MoveType = {this.Location}, InsertIndex = {this.InsertIndex}, DropDepth = {this.DropDepth}, RemoveIndex = {this.RemoveIndex}, RemoveRangeStartIndex = {this.RemoveRange.StartIndex}, RemoveRangeEndIndex = {this.RemoveRange.EndIndex}, Id = {this.Id}, DescendantCount = {this.DescendantCount}]";
    }
}