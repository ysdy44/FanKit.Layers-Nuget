using FanKit.Layers.DragDrop;
using FanKit.Layers.Ranges;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using System.Linq;
using System.Text;

namespace FanKit.Layers.Sample
{
    public sealed partial class MainPage : ContentPage
    {
        public MainPage()
        {
            base.BindingContext = this;
            this.InitializeComponent();
        }

        public void PushHistory(Undo undo)
        {
        }
    }
}