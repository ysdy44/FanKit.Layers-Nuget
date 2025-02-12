using FanKit.Layers.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.TestApp
{
    public enum LinkedPlacement
    {
        None,
        Bottom,
        Last,
        LastBottom,
    }

    partial class LinkedLayer
    {
        public Visibility HasChildrenVisibility => this.Children.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility NoneVisibility => this.placement == LinkedPlacement.None ? Visibility.Visible : Visibility.Collapsed;

        public Visibility LinkVisibility
        {
            get
            {
                switch (this.placement)
                {
                    case LinkedPlacement.None:
                    case LinkedPlacement.Bottom:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
        }

        private LinkedPlacement placement;
        public LinkedPlacement Placement
        {
            get => this.placement;
            set
            {
                if (this.placement == value)
                    return;

                this.placement = value;
                this.OnPropertyChanged(nameof(LinkVisibility));
                this.OnPropertyChanged(nameof(NoneVisibility));
            }
        }
    }

    public sealed partial class LinkedViewPage : Page
    {
        private readonly LayerManager2<LinkedLayer> M2 = new LayerManager2<LinkedLayer>();

        public LayerList<LinkedLayer> List => this.M2.List;
        public LayerCollection<LinkedLayer> Collection => this.M2.Collection;

        public DragUI<LinkedLayer> DragUI => this.M2.DragUI;

        public ObservableCollection<LinkedLayer> UILayers => this.M2.UILayers;

        public LinkedViewPage()
        {
            this.InitializeComponent();
            this.Collection.ResetByList(new LinkedLayer[]
            {
                new LinkedLayer
                {
                    Title = "1",

                    Depth = 0,
                },
                new LinkedLayer
                {
                    Title = "2",

                    Depth = 0,
                },
                new LinkedLayer
                {
                    IsGroup = true,
                    Title = "3",

                    Depth = 0,
                    IsExpanded = true,
                },
                new LinkedLayer
                {
                    IsGroup = true,
                    Title = "4",

                    Depth = 1,
                    IsExpanded = true,

                    SelectMode = SelectMode.Selected,
                },
                new LinkedLayer
                {
                    Title = "5",

                    Depth = 2,

                    SelectMode = SelectMode.Parent,
                },
                new LinkedLayer
                {
                    Title = "6",

                    Depth = 2,

                    SelectMode = SelectMode.Parent,
                },
                new LinkedLayer
                {
                    Title = "7",

                    Depth = 2,

                    SelectMode = SelectMode.Parent,
                },
                new LinkedLayer
                {
                    IsGroup = true,
                    Title = "8",

                    Depth = 0,
                    IsExpanded = true,
                },
                new LinkedLayer
                {
                    Title = "9",

                    Depth = 1,
                },
                new LinkedLayer
                {
                    IsGroup = true,
                    Title = "10",

                    Depth = 0,
                    IsExpanded = true,
                },
                new LinkedLayer
                {
                    IsGroup = true,
                    Title = "11",

                    Depth = 1,
                }
            });

            //this.UILayers.Clear();
            this.Collection.UISyncTo(this.UILayers);
            this.Update();

            this.ListView.ItemsSource = this.UILayers;
            this.UILayers.CollectionChanged += delegate
            {
                this.Update();
            };
        }

        private void Update()
        {
            switch (this.UILayers.Count)
            {
                case 0:
                    break;
                case 1:
                    this.UILayers.Single().Placement = LinkedPlacement.LastBottom;
                    break;
                default:
                    LinkedLayer last = this.UILayers.Last();
                    last.Placement = LinkedPlacement.LastBottom;

                    int maxDepth = last.Depth;
                    int depth = last.Depth;

                    for (int i = this.UILayers.Count - 2; i >= 0; i--)
                    {
                        LinkedLayer item = this.UILayers[i];

                        if (maxDepth > item.Depth)
                        {
                            item.Placement = depth < item.Depth ? LinkedPlacement.LastBottom : LinkedPlacement.Last;
                            maxDepth = item.Depth;
                            depth = item.Depth;
                        }
                        else
                        {
                            item.Placement = depth < item.Depth ? LinkedPlacement.Bottom : LinkedPlacement.None;
                            depth = item.Depth;
                        }
                    }
                    break;
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            LinkedLayer item = (LinkedLayer)e.ClickedItem;
            this.Collection.ApplySelects(this.List.SelectOnly(item));

            this.Update();
        }

        private void ExpandCommand_Invoked(object sender, LinkedLayer e)
        {
            LinkedLayer item = e;
            switch (item.CanExpand())
            {
                case ClickOptions.Collapse:
                    item.IsExpanded = false;
                    this.Collection.SyncToVisualTree();
                    this.Collection.UISyncTo(this.UILayers);
                    break;
                case ClickOptions.Expand:
                    item.IsExpanded = true;
                    this.Collection.SyncToVisualTree();
                    this.Collection.UISyncTo(this.UILayers);
                    break;
                default:
                    break;
            }
        }
    }

    public sealed class LinkedOptionCommand : ICommand
    {
        public event EventHandler<LinkedLayer> Invoked;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter != default;
        public void Execute(object parameter)
        {
            if (parameter is LinkedLayer item)
            {
                this.Invoked?.Invoke(this, item);//Delegate
            }
        }
    }

    public partial class LinkedLayer : ICloneable<LinkedLayer>, IComposite<LinkedLayer>, ILayerBase, IDisposable, INotifyPropertyChanged
    {
        public Guid Id { get; } = Guid.NewGuid();

        public NodeSettings Settings { get; } = new NodeSettings();

        public bool IsGroup { get; set; }

        public string Title { get; set; }

        public int Depth
        {
            get => this.depth;
            set
            {
                if (this.depth == value)
                    return;
                this.depth = value;
                this.OnPropertyChanged(nameof(Depth));
                this.OnPropertyChanged(nameof(DepthWidth));
            }
        }
        private int depth;
        public double DepthWidth => 32 * this.Depth;

        public bool HasChildren => this.Children.Count > 0;

        public bool IsExpanded { get; set; } = true;

        public bool IsLocked
        {
            get => this.isLocked;
            set
            {
                if (this.isLocked == value)
                    return;
                this.isLocked = value;
                this.OnPropertyChanged(nameof(IsLocked));
                this.OnPropertyChanged(nameof(LockButtonOpacity));
            }
        }
        private bool isLocked;
        public double LockButtonOpacity => this.isLocked ? 1 : 0.5;

        public bool IsVisible
        {
            get => this.isVisible;
            set
            {
                if (this.isVisible == value)
                    return;
                this.isVisible = value;
                this.OnPropertyChanged(nameof(VisibleButtonOpacity));
            }
        }
        private bool isVisible = true;

        public double VisibleButtonOpacity => this.isVisible ? 1d : 0.5d;

        public SelectMode SelectMode
        {
            get => this.selectMode;
            set
            {
                if (this.selectMode == value)
                    return;

                this.selectMode = value;
                this.OnPropertyChanged(nameof(SelectMode));
                this.OnPropertyChanged(nameof(SelectOpacity));
            }
        }

        private SelectMode selectMode = SelectMode.Deselected;

        public double SelectOpacity => this.selectMode.ToSelectOpacity();

        public int ChildrenCount => this.Children.Count;

        public IEnumerable<IChildNode> ChildNodes => this.Children;

        public IList<LinkedLayer> Children { get; } = new List<LinkedLayer>();

        public void OnChildrenCountChanged()
        {
            this.OnPropertyChanged(nameof(HasChildrenVisibility));
            this.OnPropertyChanged(nameof(HasChildren));
        }

        public void RenderThumbnail()
        {
        }

        public void LoadFromXml(XElement content)
        {
            foreach (XAttribute attribute in content.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "Depth": this.Depth = (int)attribute; break;

                    case "IsGroup": this.IsGroup = (bool)attribute; break;
                    case "Title": this.Title = attribute.Value; break;

                    case "IsExpanded": this.IsExpanded = (bool)attribute; break;
                    case "IsLocked": this.IsLocked = (bool)attribute; break;
                    case "IsVisible": this.IsVisible = (bool)attribute; break;
                    case "IsSelected": this.SelectMode = ((bool)attribute).ToSelectMode(); break;
                    default: break;
                }
            }
        }

        public XElement SaveToXml(XmlStructure structure, XObject children)
        {
            return new XElement
            (
                "Layer",
                this.SaveXmlStructure(structure, children),

                new XAttribute("IsGroup", this.IsGroup),
                new XAttribute("Title", this.Title),

                new XAttribute("IsExpanded", this.IsExpanded),
                new XAttribute("IsLocked", this.IsLocked),
                new XAttribute("IsVisible", this.IsVisible),
                new XAttribute("IsSelected", this.SelectMode.IsSelected())
             );
        }

        public LinkedLayer Clone() => new LinkedLayer
        {
            depth = this.depth,

            IsExpanded = this.IsExpanded,
            IsLocked = this.IsLocked,
            IsVisible = this.IsVisible,
            SelectMode = this.SelectMode,

            IsGroup = this.IsGroup,
            Title = this.Title,
        };

        public LinkedLayer Clone(int depth) => new LinkedLayer
        {
            depth = depth,

            IsExpanded = this.IsExpanded,
            IsLocked = this.IsLocked,
            IsVisible = this.IsVisible,
            SelectMode = this.SelectMode,

            IsGroup = this.IsGroup,
            Title = this.Title,
        };

        public void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}