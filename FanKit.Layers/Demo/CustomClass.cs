using FanKit.Layers.Core;
using System.Collections.Generic;

namespace FanKit.Layers.Demo
{
    /// <summary/>
    public class CustomClass : IComposite<CustomClass>
    {
        /// <summary/>
        public bool IsGroup { get; set; }

        /// <inheritdoc/>
        public IList<CustomClass> Children { get; set; }

        /// <inheritdoc/>
        public void OnChildrenCountChanged() { }
    }
}