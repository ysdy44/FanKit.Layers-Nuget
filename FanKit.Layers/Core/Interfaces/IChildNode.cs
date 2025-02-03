using System.Collections.Generic;

namespace FanKit.Layers.Core
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='IChildNode']/*" />
    public interface IChildNode
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='IChildNode.ChildrenCount']/*" />
        int ChildrenCount { get; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='IChildNode.ChildNodes']/*" />
        IEnumerable<IChildNode> ChildNodes { get; }
    }
}