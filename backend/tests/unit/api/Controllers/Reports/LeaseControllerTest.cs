using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Models.Search;
using Pims.Api.Areas.Reports.Controllers;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Entity = Pims.Dal.Entities;
using FluentAssertions;
using System.Linq;

namespace Pims.Api.Test.Controllers.Reports
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "report")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class LeaseControllerTest
    {
        #region Variables
        public static IEnumerable<object[]> LeaseFilters = new List<object[]>()
        {
            new object [] { new LeaseFilterModel() },
            new object [] { new LeaseFilterModel() { LFileNo = "L-000-001" } },
            new object [] { new LeaseFilterModel() { PinOrPid = "999999" } },
            new object [] { new LeaseFilterModel() { TenantName = "George" } },
        };

        public static IEnumerable<object[]> LeaseQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/leases?LFileNo=L-000-001") },
            new object [] { new Uri("http://host/api/leases?PinOrPid=999999") },
            new object [] { new Uri("http://host/api/leases?TenantName=George") },
        };
        #endregion

        #region Tests
        #region ExportLeases
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseFilters))]
        public void ExportLeases_Csv_Success(LeaseFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases, filter.Page, filter.Quantity);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.ExportLeases(filter);

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENT_TYPE_CSV, actionResult.ContentType);
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void ExportLeases_Csv_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView, uri);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.ExportLeases();

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENT_TYPE_CSV, actionResult.ContentType);
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
        }

        [Fact]
        public void ExportLeases_Lease_Mapping()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var lease = EntityHelper.CreateLease(1);
            lease.RegionCodeNavigation = new PimsRegion() { RegionCode = 1, Description = "region" };
            lease.LFileNo = "L-010-070";
            lease.OrigStartDate = new DateTime(2000, 1, 1);
            lease.LeaseProgramTypeCodeNavigation = new PimsLeaseProgramType() { LeaseProgramTypeCode = "OTHER", Description = "otherprogramdesc" };
            lease.OtherLeaseProgramType = "program";
            lease.LeaseLicenseTypeCodeNavigation = new PimsLeaseLicenseType() { LeaseLicenseTypeCode = "OTHER", Description = "othertypedesc" };
            lease.OtherLeaseLicenseType = "type";
            lease.LeasePurposeTypeCodeNavigation = new PimsLeasePurposeType() { LeasePurposeTypeCode = "OTHER", Description = "otherpurposedesc" };
            lease.OtherLeasePurposeType = "purpose";
            lease.LeaseStatusTypeCodeNavigation = new PimsLeaseStatusType() { LeaseStatusTypeCode = "STATUS", Description = "status" };
            lease.PsFileNo = "123";
            lease.LeaseNotes = "note";
            lease.InspectionDate = new DateTime(2000, 2, 2);
            lease.InspectionNotes = "inspection note";
            var leases = new[] { lease };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.GetCrossJoinLeases(new LeaseFilterModel()).FirstOrDefault();

            // Assert
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
            result.MotiRegion.Should().Be("region");
            result.LFileNo.Should().Be("L-010-070");
            result.StartDate.Should().Be(new DateTime(2000, 1, 1));
            result.ProgramName.Should().Be("otherprogramdesc - program");
            result.StatusType.Should().Be("status");
            result.PurposeType.Should().Be("otherpurposedesc - purpose");
            result.LeaseTypeName.Should().Be("othertypedesc - type");
            result.PsFileNo.Should().Be("123");
            result.LeaseNotes.Should().Be("note");
            result.InspectionDate.Should().Be(new DateTime(2000, 2, 2));
            result.InspectionNotes.Should().Be("inspection note");
        }

        [Fact]
        public void ExportLeases_LeaseTerm_Mapping()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new PimsLeaseTerm();
            leaseTerm.TermStartDate = DateTime.Now;
            leaseTerm.TermRenewalDate = DateTime.Now.AddDays(1);
            leaseTerm.TermExpiryDate = DateTime.Now.AddDays(2);
            leaseTerm.LeasePmtFreqTypeCodeNavigation = new PimsLeasePmtFreqType() {LeasePmtFreqTypeCode = "PMT", Description="pmt" };
            leaseTerm.PaymentAmount = 1000;
            lease.PimsLeaseTerms.Add(leaseTerm);

            var leases = new[] { lease };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.GetCrossJoinLeases(new LeaseFilterModel()).FirstOrDefault();

            // Assert
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
            result.CurrentTermStartDate.Should().Be(leaseTerm.TermStartDate);
            result.CurrentTermEndDate.Should().Be(leaseTerm.TermExpiryDate);
            result.TermStartDate.Should().Be(leaseTerm.TermStartDate);
            result.TermRenewalDate.Should().Be(leaseTerm.TermRenewalDate);
            result.TermExpiryDate.Should().Be(leaseTerm.TermExpiryDate);
            result.IsExpired.Should().Be("No");
            result.LeasePaymentFrequencyType.Should().Be("pmt");
            result.LeaseAmount.Should().Be(1000);
        }

        [Fact]
        public void ExportLeases_LeaseTenant_Mapping()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var lease = EntityHelper.CreateLease(1);
            var leaseOrgTenant = new PimsLeaseTenant();
            leaseOrgTenant.Organization = new PimsOrganization() { Name = "org" };
            lease.PimsLeaseTenants.Add(leaseOrgTenant);

            var leasePersonTenant = new PimsLeaseTenant();
            leasePersonTenant.Person = new PimsPerson() { FirstName = "first", MiddleNames = "middle", Surname = "last" };
            lease.PimsLeaseTenants.Add(leasePersonTenant);


            var leases = new[] { lease };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.GetCrossJoinLeases(new LeaseFilterModel());

            // Assert
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
            result.ToArray()[0].TenantName.Should().Be("org");
            result.ToArray()[1].TenantName.Should().Be("first middle last");
        }

        [Fact]
        public void ExportLeases_LeaseProperty_Mapping()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var lease = new PimsLease();
            var property = new PimsProperty();
            property.Pid = 1;
            property.Pin = 2;
            property.Address = new PimsAddress() { StreetAddress1 = "1", StreetAddress2 = "2", StreetAddress3 = "3", MunicipalityName = "m" };
            var leaseProperty = new PimsPropertyLease() { Property = property, Lease = lease, LeaseArea = 3, AreaUnitTypeCodeNavigation = new PimsAreaUnitType() { AreaUnitTypeCode = "HA", Description = "hectares" } };
            lease.PimsPropertyLeases.Add(leaseProperty);

            var leases = new[] { lease };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.GetCrossJoinLeases(new LeaseFilterModel()).FirstOrDefault();

            // Assert
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
            result.Pid.Should().Be(1);
            result.Pin.Should().Be(2);
            result.CivicAddress.Should().Be("1 2 3 m");
            result.LeaseArea.Should().Be(3);
            result.AreaUnit.Should().Be("hectares");
        }

        [Fact]
        public void ExportLeases_Cartesion()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var lease = new PimsLease();
            lease.PimsLeaseTerms.Add(new PimsLeaseTerm());
            lease.PimsLeaseTerms.Add(new PimsLeaseTerm());
            lease.PimsLeaseTerms.Add(new PimsLeaseTerm());

            lease.PimsLeaseTenants.Add(new PimsLeaseTenant());
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant());
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant());

            var leases = new[] { lease };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.GetCrossJoinLeases(new LeaseFilterModel());

            // Assert
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
            result.Count().Should().Be(9);
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseFilters))]
        public void ExportLeases_Excel_Success(LeaseFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCEL);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases, filter.Page, filter.Quantity);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.ExportLeases(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void ExportLeases_Excel_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView, uri);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCEL);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.ExportLeases();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseFilters))]
        public void ExportLeases_ExcelX_Success(LeaseFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases, filter.Page, filter.Quantity);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.ExportLeases(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void ExportLeases_ExcelX_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView, uri);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>())).Returns(page);

            // Act
            var result = controller.ExportLeases();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void ExportLeases_Query_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);

            var service = helper.GetService<Mock<IPimsRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases());
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a filter object.
        /// </summary>
        [Fact]
        public void ExportLeases_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);

            var service = helper.GetService<Mock<IPimsRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases(null));
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a valid accept header.
        /// </summary>
        [Fact]
        public void ExportLeases_NoAcceptHeader_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);

            var service = helper.GetService<Mock<IPimsRepository>>();
            var filter = new LeaseFilterModel() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases(filter));
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a valid accept header.
        /// </summary>
        [Fact]
        public void ExportLeases_InvalidAcceptHeader_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);

            var service = helper.GetService<Mock<IPimsRepository>>();
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns("invalid");
            var filter = new LeaseFilterModel() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases(filter));
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
        }
        #endregion
        #region ExportAggregatedLeases

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Fact]
        public void ExportAggregatedLeases_ExcelX_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var lease = EntityHelper.CreateLease(1, region: new PimsRegion() { Id = 1 });
            var leases = new[] { lease };

            var service = helper.GetService<Mock<IPimsService>>();
            var lookup = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var webHost = helper.GetService<Mock<IWebHostEnvironment>>();
            var path = Path.Combine(SolutionProvider.TryGetSolutionDirectoryInfo().FullName, "api");
            webHost.SetupGet(m => m.ContentRootPath).Returns(path);

            lookup.Setup(m => m.Lookup.GetRegions()).Returns(new List<PimsRegion>() { lease.RegionCodeNavigation });
            lookup.Setup(m => m.Lookup.GetLeaseProgramTypes()).Returns(new List<PimsLeaseProgramType>() { lease.LeaseProgramTypeCodeNavigation });
            service.Setup(m => m.LeaseReportsService.GetAggregatedLeaseReport(It.IsAny<int>())).Returns(leases);

            // Act
            var result = controller.ExportAggregatedLeases(2021);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            service.Setup(m => m.LeaseReportsService.GetAggregatedLeaseReport(It.IsAny<int>())).Returns(leases);
        }

        /// <summary>
        /// Make an invalid request with a fiscal year that is too old.
        /// </summary>
        [Fact]
        public void ExportAggregatedLeases_ExcelX_InvalidFiscal()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeaseController>(Permissions.LeaseView);
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();
            var webHost = helper.GetService<Mock<IWebHostEnvironment>>();
            var path = Path.Combine(SolutionProvider.TryGetSolutionDirectoryInfo().FullName, "api");
            webHost.SetupGet(m => m.ContentRootPath).Returns(path);

            service.Setup(m => m.LeaseReportsService.GetAggregatedLeaseReport(It.IsAny<int>())).Returns(leases);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportAggregatedLeases(1800));
        }

        #endregion
        #endregion
    }
}
