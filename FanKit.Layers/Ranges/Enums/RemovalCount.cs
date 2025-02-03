namespace FanKit.Layers.Ranges
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='RemovalCount']/*" />
    public enum RemovalCount : byte
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='RemovalCount.None']/*" />
        None = Constants.None,

        /// <include file="doc/docs.xml" path="docs/doc[@for='RemovalCount.Remove']/*" />
        Remove = Constants.Range | Constants.Index,

        /// <include file="doc/docs.xml" path="docs/doc[@for='RemovalCount.RemoveAll']/*" />
        RemoveAll = Constants.All,
    }
}