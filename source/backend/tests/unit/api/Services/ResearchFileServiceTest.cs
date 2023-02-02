using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchFileServiceTest
    {
        private TestHelper _helper;

        public ResearchFileServiceTest()
        {
            _helper = new TestHelper();
        }

        private ResearchFileService CreateResearchFileServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.Create<ResearchFileService>();
        }


        #region Tests
        #region GetPage
        [Fact]
        public void GetPage()
        {
            // Arrange
            var researchFile = EntityHelper.CreateResearchFile(1);

            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileView);
            var researchRepository = _helper.GetService<Mock<IResearchFileRepository>>();
            researchRepository.Setup(x => x.GetPage(It.IsAny<ResearchFilter>()));

            // Act
            var updatedLease = service.GetPage(new ResearchFilter());

            // Assert
            researchRepository.Verify(x => x.GetPage(It.IsAny<ResearchFilter>()), Times.Once);
        }

        [Fact]
        public void GetPage_NoPermission()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions();

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetPage(new ResearchFilter()));
        }

        #endregion

        #region UpdateProperties
        [Fact]
        public void UpdateProperties_Delete()
        {
            // Arrange
            var researchFile = EntityHelper.CreateResearchFile(1);
            var pimsPropertyResearchFile = new PimsPropertyResearchFile() { Property = new PimsProperty() { RegionCode = 1 } };
            pimsPropertyResearchFile.PimsPrfPropResearchPurposeTypes = new List<PimsPrfPropResearchPurposeType>() { new PimsPrfPropResearchPurposeType() { } };
            researchFile.PimsPropertyResearchFiles.Add(pimsPropertyResearchFile);

            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);
            var researchRepository = _helper.GetService<Mock<IResearchFileRepository>>();
            researchRepository.Setup(x => x.GetPage(It.IsAny<ResearchFilter>()));
            researchRepository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(2);
            var researchFilePropertyRepository = _helper.GetService<Mock<IResearchFilePropertyRepository>>();
            researchFilePropertyRepository.Setup(x => x.Delete(It.IsAny<PimsPropertyResearchFile>()));
            researchFilePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            // Act
            researchFile.PimsPropertyResearchFiles.Clear();
            researchFile.ConcurrencyControlNumber++;
            var updatedLease = service.UpdateProperties(researchFile);

            // Assert
            researchFilePropertyRepository.Verify(x => x.GetAllByResearchFileId(It.IsAny<long>()), Times.Once);
            researchFilePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PID_Success()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = _helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = _helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            // Act
            service.UpdateProperties(researchFile);

            // Assert
            filePropertyRepository.Verify(x => x.Add(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PIN_Success()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, 54321);
            property.Pid = null;
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = _helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPin(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = _helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            // Act
            service.UpdateProperties(researchFile);

            // Assert
            filePropertyRepository.Verify(x => x.Add(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PID_NewProperty_Success()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = _helper.GetService<Mock<IResearchFileRepository>>();
            PimsPropertyResearchFile updatedResearchFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var filePropertyRepository = _helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyResearchFile>())).Callback<PimsPropertyResearchFile>(x => updatedResearchFileProperty = x).Returns(researchFile.PimsPropertyResearchFiles.FirstOrDefault());

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var coordinateService = _helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(924046.3314288399, 1088892.9140135897));

            // Act
            service.UpdateProperties(researchFile);

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedResearchFileProperty.Property;
            updatedProperty.PropertyClassificationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().BeCloseTo(System.DateTime.Now, 40);
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsPropertyOfInterest.Should().Be(true);

            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PIN_NewProperty_Success()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, 54321);
            property.Pid = null;
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = _helper.GetService<Mock<IResearchFileRepository>>();
            PimsPropertyResearchFile updatedResearchFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var filePropertyRepository = _helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyResearchFile>())).Callback<PimsPropertyResearchFile>(x => updatedResearchFileProperty = x).Returns(researchFile.PimsPropertyResearchFiles.FirstOrDefault());

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPin(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var coordinateService = _helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(924046.3314288399, 1088892.9140135897));

            // Act
            service.UpdateProperties(researchFile);

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedResearchFileProperty.Property;
            updatedProperty.PropertyClassificationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().BeCloseTo(System.DateTime.Now, precision: 120000); // should be within 2 minutes to account for slow test runs
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsPropertyOfInterest.Should().Be(true);

            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileView);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = _helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit, Permissions.ResearchFileEdit);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = _helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsResearchFile>())).Returns(researchFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            // Act
            var result = service.Update(researchFile);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsResearchFile>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = CreateResearchFileServiceWithPermissions();

            var researchFile = EntityHelper.CreateResearchFile(1);

            // Act
            Action act = () => service.Update(researchFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #endregion
    }
}
