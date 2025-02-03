using FanKit.Layers.Changes;
using System.Collections.Generic;

namespace FanKit.Layers.Collections
{
    partial class LogicalTreeList<T>
    {
        #region Remove

        public void Remove()
        {
            for (int i = this.LogicalTree.Count - 1; i >= 0; i--)
            {
                T item = this.LogicalTree[i];

                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        break;
                    case SelectMode.Selected:
                    case SelectMode.Parent:
                        this.LogicalTree.RemoveAt(i);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Group

        /*
         * Move selected items as
         * □□□□□ - □□□□□ 0
         * □□□□□ - ■■■■■ 1
         * □□□□□ - □■■■■ 2
         * □□□□□ - ■■■■■ 3
         * □□□□□ - ■■■■■ 4
         * ■■■■■ - □□□□□ 5
         * □■■■■ - □□□□□ 6
         * □□□□□ - □□□□□ 7
         * ■■■■■ - □□□□□ 8
         * □□□□□ - □□□□□ 9
         * ■■■■■ - □□□□□ 10
         * □□□□□ - □□□□□ 11
         * @para frontIndex The front index
         */
        public void MoveToFirst(int frontIndex)
        {
            for (int i = frontIndex; i < this.LogicalTree.Count; i++)
            {
                T item = this.LogicalTree[i];

                switch (item.SelectMode)
                {
                    case SelectMode.Deselected:
                        break;
                    case SelectMode.Selected:
                    case SelectMode.Parent:
                        if (frontIndex == i)
                        {
                            frontIndex++;
                        }
                        else
                        {
                            this.LogicalTree.RemoveAt(i);
                            this.LogicalTree.Insert(frontIndex, item);
                            frontIndex++;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Ungroup

        public void Ungroup()
        {
            for (int i = this.LogicalTree.Count - 1; i >= 0; i--)
            {
                T item = this.LogicalTree[i];

                if (item.IsGroup)
                {
                    switch (item.SelectMode)
                    {
                        case SelectMode.Deselected:
                            break;
                        case SelectMode.Selected:
                            this.LogicalTree.RemoveAt(i);
                            break;
                        case SelectMode.Parent:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public bool CanRelease()
        {
            foreach (T item in this.LogicalTree)
            {
                switch (item.SelectMode)
                {
                    case SelectMode.Selected:
                        switch (item.Depth)
                        {
                            case 0:
                                break;
                            default:
                                return true;
                        }
                        break;
                    case SelectMode.Parent:
                        switch (item.Depth)
                        {
                            case 0:
                            case 1:
                                break;
                            default:
                                return true;
                        }
                        break;
                    default:
                        break;
                }
            }

            return false;
        }

        #endregion
    }
}