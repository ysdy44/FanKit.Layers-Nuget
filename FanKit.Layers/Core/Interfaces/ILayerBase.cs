using FanKit.Layers.Core;
using System.Xml.Linq;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase']/*" />
    public interface ILayerBase : ITreeNode, IChildNode
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase.IsGroup']/*" />
        bool IsGroup { get; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase.IsLocked']/*" />
        bool IsLocked { get; set; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase.IsVisible']/*" />
        bool IsVisible { get; set; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase.SelectMode']/*" />
        SelectMode SelectMode { get; set; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase.RenderThumbnail']/*" />
        void RenderThumbnail();

        /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase.LoadFromXml']/*" />
        void LoadFromXml(XElement content);

        /// <include file="doc/docs.xml" path="docs/doc[@for='ILayerBase.SaveToXml']/*" />
        XElement SaveToXml(XmlStructure structure, XObject children);
    }
}