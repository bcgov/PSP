using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "organization")]
    [ExcludeFromCodeCoverage]
    public class OrganizationRepositoryTest
    {
        #region Constructors
        public OrganizationRepositoryTest() { }
        #endregion

        #region Tests

        #region GetRowVersion
        [Fact]
        public void GetRowVersion_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);

            var org = EntityHelper.CreateOrganization(1, "org");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(org);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            var result = repository.GetRowVersion(1);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public void GetRowVersion_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactDelete);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            Action act = () => repository.GetRowVersion(2);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var org = EntityHelper.CreateOrganization(1, "org");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(org);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsOrganization>();
        }

        [Fact]
        public void GetById_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            Action act = ()=> repository.GetById(2);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var org = EntityHelper.CreateOrganization(1, "org");

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            var result = repository.Add(org, false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsOrganization>();
        }

        [Fact]
        public void Add_Duplicate()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var org = EntityHelper.CreateOrganization(1, "org");
            org.PimsContactMethods = new List<PimsContactMethod>() { new PimsContactMethod() { ContactMethodTypeCode = "MAILING", ContactMethodValue = "1234"} };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(org);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            Action act = () => repository.Add(org, false);

            // Assert
            act.Should().Throw<DuplicateEntityException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactEdit);

            var org = EntityHelper.CreateOrganization(1, "org");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(org);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            org.OrganizationName = "updated";
            var result = repository.Update(org);

            // Assert
            result.OrganizationName.Should().Be("updated");
        }

        [Fact]
        public void Update_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);

            var org = EntityHelper.CreateOrganization(1, "org");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(org);
            var repository = helper.CreateRepository<OrganizationRepository>(user);

            // Act
            Action act = () => repository.Update(org);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #endregion
    }
}
