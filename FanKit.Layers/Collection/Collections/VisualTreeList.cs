using FanKit.Layers.Core;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Layers.Collections
{
    public class VisualTreeList<T> : List<T>
        where T : ITreeNode
    {
        private VisualTreeList<T> VisualTree => this;

        internal UISync GetSync(IList<T> items)
        {
            switch (this.VisualTree.Count)
            {
                case 0:
                    switch (items.Count)
                    {
                        case 0:
                            return UISync.None;
                        default:
                            return UISync.Clear;
                    }
                case 1:
                    switch (items.Count)
                    {
                        case 0:
                            return UISync.Init;
                        case 1:
                            if (this.VisualTree.Single().Id == items.Single().Id)
                                return UISync.None;
                            else
                                return UISync.Reset;
                        default:
                            return UISync.OneToMany;
                    }
                default:
                    switch (items.Count)
                    {
                        case 0:
                            return UISync.Init;
                        case 1:
                            return UISync.ManyToOne;
                        default:
                            foreach (T item in this.VisualTree)
                            {
                                item.Settings.UIIndex = -2;
                            }
                            foreach (T item in items)
                            {
                                item.Settings.UIIndex = this.VisualTree.IndexOf(item);
                            }

                            if (this.VisualTree.Count < items.Count)
                            {
                                foreach (T item in items)
                                {
                                    // Any Exits
                                    switch (item.Settings.UIExists)
                                    {
                                        case SyncExists.Source:
                                            return UISync.Decrease;
                                        default:
                                            break;
                                    }
                                }

                                // All Exits
                                return UISync.Remove;
                            }
                            else if (this.VisualTree.Count > items.Count)
                            {
                                foreach (T item2 in items)
                                {
                                    // Any Exits
                                    switch (item2.Settings.UIExists)
                                    {
                                        case SyncExists.Destination:
                                            return UISync.Increase;
                                        default:
                                            break;
                                    }
                                }

                                // All Exits
                                return UISync.Add;
                            }
                            else
                            {
                                int direct = 0;

                                for (int i = 0; i < this.VisualTree.Count; i++)
                                {
                                    T item2 = this.VisualTree[i];
                                    T item1 = items[i];

                                    int index2 = item2.Settings.UIIndex;
                                    int index1 = item1.Settings.UIIndex;

                                    if (index1 == index2)
                                    {
                                    }
                                    else if (index1 < index2)
                                    {
                                        direct--;
                                    }
                                    else
                                    {
                                        direct++;
                                    }
                                }

                                if (direct > 0)
                                {
                                    return UISync.MoveToFirst;
                                }
                                else //  if (direct < 0)
                                {
                                    return UISync.MoveToLast;
                                }
                                /*
                                else
                                {
                                    return Sync.Sort;
                                }
                                 */
                            }
                    }
            }
        }

        public void VisualSortByExpand(IList<T> items)
        {
            switch (items.Count)
            {
                case 0:
                    break;
                case 1:
                    T single = items.Single();

                    this.VisualTree.Clear();
                    this.VisualTree.Add(single);
                    break;
                case 2:
                    T first = items.First();
                    T last = items.Last();

                    if (first.IsExpanded || first.Depth >= last.Depth)
                    {
                        this.VisualTree.Clear();
                        this.VisualTree.Add(first);
                        this.VisualTree.Add(last);
                    }
                    else
                    {
                        this.VisualTree.Clear();
                        this.VisualTree.Add(first);
                    }
                    break;
                default:
                    this.VisualTree.Clear();

                    int depth = 0;
                    bool value = true;

                    foreach (T item in items)
                    {
                        if (value || depth >= item.Depth)
                        {
                            this.VisualTree.Add(item);
                            depth = item.Depth;
                            value = item.IsExpanded;
                        }
                    }
                    break;
            }
        }

        #region ViewAll

        public void CollapseAll(IList<T> items)
        {
            this.VisualTree.Clear();

            foreach (T item in items)
            {
                switch (item.Depth)
                {
                    case 0:
                        item.IsExpanded = false;
                        this.VisualTree.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }

        public void ExpandAll(IList<T> items)
        {
            this.VisualTree.Clear();

            foreach (T item in items)
            {
                switch (item.Depth)
                {
                    case 0:
                        item.IsExpanded = true;
                        this.VisualTree.Add(item);
                        break;
                    default:
                        this.VisualTree.Add(item);
                        break;
                }
            }
        }

        #endregion

        #region Sync

        public void UISyncTo(IList<T> items)
        {
            switch (this.GetSync(items))
            {
                case UISync.None:
                    break;
                case UISync.Clear:
                    items.Clear();
                    break;
                case UISync.Init:
                    foreach (T item in this.VisualTree)
                    {
                        items.Add(item);
                    }
                    break;
                case UISync.Reset:
                    items.Clear();

                    foreach (T item in this.VisualTree)
                    {
                        items.Add(item);
                    }
                    break;
                case UISync.OneToMany:
                    {
                        int index = -1;
                        foreach (T item in this.VisualTree)
                        {
                            index = items.IndexOf(item);
                        }

                        if (index == -1)
                        {
                            goto case UISync.Reset;
                        }

                        for (int i = items.Count - 1; i > index; i--)
                        {
                            items.RemoveAt(i);
                        }

                        for (int i = index - 1; i >= 0; i--)
                        {
                            items.RemoveAt(i);
                        }
                    }
                    break;
                case UISync.ManyToOne:
                    {
                        int index = -1;
                        foreach (T item in items)
                        {
                            index = this.VisualTree.IndexOf(item);
                        }

                        if (index == -1)
                        {
                            goto case UISync.Reset;
                        }

                        for (int i = 0; i < index; i++)
                        {
                            T item = this.VisualTree[i];
                            items.Insert(i, item);
                        }

                        for (int i = index + 1; i < this.VisualTree.Count; i++)
                        {
                            T item = this.VisualTree[i];
                            items.Add(item);
                        }
                    }
                    break;
                case UISync.Remove:
                    {
                        int i0 = items.Count;
                        System.Guid d0;

                        int i1 = this.VisualTree.Count;
                        System.Guid d1;

                        // when VisualTree.Count == 1
                        switch (i1 < 2 ? UISyncRm.Rm0 : UISyncRm.Nx)
                        {
                            case UISyncRm.Nx:
                                // when i1 == 0 && VisualTree.Count >= 2
                                if (i1 <= 0)
                                    goto case UISyncRm.Rm0;

                                i1--;
                                d1 = this.VisualTree[i1].Id;

                                goto case UISyncRm.Ro;
                            case UISyncRm.Rm:
                                if (d1 == d0)
                                    goto case UISyncRm.Nx;
                                items.RemoveAt(i0);

                                goto case UISyncRm.Ro;
                            case UISyncRm.Rm0:
                                items.RemoveAt(0);
                                break;
                            case UISyncRm.Ro:
                                i0--;
                                d0 = items[i0].Id;

                                if (i0 > 0)
                                    goto case UISyncRm.Rm;
                                else if (d1 != d0)
                                    goto case UISyncRm.Rm0;
                                else
                                    break;
                            default:
                                break;
                        }
                    }
                    break;
                case UISync.Add:
                    {
                        int c0 = items.Count;
                        int c1 = this.VisualTree.Count;
                        int count = System.Math.Min(c1, c0);

                        for (int i = 0; i < count; i++)
                        {
                            T item = this.VisualTree[i];

                            if (items[i].Id == item.Id)
                                continue;

                            items.Insert(i, item);

                            c0++;
                            count = System.Math.Min(c1, c0);
                        }

                        for (int i = c0; i < c1; i++)
                        {
                            T item = this.VisualTree[i];

                            items.Add(item);
                        }
                    }
                    break;
                case UISync.Decrease:
                    {
                        int length = this.VisualTree.Count;
                        int oldCount = items.Count;

                        for (int i = 0; i < length; i++)
                        {
                            T item = this.VisualTree[i];

                            while (true)
                            {
                                // 1. Equals Id
                                if (item.Id == items[i].Id)
                                    break;

                                // 2. Replace when Equals Count
                                if (length == items.Count)
                                {
                                    items[i] = item;
                                    break;
                                }

                                // 3. Remove until 1. 2.
                                items.RemoveAt(i);
                            }
                        }

                        int newCount = items.Count;

                        // 4. Remove Last
                        for (int i = newCount - 1; i >= length; i--)
                        {
                            items.RemoveAt(i);
                        }
                    }
                    break;
                case UISync.Increase:
                    {
                        int length = this.VisualTree.Count;
                        int oldCount = items.Count;

                        for (int i = 0; i < oldCount; i++)
                        {
                            T item = this.VisualTree[i];

                            // 1. Equals Id
                            if (item.Id == items[i].Id)
                                continue;

                            // 2. Replace when Equals Count
                            if (length == items.Count)
                            {
                                items[i] = item;
                                continue;
                            }

                            // 3. Insert until 2.
                            items.Insert(i, item);
                        }

                        int newCount = items.Count;

                        // 4. Replace
                        for (int i = oldCount; i < newCount; i++)
                        {
                            T item = this.VisualTree[i];
                            items[i] = item;
                        }

                        // 5. Add Last
                        for (int i = newCount; i < length; i++)
                        {
                            T item = this.VisualTree[i];
                            items.Add(item);
                        }
                    }
                    break;
                case UISync.Sort:
                    for (int i = 0; i < this.VisualTree.Count; i++)
                    {
                        // 1. Equals Id
                        T item = this.VisualTree[i];
                        if (item.Id == items[i].Id)
                            continue;

                        // 2. Replace when Equals Count
                        //if (this.VisualTree.Count == items.Count)
                        {
                            items[i] = item;
                            //continue;
                        }
                    }
                    break;
                case UISync.MoveToLast:
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        T item2 = this.VisualTree[i];
                        T item1 = items[i];

                        int index2 = item2.Settings.UIIndex;
                        int index1 = item1.Settings.UIIndex;

                        switch (item1.Settings.UIExists)
                        {
                            case SyncExists.Both:
                                if (index2 < index1) // <<<<<<<<<<<<<<<<<<<<<<<<<<<<
                                    items.RemoveAt(i);
                                break;
                            default:
                                items.RemoveAt(i);
                                break;
                        }
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        T item2 = this.VisualTree[i];
                        T item1 = items[i];

                        int index2 = item2.Settings.UIIndex;
                        int index1 = item1.Settings.UIIndex;

                        switch (item1.Settings.UIExists)
                        {
                            case SyncExists.Both:
                                if (index2 != index1)
                                    items.Insert(i, item2);
                                break;
                            default:
                                items.Insert(i, item2);
                                break;
                        }
                    }

                    for (int i = items.Count; i < this.VisualTree.Count; i++)
                    {
                        T item = this.VisualTree[i];
                        items.Add(item);
                    }
                    break;
                case UISync.MoveToFirst:
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        T item2 = this.VisualTree[i];
                        T item1 = items[i];

                        int index2 = item2.Settings.UIIndex;
                        int index1 = item1.Settings.UIIndex;

                        switch (item1.Settings.UIExists)
                        {
                            case SyncExists.Both:
                                if (index2 > index1) // >>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    items.RemoveAt(i);
                                break;
                            default:
                                items.RemoveAt(i);
                                break;
                        }
                    }

                    for (int i = 0; i < this.VisualTree.Count; i++)
                    {
                        T item2 = this.VisualTree[i];
                        T item1 = items[i];

                        int index2 = item2.Settings.UIIndex;
                        int index1 = item1.Settings.UIIndex;

                        switch (item1.Settings.UIExists)
                        {
                            case SyncExists.Both:
                                if (index2 != index1)
                                    items.Insert(i, item2);
                                break;
                            default:
                                items.Insert(i, item2);
                                break;
                        }
                    }

                    for (int i = items.Count; i < VisualTree.Count; i++)
                    {
                        T item = this.VisualTree[i];
                        items.Add(item);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}