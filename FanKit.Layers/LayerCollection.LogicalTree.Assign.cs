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
                this.AssignChild2();
            }
            else
            {
                this.AssignChild1();
            }
        }

        #endregion

        #region Depth -> Children

        // Range: 0 ~ length
        internal void AssignChild1(int length)
        {
            switch (length)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree[0];

                    ChildrenClear(itemSingle);

                    //itemSingle.ParentDeselect();

                    //itemSingle.Depth = 0;
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

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 1;
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        //itemFirst.ParentDeselect();
                        //itemLast.ParentDeselect();

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 0;
                    }
                    break;
                default:
                    for (int i = 0; i < length - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item, length);
                    }

                    ChildrenClear(this.LogicalTree[length - 1]);

                    //T item0 = this.LogicalTree.First();
                    //this.LogicalTree.AssignParentSelect(item0, length);

                    //item0.Depth = 0;
                    //AssignD1(item0);

                    //for (int i = 1; i < length - 1; i++)
                    //{
                    //    T item = this.LogicalTree[i];
                    //    switch (item.Depth)
                    //    {
                    //        case 0:
                    //            AssignD1(item);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    break;
            }
        }

        // Range: All
        internal void AssignChild1()
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree.Single();

                    ChildrenClear(itemSingle);

                    //itemSingle.ParentDeselect();

                    //itemSingle.Depth = 0;
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

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 1;
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        //itemFirst.ParentDeselect();
                        //itemLast.ParentDeselect();

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 0;
                    }
                    break;
                default:
                    for (int i = 0; i < this.LogicalTree.Count - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item);
                    }

                    ChildrenClear(this.LogicalTree.Last());

                    //T item0 = this.LogicalTree.First();
                    //this.LogicalTree.AssignParentSelect(item0);

                    //item0.Depth = 0;
                    //AssignD1(item0);

                    //for (int i = 1; i < this.LogicalTree.Count - 1; i++)
                    //{
                    //    T item = this.LogicalTree[i];
                    //    switch (item.Depth)
                    //    {
                    //        case 0:
                    //            AssignD1(item);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    break;
            }
        }

        // Range: The children of the parent of the item
        internal void AssignChild1(int newIndex, T newItem)
        {
            for (int i = newIndex - 1; i >= 0; i--)
            {
                T item = this.LogicalTree[i];

                if (item.Depth < newItem.Depth)
                {
                    //switch (item.SelectMode)
                    //{
                    //    case SelectMode.Selected:
                    //    case SelectMode.Parent:
                    //        newItem.SelectMode = SelectMode.Parent;
                    //        break;
                    //    default:
                    //        switch (newItem.SelectMode)
                    //        {
                    //            case SelectMode.Parent:
                    //                newItem.SelectMode = SelectMode.Deselected;
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //        break;
                    //}

                    this.ChildrenReset(i, item);

                    //AssignDepth(item, item.Depth + 1);
                    break;
                }
            }
        }

        #endregion

        #region Depth -> Children, Children -> SelectMode

        // Range: 0 ~ length
        internal void AssignChild2(int length)
        {
            switch (length)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree[0];

                    ChildrenClear(itemSingle);

                    itemSingle.ParentDeselect();

                    //itemSingle.Depth = 0;
                    break;
                case 2:
                    T itemFirst = this.LogicalTree[0];
                    T itemLast = this.LogicalTree[1];

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

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 1;
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        itemFirst.ParentDeselect();
                        itemLast.ParentDeselect();

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 0;
                    }
                    break;
                default:
                    for (int i = 0; i < length - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item, length);
                    }

                    ChildrenClear(this.LogicalTree[length - 1]);

                    T item0 = this.LogicalTree.First();
                    this.LogicalTree.AssignParentSelect(item0, length);

                    //item0.Depth = 0;
                    //AssignD1(item0);

                    //for (int i = 1; i < length - 1; i++)
                    //{
                    //    T item = this.LogicalTree[i];
                    //    switch (item.Depth)
                    //    {
                    //        case 0:
                    //            AssignD1(item);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    break;
            }
        }

        // Range: All
        internal void AssignChild2()
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree.Single();

                    ChildrenClear(itemSingle);

                    itemSingle.ParentDeselect();

                    //itemSingle.Depth = 0;
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

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 1;
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        itemFirst.ParentDeselect();
                        itemLast.ParentDeselect();

                        //itemFirst.Depth = 0;
                        //itemLast.Depth = 0;
                    }
                    break;
                default:
                    for (int i = 0; i < this.LogicalTree.Count - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item);
                    }

                    ChildrenClear(this.LogicalTree.Last());

                    T item0 = this.LogicalTree.First();
                    this.LogicalTree.AssignParentSelect(item0);

                    //item0.Depth = 0;
                    //AssignD1(item0);

                    //for (int i = 1; i < this.LogicalTree.Count - 1; i++)
                    //{
                    //    T item = this.LogicalTree[i];
                    //    switch (item.Depth)
                    //    {
                    //        case 0:
                    //            AssignD1(item);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    break;
            }
        }

        // Range: The children of the parent of the item
        internal void AssignChild2(int newIndex, T newItem)
        {
            for (int i = newIndex - 1; i >= 0; i--)
            {
                T item = this.LogicalTree[i];

                if (item.Depth < newItem.Depth)
                {
                    switch (item.SelectMode)
                    {
                        case SelectMode.Selected:
                        case SelectMode.Parent:
                            newItem.SelectMode = SelectMode.Parent;
                            break;
                        default:
                            switch (newItem.SelectMode)
                            {
                                case SelectMode.Parent:
                                    newItem.SelectMode = SelectMode.Deselected;
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }

                    this.ChildrenReset(i, item);

                    //AssignDepth(item, item.Depth + 1);
                    break;
                }
            }
        }

        #endregion

        #region Depth -> Children, Children -> SelectMode, Children -> Depth

        // Range: 0 ~ length
        internal void AssignChild3(int length)
        {
            switch (length)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree[0];

                    ChildrenClear(itemSingle);

                    itemSingle.ParentDeselect();

                    itemSingle.Depth = 0;
                    break;
                case 2:
                    T itemFirst = this.LogicalTree[0];
                    T itemLast = this.LogicalTree[1];

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

                        itemFirst.Depth = 0;
                        itemLast.Depth = 1;
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        itemFirst.ParentDeselect();
                        itemLast.ParentDeselect();

                        itemFirst.Depth = 0;
                        itemLast.Depth = 0;
                    }
                    break;
                default:
                    for (int i = 0; i < length - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item, length);
                    }

                    ChildrenClear(this.LogicalTree[length - 1]);

                    T item0 = this.LogicalTree.First();
                    this.LogicalTree.AssignParentSelect(item0, length);

                    item0.Depth = 0;
                    AssignD1(item0);

                    for (int i = 1; i < length - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        switch (item.Depth)
                        {
                            case 0:
                                AssignD1(item);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
        }

        // Range: All
        internal void AssignChild3()
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    break;
                case 1:
                    T itemSingle = this.LogicalTree.Single();

                    ChildrenClear(itemSingle);

                    itemSingle.ParentDeselect();

                    itemSingle.Depth = 0;
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

                        itemFirst.Depth = 0;
                        itemLast.Depth = 1;
                    }
                    else
                    {
                        ChildrenClear(itemFirst);
                        ChildrenClear(itemLast);

                        itemFirst.ParentDeselect();
                        itemLast.ParentDeselect();

                        itemFirst.Depth = 0;
                        itemLast.Depth = 0;
                    }
                    break;
                default:
                    for (int i = 0; i < this.LogicalTree.Count - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        this.ChildrenReset(i, item);
                    }

                    ChildrenClear(this.LogicalTree.Last());

                    T item0 = this.LogicalTree.First();
                    this.LogicalTree.AssignParentSelect(item0);

                    item0.Depth = 0;
                    AssignD1(item0);

                    for (int i = 1; i < this.LogicalTree.Count - 1; i++)
                    {
                        T item = this.LogicalTree[i];
                        switch (item.Depth)
                        {
                            case 0:
                                AssignD1(item);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
        }

        // Range: The children of the parent of the item
        internal void AssignChild3(int newIndex, T newItem)
        {
            for (int i = newIndex - 1; i >= 0; i--)
            {
                T item = this.LogicalTree[i];

                if (item.Depth < newItem.Depth)
                {
                    switch (item.SelectMode)
                    {
                        case SelectMode.Selected:
                        case SelectMode.Parent:
                            newItem.SelectMode = SelectMode.Parent;
                            break;
                        default:
                            switch (newItem.SelectMode)
                            {
                                case SelectMode.Parent:
                                    newItem.SelectMode = SelectMode.Deselected;
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }

                    this.ChildrenReset(i, item);

                    AssignDepth(item, item.Depth + 1);
                    break;
                }
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

        private void ChildrenReset(int i, T item)
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

        #endregion

        #region Depth

        private static void AssignD1(T parent)
        {
            foreach (T item in parent.Children)
            {
                item.Depth = 1;

                if (item.Children is null)
                    continue;

                if (item.Children.Count is 0)
                    continue;

                AssignDepth(item, 2);
            }
        }

        private static void AssignDepth(T parent, int depth)
        {
            foreach (T item in parent.Children)
            {
                item.Depth = depth;

                if (item.Children is null)
                    continue;

                if (item.Children.Count is 0)
                    continue;

                AssignDepth(item, depth + 1);
            }
        }

        #endregion
    }
}