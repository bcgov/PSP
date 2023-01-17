using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Moq;
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
    [Trait("area", "admin")]
    [Trait("group", "contact")]
    [ExcludeFromCodeCoverage]
    public class ContactRepositoryTest
    {
        #region Tests

        [Fact]
        public void Get_Contact_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<ContactRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetById("123"));
        }

        [Fact]
        public void Get_Contacts_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<ContactRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetAll(new ContactFilter()));
        }

        [Fact]
        public void Get_Contacts_Invalid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);

            var service = helper.CreateRepository<ContactRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() =>
                service.GetAll(new ContactFilter()));
        }

        [Fact]
        public void Get_Contacts_Paged_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<ContactRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetPage(new ContactFilter()));
        }

        [Fact]
        public void Get_Contacts_Paged_InvalidFilter()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);

            var service = helper.CreateRepository<ContactRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() =>
                service.GetPage(new ContactFilter()));
        }
        #endregion
    }
}
