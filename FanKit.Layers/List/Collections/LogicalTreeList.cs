using FanKit.Layers.Ranges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanKit.Layers.Collections
{
    public partial class LogicalTreeList<T> : RangeList<T>
        where T : class, ILayerBase
    {
        private LogicalTreeList<T> LogicalTree => this;

        //@Linq
        private static Guid SelectId(T item) => item.Id;

        public SelectIndexer IndexerOf(T item) => new SelectIndexer(item, this.LogicalTree.IndexOf(item));

        public Guid[] GetIds()
        {
            List<T> list = this.LogicalTree;
            return list.Select(SelectId).ToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (T item in this.LogicalTree)
            {
                for (int n = 0; n < item.Depth; n++)
                {
                    sb.Append("│  ");
                }

                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }

        #region Xml

        public XmlTreeNode[] GetNodes()
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    return null;
                case 1:
                    T single = this.LogicalTree.Single();

                    return new XmlTreeNode[]
                    {
                        new XmlTreeNode(single.Id)
                    };
                case 2:
                    T first = this.LogicalTree.First();
                    T last = this.LogicalTree.Last();

                    if (last.Depth > first.Depth)
                        return new XmlTreeNode[]
                        {
                            new XmlTreeNode(first.Id, last.Id)
                        };
                    else
                        return new XmlTreeNode[]
                        {
                            new XmlTreeNode(first.Id),
                            new XmlTreeNode(last.Id)
                        };
                default:
                    return this.GetNodes(0, new IndexRange(0, this.LogicalTree.Count - 1)).ToArray();
            }
        }

        private IEnumerable<XmlTreeNode> GetNodes(int depth, IndexRange range)
        {
            int startIndex = -1;

            for (int i = range.StartIndex; i <= range.EndIndex; i++)
            {
                T item = this.LogicalTree[i];

                switch (item.Depth - depth)
                {
                    case 0:
                        if (startIndex == -1)
                        {
                            startIndex = i;
                            break;
                        }

                        int endIndex = i - 1;
                        switch (endIndex - startIndex)
                        {
                            case 0: yield return new XmlTreeNode(this.LogicalTree[startIndex].Id); break;
                            case 1: yield return new XmlTreeNode(this.LogicalTree[startIndex].Id, this.LogicalTree[endIndex].Id); break;
                            default: yield return new XmlTreeNode(this.LogicalTree[startIndex].Id, this.GetNodes(depth + 1, new IndexRange(startIndex + 1, endIndex)).ToArray()); break;
                        }
                        startIndex = i;
                        break;
                    default:
                        break;
                }
            }

            switch (range.EndIndex - startIndex)
            {
                case 0: yield return new XmlTreeNode(this.LogicalTree[startIndex].Id); break;
                case 1: yield return new XmlTreeNode(this.LogicalTree[startIndex].Id, this.LogicalTree[range.EndIndex].Id); break;
                default: yield return new XmlTreeNode(this.LogicalTree[startIndex].Id, this.GetNodes(depth + 1, new IndexRange(startIndex + 1, range.EndIndex)).ToArray()); break;
            }
        }

        #endregion

        #region Ranges

        public IEnumerable<IndexRange> GetSelectedRanges()
        {
            int first = 0;
            int last = 1;

            int depth = 0;
            int d;

            switch (this.LogicalTree[0].SelectMode)
            {
                case SelectMode.Selected:
                    {
                        Item:
                        d = this.LogicalTree[1].Depth;
                        if (depth < d)
                        {
                            last++;
                            goto Item;
                        }
                        else
                        {
                            // First
                            yield return new IndexRange(0, last - 1);

                            first = last;
                            last++;

                            if (last < this.LogicalTree.Count)
                                goto Range;
                            else
                            {
                                // Last
                                yield return new IndexRange(first, last - 1);
                                yield break;
                            }
                        }
                    }
                default:
                    {
                        first = last;
                        last++;

                        if (last < this.LogicalTree.Count)
                            goto Range;
                        else
                        {
                            switch (this.LogicalTree[first].SelectMode)
                            {
                                case SelectMode.Selected:
                                    // Last
                                    yield return new IndexRange(first, last - 1);
                                    break;
                                default:
                                    break;
                            }
                            yield break;
                        }
                    }
            }

            Range:
            switch (this.LogicalTree[first].SelectMode)
            {
                case SelectMode.Selected:
                    {
                        depth = this.LogicalTree[first].Depth;

                        Next:
                        d = this.LogicalTree[last].Depth;
                        if (depth < d)
                        {
                            last++;

                            if (last < this.LogicalTree.Count)
                                goto Next;
                            else
                            {
                                // Last
                                yield return new IndexRange(first, last - 1);
                                yield break;
                            }
                        }
                        else
                        {
                            // Range
                            yield return new IndexRange(first, last - 1);

                            first = last;
                            last++;

                            if (last < this.LogicalTree.Count)
                                goto Range;
                            else
                            {
                                // Last
                                yield return new IndexRange(first, last - 1);
                                yield break;
                            }
                        }
                    }
                default:
                    {
                        first = last;
                        last++;

                        if (last < this.LogicalTree.Count)
                            goto Range;
                        else
                        {
                            switch (this.LogicalTree[first].SelectMode)
                            {
                                case SelectMode.Selected:
                                    // Last
                                    yield return new IndexRange(first, last - 1);
                                    break;
                                default:
                                    break;
                            }
                            yield break;
                        }
                    }
            }
        }

        #endregion

        #region Assign

        public void AssignParentSelect(T itemFirst)
        {
            itemFirst.ParentDeselect();
            int index = 1;
            bool select = itemFirst.SelectMode.IsSelected();

            while (index < this.LogicalTree.Count)
            {
                T item = this.LogicalTree[index];

                switch (item.Depth)
                {
                    case 0:
                        item.ParentDeselect();
                        index++;
                        select = item.SelectMode.IsSelected();
                        break;
                    default:
                        if (select)
                        {
                            item.ParentSelect();
                            index++;
                        }
                        else
                        {
                            item.ParentDeselect();
                            index = this.AssignParentSelect(index + 1, item.Depth, item.SelectMode.IsSelected());
                        }
                        break;
                }
            }
        }

        public int AssignParentSelect(int index, int depth, bool select)
        {
            while (index < this.LogicalTree.Count)
            {
                T item = this.LogicalTree[index];

                if (depth >= item.Depth)
                {
                    return index;
                }
                else
                {
                    if (select)
                    {
                        item.ParentSelect();
                        index++;
                    }
                    else
                    {
                        item.ParentDeselect();
                        index = this.AssignParentSelect(index + 1, item.Depth, item.SelectMode.IsSelected());
                    }
                }
            }

            return this.LogicalTree.Count;
        }

        #endregion
    }
}