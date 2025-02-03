using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.History
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='DepthChanges']/*" />
    public class DepthChanges : IChange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='DepthChanges.Depths']/*" />
        public Int32Change[] Depths { get; set; }

        /// <inheritdoc/>
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                foreach (Int32Change item in this.Depths)
                {
                    yield return item.Id;
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Depths = null;
        }
    }
}