using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public static class ApplicationLanguages
    {
        private static readonly Dictionary<string, string> Manifest = new Dictionary<string, string>
        {
            ["ar"] = "ar",
            ["de"] = "de",
            ["en-US"] = "en",
            ["es"] = "es",
            ["fr"] = "fr",
            ["it"] = "it",
            ["ja"] = "ja",
            ["ko"] = "ko",
            ["nl"] = "nl",
            ["pt"] = "pt",
            ["ru"] = "ru",
            ["zh-CN"] = "zh",
        };

        private static readonly string Config = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        public static readonly string Execute = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FanKit.Layers.Sample.Wpf.exe");

        public static Dictionary<string, string> Instance = new Dictionary<string, string>();

        private static string Language;

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
            get => Language;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Language = string.Empty;

                    Instance.Clear();
                    InitByResw(GetResw(CultureInfo.CurrentUICulture.Name));

                    System.IO.File.WriteAllText(Config, $"lang={string.Empty}");
                }
                else
                {
                    Language = value;

                    Instance.Clear();
                    InitByResw(GetResw(value));

                    System.IO.File.WriteAllText(Config, $"lang={value}");
                }
            }
        }

        static ApplicationLanguages()
        {
            if (System.IO.File.Exists(Config))
            {
                string[] items = System.IO.File.ReadAllLines(Config);

                foreach (string item in items)
                {
                    string[] split = item.Split('=');
                    switch (split.Length)
                    {
                        case 2:
                            switch (split.First())
                            {
                                case "lang":
                                    Language = split.Last();
                                    InitByResw(GetResw(Language));
                                    return;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            Language = string.Empty;
            InitByResw(GetResw(CultureInfo.CurrentUICulture.Name));
        }

        private static string GetResw(string language)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            foreach (KeyValuePair<string, string> item in Manifest)
            {
                string key = item.Key;
                string value = item.Value;

                if (language.Contains(value))
                {
                    return System.IO.Path.Combine(baseDirectory, "Strings", key, "Resources.resw");
                }
            }

            return System.IO.Path.Combine(baseDirectory, "Strings", "en-US", "Resources.resw");
        }

        private static void InitByResw(string reswPath)
        {
            XDocument document = XDocument.Load(reswPath);

            foreach (XElement item in document.Root.Elements())
            {
                switch (item.Name.LocalName)
                {
                    case "data":
                        string key = item.Attribute("name").Value;
                        foreach (XElement child in item.Elements())
                        {
                            string value = child.Value;
                            Instance.Add(key, value);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}