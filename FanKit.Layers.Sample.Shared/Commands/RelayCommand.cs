using System;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    public sealed class RelayCommand : ICommand
    {
        public event EventHandler<object> Invoked;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => this.Invoked?.Invoke(this, parameter);//Delegate
    }
}