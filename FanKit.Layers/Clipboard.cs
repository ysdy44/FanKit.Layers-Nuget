using FanKit.Layers.Changes;
using FanKit.Layers.Core;
using FanKit.Layers.Options;
using FanKit.Layers.Ranges;
using System;
using System.Collections.Generic;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard']/*" />
    public class Clipboard<T> :
        IDisposable
        where T : class, ICloneable<T>, IComposite<T>, ILayerBase, IDisposable
    {
        private readonly IList<T> LogicalTree;
        private readonly IDictionary<Guid, T> Pool;

        private readonly LayerCollection<T> Collection;

        private readonly List<T> Clipbrd = new List<T>();

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.Clipboard']/*" />
        public Clipboard(IList<T> logicalTree, IDictionary<Guid, T> pool, LayerCollection<T> collection)
        {
            this.LogicalTree = logicalTree;
            this.Pool = pool;

            this.Collection = collection;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.Count']/*" />
        public SelectionCount Count
        {
            get
            {
                switch (this.Clipbrd.Count)
                {
                    case 0:
                        return SelectionCount.None;
                    case 1:
                        return SelectionCount.Single;
                    default:
                        return SelectionCount.Multiple;
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.CanCopy']/*" />
        public bool CanCopy()
        {
            foreach (T item in this.LogicalTree)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Selected:
                        return true;
                }
            }
            return false;
        }
        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.CanPaste']/*" />
        public bool CanPaste() => this.Clipbrd.Count > 0;

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.CloneSingle']/*" />
        public T CloneSingle()
        {
            foreach (T item in this.Clipbrd)
            {
                return item.Clone();
            }
            return default;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.CloneSingleWidthDepth']/*" />
        public T CloneSingle(int depth)
        {
            foreach (T item in this.Clipbrd)
            {
                return item.Clone(depth);
            }
            return default;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.Copy']/*" />
        public void Copy()
        {
            foreach (T item in this.Clipbrd)
            {
                item.Children.Clear();
                item.Dispose();
            }
            this.Clipbrd.Clear();

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

                        T parent = item.Clone(0);
                        parent.SelectMode = SelectMode.Deselected;
                        parent.Children.Clear();
                        this.Clipbrd.Add(parent);
                        break;
                    case Relp.Child:
                        T child = item.Clone(item.Depth - relate.Depth);
                        child.SelectMode = SelectMode.Deselected;
                        child.Children.Clear();
                        this.Clipbrd.Add(child);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.Paste']/*" />
        public InvalidateModes Paste(Inserter inserter, SelectChange[] selects)
        {
            switch (inserter.Placement)
            {
                case InsertPlacement.InsertAtTop:
                    {
                        int depth = inserter.Depth;

                        int count = this.Clipbrd.Count;
                        for (int i = 0; i < count; i++)
                        {
                            T item = this.Clipbrd[i];
                            T add = item.Clone(item.Depth + depth);

                            add.SelectMode = add.Depth == 0 ? SelectMode.Selected : SelectMode.Parent;
                            //add.Children.Clear();

                            this.LogicalTree.Insert(i, add);
                            this.Pool.Add(add.Id, add);
                        }

                        this.Collection.ApplySelects(selects);
                        //if (inserter.HasSelected)
                        this.Collection.AssignChild(count);
                        this.Collection.SyncToVisualTree();
                    }
                    return InvalidateModes.Sort;
                case InsertPlacement.InsertAbove:
                    {
                        int index = inserter.Index;
                        int depth = inserter.Depth;

                        for (int i = 0; i < this.Clipbrd.Count; i++)
                        {
                            T item = this.Clipbrd[i];
                            T add = item.Clone(item.Depth + depth);

                            add.SelectMode = SelectMode.Selected;
                            //add.Children.Clear();

                            this.LogicalTree.Insert(index + i, add);
                            this.Pool.Add(add.Id, add);
                        }

                        this.Collection.ApplySelects(selects);
                        this.Collection.AssignAll();
                        this.Collection.SyncToVisualTree();
                    }
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.Duplicate']/*" />
        public InvalidateModes Duplicate(Duplicator duplicator)
        {
            SelTyp2 type = duplicator.Source.Type2;
            IndexRange range = duplicator.Source.Source.Source;
            return this.DuplicateCore(type, range);
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='Clipboard.DuplicateSelection']/*" />
        public InvalidateModes Duplicate(IndexSelection selection)
        {
            SelTyp2 type = selection.Type3.ToSelTyp2();
            IndexRange range = selection.Source;
            return this.DuplicateCore(type, range);
        }

        private InvalidateModes DuplicateCore(SelTyp2 type, IndexRange range)
        {
            switch (type)
            {
                case SelTyp2.Non:
                    return default;
                case SelTyp2.S:
                    {
                        int index = range.StartIndex;

                        T item = this.LogicalTree[index];
                        T clone = item.Clone();

                        this.Pool.Add(clone.Id, clone);
                        this.LogicalTree.Insert(index, clone);

                        this.Collection.AssignChild();
                        this.Collection.SyncToVisualTree();
                    }
                    return InvalidateModes.Sort;
                case SelTyp2.SR:
                    {
                        for (int i = 0; i < range.Length; i++)
                        {
                            T item = this.LogicalTree[i + range.StartIndex];
                            T clone = item.Clone();

                            this.Pool.Add(clone.Id, clone);
                            this.LogicalTree.Insert(i + range.EndIndex + 1, clone);
                        }

                        this.Collection.AssignChild();
                        this.Collection.SyncToVisualTree();
                    }
                    return InvalidateModes.Sort;
                case SelTyp2.M:
                    {
                        Relation relate = Relation.Empty;
                        IndexRange ran = IndexRange.NegativeUnit;

                        int i = 0;
                        while (i < this.LogicalTree.Count)
                        {
                            T item = this.LogicalTree[i];
                            switch (relate.Relate(item))
                            {
                                case Relp.None:
                                    if (ran.IsNegative)
                                    {
                                        relate = Relation.Empty;
                                        ran = IndexRange.NegativeUnit;
                                        i++;
                                        break;
                                    }
                                    else
                                    {
                                        for (int j = 0; j < ran.Length; j++)
                                        {
                                            T jtem = this.LogicalTree[j + ran.StartIndex];
                                            T clone = jtem.Clone();

                                            this.Pool.Add(clone.Id, clone);
                                            this.LogicalTree.Insert(j + ran.EndIndex + 1, clone);
                                        }
                                        i += ran.Length;

                                        relate = Relation.Empty;
                                        ran = IndexRange.NegativeUnit;
                                        i++;
                                        break;
                                    }
                                case Relp.Parent:
                                    if (ran.IsNegative)
                                    {
                                        relate = new Relation(item);
                                        ran = new IndexRange(i);
                                        i++;
                                        break;
                                    }
                                    else
                                    {
                                        for (int j = 0; j < ran.Length; j++)
                                        {
                                            T jtem = this.LogicalTree[j + ran.StartIndex];
                                            T clone = jtem.Clone();

                                            this.Pool.Add(clone.Id, clone);
                                            this.LogicalTree.Insert(j + ran.EndIndex + 1, clone);
                                        }
                                        i += ran.Length;
                                        item = this.LogicalTree[i];

                                        relate = new Relation(item);
                                        ran = new IndexRange(i);
                                        i++;
                                        break;
                                    }
                                case Relp.Child:
                                    if (ran.IsNegative)
                                        ran = new IndexRange(i);
                                    else
                                        ran = new IndexRange(ran.StartIndex, i);
                                    i++;
                                    break;
                                default:
                                    i++;
                                    break;
                            }
                        }

                        if (ran.IsNegative)
                        {
                            this.Collection.AssignChild();
                            this.Collection.SyncToVisualTree();
                            return InvalidateModes.Sort;
                        }

                        for (int j = 0; j < ran.Length; j++)
                        {
                            T jtem = this.LogicalTree[j + ran.StartIndex];
                            T clone = jtem.Clone();

                            this.Pool.Add(clone.Id, clone);
                            this.LogicalTree.Insert(j + ran.EndIndex + 1, clone);
                        }

                        this.Collection.AssignChild();
                        this.Collection.SyncToVisualTree();
                    }
                    return InvalidateModes.Sort;
                case SelTyp2.AS:
                case SelTyp2.ASR:
                case SelTyp2.AM:
                    {
                        int length = this.LogicalTree.Count;
                        for (int i = 0; i < length; i++)
                        {
                            T item = this.LogicalTree[i];
                            T clone = item.Clone();

                            this.Pool.Add(clone.Id, clone);
                            this.LogicalTree.Add(clone);
                        }

                        this.Collection.AssignChild();
                        this.Collection.SyncToVisualTree();
                    }
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (T item in this.Clipbrd)
            {
                item?.Dispose();
            }
            this.Clipbrd.Clear();
        }
    }
}