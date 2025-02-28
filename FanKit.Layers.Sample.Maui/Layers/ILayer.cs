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

        RectF Rect { get; set; }
        RectF StartingRect { get; }
    }
}