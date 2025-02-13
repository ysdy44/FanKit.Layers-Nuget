using FanKit.Layers.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.Sample
{
    public abstract class Layer1
    {
        public const double ZoomFactorForDepth = 16;

        public Guid Id { get; } = Guid.NewGuid();

        public NodeSettings Settings { get; } = new NodeSettings();

        public int ChildrenCount => this.Children.Count;

        public IEnumerable<IChildNode> ChildNodes => this.Children;

        public IList<ILayer> Children { get; } = new List<ILayer>();

        public void OnChildrenCountChanged()
        {
            this.OnPropertyChanged(nameof(ChildrenSymbol));
        }

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
        protected int depth;
        public double DepthWidth => ZoomFactorForDepth * this.Depth;

        public Symbol ChildrenSymbol => this.Children.Count > 0 ? Symbol.Folder : Symbol.NewFolder;

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                if (this.isExpanded == value)
                    return;

                this.isExpanded = value;
                this.OnPropertyChanged(nameof(IsExpanded));
                this.OnPropertyChanged(nameof(ExpandButtonAngle));
            }
        }
        protected bool isExpanded = true;
        public double ExpandButtonAngle => this.isExpanded ? 90d : 0d;

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
        protected bool isLocked;
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
        protected bool isVisible = true;
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
                this.OnPropertyChanged(nameof(SelectTheme));
            }
        }
        protected SelectMode selectMode = SelectMode.Deselected;
        public double SelectOpacity => this.selectMode.ToSelectOpacity();
        public ElementTheme SelectTheme => this.selectMode == SelectMode.Deselected ? ElementTheme.Default : ElementTheme.Dark;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}