using FanKit.Layers.Changes;
using FanKit.Layers.History;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace FanKit.Layers.Sample
{
    public sealed class Undo : IUndoable, INotifyPropertyChanged
    {
        public Guid Id { get; } = Guid.NewGuid();

        public HistoryType Type { get; set; }

        public string Description { get; set; }

        public IChange Change { get; set; }

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
                this.OnPropertyChanged(nameof(SelectBrush));
                this.OnPropertyChanged(nameof(DeprecatedOpacity));
            }
        }
        private TimePeriod period;
        public double SelectOpacity => this.period == TimePeriod.Current ? 0.5d : 0.0d;
        public Brush SelectBrush => this.period == TimePeriod.Current ? Brushes.White : Brushes.Black;
        public double DeprecatedOpacity => this.period == TimePeriod.Future ? 0.5d : 1d;

        //@Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}