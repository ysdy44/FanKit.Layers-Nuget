using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FanKit.Layers.Sample
{
    public sealed partial class InfoDialog : Window
    {
        public InfoDialog(ILayer layer)
        {
            this.InitializeComponent();
            base.Title = layer.Title.ToString();
            this.CloseButton.Content = UIType.UIBack.GetString();
            this.CloseButton.Click += delegate { base.Close(); };

            this.ListView.ItemsSource = layer.Children;
            this.HeaderBorder.Child = new TextBlock
            {
                Inlines =
                {
                    new Run { Text = UIType.InfoDepth.GetString() },
                    new Run { Text = layer.Depth.ToString() },
                    new LineBreak(),

                    new Run { Text = UIType.InfoIsExpanded.GetString() },
                    new Run { Text = layer.IsExpanded.ToString() },
                    new LineBreak(),

                    new Run { Text = UIType.InfoIsLocked.GetString() },
                    new Run { Text = layer.IsLocked.ToString() },
                    new LineBreak(),

                    new Run { Text = UIType.InfoIsVisible.GetString() },
                    new Run { Text = layer.IsVisible.ToString() },
                    new LineBreak(),

                    new Run { Text = UIType.InfoSelectMode.GetString() },
                    new Run { Text = layer.SelectMode.ToString() },
                    new LineBreak(),

                    new Run { Text = UIType.InfoChildren.GetString() },
                    new LineBreak(),
                }
            };
        }
    }
}