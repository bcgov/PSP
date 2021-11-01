using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Pims.Dal.Entities;

namespace Pims.Dal
{
    /// <summary>
    /// PimsMigrationSqlGenerator class, provides a way to customize the generation of SQL.
    /// Provides a way to customize the default value constraints.
    /// </summary>
    public class PimsMigrationSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        #region Variables
        private readonly Dictionary<string, string> _tables;
        #endregion

        #region Variables
        /// <summary>
        /// Creates a new instance of a PimsMigrationSqlGenerator class, initializes with specified arguments.
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="migrationsAnnotations"></param>
        /// <returns></returns>
        public PimsMigrationSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations) : base(dependencies, migrationsAnnotations)
        {
            var baseType = typeof(BaseEntity);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => baseType.IsAssignableFrom(t));
            _tables = new Dictionary<string, string>(types.Select(t =>
                {
                    var attr = t.GetCustomAttribute<MotiTableAttribute>();
                    return new KeyValuePair<string, string>(attr?.Name ?? t.Name, attr?.Abbreviation ?? t.Name);
                }));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the table name abbreviation for the specified table.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>null if there is no match or the entity has been deleted in a future migration</returns>
        string GetAbbreviation(string table)
        {
            return table switch
            {
                "__EFMigrationsHistory" => "MIGHST",
                "PIMS_MIGRATION_HISTORY" => "MIGHST",
                _ => _tables.GetValueOrDefault(table)
            };
        }

        protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
        {
            Dictionary<string, string> defaultValues = new Dictionary<string, string>();
            foreach (var column in operation.Columns)
            {
                // If you use the DefaultValueSql
                if (!string.IsNullOrWhiteSpace(column.DefaultValueSql))
                {
                    // We pass now the information that we need
                    defaultValues.Add(column.Name, column.DefaultValueSql);
                    // Now we pass it to NULL to discard the normal behavior
                    column.DefaultValueSql = null;
                }
            }
            base.Generate(operation, model, builder, terminate);
            // Now we go through all values inside the defaultValues so we can do what we need
            foreach (var defaultValue in defaultValues)
            {
                var abbreviation = GetAbbreviation(operation.Name);
                if (abbreviation != null)
                {
                    builder
                      .AppendLine($"ALTER TABLE {operation.Name} ")
                      .AppendLine($"ADD CONSTRAINT {GetAbbreviation(operation.Name)}_{defaultValue.Key}_DEF ")
                      .AppendLine($"DEFAULT {defaultValue.Value} FOR {defaultValue.Key};")
                      .EndCommand();
                }
            }
        }
        #endregion
    }
}
