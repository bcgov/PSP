using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyServiceTest
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
        public PropertyServiceTest() { }
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
            var service = helper.CreateRepository<PropertyRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.GetPage((PropertyFilter)null));
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
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetPage(filter));
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

            var service = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.PimsProperty>>(result);
            Assert.Equal(expectedCount, result.Total);
        }
        #endregion
        #endregion
    }
}
