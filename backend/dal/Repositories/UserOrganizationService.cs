using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// OrganizationService class, provides a service layer to administrate organizations within the datasource.
    /// </summary>
    public class UserOrganizationService : BaseRepository<PimsOrganization>, IUserOrganizationService
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
        public UserOrganizationService(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<UserOrganizationService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods

        /// <summary>
        /// Get a page of organizations from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Paged<PimsOrganization> Get(int page, int quantity)
        {
            return Get(new OrganizationFilter(page, quantity));
        }

        /// <summary>
        /// Get a page of organizations from the datasource.
        /// The filter will allow queries to search for the following property values; Name, Description, ParentId.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsOrganization> Get(OrganizationFilter filter = null)
        {
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            var query = this.Context.PimsOrganizations.AsNoTracking();

            if (filter != null)
            {
                if (filter.Page < 1)
                {
                    filter.Page = 1;
                }

                if (filter.Quantity < 1)
                {
                    filter.Quantity = 1;
                }

                if (filter.Quantity > 50)
                {
                    filter.Quantity = 50;
                }

                if (filter.Sort == null)
                {
                    filter.Sort = System.Array.Empty<string>();
                }

                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    query = query.Where(a => EF.Functions.Like(a.OrganizationName, $"%{filter.Name}%"));
                }

                if (filter.IsDisabled != null)
                {
                    query = query.Where(a => a.IsDisabled == filter.IsDisabled);
                }

                if (filter.Id > 0)
                {
                    query = query.Where(a => a.OrganizationId == filter.Id);
                }

                if (filter.Sort.Any())
                {
                    query = query.OrderByProperty(filter.Sort);
                }
            }
            var organizations = query.Skip((filter.Page - 1) * filter.Quantity).Take(filter.Quantity);
            return new Paged<PimsOrganization>(organizations.ToArray(), filter.Page, filter.Quantity, query.Count());
        }


        /// <summary>
        /// Get a page of organizations from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IEnumerable<PimsOrganization> GetAll()
        {
            return this.Context.PimsOrganizations.AsNoTracking().OrderBy(p => p.OrganizationName).ToArray();
        }

        /// <summary>
        /// Get the organization for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">Organization does not exists for the specified 'id'.</exception>
        /// <returns></returns>
        public PimsOrganization Get(long id)
        {
            return this.Context.PimsOrganizations.Find(id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get all the child organizations for the specified 'parentId'.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IEnumerable<PimsOrganization> GetChildren(long parentId)
        {
            return this.Context.PimsOrganizations.Where(a => a.PrntOrganizationId == parentId);
        }

        /// <summary>
        /// Add a new organization to the datasource.
        /// The returned organization will contain all users who are affected by the update.
        /// You will need to update Keycloak with this list.
        /// </summary>
        /// <param name="add"></param>
        public PimsOrganization Add(PimsOrganization add)
        {
            add.ThrowIfNull(nameof(add));

            this.Context.PimsOrganizations.Add(add);

            this.Context.CommitTransaction();

            // If the organization has been added as a sub-organization, then all users who are associated with the parent organization need to be updated.
            // Currently in PIMS we only link a user to a single organization (although the DB supports one-to-many).
            // This means that a user linked to a parent-organization will only have a single organization in the DB.
            // However throughout the solution we also give access to all sub-organizations.
            // Keycloak keeps a list of all the organizations that the user is allowed access to (parent + sub-organizations).
            // This means we need to return a list of users that need to be updated in Keycloak.
            if (add.PrntOrganizationId.HasValue)
            {
                var users = this.Context.PimsUsers.AsNoTracking().Where(u => u.GetOrganizations().Any(a => a.Id == add.PrntOrganizationId)).ToArray();
                users.ForEach(u => add.PimsUserOrganizations.Add(new PimsUserOrganization() { UserId = u.Id, OrganizationId = add.PrntOrganizationId.Value }));
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
        public PimsOrganization Update(PimsOrganization update)
        {
            update.ThrowIfNull(nameof(update));

            var organization = this.Context.PimsOrganizations.Find(update.OrganizationId) ?? throw new KeyNotFoundException();
            var updatedUsers = new List<PimsUser>();

            // If the organization has become a sub-organization, or a parent-organization then users will need to be updated.
            // Currently in PIMS we only link a user to a single organization (although the DB supports one-to-many).
            // This means that a user linked to a parent-organization will only have a single organization in the DB.
            // However throughout the solution we also give access to all sub-organizations.
            // Keycloak keeps a list of all the organizations that the user is allowed access to (parent + sub-organizations).
            // This means we need to return a list of users that need to be updated in Keycloak.
            if (organization.PrntOrganizationId.HasValue && !update.PrntOrganizationId.HasValue)
            {
                // This organization has become a parent organization, all users associated with it through a parent-organization need to be removed.
                updatedUsers.AddRange(this.Context.PimsUsers.Where(u => u.GetOrganizations().Any(a => a.Id == organization.PrntOrganizationId)));
            }
            else if (!organization.PrntOrganizationId.HasValue && update.PrntOrganizationId.HasValue)
            {
                // This organization has become a sub-organization, all original users need their list of organizations reduced to only this organization.
                updatedUsers.AddRange(this.Context.PimsUsers.Include(u => u.GetOrganizations()).Where(u => u.GetOrganizations().Any(a => a.Id == update.PrntOrganizationId)));
            }
            else if (organization.PrntOrganizationId != update.PrntOrganizationId)
            {
                // Remove the sub-organization from currently linked users and add it to the users who belong to the new parent organization.
                updatedUsers.AddRange(this.Context.PimsUsers.Where(u => u.GetOrganizations().Any(a => a.Id == organization.PrntOrganizationId)));
                updatedUsers.AddRange(this.Context.PimsUsers.Where(u => u.GetOrganizations().Any(a => a.Id == update.PrntOrganizationId)));
            }

            if (organization.IsDisabled != update.IsDisabled)
            {
                if ((update.PrntOrganizationId.HasValue && update.IsDisabled.HasValue && update.IsDisabled.Value)
                    || (update.PrntOrganizationId.HasValue && update.IsDisabled.HasValue && !update.IsDisabled.Value))
                {
                    // Remove the sub-organization from users.
                    // Or add the sub-organization to users who are associated with the parent.
                    updatedUsers.AddRange(this.Context.PimsUsers.Where(u => u.GetOrganizations().Any(a => a.Id == update.PrntOrganizationId)));
                }
                else if (!update.PrntOrganizationId.HasValue && update.IsDisabled.HasValue && update.IsDisabled.Value)
                {
                    // Remove the organization from the users.
                    // This will result in the user not belonging to an organization.
                    var users = this.Context.PimsUsers.Include(u => u.PimsUserOrganizations).Where(u => u.PimsUserOrganizations.Any(a => a.OrganizationId == update.OrganizationId));
                    users.ForEach(u => u.PimsUserOrganizations.Clear());
                    updatedUsers.AddRange(users);
                    this.Context.UpdateRange(users);
                }
            }

            this.Context.Entry(organization).CurrentValues.SetValues(update);
            this.Context.SetOriginalConcurrencyControlNumber(organization);
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
        public void Delete(PimsOrganization delete)
        {
            delete.ThrowIfNull(nameof(delete));

            var organization = this.Context.PimsOrganizations.Include(a => a.PimsUserOrganizations).FirstOrDefault(a => a.OrganizationId == delete.OrganizationId) ?? throw new KeyNotFoundException();
            var updateUsers = new List<PimsUser>();

            // Any user associated with this organization needs to be updated.
            // Currently in PIMS we only link a user to a single organization (although the DB supports one-to-many).
            // This means that a user linked to a parent-organization will only have a single organization in the DB.
            // However throughout the solution we also give access to all sub-organizations.
            // Keycloak keeps a list of all the organizations that the user is allowed access to (parent + sub-organizations).
            // This means we need to return a list of users that need to be updated in Keycloak.
            if (organization.PrntOrganizationId.HasValue)
            {
                var users = this.Context.PimsUsers.Include(u => u.PimsUserOrganizations).Where(u => u.PimsUserOrganizations.Any(a => a.OrganizationId == delete.PrntOrganizationId));
                updateUsers.AddRange(users);
            }
            else
            {
                var users = this.Context.PimsUsers.Include(u => u.PimsUserOrganizations).Where(u => u.PimsUserOrganizations.Any(a => a.OrganizationId == delete.OrganizationId));
                users.ForEach(u => u.PimsUserOrganizations.Clear());
                this.Context.PimsUsers.UpdateRange(users);
                updateUsers.AddRange(users);
            }

            organization.ConcurrencyControlNumber = delete.ConcurrencyControlNumber;
            this.Context.SetOriginalConcurrencyControlNumber(organization);
            this.Context.PimsOrganizations.Remove(organization);
            this.Context.CommitTransaction();

            // Mutate original entity.
            this.Context.Entry(delete).CurrentValues.SetValues(organization);
            updateUsers.ForEach(u =>
            {
                this.Context.Entry(u).State = EntityState.Detached;
                delete.PimsUserOrganizations.Add(new PimsUserOrganization() { UserId = u.UserId, User = u, OrganizationId = delete.OrganizationId });
            });
        }
        #endregion
    }
}
