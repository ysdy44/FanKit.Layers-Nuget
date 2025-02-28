using Microsoft.Maui.Controls;
using System;

namespace FanKit.Layers.Sample
{
    public class ReorderScrollView : ScrollView
    {
        readonly DropGestureRecognizer Reorder = new DropGestureRecognizer
        {
            AllowDrop = true
        };

        public event EventHandler<DragEventArgs> DragOver
        {
            remove => this.Reorder.DragOver -= value;
            add => this.Reorder.DragOver += value;
        }

        public event EventHandler<DragEventArgs> DragLeave
        {
            remove => this.Reorder.DragLeave -= value;
            add => this.Reorder.DragLeave += value;
        }

        public event EventHandler<DropEventArgs> Drop
        {
            remove => this.Reorder.Drop -= value;
            add => this.Reorder.Drop += value;
        }

        public bool AllowDrop
        {
            get => base.GestureRecognizers.Count > 0;
            set
            {
                base.GestureRecognizers.Remove(this.Reorder);
                if (value)
                    base.GestureRecognizers.Add(this.Reorder);
            }
        }
    }
}