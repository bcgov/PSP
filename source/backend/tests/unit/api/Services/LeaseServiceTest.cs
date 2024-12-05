using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServiceTest
    {
        private TestHelper _helper;

        public LeaseServiceTest()
        {
            this._helper = new TestHelper();
        }

        private LeaseService CreateLeaseService(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<LeaseService>();
        }

        #region Tests

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = EntityHelper.CreateUser("Test");
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = lease.RegionCode.Value });

            var service = this.CreateLeaseService(Permissions.LeaseAdd);

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.Add(It.IsAny<PimsLease>())).Returns(lease);
            leaseRepository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsLeaseChklstItemType>() {
                new PimsLeaseChklstItemType()
                {
                    LeaseChklstItemTypeCode = "VALID-ITEM",
                    LeaseChklstSectionTypeCode = LeaseChecklistItemSectionTypes.FILEINIT.ToString(),
                    Description = "MY VALID CHECKLIST ITEM",
                    IsRequired = false,
                    EffectiveDate = DateOnly.MinValue,
                    ExpiryDate = null,
                    IsDisabled = false,
                    DisplayOrder = 1,
                    ConcurrencyControlNumber = 1,
                },
            });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyLease>())).Returns<PimsPropertyLease>(x => x);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            // Act
            var result = service.Add(lease, new List<UserOverrideCode>());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsLease>();
            result.LeaseId.Should().Be(1);
            result.PimsLeaseChecklistItems.Should().HaveCount(1);

            leaseRepository.Verify(x => x.Add(It.IsAny<PimsLease>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyLease>()), Times.Once);
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;

            var service = this.CreateLeaseService();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.Add(It.IsAny<PimsLease>())).Returns(lease);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            Action act = () => service.Add(lease, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            leaseRepository.Verify(x => x.Add(It.IsAny<PimsLease>()), Times.Never);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyLease>()), Times.Never);
        }

        [Fact]
        public void Add_InvalidAccessToLeaseFile()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = EntityHelper.CreateUser("Test");
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = 2 });

            var service = this.CreateLeaseService(Permissions.LeaseAdd);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            leaseRepository.Setup(x => x.Add(It.IsAny<PimsLease>())).Returns(lease);
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            Action act = () => service.Add(lease, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            leaseRepository.Verify(x => x.Add(It.IsAny<PimsLease>()), Times.Never);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyLease>()), Times.Never);
        }

        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = EntityHelper.CreateUser("Test");
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = lease.RegionCode.Value });

            PimsProperty retiredProperty = new PimsProperty()
            {
                PropertyId = 100,
                Pid = 1000,
                IsRetired = true,
            };

            lease.PimsPropertyLeases.Add(new PimsPropertyLease()
            {
                Property = retiredProperty,
            });

            var service = this.CreateLeaseService(Permissions.LeaseAdd);

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.Add(It.IsAny<PimsLease>())).Returns(lease);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(retiredProperty);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            Action act = () => service.Add(lease, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");

            leaseRepository.Verify(x => x.Add(It.IsAny<PimsLease>()), Times.Never);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyLease>()), Times.Never);
        }

        #endregion

        #region Properties
        [Fact]
        public void GetProperties_ByFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateLeaseService();

            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = EntityHelper.CreateUser("Test");
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = lease.RegionCode.Value });

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            // Act
            Action act = () => service.GetPropertiesByLeaseId(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetProperties_ByFileId_Success()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseView, Permissions.PropertyView);

            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = EntityHelper.CreateUser("Test");
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = lease.RegionCode.Value });

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            // Act
            var properties = service.GetPropertiesByLeaseId(1);

            // Assert
            propertyLeaseRepository.Verify(x => x.GetAllByLeaseId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetProperties_ByFileId_Success_Reproject()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseView, Permissions.PropertyView);

            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = EntityHelper.CreateUser("Test");
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = lease.RegionCode.Value });

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>()))
                .Returns(new List<PimsPropertyLease>() { new() { Property = new() { Location = new Point(1, 1) } } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyLease>>()))
                .Returns<List<PimsPropertyLease>>(x => x);

            // Act
            var properties = service.GetPropertiesByLeaseId(1);

            // Assert
            propertyLeaseRepository.Verify(x => x.GetAllByLeaseId(It.IsAny<long>()), Times.Once);
            propertyService.Verify(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyLease>>()), Times.Once);
            properties.First().Property.Location.Coordinates.Should().BeEquivalentTo(new Coordinate[] { new Coordinate(1, 1) });
        }

        #endregion

        #region Update
        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(lease, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            leaseRepository.Verify(x => x.Update(It.IsAny<PimsLease>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Update_NewTotalAllowableCompensation_Success()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseEdit);

            var currentLeaseEntity = EntityHelper.CreateLease(1, addProperty: false);

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var compReqFinancialRepository = this._helper.GetService<Mock<ICompReqFinancialService>>();

            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(currentLeaseEntity);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(currentLeaseEntity);

            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));
            compReqFinancialRepository.Setup(c => c.GetAllByLeaseFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 50 } });

            // Act
            currentLeaseEntity.TotalAllowableCompensation = 100;
            var result = service.Update(currentLeaseEntity, new List<UserOverrideCode>());

            // Assert
            result.Should().NotBeNull();
            result.TotalAllowableCompensation.Equals(100);
        }

        [Fact]
        public void Update_NewTotalAllowableCompensation_Failure_LessThenCurrentFinancials()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseEdit);

            var currentLeaseEntity = EntityHelper.CreateLease(1, addProperty: false);

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var compReqFinancialRepository = this._helper.GetService<Mock<ICompReqFinancialService>>();

            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(currentLeaseEntity);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(currentLeaseEntity);

            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));
            compReqFinancialRepository.Setup(c => c.GetAllByLeaseFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 100 } });

            // Act
            currentLeaseEntity.TotalAllowableCompensation = 99;
            Action act = () => service.Update(currentLeaseEntity, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Update_Without_StatusNote()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseEdit);

            var currentLeaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_A",
            };

            var leaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_A",
            };

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(currentLeaseEntity);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var result = service.Update(leaseEntity, new List<UserOverrideCode>());

            // Assert
            noteRepository.Verify(x => x.Add(It.IsAny<PimsLeaseNote>()), Times.Never);
        }

        [Fact]
        public void Update_With_StatusNote()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseEdit);

            var currentLeaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_A",
            };

            var leaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_B",
            };

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();

            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(currentLeaseEntity);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));
            lookupRepository.Setup(x => x.GetAllLeaseStatusTypes()).Returns(new List<PimsLeaseStatusType>() {
                new PimsLeaseStatusType()
                {
                   LeaseStatusTypeCode= "STATUS_A",
                   Description = "STATUS_A",
                },
                new PimsLeaseStatusType()
                {
                   LeaseStatusTypeCode= "STATUS_B",
                   Description = "STATUS_B",
                },
            });

            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var result = service.Update(leaseEntity, new List<UserOverrideCode>());

            // Assert
            noteRepository.Verify(x => x.Add(It.IsAny<PimsLeaseNote>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()));

            // Act
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
            propertyService.Verify(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            PimsProperty property = new PimsProperty()
            {
                PropertyId = 100,
                Pid = 1,
                IsRetired = true,
            };

            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }

        [Fact]
        public void UpdateProperties_MatchProperties_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()));

            // Act
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
            propertyService.Verify(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_NewProperty_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();
            leaseRepository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            PimsProperty newProperty = null;
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()))
                .Returns(newProperty)
                .Callback<PimsProperty, Boolean, Boolean>((x, _, _) =>
                {
                    newProperty = x;
                    newProperty.Internal_Id = 0;
                    newProperty.PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now);
                    newProperty.PropertyDataSourceTypeCode = "PMBC";
                    newProperty.PropertyTypeCode = "UNKNOWN";
                    newProperty.PropertyStatusTypeCode = "UNKNOWN";
                    newProperty.SurplusDeclarationTypeCode = "UNKNOWN";
                    newProperty.RegionCode = 1;
                });

            propertyService.Setup(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyLease>())).Returns<PimsPropertyLease>(x => x);

            // Act
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });
            PimsPropertyLease updatedLeaseProperty = updatedLease.PimsPropertyLeases.First();

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedLeaseProperty.Property;
            newProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            newProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            newProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            newProperty.PropertyDataSourceEffectiveDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            newProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            newProperty.IsOwned.Should().Be(false);

            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemovePropertyFile_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var updatedLease = EntityHelper.CreateLease(2, addProperty: false);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(3);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            updatedLease = service.Update(updatedLease, new List<UserOverrideCode>());

            // Assert
            propertyLeaseRepository.Verify(x => x.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<ICollection<PimsPropertyLease>>()));
            propertyRepository.Verify(x => x.Delete(It.IsAny<PimsProperty>()), Times.Never());
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Fails_PropertyAssignedToCompReq()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var deletedProperty = lease.PimsPropertyLeases.FirstOrDefault().Property;
            var updatedLease = EntityHelper.CreateLease(2, addProperty: false);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyLeaseRepository.Setup(x => x.LeaseFilePropertyInCompensationReq(It.IsAny<long>())).Returns(true);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(1);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(updatedLease, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>().WithMessage("Lease File property can not be removed since it's assigned as a property for a compensation requisition");
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var deletedProperty = lease.PimsPropertyLeases.FirstOrDefault().Property;
            var updatedLease = EntityHelper.CreateLease(2, addProperty: false);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.PropertyAdd, Permissions.PropertyView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyLeaseRepository.Setup(x => x.LeaseFilePropertyInCompensationReq(It.IsAny<long>())).Returns(false);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(1);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            updatedLease = service.Update(updatedLease, new List<UserOverrideCode>());

            // Assert
            propertyLeaseRepository.Verify(x => x.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<ICollection<PimsPropertyLease>>()));
            propertyRepository.Verify(x => x.Delete(deletedProperty), Times.Once);
        }

        #endregion

        #region Consultations
        [Fact]
        public void GetConsultations_NoPermission()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService();
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.GetConsultationsByLease(It.IsAny<long>())).Returns(new List<PimsLeaseConsultation>());

            // Act
            Action act = () => service.GetConsultations(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            consultationRepository.Verify(x => x.GetConsultationsByLease(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void GetConsultations_Success()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseView);
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.GetConsultationsByLease(It.IsAny<long>())).Returns(new List<PimsLeaseConsultation>());

            // Act
            var result = service.GetConsultations(1);

            // Assert
            consultationRepository.Verify(x => x.GetConsultationsByLease(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetConsultationById_NoPermission()
        {
            // Arrange
            var service = this.CreateLeaseService();
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.GetConsultationById(It.IsAny<long>())).Returns(new PimsLeaseConsultation());

            // Act
            Action act = () => service.GetConsultationById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            consultationRepository.Verify(x => x.GetConsultationById(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void GetConsultationById_Success()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseView);
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.GetConsultationById(It.IsAny<long>())).Returns(new PimsLeaseConsultation());

            // Act
            var result = service.GetConsultationById(1);

            // Assert
            consultationRepository.Verify(x => x.GetConsultationById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void AddConsultation_NoPermission()
        {
            // Arrange
            var service = this.CreateLeaseService();
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.AddConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(new PimsLeaseConsultation());

            // Act
            Action act = () => service.AddConsultation(new PimsLeaseConsultation());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            consultationRepository.Verify(x => x.AddConsultation(It.IsAny<PimsLeaseConsultation>()), Times.Never);
        }

        [Fact]
        public void AddConsultation_Success()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseEdit);
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.AddConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(new PimsLeaseConsultation());

            // Act
            var result = service.AddConsultation(new PimsLeaseConsultation());

            // Assert
            consultationRepository.Verify(x => x.AddConsultation(It.IsAny<PimsLeaseConsultation>()), Times.Once);
        }

        [Fact]
        public void Update_Consultation_NoPermission()
        {
            // Arrange
            var service = this.CreateLeaseService();
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.UpdateConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(new PimsLeaseConsultation());

            // Act
            Action act = () => service.UpdateConsultation(new PimsLeaseConsultation());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            consultationRepository.Verify(x => x.UpdateConsultation(It.IsAny<PimsLeaseConsultation>()), Times.Never);
        }

        [Fact]
        public void Update_Consultation_Success()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseEdit);
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.UpdateConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(new PimsLeaseConsultation());

            // Act
            var result = service.UpdateConsultation(new PimsLeaseConsultation());

            // Assert
            consultationRepository.Verify(x => x.UpdateConsultation(It.IsAny<PimsLeaseConsultation>()), Times.Once);
        }

        [Fact]
        public void Delete_Consultation_NoPermission()
        {
            // Arrange
            var service = this.CreateLeaseService();
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.TryDeleteConsultation(It.IsAny<long>())).Returns(true);

            // Act
            Action act = () => service.DeleteConsultation(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            consultationRepository.Verify(x => x.TryDeleteConsultation(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Delete_Consultation_Success()
        {
            // Arrange
            var service = this.CreateLeaseService(Permissions.LeaseEdit);
            var consultationRepository = this._helper.GetService<Mock<IConsultationRepository>>();

            consultationRepository.Setup(x => x.TryDeleteConsultation(It.IsAny<long>())).Returns(true);

            // Act
            var result = service.DeleteConsultation(1);

            // Assert
            consultationRepository.Verify(x => x.TryDeleteConsultation(It.IsAny<long>()), Times.Once);
            result.Should().BeTrue();
        }
        #endregion

        #endregion
    }
}
