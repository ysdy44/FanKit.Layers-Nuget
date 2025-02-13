using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    /// <summary>
    /// <see cref=" XmlStructure"/>
    /// </summary>
    public static class XmlLoadExtensions
    {
        /// <summary>
        /// <see cref=" XmlStructure.TreeNodes"/>
        /// </summary>
        public static XmlTreeNode[] LoadFromXmlTreeNodes0(this XDocument doc)
        {
            XElement nodesXml = doc.Root.Element("Nodes");
            XmlTreeNode[] nodes = (
                from e
                in nodesXml.Elements("Node")
                select new XmlTreeNode(e)
                ).ToArray();

            return nodes;
        }

        /// <summary>
        /// <see cref=" XmlStructure.TreeNodes"/>
        /// </summary>
        public static IEnumerable<XElement> LoadFromXmlTreeNodes1(this XDocument doc)
        {
            XElement layersXml = doc.Root.Element("Layers");

            return layersXml.Elements();
        }
    }
}