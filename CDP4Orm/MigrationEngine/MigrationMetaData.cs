﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationMetaData.cs" company="RHEA System S.A.">
//   Copyright (c) 2016-2018 RHEA System S.A.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CDP4Orm.MigrationEngine
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Assertion on the different kind of specialized migration script
    /// </summary>
    internal enum MigrationScriptKind
    {
        /// <summary>
        /// Assertion that the migration script is only applied on Non-Thing table
        /// </summary>
        NonThingTableMigrationTemplate,

        /// <summary>
        /// Assertion that the migration script is only applied on start-up
        /// </summary>
        OnStartUpOnly
    }

    /// <summary>
    /// Metadata class that contains information on the migration script
    /// </summary>
    public class MigrationMetaData
    {
        protected const char SCRIPT_VERSION_SEPARATOR = '_';
        protected const char CODE_VERSION_SEPARATOR = '.';
        protected static Regex MIGRATION_FILENAME_PATTERN = new Regex(@"^(?<applicableDomain>[a-zA-Z]+)(_(?<migrationClass>[a-zA-Z]+))?_(?<date>\d{8})_(?<version>\d+_\d+_\d+_\d+)_(?<name>.*\.sql)$");
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationMetaData"/> class
        /// </summary>
        /// <param name="migrationFileName">The migration script name</param>
        public MigrationMetaData(string migrationFileName)
        {
            if (string.IsNullOrWhiteSpace(migrationFileName))
            {
                throw new ArgumentNullException(nameof(migrationFileName), "migration file name cannot be null or be empty.");
            }

            this.ResourceName = migrationFileName;
            var resourceSplitName = migrationFileName.Replace(MigrationService.MIGRATION_SCRIPT_NAMESPACE, string.Empty);

            if (!MIGRATION_FILENAME_PATTERN.IsMatch(resourceSplitName))
            {
                throw new ArgumentException($"Migration file name shall match the format {MIGRATION_FILENAME_PATTERN}");
            }

            var groups = MIGRATION_FILENAME_PATTERN.Match(resourceSplitName).Groups;
            this.Version = new Version(groups["version"].Value.Replace(SCRIPT_VERSION_SEPARATOR, CODE_VERSION_SEPARATOR));
            this.MigrationScriptDate = DateTime.ParseExact(groups["date"].Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            this.Name = groups["name"].Value;

            if (!Enum.TryParse<MigrationScriptApplicationKind>(groups["applicableDomain"].Value, true, out var applicableDomain))
            {
                throw new InvalidOperationException($"applicable domain {groups["applicableDomain"].Value} could not be parsed.");
            }

            this.MigrationScriptApplicationKind = applicableDomain;

            if (Enum.TryParse<MigrationScriptKind>(groups["migrationClass"].Value, true, out var migrationScriptKind))
            {
                this.MigrationScriptKind = migrationScriptKind;
            }
        }

        /// <summary>
        /// Gets the version that this migration belongs to.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// Gets the applicability of the migration script
        /// </summary>
        public MigrationScriptApplicationKind MigrationScriptApplicationKind { get; }

        /// <summary>
        /// Gets the applicability of the migration script
        /// </summary>
        internal MigrationScriptKind? MigrationScriptKind { get; }

        /// <summary>
        /// Gets the date of the migration script
        /// </summary>
        public DateTime MigrationScriptDate { get; }

        /// <summary>
        /// Gets the name of the migration
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the resource full-name
        /// </summary>
        public string ResourceName { get; }
    }
}
