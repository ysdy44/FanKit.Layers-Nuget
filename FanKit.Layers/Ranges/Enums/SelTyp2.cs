namespace FanKit.Layers.Ranges
{
    // Selection Type 2
    internal enum SelTyp2 : byte
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

        //------------------------ All ----------------------------//

        // All_Single
        AS = Constants.All | Constants.Index,

        // All_SingleRange
        ASR = Constants.All | Constants.Range,

        // All_Multiple
        AM = Constants.All | Constants.Range | Constants.Index,
    }
}