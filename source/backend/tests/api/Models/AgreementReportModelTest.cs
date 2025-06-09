using System;
using System.Collections.Generic;
using FluentAssertions;
using Pims.Api.Areas.Reports.Models.Agreement;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test
{
    public class AgreementReportModelTest
    {
        [Fact]
        public void AgreementReportModel_TeamMember_Person()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            var propCoord = new PimsAcquisitionFileTeam() { AcqFlTeamProfileTypeCode = "PROPCOORD", PersonId = 1, Person = new PimsPerson() { PersonId = 1, Surname = "test" } };
            testAgreement.AcquisitionFile = new Dal.Entities.PimsAcquisitionFile() { PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { propCoord } };

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.PropertyCoordinator.Should().Be("test");
        }

        [Fact]
        public void AgreementReportModel_TeamMember_Organization()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            var propCoord = new PimsAcquisitionFileTeam() { AcqFlTeamProfileTypeCode = "PROPCOORD", OrganizationId = 100, Organization = new PimsOrganization() { OrganizationId = 100, Name = "FORTIS BC" } };
            testAgreement.AcquisitionFile = new Dal.Entities.PimsAcquisitionFile() { PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { propCoord } };

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.PropertyCoordinator.Should().Be("FORTIS BC");
        }

        [Fact]
        public void AgreementReportModel_TeamMember_Null()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            testAgreement.AcquisitionFile = new Dal.Entities.PimsAcquisitionFile() { PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { } };

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.PropertyCoordinator.Should().Be(string.Empty);
        }

        [Fact]
        public void AgreementReportModel_Date_Null()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            testAgreement.AgreementDate = null;

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.AgreementDate.Should().Be(string.Empty);
        }

        [Fact]
        public void AgreementReportModel_Date()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            testAgreement.AgreementDate = new DateOnly(1990, 1, 1);

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.AgreementDate.Should().Be("1990-01-01");
        }

        [Fact]
        public void AgreementReportModel_Project()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            testAgreement.AcquisitionFile = new Dal.Entities.PimsAcquisitionFile() { Project = new PimsProject() { Code = "test", Description = "description" } };

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.MinistryProject.Should().Be("test - description");
        }

        [Fact]
        public void AgreementReportModel_Project_Null()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            testAgreement.AcquisitionFile = new Dal.Entities.PimsAcquisitionFile() { Project = null };

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.MinistryProject.Should().Be(string.Empty);
        }

        [Fact]
        public void AgreementReportModel_Product()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            testAgreement.AcquisitionFile = new Dal.Entities.PimsAcquisitionFile() { Product = new PimsProduct() { Code = "test", Description = "description" } };

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.Product.Should().Be("test - description");
        }

        [Fact]
        public void AgreementReportModel_Product_Null()
        {
            // Arrange
            var testAgreement = new Dal.Entities.PimsAgreement();
            testAgreement.AcquisitionFile = new Dal.Entities.PimsAcquisitionFile() { Product = null };

            // Act
            var model = new AgreementReportModel(testAgreement, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.Product.Should().Be(string.Empty);
        }
    }
}
