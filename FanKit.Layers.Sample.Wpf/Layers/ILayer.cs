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

        System.Drawing.Rectangle Rect { get; set; }
        System.Drawing.Rectangle StartingRect { get; }
    }
}