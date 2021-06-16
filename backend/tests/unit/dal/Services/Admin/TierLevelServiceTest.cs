using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Comparers;
using Moq;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Microsoft.Extensions.Options;
using Pims.Dal.Entities.Models;
using Pims.Dal.Entities;
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
    [Trait("group", "project")]
    [ExcludeFromCodeCoverage]
    public class TierLevelServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void GetAll_TierLevels()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);
            var tierLevel = EntityHelper.CreateTierLevel(100, "testlevel");
            init.AddAndSaveChanges(tierLevel);

            var service = helper.CreateService<TierLevelService>(user);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.TierLevel>>(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Get_TierLevelById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);
            var tierLevel = EntityHelper.CreateTierLevel(100, "testlevel");
            init.AddAndSaveChanges(tierLevel);

            var service = helper.CreateService<TierLevelService>(user);

            // Act
            var result = service.Get(100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testlevel", result.Name);
        }

        [Fact]
        public void Get_TierLevelById_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var service = helper.CreateService<TierLevelService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Get(100));
        }
        #endregion
        #region Update
        [Fact]
        public void Update_TierLevel_NullArguments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<TierLevelService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_TierLevel_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var tierLevel = EntityHelper.CreateTierLevel(100, "Test");
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<TierLevelService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(tierLevel));
        }

        [Fact]
        public void Update_TierLevel_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var tierLevel = EntityHelper.CreateTierLevel(100, "Test");

            var updatedTierLevel = EntityHelper.CreateTierLevel(100, "Updated");

            using var init = helper.InitializeDatabase(user);
            init.AddAndSaveChanges(tierLevel);

            var service = helper.CreateService<TierLevelService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Update(updatedTierLevel);

            // Assert
            tierLevel.Name.Should().Be("Updated");

        }
        #endregion
        #region Remove
        [Fact]
        public void Remove_TierLevel_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var tierLevel = EntityHelper.CreateTierLevel(100, "Test");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(tierLevel);

            var service = helper.CreateService<TierLevelService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(tierLevel);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(tierLevel).State);
        }
        [Fact]
        public void Remove_TierLevel_NullArguments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<TierLevelService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Remove(null));
        }

        [Fact]
        public void Remove_TierLevel_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var tierLevel = EntityHelper.CreateTierLevel(100, "Test");
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<TierLevelService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(tierLevel));
        }
        #endregion
        #endregion
    }
}
