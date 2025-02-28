using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;

namespace FanKit.Layers.Sample
{
    public class SymbolIconExtension : IMarkupExtension<ImageSource>
    {
        public Symbols Symbol { get; set; }

        public ImageSource ProvideValue(IServiceProvider serviceProvider) => new FileImageSource
        {
            File = this.Symbol.ToFile()
        };
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => (ImageSource)new FileImageSource
        {
            File = this.Symbol.ToFile()
        };
    }
}