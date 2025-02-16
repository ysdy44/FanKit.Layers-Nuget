using FanKit.Layers.Core;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace FanKit.Layers.Sample
{
    public interface ILayer : ICloneable<ILayer>, IComposite<ILayer>, ILayerBase, IDisposable, INotifyPropertyChanged
    {
        LayerType Type { get; }
        string Title { get; }

        Brush Thumbnail { get; }

        System.Drawing.Rectangle Rect { get; set; }
        System.Drawing.Rectangle StartingRect { get; }

        Visibility IsGroupVisibility { get; }
        Visibility NotGroupVisibility { get; }

        double DepthWidth { get; }

        Symbols ChildrenSymbol { get; }

        double ExpandButtonAngle { get; }

        double LockButtonOpacity { get; }

        double VisibleButtonOpacity { get; }

        double SelectOpacity { get; }
        Brush SelectBrush { get; }

        bool FillContainsPointRecursion(System.Drawing.Point point);

        void Cache();
        void Move(double offsetX, double offsetY);

        void DrawRecursion(System.Drawing.Graphics drawingSession);
    }
}