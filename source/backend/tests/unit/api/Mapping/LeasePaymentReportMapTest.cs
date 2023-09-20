using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Helpers;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Mappings
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class LeasePaymentReportMapTest
    {
        #region Variables
        private IMapper _mapper;
        #endregion

        public LeasePaymentReportMapTest()
        {
            var helper = new TestHelper();
            this._mapper = helper.GetService<IMapper>();
        }

        #region Tests
        [Fact]
        public void MapLeasePaymentReport_Fallback_Pid()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: true);
            lease.PimsPropertyLeases.FirstOrDefault().Property.Pid = 111111111;
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("111-111-111");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Pid_Multiple()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: true);
            lease.PimsPropertyLeases.FirstOrDefault().Property.Pid = 111111111;
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Pid = 222222222 } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("111-111-111,222-222-222");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Pin()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: true);
            lease.PimsPropertyLeases.FirstOrDefault().Property.Pin = 1;
            lease.PimsPropertyLeases.FirstOrDefault().Property.Pid = null;
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("1");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Pin_Multiple()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: true);
            lease.PimsPropertyLeases.FirstOrDefault().Property.Pin = 1;
            lease.PimsPropertyLeases.FirstOrDefault().Property.Pid = null;
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Pin = 2 } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("1,2");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Address()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Address = new PimsAddress() { StreetAddress1 = "street address 1", MunicipalityName = "Victoria" } } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(street address 1 Victoria)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Address_Multiple()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Address = new PimsAddress() { StreetAddress1 = "street address 1", MunicipalityName = "Victoria" } } });
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Address = new PimsAddress() { StreetAddress1 = "street address 2", MunicipalityName = "Victoria" } } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(street address 1 Victoria),(street address 2 Victoria)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Address_Empty()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "descriptive name", Property = new PimsProperty() { Address = new PimsAddress() { StreetAddress1 = string.Empty, MunicipalityName = "Victoria" } } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(descriptive name)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Name()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "Property Name" });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(Property Name)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Name_Multiple()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "Property Name" });
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "Property Name 2" });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(Property Name),(Property Name 2)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Name_Empty()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = string.Empty });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("No Property Identifier");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Location()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Location = GeometryHelper.CreatePoint(1, 2, SpatialReference.WGS84) } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(1, 2)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_Location_Multiple()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Location = GeometryHelper.CreatePoint(1, 2, SpatialReference.WGS84) } });
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Location = GeometryHelper.CreatePoint(3, 4, SpatialReference.WGS84) } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(1, 2),(3, 4)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_NoIdentifier()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("No Property Identifier");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_PidPriority()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "test", Property = new PimsProperty() { Pid = 1, Pin = 2, Address = new PimsAddress() { StreetAddress1 = "test" }, Location = GeometryHelper.CreatePoint(1, 2, SpatialReference.WGS84) } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("000-000-001");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_PinPriority()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "test", Property = new PimsProperty() { Pin = 2, Address = new PimsAddress() { StreetAddress1 = "test" }, Location = GeometryHelper.CreatePoint(1, 2, SpatialReference.WGS84) } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("2");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_AddressPriority()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "test", Property = new PimsProperty() { Address = new PimsAddress() { StreetAddress1 = "address" }, Location = GeometryHelper.CreatePoint(1, 2, SpatialReference.WGS84) } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(address)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_NamePriority()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Name = "test", Property = new PimsProperty() { Location = GeometryHelper.CreatePoint(1, 2, SpatialReference.WGS84) } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(test)");
        }

        [Fact]
        public void MapLeasePaymentReport_Fallback_LocationPriority()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new PimsProperty() { Location = GeometryHelper.CreatePoint(1, 2, SpatialReference.WGS84) } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.PropertyList.Should().Be("(1, 2)");
        }

        [Fact]
        public void MapLeasePaymentReport_TenantList()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant() { TenantTypeCode = "TEN", Person = new PimsPerson() { FirstName = "first" } });
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant() { TenantTypeCode = "TEN", Organization = new PimsOrganization() { OrganizationName = "test org" } });
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant() { TenantTypeCode = "NOT", Organization = new PimsOrganization() { OrganizationName = "test org" } });
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.TenantList.Should().Be("first,test org");
        }

        [Fact]
        public void MapLeasePaymentReport_ProgramType()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.LeaseProgramTypeCode = "OTHER";
            lease.LeaseProgramTypeCodeNavigation = new PimsLeaseProgramType() { Description = "OTHER" };
            lease.OtherLeaseProgramType = "DESC";
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.Program.Should().Be("OTHER - DESC");
        }

        [Fact]
        public void MapLeasePaymentReport_PurposeType()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.LeasePurposeTypeCode = "OTHER";
            lease.LeasePurposeTypeCodeNavigation = new PimsLeasePurposeType() { Description = "OTHER" };
            lease.OtherLeasePurposeType = "DESC";
            var leasePayment = new PimsLeasePayment() { };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.Purpose.Should().Be("OTHER - DESC");
        }

        [Fact]
        public void MapLeasePaymentReport_LastPaymentDate()
        {
            var lease = EntityHelper.CreateLease(1, addProperty: false);

            var leasePayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };
            var leasePaymentOld = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now.AddDays(-1) };
            var leasePaymentOlder = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now.AddDays(-2) };

            leasePayment.LeaseTerm = new PimsLeaseTerm() { Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { leasePaymentOld, leasePayment, leasePaymentOlder } };

            // Arrange
            var mapped = this._mapper.Map<LeasePaymentReportModel>(leasePayment);

            mapped.LatestPaymentDate.Should().Be(DateTime.Now.ToString("MMMM dd, yyyy"));
        }
        #endregion
    }
}
