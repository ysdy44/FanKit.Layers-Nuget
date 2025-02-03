using System.Collections.Generic;

namespace FanKit.Layers.Core
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='IComposite']/*" />
    public interface IComposite<T>
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='IComposite.Children']/*" />
        IList<T> Children { get; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='IComposite.OnChildrenCountChanged']/*" />
        void OnChildrenCountChanged();
    }
}