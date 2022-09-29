using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchServiceTest
    {
        #region UpdateProperties
        [Fact]
        public void UpdateProperties_MatchProperties_PID_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);
            var service = helper.Create<ResearchFileService>(user);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            // Act
            service.UpdateProperties(researchFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetByResearchFileId(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            propertyRepository.Verify(x => x.GetByPid(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PIN_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);
            var service = helper.Create<ResearchFileService>(user);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, 54321);
            property.Pid = null;
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPin(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            // Act
            service.UpdateProperties(researchFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetByResearchFileId(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            propertyRepository.Verify(x => x.GetByPin(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PID_NewProperty_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);
            var service = helper.Create<ResearchFileService>(user);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
            PimsPropertyResearchFile updatedResearchFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var filePropertyRepository = helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyResearchFile>())).Callback<PimsPropertyResearchFile>(x => updatedResearchFileProperty = x).Returns(researchFile.PimsPropertyResearchFiles.FirstOrDefault());

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
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
            updatedProperty.PropertyDataSourceEffectiveDate.Should().BeCloseTo(System.DateTime.Now);
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsPropertyOfInterest.Should().Be(true);

            filePropertyRepository.Verify(x => x.GetByResearchFileId(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            propertyRepository.Verify(x => x.GetByPid(It.IsAny<int>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PIN_NewProperty_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);
            var service = helper.Create<ResearchFileService>(user);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, 54321);
            property.Pid = null;
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
            PimsPropertyResearchFile updatedResearchFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var filePropertyRepository = helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyResearchFile>())).Callback<PimsPropertyResearchFile>(x => updatedResearchFileProperty = x).Returns(researchFile.PimsPropertyResearchFiles.FirstOrDefault());

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPin(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
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

            filePropertyRepository.Verify(x => x.GetByResearchFileId(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetRowVersion(It.IsAny<long>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            propertyRepository.Verify(x => x.GetByPin(It.IsAny<int>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
        }
        #endregion
    }
}
