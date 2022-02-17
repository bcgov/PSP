using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using System.Collections.Generic;
using Model = Pims.Api.Areas.Lease.Models.Lease;
using Pims.Dal.Services;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseTermControllerTest
    {
        #region Tests
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeaseTerms_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseTermController>(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new Dal.Entities.PimsLeaseTerm() { LeaseTermId = 1};

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.LeaseTermService.UpdateTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(lease);

            // Act
            var result = controller.UpdateTerm(lease.LeaseId, leaseTerm.LeaseTermId, mapper.Map<Model.TermModel>(leaseTerm));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.LeaseTermService.UpdateTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void DeleteLeaseTerms_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseTermController>(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new Dal.Entities.PimsLeaseTerm();

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.LeaseTermService.DeleteTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(lease);

            // Act
            var result = controller.DeleteTerm(lease.LeaseId, mapper.Map<Model.TermModel>(leaseTerm));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.LeaseTermService.DeleteTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void AddLeaseTerms_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseTermController>(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new Dal.Entities.PimsLeaseTerm();

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.LeaseTermService.AddTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(lease);

            // Act
            var result = controller.AddTerm(lease.LeaseId, mapper.Map<Model.TermModel>(leaseTerm));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.LeaseTermService.AddTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
        }
        #endregion
    }
}
