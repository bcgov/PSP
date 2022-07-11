using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Helpers.Extensions;
using Pims.Core.Test;
using Xunit;
using Model = Pims.Api.Areas.Property.Models.Search;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class HttpResponseMessageExtensionsTest
    {
        #region Data
        public static IEnumerable<object[]> SuccessCodes =
            new List<object[]>
            {
                new object[] { HttpStatusCode.OK },
                new object[] { HttpStatusCode.Created },
            };

        public static IEnumerable<object[]> FailureCodes =
            new List<object[]>
            {
                new object[] { HttpStatusCode.BadRequest, new Exception("BadRequest") },
                new object[] { HttpStatusCode.InternalServerError, new Exception("InternalServerError") },
                new object[] { HttpStatusCode.Unauthorized, new Exception("Unauthorized") },
                new object[] { HttpStatusCode.NotFound, new Exception("NotFound") },
                new object[] { HttpStatusCode.Forbidden, new Exception("Forbidden") },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(SuccessCodes))]
        public async Task HandleResponseAsync_Success(HttpStatusCode code)
        {
            // Arrange
            var helper = new TestHelper();
            var mapper = helper.GetMapper();
            var model = mapper.Map<Model.PropertyModel>(EntityHelper.CreateProperty(1));
            var json = System.Text.Json.JsonSerializer.Serialize(model);
            var response = new HttpResponseMessage
            {
                StatusCode = code,
                Content = new StringContent(json),
            };

            // Act
            var actionResult = await response.HandleResponseAsync<Model.PropertyModel>();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Equal((int)code, jsonResult.StatusCode);
            var actualResult = Assert.IsType<Model.PropertyModel>(jsonResult.Value);
            model.Should().BeEquivalentTo(actualResult);
        }

        [Theory]
        [MemberData(nameof(FailureCodes))]
        public async Task HandleResponseAsync_Failure(HttpStatusCode code, Exception exception)
        {
            // Arrange
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(m => m.EnvironmentName).Returns("Development");
            var model = new Models.ErrorResponseModel(env.Object, exception);
            var json = System.Text.Json.JsonSerializer.Serialize(model);
            var response = new HttpResponseMessage
            {
                StatusCode = code,
                Content = new StringContent(json),
            };

            // Act
            var actionResult = await response.HandleResponseAsync<Model.PropertyModel>();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Equal((int)code, jsonResult.StatusCode);
            var actualResult = Assert.IsType<Models.ErrorResponseModel>(jsonResult.Value);
            Assert.Equal(json, actualResult.Error);
        }

        [Theory]
        [MemberData(nameof(FailureCodes))]
        public async Task HandleResponseAsync_JsonFailure(HttpStatusCode code, Exception exception)
        {
            // Arrange
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(m => m.EnvironmentName).Returns("Development");
            var model = new Models.ErrorResponseModel(env.Object, exception);
            var json = System.Text.Json.JsonSerializer.Serialize(model);
            var response = new HttpResponseMessage
            {
                StatusCode = code,
                Content = new StringContent(json),
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Act
            var actionResult = await response.HandleResponseAsync<Model.PropertyModel>();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Equal((int)code, jsonResult.StatusCode);
            var actualResult = Assert.IsType<System.Text.Json.JsonElement>(jsonResult.Value);
            Assert.Equal($"{model.Error}", $"{actualResult.GetProperty(nameof(Models.ErrorResponseModel.Error))}");
            Assert.Equal($"{model.Details}", $"{actualResult.GetProperty(nameof(Models.ErrorResponseModel.Details))}");
            Assert.Equal($"{model.Type}", $"{actualResult.GetProperty(nameof(Models.ErrorResponseModel.Type))}");
            Assert.Equal($"{model.StackTrace}", $"{actualResult.GetProperty(nameof(Models.ErrorResponseModel.StackTrace))}");
        }
        #endregion
    }
}
