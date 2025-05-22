using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
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
