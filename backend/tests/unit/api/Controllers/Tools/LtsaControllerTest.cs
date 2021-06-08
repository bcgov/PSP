using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Tools.Controllers;
using Pims.Ltsa;
using Pims.Core.Test;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Model = Pims.Ltsa.Models;

namespace Pims.Api.Test.Controllers.Tools
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "tools")]
    [Trait("group", "geocoder")]
    [ExcludeFromCodeCoverage]
    public class LtsaControllerTest
    {
        #region Variables
        #endregion

        #region Constructors
        public LtsaControllerTest() { }
        #endregion

        #region Tests
        #region FindTitleSummariesAsync
        [Fact]
        public async void FindTitleSummariesAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LtsaController>(Permissions.PropertyEdit);

            var response = new Model.TitleSummariesResponse()
            {
                TitleSummaries = new List<Model.TitleSummary>()
            };

            var service = helper.GetService<Mock<ILtsaService>>();
            service.Setup(m => m.GetTitleSummariesAsync(It.IsAny<Int32>())).ReturnsAsync(response);

            // Act
            var result = await controller.FindTitleSummariesAsync("123-456-789");

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.TitleSummariesResponse>(actionResult.Value);
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostTitleOrderAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LtsaController>(Permissions.PropertyEdit);

            var response = new Model.OrderWrapper<Model.TitleOrder>(new Model.TitleOrder());

            var service = helper.GetService<Mock<ILtsaService>>();
            service.Setup(m => m.PostTitleOrder(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await controller.PostTitleOrderAsync("titleNumber", Model.LandTitleDistrictCode.KA.ToString());

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.OrderWrapper<Model.TitleOrder>>(actionResult.Value);
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostParcelInfoOrderAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LtsaController>(Permissions.PropertyEdit);

            var response = new Model.OrderWrapper<Model.ParcelInfoOrder>(new Model.ParcelInfoOrder());

            var service = helper.GetService<Mock<ILtsaService>>();
            service.Setup(m => m.PostParcelInfoOrder(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await controller.PostParcelInfoOrderAsync("titleNumber");

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.OrderWrapper<Model.ParcelInfoOrder>>(actionResult.Value);
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostSpcpOrderAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LtsaController>(Permissions.PropertyEdit);

            var response = new Model.OrderWrapper<Model.SpcpOrder>(new Model.SpcpOrder());

            var service = helper.GetService<Mock<ILtsaService>>();
            service.Setup(m => m.PostSpcpOrder(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await controller.PostSpcpOrderAsync("strataPlanNumber");

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.OrderWrapper<Model.SpcpOrder>>(actionResult.Value);
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostLtsaFieldsAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LtsaController>(Permissions.PropertyEdit);

            var response = new Model.LtsaOrders();

            var service = helper.GetService<Mock<ILtsaService>>();
            service.Setup(m => m.PostLtsaFields(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await controller.PostLtsaFields("pid");

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.LtsaOrders>(actionResult.Value);
        }
        #endregion
        #endregion
    }
}
