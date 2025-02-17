using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.Globalization;

namespace FanKit.Layers.Sample
{
    public class CultureInfoCollection : List<CultureInfo>
    {
        //@Static
        public static bool IsRightToLeft => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        public static FlowDirection FlowDirection => IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        public XamlRoot XamlRoot { get; set; }

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

        public void SetLanguage(string language)
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == language) return;
            ApplicationLanguages.PrimaryLanguageOverride = language;

            if (string.IsNullOrEmpty(language))
                return;

            if (this.XamlRoot.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.Language != language)
                {
                    frameworkElement.Language = language;
                }
            }
        }
    }
}