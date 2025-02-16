using System.Windows;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            base.Loaded += delegate
            {
                foreach (InputBinding inputBinding in this.MainPage.InputBindings)
                {
                    base.InputBindings.Add(inputBinding);
                }
            };
        }
    }
}