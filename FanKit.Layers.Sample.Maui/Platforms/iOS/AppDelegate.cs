using Foundation;
using Microsoft.Maui.Hosting;
using Microsoft.Maui;

namespace FanKit.Layers.Sample.Maui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
