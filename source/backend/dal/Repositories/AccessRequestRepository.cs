using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class AccessRequestRepository : BaseRepository<PimsAccessRequest>, IAccessRequestRepository
    {
        #region Variables
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AccessRequestService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="accessRequest"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public AccessRequestRepository(PimsContext dbContext, System.Security.Claims.ClaimsPrincipal user, ClaimsPrincipal accessRequest, ILogger<AccessRequestRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the most recent access request that has submitted.
        /// </summary>
        /// <returns></returns>
        public PimsAccessRequest TryGet()
        {
            var key = this.User.GetUserKey();
            var accessRequest = this.Context.PimsAccessRequests
                .Include(a => a.UserTypeCodeNavigation)
                .Include(a => a.User)
                .ThenInclude(u => u.Person)
                .ThenInclude(p => p.PimsContactMethods)
                .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(x => x.User)
                .ThenInclude(u => u.UserTypeCodeNavigation)
                .Include(a => a.Role)
                .Include(a => a.UserTypeCodeNavigation)
                .Include(a => a.RegionCodeNavigation)
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
        public PimsAccessRequest GetById(long id)
        {
            var accessRequest = Context.PimsAccessRequests
                .Include(a => a.User)
                    .ThenInclude(u => u.Person)
                    .ThenInclude(p => p.PimsContactMethods)
                    .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(a => a.User)
                    .ThenInclude(u => u.UserTypeCodeNavigation)
                .Include(a => a.Role)
                .Include(a => a.UserTypeCodeNavigation)
                .Include(a => a.RegionCodeNavigation)
                .Include(a => a.PimsAccessRequestOrganizations)
                    .ThenInclude(a => a.Organization)
                .AsNoTracking()
                .OrderByDescending(a => a.AppCreateTimestamp)
                .FirstOrDefault(a => a.AccessRequestId == id) ?? throw new KeyNotFoundException();

            var isAdmin = User.HasPermission(Permissions.AdminUsers);
            var key = User.GetUserKey();
            if (!isAdmin && accessRequest.User.GuidIdentifierValue != key)
            {
                throw new NotAuthorizedException();
            }

            return accessRequest;
        }

        /// <summary>
        /// Get all the access requests that users have match the specified filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Paged access requests matching the filter criteria.</returns>
        public Paged<PimsAccessRequest> GetAll(AccessRequestFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var query = this.Context.PimsAccessRequests
                .Include(a => a.User)
                .ThenInclude(u => u.Person)
                .ThenInclude(p => p.PimsContactMethods)
                .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(a => a.Role)
                .Include(a => a.PimsAccessRequestOrganizations)
                .ThenInclude(a => a.Organization)
                .Include(a => a.AccessRequestStatusTypeCodeNavigation)
                .Include(a => a.RegionCodeNavigation)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(request => request.User != null
                && (request.User.BusinessIdentifierValue.Contains(filter.SearchText)
                || (request.User.Person != null && request.User.Person.Surname.Contains(filter.SearchText))));
            }
            if (filter.StatusType != null)
            {
                query = query.Where(request => request.AccessRequestStatusTypeCode == filter.StatusType.Id);
            }

            var accessRequests = query
                .Skip((filter.Page - 1) * filter.Quantity)
                .Take(filter.Quantity);
            return new Paged<PimsAccessRequest>(accessRequests, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Delete an access request.
        /// </summary>
        /// <param name="deleteRequest">The item to be deleted.</param>
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
            accessRequest.PimsAccessRequestOrganizations.ForEach(ao => Context.Remove(ao));

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
            var userTypeCode = addRequest.User.UserTypeCode;
            addRequest.User = this.Context.PimsUsers.FirstOrDefault(u => u.GuidIdentifierValue == key) ?? throw new KeyNotFoundException("Your account has not been activated.");
            addRequest.User.Position = position;
            addRequest.User.UserTypeCode = userTypeCode;
            addRequest.UserTypeCode = userTypeCode;
            addRequest.UserId = addRequest.User.UserId;
            addRequest.AccessRequestStatusTypeCodeNavigation = this.Context.PimsAccessRequestStatusTypes.FirstOrDefault(a => a.AccessRequestStatusTypeCode == "RECEIVED");

            this.Context.PimsAccessRequests.Add(addRequest);
            this.Context.CommitTransaction();
            return GetById(addRequest.AccessRequestId);
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
            var userTypeCode = updateRequest.User.UserTypeCode;
            if (!isAdmin && updateRequest.User.GuidIdentifierValue != key)
            {
                throw new NotAuthorizedException(); // Not allowed to update someone elses request.
            }

            // fetch the existing request from the datasource.
            var accessRequest = this.Context.PimsAccessRequests
                .Include(a => a.User)
                .ThenInclude(u => u.Person)
                .ThenInclude(p => p.PimsContactMethods)
                .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(a => a.Role)
                .Include(a => a.UserTypeCodeNavigation)
                .Include(a => a.RegionCodeNavigation)
                .Include(a => a.PimsAccessRequestOrganizations)
                .ThenInclude(a => a.Organization)
                .FirstOrDefault(a => a.AccessRequestId == updateRequest.AccessRequestId) ?? throw new KeyNotFoundException();

            // Copy values into entity.
            accessRequest.RoleId = updateRequest.RoleId;
            accessRequest.ConcurrencyControlNumber = updateRequest.ConcurrencyControlNumber;
            accessRequest.Note = updateRequest.Note;
            accessRequest.AccessRequestStatusTypeCode = updateRequest.AccessRequestStatusTypeCode;
            accessRequest.RegionCode = updateRequest.RegionCode;
            accessRequest.User.Position = position;
            accessRequest.UserTypeCode = userTypeCode;
            accessRequest.User.UserTypeCode = userTypeCode;

            this.Context.PimsAccessRequests.Update(accessRequest);
            this.Context.CommitTransaction();
            return GetById(updateRequest.AccessRequestId);
        }
        #endregion
    }
}
