using System;
using System.Threading.Tasks;

namespace Subscriber
{
    public interface IDeleteUnconsumedMessages
    {
        Task Delete(Guid messageId);
    }
}