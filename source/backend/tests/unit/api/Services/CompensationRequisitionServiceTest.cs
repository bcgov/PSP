using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "compensation-requisition")]
    [ExcludeFromCodeCoverage]

    public class CompensationRequisitionServiceTest
    {
        private readonly TestHelper _helper;

        public CompensationRequisitionServiceTest()
        {
            this._helper = new TestHelper();
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionView);
            var repo = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repo.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsCompensationRequisition { Internal_Id = 1 });

            // Act
            var result = service.GetById(1);

            // Assert
            repo.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.Update(new PimsCompensationRequisition());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_BadRequest_EntityIsNull()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);

            // Act
            Action act = () => service.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_Success_Inserts_StatusChanged_Note()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var compensationRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { TotalAllowableCompensation = 100 });

            var currentCompensationStub = new PimsCompensationRequisition
            {
                Internal_Id = 1,
                AcquisitionFileId = 7,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
            };

            compensationRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(currentCompensationStub);

            compensationRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>())).Returns(new PimsCompensationRequisition
            {
                Internal_Id = 1,
                AcquisitionFileId = 7,
                ConcurrencyControlNumber = 2,
                IsDraft = false,
                FinalizedDate = DateTime.UtcNow,
            });

            // Act
            var result = service.Update(
                new PimsCompensationRequisition
                {
                    Internal_Id = 1,
                    AcquisitionFileId = 7,
                    ConcurrencyControlNumber = 2,
                    IsDraft = false,
                });

            // Assert
            result.Should().NotBeNull();
            result.FinalizedDate.Should().NotBeNull();
            compensationRepository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 7
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Draft' to 'Final'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_Skips_StatusChanged_Note()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { TotalAllowableCompensation = 100 });

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true });
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
            });

            // Assert
            result.Should().NotBeNull();
            result.FinalizedDate.Should().BeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Draft' to 'Final'"))), Times.Never);
        }

        [Fact]
        public void Update_Status_BackToDraft_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            // Act
            Action act = () => service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Status_BackToNull_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            // Act
            Action act = () => service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = null,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Status_BackToDraft_AuthorizedAdmin()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit, Permissions.SystemAdmin);
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { TotalAllowableCompensation = 100 });

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
            });

            // Assert
            result.Should().NotBeNull();
            result.FinalizedDate.Should().BeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Final' to 'Draft'"))), Times.Once);
        }

        [Fact]
        public void Update_Status_BackToNull_AuthorizedAdmin()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit, Permissions.SystemAdmin);
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { TotalAllowableCompensation = 100 });

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = null,
            });

            // Assert
            result.Should().NotBeNull();
            result.FinalizedDate.Should().BeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Final' to 'No Status'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_Skips_StatusChanged_Note_FromNoStatus()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { TotalAllowableCompensation = 100 });

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
            });

            // Assert
            result.Should().NotBeNull();
            result.FinalizedDate.Should().BeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'No Status' to 'Draft'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_ValidTotalAllowableCompensation()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { TotalAllowableCompensation = 100 });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 1000 } },
            });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }

        [Fact]
        public void Update_Success_ValidMultipleTotalAllowableCompensation()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { CompensationRequisitionId = 1, TotalAmt = 1000 }, new PimsCompReqFinancial() { CompensationRequisitionId = 2, TotalAmt = 100 } });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 300,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { Internal_Id = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
            });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = false,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 200 } },
            });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }

        [Fact]
        public void Update_Success_TotalAllowableExceededDraft()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile() { TotalAllowableCompensation = 100 });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 1000 } },
            });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }

        [Fact]
        public void Update_Fail_TotalAllowableExceeded()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(new List<PimsCompReqFinancial>() { });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 99,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { Internal_Id = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
            });

            // Act
            // Assert
            Action act = () => service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = false,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 200 } },
            });
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Update_Fail_ValidMultipleTotalAllowableCompensation()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { CompensationRequisitionId = 1, TotalAmt = 1000 },
                new PimsCompReqFinancial() { CompensationRequisitionId = 2, TotalAmt = 100 }, });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 299,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { Internal_Id = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
            });

            // Act
            // Assert
            Action act = () => service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = false,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 200 } },
            });
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Delete_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.DeleteCompensation(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Delete_Success()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionDelete);
            var repo = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repo.Setup(x => x.TryDelete(It.IsAny<long>()));

            // Act
            var result = service.DeleteCompensation(1);

            // Assert
            repo.Verify(x => x.TryDelete(It.IsAny<long>()), Times.Once);
        }

        private CompensationRequisitionService CreateCompRequisitionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<CompensationRequisitionService>(user);
        }
    }
}
