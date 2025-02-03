using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.History
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='VisibleChanges']/*" />
    public class VisibleChanges : IChange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='VisibleChanges.Visibles']/*" />
        public BooleanChange[] Visibles { get; set; }

        /// <inheritdoc/>
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                foreach (BooleanChange item in this.Visibles)
                {
                    yield return item.Id;
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Visibles = null;
        }
    }
}