using System.Threading.Tasks;
using System.Windows.Controls;

namespace FanKit.Layers.Sample
{
    public sealed class ReorderButton : Button
    {
        public const int Delay = 15;
        public const int DelayDouble = Delay + Delay;

        public ReorderButton()
        {
            this.PreviewMouseLeftButtonDown += async (s, e) =>
            {
                await Task.Delay(Delay);
                e.Handled = true;
            };
        }
    }
}