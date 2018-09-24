﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationService.cs" company="RHEA System S.A.">
//   Copyright (c) 2016-2018 RHEA System S.A.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CDP4Orm.MigrationEngine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Npgsql;

    /// <summary>
    /// The class responsble for applying all migration scripts
    /// </summary>
    public class MigrationService : IMigrationService
    {
        /// <summary>
        /// The namespace of the migration scripts
        /// </summary>
        public const string MIGRATION_SCRIPT_NAMESPACE = "CDP4Orm.MigrationScript.";

        /// <summary>
        /// Aplply the full set of migrations on a partition
        /// </summary>
        /// <param name="transaction">The current transaction</param>
        /// <param name="partition">The target partition</param>
        public void ApplyMigrations(NpgsqlTransaction transaction, string partition)
        {
            var migrations = GetMigrations().Where(x => partition.StartsWith(x.MigrationMetaData.MigrationScriptApplicationKind.ToString()) || x.MigrationMetaData.MigrationScriptApplicationKind == MigrationScriptApplicationKind.All);
            foreach (var migrationBase in migrations.OrderBy(x => x.MigrationMetaData.Version))
            {
                migrationBase.ApplyMigration(transaction, new [] { partition });
            }
        }

        /// <summary>
        /// Gets all migration scripts
        /// </summary>
        /// <returns>The List of <see cref="MigrationBase"/></returns>
        public static IReadOnlyList<MigrationBase> GetMigrations()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceFullNames = assembly.GetManifestResourceNames().Where(x => x.StartsWith(MIGRATION_SCRIPT_NAMESPACE) && x.EndsWith(".sql"));

            var migrationScriptMetadatas = resourceFullNames.Select(x => new MigrationMetaData(x)).ToList();
            var migrationList = new List<MigrationBase>();
            foreach (var migrationMetaData in migrationScriptMetadatas)
            {
                MigrationBase migration;
                switch (migrationMetaData.MigrationScriptKind)
                {
                    case CDP4Orm.MigrationEngine.MigrationScriptKind.NonThingTableMigrationTemplate:
                        migration = new NonThingTableMigration(migrationMetaData);
                        break;
                    default:
                        migration = new GenericMigration(migrationMetaData);
                        break;
                }

                migrationList.Add(migration);
            }

            return migrationList;
        }
    }
}
