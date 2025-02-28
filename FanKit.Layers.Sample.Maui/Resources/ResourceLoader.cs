using Microsoft.Maui.Storage;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public sealed class ResourceLoader
    {
        private static readonly Dictionary<string, string> Manifest = new Dictionary<string, string>
        {
            [Constants.ar] = "ar",
            [Constants.de] = "de",
            [Constants.en] = "en",
            [Constants.es] = "es",
            [Constants.fr] = "fr",
            [Constants.it] = "it",
            [Constants.ja] = "ja",
            [Constants.ko] = "ko",
            [Constants.nl] = "nl",
            [Constants.pt] = "pt",
            [Constants.ru] = "ru",
            [Constants.zh] = "zh",
        };

        private static readonly string Config = System.IO.Path.Combine(FileSystem.AppDataDirectory, "config.ini");

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

        private System.IO.Stream GetResw(string language)
        {
            Assembly assembly = base.GetType().GetTypeInfo().Assembly;

            foreach (KeyValuePair<string, string> item in Manifest)
            {
                string key = item.Key;
                string value = item.Value;

                if (language.Contains(value))
                {
                    return assembly.GetManifestResourceStream(key);
                }
            }

            return assembly.GetManifestResourceStream(Constants.en);
        }

        private void InitByResw(System.IO.Stream stream)
        {
            XDocument document = XDocument.Load(stream);

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