using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Exceptions;
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
    [Trait("area", "take")]
    [ExcludeFromCodeCoverage]
    public class TakeServiceTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;

        public TakeServiceTest()
        {
            this._helper = new TestHelper();
        }

        private TakeService CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<TakeService>(user);
        }

        [Fact]
        public void GetByFileId_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var repo = this._helper.GetService<Mock<ITakeRepository>>();
            repo.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()));

            // Act
            var result = service.GetByFileId(1);

            // Assert
            repo.Verify(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetByFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.GetByFileId(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetByPropertyId_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByAcqPropertyId(It.IsAny<long>(), It.IsAny<long>()));

            // Act
            var result = service.GetByPropertyId(1L, 2L);

            // Assert
            takeRepository.Verify(x => x.GetAllByAcqPropertyId(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetByPropertyId_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.GetByPropertyId(1, 2);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetCountByPropertyId_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var repo = this._helper.GetService<Mock<ITakeRepository>>();
            repo.Setup(x => x.GetCountByPropertyId(It.IsAny<long>()));

            // Act
            var result = service.GetCountByPropertyId(1);

            // Assert
            repo.Verify(x => x.GetCountByPropertyId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetCountByPropertyId_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.GetCountByPropertyId(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(new PimsProperty());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>());

            // Assert
            takeRepository.Verify(x => x.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_InvalidStatus()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        public static IEnumerable<object[]> takesTestParameters = new List<object[]>() {
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true, false }, // core inventory takes priority over other interest
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActEndDt = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) }, new PimsTake() { IsNewLicenseToConstruct = true } }, false, false }, // should ignore any expired takes
            new object[] { new List<PimsTake>(), false, true }, // No takes should be core inventory
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActEndDt = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) } }, true }, // only expired takes is the same as no takes
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 15" } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 16" } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 17" } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "NOI" } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 66" } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Crown Grant (New)" } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewInterestInSrw = true } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLicenseToConstruct = true } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsThereSurplus = true } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true, IsAcquiredForInventory = false } }, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = false, IsAcquiredForInventory = true } }, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 16", IsNewHighwayDedication = true, IsAcquiredForInventory = false } }, false },
        }.ToArray();

        [Theory, MemberData(nameof(takesTestParameters))]
        public void Update_Success_Transfer_MultipleTakes_Core(List<PimsTake> takes, bool expectedIsOwned)
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            var acqStatusSolver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            acqStatusSolver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            //var takeInteractionSolver = this._helper.GetService<Mock<ITakeInteractionSolver>>();
            //takeInteractionSolver.Setup(x => x.ResultsInOwnedProperty(It.IsAny<IEnumerable<PimsTake>>())).Returns(true);

            // Act
            var result = service.UpdateAcquisitionPropertyTakes(1, takes);

            // Assert
            takeRepository.Verify(x => x.UpdateAcquisitionPropertyTakes(1, takes), Times.Once);
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), expectedIsOwned), Times.Once);
        }
    }
}
