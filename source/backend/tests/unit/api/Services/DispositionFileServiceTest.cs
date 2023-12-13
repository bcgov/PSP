using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetTopologySuite.Geometries;
using NExpect.Interfaces;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
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
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionFileServiceTest
    {
        #region Tests
        private readonly TestHelper _helper;

        public DispositionFileServiceTest()
        {
            this._helper = new TestHelper();
        }

        private DispositionFileService CreateDispositionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<DispositionFileService>(user);
        }

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispFile);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Properties

        [Fact]
        public void GetProperties_ByDispositionFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyDispositionFile>());

            // Act
            Action act = () => service.GetProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetProperties_ByDispositionFileId_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView, Permissions.PropertyView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyDispositionFile>());

            // Act
            var properties = service.GetProperties(1);

            // Assert
            repository.Verify(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetProperties_ByDispositionFileId_Success_Reproject()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView, Permissions.PropertyView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyDispositionFile>() { new PimsPropertyDispositionFile() { Property = new PimsProperty() { Location = new Point(1, 1) } } });
            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(1, 1));
            // Act
            var properties = service.GetProperties(1);

            // Assert
            repository.Verify(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            properties.FirstOrDefault().Property.Location.Coordinates.Should().BeEquivalentTo(new Coordinate[] { new Coordinate(1, 1) });
        }
        #endregion

        #region GetLastUpdate

        [Fact]
        public void GetLastUpdate_ByDispositionFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetLastUpdateBy(It.IsAny<long>())).Returns(new LastUpdatedByModel());

            // Act
            Action act = () => service.GetLastUpdateInformation(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetLastUpdate_ByDispositionFileId_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetLastUpdateBy(It.IsAny<long>())).Returns(new LastUpdatedByModel());

            // Act
            var properties = service.GetLastUpdateInformation(1);

            // Assert
            repository.Verify(x => x.GetLastUpdateBy(It.IsAny<long>()), Times.Once);
        }
        #endregion

        #region GetPage
        [Fact]
        public void GetPage_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetPageDeep(It.IsAny<DispositionFilter>())).Returns(new Paged<PimsDispositionFile>(new[] { dispFile }));

            // Act
            var result = service.GetPage(new DispositionFilter());

            // Assert
            repository.Verify(x => x.GetPageDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetPage_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            // Act
            Action act = () => service.GetPage(new DispositionFilter());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetTeamMembers
        [Fact]
        public void GetTeamMembers_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView, Permissions.ContactView);

            var dispFile = EntityHelper.CreateDispositionFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var org = EntityHelper.CreateOrganization(1, "tester org");
            List<PimsDispositionFileTeam> allTeamMembers = new()
            {
                new() { DispositionFileId = dispFile.Internal_Id, PersonId = person.Internal_Id, Person = person },
                new() { DispositionFileId = dispFile.Internal_Id, OrganizationId = org.Internal_Id, Organization = org }
            };

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetTeamMembers()).Returns(allTeamMembers);

            // Act
            var result = service.GetTeamMembers();

            // Assert
            repository.Verify(x => x.GetTeamMembers(), Times.Once);
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetTeamMembers_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            // Act
            Action act = () => service.GetTeamMembers();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #endregion
    }
}
