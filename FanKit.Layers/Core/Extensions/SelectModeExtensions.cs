namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='SelectModeExtensions']/*" />
    public static class SelectModeExtensions
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='SelectModeExtensions.ToSelectMode']/*" />
        public static SelectMode ToSelectMode(this bool value) => value ? SelectMode.Selected : SelectMode.Deselected;

        /// <include file="doc/docs.xml" path="docs/doc[@for='SelectModeExtensions.IsSelected']/*" />
        public static bool IsSelected(this SelectMode mode) => mode == SelectMode.Selected;

        /// <include file="doc/docs.xml" path="docs/doc[@for='SelectModeExtensions.ToSelectOpacity']/*" />
        public static double ToSelectOpacity(this SelectMode mode)
        {
            switch (mode)
            {
                case SelectMode.Deselected:
                    return 0d;
                case SelectMode.Parent:
                    return 0.5d;
                default:
                    return 1.0d;
            }
        }

        internal static void ParentSelect(this ILayerBase layer)
        {
            switch (layer.SelectMode)
            {
                case SelectMode.Deselected:
                case SelectMode.Selected:
                    layer.SelectMode = SelectMode.Parent;
                    break;
                default:
                    break;
            }
        }

        internal static void ParentDeselect(this ILayerBase layer)
        {
            switch (layer.SelectMode)
            {
                case SelectMode.Parent:
                    layer.SelectMode = SelectMode.Deselected;
                    break;
                default:
                    break;
            }
        }
    }
}