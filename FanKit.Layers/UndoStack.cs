using FanKit.Layers.Changes;
using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using FanKit.Layers.History;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack']/*" />
    public class UndoStack<T, U>
        where T : class, ICloneable<T>, IComposite<T>, ILayerBase, IDisposable
        where U : class, IUndoable
    {
        // History
        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.UndoLimit']/*" />
        public int UndoLimit { get => this.History.UndoLimit; set => this.History.UndoLimit = value; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.SelectedIndex']/*" />
        public int SelectedIndex { get => this.History.CurrentIndex; private set => this.History.CurrentIndex = value; }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.SelectedItem']/*" />
        public U SelectedItem => this.History.CurrentItem;

        private readonly IDictionary<Guid, T> Pool;

        private readonly LayerCollection<T> Collection;

        private readonly UndoList<U> History = new UndoList<U>();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.UndoStack']/*" />
        public UndoStack(IDictionary<Guid, T> pool, LayerCollection<T> collection)
        {
            this.Pool = pool;

            this.Collection = collection;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.UndoBuffer']/*" />
        public IEnumerable<U> UndoBuffer() => this.History.BackStack();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.RedoBuffer']/*" />
        public IEnumerable<U> RedoBuffer() => this.History.ForwardStack();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ClearUndoRedoHistory']/*" />
        public void ClearUndoRedoHistory()
        {
            this.History.CurrentIndex = -1;
            foreach (U item in this.History)
            {
                item?.Change?.Dispose();
            }
            this.History.Clear();
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.UISyncTime']/*" />
        public void UISyncTime() => this.History.UISyncTime();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.UISyncTimeIndex']/*" />
        public void UISyncTime(int index) => this.History.UISyncTime(index);

        /// <inheritdoc cref="List{T}.IndexOf(T)"/>
        public int IndexOf(U item) => this.History.IndexOf(item);

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.CanUndo']/*" />
        public bool CanUndo() => this.History.CanGoBack();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.CanRedo']/*" />
        public bool CanRedo() => this.History.CanGoForward();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.CanNavigate']/*" />
        public NavigateAction CanNavigate(int index) => this.History.CanNavigate(index);

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.Undo']/*" />
        public U Undo() => this.History.GoBack();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.Redo']/*" />
        public U Redo() => this.History.GoForward();

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.NavigateTo']/*" />
        public IndexRange NavigateTo(int index) => this.History.NavigateTo(index);

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.GetRange']/*" />
        public IEnumerable<U> GetRange(IndexRange range) => this.History.GetRange(range);

        // LayerManager.History.Clone.cs
        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.UISyncTo']/*" />
        public void UISyncTo(IList<U> items) => this.History.UISyncTo(items);

        #region History

        private int GG() // Good GC
        {
            foreach (KeyValuePair<Guid, T> item in this.Pool)
            {
                //item.Value.Settings.ReferenceCount = 0;
                item.Value.Settings.Exits = false;
            }

            foreach (T item in this.Collection)
            {
                item.Settings.Exits = true;
            }

            //foreach (U item in this.List)
            //{
            //    this.Decrement(item.Operation);
            //}

            int gcs = 0;
            for (Guid id = this.FindUselessId(); id != Guid.Empty; id = this.FindUselessId())
            {
                // Dispose
                this.Pool[id].Dispose();
                this.Pool.Remove(id);
                gcs++;
            }
            return gcs;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.GC']/*" />
        public int GC()
        {
            foreach (KeyValuePair<Guid, T> item in this.Pool)
            {
                item.Value.Settings.ReferenceCount = 0;
                item.Value.Settings.Exits = false;
            }

            foreach (T item in this.Collection)
            {
                item.Settings.Exits = true;
            }

            foreach (U item in this.History)
            {
                this.Decrement(item.Change);
            }

            int gcs = 0;
            for (Guid id = this.FindUselessId(); id != Guid.Empty; id = this.FindUselessId())
            {
                // Dispose
                this.Pool[id].Dispose();
                this.Pool.Remove(id);
                gcs++;
            }
            return gcs;
        }

        private Guid FindUselessId()
        {
            foreach (KeyValuePair<Guid, T> item in this.Pool)
            {
                if (item.Value.Settings.Exits)
                {
                    continue;
                }
                else if (item.Value.Settings.ReferenceCount > 0)
                {
                    continue;
                }
                else
                {
                    return item.Key;
                }
            }
            return Guid.Empty;
        }

        #endregion

        #region Push

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.Push']/*" />
        public int Push(U item, bool isGC)
        {
            int removes = 0;
            if (this.History.Count > 0)
            {
                int startIndex = this.SelectedIndex + 1;
                int endIndex = this.History.Count - 1;
                removes = 1 + endIndex - startIndex;

                for (int i = endIndex; i >= startIndex; i--)
                {
                    using (IChange remove = this.History[i].Change)
                    {
                        this.Increment(remove);
                        this.History.RemoveAt(i);
                    }
                }
            }

            if (this.History.Count >= this.UndoLimit)
            {
                removes++;
                this.Increment(this.History[0].Change);
                this.History.RemoveAt(0);
                this.History.Add(item);
                this.Decrement(item.Change);
            }
            else
            {
                this.SelectedIndex++;
                this.History.Add(item);
                this.Decrement(item.Change);
            }

            if (isGC)
            {
                switch (removes)
                {
                    case 0:
                        return 0;
                    default:
                        return this.GG();
                }
            }
            else
            {
                return 0;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Increment(IChange change)
        {
            IEnumerable<Guid> ids = change.ReferenceGuids;
            if (ids == null)
                return;

            foreach (Guid id in ids)
            {
                this.Pool[id].Settings.ReferenceCount--;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Decrement(IChange change)
        {
            IEnumerable<Guid> ids = change.ReferenceGuids;
            if (ids == null)
                return;

            foreach (Guid id in ids)
            {
                this.Pool[id].Settings.ReferenceCount++;
            }
        }

        #endregion

        #region Apply

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoSort']/*" />
        public InvalidateModes ApplyUndoSort(SortChange change)
        {
            if (change.OldIds is null)
            {
                this.Collection.Clear();
                return InvalidateModes.ClearUndo;
            }
            else
            {
                this.Collection.Sort(change.OldIds);

                this.Collection.SyncToVisualTree();
                return InvalidateModes.SortUndo;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoSort']/*" />
        public InvalidateModes ApplyRedoSort(SortChange change)
        {
            if (change.NewIds is null)
            {
                this.Collection.Clear();
                return InvalidateModes.ClearUndo;
            }
            else
            {
                this.Collection.Sort(change.NewIds);

                this.Collection.SyncToVisualTree();
                return InvalidateModes.SortUndo;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoDepth']/*" />
        public InvalidateModes ApplyUndoDepth(Int32Change change)
        {
            this.Pool[change.Id].Depth = change.OldValue;
            return InvalidateModes.Output;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoDepth']/*" />
        public InvalidateModes ApplyRedoDepth(Int32Change change)
        {
            this.Pool[change.Id].Depth = change.NewValue;
            return InvalidateModes.Output;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoDepths']/*" />
        public InvalidateModes ApplyUndoDepths(DepthChanges changes)
        {
            foreach (Int32Change item in changes.Depths)
            {
                this.Pool[item.Id].Depth = item.OldValue;
            }
            return InvalidateModes.Output;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoDepths']/*" />
        public InvalidateModes ApplyRedoDepths(DepthChanges changes)
        {
            foreach (Int32Change item in changes.Depths)
            {
                this.Pool[item.Id].Depth = item.NewValue;
            }
            return InvalidateModes.Output;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoLock']/*" />
        public InvalidateModes ApplyUndoLock(BooleanChange change)
        {
            this.Pool[change.Id].IsLocked = change.OldValue;
            return InvalidateModes.LockUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoLock']/*" />
        public InvalidateModes ApplyRedoLock(BooleanChange change)
        {
            this.Pool[change.Id].IsLocked = change.NewValue;
            return InvalidateModes.LockUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoVisible']/*" />
        public InvalidateModes ApplyUndoVisible(BooleanChange change)
        {
            this.Pool[change.Id].IsVisible = change.OldValue;
            return InvalidateModes.VisibleUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoVisible']/*" />
        public InvalidateModes ApplyRedoVisible(BooleanChange change)
        {
            this.Pool[change.Id].IsVisible = change.NewValue;
            return InvalidateModes.VisibleUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoVisibles']/*" />
        public InvalidateModes ApplyUndoVisibles(VisibleChanges changes)
        {
            foreach (BooleanChange item in changes.Visibles)
            {
                this.Pool[item.Id].IsVisible = item.OldValue;
            }
            return InvalidateModes.VisibleUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoVisibles']/*" />
        public InvalidateModes ApplyRedoVisibles(VisibleChanges changes)
        {
            foreach (BooleanChange item in changes.Visibles)
            {
                this.Pool[item.Id].IsVisible = item.NewValue;
            }
            return InvalidateModes.VisibleUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoSelect']/*" />
        public InvalidateModes ApplyUndoSelect(SelectChange change)
        {
            this.Pool[change.Id].SelectMode = change.OldValue;
            return InvalidateModes.SelectUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoSelect']/*" />
        public InvalidateModes ApplyRedoSelect(SelectChange change)
        {
            this.Pool[change.Id].SelectMode = change.NewValue;
            return InvalidateModes.SelectUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoSelects']/*" />
        public InvalidateModes ApplyUndoSelects(SelectChanges changes)
        {
            foreach (SelectChange item in changes.Selects)
            {
                this.Pool[item.Id].SelectMode = item.OldValue;
            }
            return InvalidateModes.SelectUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoSelects']/*" />
        public InvalidateModes ApplyRedoSelects(SelectChanges changes)
        {
            foreach (SelectChange item in changes.Selects)
            {
                this.Pool[item.Id].SelectMode = item.NewValue;
            }
            return InvalidateModes.SelectUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyUndoDoubleSelect']/*" />
        public InvalidateModes ApplyUndoDoubleSelect(DoubleSelectChange changes)
        {
            this.Pool[changes.Select0.Id].SelectMode = changes.Select0.OldValue;
            this.Pool[changes.Select1.Id].SelectMode = changes.Select1.OldValue;
            return InvalidateModes.SelectUndo;
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='UndoStack.ApplyRedoDoubleSelect']/*" />
        public InvalidateModes ApplyRedoDoubleSelect(DoubleSelectChange changes)
        {
            this.Pool[changes.Select0.Id].SelectMode = changes.Select0.NewValue;
            this.Pool[changes.Select1.Id].SelectMode = changes.Select1.NewValue;
            return InvalidateModes.SelectUndo;
        }

        #endregion
    }
}