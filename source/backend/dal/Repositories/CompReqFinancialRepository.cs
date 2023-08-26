using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            this._user.ThrowIfNotAllAuthorized(Security.Permissions.CompensationRequisitionView);

            var query = Context.PimsCompReqFinancials
                .Include(c => c.CompensationRequisition)
                .Where(c => c.CompensationRequisition.AcquisitionFileId == acquisitionFileId);

            if (finalOnly == true)
            {
                query = query.Where(c => c.CompensationRequisition.IsDraft == false);
            }
            return query.ToArray();
        }

        public IEnumerable<PimsCompReqFinancial> SearchCompensationRequisitionFinancials(AcquisitionReportFilterModel filter)
        {
            using var scope = Logger.QueryScope();

            var query = Context.PimsCompReqFinancials
                .Include(f => f.FinancialActivityCode)
                .Include(f => f.CompensationRequisition)
                    .ThenInclude(cr => cr.AlternateProject)
                .Include(f => f.CompensationRequisition)
                    .ThenInclude(cr => cr.AcquisitionFile)
                        .ThenInclude(a => a.PimsAcquisitionFilePeople)
                        .ThenInclude(afp => afp.Person)
                .Include(f => f.CompensationRequisition)
                    .ThenInclude(cr => cr.AcquisitionFile)
                        .ThenInclude(a => a.Project)
                .Include(f => f.CompensationRequisition)
                    .ThenInclude(cr => cr.AcquisitionFile)
                        .ThenInclude(a => a.Product)
                .AsNoTracking();

            if (filter.Projects != null && filter.Projects.Any())
            {
                query = query.Where(f =>
                    (f.CompensationRequisition.AlternateProjectId.HasValue && filter.Projects.Contains(f.CompensationRequisition.AlternateProjectId.Value)) ||
                    (!f.CompensationRequisition.AlternateProjectId.HasValue && f.CompensationRequisition.AcquisitionFile.ProjectId.HasValue && filter.Projects.Contains(f.CompensationRequisition.AcquisitionFile.ProjectId.Value)));
            }
            if (filter.AcquisitionTeamPersons != null && filter.AcquisitionTeamPersons.Any())
            {
                query = query.Where(f => f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFilePeople.Any(afp => filter.AcquisitionTeamPersons.Contains(afp.PersonId)));
            }

            return query.ToList();
        }
    }
}
