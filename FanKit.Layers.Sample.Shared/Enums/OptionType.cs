namespace FanKit.Layers.Sample
{
    public enum OptionType
    {
        //-------------File-------------//

        GC,
        NewByList,
        NewByTree,

        NewByCustomList,
        NewByCustomTree,

        SaveToXmlList,
        LoadFromXmlList,

        SaveToXmlTree,
        LoadFromXmlTree,

        SaveToXmlTreeNodes,
        LoadFromXmlTreeNodes,

        //-------------Edit-------------//

        Cut,
        Copy,
        Paste,

        Duplicate,
        Remove,
        Clear,

        //-------------Layer-------------//

        InsertAtTop,
        Insert,

        Ungroup,
        Group,
        Release,
        Package,

        //-------------Arrange-------------//

        BringToFront,
        SendToBack,

        BringForward,
        SendBackward,

        //-------------View-------------//

        SelectAll,
        DeselectAll,

        CollapseAll,
        ExpandAll,

        HideAll,
        ShowAll,
    }
}