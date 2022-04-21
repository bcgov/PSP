using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// AccessRequestService class, provides a service layer to interact with accessRequests within the datasource.
    /// </summary>
    public class AccessRequestService : BaseRepository<PimsAccessRequest>, IAccessRequestService
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
        public AccessRequestService(PimsContext dbContext, System.Security.Claims.ClaimsPrincipal user, ClaimsPrincipal accessRequest, IPimsRepository service, ILogger<AccessRequestService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get the most recent access request that has submitted.
        /// </summary>
        /// <returns></returns>
        public PimsAccessRequest Get()
        {
            var key = this.User.GetUserKey();
            var accessRequest = this.Context.PimsAccessRequests
                .Include(a => a.User)
                .Include(a => a.Role)
                .Include(a => a.PimsAccessRequestOrganizations)
                .ThenInclude(a => a.Organization)
                .AsNoTracking()
                .OrderByDescending(a => a.AppCreateTimestamp)
                .FirstOrDefault(a => a.User.GuidIdentifierValue == key);
            return accessRequest;
        }

        /// <summary>
        /// Get the access request for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsAccessRequest Get(long id)
        {
            var accessRequest = this.Context.PimsAccessRequests
                .Include(a => a.User)
                .Include(a => a.Role)
                .Include(a => a.PimsAccessRequestOrganizations)
                .ThenInclude(a => a.Organization)
                .AsNoTracking()
                .OrderByDescending(a => a.AppCreateTimestamp)
                .FirstOrDefault(a => a.AccessRequestId == id) ?? throw new KeyNotFoundException();

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            if (!isAdmin && accessRequest.User.GuidIdentifierValue != key)
            {
                throw new NotAuthorizedException();
            }

            return accessRequest;
        }

        /// <summary>
        /// Get all the access requests that users have match the specified filter
        /// </summary>
        /// <param name="filter"></param>
        public Paged<PimsAccessRequest> Get(AccessRequestFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var query = this.Context.PimsAccessRequests
                .Include(a => a.User)
                .ThenInclude(u => u.Person)
                .ThenInclude(p => p.PimsContactMethods)
                .Include(a => a.Role)
                .Include(a => a.PimsAccessRequestOrganizations)
                .ThenInclude(a => a.Organization)
                .Include(a => a.AccessRequestStatusTypeCodeNavigation)
                .AsNoTracking();

            var userOrganizations = this.User.GetOrganizations();
            if (userOrganizations != null && User.HasPermission(Permissions.OrganizationAdmin) && !User.HasPermission(Permissions.SystemAdmin))
            {
                query = query.Where(accessRequest => accessRequest.PimsAccessRequestOrganizations.Any(a => a.OrganizationId.HasValue && userOrganizations.Contains(a.OrganizationId.Value)));
            }

            if (!String.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(request => request.AccessRequestStatusTypeCode == filter.Status);
            }

            if (!string.IsNullOrWhiteSpace(filter.Role))
            {
                query = query.Where(ar => EF.Functions.Like(ar.Role.Name, $"%{filter.Role}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Organization))
            {
                query = query.Where(ar => ar.PimsAccessRequestOrganizations.Any(a => EF.Functions.Like(a.Organization.OrganizationName, $"%{filter.Organization}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                query = query.Where(ar => EF.Functions.Like(ar.User.BusinessIdentifierValue, $"%{filter.Username}%"));
            }

            var accessRequests = query
                .Skip((filter.Page - 1) * filter.Quantity)
                .Take(filter.Quantity);
            return new Paged<PimsAccessRequest>(accessRequests, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Delete an access request
        /// </summary>
        /// <param name="deleteRequest">The item to be deleted</param>
        /// <returns></returns>
        public PimsAccessRequest Delete(PimsAccessRequest deleteRequest)
        {
            var accessRequest = Context.PimsAccessRequests.Include(a => a.PimsAccessRequestOrganizations).ThenInclude(a => a.Organization).FirstOrDefault(a => a.AccessRequestId == deleteRequest.AccessRequestId) ?? throw new KeyNotFoundException();

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            if (!isAdmin && accessRequest.User.GuidIdentifierValue != key)
            {
                throw new NotAuthorizedException();
            }

            accessRequest.ConcurrencyControlNumber = deleteRequest.ConcurrencyControlNumber;
            Context.PimsAccessRequests.Remove(accessRequest);
            Context.CommitTransaction();

            return deleteRequest;
        }

        /// <summary>
        /// Add a new access request for the current accessRequest.
        /// </summary>
        /// <param name="addRequest"></param>
        /// <returns></returns>
        public PimsAccessRequest Add(PimsAccessRequest addRequest)
        {
            if (addRequest == null)
            {
                throw new ArgumentNullException(nameof(addRequest));
            }

            var key = this.User.GetUserKey();
            var position = addRequest.User.Position;
            addRequest.User = this.Context.PimsUsers.FirstOrDefault(u => u.GuidIdentifierValue == key) ?? throw new KeyNotFoundException("Your account has not been activated.");
            addRequest.User.Position = position;
            addRequest.UserId = addRequest.User.UserId;
            addRequest.AccessRequestStatusTypeCodeNavigation = this.Context.PimsAccessRequestStatusTypes.FirstOrDefault(a => a.AccessRequestStatusTypeCode == "RECEIVED");

            this.Context.PimsAccessRequests.Add(addRequest);
            this.Context.CommitTransaction();
            return addRequest;
        }

        /// <summary>
        /// Update the access request for the current accessRequest.
        /// </summary>
        /// <param name="updateRequest"></param>
        /// <returns></returns>
        public PimsAccessRequest Update(PimsAccessRequest updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }

            var isAdmin = this.User.HasPermission(Permissions.AdminUsers);
            var key = this.User.GetUserKey();
            var position = updateRequest.User.Position;
            updateRequest.User = this.Context.PimsUsers.FirstOrDefault(u => u.GuidIdentifierValue == key) ?? throw new KeyNotFoundException("Your account has not been activated.");
            updateRequest.User.Position = position;
            if (!isAdmin && updateRequest.User.GuidIdentifierValue != key)
            {
                throw new NotAuthorizedException(); // Not allowed to update someone elses request.
            }

            // fetch the existing request from the datasource.
            var accessRequest = this.Context.PimsAccessRequests
                .Include(a => a.User)
                .Include(a => a.Role)
                .Include(a => a.PimsAccessRequestOrganizations)
                .ThenInclude(a => a.Organization)
                .FirstOrDefault(a => a.AccessRequestId == updateRequest.AccessRequestId) ?? throw new KeyNotFoundException();

            // Copy values into entity.
            accessRequest.RoleId = updateRequest.RoleId;
            accessRequest.ConcurrencyControlNumber = updateRequest.ConcurrencyControlNumber;
            accessRequest.Note = updateRequest.Note;
            accessRequest.AccessRequestStatusTypeCode = updateRequest.AccessRequestStatusTypeCode;

            this.Context.PimsAccessRequests.Update(accessRequest);
            this.Context.CommitTransaction();
            return Get(updateRequest.AccessRequestId);
        }
        #endregion
    }
}
