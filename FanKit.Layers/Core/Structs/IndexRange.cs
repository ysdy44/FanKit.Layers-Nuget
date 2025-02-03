using System.Runtime.InteropServices;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct IndexRange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.NegativeUnit']/*" />
        public static IndexRange NegativeUnit { get; } = new IndexRange(0, -1);
   
        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.PositiveUnit']/*" />
        public static IndexRange PositiveUnit { get; } = new IndexRange(0, 1);
     
        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.Zero']/*" />
        public static IndexRange Zero { get; } = new IndexRange(0, 0);

        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.StartIndex']/*" />
        public readonly int StartIndex;
     
        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.EndIndex']/*" />
        public readonly int EndIndex;

        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.IsNegative']/*" />
        public bool IsNegative => this.EndIndex < this.StartIndex;
    
        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.IsPositive']/*" />
        public bool IsPositive => this.EndIndex > this.StartIndex;
    
        /// <include file="doc/docs.xml" path="docs/doc[@for='IndexRange.IsZero']/*" />
        public bool IsZero => this.EndIndex == this.StartIndex;
     
        internal int Length => 1 + this.EndIndex - this.StartIndex;

        internal IndexRange(int singleIndex)
        {
            this.StartIndex = singleIndex;
            this.EndIndex = singleIndex;
        }

        internal IndexRange(int startIndex, int endIndex)
        {
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
        }

        /// <inheritdoc/>
        public override string ToString() => $"[Ranges: StartIndex = {this.StartIndex}, EndIndex = {this.EndIndex}, Length = {this.Length}]";
    }
}