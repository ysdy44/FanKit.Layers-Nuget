using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.History
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='SelectChanges']/*" />
    public class SelectChanges : IChange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='SelectChanges.Selects']/*" />
        public SelectChange[] Selects { get; set; }

        /// <inheritdoc/>
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                foreach (SelectChange item in this.Selects)
                {
                    yield return item.Id;
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Selects = null;
        }
    }
}