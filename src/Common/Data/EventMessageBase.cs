using System.Data.SqlClient;
using Common.Configuration;

namespace Common.Data
{
    public class EventMessageBase
    {
        private readonly IProvideConfiguration _configurationProvider;

        public EventMessageBase(IProvideConfiguration configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_configurationProvider.Configuration["sqlserverConnectionString"]);
        }
    }
}
