using Microsoft.Maui.Controls;
using System;
using System.Globalization;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    public class LanguageCommand : CultureInfoCollection, ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            string language = $"{parameter}";

            if (string.IsNullOrEmpty(language))
            {
                SetLanguageEmpty();

                base.XamlRoot.DisplayAlert(
                    UIType.UseSystemSetting.GetString(),
                    UIType.RestartApp.GetString(),
                    UIType.UIBack.GetString()
                    );
            }
            else
            {
                SetLanguage(language);

                base.XamlRoot.DisplayAlert(
                    new CultureInfo(language).NativeName,
                    UIType.RestartApp.GetString(),
                    UIType.UIBack.GetString()
                    );
            }

            //await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync(string.Empty);
        }

        public MenuBarItem ToMenuBarItem()
        {
            MenuBarItem bar = new MenuBarItem
            {
                Text = UIType.Language.GetString(),
            };

            bar.Add(new MenuFlyoutItem
            {
                Text = UIType.UseSystemSetting.GetString(),
                CommandParameter = string.Empty,
                Command = this
            });

            bar.Add(new MenuFlyoutSeparator());

            foreach (CultureInfo item in this)
            {
                bar.Add(new MenuFlyoutItem
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