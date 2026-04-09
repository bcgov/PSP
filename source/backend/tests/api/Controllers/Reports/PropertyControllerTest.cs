using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Property.Models.Search;
using Pims.Api.Areas.Reports.Controllers;
using Pims.Api.Helpers.Constants;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
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
            new object [] { new PropertyFilterModel() { Pid = "999999" } },
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

        private readonly Mock<IPropertyRepository> _repository;
        private readonly Mock<IPropertyService> _service;
        private readonly PropertyController _controller;
        private readonly IMapper _mapper;
        private readonly TestHelper _helper;
        private readonly Mock<ILookupRepository> _lookupRepository;
        private readonly Mock<IWebHostEnvironment> _webHost;
        private readonly Mock<Microsoft.AspNetCore.Http.IHeaderDictionary> _headers;
        #endregion

        public PropertyControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<PropertyController>(Permissions.PropertyView);
            this._mapper = this._helper.GetService<IMapper>();
            this._lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            this._webHost = this._helper.GetService<Mock<IWebHostEnvironment>>();
            this._headers = this._helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            this._repository = this._helper.GetService<Mock<IPropertyRepository>>();
            this._service = this._helper.GetService<Mock<IPropertyService>>();
        }

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
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var page = new Paged<Entity.PimsPropertyVw>(properties, filter.Page, filter.Quantity);
            this._repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);
            this._service.Setup(s => s.GetTenureCleanupsForPropertyId(It.IsAny<long>())).Returns(new List<Entity.PimsPropTenureCleanup>());

            // Act
            var result = this._controller.ExportProperties(filter);

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENTTYPECSV, actionResult.ContentType);
            this._repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportProperties_Csv_Query_Success(Uri uri)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var page = new Paged<Entity.PimsPropertyVw>(properties);
            this._repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);
            this._service.Setup(s => s.GetTenureCleanupsForPropertyId(It.IsAny<long>())).Returns(new List<Entity.PimsPropTenureCleanup>());

            // Act
            var result = this._controller.ExportProperties();

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENTTYPECSV, actionResult.ContentType);
            this._repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyFilters))]
        public void ExportProperties_Excel_Success(PropertyFilterModel filter)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCEL);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var page = new Paged<Entity.PimsPropertyVw>(properties, filter.Page, filter.Quantity);
            this._repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);
            this._service.Setup(s => s.GetTenureCleanupsForPropertyId(It.IsAny<long>())).Returns(new List<Entity.PimsPropTenureCleanup>());

            // Act
            var result = this._controller.ExportProperties(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportProperties_Excel_Query_Success(Uri uri)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCEL);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var page = new Paged<Entity.PimsPropertyVw>(properties);
            this._repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);
            this._service.Setup(s => s.GetTenureCleanupsForPropertyId(It.IsAny<long>())).Returns(new List<Entity.PimsPropTenureCleanup>());

            // Act
            var result = this._controller.ExportProperties();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyFilters))]
        public void ExportProperties_ExcelX_Success(PropertyFilterModel filter)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCELX);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var page = new Paged<Entity.PimsPropertyVw>(properties, filter.Page, filter.Quantity);
            this._repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);
            this._service.Setup(s => s.GetTenureCleanupsForPropertyId(It.IsAny<long>())).Returns(new List<Entity.PimsPropTenureCleanup>());

            // Act
            var result = this._controller.ExportProperties(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportProperties_ExcelX_Query_Success(Uri uri)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCELX);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var page = new Paged<Entity.PimsPropertyVw>(properties);
            this._repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);
            this._service.Setup(s => s.GetTenureCleanupsForPropertyId(It.IsAny<long>())).Returns(new List<Entity.PimsPropTenureCleanup>());

            // Act
            var result = this._controller.ExportProperties();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());
        }

        [Fact]
        public void ExportProperties_Excel_TenureCleanup_Success()
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCEL);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var page = new Paged<Entity.PimsPropertyVw>(properties, 1, 10);
            this._repository.Setup(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>())).Returns(page);
            this._service.Setup(s => s.GetTenureCleanupsForPropertyId(It.IsAny<long>())).Returns(
                new List<Entity.PimsPropTenureCleanup>()
                {
                    new Entity.PimsPropTenureCleanup() {
                        PropTenureCleanupId = 1,
                        PropertyId = 1,
                        TenureCleanupTypeCode = "FORM12",
                        TenureCleanupTypeCodeNavigation = new Entity.PimsTenureCleanupType() { TenureCleanupTypeCode = "FORM12", Description = "Form 12" }
                    },
                    new Entity.PimsPropTenureCleanup() {
                        PropTenureCleanupId = 2,
                        PropertyId = 1,
                        TenureCleanupTypeCode = "SECTION42",
                        TenureCleanupTypeCodeNavigation = new Entity.PimsTenureCleanupType() { TenureCleanupTypeCode = "SECTION42", Description = "Section 42" }
                    },
                });

            // Act
            var result = this._controller.ExportProperties(new PropertyFilterModel() { Page = 1, Quantity = 10 });

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.PropertyFilter>()), Times.Once());

            // Assert - verify the Excel file contains the tenure cleanup column with the expected pipe-joined values
            actionResult.FileStream.Position = 0;
            using var workbook = new ClosedXML.Excel.XLWorkbook(actionResult.FileStream);
            var worksheet = workbook.Worksheets.First();

            Assert.NotNull(worksheet);
            Assert.True(worksheet.RowCount() > 1); // Assert that there is at least one row of data in addition to the header row

            var headerRow = worksheet.Row(1);
            var tenureCleanupColumn = headerRow.Cells()
                .FirstOrDefault(c => c.GetString().Equals("Tenure Cleanup", StringComparison.OrdinalIgnoreCase))
                ?? throw new Exception("Tenure Cleanup column not found in the exported Excel file.");

            var cellValue = worksheet.Row(2).Cell(tenureCleanupColumn.Address.ColumnNumber).GetString();
            Assert.Equal("Form 12|Section 42", cellValue);
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
