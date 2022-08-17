using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

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
        /// Creates a new instance of a NoteController class, initializes it with the specified arguments.
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
        /// <param name="activityModel">The activity to add.</param>
        /// <returns></returns>
        [HttpPost("")]
        [HasPermission(Permissions.NoteAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult AddActivity(NoteType type, [FromBody] ActivityInstanceModel activityModel)
        {
            var activityInstance = _mapper.Map<PimsActivityInstance>(activityModel);
            var createdActivity = _activityService.Add(activityInstance);
            return new JsonResult(_mapper.Map<ActivityInstanceModel>(createdActivity));
        }

        /// <summary>
        /// Retrieves the activity with the specified id.
        /// </summary>
        /// <param name="activityId">Used to identify the note.</param>
        /// <returns></returns>
        [HttpGet("{activityId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.NoteView)]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult GetActivityById(long activityId)
        {
            var activity = _activityService.GetById(activityId);
            return new JsonResult(_mapper.Map<ActivityInstanceModel>(activity));
        }

        /// <summary>
        /// Updates the activity with the specified id.
        /// </summary>
        /// <param name="activityId">Used to identify the activity.</param>
        /// <param name="activityModel">The updated note values.</param>
        /// <returns></returns>
        [HttpPut("{activityId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.NoteEdit)]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult UpdateActivity( long activityId, [FromBody] ActivityInstanceModel activityModel)
        {
            var activityInstance = _mapper.Map<PimsActivityInstance>(activityModel);
            var updatedActivity = _activityService.Update(activityInstance);
            return new JsonResult(_mapper.Map<ActivityInstanceModel>(updatedActivity));
        }

        /// <summary>
        /// Deletes the activity for the specified type.
        /// </summary>
        /// <param name="activityId">Used to identify the activity and delete it.</param>
        /// <returns></returns>
        [HttpDelete("{activityId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.NoteDelete)]
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
