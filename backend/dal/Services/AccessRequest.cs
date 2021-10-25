using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Comparers;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// AccessRequestService class, provides a service layer to interact with accessRequests within the datasource.
    /// </summary>
    public class AccessRequestService : BaseService<AccessRequest>, IAccessRequestService
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a AccessRequestService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="accessRequest"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public AccessRequestService(PimsContext dbContext, ClaimsPrincipal accessRequest, IPimsService service, ILogger<AccessRequestService> logger) : base(dbContext, accessRequest, service, logger)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the most recent access request that has submitted.
        /// </summary>
        /// <returns></returns>
        public AccessRequest Get()
        {
            var key = this.User.GetUserKey();
            var accessRequest = this.Context.AccessRequests
                .Include(a => a.User)
                .Include(a => a.Role)
                .Include(a => a.Organizations)
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedOn)
                .FirstOrDefault(a => a.User.KeycloakUserId == key);
            return accessRequest;
        }

        /// <summary>
        /// Get the access request for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccessRequest Get(long id)
        {
            var accessRequest = this.Context.AccessRequests
                .Include(a => a.User)
                .Include(a => a.Role)
                .Include(a => a.Organizations)
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedOn)
                .FirstOrDefault(a => a.Id == id) ?? throw new KeyNotFoundException();

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            if (!isAdmin && accessRequest.User.KeycloakUserId != key) throw new NotAuthorizedException();
            return accessRequest;
        }

        /// <summary>
        /// Get all the access requests that users have match the specified filter
        /// </summary>
        /// <param name="filter"></param>
        public Paged<AccessRequest> Get(AccessRequestFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var query = this.Context.AccessRequests
                .Include(a => a.User)
                .ThenInclude(u => u.Person)
                .ThenInclude(p => p.ContactMethods)
                .Include(a => a.Role)
                .Include(a => a.Organizations)
                .Include(a => a.Status)
                .AsNoTracking();

            var userOrganizations = this.User.GetOrganizations();
            if (userOrganizations != null && User.HasPermission(Permissions.OrganizationAdmin) && !User.HasPermission(Permissions.SystemAdmin))
                query = query.Where(accessRequest => accessRequest.Organizations.Any(a => userOrganizations.Contains(a.Id)));

            if (!String.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(request => request.StatusId == filter.Status);

            if (!string.IsNullOrWhiteSpace(filter.Role))
                query = query.Where(ar => EF.Functions.Like(ar.Role.Name, $"%{filter.Role}%"));

            if (!string.IsNullOrWhiteSpace(filter.Organization))
                query = query.Where(ar => ar.Organizations.Any(a => EF.Functions.Like(a.Name, $"%{filter.Organization}%")));

            if (!string.IsNullOrWhiteSpace(filter.Username))
                query = query.Where(ar => EF.Functions.Like(ar.User.BusinessIdentifier, $"%{filter.Username}%"));

            var accessRequests = query
                .Skip((filter.Page - 1) * filter.Quantity)
                .Take(filter.Quantity);
            return new Paged<AccessRequest>(accessRequests, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Delete an access request
        /// </summary>
        /// <param name="deleteRequest">The item to be deleted</param>
        /// <returns></returns>
        public AccessRequest Delete(AccessRequest deleteRequest)
        {
            var accessRequest = Context.AccessRequests.Include(a => a.Organizations).FirstOrDefault(a => a.Id == deleteRequest.Id) ?? throw new KeyNotFoundException();

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            if (!isAdmin && accessRequest.User.KeycloakUserId != key) throw new NotAuthorizedException();

            accessRequest.RowVersion = deleteRequest.RowVersion;
            Context.AccessRequests.Remove(accessRequest);
            Context.CommitTransaction();

            return deleteRequest;
        }

        /// <summary>
        /// Add a new access request for the current accessRequest.
        /// </summary>
        /// <param name="addRequest"></param>
        /// <returns></returns>
        public AccessRequest Add(AccessRequest addRequest)
        {
            if (addRequest == null) throw new ArgumentNullException(nameof(addRequest));
            if (addRequest.OrganizationsManyToMany?.Any() == false) throw new ArgumentException($"Argument '{nameof(addRequest)}.{nameof(addRequest.OrganizationsManyToMany)}' is required.", nameof(addRequest));
            var organizations = addRequest.OrganizationsManyToMany.Select(o => new AccessRequestOrganization() { OrganizationId = o.OrganizationId }).ToList();
            addRequest.OrganizationsManyToMany.Clear();

            var key = this.User.GetUserKey();
            var position = addRequest.User.Position;
            addRequest.User = this.Context.Users.FirstOrDefault(u => u.KeycloakUserId == key) ?? throw new KeyNotFoundException("Your account has not been activated.");
            addRequest.User.Position = position;
            addRequest.UserId = addRequest.User.Id;
            addRequest.Status = this.Context.AccessRequestStatusTypes.FirstOrDefault(a => a.Id == "RECEIVED");

            // TODO: PIMS_ACRQOR_I_S_I_TR causes the insert to fail, likely due to the trigger running at an innapropriate time when the generated sql is running.
            // adding the access request and then adding the organizations after resolves this issue.
            this.Context.AccessRequests.Add(addRequest);
            this.Context.CommitTransaction();
            organizations.ForEach(o => addRequest.OrganizationsManyToMany.Add(new AccessRequestOrganization() { OrganizationId = o.OrganizationId, AccessRequestId = addRequest.Id }));
            this.Context.CommitTransaction();
            return addRequest;
        }

        /// <summary>
        /// Update the access request for the current accessRequest.
        /// </summary>
        /// <param name="updateRequest"></param>
        /// <returns></returns>
        public AccessRequest Update(AccessRequest updateRequest)
        {
            if (updateRequest == null) throw new ArgumentNullException(nameof(updateRequest));
            if (updateRequest.OrganizationsManyToMany?.Any() == false) throw new ArgumentException($"Argument '{nameof(updateRequest)}.{nameof(updateRequest.OrganizationsManyToMany)}' is required.", nameof(updateRequest));

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            var position = updateRequest.User.Position;
            updateRequest.User = this.Context.Users.FirstOrDefault(u => u.KeycloakUserId == key) ?? throw new KeyNotFoundException("Your account has not been activated.");
            updateRequest.User.Position = position;
            if (!isAdmin && updateRequest.User.KeycloakUserId != key) throw new NotAuthorizedException(); // Not allowed to update someone elses request.

            // fetch the existing request from the datasource.
            var accessRequest = this.Context.AccessRequests
                .Include(a => a.User)
                .Include(a => a.Role)
                .Include(a => a.Organizations)
                .FirstOrDefault(a => a.Id == updateRequest.Id) ?? throw new KeyNotFoundException();

            // Remove organizations and roles if required.
            var removeOrganizations = accessRequest.OrganizationsManyToMany.Except(updateRequest.OrganizationsManyToMany, new AccessRequestOrganizationOrganizationIdComparer());
            if (removeOrganizations.Any()) accessRequest.OrganizationsManyToMany.RemoveAll(a => removeOrganizations.Any(r => r.OrganizationId == a.OrganizationId));

            // Add organizations and roles if required.
            var addOrganizations = updateRequest.OrganizationsManyToMany.Except(accessRequest.OrganizationsManyToMany, new AccessRequestOrganizationOrganizationIdComparer());
            addOrganizations.ForEach(a => accessRequest.OrganizationsManyToMany.Add(a));

            // Copy values into entity.
            accessRequest.RoleId = updateRequest.RoleId;
            accessRequest.RowVersion = updateRequest.RowVersion;
            accessRequest.Note = updateRequest.Note;
            accessRequest.StatusId = updateRequest.StatusId;

            this.Context.AccessRequests.Update(accessRequest);
            this.Context.CommitTransaction();
            return Get(updateRequest.Id);
        }
        #endregion
    }
}
