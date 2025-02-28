using Microsoft.Maui.Controls.Xaml;
using System;

namespace FanKit.Layers.Sample
{
    public class OptionTextExtension : IMarkupExtension<string>
    {
        public OptionType Type { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider) => this.Type.GetString();
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => this.Type.GetString();
    }
}