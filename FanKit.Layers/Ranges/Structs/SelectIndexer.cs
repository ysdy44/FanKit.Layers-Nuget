using System.Runtime.InteropServices;

namespace FanKit.Layers.Ranges
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='SelectIndexer']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SelectIndexer
    {
        internal readonly int Index;

        internal readonly System.Guid Id;

        internal readonly int Depth;

        internal readonly SelectMode SelectMode;

        internal SelectIndexer(ILayerBase layer, int index)
        {
            this.Id = layer.Id;
            this.Depth = layer.Depth;
            this.SelectMode = layer.SelectMode;

            this.Index = index;
        }

        /// <inheritdoc/>
        public override string ToString() => $"[SelectIndexer: Index = {this.Index}, Id = {this.Id}, Depth = {this.Depth}, SelectMode = {this.SelectMode}]";
    }
}