using System.Collections.Generic;

namespace FanKit.Layers.Collections
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList']/*" />
    public class NavigateList<T> : List<T>
        where T : class
    {
        private int PreviousIndex;

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.CurrentIndex']/*" />
        public int CurrentIndex = -1;

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.CurrentItem']/*" />
        public T CurrentItem =>
            this.CurrentIndex < 0 ? null :
            this.CurrentIndex >= this.Count ? null :
            this[this.CurrentIndex];

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.BackStack']/*" />
        public IEnumerable<T> BackStack() => this.BakAll(this.CurrentIndex);

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.ForwardStack']/*" />
        public IEnumerable<T> ForwardStack() => this.ForAll(this.CurrentIndex);

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.ClearCurrent']/*" />
        public void ClearCurrent()
        {
            this.CurrentIndex = -1;
            this.Clear();
        }

        #region Go

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.CanGoBack']/*" />
        public bool CanGoBack()
        {
            if (this.CurrentIndex < 0)
            {
                return false;
            }
            else if (this.CurrentIndex >= this.Count)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.CanGoForward']/*" />
        public bool CanGoForward()
        {
            if (this.CurrentIndex + 1 < 0)
            {
                return false;
            }
            else if (this.CurrentIndex + 1 >= this.Count)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Navigate

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.NavigateTo']/*" />
        public IndexRange NavigateTo(int index)
        {
            this.PreviousIndex = this.CurrentIndex;
            if (index < 0)
            {
                switch (this.PreviousIndex)
                {
                    case -1:
                        return new IndexRange(-1);
                    case 0:
                        this.CurrentIndex = -1;
                        if (this.Count <= 0)
                            return new IndexRange(-1);
                        else
                            return IndexRange.NegativeUnit;
                    default:
                        this.CurrentIndex = -1;
                        if (this.Count <= 0)
                            return new IndexRange(-1);
                        else
                            return new IndexRange(this.PreviousIndex, -1);
                }
            }
            else if (index >= this.Count)
            {
                switch (this.Count - this.PreviousIndex)
                {
                    case 1:
                        return new IndexRange(this.PreviousIndex);
                    case 2:
                        this.CurrentIndex = this.Count - 1;
                        return new IndexRange(this.PreviousIndex, this.Count - 1);
                    default:
                        this.CurrentIndex = this.Count - 1;
                        return new IndexRange(this.PreviousIndex, this.Count - 1);
                }
            }
            else if (index == this.PreviousIndex)
            {
                return new IndexRange(this.PreviousIndex);
            }
            else if (index < this.PreviousIndex)
            {
                switch (this.PreviousIndex - index)
                {
                    case 0:
                        return new IndexRange(this.PreviousIndex);
                    case 1:
                        this.CurrentIndex--;
                        return new IndexRange(this.PreviousIndex, this.PreviousIndex - 1);
                    default:
                        this.CurrentIndex = index;
                        return new IndexRange(this.PreviousIndex, index);
                }
            }
            else
            {
                switch (index - this.PreviousIndex)
                {
                    case 0:
                        return new IndexRange(this.PreviousIndex);
                    case 1:
                        this.CurrentIndex++;
                        return new IndexRange(this.PreviousIndex, this.PreviousIndex + 1);
                    default:
                        this.CurrentIndex = index;
                        return new IndexRange(this.PreviousIndex, index);
                }
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.GetRange']/*" />
        public IEnumerable<T> GetRange(IndexRange range)
        {
            switch (range.StartIndex - range.EndIndex)
            {
                case -1:
                    T redo = this[range.EndIndex];

                    if (redo == null)
                        return null;
                    else
                        return System.Linq.Enumerable.Repeat(redo, 1);
                case 0:
                    return null;
                case 1:
                    T undo = this[range.StartIndex];

                    if (undo == null)
                        return null;
                    else
                        return System.Linq.Enumerable.Repeat(undo, 1);
                default:
                    return range.EndIndex > range.StartIndex ? this.For(range) : this.Bak(range);
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.GoBack']/*" />
        public T GoBack()
        {
            if (this.CurrentIndex < -1)
            {
                this.CurrentIndex = -1;
                return null;
            }
            else if (this.CurrentIndex >= this.Count)
            {
                this.CurrentIndex = this.Count - 1;
                return null;
            }
            else
            {
                T undo = this[this.CurrentIndex];

                this.CurrentIndex--;

                if (undo == null)
                    return null;
                else
                    return undo;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='NavigateList.GoForward']/*" />
        public T GoForward()
        {
            if (this.CurrentIndex < -1)
            {
                this.CurrentIndex = -1;
                return null;
            }
            else if (this.CurrentIndex + 1 == this.Count)
            {
                return null;
            }
            else if (this.CurrentIndex >= this.Count)
            {
                this.CurrentIndex = this.Count - 1;
                return null;
            }
            else
            {
                T undo = this[this.CurrentIndex + 1];

                this.CurrentIndex++;

                if (undo == null)
                    return null;
                else
                    return undo;
            }
        }

        #endregion

        #region Back & Forward

        // Back All
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private IEnumerable<T> BakAll(int oldIndex)
        {
            for (int i = oldIndex; i >= 0; i--)
                yield return this[i];
        }

        // Forward All
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private IEnumerable<T> ForAll(int oldIndex)
        {
            for (int i = oldIndex + 1; i < this.Count; i++)
                yield return this[i];
        }

        // Back
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private IEnumerable<T> Bak(IndexRange n)
        {
            for (int i = n.StartIndex; i > n.EndIndex; i--)
                yield return this[i];
        }

        // Forward
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private IEnumerable<T> For(IndexRange n)
        {
            for (int i = n.StartIndex + 1; i <= n.EndIndex; i++)
                yield return this[i];
        }

        #endregion
    }
}