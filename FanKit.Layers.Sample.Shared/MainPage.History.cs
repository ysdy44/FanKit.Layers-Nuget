using FanKit.Layers.Changes;
using FanKit.Layers.History;
using System.Collections.Generic;

namespace FanKit.Layers.Sample
{
    partial class MainPage : IUndoRedo
    {
        public IEnumerable<RectChange> GetRectChanges()
        {
            foreach (ILayer item in this.Collection)
            {
                switch (item.Type)
                {
                    case LayerType.Group:
                        break;
                    default:
                        switch (item.SelectMode)
                        {
                            case SelectMode.Deselected:
                                break;
                            default:
                                if (item.StartingRect != item.Rect)
                                {
                                    yield return new RectChange(item.Id, item.StartingRect, item.Rect);
                                }
                                break;
                        }
                        break;
                }
            }
        }

        //------------------------ Undo & Redo ----------------------------//

        public bool CanUndo()
        {
            return this.History.CanUndo();
        }

        public bool CanRedo()
        {
            return this.History.CanRedo();
        }

        public InvalidateModes TryRedo()
        {
            if (this.History.CanRedo())
            {
                Undo item = this.History.Redo();
                InvalidateModes result = this.ApplyRedo(item);

                return result | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
            }
            else
                return default;
        }

        public InvalidateModes TryUndo()
        {
            if (this.History.CanUndo())
            {
                Undo item = this.History.Undo();
                InvalidateModes result = this.ApplyUndo(item);

                return result | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
            }
            else
                return default;
        }

        public InvalidateModes TryNavigate(Undo undo)
        {
            int index = this.History.IndexOf(undo);

            switch (this.History.CanNavigate(index))
            {
                case NavigateAction.None:
                    return default;
                case NavigateAction.Undo:
                    {
                        Undo item = this.History.Undo();
                        InvalidateModes result = this.ApplyUndo(item);

                        return result | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                    }
                case NavigateAction.Redo:
                    {
                        Undo item = this.History.Redo();
                        InvalidateModes result = this.ApplyRedo(item);

                        return result | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                    }
                case NavigateAction.UndoRange:
                    IndexRange undoRange = this.History.NavigateTo(index);
                    lock (this)
                    {
                        InvalidateModes result = default;

                        foreach (Undo item in this.History.GetRange(undoRange))
                        {
                            result |= this.ApplyUndo(item);
                        }

                        return result | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                    }
                case NavigateAction.RedoRange:
                    IndexRange redoRange = this.History.NavigateTo(index);
                    lock (this)
                    {
                        InvalidateModes result = default;

                        foreach (Undo item in this.History.GetRange(redoRange))
                        {
                            result |= this.ApplyRedo(item);
                        }

                        return result | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                    }
                default:
                    return default;
            }
        }

        //------------------------ Apply ----------------------------//

        public InvalidateModes ApplyUndo(Undo undo)
        {
            switch (undo.Type)
            {
                case HistoryType.Group:
                    ChangeGroup group = (ChangeGroup)undo.Change;

                    InvalidateModes result = default;

                    foreach (KeyValuePair<HistoryType, IChange> item in group)
                    {
                        result |= this.ApplyUndo(item.Key, item.Value);
                    }

                    return result;
                default:
                    return this.ApplyUndo(undo.Type, undo.Change);
            }
        }

        private InvalidateModes ApplyUndo(HistoryType type, IChange change)
        {
            switch (type)
            {
                case HistoryType.Sort: return this.History.ApplyUndoSort((SortChange)change);
                case HistoryType.Depth: return this.History.ApplyUndoDepth((Int32Change)change);
                case HistoryType.Depths: return this.History.ApplyUndoDepths((DepthChanges)change);
                case HistoryType.Lock: return this.History.ApplyUndoLock((BooleanChange)change);
                case HistoryType.Visible: return this.History.ApplyUndoVisible((BooleanChange)change);
                case HistoryType.Visibles: return this.History.ApplyUndoVisibles((VisibleChanges)change);
                case HistoryType.Select: return this.History.ApplyUndoSelect((SelectChange)change);
                case HistoryType.Selects: return this.History.ApplyUndoSelects((SelectChanges)change);
                case HistoryType.DoubleSelect: return this.History.ApplyUndoDoubleSelect((DoubleSelectChange)change);
                case HistoryType.Rect:
                    RectChange rect = (RectChange)change;

                    this.Collection[rect.Id].Rect = rect.OldValue;

                    return InvalidateModes.ValueChangedUndo;
                case HistoryType.Rects:
                    RectChanges rects = (RectChanges)change;

                    foreach (RectChange item in rects.Rects)
                    {
                        this.Collection[item.Id].Rect = item.OldValue;
                    }

                    return InvalidateModes.ValueChangedUndo;
                default:
                    return default;
            }
        }

        public InvalidateModes ApplyRedo(Undo undo)
        {
            switch (undo.Type)
            {
                case HistoryType.Group:
                    ChangeGroup group = (ChangeGroup)undo.Change;

                    InvalidateModes result = default;

                    foreach (KeyValuePair<HistoryType, IChange> item in group)
                    {
                        result |= this.ApplyRedo(item.Key, item.Value);
                    }

                    return result;
                default:
                    return this.ApplyRedo(undo.Type, undo.Change);
            }
        }

        private InvalidateModes ApplyRedo(HistoryType type, IChange change)
        {
            switch (type)
            {
                case HistoryType.Sort: return this.History.ApplyRedoSort((SortChange)change);
                case HistoryType.Depth: return this.History.ApplyRedoDepth((Int32Change)change);
                case HistoryType.Depths: return this.History.ApplyRedoDepths((DepthChanges)change);
                case HistoryType.Lock: return this.History.ApplyRedoLock((BooleanChange)change);
                case HistoryType.Visible: return this.History.ApplyRedoVisible((BooleanChange)change);
                case HistoryType.Visibles: return this.History.ApplyRedoVisibles((VisibleChanges)change);
                case HistoryType.Select: return this.History.ApplyRedoSelect((SelectChange)change);
                case HistoryType.Selects: return this.History.ApplyRedoSelects((SelectChanges)change);
                case HistoryType.DoubleSelect: return this.History.ApplyRedoDoubleSelect((DoubleSelectChange)change);
                case HistoryType.Rect:
                    RectChange rect = (RectChange)change;

                    this.Collection[rect.Id].Rect = rect.NewValue;

                    return InvalidateModes.ValueChangedUndo;
                case HistoryType.Rects:
                    RectChanges rects = (RectChanges)change;

                    foreach (RectChange item in rects.Rects)
                    {
                        this.Collection[item.Id].Rect = item.NewValue;
                    }

                    return InvalidateModes.ValueChangedUndo;
                default:
                    return default;
            }
        }
    }
}