using FanKit.Layers.Core;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Ranges
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Relation
    {
        public static Relation Empty { get; } = new Relation(-1);

        public readonly int Depth;

        public bool IsEmpty => this.Depth < 0;

        private Relation(int depth)
        {
            this.Depth = depth;
        }

        public Relation(ITreeNode item)
        {
            this.Depth = item.Depth;
        }

        public Relp Relate(ILayerBase layer)
        {
            if (layer.SelectMode is SelectMode.Deselected)
            {
                return Relp.None;
            }
            else if (this.IsEmpty)
            {
                return Relp.Parent;
            }
            else if (this.Depth < layer.Depth)
            {
                return Relp.Child;
            }
            else
            {
                return Relp.Parent;
            }
        }
    }
}