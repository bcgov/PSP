using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Extensions;
using Pims.Core.Http.Configuration;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Comparers;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// UserService class, provides a service layer to interact with users within the datasource.
    /// </summary>
    public class UserRepository : BaseRepository<PimsUser>, IUserRepository
    {
        #region Variables
        private readonly PimsOptions _options;
        private readonly IOptionsMonitor<AuthClientOptions> _keycloakOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a UserService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public UserRepository(PimsContext dbContext, ClaimsPrincipal user, IOptionsMonitor<PimsOptions> options, IOptionsMonitor<AuthClientOptions> keycloakOptions, ILogger<UserRepository> logger)
            : base(dbContext, user, logger)
        {
            _options = options.CurrentValue;
            _keycloakOptions = keycloakOptions;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Determine if the user for the specified 'keycloakUserId' exists in the datasource.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <returns></returns>
        public bool UserExists(Guid keycloakUserId)
        {
            this.User.ThrowIfNotAuthorized();

            return this.Context.PimsUsers.Any(u => u.GuidIdentifierValue == keycloakUserId);
        }

        /// <summary>
        /// Activate the new authenticated user with the PIMS datasource.
        /// If activating a service account, then the configuration must be provided to set the default attributes.
        /// </summary>
        /// <returns></returns>
        public PimsUser Activate()
        {
            this.User.ThrowIfNotAuthorized();

            var key = this.User.GetUserKey();
            var username = this.User.GetUsername() ?? _options.ServiceAccount?.Username ??
                throw new ConfigurationException($"Configuration 'Pims:ServiceAccount:Username' is invalid or missing.");
            var user = this.Context.PimsUsers.FirstOrDefault(u => u.GuidIdentifierValue == key);
            var exists = user != null;
            if (!exists)
            {
                var givenName = this.User.GetFirstName() ?? _options.ServiceAccount?.FirstName ??
                    throw new ConfigurationException($"Configuration 'Pims:ServiceAccount:FirstName' is invalid or missing.");
                var surname = this.User.GetLastName() ?? _options.ServiceAccount?.LastName ??
                    throw new ConfigurationException($"Configuration 'Pims:ServiceAccount:LastName' is invalid or missing.");
                var email = this.User.GetEmail() ?? _options.ServiceAccount?.Email ??
                    throw new ConfigurationException($"Configuration 'Pims:ServiceAccount:Email' is invalid or missing.");
                var organization = this.User.GetOrganization(this.Context);

                this.Logger.LogInformation("User Activation: key:{key}, email:{email}, username:{username}, first:{givenName}, surname:{surname}", key, email, username, givenName, surname);

                var person = new PimsPerson() { Surname = surname, FirstName = givenName };
                this.Context.PimsPeople.Add(person);
                this.Context.CommitTransaction();

                user = new PimsUser()
                {
                    GuidIdentifierValue = key,
                    BusinessIdentifierValue = username,
                    Person = person,
                    IssueDate = DateTime.UtcNow,
                };
                this.Context.PimsUsers.Add(user);
                this.Context.CommitTransaction();

                var contactMethod = new PimsContactMethod() { Person = person, Organization = organization, ContactMethodTypeCode = ContactMethodTypes.WorkEmail, ContactMethodValue = email };
                person.PimsContactMethods.Add(contactMethod);
                this.Context.CommitTransaction();
            }
            else
            {
                user.LastLogin = DateTime.UtcNow;
                this.Context.Entry(user).State = EntityState.Modified;
                this.Context.CommitTransaction();
            }

            if (!exists)
            {
                this.Logger.LogInformation("User Activated: '{username}' - '{key}'.", username, key);
            }

            return user;
        }

        public bool ValidateClaims(PimsUser user)
        {
            var keycloakClaims = this.User.Claims.Where(c => c.Type == "client_roles").ToList();

            var userRoles = this.Context.PimsUserRoles
                .Include(ur => ur.Role)
                    .ThenInclude(r => r.PimsRoleClaims)
                    .ThenInclude(rc => rc.Claim)
                .Where(ur => ur.UserId == user.UserId)
                .ToList();

            // Keycloak claims contain both the roles as well as the claims represented in pims.
            HashSet<string> missmatched = new HashSet<string>();
            foreach (var claim in keycloakClaims)
            {
                missmatched.Add(claim.Value);
            }

            HashSet<string> pimsClaimNames = new HashSet<string>();
            foreach (PimsUserRole userRole in userRoles)
            {
                foreach (var roleClaim in userRole.Role.PimsRoleClaims)
                {
                    string claimName = roleClaim.Claim.Name;
                    pimsClaimNames.Add(claimName);
                }

                // Add role names to the list as well
                string roleName = userRole.Role.Name;
                pimsClaimNames.Add(roleName);
            }

            foreach (var claim in pimsClaimNames)
            {
                bool found = missmatched.Remove(claim);
                if (!found)
                {
                    missmatched.Add(claim);
                }
            }

            return missmatched.Count == 0;
        }

        /// <summary>
        /// Get the total number of user accounts.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.PimsUsers.Count();
        }

        /// <summary>
        /// Get a page of users from the datasource.
        /// The filter will allow queries to search for anything that starts with the following properties; DisplayName, FirstName, LastName, Email, Organizations.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Paged<PimsUser> GetPage(int page, int quantity)
        {
            return GetAllByFilter(new UserFilter(page, quantity));
        }

        /// <summary>
        /// Get a page of users from the datasource.
        /// The filter will allow queries to search for the following property values; DisplayName, FirstName, LastName, Email, Organizations.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsUser> GetAllByFilter(UserFilter filter = null)
        {
            User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.AdminUsers, _keycloakOptions);

            var query = Context.PimsUsers
                .Include(u => u.PimsUserOrganizations)
                    .ThenInclude(o => o.Organization)
                .Include(u => u.PimsUserRoles)
                    .ThenInclude(r => r.Role)
                .Include(u => u.Person)
                    .ThenInclude(p => p.PimsContactMethods)
                    .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(u => u.PimsRegionUsers)
                    .ThenInclude(ru => ru.RegionCodeNavigation)
                .Include(u => u.UserTypeCodeNavigation)
                .AsNoTracking();

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

                if (filter.Sort == null)
                {
                    filter.Sort = Array.Empty<string>();
                }

                if (!string.IsNullOrWhiteSpace(filter.BusinessIdentifierValue))
                {
                    query = query.Where(u => EF.Functions.Like(u.BusinessIdentifierValue, $"%{filter.BusinessIdentifierValue}%")
                    || EF.Functions.Like(u.Person.FirstName, $"%{filter.BusinessIdentifierValue}%")
                    || EF.Functions.Like(u.Person.Surname, $"%{filter.BusinessIdentifierValue}%"));
                }

                if (filter.Region != null)
                {
                    query = query.Where(u => u.PimsRegionUsers.Any(ru => ru.RegionCode == filter.Region));
                }

                if (!string.IsNullOrWhiteSpace(filter.Email))
                {
                    query = query.Where(u => u.Person.PimsContactMethods.Any(cm => EF.Functions.Like(cm.ContactMethodValue, $"%{filter.Email}%")));
                }

                if (filter.ActiveOnly == true)
                {
                    query = query.Where(u => u.IsDisabled == false);
                }

                if (filter.Role != null)
                {
                    query = query.Where(u => u.PimsUserRoles.Any(r => r.RoleId == filter.Role));
                }

                if (filter.Sort.Any())
                {
                    var direction = filter.Sort[0].Split(" ").LastOrDefault();
                    if (filter.Sort[0].StartsWith("Email"))
                    {
                        query = direction == "asc" ?
                            query.OrderBy(u => u.Person.PimsContactMethods.Any() ? u.Person.PimsContactMethods.FirstOrDefault().ContactMethodValue : null)
                            : query.OrderByDescending(u => u.Person.PimsContactMethods.Any() ? u.Person.PimsContactMethods.FirstOrDefault().ContactMethodValue : null);
                    }
                    else if (filter.Sort[0].StartsWith("Surname"))
                    {
                        query = direction == "asc" ?
                            query.OrderBy(u => u.Person.Surname ?? null)
                            : query.OrderByDescending(u => u.Person.Surname ?? null);
                    }
                    else if (filter.Sort[0].StartsWith("FirstName"))
                    {
                        query = direction == "asc" ?
                            query.OrderBy(u => u.Person.FirstName ?? null)
                            : query.OrderByDescending(u => u.Person.FirstName ?? null);
                    }
                    else
                    {
                        query = query.OrderByProperty(filter.Sort);
                    }
                }
            }
            var users = query.Skip((filter.Page - 1) * filter.Quantity).Take(filter.Quantity);
            return new Paged<PimsUser>(users.ToArray(), filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Get the user with the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsUser GetById(long id)
        {
            User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            return Context.PimsUsers
                .Include(u => u.PimsUserRoles)
                    .ThenInclude(r => r.Role)
                .Include(u => u.Person)
                    .ThenInclude(p => p.PimsContactMethods)
                    .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(u => u.PimsRegionUsers)
                    .ThenInclude(ru => ru.RegionCodeNavigation)
                .Include(u => u.UserTypeCodeNavigation)
                .AsNoTracking()
                .SingleOrDefault(u => u.UserId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the user with the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsUser GetTrackingById(long id)
        {
            User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            return Context.PimsUsers
                .Include(u => u.PimsUserRoles)
                    .ThenInclude(r => r.Role)
                .Include(u => u.Person)
                    .ThenInclude(p => p.PimsContactMethods)
                .Include(u => u.PimsRegionUsers)
                    .ThenInclude(ru => ru.RegionCodeNavigation)
                .Include(u => u.UserTypeCodeNavigation)
                .SingleOrDefault(u => u.UserId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the user with the specified 'keycloakUserId'.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsUser GetByKeycloakUserId(Guid keycloakUserId)
        {

            return this.Context.PimsUsers
                .Include(u => u.UserTypeCodeNavigation)
                .Include(u => u.PimsUserRoles)
                .ThenInclude(r => r.Role)
                .Include(u => u.Person)
                .ThenInclude(p => p.PimsContactMethods)
                .Include(u => u.PimsRegionUsers)
                    .ThenInclude(ru => ru.RegionCodeNavigation)
                .AsNoTracking()
                .SingleOrDefault(u => u.GuidIdentifierValue == keycloakUserId) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the specified user to the datasource.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public PimsUser Add(PimsUser add)
        {
            if (add == null)
            {
                throw new ArgumentNullException(nameof(add), "user cannot be null.");
            }

            add.IssueDate = DateTime.UtcNow;
            AddWithoutSave(add);
            this.Context.CommitTransaction();
            return GetById(add.UserId);
        }

        /// <summary>
        /// Add the specified user to the datasource.
        /// </summary>
        /// <param name="add"></param>
        public void AddWithoutSave(PimsUser add)
        {
            add.ThrowIfNull(nameof(add));
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            add.PimsUserRoles.ForEach(r => this.Context.Entry(r).State = EntityState.Added);
            add.PimsUserOrganizations.ForEach(a => this.Context.Entry(a).State = EntityState.Added);

            this.Context.PimsUsers.Add(add);
        }

        /// <summary>
        /// Updates the specified user in the datasource.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsUser Update(PimsUser update)
        {
            var user = UpdateWithoutSave(update);
            this.Context.CommitTransaction();

            return user;
        }

        public PimsUser RemoveRole(PimsUser user, long roleId)
        {
            var userRole = user.PimsUserRoles.FirstOrDefault(r => r.RoleId == roleId);
            user.PimsUserRoles.Remove(userRole);
            this.Context.PimsUserRoles.Remove(userRole);
            return user;
        }

        public ICollection<PimsUserRole> UpdateAllRolesForUser(long userId, ICollection<PimsUserRole> roles)
        {
            Context.UpdateChild<PimsUser, long, PimsUserRole, long>(u => u.PimsUserRoles, userId, roles.ToArray());
            return roles;
        }

        public ICollection<PimsRegionUser> UpdateAllRegionsForUser(long userId, ICollection<PimsRegionUser> regions)
        {
            foreach (var userRegion in regions)
            {
                var region = Context.Find<PimsRegion>(userRegion.RegionCode);
                userRegion.RegionCodeNavigation = region;
            }

            Context.UpdateChild<PimsUser, long, PimsRegionUser, long>(u => u.PimsRegionUsers, userId, regions.ToArray());
            return regions;
        }

        /// <summary>
        /// Updates the specified user in the datasource.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public PimsUser UpdateOnly(PimsUser update)
        {
            this.Context.PimsUsers.Update(update);

            return update;
        }

        /// <summary>
        /// Updates the specified user in the datasource.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsUser UpdateWithoutSave(PimsUser update)
        {
            update.ThrowIfNull(nameof(update));
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var user = this.Context.PimsUsers
                .Include(u => u.PimsUserRoles)
                .ThenInclude(r => r.Role)
                .Include(u => u.PimsUserOrganizations)
                .ThenInclude(o => o.Organization)
                .Include(u => u.Person)
                .ThenInclude(p => p.PimsContactMethods)
                .FirstOrDefault(u => u.UserId == update.UserId) ?? throw new KeyNotFoundException();

            // If the user has no organizations we assume this update is an approval.
            if (!user.PimsUserOrganizations.Any())
            {
                var key = this.User.GetUserKey();
                var approvedBy = this.Context.PimsUsers.AsNoTracking().FirstOrDefault(u => u.GuidIdentifierValue == key) ?? throw new KeyNotFoundException($"Current user principal key:'{key}' does not exist");
                user.ApprovedById = approvedBy.BusinessIdentifierValue;
                user.IssueDate = DateTime.UtcNow;
            }

            user.ConcurrencyControlNumber = update.ConcurrencyControlNumber;
            this.Context.SetOriginalConcurrencyControlNumber(user);

            var addRoles = update.PimsUserRoles.Except(user.PimsUserRoles, new UserRoleRoleIdComparer());
            addRoles.ForEach(r => user.PimsUserRoles.Add(new PimsUserRole() { UserId = user.UserId, RoleId = r.RoleId }));
            var removeRoles = user.PimsUserRoles.Except(update.PimsUserRoles, new UserRoleRoleIdComparer());
            removeRoles.ForEach(r =>
            {
                var remove = user.PimsUserRoles.FirstOrDefault(r2 => r2.RoleId == r.RoleId);
                if (remove != null)
                {
                    this.Context.Entry(remove).State = EntityState.Deleted;
                }
            });

            var addOrganizations = update.PimsUserOrganizations.Except(user.PimsUserOrganizations, new UserOrganizationOrganizationIdComparer());
            addOrganizations.ForEach(o => user.PimsUserOrganizations.Add(new PimsUserOrganization() { UserId = user.UserId, OrganizationId = o.OrganizationId, RoleId = o.RoleId }));
            var removeOrganizations = user.PimsUserOrganizations.Except(update.PimsUserOrganizations, new UserOrganizationOrganizationIdComparer());
            removeOrganizations.ForEach(o =>
            {
                var remove = user.PimsUserOrganizations.FirstOrDefault(o2 => o2.OrganizationId == o.OrganizationId && o2.RoleId == o.RoleId);
                if (remove != null)
                {
                    this.Context.Entry(remove).State = EntityState.Deleted;
                }
            });
            user.Note = update.Note;
            user.Position = update.Position;
            user.LastLogin = update.LastLogin;
            user.ExpiryDate = update.ExpiryDate;
            user.UserTypeCode = update.UserTypeCode;
            this.Context.PimsUsers.Update(user);
            return user;
        }

        /// <summary>
        /// Remove the specified user from the datasource.
        /// </summary>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <param name="delete"></param>
        public void Delete(PimsUser delete)
        {
            delete.ThrowIfNull(nameof(delete));
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var user = this.Context.PimsUsers
                .Include(u => u.PimsUserRoles)
                .ThenInclude(r => r.Role)
                .Include(u => u.PimsUserOrganizations)
                .ThenInclude(o => o.Organization)
                .FirstOrDefault(u => u.UserId == delete.UserId) ?? throw new KeyNotFoundException();

            user.ConcurrencyControlNumber = delete.ConcurrencyControlNumber;
            this.Context.SetOriginalConcurrencyControlNumber(user);

            user.PimsUserRoles.ForEach(ur => this.Context.Remove(ur));
            user.PimsUserRoles.Clear();
            user.PimsUserOrganizations.ForEach(uo => this.Context.Remove(uo));
            user.PimsUserOrganizations.Clear();

            this.Context.PimsUsers.Remove(user);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Get the user with person info with the specified 'keycloakUserId'.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsUser GetUserInfoByKeycloakUserId(Guid keycloakUserId)
        {
            return this.Context.PimsUsers
                .Include(u => u.Person)
                .Include(u => u.PimsRegionUsers)
                .Include(u => u.UserTypeCodeNavigation)
                .AsNoTracking()
                .SingleOrDefault(u => u.GuidIdentifierValue == keycloakUserId) ?? throw new KeyNotFoundException();
        }
        #endregion
    }
}
