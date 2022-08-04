using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with research files within the datasource.
    /// </summary>
    public class ActivityRepository : BaseRepository<PimsActivityInstance>, IActivityRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a ResearchFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ActivityRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<ResearchFileRepository> logger, IMapper mapper) : base(dbContext, user,  logger) { }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the activity  with the specified  id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsActivityInstance GetById(long id)
        {
            return this.Context.PimsActivityInstances
                .Include(r => r.ActivityTemplate).ThenInclude(y => y.ActivityTemplateTypeCodeNavigation)
                .AsNoTracking()
                .FirstOrDefault(x => x.ActivityInstanceId == id);
        }

        /// <summary>
        /// Retrieves the activities  with the specified research file id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId)
        {

            List<PimsActivityInstance> instances = new List<PimsActivityInstance>();
            instances.Add(new PimsActivityInstance()
            {
                ActivityInstanceId = 1,
                ActivityTemplateId = 1,
                ActivityTemplate = new PimsActivityTemplate()
                {
                    ActivityTemplateId = 1,
                    ActivityTemplateTypeCodeNavigation = new PimsActivityTemplateType()
                    {
                        Id = "GENERAL",
                        ActivityTemplateTypeCode = "GENERAL",
                        Description = "General",
                    },
                },
            });
            instances.Add(new PimsActivityInstance()
            {
                ActivityInstanceId = 2,
                ActivityTemplateId = 2,
                ActivityTemplate = new PimsActivityTemplate()
                {
                    ActivityTemplateId = 2,
                    ActivityTemplateTypeCodeNavigation = new PimsActivityTemplateType()
                    {
                        Id = "SITEVIS",
                        ActivityTemplateTypeCode = "SITEVIS",
                        Description = "Site Visit",
                    },
                },
            });
            instances.Add(new PimsActivityInstance()
            {
                ActivityInstanceId = 3,
                ActivityTemplateId = 3,
                ActivityTemplate = new PimsActivityTemplate()
                {
                    ActivityTemplateId = 3,
                    ActivityTemplateTypeCodeNavigation = new PimsActivityTemplateType()
                    {
                        Id = "SURVEY",
                        ActivityTemplateTypeCode = "SURVEY",
                        Description = "Survey",
                    },
                },
            });
            return instances;
            // TODO Call actual table data
            //return this.Context.PimsActivityInstances.AsNoTracking()                
            //    .Where(x => x.ActivityInstanceId == researchFileId)
            //    .ToList();
        }

        public PimsActivityInstance Add(PimsActivityInstance instance)
        {
            instance.ThrowIfNull(nameof(instance));           
            this.Context.PimsActivityInstances.Add(instance);
            return instance;
        }

        #endregion
    }
}
