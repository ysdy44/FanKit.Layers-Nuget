using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        static Point Empty = default;

        Point PointFrom;
        Point PointTo;

        private void CacheDragOverGuide()
        {
            this.DragUI.CacheDragOverGuide(this.LayerScrollViewer.ActualWidth, Layer1.ZoomFactorForDepth, this.ContainerSize);
        }

        private double ContainerSize(int i)
        {
            return this.LayerItemsControl.ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement element ? element.ActualHeight : 0;
        }

        public Point PointToWindow()
        {
            this.PointFrom = this.LayerScrollViewer.PointFromScreen(Empty);

            this.PointTo.X = -this.PointFrom.X;
            this.PointTo.Y = -this.PointFrom.Y;

            return this.PointTo;
        }

        //------------------------ Menu ----------------------------//

        private FrameworkElement ToMenu(OptionTypeMenu item)
        {
            if (item == null)
                return new Separator();

            string text = item.IsUndo ? UIType.Undo.GetString() : item.IsRedo ? UIType.Redo.GetString() : item.Type.GetString();
            object parameter = (item.IsUndo || item.IsRedo) ? null : (object)item.Type;
            ICommand command = item.IsUndo ? this.UndoCommand : item.IsRedo ? this.RedoCommand : this.Command;
            SymbolIcon icon = item.Symbol.HasValue ? new SymbolIcon
            {
                Symbol = item.Symbol.Value
            } : null;

            if (item.KeyboardAccelerators == null)
            {
                return new MenuItem
                {
                    Icon = icon,
                    Header = text,
                    CommandParameter = parameter,
                    Command = command,
                };
            }

            string[] split = item.KeyboardAccelerators.Split('|');
            switch (split.Length)
            {
                case 1:
                    return new MenuItem
                    {
                        Icon = icon,
                        Header = text,
                        CommandParameter = parameter,
                        Command = command,
                        InputGestureText = item.KeyboardAccelerators,
                        InputBindings =
                        {
                            ToKey(split.Single())
                        }
                    };
                case 2:
                    return new MenuItem
                    {
                        Icon = icon,
                        Header = text,
                        CommandParameter = parameter,
                        Command = command,
                        InputGestureText = item.KeyboardAccelerators.Replace("|", ", "),
                        InputBindings =
                        {
                            ToKey(split.First()),
                            ToKey(split.Last()),
                        }
                    };
                default:
                    return new MenuItem
                    {
                        Icon = icon,
                        Header = text,
                        CommandParameter = parameter,
                        Command = command,
                    };
            }
        }

        private static KeyBinding ToKey(string key)
        {
            string[] split = key.Split('+');

            switch (split.First())
            {
                case "Ctrl":
                    return new KeyBinding
                    {
                        Modifiers = ModifierKeys.Control,
                        Key = (Key)System.Enum.Parse(typeof(Key), split.Last())
                    };
                case "Shift":
                    return new KeyBinding
                    {
                        Modifiers = ModifierKeys.Shift,
                        Key = (Key)System.Enum.Parse(typeof(Key), split.Last())
                    };
                default:
                    return new KeyBinding
                    {
                        Key = (Key)System.Enum.Parse(typeof(Key), split.Last())
                    };
            }
        }
    }
}