using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    public class ConsultationRepositoryTest
    {
        private readonly TestHelper _helper;

        public ConsultationRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private ConsultationRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<ConsultationRepository>(user);
        }

        [Fact]
        public void GetConsultationByLease_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            var consultation = EntityHelper.CreateLeaseConsultationItem();
            _helper.AddAndSaveChanges(consultation);

            // Act
            var result = repository.GetConsultationsByLease(consultation.LeaseId);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetConsultationById_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            var consultation = EntityHelper.CreateLeaseConsultationItem();
            _helper.AddAndSaveChanges(consultation);

            // Act
            var result = repository.GetConsultationById(1);

            // Assert
            result.LeaseConsultationId.Should().Be(1);
        }

        [Fact]
        public void GetConsultationById_KeyNotFoundException()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            // Act
            Action act = () => repository.GetConsultationById(1);

            act.Should().Throw<KeyNotFoundException>();

        }

        [Fact]
        public void AddConsultationDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            // Act
            var result = repository.AddConsultation(EntityHelper.CreateLeaseConsultationItem());

            // Assert
            result.LeaseConsultationId.Should().Be(1);
        }

        [Fact]
        public void UpdateConsultation_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);
            var consultation = EntityHelper.CreateLeaseConsultationItem();

            _helper.AddAndSaveChanges(consultation);

            var updatedConsultation = EntityHelper.CreateLeaseConsultationItem();
            updatedConsultation.IsResponseReceived = true;

            // Act
            var result = repository.UpdateConsultation(updatedConsultation);

            // Assert
            result.IsResponseReceived.Should().Be(true);
        }

        [Fact]
        public void UpdateConsultation_RequestedOnNull_DeletesMatchingNotifications()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);
            _helper.CreatePimsContext(user, true);

            var notificationRepository = new Mock<INotificationRepository>();
            _helper.AddSingleton(notificationRepository.Object);

            var repository = _helper.CreateRepository<ConsultationRepository>(user);
            var consultation = EntityHelper.CreateLeaseConsultationItem();
            _helper.AddAndSaveChanges(consultation);

            var matchingNotification = new PimsNotification
            {
                NotificationId = 1,
                NotificationTypeCode = "TEST",
                LeaseId = consultation.LeaseId,
                LeaseConsultationId = consultation.LeaseConsultationId,
            };
            var unrelatedNotification = new PimsNotification
            {
                NotificationId = 2,
                NotificationTypeCode = "TEST",
                LeaseId = consultation.LeaseId + 1,
                LeaseConsultationId = consultation.LeaseConsultationId,
            };
            _helper.AddAndSaveChanges(matchingNotification, unrelatedNotification);

            var updatedConsultation = EntityHelper.CreateLeaseConsultationItem(
                leaseConsultationId: consultation.LeaseConsultationId,
                leaseId: consultation.LeaseId);
            updatedConsultation.RequestedOn = null;

            // Act
            repository.UpdateConsultation(updatedConsultation);

            // Assert
            notificationRepository.Verify(x => x.Delete(matchingNotification.NotificationId), Times.Once);
            notificationRepository.Verify(x => x.Delete(unrelatedNotification.NotificationId), Times.Never);
        }

        [Fact]
        public void UpdateConsultation_KeyNotFoundException()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            var updatedConsultation = EntityHelper.CreateLeaseConsultationItem();
            updatedConsultation.IsResponseReceived = true;

            // Act
            Action act = () => repository.UpdateConsultation(updatedConsultation);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void DeleteConsultationDocument_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            var consultation = EntityHelper.CreateLeaseConsultationItem();
            _helper.AddAndSaveChanges(consultation);

            // Act
            var result = repository.TryDeleteConsultation(consultation.LeaseConsultationId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TryDeleteConsultation_DeletesMatchingNotifications()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);
            _helper.CreatePimsContext(user, true);

            var notificationRepository = new Mock<INotificationRepository>();
            _helper.AddSingleton(notificationRepository.Object);

            var repository = _helper.CreateRepository<ConsultationRepository>(user);
            var consultation = EntityHelper.CreateLeaseConsultationItem();
            _helper.AddAndSaveChanges(consultation);

            var matchingNotification = new PimsNotification
            {
                NotificationId = 1,
                NotificationTypeCode = "TEST",
                LeaseId = consultation.LeaseId,
                LeaseConsultationId = consultation.LeaseConsultationId,
            };
            var unrelatedNotification = new PimsNotification
            {
                NotificationId = 2,
                NotificationTypeCode = "TEST",
                LeaseId = consultation.LeaseId + 1,
                LeaseConsultationId = consultation.LeaseConsultationId,
            };
            _helper.AddAndSaveChanges(matchingNotification, unrelatedNotification);

            // Act
            var result = repository.TryDeleteConsultation(consultation.LeaseConsultationId);

            // Assert
            result.Should().BeTrue();
            notificationRepository.Verify(x => x.Delete(matchingNotification.NotificationId), Times.Once);
            notificationRepository.Verify(x => x.Delete(unrelatedNotification.NotificationId), Times.Never);
        }

        [Fact]
        public void DeleteConsultationDocument_Null()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            // Act
            var result = repository.TryDeleteConsultation(1);

            // Assert
            result.Should().BeFalse();
        }
    }
}
