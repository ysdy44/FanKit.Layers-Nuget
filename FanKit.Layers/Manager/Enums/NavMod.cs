namespace FanKit.Layers
{
    // Navigation Mode
    internal enum NavMod
    {
        // None
        Non,

        // Singe Undo
        SU,
        // Singe Redo
        SR,

        // Multiple Undo
        MU,
        // Multiple Redo
        MR,

        // Unknown
        Unk,
    }
}