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

        public IEnumerable<PimsCompReqFinancial> SearchCompensationRequisitionFinancials(AcquisitionReportFilterModel filter, bool includeAcquisitions = true, bool includeLeases = true)
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
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.AcquisitionFile)
                            .ThenInclude(a => a.Product);
            }

            if (includeLeases)
            {
                query = query
                    .Include(f => f.CompensationRequisition)
                        .ThenInclude(cr => cr.Lease)
                            .ThenInclude(l => l.Project)
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

                predicate.And(projectBuilder);
            }

            if (includeAcquisitions && filter.AcquisitionTeamPersons != null && filter.AcquisitionTeamPersons.Any())
            {
                predicate.And(f => f.CompensationRequisition.AcquisitionFile != null && f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFileTeams.Any(afp => afp.PersonId.HasValue && filter.AcquisitionTeamPersons.Contains((long)afp.PersonId)));
            }

            if (includeAcquisitions && filter.AcquisitionTeamOrganizations != null && filter.AcquisitionTeamOrganizations.Any())
            {
                predicate.And(f => f.CompensationRequisition.AcquisitionFile != null && f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFileTeams.Any(o => o.OrganizationId.HasValue && filter.AcquisitionTeamOrganizations.Contains((long)o.OrganizationId)));
            }

            return query.Where(predicate).ToList();
        }
    }
}
