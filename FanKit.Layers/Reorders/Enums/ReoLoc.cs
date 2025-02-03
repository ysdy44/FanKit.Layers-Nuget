namespace FanKit.Layers.Reorders
{
    // ReorderLocation
    // The Index Location of Drop Target in List
    internal enum ReoLoc : byte
    {
        Non, // None

        SSX, // Sibling_Single_IndexOf
        SSL, // Sibling_Single_Last
        SSF, // Sibling_Single_First

        SSRX, // Sibling_SingleRange_IndexOf
        SSRL, // Sibling_SingleRange_Last
        SSRF, // Sibling_SingleRange_First

        NSM, // NextSibling_Multiple

        PSM, // PreviousSibling_Multiple

        SX, // Single_IndexOf
        SL, // Single_Last
        SF, // Single_First

        SRX, // SingleRange_IndexOf
        SRL, // SingleRange_Last
        SRF, // SingleRange_First

        MX, // Multiple_IndexOf
        ML, // Multiple_Last
        MF, // Multiple_First
    }
}