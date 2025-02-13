using FanKit.Layers.Core;
using System;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    /// <summary>
    /// <see cref=" XmlStructure"/>
    /// </summary>
    public static class XmlSaveExtensions
    {
        /// <summary>
        /// <see cref=" XmlStructure.List"/>
        /// </summary>
        public static XElement SaveToXmlList<T>(this LayerList<T> list)
            where T : class, ILayerBase
        {
            switch (list.Count)
            {
                case 0:
                    return new XElement("Root");
                case 1:
                    ILayerBase single = list.Single();

                    return new XElement("Root", single.SaveToXml(XmlStructure.List, null));
                default:
                    return new XElement("Root", (
                        from e
                        in list
                        select e.SaveToXml(XmlStructure.List, null)
                        ).ToArray());
            }
        }

        /// <summary>
        /// <see cref=" XmlStructure.Tree"/>
        /// </summary>
        public static XElement SaveToXmlTree<T>(this LayerCollection<T> collection, XmlTreeNode[] nodes)
            where T : class, IComposite<T>, ILayerBase, IDisposable
        {
            switch (nodes is null ? 0 : nodes.Length)
            {
                case 0:
                    return (new XElement("Root"));
                case 1:
                    XmlTreeNode single = nodes.Single();

                    return (new XElement("Root", collection.SaveToXml(single)));
                default:
                    return (new XElement("Root", collection.SaveToXml(nodes)));
            }
        }

        /// <summary>
        /// <see cref=" XmlStructure.TreeNodes"/>
        /// </summary>
        public static XElement SaveToXmlTreeNodes0(this XmlTreeNode[] nodes)
        {
            return new XElement("Root", new XElement("Nodes", (
                from e
                in nodes
                select e.SaveToXml("Node")
                ).ToArray()));
        }

        /// <summary>
        /// <see cref=" XmlStructure.TreeNodes"/>
        /// </summary>
        public static XElement SaveToXmlTreeNodes1<T>(this LayerList<T> list)
            where T : class, ILayerBase
        {
            return new XElement("Root", new XElement("Layers", (
                from e
                in list
                select e.SaveToXml(XmlStructure.TreeNodes, null)
                ).ToArray()));
        }

        /// <summary>
        /// <see cref=" XmlStructure.TreeNodes"/>
        /// </summary>
        public static XElement SaveToXmlTreeNodes01<T>(this LayerList<T> list, XmlTreeNode[] nodes)
            where T : class, ILayerBase
        {
            switch (nodes is null ? 0 : nodes.Length)
            {
                case 0:
                    return new XElement("Root");
                default:
                    return
                        new XElement("Root",
                            new XElement("Nodes", (
                                from e
                                in nodes
                                select e.SaveToXml("Node")
                                ).ToArray()),
                            new XElement("Layers", (
                                from e
                                in list
                                select e.SaveToXml(XmlStructure.TreeNodes, null)
                                ).ToArray())
                        );
            }
        }
    }
}