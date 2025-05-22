using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;
using Pims.Core.Exceptions;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class CompReqFinancialRepositoryTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;

        public CompReqFinancialRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private CompReqFinancialRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<CompReqFinancialRepository>(user);
        }

        [Fact]
        public void GetAll_Success()
        {
            // Arrange
            var codeToAdd = new PimsCompReqFinancial()
            {
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    IsDraft = true,
                    AcquisitionFileId = 1,
                },
            };
            var repository = this.CreateWithPermissions(Permissions.CompensationRequisitionView);
            this._helper.AddAndSaveChanges(codeToAdd);

            // Act
            var result = repository.GetAllByAcquisitionFileId(1, false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsCompReqFinancial>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetAll_FinalOnly()
        {
            // Arrange
            var codeToAdd = new PimsCompReqFinancial()
            {
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition()
                {
                    IsDraft = true,
                    AcquisitionFileId = 1,
                },
            };
            var repository = this.CreateWithPermissions(Permissions.CompensationRequisitionView);
            this._helper.AddAndSaveChanges(codeToAdd);

            // Act
            var result = repository.GetAllByAcquisitionFileId(1, true);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsCompReqFinancial>>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetAll_ThrowIfNotAuthorized()
        {
            // Arrange
            var repository = this.CreateWithPermissions(Permissions.SystemAdmin);

            // Act
            Action act = () => repository.GetAllByAcquisitionFileId(1, true);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Project()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.Project = EntityHelper.CreateProject(1, "test", "description");
            acqFile.ProjectId = 1;
            var financial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    AcquisitionFileId = acqFile.Internal_Id,
                    AcquisitionFile = acqFile,
                },
            };

            var repository = this.CreateWithPermissions(Permissions.AcquisitionFileAdd);
            this._helper.AddAndSaveChanges(financial);

            // Act
            var filter = new AcquisitionReportFilterModel() { Projects = new List<long> { 1 } };
            var result = repository.SearchCompensationRequisitionFinancials(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Project_Lease()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            lease.Project = EntityHelper.CreateProject(1, "test", "description");
            lease.ProjectId = 1;
            var financial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    LeaseId = lease.Internal_Id,
                    Lease = lease,
                },
            };

            var repository = this.CreateWithPermissions(Permissions.LeaseAdd, Permissions.LeaseView, Permissions.CompensationRequisitionView);
            this._helper.AddAndSaveChanges(financial);

            // Act
            var filter = new AcquisitionReportFilterModel() { Projects = new List<long> { 1 } };
            var result = repository.SearchCompensationRequisitionFinancials(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_AlternateProject()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            var financial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    AcquisitionFileId = acqFile.Internal_Id,
                    AcquisitionFile = acqFile,
                    AlternateProject = EntityHelper.CreateProject(1, "test", "description"),
                    AlternateProjectId = 1,
                },
            };

            var repository = this.CreateWithPermissions(Permissions.AcquisitionFileAdd);
            this._helper.AddAndSaveChanges(financial);

            // Act
            var filter = new AcquisitionReportFilterModel() { Projects = new List<long> { 1 } };
            var result = repository.SearchCompensationRequisitionFinancials(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_AlternateProject_Leases()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var financial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    LeaseId = lease.Internal_Id,
                    Lease = lease,
                    AlternateProject = EntityHelper.CreateProject(1, "test", "description"),
                    AlternateProjectId = 1,
                },
            };

            var repository = this.CreateWithPermissions(Permissions.LeaseAdd, Permissions.LeaseView, Permissions.CompensationRequisitionView);
            this._helper.AddAndSaveChanges(financial);

            // Act
            var filter = new AcquisitionReportFilterModel() { Projects = new List<long> { 1 } };
            var result = repository.SearchCompensationRequisitionFinancials(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Team_Person()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { PersonId = 1 } };
            var financial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    AcquisitionFileId = acqFile.Internal_Id,
                    AcquisitionFile = acqFile,
                },
            };

            var repository = this.CreateWithPermissions(Permissions.AcquisitionFileAdd);
            this._helper.AddAndSaveChanges(financial);

            // Act
            var filter = new AcquisitionReportFilterModel() { AcquisitionTeamPersons = new List<long> { 1 } };
            var result = repository.SearchCompensationRequisitionFinancials(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Team_Organization()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { OrganizationId = 100 } };
            var financial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    AcquisitionFileId = acqFile.Internal_Id,
                    AcquisitionFile = acqFile,
                },
            };

            var repository = this.CreateWithPermissions(Permissions.AcquisitionFileAdd);
            this._helper.AddAndSaveChanges(financial);

            // Act
            var filter = new AcquisitionReportFilterModel() { AcquisitionTeamOrganizations = new List<long> { 100 } };
            var result = repository.SearchCompensationRequisitionFinancials(filter);

            // Assert
            result.Should().HaveCount(1);
        }


        [Theory]
        [InlineData(true, false, 1)]
        [InlineData(false, true, 1)]
        [InlineData(true, true, 2)]
        public void SearchCompensationRequisitionFinancials_Returns_FileTypes_Based_On_Paramenters(bool includeAcquisitions, bool includeLeases, int expected)
        {
            // Arrange
            var project = EntityHelper.CreateProject(1, "test", "description");
            var region = new PimsRegion("Northern") { RegionCode = 1, ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test" };

            var acqFile = EntityHelper.CreateAcquisitionFile(region: region);
            acqFile.Project = project;
            acqFile.ProjectId = project.Internal_Id;
            var acquisitionFinancial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 1,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    Internal_Id = 1,
                    AcquisitionFileId = acqFile.Internal_Id,
                    AcquisitionFile = acqFile,
                },
            };

            var lease = EntityHelper.CreateLease(1, region: region, addProperty: false);
            lease.Project = project;
            lease.ProjectId = project.Internal_Id;
            var leaseFinancial = new PimsCompReqFinancial
            {
                FinancialActivityCode = new PimsFinancialActivityCode { Code = "test", Description = "" },
                CompensationRequisitionId = 2,
                CompensationRequisition = new PimsCompensationRequisition
                {
                    Internal_Id = 2,
                    LeaseId = lease.Internal_Id,
                    Lease = lease,
                },
            };

            var repository = this.CreateWithPermissions(Permissions.LeaseAdd, Permissions.LeaseView, Permissions.CompensationRequisitionView, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);

            this._helper.AddAndSaveChanges(region);
            if (includeAcquisitions)
            {
                this._helper.AddAndSaveChanges(acquisitionFinancial);
            }

            if (includeLeases)
            {
                this._helper.AddAndSaveChanges(leaseFinancial);
            }


            // Act
            var filter = new AcquisitionReportFilterModel() { Projects = new List<long> { 1 } };
            var result = repository.SearchCompensationRequisitionFinancials(filter, includeAcquisitions, includeLeases);

            // Assert
            result.Should().HaveCount(expected);
        }

    }
}
