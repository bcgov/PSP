using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
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
        private readonly IResearchFileService _researchFileService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="researchFileService"></param>
        /// <param name="activityService"></param>
        /// <param name="mapper"></param>
        ///
        public ResearchFileController(IResearchFileService researchFileService, IMapper mapper)
        {
            _researchFileService = researchFileService;
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
            var researchFile = _researchFileService.GetById(id);
            return new JsonResult(_mapper.Map<ResearchFileModel>(researchFile));
        }

        /// <summary>
        /// Get the research file properties.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/properties")]
        [HasPermission(Permissions.ResearchFileView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ResearchFilePropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "researchfile" })]
        public IActionResult GetResearchFileProperties(long id)
        {
            var researchFileProperties = _researchFileService.GetProperties(id);

            return new JsonResult(_mapper.Map<IEnumerable<ResearchFilePropertyModel>>(researchFileProperties));
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
        public IActionResult AddResearchFile(ResearchFileModel researchFileModel, [FromQuery] string[] userOverrideCodes)
        {
            var researchFileEntity = _mapper.Map<Dal.Entities.PimsResearchFile>(researchFileModel);
            var researchFile = _researchFileService.Add(researchFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));

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
            var researchFile = _researchFileService.Update(researchFileEntity);

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
        public IActionResult UpdateResearchFileProperties([FromBody] ResearchFileModel researchFileModel, [FromQuery] string[] userOverrideCodes)
        {
            var researchFileEntity = _mapper.Map<Dal.Entities.PimsResearchFile>(researchFileModel);
            var researchFile = _researchFileService.UpdateProperties(researchFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));

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
            var researchFile = _researchFileService.UpdateProperty(researchFileId, researchFilePropertyModel.File.RowVersion, researchFilePropertyEntity);

            return new JsonResult(_mapper.Map<ResearchFileModel>(researchFile));
        }

        #endregion
    }
}
