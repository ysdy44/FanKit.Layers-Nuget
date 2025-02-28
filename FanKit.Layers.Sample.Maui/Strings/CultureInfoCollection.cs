using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FanKit.Layers.Sample
{
    public class CultureInfoCollection : List<CultureInfo>
    {
        //@Static
        public static bool IsRightToLeft => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        public static FlowDirection FlowDirection => IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        public Page XamlRoot { get; set; }

        public CultureInfoCollection() : base(
            from a
            in ApplicationLanguages.ManifestLanguages
            select new CultureInfo(a))
        {
        }

        public void SetLanguageEmpty()
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == string.Empty) return;
            ApplicationLanguages.PrimaryLanguageOverride = string.Empty;
        }

        public void SetLanguage(string language)
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == language) return;
            ApplicationLanguages.PrimaryLanguageOverride = language;
        }
    }
}