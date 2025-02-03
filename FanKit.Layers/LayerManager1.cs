using FanKit.Layers.Collections;
using System;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager1']/*" />
    public class LayerManager1<T>
        where T : class, ILayerBase, IDisposable
    {
        private readonly LogicalTreeList<T> LogicalTree;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager1.List']/*" />
        public readonly LayerList<T> List;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager1.LayerManager1']/*" />
        public LayerManager1()
        {
            this.LogicalTree = new LogicalTreeList<T>();

            this.List = new LayerList<T>(this.LogicalTree);
        }
    }
}