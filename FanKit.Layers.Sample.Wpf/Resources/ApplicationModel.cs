using System.Windows;
using System.Diagnostics;

namespace FanKit.Layers.Sample
{
    // await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync(string.Empty);
    public static class ApplicationModel
    {
        public static void RequestRestartAsync()
        {
            // Hide self before Restart
            foreach (Window item in App.Current.Windows)
            {
                item.Hide();
            }

            Process process = Process.GetCurrentProcess();
            string exe = process.MainModule.FileName;

            Process.Start(exe);
            System.Environment.Exit(0);
        }
    }
}