using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> LeaseFilterData =>
            new List<object[]>
            {
                new object[] { new LeaseFilter() { TenantName = "tenant" }, 1 },
                new object[] { new LeaseFilter() { TenantName = "fake" }, 0 },
                new object[] { new LeaseFilter() { LFileNo = "123" }, 1 },
                new object[] { new LeaseFilter() { LFileNo = "fake" }, 0 },
                new object[] { new LeaseFilter() { PinOrPid = "456" }, 1 },
                new object[] { new LeaseFilter() { PinOrPid = "789" }, 0 },
                new object[] { new LeaseFilter() { Historical = "111" }, 1 },
                new object[] { new LeaseFilter() { Historical = "222" }, 0 },
                new object[] { new LeaseFilter() { Address = "1234 St" }, 1 },
                new object[] { new LeaseFilter() { Historical = "fake address" }, 0 },
                new object[] { new LeaseFilter() { Programs = new List<string>() { "testProgramType" } }, 1 },
                new object[] { new LeaseFilter() { Programs = new List<string>() { "fake" } }, 0 },
                new object[] { new LeaseFilter() { LeaseStatusTypes = new List<string>() { "testStatusType" } }, 1 },
                new object[] { new LeaseFilter() { LeaseStatusTypes = new List<string>() { "fake" } }, 0 },
                new object[] { new LeaseFilter() { Details = "details" }, 1 },
                new object[] { new LeaseFilter() { Details = "test" }, 0 },
                new object[] { new LeaseFilter() { RegionType = 2 }, 0 },
                new object[] { new LeaseFilter() { ExpiryStartDate = new DateOnly(1999, 1,1) }, 1 },
                new object[] { new LeaseFilter() { ExpiryStartDate = new DateOnly(2001,1,1) }, 0 },
                new object[] { new LeaseFilter() { ExpiryEndDate = new DateOnly(1999, 1,1) }, 0 },
                new object[] { new LeaseFilter() { ExpiryEndDate = new DateOnly(2001,1,1) }, 1 },
                new object[] { new LeaseFilter() { ExpiryEndDate = new DateOnly(1999, 1,1), ExpiryStartDate = new DateOnly(1999, 1, 1) }, 0 },
                new object[] { new LeaseFilter() { ExpiryEndDate = new DateOnly(2001, 1, 1), ExpiryStartDate = new DateOnly(1999, 1, 1) }, 1 },
                new object[] { new LeaseFilter() { ExpiryAfterDate = new DateTime(2001,1,1) }, 0 },
                new object[] { new LeaseFilter() { ExpiryAfterDate = new DateTime(2000,1,1) }, 1 },
                new object[] { new LeaseFilter() { StartBeforeDate = new DateTime(1999, 1,1) }, 0 },
                new object[] { new LeaseFilter() { StartBeforeDate = new DateTime(2000,1,1) }, 1 },
                new object[] { new LeaseFilter() { NotInStatus = new List<string>() { "testStatusType" } }, 0 },
                new object[] { new LeaseFilter() { NotInStatus = new List<string>() { "someOtherValue" } }, 1 },
                new object[] { new LeaseFilter() { IsReceivable = true }, 0 },
                    new object[] { new LeaseFilter() { IsReceivable = false }, 1 },
                new object[] { new LeaseFilter() { PinOrPid = "789" }, 0 },
                new object[] { new LeaseFilter(), 1 },
                new object[] { new LeaseFilter() { Sort = new string[] {"ExpiryDate"} }, 1 },
            };
        #endregion

        #region Tests
        [Fact]
        public void Lease_Count()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(elease);

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.Count();

            // Assert
            Assert.Equal(1, result);
        }

        #region Get
        [Theory]
        [MemberData(nameof(LeaseFilterData))]
        public void Get_Leases_Paged(LeaseFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", stakeholderLastName: "tenant", addStakeholder: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);
            elease.PsFileNo = "111";
            elease.LeaseProgramTypeCode = "testProgramType";
            elease.LeaseStatusTypeCode = "testStatusType";
            elease.LeaseDescription = "details";

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetAllByFilter(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsLease[]>(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void Get_Leases_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetAllByFilter(null, new HashSet<short>()));
        }

        [Fact]
        public void Get_Leases_InvalidFilter()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() =>
                service.GetAllByFilter(new LeaseFilter() { ExpiryStartDate = DateOnly.MaxValue, ExpiryEndDate = DateOnly.MinValue }, new HashSet<short>()));
        }

        [Theory]
        [MemberData(nameof(LeaseFilterData))]
        public void Get_Leases_Filter(LeaseFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", stakeholderLastName: "tenant", addStakeholder: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);
            elease.PsFileNo = "111";
            elease.LeaseProgramTypeCode = "testProgramType";
            elease.LeaseStatusTypeCode = "testStatusType";
            elease.LeaseDescription = "details";

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetPage(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsLease>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
        }

        [Fact]
        public void Get_Leases_Filter_Historical_LISNO()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", stakeholderLastName: "tenant", addStakeholder: true, addProperty: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);
            elease.PsFileNo = "111";
            elease.LeaseProgramTypeCode = "testProgramType";
            elease.LeaseStatusTypeCode = "testStatusType";
            elease.LeaseDescription = "details";

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var fileNumber = new PimsHistoricalFileNumber();
            fileNumber.PropertyId = elease.PimsPropertyLeases.FirstOrDefault().PropertyId;
            fileNumber.HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.LISNO.ToString();
            fileNumber.HistoricalFileNumber = "99999";
            fileNumber.AppCreateUserid = "tester";
            fileNumber.AppCreateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserid = "tester";
            fileNumber.DbCreateUserid = "tester";
            fileNumber.DbLastUpdateUserid = "tester";

            context.AddAndSaveChanges(fileNumber);

            LeaseFilter filter = new LeaseFilter() { Historical = "99999" };
            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetPage(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsLease>>(result);
            Assert.Equal(1, result.Items.Count);
        }

        [Fact]
        public void Get_Leases_Filter_Historical_PSNO()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", stakeholderLastName: "tenant", addStakeholder: true, addProperty: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);
            elease.PsFileNo = "111";
            elease.LeaseProgramTypeCode = "testProgramType";
            elease.LeaseStatusTypeCode = "testStatusType";
            elease.LeaseDescription = "details";

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var fileNumber = new PimsHistoricalFileNumber();
            fileNumber.PropertyId = elease.PimsPropertyLeases.FirstOrDefault().PropertyId;
            fileNumber.HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.PSNO.ToString();
            fileNumber.HistoricalFileNumber = "88888";
            fileNumber.AppCreateUserid = "tester";
            fileNumber.AppCreateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserid = "tester";
            fileNumber.DbCreateUserid = "tester";
            fileNumber.DbLastUpdateUserid = "tester";

            context.AddAndSaveChanges(fileNumber);

            LeaseFilter filter = new LeaseFilter() { Historical = "88888" };
            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetPage(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsLease>>(result);
            Assert.Equal(1, result.Items.Count);
        }

        [Fact]
        public void Get_Leases_Filter_Historical_OTHERNO()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", stakeholderLastName: "tenant", addStakeholder: true, addProperty: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);
            elease.PsFileNo = "111";
            elease.LeaseProgramTypeCode = "testProgramType";
            elease.LeaseStatusTypeCode = "testStatusType";
            elease.LeaseDescription = "details";

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var fileNumber = new PimsHistoricalFileNumber();
            fileNumber.PropertyId = elease.PimsPropertyLeases.FirstOrDefault().PropertyId;
            fileNumber.HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.OTHER.ToString();
            fileNumber.OtherHistFileNumberTypeCode = HistoricalFileNumberTypes.OTHER.ToString();
            fileNumber.HistoricalFileNumber = "77777";
            fileNumber.AppCreateUserid = "tester";
            fileNumber.AppCreateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserid = "tester";
            fileNumber.DbCreateUserid = "tester";
            fileNumber.DbLastUpdateUserid = "tester";

            context.AddAndSaveChanges(fileNumber);

            LeaseFilter filter = new LeaseFilter() { Historical = "77777" };
            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetPage(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsLease>>(result);
            Assert.Equal(1, result.Items.Count);
        }

        [Fact]
        public void Get_Leases_Filter_Historical_File_Numbers()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", stakeholderLastName: "tenant", addStakeholder: true, addProperty: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);
            elease.PsFileNo = "111";
            elease.LeaseProgramTypeCode = "testProgramType";
            elease.LeaseStatusTypeCode = "testStatusType";
            elease.LeaseDescription = "details";

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var fileNumber = new PimsHistoricalFileNumber();
            fileNumber.PropertyId = elease.PimsPropertyLeases.FirstOrDefault().PropertyId;
            fileNumber.HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.PROPNEG.ToString();
            fileNumber.HistoricalFileNumber = "66666";
            fileNumber.AppCreateUserid = "tester";
            fileNumber.AppCreateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserDirectory = "PIMS";
            fileNumber.AppLastUpdateUserid = "tester";
            fileNumber.DbCreateUserid = "tester";
            fileNumber.DbLastUpdateUserid = "tester";

            context.AddAndSaveChanges(fileNumber);

            LeaseFilter filter = new LeaseFilter() { Historical = "66666" };
            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetPage(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsLease>>(result);
            Assert.Equal(1, result.Items.Count);
        }

        #endregion

        #region Add Lease
        [Fact]
        public void Add_Lease_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);
            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(50);
            helper.AddSingleton(mockSequenceRepo.Object);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = repository.Add(lease);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsLease>();
            result.LeaseId.Should().Be(50);
            result.LFileNo.Should().Be("L-000-050");
        }

        [Fact]
        public void Add_Lease_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            // Act
            Action act = () => repository.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_Lease_WithRetiredProperty_Should_Fail()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyLeases = new List<PimsPropertyLease>() {
                new PimsPropertyLease()
                {
                    PropertyId = 100,
                    Property = new PimsProperty()
                    {
                        Pid = 1,
                        IsRetired = true,
                    }
                }
            };

            // Act
            Action act = () => repository.Add(lease);

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }

        #endregion

        #region GetLastUpdateBy
        [Fact]
        public void GetLastUpdateBy_Details_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);
            helper.CreatePimsContext(user, true);

            var lease = EntityHelper.CreateLease(1);
            lease.AppLastUpdateUserid = "test";
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var lastUpdateModel = repository.GetLastUpdateBy(lease.Internal_Id);

            // Assert
            lastUpdateModel.AppLastUpdateTimestamp.Should().BeWithin(TimeSpan.FromMilliseconds(100));
            lastUpdateModel.AppLastUpdateUserid.Should().Be("service");
        }
        #endregion

        #region GetNoTracking
        [Fact]
        public void GetNoTracking_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var response = repository.GetNoTracking(lease.Internal_Id);

            // Assert
            response.Internal_Id.Should().Be(1);
        }
        #endregion

        #region GetAllLeaseDocuments
        [Fact]
        public void GetAllLeaseDocuments_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsLeaseDocuments = new List<PimsLeaseDocument>() { new PimsLeaseDocument() { Document = EntityHelper.CreateDocument("test", 1) }, new PimsLeaseDocument() { Document = EntityHelper.CreateDocument("doc", 2) } };
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var response = repository.GetAllLeaseDocuments(lease.Internal_Id);

            // Assert
            response.Count.Should().Be(2);
        }
        #endregion

        #region AddLeaseDocument
        [Fact]
        public void AddLeaseDocument_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var response = repository.AddLeaseDocument(new PimsLeaseDocument() { Document = EntityHelper.CreateDocument("test", 1) });
            context.CommitTransaction();

            // Assert
            context.PimsLeaseDocuments.Should().HaveCount(1);
            context.PimsLeaseDocuments.FirstOrDefault().Document.FileName.Should().Be("test");
        }
        #endregion

        #region DeleteLeaseDocument
        [Fact]
        public void DeleteLeaseDocument_Success()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsLeaseDocuments = new List<PimsLeaseDocument>() { new PimsLeaseDocument() { LeaseDocumentId = 1, Document = EntityHelper.CreateDocument("test", 1) } };
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseRepository>(user);

            // Act
            repository.DeleteLeaseDocument(1);
            context.CommitTransaction();

            // Assert
            context.PimsLeaseDocuments.Should().HaveCount(0);
        }
        #endregion

        #region Update Lease
        [Fact]
        public void Update_Lease_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddRange(lease);
            var repository = helper.CreateRepository<LeaseRepository>(user);
            helper.SaveChanges();

            // Act
            lease.LeaseDescription = "updated";
            var updated = repository.Update(lease);

            // Assert
            updated.LeaseDescription.Should().Be("updated");
        }

        [Fact]
        public void Update_Lease_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddRange(lease);
            var repository = helper.CreateRepository<LeaseRepository>(user);
            helper.SaveChanges();

            // Act
            lease.LeaseDescription = "updated";
            Action act = () => repository.UpdateChecklistItem(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_Lease_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddRange(lease);
            var repository = helper.CreateRepository<LeaseRepository>(user);
            helper.SaveChanges();

            // Act
            lease.LeaseDescription = "updated";
            Action act = () => repository.Update(lease);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Lease_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddRange(lease);
            var repository = helper.CreateRepository<LeaseRepository>(user);
            helper.SaveChanges();

            // Act
            lease.LeaseDescription = "updated";
            lease.LeaseId = 2;
            Action act = () => repository.Update(lease);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Update Stakeholder
        [Fact]
        public void Update_Lease_Stakeholders_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseStakeholderRepository>(user);

            // Act
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            var addStakeholderPerson = new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, LeaseStakeholderTypeCodeNavigation = new PimsLeaseStakeholderType() { Id = "TEN1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } };
            var addStakeholderOrganization = new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER2", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, LeaseStakeholderTypeCodeNavigation = new PimsLeaseStakeholderType() { Id = "TEN2", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } };
            lease.PimsLeaseStakeholders.Add(addStakeholderPerson);
            lease.PimsLeaseStakeholders.Add(addStakeholderOrganization);
            repository.Update(1, lease.PimsLeaseStakeholders);
            repository.SaveChanges();
            var updatedStakeholders = repository.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedStakeholders.Should().HaveCount(2);
            updatedStakeholders.FirstOrDefault().Internal_Id.Should().Be(addStakeholderOrganization.Internal_Id);
        }

        [Fact]
        public void Update_Lease_Stakeholder_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var updatePerson = EntityHelper.CreatePerson(2, "tester", "two");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            lease.PimsLeaseStakeholders.Add(new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, LeaseStakeholderTypeCodeNavigation = new PimsLeaseStakeholderType() { Id = "TEN1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } });
            lease.PimsLeaseStakeholders.Add(new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER2", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, LeaseStakeholderTypeCodeNavigation = new PimsLeaseStakeholderType() { Id = "TEN2", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseStakeholderRepository>(user);

            // Act
            var updateStakeholder = lease.PimsLeaseStakeholders.FirstOrDefault();
            updateStakeholder.PersonId = updatePerson.PersonId;
            repository.Update(1, lease.PimsLeaseStakeholders);
            repository.SaveChanges();
            var updatedStakeholder = repository.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedStakeholder.Should().HaveCount(2);
            updatedStakeholder.FirstOrDefault().Internal_Id.Should().Be(updateStakeholder.Internal_Id);
        }
        [Fact]
        public void Update_Lease_Stakeholder_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            lease.PimsLeaseStakeholders.Add(new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, LeaseStakeholderTypeCodeNavigation = new PimsLeaseStakeholderType() { Id = "TEN1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseStakeholderRepository>(user);

            // Act
            var deleteStakeholder = lease.PimsLeaseStakeholders.FirstOrDefault();
            lease.PimsLeaseStakeholders.Remove(deleteStakeholder);
            context.ChangeTracker.Clear();
            repository.Update(1, lease.PimsLeaseStakeholders);
            repository.SaveChanges();
            var updatedLeaseStakeholders = repository.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedLeaseStakeholders.Should().BeEmpty();
        }
        [Fact]
        public void Update_Lease_Stakeholder_AddRemove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var addPerson = EntityHelper.CreatePerson(2, "tester", "two");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            lease.PimsLeaseStakeholders.Add(new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCode = "PER1", LeaseStakeholderTypeCode = "TEN1", LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, LeaseStakeholderTypeCodeNavigation = new PimsLeaseStakeholderType() { Id = "TEN1", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } });
            lease.PimsLeaseStakeholders.Add(new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId, LessorTypeCode = "PER1", LeaseStakeholderTypeCode = "TEN1", LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER2", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, LeaseStakeholderTypeCodeNavigation = new PimsLeaseStakeholderType() { Id = "TEN2", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseStakeholderRepository>(user);

            // Act
            var deleteStakeholder = lease.PimsLeaseStakeholders.FirstOrDefault();
            lease.PimsLeaseStakeholders.Remove(deleteStakeholder);
            var addStakeholder = new Dal.Entities.PimsLeaseStakeholder() { LeaseId = lease.LeaseId, PersonId = addPerson.PersonId, LessorTypeCode = "PER1", LeaseStakeholderTypeCode = "TEN1" };
            lease.PimsLeaseStakeholders.Add(addStakeholder);
            context.ChangeTracker.Clear();
            repository.Update(1, lease.PimsLeaseStakeholders);
            repository.SaveChanges();
            var updatedLeaseStakeholders = repository.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedLeaseStakeholders.Should().HaveCount(2);
            updatedLeaseStakeholders.FirstOrDefault().Internal_Id.Should().NotBe(deleteStakeholder.Internal_Id);
        }
        #endregion

        #region Update Lease Properties
        [Fact]
        public void Update_Lease_Properties_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddRange(propertyOne, lease);
            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);
            helper.SaveChanges();

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            repository.UpdatePropertyLeases(1, new List<PimsPropertyLease>() { addProperty });
            repository.CommitTransaction();
            var updatedPropertyLeases = repository.GetAllByLeaseId(lease.LeaseId);

            // Assert
            updatedPropertyLeases.Should().HaveCount(1);
            updatedPropertyLeases.FirstOrDefault().Internal_Id.Should().Be(addProperty.Internal_Id);
        }

        //[Fact]
        public void Update_Lease_Properties_AddPropertyInLease()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(propertyOne, lease);

            var service = helper.Create<LeaseService>(user);
            helper.SaveChanges();

            var leaseTwo = context.CreateLease(2, addProperty: false);
            propertyOne.PimsPropertyLeases = new List<PimsPropertyLease>() { new Dal.Entities.PimsPropertyLease() { LeaseId = leaseTwo.LeaseId, Lease = leaseTwo, PropertyId = propertyOne.PropertyId } };
            helper.SaveChanges();

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(propertyOne);

            var repository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            repository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(propertyOne.PimsPropertyLeases);

            var leaseRepository = helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, Lease = lease, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);

            // Assert
            Assert.Throws<UserOverrideException>(() =>
                service.Update(lease, Array.Empty<UserOverrideCode>()));
        }

        //[Fact]
        public void Update_Lease_Properties_AddPropertyInLeaseOverride()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(propertyOne, lease);
            var service = helper.Create<LeaseService>();
            helper.SaveChanges();
            var leaseTwo = context.CreateLease(2, addProperty: false);
            propertyOne.PimsPropertyLeases = new List<PimsPropertyLease>() { new Dal.Entities.PimsPropertyLease() { LeaseId = leaseTwo.LeaseId, PropertyId = propertyOne.PropertyId, Lease = leaseTwo } };

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(propertyOne);

            var repository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            repository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(propertyOne.PimsPropertyLeases);

            var leaseRepository = helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Update(It.IsAny<PimsLease>(), false));

            var propertyLeaseRepository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeaseRepository.Setup(x => x.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<List<PimsPropertyLease>>()));

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty, UserOverrideCode.AddPropertyToInventory });

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
            propertyLeaseRepository.Verify(x => x.UpdatePropertyLeases(lease.LeaseId, lease.PimsPropertyLeases), Times.Once);
        }

        [Fact]
        public void Update_Lease_Properties_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            lease.PimsPropertyLeases.Add(new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne });
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease, propertyOne);
            var service = helper.Create<LeaseService>(user);
            helper.SaveChanges();

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(propertyOne);

            var repository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            repository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>());

            var leaseRepository = helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Update(It.IsAny<PimsLease>(), false));

            var propertyLeaseRepository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeaseRepository.Setup(x => x.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<List<PimsPropertyLease>>()));

            var userRepository = helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("test"));

            // Act
            var updateProperty = EntityHelper.CreateProperty(context, 2);
            helper.SaveChanges();
            var propertyToUpdate = lease.PimsPropertyLeases.FirstOrDefault();
            propertyToUpdate.PropertyId = updateProperty.PropertyId;
            propertyToUpdate.Property = updateProperty;
            context.ChangeTracker.Clear();
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
            propertyLeaseRepository.Verify(x => x.UpdatePropertyLeases(lease.LeaseId, lease.PimsPropertyLeases), Times.Once);
        }

        [Fact]
        public void Update_Lease_Properties_UpdateNonExistantProperty()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            lease.PimsPropertyLeases.Add(new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne });
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease, propertyOne);
            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);
            helper.SaveChanges();

            var propertyLeaseToUpdate = lease.PimsPropertyLeases.FirstOrDefault();
            propertyLeaseToUpdate.PropertyId = 2;
            propertyLeaseToUpdate.Property = EntityHelper.CreateProperty(2);

            // Act
            var properties = repository.UpdatePropertyLeases(1, lease.PimsPropertyLeases);

            // Assert
            properties.Should().HaveCount(1);
        }

        [Fact]
        public void Update_Lease_Properties_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            lease.PimsPropertyLeases.Add(new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);

            // Act
            var deleteProperty = lease.PimsPropertyLeases.FirstOrDefault();
            lease.PimsPropertyLeases.Remove(deleteProperty);
            context.ChangeTracker.Clear();
            var properties = repository.UpdatePropertyLeases(1, lease.PimsPropertyLeases);

            // Assert
            properties.Should().BeEmpty();
        }
        [Fact]
        public void Update_Lease_Properties_AddRemove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var property = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(property, lease);
            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);
            helper.SaveChanges();

            // Act
            var deleteProperty = lease.PimsPropertyLeases.FirstOrDefault();
            lease.PimsPropertyLeases.Remove(deleteProperty);

            var addProperty = EntityHelper.CreateProperty(context, 2);
            helper.SaveChanges();

            var addPropertyLease = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = addProperty.PropertyId, Property = addProperty };
            lease.PimsPropertyLeases.Add(addPropertyLease);
            repository.UpdatePropertyLeases(1, lease.PimsPropertyLeases);
            repository.CommitTransaction();
            var updatedProperties = repository.GetAllByLeaseId(lease.LeaseId);

            // Assert
            updatedProperties.Should().HaveCount(1);
            updatedProperties.Should().NotContain(deleteProperty);
            updatedProperties.FirstOrDefault().Internal_Id.Should().Be(addPropertyLease.Internal_Id);
        }

        [Fact]
        public void Update_Lease_Properties_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var lease = EntityHelper.CreateLease(1);

            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                repository.UpdatePropertyLeases(1, new List<Pims.Dal.Entities.PimsPropertyLease>()));
        }
        #endregion

        #region Update Lease Property Improvements
        [Fact]
        public void Update_Lease_Improvements_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddRange(lease);
            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);
            helper.SaveChanges();

            // Act
            var addImprovement = new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId };
            lease.PimsPropertyImprovements.Add(addImprovement);
            var improvements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            improvements.Should().HaveCount(1);
            improvements.Should().Contain(addImprovement);
        }

        [Fact]
        public void Update_Lease_Improvements_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements.Add(new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId, PropertyImprovementId = 1, PropertyImprovementTypeCode = "OTHER" });
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease);
            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);
            helper.SaveChanges();

            // Act
            var improvementToUpdate = lease.PimsPropertyImprovements.FirstOrDefault();
            improvementToUpdate.Address = "test update";
            var updatedImprovements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            updatedImprovements.Should().HaveCount(1);
            updatedImprovements.Should().Contain(improvementToUpdate);
        }

        [Fact]
        public void Update_Lease_Improvements_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements.Add(new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId, PropertyImprovementId = 1, PropertyImprovementTypeCode = "OTHER" });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);

            // Act
            var deleteImprovement = lease.PimsPropertyImprovements.FirstOrDefault();
            lease.PimsPropertyImprovements.Remove(deleteImprovement);
            context.ChangeTracker.Clear();
            var updatedImprovements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            updatedImprovements.Should().BeEmpty();
        }
        [Fact]
        public void Update_Lease_Improvements_AddRemove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease);
            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);
            helper.SaveChanges();

            // Act
            var deleteProperty = lease.PimsPropertyImprovements.FirstOrDefault();
            lease.PimsPropertyImprovements.Remove(deleteProperty);

            var addPropertyImprovement = new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId };
            lease.PimsPropertyImprovements.Add(addPropertyImprovement);
            var updatedImprovements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            updatedImprovements.Should().HaveCount(1);
            updatedImprovements.Should().NotContain(deleteProperty);
            updatedImprovements.Should().Contain(addPropertyImprovement);
        }
        #endregion
        #endregion
    }
}
