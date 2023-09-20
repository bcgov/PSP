using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Models.Search;
using Pims.Api.Areas.Reports.Controllers;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Entity = Pims.Dal.Entities;

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

        private Mock<ILeaseReportsService> _service;
        private Mock<ILeasePaymentService> _paymentService;
        private Mock<ILeaseService> _leaseService;
        private LeaseController _controller;
        private IMapper _mapper;
        private TestHelper _helper;
        private Mock<ILookupRepository> _lookupRepository;
        private Mock<IWebHostEnvironment> _webHost;
        private Mock<Microsoft.AspNetCore.Http.IHeaderDictionary> _headers;
        #endregion
        public LeaseControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<LeaseController>(Permissions.LeaseView);
            this._mapper = this._helper.GetService<IMapper>();
            this._service = this._helper.GetService<Mock<ILeaseReportsService>>();
            this._paymentService = this._helper.GetService<Mock<ILeasePaymentService>>();
            this._lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            this._webHost = this._helper.GetService<Mock<IWebHostEnvironment>>();
            this._headers = this._helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            this._leaseService = this._helper.GetService<Mock<ILeaseService>>();
        }

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
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var page = new Paged<Entity.PimsLease>(leases, filter.Page, filter.Quantity);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.ExportLeases(filter);

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENTTYPECSV, actionResult.ContentType);
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void ExportLeases_Csv_Query_Success(Uri uri)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var page = new Paged<Entity.PimsLease>(leases);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.ExportLeases();

            // Assert
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
        }

        [Fact]
        public void ExportLeases_Lease_Mapping()
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

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

            var page = new Paged<Entity.PimsLease>(leases);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.GetCrossJoinLeases(new LeaseFilterModel()).FirstOrDefault();

            // Assert
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
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
            var headers = this._helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new PimsLeaseTerm();
            leaseTerm.TermStartDate = DateTime.Now;
            leaseTerm.TermRenewalDate = DateTime.Now.AddDays(1);
            leaseTerm.TermExpiryDate = DateTime.Now.AddDays(2);
            leaseTerm.LeasePmtFreqTypeCodeNavigation = new PimsLeasePmtFreqType() { LeasePmtFreqTypeCode = "PMT", Description = "pmt" };
            leaseTerm.PaymentAmount = 1000;
            lease.PimsLeaseTerms.Add(leaseTerm);

            var leases = new[] { lease };

            var page = new Paged<Entity.PimsLease>(leases);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.GetCrossJoinLeases(new LeaseFilterModel()).FirstOrDefault();

            // Assert
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
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
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var lease = EntityHelper.CreateLease(1);
            var leaseOrgTenant = new PimsLeaseTenant();
            leaseOrgTenant.Organization = new PimsOrganization() { Name = "org" };
            lease.PimsLeaseTenants.Add(leaseOrgTenant);

            var leasePersonTenant = new PimsLeaseTenant();
            leasePersonTenant.Person = new PimsPerson() { FirstName = "first", MiddleNames = "middle", Surname = "last" };
            lease.PimsLeaseTenants.Add(leasePersonTenant);

            var leases = new[] { lease };

            var page = new Paged<Entity.PimsLease>(leases);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.GetCrossJoinLeases(new LeaseFilterModel());

            // Assert
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
            result.ToArray()[0].TenantName.Should().Be("org");
            result.ToArray()[1].TenantName.Should().Be("first middle last");
        }

        [Fact]
        public void ExportLeases_LeaseProperty_Mapping()
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var lease = new PimsLease();
            var property = new PimsProperty();
            property.Pid = 1;
            property.Pin = 2;
            property.Address = new PimsAddress() { StreetAddress1 = "1", StreetAddress2 = "2", StreetAddress3 = "3", MunicipalityName = "m" };
            var leaseProperty = new PimsPropertyLease() { Property = property, Lease = lease, LeaseArea = 3, AreaUnitTypeCodeNavigation = new PimsAreaUnitType() { AreaUnitTypeCode = "HA", Description = "hectares" } };
            lease.PimsPropertyLeases.Add(leaseProperty);

            var leases = new[] { lease };

            var page = new Paged<Entity.PimsLease>(leases);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.GetCrossJoinLeases(new LeaseFilterModel()).FirstOrDefault();

            // Assert
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
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
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPECSV);

            var lease = new PimsLease();
            lease.PimsLeaseTerms.Add(new PimsLeaseTerm());
            lease.PimsLeaseTerms.Add(new PimsLeaseTerm());
            lease.PimsLeaseTerms.Add(new PimsLeaseTerm());

            lease.PimsLeaseTenants.Add(new PimsLeaseTenant());
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant());
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant());

            var leases = new[] { lease };

            var page = new Paged<Entity.PimsLease>(leases);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.GetCrossJoinLeases(new LeaseFilterModel());

            // Assert
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
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
            var headers = this._helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCEL);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var page = new Paged<Entity.PimsLease>(leases, filter.Page, filter.Quantity);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.ExportLeases(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void ExportLeases_Excel_Query_Success(Uri uri)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCEL);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = this._helper.GetService<Mock<ILeaseService>>();
            var mapper = this._helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsLease>(leases);
            service.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.ExportLeases();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseFilters))]
        public void ExportLeases_ExcelX_Success(LeaseFilterModel filter)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCELX);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var page = new Paged<Entity.PimsLease>(leases, filter.Page, filter.Quantity);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.ExportLeases(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void ExportLeases_ExcelX_Query_Success(Uri uri)
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCELX);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var page = new Paged<Entity.PimsLease>(leases);
            this._leaseService.Setup(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false)).Returns(page);

            // Act
            var result = this._controller.ExportLeases();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._leaseService.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Once());
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

            var service = helper.GetService<Mock<ILeaseService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases());
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Never());
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

            var service = helper.GetService<Mock<ILeaseService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases(null));
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Never());
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

            var service = helper.GetService<Mock<ILeaseService>>();
            var filter = new LeaseFilterModel() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases(filter));
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Never());
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

            var service = helper.GetService<Mock<ILeaseService>>();
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns("invalid");
            var filter = new LeaseFilterModel() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportLeases(filter));
            service.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Never());
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
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCELX);

            var lease = EntityHelper.CreateLease(1, region: new PimsRegion() { Id = 1 });
            var leases = new[] { lease };

            var path = Path.Combine(SolutionProvider.TryGetSolutionDirectoryInfo().FullName, "api");
            this._webHost.SetupGet(m => m.ContentRootPath).Returns(path);

            this._lookupRepository.Setup(m => m.GetAllRegions()).Returns(new List<PimsRegion>() { lease.RegionCodeNavigation });
            this._lookupRepository.Setup(m => m.GetAllLeaseProgramTypes()).Returns(new List<PimsLeaseProgramType>() { lease.LeaseProgramTypeCodeNavigation });
            this._service.Setup(m => m.GetAggregatedLeaseReport(It.IsAny<int>())).Returns(leases);

            // Act
            var result = this._controller.ExportAggregatedLeases(2021);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            this._service.Setup(m => m.GetAggregatedLeaseReport(It.IsAny<int>())).Returns(leases);
        }

        /// <summary>
        /// Make an invalid request with a fiscal year that is too old.
        /// </summary>
        [Fact]
        public void ExportAggregatedLeases_ExcelX_InvalidFiscal()
        {
            // Arrange
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCELX);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var path = Path.Combine(SolutionProvider.TryGetSolutionDirectoryInfo().FullName, "api");
            this._webHost.SetupGet(m => m.ContentRootPath).Returns(path);

            this._service.Setup(m => m.GetAggregatedLeaseReport(It.IsAny<int>())).Returns(leases);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => this._controller.ExportAggregatedLeases(1800));
        }

        #endregion

        #region ExportPayments

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Fact]
        public void ExportLeasePayments_ExcelX_Success()
        {
            // Arrange
            var helper = new TestHelper();
            this._headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENTTYPEEXCELX);

            var path = Path.Combine(SolutionProvider.TryGetSolutionDirectoryInfo().FullName, "api");
            this._webHost.SetupGet(m => m.ContentRootPath).Returns(path);

            this._paymentService.Setup(m => m.GetAllByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<PimsLeasePayment>());

            // Act
            var result = this._controller.ExportLeasePayments(2021);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENTTYPEEXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
        }

        #endregion
        #endregion
    }
}
