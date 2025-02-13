using System.Globalization;
using System.Windows.Input;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.Sample
{
    public class LanguageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
        }
    }
}