using System;
using System.Collections.Generic;
using FluentAssertions;
using Pims.Api.Areas.Reports.Models.Management;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test
{
    public class ManagementActivityInvoicesReportModelTest
    {
        [Fact]
        public void Constructor_Throws_OnNull()
        {
            Action act = () => new ManagementActivityInvoicesReportModel(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_BasicProperties_MapCorrectly()
        {
            var invoice = new PimsManagementActivityInvoice
            {
                InvoiceNum = "INV-001",
                PretaxAmt = 100,
                GstAmt = 5,
                PstAmt = 7,
                TotalAmt = 112,
                ManagementActivity = new PimsManagementActivity
                {
                    CompletionDt = new DateOnly(2022, 12, 31),
                    MgmtActivityTypeCodeNavigation = new PimsMgmtActivityType { Description = "TypeA" },
                    PimsMgmtActivityActivitySubtyps = new List<PimsMgmtActivityActivitySubtyp>
                    {
                        new PimsMgmtActivityActivitySubtyp { MgmtActivitySubtypeCodeNavigation = new PimsMgmtActivitySubtype { Description = "Sub1" } },
                    },
                    ManagementFile = new PimsManagementFile
                    {
                        FileName = "FileA",
                        LegacyFileNum = "L001",
                        AcquisitionFundingTypeCodeNavigation = new PimsAcquisitionFundingType { Description = "FundDesc" },
                        ManagementFilePurposeTypeCodeNavigation = new PimsManagementFilePurposeType { Description = "PurposeDesc" },
                        AppCreateUserid = "creator",
                        ManagementFileStatusTypeCodeNavigation = new PimsManagementFileStatusType { Description = "Active" },
                        PimsManagementFileContacts = new List<PimsManagementFileContact>
                        {
                            new PimsManagementFileContact { Person = new PimsPerson { FirstName = "John", Surname = "Doe" } },
                            new PimsManagementFileContact { Organization = new PimsOrganization { Name = "OrgA" } },
                        }
                    },
                    ServiceProviderOrg = new PimsOrganization { Name = "OrgB" },
                }
            };
            invoice.ManagementActivity.PimsManagementActivityInvoices = new List<PimsManagementActivityInvoice> { invoice };

            var model = new ManagementActivityInvoicesReportModel(invoice);

            model.InvoiceNumber.Should().Be("INV-001");
            model.ManagementFileName.Should().Be("FileA");
            model.LegacyFileNum.Should().Be("L001");
            model.Funding.Should().Be("FundDesc");
            model.Purpose.Should().Be("PurposeDesc");
            model.CreatedBy.Should().Be("creator");
            model.PropertyContacts.Should().Be("John Doe|OrgA");
            model.ManagementFileStatus.Should().Be("Active");
            model.ActivityType.Should().Be("TypeA");
            model.ActivitySubTypes.Should().Be("Sub1");
            model.CompletionDate.Should().Be("2022-12-31");
            model.ServicesProvider.Should().Be("OrgB");
            model.InvoicePreTaxTotal.Should().Be(100);
            model.InvoiceGstAmount.Should().Be(5);
            model.InvoicePstAmount.Should().Be(7);
            model.InvoiceTotal.Should().Be(112);
            model.ActivityPreTaxTotal.Should().Be(100);
            model.ActivityGstAmount.Should().Be(5);
            model.ActivityPstAmount.Should().Be(7);
            model.ActivityTotal.Should().Be(112);
        }

        [Fact]
        public void Constructor_Empty_WhenNulls()
        {
            var invoice = new PimsManagementActivityInvoice();

            var model = new ManagementActivityInvoicesReportModel(invoice);

            model.InvoiceNumber.Should().BeEmpty();
            model.ManagementFileName.Should().BeEmpty();
            model.LegacyFileNum.Should().BeEmpty();
            model.Funding.Should().BeEmpty();
            model.Purpose.Should().BeEmpty();
            model.CreatedBy.Should().BeEmpty();
            model.PropertyContacts.Should().BeEmpty();
            model.ManagementFileStatus.Should().BeEmpty();
            model.ActivityType.Should().BeEmpty();
            model.ActivitySubTypes.Should().BeEmpty();
            model.ServicesProvider.Should().BeEmpty();
            model.Properties.Should().BeEmpty();
            model.InvoicePreTaxTotal.Should().Be(0);
            model.InvoiceGstAmount.Should().Be(0);
            model.InvoicePstAmount.Should().Be(0);
            model.InvoiceTotal.Should().Be(0);
            model.ActivityPreTaxTotal.Should().Be(0);
            model.ActivityGstAmount.Should().Be(0);
            model.ActivityPstAmount.Should().Be(0);
            model.ActivityTotal.Should().Be(0);
        }

        [Fact]
        public void Constructor_Properties_JoinsCorrectly()
        {
            var invoice = new PimsManagementActivityInvoice
            {
                ManagementActivity = new PimsManagementActivity
                {
                    PimsManagementActivityProperties = new List<PimsManagementActivityProperty>
                    {
                        new PimsManagementActivityProperty { Property = new PimsProperty { Pid = 111111111, Address = new PimsAddress { StreetAddress1 = "Addr1" } } },
                        new PimsManagementActivityProperty { Property = new PimsProperty { Pid = 222222222, Address = new PimsAddress { StreetAddress1 = "Addr2" } } },
                    }
                }
            };

            var model = new ManagementActivityInvoicesReportModel(invoice);

            model.Properties.Should().Be("111-111-111|222-222-222");
        }

        [Fact]
        public void Constructor_ServicesProvider_PrefersPerson()
        {
            var invoice = new PimsManagementActivityInvoice
            {
                ManagementActivity = new PimsManagementActivity
                {
                    ServiceProviderPerson = new PimsPerson { FirstName = "Jane", Surname = "Smith" },
                    ServiceProviderOrg = new PimsOrganization { Name = "OrgX" }
                }
            };

            var model = new ManagementActivityInvoicesReportModel(invoice);

            model.ServicesProvider.Should().Be("Jane Smith");
        }

        [Fact]
        public void Constructor_ActivityTotals_SumsInvoices()
        {
            var inv1 = new PimsManagementActivityInvoice { PretaxAmt = 50, GstAmt = 2, PstAmt = 3, TotalAmt = 55 };
            var inv2 = new PimsManagementActivityInvoice { PretaxAmt = 60, GstAmt = 3, PstAmt = 4, TotalAmt = 67 };
            var act = new PimsManagementActivity { PimsManagementActivityInvoices = new List<PimsManagementActivityInvoice> { inv1, inv2 } };
            inv1.ManagementActivity = act;
            inv2.ManagementActivity = act;

            var model = new ManagementActivityInvoicesReportModel(inv1);

            model.ActivityPreTaxTotal.Should().Be(110);
            model.ActivityGstAmount.Should().Be(5);
            model.ActivityPstAmount.Should().Be(7);
            model.ActivityTotal.Should().Be(122);
        }
    }
}
