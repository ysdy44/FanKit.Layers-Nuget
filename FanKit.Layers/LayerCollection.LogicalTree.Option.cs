using FanKit.Layers.Changes;
using FanKit.Layers.DragDrop;
using FanKit.Layers.Options;
using FanKit.Layers.Ranges;
using System.Linq;

namespace FanKit.Layers
{
    partial class LayerCollection<T>
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.CollapseAll']/*" />
        public void CollapseAll() => this.VisualTree.CollapseAll(this.LogicalTree);

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ExpandAll']/*" />
        public void ExpandAll() => this.VisualTree.ExpandAll(this.LogicalTree);

        //------------------------ Remove ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Remove']/*" />
        public InvalidateModes Remove(Remover remover)
        {
            SelTyp2 type = remover.Source.ToSelTyp2(remover.ItemsCount);
            IndexRange range = remover.Source.Source;
            return this.RemoveCore(type, range);
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Remove2']/*" />
        public InvalidateModes Remove(IndexSelection selection)
        {
            SelTyp2 type = selection.Type3.ToSelTyp2();
            IndexRange range = selection.Source;
            return this.RemoveCore(type, range);
        }

        private InvalidateModes RemoveCore(SelTyp2 type, IndexRange range)
        {
            switch (type)
            {
                case SelTyp2.Non: break;
                case SelTyp2.S: this.LogicalTree.RemoveAt(range.StartIndex); break;
                case SelTyp2.SR: this.LogicalTree.RemoveRange(range); break;
                default: this.LogicalTree.Remove(); break;
            }

            this.AssignChild1();
            this.SyncToVisualTree();
            return InvalidateModes.Sort;
        }

        //------------------------ Clear ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Clear']/*" />
        public InvalidateModes Clear()
        {
            this.LogicalTree.Clear();
            this.VisualTree.Clear();
            return InvalidateModes.Clear;
        }

        //------------------------ Drop ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Insert']/*" />
        public InvalidateModes Insert(Dropper dropper, T newItem, SelectChange[] selects)
        {
            switch (dropper.Location)
            {
                case IdxLoc.Non:
                    return default;
                case IdxLoc.Xof:
                    int index = dropper.Index;

                    this.Pool.Add(newItem.Id, newItem);
                    this.LogicalTree.Insert(index, newItem);

                    this.ApplySelects(selects);
                    this.AssignChild1();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case IdxLoc.Lst:
                    return this.InsertAtBottom(newItem, selects);
                case IdxLoc.Fst:
                    return this.InsertAtTop(newItem, selects);
                default:
                    return default;
            }
        }

        //------------------------ Insert ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Insert2']/*" />
        public InvalidateModes Insert(Inserter inserter, T newItem, SelectChange[] selects)
        {
            switch (inserter.Placement)
            {
                case InsertPlacement.InsertAtTop:
                    return this.InsertAtTop(newItem, selects);
                case InsertPlacement.InsertAbove:
                    this.Pool.Add(newItem.Id, newItem);
                    this.LogicalTree.Insert(inserter.Index, newItem);

                    this.ApplySelects(selects);

                    this.AssignChild2(inserter.Index, newItem);

                    if (inserter.IsParentExpanded)
                        this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.InsertAtTop']/*" />
        public InvalidateModes InsertAtTop(T newItem)
        {
            this.Pool.Add(newItem.Id, newItem);

            this.LogicalTree.Insert(0, newItem);
            this.VisualTree.Insert(0, newItem);

            return InvalidateModes.Sort;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.InsertAtTop2']/*" />
        public InvalidateModes InsertAtTop(T newItem, SelectChange[] selects)
        {
            this.Pool.Add(newItem.Id, newItem);

            this.LogicalTree.Insert(0, newItem);
            this.VisualTree.Insert(0, newItem);

            this.ApplySelects(selects);
            return InvalidateModes.Sort;
        }

        private InvalidateModes InsertAtBottom(T newItem)
        {
            this.Pool.Add(newItem.Id, newItem);

            this.LogicalTree.Add(newItem);
            this.VisualTree.Add(newItem);

            return InvalidateModes.Sort;
        }

        private InvalidateModes InsertAtBottom(T newItem, SelectChange[] selects)
        {
            this.Pool.Add(newItem.Id, newItem);

            this.LogicalTree.Add(newItem);
            this.VisualTree.Add(newItem);

            this.ApplySelects(selects);
            return InvalidateModes.Sort;
        }

        //------------------------ Ungroup ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.UngroupSingle']/*" />
        public InvalidateModes UngroupSingle(Ungrouper ungrouper)
        {
            this.LogicalTree.RemoveAt(ungrouper.Source.Source.StartIndex);

            this.AssignChild1();
            this.SyncToVisualTree();
            return InvalidateModes.Sort;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.UngroupMultiple']/*" />
        public InvalidateModes UngroupMultiple(Ungrouper ungrouper, Int32Change[] depths, SelectChange[] selects)
        {
            switch (ungrouper.Mode)
            {
                case UngroupMode.None:
                    return default;
                case UngroupMode.SingleAtLast:
                case UngroupMode.SingleWithoutChild:
                    return default;
                case UngroupMode.SingleRangeExpand:
                case UngroupMode.SingleRangeUnexpand:
                    this.LogicalTree.RemoveAt(ungrouper.Source.Source.StartIndex);

                    this.ApplyDepths(depths);
                    this.ApplySelects(selects);
                    this.AssignChild1();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                case UngroupMode.MultipleRanges:
                    this.LogicalTree.Ungroup();

                    this.ApplyDepths(depths);
                    this.ApplySelects(selects);
                    this.AssignChild1();
                    this.SyncToVisualTree();
                    return InvalidateModes.Sort;
                default:
                    return default;
            }
        }

        //------------------------ Group ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.GroupSingle']/*" />
        public InvalidateModes GroupSingle(Grouper grouper, T newItem, Int32Change depth, SelectChange select)
        {
            int index = grouper.Index;

            T item = this.LogicalTree[index];
            int vi = this.VisualTree.IndexOf(item);

            this.Pool.Add(newItem.Id, newItem);

            this.LogicalTree.Insert(index, newItem);

            newItem.Children.Clear();
            newItem.Children.Add(item);

            newItem.OnChildrenCountChanged();

            this.VisualTree[vi] = newItem;

            this.ApplyDepth(depth);
            this.ApplySelect(select);
            this.AssignChild2(index, newItem);
            return InvalidateModes.Sort;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.GroupMultiple']/*" />
        public InvalidateModes GroupMultiple(Grouper grouper, T newItem, Int32Change[] depths, SelectChange[] selects)
        {
            switch (grouper.Source.Source.ToSelTyp2(this.LogicalTree.Count))
            {
                case SelTyp2.Non:
                    return default;
                case SelTyp2.S:
                    return default;
                case SelTyp2.SR:
                    this.Pool.Add(newItem.Id, newItem);
                    this.LogicalTree.Insert(grouper.Index, newItem);

                    this.ApplyDepths(depths);
                    this.ApplySelects(selects);
                    this.AssignChild1();
                    this.SyncToVisualTree();
                    return InvalidateModes.ClearAndSort;
                case SelTyp2.M:
                    this.Pool.Add(newItem.Id, newItem);
                    this.LogicalTree.Insert(grouper.Index, newItem);
                    this.LogicalTree.MoveToFirst(grouper.Index + 2);

                    this.ApplyDepths(depths);
                    this.ApplySelects(selects);
                    this.AssignChild1();
                    this.SyncToVisualTree();
                    return InvalidateModes.ClearAndSort;
                case SelTyp2.AS:
                case SelTyp2.ASR:
                case SelTyp2.AM:
                    return this.Package(newItem, depths, selects);
                default:
                    return default;
            }
        }

        //------------------------ Release ----------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ReleaseSingle']/*" />
        public InvalidateModes ReleaseSingle(Releaser releaser)
        {
            switch (releaser.Source.Type2)
            {
                case SelTyp2.Non:
                    return default;
                case SelTyp2.S:
                    {
                        int index = releaser.Source.Source.Source.StartIndex;

                        T single = this.LogicalTree[index];
                        int depth = single.Depth;

                        for (int i = index; i >= 0; i--)
                        {
                            T item = this.LogicalTree[i];

                            int d = item.Depth;
                            if (d < depth)
                            {
                                single.Depth = d;
                                this.LogicalTree.RemoveAt(index);
                                this.LogicalTree.Insert(i, single);

                                this.ApplyDepth(releaser.DepthOfSingle);

                                this.AssignChild1();
                                this.SyncToVisualTree();
                                return InvalidateModes.Sort;
                            }
                        }

                        return default;
                    }
                case SelTyp2.SR:
                    return default;
                case SelTyp2.M:
                    return default;
                case SelTyp2.AS:
                    return default;
                case SelTyp2.ASR:
                    return default;
                case SelTyp2.AM:
                    return default;
                default:
                    return default;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.ReleaseMultiple']/*" />
        public InvalidateModes ReleaseMultiple(Releaser releaser, Int32Change[] depths)
        {
            switch (releaser.Source.Type2)
            {
                case SelTyp2.Non:
                    return default;
                case SelTyp2.S:
                    return default;
                case SelTyp2.SR:
                    {
                        IndexRange range = releaser.Source.Source.Source;

                        int index = range.StartIndex;

                        T single = this.LogicalTree[index];
                        int depth = single.Depth;

                        for (int i = index; i >= 0; i--)
                        {
                            T item = this.LogicalTree[i];

                            int d = item.Depth;
                            if (d < depth)
                            {
                                this.LogicalTree.MoveRange(range, i);

                                this.ApplyDepths(depths);

                                this.AssignChild1();
                                this.SyncToVisualTree();
                                return InvalidateModes.Sort;
                            }
                        }

                        return default;
                    }
                case SelTyp2.M:
                    IndexRange[] selectedRanges = this.LogicalTree.GetSelectedRanges().ToArray();

                    foreach (IndexRange range in selectedRanges)
                    {
                        T single = this.LogicalTree[range.StartIndex];

                        for (int i = range.StartIndex; i >= 0; i--)
                        {
                            T item = this.LogicalTree[i];

                            if (item.Depth < single.Depth)
                            {
                                switch (range.Length)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        this.LogicalTree.RemoveAt(range.StartIndex);
                                        this.LogicalTree.Insert(i, single);
                                        goto Continue;
                                    default:
                                        this.LogicalTree.MoveRange(range, i);
                                        goto Continue;
                                }
                            }
                        }

                        Continue:
                        continue;
                    }

                    this.ApplyDepths(depths);

                    this.AssignChild1();
                    this.SyncToVisualTree();
                    return (InvalidateModes.Sort);
                case SelTyp2.AS:
                    return default;
                case SelTyp2.ASR:
                    return default;
                case SelTyp2.AM:
                    return default;
                default:
                    return default;
            }
        }

        //------------------------ Package ---------------------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='LayerCollection.Package']/*" />
        public InvalidateModes Package(T newItem, Int32Change[] depths, SelectChange[] selects)
        {
            this.VisualTree.Clear();

            //foreach (T item in this.LogicalTree)
            //{
            //    item.SelectMode = SelectMode.Parent;
            //    item.Depth++;
            //}

            this.Pool.Add(newItem.Id, newItem);
            this.LogicalTree.Insert(0, newItem);
            this.VisualTree.Add(newItem);

            this.ApplySelects(selects);
            this.ApplyDepths(depths);
            this.AssignChild1();

            switch (selects.Length)
            {
                case 0:
                    return InvalidateModes.Sort;
                default:
                    switch (depths.Length)
                    {
                        case 0:
                            return InvalidateModes.Sort;
                        default:
                            return InvalidateModes.ClearAndSort;
                    }
            }
        }
    }
}