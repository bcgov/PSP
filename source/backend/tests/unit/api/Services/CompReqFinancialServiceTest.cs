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
using Pims.Dal.Security;
using Xunit;

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
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView, Permissions.ProjectView);
            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>())).Returns(new List<PimsCompReqFinancial>());

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var filter = new AcquisitionReportFilterModel();

            // Act
            var financials = service.SearchCompensationRequisitionFinancials(filter);

            // Assert
            financials.Should().BeEmpty();
        }

        [Fact]
        public void SearchCompensationRequisitionFinancials_Region_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView, Permissions.ProjectView);

            var matchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { RegionCode = 1 } } };
            var nonMatchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { RegionCode = 2 } } };
            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>())).Returns(new List<PimsCompReqFinancial> { matchingFinancial, nonMatchingFinancial });

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
                        PimsAcquisitionFilePeople = new List<PimsAcquisitionFilePerson> { new PimsAcquisitionFilePerson { PersonId = contractorUser.PersonId } }
                    },
                },
            };
            var nonMatchingFinancial = new PimsCompReqFinancial { CompensationRequisition = new PimsCompensationRequisition { AcquisitionFile = new PimsAcquisitionFile { } } };
            var repo = this._helper.GetService<Mock<ICompReqFinancialRepository>>();
            repo.Setup(x => x.SearchCompensationRequisitionFinancials(It.IsAny<AcquisitionReportFilterModel>())).Returns(new List<PimsCompReqFinancial> { matchingFinancial, nonMatchingFinancial });

            contractorUser.PimsRegionUsers = new List<PimsRegionUser> { new PimsRegionUser { RegionCode = 1 } };
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var filter = new AcquisitionReportFilterModel();

            // Act
            var financials = service.SearchCompensationRequisitionFinancials(filter);

            // Assert
            financials.Should().HaveCount(1);
            financials.FirstOrDefault().CompensationRequisition.AcquisitionFile.PimsAcquisitionFilePeople.FirstOrDefault().PersonId.Should().Be(contractorUser.PersonId);
        }

        private CompReqFinancialService CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<CompReqFinancialService>(user);
        }
    }
}
