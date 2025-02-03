using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Changes
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='DoubleChange']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public struct DoubleChange : IChange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='Change.Id']/*" />
        public Guid Id;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Change.OldValue']/*" />
        public double OldValue;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Change.NewValue']/*" />
        public double NewValue;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Change.IsEmpty']/*" />
        public bool IsEmpty => this.OldValue == this.NewValue;

        /// <inheritdoc/>
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                yield return this.Id;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public override string ToString() => $"[DoubleChange: Id = {this.Id}, OldValue = {this.OldValue}, NewValue = {this.NewValue}]";
    }
}