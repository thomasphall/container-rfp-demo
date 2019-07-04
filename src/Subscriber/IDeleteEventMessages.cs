using System;
using System.Threading.Tasks;

namespace Subscriber
{
    public interface IDeleteEventMessages
    {
        Task Delete(Guid messageId);
    }
}