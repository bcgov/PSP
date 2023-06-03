using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with InterestHolders within the datasource.
    /// </summary>
    public class InterestHolderRepository : BaseRepository<PimsAgreement>, IInterestHolderRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a InterestHolderRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public InterestHolderRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AgreementRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public List<PimsInterestHolder> GetInterestHoldersByAcquisitionFile(long acquisitionFileId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsInterestHolders
                .Where(ih => ih.AcquisitionFileId == acquisitionFileId)
                .Include(ih => ih.Organization)
                .Include(ih => ih.Person)
                .Include(ih => ih.PimsInthldrPropInterests)
                .ThenInclude(ip => ip.InterestHolderInterestTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public List<PimsInterestHolder> UpdateAllForAcquisition(long acquisitionFileId, List<PimsInterestHolder> interestHolders)
        {
            List<PimsInterestHolder> currentInterestHolders = GetInterestHoldersByAcquisitionFile(acquisitionFileId);
            Context.UpdateChild<PimsAcquisitionFile, long, PimsInterestHolder, long>(p => p.PimsInterestHolders, acquisitionFileId, interestHolders.ToArray());
            interestHolders.ForEach(ih =>
            {
                if (ih.InterestHolderId > 0)
                {
                    Context.UpdateChild<PimsInterestHolder, long, PimsInthldrPropInterest, long>(p => p.PimsInthldrPropInterests, ih.InterestHolderId, ih.PimsInthldrPropInterests.ToArray());
                }
            });
            IEnumerable<PimsInterestHolder> deletedInterestHolders = currentInterestHolders.Where(cih => !interestHolders.Any(ih => ih.InterestHolderId == cih.InterestHolderId));
            deletedInterestHolders.SelectMany(dih => dih.PimsInthldrPropInterests).ForEach(pihp =>
            {
                pihp.InterestHolderInterestTypeCodeNavigation = null;
                pihp.InterestHolder = null;
                Context.Remove(pihp);
            });

            return GetInterestHoldersByAcquisitionFile(acquisitionFileId);
        }

        #endregion
    }
}
