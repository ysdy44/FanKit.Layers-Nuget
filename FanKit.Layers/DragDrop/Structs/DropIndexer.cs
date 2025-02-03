using System.Runtime.InteropServices;

namespace FanKit.Layers.DragDrop
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='DropIndexer']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct DropIndexer
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='DropIndexer.Placement']/*" />
        public readonly DropPlacement Placement;
        /// <include file="doc/docs.xml" path="docs/doc[@for='DropIndexer.Index']/*" />
        public readonly int Index;

        internal static DropIndexer Empty { get; } = new DropIndexer(DropPlacement.None, -1);

        internal static DropIndexer InsertAtTop { get; } = new DropIndexer(DropPlacement.InsertAtTop, -1);
        internal static DropIndexer InsertAtBottom { get; } = new DropIndexer(DropPlacement.InsertAtBottom, -1);

        internal static DropIndexer InsertAbove(int index) => new DropIndexer(DropPlacement.InsertAbove, index);
        internal static DropIndexer InsertBelow(int index) => new DropIndexer(DropPlacement.InsertBelow, index);

        private DropIndexer(DropPlacement placement, int index)
        {
            this.Placement = placement;
            this.Index = index;
        }

        /// <inheritdoc/>
        public override string ToString() => $"[DropIndexer: Placement = {this.Placement}, Index = {this.Index}]";
    }
}