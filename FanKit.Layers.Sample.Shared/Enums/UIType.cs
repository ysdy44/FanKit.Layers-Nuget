namespace FanKit.Layers.Sample
{
    public enum UIType
    {
        None,
        Undo,
        Redo,

        LayerGroup,
        LayerBitmap,
        LayerFill,

        UITreeView,
        UIHistory,

        UICanvas,
        UIDragDrop1,
        UIDragDrop2,
        UIXml0,
        UIXml1,
        UIXml2,

        UILayers,
        UINodes,

        UIDrag,
        UIDrop,

        UIMore,
        UIBack,

        UIGCSuccess,
        UIDispose, // Format: {0}
        UIGCFailed,
        UINotFound,

        InfoDepth,
        InfoIsExpanded,
        InfoIsLocked,
        InfoIsVisible,
        InfoSelectMode,
        InfoChildren,

        AddLayer,
        AddGroup,
        InsertLayer,

        Language,
        RestartApp,
        UseSystemSetting,

        ToClearAll,
        ToDrop,

        ToDuplicate,
        ToDuplicates,

        ToPaste,
        ToPastes,

        ToMove,
        ToMoves,

        ToRemove,
        ToRemoves,

        ToUngroup,
        ToUngroups,

        ToGroup,
        ToGroups,

        ToRelease,
        ToReleases,

        ToPackage,
        ToPackageAll,

        ToUnlock,
        ToLock,

        ToHide,
        ToHideAll,

        ToShow,
        ToShowAll,

        ToDeselect,
        ToDeselects,
        ToDeselectAll,

        ToSelect,
        ToSelects,
        ToSelectAll,
    }
}