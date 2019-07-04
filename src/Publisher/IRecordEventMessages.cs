using System;
using System.Threading.Tasks;

namespace Publisher
{
    internal interface IRecordEventMessages
    {
        Task Record(Guid eventMessageMessageId);
    }
}