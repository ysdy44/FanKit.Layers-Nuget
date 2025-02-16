using System;
using System.Windows.Input;

namespace FanKit.Layers.Sample
{
    partial class MainPage : ICommand
    {
        public ICommand Command => this;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (parameter is OptionType type)
            {
                return this.CanClick(type);
            }
            else if (parameter is int value)
            {
                return this.CanClick((OptionType)value);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            if (parameter is OptionType type)
            {
                this.Click(type);
            }
            else if (parameter is int value)
            {
                this.Click((OptionType)value);
            }
        }

        private bool CanClick(OptionType type)
        {
            switch (type)
            {
                case OptionType.Cut:
                case OptionType.Copy: return this.Clipboard.CanCopy();
                case OptionType.Paste: return this.Clipboard.CanPaste();

                case OptionType.BringToFront: return this.Collection.CanArrange(ArrangeType.BringToFront, this.Selection);
                case OptionType.SendToBack: return this.Collection.CanArrange(ArrangeType.SendToBack, this.Selection);
                case OptionType.BringForward: return this.Collection.CanArrange(ArrangeType.BringForward, this.Selection);
                case OptionType.SendBackward: return this.Collection.CanArrange(ArrangeType.SendBackward, this.Selection);

                case OptionType.Duplicate:
                case OptionType.Remove:
                case OptionType.Group:
                    return this.Selection.IsEmpty is false;
                case OptionType.Release: return this.List.CanRelease(this.Selection);
                default: return true;
            }
        }

        //------------- Command -------------//

        public LanguageCommand LanguageCommand { get; } = new LanguageCommand();
        public UndoCommand UndoCommand { get; }
        public RedoCommand RedoCommand { get; }

        //------------------------ Menu ----------------------------//

        private readonly OptionTypeMenu[] Menus = new OptionTypeMenu[]
        {
            new OptionTypeMenu { Symbol = Symbols.Cut, Type = OptionType.Cut, },
            new OptionTypeMenu { Symbol = Symbols.Copy, Type = OptionType.Copy, },
            new OptionTypeMenu { Symbol = Symbols.Paste, Type = OptionType.Paste, },
            null,
            new OptionTypeMenu { Symbol = Symbols.TwoPage, Type = OptionType.Duplicate, },
            new OptionTypeMenu { Symbol = Symbols.Delete, Type = OptionType.Remove, },
            null,
            new OptionTypeMenu { Symbol = Symbols.ImportAll, Type = OptionType.InsertAtTop, },
            new OptionTypeMenu { Symbol = Symbols.Import, Type = OptionType.Insert,  },
            null,
            new OptionTypeMenu { Symbol = Symbols.BackToWindow, Type = OptionType.Group, },
            new OptionTypeMenu { Symbol = Symbols.FullScreen, Type = OptionType.Ungroup, },
            null,
            new OptionTypeMenu { Symbol = Symbols.ClosePane, Type = OptionType.Release, },
            new OptionTypeMenu { Symbol = Symbols.NewWindow, Type = OptionType.Package, },
        };

        private readonly OptionCatalogMenu[] CatalogMenus = new OptionCatalogMenu[]
        {
            new OptionCatalogMenu
            {
                Catalog = OptionCatalog.File,
                Items =
                {
                    new OptionTypeMenu { Symbol = Symbols.Refresh, Type = OptionType.NewByList, },
                    new OptionTypeMenu { Symbol = Symbols.Refresh, Type = OptionType.NewByTree, },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.Refresh, Type = OptionType.NewByCustomList, },
                    new OptionTypeMenu { Symbol = Symbols.Refresh, Type = OptionType.NewByCustomTree, },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.Save, Type = OptionType.SaveToXmlList, },
                    new OptionTypeMenu { Symbol = Symbols.OpenFile, Type = OptionType.LoadFromXmlList, },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.Save, Type = OptionType.SaveToXmlTree, },
                    new OptionTypeMenu { Symbol = Symbols.OpenFile, Type = OptionType.LoadFromXmlTree, },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.Save, Type = OptionType.SaveToXmlTreeNodes, },
                    new OptionTypeMenu { Symbol = Symbols.OpenFile, Type = OptionType.LoadFromXmlTreeNodes, },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.Clear, Type = OptionType.GC, },
                }
            },
            new OptionCatalogMenu
            {
                Catalog = OptionCatalog.Edit,
                Items =
                {
                    new OptionTypeMenu { Symbol = Symbols.Undo, IsUndo = true, KeyboardAccelerators = "Ctrl+Z", },
                    new OptionTypeMenu { Symbol = Symbols.Redo, IsRedo = true, KeyboardAccelerators = "Ctrl+Y", },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.Cut, Type = OptionType.Cut, KeyboardAccelerators = "Ctrl+X", },
                    new OptionTypeMenu { Symbol = Symbols.Copy, Type = OptionType.Copy, KeyboardAccelerators = "Ctrl+C", },
                    new OptionTypeMenu { Symbol = Symbols.Paste, Type = OptionType.Paste, KeyboardAccelerators = "Ctrl+V", },
                    new OptionTypeMenu { Symbol = Symbols.TwoPage, Type = OptionType.Duplicate, KeyboardAccelerators = "Ctrl+D", },
                    new OptionTypeMenu { Symbol = Symbols.Delete, Type = OptionType.Remove, KeyboardAccelerators = "Delete|Back", },
                    new OptionTypeMenu { Symbol = Symbols.Clear, Type = OptionType.Clear, },
                }
            },
            new OptionCatalogMenu
            {
                Catalog = OptionCatalog.Layer,
                Items =
                {
                    new OptionTypeMenu { Symbol = Symbols.ImportAll, Type = OptionType.InsertAtTop, KeyboardAccelerators = "Add", },
                    new OptionTypeMenu { Symbol = Symbols.Import, Type = OptionType.Insert, KeyboardAccelerators = "Insert", },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.BackToWindow, Type = OptionType.Group, KeyboardAccelerators = "Ctrl+G", },
                    new OptionTypeMenu { Symbol = Symbols.FullScreen, Type = OptionType.Ungroup, KeyboardAccelerators = "Shift+G", },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.ClosePane, Type = OptionType.Release, },
                    new OptionTypeMenu { Symbol = Symbols.NewWindow, Type = OptionType.Package, },
                }
            },
            new OptionCatalogMenu
            {
                Catalog = OptionCatalog.Arrange,
                Items =
                {
                    new OptionTypeMenu { Type = OptionType.BringToFront, },
                    null,
                    new OptionTypeMenu { Type = OptionType.BringForward, },
                    new OptionTypeMenu { Type = OptionType.SendBackward, },
                    null,
                    new OptionTypeMenu { Type = OptionType.SendToBack, },
                }
            },
            new OptionCatalogMenu
            {
                Catalog = OptionCatalog.View,
                Items =
                {
                    new OptionTypeMenu { Symbol = Symbols.Preview, Type = OptionType.HideAll, },
                    new OptionTypeMenu { Symbol = Symbols.Page, Type = OptionType.ShowAll, },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.ViewAll, Type = OptionType.DeselectAll, },
                    new OptionTypeMenu { Symbol = Symbols.SelectAll, Type = OptionType.SelectAll, },
                    null,
                    new OptionTypeMenu { Symbol = Symbols.HideBcc, Type = OptionType.CollapseAll, },
                    new OptionTypeMenu { Symbol = Symbols.ShowBcc, Type = OptionType.ExpandAll, },
                }
            },
        };

        //------------------------ Random ----------------------------//

        private readonly System.Random Random = new System.Random();

        private readonly byte[,] RandomColors = new byte[,]
        {
            {255, 255, 255},
            {223, 223, 223},
            {191, 191, 191},
            {159, 159, 159},
            {127, 127, 127},
            {63, 63, 63},
            {31, 31, 31},
            {0, 0, 0},
            {255, 192, 203},
            {220, 20, 60},
            {255, 240, 245},
            {219, 112, 147},
            {255, 105, 180},
            {199, 21, 133},
            {218, 112, 214},
            {216, 191, 216},
            {221, 160, 221},
            {238, 130, 238},
            {255, 0, 255},
            {139, 0, 139},
            {128, 0, 128},
            {186, 85, 211},
            {148, 0, 211},
            {75, 0, 130},
            {138, 43, 226},
            {147, 112, 219},
            {123, 104, 238},
            {106, 90, 205},
            {72, 61, 139},
            {230, 230, 250},
            {0, 0, 205},
            {25, 25, 112},
            {0, 0, 139},
            {0, 0, 128},
            {65, 105, 225},
            {100, 149, 237},
            {119, 136, 153},
            {112, 128, 144},
            {30, 144, 255},
            {240, 248, 255},
            {70, 130, 180},
            {135, 206, 250},
            {135, 206, 235},
            {0, 191, 255},
            {173, 216, 230},
            {176, 216, 230},
            {95, 158, 160},
            {240, 255, 255},
            {224, 255, 255},
            {175, 238, 238},
            {0, 255, 255},
            {0, 206, 209},
            {47, 79, 79},
            {0, 139, 139},
            {0, 128, 128},
            {72, 209, 204},
            {32, 178, 170},
            {64, 224, 208},
            {127, 255, 212},
            {102, 205, 170},
            {0, 250, 154},
            {245, 255, 250},
            {0, 255, 127},
            {60, 179, 113},
            {46, 139, 87},
            {144, 238, 144},
            {152, 251, 152},
            {143, 188, 143},
            {50, 205, 50},
            {0, 255, 0},
            {34, 139, 34},
            {127, 255, 0},
            {124, 252, 0},
            {173, 255, 47},
            {85, 107, 47},
            {154, 205, 50},
            {107, 142, 35},
            {245, 245, 220},
            {250, 250, 210},
            {255, 255, 240},
            {255, 255, 224},
            {255, 255, 0},
            {128, 128, 0},
            {189, 183, 107},
            {255, 250, 205},
            {238, 232, 170},
            {240, 230, 140},
            {255, 215, 0},
            {255, 248, 220},
            {218, 165, 32},
            {184, 134, 11},
            {255, 250, 240},
            {253, 245, 230},
            {245, 222, 179},
            {255, 228, 181},
            {255, 165, 0},
            {255, 239, 213},
            {255, 235, 205},
            {255, 222, 173},
            {250, 235, 215},
            {210, 180, 140},
            {222, 184, 135},
            {255, 228, 196},
            {255, 140, 0},
            {250, 240, 230},
            {205, 133, 63},
            {244, 164, 96},
            {210, 105, 30},
            {255, 245, 238},
            {160, 82, 45},
            {255, 160, 122},
            {255, 160, 122},
            {255, 69, 0},
            {255, 99, 71},
            {255, 228, 225},
            {250, 128, 114},
            {255, 250, 250},
            {240, 128, 128},
            {188, 143, 143},
            {205, 92, 92},
            {165, 42, 42},
            {178, 34, 34},
            {139, 0, 0},
            {128, 0, 0},
          };
    }
}