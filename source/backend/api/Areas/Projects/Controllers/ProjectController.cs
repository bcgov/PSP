using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Product;
using Pims.Api.Models.Concepts.Project;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
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
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<ProjectController> _logger;
        #endregion

        /// <summary>
        /// Creates a new instance of a ProjectController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="projectService"></param>
        /// <param name="projectRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ProjectController(IProjectService projectService, IProjectRepository projectRepository, IMapper mapper, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _logger = logger;
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
        [TypeFilter(typeof(NullJsonResultFilter))]
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
        /// <returns>An array of projects matching the filter.</returns>
        [HttpGet("search={input}&top={top}")]
        [HasPermission(Permissions.ProjectView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ProjectModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "project" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
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
        /// Retrieves all projects within PIMS.
        /// </summary>
        /// <returns>An array of projects matching the filter.</returns>
        [HttpGet("")]
        [HasPermission(Permissions.ProjectView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ProjectModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "project" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAll()
        {
            var projects = _projectService.GetAll();

            return new JsonResult(_mapper.Map<IList<ProjectModel>>(projects));
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
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddProject(ProjectModel projectModel, [FromQuery] string[] userOverrideCodes)
        {
            try
            {
                var newProject = _projectService.Add(_mapper.Map<Dal.Entities.PimsProject>(projectModel), userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));
                return new JsonResult(_mapper.Map<ProjectModel>(newProject));
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
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
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateProject([FromRoute] long id, [FromBody] ProjectModel model, [FromQuery] string[] userOverrideCodes)
        {
            if (id != model.Id)
            {
                return BadRequest("Model and path id do not match.");
            }

            try
            {
                var updatedProject = _projectService.Update(_mapper.Map<Dal.Entities.PimsProject>(model), userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));
                return new JsonResult(updatedProject);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
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
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetProducts(long projectId)
        {
            var products = _projectService.GetProducts(projectId);

            return new JsonResult(_mapper.Map<IList<ProductModel>>(products));
        }

        [HttpGet("{id:long}/historical")]
        [Produces("application/json")]
        [HasPermission(Permissions.ProjectView)]
        [ProducesResponseType(typeof(ProjectModel), 200)]
        [SwaggerOperation(Tags = new[] { "project" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetProjectAtTime([FromRoute] long id, [FromQuery] DateTime time)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ProductController),
                nameof(GetProjectAtTime),
                User.GetUsername(),
                DateTime.Now);

            var pimsProject = _projectRepository.GetProjectAtTime(id, time);
            return new JsonResult(_mapper.Map<ProjectModel>(pimsProject));
        }
    }
}
