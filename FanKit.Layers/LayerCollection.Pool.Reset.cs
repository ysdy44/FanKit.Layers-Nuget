using FanKit.Layers.Core;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FanKit.Layers
{
    partial class LayerCollection<T>
    {
        #region Reset T

        private void AddRecursion(IEnumerable<T> children, int depth)
        {
            foreach (T item in children)
            {
                item.Depth = depth;
                this.LogicalTree.Add(item);

                Guid key = item.Id;
                this.Pool.Add(key, item);

                if (item.Children != null)
                {
                    this.AddRecursion(item.Children, depth + 1);
                }
            }
        }

        #endregion

        #region Reset T

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ResetByList']/*" />
        public InvalidateModes ResetByList(IEnumerable<T> list)
        {
            this.Reset();

            this.LogicalTree.AddRange(list);
            foreach (T item in this.LogicalTree)
            {
                Guid key = item.Id;
                this.Pool.Add(key, item);
            }

            this.AssignChild3();
            this.SyncToVisualTree();
            return InvalidateModes.Reset;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ResetByTree']/*" />
        public InvalidateModes ResetByTree(IEnumerable<T> tree)
        {
            this.Reset();

            this.AddRecursion(tree, 0);

            //this.ResetChildren();
            this.SyncToVisualTree();
            return InvalidateModes.Reset;
        }

        #endregion

        #region Reset Custom

        private void AddRecursion<D>(IEnumerable<D> children, int depth, CreateAndLoadWithDepthEventHandler<T, D> create)
            where D : IComposite<D>
        {
            foreach (D item in children)
            {
                this.LogicalTree.Add(create(item, depth));

                if (item.Children != null)
                {
                    this.AddRecursion(item.Children, depth + 1, create);
                }
            }
        }

        #endregion

        #region Reset Custom

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ResetByCustomList']/*" />
        public InvalidateModes ResetByCustomList<D>(IEnumerable<D> listOfData, CreateAndLoadEventHandler<T, D> creator)
        {
            this.Reset();

            foreach (D data in listOfData)
            {
                T item = creator(data);
                this.LogicalTree.Add(item);

                Guid key = item.Id;
                this.Pool.Add(key, item);
            }

            this.AssignChild3();
            this.SyncToVisualTree();
            return InvalidateModes.Reset;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ResetByCustomTree']/*" />
        public InvalidateModes ResetByCustomTree<D>(IEnumerable<D> treeOfData, CreateAndLoadWithDepthEventHandler<T, D> creator)
            where D : IComposite<D>
        {
            this.Reset();

            this.AddRecursion(treeOfData, 0, creator);

            foreach (T item in this.LogicalTree)
            {
                Guid key = item.Id;
                this.Pool.Add(key, item);
            }

            this.AssignChild1();
            this.SyncToVisualTree();
            return InvalidateModes.Reset;
        }

        #endregion

        private void PoolAdd(XElement element, T item)
        {
            item.LoadFromXml(element);

            Guid key = item.Id;
            this.Pool.Add(key, item);

            this.LogicalTree.Add(item);
        }

        private void PoolParse(XElement element, T item)
        {
            XAttribute attribute = element.Attribute("Id");
            Guid id = Guid.Parse(attribute.Value);

            item.LoadFromXml(element);

            this.Pool.Add(id, item);
        }

        #region Reset Xml

        private void AddRecursion(XmlTreeNode node, int depth)
        {
            T item = this.Pool[node.Id];
            item.Depth = depth;

            this.LogicalTree.Add(item);

            XmlTreeNode[] children = node.Children;
            if (children is null)
                return;

            foreach (XmlTreeNode child in children)
            {
                this.AddRecursion(child, depth + 1);
            }
        }

        private void AddRecursion(XElement element, int depth, CreateWithDepthEventHandler<T> creator)
        {
            this.PoolAdd(element, creator(element, depth));

            XElement children = element.Element("Children");
            if (children is null)
                return;

            foreach (XElement child in children.Elements())
            {
                this.AddRecursion(child, depth + 1, creator);
            }
        }

        #endregion

        #region Reset Xml

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ResetByXmlList']/*" />
        public InvalidateModes ResetByXmlList(IEnumerable<XElement> listOfXml, CreateEventHandler<T> creator)
        {
            this.Reset();

            foreach (XElement child in listOfXml)
            {
                this.PoolAdd(child, creator(child));
            }
            this.AssignChild3();

            this.SyncToVisualTree();
            return InvalidateModes.Reset;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ResetByXmlTree']/*" />
        public InvalidateModes ResetByXmlTree(IEnumerable<XElement> treeOfXml, CreateWithDepthEventHandler<T> creator)
        {
            this.Reset();

            foreach (XElement child in treeOfXml)
            {
                this.AddRecursion(child, 0, creator);
            }

            this.AssignChild2();

            this.SyncToVisualTree();
            return InvalidateModes.Reset;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ResetByXmlTreeNodes']/*" />
        public InvalidateModes ResetByXmlTreeNodes(IEnumerable<XElement> items, IEnumerable<XmlTreeNode> nodes, CreateEventHandler<T> creator)
        {
            this.Reset();

            // Add into Pool
            foreach (XElement child in items)
            {
                this.PoolParse(child, creator(child));
            }

            //this.LogicalTree.Clear();
            foreach (XmlTreeNode child in nodes)
            {
                this.AddRecursion(child, 0);
            }

            // Dispose
            this.Pool.Clear();

            foreach (T item in this.LogicalTree)
            {
                Guid key = item.Id;
                this.Pool.Add(key, item);
            }

            this.AssignChild2();

            this.SyncToVisualTree();
            return InvalidateModes.Reset;
        }

        #endregion
    }
}