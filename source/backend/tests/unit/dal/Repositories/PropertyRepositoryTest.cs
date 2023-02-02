using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Extensions;
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
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> AllPropertyFilters =>
            new List<object[]>
            {
                new object[] { new PropertyFilter() { PinOrPid = "111-111-111" }, 1 },
                new object[] { new PropertyFilter() { PinOrPid = "111" }, 2 },
                new object[] { new PropertyFilter() { Address = "12342 Test Street" }, 5 },
                new object[] { new PropertyFilter() { Page = 1, Quantity = 10 }, 6 },
                new object[] { new PropertyFilter(), 6 },
            };
        #endregion

        #region Constructors
        public PropertyRepositoryTest() { }
        #endregion

        #region Tests
        #region Get Paged Properties
        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Properties_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => repository.GetPage((PropertyFilter)null));
        }

        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Properties_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var filter = new PropertyFilter();

            var service = helper.CreateRepository<PropertyRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetPage(filter));
        }

        [Theory]
        [MemberData(nameof(AllPropertyFilters))]
        public void GetPage_Properties(PropertyFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            using var init = helper.InitializeDatabase(user);

            init.CreateProperty(2);
            init.CreateProperty(3, pin: 111);
            init.CreateProperty(4, address: init.PimsAddresses.FirstOrDefault());
            init.Add(new Entity.PimsProperty() { Location = new NetTopologySuite.Geometries.Point(-123.720810, 48.529338) });
            init.CreateProperty(5, classification: init.PimsPropertyClassificationTypes.FirstOrDefault(c => c.PropertyClassificationTypeCode == "Core Operational"));
            init.CreateProperty(111111111);

            init.SaveChanges();

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = repository.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.PimsProperty>>(result);
            Assert.Equal(expectedCount, result.Total);
        }
        #endregion

        #region Get
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            var property = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Pid.Should().Be(1);
        }
        #endregion

        #region GetByPid
        [Fact]
        public void GetByPid_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var pid = 1111;
            var property = EntityHelper.CreateProperty(pid);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = repository.GetByPid(pid.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Pid.Should().Be(pid);
        }
        #endregion

        #region GetByPin
        [Fact]
        public void GetByPin_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var pin = 1111;
            var property = EntityHelper.CreateProperty(1, pin);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = repository.GetByPin(pin);

            // Assert
            result.Should().NotBeNull();
            result.Pin.Should().Be(pin);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Property_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var repository = helper.CreateRepository<PropertyRepository>(user);

            var newValues = new Entity.PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;

            // Act
            var updatedProperty = repository.Update(newValues);

            // Assert
            updatedProperty.Description.Should().Be("test");
            updatedProperty.Pid.Should().Be(200);
        }

        [Fact]
        public void Update_Property_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView, Permissions.PropertyEdit);
            helper.CreatePimsContext(user, true);
            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Assert
            Assert.Throws<KeyNotFoundException>(() => repository.Update(property));
        }

        [Fact]
        public void Update_Property_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Assert
            Assert.Throws<ArgumentNullException>(() => repository.Update(null));
        }
        #endregion

        #endregion
    }
}
