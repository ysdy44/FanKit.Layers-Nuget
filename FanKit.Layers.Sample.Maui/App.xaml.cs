using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace FanKit.Layers.Sample
{
    public partial class App : Application
    {
        internal static readonly ResourceLoader Resource = ResourceLoader.GetForCurrentView();

        public App()
        {
            this.InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}