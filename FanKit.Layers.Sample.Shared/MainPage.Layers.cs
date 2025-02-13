using FanKit.Layers.Changes;
using FanKit.Layers.History;
using System.Linq;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        private InvalidateModes Click(ClickOptions type, ILayer item)
        {
            switch (type)
            {
                case ClickOptions.None:
                    return default;
                case ClickOptions.Deselect:
                    SelectChange[] deselect = this.List.Deselect(item);

                    switch (deselect.Length)
                    {
                        case 0:
                            return default;
                        case 1:
                            SelectChange select = deselect.Single();
                            this.Collection.ApplySelect(select);

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Select,
                                Description = UIType.ToDeselect.GetString(),
                                Change = select,
                            });

                            return InvalidateModes.Select;
                        default:
                            this.Collection.ApplySelects(deselect);

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Selects,
                                Description = UIType.ToDeselects.GetString(),
                                Change = new SelectChanges
                                {
                                    Selects = deselect
                                }
                            });

                            return InvalidateModes.Select;
                    }
                case ClickOptions.Select:
                    this.Indexer = this.List.IndexerOf(item);

                    SelectChange[] selects = this.List.Select(this.Indexer);

                    switch (selects.Length)
                    {
                        case 0:
                            return default;
                        case 1:
                            SelectChange select = selects.Single();
                            this.Collection.ApplySelect(select);

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Select,
                                Description = UIType.ToSelect.GetString(),
                                Change = select,
                            });

                            return InvalidateModes.Select;
                        default:
                            this.Collection.ApplySelects(selects);

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Selects,
                                Description = UIType.ToSelects.GetString(),
                                Change = new SelectChanges
                                {
                                    Selects = selects
                                }
                            });

                            return InvalidateModes.Select;
                    }
                case ClickOptions.SelectOnly:
                    this.Indexer = this.List.IndexerOf(item);

                    SelectChange[] selectOnly = this.List.SelectOnly(this.Indexer);

                    switch (selectOnly.Length)
                    {
                        case 0:
                            return default;
                        case 1:
                            SelectChange select = selectOnly.Single();
                            this.Collection.ApplySelect(select);

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Select,
                                Description = UIType.ToSelect.GetString(),
                                Change = select,
                            });

                            return InvalidateModes.Select;
                        case 2:
                            SelectChange first = selectOnly.First();
                            SelectChange last = selectOnly.Last();
                            this.Collection.ApplySelect(first);
                            this.Collection.ApplySelect(last);

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.DoubleSelect,
                                Description = UIType.ToSelect.GetString(),
                                Change = new DoubleSelectChange
                                {
                                    Select0 = first,
                                    Select1 = last,
                                }
                            });

                            return InvalidateModes.Select;
                        default:
                            this.Collection.ApplySelects(selectOnly);

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Selects,
                                Description = UIType.ToSelects.GetString(),
                                Change = new SelectChanges
                                {
                                    Selects = selectOnly
                                }
                            });

                            return InvalidateModes.Select;
                    }
                case ClickOptions.SelectRangeOnly:
                    {
                        IndexRange selectRange = this.List.IndexRangeOf(item, this.Indexer);
                        if (selectRange.IsNegative)
                            return default;

                        SelectChange[] selectRangeOnly = this.List.SelectRangeOnly(selectRange);
                        switch (selectRangeOnly.Length)
                        {
                            case 0:
                                return default;
                            case 1:
                                SelectChange select = selectRangeOnly.Single();
                                this.Collection.ApplySelect(select);

                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Select,
                                    Description = UIType.ToSelect.GetString(),
                                    Change = select,
                                });

                                return InvalidateModes.Select;
                            default:
                                this.Collection.ApplySelects(selectRangeOnly);

                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Selects,
                                    Description = UIType.ToSelects.GetString(),
                                    Change = new SelectChanges
                                    {
                                        Selects = selectRangeOnly
                                    }
                                });

                                return InvalidateModes.Select;
                        }
                    }
                case ClickOptions.Hide:
                    BooleanChange hide = item.ToFalse();
                    this.Collection.ApplyVisible(hide);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Visible,
                        Description = UIType.ToHide.GetString(),
                        Change = hide
                    });

                    return InvalidateModes.Visible;
                case ClickOptions.Show:
                    BooleanChange show = item.ToTrue();
                    this.Collection.ApplyVisible(show);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Visible,
                        Description = UIType.ToShow.GetString(),
                        Change = show
                    });

                    return InvalidateModes.Visible;
                case ClickOptions.Lock:
                    BooleanChange locks = item.ToTrue();
                    this.Collection.ApplyLock(locks);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Lock,
                        Description = UIType.ToLock.GetString(),
                        Change = locks
                    });

                    return InvalidateModes.Lock;
                case ClickOptions.Unlock:
                    BooleanChange unlock = item.ToFalse();
                    this.Collection.ApplyLock(unlock);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Lock,
                        Description = UIType.ToUnlock.GetString(),
                        Change = unlock
                    });

                    return InvalidateModes.Lock;
                case ClickOptions.Collapse:
                    item.IsExpanded = false;
                    this.Collection.SyncToVisualTree();

                    return InvalidateModes.Expand;
                case ClickOptions.Expand:
                    item.IsExpanded = true;
                    this.Collection.SyncToVisualTree();

                    return InvalidateModes.Expand;
                default:
                    return default;
            }
        }
    }
}