using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with acquisition payees within the datasource.
    /// </summary>
    public class AcquisitionPayeeRepository : BaseRepository<PimsAcquisitionPayee>, IAcquisitionPayeeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionPayeeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AcquisitionPayeeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        public PimsAcquisitionPayee GetById(long payeeId)
        {
            var entity = Context.PimsAcquisitionPayees
                .Include(ap => ap.AcquisitionFilePerson)
                    .ThenInclude(afp => afp.Person)
                .Include(ap => ap.AcquisitionOwner)
                .Include(ap => ap.InterestHolder)
                    .ThenInclude(ih => ih.Person)
                .Include(ap => ap.InterestHolder)
                    .ThenInclude(ih => ih.Organization)
                .Include(ap => ap.OwnerRepresentative)
                    .ThenInclude(or => or.Person)
                .Include(ap => ap.OwnerSolicitor)
                    .ThenInclude(os => os.Organization)
                .Include(ap => ap.OwnerSolicitor)
                    .ThenInclude(os => os.Person)
                .Include(ap => ap.PimsAcqPayeeCheques)
                .AsNoTracking()
                .FirstOrDefault(x => x.AcquisitionPayeeId.Equals(payeeId)) ?? throw new KeyNotFoundException();

            return entity;
        }
    }
}
