using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeasesChecklist : PageObjectBase
    {
        private By checklistLinkTab = By.XPath("//a[contains(text(),'Checklist')]");

        private By checklistEditBttn = By.CssSelector("div[class='tab-content'] button");
        private By checklistInfo = By.XPath("//div/em[contains(text(),'This checklist was last updated')]");

        //Checklist View Elements
        private By checklistFileInitiationTitle = By.XPath("//h2/div/div[contains(text(),'File Initiation')]");
        private By checklistFileInitiation1Label = By.XPath("//label[contains(text(),'Initiating Document')]");
        private By checklistFileInitiation1Content = By.XPath("//label[contains(text(),'Initiating Document')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFileInitiation2Label = By.XPath("//label[contains(text(),'Current title')]");
        private By checklistFileInitiation2Content = By.XPath("//label[contains(text(),'Current title')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFileInitiation3Label = By.XPath("//label[contains(text(),'Company search')]");
        private By checklistFileInitiation3Content = By.XPath("//label[contains(text(),'Company search')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFileInitiation4Label = By.XPath("//label[contains(text(),'Land title documents or plans')]");
        private By checklistFileInitiation4Content = By.XPath("//label[contains(text(),'Land title documents or plans')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFileInitiation5Label = By.XPath("//label[contains(text(),'Current/historical files reviewed for conflicts')]");
        private By checklistFileInitiation5Content = By.XPath("//label[contains(text(),'Current/historical files reviewed for conflicts')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFileInitiation6Label = By.XPath("//label[contains(text(),'Update property data in the system')]");
        private By checklistFileInitiation6Content = By.XPath("//label[contains(text(),'Update property data in the system')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistReferralApprovalsTitle = By.XPath("//h2/div/div[contains(text(),'Referrals and Approvals')]");
        private By checklistReferralApprovals1Label = By.XPath("//label[contains(text(),'First Nations')]");
        private By checklistReferralApprovals1Content = By.XPath("//label[contains(text(),'First Nations')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReferralApprovals2Label = By.XPath("//label[contains(text(),'Strategic Real Estate (SRE)')]");
        private By checklistReferralApprovals2Content = By.XPath("//label[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReferralApprovals3Label = By.XPath("//label[contains(text(),'Regional planning')]");
        private By checklistReferralApprovals3Content = By.XPath("//label[contains(text(),'Regional planning')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReferralApprovals4Label = By.XPath("//label[contains(text(),'Regional Property Services')]");
        private By checklistReferralApprovals4Content = By.XPath("//label[contains(text(),'Regional Property Services')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReferralApprovals5Label = By.XPath("//label[contains(text(),'District')]");
        private By checklistReferralApprovals5Content = By.XPath("//label[contains(text(),'District')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReferralApprovals6Label = By.XPath("//label[contains(text(),'Headquarters (HQ)')]");
        private By checklistReferralApprovals6Content = By.XPath("//label[contains(text(),'Headquarters (HQ)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReferralApprovals7Label = By.XPath("//label[contains(text(),'Legal review')]");
        private By checklistReferralApprovals7Content = By.XPath("//label[contains(text(),'Legal review')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReferralApprovals8Label = By.XPath("//label[contains(text(),'Additional reviews or approvals, if applicable')]");
        private By checklistReferralApprovals8Content = By.XPath("//label[contains(text(),'Additional reviews or approvals, if applicable')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistSectionAgreementPreparationTitle = By.XPath("//h2/div/div[contains(text(),'Agreement Preparation')]");
        private By checklistAgreementPreparation1Label = By.XPath("//label[contains(text(),'Fee determination')]");
        private By checklistAgreementPreparation1Content = By.XPath("//label[contains(text(),'Fee determination')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation2Label = By.XPath("//label[contains(text(),'Draft appropriate agreement type')]");
        private By checklistAgreementPreparation2Content = By.XPath("//label[contains(text(),'Draft appropriate agreement type')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation3Label = By.XPath("//label[contains(text(),'Deposit')]");
        private By checklistAgreementPreparation3Content = By.XPath("//label[contains(text(),'Deposit')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation4Label = By.XPath("//label[contains(text(),'Appraisal and Reviews')]");
        private By checklistAgreementPreparation4Content = By.XPath("//label[contains(text(),'Appraisal and Reviews')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation5Label = By.XPath("//label[contains(text(),'Agreement approval')]");
        private By checklistAgreementPreparation5Content = By.XPath("//label[contains(text(),'Agreement approval')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation6Label = By.XPath("//label[contains(text(),'Insurance details')]");
        private By checklistAgreementPreparation6Content = By.XPath("//label[contains(text(),'Insurance details')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation7Label = By.XPath("//label[contains(text(),'Other reports')]");
        private By checklistAgreementPreparation7Content = By.XPath("//label[contains(text(),'Other reports')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation8Label = By.XPath("//label[contains(text(),'Pre-tenancy photos')]");
        private By checklistAgreementPreparation8Content = By.XPath("//label[contains(text(),'Pre-tenancy photos')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation9Label = By.XPath("//label[contains(text(),'Finance team notified')]");
        private By checklistAgreementPreparation9Content = By.XPath("//label[contains(text(),'Finance team notified')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation10Label = By.XPath("//label[contains(text(),'Agreement signed by both parties')]");
        private By checklistAgreementPreparation10Content = By.XPath("//label[contains(text(),'Agreement signed by both parties')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation11Label = By.XPath("//label[contains(text(),'Executed agreement sent to other party (BCTFA)')]");
        private By checklistAgreementPreparation11Content = By.XPath("//label[contains(text(),'Executed agreement sent to other party (BCTFA)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation12Label = By.XPath("//label[contains(text(),'Landlord responsibilities noted')]");
        private By checklistAgreementPreparation12Content = By.XPath("//label[contains(text(),'Landlord responsibilities noted')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAgreementPreparation13Label = By.XPath("//label[contains(text(),'BC Assessment notified')]");
        private By checklistAgreementPreparation13Content = By.XPath("//label[contains(text(),'BC Assessment notified')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistLeaseLicenceCompletionTitle = By.XPath("//h2/div/div[contains(text(),'Lease/Licence Completion')]");
        private By checklistLeaseLicenceCompletion1Label = By.XPath("//label[contains(text(),'File data entered into PIMS')]");
        private By checklistLeaseLicenceCompletion1Content = By.XPath("//label[contains(text(),'File data entered into PIMS')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLeaseLicenceCompletion2Label = By.XPath("//label[contains(text(),'Update the lease/license status')]");
        private By checklistLeaseLicenceCompletion2Content = By.XPath("//label[contains(text(),'Update the lease/license status')]/parent::div/following-sibling::div/div/div[2]/span");

        //Checklist Edit Mode Elements
        private By checklistFileInitiationItem1Select = By.Id("input-checklistSections[0].items[0].statusType");
        private By checklistFileInitiationItem2Select = By.Id("input-checklistSections[0].items[1].statusType");
        private By checklistFileInitiationItem3Select = By.Id("input-checklistSections[0].items[2].statusType");
        private By checklistFileInitiationItem4Select = By.Id("input-checklistSections[0].items[3].statusType");
        private By checklistFileInitiationItem5Select = By.Id("input-checklistSections[0].items[4].statusType");
        private By checklistFileInitiationItem6Select = By.Id("input-checklistSections[0].items[5].statusType");

        private By checklistReferralsApprovals1Select = By.Id("input-checklistSections[1].items[0].statusType");
        private By checklistReferralsApprovals2Select = By.Id("input-checklistSections[1].items[1].statusType");
        private By checklistReferralsApprovals3Select = By.Id("input-checklistSections[1].items[2].statusType");
        private By checklistReferralsApprovals4Select = By.Id("input-checklistSections[1].items[3].statusType");
        private By checklistReferralsApprovals5Select = By.Id("input-checklistSections[1].items[4].statusType");
        private By checklistReferralsApprovals6Select = By.Id("input-checklistSections[1].items[5].statusType");
        private By checklistReferralsApprovals7Select = By.Id("input-checklistSections[1].items[6].statusType");
        private By checklistReferralsApprovals8Select = By.Id("input-checklistSections[1].items[7].statusType");

        private By checklistAgreementPreparation1Select = By.Id("input-checklistSections[2].items[0].statusType");
        private By checklistAgreementPreparation2Select = By.Id("input-checklistSections[2].items[1].statusType");
        private By checklistAgreementPreparation3Select = By.Id("input-checklistSections[2].items[2].statusType");
        private By checklistAgreementPreparation4Select = By.Id("input-checklistSections[2].items[3].statusType");
        private By checklistAgreementPreparation5Select = By.Id("input-checklistSections[2].items[4].statusType");
        private By checklistAgreementPreparation6Select = By.Id("input-checklistSections[2].items[5].statusType");
        private By checklistAgreementPreparation7Select = By.Id("input-checklistSections[2].items[6].statusType");
        private By checklistAgreementPreparation8Select = By.Id("input-checklistSections[2].items[7].statusType");
        private By checklistAgreementPreparation9Select = By.Id("input-checklistSections[2].items[8].statusType");
        private By checklistAgreementPreparation10Select = By.Id("input-checklistSections[2].items[9].statusType");
        private By checklistAgreementPreparation11Select = By.Id("input-checklistSections[2].items[10].statusType");
        private By checklistAgreementPreparation12Select = By.Id("input-checklistSections[2].items[11].statusType");
        private By checklistAgreementPreparation13Select = By.Id("input-checklistSections[2].items[12].statusType");

        private By checklistLeaseLicenseCompletion1Select = By.Id("input-checklistSections[3].items[0].statusType");
        private By checklistLeaseLicenseCompletion2Select = By.Id("input-checklistSections[3].items[1].statusType");

        public LeasesChecklist(IWebDriver webDriver) : base(webDriver)
        { }

        public void NavigateChecklistTab()
        {
            WaitUntilClickable(checklistLinkTab);
            webDriver.FindElement(checklistLinkTab).Click();
        }

        public void EditChecklistButton()
        {
            WaitUntilClickable(checklistEditBttn);
            webDriver.FindElement(checklistEditBttn).Click();
        }

        public void VerifyChecklistInitViewForm()
        {
            Wait();

            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistFileInitiation1Label);
            AssertTrueIsDisplayed(checklistFileInitiation1Content);
            AssertTrueIsDisplayed(checklistFileInitiation2Label);
            AssertTrueIsDisplayed(checklistFileInitiation2Content);
            AssertTrueIsDisplayed(checklistFileInitiation3Label);
            AssertTrueIsDisplayed(checklistFileInitiation3Content);
            AssertTrueIsDisplayed(checklistFileInitiation4Label);
            AssertTrueIsDisplayed(checklistFileInitiation4Content);
            AssertTrueIsDisplayed(checklistFileInitiation5Label);
            AssertTrueIsDisplayed(checklistFileInitiation5Content);
            AssertTrueIsDisplayed(checklistFileInitiation6Label);
            AssertTrueIsDisplayed(checklistFileInitiation6Content);

            AssertTrueIsDisplayed(checklistReferralApprovalsTitle);
            AssertTrueIsDisplayed(checklistReferralApprovals1Label);
            AssertTrueIsDisplayed(checklistReferralApprovals1Content);
            AssertTrueIsDisplayed(checklistReferralApprovals2Label);
            AssertTrueIsDisplayed(checklistReferralApprovals2Content);
            AssertTrueIsDisplayed(checklistReferralApprovals3Label);
            AssertTrueIsDisplayed(checklistReferralApprovals3Content);
            AssertTrueIsDisplayed(checklistReferralApprovals4Label);
            AssertTrueIsDisplayed(checklistReferralApprovals4Content);
            AssertTrueIsDisplayed(checklistReferralApprovals5Label);
            AssertTrueIsDisplayed(checklistReferralApprovals5Content);
            AssertTrueIsDisplayed(checklistReferralApprovals6Label);
            AssertTrueIsDisplayed(checklistReferralApprovals6Content);
            AssertTrueIsDisplayed(checklistReferralApprovals7Label);
            AssertTrueIsDisplayed(checklistReferralApprovals7Content);
            AssertTrueIsDisplayed(checklistReferralApprovals8Label);
            AssertTrueIsDisplayed(checklistReferralApprovals8Content);

            AssertTrueIsDisplayed(checklistSectionAgreementPreparationTitle);
            AssertTrueIsDisplayed(checklistAgreementPreparation1Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation1Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation2Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation2Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation3Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation3Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation4Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation4Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation5Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation5Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation6Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation6Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation11Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation11Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation12Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation12Content);
            AssertTrueIsDisplayed(checklistAgreementPreparation10Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation10Content);

            AssertTrueIsDisplayed(checklistLeaseLicenceCompletionTitle);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion1Label);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion1Content);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion2Label);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion2Content);
        }

        public void VerifyChecklistEditForm()
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistFileInitiation1Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem1Select);
            AssertTrueIsDisplayed(checklistFileInitiation2Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem2Select);
            AssertTrueIsDisplayed(checklistFileInitiation3Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem3Select);
            AssertTrueIsDisplayed(checklistFileInitiation4Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem4Select);
            AssertTrueIsDisplayed(checklistFileInitiation5Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem5Select);
            AssertTrueIsDisplayed(checklistFileInitiation6Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem6Select);

            AssertTrueIsDisplayed(checklistReferralApprovalsTitle);
            AssertTrueIsDisplayed(checklistReferralApprovals1Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals1Select);
            AssertTrueIsDisplayed(checklistReferralApprovals2Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals2Select);
            AssertTrueIsDisplayed(checklistReferralApprovals3Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals3Select);
            AssertTrueIsDisplayed(checklistReferralApprovals4Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals4Select);
            AssertTrueIsDisplayed(checklistReferralApprovals5Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals5Select);
            AssertTrueIsDisplayed(checklistReferralApprovals6Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals6Select);
            AssertTrueIsDisplayed(checklistReferralApprovals7Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals7Select);
            AssertTrueIsDisplayed(checklistReferralApprovals8Label);
            AssertTrueIsDisplayed(checklistReferralsApprovals8Select);

            AssertTrueIsDisplayed(checklistSectionAgreementPreparationTitle);
            AssertTrueIsDisplayed(checklistAgreementPreparation1Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation1Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation2Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation2Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation3Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation3Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation4Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation4Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation5Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation5Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation6Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation6Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation7Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation7Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation8Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation8Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation9Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation9Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation10Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation10Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation11Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation11Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation12Select);
            AssertTrueIsDisplayed(checklistAgreementPreparation12Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation13Label);
            AssertTrueIsDisplayed(checklistAgreementPreparation13Select);

            AssertTrueIsDisplayed(checklistLeaseLicenceCompletionTitle);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion1Label);
            AssertTrueIsDisplayed(checklistLeaseLicenseCompletion1Select);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion2Label);
            AssertTrueIsDisplayed(checklistLeaseLicenseCompletion2Select);
        }

        public void VerifyChecklistViewForm(LeaseChecklist checklist)
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistFileInitiation1Label);
            AssertTrueContentEquals(checklistFileInitiation1Content, checklist.FileInitiationSelect1);
            AssertTrueIsDisplayed(checklistFileInitiation2Label);
            AssertTrueContentEquals(checklistFileInitiation2Content, checklist.FileInitiationSelect2);
            AssertTrueIsDisplayed(checklistFileInitiation3Label);
            AssertTrueContentEquals(checklistFileInitiation3Content, checklist.FileInitiationSelect3);
            AssertTrueIsDisplayed(checklistFileInitiation4Label);
            AssertTrueContentEquals(checklistFileInitiation4Content, checklist.FileInitiationSelect4);
            AssertTrueIsDisplayed(checklistFileInitiation5Label);
            AssertTrueContentEquals(checklistFileInitiation5Content, checklist.FileInitiationSelect5);
            AssertTrueIsDisplayed(checklistFileInitiation6Label);
            AssertTrueContentEquals(checklistFileInitiation6Content, checklist.FileInitiationSelect6);

            AssertTrueIsDisplayed(checklistReferralApprovalsTitle);
            AssertTrueIsDisplayed(checklistReferralApprovals1Label);
            AssertTrueContentEquals(checklistReferralApprovals1Content, checklist.ReferralsApprovalsSelect1);
            AssertTrueIsDisplayed(checklistReferralApprovals2Label);
            AssertTrueContentEquals(checklistReferralApprovals2Content, checklist.ReferralsApprovalsSelect2);
            AssertTrueIsDisplayed(checklistReferralApprovals3Label);
            AssertTrueContentEquals(checklistReferralApprovals3Content, checklist.ReferralsApprovalsSelect3);
            AssertTrueIsDisplayed(checklistReferralApprovals4Label);
            AssertTrueContentEquals(checklistReferralApprovals4Content, checklist.ReferralsApprovalsSelect4);
            AssertTrueIsDisplayed(checklistReferralApprovals5Label);
            AssertTrueContentEquals(checklistReferralApprovals5Content, checklist.ReferralsApprovalsSelect5);
            AssertTrueIsDisplayed(checklistReferralApprovals6Label);
            AssertTrueContentEquals(checklistReferralApprovals6Content, checklist.ReferralsApprovalsSelect6);
            AssertTrueIsDisplayed(checklistReferralApprovals7Label);
            AssertTrueContentEquals(checklistReferralApprovals7Content, checklist.ReferralsApprovalsSelect7);
            AssertTrueIsDisplayed(checklistReferralApprovals8Label);
            AssertTrueContentEquals(checklistReferralApprovals8Content, checklist.ReferralsApprovalsSelect8);

            AssertTrueIsDisplayed(checklistSectionAgreementPreparationTitle);
            AssertTrueIsDisplayed(checklistAgreementPreparation1Label);
            AssertTrueContentEquals(checklistAgreementPreparation1Content, checklist.AgreementPreparationSelect1);
            AssertTrueIsDisplayed(checklistAgreementPreparation2Label);
            AssertTrueContentEquals(checklistAgreementPreparation2Content, checklist.AgreementPreparationSelect2);
            AssertTrueIsDisplayed(checklistAgreementPreparation3Label);
            AssertTrueContentEquals(checklistAgreementPreparation3Content, checklist.AgreementPreparationSelect3);
            AssertTrueIsDisplayed(checklistAgreementPreparation4Label);
            AssertTrueContentEquals(checklistAgreementPreparation4Content, checklist.AgreementPreparationSelect4);
            AssertTrueIsDisplayed(checklistAgreementPreparation5Label);
            AssertTrueContentEquals(checklistAgreementPreparation5Content, checklist.AgreementPreparationSelect5);
            AssertTrueIsDisplayed(checklistAgreementPreparation6Label);
            AssertTrueContentEquals(checklistAgreementPreparation6Content, checklist.AgreementPreparationSelect6);
            AssertTrueIsDisplayed(checklistAgreementPreparation7Label);
            AssertTrueContentEquals(checklistAgreementPreparation7Content, checklist.AgreementPreparationSelect7);
            AssertTrueIsDisplayed(checklistAgreementPreparation8Label);
            AssertTrueContentEquals(checklistAgreementPreparation8Content, checklist.AgreementPreparationSelect8);
            AssertTrueIsDisplayed(checklistAgreementPreparation9Label);
            AssertTrueContentEquals(checklistAgreementPreparation9Content, checklist.AgreementPreparationSelect9);
            AssertTrueIsDisplayed(checklistAgreementPreparation10Label);
            AssertTrueContentEquals(checklistAgreementPreparation10Content, checklist.AgreementPreparationSelect10);
            AssertTrueIsDisplayed(checklistAgreementPreparation11Label);
            AssertTrueContentEquals(checklistAgreementPreparation11Content, checklist.AgreementPreparationSelect11);
            AssertTrueIsDisplayed(checklistAgreementPreparation12Label);
            AssertTrueContentEquals(checklistAgreementPreparation12Content, checklist.AgreementPreparationSelect12);
            AssertTrueIsDisplayed(checklistAgreementPreparation13Label);
            AssertTrueContentEquals(checklistAgreementPreparation13Content, checklist.AgreementPreparationSelect13);

            AssertTrueIsDisplayed(checklistLeaseLicenceCompletionTitle);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion1Label);
            AssertTrueContentEquals(checklistLeaseLicenceCompletion1Content, checklist.LeaseLicenceCompletionSelect1);
            AssertTrueIsDisplayed(checklistLeaseLicenceCompletion2Label);
            AssertTrueContentEquals(checklistLeaseLicenceCompletion2Content, checklist.LeaseLicenceCompletionSelect2);
        }

        public void UpdateChecklist(LeaseChecklist checklist)
        {
            Wait();

            if (checklist.FileInitiationSelect1 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem1Select, checklist.FileInitiationSelect1);
            if (checklist.FileInitiationSelect2 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem2Select, checklist.FileInitiationSelect2);
            if (checklist.FileInitiationSelect3 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem3Select, checklist.FileInitiationSelect3);
            if (checklist.FileInitiationSelect4 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem4Select, checklist.FileInitiationSelect4);
            if (checklist.FileInitiationSelect5 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem5Select, checklist.FileInitiationSelect5);
            if (checklist.FileInitiationSelect6 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem6Select, checklist.FileInitiationSelect6);

            if (checklist.ReferralsApprovalsSelect1 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals1Select, checklist.ReferralsApprovalsSelect1);
            if (checklist.ReferralsApprovalsSelect2 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals2Select, checklist.ReferralsApprovalsSelect2);
            if (checklist.ReferralsApprovalsSelect3 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals3Select, checklist.ReferralsApprovalsSelect3);
            if (checklist.ReferralsApprovalsSelect4 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals4Select, checklist.ReferralsApprovalsSelect4);
            if (checklist.ReferralsApprovalsSelect5 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals5Select, checklist.ReferralsApprovalsSelect5);
            if (checklist.ReferralsApprovalsSelect6 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals6Select, checklist.ReferralsApprovalsSelect6);
            if (checklist.ReferralsApprovalsSelect7 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals7Select, checklist.ReferralsApprovalsSelect7);
            if (checklist.ReferralsApprovalsSelect8 != "")
                ChooseSpecificSelectOption(checklistReferralsApprovals8Select, checklist.ReferralsApprovalsSelect8);

            if (checklist.AgreementPreparationSelect1 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation1Select, checklist.AgreementPreparationSelect1);
            if (checklist.AgreementPreparationSelect2 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation2Select, checklist.AgreementPreparationSelect2);
            if (checklist.AgreementPreparationSelect3 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation3Select, checklist.AgreementPreparationSelect3);
            if (checklist.AgreementPreparationSelect4 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation4Select, checklist.AgreementPreparationSelect4);
            if (checklist.AgreementPreparationSelect5 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation5Select, checklist.AgreementPreparationSelect5);
            if (checklist.AgreementPreparationSelect6 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation6Select, checklist.AgreementPreparationSelect6);
            if (checklist.AgreementPreparationSelect7 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation7Select, checklist.AgreementPreparationSelect7);
            if (checklist.AgreementPreparationSelect8 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation8Select, checklist.AgreementPreparationSelect8);
            if (checklist.AgreementPreparationSelect9 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation9Select, checklist.AgreementPreparationSelect9);
            if (checklist.AgreementPreparationSelect10 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation10Select, checklist.AgreementPreparationSelect10);
            if (checklist.AgreementPreparationSelect11 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation11Select, checklist.AgreementPreparationSelect11);
            if (checklist.AgreementPreparationSelect12 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation12Select, checklist.AgreementPreparationSelect12);
            if (checklist.AgreementPreparationSelect13 != "")
                ChooseSpecificSelectOption(checklistAgreementPreparation13Select, checklist.AgreementPreparationSelect13);

            if (checklist.LeaseLicenceCompletionSelect1 != "")
                ChooseSpecificSelectOption(checklistLeaseLicenseCompletion1Select, checklist.LeaseLicenceCompletionSelect1);
            if (checklist.LeaseLicenceCompletionSelect2 != "")
                ChooseSpecificSelectOption(checklistLeaseLicenseCompletion2Select, checklist.LeaseLicenceCompletionSelect2);
        }

        public void SaveLeaseChecklist()
        {
            Wait();
            ButtonElement("Save");

            AssertTrueIsDisplayed(checklistEditBttn);
        }
    }
}

