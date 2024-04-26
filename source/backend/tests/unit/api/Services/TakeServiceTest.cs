using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        public void Update_TakeComplete_No_Date()
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
        public void Update_InvalidStatus_AcquisitionFile_Complete()
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
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
        }

        [Fact]
        public void Update_CompleteTake_No_Date()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(
                new PimsAcquisitionFile()
                {
                    AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
                }
            );

            PimsTake completedTake = new()
            {
                TakeId = 100,
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.COMPLETE.ToString(),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(
                new List<PimsTake>() { completedTake }
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>() { completedTake });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("A completed take must have a completion date.");
        }

        [Fact]
        public void Update_CompleteTake_LandActType_No_EndDt()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(
                new PimsAcquisitionFile()
                {
                    AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
                }
            );

            PimsTake completedTake = new()
            {
                TakeId = 100,
                CompletionDt = DateOnly.FromDateTime(DateTime.Now),
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.COMPLETE.ToString(),
                IsNewLandAct = true,
                LandActTypeCode = LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.ToString(),
                LandActEndDt = DateOnly.FromDateTime(DateTime.Now),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(
                new List<PimsTake>() { completedTake }
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>() { completedTake });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("'Crown Grant' and 'Transfer' Land Acts cannot have an end date.");
        }

        [Fact]
        public void Update_InvalidStatus_AcquisitionFile_Active_DeleteCompleteTake_NotAdmin()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(
                new PimsAcquisitionFile() { 
                    AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() 
                }
            );

            PimsTake completedTake = new()
            {
                TakeId = 100,
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.COMPLETE.ToString(),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(
                new List<PimsTake>() { completedTake }
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
        }

        [Fact]
        public void Update_AcquisitionFile_Active_DeleteCompleteTake_Admin_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin, Permissions.PropertyView, Permissions.AcquisitionFileView);

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(new PimsProperty() { PropertyId = 1 });
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            PimsTake completedTake = new()
            {
                TakeId = 100,
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.COMPLETE.ToString(),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsTake>() { completedTake });
            //takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(takes);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>());

            // Assert
            Assert.NotNull(result);
            takeRepository.Verify(x => x.UpdateAcquisitionPropertyTakes(1, new List<PimsTake>()), Times.Once);
        }

        public static IEnumerable<object[]> takesTestParameters = new List<object[]>() {
            //new object[] { new List<PimsTake>(), false }, // No takes should be core inventory
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="CANCELLED" }}, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="INPROGRESS"  }}, false , false},
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", CompletionDt = new DateOnly() }}, true, true },
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", CompletionDt = new DateOnly() }}, false , false},
        }.ToArray();

        [Theory, MemberData(nameof(takesTestParameters))]
        public void Update_Success_Transfer_MultipleTakes_Core(List<PimsTake> takes, bool solverResult, bool expectTransfer)
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(takes);

            var takeInteractionSolver = this._helper.GetMock<ITakeInteractionSolver>();
            takeInteractionSolver.Setup(x => x.ResultsInOwnedProperty(It.IsAny<IEnumerable<PimsTake>>())).Returns(solverResult);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(new PimsProperty() { PropertyId = 1 });
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            var acqStatusSolver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            acqStatusSolver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.UpdateAcquisitionPropertyTakes(1, takes);

            var completedCount = takes.Count(x => x.TakeStatusTypeCode == "COMPLETE");

            // Assert
            takeRepository.Verify(x => x.UpdateAcquisitionPropertyTakes(1, takes), Times.Once);
            takeInteractionSolver.Verify(x => x.ResultsInOwnedProperty(takes), completedCount > 0 ? Times.Once : Times.Never);
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), true), expectTransfer ? Times.Once : Times.Never);
        }
    }
}
