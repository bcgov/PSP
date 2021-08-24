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
        /// <param name="delete">The item to be deleted</param>
        /// <returns></returns>
        public AccessRequest Delete(AccessRequest delete)
        {
            var accessRequest = Context.AccessRequests.Include(a => a.Organizations).FirstOrDefault(a => a.Id == delete.Id) ?? throw new KeyNotFoundException();

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            if (!isAdmin && accessRequest.User.KeycloakUserId != key) throw new NotAuthorizedException();

            accessRequest.RowVersion = delete.RowVersion;
            Context.AccessRequests.Remove(accessRequest);
            Context.CommitTransaction();

            return delete;
        }

        /// <summary>
        /// Add a new access request for the current accessRequest.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public AccessRequest Add(AccessRequest add)
        {
            if (add == null) throw new ArgumentNullException(nameof(add));
            if (add.OrganizationsManyToMany?.Any() == false) throw new ArgumentException($"Argument '{nameof(add)}.{nameof(add.OrganizationsManyToMany)}' is required.", nameof(add));
            var organizations = add.OrganizationsManyToMany.Select(o => new AccessRequestOrganization() { OrganizationId = o.OrganizationId }).ToList();
            add.OrganizationsManyToMany.Clear();

            var key = this.User.GetUserKey();
            var position = add.User.Position;
            add.User = this.Context.Users.FirstOrDefault(u => u.KeycloakUserId == key) ?? throw new KeyNotFoundException("Your account has not been activated.");
            add.User.Position = position;
            add.UserId = add.User.Id;
            add.Status = this.Context.AccessRequestStatusTypes.FirstOrDefault(a => a.Id == "RECEIVED");

            // TODO: PIMS_ACRQOR_I_S_I_TR causes the insert to fail, likely due to the trigger running at an innapropriate time when the generated sql is running.
            // adding the access request and then adding the organizations after resolves this issue.
            this.Context.AccessRequests.Add(add);
            this.Context.CommitTransaction();
            organizations.ForEach(o => add.OrganizationsManyToMany.Add(new AccessRequestOrganization() { OrganizationId = o.OrganizationId, AccessRequestId = add.Id }));
            this.Context.CommitTransaction();
            return add;
        }

        /// <summary>
        /// Update the access request for the current accessRequest.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public AccessRequest Update(AccessRequest update)
        {
            if (update == null) throw new ArgumentNullException(nameof(update));
            if (update.OrganizationsManyToMany?.Any() == false) throw new ArgumentException($"Argument '{nameof(update)}.{nameof(update.OrganizationsManyToMany)}' is required.", nameof(update));

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            var position = update.User.Position;
            update.User = this.Context.Users.FirstOrDefault(u => u.KeycloakUserId == key) ?? throw new KeyNotFoundException("Your account has not been activated.");
            update.User.Position = position;
            if (!isAdmin && update.User.KeycloakUserId != key) throw new NotAuthorizedException(); // Not allowed to update someone elses request.

            // fetch the existing request from the datasource.
            var accessRequest = this.Context.AccessRequests
                .Include(a => a.User)
                .Include(a => a.Role)
                .Include(a => a.Organizations)
                .FirstOrDefault(a => a.Id == update.Id) ?? throw new KeyNotFoundException();

            // Remove organizations and roles if required.
            var removeOrganizations = accessRequest.OrganizationsManyToMany.Except(update.OrganizationsManyToMany, new AccessRequestOrganizationOrganizationIdComparer());
            if (removeOrganizations.Any()) accessRequest.OrganizationsManyToMany.RemoveAll(a => removeOrganizations.Any(r => r.OrganizationId == a.OrganizationId));

            // Add organizations and roles if required.
            var addOrganizations = update.OrganizationsManyToMany.Except(accessRequest.OrganizationsManyToMany, new AccessRequestOrganizationOrganizationIdComparer());
            addOrganizations.ForEach(a => accessRequest.OrganizationsManyToMany.Add(a));

            // Copy values into entity.
            accessRequest.RoleId = update.RoleId;
            accessRequest.RowVersion = update.RowVersion;
            accessRequest.Note = update.Note;
            accessRequest.StatusId = update.StatusId;

            this.Context.AccessRequests.Update(accessRequest);
            this.Context.CommitTransaction();
            return Get(update.Id);
        }
        #endregion
    }
}
