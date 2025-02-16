namespace FanKit.Layers.Sample
{
    public struct Transformer
    {
        public System.Drawing.Rectangle Rect;

        public float Left12;
        public float Top12;
        public float Right12;
        public float Bottom12;

        public float Left8;
        public float Top8;
        public float Right8;
        public float Bottom8;

        public void Draw(System.Drawing.Graphics drawingSession)
        {
            if (this.Rect.IsEmpty)
                return;

            drawingSession.DrawRectangle(System.Drawing.Pens.DodgerBlue, this.Rect);

            drawingSession.FillRectangle(System.Drawing.Brushes.DodgerBlue, this.Left12, this.Top12, 12, 12);
            drawingSession.FillRectangle(System.Drawing.Brushes.DodgerBlue, this.Right12, this.Top12, 12, 12);
            drawingSession.FillRectangle(System.Drawing.Brushes.DodgerBlue, this.Left12, this.Bottom12, 12, 12);
            drawingSession.FillRectangle(System.Drawing.Brushes.DodgerBlue, this.Right12, this.Bottom12, 12, 12);

            drawingSession.FillRectangle(System.Drawing.Brushes.White, this.Left8, this.Top8, 8f, 8f);
            drawingSession.FillRectangle(System.Drawing.Brushes.White, this.Right8, this.Top8, 8f, 8f);
            drawingSession.FillRectangle(System.Drawing.Brushes.White, this.Left8, this.Bottom8, 8f, 8f);
            drawingSession.FillRectangle(System.Drawing.Brushes.White, this.Right8, this.Bottom8, 8f, 8f);
        }

        public static implicit operator Transformer(System.Drawing.Rectangle rect)
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