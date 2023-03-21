using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Helpers.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "extensions")]
    [Trait("group", "building")]
    [ExcludeFromCodeCoverage]
    public class EntityExtensionsTest
    {
        #region Tests
        #region ThrowIfNotAllowedToEdit Role
        [Fact]
        public void ThrowIfNotAllowedToEdit_Role_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForRole("role");
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            property.ThrowIfNotAllowedToEdit("paramName", user, "role");

            // Assert
            Assert.True(true); // It didn't throw a NotAuthorizedException.
        }

        [Fact]
        public void ThrowIfNotAllowedToEdit_Role_Entity_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForRole("role");
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => EntityExtensions.ThrowIfNotAllowedToEdit((PimsProperty)null, "paramName", user, "role"));
        }

        [Fact]
        public void ThrowIfNotAllowedToEdit_Role_NotAuthorizedException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForRole("role");
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => property.ThrowIfNotAllowedToEdit("paramName", user, "test"));
        }
        #endregion

        #region ThrowIfNotAllowedToEdit Permission
        [Fact]
        public void ThrowIfNotAllowedToEdit_Permission_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            property.ThrowIfNotAllowedToEdit("paramName", user, Permissions.PropertyEdit);

            // Assert
            Assert.True(true); // It didn't throw a NotAuthorizedException.
        }

        [Fact]
        public void ThrowIfNotAllowedToEdit_Permission_Entity_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => EntityExtensions.ThrowIfNotAllowedToEdit((PimsProperty)null, "paramName", user, Permissions.PropertyEdit));
        }

        [Fact]
        public void ThrowIfNotAllowedToEdit_Permission_NotAuthorizedException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => property.ThrowIfNotAllowedToEdit("paramName", user, Permissions.AdminProjects));
        }
        #endregion

        #region ThrowIfNotAllowedToEdit Permissions
        [Fact]
        public void ThrowIfNotAllowedToEdit_Permissions_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            property.ThrowIfNotAllowedToEdit("paramName", user, new[] { Permissions.PropertyEdit, Permissions.PropertyAdd });

            // Assert
            Assert.True(true); // It didn't throw a NotAuthorizedException.
        }

        [Fact]
        public void ThrowIfNotAllowedToEdit_Permissions_Entity_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => EntityExtensions.ThrowIfNotAllowedToEdit((PimsProperty)null, "paramName", user, new[] { Permissions.PropertyEdit, Permissions.PropertyAdd }));
        }

        [Fact]
        public void ThrowIfNotAllowedToEdit_Permissions_NotAuthorizedException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => property.ThrowIfNotAllowedToEdit("paramName", user, new[] { Permissions.PropertyView, Permissions.PropertyAdd }));
        }

        [Fact]
        public void ThrowIfNotAllowedToEdit_Permissions_AllRequired_NotAuthorizedException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = helper.CreateForPermission(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(123);
            property.ConcurrencyControlNumber = 1;

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => property.ThrowIfNotAllowedToEdit("paramName", user, new[] { Permissions.PropertyEdit, Permissions.PropertyAdd }, true));
        }
        #endregion

        #endregion
    }
}
