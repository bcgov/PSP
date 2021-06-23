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
    [Trait("group", "buildingconstructiontype")]
    [ExcludeFromCodeCoverage]
    public class BuildingConstructionTypeServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void GetAll_BuildingConstructionTypes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);

            var service = helper.CreateService<BuildingConstructionTypeService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.BuildingConstructionType>>(result);
            Assert.NotEmpty(result);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_BuildingConstructionType_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var service = helper.CreateService<BuildingConstructionTypeService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_BuildingingConstructionType_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var constructionType = EntityHelper.CreateBuildingConstructionType(100, "Test");
            var init = helper.InitializeDatabase(user);
            init.AddAndSaveChanges(constructionType);
            var newName = "Update";

            var updatedConstructionType = EntityHelper.CreateBuildingConstructionType(100, newName);

            var service = helper.CreateService<BuildingConstructionTypeService>(user);
            var context = helper.GetService<PimsContext>();


            // Act
            service.Update(updatedConstructionType);

            // Assert
            constructionType.Name.Should().Be(newName);

        }

        [Fact]
        public void Update_BuildingConstructionType_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var constructionType = EntityHelper.CreateBuildingConstructionType(100, "Test");

            var service = helper.CreateService<BuildingConstructionTypeService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(constructionType));
        }
        #endregion

        #region Remove
       [Fact]
        public void Remove_BuildingConstructionType_ArgumentNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateService<BuildingConstructionTypeService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Remove(null));
        }

        [Fact]
        public void Remove_BuildingConstructionType_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var agency = EntityHelper.CreateAgency(1);
            var constructionType = EntityHelper.CreateBuildingConstructionType(100, "Test");
            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<BuildingConstructionTypeService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(constructionType));
        }

        [Fact]
        public void Remove_BuildingConstructionType_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var constructionType = EntityHelper.CreateBuildingConstructionType(100, "Test");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(constructionType);

            var service = helper.CreateService<BuildingConstructionTypeService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(constructionType);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(constructionType).State);
        }
        #endregion
        #endregion
    }
}
