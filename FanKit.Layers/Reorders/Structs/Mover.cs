using FanKit.Layers.Ranges;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanKit.Layers.Reorders
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Mover
    {
        public readonly MoveDirect Direct;
        public readonly int Index;

        public static Mover BringToFront { get; } = new Mover(MoveDirect.First, -1);
        public static Mover SendToBack { get; } = new Mover(MoveDirect.Last, -1);

        public static Mover BringForward { get; } = new Mover(MoveDirect.Previous, -1);
        public static Mover SendBackward { get; } = new Mover(MoveDirect.Next, -1);

        public static Mover MoveBefore(int index) => new Mover(MoveDirect.Before, index);
        public static Mover MoveAfter(int index) => new Mover(MoveDirect.After, index);

        private Mover(MoveDirect direct, int index)
        {
            this.Direct = direct;
            this.Index = index;
        }

        public MoveTo To(IReadOnlyList<ILayerBase> items, IndexSelection selection)
        {
            switch (selection.Type3)
            {
                case SelTyp3.Non:
                    return MoveTo.Non;

                case SelTyp3.S:
                    switch (this.Direct)
                    {
                        case MoveDirect.Next: return MoveTo.SN;
                        case MoveDirect.Previous: return MoveTo.SP;

                        case MoveDirect.Before:
                            if (this.Index == selection.Source.StartIndex)
                                return MoveTo.Non;
                            else if (this.Index - 1 != selection.Source.StartIndex)
                                return MoveTo.SB;
                            //@Debug: Crash if index == last
                            //else if (items[this.Index + 1].Depth < items[this.Index].Depth)
                            //    return MoveTo.Single_MoveBefore;
                            else
                                return MoveTo.Non;

                        case MoveDirect.After:
                            if (this.Index == selection.Source.StartIndex)
                                return MoveTo.Non;
                            else if (this.Index + 1 != selection.Source.StartIndex)
                                return MoveTo.SA;
                            else if (items[this.Index + 1].Depth < items[this.Index].Depth)
                                return MoveTo.SA;
                            else
                                return MoveTo.Non;

                        case MoveDirect.First: return MoveTo.SF;
                        case MoveDirect.Last: return MoveTo.SL;

                        default: return MoveTo.Non;
                    }

                case SelTyp3.SR:
                    switch (this.Direct)
                    {
                        case MoveDirect.Next: return MoveTo.SRN;
                        case MoveDirect.Previous: return MoveTo.SRP;

                        case MoveDirect.Before:
                            if (this.Index < selection.Source.StartIndex)
                                return MoveTo.SRB;
                            else if (this.Index > selection.Source.EndIndex)
                                return MoveTo.SRB;
                            else
                                return MoveTo.Non;

                        case MoveDirect.After:
                            if (this.Index < selection.Source.StartIndex)
                                return MoveTo.SRA;
                            else if (this.Index > selection.Source.EndIndex)
                                return MoveTo.SRA;
                            else
                                return MoveTo.Non;

                        case MoveDirect.First: return MoveTo.SRF;
                        case MoveDirect.Last: return MoveTo.SRL;

                        default: return MoveTo.Non;
                    }

                case SelTyp3.M:
                    switch (this.Direct)
                    {
                        case MoveDirect.Next: return MoveTo.MN;
                        case MoveDirect.Previous: return MoveTo.MP;

                        case MoveDirect.Before:
                            switch (items[this.Index].SelectMode)
                            {
                                case SelectMode.Deselected:
                                    return MoveTo.MA;
                                default:
                                    return MoveTo.Non;
                            }

                        case MoveDirect.First: return MoveTo.MF;
                        case MoveDirect.Last: return MoveTo.ML;

                        default: return MoveTo.Non;
                    }

                case SelTyp3.AS:
                case SelTyp3.ASR:
                case SelTyp3.AM:
                    return MoveTo.Non;

                case SelTyp3.FS:
                    switch (this.Direct)
                    {
                        case MoveDirect.Next: return MoveTo.MN;
                        case MoveDirect.Before: return MoveTo.SB;
                        case MoveDirect.After: return MoveTo.SA;
                        case MoveDirect.Last: return MoveTo.SL;

                        default: return MoveTo.Non;
                    }

                case SelTyp3.FSR:
                    switch (this.Direct)
                    {
                        case MoveDirect.Before: return MoveTo.SRB;
                        case MoveDirect.After: return MoveTo.SRA;
                        case MoveDirect.Last: return MoveTo.SRL;

                        default: return MoveTo.Non;
                    }

                case SelTyp3.BS:
                    switch (this.Direct)
                    {
                        case MoveDirect.Previous: return MoveTo.MP;
                        case MoveDirect.Before: return MoveTo.SB;
                        case MoveDirect.After: return MoveTo.SA;
                        case MoveDirect.First: return MoveTo.SF;

                        default: return MoveTo.Non;
                    }

                case SelTyp3.BSR:
                    switch (this.Direct)
                    {
                        case MoveDirect.Before: return MoveTo.SRB;
                        case MoveDirect.After: return MoveTo.SRA;
                        case MoveDirect.First: return MoveTo.SRF;

                        default: return MoveTo.Non;
                    }

                default:
                    return MoveTo.Non;
            }
        }
    }
}