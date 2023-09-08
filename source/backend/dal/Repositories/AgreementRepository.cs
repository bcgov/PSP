using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with agreements within the datasource.
    /// </summary>
    public class AgreementRepository : BaseRepository<PimsAgreement>, IAgreementRepository
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

        public List<PimsAgreement> GetAgreementsByAquisitionFile(long acquisitionFileId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsAgreements
                .Where(ci => ci.AcquisitionFileId == acquisitionFileId)
                .Include(ci => ci.AgreementTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public List<PimsAgreement> SearchAgreements(AcquisitionReportFilterModel filter)
        {
            using var scope = Logger.QueryScope();

            var query = Context.PimsAgreements
                .Include(a => a.AgreementTypeCodeNavigation)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.PimsAcquisitionFilePeople)
                    .ThenInclude(afp => afp.Person)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.Project)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.Product)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.AcquisitionFileStatusTypeCodeNavigation)
                .AsNoTracking();

            if (filter.Projects != null && filter.Projects.Any())
            {
                query = query.Where(a => a.AcquisitionFile.ProjectId.HasValue && filter.Projects.Contains(a.AcquisitionFile.ProjectId.Value));
            }
            if (filter.AcquisitionTeamPersons != null && filter.AcquisitionTeamPersons.Any())
            {
                query = query.Where(a => a.AcquisitionFile.PimsAcquisitionFilePeople.Any(afp => filter.AcquisitionTeamPersons.Contains(afp.PersonId)));
            }
            return query.ToList();
        }

        public PimsAgreement Update(PimsAgreement agreement)
        {
            agreement.ThrowIfNull(nameof(agreement));

            Context.Entry(agreement).CurrentValues.SetValues(agreement);
            Context.Entry(agreement).State = EntityState.Modified;
            return agreement;
        }

        public List<PimsAgreement> UpdateAllForAcquisition(long acquisitionFileId, List<PimsAgreement> agreements)
        {
            Context.UpdateChild<PimsAcquisitionFile, long, PimsAgreement, long>(p => p.PimsAgreements, acquisitionFileId, agreements.ToArray());
            return agreements;
        }

        #endregion
    }
}
