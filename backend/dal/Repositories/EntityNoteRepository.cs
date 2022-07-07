using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with notes within the datasource.
    /// </summary>
    public class EntityNoteRepository : BaseRepository, IEntityNoteRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a NoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public EntityNoteRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<EntityNoteRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }

        #endregion

        #region Methods

        /// <summary>
        /// Associates a note to an existing parent entity via intermediate table (e.g. PimsActivityInstanceNote)
        /// </summary>
        /// <typeparam name="T">The entity type - e.g. PimsActivityInstanceNote.</typeparam>
        /// <param name="entity">The entity to add to the datastore.</param>
        /// <returns>The created association.</returns>
        public T Add<T>(T entity) where T : class
        {
            entity.ThrowIfNull(nameof(entity));
            this.Context.Add<T>(entity);
            return entity;
        }

        /// <summary>
        /// Retrieves all notes associated with a parent entity
        /// </summary>
        /// <typeparam name="T">The entity type - e.g. PimsActivityInstanceNote.</typeparam>
        /// <param name="predicate">The predicate to filter all notes - e.g. where parentId == parent.Id</param>
        /// <returns>The entity-notes associations</returns>
        public IEnumerable<T> GetAll<T>(Func<T, bool> predicate) where T : class
        {
            predicate.ThrowIfNull(nameof(predicate));
            return this.Context.Set<T>().AsNoTracking().Where(predicate).ToArray();
        }

        #endregion
    }
}
