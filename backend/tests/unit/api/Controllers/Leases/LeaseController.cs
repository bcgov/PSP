using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseControllerTest
    {
        #region Tests
        #region GetProperties
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void GetProperties_All_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.PropertyView);

            var lease = EntityHelper.CreateLease(1);

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Lease.Get(It.IsAny<int>())).Returns(lease);

            // Act
            var result = controller.GetLease(1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lease.Get(It.IsAny<int>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
