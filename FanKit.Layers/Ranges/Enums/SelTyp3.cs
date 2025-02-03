namespace FanKit.Layers.Ranges
{
    // Selection Type 3
    internal enum SelTyp3 : byte
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

        //------------------------ Front ----------------------------//

        // Front_Single
        FS = Constants.First | Constants.Index,

        // Front_SingleRange
        FSR = Constants.First | Constants.Range,

        //------------------------ Back ----------------------------//

        // Back_Single
        BS = Constants.Last | Constants.Index,

        // Back_SingleRange
        BSR = Constants.Last | Constants.Range,
    }
}