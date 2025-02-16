using FanKit.Layers.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using Windows.Foundation;

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