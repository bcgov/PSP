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
    /// Provides a repository to interact with agreements within the datasource.
    /// </summary>
    public class AgreementRepository : BaseRepository<PimsAcquisitionChecklistItem>, IAgreementRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AgreementRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AgreementRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AgreementRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public IEnumerable<PimsAcqChklstItemType> GetAllChecklistItemTypes()
        {
            return Context.PimsAcqChklstItemTypes
                .Include(it => it.AcqChklstSectionTypeCodeNavigation)
                .OrderBy(it => it.DisplayOrder)
                .AsNoTracking()
                .ToArray();
        }

        public List<PimsAgreement> GetAgreementsByAquisitionFile(long acquisitionFileId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsAgreements
                .Where(ci => ci.AcquisitionFileId == acquisitionFileId)
                .Include(ci => ci.AgreementTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public PimsAgreement Update(PimsAgreement agreement)
        {
            agreement.ThrowIfNull(nameof(agreement));

            Context.Entry(agreement).CurrentValues.SetValues(agreement);
            Context.Entry(agreement).State = EntityState.Modified;
            return agreement;
        }

        #endregion
    }
}
