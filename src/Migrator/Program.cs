using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Common.ConsoleSupport;
using DbUp;
using Migrations;

namespace Migrator
{
    class Program
    {
        static int Main(string[] args)
        {
            // Completely wrong, intolerant way to way for SQL Server to start.
            Thread.Sleep(5000);

            var connectionStringArg = args.SingleOrDefault();

            var connectionStringBuilder = connectionStringArg != null
                ? new SqlConnectionStringBuilder(connectionStringArg)
                : new SqlConnectionStringBuilder
                {
                    ApplicationName = "RabbitMQ Stress Test Migrations",
                    DataSource = "sqlserver",
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
