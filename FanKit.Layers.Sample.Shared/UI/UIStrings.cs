namespace FanKit.Layers.Sample
{
    public class UIStrings
    {
        public UIType Type { get; set; }
        public override string ToString() => this.Type.GetString();
    }
}