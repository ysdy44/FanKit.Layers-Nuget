﻿using FanKit.Layers.Demo;
using FanKit.Layers.Options;
using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace FanKit.Layers.Sample
{
    partial class MainPage
    {
        public void Click(OptionType type)
        {
            switch (type)
            {
                case OptionType.GC:
                    int gcs = this.History.GC();
                    if (gcs == 0)
                    {
                        MessageBox.Show(
                            UIType.UINotFound.GetString(),
                            UIType.UIGCFailed.GetString()
                            );
                    }
                    else
                    {
                        MessageBox.Show(
                            string.Format(UIType.UIDispose.GetString(), gcs),
                            UIType.UIGCSuccess.GetString()
                            );
                    }
                    break;
                case OptionType.NewByList:
                    {
                        // ■■■■■ Depth = 0
                        // ■■■■■ Depth = 0
                        // ■■■■■ Depth = 0
                        // □■■■■■ Depth = 1
                        // □□■■■■■ Depth = 2
                        // □□■■■■■ Depth = 2
                        // □□■■■■■ Depth = 2
                        // ■■■■■ Depth = 0
                        // □■■■■■ Depth = 1
                        // ■■■■■ Depth = 0
                        // □■■■■■ Depth = 1
                        IEnumerable<ILayer> items = new ILayer[]
                        {
                            new BitmapLayer
                            {
                                Depth = 0,

                                Rect = this.RandomRect(),

                                BitmapThumbnail = new Thumbnail(this.CanvasControl),

                                BitmapWidth = this.BitmapWidth,
                                BitmapHeight = this.BitmapHeight,
                                Bitmap = this.Bitmap,
                            },
                            new BitmapLayer
                            {
                                Depth = 0,

                                Rect = this.RandomRect(),

                                BitmapThumbnail = new Thumbnail(this.CanvasControl),

                                BitmapWidth = this.BitmapWidth,
                                BitmapHeight = this.BitmapHeight,
                                Bitmap = this.Bitmap,
                            },
                            new GroupLayer
                            {
                                Depth = 0,
                                IsExpanded = true,
                            },
                            new GroupLayer
                            {
                                Depth = 1,
                                IsExpanded = true,
                            },
                            new FillLayer
                            {
                                Depth = 2,

                                Fill = this.RandomColor(),
                                Rect = this.RandomRect(),
                            },
                            new FillLayer
                            {
                                Depth = 2,

                                Fill = this.RandomColor(),
                                Rect = this.RandomRect(),
                            },
                            new FillLayer
                            {
                                Depth = 2,

                                Fill = this.RandomColor(),
                                Rect = this.RandomRect(),
                            },
                            new GroupLayer
                            {
                                Depth = 0,
                                IsExpanded = true,
                            },
                            new BitmapLayer
                            {
                                Depth = 1,

                                Rect = this.RandomRect(),

                                BitmapThumbnail = new Thumbnail(this.CanvasControl),

                                BitmapWidth = this.BitmapWidth,
                                BitmapHeight = this.BitmapHeight,
                                Bitmap = this.Bitmap,
                            },
                            new GroupLayer
                            {
                                Depth = 0,
                                IsExpanded = true,
                            },
                            new GroupLayer
                            {
                                Depth = 1,
                            }
                        };

                        InvalidateModes modes = this.Collection.ResetByList(items);
                        this.Invalidate(modes);
                    }
                    break;
                case OptionType.NewByTree:
                    {
                        // ■■■■■
                        // ■■■■■
                        // ■■■■■
                        // {
                        //    ■■■■■
                        //    {
                        //         ■■■■■
                        //         ■■■■■
                        //         ■■■■■
                        //    }
                        // }
                        // ■■■■■
                        // {
                        //     ■■■■■
                        // }
                        // ■■■■■
                        // {
                        //     ■■■■■
                        // }
                        IEnumerable<ILayer> items = new ILayer[]
                        {
                            new BitmapLayer
                            {
                                Depth = 0,

                                Rect = this.RandomRect(),

                                BitmapThumbnail = new Thumbnail(this.CanvasControl),

                                BitmapWidth = this.BitmapWidth,
                                BitmapHeight = this.BitmapHeight,
                                Bitmap = this.Bitmap,
                            },
                            new BitmapLayer
                            {
                                Depth = 0,

                                Rect = this.RandomRect(),

                                BitmapThumbnail = new Thumbnail(this.CanvasControl),

                                BitmapWidth = this.BitmapWidth,
                                BitmapHeight = this.BitmapHeight,
                                Bitmap = this.Bitmap,
                            },
                            new GroupLayer
                            {
                                Depth = 0,
                                IsExpanded = true,
                                Children =
                                {
                                    new GroupLayer
                                    {
                                        Depth = 1,
                                        IsExpanded = true,
                                        Children =
                                        {
                                            new FillLayer
                                            {
                                                Depth = 2,

                                                Fill = this.RandomColor(),
                                                Rect = this.RandomRect(),
                                            },
                                            new FillLayer
                                            {
                                                Depth = 2,

                                                Fill = this.RandomColor(),
                                                Rect = this.RandomRect(),
                                            },
                                            new FillLayer
                                            {
                                                Depth = 2,

                                                Fill = this.RandomColor(),
                                                Rect = this.RandomRect(),
                                            }
                                        }
                                    }
                                }
                            },
                            new GroupLayer
                            {
                                Depth = 0,
                                IsExpanded = true,
                                Children =
                                {
                                    new BitmapLayer
                                    {
                                        Depth = 1,

                                        Rect = this.RandomRect(),

                                        BitmapThumbnail = new Thumbnail(this.CanvasControl),

                                        BitmapWidth = this.BitmapWidth,
                                        BitmapHeight = this.BitmapHeight,
                                        Bitmap = this.Bitmap,
                                    }
                                }
                            },
                            new GroupLayer
                            {
                                Depth = 0,
                                IsExpanded = true,
                                Children =
                                {
                                    new GroupLayer
                                    {
                                        Depth = 1,
                                    }
                                }
                            }
                        };

                        InvalidateModes modes = this.Collection.ResetByTree(items);
                        this.Invalidate(modes);
                    }
                    break;
                case OptionType.NewByCustomList:
                    {
                        // ■■■■■ Depth = 0
                        // ■■■■■ Depth = 0
                        // ■■■■■ Depth = 0
                        // □■■■■■ Depth = 1
                        // □□■■■■■ Depth = 2
                        // □□■■■■■ Depth = 2
                        // □□■■■■■ Depth = 2
                        // ■■■■■ Depth = 0
                        // □■■■■■ Depth = 1
                        // ■■■■■ Depth = 0
                        // □■■■■■ Depth = 1
                        IEnumerable<CustomStruct> items = new CustomStruct[]
                        {
                            new CustomStruct
                            {
                                Depth = 0,
                            },
                            new CustomStruct
                            {
                                Depth = 0,
                            },
                            new CustomStruct
                            {
                                IsGroup = true,
                                Depth = 0,
                            },
                            new CustomStruct
                            {
                                IsGroup = true,
                                Depth = 1,
                            },
                            new CustomStruct
                            {
                                Depth = 2,
                            },
                            new CustomStruct
                            {
                                Depth = 2,
                            },
                            new CustomStruct
                            {
                                Depth = 2,
                            },
                            new CustomStruct
                            {
                                IsGroup = true,
                                Depth = 0,
                            },
                            new CustomStruct
                            {
                                Depth = 1,
                            },
                            new CustomStruct
                            {
                                IsGroup = true,
                                Depth = 0,
                            },
                            new CustomStruct
                            {
                                Depth = 1,
                            }
                        };

                        InvalidateModes modes = this.Collection.ResetByCustomList(items, this.CreateAndLoad);
                        this.Invalidate(modes);
                    }
                    break;
                case OptionType.NewByCustomTree:
                    {
                        // ■■■■■
                        // ■■■■■
                        // ■■■■■
                        // {
                        //    ■■■■■
                        //    {
                        //         ■■■■■
                        //         ■■■■■
                        //         ■■■■■
                        //    }
                        // }
                        // ■■■■■
                        // {
                        //     ■■■■■
                        // }
                        // ■■■■■
                        // {
                        //     ■■■■■
                        // }
                        IEnumerable<CustomClass> items = new CustomClass[]
                        {
                            new CustomClass(),
                            new CustomClass(),
                            new CustomClass
                            {
                                Children = new List<CustomClass>
                                {
                                    new CustomClass
                                    {
                                        Children = new List<CustomClass>
                                        {
                                            new CustomClass(),
                                            new CustomClass(),
                                            new CustomClass(),
                                        }
                                    }
                                }
                            },
                            new CustomClass
                            {
                                Children = new List<CustomClass>
                                {
                                    new CustomClass()
                                }
                            },
                            new CustomClass
                            {
                                Children = new List<CustomClass>
                                {
                                    new CustomClass()
                                }
                            }
                        };

                        InvalidateModes modes = this.Collection.ResetByCustomTree(items, this.CreateAndLoadWithDepth);
                        this.Invalidate(modes);
                    }
                    break;
                case OptionType.SaveToXmlList:
                    {
                        // Picker
                        Microsoft.Win32.SaveFileDialog savePicker = new Microsoft.Win32.SaveFileDialog
                        {
                            FileName = "Untitled.xml",
                            DefaultExt = ".xml",
                            Filter = "XML Files (*.xml)|*.xml"
                        };

                        // File
                        bool? result = savePicker.ShowDialog();

                        if (result != true)
                            break;

                        using (Stream stream = savePicker.OpenFile())
                        {
                            stream.SetLength(0);

                            XDocument doc = new XDocument(this.List.SaveToXmlList());

                            doc.Save(stream, SaveOptions.DisableFormatting);
                        }
                    }
                    break;
                case OptionType.LoadFromXmlList:
                    {
                        // Picker
                        Microsoft.Win32.OpenFileDialog openPicker = new Microsoft.Win32.OpenFileDialog
                        {
                            DefaultExt = ".xml",
                            FileName = "Untitled.xml",
                            Filter = "XML Files (*.xml)|*.xml",
                            FilterIndex = 1,
                        };

                        // File
                        bool? result = openPicker.ShowDialog();

                        if (result != true)
                            break;

                        using (Stream stream = openPicker.OpenFile())
                        {
                            XDocument doc = XDocument.Load(stream);

                            InvalidateModes modes = this.Collection.ResetByXmlList(doc.Root.Elements(), this.Create);

                            this.Invalidate(modes);
                        }
                    }
                    break;
                case OptionType.SaveToXmlTree:
                    {
                        // Picker
                        Microsoft.Win32.SaveFileDialog savePicker = new Microsoft.Win32.SaveFileDialog
                        {
                            FileName = "Untitled.xml",
                            DefaultExt = ".xml",
                            Filter = "XML Files (*.xml)|*.xml"
                        };

                        // File
                        bool? result = savePicker.ShowDialog();

                        if (result != true)
                            break;

                        using (Stream stream = savePicker.OpenFile())
                        {
                            stream.SetLength(0);

                            XmlTreeNode[] nodes = this.List.GetNodes();

                            XDocument doc = new XDocument(this.Collection.SaveToXmlTree(nodes));

                            doc.Save(stream, SaveOptions.DisableFormatting);
                        }
                    }
                    break;
                case OptionType.LoadFromXmlTree:
                    {
                        // Picker
                        Microsoft.Win32.OpenFileDialog openPicker = new Microsoft.Win32.OpenFileDialog
                        {
                            DefaultExt = ".xml",
                            FileName = "Untitled.xml",
                            Filter = "XML Files (*.xml)|*.xml",
                            FilterIndex = 1,
                        };

                        // File
                        bool? result = openPicker.ShowDialog();

                        if (result != true)
                            break;

                        using (Stream stream = openPicker.OpenFile())
                        {
                            XDocument doc = XDocument.Load(stream);

                            InvalidateModes modes = this.Collection.ResetByXmlTree(doc.Root.Elements(), this.CreateWithDepth);

                            this.Invalidate(modes);
                        }
                    }
                    break;
                case OptionType.SaveToXmlTreeNodes:
                    {
                        // Picker
                        Microsoft.Win32.SaveFileDialog savePicker = new Microsoft.Win32.SaveFileDialog
                        {
                            FileName = "Untitled.xml",
                            DefaultExt = ".xml",
                            Filter = "XML Files (*.xml)|*.xml"
                        };

                        // File
                        bool? result = savePicker.ShowDialog();

                        if (result != true)
                            break;

                        using (Stream stream = savePicker.OpenFile())
                        {
                            stream.SetLength(0);

                            XmlTreeNode[] nodes = this.List.GetNodes();

                            XDocument doc = new XDocument(this.List.SaveToXmlTreeNodes01(nodes));

                            doc.Save(stream, SaveOptions.DisableFormatting);
                        }
                    }
                    break;
                case OptionType.LoadFromXmlTreeNodes:
                    {
                        // Picker
                        Microsoft.Win32.OpenFileDialog openPicker = new Microsoft.Win32.OpenFileDialog
                        {
                            DefaultExt = ".xml",
                            FileName = "Untitled.xml",
                            Filter = "XML Files (*.xml)|*.xml",
                            FilterIndex = 1,
                        };

                        // File
                        bool? result = openPicker.ShowDialog();

                        if (result != true)
                            break;

                        using (Stream stream = openPicker.OpenFile())
                        {
                            XDocument doc = XDocument.Load(stream);

                            XmlTreeNode[] nodes = doc.LoadFromXmlTreeNodes0();
                            IEnumerable<XElement> items = doc.LoadFromXmlTreeNodes1();

                            InvalidateModes modes = this.Collection.ResetByXmlTreeNodes(items, nodes, this.Create);

                            this.Invalidate(modes);
                        }
                    }
                    break;
                case OptionType.Cut:
                    this.Invalidate(this.TryCut());
                    break;
                case OptionType.Copy:
                    this.TryCopy();
                    this.Invalidate(InvalidateModes.LayerCanExecuteChanged);
                    break;
                case OptionType.Paste:
                    this.Invalidate(this.TryPaste());
                    break;
                case OptionType.Duplicate:
                    this.Invalidate(this.TryDuplicateSelection());
                    break;
                case OptionType.Remove:
                    this.Invalidate(this.TryRemoveSelection());
                    break;
                case OptionType.Clear:
                    this.Invalidate(this.TryClear());
                    break;
                case OptionType.InsertAtTop:
                    this.Invalidate(this.TryInsertAtTop(new BitmapLayer
                    {
                        SelectMode = SelectMode.Selected,

                        Rect = this.RandomRect(),

                        BitmapThumbnail = new Thumbnail(this.CanvasControl),

                        BitmapWidth = this.BitmapWidth,
                        BitmapHeight = this.BitmapHeight,
                        Bitmap = this.Bitmap,
                    }));
                    break;
                case OptionType.Insert:
                    {
                        Inserter inserter = new Inserter(this.List);

                        this.Invalidate(this.TryInsert(inserter, new BitmapLayer
                        {
                            Depth = inserter.Depth,

                            SelectMode = SelectMode.Selected,

                            Rect = this.RandomRect(),

                            BitmapThumbnail = new Thumbnail(this.CanvasControl),

                            BitmapWidth = this.BitmapWidth,
                            BitmapHeight = this.BitmapHeight,
                            Bitmap = this.Bitmap,
                        }));
                    }
                    break;
                case OptionType.Ungroup:
                    this.Invalidate(this.TryUngroupSelection());
                    break;
                case OptionType.Group:
                    {
                        Grouper grouper = new Grouper(this.List);

                        switch (grouper.Count)
                        {
                            case SelectionCount.None:
                                break;
                            case SelectionCount.Single:
                                this.Invalidate(this.TryGroupSingle(grouper, new GroupLayer
                                {
                                    Depth = grouper.Depth,
                                    IsExpanded = false,

                                    SelectMode = SelectMode.Selected,
                                }));
                                break;
                            case SelectionCount.Multiple:
                                this.Invalidate(this.TryGroupMultiple(grouper, new GroupLayer
                                {
                                    Depth = grouper.Depth,
                                    IsExpanded = false,

                                    SelectMode = SelectMode.Selected,
                                }));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case OptionType.Release:
                    {
                        Releaser releaser = new Releaser(this.List);

                        switch (releaser.Count)
                        {
                            case SelectionCount.None:
                                break;
                            case SelectionCount.Single:
                                this.Invalidate(this.TryReleaseSingle(releaser));
                                break;
                            case SelectionCount.Multiple:
                                this.Invalidate(this.TryReleaseMultiple(releaser));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case OptionType.Package:
                    this.Invalidate(this.TryPackage(new GroupLayer
                    {
                        IsExpanded = false,

                        SelectMode = SelectMode.Selected,
                    }));
                    break;
                case OptionType.BringToFront:
                    this.Invalidate(this.Arrange(ArrangeType.BringToFront));
                    break;
                case OptionType.SendToBack:
                    this.Invalidate(this.Arrange(ArrangeType.SendToBack));
                    break;
                case OptionType.BringForward:
                    this.Invalidate(this.Arrange(ArrangeType.BringForward));
                    break;
                case OptionType.SendBackward:
                    this.Invalidate(this.Arrange(ArrangeType.SendBackward));
                    break;
                case OptionType.SelectAll:
                    this.Invalidate(this.TrySelectAll());
                    break;
                case OptionType.DeselectAll:
                    this.Invalidate(this.TryDeselectAll());
                    break;
                case OptionType.CollapseAll:
                    this.Collection.CollapseAll();
                    this.Invalidate(InvalidateModes.LayersChanged);
                    break;
                case OptionType.ExpandAll:
                    this.Collection.ExpandAll();
                    this.Invalidate(InvalidateModes.LayersChanged);
                    break;
                case OptionType.HideAll:
                    this.Invalidate(this.TryHideAll());
                    break;
                case OptionType.ShowAll:
                    this.Invalidate(this.TryShowAll());
                    break;
                default:
                    break;
            }
        }
    }
}