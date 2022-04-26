
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
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
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ResearchFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public ResearchFileController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
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


        #endregion
    }
}
