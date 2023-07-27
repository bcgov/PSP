using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "users")]
    [ExcludeFromCodeCoverage]
    public class UserRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> UserFilterData =>
            new List<object[]>
            {
                new object[] { new UserFilter(1, 2, null, null, false, null, null, new string[] { "Position desc" }), 2 },
                new object[] { new UserFilter() { BusinessIdentifierValue = "ttester" }, 1 },
                new object[] { new UserFilter() { ActiveOnly = true }, 1 },
                new object[] { new UserFilter() { ActiveOnly = false }, 2 },
                new object[] { new UserFilter() { Email = "test@test.com" }, 1 },
                new object[] { new UserFilter() { Sort = new string[] { "Email desc" } }, 2 },
                new object[] { new UserFilter() { Sort = new string[] { "Surname asc" } }, 2 },
                new object[] { new UserFilter() { Sort = new string[] { "FirstName desc" } }, 2 },
            };
        #endregion

        #region Tests
        [Fact]
        public void User_Count()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Tester");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.Count();

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void User_Exists()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Tester");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.UserExists(euser.GuidIdentifierValue.Value);

            // Assert
            Assert.True(result, "Unable to find valid user by their guid");
        }

        [Fact]
        public void User_Exists_None()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.UserExists(Guid.NewGuid());

            // Assert
            Assert.False(result, "No user should be found for random guid.");
        }

        #region Activate

        [Fact]
        public void Activate_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var updatedUser = user.AddClaim("idir_username", "username@");
            updatedUser = updatedUser.AddClaim(ClaimTypes.GivenName, "first");
            updatedUser = updatedUser.AddClaim(ClaimTypes.Surname, "last");
            helper.CreatePimsContext(updatedUser, true);

            var service = helper.CreateRepository<UserRepository>(updatedUser);

            // Act
            var result = service.Activate();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsUser>(result);
            result.Person.FirstName.Should().Be("first");
            result.Person.Surname.Should().Be("last");
            result.BusinessIdentifierValue.Should().Be("username");
            result.IsDisabled.Should().BeFalse();
            result.GuidIdentifierValue.Value.Should().Be(updatedUser.FindFirstValue("idir_user_guid"));
            result.Person.PimsContactMethods.First().ContactMethodValue.Should().Be("test@test.com");
        }

        [Fact]
        public void Activate_Success_Options()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            helper.CreatePimsContext(user, true);
            var monitor = new Mock<IOptionsMonitor<PimsOptions>>();
            monitor.Setup(m => m.CurrentValue).Returns(new PimsOptions()
            {
                ServiceAccount = new ServiceAccountOptions()
                {
                    Email = "test@test.com",
                    Username = "username",
                    FirstName = "first",
                    LastName = "last",
                },
            });
            helper.AddSingleton(monitor);
            helper.AddSingleton(monitor.Object);

            var service = helper.CreateRepository<UserRepository>(user, monitor);

            // Act
            var result = service.Activate();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsUser>(result);
            result.Person.FirstName.Should().Be("first");
            result.Person.Surname.Should().Be("last");
            result.BusinessIdentifierValue.Should().Be("username");
            result.IsDisabled.Should().BeFalse();
            result.GuidIdentifierValue.Value.Should().Be(user.FindFirstValue("idir_user_guid"));
            result.Person.PimsContactMethods.First().ContactMethodValue.Should().Be("test@test.com");
        }

        [Fact]
        public void Activate_Existing()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var updatedUser = user.AddClaim("idir_username", "username@");
            var euser = EntityHelper.CreateUser("Tester");
            euser.GuidIdentifierValue = new Guid(user.FindFirstValue("idir_user_guid"));
            helper.CreatePimsContext(updatedUser, true).AddAndSaveChanges(euser);
            var now = DateTime.UtcNow;

            var service = helper.CreateRepository<UserRepository>(updatedUser);

            // Act
            var result = service.Activate();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsUser>(result);
            result.Should().BeEquivalentTo(euser);
            result.LastLogin.Should().BeAfter(now);
        }
        #endregion

        #region Get
        [Fact]
        public void Get_Users_Paged()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Tester");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);
            var expectedCount = 1;

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetPage(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsUser>>(result);
            Assert.Equal(expectedCount, result.Items.Count());
        }

        [Fact]
        public void Get_Users_Paged_MinPage()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Tester");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetPage(0, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsUser>>(result);
            Assert.Equal(1, result.Page);
        }

        [Fact]
        public void Get_Users_Paged_MinQuantity()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Tester");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetPage(1, 0);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsUser>>(result);
            Assert.Equal(1, result.Quantity);
        }

        [Fact]
        public void Get_Users_Paged_MaxQuantity()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Tester");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetPage(1, 51);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsUser>>(result);
            Assert.Equal(50, result.Quantity);
        }

        [Fact]
        public void Get_Users_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetAllByFilter());
        }

        [Theory]
        [MemberData(nameof(UserFilterData))]
        public void Get_Users_Filter(UserFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);

            var contactMethodType = EntityHelper.CreateContactMethodType("Email");
            var organization = EntityHelper.CreateOrganization(1, "test org");
            var role = EntityHelper.CreateRole("test role");

            var euser = EntityHelper.CreateUser(1, Guid.NewGuid(), "Tester", organization: organization, role: role);
            euser.Person.FirstName = "Test";
            euser.Person.Surname = "McTest";
            euser.BusinessIdentifierValue = "ttester";
            euser.Position = "position";
            euser.Person.PimsContactMethods.Add(EntityHelper.CreateContactMethod(1, "test@test.com", euser.Person, euser.GetOrganizations().First(), contactMethodType));
            euser.IsDisabled = false;

            var disabledEUser = EntityHelper.CreateUser(2, Guid.NewGuid(), "Tester2", organization: organization, role: role);
            disabledEUser.UserId = 2;
            disabledEUser.Person.FirstName = "disabled";
            disabledEUser.Person.Surname = "Other";
            disabledEUser.BusinessIdentifierValue = "oDisabled";
            disabledEUser.Position = "other";
            disabledEUser.Person.PimsContactMethods.Add(EntityHelper.CreateContactMethod(2, "diabled@test.com", euser.Person, euser.GetOrganizations().First(), contactMethodType));
            disabledEUser.IsDisabled = true;

            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser, disabledEUser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetAllByFilter(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsUser>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
            result.First().UserId.Should().Be(euser.UserId);
        }

        [Fact]
        public void Get_User_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var key = Guid.NewGuid();
            var euser = EntityHelper.CreateUser(1, key, "ttester", "Tester", "McTest");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetByKeycloakUserId(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(key, result.GuidIdentifierValue);
        }

        [Fact]
        public void Get_User_Tracking_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var key = Guid.NewGuid();
            var euser = EntityHelper.CreateUser(1, key, "ttester", "Tester", "McTest");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetTrackingById(euser.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(key, result.GuidIdentifierValue);
        }

        [Fact]
        public void Get_User_Tracking_ById_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var key = Guid.NewGuid();
            var euser = EntityHelper.CreateUser(1, key, "ttester", "Tester", "McTest");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            // Assert
            var result = Assert.Throws<NotAuthorizedException>(() => service.GetTrackingById(euser.UserId));
        }

        [Fact]
        public void Get_User_Info_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var key = Guid.NewGuid();
            var euser = EntityHelper.CreateUser(1, key, "ttester", "Tester", "McTest");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            var result = service.GetUserInfoByKeycloakUserId(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(key, result.GuidIdentifierValue);
        }
        #endregion

        #region Add
        [Fact]
        public void Add_User()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers, Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser(1, Guid.NewGuid(), "ttester", "Tester", "McTest");
            helper.CreatePimsContext(user, true);

            var service = helper.CreateRepository<UserRepository>(user);

            // Act
            service.Add(euser);
            var result = service.GetByKeycloakUserId(euser.GuidIdentifierValue.Value);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ttester", result.BusinessIdentifierValue);
        }

        [Fact]
        public void Add_User_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => service.Add(null));
        }

        #endregion

        #region Update
        [Fact]
        public void Update_User_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => service.Update(null));
        }

        [Fact]
        public void Update_User_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() => service.Update(euser));
        }

        [Fact]
        public void Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");
            var updateUser = EntityHelper.CreateUser(1, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue);
            updateUser.Note = "note";
            updateUser.Position = "position";
            updateUser.ExpiryDate = DateTime.Now;
            updateUser.LastLogin = DateTime.Now;
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Update(updateUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsUser>(result);
            result.PimsUserOrganizations.Should().HaveCount(1);
            result.PimsUserRoles.Should().HaveCount(1);
            result.Note.Should().Be(updateUser.Note);
            result.Position.Should().Be(updateUser.Position);
            result.ExpiryDate.Should().Be(updateUser.ExpiryDate);
            result.LastLogin.Should().Be(updateUser.LastLogin);
        }

        [Fact]
        public void Update_NoExistingOrganization()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");
            euser.GuidIdentifierValue = new Guid(user.FindFirstValue("idir_user_guid"));
            euser.PimsUserOrganizations.Clear();
            var updateUser = EntityHelper.CreateUser(1, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Update(updateUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsUser>(result);
            result.PimsUserOrganizations.Should().HaveCount(1);
            result.ApprovedById.Should().Be(euser.BusinessIdentifierValue);
        }

        [Fact]
        public void Update_NoOrganization()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");
            var updateUser = EntityHelper.CreateUser(1, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue);
            updateUser.PimsUserOrganizations.Clear();
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Update(updateUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsUser>(result);
            result.PimsUserOrganizations.Should().BeEmpty();
        }

        [Fact]
        public void Update_NoRole()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");
            var updateUser = EntityHelper.CreateUser(1, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue);
            updateUser.PimsUserRoles.Clear();
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Update(updateUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsUser>(result);
            result.PimsUserRoles.Should().BeEmpty();
        }

        #endregion
        public void Delete_User_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => service.Delete(null));
        }

        [Fact]
        public void Delete_User_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() => service.Delete(euser));
        }

        [Fact]
        public void Delete_User_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var euser = EntityHelper.CreateUser("Test");

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.Delete(euser));
        }

        [Fact]
        public void Delete()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Delete(euser);

            // Assert
            context.PimsUserRoles.Should().BeEmpty();
            context.PimsUserOrganizations.Should().BeEmpty();
            context.PimsUsers.Should().BeEmpty();
        }

        [Fact]
        public void RemoveRole()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Test");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.RemoveRole(euser, 1);

            // Assert
            context.PimsUsers.First().PimsUserRoles.Should().BeEmpty();
        }
        #region Delete

        #endregion
        #endregion
    }
}
