using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.ResearchFile.Controllers
{
    /// <summary>
    /// ResearchFileController class, provides endpoints for interacting with research files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("researchfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ResearchFileController : ControllerBase
    {
        #region Variables
        private readonly IPimsService _pimsService;
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="activityService"></param>
        /// <param name="mapper"></param>
        ///
        public ResearchFileController(IPimsService pimsService, IActivityService activityService, IMapper mapper)
        {
            _pimsService = pimsService;
            _activityService = activityService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the specified research file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.ResearchFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResearchFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "researchfile" })]
        public IActionResult GetResearchFile(long id)
        {
            var researchFile = _pimsService.ResearchFileService.GetById(id);
            return new JsonResult(_mapper.Map<ResearchFileModel>(researchFile));
        }

        /// <summary>
        /// Gets the activities for specified research file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{researchFileId:long}/activities")]
        [HasPermission(Permissions.ResearchFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ActivityInstanceModel>), 200)]
        [SwaggerOperation(Tags = new[] { "researchfile" })]
        public IActionResult GetActivitiesForResearchFile(long researchFileId)
        {
            var activityInstances = _activityService.GetAllByResearchFileId(researchFileId);
            List<ActivityInstanceModel> models = _mapper.Map<List<ActivityInstanceModel>>(activityInstances);

            return new JsonResult(models);
        }

        /// <summary>
        /// Get the activity template types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("activity-templates")]
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
        /// Add the specified research file activity.
        /// </summary>
        /// <returns></returns>
        [HttpPost("activity")]
        [HasPermission(Permissions.ActivityAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActivityInstanceModel), 200)]
        [SwaggerOperation(Tags = new[] { "activityInstance" })]
        public IActionResult AddActivityInstance(ActivityInstanceModel activityInstanceModel)
        {
            var activityInstanceEntity = _mapper.Map<Dal.Entities.PimsActivityInstance>(activityInstanceModel);
            var activityInstance = _activityService.Add(activityInstanceEntity);

            return new JsonResult(_mapper.Map<ActivityInstanceModel>(activityInstance));
        }

        /// <summary>
        /// Add the specified research file.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.ResearchFileAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResearchFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "researchfile" })]
        public IActionResult AddResearchFile(ResearchFileModel researchFileModel)
        {
            var researchFileEntity = _mapper.Map<Dal.Entities.PimsResearchFile>(researchFileModel);
            var researchFile = _pimsService.ResearchFileService.Add(researchFileEntity);

            return new JsonResult(_mapper.Map<ResearchFileModel>(researchFile));
        }

        /// <summary>
        /// Update the research file.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.ResearchFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResearchFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "researchfile" })]
        public IActionResult UpdateResearchFile([FromBody] ResearchFileModel researchFileModel)
        {
            var researchFileEntity = _mapper.Map<Dal.Entities.PimsResearchFile>(researchFileModel);
            var researchFile = _pimsService.ResearchFileService.Update(researchFileEntity);

            return new JsonResult(_mapper.Map<ResearchFileModel>(researchFile));
        }

        /// <summary>
        /// Update the research file properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}/properties")]
        [HasPermission(Permissions.ResearchFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResearchFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "researchfile" })]
        public IActionResult UpdateResearchFileProperties([FromBody] ResearchFileModel researchFileModel)
        {
            var researchFileEntity = _mapper.Map<Dal.Entities.PimsResearchFile>(researchFileModel);
            var researchFile = _pimsService.ResearchFileService.UpdateProperties(researchFileEntity);

            return new JsonResult(_mapper.Map<ResearchFileModel>(researchFile));
        }

        /// <summary>
        /// Update the specified property on the passed research file.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{researchFileId:long}/properties/{researchFilePropertyId:long}")]
        [HasPermission(Permissions.ResearchFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResearchFilePropertyModel), 200)]
        [SwaggerOperation(Tags = new[] { "researchFile" })]
        public IActionResult UpdateResearchFileProperty(long researchFileId, long researchFilePropertyId, [FromBody] ResearchFilePropertyModel researchFilePropertyModel)
        {
            if (researchFilePropertyId != researchFilePropertyModel.Id)
            {
                throw new BadRequestException($"Bad payload id.");
            }

            var researchFilePropertyEntity = _mapper.Map<Dal.Entities.PimsPropertyResearchFile>(researchFilePropertyModel);
            var researchFile = _pimsService.ResearchFileService.UpdateProperty(researchFileId, researchFilePropertyModel.ResearchFile.RowVersion, researchFilePropertyEntity);

            return new JsonResult(_mapper.Map<ResearchFileModel>(researchFile));
        }

        #endregion
    }
}
