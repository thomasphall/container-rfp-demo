using System;

using DbUp;
using DbUp.Engine;

namespace Migrations
{
    /// <summary>
    /// Executes all the embedded scripts in the Migrations assembly.
    /// </summary>
    public class SqlServerEmbeddedScriptMigrator
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerEmbeddedScriptMigrator"/> class.
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlServerEmbeddedScriptMigrator(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Upgrades the database specified in the instance's connection string.
        /// </summary>
        /// <returns>The <see cref="DatabaseUpgradeResult"/>.</returns>
        public DatabaseUpgradeResult PerformUpgrade()
        {
            var upgradeEngine = DeployChanges
                .To
                .SqlDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(GetType().Assembly)
                .LogToConsole()
                .Build();

            return upgradeEngine.PerformUpgrade();
        }
    }
}
