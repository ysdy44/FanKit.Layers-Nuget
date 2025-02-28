using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Linq;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        private void CacheDragOverGuide()
        {
            this.DragUI.CacheDragOverGuide(this.LayerScrollView.Width, Layer1.ZoomFactorForDepth, this.ContainerSize);
        }

        private double ContainerSize(int i)
        {
            return 38f;
        }

        public Point PointToWindow()
        {
            // The AbsoluteLayout cover the LayerListView
            return Microsoft.Maui.Graphics.Point.Zero;

            /*
            Point p = Microsoft.Maui.Graphics.Point.Zero;
            VisualElement parent = this.LayerListView;
            ScrollView scrollView = this.LayerListView;

            while (parent != null)
            {
                p.X += parent.X;
                p.Y += parent.Y;

                if (scrollView != null)
                {
                    p.X -= scrollView.ScrollX;
                    p.Y -= scrollView.ScrollY;
                }
                parent = parent.Parent as VisualElement;
                scrollView = parent as ScrollView;
            }

            return p;
             */
        }

        //------------------------ Pivot ----------------------------//

        private void Pivot(int index, bool selected)
        {
            Label header = (Label)this.HorizontalStackLayout.Children[index];
            VisualElement content = (VisualElement)this.Grid.Children[index];

            header.TextDecorations = selected ? TextDecorations.Underline : TextDecorations.None;
            content.IsVisible = selected;
        }

        //------------------------ Menu ----------------------------//

        private ToolbarItem ToTool(OptionTypeMenu item)
        {
            return new ToolbarItem
            {
                Order = ToolbarItemOrder.Secondary,
                Text = item.Type.GetString(),

                Command = this.Command,
                CommandParameter = item.Type,

                IconImageSource = item.Symbol.HasValue ? new FileImageSource
                {
                    File = item.Symbol.Value.ToFile()
                } : null,
            };
        }

        private IMenuElement ToMenu(OptionTypeMenu item)
        {
            if (item == null)
                return new MenuFlyoutSeparator();

            string text = item.IsUndo ? UIType.Undo.GetString() : item.IsRedo ? UIType.Redo.GetString() : item.Type.GetString();
            object parameter = (item.IsUndo || item.IsRedo) ? null : (object)item.Type;
            ICommand command = item.IsUndo ? this.UndoCommand : item.IsRedo ? this.RedoCommand : this.Command;
            ImageSource icon = item.Symbol.HasValue ? new FileImageSource
            {
                File = item.Symbol.Value.ToFile()
            } : null;

            if (item.KeyboardAccelerators == null)
            {
                return new MenuFlyoutItem
                {
                    IconImageSource = icon,
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
                        IconImageSource = icon,
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
                        IconImageSource = icon,
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
                        IconImageSource = icon,
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
                        Modifiers = KeyboardAcceleratorModifiers.Ctrl,
                        Key = split.Last()
                    };
                case "Shift":
                    return new KeyboardAccelerator
                    {
                        Modifiers = KeyboardAcceleratorModifiers.Shift,
                        Key = split.Last()
                    };
                default:
                    return new KeyboardAccelerator
                    {
                        Key = split.Last()
                    };
            }
        }
    }
}