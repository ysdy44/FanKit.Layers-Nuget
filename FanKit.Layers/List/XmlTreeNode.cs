using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FanKit.Layers
{
    /*
    * A
    * │
    * ├─1. B
    * │  │
    * │  ├─2.  C
    * │  │  │
    * │  │  ├─3. D
    * │  │  │  │
    * │  │  │  └─4. E
    * │  │  │
    * │  │  ├─3. F
    * │  │  │  │
    * │  │  │  └─4. G
    * │  │  │
    * │  ├─2. G
    * │  │  │
    * │  │  ├─3. I
    * │  │  │  │
    * │  │  │  └─4. J
    * │  │  │
    */
    /// <include file="doc/docs.xml" path="docs/doc[@for='XmlTreeNode']/*" />
    public sealed class XmlTreeNode
    {
        internal Guid Id;
        internal XmlTreeNode[] Children;

        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlTreeNode.XmlTreeNodeWithGuid']/*" />
        public XmlTreeNode(Guid id)
        {
            this.Id = id;
        }

        internal XmlTreeNode(Guid id, Guid childId)
        {
            this.Id = id;
            this.Children = new XmlTreeNode[]
            {
                new XmlTreeNode(childId)
            };
        }

        internal XmlTreeNode(Guid id, XmlTreeNode[] children)
        {
            this.Id = id;
            this.Children = children;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlTreeNode.XmlTreeNodeWithXml']/*" />
        public XmlTreeNode(XElement element)
        {
            XAttribute attribute = element.Attribute("Id");
            this.Id = Guid.Parse(attribute.Value);

            XmlTreeNode[] nodes = (
                    from e
                    in element.Elements()
                    select new XmlTreeNode(e)
                    ).ToArray();
            this.Children = nodes.Length == 0 ? null : nodes;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlTreeNode.SaveToXml']/*" />
        public XElement SaveToXml(string elementChildName)
        {
            switch (this.Children is null ? 0 : this.Children.Length)
            {
                case 0:
                    return new XElement(elementChildName, new XAttribute("Id", this.Id));
                case 1:
                    XmlTreeNode single = this.Children.Single();

                    return new XElement(elementChildName,
                        new XAttribute("Id", this.Id),
                        single.SaveToXml(elementChildName)
                        );
                default:
                    object[] content = new object[this.Children.Length + 1];
                    content[0] = new XAttribute("Id", this.Id);

                    for (int i = 0; i < this.Children.Length; i++)
                    {
                        XmlTreeNode item = this.Children[i];

                        content[i + 1] = item.SaveToXml(elementChildName);
                    }

                    return new XElement(elementChildName, content);
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='XmlTreeNode.AppendTo']/*" />
        public void AppendTo(StringBuilder stringBuilder)
        {
            this.Build(stringBuilder, 0);
        }

        private void Build(StringBuilder sb, int depth)
        {
            for (int n = 0; n < depth; n++)
            {
                sb.Append("│  ");
            }

            sb.AppendLine(this.Id.ToString());

            switch (this.Children is null ? 0 : this.Children.Length)
            {
                case 0:
                    break;
                default:
                    foreach (XmlTreeNode item in this.Children)
                    {
                        item.Build(sb, depth + 1);
                    }
                    break;
            }
        }
    }
}