using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.Sample
{
    public sealed class RectChanges : IChange
    {
        public RectChange[] Rects { get; set; }

        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                foreach (RectChange item in this.Rects)
                {
                    yield return item.Id;
                }
            }
        }

        public void Dispose()
        {
            this.Rects = null;
        }
    }
}