using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with compensation requisition categories within the datasource.
    /// </summary>
    public class H120CategoryRepository : BaseRepository<PimsH120Category>, IH120CategoryRepository
    {
        #region Constructors

        private readonly ClaimsPrincipal _user;

        /// <summary>
        /// Creates a new instance of a CompReqH120Repository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public H120CategoryRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
            this._user = user;
        }
        #endregion

        public IEnumerable<PimsH120Category> GetAll()
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return Context.PimsH120Categories.AsNoTracking().ToArray();
        }
    }
}
