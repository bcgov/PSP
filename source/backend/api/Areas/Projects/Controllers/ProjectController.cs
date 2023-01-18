using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Projects.Controllers
{
    /// <summary>
    /// ProjectController class, provides endpoints for interacting with projects.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("projects")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ProjectController : ControllerBase
    {
        #region fields
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        #endregion

        /// <summary>
        /// Creates a new instance of a ProjectController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="projectService"></param>
        /// <param name="mapper"></param>
        ///
        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a matching set of projects for the given input filter.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="top"></param>
        /// <returns>An array of contacts matching the filter.</returns>
        [HttpGet("search={input}&top={top}")]
        [HasPermission(Permissions.ProjectView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ProjectModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "project" })]
        public IActionResult SearchProjects(string input, int top)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new BadRequestException("Autocomplete request must contain valid values.");
            }

            var predictions = _projectService.SearchProjects(input, top);

            return new JsonResult(_mapper.Map<IList<ProjectModel>>(predictions));
        }

        /// <summary>
        /// Add the specified lease. Allows the user to override the normal restriction on adding properties already associated to a lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.ProjectAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectModel), 200)]
        [SwaggerOperation(Tags = new[] { "project" })]
        public IActionResult AddLease(ProjectModel projectModel)
        {
            var leaseEntity = _mapper.Map<Dal.Entities.PimsProject>(projectModel);
            var project = _projectService.Add(leaseEntity);

            return new JsonResult(_mapper.Map<ProjectModel>(project));
        }
    }
}
