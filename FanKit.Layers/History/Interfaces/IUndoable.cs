using FanKit.Layers.Changes;
using FanKit.Layers.History;
using System;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='IUndoable']/*" />
    public interface IUndoable
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='IUndoable.Id']/*" />
        Guid Id { get; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='IUndoable.Change']/*" />
        IChange Change { get; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='IUndoable.Period']/*" />
        TimePeriod Period { get; set; }
    }
}