using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Property.Models.Search;
using Pims.Api.Areas.Reports.Controllers;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Controllers.Reports
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "report")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyControllerTest
    {
        #region Variables
        public static IEnumerable<object[]> PropertyFilters = new List<object[]>()
        {
            new object [] { new PropertyFilterModel() },
            new object [] { new PropertyFilterModel() { Address = "Address" } },
            new object [] { new PropertyFilterModel() { PinOrPid = "999999" } },
        };

        public static IEnumerable<object[]> PropertyQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/properties?Organizations=1,2") },
            new object [] { new Uri("http://host/api/properties?StatusId=2") },
            new object [] { new Uri("http://host/api/properties?ClassificationId=1") },
            new object [] { new Uri("http://host/api/properties?Address=Address") },
            new object [] { new Uri("http://host/api/properties?ProjectNumber=ProjectNumber") },
            new object [] { new Uri("http://host/api/properties?MinLotArea=1") },
            new object [] { new Uri("http://host/api/properties?MaxLotArea=1") },
            new object [] { new Uri("http://host/api/properties?MinLandArea=1") },
            new object [] { new Uri("http://host/api/properties?MaxLandArea=1") },
            new object [] { new Uri("http://host/api/properties?ConstructionTypeId=1") },
            new object [] { new Uri("http://host/api/properties?PredominateUseId=1") },
            new object [] { new Uri("http://host/api/properties?FloorCount=1") },
            new object [] { new Uri("http://host/api/properties?Tenancy=Tenancy") },
            new object [] { new Uri("http://host/api/properties?MinRentableArea=1") },
            new object [] { new Uri("http://host/api/properties?MaxRentableArea=1") },
        };
        #endregion

        #region Tests
        #region ExportProperties
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyFilters))]
        public void ExportProperties_Csv_Success(PropertyFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties, filter.Page, filter.Quantity);
            repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);

            // Act
            var result = controller.ExportProperties(filter);

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENT_TYPE_CSV, actionResult.ContentType);
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportProperties_Csv_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView, uri);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties);
            repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);

            // Act
            var result = controller.ExportProperties();

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENT_TYPE_CSV, actionResult.ContentType);
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyFilters))]
        public void ExportProperties_Excel_Success(PropertyFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCEL);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties, filter.Page, filter.Quantity);
            repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);

            // Act
            var result = controller.ExportProperties(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportProperties_Excel_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView, uri);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCEL);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties);
            repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);

            // Act
            var result = controller.ExportProperties();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyFilters))]
        public void ExportProperties_ExcelX_Success(PropertyFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties, filter.Page, filter.Quantity);
            repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);

            // Act
            var result = controller.ExportProperties(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportProperties_ExcelX_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView, uri);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties);
            repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);

            // Act
            var result = controller.ExportProperties();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void ExportProperties_Query_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IPropertyRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportProperties());
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a filter object.
        /// </summary>
        [Fact]
        public void ExportProperties_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IPropertyRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportProperties(null));
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a valid accept header.
        /// </summary>
        [Fact]
        public void ExportProperties_NoAcceptHeader_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var filter = new PropertyFilterModel() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportProperties(filter));
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a valid accept header.
        /// </summary>
        [Fact]
        public void ExportProperties_InvalidAcceptHeader_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns("invalid");
            var filter = new PropertyFilterModel() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportProperties(filter));
            repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
