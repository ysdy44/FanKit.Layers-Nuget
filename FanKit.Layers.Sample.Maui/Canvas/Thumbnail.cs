using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace FanKit.Layers.Sample
{
    public sealed class Thumbnail
    {
        public Brush ImageBrush;
        public ImagePaint ImagePaint = new ImagePaint();

        public Thumbnail(Thumbnail thumbnail)
        {
            ImagePaint = new ImagePaint
            {
                Image = thumbnail.ImagePaint.Image
            };
            ImageBrush = ImagePaint;
        }

        public Thumbnail(GraphicsView canvasControl)
        {
            ImagePaint = new ImagePaint();
            ImageBrush = ImagePaint;
        }

        public void Invalidate(IImage bitmap, float bitmapWidth, float bitmapHeight, Rect rect)
        {
            ImagePaint = new ImagePaint
            {
                Image = bitmap
            };
            ImageBrush = ImagePaint;
        }

        public Thumbnail Clone()
        {
            return new Thumbnail(this);
        }
    }
}