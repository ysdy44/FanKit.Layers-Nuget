using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.DragDrop
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Dropper']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Dropper
    {
        internal readonly IdxLoc Location;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Dropper.Depth']/*" />
        public readonly int Depth;

        internal readonly int Index;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Dropper.Dropper']/*" />
        public Dropper(IReadOnlyList<ILayerBase> items, DropIndexer indexer)
        {
            switch (indexer.Placement)
            {
                case DropPlacement.None:
                    this.Location = IdxLoc.Non;
                    this.Depth = 0;
                    this.Index = -1;
                    break;
                case DropPlacement.InsertAtTop:
                    this.Location = IdxLoc.Fst;
                    this.Depth = 0;
                    this.Index = -1;
                    break;
                case DropPlacement.InsertAtBottom:
                    this.Location = IdxLoc.Lst;
                    this.Depth = 0;
                    this.Index = -1;
                    break;
                case DropPlacement.InsertAbove:
                    {
                        ILayerBase drop = items[indexer.Index];

                        this.Location = IdxLoc.Xof;
                        this.Depth = drop.Depth;
                        this.Index = indexer.Index;
                    }
                    break;
                case DropPlacement.InsertBelow:
                    {
                        ILayerBase drop = items[indexer.Index];

                        this.Location = IdxLoc.Xof;
                        this.Depth = drop.Depth;
                        this.Index = drop.Settings.GetDescendantCount(drop) + indexer.Index + 1;
                    }
                    break;
                default:
                    this.Location = IdxLoc.Non;
                    this.Depth = 0;
                    this.Index = -1;
                    break;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Dropper: Placement = {this.Location}, Index = {this.Index}, Depth = {this.Depth}]";
    }
}