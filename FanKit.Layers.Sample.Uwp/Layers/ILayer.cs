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

        Rect Rect { get; set; }
        Rect StartingRect { get; }
    }
}