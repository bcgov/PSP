using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;
using Pims.Core.Exceptions;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "compensation-requisition")]
    [ExcludeFromCodeCoverage]

    public class CompReqFinancialServiceTest
    {
        private readonly TestHelper _helper;

        public CompReqFinancialServiceTest()
        {
            this._helper = new TestHelper();
        }

        [Fact]
        public void GetAllByAcquisitionFileId_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView);
            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), null));

            // Act
            service.GetAllByAcquisitionFileId(1, null);

            // Assert
            repo.Verify(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), null), Times.Once);
        }

        [Fact]
        public void GetAllByAcquisitionFileId_Unauthorized()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionEdit);
            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();

            // Act
            Action act = () => service.GetAllByAcquisitionFileId(1, null);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Unauthorized()
        {
            // Arrange
            var service = this.CreateWithPermissions();
            var filter = new AcquisitionReportFilterModel();

            // Act
            Action act = () => service.SearchCompensationRequisitionFinancials(filter);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView, Permissions.ProjectView, Permissions.LeaseView);

            // Compensation requisitions can be associated to Acquisition Files or Leases
            var acquisitionFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { RegionCode = 1 } } };
            var leaseFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { Lease = new PimsLease { RegionCode = 2 } } };

            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(new List<PimsCompReqFinancial>() { acquisitionFinancial, leaseFinancial });

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: false);
            user.PimsRegionUsers = new List<PimsRegionUser> { new PimsRegionUser { RegionCode = 1 }, new PimsRegionUser { RegionCode = 2 } };
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            var filter = new AcquisitionReportFilterModel();

            // Act
            var financials = service.SearchCompensationRequisitionFinancials(filter);

            // Assert
            financials.Should().HaveCount(2);
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Region_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView, Permissions.ProjectView);

            var matchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { RegionCode = 1 } } };
            var nonMatchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { RegionCode = 2 } } };
            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(new List<PimsCompReqFinancial> { matchingFinancial, nonMatchingFinancial });

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: false);
            user.PimsRegionUsers = new List<PimsRegionUser> { new PimsRegionUser { RegionCode = 1 } };
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            var filter = new AcquisitionReportFilterModel();

            // Act
            var financials = service.SearchCompensationRequisitionFinancials(filter);

            // Assert
            financials.Should().HaveCount(1);
            financials.FirstOrDefault().CompensationRequisition.AcquisitionFile.RegionCode.Should().Be(1);
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Region_Leases_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView, Permissions.ProjectView);

            var matchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { Lease = new PimsLease { RegionCode = 1 } } };
            var nonMatchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { Lease = new PimsLease { RegionCode = 2 } } };
            var noRegionLeaseFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { Lease = new PimsLease { RegionCode = null } } }; // Region is OPTIONAL for leases and can be NULL

            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(new List<PimsCompReqFinancial> { noRegionLeaseFinancial, matchingFinancial, nonMatchingFinancial });

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: false);
            user.PimsRegionUsers = new List<PimsRegionUser> { new PimsRegionUser { RegionCode = 1 } };
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            var filter = new AcquisitionReportFilterModel();

            // Act
            var financials = service.SearchCompensationRequisitionFinancials(filter);

            // Assert - for Leases without region, export them regardless of the users' region. Otherwise filter lease data by the user’s region.
            financials.Should().HaveCount(2);
            financials.FirstOrDefault().CompensationRequisition.Lease.RegionCode.Should().BeNull();
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Contractor_Filter()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView, Permissions.ProjectView);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            var matchingFinancial = new PimsCompReqFinancial
            {
                CompensationRequisition = new PimsCompensationRequisition
                {
                    AcquisitionFile = new PimsAcquisitionFile
                    {
                        RegionCode = 1,
                        PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam> { new PimsAcquisitionFileTeam { PersonId = contractorUser.PersonId } }
                    },
                },
            };
            var nonMatchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { } } };
            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(new List<PimsCompReqFinancial> { matchingFinancial, nonMatchingFinancial });

            contractorUser.PimsRegionUsers = new List<PimsRegionUser> { new PimsRegionUser { RegionCode = 1 } };
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var filter = new AcquisitionReportFilterModel();

            // Act
            var financials = service.SearchCompensationRequisitionFinancials(filter);

            // Assert
            financials.Should().HaveCount(1);
            financials.FirstOrDefault().CompensationRequisition.AcquisitionFile.PimsAcquisitionFileTeams.FirstOrDefault().PersonId.Should().Be(contractorUser.PersonId);
        }

        [Theory]
        [InlineData(new Permissions[] { Permissions.AcquisitionFileView }, 1)]
        [InlineData(new Permissions[] { Permissions.LeaseView }, 1)]
        [InlineData(new Permissions[] { Permissions.AcquisitionFileView, Permissions.LeaseView }, 2)]
        public void SearchCompensationRequisitionFinancials_Returns_FileTypes_Based_On_UserPermissions(Permissions[] permissions, int expected)
        {
            // Arrange
            Permissions[] basePermissions = { Permissions.CompensationRequisitionView, Permissions.ProjectView };
            var service = this.CreateWithPermissions(basePermissions.Concat(permissions).ToArray());

            // Compensation requisitions can be associated to Acquisition Files or Leases
            var acquisitionFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { RegionCode = 1 } } };
            var leaseFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { Lease = new PimsLease { RegionCode = 2 } } };

            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>(), true, false)).Returns(new List<PimsCompReqFinancial>() { acquisitionFinancial });
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>(), false, true)).Returns(new List<PimsCompReqFinancial>() { leaseFinancial });
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>(), true, true)).Returns(new List<PimsCompReqFinancial>() { acquisitionFinancial, leaseFinancial });

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: false);
            user.PimsRegionUsers = new List<PimsRegionUser> { new PimsRegionUser { RegionCode = 1 }, new PimsRegionUser { RegionCode = 2 } };
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            var filter = new AcquisitionReportFilterModel();

            // Act
            var financials = service.SearchCompensationRequisitionFinancials(filter);

            // Assert
            financials.Should().HaveCount(expected);
        }
        private CompReqFinancialService CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<CompReqFinancialService>(user);
        }
    }
}
