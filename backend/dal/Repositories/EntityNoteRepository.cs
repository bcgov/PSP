using System.Security.Claims;
using MapsterMapper;
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
        public T Add<T>(T model) where T : class
        {
            model.ThrowIfNull(nameof(model));

            // TODO: implement
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
