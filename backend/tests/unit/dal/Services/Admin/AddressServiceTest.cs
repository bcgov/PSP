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
    [Trait("group", "address")]
    [ExcludeFromCodeCoverage]
    public class AddressServiceTest
    {
        #region Tests
        #region Update
        [Fact]
        public void Update_Address()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var address = EntityHelper.CreateAddress(1, "1234 Street", null, "V9C9C9");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(address);

            var service = helper.CreateService<AddressService>(user);

            var updateAddress= EntityHelper.CreateAddress(1, "1234 Street", null, "V9C9C9");;
            var newName = "1234 Updated Street";
            updateAddress.Address1 = newName;

            // Act
            service.Update(updateAddress);

            // Assert
            address.Address1.Should().Be(newName);
        }

        [Fact]
        public void Update_Address_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var address = EntityHelper.CreateAddress(1, "1234 Street", null, "V9C9C9");

            helper.CreatePimsContext(user, true).AddAndSaveChanges(address);

            var service = helper.CreateService<AddressService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_Address_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var address = EntityHelper.CreateAddress(1, "1234 Street", null, "V9C9C9");

            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<AddressService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(address));
        }
        #endregion

        #region Remove
        [Fact]
        public void Remove_Address()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin, Permissions.AdminRoles);
            var address = EntityHelper.CreateAddress(1, "1234 Street", null, "V9C9C9");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(address);

            var service = helper.CreateService<AddressService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(address);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(address).State);
        }

        [Fact]
        public void Remove_Address_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var address = EntityHelper.CreateAddress(1, "1234 Street", null, "V9C9C9");

            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<AddressService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(address));
        }

        [Fact]
        public void Remove_Address_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var address = EntityHelper.CreateAddress(1, "1234 Street", null, "V9C9C9");

            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<AddressService>(user);
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
