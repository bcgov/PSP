using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Moq;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class ResearchServiceTest
    {

        #region Tests
        #region GetPage
        [Fact]
        public void GetPage()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);

            var researchFile = EntityHelper.CreateResearchFile(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(researchFile);

            var service = helper.Create<ResearchFileService>();
            var researchRepository = helper.GetService<Mock<IResearchFileRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var researchFile = EntityHelper.CreateResearchFile(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(researchFile);

            var service = helper.Create<ResearchFileService>();
            var researchRepository = helper.GetService<Mock<IResearchFileRepository>>();
            researchRepository.Setup(x => x.GetPage(It.IsAny<ResearchFilter>()));

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetPage(new ResearchFilter()));
            researchRepository.Verify(x => x.GetPage(It.IsAny<ResearchFilter>()), Times.Never);
        }

        #endregion

        #region UpdateProperties
        [Fact]
        public void UpdateProperties_Delete()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit);

            var researchFile = EntityHelper.CreateResearchFile(1);
            var pimsPropertyResearchFile = new PimsPropertyResearchFile() { Property = new PimsProperty() { RegionCode = 1 } };
            pimsPropertyResearchFile.PimsPrfPropResearchPurposeTypes = new List<PimsPrfPropResearchPurposeType>() { new PimsPrfPropResearchPurposeType() { } };
            researchFile.PimsPropertyResearchFiles.Add(pimsPropertyResearchFile);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(researchFile);

            var service = helper.Create<ResearchFileService>();
            var researchRepository = helper.GetService<Mock<IResearchFileRepository>>();
            researchRepository.Setup(x => x.GetPage(It.IsAny<ResearchFilter>()));
            researchRepository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(2);
            var researchFilePropertyRepository = helper.GetService<Mock<IResearchFilePropertyRepository>>();
            researchFilePropertyRepository.Setup(x => x.Delete(It.IsAny<PimsPropertyResearchFile>()));
            researchFilePropertyRepository.Setup(x => x.GetByResearchFileId(It.IsAny<long>())).Returns(researchFile.PimsPropertyResearchFiles.ToList());

            // Act
            researchFile.PimsPropertyResearchFiles.Clear();
            var updatedLease = service.UpdateProperties(researchFile);

            // Assert
            researchFilePropertyRepository.Verify(x => x.GetByResearchFileId(It.IsAny<long>()), Times.Once);
            researchFilePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyResearchFile>()), Times.Once);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileView);
            var service = helper.Create<ResearchFileService>(user);

            var mapper = helper.GetService<IMapper>();
            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ResearchFileService>(user);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

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
            var user = PrincipalHelper.CreateForPermission(Permissions.ResearchFileEdit, Permissions.ResearchFileEdit);
            var service = helper.Create<ResearchFileService>(user);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ResearchFileService>(user);

            var researchFile = EntityHelper.CreateResearchFile(1);

            var repository = helper.GetService<Mock<IResearchFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsResearchFile>())).Returns(researchFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(researchFile);

            // Act
            Action act = () => service.Update(researchFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsResearchFile>()), Times.Never);
        }
        #endregion

        #endregion
    }
}
