using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace Subscriber
{
    class EventMessageDeleter : IDeleteEventMessages
    {
        private readonly string _clientName;
        private readonly IProvideConfiguration _configurationProvider;

        public EventMessageDeleter(IProvideConfiguration configurationProvider, string clientName)
        {
            _configurationProvider = configurationProvider;
            _clientName = clientName;
        }

        public async Task Delete(Guid messageId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.spUnconsumedMessageDelete";
                var clientNameParameter = new SqlParameter("clientName", SqlDbType.VarChar, 100)
                {
                    Value = _clientName
                };

                var messageIdParameter = new SqlParameter("messageId", SqlDbType.UniqueIdentifier)
                {
                    Value = messageId
                };

                command.Parameters.Add(clientNameParameter);
                command.Parameters.Add(messageIdParameter);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configurationProvider.Configuration["sqlserverConnectionString"]);
        }
    }
}