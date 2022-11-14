using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Constants;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with research files within the datasource.
    /// </summary>
    public class ActivityTemplateRepository : BaseRepository<PimsActivityTemplate>, IActivityTemplateRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ActivityTemplateRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResearchFileRepository> logger, IMapper mapper)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all activity templates.
        /// </summary>
        /// <returns></returns>
        public IList<PimsActivityTemplate> GetAllActivityTemplates()
        {
            return this.Context.PimsActivityTemplates.AsNoTracking()
                .Include(x => x.ActivityTemplateTypeCodeNavigation)
                .ToList();
        }

        /// <summary>
        /// Retrieves template for the matching template type.
        /// </summary>
        /// <returns></returns>
        public PimsActivityTemplate GetActivityTemplateByCode(string templateType)
        {
            return this.Context.PimsActivityTemplates.AsNoTracking()
                .Include(x => x.ActivityTemplateTypeCodeNavigation)
                .FirstOrDefault(x => x.ActivityTemplateTypeCode == templateType);
        }

        #endregion
    }
}
