using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Acquisition.Controllers;
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
        /// Get the specified Project by Id.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [HasPermission(Permissions.ProjectView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectModel), 200)]
        [ProducesResponseType(typeof(ProjectModel), 404)]
        [SwaggerOperation(Tags = new[] { "project" })]
        public IActionResult GetById([FromRoute] long id)
        {
            var project = _projectService.GetById(id);

            return new JsonResult(_mapper.Map<ProjectModel>(project));
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
        /// Add the specified Project.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.ProjectAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "project" })]
        public IActionResult AddProject(ProjectModel projectModel)
        {
            var newProject = _projectService.Add(_mapper.Map<Dal.Entities.PimsProject>(projectModel));
            return new JsonResult(_mapper.Map<ProjectModel>(newProject));
        }

        /// <summary>
        /// Updates the project.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.ProjectEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectModel), 200)]
        [SwaggerOperation(Tags = new[] { "project" })]
        public IActionResult UpdateProject([FromRoute]long id, [FromBody] ProjectModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var updatedProject = _projectService.Update(id, _mapper.Map<Dal.Entities.PimsProject>(model));

            return new JsonResult(updatedProject);
        }

        /// <summary>
        /// Retrieves a the products for the given project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>An array of contacts matching the filter.</returns>
        [HttpGet("{projectId}/products")]
        [HasPermission(Permissions.ProjectView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ProductModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "project" })]
        public IActionResult GetProducts(long projectId)
        {
            var products = _projectService.GetProducts(projectId);

            return new JsonResult(_mapper.Map<IList<ProductModel>>(products));
        }
    }
}
