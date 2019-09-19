using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public interface ISyncInvQueue
    {
        /// <summary>
        /// 推送ItemNos入队列
        /// </summary>
        void Push(IList<string> itemNos);

        /// <summary>
        /// 取出队列所有元素
        /// </summary>
        IList<string> PopAll();
    }
}
