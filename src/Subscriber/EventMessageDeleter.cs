using System;
using System.Data;
using System.Data.SqlClient;
using Common.Configuration;
using Common.Data;

namespace Subscriber
{
    class EventMessageDeleter : EventMessageBase, IDeleteEventMessages
    {
        public EventMessageDeleter(IProvideConfiguration configurationProvider) : base(configurationProvider)
        {
        }

        public void Delete(Guid messageId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.spUnconsumedMessageDelete";
                var messageIdParameter = new SqlParameter("messageId", SqlDbType.UniqueIdentifier)
                {
                    Value = messageId
                };

                command.Parameters.Add(messageIdParameter);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}