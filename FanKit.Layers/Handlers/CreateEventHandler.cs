using System.Xml.Linq;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='CreateEventHandler']/*" />
    public delegate T CreateEventHandler<T>(XElement element);
}