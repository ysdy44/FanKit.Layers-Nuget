using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FanKit.Layers.Sample
{
    public class LanguageToolbarItem : ToolbarItem
    {
        public readonly Dictionary<string, string> Dictionary = new Dictionary<string, string>();

        public readonly string[] Buttons;

        public MainPage XamlRoot;

        public LanguageToolbarItem(IEnumerable<CultureInfo> cultures)
        {
            this.Dictionary.Add(UIType.UseSystemSetting.GetString(), string.Empty);

            foreach (CultureInfo item in cultures)
            {
                this.Dictionary.Add(item.NativeName, item.Name);
            }

            this.Buttons = this.Dictionary.Keys.ToArray();

            base.Text = UIType.Language.GetString();
        }

        protected override async void OnClicked()
        {
            base.OnClicked();

            string result = await this.XamlRoot.DisplayActionSheet(
                UIType.Language.GetString(),
                UIType.UIBack.GetString(),
                UIType.UIBack.GetString(),
                this.Buttons);

            if (this.Dictionary.ContainsKey(result))
            {
                this.XamlRoot.LanguageCommand.Execute(this.Dictionary[result]);
            }
        }
    }
}