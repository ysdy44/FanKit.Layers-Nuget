using System;
using System.Windows.Input;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes']/*" />
    [Flags]
    public enum InvalidateModes
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.None']/*" />
        None = 0,

        //-------------Selection-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.SelectionCleared']/*" />
        SelectionCleared = 1,
        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.SelectionChanged']/*" />
        SelectionChanged = 2,

        //-------------Canvas-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.CanvasControlInvalidate']/*" />
        CanvasControlInvalidate = 8,
        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.AllThumbnailInvalidate']/*" />
        AllThumbnailInvalidate = 16,

        //-------------Layer-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.LayersCleared']/*" />
        LayersCleared = 32,
        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.LayersChanged']/*" />
        LayersChanged = 64,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.LayerCanExecuteChanged']/*" />
        LayerCanExecuteChanged = 128,

        //-------------History-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.HistoryCleared']/*" />
        HistoryCleared = 256,
        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.HistoryChanged']/*" />
        HistoryChanged = 512,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.HistoryCanExecuteChanged']/*" />
        HistoryCanExecuteChanged = 1024,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.HistorySelectionChanged']/*" />
        HistorySelectionChanged = 2048,

        //-------------Output-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Output']/*" />
        Output = 4096,

        //-------------Flags 0-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Reset']/*" />
        Reset = None
            | SelectionChanged
            | CanvasControlInvalidate
            | AllThumbnailInvalidate

            | LayersCleared
            | LayersChanged
            | LayerCanExecuteChanged

            | HistoryCleared
            | HistoryCanExecuteChanged

            | Output,

        //-------------Flags 1-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Clear']/*" />
        Clear = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayersCleared
            | LayerCanExecuteChanged

            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.ClearUndo']/*" />
        ClearUndo = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayersCleared
            | LayerCanExecuteChanged

            //| HistoryChanged
            //| HistoryCanExecuteChanged
            //| HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Sort']/*" />
        Sort = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayersChanged
            | LayerCanExecuteChanged

            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.SortUndo']/*" />
        SortUndo = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayersChanged
            | LayerCanExecuteChanged

            //| HistoryChanged
            //| HistoryCanExecuteChanged
            //| HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.ClearAndSort']/*" />
        ClearAndSort = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayersCleared
            | LayersChanged
            | LayerCanExecuteChanged

            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        //-------------Flags 2-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Expand']/*" />
        Expand = None
            | LayersChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Lock']/*" />
        Lock = None
            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.LockUndo']/*" />
        LockUndo = None
            //| HistoryChanged
            //| HistoryCanExecuteChanged
            //| HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Visible']/*" />
        Visible = None
            | CanvasControlInvalidate

            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.VisibleUndo']/*" />
        VisibleUndo = None
            | CanvasControlInvalidate

            //| HistoryChanged
            //| HistoryCanExecuteChanged
            //| HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.Select']/*" />
        Select = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayerCanExecuteChanged

            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.SelectUndo']/*" />
        SelectUndo = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayerCanExecuteChanged

            //| HistoryChanged
            //| HistoryCanExecuteChanged
            //| HistorySelectionChanged

            | Output,

        //-------------Flags 3-------------//

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.ValueChangeStarted']/*" />
        ValueChangeStarted = None
            | SelectionChanged
            | CanvasControlInvalidate,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.ValueChangeDelta']/*" />
        ValueChangeDelta = None
            | CanvasControlInvalidate,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.ValueChangeCompleted']/*" />
        ValueChangeCompleted = None
            | CanvasControlInvalidate

            | LayerCanExecuteChanged

            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.ValueChanged']/*" />
        ValueChanged = None
            | SelectionChanged
            | CanvasControlInvalidate

            | LayerCanExecuteChanged

            | HistoryChanged
            | HistoryCanExecuteChanged
            | HistorySelectionChanged

            | Output,

        /// <include file="doc/docs.xml" path="docs/doc[@for='InvalidateModes.ValueChangedUndo']/*" />
        ValueChangedUndo = None
            | SelectionChanged
            | CanvasControlInvalidate

            //| HistoryChanged
            //| HistoryCanExecuteChanged
            //| HistorySelectionChanged

            | Output,
    }
}