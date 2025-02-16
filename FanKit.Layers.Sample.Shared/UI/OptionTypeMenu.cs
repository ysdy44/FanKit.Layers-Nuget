namespace FanKit.Layers.Sample
{
    public class OptionTypeMenu
    {
        public bool IsUndo { get; set; }
        public bool IsRedo { get; set; }

        public Symbols? Symbol { get; set; }
        public OptionType Type { get; set; }
        public string KeyboardAccelerators { get; set; }
    }
}