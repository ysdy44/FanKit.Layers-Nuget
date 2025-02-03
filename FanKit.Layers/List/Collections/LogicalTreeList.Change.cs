using FanKit.Layers.Changes;
using FanKit.Layers.Ranges;
using System.Collections.Generic;

namespace FanKit.Layers.Collections
{
    partial class LogicalTreeList<T>
    {
        #region SelectRange

        public IEnumerable<SelectChange> SelectRangeOnly(IndexRange range)
        {
            // 1. Begin -> First
            for (int i = 0; i < range.StartIndex; i++)
            {
                T item = this.LogicalTree[i];
                if (item.SelectMode != SelectMode.Deselected)
                {
                    yield return item.ToDeselected();
                }
            }

            // 2. Start
            int f = range.StartIndex;
            T itemFirst = this.LogicalTree[f];

            int d = itemFirst.Depth;

            if (itemFirst.SelectMode != SelectMode.Selected)
            {
                yield return itemFirst.ToSelected();
            }

            // 3. Start -> Last
            for (int i = range.StartIndex + 1; i < this.LogicalTree.Count; i++)
            {
                T item = this.LogicalTree[i];

                if (item.Depth > d)
                {
                    // Children
                    if (item.SelectMode != SelectMode.Parent)
                    {
                        yield return item.ToParent();
                    }
                }
                else if (i > range.EndIndex)
                {
                    // 4. End -> Last
                    for (int j = i; j < this.LogicalTree.Count; j++)
                    {
                        T jtem = this.LogicalTree[j];
                        if (jtem.SelectMode != SelectMode.Deselected)
                        {
                            yield return jtem.ToDeselected();
                        }
                    }
                    break;
                }
                else
                {
                    d = item.Depth;

                    // Parent
                    if (item.SelectMode != SelectMode.Selected)
                    {
                        yield return item.ToSelected();
                    }
                }
            }
        }

        #endregion

        #region View

        public IEnumerable<BooleanChange> HideAll()
        {
            foreach (T item in this.LogicalTree)
            {
                if (item.IsVisible)
                    yield return item.ToFalse();
            }
        }

        public IEnumerable<BooleanChange> ShowAll()
        {
            foreach (T item in this.LogicalTree)
            {
                if (item.IsVisible)
                    continue;

                yield return item.ToTrue();
            }
        }

        public IEnumerable<SelectChange> SelectAll()
        {
            foreach (T item in this.LogicalTree)
            {
                switch (item.Depth)
                {
                    case 0:
                        if (item.SelectMode != SelectMode.Selected)
                        {
                            yield return item.ToSelected();
                        }
                        break;
                    default:
                        if (item.SelectMode != SelectMode.Parent)
                        {
                            yield return item.ToParent();
                        }
                        break;
                }
            }
        }

        public IEnumerable<SelectChange> DeselectAll()
        {
            foreach (T item in this.LogicalTree)
            {
                if (item.SelectMode != SelectMode.Deselected)
                {
                    yield return item.ToDeselected();
                }
            }
        }

        #endregion

        #region Group

        public IEnumerable<Int32Change> DepthGroup(int depth)
        {
            Relation relate = Relation.Empty;

            foreach (T item in this.LogicalTree)
            {
                switch (relate.Relate(item))
                {
                    case Relp.None:
                        relate = Relation.Empty;
                        break;
                    case Relp.Parent:
                        relate = new Relation(item);

                        yield return item.Depth(depth);
                        break;
                    case Relp.Child:
                        yield return item.Depth(depth + item.Depth - relate.Depth);
                        break;
                    default:
                        break;
                }
            }
        }

        public IEnumerable<Int32Change> DepthUngroup()
        {
            int depth = -1;
            foreach (T item in this.LogicalTree)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Selected:
                        depth = item.IsGroup ? item.Depth : -1;
                        break;
                    default:
                        if (depth == -1)
                            break;

                        if (item.Depth > depth)
                            yield return item.Decrease();
                        else
                            depth = -1;
                        break;
                }
            }
        }

        public IEnumerable<Int32Change> DepthRelease()
        {
            foreach (T item in this.LogicalTree)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        break;
                    case SelectMode.Selected:
                        switch (item.Depth)
                        {
                            case 0:
                            default:
                                yield return item.Decrease();
                                break;
                        }
                        break;
                    case SelectMode.Parent:
                        switch (item.Depth)
                        {
                            case 0:
                            case 1:
                                break;
                            default:
                                yield return item.Decrease();
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Group

        public IEnumerable<SelectChange> DeselectAllGroupChild()
        {
            int depth = -1;
            foreach (T item in this.LogicalTree)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        depth = -1;
                        break;
                    case SelectMode.Selected:
                        if (item.IsGroup)
                        {
                            depth = item.Depth;
                        }
                        else
                        {
                            depth = -1;
                        }
                        break;
                    case SelectMode.Parent:
                        if (depth != -1)
                        {
                            if (item.Depth > depth)
                            {
                                yield return item.ToDeselected();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public IEnumerable<SelectChange> DeselectChildren(IndexRange range)
        {
            for (int i = range.StartIndex + 1; i <= range.EndIndex; i++)
            {
                T item = this.LogicalTree[i];
                if (item.SelectMode != SelectMode.Deselected)
                {
                    yield return item.ToDeselected();
                }
            }
        }

        public IEnumerable<SelectChange> DeselectParentSelect(IndexRange range)
        {
            for (int i = range.StartIndex; i <= range.EndIndex; i++)
            {
                T item = this.LogicalTree[i];
                if (item.SelectMode != SelectMode.Parent)
                {
                    yield return item.ToParent();
                }
            }
        }

        public IEnumerable<SelectChange> DeselectParentSelectAll()
        {
            foreach (T item in this.LogicalTree)
            {
                if (item.SelectMode != SelectMode.Parent)
                {
                    yield return item.ToParent();
                }
            }
        }

        public IEnumerable<SelectChange> DeselectParentSelectAllDeselect()
        {
            foreach (T item in this.LogicalTree)
            {
                if (item.SelectMode != SelectMode.Deselected)
                {
                    yield return item.ToParent();
                }
            }
        }

        #endregion

        #region Reorder

        public IEnumerable<Int32Change> ResetDepths(IndexRange range)
        {
            T itemFirst = this.LogicalTree[range.StartIndex];

            int d = itemFirst.Depth;
            int diff = d - 0;
            yield return itemFirst.Depth(0);

            for (int i = range.StartIndex + 1; i <= range.EndIndex; i++)
            {
                T item = this.LogicalTree[i];

                yield return item.Different(diff);
            }
        }

        public IEnumerable<Int32Change> ResetDepths(IndexRange range, int depth)
        {
            T itemFirst = this.LogicalTree[range.StartIndex];

            int d = itemFirst.Depth;
            int diff = d - depth;
            yield return itemFirst.Depth(depth);

            for (int i = range.StartIndex + 1; i <= range.EndIndex; i++)
            {
                T item = this.LogicalTree[i];

                yield return item.Different(diff);
            }
        }

        public IEnumerable<Int32Change> ResetDepths(IEnumerable<IndexRange> ranges)
        {
            foreach (IndexRange range in ranges)
            {
                T itemFirst = this.LogicalTree[range.StartIndex];

                int d = itemFirst.Depth;
                int diff = d - 0;
                yield return itemFirst.Depth(0);

                for (int i = range.StartIndex + 1; i <= range.EndIndex; i++)
                {
                    T item = this.LogicalTree[i];

                    yield return item.Different(diff);
                }
            }
        }

        public IEnumerable<Int32Change> ResetDepths(IEnumerable<IndexRange> ranges, int depth)
        {
            foreach (IndexRange range in ranges)
            {
                T itemFirst = this.LogicalTree[range.StartIndex];

                int d = itemFirst.Depth;
                int diff = d - depth;
                yield return itemFirst.Depth(depth);

                for (int i = range.StartIndex + 1; i <= range.EndIndex; i++)
                {
                    T item = this.LogicalTree[i];

                    yield return item.Different(diff);
                }
            }
        }

        #endregion
    }
}