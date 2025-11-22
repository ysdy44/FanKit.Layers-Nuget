using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace FanKit.Layers.TestApp
{
    internal sealed class Kvp
    {
        public readonly string Key;
        public readonly string Text;
        public readonly Type PageType;

        public Kvp(string key, string text, Type value)
        {
            this.Key = key;
            this.Text = text;
            this.PageType = value;
        }
    }

    public sealed partial class MainPage : Page, ICommand
    {
        //@Instance
        private readonly Lazy<SystemNavigationManager> ManagerLazy = new Lazy<SystemNavigationManager>(() => SystemNavigationManager.GetForCurrentView());
        private readonly Lazy<ApplicationView> ViewLazy = new Lazy<ApplicationView>(() => ApplicationView.GetForCurrentView());
        private SystemNavigationManager Manager => this.ManagerLazy.Value;
        private ApplicationView View => this.ViewLazy.Value;

        readonly IDictionary<string, Kvp> DictionaryKey = (
            from item in Tree
            from kvp in item.Value
            where kvp != null
            select kvp).ToDictionary(c => c.Key);
        readonly IDictionary<string, Kvp> DictionaryText = (
            from item in Tree
            from kvp in item.Value
            where kvp != null
            select kvp).ToDictionary(c => c.Text);

        public MainPage()
        {
            this.InitializeComponent();
            foreach (KeyValuePair<string, Kvp[]> item in Tree)
            {
                MenuFlyoutSubItem subItem = new MenuFlyoutSubItem
                {
                    Text = item.Key,
                };

                foreach (Kvp type in item.Value)
                {
                    if (type == null)
                        subItem.Items.Add(new MenuFlyoutSeparator());
                    else
                        subItem.Items.Add(new MenuFlyoutItem
                        {
                            Text = type.Text,
                            CommandParameter = type.Key,
                            Command = this,
                        });
                }

                this.MenuFlyout.Items.Add(subItem);
            }

            // Register a handler for BackRequested events
            base.Unloaded += delegate { this.Manager.BackRequested -= this.OnBackRequested; };
            base.Loaded += delegate
            {
                this.Manager.BackRequested += this.OnBackRequested;
                this.AutoSuggestBox.Focus(FocusState.Keyboard);
            };

            this.Hyperlink0.Inlines.Add(new Run { Text = nameof(LayersPanelPage) });
            this.Hyperlink0.Click += delegate { this.NavigateKey("LayersPanel"); };

            this.Hyperlink1.Inlines.Add(new Run { Text = nameof(TreeView2Page) });
            this.Hyperlink1.Click += delegate { this.NavigateKey("TreeView2"); };

            this.Hyperlink2.Inlines.Add(new Run { Text = nameof(HistoryPanelPage) });
            this.Hyperlink2.Click += delegate { this.NavigateKey("HistoryPanel"); };

            this.Hyperlink3.Inlines.Add(new Run { Text = nameof(UISyncPage) });
            this.Hyperlink3.Click += delegate { this.NavigateKey("UISync"); };

            this.Hyperlink4.Inlines.Add(new Run { Text = nameof(SelectedRangesPage) });
            this.Hyperlink4.Click += delegate { this.NavigateKey("SelectedRanges"); };

            this.ListView.ItemsSource = this.Overlay.Children.Select(c => ((FrameworkElement)c).Tag).ToArray();
            this.ListView.SelectionChanged += delegate
            {
                int index = this.ListView.SelectedIndex;

                for (int i = 0; i < this.Overlay.Children.Count; i++)
                {
                    UIElement item = this.Overlay.Children[i];

                    item.Visibility = index == i ? Visibility.Visible : Visibility.Collapsed;
                }
                this.Overlay.Visibility = index < 0 ? Visibility.Collapsed : Visibility.Visible;
            };

            this.Overlay.Tapped += delegate { this.ListView.SelectedIndex = -1; };
            this.AutoSuggestBox.SuggestionChosen += (s, e) => this.NavigateText($"{e.SelectedItem}");
            this.AutoSuggestBox.TextChanged += (sender, args) =>
            {
                switch (args.Reason)
                {
                    case AutoSuggestionBoxTextChangeReason.ProgrammaticChange:
                    case AutoSuggestionBoxTextChangeReason.SuggestionChosen:
                        break;
                    default:
                        if (string.IsNullOrEmpty(sender.Text))
                        {
                            sender.ItemsSource = null;
                            this.View.Title = "Pages";
                        }
                        else
                        {
                            string text = sender.Text.ToLower();
                            IEnumerable<string> suitableItems = this.DictionaryText.Keys.Where(x => x.ToLower().Contains(text));

                            int count = suitableItems.Count();
                            if (count is 0)
                            {
                                sender.ItemsSource = null;
                                this.View.Title = "No results found";
                            }
                            else
                            {
                                sender.ItemsSource = suitableItems;
                                this.View.Title = $"{count} results";
                            }
                        }
                        break;
                }
            };
        }

        // Command
        public ICommand Command => this;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => this.NavigateKey($"{parameter}");

        private void NavigateKey(string key)
        {
            if (this.DictionaryKey.ContainsKey(key))
            {
                Kvp item = this.DictionaryKey[key];

                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Navigate(item.PageType);

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = item.Text;
            }
        }
        private void NavigateText(string text)
        {
            if (this.DictionaryText.ContainsKey(text))
            {
                Kvp item = this.DictionaryText[text];

                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Navigate(item.PageType);

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = item.Text;
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            if (this.ContentFrame.CanGoBack)
            {
                this.ListView.SelectedIndex = -1;
                this.ContentFrame.GoBack();

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = this.ContentFrame.Content.GetType().Name;
            }
            else
            {
                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Content = this.ContentPage;

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                this.View.Title = string.Empty;
            }
        }

        static readonly IDictionary<string, Kvp[]> Tree = new Dictionary<string, Kvp[]>
        {
            ["Tree View"] = new Kvp[]
            {
                new Kvp("TreeView0", "Tree View 0", typeof(TreeView0Page)),
                new Kvp("TreeView1", "Tree View 1", typeof(TreeView1Page)),
                new Kvp("TreeView2", "Tree View 2", typeof(TreeView2Page)),
            },
            ["History Panel"] = new Kvp[]
            {
                new Kvp("HistoryPanel", "History Panel", typeof(HistoryPanelPage)),
            },
            ["Nodes View"] = new Kvp[]
            {
                new Kvp("Clipboard", "Clipboard", typeof(ClipboardPage)),
                new Kvp("DragDrop", "Drag & Drop", typeof(DragDropPage)),
                new Kvp("Reorder", "Reorder", typeof(ReorderPage)),
            },
            ["Layers Panel"] = new Kvp[]
            {
                new Kvp("LayersPanel", "Layers Panel", typeof(LayersPanelPage)),
            },
            ["UI/UE"] = new Kvp[]
            {
                new Kvp("LinkedView", "Linked View", typeof(LinkedViewPage)),
                new Kvp("RoundedSelection", "Rounded Selection", typeof(RoundedSelectionPage)),
                new Kvp("Swipe", "Swipe", typeof(SwipePage)),
            },
            ["Others"] = new Kvp[]
            {
                new Kvp("SelectedRanges", "Selected Ranges", typeof(SelectedRangesPage)),
                new Kvp("UISync", "UI Synchronization", typeof(UISyncPage)),
            },
        };
    }
}