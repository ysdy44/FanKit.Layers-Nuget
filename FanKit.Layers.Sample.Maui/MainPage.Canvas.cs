using FanKit.Layers.Ranges;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FanKit.Layers.Sample
{
    partial class MainPage : IDrawable
    {
        public IDrawable Drawable => this;

        //------------------------ Draw ----------------------------//

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
        }

        //------------------------ Invalidate ----------------------------//

        public void Invalidate(InvalidateModes modes)
        {
        }
    }
}