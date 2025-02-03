using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.History
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='DoubleSelectChange']/*" />
    public class DoubleSelectChange : IChange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='DoubleSelectChange.Select0']/*" />
        public SelectChange Select0 { get; set; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='DoubleSelectChange.Select1']/*" />
        public SelectChange Select1 { get; set; }
        
        /// <inheritdoc/>
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                yield return this.Select0.Id;
                yield return this.Select1.Id;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Select0 = default;
            this.Select1 = default;
        }
    }
}