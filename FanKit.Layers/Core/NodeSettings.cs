using FanKit.Layers.Collections;
using System.Linq;

namespace FanKit.Layers.Core
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='NodeSettings']/*" />
    public sealed class NodeSettings
    {
        private int DescendantCount;

        internal int ReferenceCount;

        internal DraggingGuide Guide;

        /*
         * Represents a value indicating whether the <see cref="ITreeNode"/> in the <see cref="LogicalTreeList{T}"/>.
         */
        internal bool Exits;

        /*
         * The zero-base index of the <see cref="ITreeNode"/> within the <see cref="VisualTreeList{T}"/>.
         * -2: Source contains it only
         * -1: Destination contains it only
         * 0~N: Source and Destination contains it
         */
        internal int UIIndex;
        internal SyncExists UIExists
        {
            get
            {
                switch (this.UIIndex)
                {
                    case -2:
                        return SyncExists.Source;
                    case -1:
                        return SyncExists.Destination;
                    default:
                        return SyncExists.Both;
                }
            }
        }

        internal int GetDescendantCount(IChildNode parent)
        {
            this.DescendantCount = 0;
            this.Recursion(parent);
            return this.DescendantCount;
        }

        private void Recursion(IChildNode parent)
        {
            switch (parent.ChildrenCount)
            {
                case 0:
                    break;
                case 1:
                    this.DescendantCount++;
                    this.Recursion(parent.ChildNodes.Single());
                    break;
                default:
                    foreach (IChildNode item in parent.ChildNodes)
                    {
                        this.DescendantCount++;
                        this.Recursion(item);
                    }
                    break;
            }
        }
    }
}