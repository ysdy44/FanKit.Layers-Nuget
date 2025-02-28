using FanKit.Layers.Core;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.ComponentModel;

namespace FanKit.Layers.Sample
{
    public interface ILayer : ICloneable<ILayer>, IComposite<ILayer>, ILayerBase, IDisposable, INotifyPropertyChanged
    {
        LayerType Type { get; }
        string Title { get; }

        IDrawable Thumbnail { get; }

        RectF Rect { get; set; }
        RectF StartingRect { get; }

        bool IsGroupVisibility { get; }
        bool NotGroupVisibility { get; }

        GridLength DepthWidth { get; }

        FileImageSource ChildrenSymbol { get; }

        string ExpandButtonGlyph { get; }

        double LockButtonOpacity { get; }

        double VisibleButtonOpacity { get; }

        double SelectOpacity { get; }
        Color SelectColor { get; }

        bool FillContainsPointRecursion(PointF point);

        void Cache();
        void Move(float offsetX, float offsetY);

        void DrawRecursion(ICanvas canvas);
    }
}