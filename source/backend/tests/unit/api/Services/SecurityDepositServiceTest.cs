using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class SecurityDepositServiceTest
    {
        private TestHelper _helper;

        public SecurityDepositServiceTest()
        {
            this._helper = new TestHelper();
        }

        private SecurityDepositService CreateSecurityDepositService(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<SecurityDepositService>();
        }

        #region Tests

        #region Add
        [Fact]
        public void Add_FinalFile()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateSecurityDepositService(Permissions.LeaseAdd);
            var leaseService = this._helper.GetService<Mock<ILeaseService>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditPayments(It.IsAny<LeaseStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.AddLeaseDeposit(1, new PimsSecurityDeposit());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }
        #endregion

        #region Update
        [Fact]
        public void Update_FinalFile()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateSecurityDepositService(Permissions.LeaseEdit);
            var leaseService = this._helper.GetService<Mock<ILeaseService>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditPayments(It.IsAny<LeaseStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.UpdateLeaseDeposit(1, new PimsSecurityDeposit());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void UpdateNote_FinalFile()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateSecurityDepositService(Permissions.LeaseEdit);
            var leaseService = this._helper.GetService<Mock<ILeaseService>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditPayments(It.IsAny<LeaseStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.UpdateLeaseDepositNote(1, "new note");

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_FinalFile()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateSecurityDepositService(Permissions.LeaseEdit);
            var leaseService = this._helper.GetService<Mock<ILeaseService>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditPayments(It.IsAny<LeaseStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.DeleteLeaseDeposit(new PimsSecurityDeposit());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }
        #endregion


        #endregion
    }
}
