using System;
using System.Collections.Generic;

namespace FanKit.Layers.Changes
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='IChange']/*" />
    public interface IChange : IDisposable
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='IChange.ReferenceGuids']/*" />
        IEnumerable<Guid> ReferenceGuids { get; }
    }
}