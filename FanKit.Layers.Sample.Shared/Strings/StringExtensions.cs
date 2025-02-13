namespace FanKit.Layers.Sample
{
    public static class StringExtensions
    {
        public static string GetString(this OptionCatalog catalog)
        {
            return App.Resource.GetString($"{catalog}");
        }

        public static string GetString(this OptionType type)
        {
            return App.Resource.GetString($"{type}");
        }

        public static string GetString(this UIType type)
        {
            return App.Resource.GetString($"{type}");
        }
    }
}