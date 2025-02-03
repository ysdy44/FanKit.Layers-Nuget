using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.Demo
{
    /// <summary/>
    public class DemoChange : IChange
    {
        /// <summary/>
        public double OldX;
        /// <summary/>
        public double OldY;

        /// <summary/>
        public double NewX;
        /// <summary/>
        public double NewY;

        /// <inheritdoc/>
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}