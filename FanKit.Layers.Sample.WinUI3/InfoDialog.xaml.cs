using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;

namespace FanKit.Layers.Sample
{
    public sealed partial class InfoDialog : ContentDialog
    {
        public InfoDialog(ILayer layer)
        {
            this.InitializeComponent();
            base.Title = layer.Title.ToString();
            base.CloseButtonText = UIType.UIBack.GetString();
            //base.CloseButtonClick += delegate { base.Hide(); };

            this.ListView.ItemsSource = layer.Children;
            this.ListView.Header = new TextBlock
            {
                IsTextSelectionEnabled = true,
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