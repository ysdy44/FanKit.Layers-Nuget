using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public sealed class ResourceLoader
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

        public Dictionary<string, string> Instance = new Dictionary<string, string>();

        public string Language { get; private set; }

        public ResourceLoader(string language)
        {
            this.InitByResw(this.GetResw(language));
        }

        public void SetLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                this.Language = string.Empty;

                this.Instance.Clear();
                this.InitByResw(this.GetResw(CultureInfo.CurrentUICulture.Name));

                System.IO.File.WriteAllText(Config, $"lang={string.Empty}");
            }
            else
            {
                this.Language = language;

                this.Instance.Clear();
                this.InitByResw(this.GetResw(language));

                System.IO.File.WriteAllText(Config, $"lang={language}");
            }
        }

        private string GetResw(string language)
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

        private void InitByResw(string reswPath)
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
                            this.Instance.Add(key, value);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public string GetString(string resource)
        {
            if (this.Instance.ContainsKey(resource))
            {
                return this.Instance[resource];
            }
            else
            {
                return string.Empty;
            }
        }

        public static ResourceLoader GetForCurrentView()
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
                                    string language = split.Last();

                                    return new ResourceLoader(language)
                                    {
                                        Language = language
                                    };
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return new ResourceLoader(CultureInfo.CurrentUICulture.Name)
            {
                Language = string.Empty
            };
        }
    }
}