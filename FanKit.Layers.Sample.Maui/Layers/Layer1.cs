using FanKit.Layers.Core;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FanKit.Layers.Sample
{
    public abstract class Layer1
    {
        public const double ZoomFactorForDepth = 16;

        private static float Red = Colors.DodgerBlue.Red;
        private static float Green = Colors.DodgerBlue.Green;
        private static float Blue = Colors.DodgerBlue.Blue;

        private static Color DeselectedColor = Color.FromRgba(Red, Green, Blue, (float)SelectMode.Deselected.ToSelectOpacity());
        private static Color SelectedColor = Color.FromRgba(Red, Green, Blue, (float)SelectMode.Selected.ToSelectOpacity());
        private static Color ParentColor = Color.FromRgba(Red, Green, Blue, (float)SelectMode.Parent.ToSelectOpacity());

        private readonly FileImageSource Folder = new FileImageSource
        {
            File = Symbols.Folder.ToFile()
        };
        private readonly FileImageSource NewFolder = new FileImageSource
        {
            File = Symbols.NewFolder.ToFile()
        };

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
        public GridLength DepthWidth => ZoomFactorForDepth * this.Depth;

        public FileImageSource ChildrenSymbol => this.Children.Count > 0 ? this.Folder : this.NewFolder;

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                if (this.isExpanded == value)
                    return;

                this.isExpanded = value;
                this.OnPropertyChanged(nameof(IsExpanded));
                this.OnPropertyChanged(nameof(ExpandButtonGlyph));
            }
        }
        protected bool isExpanded = true;
        public string ExpandButtonGlyph => this.isExpanded ? "+" : "-";

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
                this.OnPropertyChanged(nameof(SelectColor));
            }
        }
        protected SelectMode selectMode = SelectMode.Deselected;
        public double SelectOpacity => this.selectMode.ToSelectOpacity();
        public Color SelectColor
        {
            get
            {
                switch (this.selectMode)
                {
                    case SelectMode.Deselected: return DeselectedColor;
                    case SelectMode.Selected: return SelectedColor;
                    case SelectMode.Parent: return ParentColor;
                    default: return DeselectedColor;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}