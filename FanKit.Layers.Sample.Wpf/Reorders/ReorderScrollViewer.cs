using FanKit.Layers.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FanKit.Layers.Sample
{
    public sealed class ReorderScrollViewer : ScrollViewer
    {
        public event EventHandler<ItemClickEventArgs> ItemClick;
        public event EventHandler<DragItemsEventArgs> DragItemsStarting;

        private bool IsDown;

        private Point Position;
        private Point StartingPosition;

        private FrameworkElement Container;
        private FrameworkElement StartingContainer;

        private ITreeNode Item;
        private ITreeNode StartingItem;

        public bool IsItemClickEnabled { get; set; }
        public bool CanDragItems { get; set; }

        public ReorderScrollViewer()
        {
            this.PreviewMouseLeftButtonDown += async (s, e) =>
            {
                await Task.Delay(ReorderButton.DelayDouble);

                if (e.Handled)
                    return;

                this.IsDown = true;
                this.StartingPosition = this.Position = e.GetPosition(this);
                this.StartingContainer = e.OriginalSource as FrameworkElement;
                this.StartingItem = this.StartingContainer is null ? null : this.StartingContainer.DataContext as ILayer;
            };
            this.MouseMove += (s, e) =>
            {
                if (this.IsDown is false)
                    return;

                if (this.StartingItem is null)
                    return;

                this.Position = e.GetPosition(this);

                this.Container = e.OriginalSource as FrameworkElement;
                if (this.Container != null)
                {
                    this.Item = this.Container.DataContext as ILayer;

                    if (this.Item != null && this.StartingItem.Id == this.Item.Id)
                    {
                        double x = System.Math.Abs(this.StartingPosition.X - this.Position.X);
                        double y = System.Math.Abs(this.StartingPosition.Y - this.Position.Y);
                        if (x * x + y * y < 100)
                        {
                            return;
                        }
                    }
                }

                this.IsDown = false;
                if (this.CanDragItems)
                    this.DragItemsStarting?.Invoke(this, new DragItemsEventArgs(this.StartingItem)); // Delegate

                DataObject data = new DataObject(typeof(System.Guid), this.StartingItem.Id);

                this.Container = null;
                this.StartingContainer = null;
                this.Item = null;
                this.StartingItem = null;

                System.Windows.DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            };
            this.MouseUp += (s, e) =>
            {
                if (this.IsDown is false)
                    return;

                if (this.StartingItem is null)
                    return;

                this.IsDown = false;

                if (this.StartingItem is null)
                {
                    this.Container = null;
                    this.StartingContainer = null;
                    this.Item = null;
                    this.StartingItem = null;

                    if (this.IsItemClickEnabled)
                        this.ItemClick?.Invoke(s, null); // Delegate
                }
                else if (this.IsItemClickEnabled && this.ItemClick != null)
                {
                    ItemClickEventArgs args = new ItemClickEventArgs(this.StartingItem);

                    this.Container = null;
                    this.StartingContainer = null;
                    this.Item = null;
                    this.StartingItem = null;

                    this.ItemClick.Invoke(s, args); // Delegate
                }
                else
                {
                    this.Container = null;
                    this.StartingContainer = null;
                    this.Item = null;
                    this.StartingItem = null;
                }
            };
        }
    }
}