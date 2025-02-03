using FanKit.Layers.Core;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='XmlStructure']/*" />
    public enum XmlStructure : byte
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlStructure.List']/*" />
        List = 0,

        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlStructure.Tree']/*" />
        Tree = 1,

        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlStructure.TreeNodes']/*" />
        TreeNodes = 2,
    }
}