using System;

namespace Publisher
{
    internal interface IRecordEventMessages
    {
        void Record(Guid eventMessageMessageId);
    }
}