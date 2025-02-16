using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FanKit.Layers.Sample
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            base.DataContext = this;
            this.InitializeComponent();
        }

        public void PushHistory(Undo undo)
        {
        }
    }
}