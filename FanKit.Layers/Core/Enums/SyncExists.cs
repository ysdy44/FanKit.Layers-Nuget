using FanKit.Layers.Core;
using FanKit.Layers.Collections;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='SyncExists']/*" />
    public enum SyncExists : byte
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='SyncExists.Source']/*" />
        Source,

        /// <include file="doc/docs.xml" path="docs/doc[@for='SyncExists.Destination']/*" />
        Destination,

        /// <include file="doc/docs.xml" path="docs/doc[@for='SyncExists.Both']/*" />
        Both,
    }
}