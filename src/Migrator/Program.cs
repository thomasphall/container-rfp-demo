// ---------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

using Common.ConsoleSupport;

using DbUp;

using Migrations;

namespace Migrator
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            // Completely wrong, intolerant way to way for SQL Server to start.
            Thread.Sleep(10000);

            var connectionStringArg = args.SingleOrDefault();

            var connectionStringBuilder = connectionStringArg != null
                ? new SqlConnectionStringBuilder(connectionStringArg)
                : new SqlConnectionStringBuilder
                {
                    ApplicationName = "RabbitMQ Stress Test Migrations",
                    DataSource = "rabbitmq.sandoval.svc.cluster.local",
                    InitialCatalog = "RabbitStressTest",
                    IntegratedSecurity = false,
                    MultipleActiveResultSets = false,
                    Password = "yourStrong(!)Password",
                    UserID = "sa"
                };

            var connectionString = connectionStringBuilder.ConnectionString;

            DropDatabase.For.SqlDatabase(connectionString);
            EnsureDatabase.For.SqlDatabase(connectionString);

            var embeddedScriptMigrator = new SqlServerEmbeddedScriptMigrator(connectionString);
            var result = embeddedScriptMigrator.PerformUpgrade();

            if (!result.Successful)
            {
                ConsoleUtilities.WriteLineWithColor(ConsoleColor.Red, result.Error);

                return Return(-1);
            }

            ConsoleUtilities.WriteLineWithColor(ConsoleColor.Green, "Migrations executed successfully!");

            return Return(0);
        }

        private static int Return(int exitCode)
        {
            return exitCode;
        }
    }
}
