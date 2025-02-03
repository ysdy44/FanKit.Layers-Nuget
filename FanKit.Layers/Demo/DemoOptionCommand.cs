using System;
using System.Windows.Input;

namespace FanKit.Layers.Demo
{
    /// <summary/>
    public sealed class DemoOptionCommand : ICommand
    {
        //@Delegate
        /// <summary/>
        public event EventHandler<DemoLayer> Invoked;
        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => parameter != default;
        /// <summary/>
        public void Execute(DemoLayer item) => this.Invoked?.Invoke(this, item);//Delegate
        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if (parameter is DemoLayer item)
            {
                this.Execute(item);
            }
        }
    }
}