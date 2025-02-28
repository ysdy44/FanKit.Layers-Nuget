using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Layers.Sample
{
    public class CatalogToolbarItem : ToolbarItem
    {
        public readonly Dictionary<string, OptionType> Dictionary = new Dictionary<string, OptionType>();

        public readonly string[] Buttons;

        public MainPage XamlRoot;

        public CatalogToolbarItem(OptionCatalogMenu catalog)
        {
            foreach (OptionTypeMenu item in catalog.Items)
            {
                if (item is null)
                    continue;

                string key = item.Type.GetString();

                if (this.Dictionary.ContainsKey(key))
                    continue;

                this.Dictionary.Add(key, item.Type);
            }

            this.Buttons = this.Dictionary.Keys.ToArray();

            base.Text = catalog.Catalog.GetString();
        }

        protected override async void OnClicked()
        {
            base.OnClicked();

            string result = await this.XamlRoot.DisplayActionSheet(
                base.Text,
                UIType.UIBack.GetString(),
                UIType.UIBack.GetString(),
                this.Buttons);

            if (this.Dictionary.ContainsKey(result))
            {
                this.XamlRoot.Command.Execute(this.Dictionary[result]);
            }
        }
    }
}