using Microsoft.Maui.Graphics;

namespace FanKit.Layers.Sample
{
    public struct Transformer
    {
        public Rect Rect;

        public float Left12;
        public float Top12;
        public float Right12;
        public float Bottom12;

        public float Left8;
        public float Top8;
        public float Right8;
        public float Bottom8;

        public void Draw(ICanvas canvas)
        {
            if (this.Rect.IsEmpty)
                return;

            canvas.StrokeSize = 2f;
            canvas.StrokeColor = Colors.DodgerBlue;
            canvas.DrawRectangle(this.Rect);

            canvas.FillColor = Colors.DodgerBlue;
            canvas.FillRectangle(this.Left12, this.Top12, 12, 12);
            canvas.FillRectangle(this.Right12, this.Top12, 12, 12);
            canvas.FillRectangle(this.Left12, this.Bottom12, 12, 12);
            canvas.FillRectangle(this.Right12, this.Bottom12, 12, 12);

            canvas.FillColor = Colors.White;
            canvas.FillRectangle(this.Left8, this.Top8, 8f, 8f);
            canvas.FillRectangle(this.Right8, this.Top8, 8f, 8f);
            canvas.FillRectangle(this.Left8, this.Bottom8, 8f, 8f);
            canvas.FillRectangle(this.Right8, this.Bottom8, 8f, 8f);
        }

        public static implicit operator Transformer(Rect rect)
        {
            return new Transformer
            {
                Rect = rect,

                Left12 = (float)(rect.X - 6f),
                Top12 = (float)(rect.Y - 6f),
                Right12 = (float)(rect.X + rect.Width - 6f),
                Bottom12 = (float)(rect.Y + rect.Height - 6f),

                Left8 = (float)(rect.X - 4f),
                Top8 = (float)(rect.Y - 4f),
                Right8 = (float)(rect.X + rect.Width - 4f),
                Bottom8 = (float)(rect.Y + rect.Height - 4f)
            };
        }
    }
}