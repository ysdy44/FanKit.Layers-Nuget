using FanKit.Layers.Changes;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Reorders
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct MoveIndex
    {
        public readonly IdxLoc Location;
        public readonly Int32Change Depth;

        public readonly int RemoveIndex;
        public readonly int InsertIndex;

        public ReoLoc ToLocation(MoveTo to)
        {
            switch (to)
            {
                case MoveTo.SN:
                case MoveTo.SP:
                    switch (this.Location)
                    {
                        case IdxLoc.Non: return ReoLoc.Non;
                        case IdxLoc.Xof: return ReoLoc.SSX;
                        case IdxLoc.Lst: return ReoLoc.SSL;
                        case IdxLoc.Fst: return ReoLoc.SSF;
                        default: return ReoLoc.Non;
                    }

                case MoveTo.SA:
                case MoveTo.SB:

                case MoveTo.SL:
                case MoveTo.SF:
                    if (this.Depth.IsEmpty)
                    {
                        switch (this.Location)
                        {
                            case IdxLoc.Non: return ReoLoc.Non;
                            case IdxLoc.Xof: return ReoLoc.SSX;
                            case IdxLoc.Lst: return ReoLoc.SSL;
                            case IdxLoc.Fst: return ReoLoc.SSF;
                            default: return ReoLoc.Non;
                        }
                    }
                    else
                    {
                        switch (this.Location)
                        {
                            case IdxLoc.Non: return ReoLoc.Non;
                            case IdxLoc.Xof: return ReoLoc.SX;
                            case IdxLoc.Lst: return ReoLoc.SL;
                            case IdxLoc.Fst: return ReoLoc.SF;
                            default: return ReoLoc.Non;
                        }
                    }

                default:
                    return ReoLoc.Non;
            }
        }

        public MoveIndex(IReadOnlyList<ILayerBase> items, int dragIndex, MoveDirect direct, int index)
        {
            switch (direct)
            {
                case MoveDirect.Next:
                    {
                        ILayerBase drag = items[dragIndex];

                        int indexNext = dragIndex + 1;
                        ILayerBase itemNext = items[indexNext];

                        // None
                        if (itemNext.Depth < drag.Depth)
                        {
                            this.Location = IdxLoc.Non;
                            this.Depth = default;
                            this.RemoveIndex = -1;
                            this.InsertIndex = -1;
                            break;
                        }

                        for (int i = dragIndex + 1 + 1; i < items.Count; i++)
                        {
                            ILayerBase item = items[i];

                            if (item.Depth <= drag.Depth)
                            {
                                int dropIndex = i - 1;

                                this.Location = IdxLoc.Xof;
                                this.Depth = drag.Empty();
                                this.RemoveIndex = dragIndex;
                                this.InsertIndex = dropIndex;
                                return;
                            }
                        }

                        this.Location = IdxLoc.Lst;
                        this.Depth = new Int32Change
                        {
                            Id = drag.Id,
                        };
                        this.RemoveIndex = dragIndex;
                        this.InsertIndex = -1;
                        break;
                    }
                case MoveDirect.Previous:
                    {
                        int dropIndex = dragIndex - 1;

                        ILayerBase drag = items[dragIndex];

                        for (int i = dropIndex; i > 0; i--)
                        {
                            ILayerBase drop = items[i];

                            if (drop.Depth < drag.Depth)
                            {
                                this.Location = IdxLoc.Non;
                                this.Depth = default;
                                this.RemoveIndex = -1;
                                this.InsertIndex = -1;
                                return;
                            }
                            else if (drop.Depth == drag.Depth)
                            {
                                this.Location = IdxLoc.Xof;
                                this.Depth = drag.Empty();
                                this.RemoveIndex = dragIndex;
                                this.InsertIndex = i;
                                return;
                            }
                        }

                        const int indexTop = 0;
                        ILayerBase dropTop = items[indexTop];

                        if (dropTop.Depth == drag.Depth)
                        {
                            this.Location = IdxLoc.Fst;
                            this.Depth = drag.Empty();
                            this.RemoveIndex = dragIndex;
                            this.InsertIndex = -1;
                        }
                        else
                        {
                            this.Location = IdxLoc.Non;
                            this.Depth = default;
                            this.RemoveIndex = -1;
                            this.InsertIndex = -1;
                        }
                        break;
                    }
                case MoveDirect.Before:
                    {
                        ILayerBase drag = items[dragIndex];

                        ILayerBase drop = items[index];

                        this.Location = IdxLoc.Xof;
                        this.Depth = drag.Depth(drop.Depth);

                        this.RemoveIndex = dragIndex;

                        int dropIndex = index;
                        if (dropIndex < dragIndex)
                            this.InsertIndex = dropIndex;
                        else
                            this.InsertIndex = dropIndex - 1;
                    }
                    break;
                case MoveDirect.After:
                    {
                        ILayerBase drag = items[dragIndex];

                        ILayerBase drop = items[index];

                        this.Location = IdxLoc.Xof;
                        this.Depth = drag.Depth(drop.Depth);

                        this.RemoveIndex = dragIndex;

                        int dropIndex = drop.Settings.GetDescendantCount(drop) + index;
                        if (dropIndex < dragIndex)
                            this.InsertIndex = dropIndex + 1;
                        else
                            this.InsertIndex = dropIndex; // + 1 - 1
                    }
                    break;
                case MoveDirect.First:
                    {
                        ILayerBase drag = items[dragIndex];

                        this.Location = IdxLoc.Fst;
                        this.Depth = drag.Depth(0);

                        this.RemoveIndex = dragIndex;
                        this.InsertIndex = -1;
                    }
                    break;
                case MoveDirect.Last:
                    {
                        ILayerBase drag = items[dragIndex];

                        this.Location = IdxLoc.Lst;
                        this.Depth = drag.Depth(0);

                        this.RemoveIndex = dragIndex;
                        this.InsertIndex = -1;
                    }
                    break;
                default:
                    this.Location = IdxLoc.Non;
                    this.Depth = default;

                    this.RemoveIndex = -1;
                    this.InsertIndex = -1;
                    break;
            }
        }
    }
}