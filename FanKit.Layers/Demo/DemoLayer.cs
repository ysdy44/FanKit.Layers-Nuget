using FanKit.Layers.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace FanKit.Layers.Demo
{
    /// <summary/>
    public class DemoLayer : ICloneable<DemoLayer>, IComposite<DemoLayer>, ILayerBase, IDisposable, INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc/>
        public NodeSettings Settings { get; } = new NodeSettings();

        /// <inheritdoc/>
        public bool IsGroup { get; set; }

        /// <summary/>
        public string Title { get; set; }

        /// <inheritdoc/>
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
        /// <summary/>
        public double DepthWidth => 16 * this.Depth;

        /// <summary/>
        public bool HasChildren => this.Children.Count > 0;

        /// <inheritdoc/>
        public bool IsExpanded { get; set; } = true;

        /// <inheritdoc/>
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
        /// <summary/>
        public double LockButtonOpacity => this.isLocked ? 1 : 0.5;

        /// <inheritdoc/>
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

        /// <summary/>
        public double VisibleButtonOpacity => this.isVisible ? 1d : 0.5d;

        /// <inheritdoc/>
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

        /// <summary/>
        public double SelectOpacity => this.selectMode.ToSelectOpacity();

        /// <inheritdoc/>
        public int ChildrenCount => this.Children.Count;

        /// <inheritdoc/>
        public IEnumerable<IChildNode> ChildNodes => this.Children;

        /// <inheritdoc/>
        public IList<DemoLayer> Children { get; } = new List<DemoLayer>();

        /// <inheritdoc/>
        public void OnChildrenCountChanged()
        {
            this.OnPropertyChanged(nameof(HasChildren));
        }

        /// <inheritdoc/>
        public void RenderThumbnail()
        {
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DemoLayer Clone() => new DemoLayer
        {
            depth = this.depth,

            IsExpanded = this.IsExpanded,
            IsLocked = this.IsLocked,
            IsVisible = this.IsVisible,
            SelectMode = this.SelectMode,

            IsGroup = this.IsGroup,
            Title = this.Title,
        };

        /// <inheritdoc/>
        public DemoLayer Clone(int depth) => new DemoLayer
        {
            depth = depth,

            IsExpanded = this.IsExpanded,
            IsLocked = this.IsLocked,
            IsVisible = this.IsVisible,
            SelectMode = this.SelectMode,

            IsGroup = this.IsGroup,
            Title = this.Title,
        };

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}