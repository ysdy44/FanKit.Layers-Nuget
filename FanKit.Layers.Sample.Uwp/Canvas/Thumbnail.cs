using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FanKit.Layers.Sample
{
    public class Thumbnail : IDisposable
    {
        const int W = 50;
        const int H = 50;
        const int Dpi = 96;

        public readonly WriteableBitmap WriteableBitmap;
        public readonly CanvasRenderTarget RenderTarget;
        public readonly ImageBrush ImageBrush;

        public Thumbnail(ICanvasResourceCreator resourceCreator)
        {
            this.WriteableBitmap = new WriteableBitmap(W, H);
            this.RenderTarget = new CanvasRenderTarget(resourceCreator, W, H, Dpi);

            this.ImageBrush = new ImageBrush
            {
                ImageSource = this.WriteableBitmap
            };
        }

        private Thumbnail(Thumbnail thumbnail)
        {
            this.WriteableBitmap = new WriteableBitmap(W, H);
            thumbnail.WriteableBitmap.PixelBuffer.CopyTo(this.WriteableBitmap.PixelBuffer);

            CanvasDevice device = thumbnail.RenderTarget.Device;
            this.RenderTarget = new CanvasRenderTarget(device, W, H, Dpi);
            this.RenderTarget.SetPixelBytes(thumbnail.RenderTarget.GetPixelBytes());

            this.ImageBrush = new ImageBrush
            {
                ImageSource = this.WriteableBitmap
            };
        }

        public Thumbnail Clone() => new Thumbnail(this);

        public void Invalidate(IGraphicsEffectSource bitmap, float w, float h, Rect rect)
        {
            using (CanvasDrawingSession ds = this.RenderTarget.CreateDrawingSession())
            {
                if (rect.Height < 0)
                {
                    if (rect.Width < 0)
                    {
                        if (rect.Width < rect.Height)
                        {
                            float sx = (float)(-50 / w);
                            float sy = (float)(-50 / rect.Width * rect.Height / h);

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.Clear(Colors.Black);
                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                        else
                        {
                            float sx = (float)(-50 / rect.Height * rect.Width / w);
                            float sy = (float)(-50 / h);

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.Clear(Colors.Black);
                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                    }
                    else
                    {
                        if (rect.Width > -rect.Height)
                        {
                            float sx = 50 / w;
                            float sy = (float)(50 / rect.Width * rect.Height / h);

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.Clear(Colors.Black);
                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                        else
                        {
                            float sx = (float)(-50 / rect.Height * rect.Width / w);
                            float sy = -50 / h;

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.Clear(Colors.Black);
                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                    }
                }
                else
                {
                    if (rect.Width < 0)
                    {
                        if (-rect.Width > rect.Height)
                        {
                            float sx = -50 / w;
                            float sy = (float)(-50 / rect.Width * rect.Height / h);

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                        else
                        {
                            float sx = (float)(50 / rect.Height * rect.Width / w);
                            float sy = 50 / h;

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                    }
                    else
                    {
                        if (rect.Width < rect.Height)
                        {
                            float sx = (float)(50 / rect.Height * rect.Width / w);
                            float sy = 50 / h;

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                        else
                        {
                            float sx = 50 / w;
                            float sy = (float)(50 / rect.Width * rect.Height / h);

                            Matrix3x2 m = Matrix3x2.CreateTranslation(-w / 2, -h / 2)
                                * Matrix3x2.CreateScale(sx, sy)
                                * Matrix3x2.CreateTranslation(25, 25);

                            ds.DrawImage(new Transform2DEffect
                            {
                                BorderMode = EffectBorderMode.Hard,
                                InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                                Source = bitmap,
                                TransformMatrix = m
                            });
                        }
                    }
                }
            }

            this.Invalidate();
        }

        public void Invalidate()
        {
            byte[] bytes = this.RenderTarget.GetPixelBytes();
            using (Stream stream = this.WriteableBitmap.PixelBuffer.AsStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            this.WriteableBitmap.Invalidate();
        }

        public void Dispose() => this.RenderTarget.Dispose();
    }
}