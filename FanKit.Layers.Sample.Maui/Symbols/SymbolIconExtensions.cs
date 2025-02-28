namespace FanKit.Layers.Sample
{
    public static class SymbolIconExtensions
    {
        public static string ToFile(this Symbols symbol)
        {
            switch (symbol)
            {
                case Symbols.Save: return "e105.png";
                case Symbols.Clear: return "e106.png";
                case Symbols.Delete: return "e107.png";
                case Symbols.Redo: return "e10d.png";
                case Symbols.Undo: return "e10e.png";
                case Symbols.Mail: return "e119.png";
                case Symbols.TwoPage: return "e11e.png";
                case Symbols.Clock: return "e121.png";
                case Symbols.ClosePane: return "e127.png";
                case Symbols.Page: return "e132.png";
                case Symbols.ViewAll: return "e138.png";
                case Symbols.Refresh: return "e149.png";
                case Symbols.SelectAll: return "e14e.png";
                case Symbols.Import: return "e150.png";
                case Symbols.ImportAll: return "e151.png";
                case Symbols.Read: return "e166.png";
                case Symbols.Link: return "e167.png";
                case Symbols.ShowBcc: return "e169.png";
                case Symbols.HideBcc: return "e16a.png";
                case Symbols.Cut: return "e16b.png";
                case Symbols.Paste: return "e16d.png";
                case Symbols.Copy: return "e16f.png";
                case Symbols.Important: return "e171.png";
                case Symbols.NewWindow: return "e17c.png";
                case Symbols.Folder: return "e188.png";
                case Symbols.View: return "e18b.png";
                case Symbols.OpenFile: return "e1a5.png";
                case Symbols.BackToWindow: return "e1d8.png";
                case Symbols.FullScreen: return "e1d9.png";
                case Symbols.NewFolder: return "e1da.png";
                case Symbols.Preview: return "e295.png";
                default: return string.Empty;
            }
        }
    }
}