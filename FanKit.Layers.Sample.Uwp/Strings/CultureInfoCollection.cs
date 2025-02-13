using System.Collections.Generic;
using System.Globalization;
using Windows.Globalization;
using Windows.UI.Xaml;
using System.Linq;

namespace FanKit.Layers.Sample
{
    public class CultureInfoCollection : List<CultureInfo>
    {
        //@Static
        public static bool IsRightToLeft => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        public static FlowDirection FlowDirection => IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        public CultureInfoCollection() : base(
            from a
            in ApplicationLanguages.ManifestLanguages.OrderBy(OrderBy)
            select new CultureInfo(a))
        {
        }

        private static string OrderBy(string language) => language;

        public static void SetLanguageEmpty()
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == string.Empty) return;
            ApplicationLanguages.PrimaryLanguageOverride = string.Empty;
        }

        public static void SetLanguage(string language)
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == language) return;
            ApplicationLanguages.PrimaryLanguageOverride = language;

            if (string.IsNullOrEmpty(language))
                return;

            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.Language != language)
                {
                    frameworkElement.Language = language;
                }
            }
        }
    }
}