using FanKit.Layers.Changes;
using FanKit.Layers.History;
using System;
using System.ComponentModel;

namespace FanKit.Layers.Demo
{
    /// <summary/>
    public class DemoUndo : IUndoable, IDisposable, INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary/>
        public string Description { get; set; }

        /// <summary/>
        public DemoChange DemoChange { get; } = new DemoChange();

        /// <inheritdoc/>
        public IChange Change => this.DemoChange;

        /// <inheritdoc/>
        public TimePeriod Period
        {
            get => this.period;
            set
            {
                if (this.period == value)
                    return;
                this.period = value;
                this.OnPropertyChanged(nameof(Period));
                this.OnPropertyChanged(nameof(SelectOpacity));
                this.OnPropertyChanged(nameof(DeprecatedOpacity));
            }
        }
        private TimePeriod period;
        /// <summary/>
        public double SelectOpacity => this.period == TimePeriod.Current ? 0.5d : 0.0d;
        /// <summary/>
        public double DeprecatedOpacity => this.period == TimePeriod.Future ? 0.5d : 1d;

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}