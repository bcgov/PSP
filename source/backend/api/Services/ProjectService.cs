using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly ILogger _logger;
        private readonly IProjectRepository _projectRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAcquisitionFileRepository _acquisitionFileRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILookupRepository _lookupRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly ClaimsPrincipal _user;

        /// <summary>
        /// Creates a new instance of a ProjectService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="projectRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="acquisitionFileRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="lookupRepository"></param>
        /// <param name="entityNoteRepository"></param>
        public ProjectService(
            ClaimsPrincipal user,
            ILogger<ProjectService> logger,
            IProjectRepository projectRepository,
            IProductRepository productRepository,
            IAcquisitionFileRepository acquisitionFileRepository,
            IUserRepository userRepository,
            ILookupRepository lookupRepository,
            IEntityNoteRepository entityNoteRepository)
            : base(user, logger)
        {
            _logger = logger;
            _projectRepository = projectRepository;
            _productRepository = productRepository;
            _acquisitionFileRepository = acquisitionFileRepository;
            _userRepository = userRepository;
            _user = user;
            _lookupRepository = lookupRepository;
            _entityNoteRepository = entityNoteRepository;
        }

        public IList<PimsProject> SearchProjects(string filter, int maxResult)
        {
            _logger.LogInformation("Searching for projects that match {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            // Limit search results to user's assigned region(s), but always include "Cannot determine" region
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();

            return _projectRepository.SearchProjects(filter, userRegions, maxResult);
        }

        public IList<PimsProject> GetAll()
        {
            _logger.LogInformation("Retrieving all PIMS projects");
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            // Limit search results to user's assigned region(s), but always include "Cannot determine" region
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();

            return _projectRepository.SearchProjects(string.Empty, userRegions, int.MaxValue);
        }

        public Task<Paged<PimsProject>> GetPage(ProjectFilter filter)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            _logger.LogInformation("Searching for projects ...");
            _logger.LogDebug("Project search with filter", filter);

            // Limit search results to user's assigned region(s), but always include "Cannot determine" region
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            return GetPageAsync(filter, userRegions);
        }

        public PimsProject GetById(long projectId)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);
            _logger.LogInformation("Getting Project by Id ...");

            return _projectRepository.Get(projectId);
        }

        public IList<PimsProduct> GetProducts(long projectId)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            _logger.LogInformation("Geting products for project ...");
            return _productRepository.GetByProject(projectId);
        }

        public List<PimsAcquisitionFile> GetProductFiles(long productId)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            _logger.LogInformation("Geting files for product ...");
            return _acquisitionFileRepository.GetByProductId(productId);
        }

        public PimsProject Add(PimsProject project)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectAdd);
            _logger.LogInformation("Adding new project...");
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }
            CheckForDuplicateProducts(project.PimsProducts, project.Id);

            var newProject = _projectRepository.Add(project);
            _projectRepository.CommitTransaction();

            return _projectRepository.Get(newProject.Internal_Id);
        }

        public PimsProject Update(PimsProject project)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectEdit);
            project.ThrowIfNull(nameof(project));
            _logger.LogInformation($"Updating project with id ${project.Internal_Id}");

            CheckForDuplicateProducts(project.PimsProducts, project.Id);
            var updatedProject = _projectRepository.Update(project);
            AddNoteIfStatusChanged(project);
            _projectRepository.CommitTransaction();

            return updatedProject;
        }

        private void CheckForDuplicateProducts(IEnumerable<PimsProduct> products, long projectId)
        {
            var duplicateProductsInArray = products.GroupBy(p => (p.Code, p.Description)).Where(g => g.Count() > 1).Select(g => g.Key);
            if (duplicateProductsInArray.Any())
            {
                throw new DuplicateEntityException($"Unable to add project with duplicated product codes: {string.Join(", ", duplicateProductsInArray.Select(dp => dp.Code))}");
            }

            IEnumerable<PimsProduct> duplicateProducts = _productRepository.GetByProductBatch(products, projectId);
            if (duplicateProducts.Any())
            {
                throw new DuplicateEntityException($"Unable to add project with duplicated product codes: {string.Join(", ", duplicateProducts.Select(dp => dp.Code))}");
            }
        }

        private async Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter, IEnumerable<short> userRegions)
        {
            return await _projectRepository.GetPageAsync(filter, userRegions);
        }

        private void AddNoteIfStatusChanged(PimsProject updatedProject)
        {
            var currentProject = _projectRepository.Get(updatedProject.Internal_Id);
            bool statusChanged = currentProject.ProjectStatusTypeCode != updatedProject.ProjectStatusTypeCode;
            if (!statusChanged)
            {
                return;
            }

            PimsProjectNote projectNoteInstance = new()
            {
                ProjectId = updatedProject.Internal_Id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = GetUpdatedNoteText(currentProject.ProjectStatusTypeCode, updatedProject.ProjectStatusTypeCode),
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _entityNoteRepository.Add(projectNoteInstance);
        }

        private string GetUpdatedNoteText(string oldStatusCode, string newStatusCode)
        {
            string newStatusDescription = string.Empty;
            string oldStatusDescription = string.Empty;
            var projectStatuses = _lookupRepository.GetAllProjectStatusTypes().ToList();

            if (newStatusCode is null)
            {
                newStatusDescription = "'No Status'";
            }
            else
            {
                newStatusDescription = projectStatuses.FirstOrDefault(x => x.ProjectStatusTypeCode == newStatusCode).Description;
            }

            if (oldStatusCode is null)
            {
                oldStatusDescription = "'No Status'";
            }
            else
            {
                oldStatusDescription = projectStatuses.FirstOrDefault(x => x.ProjectStatusTypeCode == oldStatusCode).Description;
            }

            return $"Project status changed from {oldStatusDescription} to {newStatusDescription}";
        }
    }
}
