using FanKit.Layers.Changes;
using FanKit.Layers.DragDrop;
using FanKit.Layers.History;
using FanKit.Layers.Options;
using FanKit.Layers.Ranges;
using FanKit.Layers.Reorders;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        const double GuideHeight = 16;

        SelectIndexer Indexer;
        IndexSelection Selection;

        // DragItemsStarting
        IndexSelection DragSelection;

        // DragStarting
        DragSourceType DragSourceType;

        // DragOver
        DragOverUIPoint UIPoint;
        DragOverUIRect UIRect;
        DropIndexer DropIndexer;

        public LayerList<ILayer> List => this.M4.List;
        public LayerCollection<ILayer> Collection => this.M4.Collection;

        public DragUI<ILayer> DragUI => this.M4.DragUI;
        public Clipboard<ILayer> Clipboard => this.M4.Clipboard;
        public UndoStack<ILayer, Undo> History => this.M4.History;

        // UI
        public ObservableCollection<ILayer> UILayers => this.M4.UILayers;
        public ObservableCollection<Undo> UIHistory => this.M4.UIHistory;

        readonly LayerManager4<ILayer, Undo> M4 = new LayerManager4<ILayer, Undo>
        {
            DragUI =
            {
                GuideHeight = GuideHeight
            }
        };

        //------------------------ Clipboard ----------------------------//

        public InvalidateModes TryCut()
        {
            this.TryCopy();
            return this.TryRemoveSelection();
        }

        public void TryCopy()
        {
            this.Clipboard.Copy();
        }

        public InvalidateModes TryPaste()
        {
            switch (this.Clipboard.Count)
            {
                case SelectionCount.None:
                    return default;
                case SelectionCount.Single:
                    {
                        Inserter inserter = new Inserter(this.List);

                        ILayer clone = this.Clipboard.CloneSingle(inserter.Depth);

                        clone.SelectMode = SelectMode.Selected;

                        return this.TryInsert(inserter, clone);
                    }
                default:
                    {
                        Guid[] oldIds = this.List.GetIds();

                        Inserter inserter = new Inserter(this.List);
                        SelectChange[] selects = this.List.DeselectAll();
                        InvalidateModes modes = this.Clipboard.Paste(inserter, selects);

                        switch (selects.Length)
                        {
                            case 0:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Sort,
                                    Description = UIType.ToPastes.GetString(),
                                    Change = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    }
                                });
                                break;
                            case 1:
                                SelectChange select = selects.Single();

                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.ToPastes.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Select] = select,
                                    }
                                });
                                break;
                            default:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.ToPastes.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Selects] = new SelectChanges
                                        {
                                            Selects = selects,
                                        },
                                    }
                                });
                                break;
                        }

                        return modes;
                    }
            }
        }

        public InvalidateModes TryDuplicateSelection()
        {
            Duplicator duplicator = new Duplicator(this.List);

            if (duplicator.Count is SelectionCount.None)
                return default;

            Guid[] oldIds = this.List.GetIds();
            InvalidateModes modes = this.Clipboard.Duplicate(duplicator);

            this.PushHistory(new Undo
            {
                Type = HistoryType.Sort,
                Description = UIType.ToDuplicates.GetString(),
                Change = new SortChange
                {
                    OldIds = oldIds,
                    NewIds = this.List.GetIds(),
                }
            });

            return modes;
        }

        public InvalidateModes TryRemoveSelection()
        {
            return this.TryRemove(new Remover(this.List));
        }

        //------------------------ View ----------------------------//

        public InvalidateModes TrySelectAll()
        {
            SelectChange[] selects = this.List.SelectAll();

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
                        Description = UIType.ToShowAll.GetString(),
                        Change = new SelectChanges
                        {
                            Selects = selects
                        }
                    });

                    return InvalidateModes.Select;
            }
        }

        public InvalidateModes TryDeselectAll()
        {
            SelectChange[] selects = this.List.DeselectAll();

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
                        Description = UIType.ToDeselect.GetString(),
                        Change = select,
                    });

                    return InvalidateModes.Select;
                default:
                    this.Collection.ApplySelects(selects);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Selects,
                        Description = UIType.ToDeselectAll.GetString(),
                        Change = new SelectChanges
                        {
                            Selects = selects
                        }
                    });

                    return InvalidateModes.Select;
            }
        }

        public InvalidateModes TryHideAll()
        {
            BooleanChange[] visibles = this.List.HideAll();

            switch (visibles.Length)
            {
                case 0:
                    return default;
                case 1:
                    BooleanChange visible = visibles.Single();

                    this.Collection.ApplyVisible(visible);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Visible,
                        Description = UIType.ToHide.GetString(),
                        Change = visible
                    });

                    return InvalidateModes.Visible;
                default:
                    this.Collection.ApplyVisibles(visibles);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Visibles,
                        Description = UIType.ToHideAll.GetString(),
                        Change = new VisibleChanges
                        {
                            Visibles = visibles
                        }
                    });

                    return InvalidateModes.Visible;
            }
        }

        public InvalidateModes TryShowAll()
        {
            BooleanChange[] visibles = this.List.ShowAll();

            switch (visibles.Length)
            {
                case 0:
                    return default;
                case 1:
                    BooleanChange visible = visibles.Single();

                    this.Collection.ApplyVisible(visible);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Visible,
                        Description = UIType.ToShow.GetString(),
                        Change = visible
                    });

                    return InvalidateModes.Visible;
                default:
                    this.Collection.ApplyVisibles(visibles);

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Visibles,
                        Description = UIType.ToShowAll.GetString(),
                        Change = new VisibleChanges
                        {
                            Visibles = visibles
                        }
                    });

                    return InvalidateModes.Visible;
            }
        }

        //------------------------ Remov ----------------------------//

        public InvalidateModes TryClear()
        {
            switch (this.List.Count)
            {
                case 0:
                    return InvalidateModes.None;
                default:
                    Guid[] oldIds = this.List.GetIds();

                    InvalidateModes modes = this.Collection.Clear();

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Sort,
                        Description = UIType.ToClearAll.GetString(),
                        Change = new SortChange
                        {
                            OldIds = oldIds
                        }
                    });

                    return modes;
            }
        }

        public InvalidateModes TryRemove(Remover remover)
        {
            switch (remover.Count)
            {
                case RemovalCount.None:
                    return default;
                case RemovalCount.Remove:
                    Guid[] oldIds = this.List.GetIds();
                    InvalidateModes modes = this.Collection.Remove(remover);
                    this.TryRemoveCore(oldIds);
                    return modes;
                case RemovalCount.RemoveAll:
                    return this.TryClear();
                default:
                    return default;
            }
        }

        public InvalidateModes TryRemove(IndexSelection selection)
        {
            switch (selection.RemovalCount)
            {
                case RemovalCount.None:
                    return default;
                case RemovalCount.Remove:
                    Guid[] oldIds = this.List.GetIds();
                    InvalidateModes modes = this.Collection.Remove(selection);
                    this.TryRemoveCore(oldIds);
                    return modes;
                case RemovalCount.RemoveAll:
                    return this.TryClear();
                default:
                    return default;
            }
        }

        private void TryRemoveCore(Guid[] oldIds)
        {
            this.PushHistory(new Undo
            {
                Type = HistoryType.Sort,
                Description = UIType.ToRemove.GetString(),
                Change = new SortChange
                {
                    OldIds = oldIds,
                    NewIds = this.List.GetIds(),
                }
            });
        }

        //------------------------ Insert ----------------------------//

        public InvalidateModes TryInsertAtTop(ILayer add)
        {
            add.RenderThumbnail();

            Guid[] oldIds = this.List.GetIds();

            SelectChange[] selects = this.List.DeselectAll();
            InvalidateModes modes = this.Collection.InsertAtTop(add, selects);

            switch (selects.Length)
            {
                case 0:
                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Sort,
                        Description = UIType.InsertLayer.GetString(),
                        Change = new SortChange
                        {
                            OldIds = oldIds,
                            NewIds = this.List.GetIds(),
                        }
                    });
                    break;
                case 1:
                    SelectChange select = selects.Single();

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Group,
                        Description = UIType.InsertLayer.GetString(),
                        Change = new ChangeGroup
                        {
                            [HistoryType.Sort] = new SortChange
                            {
                                OldIds = oldIds,
                                NewIds = this.List.GetIds(),
                            },
                            [HistoryType.Select] = select,
                        }
                    });
                    break;
                default:
                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Group,
                        Description = UIType.InsertLayer.GetString(),
                        Change = new ChangeGroup
                        {
                            [HistoryType.Sort] = new SortChange
                            {
                                OldIds = oldIds,
                                NewIds = this.List.GetIds(),
                            },
                            [HistoryType.Selects] = new SelectChanges
                            {
                                Selects = selects,
                            },
                        }
                    });
                    break;
            }
            return modes;
        }

        public InvalidateModes TryInsert(Inserter inserter, ILayer add)
        {
            add.RenderThumbnail();

            Guid[] oldIds = this.List.GetIds();

            switch (inserter.HasSelected)
            {
                case false:
                    {
                        InvalidateModes modes = this.Collection.InsertAtTop(add);

                        this.PushHistory(new Undo
                        {
                            Type = HistoryType.Sort,
                            Description = UIType.InsertLayer.GetString(),
                            Change = new SortChange
                            {
                                OldIds = oldIds,
                                NewIds = this.List.GetIds(),
                            }
                        });
                        return modes;
                    }
                case true:
                    {
                        SelectChange[] selects = this.List.DeselectAll();

                        InvalidateModes modes = this.Collection.Insert(inserter, add, selects);

                        switch (selects.Length)
                        {
                            case 0:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Sort,
                                    Description = UIType.InsertLayer.GetString(),
                                    Change = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    }
                                });
                                break;
                            case 1:
                                SelectChange select = selects.Single();

                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.InsertLayer.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Select] = select,
                                    }
                                });
                                break;
                            default:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.InsertLayer.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Selects] = new SelectChanges
                                        {
                                            Selects = selects,
                                        },
                                    }
                                });
                                break;
                        }
                        return modes;
                    }
                default:
                    return default;
            }
        }

        //------------------------ Group ----------------------------//

        public InvalidateModes TryGroupSingle(Grouper grouper, ILayer group)
        {
            group.RenderThumbnail();

            Guid[] oldIds = this.List.GetIds();

            Int32Change depth = grouper.DepthOfSingle;
            SelectChange select = grouper.SelectingOfSingle;
            InvalidateModes modes = this.Collection.GroupSingle(grouper, group, depth, select);

            this.PushHistory(new Undo
            {
                Type = HistoryType.Group,
                Description = UIType.ToGroup.GetString(),
                Change = new ChangeGroup
                {
                    [HistoryType.Sort] = new SortChange
                    {
                        OldIds = oldIds,
                        NewIds = this.List.GetIds(),
                    },
                    [HistoryType.Depth] = depth,
                    [HistoryType.Select] = select,
                }
            });

            return modes;
        }

        public InvalidateModes TryGroupMultiple(Grouper grouper, ILayer group)
        {
            group.RenderThumbnail();

            Guid[] oldIds = this.List.GetIds();

            Int32Change[] depths = this.List.GetDepthsForGroupMultiple(grouper);
            SelectChange[] selects = this.List.GetSelectsForGroupMultiple(grouper);
            InvalidateModes modes = this.Collection.GroupMultiple(grouper, group, depths, selects);

            switch (selects.Length)
            {
                case 0:
                    switch (depths.Length)
                    {
                        case 0:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Sort,
                                Description = UIType.ToGroups.GetString(),
                                Change = new SortChange
                                {
                                    OldIds = oldIds,
                                    NewIds = this.List.GetIds(),
                                }
                            });
                            break;
                        case 1:
                            Int32Change depth = depths.Single();

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToGroups.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depth] = depth,
                                }
                            });
                            break;
                        default:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToGroups.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depths] = new DepthChanges
                                    {
                                        Depths = depths,
                                    },
                                }
                            });
                            break;
                    }
                    break;
                case 1:
                    SelectChange select = selects.Single();

                    switch (depths.Length)
                    {
                        case 0:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToGroups.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Select] = select,
                                }
                            });
                            break;
                        case 1:
                            Int32Change depth = depths.Single();

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToGroups.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depth] = depth,
                                    [HistoryType.Select] = select,
                                }
                            });
                            break;
                        default:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToGroups.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depths] = new DepthChanges
                                    {
                                        Depths = depths,
                                    },
                                    [HistoryType.Select] = select,
                                }
                            });
                            break;
                    }
                    break;
                default:
                    switch (depths.Length)
                    {
                        case 0:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.AddGroup.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Selects] = new SelectChanges
                                    {
                                        Selects = selects,
                                    },
                                }
                            });
                            break;
                        case 1:
                            Int32Change depth = depths.Single();

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToGroups.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depth] = depth,
                                    [HistoryType.Selects] = new SelectChanges
                                    {
                                        Selects = selects,
                                    },
                                }
                            });
                            break;
                        default:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToGroups.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depths] = new DepthChanges
                                    {
                                        Depths = depths,
                                    },
                                    [HistoryType.Selects] = new SelectChanges
                                    {
                                        Selects = selects,
                                    },
                                }
                            });
                            break;
                    }
                    break;
            }

            return modes;
        }

        public InvalidateModes TryUngroupSelection()
        {
            Ungrouper ungrouper = new Ungrouper(this.List);

            switch (ungrouper.Count)
            {
                case SelectionCount.None:
                    return default;
                case SelectionCount.Single:
                    {
                        Guid[] oldIds = this.List.GetIds();

                        InvalidateModes modes = this.Collection.UngroupSingle(ungrouper);
                        this.PushHistory(new Undo
                        {
                            Type = HistoryType.Sort,
                            Description = UIType.ToUngroup.GetString(),
                            Change = new SortChange
                            {
                                OldIds = oldIds,
                                NewIds = this.List.GetIds(),
                            }
                        });

                        return modes;
                    }
                case SelectionCount.Multiple:
                    {
                        Guid[] oldIds = this.List.GetIds();

                        Int32Change[] depths = this.List.GetDepthsForUngroupMultiple(ungrouper);
                        SelectChange[] selects = this.List.GetSelectsForUngroupMultiple(ungrouper);
                        InvalidateModes modes = this.Collection.UngroupMultiple(ungrouper, depths, selects);
                        this.PushHistory(new Undo
                        {
                            Type = HistoryType.Sort,
                            Description = UIType.ToUngroups.GetString(),
                            Change = new SortChange
                            {
                                OldIds = oldIds,
                                NewIds = this.List.GetIds(),
                            }
                        });

                        return modes;
                    }
                default:
                    return default;
            }
        }

        public InvalidateModes TryReleaseSingle(Releaser releaser)
        {
            Guid[] oldIds = this.List.GetIds();

            Int32Change depth = releaser.DepthOfSingle;
            InvalidateModes modes = this.Collection.ReleaseSingle(releaser);

            this.PushHistory(new Undo
            {
                Type = HistoryType.Group,
                Description = UIType.ToRelease.GetString(),
                Change = new ChangeGroup
                {
                    [HistoryType.Sort] = new SortChange
                    {
                        OldIds = oldIds,
                        NewIds = this.List.GetIds(),
                    },
                    [HistoryType.Depth] = depth,
                }
            });
            return modes;
        }

        public InvalidateModes TryReleaseMultiple(Releaser releaser)
        {
            Guid[] oldIds = this.List.GetIds();

            Int32Change[] depths = this.List.GetDepthsForRelease();
            InvalidateModes modes = this.Collection.ReleaseMultiple(releaser, depths);

            this.PushHistory(new Undo
            {
                Type = HistoryType.Group,
                Description = UIType.ToReleases.GetString(),
                Change = new ChangeGroup
                {
                    [HistoryType.Sort] = new SortChange
                    {
                        OldIds = oldIds,
                        NewIds = this.List.GetIds(),
                    },
                    [HistoryType.Depths] = new DepthChanges
                    {
                        Depths = depths,
                    },
                }
            });
            return modes;
        }

        public InvalidateModes TryPackage(ILayer group)
        {
            group.RenderThumbnail();

            Guid[] oldIds = this.List.GetIds();

            Int32Change[] depths = this.List.GetDepthsForPackage();
            SelectChange[] selects = this.List.GetSelectsForPackage();
            InvalidateModes modes = this.Collection.Package(group, depths, selects);

            switch (selects.Length)
            {
                case 0:
                    switch (depths.Length)
                    {
                        case 0:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Sort,
                                Description = UIType.AddGroup.GetString(),
                                Change = new SortChange
                                {
                                    OldIds = oldIds,
                                    NewIds = this.List.GetIds(),
                                }
                            });
                            break;
                        case 1:
                            Int32Change depth = depths.Single();

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToPackage.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depth] = depth,
                                }
                            });
                            break;
                        default:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToPackageAll.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depths] = new DepthChanges
                                    {
                                        Depths = depths,
                                    },
                                }
                            });
                            break;
                    }
                    break;
                case 1:
                    SelectChange select = selects.Single();

                    switch (depths.Length)
                    {
                        case 0:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.AddGroup.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Select] = select,
                                }
                            });
                            break;
                        case 1:
                            Int32Change depth = depths.Single();

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToPackage.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depth] = depth,
                                    [HistoryType.Select] = select,
                                }
                            });
                            break;
                        default:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToPackageAll.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depths] = new DepthChanges
                                    {
                                        Depths = depths,
                                    },
                                    [HistoryType.Select] = select,
                                }
                            });
                            break;
                    }
                    break;
                default:
                    switch (depths.Length)
                    {
                        case 0:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.AddGroup.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Selects] = new SelectChanges
                                    {
                                        Selects = selects,
                                    },
                                }
                            });
                            break;
                        case 1:
                            Int32Change depth = depths.Single();

                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToPackage.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depth] = depth,
                                    [HistoryType.Selects] = new SelectChanges
                                    {
                                        Selects = selects,
                                    },
                                }
                            });
                            break;
                        default:
                            this.PushHistory(new Undo
                            {
                                Type = HistoryType.Group,
                                Description = UIType.ToPackageAll.GetString(),
                                Change = new ChangeGroup
                                {
                                    [HistoryType.Sort] = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    },
                                    [HistoryType.Depths] = new DepthChanges
                                    {
                                        Depths = depths,
                                    },
                                    [HistoryType.Selects] = new SelectChanges
                                    {
                                        Selects = selects,
                                    },
                                }
                            });
                            break;
                    }
                    break;
            }

            return modes;
        }

        //------------------------ Drop ----------------------------//

        public InvalidateModes TryDrop(Dropper dropper, ILayer drag)
        {
            drag.RenderThumbnail();

            Guid[] oldIds = this.List.GetIds();

            SelectChange[] selects = this.List.DeselectAll();
            InvalidateModes modes = this.Collection.Insert(dropper, drag, selects);

            switch (selects.Length)
            {
                case 0:
                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Sort,
                        Description = UIType.ToDrop.GetString(),
                        Change = new SortChange
                        {
                            OldIds = oldIds,
                            NewIds = this.List.GetIds(),
                        }
                    });
                    break;
                case 1:
                    SelectChange select = selects.Single();

                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Group,
                        Description = UIType.ToDrop.GetString(),
                        Change = new ChangeGroup
                        {
                            [HistoryType.Sort] = new SortChange
                            {
                                OldIds = oldIds,
                                NewIds = this.List.GetIds(),
                            },
                            [HistoryType.Select] = select,
                        }
                    });
                    break;
                default:
                    this.PushHistory(new Undo
                    {
                        Type = HistoryType.Group,
                        Description = UIType.ToDrop.GetString(),
                        Change = new ChangeGroup
                        {
                            [HistoryType.Sort] = new SortChange
                            {
                                OldIds = oldIds,
                                NewIds = this.List.GetIds(),
                            },
                            [HistoryType.Selects] = new SelectChanges
                            {
                                Selects = selects,
                            },
                        }
                    });
                    break;
            }
            return modes;
        }

        //------------------------ Reorder ----------------------------//

        public InvalidateModes Arrange(ArrangeType type)
        {
            return this.Reorder(new Reorder(this.List, type));
        }

        public InvalidateModes ReorderItems(DropIndexer indexer, IndexSelection selection)
        {
            return this.Reorder(new Reorder(this.List, indexer, selection));
        }

        public InvalidateModes Reorder(Reorder reorder)
        {
            if (reorder.IsSibling)
            {
                Guid[] oldIds = this.List.GetIds();

                InvalidateModes modes = this.Collection.MoveAboveSibling(reorder);

                this.PushHistory(new Undo
                {
                    Type = HistoryType.Sort,
                    Description = UIType.ToMove.GetString(),
                    Change = new SortChange
                    {
                        OldIds = oldIds,
                        NewIds = this.List.GetIds(),
                    }
                });
                return modes;
            }

            switch (reorder.Count)
            {
                case ReorderCount.None:
                    return default;
                case ReorderCount.Single:
                    {
                        Guid[] oldIds = this.List.GetIds();

                        Int32Change depth = reorder.DepthOfSingle;
                        InvalidateModes modes = this.Collection.ReorderSingle(reorder);

                        this.PushHistory(new Undo
                        {
                            Type = HistoryType.Group,
                            Description = UIType.ToMove.GetString(),
                            Change = new ChangeGroup
                            {
                                [HistoryType.Sort] = new SortChange
                                {
                                    OldIds = oldIds,
                                    NewIds = this.List.GetIds(),
                                },
                                [HistoryType.Depth] = depth,
                            }
                        });
                        return modes;
                    }
                case ReorderCount.SingleRange:
                    {
                        Guid[] oldIds = this.List.GetIds();

                        Int32Change[] depths = this.List.GetDepthsForReorderMultiple(reorder);
                        InvalidateModes modes = this.Collection.ReorderSingleRange(reorder, depths);

                        switch (depths.Length)
                        {
                            case 0:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Sort,
                                    Description = UIType.ToMoves.GetString(),
                                    Change = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    }
                                });
                                break;
                            case 1:
                                Int32Change depth = depths.Single();

                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.ToMoves.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Depth] = depth,
                                    }
                                });
                                break;
                            default:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.ToMoves.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Depths] = new DepthChanges
                                        {
                                            Depths = depths,
                                        },
                                    }
                                });
                                break;
                        }
                        return modes;
                    }
                case ReorderCount.Multiple:
                    {
                        Guid[] oldIds = this.List.GetIds();

                        IndexRange[] selectedRanges = this.List.GetSelectedRanges();
                        Int32Change[] depths = this.List.GetDepthsForReorderMultiple(reorder, selectedRanges);
                        InvalidateModes modes = this.Collection.ReorderMultiple(reorder, depths, selectedRanges);

                        switch (depths.Length)
                        {
                            case 0:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Sort,
                                    Description = UIType.ToMoves.GetString(),
                                    Change = new SortChange
                                    {
                                        OldIds = oldIds,
                                        NewIds = this.List.GetIds(),
                                    }
                                });
                                break;
                            case 1:
                                Int32Change depth = depths.Single();

                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.ToMoves.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Depth] = depth,
                                    }
                                });
                                break;
                            default:
                                this.PushHistory(new Undo
                                {
                                    Type = HistoryType.Group,
                                    Description = UIType.ToMoves.GetString(),
                                    Change = new ChangeGroup
                                    {
                                        [HistoryType.Sort] = new SortChange
                                        {
                                            OldIds = oldIds,
                                            NewIds = this.List.GetIds(),
                                        },
                                        [HistoryType.Depths] = new DepthChanges
                                        {
                                            Depths = depths,
                                        },
                                    }
                                });
                                break;
                        }
                        return modes;
                    }
                default:
                    return default;
            }
        }
    }
}