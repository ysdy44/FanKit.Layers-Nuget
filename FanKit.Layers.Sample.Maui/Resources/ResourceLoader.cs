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
        public string GetString(string resource)
        {
            {
                return string.Empty;
            }
        }

        public static ResourceLoader GetForCurrentView()
        {
            return new ResourceLoader
            {
            };
        }
    }
}