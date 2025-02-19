using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.TestApp
{
    public sealed partial class SwipePage : Page
    {
        int count;

        public SwipePage()
        {
            this.InitializeComponent();
        }

        private void SelectCommand_Invoked(object sender, SwipeLayer e)
        {
            this.count++;

            if (this.count == 1)
                this.CounterTextBlock.Text = $"Swiped {this.count} time";
            else
                this.CounterTextBlock.Text = $"Swiped {this.count} times";

            e.SelectMode = e.SelectMode == SelectMode.Deselected ? SelectMode.Selected : SelectMode.Deselected;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView listView = (ListView)sender;
            SwipeLayerCollection collection = (SwipeLayerCollection)((ItemsControl)listView).ItemsSource;
            int index = collection.IndexOf((SwipeLayer)e.ClickedItem);

            for (int i = 0; i < collection.Count; i++)
            {
                SwipeLayer item = collection[i];
                item.SelectMode = i == index ? SelectMode.Selected : SelectMode.Deselected;
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggle = (ToggleSwitch)sender;

            SwipeView.Mode = toggle.IsOn ? SwipeViewMode.Reveal : SwipeViewMode.Execute;
        }
    }

    public class SwipeOptionCommand : ICommand
    {
        public event EventHandler<SwipeLayer> Invoked;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter != default;
        public void Execute(SwipeLayer item) => this.Invoked?.Invoke(this, item);//Delegate
        public void Execute(object parameter)
        {
            if (parameter is SwipeLayer item)
            {
                this.Execute(item);
            }
        }
    }

    public class SwipeLayerCollection : List<SwipeLayer>
    {
        public SwipeLayerCollection() : base(from c in Enumerable.Range(0, 10)
                                             select new SwipeLayer
                                             {
                                                 Title = c.ToString()
                                             })
        {
        }
    }

    public class SwipeLayer : INotifyPropertyChanged
    {
        public string Title { get; set; }

        public SelectMode SelectMode
        {
            get => this.selectMode;
            set
            {
                if (this.selectMode == value)
                    return;

                this.selectMode = value;
                this.OnPropertyChanged(nameof(SelectOpacity));
            }
        }
        private SelectMode selectMode = SelectMode.Deselected;
        public double SelectOpacity => this.selectMode == SelectMode.Deselected ? 0.0 : 1.0;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}