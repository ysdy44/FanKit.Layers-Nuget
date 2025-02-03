namespace FanKit.Layers.Reorders
{
    internal enum MoveTo : byte
    {
        Non, // None


        SN, // Single_MoveToNext
        SP, // Single_MoveToPrevious

        SA, // Single_MoveAfter
        SB, // Single_MoveBefore

        SL, // Single_MoveToLast
        SF, // Single_MoveToFirst


        SRN, // SingleRange_MoveToNext
        SRP, // SingleRange_MoveToPrevious

        SRA, // SingleRange_MoveAfter
        SRB, // SingleRange_MoveBefore

        SRL, // SingleRange_MoveToLast
        SRF, // SingleRange_MoveToFirst


        MN, // Multiple_MoveToNext
        MP, // Multiple_MoveToPrevious

        MA, // Multiple_MoveAfter
        MB, // Multiple_MoveBefore

        ML, // Multiple_MoveToLast
        MF, // Multiple_MoveToFirst
    }
}