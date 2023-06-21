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
            _helper = new TestHelper();
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionView);
            var repo = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
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
            var service = CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.Update(new PimsCompensationRequisition());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_BadRequest_EntityIsNull()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);

            // Act
            Action act = () => service.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_Success_Inserts_StatusChanged_Note()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1 , AcquisitionFileId=1, IsDraft = false });
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId=1, IsDraft = true });

            // Act
            var result = service.Update(new PimsCompensationRequisition() {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = false }
            );

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Draft' to 'Final'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_Skips_StatusChanged_Note()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

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
                IsDraft = true
            }
            );

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Draft' to 'Final'"))), Times.Never);
        }

        [Fact]
        public void Update_Status_BackToDraft_NoPermission()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            // Act
            Action act = () => service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Status_BackToNull_NoPermission()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            // Act
            Action act = () => service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = null
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Status_BackToDraft_AuthorizedAdmin()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit, Permissions.AdminUsers);
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

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
                IsDraft = true
            });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Final' to 'Draft'"))), Times.Once);
        }

        [Fact]
        public void Update_Status_BackToNull_AuthorizedAdmin()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit, Permissions.AdminUsers);
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

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
                IsDraft = null
            });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Final' to 'No Status'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_Skips_StatusChanged_Note_FromNoStatus()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true });;
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            // Act
            var result = service.Update(new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true
            }
            );

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'No Status' to 'Draft'"))), Times.Once);
        }

        [Fact]
        public void Delete_NoPermission()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.DeleteCompensation(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Delete_Success()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionDelete);
            var repo = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repo.Setup(x => x.TryDelete(It.IsAny<long>()));

            // Act
            var result = service.DeleteCompensation(1);

            // Assert
            repo.Verify(x => x.TryDelete(It.IsAny<long>()), Times.Once);
        }

        private CompensationRequisitionService CreateCompRequisitionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.Create<CompensationRequisitionService>(user);
        }
    }
}
