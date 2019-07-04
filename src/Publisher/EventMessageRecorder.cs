using System;
using System.Data;
using System.Data.SqlClient;
using Common.Configuration;
using Common.Data;

namespace Publisher
{
    internal class EventMessageRecorder : EventMessageBase, IRecordEventMessages
    {
        private readonly string _clientName;

        public EventMessageRecorder(IProvideConfiguration configurationProvider, string clientName) : base(configurationProvider)
        {
            _clientName = clientName;
        }

        public void Record(Guid messageId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.spUnconsumedMessageInsert";
                var clientNameParameter = new SqlParameter("clientName", SqlDbType.VarChar, 100)
                {
                    Value = _clientName
                };
                var messageIdParameter = new SqlParameter("messageId", SqlDbType.UniqueIdentifier) { Value = messageId };

                command.Parameters.Add(clientNameParameter);
                command.Parameters.Add(messageIdParameter);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
