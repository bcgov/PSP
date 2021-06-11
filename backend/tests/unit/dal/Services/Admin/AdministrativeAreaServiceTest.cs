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
    [Trait("group", "administrativearea")]
    [ExcludeFromCodeCoverage]
    public class AdministrativeAreaServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void Get_AdministrativeArea()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var name = "Testville";
            var administrativeArea = EntityHelper.CreateAdministrativeArea("Testville", "TST");
            using var init = helper.InitializeDatabase(user);

            init.AddAndSaveChanges(administrativeArea);

            var service = helper.CreateService<AdministrativeAreaService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Get(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
        }

        [Fact]
        public void Get_AdministrativeArea_NullArguments()
        {

            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<AdministrativeAreaService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Get(null));
        }

        [Fact]
        public void GetAll_AdministrativeAreas()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);

            var service = helper.CreateService<AdministrativeAreaService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.GetAll();

            // Assert
            // There are 6 default admin areas
            Assert.Equal(6, result.Count());
        }
        #endregion

        #region Update
        [Fact]
        public void Update_AdministrativeArea_NullArguments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<AdministrativeAreaService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_AdministrativeArea_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var administrativeArea = EntityHelper.CreateAdministrativeArea("Testville", "TEST");
            administrativeArea.Id = 7;

            var updateAdminArea = EntityHelper.CreateAdministrativeArea("Updatedville", "UPDT");
            updateAdminArea.Id = 7;

            using var init = helper.InitializeDatabase(user);
            init.AddAndSaveChanges(administrativeArea);

            var service = helper.CreateService<AdministrativeAreaService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Update(updateAdminArea);

            // Assert
            administrativeArea.Name.Should().Be("Updatedville");

        }
        #endregion

        #region Remove
        [Fact]
        public void Remove_AdministrativeArea()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var administrativeArea = EntityHelper.CreateAdministrativeArea("Testville", "TEST");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(administrativeArea);

            var service = helper.CreateService<AdministrativeAreaService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(administrativeArea);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(administrativeArea).State);
        }

        [Fact]
        public void Remove_AdministrativeArea_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<AdministrativeAreaService>(user);
            var administrativeArea = EntityHelper.CreateAdministrativeArea("Test", "TEST");
            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(administrativeArea));
        }
        #endregion
        #endregion
    }
}
