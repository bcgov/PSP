using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public List<PimsAgreement> GetAgreementsByAcquisitionFile(long acquisitionFileId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsAgreements
                .Where(ci => ci.AcquisitionFileId == acquisitionFileId)
                .Include(ci => ci.AgreementTypeCodeNavigation)
                .Include(ci => ci.AgreementStatusTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public PimsAgreement AddAgreement(PimsAgreement agreement)
        {
            using var scope = Logger.QueryScope();

            Context.PimsAgreements.Add(agreement);

            return agreement;
        }

        public PimsAgreement GetAgreementById(long agreementId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsAgreements.Where(x => x.AgreementId == agreementId)
                .AsNoTracking()
                .Include(x => x.AgreementTypeCodeNavigation)
                .Include(x => x.AgreementStatusTypeCodeNavigation)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public List<PimsAgreement> SearchAgreements(AcquisitionReportFilterModel filter)
        {
            using var scope = Logger.QueryScope();

            var predicate = PredicateBuilder.New<PimsAgreement>(ag => true);

            if (filter.Projects != null && filter.Projects.Any())
            {
                predicate.And(a => a.AcquisitionFile.ProjectId.HasValue && filter.Projects.Contains(a.AcquisitionFile.ProjectId.Value));
            }

            if (filter.AcquisitionTeamPersons != null && filter.AcquisitionTeamPersons.Any())
            {
                predicate.And(a => a.AcquisitionFile.PimsAcquisitionFileTeams.Any(afp => afp.PersonId.HasValue && filter.AcquisitionTeamPersons.Contains((long)afp.PersonId)));
            }

            if (filter.AcquisitionTeamOrganizations != null && filter.AcquisitionTeamOrganizations.Any())
            {
                predicate.And(a => a.AcquisitionFile.PimsAcquisitionFileTeams.Any(o => o.OrganizationId.HasValue && filter.AcquisitionTeamOrganizations.Contains((long)o.OrganizationId)));
            }

            var query = Context.PimsAgreements
                .Include(a => a.AgreementTypeCodeNavigation)
                .Include(a => a.AgreementStatusTypeCodeNavigation)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.PimsAcquisitionFileTeams)
                    .ThenInclude(afp => afp.Person)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.PimsAcquisitionFileTeams)
                    .ThenInclude(o => o.Organization)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.Project)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.Product)
                .Include(a => a.AcquisitionFile)
                    .ThenInclude(a => a.AcquisitionFileStatusTypeCodeNavigation)
                .AsNoTracking()
                .Where(predicate);

            return query.ToList();
        }

        public PimsAgreement UpdateAgreement(PimsAgreement agreement)
        {
            using var scope = Logger.QueryScope();

            var existingAgreement = Context.PimsAgreements.FirstOrDefault(x => x.AgreementId == agreement.AgreementId) ?? throw new KeyNotFoundException();

            Context.Entry(existingAgreement).CurrentValues.SetValues(agreement);

            return existingAgreement;
        }

        public bool TryDeleteAgreement(long acquisitionFileId, long agreementId)
        {
            using var scope = Logger.QueryScope();

            var deletedEntity = Context.PimsAgreements.Where(x => x.AcquisitionFileId == acquisitionFileId && x.AgreementId == agreementId).FirstOrDefault();
            if (deletedEntity is not null)
            {
                Context.PimsAgreements.Remove(deletedEntity);

                return true;
            }

            return false;
        }

        #endregion
    }
}
