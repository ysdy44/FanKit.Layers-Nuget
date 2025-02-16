using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace FanKit.Layers.Sample
{
    public class CultureInfoCollection : List<CultureInfo>
    {
        //@Static
        public static bool IsRightToLeft => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        public static FlowDirection FlowDirection => IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        public CultureInfoCollection() : base(
            from a
            in ApplicationLanguages.ManifestLanguages
            select new CultureInfo(a))
        {
        }

        public static void SetLanguageEmpty()
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == string.Empty) return;
            ApplicationLanguages.PrimaryLanguageOverride = string.Empty;
        }

        public static void SetLanguage(string language)
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == language) return;
            ApplicationLanguages.PrimaryLanguageOverride = language;
        }
    }
}