using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Projects.Models;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Project;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Projects.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("projects")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public SearchController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet]
        [HasPermission(Permissions.ProjectView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ProjectModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "project" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> GetProject([FromQuery] ProjectFilterModel filter)
        {
            var projects = await _projectService.GetPage((ProjectFilter)filter);
            return Ok(_mapper.Map<PageModel<ProjectModel>>(projects));
        }
    }
}
