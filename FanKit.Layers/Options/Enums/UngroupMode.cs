namespace FanKit.Layers.Options
{
    internal enum UngroupMode : byte
    {
        None,

        SingleAtLast,
        SingleWithoutChild,

        SingleRangeExpand,
        SingleRangeUnexpand,

        MultipleRanges,
    }
}