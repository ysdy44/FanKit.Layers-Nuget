using System;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    public sealed class RedoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IUndoRedo Host;
        public RedoCommand(IUndoRedo host) => this.Host = host;

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, null);
        public bool CanExecute(object parameter) => this.Host.CanRedo();
        public void Execute(object parameter)
        {
            InvalidateModes modes = this.Host.TryRedo();
            this.Host.Invalidate(modes);
        }
    }
}