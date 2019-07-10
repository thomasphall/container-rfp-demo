// ---------------------------------------------------------------------------------------------------------------
// <copyright file="SqlServerEmbeddedScriptMigrator.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

using DbUp;
using DbUp.Engine;

namespace Migrations
{
    /// <summary>
    ///     Executes all the embedded scripts in the Migrations assembly.
    /// </summary>
    public class SqlServerEmbeddedScriptMigrator
    {
        private readonly string _connectionString;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlServerEmbeddedScriptMigrator" /> class.
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlServerEmbeddedScriptMigrator(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        ///     Upgrades the database specified in the instance's connection string.
        /// </summary>
        /// <returns>The <see cref="DatabaseUpgradeResult" />.</returns>
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
