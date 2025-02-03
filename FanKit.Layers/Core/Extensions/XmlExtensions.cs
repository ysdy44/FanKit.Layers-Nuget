using FanKit.Layers.Core;
using System.Xml.Linq;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='XmlExtensions']/*" />
    public static class XmlExtensions
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlExtensions.SaveXmlStructure']/*" />
        public static XObject SaveXmlStructure(this ITreeNode node, XmlStructure type, XObject children)
        {
            switch (type)
            {
                case XmlStructure.List:
                    return new XAttribute("Depth", node.Depth);
                case XmlStructure.TreeNodes:
                    return new XAttribute("Id", node.Id);
                default:
                    return children;
            }
        }
    }
}