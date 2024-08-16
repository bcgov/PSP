using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchFileServiceTest
    {
        private readonly TestHelper _helper;

        public ResearchFileServiceTest()
        {
            this._helper = new TestHelper();
        }

        private ResearchFileService CreateResearchFileServiceWithPermissions(params Permissions[] permissions)
        {
            ClaimsPrincipal user = PrincipalHelper.CreateForPermission(permissions);
            user.AddClaim("idir_username", "TestIdirUsername@domain");
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<ResearchFileService>();
        }

        #region Tests
        #region GetPage
        [Fact]
        public void GetPage()
        {
            // Arrange
            var researchFile = EntityHelper.CreateResearchFile(1);

            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileView);
            var researchRepository = this._helper.GetService<Mock<IResearchFileRepository>>();
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
            var service = this.CreateResearchFileServiceWithPermissions();

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetPage(new ResearchFilter()));
        }

        #endregion

        #region Properties
        [Fact]
        public void GetProperties_ByFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileView);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            repository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(new List<PimsPropertyResearchFile>());

            // Act
            Action act = () => service.GetProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetProperties_ByFileId_Success()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileView, Permissions.PropertyView);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            propertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(new List<PimsPropertyResearchFile>());

            // Act
            var properties = service.GetProperties(1);

            // Assert
            propertyRepository.Verify(x => x.GetAllByResearchFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetProperties_ByFileId_Success_Reproject()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileView, Permissions.PropertyView);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            propertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>()))
                .Returns(new List<PimsPropertyResearchFile>() { new() { Property = new() { Location = new Point(1, 1) } } });

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyResearchFile>>()))
                .Returns<List<PimsPropertyResearchFile>>(x => x);

            // Act
            var properties = service.GetProperties(1);

            // Assert
            propertyRepository.Verify(x => x.GetAllByResearchFileId(It.IsAny<long>()), Times.Once);
            propertyService.Verify(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyResearchFile>>()), Times.Once);
            properties.FirstOrDefault().Property.Location.Coordinates.Should().BeEquivalentTo(new Coordinate[] { new Coordinate(1, 1) });
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

            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);
            var researchRepository = this._helper.GetService<Mock<IResearchFileRepository>>();
            researchRepository.Setup(x => x.GetPage(It.IsAny<ResearchFilter>()));
            researchRepository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(2);
            var researchFilePropertyRepository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            researchFilePropertyRepository.Setup(x => x.Delete(It.IsAny<PimsPropertyResearchFile>()));
            researchFilePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            // Act
            researchFile.PimsPropertyResearchFiles.Clear();
            researchFile.ConcurrencyControlNumber++;
            var updatedLease = service.UpdateProperties(researchFile, new List<UserOverrideCode>());

            // Assert
            researchFilePropertyRepository.Verify(x => x.GetAllByResearchFileId(It.IsAny<long>()), Times.Once);
            researchFilePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PID_Success()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Internal_Id = 1, Property = property } };

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);

            var filePropertyRepository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()));
            propertyService.Setup(x => x.UpdateFilePropertyLocation<PimsPropertyResearchFile>(It.IsAny<PimsPropertyResearchFile>(), It.IsAny<PimsPropertyResearchFile>()));

            // Act
            service.UpdateProperties(researchFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
            propertyService.Verify(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsPropertyResearchFile>(It.IsAny<PimsPropertyResearchFile>(), It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PIN_Success()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, 54321);
            property.Pid = null;
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Internal_Id = 1, Property = property } };

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPin(It.IsAny<int>(), true)).Returns(property);

            var filePropertyRepository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()));
            propertyService.Setup(x => x.UpdateFilePropertyLocation<PimsPropertyResearchFile>(It.IsAny<PimsPropertyResearchFile>(), It.IsAny<PimsPropertyResearchFile>()));

            // Act
            service.UpdateProperties(researchFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
            propertyService.Verify(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsPropertyResearchFile>(It.IsAny<PimsPropertyResearchFile>(), It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PID_NewProperty_Success()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.Location.SRID = SpatialReference.WGS84;
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            PimsPropertyResearchFile updatedResearchFileProperty = null;

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var filePropertyRepository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyResearchFile>())).Callback<PimsPropertyResearchFile>(x => updatedResearchFileProperty = x).Returns(researchFile.PimsPropertyResearchFiles.FirstOrDefault());

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>())).Returns(new PimsProperty()
            {
                PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now),
                PropertyDataSourceTypeCode = "PMBC",
                PropertyTypeCode = "UNKNOWN",
                PropertyStatusTypeCode = "UNKNOWN",
                SurplusDeclarationTypeCode = "UNKNOWN",
                RegionCode = 1
            });
            propertyService.Setup(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyResearchFile>())).Returns<PimsPropertyResearchFile>(x => x);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();

            // Act
            service.UpdateProperties(researchFile, new List<UserOverrideCode>());

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedResearchFileProperty.Property;
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().Be(DateOnly.FromDateTime(System.DateTime.Now));
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsOwned.Should().Be(false);

            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_PIN_NewProperty_Success()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);

            var researchFile = EntityHelper.CreateResearchFile();
            researchFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, 54321);
            property.Location.SRID = SpatialReference.WGS84;
            property.Pid = null;
            researchFile.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { Property = property } };

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
            PimsPropertyResearchFile updatedResearchFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            var filePropertyRepository = this._helper.GetService<Mock<IResearchFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyResearchFile>())).Callback<PimsPropertyResearchFile>(x => updatedResearchFileProperty = x).Returns(researchFile.PimsPropertyResearchFiles.FirstOrDefault());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPin(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>())).Returns(new PimsProperty()
            {
                PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now),
                PropertyDataSourceTypeCode = "PMBC",
                PropertyTypeCode = "UNKNOWN",
                PropertyStatusTypeCode = "UNKNOWN",
                SurplusDeclarationTypeCode = "UNKNOWN",
                RegionCode = 1
            });
            propertyService.Setup(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyResearchFile>())).Returns<PimsPropertyResearchFile>(x => x);

            // Act
            service.UpdateProperties(researchFile, new List<UserOverrideCode>());

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedResearchFileProperty.Property;
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsOwned.Should().Be(false);

            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileView);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
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
            var service = this.CreateResearchFileServiceWithPermissions();

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

            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);
            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = this._helper.GetService<Mock<IResearchFileRepository>>();
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
            var service = this.CreateResearchFileServiceWithPermissions();

            var researchFile = EntityHelper.CreateResearchFile(1);

            // Act
            Action act = () => service.Update(researchFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_ThrowIf_Null()
        {
            // Arrange
            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);

            // Act
            Action act = () => service.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_StatusChange_CreatesNote()
        {
            // Arrange
            const long researchFileId = 1;
            PimsResearchFileStatusType newStatusType = new PimsResearchFileStatusType() { ResearchFileStatusTypeCode = "STATUS_B", Description = "Status B Description" };
            PimsResearchFileStatusType oldStatusType = new PimsResearchFileStatusType() { ResearchFileStatusTypeCode = "STATUS_A", Description = "Status A Description" };
            var updatedResearchFileRequest = new PimsResearchFile()
            {
                ResearchFileId = researchFileId,
                ResearchFileStatusTypeCode = newStatusType.ResearchFileStatusTypeCode,
            };

            var existingResearchFile = new PimsResearchFile()
            {
                ResearchFileId = researchFileId,
                ResearchFileStatusTypeCode = oldStatusType.ResearchFileStatusTypeCode,
                ResearchFileStatusTypeCodeNavigation = oldStatusType,
            };

            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);
            var researchRepositoryMock = this._helper.GetService<Mock<IResearchFileRepository>>();
            researchRepositoryMock.Setup(x => x.GetById(researchFileId)).Returns(existingResearchFile);
            researchRepositoryMock.Setup(x => x.Update(updatedResearchFileRequest)).Returns(updatedResearchFileRequest);

            var lookupRepositoryMock = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepositoryMock.Setup(x => x.GetAllResearchFileStatusTypes()).Returns(new List<PimsResearchFileStatusType>() { newStatusType, oldStatusType });
            var noteEntityRepositoryMock = this._helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var updatedResearchFile = service.Update(updatedResearchFileRequest);

            // Assert
            noteEntityRepositoryMock.Verify(x => x.Add(It.IsAny<PimsResearchFileNote>()), Times.Once());
        }

        [Fact]
        public void Update_NoStatusChange_DoesNotCreateNote()
        {
            // Arrange
            const long researchFileId = 1;
            PimsResearchFileStatusType sameStatusType = new PimsResearchFileStatusType() { ResearchFileStatusTypeCode = "STATUS_A", Description = "Status B Description" };
            var updatedResearchFileRequest = new PimsResearchFile()
            {
                ResearchFileId = researchFileId,
                ResearchFileStatusTypeCode = sameStatusType.ResearchFileStatusTypeCode,
            };

            var existingResearchFile = new PimsResearchFile()
            {
                ResearchFileId = researchFileId,
                ResearchFileStatusTypeCode = sameStatusType.ResearchFileStatusTypeCode,
                ResearchFileStatusTypeCodeNavigation = sameStatusType,
            };

            var service = this.CreateResearchFileServiceWithPermissions(Permissions.ResearchFileEdit);
            var researchRepositoryMock = this._helper.GetService<Mock<IResearchFileRepository>>();
            researchRepositoryMock.Setup(x => x.GetById(researchFileId)).Returns(existingResearchFile);
            researchRepositoryMock.Setup(x => x.Update(updatedResearchFileRequest)).Returns(updatedResearchFileRequest);

            var lookupRepositoryMock = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepositoryMock.Setup(x => x.GetAllResearchFileStatusTypes()).Returns(new List<PimsResearchFileStatusType>() { sameStatusType });
            var noteEntityRepositoryMock = this._helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var updatedResearchFile = service.Update(updatedResearchFileRequest);

            // Assert
            noteEntityRepositoryMock.Verify(x => x.Add(It.IsAny<PimsResearchFileNote>()), Times.Never());
        }
        #endregion

        #endregion
    }
}
