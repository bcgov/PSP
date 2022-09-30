using Microsoft.EntityFrameworkCore;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// DbContextExtensions static class, provides extension methods for DbContext objects.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// When manipulating entities it is necessary to reset the original value for 'ConcurrencyControlNumber' so that concurrency checking can occur.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="source">The original source entity from the database.</param>
        public static void SetOriginalConcurrencyControlNumber<T>(this DbContext context, T source)
            where T : IBaseEntity
        {
            context.Entry(source).OriginalValues[nameof(IBaseEntity.ConcurrencyControlNumber)] = source.ConcurrencyControlNumber;
        }

        /// <summary>
        /// Detach the entity from the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        public static void Detach<T>(this DbContext context, T entity)
            where T : IBaseEntity
        {
            context.Entry(entity).State = EntityState.Detached;
        }
    }
}
