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
        public readonly string Key1;
        public readonly string Key2;
        public readonly Type PageType;

        public Kvp(string text, string key, Type value)
        {
            this.Key1 = text;
            this.Key2 = key;
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

        readonly IDictionary<string, Kvp> Dictionary1 = (
            from item in Tree
            from kvp in item.Value
            where kvp != null
            select kvp).ToDictionary(c => c.Key2);
        readonly IDictionary<string, Kvp> Dictionary2 = (
            from item in Tree
            from kvp in item.Value
            where kvp != null
            select kvp).ToDictionary(c => c.Key1);

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
                            Text = type.Key1,
                            CommandParameter = type.Key2,
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
            this.Hyperlink0.Click += delegate { this.Navigate1("LayersPanel"); };

            this.Hyperlink1.Inlines.Add(new Run { Text = nameof(TreeView2Page) });
            this.Hyperlink1.Click += delegate { this.Navigate1("TreeView2"); };

            this.Hyperlink2.Inlines.Add(new Run { Text = nameof(HistoryPanelPage) });
            this.Hyperlink2.Click += delegate { this.Navigate1("HistoryPanel"); };

            this.Hyperlink3.Inlines.Add(new Run { Text = nameof(UISyncPage) });
            this.Hyperlink3.Click += delegate { this.Navigate1("UISync"); };

            this.Hyperlink4.Inlines.Add(new Run { Text = nameof(SelectedRangesPage) });
            this.Hyperlink4.Click += delegate { this.Navigate1("SelectedRanges"); };

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
            this.AutoSuggestBox.SuggestionChosen += (s, e) => this.Navigate2($"{e.SelectedItem}");
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
                            IEnumerable<string> suitableItems = this.Dictionary2.Keys.Where(x => x.ToLower().Contains(text));

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
        public void Execute(object parameter) => this.Navigate1($"{parameter}");

        private void Navigate1(string key)
        {
            if (this.Dictionary1.ContainsKey(key))
            {
                Kvp item = this.Dictionary1[key];

                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Navigate(item.PageType);

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = item.Key1;
            }
        }
        private void Navigate2(string key)
        {
            if (this.Dictionary2.ContainsKey(key))
            {
                Kvp item = this.Dictionary2[key];

                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Navigate(item.PageType);

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = item.Key1;
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
                new Kvp("Tree View 0", "TreeView0", typeof(TreeView0Page)),
                new Kvp("Tree View 1", "TreeView1", typeof(TreeView1Page)),
                new Kvp("Tree View 2", "TreeView2", typeof(TreeView2Page)),
            },
            ["History Panel"] = new Kvp[]
            {
                new Kvp("History Panel", "HistoryPanel", typeof(HistoryPanelPage)),
            },
            ["Nodes View"] = new Kvp[]
            {
                new Kvp("Clipboard", "Clipboard", typeof(ClipboardPage)),
                new Kvp("Drag & Drop", "DragDrop", typeof(DragDropPage)),
                new Kvp("Reorder", "Reorder", typeof(ReorderPage)),
            },
            ["Layers Panel"] = new Kvp[]
            {
                new Kvp("Layers Panel", "LayersPanel", typeof(LayersPanelPage)),
            },
            ["UI/UE"] = new Kvp[]
            {
                new Kvp("Linked View", "LinkedView", typeof(LinkedViewPage)),
                new Kvp("Rounded Selection", "RoundedSelection", typeof(RoundedSelectionPage)),
                new Kvp("Swipe", "Swipe", typeof(SwipePage)),
            },
            ["Others"] = new Kvp[]
            {
                new Kvp("Selected Ranges", "SelectedRanges", typeof(SelectedRangesPage)),
                new Kvp("UI Synchronization", "UISync", typeof(UISyncPage)),
            },
        };
    }
}