// ---------------------------------------------------------------------------------------------------------------
// <copyright file="EventMessageRecorder.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

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

        public EventMessageRecorder(IProvideConfiguration configurationProvider, string clientName)
            : base(configurationProvider)
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
