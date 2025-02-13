using System.Collections.Generic;

namespace FanKit.Layers.Sample
{
    public class OptionCatalogMenu
    {
        public OptionCatalog Catalog { get; set; }
        public List<OptionTypeMenu> Items { get; } = new List<OptionTypeMenu>();
    }
}