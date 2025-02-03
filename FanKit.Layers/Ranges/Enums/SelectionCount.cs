namespace FanKit.Layers.Ranges
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='SelectionCount']/*" />
    public enum SelectionCount : byte
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='SelectionCount.None']/*" />
        None = Constants.None,

        /// <include file="doc/docs.xml" path="docs/doc[@for='SelectionCount.Single']/*" />
        Single = Constants.Index,

        /// <include file="doc/docs.xml" path="docs/doc[@for='SelectionCount.Multiple']/*" />
        Multiple = Constants.Range | Constants.Index,
    }
}