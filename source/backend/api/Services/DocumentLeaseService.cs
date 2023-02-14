using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentLeaseService implementation provides document managing capabilities for leases.
    /// </summary>
    public class DocumentLeaseService : BaseService, IDocumentLeaseService
    {
        private readonly IDocumentActivityService documentActivityService;
        private readonly IActivityTemplateRepository activityTemplateRepository;
        private readonly IActivityService activityService;

        public DocumentLeaseService(
            ClaimsPrincipal user,
            ILogger<DocumentLeaseService> logger,
            IDocumentActivityService documentActivityService,
            IActivityService activityService,
            IActivityTemplateRepository activityTemplateRepository)
            : base(user, logger)
        {
            this.documentActivityService = documentActivityService;
            this.activityService = activityService;
            this.activityTemplateRepository = activityTemplateRepository;
        }

        public IList<PimsActivityInstanceDocument> GetLeaseDocuments(long leaseId)
        {
            this.Logger.LogInformation("Retrieving PIMS document lease");
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            var genericActivity = GetGenericActivity(leaseId);

            if (genericActivity == null)
            {
                return new List<PimsActivityInstanceDocument>();
            }

            return documentActivityService.GetActivityDocuments(genericActivity.ActivityInstanceId);
        }

        public Task<DocumentUploadRelationshipResponse> UploadLeaseDocumentAsync(long leaseId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for lease");
            this.User.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            var genericActivity = GetGenericActivity(leaseId);
            if (genericActivity == null)
            {
                var generalActivityTemplate = activityTemplateRepository.GetActivityTemplateByCode(ActivityTemplateType.General);

                PimsActivityInstance activity = new PimsActivityInstance()
                {
                    Description = string.Empty,
                    ActivityDataJson = string.Empty,
                    ActivityTemplateId = generalActivityTemplate.ActivityTemplateId,
                    PimsLeaseActivityInstances = new List<PimsLeaseActivityInstance>(),
                };

                activity.PimsLeaseActivityInstances.Add(new PimsLeaseActivityInstance()
                {
                    LeaseId = leaseId,
                    ActivityInstance = activity,
                });

                genericActivity = activityService.Add(activity);
            }

            return documentActivityService.UploadActivityDocumentAsync(genericActivity.Internal_Id, uploadRequest);
        }

        public Task<ExternalResult<string>> DeleteLeaseDocumentAsync(PimsActivityInstanceDocument leaseDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for lease");
            this.User.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);
            return documentActivityService.DeleteActivityDocumentAsync(leaseDocument);
        }

        private PimsActivityInstance GetGenericActivity(long leaseId)
        {
            var activities = activityService.GetAllByLeaseId(leaseId);
            var genericActivity = activities.FirstOrDefault(x => x.ActivityTemplate.ActivityTemplateTypeCode == ActivityTemplateType.General);
            return genericActivity;
        }
    }
}
