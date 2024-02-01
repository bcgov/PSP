using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
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
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { PersonId = 1 } };
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
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { OrganizationId = 100 } };
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
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { PersonId = 1 } };
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
