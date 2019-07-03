using System;

namespace Common.Messaging
{
    public interface IProvideEnvironmentOperations
    {
        void FailFast(string message, Exception exception);
    }
}