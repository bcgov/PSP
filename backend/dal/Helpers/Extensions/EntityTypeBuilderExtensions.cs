using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Extensions
{
    /// <summary>
    /// EntityTypeBuilderExtensions static class, provides extension methods for EntityTypeBuilder`T.
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Adds the configured table name
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<T> ToMotiTable<T>(this EntityTypeBuilder<T> builder)
            where T : class
        {
            var type = typeof(T);
            var table = type.GetCustomAttribute<MotiTableAttribute>() ?? throw new InvalidOperationException($"Entity '{type.Name}' model requires MotiAttribute defined.");
            builder.ToTable(table.Name);
            return builder;
        }

        /// <summary>
        /// Add the primary key to the table and name it with the abbreviation configured.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="keyExpression"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static KeyBuilder HasMotiKey<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, object>> keyExpression)
            where T : class
        {
            var type = typeof(T);
            var table = type.GetCustomAttribute<MotiTableAttribute>() ?? throw new InvalidOperationException($"Entity '{type.Name}' model requires MotiAttribute defined.");
            var primaryKeyName = $"{table.Abbreviation}_PK";
            return builder.HasKey(keyExpression).HasName(primaryKeyName);
        }

        /// <summary>
        /// Add a sequence property to the table with the appropriate naming convention.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyExpression"></param>
        /// <param name="isRequired"></param>
        /// <param name="sequenceName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<object> HasMotiSequence<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, object>> propertyExpression, bool isRequired = true, string sequenceName = null)
            where T : class
        {
            // Generate a sequence name based on the column name.
            if (String.IsNullOrWhiteSpace(sequenceName))
            {
                var propInfo = GetPropertyInfo(propertyExpression);
                var type = typeof(T);
                var seqPropInfo = type.GetProperty(propInfo.Name);
                var column = seqPropInfo.GetCustomAttribute<ColumnAttribute>(true)?.Name ?? throw new InvalidOperationException($"Sequence property '{propInfo.DeclaringType}.{propInfo.Name}' require ColumnAttribute.");
                sequenceName = $"PIMS_{column}_SEQ";
            }

            return builder.Property(propertyExpression)
                .HasColumnType("BIGINT")
                .IsRequired(isRequired)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql($"NEXT VALUE FOR {sequenceName}");
        }

        /// <summary>
        /// Extract the property information from the Lambda expression.
        /// </summary>
        /// <param name="propertyLambda"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            var member = propertyLambda.Body switch
            {
                MemberExpression _ => propertyLambda.Body as MemberExpression,
                UnaryExpression _ => ((UnaryExpression)propertyLambda.Body).Operand as MemberExpression,
                _ => throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a method, not a property.",
                        propertyLambda.ToString()))
            };

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }
    }
}
