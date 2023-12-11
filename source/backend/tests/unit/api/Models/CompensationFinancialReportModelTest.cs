using System;
using System.Collections.Generic;
using FluentAssertions;
using Pims.Api.Areas.Reports.Models.Acquisition;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test
{
    public class CompensationFinancialReportModelTest
    {
        [Fact]
        public void CompensationFinancialReportModel_Date()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition() { FinalizedDate = new DateTime(1990, 1, 1) },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.FinalDate.Should().Be("1990-01-01");
        }

        [Fact]
        public void CompensationFinancialReportModel_Date_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition() { FinalizedDate = null },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.FinalDate.Should().Be(string.Empty);
        }

        [Fact]
        public void CompensationFinancialReportModel_RequisitionState()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition() { IsDraft = false },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.RequisitionState.Should().Be("Final");
        }

        [Fact]
        public void CompensationFinancialReportModel_RequisitionState_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition() { IsDraft = null },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.RequisitionState.Should().Be("Draft");
        }

        [Fact]
        public void CompensationFinancialReportModel_FileNumberAndName()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AcquisitionFile = new PimsAcquisitionFile() { FileNumber = "9999", FileName = "test" },
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.AcquisitionNumberAndName.Should().Be("9999 - test");
        }

        [Fact]
        public void CompensationFinancialReportModel_FileNumberAndName_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AcquisitionFile = null,
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.AcquisitionNumberAndName.Should().Be(string.Empty);
        }

        [Fact]
        public void CompensationFinancialReportModel_Project()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AcquisitionFile = new PimsAcquisitionFile() { Project = new PimsProject() { Code = "test", Description = "description" } },
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.MinistryProject.Should().Be("test - description");
        }

        [Fact]
        public void CompensationFinancialReportModel_Project_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AcquisitionFile = new PimsAcquisitionFile() { Project = null },
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.MinistryProject.Should().Be(string.Empty);
        }

        [Fact]
        public void CompensationFinancialReportModel_AlternateProject()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AlternateProject = new PimsProject() { Code = "alternate", Description = "project" },
                    AcquisitionFile = new PimsAcquisitionFile() { Project = new PimsProject() { Code = "test", Description = "description" } },
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.AlternateProject.Should().Be("alternate - project");
        }

        [Fact]
        public void CompensationFinancialReportModel_AlternateProject_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AlternateProject = null,
                    AcquisitionFile = new PimsAcquisitionFile() { Project = new PimsProject() { Code = "test", Description = "description" } },
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.AlternateProject.Should().Be(string.Empty);
        }

        [Fact]
        public void CompensationFinancialReportModel_Product()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AcquisitionFile = new PimsAcquisitionFile() { Product = new PimsProduct() { Code = "test", Description = "description" } },
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.Product.Should().Be("test - description");
        }

        [Fact]
        public void CompensationFinancialReportModel_Product_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    AcquisitionFile = new PimsAcquisitionFile() { Product = null },
                },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.Product.Should().Be(string.Empty);
        }

        [Fact]
        public void CompensationFinancialReportModel_FinancialActivity()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode() { Code = "test", Description = "description" },
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.FinancialActivityName.Should().Be("test - description");
        }

        [Fact]
        public void CompensationFinancialReportModel_FinancialActivity_Null()
        {
            var testFinancial = new PimsCompReqFinancial
            {
                FinancialActivityCode = null,
            };

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial>()), new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.FinancialActivityName.Should().Be(string.Empty);
        }

        [Fact]
        public void CompensationFinancialReportModel_ProjectDraftAmounts()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                PretaxAmt = 100,
                TaxAmt = 50,
                TotalAmt = 150,
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    IsDraft = true,
                    AcquisitionFile = new PimsAcquisitionFile() { Project = new PimsProject() { Id = 1, Code = "test", Description = "description" } },
                },
            };

            var totals = new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial> { testFinancial });

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, totals, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.ProjectDraftPreTaxAmount.Should().Be(100);
            model.ProjectDraftTaxAmount.Should().Be(50);
            model.ProjectDraftTotalAmount.Should().Be(150);
        }

        [Fact]
        public void CompensationFinancialReportModel_ProjectDraftAmounts_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                PretaxAmt = 100,
                TaxAmt = 50,
                TotalAmt = 150,
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    IsDraft = true,
                    AcquisitionFile = new PimsAcquisitionFile() { Project = null },
                },
            };

            var totals = new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial> { testFinancial });

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, totals, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.ProjectDraftPreTaxAmount.Should().Be(0);
            model.ProjectDraftTaxAmount.Should().Be(0);
            model.ProjectDraftTotalAmount.Should().Be(0);
        }

        [Fact]
        public void CompensationFinancialReportModel_ProjectFinalAmounts()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                PretaxAmt = 100,
                TaxAmt = 50,
                TotalAmt = 150,
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    IsDraft = false,
                    AcquisitionFile = new PimsAcquisitionFile() { Project = new PimsProject() { Id = 1, Code = "test", Description = "description" } },
                },
            };

            var totals = new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial> { testFinancial });

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, totals, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.ProjectFinalPreTaxAmount.Should().Be(100);
            model.ProjectFinalTaxAmount.Should().Be(50);
            model.ProjectFinalTotalAmount.Should().Be(150);
        }

        [Fact]
        public void CompensationFinancialReportModel_ProjectFinalAmounts_Null()
        {
            // Arrange
            var testFinancial = new PimsCompReqFinancial
            {
                PretaxAmt = 100,
                TaxAmt = 50,
                TotalAmt = 150,
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    IsDraft = false,
                    AcquisitionFile = new PimsAcquisitionFile() { Project = null },
                },
            };

            var totals = new CompensationFinancialReportTotalsModel(new List<PimsCompReqFinancial> { testFinancial });

            // Act
            var model = new CompensationFinancialReportModel(testFinancial, totals, new System.Security.Claims.ClaimsPrincipal());

            // Assert
            model.ProjectFinalPreTaxAmount.Should().Be(0);
            model.ProjectFinalTaxAmount.Should().Be(0);
            model.ProjectFinalTotalAmount.Should().Be(0);
        }
    }
}
