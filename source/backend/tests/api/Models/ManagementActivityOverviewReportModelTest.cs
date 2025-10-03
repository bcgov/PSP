using System;
using System.Collections.Generic;
using FluentAssertions;
using Pims.Api.Areas.Reports.Models.Management;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test
{
    public class ManagementActivityOverviewReportModelTest
    {
        [Fact]
        public void Constructor_Throws_OnNull()
        {
            Action act = () => new ManagementActivityOverviewReportModel(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_BasicProperties_MapCorrectly()
        {
            var activity = new PimsManagementActivity
            {
                ManagementFile = new PimsManagementFile
                {
                    FileName = "FileA",
                    LegacyFileNum = "L123",
                    AcquisitionFundingTypeCodeNavigation = new PimsAcquisitionFundingType() { Description = "FundDesc" },
                    ManagementFilePurposeTypeCodeNavigation = new PimsManagementFilePurposeType() { Description = "PurposeDesc" },
                    AppCreateUserid = "creator",
                    ManagementFileStatusTypeCodeNavigation = new PimsManagementFileStatusType() { Description = "StatusDesc" },
                },
                MgmtActivityStatusTypeCodeNavigation = new PimsMgmtActivityStatusType() { Description = "ActStatus" },
                MgmtActivityTypeCodeNavigation = new PimsMgmtActivityType() { Description = "ActType" },
                RequestAddedDt = new DateOnly(2020, 1, 1),
                CompletionDt = new DateOnly(2021, 2, 2),
                Description = "desc",
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.ManagementFileName.Should().Be("FileA");
            model.LegacyFileNum.Should().Be("L123");
            model.Funding.Should().Be("FundDesc");
            model.Purpose.Should().Be("PurposeDesc");
            model.CreatedBy.Should().Be("creator");
            model.ManagementFileStatus.Should().Be("StatusDesc");
            model.ActivityStatus.Should().Be("ActStatus");
            model.ActivityType.Should().Be("ActType");
            model.RequestAddedDate.Should().Be("2020-01-01");
            model.CompletionDate.Should().Be("2021-02-02");
            model.Description.Should().Be("desc");
        }

        [Fact]
        public void Constructor_Properties_EmptyIfNull()
        {
            var activity = new PimsManagementActivity();

            var model = new ManagementActivityOverviewReportModel(activity);

            model.ManagementFileName.Should().BeEmpty();
            model.LegacyFileNum.Should().BeEmpty();
            model.Properties.Should().BeEmpty();
            model.Funding.Should().BeEmpty();
            model.Purpose.Should().BeEmpty();
            model.CreatedBy.Should().BeEmpty();
            model.PropertyContacts.Should().BeEmpty();
            model.ManagementFileStatus.Should().BeEmpty();
            model.ActivityStatus.Should().BeEmpty();
            model.ActivityType.Should().BeEmpty();
            model.ActivitySubTypes.Should().BeEmpty();
            model.ServicesProvider.Should().BeEmpty();
            model.MinistryContacts.Should().BeEmpty();
            model.Description.Should().BeEmpty();
            model.ExternalContacts.Should().BeEmpty();
            model.InvoicesPreTaxTotal.Should().Be(0);
            model.InvoicesTotal.Should().Be(0);
        }

        [Fact]
        public void Constructor_ActivitySubTypes_JoinsCorrectly()
        {
            var activity = new PimsManagementActivity
            {
                PimsMgmtActivityActivitySubtyps = new List<PimsMgmtActivityActivitySubtyp>
                {
                    new PimsMgmtActivityActivitySubtyp { MgmtActivitySubtypeCodeNavigation = new PimsMgmtActivitySubtype { Description = "Sub1" } },
                    new PimsMgmtActivityActivitySubtyp { MgmtActivitySubtypeCodeNavigation = new PimsMgmtActivitySubtype { Description = "Sub2" } },
                }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.ActivitySubTypes.Should().Be("Sub1|Sub2");
        }

        [Fact]
        public void Constructor_ServicesProvider_PersonPreferred()
        {
            var activity = new PimsManagementActivity
            {
                ServiceProviderPerson = new PimsPerson { FirstName = "A", Surname = "B" },
                ServiceProviderOrg = new PimsOrganization { Name = "Org" }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.ServicesProvider.Should().Be("A B");
        }

        [Fact]
        public void Constructor_ServicesProvider_FallsBackToOrganization()
        {
            var activity = new PimsManagementActivity
            {
                ServiceProviderOrg = new PimsOrganization { Name = "Org" }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.ServicesProvider.Should().Be("Org");
        }

        [Fact]
        public void Constructor_PropertyContacts_JoinsCorrectly()
        {
            var activity = new PimsManagementActivity
            {
                ManagementFile = new PimsManagementFile
                {
                    PimsManagementFileContacts = new List<PimsManagementFileContact>
                    {
                        new PimsManagementFileContact { Person = new PimsPerson { FirstName = "A", Surname = "B" } },
                        new PimsManagementFileContact { Organization = new PimsOrganization { Name = "Org" } },
                    }
                }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.PropertyContacts.Should().Be("A B|Org");
        }

        [Fact]
        public void Constructor_MinistryContacts_JoinsCorrectly()
        {
            var activity = new PimsManagementActivity
            {
                PimsMgmtActMinContacts = new List<PimsMgmtActMinContact>
                {
                    new PimsMgmtActMinContact { Person = new PimsPerson { FirstName = "M", Surname = "N" } }
                }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.MinistryContacts.Should().Be("M N");
        }

        [Fact]
        public void Constructor_ExternalContacts_JoinsCorrectly()
        {
            var activity = new PimsManagementActivity
            {
                PimsMgmtActInvolvedParties = new List<PimsMgmtActInvolvedParty>
                {
                    new PimsMgmtActInvolvedParty { Person = new PimsPerson { FirstName = "X", Surname = "Y" } },
                    new PimsMgmtActInvolvedParty { Organization = new PimsOrganization { Name = "OtherOrg" } },
                }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.ExternalContacts.Should().Be("X Y|OtherOrg");
        }

        [Fact]
        public void Constructor_Properties_JoinsDistinctNames()
        {
            var activity = new PimsManagementActivity
            {
                PimsManagementActivityProperties = new List<PimsManagementActivityProperty>
                {
                    new PimsManagementActivityProperty { Property = new PimsProperty { Pid = 111111111, Address = new PimsAddress { StreetAddress1 = "Addr1" } } },
                    new PimsManagementActivityProperty { Property = new PimsProperty { Pid = 222222222, Address = new PimsAddress { StreetAddress1 = "Addr2" } } }
                }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.Properties.Should().Be("111-111-111|222-222-222");
        }

        [Fact]
        public void Constructor_Invoices_SumsCorrectly()
        {
            var activity = new PimsManagementActivity
            {
                PimsManagementActivityInvoices = new List<PimsManagementActivityInvoice>
                {
                    new PimsManagementActivityInvoice { PretaxAmt = 10, TotalAmt = 15 },
                    new PimsManagementActivityInvoice { PretaxAmt = 20, TotalAmt = 25 }
                }
            };

            var model = new ManagementActivityOverviewReportModel(activity);

            model.InvoicesPreTaxTotal.Should().Be(30);
            model.InvoicesTotal.Should().Be(40);
        }
    }
}
