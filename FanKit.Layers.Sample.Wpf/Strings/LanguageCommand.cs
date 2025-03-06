using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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

                MessageBox.Show(
                    UIType.RestartApp.GetString(),
                    UIType.UseSystemSetting.GetString()
                    );
            }
            else
            {
                SetLanguage(language);

                MessageBox.Show(
                    UIType.RestartApp.GetString(),
                    new CultureInfo(language).NativeName
                    );
            }

            ApplicationModel.RequestRestartAsync();
        }

        public MenuItem ToMenuBarItem()
        {
            MenuItem bar = new MenuItem
            {
                Header = UIType.Language.GetString(),
                Items =
                {
                    new MenuItem
                    {
                        Header = UIType.UseSystemSetting.GetString(),
                        CommandParameter = string.Empty,
                        Command = this
                    },
                    new Separator(),
                }
            };

            foreach (CultureInfo item in this)
            {
                bar.Items.Add(new MenuItem
                {
                    Header = item.NativeName,
                    CommandParameter = item.Name,
                    Command = this
                });
            }

            return bar;
        }
    }
}