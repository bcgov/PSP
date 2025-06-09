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
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionFilePropertyRepositoryTest
    {
        #region Data
        #endregion

        #region Tests

        #region Get Disposition Properties By File Id
        [Fact]
        public void GetByDispositionFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);

            var dspFile = EntityHelper.CreateDispositionFile();
            var pimsDispositionFileProperty = new PimsDispositionFileProperty() { DispositionFileId = dspFile.DispositionFileId, Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsDispositionFileProperty);
            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByDispositionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionFileProperty>>();
            result.Should().HaveCount(1);
            result.FirstOrDefault().DispositionFilePropertyId.Should().Be(pimsDispositionFileProperty.DispositionFilePropertyId);
        }

        [Fact]
        public void GetByDispositionFileId_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByDispositionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionFileProperty>>();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Get Disposition Properties Count By File Id
        [Fact]
        public void GetDispositionFilePropertyRelatedCount_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);

            var dspFile = EntityHelper.CreateDispositionFile();
            var pimsDispositionFileProperty = new PimsDispositionFileProperty() { DispositionFileId = dspFile.DispositionFileId, Property = EntityHelper.CreateProperty(1) };
            var pimsDispositionFileProperty2 = new PimsDispositionFileProperty() { DispositionFileId = dspFile.DispositionFileId, Property = EntityHelper.CreateProperty(2) };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsDispositionFileProperty);
            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetDispositionFilePropertyRelatedCount(1);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void GetDispositionFilePropertyRelatedCount_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetDispositionFilePropertyRelatedCount(1);

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
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            var pimsDispositionFileProperty = new PimsDispositionFileProperty() { DispositionFileId = 1, PropertyId = 1, Property = new PimsProperty() { RegionCode = 1, PropertyId = 1 } };

            // Act
            var result = repository.Add(pimsDispositionFileProperty);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFileProperty>();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

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
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            var pimsDispositionFileProperty = new PimsDispositionFileProperty()
            {
                DispositionFileId = 1,
                PropertyId = 1,
                Property = new PimsProperty()
                {
                    RegionCode = 1,
                    PropertyId = 1,
                    IsRetired = true
                }
            };


            // Act
            Action act = () => repository.Add(pimsDispositionFileProperty);

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
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);

            var pimsDispositionFileProperty = new PimsDispositionFileProperty() { DispositionFileId = 1, Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);
            context.AddAndSaveChanges(pimsDispositionFileProperty);

            // Act
            var result = repository.Update(new PimsDispositionFileProperty() { DispositionFileId = 1, PropertyName = "updated", PropertyId = 1, Property = new PimsProperty() { RegionCode = 1, PropertyId = 2 } });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFileProperty>();
            result.PropertyName.Should().Be("updated");
        }

        [Fact]
        public void Update_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_PropertyDispositionFile()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionDelete);
            var pimsDispositionFileProperty = new PimsDispositionFileProperty() { Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(pimsDispositionFileProperty);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            repository.Delete(pimsDispositionFileProperty);

            // Assert
            var result = repository.GetPropertiesByDispositionFileId(pimsDispositionFileProperty.DispositionFilePropertyId);
            result.Should().BeEmpty();
        }

        [Fact]
        public void Delete_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionDelete);

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Delete(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #endregion
    }
}
