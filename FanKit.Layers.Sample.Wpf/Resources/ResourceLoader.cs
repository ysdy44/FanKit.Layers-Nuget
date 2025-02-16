namespace FanKit.Layers.Sample
{
    public sealed class ResourceLoader
    {
        public static ResourceLoader GetForCurrentView() => new ResourceLoader();

        public string GetString(string resource)
        {
            if (ApplicationLanguages.Instance.ContainsKey(resource))
            {
                return ApplicationLanguages.Instance[resource];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}