using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Reorders
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct MoveRange
    {
        public readonly IdxLoc Location;
        public readonly int DropDepth;

        public readonly IndexRange RemoveRange;
        public readonly int InsertIndex;

        public ReoLoc ToLocation(MoveTo to)
        {
            switch (to)
            {
                case MoveTo.SRN:
                case MoveTo.SRP:
                    switch (this.Location)
                    {
                        case IdxLoc.Non: return ReoLoc.Non;
                        case IdxLoc.Xof: return ReoLoc.SSRX;
                        case IdxLoc.Lst: return ReoLoc.SSRL;
                        case IdxLoc.Fst: return ReoLoc.SSRF;
                        default: return ReoLoc.Non;
                    }

                case MoveTo.SRA:
                case MoveTo.SRB:

                case MoveTo.SRL:
                case MoveTo.SRF:
                    switch (this.Location)
                    {
                        case IdxLoc.Non: return ReoLoc.Non;
                        case IdxLoc.Xof: return ReoLoc.SRX;
                        case IdxLoc.Lst: return ReoLoc.SRL;
                        case IdxLoc.Fst: return ReoLoc.SRF;
                        default: return ReoLoc.Non;
                    }

                default: return ReoLoc.Non;
            }
        }

        public MoveRange(IReadOnlyList<ILayerBase> items, IndexRange dragRange, MoveDirect direct, int index)
        {
            switch (direct)
            {
                case MoveDirect.Next:
                    {
                        ILayerBase drag = items[dragRange.StartIndex];

                        int indexNext = dragRange.EndIndex + 1;
                        ILayerBase itemNext = items[indexNext];

                        if (itemNext.Depth < drag.Depth)
                        {
                            this.Location = IdxLoc.Non;
                            this.DropDepth = default;
                            this.RemoveRange = IndexRange.NegativeUnit;
                            this.InsertIndex = -1;
                            break;
                        }

                        for (int i = dragRange.EndIndex + 1 + 1; i < items.Count; i++)
                        {
                            ILayerBase item = items[i];

                            if (item.Depth <= drag.Depth)
                            {
                                int dropIndex = i - dragRange.Length;

                                this.Location = IdxLoc.Xof;
                                this.DropDepth = drag.Depth;
                                this.RemoveRange = dragRange;
                                this.InsertIndex = dropIndex;
                                return;
                            }
                        }

                        this.Location = IdxLoc.Lst;
                        this.DropDepth = 0;
                        this.RemoveRange = dragRange;
                        this.InsertIndex = -1;
                        break;
                    }
                case MoveDirect.Previous:
                    {
                        int dropIndex = dragRange.StartIndex - 1;

                        ILayerBase drag = items[dragRange.StartIndex];

                        for (int i = dropIndex; i > 0; i--)
                        {
                            ILayerBase drop = items[i];

                            if (drop.Depth < drag.Depth)
                            {
                                this.Location = IdxLoc.Non;
                                this.DropDepth = default;
                                this.RemoveRange = IndexRange.NegativeUnit;
                                this.InsertIndex = -1;
                                return;
                            }
                            else if (drop.Depth == drag.Depth)
                            {
                                this.Location = IdxLoc.Xof;
                                this.DropDepth = drag.Depth;
                                this.RemoveRange = dragRange;
                                this.InsertIndex = i;
                                return;
                            }
                        }

                        const int indexTop = 0;
                        ILayerBase dropTop = items[indexTop];

                        if (dropTop.Depth == drag.Depth)
                        {
                            this.Location = IdxLoc.Fst;
                            this.DropDepth = drag.Depth;
                            this.RemoveRange = dragRange;
                            this.InsertIndex = -1;
                        }
                        else
                        {
                            this.Location = IdxLoc.Non;
                            this.DropDepth = default;
                            this.RemoveRange = IndexRange.NegativeUnit;
                            this.InsertIndex = -1;
                        }
                        break;
                    }
                case MoveDirect.Before:
                    {
                        ILayerBase drag = items[dragRange.StartIndex];

                        ILayerBase drop = items[index];

                        this.Location = IdxLoc.Xof;
                        this.DropDepth = drop.Depth;

                        this.RemoveRange = dragRange;

                        int dropIndex = index;
                        if (dropIndex < dragRange.StartIndex)
                            this.InsertIndex = dropIndex;
                        else
                            this.InsertIndex = dropIndex - dragRange.Length;
                    }
                    break;
                case MoveDirect.After:
                    {
                        ILayerBase drag = items[dragRange.StartIndex];

                        ILayerBase drop = items[index];

                        this.Location = IdxLoc.Xof;
                        this.DropDepth = drop.Depth;

                        this.RemoveRange = dragRange;

                        int dropIndex = drop.Settings.GetDescendantCount(drop) + index;
                        if (dropIndex < dragRange.StartIndex)
                            this.InsertIndex = dropIndex + 1;
                        else
                            this.InsertIndex = dropIndex + 1 - dragRange.Length;
                    }
                    break;
                case MoveDirect.First:
                    this.Location = IdxLoc.Fst;
                    this.DropDepth = 0;
                    this.RemoveRange = dragRange;
                    this.InsertIndex = -1;
                    break;
                case MoveDirect.Last:
                    this.Location = IdxLoc.Lst;
                    this.DropDepth = 0;
                    this.RemoveRange = dragRange;
                    this.InsertIndex = -1;
                    break;
                default:
                    this.Location = IdxLoc.Non;
                    this.DropDepth = 0;
                    this.RemoveRange = IndexRange.NegativeUnit;
                    this.InsertIndex = -1;
                    break;
            }
        }
    }
}