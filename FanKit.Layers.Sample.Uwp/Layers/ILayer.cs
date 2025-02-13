using FanKit.Layers.Core;
using Microsoft.Graphics.Canvas;
using System;
using System.ComponentModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FanKit.Layers.Sample
{
    public interface ILayer : ICloneable<ILayer>, IComposite<ILayer>, ILayerBase, IDisposable, INotifyPropertyChanged
    {
        LayerType Type { get; }
        string Title { get; }

        Brush Thumbnail { get; }

        Rect Rect { get; set; }
        Rect StartingRect { get; }

        Visibility IsGroupVisibility { get; }
        Visibility NotGroupVisibility { get; }

        double DepthWidth { get; }

        Symbol ChildrenSymbol { get; }

        double ExpandButtonAngle { get; }

        double LockButtonOpacity { get; }

        double VisibleButtonOpacity { get; }

        double SelectOpacity { get; }
        ElementTheme SelectTheme { get; }

        bool FillContainsPointRecursion(Point point);

        void Cache();
        void Move(double offsetX, double offsetY);

        void DrawRecursion(CanvasDrawingSession drawingSession);
    }
}