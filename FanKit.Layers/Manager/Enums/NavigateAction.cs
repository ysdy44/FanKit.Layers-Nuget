using FanKit.Layers.Collections;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateAction']/*" />
    public enum NavigateAction
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateAction.None']/*" />
        None,

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateAction.Undo']/*" />
        Undo,

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateAction.Redo']/*" />
        Redo,

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateAction.UndoRange']/*" />
        UndoRange,

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateAction.RedoRange']/*" />
        RedoRange,
    }
}