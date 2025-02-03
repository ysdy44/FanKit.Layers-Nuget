using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.History
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='SortChange']/*" />
    public class SortChange : IChange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='SortChange.OldIds']/*" />
        public Guid[] OldIds { get; set; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='SortChange.NewIds']/*" />
        public Guid[] NewIds { get; set; }

        /// <inheritdoc/>
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                // Projects each node of a sequence into a ID.
                if (this.OldIds != null)
                {
                    foreach (Guid id in this.OldIds)
                        yield return id;
                }

                if (this.NewIds != null)
                {
                    foreach (Guid id in this.NewIds)
                        yield return id;
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.OldIds = null;
            this.NewIds = null;
        }
    }
}