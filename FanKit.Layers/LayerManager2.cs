using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager2']/*" />
    public class LayerManager2<T>
        where T : class, IComposite<T>, ILayerBase, IDisposable
    {
        private readonly LogicalTreeList<T> LogicalTree;
        private readonly VisualTreeList<T> VisualTree;
        private readonly Dictionary<Guid, T> Pool;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager2.List']/*" />
        public readonly LayerList<T> List;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager2.Collection']/*" />
        public readonly LayerCollection<T> Collection;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager2.DragUI']/*" />
        public readonly DragUI<T> DragUI;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager2.UILayers']/*" />
        public readonly ObservableCollection<T> UILayers = new ObservableCollection<T>();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager2.LayerManager2']/*" />
        public LayerManager2()
        {
            this.LogicalTree = new LogicalTreeList<T>();
            this.VisualTree = new VisualTreeList<T>();
            this.Pool = new Dictionary<Guid, T>();

            this.List = new LayerList<T>(this.LogicalTree);
            this.Collection = new LayerCollection<T>(this.LogicalTree, this.VisualTree, this.Pool);

            this.DragUI = new DragUI<T>(this.LogicalTree, this.VisualTree);
        }
    }
}