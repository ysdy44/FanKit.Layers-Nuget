namespace FanKit.Layers.Ranges
{
    internal static class Constants
    {
        // Length of Range is 0
        internal const byte None = 0;

        //------------------------ Normal ----------------------------//

        // Length of Range is 1
        internal const byte Index = 1;

        // Length of Range is N
        internal const byte Range = 2; // A Parent + some Children

        //------------------------ All ----------------------------//

        // Selection is All
        internal const byte All = 4;

        //------------------------ First & Last ----------------------------//

        // Selection is First of items
        internal const byte First = 8;

        // Selection is Last of items
        internal const byte Last = 16;

        #region Selection1

        /*
         * @para items Count of selected items
         * @para ranges Count of selected ranges
         */
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool ToIsEmpty(this Selection1 selection)
        {
            int items = selection.SelectedItemsCount;
            int ranges = selection.SelectedRangesCount;

            return items == 0 || ranges == 0;
        }

        /*
         * @para items Count of selected items
         * @para ranges Count of selected ranges
         */
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SelTyp1 ToSelTyp1(this Selection1 selection)
        {
            int items = selection.SelectedItemsCount;
            int ranges = selection.SelectedRangesCount;

            switch (items)
            {
                case 0:
                    return SelTyp1.Non;
                case 1:
                    return SelTyp1.S;
                default:
                    switch (ranges)
                    {
                        case 0:
                            return SelTyp1.Non;
                        case 1:
                            return SelTyp1.SR;
                        default:
                            return SelTyp1.M;
                    }
            }
        }

        #endregion

        #region Selection2

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SelectionCount ToSelectionCount(this SelTyp2 type2)
        {
            switch (type2)
            {
                case SelTyp2.Non: return SelectionCount.None;
                case SelTyp2.S: return SelectionCount.Single;
                case SelTyp2.SR:
                case SelTyp2.M: return SelectionCount.Multiple;

                case SelTyp2.AS:
                case SelTyp2.ASR:
                case SelTyp2.AM: return SelectionCount.Multiple;
                default: return SelectionCount.None;
            }
        }

        /*
         * @para items Count of selected items
         * @para ranges Count of selected ranges
         * @para count Count of items
         */
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static RemovalCount ToRemovalCount(this Selection1 selection, int count)
        {
            int items = selection.SelectedItemsCount;
            int ranges = selection.SelectedRangesCount;

            switch (items)
            {
                case 0:
                    return RemovalCount.None;
                case 1:
                    return count > 1 ? RemovalCount.Remove : RemovalCount.RemoveAll;
                default:
                    switch (ranges)
                    {
                        case 0:
                            return RemovalCount.None;
                        case 1:
                            return count > items ? RemovalCount.Remove : RemovalCount.RemoveAll;
                        default:
                            return count > items ? RemovalCount.Remove : RemovalCount.RemoveAll;
                    }
            }
        }

        /*
         * @para items Count of selected items
         * @para ranges Count of selected ranges
         * @para count Count of items
         */
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SelTyp2 ToSelTyp2(this Selection1 selection, int count)
        {
            int items = selection.SelectedItemsCount;
            int ranges = selection.SelectedRangesCount;

            switch (items)
            {
                case 0:
                    return SelTyp2.Non;
                case 1:
                    return count > 1 ? SelTyp2.S : SelTyp2.AS;
                default:
                    switch (ranges)
                    {
                        case 0:
                            return SelTyp2.Non;
                        case 1:
                            return count > items ? SelTyp2.SR : SelTyp2.ASR;
                        default:
                            return count > items ? SelTyp2.M : SelTyp2.AM;
                    }
            }
        }

        #endregion

        #region IndexSelection

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static RemovalCount ToRemovalCount(this SelTyp3 type3)
        {
            switch (type3)
            {
                case SelTyp3.Non: return RemovalCount.None;
                case SelTyp3.AS:
                case SelTyp3.ASR:
                case SelTyp3.AM: return RemovalCount.RemoveAll;
                default: return RemovalCount.Remove;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SelectionCount ToSelectionCount(this SelTyp3 type3)
        {
            switch (type3)
            {
                case SelTyp3.Non: return SelectionCount.None;
                case SelTyp3.S:
                case SelTyp3.AS:
                case SelTyp3.FS:
                case SelTyp3.BS: return SelectionCount.Single;
                default: return SelectionCount.Multiple;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SelTyp1 ToSelTyp1(this SelTyp3 type3)
        {
            switch (type3)
            {
                case SelTyp3.Non: return SelTyp1.Non;
                case SelTyp3.S: return SelTyp1.S;
                case SelTyp3.SR: return SelTyp1.SR;
                case SelTyp3.M: return SelTyp1.M;

                case SelTyp3.AS: return SelTyp1.S;
                case SelTyp3.ASR: return SelTyp1.SR;
                case SelTyp3.AM: return SelTyp1.M;

                case SelTyp3.FS: return SelTyp1.S;
                case SelTyp3.FSR: return SelTyp1.SR;
                case SelTyp3.BS: return SelTyp1.S;
                case SelTyp3.BSR: return SelTyp1.SR;
                default: return SelTyp1.Non;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SelTyp2 ToSelTyp2(this SelTyp3 type3)
        {
            switch (type3)
            {
                case SelTyp3.Non: return SelTyp2.Non;
                case SelTyp3.S: return SelTyp2.S;
                case SelTyp3.SR: return SelTyp2.SR;
                case SelTyp3.M: return SelTyp2.M;

                case SelTyp3.AS: return SelTyp2.AS;
                case SelTyp3.ASR: return SelTyp2.ASR;
                case SelTyp3.AM: return SelTyp2.AM;

                case SelTyp3.FS: return SelTyp2.S;
                case SelTyp3.FSR: return SelTyp2.SR;
                case SelTyp3.BS: return SelTyp2.S;
                case SelTyp3.BSR: return SelTyp2.SR;
                default: return SelTyp2.Non;
            }
        }

        #endregion
    }
}