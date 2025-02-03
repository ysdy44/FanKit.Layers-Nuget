using System.Xml.Linq;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='CreateWithDepthEventHandler']/*" />
    public delegate T CreateWithDepthEventHandler<T>(XElement element, int depth);
}