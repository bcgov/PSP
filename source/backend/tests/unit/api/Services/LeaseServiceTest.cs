using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Humanizer;
using MapsterMapper;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
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
            var user = new PimsUser();
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = lease.RegionCode.Value });

            var service = this.CreateLeaseService(Permissions.LeaseAdd);

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.Add(It.IsAny<PimsLease>())).Returns(lease);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            // Act
            var result = service.Add(lease, new List<UserOverrideCode>());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsLease>();
            result.LeaseId.Should().Be(1);
            leaseRepository.Verify(x => x.Add(It.IsAny<PimsLease>()), Times.Once);
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

            // Act
            Action act = () => service.Add(lease, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            leaseRepository.Verify(x => x.Add(It.IsAny<PimsLease>()), Times.Never);
        }

        [Fact]
        public void Add_InvalidAccessToLeaseFile()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = new PimsUser();
            user.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = 2 });

            var service = this.CreateLeaseService(Permissions.LeaseAdd);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            leaseRepository.Setup(x => x.Add(It.IsAny<PimsLease>())).Returns(lease);
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            // Act
            Action act = () => service.Add(lease, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            leaseRepository.Verify(x => x.Add(It.IsAny<PimsLease>()), Times.Never);
        }

        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var user = new PimsUser();
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

            // Act
            Action act = () => service.Add(lease, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }

        #endregion

        #region Update
        [Fact]
        public void Update_WithoutStatusNote()
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
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var result = service.Update(leaseEntity, new List<UserOverrideCode>());

            // Assert
            noteRepository.Verify(x => x.Add(It.IsAny<PimsLeaseNote>()), Times.Never);
        }

        [Fact]
        public void Update_WithStatusNote()
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
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(currentLeaseEntity);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var result = service.Update(leaseEntity, new List<UserOverrideCode>());

            // Assert
            noteRepository.Verify(x => x.Add(It.IsAny<PimsLeaseNote>()), Times.Once);
        }

        [Fact]
        public void Update_Properties_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            // Act
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
        }

        [Fact]
        public void Update_Properties_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.LeaseView);
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
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            // Act
            Action act = () => service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }

        [Fact]
        public void Update_Properties_Delete_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var updatedLease = EntityHelper.CreateLease(2, addProperty: false);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(3);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            // Act
            updatedLease = service.Update(updatedLease, new List<UserOverrideCode>());

            // Assert
            propertyRepository.Verify(x => x.Delete(It.IsAny<PimsProperty>()), Times.Never());
        }

        [Fact]
        public void Update_Properties_Delete_POI_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var deletedProperty = lease.PimsPropertyLeases.FirstOrDefault().Property;
            var updatedLease = EntityHelper.CreateLease(2, addProperty: false);

            var service = this.CreateLeaseService(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(EntityHelper.CreateLease(1));
            userRepository.Setup(x => x.GetByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            // Act
            updatedLease = service.Update(updatedLease, new List<UserOverrideCode>());

            // Assert
            propertyRepository.Verify(x => x.Delete(deletedProperty), Times.Once);
        }

        #endregion

        #endregion
    }
}
