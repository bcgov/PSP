using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
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
    [Trait("group", "organization")]
    [ExcludeFromCodeCoverage]
    public class UserOrganizationServiceTest
    {
        #region Data
        public static IEnumerable<object[]> OrganizationFilterData =>
            new List<object[]>
            {
                new object[] { new OrganizationFilter(1, 1, 100, "TST", 0, false, null), 1 },
                new object[] { new OrganizationFilter() { ParentId = 0, Id = 101 }, 1 },
                new object[] { new OrganizationFilter() { IsDisabled = true }, 1 },
                new object[] { new OrganizationFilter() { Id = 200 }, 1 },
            };
        #endregion

        #region Tests
        #region Get
        [Fact]
        public void Get_Organizations_Paged()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);
            var organization1 = init.CreateOrganization(100, "AG1");
            init.AddAndSaveChanges(organization1);

            var service = helper.CreateRepository<UserOrganizationService>(user);

            // Act
            var result = service.Get(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsOrganization>>(result);
            result.Items.Count.Should().Be(1);
        }

        [Fact]
        public void Get_Organizations_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<UserOrganizationService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get(1, 1));
        }

        [Theory]
        [MemberData(nameof(OrganizationFilterData))]
        public void Get_Organizations(OrganizationFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);
            var organization1 = init.CreateOrganization(100, "TST");
            organization1.PrntOrganizationId = 0;
            organization1.IsDisabled = false;
            var organization2 = init.CreateOrganization(101, "TST2");
            organization2.PrntOrganizationId = 0;
            var organization3 = init.CreateOrganization(102, "TST3");
            organization3.IsDisabled = true;
            var organization4 = init.CreateOrganization(200, "TST4");

            init.AddAndSaveChanges(organization1, organization2, organization3, organization4);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Get(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsOrganization>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
        }

        [Fact]
        public void GetAll_Organizations()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.PimsOrganization>>(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Get_Organization_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);

            var organization = init.CreateOrganization(19, "TST");
            init.AddAndSaveChanges(organization);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Get(19);

            // Assert
            Assert.Equal("TST", result.OrganizationName);
        }

        [Fact]
        public void Get_Organization_ById_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Get(19));
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Organization()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var init = helper.InitializeDatabase(user);
            var service = helper.CreateRepository<UserOrganizationService>(user);

            var orgType = init.PimsOrganizationTypes.First();
            var organization = EntityHelper.CreateOrganization(19, "TST", orgType);

            // Act
            service.Add(organization);
            var result = service.Get(organization.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(19, result.Id);

        }

        [Fact]
        public void Add_Organization_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Add(null));
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Organization_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_Organization()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var init = helper.InitializeDatabase(user);

            var organization = init.CreateOrganization(19, "TST");
            init.AddAndSaveChanges(organization);
            var newName = "Update";
            var updatedOrganization = init.CreateOrganization(19, newName);
            updatedOrganization.ConcurrencyControlNumber = organization.ConcurrencyControlNumber;

            var service = helper.CreateRepository<UserOrganizationService>(user);

            // Act
            service.Update(updatedOrganization);

            // Assert
            organization.OrganizationName.Should().Be(newName);

        }

        [Fact]
        public void Update_Organization_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var organization = EntityHelper.CreateOrganization(22, "TST");

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(organization));
        }
        #endregion


        #region Remove
        /// <summary>
        /// Argument cannot be null.
        /// </summary>
        [Fact]
        public void Remove_ArgumentNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var organization = EntityHelper.CreateOrganization(1, "TST");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(organization);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Delete(null));
        }

        [Fact]
        public void Remove_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var organization = EntityHelper.CreateOrganization(1, "TST");
            helper.CreatePimsContext(user, true);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Delete(organization));
        }

        [Fact]
        public void Delete_Organization_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var organization = EntityHelper.CreateOrganization(19, "TST");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(organization);

            var service = helper.CreateRepository<UserOrganizationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Delete(organization);

            // Assert
            context.PimsOrganizations.FirstOrDefault().Should().BeNull();
        }
        #endregion
        #endregion
    }
}
