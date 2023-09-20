using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Tools.Controllers;
using Pims.Core.Test;
using Pims.Dal.Security;
using Pims.Ltsa;
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
        private Mock<ILtsaService> _service;
        private LtsaController _controller;
        private TestHelper _helper;
        #endregion

        public LtsaControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<LtsaController>(Permissions.PropertyView);
            this._service = this._helper.GetService<Mock<ILtsaService>>();
        }

        #region Tests
        #region FindTitleSummariesAsync
        [Fact]
        public async void FindTitleSummariesAsync_Success()
        {
            // Arrange

            var response = new Model.TitleSummariesResponse()
            {
                TitleSummaries = new List<Model.TitleSummary>(),
            };

            this._service.Setup(m => m.GetTitleSummariesAsync(It.IsAny<int>())).ReturnsAsync(response);

            // Act
            var result = await this._controller.FindTitleSummariesAsync("123-456-789");

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<ICollection<Model.TitleSummary>>(actionResult.Value);
            this._service.Verify(m => m.GetTitleSummariesAsync(It.IsAny<int>()), Times.Once());
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostTitleOrderAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LtsaController>(Permissions.PropertyEdit);

            var response = new Model.OrderWrapper<Model.OrderParent<Model.Title>>(new Model.TitleOrder());

            var service = helper.GetService<Mock<ILtsaService>>();
            service.Setup(m => m.PostTitleOrder(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await controller.PostTitleOrderAsync("titleNumber", Model.LandTitleDistrictCode.KA.ToString());

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.TitleOrder>(actionResult.Value);
            service.Verify(m => m.PostTitleOrder(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostParcelInfoOrderAsync_Success()
        {
            // Arrange
            var response = new Model.OrderWrapper<Model.OrderParent<Model.ParcelInfo>>(new Model.ParcelInfoOrder());

            this._service.Setup(m => m.PostParcelInfoOrder(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await this._controller.PostParcelInfoOrderAsync("titleNumber");

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.ParcelInfoOrder>(actionResult.Value);
            this._service.Verify(m => m.PostParcelInfoOrder(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async void PostParcelInfoOrderAsync_Success_ConvertPid()
        {
            // Arrange
            var response = new Model.OrderWrapper<Model.OrderParent<Model.ParcelInfo>>(new Model.ParcelInfoOrder());

            this._service.Setup(m => m.PostParcelInfoOrder(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await this._controller.PostParcelInfoOrderAsync("123456789");

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var results = Assert.IsAssignableFrom<Model.ParcelInfoOrder>(actionResult.Value);
            this._service.Verify(m => m.PostParcelInfoOrder(It.Is<string>(s => s.Equals("123-456-789"))), Times.Once());
        }

        [Fact]
        public async void PostParcelInfoOrderAsync_MissingPidFailure()
        {
            // Arrange
            var response = new Model.OrderWrapper<Model.OrderParent<Model.ParcelInfo>>(new Model.ParcelInfoOrder());

            this._service.Setup(m => m.PostParcelInfoOrder(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            await Assert.ThrowsAsync<BadHttpRequestException>(() => this._controller.PostParcelInfoOrderAsync(string.Empty));
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostSpcpOrderAsync_Success()
        {
            // Arrange
            var response = new Model.OrderWrapper<Model.OrderParent<Model.StrataPlanCommonProperty>>(new Model.SpcpOrder());

            this._service.Setup(m => m.PostSpcpOrder(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await this._controller.PostSpcpOrderAsync("strataPlanNumber");

            // Assert
            this._service.Verify(m => m.PostSpcpOrder(It.IsAny<string>()), Times.Once());
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact]
        public async void PostLtsaFieldsAsync_Success()
        {
            // Arrange
            var response = new Model.LtsaOrders();

            this._service.Setup(m => m.PostLtsaFields(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await this._controller.PostLtsaFields("pid");

            // Assert
            this._service.Verify(m => m.PostLtsaFields(It.IsAny<string>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
