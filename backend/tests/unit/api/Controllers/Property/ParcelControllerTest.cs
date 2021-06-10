using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Property.Controllers;
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
using SModel = Pims.Api.Areas.Property.Models.Parcel;

namespace Pims.Api.Test.Controllers.Property
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class ParcelControllerTest
    {
        #region Variables
        public static IEnumerable<object[]> PropertyQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/properties?Agencies=1,2") },
            new object [] { new Uri("http://host/api/properties?StatusId=2") },
        };
        #endregion

        #region Tests
        #region GetParcel
        [Fact]
        public void GetParcel_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);

            var parcel = new Entity.Parcel(1, 51, 25);
            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Parcel.Get(It.IsAny<int>())).Returns(parcel);

            // Act
            var result = controller.GetParcel(1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.ParcelModel>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.ParcelModel>(parcel);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.Get(It.IsAny<int>()), Times.Once());
        }
        #endregion

        #region GetParcels With Query
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void GetParcelsWithQuery_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView, uri);

            var parcel = new Entity.Parcel(1, 51, 25);
            var parcels = new[] { parcel };
            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Parcel.Get(It.IsAny<ParcelFilter>())).Returns(parcels);

            // Act
            var result = controller.GetParcels();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.ParcelModel[]>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.ParcelModel[]>(parcels);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.Get(It.IsAny<ParcelFilter>()), Times.Once());
        }
        #endregion

        #region GetParcels With Filter
        [Fact]
        public void GetParcelsWithFilter_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);

            var parcel = new Entity.Parcel(1, 51, 25);
            var parcels = new[] { parcel };
            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Parcel.Get(It.IsAny<ParcelFilter>())).Returns(parcels);
            var filter = new ParcelFilter(1, 1, 1, 1);

            // Act
            var result = controller.GetParcels(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.ParcelModel[]>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.ParcelModel[]>(parcels);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.Get(It.IsAny<ParcelFilter>()), Times.Once());
        }
        #endregion

        #region IsPidAvailable
        [Fact]
        public void IsPidAvailable_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);
            var service = helper.GetService<Mock<IPimsService>>();

            service.Setup(m => m.Parcel.IsPidAvailable(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = controller.IsPidAvailable(1, 1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.CheckPidAvailabilityResponseModel>(actionResult.Value);
            var expectedResult = new SModel.CheckPidAvailabilityResponseModel() { Available = true };
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.IsPidAvailable(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }
        #endregion

        #region IsPinAvailable
        [Fact]
        public void IsPinAvailable_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);
            var service = helper.GetService<Mock<IPimsService>>();

            service.Setup(m => m.Parcel.IsPinAvailable(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = controller.IsPinAvailable(1, 1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.CheckPidAvailabilityResponseModel>(actionResult.Value);
            var expectedResult = new SModel.CheckPidAvailabilityResponseModel() { Available = true };
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.IsPinAvailable(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }
        #endregion

        #region AddParcel
        [Fact]
        public void AddParcel_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);

            var parcel = new Entity.Parcel(1, 51, 25);
            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Parcel.Add(It.IsAny<Entity.Parcel>())).Returns(parcel);

            var model = mapper.Map<SModel.ParcelModel>(parcel);

            // Act
            var result = controller.AddParcel(model);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualResult = Assert.IsType<SModel.ParcelModel>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.ParcelModel>(parcel);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.Add(It.IsAny<Entity.Parcel>()), Times.Once());
        }
        #endregion

        #region UpdateParcel
        [Fact]
        public void UpdateParcel_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);

            var parcel = new Entity.Parcel(1, 51, 25);
            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Parcel.Update(It.IsAny<Entity.Parcel>())).Returns(parcel);

            var model = mapper.Map<SModel.ParcelModel>(parcel);

            // Act
            var result = controller.UpdateParcel(1, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.ParcelModel>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.ParcelModel>(parcel);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.Update(It.IsAny<Entity.Parcel>()), Times.Once());
        }
        #endregion

        #region UpdateParcelFinancials
        [Fact]
        public void UpdateParcelFinancials_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);

            var parcel = new Entity.Parcel(1, 51, 25);
            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Parcel.UpdateFinancials(It.IsAny<Entity.Parcel>())).Returns(parcel);

            var model = mapper.Map<SModel.ParcelModel>(parcel);

            // Act
            var result = controller.UpdateParcelFinancials(1, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.ParcelModel>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.ParcelModel>(parcel);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.UpdateFinancials(It.IsAny<Entity.Parcel>()), Times.Once());
        }
        #endregion

        #region DeleteParcel
        [Fact]
        public void DeleteParcel_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ParcelController>(Permissions.PropertyView);

            var parcel = new Entity.Parcel(1, 51, 25);
            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Parcel.Remove(It.IsAny<Entity.Parcel>()));

            var model = mapper.Map<SModel.ParcelModel>(parcel);

            // Act
            var result = controller.DeleteParcel(1, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.ParcelModel>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.ParcelModel>(parcel);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Parcel.Remove(It.IsAny<Entity.Parcel>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
