using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Changes
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Int32Change']/*" />
    [StructLayout(LayoutKind.Sequential)]
    public struct Int32Change : IChange
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='Change.Id']/*" />
        public Guid Id;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Change.OldValue']/*" />
        public int OldValue;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Change.NewValue']/*" />
        public int NewValue;

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
        public override string ToString() => $"[Int32Change: Id = {this.Id}, OldValue = {this.OldValue}, NewValue = {this.NewValue}]";
    }
}