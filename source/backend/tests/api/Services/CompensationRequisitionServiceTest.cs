using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Models.Cdogs;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Sprache;
using Xunit;
using FileTypes = Pims.Api.Models.CodeTypes.FileTypes;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "compensation-requisition")]
    [ExcludeFromCodeCoverage]

    public class CompensationRequisitionServiceTest
    {
        private readonly TestHelper _helper;

        public static IEnumerable<object[]> FileTypesDataNoAccess =
            new List<object[]>
            {
                        new object[] { FileTypes.Acquisition, new PimsCompensationRequisition() { AcquisitionFileId = 1, LeaseId = null, }, new NotAuthorizedException() },
                        new object[] { FileTypes.Lease, new PimsCompensationRequisition() { LeaseId = 1, AcquisitionFileId = null, }, new NotAuthorizedException() },
                        new object[] { FileTypes.Research, new PimsCompensationRequisition(), new BadRequestException("Relationship type not valid.") },
                        new object[] { FileTypes.Disposition, new PimsCompensationRequisition(), new BadRequestException("Relationship type not valid.") },
            };

        public static IEnumerable<object[]> FileTypesUpdateNoAccess =
    new List<object[]>
    {
                        new object[] { FileTypes.Acquisition, new NotAuthorizedException() },
                        new object[] { FileTypes.Lease, new NotAuthorizedException() },
                        new object[] { FileTypes.Research, new BadRequestException("Relationship type not valid.") },
                        new object[] { FileTypes.Disposition, new BadRequestException("Relationship type not valid.") },
    };

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
        public void GetAcquisitionProperties_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(12345);
            property.Location = new NetTopologySuite.Geometries.Point(1229480.4045231808, 463288.8298389828) { SRID = SpatialReference.BCALBERS };

            var compReqProperty = new PimsPropAcqFlCompReq()
            {
                Internal_Id = 1,
                CompensationRequisitionId = 1,
                PropertyAcquisitionFileId = 1,
                PropertyAcquisitionFile = new()
                {
                    AcquisitionFileId = 1,
                    PropertyId = property.Internal_Id,
                    Property = property,
                }
            };

            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionView);
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repository.Setup(x => x.GetAcquisitionCompReqPropertiesById(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { compReqProperty.PropertyAcquisitionFile });
            // mock the spatial conversion to lat/long
            var propertyService = _helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyAcquisitionFile>>()))
                .Returns<List<PimsPropertyAcquisitionFile>>(x => x.Select(pp =>
                {
                    pp.Property.Location = new NetTopologySuite.Geometries.Point(-122, 49) { SRID = SpatialReference.WGS84 };
                    return pp;
                }).ToList());

            // Act
            var result = service.GetAcquisitionProperties(1);

            // Assert
            repository.Verify(x => x.GetAcquisitionCompReqPropertiesById(It.IsAny<long>()), Times.Once);
            propertyService.Verify(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyAcquisitionFile>>()), Times.Once);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsPropertyAcquisitionFile>>();
            result.Should().HaveCount(1);
            result.First().Property.Pid.Should().Be(12345);
            // service should perform spatial conversion to Lat/Long so that it can be returned to the frontend
            result.First().Property.Location.SRID.Should().Be(SpatialReference.WGS84);
            result.First().Property.Location.As<NetTopologySuite.Geometries.Point>().X.Should().Be(-122);
            result.First().Property.Location.As<NetTopologySuite.Geometries.Point>().Y.Should().Be(49);
        }

        [Fact]
        public void GetAcquisitionProperties_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetAcquisitionProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetLeaseProperties_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(12345);
            property.Location = new NetTopologySuite.Geometries.Point(1229480.4045231808, 463288.8298389828) { SRID = SpatialReference.BCALBERS };

            var leaseCompReqProperty = new PimsPropLeaseCompReq()
            {
                Internal_Id = 1,
                CompensationRequisitionId = 1,
                PropertyLeaseId = 1,
                PropertyLease = new()
                {
                    LeaseId = 1,
                    PropertyId = property.Internal_Id,
                    Property = property,
                }
            };

            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionView);
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repository.Setup(x => x.GetLeaseCompReqPropertiesById(It.IsAny<long>())).Returns(new List<PimsPropertyLease>() { leaseCompReqProperty.PropertyLease });
            // mock the spatial conversion to lat/long
            var propertyService = _helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyLease>>()))
                .Returns<List<PimsPropertyLease>>(x => x.Select(pp =>
                {
                    pp.Property.Location = new NetTopologySuite.Geometries.Point(-122, 49) { SRID = SpatialReference.WGS84 };
                    return pp;
                }).ToList());

            // Act
            var result = service.GetLeaseProperties(1);

            // Assert
            repository.Verify(x => x.GetLeaseCompReqPropertiesById(It.IsAny<long>()), Times.Once);
            propertyService.Verify(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyLease>>()), Times.Once);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsPropertyLease>>();
            result.Should().HaveCount(1);
            result.First().Property.Pid.Should().Be(12345);
            // service should perform spatial conversion to Lat/Long so that it can be returned to the frontend
            result.First().Property.Location.SRID.Should().Be(SpatialReference.WGS84);
            result.First().Property.Location.As<NetTopologySuite.Geometries.Point>().X.Should().Be(-122);
            result.First().Property.Location.As<NetTopologySuite.Geometries.Point>().Y.Should().Be(49);
        }

        [Fact]
        public void GetLeaseProperties_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetLeaseProperties(1);

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

        [Theory]
        [MemberData(nameof(FileTypesUpdateNoAccess))]
        public void GetCompensationsRequisitions_NoPermissions(FileTypes fileType, Exception exception)
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            Exception ex = Assert.Throws(exception.GetType(), () => service.GetFileCompensationRequisitions(fileType, 1));
        }

        [Fact]
        public void Get_Acquisition_CompensationsRequisitions_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(EntityHelper.CreateAcquisitionFile());

            // Act
            Action act = () => service.GetFileCompensationRequisitions(FileTypes.Acquisition, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetCompensationsRequisitions_Success()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>()
                {
                    new PimsCompensationRequisition(),
                });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetFileCompensationRequisitions(FileTypes.Acquisition, 1);

            // Assert
            repository.Verify(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(FileTypesDataNoAccess))]
        public void AddCompensationsRequisitions_NoPermissions(FileTypes fileType, PimsCompensationRequisition compReq, Exception exception)
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            Exception ex = Assert.Throws(exception.GetType(), () => service.AddCompensationRequisition(fileType, compReq));
        }

        [Fact]
        public void Add_Acquisition_CompensationsRequisitions_NullException()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.AddCompensationRequisition(FileTypes.Acquisition, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_Acquisition_CompensationsRequisitions_BadRequest_Missing_ParentId()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var newCompensationRequisition = new PimsCompensationRequisition()
            {
                AcquisitionFileId = null,
                LeaseId = null,
            };

            // Act
            Action act = () => service.AddCompensationRequisition(FileTypes.Acquisition, newCompensationRequisition);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Add_Acquisition_CompensationsRequisitions_BadRequest_Duplicate_ParentId()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var newCompensationRequisition = new PimsCompensationRequisition()
            {
                AcquisitionFileId = 100,
                LeaseId = 200,
            };

            // Act
            Action act = () => service.AddCompensationRequisition(FileTypes.Acquisition, newCompensationRequisition);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void AddCompensationsRequisitions_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            newCompensationReq.AcquisitionFileId = 100;
            // Act
            Action act = () => service.AddCompensationRequisition(FileTypes.Acquisition, newCompensationReq);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddCompensationsRequisitions_FinalFile_Acquisition()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(false);

            newCompensationReq.AcquisitionFileId = 100;
            // Act
            Action act = () => service.AddCompensationRequisition(FileTypes.Acquisition, newCompensationReq);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void AddCompensationsRequisitions_FinalFile_Lease()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1);
            newCompensationReq.LeaseId = 1;
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<LeaseStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(false);

            newCompensationReq.AcquisitionFileId = 100;
            // Act
            Action act = () => service.AddCompensationRequisition(FileTypes.Lease, newCompensationReq);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void AddCompensationsRequisitions_Success()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            newCompensationReq.AcquisitionFileId = 1;
            newCompensationReq.LeaseId = null;

            // Act
            var result = service.AddCompensationRequisition(FileTypes.Acquisition, newCompensationReq);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }

        [Fact]
        public void Add_Lease_CompensationsRequisitions_NullException()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.AddCompensationRequisition(FileTypes.Lease, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_NoPermission_AcquisitionFile()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.Update(FileTypes.Acquisition, new PimsCompensationRequisition() { CompensationRequisitionId = 1, AcquisitionFileId = 1 });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_NoPermission_LeaseFile()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.Update(FileTypes.Acquisition, new PimsCompensationRequisition() { CompensationRequisitionId = 1, LeaseId = 1 });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_BadRequest_EntityIsNull()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);

            // Act
            Action act = () => service.Update(FileTypes.Acquisition, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_Success_Inserts_StatusChanged_Note()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compensationRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
                new PimsAcquisitionFile()
                {
                    TotalAllowableCompensation = 100,
                    AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
                });

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
                FinalizedDate = DateOnly.FromDateTime(DateTime.UtcNow),
            });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            var result = service.Update(
                FileTypes.Acquisition,
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
            noteRepository.Verify(x => x.AddNoteRelationship(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 7
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Draft' to 'Final'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_Skips_StatusChanged_Note()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 100,
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true });
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);


            // Act
            var result = service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
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
            noteRepository.Verify(x => x.AddNoteRelationship(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Draft' to 'Final'"))), Times.Never);
        }

        [Fact]
        public void Update_Status_BackToDraft_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();

            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
              new PimsAcquisitionFile()
              {
                  AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
              });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(false);

            // Act
            Action act = () => service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
            });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void Update_Status_BackToNull_NoPermission()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
              new PimsAcquisitionFile()
              {
                  AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
              });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(false);

            // Act
            Action act = () => service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = null,
            });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Update_Status_BackToDraft_AuthorizedAdmin()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit, Permissions.SystemAdmin);
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 100,
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.Is<bool>(b => b == true))).Returns(true);

            // Act
            var result = service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
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
            noteRepository.Verify(x => x.AddNoteRelationship(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Final' to 'Draft'"))), Times.Once);
        }

        [Fact]
        public void Update_Status_BackToNull_AuthorizedAdmin()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit, Permissions.SystemAdmin);
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 100,
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = false });

            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.Is<bool>(b => b == true))).Returns(true);

            // Act
            var result = service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
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
            noteRepository.Verify(x => x.AddNoteRelationship(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'Final' to 'No Status'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_Skips_StatusChanged_Note_FromNoStatus()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 100,
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            var result = service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
            });

            // Assert
            result.Should().NotBeNull();
            result.FinalizedDate.Should().BeNull();
            compRepository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
            noteRepository.Verify(x => x.AddNoteRelationship(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                && x.Note.NoteTxt.Equals("Compensation Requisition with # 1, changed status from 'No Status' to 'Draft'"))), Times.Once);
        }

        [Fact]
        public void Update_Success_ValidTotalAllowableCompensation()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 100,
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            var result = service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 1000 } },
            });

            // Assert
            result.Should().NotBeNull();
            compRepository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }

        [Fact]
        public void Update_Success_ValidMultipleTotalAllowableCompensation()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { CompensationRequisitionId = 1, TotalAmt = 1000 }, new PimsCompReqFinancial() { CompensationRequisitionId = 2, TotalAmt = 100 } });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 300,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { Internal_Id = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            var result = service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = false,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 200 } },
            });

            // Assert
            result.Should().NotBeNull();
            compRepository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }

        [Fact]
        public void Update_Success_TotalAllowableExceededDraft()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 100,
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            var result = service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                AcquisitionFileId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = true,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 1000 } },
            });

            // Assert
            result.Should().NotBeNull();
            compRepository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }

        [Fact]
        public void Update_Fail_TotalAllowableExceeded()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqH120Service = this._helper.GetService<Mock<ICompReqFinancialService>>();
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(new List<PimsCompReqFinancial>() { });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 99,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { Internal_Id = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);


            // Act
            // Assert
            Action act = () => service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
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
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsAcquisitionFileNote>>>();
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1, IsDraft = null });

            compReqH120Service.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { CompensationRequisitionId = 1, TotalAmt = 1000 },
                new PimsCompReqFinancial() { CompensationRequisitionId = 2, TotalAmt = 100 }, });

            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                TotalAllowableCompensation = 299,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { Internal_Id = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
                AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
            });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            // Assert
            Action act = () => service.Update(FileTypes.Acquisition, new PimsCompensationRequisition()
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
        public void Update_Lease_Fail_ValidMultipleTotalAllowableCompensation()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqFinancialsService = this._helper.GetService<Mock<ICompReqFinancialService>>();

            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, LeaseId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, LeaseId = 1, IsDraft = null });

            compReqFinancialsService.Setup(x => x.GetAllByLeaseFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { CompensationRequisitionId = 1, TotalAmt = 1000 },
                new PimsCompReqFinancial() { CompensationRequisitionId = 2, TotalAmt = 100 }, });

            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(new PimsLease()
            {
                TotalAllowableCompensation = 299,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { CompensationRequisitionId = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
                LeaseStatusTypeCode = LeaseStatusTypes.ACTIVE.ToString()
            });

            // Act
            Action act = () => service.Update(FileTypes.Lease, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                LeaseId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = false,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 200 } },
            });

            //Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Update_Lease_Fail_TotalAllowableExceeded()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var compReqFinancialsService = this._helper.GetService<Mock<ICompReqFinancialService>>();

            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            compRepository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, LeaseId = 1, IsDraft = true }); ;
            compRepository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(new PimsCompensationRequisition { Internal_Id = 1, LeaseId = 1, IsDraft = null });

            compReqFinancialsService.Setup(x => x.GetAllByLeaseFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { CompensationRequisitionId = 1, TotalAmt = 1000 },
                new PimsCompReqFinancial() { CompensationRequisitionId = 2, TotalAmt = 100 }, });

            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(new PimsLease()
            {
                TotalAllowableCompensation = 99,
                PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { CompensationRequisitionId = 1,
                    PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } } }, },
                LeaseStatusTypeCode = LeaseStatusTypes.ACTIVE.ToString()
            });

            // Act
            Action act = () => service.Update(FileTypes.Lease, new PimsCompensationRequisition()
            {
                Internal_Id = 1,
                LeaseId = 1,
                ConcurrencyControlNumber = 2,
                IsDraft = false,
                PimsCompReqFinancials = new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 200 } },
            });

            //Assert
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
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compRepository.Setup(x => x.TryDelete(It.IsAny<long>()));
            compRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsCompensationRequisition { Internal_Id = 1 });

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
              new PimsAcquisitionFile()
              {
                  AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
              });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            var result = service.DeleteCompensation(1);

            // Assert
            compRepository.Verify(x => x.TryDelete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Delete_Acquisition_FinalFile()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionDelete);
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compRepository.Setup(x => x.TryDelete(It.IsAny<long>()));
            compRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1 });

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
              new PimsAcquisitionFile()
              {
                  AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
              });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(false);

            // Act
            Action act = () => service.DeleteCompensation(1);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void Delete_Lease_FinalFile()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionDelete);
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compRepository.Setup(x => x.TryDelete(It.IsAny<long>()));
            compRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsCompensationRequisition { Internal_Id = 1, LeaseId = 1 });

            var leaseFileRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseFileRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(
              new PimsLease()
              {
                  LeaseStatusTypeCode = LeaseStatusTypes.ACTIVE.ToString()
              });

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<LeaseStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(false);

            // Act
            Action act = () => service.DeleteCompensation(1);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void Delete_InvalidStatus_AuthorizedAdmin()
        {
            // Arrange
            var service = this.CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionDelete, Permissions.SystemAdmin);
            var compRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compRepository.Setup(x => x.TryDelete(It.IsAny<long>()));
            compRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsCompensationRequisition { Internal_Id = 1, AcquisitionFileId = 1 });

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(
              new PimsAcquisitionFile()
              {
                  AcquisitionFileId = 1,
                  AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString()
              });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditOrDeleteCompensation(It.IsAny<AcquisitionStatusTypes?>(), It.IsAny<bool?>(), It.IsAny<bool?>())).Returns(true);

            // Act
            service.DeleteCompensation(1);

            // Assert
            compRepository.Verify(x => x.TryDelete(It.IsAny<long>()), Times.Once);
        }


        private CompensationRequisitionService CreateCompRequisitionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<CompensationRequisitionService>(user);
        }
    }
}
