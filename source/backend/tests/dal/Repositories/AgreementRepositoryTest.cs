using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AgreementRepositoryTest
    {
        #region Constructors
        public AgreementRepositoryTest() { }
        #endregion

        #region Tests

        [Fact]
        public void TryDeleteAgreement_RemovesItsNotificationsAndChildren()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var context = helper.CreatePimsContext(user, true);
            var notificationRepository = new NotificationRepository(context, user, NullLogger<NotificationRepository>.Instance);
            var repository = new AgreementRepository(context, user, NullLogger<AgreementRepository>.Instance, notificationRepository);
            var agreement = new PimsAgreement
            {
                AcquisitionFile = EntityHelper.CreateAcquisitionFile(),
                AgreementTypeCode = "test",
                AgreementStatusTypeCode = "draft",
            };
            context.Add(agreement);
            context.SaveChanges();
            var notifications = new[]
            {
                new PimsNotification { AgreementId = agreement.AgreementId, NotificationTypeCode = "AGMT_POSNDT" },
                new PimsNotification { AgreementId = agreement.AgreementId, NotificationTypeCode = "AGMT_COMPTDT" },
            };
            foreach (var notification in notifications)
            {
                var notificationUser = new PimsNotificationUser { UserId = 1 };
                notificationUser.PimsNotificationUserOutputs.Add(new PimsNotificationUserOutput { NotificationOutputTypeCode = "EMAIL" });
                notification.PimsNotificationUsers.Add(notificationUser);
            }
            var unrelated = new PimsNotification { NotificationTypeCode = "AGMT_POSNDT" };
            context.AddRange(notifications);
            context.Add(unrelated);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            var result = repository.TryDeleteAgreement(agreement.AcquisitionFileId, agreement.AgreementId);
            repository.CommitTransaction();
            context.ChangeTracker.Clear();

            result.Should().BeTrue();
            context.PimsAgreements.Any(a => a.AgreementId == agreement.AgreementId).Should().BeFalse();
            context.PimsNotifications.Select(n => n.NotificationId).Should().Equal(unrelated.NotificationId);
            context.PimsNotificationUsers.Should().BeEmpty();
            context.PimsNotificationUserOutputs.Should().BeEmpty();
        }

        #region SearchAgreements
        [Fact]
        public void SearchAgreement_Project()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.Project = new PimsProject() { Id = 1, Description = "project", ProjectStatusTypeCode ="draft" };
            var agreement = new PimsAgreement()
            {
                AcquisitionFile = acqFile,
                AgreementTypeCodeNavigation = new PimsAgreementType() { AgreementTypeCode = "test", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "test" },
                AgreementStatusTypeCodeNavigation = new PimsAgreementStatusType() { AgreementStatusTypeCode = "draft", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "draft" },
                AgreementStatusTypeCode = "draft"
            };
            var filter = new AcquisitionReportFilterModel() { Projects = new List<long> { 1 } };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(agreement);

            var repository = helper.CreateRepository<AgreementRepository>(user);

            // Act
            var result = repository.SearchAgreements(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchAgreement_Team_Person()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = "EXPRAGENT", } };
            var agreement = new PimsAgreement()
            {
                AcquisitionFile = acqFile,
                AgreementTypeCodeNavigation = new PimsAgreementType() { AgreementTypeCode = "test", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "test" },
                AgreementStatusTypeCodeNavigation = new PimsAgreementStatusType() { AgreementStatusTypeCode = "draft", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "draft" },
                AgreementStatusTypeCode = "draft"
            };
            var filter = new AcquisitionReportFilterModel() { AcquisitionTeamPersons = new List<long> { 1 } };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(agreement);

            var repository = helper.CreateRepository<AgreementRepository>(user);

            // Act
            var result = repository.SearchAgreements(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchAgreement_Team_Organization()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { OrganizationId = 100, AcqFlTeamProfileTypeCode = "EXPRAGENT", } };
            var agreement = new PimsAgreement()
            {
                AcquisitionFile = acqFile,
                AgreementTypeCodeNavigation = new PimsAgreementType() { AgreementTypeCode = "test", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "test" },
                AgreementStatusTypeCodeNavigation = new PimsAgreementStatusType() { AgreementStatusTypeCode = "draft", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "draft" },
                AgreementStatusTypeCode = "draft"
            };
            var filter = new AcquisitionReportFilterModel() { AcquisitionTeamOrganizations = new List<long> { 100 } };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(agreement);

            var repository = helper.CreateRepository<AgreementRepository>(user);

            // Act
            var result = repository.SearchAgreements(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchAgreement_TeamAndProject()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = "EXPRAGENT", } };
            var agreement = new PimsAgreement()
            {
                AcquisitionFile = acqFile,
                AgreementTypeCodeNavigation = new PimsAgreementType() { AgreementTypeCode = "test", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "test" },
                AgreementStatusTypeCodeNavigation = new PimsAgreementStatusType() { AgreementStatusTypeCode = "draft", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "draft" },
                AgreementStatusTypeCode = "draft"
            };
            var filter = new AcquisitionReportFilterModel() { AcquisitionTeamPersons = new List<long> { 1 }, Projects = new List<long> { 1 } };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(agreement);

            var repository = helper.CreateRepository<AgreementRepository>(user);

            // Act
            var result = repository.SearchAgreements(filter);

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public void SearchAgreement()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            var agreement = new PimsAgreement()
            {
                AcquisitionFile = acqFile,
                AgreementTypeCodeNavigation = new PimsAgreementType() { AgreementTypeCode = "test", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "test" },
                AgreementStatusTypeCodeNavigation = new PimsAgreementStatusType() { AgreementStatusTypeCode = "draft", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "draft" },
                AgreementStatusTypeCode = "draft"
            };
            var filter = new AcquisitionReportFilterModel();

            helper.CreatePimsContext(user, true).AddAndSaveChanges(agreement);

            var repository = helper.CreateRepository<AgreementRepository>(user);

            // Act
            var result = repository.SearchAgreements(filter);

            // Assert
            result.Should().HaveCount(1);
        }
        #endregion

        #endregion
    }
}
