using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager3']/*" />
    public class LayerManager3<T>
        where T : class, ICloneable<T>, IComposite<T>, ILayerBase, IDisposable
    {
        private readonly LogicalTreeList<T> LogicalTree;
        private readonly VisualTreeList<T> VisualTree;
        private readonly Dictionary<Guid, T> Pool;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager3.List']/*" />
        public readonly LayerList<T> List;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager3.Collection']/*" />
        public readonly LayerCollection<T> Collection;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager3.DragUI']/*" />
        public readonly DragUI<T> DragUI;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager3.Clipboard']/*" />
        public readonly Clipboard<T> Clipboard;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager3.UILayers']/*" />
        public readonly ObservableCollection<T> UILayers = new ObservableCollection<T>();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager3.LayerManager3']/*" />
        public LayerManager3()
        {
            this.LogicalTree = new LogicalTreeList<T>();
            this.VisualTree = new VisualTreeList<T>();
            this.Pool = new Dictionary<Guid, T>();

            this.List = new LayerList<T>(this.LogicalTree);
            this.Collection = new LayerCollection<T>(this.LogicalTree, this.VisualTree, this.Pool);

            this.DragUI = new DragUI<T>(this.LogicalTree, this.VisualTree);
            this.Clipboard = new Clipboard<T>(this.LogicalTree, this.Pool, this.Collection);
        }
    }
}