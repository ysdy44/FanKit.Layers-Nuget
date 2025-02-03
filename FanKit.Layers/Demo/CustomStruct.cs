using FanKit.Layers.Core;

namespace FanKit.Layers.Demo
{
    /// <summary/>
    public struct CustomStruct //: ILayerBase, ITreeNode
    {
        /// <inheritdoc cref="ILayerBase.IsGroup"/>
        public bool IsGroup;

        /// <inheritdoc cref="ITreeNode.Depth"/>
        public int Depth;
    }
}