namespace FanKit.Layers.Sample
{
    public class OptionStrings
    {
        public OptionType Type { get; set; }
        public override string ToString() => this.Type.GetString();
    }
}