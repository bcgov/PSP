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
