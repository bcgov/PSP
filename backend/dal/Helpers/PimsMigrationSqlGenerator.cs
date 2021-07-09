using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Pims.Core.Extensions;
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
        private readonly Dictionary<string, Type> _tableTypes;
        private Type _currentEntityType;
        private string _currentColumnName;
        private string _currentTableName;
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
            _tableTypes = new Dictionary<string, Type>(types.Select(t =>
                {
                    var attr = t.GetCustomAttribute<MotiTableAttribute>();
                    return new KeyValuePair<string, Type>(attr?.Name ?? t.Name, t);
                }));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the table name abbreviation for the specified table.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetAbbreviation(string table)
        {
            return table switch
            {
                "__EFMigrationsHistory" => "MIGHST",
                "PIMS_MIGRATION_HISTORY" => "MIGHST",
                _ => _tables.GetValueOrDefault(table) ?? throw new InvalidOperationException($"Abbreviation is required for table {table}")
            };
        }

        /// <summary>
        /// Get the entity type for the specified table name.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        Type GetEntityType(string table)
        {
            return table switch
            {
                "__EFMigrationsHistory" => null,
                "PIMS_MIGRATION_HISTORY" => null,
                _ => _tableTypes.GetValueOrDefault(table) ?? throw new InvalidOperationException($"Entity model is required for table {table}")
            };
        }

        /// <summary>
        /// Extract the table and column name for subsequent steps.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        /// <param name="builder"></param>
        protected override void ColumnDefinition(
            string schema,
            string table,
            string name,
            ColumnOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            _currentEntityType = GetEntityType(table);
            _currentTableName = table;
            _currentColumnName = name;
            base.ColumnDefinition(schema, table, name, operation, model, builder);
        }

        /// <summary>
        /// Applies the MOTI naming standards for foreign key constraints.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        /// <param name="builder"></param>
        protected override void ForeignKeyConstraint(AddForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            var entityProps = _currentEntityType?.GetProperties();
            if (operation.Columns.Length > 1)
            {
                // If the foreign key is composed of multiple keys then join the names together.
                var columnNames = String.Join("_", operation.Columns
                                    .Select(column => entityProps
                                        .FirstOrDefault(p => column.IsIn(p.GetCustomAttribute<ColumnAttribute>()?.Name, p.Name))?.GetCustomAttribute<ColumnAttribute>()?.Name ?? column)
                                    .NotNull());
                operation.Name = $"PIMS_{GetAbbreviation(operation.PrincipalTable)}_PIMS_{GetAbbreviation(operation.Table)}_{columnNames}_FK";
            }
            else
            {
                // Single foreign key column, look for the ForeignKeyAttribute.
                var column = operation.Columns.First();
                var foreignKeyName = entityProps.FirstOrDefault(p => column.IsIn(p.GetCustomAttribute<ColumnAttribute>()?.Name, p.Name))?.GetCustomAttribute<ForeignKeyAttribute>()?.Name;
                operation.Name = String.IsNullOrWhiteSpace(foreignKeyName) ? $"PIMS_{GetAbbreviation(operation.PrincipalTable)}_PIMS_{GetAbbreviation(operation.Table)}_FK" : foreignKeyName;
            }
            base.ForeignKeyConstraint(operation, model, builder);
        }

        /// <summary>
        /// Default value constraint handler.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="defaultValueSql"></param>
        /// <param name="columnType"></param>
        /// <param name="builder"></param>
        protected override void DefaultValue(
            object defaultValue,
            string defaultValueSql,
            string columnType,
            MigrationCommandListBuilder builder)
        {
            CreateDefaultValueConstraint(defaultValue, defaultValueSql, builder);
            base.DefaultValue(defaultValue, defaultValueSql, columnType, builder);
        }

        /// <summary>
        /// Applies the MOTI naming standard for default constraints.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="defaultValueSql"></param>
        /// <param name="builder"></param>
        void CreateDefaultValueConstraint(object defaultValue, string defaultValueSql, MigrationCommandListBuilder builder)
        {
            if (!string.IsNullOrEmpty(_currentTableName)
                && !string.IsNullOrEmpty(_currentColumnName)
                && !(defaultValueSql == null && defaultValue == null))
            {
                builder.Append($" CONSTRAINT {GetAbbreviation(_currentTableName)}_{_currentColumnName}_DEF");
            }
        }
        #endregion
    }
}
