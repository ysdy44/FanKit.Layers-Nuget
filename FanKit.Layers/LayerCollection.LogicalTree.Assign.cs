using FanKit.Layers.Core;
using System.Linq;

namespace FanKit.Layers
{
    partial class LayerCollection<T>
    {
        #region Assign

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.AssignChildren']/*" />
        public void AssignChildren(bool updateSelectMode)
        {
            if (updateSelectMode)
            {
                this.AssignAll();
            }
            else
            {
                this.AssignChild();
            }
        }

        internal void AssignChild(int length)
        {
            switch (length)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree[0];

                    ChildrenClear(itemSingle);

                    //itemSingle.ParentDeselect();
                    break;
                case 2:
                    T itemFirst = this.LogicalTree[0];
                    T itemLast = this.LogicalTree[1];

                    if (itemFirst.Depth < itemLast.Depth)
                    {
                        ChildrenAdd(itemFirst, itemLast);
                        ChildrenClear(itemLast);

                        //switch (itemFirst.SelectMode)
                        //{
                        //    case SelectMode.Deselected:
                        //        itemLast.ParentDeselect();
                        //        break;
                        //    case SelectMode.Selected:
                        //        itemLast.ParentSelect();
                        //        break;
                        //    case SelectMode.Parent:
                        //        itemFirst.SelectMode = SelectMode.Deselected;
                        //        itemLast.ParentDeselect();
                        //        break;
                        //    default:
                        //        break;
                        //}
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        //itemFirst.ParentDeselect();
                        //itemLast.ParentDeselect();
                    }
                    break;
                default:
                    for (int i = 0; i < length - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item, length);
                    }

                    ChildrenClear(this.LogicalTree[length - 1]);

                    //this.LogicalTree.AssignParentSelect(this.LogicalTree.First());
                    break;
            }
        }

        internal void AssignChild()
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree.Single();

                    ChildrenClear(itemSingle);

                    //itemSingle.ParentDeselect();
                    break;
                case 2:
                    T itemFirst = this.LogicalTree.First();
                    T itemLast = this.LogicalTree.Last();

                    if (itemFirst.Depth < itemLast.Depth)
                    {
                        ChildrenAdd(itemFirst, itemLast);
                        ChildrenClear(itemLast);

                        //switch (itemFirst.SelectMode)
                        //{
                        //    case SelectMode.Deselected:
                        //        itemLast.ParentDeselect();
                        //        break;
                        //    case SelectMode.Selected:
                        //        itemLast.ParentSelect();
                        //        break;
                        //    case SelectMode.Parent:
                        //        itemFirst.SelectMode = SelectMode.Deselected;
                        //        itemLast.ParentDeselect();
                        //        break;
                        //    default:
                        //        break;
                        //}
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        //itemFirst.ParentDeselect();
                        //itemLast.ParentDeselect();
                    }
                    break;
                default:
                    for (int i = 0; i < this.LogicalTree.Count - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item);
                    }

                    ChildrenClear(this.LogicalTree.Last());

                    //this.LogicalTree.AssignParentSelect(this.LogicalTree.First());
                    break;
            }
        }

        internal void AssignAll()
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree.Single();

                    ChildrenClear(itemSingle);

                    itemSingle.ParentDeselect();
                    break;
                case 2:
                    T itemFirst = this.LogicalTree.First();
                    T itemLast = this.LogicalTree.Last();

                    if (itemFirst.Depth < itemLast.Depth)
                    {
                        ChildrenAdd(itemFirst, itemLast);
                        ChildrenClear(itemLast);

                        switch (itemFirst.SelectMode)
                        {
                            case SelectMode.Deselected:
                                itemLast.ParentDeselect();
                                break;
                            case SelectMode.Selected:
                                itemLast.ParentSelect();
                                break;
                            case SelectMode.Parent:
                                itemFirst.SelectMode = SelectMode.Deselected;
                                itemLast.ParentDeselect();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        itemFirst.ParentDeselect();
                        itemLast.ParentDeselect();
                    }
                    break;
                default:
                    for (int i = 0; i < this.LogicalTree.Count - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item);
                    }

                    ChildrenClear(this.LogicalTree.Last());

                    this.LogicalTree.AssignParentSelect(this.LogicalTree.First());
                    break;
            }
        }

        #endregion

        #region Children

        private static void ChildrenClear(T item)
        {
            switch (item.Children.Count)
            {
                case 0:
                    break;
                default:
                    item.Children.Clear();
                    item.OnChildrenCountChanged();
                    break;
            }
        }

        private static void ChildrenAdd(T itemFirst, T itemLast)
        {
            switch (itemFirst.Children.Count)
            {
                case 0:
                    itemFirst.Children.Add(itemLast);

                    itemFirst.OnChildrenCountChanged();
                    break;
                case 1:
                    itemFirst.Children.Clear();
                    itemFirst.Children.Add(itemLast);
                    break;
                default:
                    itemFirst.Children.Clear();
                    itemFirst.Children.Add(itemLast);

                    itemFirst.OnChildrenCountChanged();
                    break;
            }
        }

        private void ChildrenReset(int i, T item, int length)
        {
            {
                int count = item.Children.Count;
                item.Children.Clear();

                switch (item.Depth)
                {
                    case 0:
                        for (int j = i + 1; j < length; j++)
                        {
                            T jtem = this.LogicalTree[j];

                            if (jtem.Depth == 1)
                            {
                                item.Children.Add(jtem);
                            }
                            else if (jtem.Depth == 0)
                            {
                                break;
                            }
                        }

                        if (count != item.Children.Count)
                            item.OnChildrenCountChanged();

                        break;
                    default:
                        int depth = item.Depth + 1;

                        for (int j = i + 1; j < length; j++)
                        {
                            T jtem = this.LogicalTree[j];

                            if (jtem.Depth == depth)
                            {
                                item.Children.Add(jtem);
                            }
                            else if (jtem.Depth < depth)
                            {
                                break;
                            }
                        }

                        if (count != item.Children.Count)
                            item.OnChildrenCountChanged();

                        break;
                }
            }
        }

        private void ChildrenReset(int i, T item)
        {
            {
                int count = item.Children.Count;
                item.Children.Clear();

                switch (item.Depth)
                {
                    case 0:
                        for (int j = i + 1; j < this.LogicalTree.Count; j++)
                        {
                            T jtem = this.LogicalTree[j];

                            if (jtem.Depth == 1)
                            {
                                item.Children.Add(jtem);
                            }
                            else if (jtem.Depth == 0)
                            {
                                break;
                            }
                        }

                        if (count != item.Children.Count)
                            item.OnChildrenCountChanged();

                        break;
                    default:
                        int depth = item.Depth + 1;

                        for (int j = i + 1; j < this.LogicalTree.Count; j++)
                        {
                            T jtem = this.LogicalTree[j];

                            if (jtem.Depth == depth)
                            {
                                item.Children.Add(jtem);
                            }
                            else if (jtem.Depth < depth)
                            {
                                break;
                            }
                        }

                        if (count != item.Children.Count)
                            item.OnChildrenCountChanged();

                        break;
                }
            }
        }

        #endregion
    }
}