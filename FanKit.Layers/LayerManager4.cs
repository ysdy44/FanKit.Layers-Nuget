using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4']/*" />
    public class LayerManager4<T, U>
        where T : class, ICloneable<T>, IComposite<T>, ILayerBase, IDisposable
        where U : class, IUndoable
    {
        private readonly LogicalTreeList<T> LogicalTree;
        private readonly VisualTreeList<T> VisualTree;
        private readonly Dictionary<Guid, T> Pool;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.List']/*" />
        public readonly LayerList<T> List;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.Collection']/*" />
        public readonly LayerCollection<T> Collection;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.DragUI']/*" />
        public readonly DragUI<T> DragUI;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.Clipboard']/*" />
        public readonly Clipboard<T> Clipboard;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.History']/*" />
        public readonly UndoStack<T, U> History;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.UILayers']/*" />
        public readonly ObservableCollection<T> UILayers = new ObservableCollection<T>();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.UIHistory']/*" />
        public readonly ObservableCollection<U> UIHistory = new ObservableCollection<U>();

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerManager4.LayerManager4']/*" />
        public LayerManager4()
        {
            this.LogicalTree = new LogicalTreeList<T>();
            this.VisualTree = new VisualTreeList<T>();
            this.Pool = new Dictionary<Guid, T>();

            this.List = new LayerList<T>(this.LogicalTree);
            this.Collection = new LayerCollection<T>(this.LogicalTree, this.VisualTree, this.Pool);

            this.DragUI = new DragUI<T>(this.LogicalTree, this.VisualTree);
            this.Clipboard = new Clipboard<T>(this.LogicalTree, this.Pool, this.Collection);
            this.History = new UndoStack<T, U>(this.Pool, this.Collection);

            this.Collection.Resetted += delegate
            {
                this.History.ClearUndoRedoHistory();
            };
        }
    }
}