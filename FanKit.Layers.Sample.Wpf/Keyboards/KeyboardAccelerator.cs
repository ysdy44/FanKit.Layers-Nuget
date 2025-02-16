using System.Linq;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    public readonly struct KeyboardAccelerator
    {
        public readonly Key Key;
        public readonly ModifierKeys Modifiers;

        public KeyboardAccelerator(string keyboardAccelerator)
        {
            string[] split = keyboardAccelerator.Split('+');

            this.Key = (Key)System.Enum.Parse(typeof(Key), split.Last());
            switch (split.First())
            {
                case "Ctrl":
                    this.Modifiers = ModifierKeys.Control;
                    break;
                case "Shift":
                    this.Modifiers = ModifierKeys.Shift;
                    break;
                default:
                    this.Modifiers = ModifierKeys.None;
                    break;
            }
        }
    }
}