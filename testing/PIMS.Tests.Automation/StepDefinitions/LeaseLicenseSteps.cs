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
        private readonly LeasePayments payments;

        private readonly string userName = "sutairak";

        private readonly string leaseStartDate = "02/22/2022";
        private readonly string leaseExpiryDate = "03/22/2024";
        private readonly string motiContact = "Automation Test";
        private readonly string responsibilityDate = "02/23/2022";
        private readonly string locationDoc = "Automation Test Location of Documents";
        private readonly string pid1 = "23915803";
        private readonly string description = "Automation Test Description";
        private readonly string notes = "Automation Test Notes";

        private readonly string individualTenant = "John Smith";
        private readonly string organizationTenant1 = "Astro Vancouver";
        private readonly string organizationTenant2 = "Yahoo Ventures";
        private readonly string organizationTenant3 = "Bishop of Victoria";

        private readonly string firstTermEndDate = "04/22/2022";
        private readonly string firstTermAgreedPayment = "2500";
        private readonly string firstTermPaymentSentDate = "04/23/2022";
        private readonly string firstTermPaymentReceived = "2625";

        private readonly string secondTermStartDate = "04/22/2022";
        private readonly string secondTermEndDate = "06/30/2022";
        private readonly string secondTermAgreedPayment = "3500";
        private readonly string secondTermPaymentSentDate = "04/23/2022";
        private readonly string secondTermPaymentReceived = "3000";

        private readonly string termPaymentDue = "Automation Test Due";
        private readonly string noGST = "N";
        private readonly string yesGST = "Y";
        private readonly string termExercised = "Exercised";
        private readonly string termNotExercised = "Not Exercised";



        public LeaseLicenseSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            leaseDetails = new LeaseDetails(driver.Current);
            tenant = new Tenants(driver.Current);
            payments = new LeasePayments(driver.Current);
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

            //Adding an individual Tenant
            tenant.AddIndividualTenant(individualTenant);

            //Saving Tenant
            tenant.SaveTenant();


        }

        [StepDefinition(@"I create a new Lease with all fields")]
        public void MaximumLeaseLicense()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            leaseDetails.NavigateToCreateNewLicense();

            //LEASE DETAILS
            //Create a new Lease/ License Details with maximum fields
            leaseDetails.LicenseDetailsMaxFields(leaseStartDate, leaseExpiryDate, motiContact, responsibilityDate, locationDoc, pid1, description, notes);

            //Save the new license details
            leaseDetails.SaveLicense();

            //TENANTS
            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding an individual Tenant
            tenant.AddIndividualTenant(individualTenant);

            //Saving Tenant 1
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding an organization Tenant
            tenant.AddOrganizationTenant(organizationTenant1);

            //Saving Tenant 2
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding another Organization Tenant
            tenant.AddOrganizationTenant(organizationTenant2);

            //Saving Tenant 3
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding another Organization Tenant
            tenant.AddOrganizationTenant(organizationTenant3);

            //Saving Tenant 4
            tenant.SaveTenant();

            //Assert quantity of tenants
            Assert.True(tenant.TotalTenants());

            //PAYMENTS
            //Navigating to Payments section
            payments.NavigateToTenantSection();

            //Inserting first term
            payments.AddTerm(leaseStartDate, firstTermEndDate, firstTermAgreedPayment, termPaymentDue, yesGST, termExercised);

            //Inserting Payment for first term
            payments.AddPayment(firstTermPaymentSentDate, firstTermPaymentReceived);

            //Inserting second term
            payments.AddTerm(secondTermStartDate, secondTermEndDate, secondTermAgreedPayment, termPaymentDue, noGST, termNotExercised);

            //Inserting Payment for second term
            payments.AddPayment(secondTermPaymentSentDate, secondTermPaymentReceived);

        }
    }
}
