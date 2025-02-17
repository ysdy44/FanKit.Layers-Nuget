using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Globalization;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    public class LanguageCommand : CultureInfoCollection, ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public async void Execute(object parameter)
        {
            string language = $"{parameter}";

            if (string.IsNullOrEmpty(language))
            {
                SetLanguageEmpty();

                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    CloseButtonText = UIType.UIBack.GetString(),
                    Title = UIType.RestartApp.GetString(),
                    Content = UIType.UseSystemSetting.GetString()
                }.ShowAsync();
            }
            else
            {
                SetLanguage(language);

                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    CloseButtonText = UIType.UIBack.GetString(),
                    Title = UIType.RestartApp.GetString(),
                    Content = new CultureInfo(language).NativeName
                }.ShowAsync();
            }

            Microsoft.Windows.AppLifecycle.AppInstance.Restart(string.Empty);
        }

        public MenuBarItem ToMenuBarItem()
        {
            MenuBarItem bar = new MenuBarItem
            {
                Title = UIType.Language.GetString(),
                Items =
                {
                    new MenuFlyoutItem
                    {
                        Text = UIType.UseSystemSetting.GetString(),
                        CommandParameter = string.Empty,
                        Command = this
                    },
                    new MenuFlyoutSeparator(),
                }
            };

            foreach (CultureInfo item in this)
            {
                bar.Items.Add(new MenuFlyoutItem
                {
                    Text = item.NativeName,
                    CommandParameter = item.Name,
                    Command = this
                });
            }

            return bar;
        }
    }
}