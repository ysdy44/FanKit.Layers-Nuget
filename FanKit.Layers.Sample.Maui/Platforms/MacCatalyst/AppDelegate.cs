﻿using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace FanKit.Layers.Sample.Maui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
