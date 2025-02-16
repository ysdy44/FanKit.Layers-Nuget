using System.Runtime.InteropServices;

namespace FanKit.Layers.Sample
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct CanvasDpi
    {
        public const float Dpi = 96;

        public readonly float Value;
        public readonly float ValueX;
        public readonly float ValueY;

        public CanvasDpi(float valueX, float valueY)
        {
            this.Value = (valueX + valueY) / 2;
            this.ValueX = valueX;
            this.ValueY = valueY;
        }

        public CanvasDpi(float valueX, float valueY, float factorScale)
        {
            this.Value = (valueX + valueY) * factorScale / 2;
            this.ValueX = valueX * factorScale;
            this.ValueY = valueY * factorScale;
        }
    }
}