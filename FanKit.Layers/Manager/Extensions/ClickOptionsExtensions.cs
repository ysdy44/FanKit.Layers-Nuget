using FanKit.Layers.Core;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='ClickOptionsExtensions']/*" />
    public static class ClickOptionsExtensions
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='ClickOptionsExtensions.CanSelect']/*" />
        public static ClickOptions CanSelect(this ILayerBase layer)
        {
            switch (layer.SelectMode)
            {
                case SelectMode.Deselected: return ClickOptions.Select;
                case SelectMode.Selected: return ClickOptions.Deselect;
                default: return ClickOptions.None;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='ClickOptionsExtensions.CanSelectWithKey']/*" />
        public static ClickOptions CanSelect(this ILayerBase layer, bool isShiftKeyDown, bool isControlKeyDown)
            => isShiftKeyDown ? ClickOptions.SelectRangeOnly : isControlKeyDown ? layer.CanSelect() : ClickOptions.SelectOnly;

        /// <include file="doc/docs.xml" path="docs/doc[@for='ClickOptionsExtensions.CanVisible']/*" />
        public static ClickOptions CanVisible(this ILayerBase layer) => layer.IsVisible ? ClickOptions.Hide : ClickOptions.Show;

        /// <include file="doc/docs.xml" path="docs/doc[@for='ClickOptionsExtensions.CanLock']/*" />
        public static ClickOptions CanLock(this ILayerBase layer) => layer.IsLocked ? ClickOptions.Unlock : ClickOptions.Lock;

        /// <include file="doc/docs.xml" path="docs/doc[@for='ClickOptionsExtensions.CanExpand']/*" />
        public static ClickOptions CanExpand(this ITreeNode node) => node.IsExpanded ? ClickOptions.Collapse : ClickOptions.Expand;
    }
}