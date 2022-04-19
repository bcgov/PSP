using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "users")]
    [ExcludeFromCodeCoverage]
    public class UserServiceTest
    {
        #region Data
        public static IEnumerable<object[]> UserFilterData =>
            new List<object[]>
            {
                new object[] { new UserFilter(1, 1, null, "ttester", "McTest", "Test", "test@test.com", false, null, null, "position"), 1 },
                new object[] { new UserFilter() { BusinessIdentifierValue = "ttester" }, 1 },
                new object[] { new UserFilter() { IsDisabled = true }, 0 },
                new object[] { new UserFilter() { Organization = "Test" }, 0 },
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

            var service = helper.CreateRepository<UserService>(user);

            // Act
            var result = service.Count();

            // Assert
            Assert.Equal(1, result);
        }

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

            var service = helper.CreateRepository<UserService>(user);

            // Act
            var result = service.Get(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsUser>>(result);
            Assert.Equal(expectedCount, result.Items.Count());
        }

        [Fact]
        public void Get_Users_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<UserService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get());
        }

        [Theory]
        [MemberData(nameof(UserFilterData))]
        public void Get_Users_Filter(UserFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var euser = EntityHelper.CreateUser("Tester");
            euser.Person.FirstName = "Test";
            euser.Person.Surname = "McTest";
            euser.BusinessIdentifierValue = "ttester";
            euser.Position = "position";
            euser.Person.PimsContactMethods.Add(EntityHelper.CreateContactMethod(1, "test@test.com", euser.Person, euser.GetOrganizations().First()));
            euser.IsDisabled = false;

            helper.CreatePimsContext(user, true).AddAndSaveChanges(euser);

            var service = helper.CreateRepository<UserService>(user);

            // Act
            var result = service.Get(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsUser>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
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

            var service = helper.CreateRepository<UserService>(user);

            // Act
            var result = service.Get(key);

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

            var service = helper.CreateRepository<UserService>(user);

            // Act
            service.Add(euser);
            var result = service.Get(euser.GuidIdentifierValue.Value);

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

            var service = helper.CreateRepository<UserService>(user);
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

            var service = helper.CreateRepository<UserService>(user);
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

            var service = helper.CreateRepository<UserService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() => service.Update(euser));
        }
        #endregion
        #endregion
    }
}
