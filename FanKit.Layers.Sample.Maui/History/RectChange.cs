using FanKit.Layers.Changes;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using Microsoft.Maui.Graphics;

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