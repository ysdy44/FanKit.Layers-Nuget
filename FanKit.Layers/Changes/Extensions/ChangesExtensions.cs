using FanKit.Layers.Changes;
using FanKit.Layers.Core;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='ChangesExtensions']/*" />
    public static class ChangesExtensions
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='ChangesExtensions.ToFalse']/*" />
        public static BooleanChange ToFalse(this ITreeNode item) => new BooleanChange
        {
            Id = item.Id,
            OldValue = true,
            NewValue = false,
        };

        /// <include file="doc/docs.xml" path="docs/doc[@for='ChangesExtensions.ToTrue']/*" />
        public static BooleanChange ToTrue(this ITreeNode item) => new BooleanChange
        {
            Id = item.Id,
            OldValue = false,
            NewValue = true,
        };

        //------------------------ Int32 ----------------------------//

        internal static Int32Change Empty(this ITreeNode item) => new Int32Change
        {
            Id = item.Id,
            OldValue = item.Depth,
            NewValue = item.Depth,
        };

        internal static Int32Change Depth(this ITreeNode item, int depth) => new Int32Change
        {
            Id = item.Id,
            OldValue = item.Depth,
            NewValue = depth,
        };

        internal static Int32Change Different(this ITreeNode item, int diff) => new Int32Change
        {
            Id = item.Id,
            OldValue = item.Depth,
            NewValue = item.Depth - diff,
        };

        internal static Int32Change Decrease(this ITreeNode item) => new Int32Change
        {
            Id = item.Id,
            OldValue = item.Depth,
            NewValue = item.Depth - 1
        };

        internal static Int32Change Increase(this ITreeNode item) => new Int32Change
        {
            Id = item.Id,
            OldValue = item.Depth,
            NewValue = item.Depth + 1
        };

        //------------------------ Select ----------------------------//

        internal static SelectChange ToDeselected(this ILayerBase item) => new SelectChange
        {
            Id = item.Id,
            OldValue = item.SelectMode,
            NewValue = SelectMode.Deselected
        };

        internal static SelectChange ToSelected(this ILayerBase item) => new SelectChange
        {
            Id = item.Id,
            OldValue = item.SelectMode,
            NewValue = SelectMode.Selected
        };

        internal static SelectChange ToParent(this ILayerBase item) => new SelectChange
        {
            Id = item.Id,
            OldValue = item.SelectMode,
            NewValue = SelectMode.Parent
        };
    }
}