using System;

namespace FanKit.Layers.Core
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='ITreeNode']/*" />
    public interface ITreeNode
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='ITreeNode.Id']/*" />
        Guid Id { get; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ITreeNode.IsExpanded']/*" />
        int Depth { get; set; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ITreeNode.IsExpanded']/*" />
        bool IsExpanded { get; set; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ITreeNode.Settings']/*" />
        NodeSettings Settings { get; }
    }
}