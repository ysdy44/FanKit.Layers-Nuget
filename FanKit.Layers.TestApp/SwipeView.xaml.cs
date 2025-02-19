using System.Windows.Input;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace FanKit.Layers.TestApp
{
    // Defines constants that specify the effect of a swipe interaction.
    public enum SwipeViewMode // Windows.UI.Xaml.Controls.SwipeMode
    {
        // A swipe reveals a menu of commands.
        Reveal = 0,

        // A swipe executes a command.
        Execute = 1
    }

    [ContentProperty(Name = nameof(Child))]
    public sealed partial class SwipeView : UserControl
    {
        public static SwipeViewMode Mode = SwipeViewMode.Execute;
        private static readonly UISettings UISettings = new UISettings();

        private double StartingX;

        public double BrushOpacity { get => this.Brush.Opacity; set => this.Brush.Opacity = value; }
        public object Child { get => this.Presenter.Content; set => this.Presenter.Content = value; }

        public object CommandParameter { get => this.Button.CommandParameter; set => this.Button.CommandParameter = value; }
        public ICommand Command { get => this.Button.Command; set => this.Button.Command = value; }

        public SwipeView()
        {
            this.InitializeComponent();
            this.Button.Click += delegate { this.Close(); };
            this.ExecuteStoryboard.Completed += delegate { this.Button.Command.Execute(this.Button.CommandParameter); };
            this.Presenter.ManipulationStarting += delegate { this.StartingX = this.Transform.X; };
            this.Presenter.ManipulationDelta += (s, e) =>
            {
                double x = this.StartingX + e.Cumulative.Translation.X;

                if (x < 0.0d)
                    this.Transform.X = 0.0d;
                else if (x > this.ActualWidth)
                    this.Transform.X = this.ActualWidth;
                else
                    this.Transform.X = x;
            };
            this.Presenter.ManipulationCompleted += (s, e) =>
            {
                if (this.Button.Command is null)
                    this.Close();
                else if (e.Cumulative.Translation.X >= this.Button.ActualWidth)
                {
                    switch (Mode)
                    {
                        case SwipeViewMode.Reveal:
                            this.Open();
                            break;
                        case SwipeViewMode.Execute:
                            this.Execute();
                            break;
                        default:
                            break;
                    }
                }
                else
                    this.Close();
            };
            this.Presenter.PointerPressed += (s, e) =>
            {
                switch (e.Pointer.PointerDeviceType)
                {
                    case PointerDeviceType.Touch:
                        this.Presenter.ManipulationMode = ManipulationModes.TranslateX;
                        break;
                    default:
                        this.Presenter.ManipulationMode = ManipulationModes.None;
                        break;
                }
            };
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                double w = e.NewSize.Width;
                double h = e.NewSize.Height;
                this.Geometry.Rect = new Rect(-w, 0, w, h);
            };
        }

        public void Close()
        {
            if (UISettings.AnimationsEnabled)
                this.CloseStoryboard.Begin();
            else
                this.Transform.X = 0;
        }

        public void Open()
        {
            if (UISettings.AnimationsEnabled)
                this.OpenStoryboard.Begin();
            else
                this.Transform.X = -this.Button.ActualWidth;
        }

        private void Execute()
        {
            if (UISettings.AnimationsEnabled)
                this.ExecuteStoryboard.Begin();
            else
            {
                this.Transform.X = 0;
                this.Button.Command.Execute(this.Button.CommandParameter);
            }
        }
    }
}