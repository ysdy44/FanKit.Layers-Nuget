using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FanKit.Layers.TestApp
{
    public static class DependencyObjectExtensions
    {
        public static Point PointToWindow(this UIElement visual) => visual.TransformToVisual(Window.Current.Content).TransformPoint(default);
        public static ScrollViewer FindDescendantScrollViewer(this ListView listView) => listView.FindDescendantScrollViewerCore();
        private static ScrollViewer FindDescendantScrollViewerCore(this DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is ScrollViewer scrollViewer)
                    return scrollViewer;

                if (child.FindDescendantScrollViewerCore() is ScrollViewer scrollViewerChild)
                    return scrollViewerChild;
            }

            return null;
        }
    }
}