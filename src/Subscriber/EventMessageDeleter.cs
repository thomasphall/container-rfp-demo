// ---------------------------------------------------------------------------------------------------------------
// <copyright file="EventMessageDeleter.cs" company="Enterprise Products Partners L.P. (Enterprise)">
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

namespace Subscriber
{
    internal class EventMessageDeleter : EventMessageBase, IDeleteEventMessages
    {
        public EventMessageDeleter(IProvideConfiguration configurationProvider)
            : base(configurationProvider)
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
