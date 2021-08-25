using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// OrganizationService class, provides a service layer to administrate organizations within the datasource.
    /// </summary>
    public class OrganizationService : BaseService<Organization>, IOrganizationService
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OrganizationService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public OrganizationService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<OrganizationService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods

        /// <summary>
        /// Get a page of organizations from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Paged<Organization> Get(int page, int quantity)
        {
            return Get(new OrganizationFilter(page, quantity));
        }

        /// <summary>
        /// Get a page of organizations from the datasource.
        /// The filter will allow queries to search for the following property values; Name, Description, ParentId.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<Organization> Get(OrganizationFilter filter = null)
        {
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            var query = this.Context.Organizations.AsNoTracking();

            if (filter != null)
            {
                if (filter.Page < 1) filter.Page = 1;
                if (filter.Quantity < 1) filter.Quantity = 1;
                if (filter.Quantity > 50) filter.Quantity = 50;
                if (filter.Sort == null) filter.Sort = System.Array.Empty<string>();

                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(a => EF.Functions.Like(a.Name, $"%{filter.Name}%"));
                if (filter.IsDisabled != null)
                    query = query.Where(a => a.IsDisabled == filter.IsDisabled);
                if (filter.Id > 0)
                    query = query.Where(a => a.Id == filter.Id);

                if (filter.Sort.Any())
                    query = query.OrderByProperty(filter.Sort);
            }
            var organizations = query.Skip((filter.Page - 1) * filter.Quantity).Take(filter.Quantity);
            return new Paged<Organization>(organizations.ToArray(), filter.Page, filter.Quantity, query.Count());
        }


        /// <summary>
        /// Get a page of organizations from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IEnumerable<Organization> GetAll()
        {
            return this.Context.Organizations.AsNoTracking().OrderBy(p => p.Name).ToArray();
        }

        /// <summary>
        /// Get the organization for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">Organization does not exists for the specified 'id'.</exception>
        /// <returns></returns>
        public Organization Get(long id)
        {
            return this.Context.Organizations.Find(id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get all the child organizations for the specified 'parentId'.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IEnumerable<Organization> GetChildren(long parentId)
        {
            return this.Context.Organizations.Where(a => a.ParentId == parentId);
        }

        /// <summary>
        /// Add a new organization to the datasource.
        /// The returned organization will contain all users who are affected by the update.
        /// You will need to update Keycloak with this list.
        /// </summary>
        /// <param name="add"></param>
        public Organization Add(Organization add)
        {
            add.ThrowIfNull(nameof(add));

            this.Context.Organizations.Add(add);

            this.Context.CommitTransaction();

            // If the organization has been added as a sub-organization, then all users who are associated with the parent organization need to be updated.
            // Currently in PIMS we only link a user to a single organization (although the DB supports one-to-many).
            // This means that a user linked to a parent-organization will only have a single organization in the DB.
            // However throughout the solution we also give access to all sub-organizations.
            // Keycloak keeps a list of all the organizations that the user is allowed access to (parent + sub-organizations).
            // This means we need to return a list of users that need to be updated in Keycloak.
            if (add.ParentId.HasValue)
            {
                var users = this.Context.Users.AsNoTracking().Where(u => u.Organizations.Any(a => a.Id == add.ParentId)).ToArray();
                users.ForEach(u => add.Users.Add(u));
            }
            return add;
        }

        /// <summary>
        /// Updates the specified organization in the datasource.
        /// The returned organization will contain all users who are affected by the update.
        /// You will need to update Keycloak with this list.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">organization does not exist in the datasource.</exception>
        /// <returns></returns>
        public Organization Update(Organization update)
        {
            update.ThrowIfNull(nameof(update));

            var organization = this.Context.Organizations.Find(update.Id) ?? throw new KeyNotFoundException();
            var updatedUsers = new List<User>();

            // If the organization has become a sub-organization, or a parent-organization then users will need to be updated.
            // Currently in PIMS we only link a user to a single organization (although the DB supports one-to-many).
            // This means that a user linked to a parent-organization will only have a single organization in the DB.
            // However throughout the solution we also give access to all sub-organizations.
            // Keycloak keeps a list of all the organizations that the user is allowed access to (parent + sub-organizations).
            // This means we need to return a list of users that need to be updated in Keycloak.
            if (organization.ParentId.HasValue && !update.ParentId.HasValue)
            {
                // This organization has become a parent organization, all users associated with it through a parent-organization need to be removed.
                updatedUsers.AddRange(this.Context.Users.Where(u => u.Organizations.Any(a => a.Id == organization.ParentId)));
            }
            else if (!organization.ParentId.HasValue && update.ParentId.HasValue)
            {
                // This organization has become a sub-organization, all original users need their list of organizations reduced to only this organization.
                updatedUsers.AddRange(this.Context.Users.Include(u => u.Organizations).Where(u => u.Organizations.Any(a => a.Id == update.ParentId)));
            }
            else if (organization.ParentId != update.ParentId)
            {
                // Remove the sub-organization from currently linked users and add it to the users who belong to the new parent organization.
                updatedUsers.AddRange(this.Context.Users.Where(u => u.Organizations.Any(a => a.Id == organization.ParentId)));
                updatedUsers.AddRange(this.Context.Users.Where(u => u.Organizations.Any(a => a.Id == update.ParentId)));
            }

            if (organization.IsDisabled != update.IsDisabled)
            {
                if ((update.ParentId.HasValue && update.IsDisabled)
                    || (update.ParentId.HasValue && !update.IsDisabled))
                {
                    // Remove the sub-organization from users.
                    // Or add the sub-organization to users who are associated with the parent.
                    updatedUsers.AddRange(this.Context.Users.Where(u => u.Organizations.Any(a => a.Id == update.ParentId)));
                }
                else if (!update.ParentId.HasValue && update.IsDisabled)
                {
                    // Remove the organization from the users.
                    // This will result in the user not belonging to an organization.
                    var users = this.Context.Users.Include(u => u.Organizations).Where(u => u.Organizations.Any(a => a.Id == update.Id));
                    users.ForEach(u => u.Organizations.Clear());
                    updatedUsers.AddRange(users);
                    this.Context.UpdateRange(users);
                }
            }

            this.Context.Entry(organization).CurrentValues.SetValues(update);
            this.Context.SetOriginalRowVersion(organization);
            this.Context.CommitTransaction();

            return organization;
        }

        /// <summary>
        /// Remove the specified organization from the datasource.
        /// The returned organization will contain all users who are affected by the update.
        /// You will need to update Keycloak with this list.
        /// </summary>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <param name="organization"></param>
        public void Delete(Organization delete)
        {
            delete.ThrowIfNull(nameof(delete));

            var organization = this.Context.Organizations.Include(a => a.Users).FirstOrDefault(a => a.Id == delete.Id) ?? throw new KeyNotFoundException();
            var updateUsers = new List<User>();

            // Any user associated with this organization needs to be updated.
            // Currently in PIMS we only link a user to a single organization (although the DB supports one-to-many).
            // This means that a user linked to a parent-organization will only have a single organization in the DB.
            // However throughout the solution we also give access to all sub-organizations.
            // Keycloak keeps a list of all the organizations that the user is allowed access to (parent + sub-organizations).
            // This means we need to return a list of users that need to be updated in Keycloak.
            if (organization.ParentId.HasValue)
            {
                var users = this.Context.Users.Include(u => u.Organizations).Where(u => u.Organizations.Any(a => a.Id == delete.ParentId));
                updateUsers.AddRange(users);
            }
            else
            {
                var users = this.Context.Users.Include(u => u.Organizations).Where(u => u.Organizations.Any(a => a.Id == delete.Id));
                users.ForEach(u => u.Organizations.Clear());
                this.Context.Users.UpdateRange(users);
                updateUsers.AddRange(users);
            }

            organization.RowVersion = delete.RowVersion;
            this.Context.SetOriginalRowVersion(organization);
            this.Context.Organizations.Remove(organization);
            this.Context.CommitTransaction();

            // Mutate original entity.
            this.Context.Entry(delete).CurrentValues.SetValues(organization);
            updateUsers.ForEach(u =>
            {
                this.Context.Entry(u).State = EntityState.Detached;
                delete.Users.Add(u);
            });
        }
        #endregion
    }
}
