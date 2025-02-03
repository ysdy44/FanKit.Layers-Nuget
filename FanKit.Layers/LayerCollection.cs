using FanKit.Layers.Changes;
using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection']/*" />
    public partial class LayerCollection<T> :
        IReadOnlyList<T>,
        IReadOnlyCollection<T>
        where T : class, IComposite<T>, ILayerBase, IDisposable
    {
        // Delegate
        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Resetted']/*" />
        public event EventHandler Resetted;

        private readonly LogicalTreeList<T> LogicalTree;

        private readonly VisualTreeList<T> VisualTree;

        private readonly Dictionary<Guid, T> Pool;

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.LayerCollection']/*" />
        public LayerCollection(LogicalTreeList<T> logicalTree, VisualTreeList<T> visualTree, Dictionary<Guid, T> pool)
        {
            this.VisualTree = visualTree;
            this.LogicalTree = logicalTree;
            this.Pool = pool;
        }

        // Indexer
        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.this"/>
        public T this[Guid key] => this.Pool[key];

        // Indexer
        /// <inheritdoc cref="IReadOnlyList{T}.this"/>
        public T this[int index] => this.LogicalTree[index];

        /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
        public int Count => this.LogicalTree.Count;

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => this.LogicalTree.GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator() => this.LogicalTree.GetEnumerator();

        //------------------------ Reset ----------------------------//  

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Reset']/*" />
        public void Reset()
        {
            foreach (KeyValuePair<Guid, T> item in this.Pool)
            {
                item.Value.Dispose();
            }

            this.Pool.Clear();

            this.LogicalTree.Clear();
            this.VisualTree.Clear();

            this.Resetted?.Invoke(this, null); // Delegate
        }

        //------------------------ Xml ----------------------------//  

        // Xml.Nodes
        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.SaveToXml']/*" />
        public XElement SaveToXml(XmlTreeNode treeNode)
        {
            T layer = this.Pool[treeNode.Id];

            switch (treeNode.Children is null ? 0 : treeNode.Children.Length)
            {
                case 0:
                    return layer.SaveToXml(XmlStructure.Tree, null);
                case 1:
                    XmlTreeNode singe = treeNode.Children.Single();

                    return layer.SaveToXml(XmlStructure.Tree, new XElement("Children", this.SaveToXml(singe)));
                default:
                    return layer.SaveToXml(XmlStructure.Tree, new XElement("Children", this.SaveToXml(treeNode.Children)));
            }
        }
        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.SaveToXml2']/*" />
        public XElement[] SaveToXml(XmlTreeNode[] treeNode) => treeNode.Select(this.SaveToXml).ToArray();

        // LayerPool.History.Operate.cs
        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Sort']/*" />
        public void Sort(Guid[] ids)
        {
            this.LogicalTree.Clear();

            foreach (Guid item in ids)
            {
                this.LogicalTree.Add(this.Pool[item]);
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.SyncToVisualTree']/*" />
        public void SyncToVisualTree() => this.VisualTree.VisualSortByExpand(this.LogicalTree);

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.UISyncTo']/*" />
        public void UISyncTo(IList<T> items) => this.VisualTree.UISyncTo(items);

        #region ILayerCollection5

        public void ApplyDepth(Int32Change change)
        {
            this.Pool[change.Id].Depth = change.NewValue;
        }

        public void ApplyDepths(IEnumerable<Int32Change> changes)
        {
            foreach (Int32Change item in changes)
            {
                this.Pool[item.Id].Depth = item.NewValue;
            }
        }

        public void ApplyLock(BooleanChange change)
        {
            this.Pool[change.Id].IsLocked = change.NewValue;
        }

        public void ApplyVisible(BooleanChange change)
        {
            this.Pool[change.Id].IsVisible = change.NewValue;
        }

        public void ApplyVisibles(IEnumerable<BooleanChange> changes)
        {
            foreach (BooleanChange item in changes)
            {
                this.Pool[item.Id].IsVisible = item.NewValue;
            }
        }

        public void ApplySelect(SelectChange change)
        {
            this.Pool[change.Id].SelectMode = change.NewValue;
        }

        public void ApplySelects(IEnumerable<SelectChange> changes)
        {
            foreach (SelectChange item in changes)
            {
                this.Pool[item.Id].SelectMode = item.NewValue;
            }
        }

        #endregion
    }
}