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
    [Trait("group", "province")]
    [ExcludeFromCodeCoverage]
    public class ProvinceServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void Get()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);

            var service = helper.CreateService<ProvinceService>(user);

            // Act
            var result = service.Get();

            // Assert
            Assert.True(result.Count() > 0);
        }
        #endregion
        #region Update
        [Fact]
        public void Update_Province()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var province = EntityHelper.CreateProvince("TP", "Test Province");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(province);

            var service = helper.CreateService<ProvinceService>(user);

            var updateProvince = EntityHelper.CreateProvince("TP", "Test Province");

            var newName = "Updated Province";
            updateProvince.Name = newName;

            // Act
            service.Update(updateProvince);

            // Assert
            province.Name.Should().Be(newName);
        }

        [Fact]
        public void Update_Province_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<ProvinceService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_Province_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var province = EntityHelper.CreateProvince("TP", "Test Province");

            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<ProvinceService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(province));
        }
        #endregion
        #region Remove
        [Fact]
        public void Remove_Province()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin, Permissions.AdminRoles);
            var province = EntityHelper.CreateProvince("TP", "Test Province");

            helper.CreatePimsContext(user, true).AddAndSaveChanges(province);

            var service = helper.CreateService<ProvinceService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(province);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(province).State);
        }

        [Fact]
        public void Remove_Province_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var province = EntityHelper.CreateProvince("TP", "Test Province");

            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<ProvinceService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(province));
        }

        [Fact]
        public void Remove_Province_NullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<ProvinceService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Remove(null));
        }
        #endregion
        #endregion
    }
}
