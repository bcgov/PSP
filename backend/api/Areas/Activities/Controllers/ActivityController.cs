using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Pims.Api.Areas.Activities.Controllers
{
    /// <summary>
    /// ActivityController class, provides endpoints for interacting with activities.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("activities")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ActivityController : ControllerBase
    {
        #region Variables
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ActivityController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="activityService"></param>
        /// <param name="mapper"></param>
        ///
        public ActivityController(IActivityService activityService, IMapper mapper)
        {
            _activityService = activityService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Add the specified activity.
        /// </summary>
        /// <param name="fileType">The type of the file.</param>
        /// <param name="activityFileModel">The activity to add.</param>
        /// <returns></returns>
        [HttpPost("{fileType}")]
        [HasPermission(Permissions.ActivityAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult AddActivityFile(FileType fileType, [FromBody] ActivityInstanceFileModel activityFileModel)
        {
            var activityInstance = _mapper.Map<PimsActivityInstance>(activityFileModel.Activity);
            PimsActivityInstance createdActivity;
            createdActivity = fileType switch
            {
                FileType.Research => _activityService.AddResearchActivity(activityInstance, activityFileModel.FileId),
                FileType.Acquisition => _activityService.AddAcquisitionActivity(activityInstance, activityFileModel.FileId),
                _ => throw new BadRequestException("File type must be a known type"),
            };
            return new JsonResult(_mapper.Map<ActivityInstanceModel>(createdActivity));
        }

        /// <summary>
        /// Retrieves the activity with the specified id.
        /// </summary>
        /// <param name="activityId">Used to identify the activity.</param>
        /// <returns></returns>
        [HttpGet("{activityId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.ActivityView)]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult GetActivityById(long activityId)
        {
            var activity = _activityService.GetById(activityId);
            return new JsonResult(_mapper.Map<ActivityInstanceModel>(activity));
        }

        /// <summary>
        /// Retrieves all activities corresponding to the passed file id and file type.
        /// </summary>
        /// <param name="fileType">The type of file the fileId belongs to.</param>
        /// <param name="fileId">the id of the file.</param>
        /// <returns></returns>
        [HttpGet("{fileType}/{fileId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.ActivityView)]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult GetFileActivities(FileType fileType, long fileId)
        {
            IList<PimsActivityInstance> fileActivities;
            fileActivities = fileType switch
            {
                FileType.Research => _activityService.GetAllByResearchFileId(fileId),
                FileType.Acquisition => _activityService.GetAllByAcquisitionFileId(fileId),
                _ => throw new BadRequestException("File type must be a known type"),
            };
            return new JsonResult(_mapper.Map<List<ActivityInstanceModel>>(fileActivities));
        }

        /// <summary>
        /// Updates the activity with the specified id.
        /// </summary>
        /// <param name="activityId">Used to identify the activity.</param>
        /// <param name="activityModel">The updated activity values.</param>
        /// <returns></returns>
        [HttpPut("{activityId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.ActivityEdit)]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult UpdateActivity(long activityId, [FromBody] ActivityInstanceModel activityModel)
        {
            var activityInstance = _mapper.Map<PimsActivityInstance>(activityModel);
            var updatedActivity = _activityService.Update(activityInstance);
            return new JsonResult(_mapper.Map<ActivityInstanceModel>(updatedActivity));
        }

        /// <summary>
        /// Get the activity template types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("templates")]
        [HasPermission(Permissions.ActivityView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ActivityTemplateModel>), 200)]
        [SwaggerOperation(Tags = new[] { "activity-templates" })]
        public IActionResult GetActivityTemplateTypes()
        {
            var activityTemplates = _activityService.GetAllActivityTemplates();
            var mappedActivityTemplates = _mapper.Map<List<ActivityTemplateModel>>(activityTemplates);
            return new JsonResult(mappedActivityTemplates);
        }

        /// <summary>
        /// Deletes the activity for the specified type.
        /// </summary>
        /// <param name="activityId">Used to identify the activity and delete it.</param>
        /// <returns></returns>
        [HttpDelete("{activityId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.ActivityDelete)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult DeleteActivity(long activityId)
        {
            _activityService.Delete(activityId);
            return new JsonResult(true);
        }
        #endregion
    }
}
