using System.Collections.Generic;

namespace FanKit.Layers.Collections
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList']/*" />
    public class RangeList<T> : List<T>
    {
        private List<T> LogicalTree => this;

        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.Item']/*" />
        public IEnumerable<T> this[IndexRange range]
        {
            get
            {
                for (int i = range.StartIndex; i <= range.EndIndex; i++)
                {
                    yield return this.LogicalTree[i];
                }
            }
        }

        #region Remove

        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.RemoveRange']/*" />
        public void RemoveRange(IndexRange range) => this.Rm(range);

        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.RemoveRanges']/*" />
        public void RemoveRanges(IndexRange[] ranges) => this.Rms(ranges);

        // Remove
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Rm(IndexRange range) => this.LogicalTree.RemoveRange(range.StartIndex, range.Length);

        // Remove Ranges
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Rms(IndexRange[] ranges)
        {
            for (int i = ranges.Length - 1; i >= 0; i--)
            {
                IndexRange range = ranges[i];
                this.Rm(range);
            }
        }

        // Get Remove Ranges
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int Gms(IndexRange[] ranges)
        {
            int length = 0;
            for (int i = ranges.Length - 1; i >= 0; i--)
            {
                IndexRange range = ranges[i];

                length += range.Length;
                this.Rm(range);
            }

            return length;
        }

        #endregion

        #region Move

        // MoveTo
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Mv(int length, int oldIndex, int newIndex)
        {
            for (int i = 0; i < length; i++)
            {
                T item = this.LogicalTree[oldIndex];

                this.LogicalTree.RemoveAt(oldIndex);
                this.LogicalTree.Insert(newIndex, item);
            }
        }

        // Backup
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Bak(IndexRange range)
        {
            for (int i = range.StartIndex; i <= range.EndIndex; i++)
            {
                T item = this.LogicalTree[i];

                this.LogicalTree.Add(item);
            }
        }

        #endregion

        #region Move.Range

        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.MoveRangeToLast']/*" />
        public void MoveRangeToLast(IndexRange range)
        {
            this.Bak(range);
            this.Rm(range);
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.MoveRangeToFirst']/*" />
        public void MoveRangeToFirst(IndexRange range)
        {
            int count = this.LogicalTree.Count - 1;

            this.Bak(range);
            this.Rm(range);

            this.Mv(range.Length, count, 0);
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.MoveRange']/*" />
        public void MoveRange(IndexRange range, int index)
        {
            int count = this.LogicalTree.Count - 1;

            this.Bak(range);
            this.Rm(range);

            this.Mv(range.Length, count, index);
        }

        #endregion

        #region Move.Ranges

        // Backup Ranges
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Bas(IndexRange[] ranges)
        {
            foreach (IndexRange range in ranges)
            {
                for (int i = range.StartIndex; i <= range.EndIndex; i++)
                {
                    T item = this.LogicalTree[i];

                    this.LogicalTree.Add(item);
                }
            }
        }

        // Move items as
        // □□□□□ - □□□□□ 0
        // □□□□□ - □□□□□ 1
        // □□□□□ - □□□□□ 2
        // □□□□□ - □□□□□ 3
        // □□□□□ - □□□□□ 4
        // ■■■■■ - □□□□□ 5
        // □■■■■ - □□□□□ 6
        // □□□□□ - □□□□□ 7
        // ■■■■■ - ■■■■■ 8
        // □□□□□ - □■■■■ 9
        // ■■■■■ - ■■■■■ 10
        // □□□□□ - ■■■■■ 11
        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.MoveRangesToLast']/*" />
        public void MoveRangesToLast(IndexRange[] ranges)
        {
            this.Bas(ranges);
            this.Rms(ranges);
        }

        // Move selected items as
        // □□□□□ - ■■■■■ 0
        // □□□□□ - □■■■■ 1
        // □□□□□ - ■■■■■ 2
        // □□□□□ - ■■■■■ 3
        // □□□□□ - □□□□□ 4
        // ■■■■■ - □□□□□ 5
        // □■■■■ - □□□□□ 6
        // □□□□□ - □□□□□ 7
        // ■■■■■ - □□□□□ 8
        // □□□□□ - □□□□□ 9
        // ■■■■■ - □□□□□ 10
        // □□□□□ - □□□□□ 11
        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.MoveRangesToFirst']/*" />
        public void MoveRangesToFirst(IndexRange[] ranges)
        {
            int count = this.LogicalTree.Count - 1;

            this.Bas(ranges);
            int length = this.Gms(ranges);

            this.Mv(length, count, 0);
        }

        // Move items as
        // □□□□□ - □□□□□ 0
        // □□□□□ - ■■■■■ 1
        // □□□□□ - □■■■■ 2
        // □□□□□ - ■■■■■ 3
        // □□□□□ - ■■■■■ 4
        // ■■■■■ - □□□□□ 5
        // □■■■■ - □□□□□ 6
        // □□□□□ - □□□□□ 7
        // ■■■■■ - □□□□□ 8
        // □□□□□ - □□□□□ 9
        // ■■■■■ - □□□□□ 10
        // □□□□□ - □□□□□ 11
        /// <include file="doc/docs.xml" path="docs/doc[@for='RangeList.MoveRanges']/*" />
        public void MoveRanges(IndexRange[] ranges, T target, int offset)
        {
            int count = this.LogicalTree.Count - 1;

            this.Bas(ranges);
            int length = this.Gms(ranges);

            int index = offset + this.LogicalTree.IndexOf(target);
            this.Mv(length, count, index);
        }

        #endregion
    }
}