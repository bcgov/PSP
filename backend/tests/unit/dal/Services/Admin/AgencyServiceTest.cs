using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Comparers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services.Admin
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "agency")]
    [ExcludeFromCodeCoverage]
    public class AgencyServiceTest
    {
        #region Data
        public static IEnumerable<object[]> AgencyFilterData =>
            new List<object[]>
            {
                new object[] { new AgencyFilter(1, 1, 100, "Test", 0, false, null), 1 },
                new object[] { new AgencyFilter() { ParentId = 0, Id = 101 }, 1 },
                new object[] { new AgencyFilter() { IsDisabled = true }, 1 },
                new object[] { new AgencyFilter() { Id = 200 }, 1 },
            };
        #endregion

        #region Tests
        #region Get
        [Fact]
        public void Get_Agencies_Paged()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);
            var agency1 = EntityHelper.CreateAgency(100, "AG1", "Agency 1");
            init.AddAndSaveChanges(agency1);
            var expectedCount = 1;

            var service = helper.CreateService<AgencyService>(user);

            // Act
            var result = service.Get(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.Agency>>(result);
            Assert.Equal(expectedCount, result.Items.Count());
        }

        [Fact]
        public void Get_Agencies_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateService<AgencyService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get(1, 1));
        }

        [Theory]
        [MemberData(nameof(AgencyFilterData))]
        public void Get_Agencies(AgencyFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);
            var agency1 = EntityHelper.CreateAgency(100, "TST", "Test");
            agency1.ParentId = 0;
            agency1.IsDisabled = false;
            var agency2 = EntityHelper.CreateAgency(101);
            agency2.ParentId = 0;
            var agency3 = EntityHelper.CreateAgency(102, "TST3", "Agency 3");
            agency3.IsDisabled = true;
            var agency4 = EntityHelper.CreateAgency(200, "TST4", "Agency 4");

            init.AddAndSaveChanges(agency1, agency2, agency3, agency4);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Get(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.Agency>>(result);
            Assert.Equal(expectedCount, result.Items.Count());
        }

        [Fact]
        public void GetAll_Agencies()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.GetAll();

            // Assert
            Assert.Equal(18, result.Count());
        }

        [Fact]
        public void Get_Agency_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);

            var agency = EntityHelper.CreateAgency(19, "TST", "Test");
            init.AddAndSaveChanges(agency);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Get(19);

            // Assert
            Assert.Equal("TST", result.Code);
        }

        [Fact]
        public void Get_Agency_ById_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Get(19));
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Agency()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var agency = EntityHelper.CreateAgency(19, "TST", "Test");

            var init = helper.InitializeDatabase(user);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Add(agency);
            var result = service.Get(agency.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(19, result.Id);

        }

        [Fact]
        public void Add_Agency_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Add(null));
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Agency_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_Agency()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var agency = EntityHelper.CreateAgency(19, "TST", "Test");

            var init = helper.InitializeDatabase(user);
            init.AddAndSaveChanges(agency);
            var newName = "Update";

            var updatedAgency = EntityHelper.CreateAgency(19, "UPDT", newName);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();


            // Act
            service.Update(updatedAgency);

            // Assert
            agency.Name.Should().Be(newName);

        }

        [Fact]
        public void Update_Agency_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var agency = EntityHelper.CreateAgency(22);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(agency));
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
            var agency = EntityHelper.CreateAgency(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(agency);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Remove(null));
        }

        [Fact]
        public void Remove_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var agency = EntityHelper.CreateAgency(1);
            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(agency));
        }

        [Fact]
        public void Remove_Agency_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var agency = EntityHelper.CreateAgency(19);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(agency);

            var service = helper.CreateService<AgencyService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(agency);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(agency).State);
        }
        #endregion
        #endregion
    }
}
