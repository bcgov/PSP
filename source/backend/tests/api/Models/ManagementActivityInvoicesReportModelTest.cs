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
        public void Constructor_BasicProperties()
        {
            var invoice = new PimsManagementActivityInvoice
            {
                InvoiceNum = "INV-001",
                PretaxAmt = 100,
                GstAmt = 5,
                PstAmt = 7,
                TotalAmt = 112,
                IsPaymentApproved = false,
                AppCreateUserid = "creator-2",
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
                        AppCreateUserid = "creator-1",
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
            model.CreatedBy.Should().Be("creator-2");
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

            // The invoice is not approved, so approved totals should be zero.
            model.IsPaymentApproved.Should().Be("No");
            model.IsPaymentForwarded.Should().Be("No");
            model.ApprovedPreTaxTotal.Should().Be(0);
            model.ApprovedGstAmount.Should().Be(0);
            model.ApprovedPstAmount.Should().Be(0);
            model.ApprovedTotal.Should().Be(0);

            // Total across all invoices for the activity (both approved and unapproved).
            model.AllPreTaxTotal.Should().Be(100);
            model.AllGstAmount.Should().Be(5);
            model.AllPstAmount.Should().Be(7);
            model.AllTotal.Should().Be(112);
        }

        [Fact]
        public void Constructor_BasicProperties_PaymentApproved()
        {
            var invoice = new PimsManagementActivityInvoice
            {
                InvoiceNum = "INV-001",
                PretaxAmt = 100,
                GstAmt = 5,
                PstAmt = 7,
                TotalAmt = 112,
                IsPaymentApproved = true,
                AppCreateUserid = "creator-2",
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
                        AppCreateUserid = "creator-1",
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
            model.CreatedBy.Should().Be("creator-2");
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

            // The invoice is approved, so approved totals should be the same as invoice totals.
            model.IsPaymentApproved.Should().Be("Yes");
            model.IsPaymentForwarded.Should().Be("No");
            model.ApprovedPreTaxTotal.Should().Be(100);
            model.ApprovedGstAmount.Should().Be(5);
            model.ApprovedPstAmount.Should().Be(7);
            model.ApprovedTotal.Should().Be(112);

            // Total across all invoices for the activity (both approved and unapproved).
            model.AllPreTaxTotal.Should().Be(100);
            model.AllGstAmount.Should().Be(5);
            model.AllPstAmount.Should().Be(7);
            model.AllTotal.Should().Be(112);
        }

        [Fact]
        public void Constructor_BasicProperties_Without_Management_File_MapCorrectly()
        {
            var invoice = new PimsManagementActivityInvoice
            {
                InvoiceNum = "INV-001",
                PretaxAmt = 100,
                GstAmt = 5,
                PstAmt = 7,
                TotalAmt = 112,
                IsPaymentApproved = false,
                AppCreateUserid = "creator-2",
                ManagementActivity = new PimsManagementActivity
                {
                    CompletionDt = new DateOnly(2022, 12, 31),
                    MgmtActivityTypeCodeNavigation = new PimsMgmtActivityType { Description = "TypeA" },
                    PimsMgmtActivityActivitySubtyps = new List<PimsMgmtActivityActivitySubtyp>
                    {
                        new PimsMgmtActivityActivitySubtyp { MgmtActivitySubtypeCodeNavigation = new PimsMgmtActivitySubtype { Description = "Sub1" } },
                    },
                    ManagementFile = null,
                    ServiceProviderOrg = new PimsOrganization { Name = "OrgB" },
                }
            };
            invoice.ManagementActivity.PimsManagementActivityInvoices = new List<PimsManagementActivityInvoice> { invoice };

            var model = new ManagementActivityInvoicesReportModel(invoice);

            model.InvoiceNumber.Should().Be("INV-001");
            model.ManagementFileName.Should().Be("");
            model.LegacyFileNum.Should().Be("");
            model.Funding.Should().Be("");
            model.Purpose.Should().Be("");
            model.CreatedBy.Should().Be("creator-2");
            model.PropertyContacts.Should().Be("");
            model.ManagementFileStatus.Should().Be("");
            model.ActivityType.Should().Be("TypeA");
            model.ActivitySubTypes.Should().Be("Sub1");
            model.CompletionDate.Should().Be("2022-12-31");
            model.ServicesProvider.Should().Be("OrgB");
            model.InvoicePreTaxTotal.Should().Be(100);
            model.InvoiceGstAmount.Should().Be(5);
            model.InvoicePstAmount.Should().Be(7);
            model.InvoiceTotal.Should().Be(112);

            // The invoice is not approved, so approved totals should be zero.
            model.IsPaymentApproved.Should().Be("No");
            model.IsPaymentForwarded.Should().Be("No");
            model.ApprovedPreTaxTotal.Should().Be(0);
            model.ApprovedGstAmount.Should().Be(0);
            model.ApprovedPstAmount.Should().Be(0);
            model.ApprovedTotal.Should().Be(0);

            // Total across all invoices for the activity (both approved and unapproved).
            model.AllPreTaxTotal.Should().Be(100);
            model.AllGstAmount.Should().Be(5);
            model.AllPstAmount.Should().Be(7);
            model.AllTotal.Should().Be(112);
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
            model.IsPaymentApproved.Should().Be("No");
            model.ApprovedPreTaxTotal.Should().Be(0);
            model.ApprovedGstAmount.Should().Be(0);
            model.ApprovedPstAmount.Should().Be(0);
            model.ApprovedTotal.Should().Be(0);
            model.AllPreTaxTotal.Should().Be(0);
            model.AllGstAmount.Should().Be(0);
            model.AllPstAmount.Should().Be(0);
            model.AllTotal.Should().Be(0);
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
            var inv1 = new PimsManagementActivityInvoice { PretaxAmt = 50, GstAmt = 2, PstAmt = 3, TotalAmt = 55, IsPaymentApproved = false };
            var inv2 = new PimsManagementActivityInvoice { PretaxAmt = 60, GstAmt = 3, PstAmt = 4, TotalAmt = 67, IsPaymentApproved = false };
            var act = new PimsManagementActivity { PimsManagementActivityInvoices = new List<PimsManagementActivityInvoice> { inv1, inv2 } };
            inv1.ManagementActivity = act;
            inv2.ManagementActivity = act;

            var model = new ManagementActivityInvoicesReportModel(inv1);

            model.AllPreTaxTotal.Should().Be(110);
            model.AllGstAmount.Should().Be(5);
            model.AllPstAmount.Should().Be(7);
            model.AllTotal.Should().Be(122);
        }

        [Fact]
        public void Constructor_ActivityTotals_SumsInvoices_With_ApprovedInvoices()
        {
            var inv1 = new PimsManagementActivityInvoice { PretaxAmt = 50, GstAmt = 2, PstAmt = 3, TotalAmt = 55, IsPaymentApproved = false };
            var inv2 = new PimsManagementActivityInvoice { PretaxAmt = 60, GstAmt = 3, PstAmt = 4, TotalAmt = 67, IsPaymentApproved = false };
            var inv3 = new PimsManagementActivityInvoice { PretaxAmt = 1000, GstAmt = 50, PstAmt = 0, TotalAmt = 1050, IsPaymentApproved = true };
            var inv4 = new PimsManagementActivityInvoice { PretaxAmt = 1000, GstAmt = 50, PstAmt = 0, TotalAmt = 1050, IsPaymentApproved = true };

            var act = new PimsManagementActivity { PimsManagementActivityInvoices = new List<PimsManagementActivityInvoice> { inv1, inv2, inv3, inv4 } };
            inv1.ManagementActivity = act;
            inv2.ManagementActivity = act;
            inv3.ManagementActivity = act;
            inv4.ManagementActivity = act;

            var model = new ManagementActivityInvoicesReportModel(inv1);

            // Totals for approved invoices only.
            model.ApprovedPreTaxTotal.Should().Be(2000);
            model.ApprovedGstAmount.Should().Be(100);
            model.ApprovedPstAmount.Should().Be(0);
            model.ApprovedTotal.Should().Be(2100);

            // Totals for approved and not approved invoices.
            model.AllPreTaxTotal.Should().Be(2110);
            model.AllGstAmount.Should().Be(105);
            model.AllPstAmount.Should().Be(7);
            model.AllTotal.Should().Be(2222);
        }
    }
}
