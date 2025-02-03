using FanKit.Layers.Ranges;

namespace FanKit.Layers.Reorders
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='ReorderCount']/*" />
    public enum ReorderCount : byte
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='ReorderCount.None']/*" />
        None = Constants.None,

        /// <include file="doc/docs.xml" path="docs/doc[@for='ReorderCount.Single']/*" />
        Single = Constants.Index,

        /// <include file="doc/docs.xml" path="docs/doc[@for='ReorderCount.SingleRange']/*" />
        SingleRange = Constants.Range,

        /// <include file="doc/docs.xml" path="docs/doc[@for='ReorderCount.Multiple']/*" />
        Multiple = Constants.Range | Constants.Index,
    }
}