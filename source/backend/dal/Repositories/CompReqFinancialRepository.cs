using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with compensation requisition h120s within the datasource.
    /// </summary>
    public class CompReqFinancialRepository : BaseRepository<PimsCompReqFinancial>, ICompReqFinancialRepository
    {
        #region Constructors
        private readonly ClaimsPrincipal _user;

        /// <summary>
        /// Creates a new instance of a CompReqFinancialRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CompReqFinancialRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
            this._user = user;
        }
        #endregion

        public IEnumerable<PimsCompReqFinancial> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly)
        {
            this._user.ThrowIfNotAllAuthorized(Permissions.CompensationRequisitionView);

            var query = Context.PimsCompReqFinancials
                .Include(c => c.CompensationRequisition)
                .Where(c => c.CompensationRequisition.AcquisitionFileId == acquisitionFileId);

            if (finalOnly == true)
            {
                query = query.Where(c => c.CompensationRequisition.IsDraft == false);
            }

            return query.AsNoTracking().ToArray();
        }

        public IEnumerable<PimsCompReqFinancial> GetAllByLeaseFileId(long leaseFileId, bool? finalOnly)
        {
            this._user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            var query = Context.PimsCompReqFinancials
                .Include(c => c.CompensationRequisition)
                .Where(c => c.CompensationRequisition.LeaseId == leaseFileId);

            if (finalOnly == true)
            {
                query = query.Where(c => c.CompensationRequisition.IsDraft == false);
            }

            return query.AsNoTracking().ToList();
        }

        public IEnumerable<PimsCompReqFinancial> SearchCompensationRequisitionFinancials(AcquisitionReportFilterModel filter, HashSet<short> regions, long? contractorPersonId = null, bool includeAcquisitions = true, bool includeLeases = true)
        {
            using var scope = Logger.QueryScope();

            if (!includeAcquisitions && !includeLeases)
            {
                throw new BusinessRuleViolationException("Compensation Requisition Export requires access to either acquisition files or leases");
            }

            IQueryable<PimsCompReqFinancial> query = Context.PimsCompReqFinancials.AsNoTracking()
                .Include(f => f.FinancialActivityCode)
                .Include(f => f.CompensationRequisition)
                    .ThenInclude(cr => cr.AlternateProject);

            if (includeAcquisitions)
            {
                query = query
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.AcquisitionFile)
                            .ThenInclude(a => a.PimsAcquisitionFileTeams)
                                .ThenInclude(afp => afp.Person)
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.AcquisitionFile)
                            .ThenInclude(a => a.PimsAcquisitionFileTeams)
                                .ThenInclude(o => o.Organization)
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.AcquisitionFile)
                            .ThenInclude(a => a.Project)
                                .ThenInclude(p => p.PimsProjectPeople)
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.AcquisitionFile)
                            .ThenInclude(a => a.Product);
            }

            if (includeLeases)
            {
                query = query
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.Lease)
                            .ThenInclude(l => l.PimsLeaseLicenseTeams)
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.Lease)
                            .ThenInclude(l => l.Project)
                                .ThenInclude(p => p.PimsProjectPeople)
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.Lease)
                            .ThenInclude(l => l.Product);
            }

            var predicate = PredicateBuilder.New<PimsCompReqFinancial>(p => true);

            if (filter.Projects != null && filter.Projects.Any())
            {
                var projectBuilder = PredicateBuilder.New<PimsCompReqFinancial>(p => false);
                projectBuilder.Or(f => f.CompensationRequisition.AlternateProjectId.HasValue && filter.Projects.Contains(f.CompensationRequisition.AlternateProjectId.Value));
                projectBuilder.Or(f => !f.CompensationRequisition.AlternateProjectId.HasValue && f.CompensationRequisition.AcquisitionFile != null && f.CompensationRequisition.AcquisitionFile.ProjectId.HasValue && filter.Projects.Contains(f.CompensationRequisition.AcquisitionFile.ProjectId.Value));
                projectBuilder.Or(f => !f.CompensationRequisition.AlternateProjectId.HasValue && f.CompensationRequisition.Lease != null && f.CompensationRequisition.Lease.ProjectId.HasValue && filter.Projects.Contains(f.CompensationRequisition.Lease.ProjectId.Value));

                predicate = predicate.And(projectBuilder);
            }

            var acquisitionPredicate = PredicateBuilder.New<PimsCompReqFinancial>(p => true);
            if (includeAcquisitions)
            {
                acquisitionPredicate = acquisitionPredicate.And(f => f.CompensationRequisition.AcquisitionFile != null);

                // The system will only provide data that adheres to the user's "region limited data".
                acquisitionPredicate = acquisitionPredicate.And(f => regions.Contains(f.CompensationRequisition.AcquisitionFile.RegionCode) || f.CompensationRequisition.AcquisitionFile.RegionCode == 4);

                // If the user is a contractor, they should only see financials for acquisition files they are assigned to.
                if (contractorPersonId is not null)
                {
                    acquisitionPredicate = acquisitionPredicate.And(f => f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFileTeams.Any(afp => afp.PersonId == contractorPersonId) ||
                        (f.CompensationRequisition.AcquisitionFile.Project != null && f.CompensationRequisition.AcquisitionFile.Project.PimsProjectPeople.Any(pp => pp.PersonId == contractorPersonId)));
                }

                if (filter.AcquisitionTeamPersons is not null && filter.AcquisitionTeamPersons.Any())
                {
                    acquisitionPredicate = acquisitionPredicate.And(f => f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFileTeams.Any(afp => afp.PersonId != null && filter.AcquisitionTeamPersons.Contains(afp.PersonId.Value)));
                }

                if (filter.AcquisitionTeamOrganizations is not null && filter.AcquisitionTeamOrganizations.Any())
                {
                    acquisitionPredicate = acquisitionPredicate.And(f => f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFileTeams.Any(o => o.OrganizationId != null && filter.AcquisitionTeamOrganizations.Contains(o.OrganizationId.Value)));
                }
            }

            var leasePredicate = PredicateBuilder.New<PimsCompReqFinancial>(p => true);
            if (includeLeases)
            {
                leasePredicate = leasePredicate.And(f => f.CompensationRequisition.Lease != null);

                // The system will only provide data that adheres to the user's "region limited data".
                leasePredicate = leasePredicate.And(f => !f.CompensationRequisition.Lease.RegionCode.HasValue || regions.Contains(f.CompensationRequisition.Lease.RegionCode.Value));

                // If the user is a contractor, they should only see financials for leases they are assigned to.
                if (contractorPersonId is not null)
                {
                    leasePredicate = leasePredicate.And(f => f.CompensationRequisition.Lease.PimsLeaseLicenseTeams.Any(lt => lt.PersonId == contractorPersonId) ||
                        (f.CompensationRequisition.Lease.Project != null && f.CompensationRequisition.Lease.Project.PimsProjectPeople.Any(pp => pp.PersonId == contractorPersonId)));
                }

                if (filter.AcquisitionTeamPersons is not null && filter.AcquisitionTeamPersons.Any())
                {
                    leasePredicate = leasePredicate.And(f => f.CompensationRequisition.Lease.PimsLeaseLicenseTeams.Any(lt => lt.PersonId != null && filter.AcquisitionTeamPersons.Contains(lt.PersonId.Value)));
                }

                if (filter.AcquisitionTeamOrganizations is not null && filter.AcquisitionTeamOrganizations.Any())
                {
                    leasePredicate = leasePredicate.And(f => f.CompensationRequisition.Lease.PimsLeaseLicenseTeams.Any(o => o.OrganizationId != null && filter.AcquisitionTeamOrganizations.Contains(o.OrganizationId.Value)));
                }
            }

            predicate = predicate.And(acquisitionPredicate.Or(leasePredicate));
            return query.Where(predicate).ToList();
        }
    }
}
