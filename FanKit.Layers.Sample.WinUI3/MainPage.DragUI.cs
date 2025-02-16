using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
using Windows.System;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        private void CacheDragOverGuide()
        {
            this.DragUI.CacheDragOverGuide(this.LayerListView.ActualWidth, Layer1.ZoomFactorForDepth, this.ContainerSize);
        }

        private double ContainerSize(int i)
        {
            return this.LayerListView.ContainerFromIndex(i) is FrameworkElement element ? element.ActualHeight : 0;
        }

        public Point PointToWindow()
        {
            return this.LayerListView.TransformToVisual(this.RootLayout).TransformPoint(default);
        }

        //------------------------ ScrollViewer ----------------------------//

        public ScrollViewer FindDescendant()
        {
            return FindDescendant(this.LayerListView);
        }

        private static ScrollViewer FindDescendant(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is ScrollViewer scrollViewer)
                    return scrollViewer;

                if (FindDescendant(child) is ScrollViewer scrollViewerChild)
                    return scrollViewerChild;
            }

            return null;
        }

        //------------------------ Menu ----------------------------//

        private MenuFlyoutItemBase ToMenu(OptionTypeMenu item)
        {
            if (item == null)
                return new MenuFlyoutSeparator();

            string text = item.IsUndo ? UIType.Undo.GetString() : item.IsRedo ? UIType.Redo.GetString() : item.Type.GetString();
            object parameter = (item.IsUndo || item.IsRedo) ? null : (object)item.Type;
            ICommand command = item.IsUndo ? this.UndoCommand : item.IsRedo ? this.RedoCommand : this.Command;
            SymbolIcon icon = item.Symbol.HasValue ? new SymbolIcon
            {
                Symbol = (Symbol)System.Enum.Parse(typeof(Symbol), item.Symbol.Value.ToString())
            } : null;

            if (item.KeyboardAccelerators == null)
            {
                return new MenuFlyoutItem
                {
                    Icon = icon,
                    Text = text,
                    CommandParameter = parameter,
                    Command = command,
                };
            }

            string[] split = item.KeyboardAccelerators.Split('|');
            switch (split.Length)
            {
                case 1:
                    return new MenuFlyoutItem
                    {
                        Icon = icon,
                        Text = text,
                        CommandParameter = parameter,
                        Command = command,
                        KeyboardAccelerators =
                        {
                            ToKey(split.Single())
                        }
                    };
                case 2:
                    return new MenuFlyoutItem
                    {
                        Icon = icon,
                        Text = text,
                        CommandParameter = parameter,
                        Command = command,
                        KeyboardAccelerators =
                        {
                            ToKey(split.First()),
                            ToKey(split.Last()),
                        }
                    };
                default:
                    return new MenuFlyoutItem
                    {
                        Icon = icon,
                        Text = text,
                        CommandParameter = parameter,
                        Command = command,
                    };
            }
        }

        private static KeyboardAccelerator ToKey(string key)
        {
            string[] split = key.Split('+');

            switch (split.First())
            {
                case "Ctrl":
                    return new KeyboardAccelerator
                    {
                        Modifiers = VirtualKeyModifiers.Control,
                        Key = (VirtualKey)System.Enum.Parse(typeof(VirtualKey), split.Last())
                    };
                case "Shift":
                    return new KeyboardAccelerator
                    {
                        Modifiers = VirtualKeyModifiers.Shift,
                        Key = (VirtualKey)System.Enum.Parse(typeof(VirtualKey), split.Last())
                    };
                default:
                    return new KeyboardAccelerator
                    {
                        Key = (VirtualKey)System.Enum.Parse(typeof(VirtualKey), split.Last())
                    };
            }
        }
    }
}