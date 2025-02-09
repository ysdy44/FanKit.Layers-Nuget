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
    partial class RoundedLayer
    {
        private VerticalAlignment Align;
        public CornerRadius CornerRadius
        {
            get
            {
                switch (this.Align)
                {
                    case VerticalAlignment.Top: return new CornerRadius(12, 12, 0, 0);
                    case VerticalAlignment.Center: return new CornerRadius(0);
                    case VerticalAlignment.Bottom: return new CornerRadius(0, 0, 12, 12);
                    case VerticalAlignment.Stretch: return new CornerRadius(12);
                    default: return new CornerRadius(12);
                }
            }
        }

        private bool isBottom = true;
        public bool IsBottom
        {
            get => this.isBottom;
            set
            {
                if (this.isBottom == value)
                    return;

                this.isBottom = value;
                this.Align = this.GetAlign();
                this.OnPropertyChanged(nameof(CornerRadius));
            }
        }

        private void OnCornerRadiusChanged()
        {
            this.Align = this.GetAlign();
            this.OnPropertyChanged(nameof(CornerRadius));
        }

        private VerticalAlignment GetAlign()
        {
            switch (this.selectMode)
            {
                case SelectMode.Selected: return this.IsExpanded && this.Children.Count != 0 ? VerticalAlignment.Top : VerticalAlignment.Stretch;
                case SelectMode.Parent: return this.isBottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;
                default: return VerticalAlignment.Center;
            }
        }
    }

    public sealed partial class RoundedSelectionPage : Page
    {
        private readonly LayerManager2<RoundedLayer> M2 = new LayerManager2<RoundedLayer>();

        public LayerList<RoundedLayer> List => this.M2.List;
        public LayerCollection<RoundedLayer> Collection => this.M2.Collection;

        public DragUI<RoundedLayer> DragUI => this.M2.DragUI;

        public ObservableCollection<RoundedLayer> UILayers => this.M2.UILayers;

        public RoundedSelectionPage()
        {
            this.InitializeComponent();
            this.Collection.ResetByList(new RoundedLayer[]
            {
                new RoundedLayer
                {
                    Title = "1",

                    Depth = 0,
                },
                new RoundedLayer
                {
                    Title = "2",

                    Depth = 0,
                },
                new RoundedLayer
                {
                    IsGroup = true,
                    Title = "3",

                    Depth = 0,
                    IsExpanded = true,
                },
                new RoundedLayer
                {
                    IsGroup = true,
                    Title = "4",

                    Depth = 1,
                    IsExpanded = true,

                    SelectMode = SelectMode.Selected,
                },
                new RoundedLayer
                {
                    Title = "5",

                    Depth = 2,

                    SelectMode = SelectMode.Parent,
                },
                new RoundedLayer
                {
                    Title = "6",

                    Depth = 2,

                    SelectMode = SelectMode.Parent,
                },
                new RoundedLayer
                {
                    Title = "7",

                    Depth = 2,

                    SelectMode = SelectMode.Parent,
                },
                new RoundedLayer
                {
                    IsGroup = true,
                    Title = "8",

                    Depth = 0,
                    IsExpanded = true,
                },
                new RoundedLayer
                {
                    Title = "9",

                    Depth = 1,
                },
                new RoundedLayer
                {
                    IsGroup = true,
                    Title = "10",

                    Depth = 0,
                    IsExpanded = true,
                },
                new RoundedLayer
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
                    this.UILayers.Single().IsBottom = true;
                    break;
                default:
                    this.UILayers.Last().IsBottom = true;

                    for (int i = 1; i < this.UILayers.Count; i++)
                    {
                        RoundedLayer current = this.UILayers[i - 1];
                        RoundedLayer next = this.UILayers[i];

                        const SelectMode d = SelectMode.Deselected;
                        current.IsBottom = next.SelectMode == d && current.SelectMode != d;
                    }
                    break;
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            RoundedLayer item = (RoundedLayer)e.ClickedItem;
            this.Collection.ApplySelects(this.List.SelectOnly(item));

            this.Update();
        }

        private void ExpandCommand_Invoked(object sender, RoundedLayer e)
        {
            RoundedLayer item = e;
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

    public sealed class RoundedOptionCommand : ICommand
    {
        public event EventHandler<RoundedLayer> Invoked;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter != default;
        public void Execute(object parameter)
        {
            if (parameter is RoundedLayer item)
            {
                this.Invoked?.Invoke(this, item);//Delegate
            }
        }
    }

    public partial class RoundedLayer : ICloneable<RoundedLayer>, IComposite<RoundedLayer>, ILayerBase, IDisposable, INotifyPropertyChanged
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
        public double DepthWidth => 16 * this.Depth;

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
                OnCornerRadiusChanged();
                this.OnPropertyChanged(nameof(SelectMode));
                this.OnPropertyChanged(nameof(SelectOpacity));
            }
        }

        private SelectMode selectMode = SelectMode.Deselected;

        public double SelectOpacity => this.selectMode.ToSelectOpacity();

        public int ChildrenCount => this.Children.Count;

        public IEnumerable<IChildNode> ChildNodes => this.Children;

        public IList<RoundedLayer> Children { get; } = new List<RoundedLayer>();

        public void OnChildrenCountChanged()
        {
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

        public RoundedLayer Clone() => new RoundedLayer
        {
            depth = this.depth,

            IsExpanded = this.IsExpanded,
            IsLocked = this.IsLocked,
            IsVisible = this.IsVisible,
            SelectMode = this.SelectMode,

            IsGroup = this.IsGroup,
            Title = this.Title,
        };

        public RoundedLayer Clone(int depth) => new RoundedLayer
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