using System;

namespace Subscriber
{
    public interface IDeleteEventMessages
    {
        void Delete(Guid messageId);
    }
}