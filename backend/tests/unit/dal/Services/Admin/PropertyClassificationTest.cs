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
    [Trait("group", "propertyclassification")]
    [ExcludeFromCodeCoverage]
    public class PropertyClassificationServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void GetAll_PropertyType()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            using var init = helper.InitializeDatabase(user);

            var service = helper.CreateService<PropertyClassificationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.GetAll();

            // Assert
            // There are 8 classificaitons by default
            Assert.True(result.Count() > 0);
        }
        #endregion
        #region Update
        [Fact]
        public void Update_PropertyClassification_NullArguments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<PropertyClassificationService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_PropertyClassification_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<PropertyClassificationService>(user);
            var classification = EntityHelper.CreatePropertyClassification("TEST");
            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(classification));
        }
        #endregion
        #region Remove
        [Fact]
        public void Remove_PropertyClassification_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<PropertyClassificationService>(user);
            var classification = EntityHelper.CreatePropertyClassification("TEST");
            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(classification));
        }

        [Fact]
        public void Remove_PropertyClassification_NullArguments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<PropertyClassificationService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Remove(null));
        }

        [Fact]
        public void Remove_PropertyClassification_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var classification = EntityHelper.CreatePropertyClassification("TEST");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(classification);

            var service = helper.CreateService<PropertyClassificationService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(classification);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(classification).State);
        }

        #endregion
        #endregion
    }
}
