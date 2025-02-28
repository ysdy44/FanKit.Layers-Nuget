using FanKit.Layers.Changes;
using FanKit.Layers.History;
using Microsoft.Maui.Graphics;
using System;
using System.ComponentModel;

namespace FanKit.Layers.Sample
{
    public sealed class Undo : IUndoable, INotifyPropertyChanged
    {
        private static readonly Color SelectedColor = Colors.DodgerBlue.WithAlpha(0.5f);

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
                this.OnPropertyChanged(nameof(SelectColor));
                this.OnPropertyChanged(nameof(DeprecatedOpacity));
            }
        }
        private TimePeriod period;
        public double SelectOpacity => this.period == TimePeriod.Current ? 0.5d : 0.0d;
        public Color SelectColor => this.Period == TimePeriod.Current ? SelectedColor : Colors.Transparent;
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