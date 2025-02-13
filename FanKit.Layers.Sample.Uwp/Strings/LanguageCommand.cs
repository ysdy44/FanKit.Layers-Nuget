using System.Globalization;
using System.Windows.Input;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

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

                await new MessageDialog(
                    UIType.RestartApp.GetString(),
                    UIType.UseSystemSetting.GetString()
                    ).ShowAsync();
            }
            else
            {
                SetLanguage(language);

                await new MessageDialog(
                    UIType.RestartApp.GetString(),
                    new CultureInfo(language).NativeName
                    ).ShowAsync();
            }

            await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync(string.Empty);
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