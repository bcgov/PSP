using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with notes within the datasource.
    /// </summary>
    public class NoteRepository : BaseRepository<PimsNote>, INoteRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a NoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public NoteRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<NoteRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        public PimsNote Add(PimsNote note)
        {
            // TODO: implement
            throw new System.NotImplementedException();
        }

        public long GetRowVersion(long id)
        {
            // TODO: implement
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
