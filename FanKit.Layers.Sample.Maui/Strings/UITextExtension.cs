using Microsoft.Maui.Controls.Xaml;
using System;

namespace FanKit.Layers.Sample
{
    public class UITextExtension : IMarkupExtension<string>
    {
        public UIType Type { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider) => this.Type.GetString();
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => this.Type.GetString();
    }
}