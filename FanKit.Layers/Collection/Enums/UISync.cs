namespace FanKit.Layers
{
    // UI Synchronize Mode
    internal enum UISync
    {
        None,

        Clear,
        Init,
        Reset,

        OneToMany,
        ManyToOne,

        Remove,
        Add,

        Decrease,
        Increase,

        Sort,
        MoveToLast,
        MoveToFirst,
    }
}