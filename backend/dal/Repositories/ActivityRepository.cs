using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
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
                .Include(a => a.ActivityInstanceStatusTypeCodeNavigation)
                .Include(a => a.PimsActInstPropAcqFiles)
                .Include(a => a.PimsActInstPropRsrchFiles)
                .AsNoTracking()
                .FirstOrDefault(x => x.ActivityInstanceId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Retrieves the activities with the specified research file id.
        /// </summary>
        /// <param name="researchFileId"></param>
        /// <returns></returns>
        public IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId)
        {
             return this.Context.PimsActivityInstances.AsNoTracking()
                .Include(r => r.ActivityTemplate).ThenInclude(y => y.ActivityTemplateTypeCodeNavigation)
                .Include(a => a.ActivityInstanceStatusTypeCodeNavigation)
                .Where(x => x.PimsResearchActivityInstances.Any(ra => ra.ResearchFileId == researchFileId))
                .ToList();
        }

        /// <summary>
        /// Retrieves the activities with the specified acquisition file id.
        /// </summary>
        /// <param name="acquisitionFileId"></param>
        /// <returns></returns>
        public IList<PimsActivityInstance> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            return this.Context.PimsActivityInstances.AsNoTracking()
               .Include(r => r.ActivityTemplate).ThenInclude(y => y.ActivityTemplateTypeCodeNavigation)
               .Include(a => a.ActivityInstanceStatusTypeCodeNavigation)
               .Where(x => x.PimsAcquisitionActivityInstances.Any(ra => ra.AcquisitionFileId == acquisitionFileId))
               .ToList();
        }

        public PimsActivityInstance Add(PimsActivityInstance instance)
        {
            instance.ThrowIfNull(nameof(instance));
            if (instance.ActivityTemplate != null)
            {
                Context.Entry(instance.ActivityTemplate).State = EntityState.Unchanged;
            }
            this.Context.PimsActivityInstances.Add(instance);
            return instance;
        }

        public PimsActivityInstance Update(PimsActivityInstance instance)
        {
            instance.ThrowIfNull(nameof(instance));
            var currentActivity = this.Context.PimsActivityInstances
                .FirstOrDefault(x => x.ActivityInstanceId == instance.Id) ?? throw new KeyNotFoundException();
            this.Context.Entry(currentActivity).CurrentValues.SetValues(instance);

            return instance;
        }

        public PimsActivityInstance UpdateActivityResearchProperties(PimsActivityInstance instance)
        {
            instance.ThrowIfNull(nameof(instance));
            this.Context.UpdateChild<PimsActivityInstance, long, PimsActInstPropRsrchFile>(a => a.PimsActInstPropRsrchFiles, instance.Id, instance.PimsActInstPropRsrchFiles.ToArray(), false);

            return instance;
        }

        public PimsActivityInstance UpdateActivityAcquisitionProperties(PimsActivityInstance instance)
        {
            instance.ThrowIfNull(nameof(instance));
            this.Context.UpdateChild<PimsActivityInstance, long, PimsActInstPropRsrchFile>(a => a.PimsActInstPropRsrchFiles, instance.Id, instance.PimsActInstPropRsrchFiles.ToArray(), false);

            return instance;
        }

        public bool Delete(long activityId)
        {
            var instance = this.Context.PimsActivityInstances
                .Include(a => a.PimsResearchActivityInstances)
                .Include(a => a.PimsAcquisitionActivityInstances)
                .Include(a => a.PimsActivityInstanceDocuments)
                .Include(a => a.PimsActivityInstanceNotes)
                .Include(a => a.PimsActInstPropAcqFiles)
                .Include(a => a.PimsActInstPropRsrchFiles)
                .FirstOrDefault(x => x.ActivityInstanceId == activityId) ?? throw new KeyNotFoundException();

            foreach(var acquisitionActivityInstance in instance.PimsAcquisitionActivityInstances)
            {
                this.Context.PimsAcquisitionActivityInstances.Remove(acquisitionActivityInstance);
            }

            foreach (var researchActivityInstance in instance.PimsResearchActivityInstances)
            {
                this.Context.PimsResearchActivityInstances.Remove(researchActivityInstance);
            }

            foreach (var activityDocument in instance.PimsActivityInstanceDocuments)
            {
                this.Context.PimsActivityInstanceDocuments.Remove(activityDocument);
            }

            foreach (var activityNote in instance.PimsActivityInstanceNotes)
            {
                this.Context.PimsActivityInstanceNotes.Remove(activityNote);
            }

            foreach (var propAcqFile in instance.PimsActInstPropAcqFiles)
            {
                this.Context.PimsActInstPropAcqFiles.Remove(propAcqFile);
            }

            foreach (var propRsrchFile in instance.PimsActInstPropRsrchFiles)
            {
                this.Context.PimsActInstPropRsrchFiles.Remove(propRsrchFile);
            }

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
