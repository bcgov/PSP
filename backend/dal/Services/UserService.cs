using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    /// UserService class, provides a service layer to interact with users within the datasource.
    /// </summary>
    public class UserService : BaseService<User>, IUserService
    {
        #region Variables
        private readonly PimsOptions _options;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public UserService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, IOptionsMonitor<PimsOptions> options, ILogger<UserService> logger) : base(dbContext, user, service, logger)
        {
            _options = options.CurrentValue;
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

            return this.Context.Users.Any(u => u.KeycloakUserId == keycloakUserId);
        }

        /// <summary>
        /// Activate the new authenticated user with the PIMS datasource.
        /// If activating a service account, then the configuration must be provided to set the default attributes.
        /// </summary>
        /// <returns></returns>
        public User Activate()
        {
            this.User.ThrowIfNotAuthorized();

            var key = this.User.GetUserKey();
            var username = this.User.GetUsername() ?? _options.ServiceAccount?.Username ??
                throw new ConfigurationException($"Configuration 'Pims:ServiceAccount:Username' is invalid or missing.");
            var user = this.Context.Users.FirstOrDefault(u => u.KeycloakUserId == key);
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

                this.Logger.LogInformation($"User Activation: key:{key}, email:{email}, username:{username}, first:{givenName}, surname:{surname}");

                var person = new Person(surname, givenName);
                this.Context.Persons.Add(person);
                this.Context.CommitTransaction();

                user = new User(key, username, person);
                user.IssueOn = DateTime.UtcNow;
                this.Context.Users.Add(user);
                this.Context.CommitTransaction();

                var contactMethod = new ContactMethod(person, organization, ContactMethodTypes.WorkEmail, email);
                person.ContactMethods.Add(contactMethod);
                this.Context.CommitTransaction();
            } else
            {
                user.LastLogin = DateTime.UtcNow;
                this.Context.Entry(user).State = EntityState.Modified;
                this.Context.CommitTransaction();
            }

            if (!exists) this.Logger.LogInformation($"User Activated: '{username}' - '{key}'.");
            return user;
        }
        /// <summary>
        /// Get the total number of user accounts.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.Users.Count();
        }

        /// <summary>
        /// Get a page of users from the datasource.
        /// The filter will allow queries to search for anything that starts with the following properties; DisplayName, FirstName, LastName, Email, Organizations.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Paged<User> Get(int page, int quantity)
        {
            return Get(new UserFilter(page, quantity));
        }

        /// <summary>
        /// Get a page of users from the datasource.
        /// The filter will allow queries to search for the following property values; DisplayName, FirstName, LastName, Email, Organizations.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<User> Get(UserFilter filter = null)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var query = this.Context.Users
                .Include(u => u.OrganizationsManyToMany)
                .ThenInclude(o => o.Organization)
                .Include(u => u.Roles)
                .Include(u => u.Person)
                .ThenInclude(p => p.ContactMethods)
                .AsNoTracking();

            if (User.HasPermission(Permissions.OrganizationAdmin) && !User.HasPermission(Permissions.SystemAdmin))
            {
                var userOrganizations = this.User.GetOrganizations();
                query = query.Where(user => user.Organizations.Any(o => userOrganizations.Contains(o.Id)));
            }

            if (filter != null)
            {
                if (filter.Page < 1) filter.Page = 1;
                if (filter.Quantity < 1) filter.Quantity = 1;
                if (filter.Quantity > 50) filter.Quantity = 50;
                if (filter.Sort == null) filter.Sort = Array.Empty<string>();

                if (!string.IsNullOrWhiteSpace(filter.Username))
                    query = query.Where(u => EF.Functions.Like(u.BusinessIdentifier, $"%{filter.Username}%"));
                if (!string.IsNullOrWhiteSpace(filter.FirstName))
                    query = query.Where(u => EF.Functions.Like(u.Person.FirstName, $"%{filter.FirstName}%"));
                if (!string.IsNullOrWhiteSpace(filter.LastName))
                    query = query.Where(u => EF.Functions.Like(u.Person.Surname, $"%{filter.LastName}%"));
                if (!string.IsNullOrWhiteSpace(filter.Email))
                    query = query.Where(u => u.Person.ContactMethods.Any(cm => EF.Functions.Like(cm.Value, $"%{filter.Email}%")));
                if (filter.IsDisabled != null)
                    query = query.Where(u => u.IsDisabled == filter.IsDisabled);
                if (!string.IsNullOrWhiteSpace(filter.Role))
                    query = query.Where(u => u.Roles.Any(r =>
                        EF.Functions.Like(r.Name, $"%{filter.Role}")));
                if (!string.IsNullOrWhiteSpace(filter.Organization))
                    query = query.Where(u => u.Organizations.Any(a =>
                        EF.Functions.Like(a.Name, $"%{filter.Organization}")));

                if (filter.Sort.Any())
                {
                    var direction = filter.Sort[0].Split(" ").FirstOrDefault();
                    if (filter.Sort[0].StartsWith("Organization"))
                    {
                        query = direction == "asc" ?
                            query.OrderBy(u => u.Organizations.Any() ? u.Organizations.FirstOrDefault().Name : null)
                            : query.OrderByDescending(u => u.Organizations.Any() ? u.Organizations.FirstOrDefault().Name : null);
                    }
                    else if (filter.Sort[0].StartsWith("Email"))
                    {
                        query = direction == "asc" ?
                            query.OrderBy(u => u.Person.ContactMethods.Any() ? u.Person.ContactMethods.FirstOrDefault().Value : null)
                            : query.OrderByDescending(u => u.Person.ContactMethods.Any() ? u.Person.ContactMethods.FirstOrDefault().Value : null);
                    }
                    else if (filter.Sort[0].StartsWith("SurName"))
                    {
                        query = direction == "asc" ?
                            query.OrderBy(u => u.Person.Surname != null ? u.Person.Surname : null)
                            : query.OrderByDescending(u => u.Person.Surname != null ? u.Person.Surname : null);
                    }
                    else if (filter.Sort[0].StartsWith("FirstName"))
                    {
                        query = direction == "asc" ?
                            query.OrderBy(u => u.Person.FirstName != null ? u.Person.FirstName : null)
                            : query.OrderByDescending(u => u.Person.FirstName != null ? u.Person.FirstName : null);
                    }
                    else
                    {
                        query = query.OrderByProperty(filter.Sort);
                    }
                }
            }
            var users = query.Skip((filter.Page - 1) * filter.Quantity).Take(filter.Quantity);
            return new Paged<User>(users.ToArray(), filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Get the user with the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public User Get(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            return this.Context.Users
                .Include(u => u.Roles)
                .Include(u => u.Organizations)
                .ThenInclude(o => o.Parent)
                .Include(u => u.Person)
                .ThenInclude(p => p.ContactMethods)
                .AsNoTracking()
                .SingleOrDefault(u => u.Id == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the user with the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public User GetTracking(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            return this.Context.Users
                .Include(u => u.Roles)
                .Include(u => u.Organizations)
                .ThenInclude(o => o.Parent)
                .Include(u => u.Person)
                .ThenInclude(p => p.ContactMethods)
                .SingleOrDefault(u => u.Id == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Load the specified 'user' organizations into context.
        /// </summary>
        /// <param name="user"></param>
        public void LoadOrganizations(User user)
        {
            this.Context.Entry(user)
                .Collection(u => u.OrganizationsManyToMany)
                .Load();

            this.Context.Entry(user)
                .Collection(u => u.Organizations)
                .Load();
        }

        /// <summary>
        /// Load the specified 'user' roles into context.
        /// </summary>
        /// <param name="user"></param>
        public void LoadRoles(User user)
        {
            this.Context.Entry(user)
                .Collection(u => u.RolesManyToMany)
                .Load();

            this.Context.Entry(user)
                .Collection(u => u.Roles)
                .Load();
        }

        /// <summary>
        /// Get the user with the specified 'keycloakUserId'.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public User Get(Guid keycloakUserId)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            return this.Context.Users
                .Include(u => u.Roles)
                .Include(u => u.Organizations)
                .ThenInclude(o => o.Parent)
                .Include(u => u.Person)
                .ThenInclude(p => p.ContactMethods)
                .AsNoTracking()
                .SingleOrDefault(u => u.KeycloakUserId == keycloakUserId) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the specified user to the datasource.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public User Add(User add)
        {
            if (add == null) throw new ArgumentNullException();
            add.IssueOn = DateTime.UtcNow;
            AddWithoutSave(add);
            this.Context.CommitTransaction();
            return Get(add.Id);
        }

        /// <summary>
        /// Add the specified user to the datasource.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public void AddWithoutSave(User add)
        {
            add.ThrowIfNull(nameof(add));
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            add.Roles.ForEach(r => this.Context.Entry(r).State = EntityState.Added);
            add.Organizations.ForEach(a => this.Context.Entry(a).State = EntityState.Added);
            add.RolesManyToMany.ForEach(r => this.Context.Entry(r).State = EntityState.Added);
            add.OrganizationsManyToMany.ForEach(a => this.Context.Entry(a).State = EntityState.Added);

            this.Context.Users.Add(add);
        }

        /// <summary>
        /// Updates the specified user in the datasource.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public User Update(User update)
        {
            var user = UpdateWithoutSave(update);
            this.Context.CommitTransaction();

            return user;
        }

        /// <summary>
        /// Updates the specified user in the datasource.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public User UpdateOnly(User update)
        {
            this.Context.Users.Update(update);

            return update;
        }

        /// <summary>
        /// Updates the specified user in the datasource.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public User UpdateWithoutSave(User update)
        {
            update.ThrowIfNull(nameof(update));
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var user = this.Context.Users
                .Include(u => u.RolesManyToMany)
                .Include(u => u.OrganizationsManyToMany)
                .Include(u => u.Person)
                .ThenInclude(p => p.ContactMethods)
                .FirstOrDefault(u => u.Id == update.Id) ?? throw new KeyNotFoundException();

            //If the user has no organizations we assume this update is an approval.
            if (!user.Organizations.Any())
            {
                var key = this.User.GetUserKey();
                var approvedBy = this.Context.Users.AsNoTracking().FirstOrDefault(u => u.KeycloakUserId == key) ?? throw new KeyNotFoundException($"Current user principal key:'{key}' does not exist");
                user.ApprovedBy = approvedBy.BusinessIdentifier;
                user.IssueOn = DateTime.UtcNow;
            }

            user.RowVersion = update.RowVersion;
            this.Context.SetOriginalRowVersion(user);

            var addRoles = update.RolesManyToMany.Except(user.RolesManyToMany, new UserRoleRoleIdComparer());
            addRoles.ForEach(r => user.RolesManyToMany.Add(new UserRole(user.Id, r.Id)));
            var removeRoles = user.RolesManyToMany.Except(update.RolesManyToMany, new UserRoleRoleIdComparer());
            removeRoles.ForEach(r =>
            {
                var remove = user.RolesManyToMany.FirstOrDefault(r2 => r2.RoleId == r.RoleId);
                if (remove != null)
                    this.Context.Entry(remove).State = EntityState.Deleted;
            });

            var addOrganizations = update.OrganizationsManyToMany.Except(user.OrganizationsManyToMany, new UserOrganizationOrganizationIdComparer());
            addOrganizations.ForEach(o => user.OrganizationsManyToMany.Add(new UserOrganization(user.Id, o.OrganizationId, o.RoleId)));
            var removeOrganizations = user.OrganizationsManyToMany.Except(update.OrganizationsManyToMany, new UserOrganizationOrganizationIdComparer());
            removeOrganizations.ForEach(o =>
            {
                var remove = user.OrganizationsManyToMany.FirstOrDefault(o2 => o2.OrganizationId == o.OrganizationId && o2.RoleId == o.RoleId);
                if (remove != null)
                    this.Context.Entry(remove).State = EntityState.Deleted;
            });
            user.Note = update.Note;
            user.Position = update.Position;
            user.LastLogin = update.LastLogin;
            user.ExpiryOn = update.ExpiryOn;
            this.Context.Users.Update(user);
            return user;
        }

        /// <summary>
        /// Remove the specified user from the datasource.
        /// </summary>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <param name="delete"></param>
        public void Delete(User delete)
        {
            delete.ThrowIfNull(nameof(delete));
            this.User.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var user = this.Context.Users
                .Include(u => u.RolesManyToMany)
                .Include(u => u.OrganizationsManyToMany)
                .FirstOrDefault(u => u.Id == delete.Id) ?? throw new KeyNotFoundException();

            user.RowVersion = delete.RowVersion;
            this.Context.SetOriginalRowVersion(user);

            user.RolesManyToMany.Clear();
            user.OrganizationsManyToMany.Clear();

            this.Context.Users.Remove(user);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Get an array of organization IDs for the specified 'keycloakUserId'.
        /// This only returns the first two layers (direct parents, their immediate children).
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <returns></returns>
        public IEnumerable<long> GetOrganizations(Guid keycloakUserId)
        {
            var user = this.Context.Users
                .Include(u => u.Organizations)
                .ThenInclude(a => a.Children)
                .Single(u => u.KeycloakUserId == keycloakUserId) ?? throw new KeyNotFoundException();
            var organizations = user.Organizations.Select(a => a.Id).ToList();
            organizations.AddRange(user.Organizations.SelectMany(a => a.Children.Where(ac => !ac.IsDisabled)).Select(a => a.Id));

            return organizations.ToArray();
        }

        /// <summary>
        /// Get all the system administrators, and organization administrators for the specified 'organizationId'.
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        public IEnumerable<User> GetAdmininstrators(params long[] organizations)
        {
            if (organizations == null) throw new ArgumentNullException(nameof(organizations));

            return this.Context.Users
                .Include(u => u.Person)
                .ThenInclude(p => p.ContactMethods)
                .AsNoTracking()
                .Where(u => u.Roles.Any(r => r.Claims.Any(c => c.Name == Permissions.SystemAdmin.GetName()))
                    || (u.Organizations.Any(a => organizations.Contains(a.Id))
                        && u.Roles.Any(r => r.Claims.Any(c => c.Name == Permissions.OrganizationAdmin.GetName()))
                ));
        }
        #endregion
    }
}
