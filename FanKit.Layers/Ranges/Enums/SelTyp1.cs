namespace FanKit.Layers.Ranges
{
    // Selection Type 1
    internal enum SelTyp1 : byte
    {
        // None
        Non = Constants.None,

        //------------------------ Normal ----------------------------//

        // Single
        S = Constants.Index,

        // SingleRange
        SR = Constants.Range,

        // Multiple
        M = Constants.Range | Constants.Index,
    }
}