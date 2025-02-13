using FanKit.Layers.Changes;
using System;
using System.Collections.Generic;

namespace FanKit.Layers.Sample
{
    public class ChangeGroup : Dictionary<HistoryType, IChange>, IChange
    {
        public IEnumerable<Guid> ReferenceGuids
        {
            get
            {
                foreach (KeyValuePair<HistoryType, IChange> child in this)
                {
                    foreach (Guid item in child.Value.ReferenceGuids)
                    {
                        yield return item;
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (KeyValuePair<HistoryType, IChange> child in this)
            {
                child.Value.Dispose();
            }
        }
    }
}