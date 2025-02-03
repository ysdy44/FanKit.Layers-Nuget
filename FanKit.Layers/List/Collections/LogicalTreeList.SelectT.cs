using FanKit.Layers.Changes;
using FanKit.Layers.Ranges;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.Collections
{
    partial class LogicalTreeList<T>
    {
        public IndexRange IndexRangeOf(T newItem, SelectIndexer oldIndexer)
        {
            if (oldIndexer.Id == Guid.Empty)
                return IndexRange.NegativeUnit;

            int oldIndex = oldIndexer.Index;
            if (oldIndex < 0)
                return IndexRange.NegativeUnit;

            if (oldIndex >= this.LogicalTree.Count)
                return IndexRange.NegativeUnit;

            T select = this.LogicalTree[oldIndex];
            if (select.Id != oldIndexer.Id)
                return IndexRange.NegativeUnit;

            if (select.SelectMode != SelectMode.Selected)
                return IndexRange.NegativeUnit;

            int index = this.LogicalTree.IndexOf(newItem);
            if (index == oldIndex)
                return IndexRange.NegativeUnit;

            if (index < oldIndex)
            {
                return new IndexRange(index, oldIndex);
            }
            else
            {
                return new IndexRange(oldIndex, index);
            }
        }

        public IEnumerable<SelectChange> Deselect(T item)
        {
            switch (item.SelectMode)
            {
                case SelectMode.Deselected:
                    break;
                case SelectMode.Selected:
                    yield return new SelectChange
                    {
                        Id = item.Id,
                        OldValue = SelectMode.Selected,
                        NewValue = SelectMode.Deselected
                    };
                    break;
                case SelectMode.Parent:
                    yield return new SelectChange
                    {
                        Id = item.Id,
                        OldValue = SelectMode.Parent,
                        NewValue = SelectMode.Deselected
                    };
                    break;
                default:
                    break;
            }

            int depth = item.Depth;
            int index = this.LogicalTree.IndexOf(item);

            for (int i = index + 1; i < this.LogicalTree.Count; i++)
            {
                T child = this.LogicalTree[i];

                if (child.Depth > depth)
                {
                    if (child.SelectMode != SelectMode.Deselected)
                    {
                        yield return child.ToDeselected();
                    }
                    continue;
                }

                break;
            }
        }

        public IEnumerable<SelectChange> Select(T item)
        {
            switch (item.SelectMode)
            {
                case SelectMode.Deselected:
                    yield return new SelectChange
                    {
                        Id = item.Id,
                        OldValue = SelectMode.Deselected,
                        NewValue = SelectMode.Selected,
                    };
                    break;
                case SelectMode.Selected:
                    break;
                case SelectMode.Parent:
                    yield return new SelectChange
                    {
                        Id = item.Id,
                        OldValue = SelectMode.Parent,
                        NewValue = SelectMode.Selected,
                    };
                    break;
                default:
                    break;
            }

            int depth = item.Depth;
            int index = this.LogicalTree.IndexOf(item);

            for (int i = index + 1; i < this.LogicalTree.Count; i++)
            {
                T child = this.LogicalTree[i];

                if (child.Depth > depth)
                {
                    if (child.SelectMode != SelectMode.Parent)
                    {
                        yield return child.ToParent();
                    }
                    continue;
                }

                break;
            }
        }

        public IEnumerable<SelectChange> SelectOnly(T item)
        {
            switch (item.SelectMode)
            {
                case SelectMode.Deselected:
                    yield return new SelectChange
                    {
                        Id = item.Id,
                        OldValue = SelectMode.Deselected,
                        NewValue = SelectMode.Selected,
                    };
                    break;
                case SelectMode.Selected:
                    break;
                case SelectMode.Parent:
                    yield return new SelectChange
                    {
                        Id = item.Id,
                        OldValue = SelectMode.Parent,
                        NewValue = SelectMode.Selected,
                    };
                    break;
                default:
                    break;
            }

            int depth = item.Depth;
            int index = this.LogicalTree.IndexOf(item);

            for (int i = 0; i < index; i++)
            {
                T child = this.LogicalTree[i];
                if (child.SelectMode != SelectMode.Deselected)
                {
                    yield return child.ToDeselected();
                }
            }

            for (int i = index + 1; i < this.LogicalTree.Count; i++)
            {
                T child = this.LogicalTree[i];

                if (child.Depth > depth)
                {
                    if (child.SelectMode != SelectMode.Parent)
                    {
                        yield return child.ToParent();
                    }
                    continue;
                }

                if (child.SelectMode != SelectMode.Deselected)
                {
                    yield return child.ToDeselected();
                }

                for (int j = i + 1; j < this.LogicalTree.Count; j++)
                {
                    T chjld = this.LogicalTree[j];
                    if (chjld.SelectMode != SelectMode.Deselected)
                    {
                        yield return chjld.ToDeselected();
                    }
                }
                break;
            }
        }
    }
}