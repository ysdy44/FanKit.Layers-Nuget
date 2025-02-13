using System;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    public sealed class UndoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IUndoRedo Host;
        public UndoCommand(IUndoRedo host) => this.Host = host;

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, null);
        public bool CanExecute(object parameter) => this.Host.CanUndo();
        public void Execute(object parameter)
        {
            InvalidateModes modes = this.Host.TryUndo();
            this.Host.Invalidate(modes);
        }
    }
}