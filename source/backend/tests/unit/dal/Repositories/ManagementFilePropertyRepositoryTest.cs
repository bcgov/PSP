using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "management")]
    [ExcludeFromCodeCoverage]
    public class ManagementFilePropertyRepositoryTest
    {
        #region Data
        #endregion

        #region Tests

        #region Get Management Properties By File Id
        [Fact]
        public void GetByManagementFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementView);

            var dspFile = EntityHelper.CreateManagementFile();
            var pimsManagementFileProperty = new PimsManagementFileProperty() { ManagementFileId = dspFile.ManagementFileId, Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsManagementFileProperty);
            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByManagementFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsManagementFileProperty>>();
            result.Should().HaveCount(1);
            result.FirstOrDefault().ManagementFilePropertyId.Should().Be(pimsManagementFileProperty.ManagementFilePropertyId);
        }

        [Fact]
        public void GetByManagementFileId_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByManagementFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsManagementFileProperty>>();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Get Management Properties Count By File Id
        [Fact]
        public void GetManagementFilePropertyRelatedCount_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementView);

            var dspFile = EntityHelper.CreateManagementFile();
            var pimsManagementFileProperty = new PimsManagementFileProperty() { ManagementFileId = dspFile.ManagementFileId, Property = EntityHelper.CreateProperty(1) };
            var pimsManagementFileProperty2 = new PimsManagementFileProperty() { ManagementFileId = dspFile.ManagementFileId, Property = EntityHelper.CreateProperty(2) };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsManagementFileProperty);
            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            var result = repository.GetManagementFilePropertyRelatedCount(1);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void GetManagementFilePropertyRelatedCount_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            var result = repository.GetManagementFilePropertyRelatedCount(1);

            // Assert
            result.Should().Be(0);
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            var pimsManagementFileProperty = new PimsManagementFileProperty() { ManagementFileId = 1, PropertyId = 1, Property = new PimsProperty() { RegionCode = 1, PropertyId = 1 } };

            // Act
            var result = repository.Add(pimsManagementFileProperty);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsManagementFileProperty>();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            var pimsManagementFileProperty = new PimsManagementFileProperty()
            {
                ManagementFileId = 1,
                PropertyId = 1,
                Property = new PimsProperty()
                {
                    RegionCode = 1,
                    PropertyId = 1,
                    IsRetired = true
                }
            };


            // Act
            Action act = () => repository.Add(pimsManagementFileProperty);

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }

        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementAdd);

            var pimsManagementFileProperty = new PimsManagementFileProperty() { ManagementFileId = 1, Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);
            context.AddAndSaveChanges(pimsManagementFileProperty);

            // Act
            var result = repository.Update(new PimsManagementFileProperty() { ManagementFileId = 1, PropertyName = "updated", PropertyId = 1, Property = new PimsProperty() { RegionCode = 1, PropertyId = 2 } });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsManagementFileProperty>();
            result.PropertyName.Should().Be("updated");
        }

        [Fact]
        public void Update_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_PropertyManagementFile()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementDelete);
            var pimsManagementFileProperty = new PimsManagementFileProperty() { Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(pimsManagementFileProperty);

            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            repository.Delete(pimsManagementFileProperty);

            // Assert
            var result = repository.GetPropertiesByManagementFileId(pimsManagementFileProperty.ManagementFilePropertyId);
            result.Should().BeEmpty();
        }

        [Fact]
        public void Delete_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementDelete);

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ManagementFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Delete(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #endregion
    }
}
