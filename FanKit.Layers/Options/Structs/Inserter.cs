using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Options
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Inserter']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Inserter
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='Inserter.Index']/*" />
        public readonly int Index;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Inserter.Depth']/*" />
        public readonly int Depth;

        internal readonly bool IsParentExpanded;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Inserter.Placement']/*" />
        public InsertPlacement Placement
        {
            get
            {
                switch (this.Index)
                {
                    case -1:
                    case 0: return InsertPlacement.InsertAtTop;
                    default: return InsertPlacement.InsertAbove;
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Inserter.HasSelected']/*" />
        public bool HasSelected
        {
            get
            {
                switch (this.Index)
                {
                    case -1: return false;
                    case 0:
                    default: return true;
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Inserter.Inserter']/*" />
        public Inserter(IReadOnlyList<ILayerBase> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ILayerBase item = items[i];

                if (item.SelectMode is SelectMode.Deselected)
                    continue;

                this.Index = i;

                switch (item.Depth)
                {
                    case 0:
                        this.Depth = 0;
                        this.IsParentExpanded = true;
                        return;
                    default:
                        int depth = item.Depth;

                        for (int j = i - 1; j >= 0; j--)
                        {
                            ILayerBase jtem = items[j];

                            switch (jtem.Depth)
                            {
                                case 0:
                                    this.Depth = item.Depth;
                                    this.IsParentExpanded = jtem.IsExpanded;
                                    return;
                                default:
                                    if (depth > jtem.Depth)
                                    {
                                        depth = jtem.Depth;

                                        if (jtem.IsExpanded is false)
                                        {
                                            this.Depth = jtem.Depth;
                                            this.IsParentExpanded = false;
                                            return;
                                        }
                                    }
                                    break;
                            }
                        }

                        this.Depth = item.Depth;
                        this.IsParentExpanded = true;
                        return;
                }
            }

            this.Index = -1;
            this.Depth = 0;
            this.IsParentExpanded = false;
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Inserter: Index = {this.Index}, Depth = {this.Depth}]";
    }
}