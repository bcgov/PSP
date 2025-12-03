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
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class SecurityDepositReturnServiceTest
    {
        private TestHelper _helper;

        public SecurityDepositReturnServiceTest()
        {
            this._helper = new TestHelper();
        }

        private SecurityDepositReturnService CreateSecurityDepositReturnService(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<SecurityDepositReturnService>();
        }

        #region Tests

        #region Add
        [Fact]
        public void Add_FinalFile()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateSecurityDepositReturnService(Permissions.LeaseAdd);
            var leaseService = this._helper.GetService<Mock<ILeaseService>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditDeposits(It.IsAny<LeaseStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.AddLeaseDepositReturn(1, new PimsSecurityDepositReturn());

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

            var service = this.CreateSecurityDepositReturnService(Permissions.LeaseEdit);
            var leaseService = this._helper.GetService<Mock<ILeaseService>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditDeposits(It.IsAny<LeaseStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.UpdateLeaseDepositReturn(1, new PimsSecurityDepositReturn());

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

            var service = this.CreateSecurityDepositReturnService(Permissions.LeaseEdit);
            var leaseService = this._helper.GetService<Mock<ILeaseService>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);

            var solver = this._helper.GetService<Mock<ILeaseStatusSolver>>();
            solver.Setup(x => x.CanEditDeposits(It.IsAny<LeaseStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.DeleteLeaseDepositReturn(1, new PimsSecurityDepositReturn());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }
        #endregion


        #endregion
    }
}
