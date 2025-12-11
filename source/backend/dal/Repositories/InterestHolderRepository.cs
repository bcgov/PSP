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
                .Include(ih => ih.InterestHolderTypeCodeNavigation)
                .Include(ih => ih.PrimaryContact)
                    .ThenInclude(pc => pc.PimsResearchFiles)
                .Include(ih => ih.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(oa => oa.Address)
                    .ThenInclude(a => a.Country)
                .Include(ih => ih.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(oa => oa.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(ih => ih.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(oa => oa.AddressUsageTypeCodeNavigation)
                .Include(ih => ih.Person)
                    .ThenInclude(p => p.PimsPersonAddresses)
                    .ThenInclude(oa => oa.Address)
                    .ThenInclude(a => a.Country)
                .Include(ih => ih.Person)
                    .ThenInclude(p => p.PimsPersonAddresses)
                    .ThenInclude(oa => oa.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(ih => ih.Person)
                    .ThenInclude(p => p.PimsPersonAddresses)
                    .ThenInclude(oa => oa.AddressUsageTypeCodeNavigation)
                .Include(ih => ih.PimsInthldrPropInterests)
                    .ThenInclude(ip => ip.PimsPropInthldrInterestTyps)
                    .ThenInclude(ipt => ipt.InterestHolderInterestTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public List<PimsInterestHolder> UpdateAllForAcquisition(long acquisitionFileId, List<PimsInterestHolder> interestHolders)
        {
            interestHolders.ForEach(ih =>
            {
                // ignore interest holders with no id, as those are new and do not require updates, the will be inserted by the parent's updateChild.
                if (ih.InterestHolderId > 0)
                {
                    ih.PimsInthldrPropInterests.ForEach(ihp =>
                    {
                        // ignore interest holders properties, as those are new and do not require updates, the will be inserted by the parent's updateChild.
                        if (ihp.PimsInthldrPropInterestId > 0)
                        {
                            Context.UpdateChild<PimsInthldrPropInterest, long, PimsPropInthldrInterestTyp, long>(p => p.PimsPropInthldrInterestTyps, ihp.PimsInthldrPropInterestId, ihp.PimsPropInthldrInterestTyps.ToArray());
                        }
                    });
                    Context.CommitTransaction();

                    Context.UpdateChild<PimsInterestHolder, long, PimsInthldrPropInterest, long>(p => p.PimsInthldrPropInterests, ih.InterestHolderId, ih.PimsInthldrPropInterests.ToArray());
                    Context.CommitTransaction();
                }
            });

            Context.UpdateChild<PimsAcquisitionFile, long, PimsInterestHolder, long>(p => p.PimsInterestHolders, acquisitionFileId, interestHolders.ToArray());

            return GetInterestHoldersByAcquisitionFile(acquisitionFileId);
        }

        public bool DeleteInterestHoldersPropertyTypes(ICollection<PimsPropInthldrInterestTyp> inthldPropTypes)
        {
            inthldPropTypes.ForEach(ihpt =>
            {
                Context.PimsPropInthldrInterestTyps.Remove(new PimsPropInthldrInterestTyp() { Internal_Id = ihpt.Internal_Id });
            });

            Context.CommitTransaction();

            return true;
        }

        public bool DeleteInterestHoldersProperties(ICollection<PimsInthldrPropInterest> interestHolderProperties)
        {
            interestHolderProperties.ForEach(ihp =>
            {
                DeleteInterestHoldersPropertyTypes(ihp.PimsPropInthldrInterestTyps);
                Context.PimsInthldrPropInterests.Remove(new PimsInthldrPropInterest() { Internal_Id = ihp.Internal_Id });
            });
            Context.CommitTransaction();

            return true;
        }

        public bool DeleteInterestHolders(ICollection<PimsInterestHolder> interestHolders)
        {
            interestHolders.ForEach(ih =>
            {
                DeleteInterestHoldersProperties(ih.PimsInthldrPropInterests);
                Context.PimsInterestHolders.Remove(new PimsInterestHolder() { Internal_Id = ih.Internal_Id });
            });
            Context.CommitTransaction();

            return true;
        }

        #endregion
    }
}
