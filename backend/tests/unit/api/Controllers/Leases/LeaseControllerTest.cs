using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Model = Pims.Api.Areas.Lease.Models.Lease;
using System.Linq;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseControllerTest
    {
        #region Tests
        #region GetLeases
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void GetLeases_All_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.PropertyView);

            var lease = EntityHelper.CreateLease(1);

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Lease.Get(It.IsAny<long>())).Returns(lease);

            // Act
            var result = controller.GetLease(1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lease.Get(It.IsAny<long>()), Times.Once());
        }
        #endregion
        #region UpdateLeases
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeases_All_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.PropertyEdit);

            var lease = EntityHelper.CreateLease(1);

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Lease.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>())).Returns(lease);
            service.Setup(m => m.Lease.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyLease>>(), It.IsAny<bool>())).Returns(lease);

            // Act
            var result = controller.UpdateLease(mapper.Map<Model.LeaseModel>(lease));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lease.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>()), Times.Once());
        }

        [Fact]
        public void UpdateLeases_AreaUnit_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.PropertyEdit);

            var lease = EntityHelper.CreateLease(1);
            var propertyLease = new PimsPropertyLease() { AreaUnitTypeCode = "HA", AreaUnitTypeCodeNavigation = new PimsAreaUnitType() { AreaUnitTypeCode = "HA" }, LeaseArea = 1 };
            lease.PimsPropertyLeases.Clear();
            lease.PimsPropertyLeases.Add(propertyLease);

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Lease.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>())).Returns(lease);
            service.Setup(m => m.Lease.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyLease>>(), It.IsAny<bool>())).Returns(lease);

            // Act
            var result = controller.UpdateLease(mapper.Map<Model.LeaseModel>(lease));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            Assert.Equal("HA", actualResult.Properties.First().AreaUnitType.Id);
            Assert.Equal(1, actualResult.Properties.First().LandArea);
            service.Verify(m => m.Lease.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>()), Times.Once());
        }
        #endregion
        #region UpdateImprovements
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateImprovements_All_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyImprovementController>(Permissions.PropertyEdit);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements = new List<Pims.Dal.Entities.PimsPropertyImprovement>() { new Dal.Entities.PimsPropertyImprovement() { Id = 1 } };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Lease.UpdateLeaseImprovements(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>())).Returns(lease);

            // Act
            var result = controller.UpdateImprovements(lease.Id, mapper.Map<Model.LeaseModel>(lease));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lease.UpdateLeaseImprovements(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
