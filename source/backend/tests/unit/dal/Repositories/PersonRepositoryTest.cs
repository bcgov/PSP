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
    [Trait("group", "person")]
    [ExcludeFromCodeCoverage]
    public class PersonRepositoryTest
    {
        #region Constructors
        public PersonRepositoryTest() { }
        #endregion

        #region Tests

        #region GetRowVersion
        [Fact]
        public void GetRowVersion_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);

            var person = EntityHelper.CreatePerson(1, "last", "first");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(person);
            var repository = helper.CreateRepository<PersonRepository>(user);

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
            var repository = helper.CreateRepository<PersonRepository>(user);

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

            var person = EntityHelper.CreatePerson(1, "last", "first");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(person);
            var repository = helper.CreateRepository<PersonRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsPerson>();
        }

        [Fact]
        public void GetById_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<PersonRepository>(user);

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

            var person = EntityHelper.CreatePerson(1, "last", "first");

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<PersonRepository>(user);

            // Act
            var result = repository.Add(person, false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsPerson>();
        }

        [Fact]
        public void Add_Duplicate()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var person = EntityHelper.CreatePerson(1, "last", "first");
            person.PimsContactMethods = new List<PimsContactMethod>() { new PimsContactMethod() { ContactMethodTypeCode = "MAILING", ContactMethodValue = "1234"} };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(person);
            var repository = helper.CreateRepository<PersonRepository>(user);

            // Act
            Action act = () => repository.Add(person, false);

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

            var person = EntityHelper.CreatePerson(1, "last", "first");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(person);
            var repository = helper.CreateRepository<PersonRepository>(user);

            // Act
            person.Surname = "updated";
            var result = repository.Update(person);

            // Assert
            result.Surname.Should().Be("updated");
        }

        [Fact]
        public void Update_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);

            var person = EntityHelper.CreatePerson(1, "last", "first");

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(person);
            var repository = helper.CreateRepository<PersonRepository>(user);

            // Act
            Action act = () => repository.Update(person);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #endregion
    }
}
