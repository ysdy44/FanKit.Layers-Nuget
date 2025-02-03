using FanKit.Layers.Collections;
using FanKit.Layers.Demo;
using System.Collections.ObjectModel;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FanKit.Layers.TestApp
{
    public sealed partial class HistoryPanelPage : Page
    {
        double OldX;
        double OldY;

        double NewX;
        double NewY;

        readonly UndoList<DemoUndo> History = new UndoList<DemoUndo>();

        public ObservableCollection<DemoUndo> UIHistory { get; } = new ObservableCollection<DemoUndo>();

        public HistoryPanelPage()
        {
            this.InitializeComponent();

            this.UndoButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.Control, Key = VirtualKey.Z });
            this.UndoButton.Click += (s, e) => this.TryUndo();

            this.RedoButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = VirtualKeyModifiers.Control, Key = VirtualKey.Y });
            this.RedoButton.Click += (s, e) => this.TryRedo();

            #region Move

            this.Rectangle.ManipulationStarted += (s, e) =>
            {
                this.OldX = this.NewX = Canvas.GetLeft(this.Rectangle);
                this.OldY = this.NewY = Canvas.GetTop(this.Rectangle);
            };
            this.Rectangle.ManipulationDelta += (s, e) =>
            {
                double x = e.Delta.Translation.X;
                double y = e.Delta.Translation.Y;

                this.NewX += x;
                this.NewY += y;

                const InvalidateModes modes = InvalidateModes.CanvasControlInvalidate;
                this.Invalidate(modes);
            };
            this.Rectangle.ManipulationCompleted += (s, e) =>
            {
                this.NewX = Canvas.GetLeft(this.Rectangle);
                this.NewY = Canvas.GetTop(this.Rectangle);

                int x = (int)this.NewX;
                int y = (int)this.NewY;

                this.History.Push(new DemoUndo
                {
                    Description = $"Move to ({x}, {y})",
                    DemoChange =
                    {
                        OldX = this.OldX,
                        OldY = this.OldY,
                        NewX = this.NewX,
                        NewY = this.NewY
                    }
                });

                const InvalidateModes modes = InvalidateModes.HistoryChanged | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                this.Invalidate(modes);
            };

            this.HistoryListView.PreviewKeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case VirtualKey.Left:
                    case VirtualKey.Up:
                        this.TryUndo();
                        e.Handled = true;
                        break;
                    case VirtualKey.Right:
                    case VirtualKey.Down:
                        this.TryRedo();
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            };

            #endregion

            #region Undo & Redo

            this.Flyout.Closed += (s, e) => this.Slider.IsEnabled = false;
            this.Flyout.Opened += (s, e) =>
            {
                this.Slider.Minimum = -1;
                this.Slider.Maximum = this.History.Count - 1;
                this.Slider.Value = this.History.CurrentIndex;
                this.Slider.IsEnabled = true;
            };

            this.Slider.ValueChanged += (s, e) =>
            {
                if (this.Slider.IsEnabled)
                {
                    int index = (int)e.NewValue;
                    this.TryNavigate(index);
                }
            };

            this.HistoryListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is DemoUndo history)
                {
                    int index = this.History.IndexOf(history);

                    this.TryNavigate(index);
                }
            };

            #endregion
        }

        private void TryUndo()
        {
            if (this.History.CanGoBack())
            {
                DemoUndo item = this.History.GoBack();
                this.ApplyUndo(item);

                const InvalidateModes modes = InvalidateModes.CanvasControlInvalidate | InvalidateModes.HistoryChanged | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                this.Invalidate(modes);
            }
        }

        private void TryRedo()
        {
            if (this.History.CanGoForward())
            {
                DemoUndo item = this.History.GoForward();
                this.ApplyRedo(item);

                const InvalidateModes modes = InvalidateModes.CanvasControlInvalidate | InvalidateModes.HistoryChanged | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                this.Invalidate(modes);
            }
        }

        private void TryNavigate(int index)
        {
            switch (this.History.CanNavigate(index))
            {
                case NavigateAction.None:
                    break;
                case NavigateAction.Undo:
                    {
                        DemoUndo item = this.History.GoBack();
                        this.ApplyUndo(item);

                        const InvalidateModes modes = InvalidateModes.CanvasControlInvalidate | InvalidateModes.HistoryChanged | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                        this.Invalidate(modes);
                    }
                    break;
                case NavigateAction.Redo:
                    {
                        DemoUndo item = this.History.GoForward();
                        this.ApplyRedo(item);

                        const InvalidateModes modes = InvalidateModes.CanvasControlInvalidate | InvalidateModes.HistoryChanged | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;
                        this.Invalidate(modes);
                    }
                    break;
                case NavigateAction.UndoRange:
                    IndexRange undoRange = this.History.NavigateTo(index);
                    lock (this)
                    {
                        const InvalidateModes modes = InvalidateModes.CanvasControlInvalidate | InvalidateModes.HistoryChanged | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;

                        foreach (DemoUndo item in this.History.GetRange(undoRange))
                        {
                            this.ApplyUndo(item);
                        }

                        this.Invalidate(modes);
                    }
                    break;
                case NavigateAction.RedoRange:
                    IndexRange redoRange = this.History.NavigateTo(index);
                    lock (this)
                    {
                        const InvalidateModes modes = InvalidateModes.CanvasControlInvalidate | InvalidateModes.HistoryChanged | InvalidateModes.HistoryCanExecuteChanged | InvalidateModes.HistorySelectionChanged;

                        foreach (DemoUndo item in this.History.GetRange(redoRange))
                        {
                            this.ApplyRedo(item);
                        }

                        this.Invalidate(modes);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ApplyUndo(DemoUndo undo)
        {
            if (undo is null)
                return;

            // Sets X and Y
            this.NewX = undo.DemoChange.OldX;
            this.NewY = undo.DemoChange.OldY;
        }

        private void ApplyRedo(DemoUndo undo)
        {
            if (undo is null)
                return;

            // Sets X and Y
            this.NewX = undo.DemoChange.NewX;
            this.NewY = undo.DemoChange.NewY;
        }

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.CanvasControlInvalidate))
            {
                // Gets X and Y 
                Canvas.SetLeft(this.Rectangle, this.NewX);
                Canvas.SetTop(this.Rectangle, this.NewY);
            }

            if (modes.HasFlag(InvalidateModes.HistoryCleared)) this.UIHistory.Clear();
            if (modes.HasFlag(InvalidateModes.HistoryChanged)) this.History.UISyncTo(this.UIHistory);
            if (modes.HasFlag(InvalidateModes.HistoryCanExecuteChanged))
            {
                this.UndoButton.IsEnabled = this.History.CanGoBack();
                this.RedoButton.IsEnabled = this.History.CanGoForward();
            }
            if (modes.HasFlag(InvalidateModes.HistorySelectionChanged)) this.History.UISyncTime();
        }
    }
}