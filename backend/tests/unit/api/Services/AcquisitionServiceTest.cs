using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionServiceTest
    {
        #region Tests
        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            var result = service.Add(acqFile);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            Action act = () => service.Add(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            Action act = () => service.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.Update(acqFile);

            // Assert

            // TODO: Update test when Update gets implemented
            act.Should().Throw<System.NotImplementedException>();
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.Update(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            Action act = () => service.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void UpdateProperties_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } };

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            propertyRepository.Verify(x => x.GetByPid(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_NewProperty_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } };

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            PimsPropertyAcquisitionFile updatedAcquisitionFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyAcquisitionFile>())).Callback<PimsPropertyAcquisitionFile>(x => updatedAcquisitionFileProperty = x).Returns(acqFile.PimsPropertyAcquisitionFiles.FirstOrDefault());

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(924046.3314288399, 1088892.9140135897));

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedAcquisitionFileProperty.Property;
            updatedProperty.PropertyClassificationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().BeCloseTo(System.DateTime.Now);
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsPropertyOfInterest.Should().Be(true);

            filePropertyRepository.Verify(x => x.GetByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            propertyRepository.Verify(x => x.GetByPid(It.IsAny<int>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
        }

        [Fact]
        public void UpdateProperties_UpdatePropertyFile_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var service = helper.Create<AcquisitionFileService>(user);

            var property = EntityHelper.CreateProperty(12345);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Id = 1, Property = property } };
            acqFile.ConcurrencyControlNumber = 1;

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Id = 1, Property = property, PropertyName = "updated" } });

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemovePropertyFile_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } });

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            propertyRepository.Verify(x => x.Delete(It.IsAny<PimsProperty>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<AcquisitionFileService>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.UpdateProperties(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }
        #endregion

        #endregion
    }
}
