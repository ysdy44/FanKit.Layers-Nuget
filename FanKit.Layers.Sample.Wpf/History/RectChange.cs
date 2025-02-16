using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Sample
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RectChange : IChange
    {
        public readonly Guid Id;
        public readonly System.Drawing.Rectangle OldValue;
        public readonly System.Drawing.Rectangle NewValue;

        public RectChange(Guid id, System.Drawing.Rectangle oldValue, System.Drawing.Rectangle newValue)
        {
            this.Id = id;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                yield return this.Id;
            }
        }

        public void Dispose()
        {
        }
    }
}