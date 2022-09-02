using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

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
        public ActivityRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<ResearchFileRepository> logger, IMapper mapper)
            : base(dbContext, user, logger)
        {
        }
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
                .FirstOrDefault(x => x.ActivityInstanceId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Retrieves the activities  with the specified research file id.
        /// </summary>
        /// <param name="researchFileId"></param>
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
            // return this.Context.PimsActivityInstances.AsNoTracking()
            // .Include(i => i.ActivityTemplate)
            // .ThenInclude(t => t.ActivityTemplateTypeCodeNavigation)
            // .Where(x => x.ActivityInstanceId == researchFileId)
            // .ToList();
        }

        public PimsActivityInstance Add(PimsActivityInstance instance)
        {
            instance.ThrowIfNull(nameof(instance));
            this.Context.PimsActivityInstances.Add(instance);
            return instance;
        }

        public PimsActivityInstance Update(PimsActivityInstance instance)
        {
            instance.ThrowIfNull(nameof(instance));
            GetById(instance.ActivityInstanceId);
            if (instance.ActivityTemplate != null)
            {
                Context.Entry(instance.ActivityTemplate).State = EntityState.Unchanged;
            }
            foreach (PimsActivityInstanceDocument activityInstanceDocument in instance.PimsActivityInstanceDocuments)
            {
                Context.Entry(activityInstanceDocument).State = EntityState.Unchanged;
            }

            foreach (PimsActivityInstanceNote activityInstanceNote in instance.PimsActivityInstanceNotes)
            {
                Context.Entry(activityInstanceNote).State = EntityState.Unchanged;
            }

            this.Context.PimsActivityInstances.Update(instance);
            return instance;
        }

        public bool Delete(long activityId)
        {
            var instance = this.Context.PimsActivityInstances
                .FirstOrDefault(x => x.ActivityInstanceId == activityId) ?? throw new KeyNotFoundException();
            this.Context.PimsActivityInstances.Remove(instance);
            return true;
        }

        /// <summary>
        /// Retrieves the row version of the activity with the specified id.
        /// </summary>
        /// <param name="activityId">The activity id.</param>
        /// <returns>The row version.</returns>
        public long GetRowVersion(long activityId)
        {
            return this.Context.PimsActivityInstances.AsNoTracking()
                .Where(n => n.ActivityInstanceId == activityId)?
                .Select(n => n.ConcurrencyControlNumber)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        #endregion
    }
}
