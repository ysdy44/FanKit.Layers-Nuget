using FanKit.Layers.Changes;
using FanKit.Layers.History;
using System.Collections.Generic;

namespace FanKit.Layers.Collections
{
    public class UndoList<U> : NavigateList<U>
        where U : class, IUndoable
    {
        private List<U> History => this;

        // History
        public int UndoLimit = 20;

        public void UISyncTime()
        {
            for (int i = 0; i < this.History.Count; i++)
            {
                U item = this.History[i];

                item.Period = i == this.CurrentIndex ? TimePeriod.Current :
                    i > this.CurrentIndex ? TimePeriod.Future : TimePeriod.Past;
            }
        }
        public void UISyncTime(int index)
        {
            for (int i = 0; i < this.History.Count; i++)
            {
                U item = this.History[i];

                item.Period = i == index ? TimePeriod.Current :
                    i > index ? TimePeriod.Future : TimePeriod.Past;
            }
        }

        #region Push

        public int Push(U item)
        {
            int removes = 0;
            if (this.History.Count > 0)
            {
                int startIndex = this.CurrentIndex + 1;
                int endIndex = this.History.Count - 1;
                removes = 1 + endIndex - startIndex;

                for (int i = endIndex; i >= startIndex; i--)
                {
                    using (IChange remove = this.History[i].Change)
                    {
                        this.History.RemoveAt(i);
                    }
                }
            }
            if (this.History.Count >= this.UndoLimit)
            {
                this.History.RemoveAt(0);
                this.History.Add(item);
            }
            else
            {
                this.CurrentIndex++;
                this.History.Add(item);
            }

            return removes;
        }

        #endregion

        #region Navigate

        public NavigateAction CanNavigate(int index)
        {
            switch (Nav(index))
            {
                case NavMod.Non: return NavigateAction.None;
                case NavMod.SU: return NavigateAction.Undo;
                case NavMod.SR: return NavigateAction.Redo;
                case NavMod.MU: return NavigateAction.UndoRange;
                case NavMod.MR: return NavigateAction.RedoRange;
                case NavMod.Unk: return NavigateAction.None;
                default: return NavigateAction.None;
            }
        }

        private NavMod Nav(int index)
        {
            if (index < 0)
            {
                switch (this.CurrentIndex)
                {
                    case -1:
                        return NavMod.Non;
                    case 0:
                        if (this.History.Count <= 0)
                            return NavMod.Unk;
                        else
                            return NavMod.SU;
                    default:
                        if (this.History.Count <= 0)
                            return NavMod.Unk;
                        else
                            return NavMod.MU;
                }
            }
            else if (index >= this.History.Count)
            {
                switch (this.History.Count - this.CurrentIndex)
                {
                    case 1:
                        return NavMod.Non;
                    case 2:
                        return NavMod.SR;
                    default:
                        return NavMod.MR;
                }
            }
            else if (index == this.CurrentIndex)
            {
                return NavMod.Non;
            }
            else if (index < this.CurrentIndex)
            {
                switch (this.CurrentIndex - index)
                {
                    case 0:
                        return NavMod.Non;
                    case 1:
                        return NavMod.SU;
                    default:
                        return NavMod.MU;
                }
            }
            else
            {
                switch (index - this.CurrentIndex)
                {
                    case 0:
                        return NavMod.Non;
                    case 1:
                        return NavMod.SR;
                    default:
                        return NavMod.MR;
                }
            }
        }

        #endregion

        public void UISyncTo(IList<U> items)
        {
            int length = this.Count;
            int oldCount = items.Count;

            if (length == 0)
            {
                switch (oldCount)
                {
                    case 0:
                        break;
                    default:
                        items.Clear();
                        break;
                }
            }
            else if (length < oldCount)
            {
                for (int i = 0; i < length; i++)
                {
                    U item = this[i];

                    // 1. Equals Id
                    if (item.Id == items[i].Id)
                        continue;

                    // 2. Replace
                    items[i] = item;
                }

                // 3. Remove Last
                for (int i = oldCount - 1; i >= length; i--)
                {
                    items.RemoveAt(i);
                }
            }
            else if (length > oldCount)
            {
                for (int i = 0; i < oldCount; i++)
                {
                    U item = this[i];

                    // 1. Equals Id
                    if (item.Id == items[i].Id)
                        continue;

                    // 2. Replace
                    items[i] = item;
                }

                // 3. Add Last
                for (int i = oldCount; i < length; i++)
                {
                    U item = this[i];
                    items.Add(item);
                }
            }
            else
            {
                int index = 0;

                do
                {
                    U item = this[index];

                    if (items.Count <= index)
                    {
                        items.Add(item);
                        index++;
                    }
                    else if (items[index].Id == item.Id)
                    {
                        index++;
                    }
                    else
                    {
                        items.RemoveAt(index);
                    }
                }
                while (index < length);
            }
        }
    }
}