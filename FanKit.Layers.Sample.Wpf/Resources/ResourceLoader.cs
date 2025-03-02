using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    public sealed class ResourceLoader
    {
        public static readonly string Execute = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FanKit.Layers.Sample.Wpf.exe");

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