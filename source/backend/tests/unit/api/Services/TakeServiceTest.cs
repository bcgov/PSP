using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
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
        public void GetById_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var repo = this._helper.GetService<Mock<ITakeRepository>>();
            repo.Setup(x => x.GetById(It.IsAny<long>()));

            // Act
            var result = service.GetById(1);

            // Assert
            repo.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
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
        public void Add_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.AddTake(It.IsAny<PimsTake>()));

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(new PimsProperty());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.AddAcquisitionPropertyTake(1, new PimsTake());

            // Assert
            takeRepository.Verify(x => x.AddTake(It.IsAny<PimsTake>()), Times.Once);
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.AddAcquisitionPropertyTake(1, new PimsTake());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_InvalidStatus_AcquisitionFile_Complete()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.AddTake(It.IsAny<PimsTake>()));

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.AddAcquisitionPropertyTake(1, new PimsTake());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
        }

        [Fact]
        public void Add_CompleteTake_No_Date()
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
            Action act = () => service.AddAcquisitionPropertyTake(1, completedTake);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("A completed take must have a completion date.");
        }

        [Fact]
        public void Add_CompleteTake_LandActType_No_EndDt()
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
            Action act = () => service.AddAcquisitionPropertyTake(1, completedTake);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("'Crown Grant' and 'Transfer' Land Acts cannot have an end date.");
        }

        [Fact]
        public void Add_AcquisitionFile_Active_Update_CompleteTake_Admin_Success()
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
            takeRepository.Setup(x => x.UpdateTake(It.IsAny<PimsTake>())).Returns(completedTake);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.AddAcquisitionPropertyTake(1, new PimsTake());

            // Assert
            takeRepository.Verify(x => x.AddTake(It.IsAny<PimsTake>()), Times.Once);
        }

        [Fact]
        public void Add_AcquisitionFile_Active_Update_CompleteTake_Error()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);

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
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsTake>() { completedTake });
            takeRepository.Setup(x => x.UpdateTake(It.IsAny<PimsTake>())).Returns(completedTake);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.AddAcquisitionPropertyTake(1, new PimsTake() { TakeId = 100 });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        public static IEnumerable<object[]> takesAddTestParameters = new List<object[]>() {
            //new object[] { new List<PimsTake>(), false }, // No takes should be core inventory
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="CANCELLED" }}, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="INPROGRESS"  }}, false , false},
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", CompletionDt = new DateOnly() }}, true, true },
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", CompletionDt = new DateOnly() }}, false , false},
        }.ToArray();

        [Theory, MemberData(nameof(takesAddTestParameters))]
        public void Add_Success_Transfer_MultipleTakes_Core(List<PimsTake> takes, bool solverResult, bool expectTransfer)
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.UpdateTake(It.IsAny<PimsTake>()));
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsTake>());

            var takeInteractionSolver = this._helper.GetService<Mock<ITakeInteractionSolver>>();
            takeInteractionSolver.Setup(x => x.ResultsInOwnedProperty(It.IsAny<IEnumerable<PimsTake>>())).Returns(solverResult);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(new PimsProperty() { PropertyId = 1 });
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            var acqStatusSolver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            acqStatusSolver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.AddAcquisitionPropertyTake(1, takes.FirstOrDefault());

            var completedCount = takes.Count(x => x.TakeStatusTypeCode == "COMPLETE");

            // Assert
            takeRepository.Verify(x => x.AddTake(takes.FirstOrDefault()), Times.Once);
            takeInteractionSolver.Verify(x => x.ResultsInOwnedProperty(It.IsAny<List<PimsTake>>()), completedCount > 0 ? Times.Once : Times.Never);
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), true), expectTransfer ? Times.Once : Times.Never);
        }

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);
            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x =>
                x.UpdateTake(It.IsAny<PimsTake>()));

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(new PimsProperty());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.UpdateAcquisitionPropertyTake(1, new PimsTake());

            // Assert
            takeRepository.Verify(x => x.UpdateTake(It.IsAny<PimsTake>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTake(1, new PimsTake());

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
                x.UpdateTake(It.IsAny<PimsTake>()));

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTake(1, new PimsTake());

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
            Action act = () => service.UpdateAcquisitionPropertyTake(1, completedTake);

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
            Action act = () => service.UpdateAcquisitionPropertyTake(1, completedTake);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("'Crown Grant' and 'Transfer' Land Acts cannot have an end date.");
        }

        [Fact]
        public void Update_AcquisitionFile_Active_Update_CompleteTake_Admin_Success()
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
            takeRepository.Setup(x => x.UpdateTake(It.IsAny<PimsTake>())).Returns(completedTake);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.UpdateAcquisitionPropertyTake(1, new PimsTake());

            // Assert
            Assert.NotNull(result);
            takeRepository.Verify(x => x.UpdateTake(It.IsAny<PimsTake>()), Times.Once);
        }

        [Fact]
        public void Update_AcquisitionFile_Active_Update_CompleteTake_Error()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView);

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
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsTake>() { completedTake });
            takeRepository.Setup(x => x.UpdateTake(It.IsAny<PimsTake>())).Returns(completedTake);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateAcquisitionPropertyTake(1, new PimsTake() { TakeId = 100 });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
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
                x.UpdateTake(It.IsAny<PimsTake>()));
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsTake>());

            var takeInteractionSolver = this._helper.GetService<Mock<ITakeInteractionSolver>>();
            takeInteractionSolver.Setup(x => x.ResultsInOwnedProperty(It.IsAny<IEnumerable<PimsTake>>())).Returns(solverResult);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(new PimsProperty() { PropertyId = 1 });
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString() });

            var acqStatusSolver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            acqStatusSolver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.UpdateAcquisitionPropertyTake(1, takes.FirstOrDefault());

            var completedCount = takes.Count(x => x.TakeStatusTypeCode == "COMPLETE");

            // Assert
            takeRepository.Verify(x => x.UpdateTake(takes.FirstOrDefault()), Times.Once);
            takeInteractionSolver.Verify(x => x.ResultsInOwnedProperty(It.IsAny<List<PimsTake>>()), completedCount > 0 ? Times.Once : Times.Never);
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), true), expectTransfer ? Times.Once : Times.Never);
        }

        [Fact]
        public void Delete_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.DeleteAcquisitionPropertyTake(1, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Delete_AcquisitionFile_Active_DeleteCompleteTake_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView, Permissions.SystemAdmin);

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(
                new PimsAcquisitionFile()
                {
                    AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
                }
            );
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(
                EntityHelper.CreateProperty(1)
            );

            PimsTake completedTake = new()
            {
                TakeId = 100,
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.COMPLETE.ToString(),
                PropertyAcquisitionFile = new PimsPropertyAcquisitionFile(),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                completedTake
            );
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsTake>() {
                completedTake
            }
            );
            takeRepository.Setup(x => x.TryDeleteTake(It.IsAny<long>())).Returns(true);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();

            // Act
            var deleted = service.DeleteAcquisitionPropertyTake(1, new List<UserOverrideCode>() { UserOverrideCode.DeleteCompletedTake });

            // Assert
            deleted.Should().BeTrue();
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), false), Times.Once);
        }

        [Fact]
        public void Delete_AcquisitionFile_Active_DeleteCompleteTake_Success_Owned()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.PropertyView, Permissions.AcquisitionFileView, Permissions.SystemAdmin);

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(
                new PimsAcquisitionFile()
                {
                    AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
                }
            );
            acqRepository.Setup(x => x.GetProperty(It.IsAny<long>())).Returns(
                EntityHelper.CreateProperty(1)
            );

            PimsTake completedTake = new()
            {
                TakeId = 100,
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.COMPLETE.ToString(),
                PropertyAcquisitionFile = new PimsPropertyAcquisitionFile(),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                completedTake
            );
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsTake>() {
                completedTake
            }
            );
            takeRepository.Setup(x => x.TryDeleteTake(It.IsAny<long>())).Returns(true);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            var takeSolver = this._helper.GetService<Mock<ITakeInteractionSolver>>();
            takeSolver.Setup(x => x.ResultsInOwnedProperty(It.IsAny<IEnumerable<PimsTake>>())).Returns(true);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();

            // Act
            var deleted = service.DeleteAcquisitionPropertyTake(1, new List<UserOverrideCode>() { UserOverrideCode.DeleteCompletedTake });

            // Assert
            deleted.Should().BeTrue();
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), true), Times.Once);
        }

        [Fact]
        public void Delete_AcquisitionFile_Active_DeleteCompleteTake_NotAdmin()
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
                PropertyAcquisitionFile = new PimsPropertyAcquisitionFile(),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                completedTake
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.DeleteAcquisitionPropertyTake(1, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
        }

        [Fact]
        public void Delete_AcquisitionFile_Active_DeleteTake_CompleteFile_NotAdmin()
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
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.INPROGRESS.ToString(),
                PropertyAcquisitionFile = new PimsPropertyAcquisitionFile(),
            };

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                completedTake
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.DeleteAcquisitionPropertyTake(1, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
        }

        [Fact]
        public void Delete_AcquisitionFile_Active_DeleteTake_IsRetired()
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
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.INPROGRESS.ToString(),
            };

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var property = EntityHelper.CreateProperty(1);
            property.IsRetired = true;
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            completedTake.PropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { Property = property };
            takeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                completedTake
            );
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(
                new List<PimsTake>() { completedTake }
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.DeleteAcquisitionPropertyTake(1, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("You cannot delete a take from a retired property.");
        }

        [Fact]
        public void Delete_InvalidStatus_AcquisitionFile_Active_DeleteTake_CompleteDisposition()
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
                TakeStatusTypeCode = AcquisitionTakeStatusTypes.INPROGRESS.ToString(),
            };

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var property = EntityHelper.CreateProperty(1);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.DispositionFileStatusTypeCode = DispositionFileStatusTypes.COMPLETE.ToString();
            property.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>() { new PimsDispositionFileProperty() { DispositionFile = dispFile } };
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            completedTake.PropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { Property = property };
            takeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                completedTake
            );
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(
                new List<PimsTake>() { completedTake }
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.DeleteAcquisitionPropertyTake(1, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("You cannot delete a take that has a completed disposition attached to the same property.");
        }

        public static IEnumerable<object[]> deleteTakeTestParameters = new List<object[]>() {
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE" }}, UserOverrideCode.DeleteTakeActiveDisposition, true, new List<UserOverrideCode>() },
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE" }}, UserOverrideCode.DeleteCompletedTake, false, new List<UserOverrideCode>()},
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", TakeId = 1, }}, UserOverrideCode.DeleteLastTake, false, new List<UserOverrideCode>()},
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", TakeId = 1, }, new PimsTake() { TakeStatusTypeCode = "ACTIVE", TakeId = 2, } }, UserOverrideCode.DeleteLastTake, false, new List<UserOverrideCode>()},
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", TakeId = 1, }, new PimsTake() { TakeStatusTypeCode = "COMPLETE", IsNewLicenseToConstruct = true, LtcEndDt = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TakeId = 2, } }, UserOverrideCode.DeleteLastTake, false, new List<UserOverrideCode>()},
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", TakeId = 1, }, new PimsTake() { TakeStatusTypeCode = "COMPLETE", TakeId = 2, } }, UserOverrideCode.DeleteCompletedTake, false, new List<UserOverrideCode>()},
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE" }}, null, true, new List<UserOverrideCode>() { UserOverrideCode.DeleteTakeActiveDisposition } },
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE" }}, null, false, new List<UserOverrideCode>() { UserOverrideCode.DeleteCompletedTake } },
            new object[] { new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode="COMPLETE", TakeId = 1, }}, null, false, new List<UserOverrideCode>() { UserOverrideCode.DeleteLastTake } },
        }.ToArray();
        [Theory, MemberData(nameof(deleteTakeTestParameters))]
        public void Delete_UserOverride(List<PimsTake> takes, UserOverrideCode expectedOverride, bool hasDisposition, List<UserOverrideCode> userOverrideCodes)
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin, Permissions.AcquisitionFileView);

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetByAcquisitionFilePropertyId(It.IsAny<long>())).Returns(
                new PimsAcquisitionFile()
                {
                    AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
                }
            );

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var property = EntityHelper.CreateProperty(1);
            if (hasDisposition)
            {
                var dispFile = EntityHelper.CreateDispositionFile();
                property.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>() { new PimsDispositionFileProperty() { DispositionFile = dispFile } };
            }
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takes.FirstOrDefault().PropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { Property = property };
            takeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                takes.FirstOrDefault()
            );
            takeRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(
                takes
            );

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditTakes(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.DeleteAcquisitionPropertyTake(1, userOverrideCodes);

            // Assert
            if (expectedOverride != null)
            {
                var exception = act.Should().Throw<UserOverrideException>().Which;
                exception.UserOverride.Should().Be(expectedOverride);
            } else
            {
                var result = act.Should().NotThrow();
            }
        }
    }
}
