using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public static class ApplicationLanguages
    {
        public static IEnumerable<string> ManifestLanguages
        {
            get
            {
                yield return "ar";
                yield return "de";
                yield return "en-US";
                yield return "es";
                yield return "fr";
                yield return "it";
                yield return "ja";
                yield return "ko";
                yield return "nl";
                yield return "pt";
                yield return "ru";
                yield return "zh-CN";
            }
        }

        public static string PrimaryLanguageOverride
        {
            get => App.Resource.Language;
            set => App.Resource.SetLanguage(value);
        }
    }
}