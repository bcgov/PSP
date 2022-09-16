using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public sealed class LeaseLicenseSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly LeaseDetails leaseDetails;
        private readonly Tenants tenant;

        private readonly string userName = "sutairak";

        private readonly string leaseStartDate = "02/22/2022";
        private readonly string leaseExpiryDate = "03/22/2024";
        private readonly string motiContact = "Automation Test";
        private readonly string responsibilityDate = "02/23/2022";
        private readonly string locationDoc = "Automation Test Location of Documents";
        private readonly string pid1 = "23915803";
        private readonly string description = "Automation Test Description";
        private readonly string notes = "Automation Test Notes";

        public LeaseLicenseSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            leaseDetails = new LeaseDetails(driver.Current);
            tenant = new Tenants(driver.Current);
        }

        [StepDefinition(@"I create a new Lease with minimum fields")]
        public void MinimumLeaseLicense()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            leaseDetails.NavigateToCreateNewLicense();

            //Create a new Lease/ License Details with minimum fields
            leaseDetails.LicenseDetailsMinFields(leaseStartDate, pid1);

            //Save the new license details
            leaseDetails.SaveLicense();

            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

        }

        [StepDefinition(@"I create a new Lease with all fields")]
        public void MaximumLeaseLicense()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            leaseDetails.NavigateToCreateNewLicense();

            //Create a new Lease/ License Details with maximum fields
            leaseDetails.LicenseDetailsMaxFields(leaseStartDate, leaseExpiryDate, motiContact, responsibilityDate, locationDoc, pid1, description, notes);

            //Save the new license details
            leaseDetails.SaveLicense();

            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

        }
    }
}
