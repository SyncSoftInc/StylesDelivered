using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class InMemorySyncInvQueue : ISyncInvQueue
    {
        ConcurrentDictionary<string, bool> _queues = new ConcurrentDictionary<string, bool>();

        public void Push(IList<string> itemNos)
        {
            foreach (var itemNo in itemNos)
            {
                if (!_queues.ContainsKey(itemNo))
                {
                    _queues.TryAdd(itemNo, true);
                }
            }
        }

        public IList<string> PopAll()
        {
            var list = _queues.Keys.ToList();
            _queues.Clear();
            return list;
        }
    }
}
