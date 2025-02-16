using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation;

namespace FanKit.Layers.Sample
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RectChange : IChange
    {
        public readonly Guid Id;
        public readonly Rect OldValue;
        public readonly Rect NewValue;

        public RectChange(Guid id, Rect oldValue, Rect newValue)
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