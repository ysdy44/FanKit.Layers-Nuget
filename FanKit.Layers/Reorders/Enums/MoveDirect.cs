using FanKit.Layers.Core;
using FanKit.Layers.DragDrop;

namespace FanKit.Layers.Reorders
{
    internal enum MoveDirect : byte
    {
        Next,
        Previous,

        Before,
        After,

        First,
        Last,
    }
}