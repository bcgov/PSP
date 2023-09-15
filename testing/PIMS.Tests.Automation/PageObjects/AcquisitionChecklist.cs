using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System.Runtime.InteropServices;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionChecklist : PageObjectBase
    {
        private By checklistLinkTab = By.XPath("//a[contains(text(),'Checklist')]");

        private By checklistEditBttn = By.CssSelector("button[title='Edit acquisition checklist']");
        private By checklistInfo = By.XPath("//div/em[contains(text(),'This checklist was last updated')]");

        //Checklist View Elements
        private By checklistFileInitiationTitle = By.XPath("//h2/div/div[contains(text(),'File Initiation')]");
        private By checklistPreAcquisitionTitleLabel = By.XPath("//label[contains(text(),'Pre-acquisition title')]");
        private By checklistPreAcquisitionTitleContent = By.XPath("//label[contains(text(),'Pre-acquisition title')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistBCAssessmentLabel = By.XPath("//label[contains(text(),'BC Assessment')]");
        private By checklistBCAssessmentContent = By.XPath("//label[contains(text(),'BC Assessment')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCompanySearchLabel = By.XPath("//label[contains(text(),'Company search')]");
        private By checklistCompanySearchContent = By.XPath("//label[contains(text(),'Company search')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLandTitleDocsLabel = By.XPath("//label[contains(text(),'Land Title documents or plans')]");
        private By checklistLandTitleDocsContent = By.XPath("//label[contains(text(),'Land Title documents or plans')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistPopertyAcquisitionPlanLabel = By.XPath("//label[contains(text(),'Property Acquisition (PA) Plan')]");
        private By checklistPropertyAcquisitionPlanContent = By.XPath("//label[contains(text(),'Property Acquisition (PA) Plan')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistActiveFileManagementTitle = By.XPath("//h2/div/div[contains(text(),'Active File Management')]");
        private By checklistPhotosLabel = By.XPath("//label[contains(text(),'Photos')]");
        private By checklistPhotosContent = By.XPath("//label[contains(text(),'Photos')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAppraisalsReviewsLabel = By.XPath("//label[contains(text(),'Appraisals and reviews')]");
        private By checklistAppraisalsReviewsContent = By.XPath("//label[contains(text(),'Appraisals and reviews')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLegalSurveyLabel = By.XPath("//label[contains(text(),'Legal survey(s)')]");
        private By checklistLegalSurveyContent = By.XPath("//label[contains(text(),'Legal survey(s)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistOtherReportsLabel = By.XPath("//label[contains(text(),'Other reports')]");
        private By checklistOtherReportsContent = By.XPath("//label[contains(text(),'Other reports')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistRecordNegotiationLabel = By.XPath("//label[contains(text(),'Record of negotiation')]");
        private By checklistRecordNegotiationContent = By.XPath("//label[contains(text(),'Record of negotiation')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopiesCorrespondenceLabel = By.XPath("//label[contains(text(),'Copies of correspondence')]");
        private By checklistCopiesCorrespondenceContent = By.XPath("//label[contains(text(),'Copies of correspondence')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSpendingAuthorityApprovalLabel = By.XPath("//label[contains(text(),'Spending Authority Approval(s) - SAA')]");
        private By checklistSpendingAuthorityApprovalContent = By.XPath("//label[contains(text(),'Spending Authority Approval(s) - SAA')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFinalAgreementsOwnerLabel = By.XPath("//label[contains(text(),'Final agreements with owner(s)')]");
        private By checklistFinalAgreementsOwnerContent = By.XPath("//label[contains(text(),'Final agreements with owner(s)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistH120ProcessedCompensationLabel = By.XPath("//label[contains(text(),'H120(s) processed for compensation')]");
        private By checklistH120ProcessedCompensationContent = By.XPath("//label[contains(text(),'H120(s) processed for compensation')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistInstructionsSentLawyerLabel = By.XPath("//label[contains(text(),'Instructions or documents sent to lawyer')]");
        private By checklistInstructionsSentLawyerContent = By.XPath("//label[contains(text(),'Instructions or documents sent to lawyer')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopyOwnersCompensationChequeLabel = By.XPath("//label[contains(text(),'Copy of owners')]");
        private By checklistCopyOwnersCompensationChequeContent = By.XPath("//label[contains(text(),'Copy of owners')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistH0443Label = By.XPath("//label[contains(text(),'Conditions of Entry (H0443)')]");
        private By checklistH0443Content = By.XPath("//label[contains(text(),'Conditions of Entry (H0443)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistConveyanceClosingDocumentsLabel = By.XPath("//label[contains(text(),'Conveyance closing documents')]");
        private By checklistConveyanceClosingDocumentsContent = By.XPath("//label[contains(text(),'Conveyance closing documents')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistRegisteredPlanLabel = By.XPath("//label[contains(text(),'Registered Plan')]");
        private By checklistRegisteredPlanContent = By.XPath("//label[contains(text(),'Registered Plan')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistALCOrderLabel = By.XPath("//label[contains(text(),'ALC order')]");
        private By checklistALCOrderContent = By.XPath("//label[contains(text(),'ALC order')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopyNewTitleLabel = By.XPath("//label[contains(text(),'Copy of new title')]");
        private By checklistCopyNewTitleContent = By.XPath("//label[contains(text(),'Copy of new title')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistBCTFANotifiedLabel = By.XPath("//label[contains(text(),'BCTFA notified')]");
        private By checklistBCTFANotifiedContent = By.XPath("//label[contains(text(),'BCTFA notified')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistCrownLandTitle = By.XPath("//h2/div/div[contains(text(),'Crown Land')]");
        private By checklistCrownLandResearchLabel = By.XPath("//label[contains(text(),'Crown Land research')]");
        private By checklistCrownLandResearchContent = By.XPath("//label[contains(text(),'Crown Land research')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSubmitCrownLandTenureApplicationLabel = By.XPath("//label[contains(text(),'Submit Crown Land tenure application')]");
        private By checklistSubmitCrownLandTenureApplicationContent = By.XPath("//label[contains(text(),'Submit Crown Land tenure application')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCrownGrantPackageLabel = By.XPath("//label[contains(text(),'Crown grant package')]");
        private By checklistCrownGrantPackageContent = By.XPath("//label[contains(text(),'Crown grant package')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistSection3AgreementTitle = By.XPath("//h2/div/div[contains(text(),'Section 3 - Agreement')]");
        private By checklistSec3NoticeAdvancedPaymentForm8Label = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Notice of Advanced Payment (Form 8)')]");
        private By checklistSec3NoticeAdvancedPaymentForm8Content = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Notice of Advanced Payment (Form 8)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistReleaseClaimsLabel = By.XPath("//label[contains(text(),'Release of Claims')]");
        private By checklistReleaseClaimsContent = By.XPath("//label[contains(text(),'Release of Claims')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistConveyanceLetterLabel = By.XPath("//label[contains(text(),'Conveyance letter')]");
        private By checklistConveyanceLetterContent = By.XPath("//label[contains(text(),'Conveyance letter')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopyExpropriationActLabel = By.XPath("//label[contains(text(),'Copy of Expropriation Act')]");
        private By checklistCopyExpropriationActContent = By.XPath("//label[contains(text(),'Copy of Expropriation Act')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSignedSection3AgreementLabel = By.XPath("//label[contains(text(),'Signed Section 3 Agreement')]");
        private By checklistSignedSection3AgreementContent = By.XPath("//label[contains(text(),'Signed Section 3 Agreement')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistTitleChargesLabel = By.XPath("//label[contains(text(),'Title and charges')]");
        private By checklistTitleChargesContent = By.XPath("//label[contains(text(),'Title and charges')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCurrentYearPropertyAssessmentLabel = By.XPath("//label[contains(text(),'Current year property assessment')]");
        private By checklistCurrentYearPropertyAssessmentContent = By.XPath("//label[contains(text(),'Current year property assessment')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSec3CopyChequeToOwnerLabel = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of cheque to owner(s)')]");
        private By checklistSec3CopyChequeToOwnerContent = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of cheque to owner(s)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSec3CopyAppraisalOwnerLabel = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisal to owner(s)')]");
        private By checklistSec3CopyAppraisalOwnerContent = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisal to owner(s)')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistSection6ExpropriationTitle = By.XPath("//h2/div/div[contains(text(),'Section 6 - Expropriation')]");
        private By checklistCopyOfferExtendedOwnerLabel = By.XPath("//label[contains(text(),'Copy of offer(s) extended to owner(s)')]");
        private By checklistCopyOfferExtendedOwnerContent = By.XPath("//label[contains(text(),'Copy of offer(s) extended to owner(s)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopyExpropriationApprovalPackagesLabel = By.XPath("//label[contains(text(),'Copy of expropriation approval packages')]");
        private By checklistCopyExpropriationApprovalPackagesContent = By.XPath("//label[contains(text(),'Copy of expropriation approval packages')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistNoticeExpropriationForm1Label = By.XPath("//label[contains(text(),'Notice of Expropriation (Form 1)')]");
        private By checklistNoticeExpropriationForm1Content = By.XPath("//label[contains(text(),'Notice of Expropriation (Form 1)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistApprovalExpropriationForm5Label = By.XPath("//label[contains(text(),'Approval of Expropriation (Form 5)')]");
        private By checklistApprovalExpropriationForm5Content = By.XPath("//label[contains(text(),'Approval of Expropriation (Form 5)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSec6NoticeAdvancedPaymentForm8Label = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Notice of Advanced Payment (Form 8)')]");
        private By checklistSec6NoticeAdvancedPaymentForm8Content = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Notice of Advanced Payment (Form 8)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistVestingNoticeForm9Label = By.XPath("//label[contains(text(),'Vesting Notice (Form 9)')]");
        private By checklistVestingNoticeForm9Content = By.XPath("//label[contains(text(),'Vesting Notice (Form 9)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopiesAffidavitsServiceLabel = By.XPath("//label[contains(text(),'Copies of Affidavits of Service')]");
        private By checklistCopiesAffidavitsServiceContent = By.XPath("//label[contains(text(),'Copies of Affidavits of Service')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSec6CopyChequeToOwnerLabel = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of cheque to owner(s)')]");
        private By checklistSec6CopyChequeToOwnerContent = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of cheque to owner(s)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSec6CopyAppraisalOwnerLabel = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisal to owner(s)')]");
        private By checklistSec6CopyAppraisalOwnerContent = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisal to owner(s)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSec6ReleaseClaimsLabel = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Release of Claims')]");
        private By checklistSec6ReleaseClaimsContent = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Release of Claims')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopyEasementLabel = By.XPath("//label[contains(text(),'Copy of Easement')]");
        private By checklistCopyEasementContent = By.XPath("//label[contains(text(),'Copy of Easement')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistAcquisitionCompletionTitle = By.XPath("//h2/div/div[contains(text(),'Acquisition Completion')]");
        private By checklistFileDataEnteredIntoPIMSLabel = By.XPath("//label[contains(text(),'File data entered into PIMS')]");
        private By checklistFileDataEnteredIntoPIMSContent = By.XPath("//label[contains(text(),'File data entered into PIMS')]/parent::div/following-sibling::div/div/div[2]/span");

        private By checklistTooltips = By.CssSelector("span[data-testid='tooltip-icon-section-field-tooltip']");

        //Checklist Edit Mode Elements
        private By checklistPreAcquisitionTitleSelect = By.Id("input-checklistSections[0].items[0].statusType");
        private By checklistBCAssessmentSelect = By.Id("input-checklistSections[0].items[1].statusType");
        private By checklistCompanySearchSelect = By.Id("input-checklistSections[0].items[2].statusType");
        private By checklistLandTitleDocumentsSelect = By.Id("input-checklistSections[0].items[3].statusType");
        private By checklistPropertyAcquisitionPlanSelect = By.Id("input-checklistSections[0].items[4].statusType");

        private By checklistPhotosSelect = By.Id("input-checklistSections[1].items[0].statusType");
        private By checklistAppraisalReviewsSelect = By.Id("input-checklistSections[1].items[1].statusType");
        private By checklistLegalSurveySelect = By.Id("input-checklistSections[1].items[2].statusType");
        private By checklistOtherReportsSelect = By.Id("input-checklistSections[1].items[3].statusType");
        private By checklistRecordOfNegotiationSelect = By.Id("input-checklistSections[1].items[4].statusType");
        private By checklistCopiesCorrespondenceSelect = By.Id("input-checklistSections[1].items[5].statusType");
        private By checklistSpendingAuthorityApprovalSelect = By.Id("input-checklistSections[1].items[6].statusType");
        private By checklistFinalAgreementWithOwnerSelect = By.Id("input-checklistSections[1].items[7].statusType");
        private By checklistH120ProcessedCompensationSelect = By.Id("input-checklistSections[1].items[8].statusType");
        private By checklistInstructionsDocumentsSentToLawyerSelect = By.Id("input-checklistSections[1].items[9].statusType");
        private By checklistCopyOwnersCompensationChequeSelect = By.Id("input-checklistSections[1].items[10].statusType");
        private By checklistH0443Select = By.Id("input-checklistSections[1].items[11].statusType");
        private By checklistConveyanceClosingDocumentsSelect = By.Id("input-checklistSections[1].items[12].statusType");
        private By checklistRegisteredPlanSelect = By.Id("input-checklistSections[1].items[13].statusType");
        private By checklistALCOrderSelect = By.Id("input-checklistSections[1].items[14].statusType");
        private By checklistCopyNewTitleSelect = By.Id("input-checklistSections[1].items[15].statusType");
        private By checklistBCTFANotifiedSelect = By.Id("input-checklistSections[1].items[16].statusType");

        private By checklistCrownLandResearchSelect = By.Id("input-checklistSections[2].items[0].statusType");
        private By checklistSubmitCrownLandTenureApplicationSelect = By.Id("input-checklistSections[2].items[1].statusType");
        private By checklistCrownGrantPackageSelect = By.Id("input-checklistSections[2].items[2].statusType");

        private By checklistSec3NoticeAdvancedPaymentForm8Select = By.Id("input-checklistSections[3].items[0].statusType");
        private By checklistSec3ReleaseClaimsSelect = By.Id("input-checklistSections[3].items[1].statusType");
        private By checklistConveyanceLetterSelect = By.Id("input-checklistSections[3].items[2].statusType");
        private By checklistCopyExpropriationActSelect = By.Id("input-checklistSections[3].items[3].statusType");
        private By checklistSignedSection3AgreementSelect = By.Id("input-checklistSections[3].items[4].statusType");
        private By checklistTitleChargesSelect = By.Id("input-checklistSections[3].items[5].statusType");
        private By checklistCurrentYearPropertyAssessmentSelect = By.Id("input-checklistSections[3].items[6].statusType");
        private By checklistSec3CopyChequeToOwnerSelect = By.Id("input-checklistSections[3].items[7].statusType");
        private By checklistCopyAppraisialOwnerSelect = By.Id("input-checklistSections[3].items[8].statusType");

        private By checklistSec6CopyOfferExtendedOwnerSelect = By.Id("input-checklistSections[4].items[0].statusType");
        private By checklistCopyExpropriationApprovalPackagesSelect = By.Id("input-checklistSections[4].items[1].statusType");
        private By checklistNoticeExpropriationForm1Select = By.Id("input-checklistSections[4].items[2].statusType");
        private By checklistApprovalExpropriationForm5Select = By.Id("input-checklistSections[4].items[3].statusType");
        private By checklistNoticeAdvancedPaymentForm8Select = By.Id("input-checklistSections[4].items[4].statusType");
        private By checklistVestingNoticeForm9Select = By.Id("input-checklistSections[4].items[5].statusType");
        private By checklistCopiesAffidavitsServiceSelect = By.Id("input-checklistSections[4].items[6].statusType");
        private By checklistSec6CopyChequeToOwnerSelect = By.Id("input-checklistSections[4].items[7].statusType");
        private By checklistSec6CopyAppraisalToOwnerSelect = By.Id("input-checklistSections[4].items[8].statusType");
        private By checklistSec6ReleaseClaimsSelect = By.Id("input-checklistSections[4].items[9].statusType");
        private By checklistCopyEasementSelect = By.Id("input-checklistSections[4].items[10].statusType");

        private By checklistFileDataEnteredIntoPIMSSelect = By.Id("input-checklistSections[5].items[0].statusType");

        public AcquisitionChecklist(IWebDriver webDriver) : base(webDriver)
        {}

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
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistPreAcquisitionTitleLabel);
            AssertTrueIsDisplayed(checklistPreAcquisitionTitleContent);
            AssertTrueIsDisplayed(checklistBCAssessmentLabel);
            AssertTrueIsDisplayed(checklistBCAssessmentContent);
            AssertTrueIsDisplayed(checklistCompanySearchLabel);
            AssertTrueIsDisplayed(checklistCompanySearchContent);
            AssertTrueIsDisplayed(checklistLandTitleDocsLabel);
            AssertTrueIsDisplayed(checklistLandTitleDocsContent);
            AssertTrueIsDisplayed(checklistPopertyAcquisitionPlanLabel);
            AssertTrueIsDisplayed(checklistPropertyAcquisitionPlanContent);

            AssertTrueIsDisplayed(checklistActiveFileManagementTitle);
            AssertTrueIsDisplayed(checklistPhotosLabel);
            AssertTrueIsDisplayed(checklistPhotosContent);
            AssertTrueIsDisplayed(checklistAppraisalsReviewsLabel);
            AssertTrueIsDisplayed(checklistAppraisalsReviewsContent);
            AssertTrueIsDisplayed(checklistLegalSurveyLabel);
            AssertTrueIsDisplayed(checklistLegalSurveyContent);
            AssertTrueIsDisplayed(checklistOtherReportsLabel);
            AssertTrueIsDisplayed(checklistOtherReportsContent);
            AssertTrueIsDisplayed(checklistRecordNegotiationLabel);
            AssertTrueIsDisplayed(checklistRecordNegotiationContent);
            AssertTrueIsDisplayed(checklistCopiesCorrespondenceLabel);
            AssertTrueIsDisplayed(checklistCopiesCorrespondenceContent);
            AssertTrueIsDisplayed(checklistSpendingAuthorityApprovalLabel);
            AssertTrueIsDisplayed(checklistSpendingAuthorityApprovalContent);
            AssertTrueIsDisplayed(checklistFinalAgreementsOwnerLabel);
            AssertTrueIsDisplayed(checklistFinalAgreementsOwnerContent);
            AssertTrueIsDisplayed(checklistH120ProcessedCompensationLabel);
            AssertTrueIsDisplayed(checklistH120ProcessedCompensationContent);
            AssertTrueIsDisplayed(checklistInstructionsSentLawyerLabel);
            AssertTrueIsDisplayed(checklistInstructionsSentLawyerContent);
            AssertTrueIsDisplayed(checklistCopyOwnersCompensationChequeLabel);
            AssertTrueIsDisplayed(checklistCopyOwnersCompensationChequeContent);
            AssertTrueIsDisplayed(checklistH0443Label);
            AssertTrueIsDisplayed(checklistH0443Content);
            AssertTrueIsDisplayed(checklistConveyanceClosingDocumentsLabel);
            AssertTrueIsDisplayed(checklistConveyanceClosingDocumentsContent);
            AssertTrueIsDisplayed(checklistRegisteredPlanLabel);
            AssertTrueIsDisplayed(checklistRegisteredPlanContent);
            AssertTrueIsDisplayed(checklistALCOrderLabel);
            AssertTrueIsDisplayed(checklistALCOrderContent);
            AssertTrueIsDisplayed(checklistCopyNewTitleLabel);
            AssertTrueIsDisplayed(checklistCopyNewTitleContent);
            AssertTrueIsDisplayed(checklistBCTFANotifiedLabel);
            AssertTrueIsDisplayed(checklistBCTFANotifiedContent);

            AssertTrueIsDisplayed(checklistCrownLandTitle);
            AssertTrueIsDisplayed(checklistCrownLandResearchLabel);
            AssertTrueIsDisplayed(checklistCrownLandResearchContent);
            AssertTrueIsDisplayed(checklistSubmitCrownLandTenureApplicationLabel);
            AssertTrueIsDisplayed(checklistSubmitCrownLandTenureApplicationContent);
            AssertTrueIsDisplayed(checklistCrownGrantPackageLabel);
            AssertTrueIsDisplayed(checklistCrownGrantPackageContent);

            AssertTrueIsDisplayed(checklistSection3AgreementTitle);
            AssertTrueIsDisplayed(checklistSec3NoticeAdvancedPaymentForm8Label);
            AssertTrueIsDisplayed(checklistSec3NoticeAdvancedPaymentForm8Content);
            AssertTrueIsDisplayed(checklistReleaseClaimsLabel);
            AssertTrueIsDisplayed(checklistReleaseClaimsContent);
            AssertTrueIsDisplayed(checklistConveyanceLetterLabel);
            AssertTrueIsDisplayed(checklistConveyanceLetterContent);
            AssertTrueIsDisplayed(checklistCopyExpropriationActLabel);
            AssertTrueIsDisplayed(checklistCopyExpropriationActContent);
            AssertTrueIsDisplayed(checklistSignedSection3AgreementLabel);
            AssertTrueIsDisplayed(checklistSignedSection3AgreementContent);
            AssertTrueIsDisplayed(checklistTitleChargesLabel);
            AssertTrueIsDisplayed(checklistTitleChargesContent);
            AssertTrueIsDisplayed(checklistCurrentYearPropertyAssessmentLabel);
            AssertTrueIsDisplayed(checklistCurrentYearPropertyAssessmentContent);
            AssertTrueIsDisplayed(checklistSec3CopyChequeToOwnerLabel);
            AssertTrueIsDisplayed(checklistSec3CopyChequeToOwnerContent);
            AssertTrueIsDisplayed(checklistSec3CopyAppraisalOwnerLabel);
            AssertTrueIsDisplayed(checklistSec3CopyAppraisalOwnerContent);

            AssertTrueIsDisplayed(checklistSection6ExpropriationTitle);
            AssertTrueIsDisplayed(checklistCopyOfferExtendedOwnerLabel);
            AssertTrueIsDisplayed(checklistCopyOfferExtendedOwnerContent);
            AssertTrueIsDisplayed(checklistCopyExpropriationApprovalPackagesLabel);
            AssertTrueIsDisplayed(checklistCopyExpropriationApprovalPackagesContent);
            AssertTrueIsDisplayed(checklistNoticeExpropriationForm1Label);
            AssertTrueIsDisplayed(checklistNoticeExpropriationForm1Content);
            AssertTrueIsDisplayed(checklistApprovalExpropriationForm5Label);
            AssertTrueIsDisplayed(checklistApprovalExpropriationForm5Content);
            AssertTrueIsDisplayed(checklistSec6NoticeAdvancedPaymentForm8Label);
            AssertTrueIsDisplayed(checklistSec6NoticeAdvancedPaymentForm8Content);
            AssertTrueIsDisplayed(checklistVestingNoticeForm9Label);
            AssertTrueIsDisplayed(checklistVestingNoticeForm9Content);
            AssertTrueIsDisplayed(checklistCopiesAffidavitsServiceLabel);
            AssertTrueIsDisplayed(checklistCopiesAffidavitsServiceContent);
            AssertTrueIsDisplayed(checklistSec6CopyChequeToOwnerLabel);
            AssertTrueIsDisplayed(checklistSec6CopyChequeToOwnerContent);
            AssertTrueIsDisplayed(checklistSec6CopyAppraisalOwnerLabel);
            AssertTrueIsDisplayed(checklistSec6CopyAppraisalOwnerContent);
            AssertTrueIsDisplayed(checklistSec6ReleaseClaimsLabel);
            AssertTrueIsDisplayed(checklistSec6ReleaseClaimsContent);
            AssertTrueIsDisplayed(checklistCopyEasementLabel);
            AssertTrueIsDisplayed(checklistCopyEasementContent);

            AssertTrueIsDisplayed(checklistAcquisitionCompletionTitle);
            AssertTrueIsDisplayed(checklistFileDataEnteredIntoPIMSLabel);
            AssertTrueIsDisplayed(checklistFileDataEnteredIntoPIMSContent);

            Assert.True(webDriver.FindElements(checklistTooltips).Count == 23);
        }

        public void VerifyChecklistEditForm()
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistPreAcquisitionTitleLabel);
            AssertTrueIsDisplayed(checklistPreAcquisitionTitleSelect);
            AssertTrueIsDisplayed(checklistBCAssessmentLabel);
            AssertTrueIsDisplayed(checklistBCAssessmentSelect);
            AssertTrueIsDisplayed(checklistCompanySearchLabel);
            AssertTrueIsDisplayed(checklistCompanySearchSelect);
            AssertTrueIsDisplayed(checklistLandTitleDocsLabel);
            AssertTrueIsDisplayed(checklistLandTitleDocumentsSelect);
            AssertTrueIsDisplayed(checklistPopertyAcquisitionPlanLabel);
            AssertTrueIsDisplayed(checklistPropertyAcquisitionPlanSelect);

            AssertTrueIsDisplayed(checklistActiveFileManagementTitle);
            AssertTrueIsDisplayed(checklistPhotosLabel);
            AssertTrueIsDisplayed(checklistPhotosSelect);
            AssertTrueIsDisplayed(checklistAppraisalsReviewsLabel);
            AssertTrueIsDisplayed(checklistAppraisalReviewsSelect);
            AssertTrueIsDisplayed(checklistLegalSurveyLabel);
            AssertTrueIsDisplayed(checklistLegalSurveySelect);
            AssertTrueIsDisplayed(checklistOtherReportsLabel);
            AssertTrueIsDisplayed(checklistOtherReportsSelect);
            AssertTrueIsDisplayed(checklistRecordNegotiationLabel);
            AssertTrueIsDisplayed(checklistRecordOfNegotiationSelect);
            AssertTrueIsDisplayed(checklistCopiesCorrespondenceLabel);
            AssertTrueIsDisplayed(checklistCopiesCorrespondenceSelect);
            AssertTrueIsDisplayed(checklistSpendingAuthorityApprovalLabel);
            AssertTrueIsDisplayed(checklistSpendingAuthorityApprovalSelect);
            AssertTrueIsDisplayed(checklistFinalAgreementsOwnerLabel);
            AssertTrueIsDisplayed(checklistFinalAgreementWithOwnerSelect);
            AssertTrueIsDisplayed(checklistH120ProcessedCompensationLabel);
            AssertTrueIsDisplayed(checklistH120ProcessedCompensationSelect);
            AssertTrueIsDisplayed(checklistInstructionsSentLawyerLabel);
            AssertTrueIsDisplayed(checklistInstructionsDocumentsSentToLawyerSelect);
            AssertTrueIsDisplayed(checklistCopyOwnersCompensationChequeLabel);
            AssertTrueIsDisplayed(checklistCopyOwnersCompensationChequeSelect);
            AssertTrueIsDisplayed(checklistH0443Label);
            AssertTrueIsDisplayed(checklistH0443Select);
            AssertTrueIsDisplayed(checklistConveyanceClosingDocumentsLabel);
            AssertTrueIsDisplayed(checklistConveyanceClosingDocumentsSelect);
            AssertTrueIsDisplayed(checklistRegisteredPlanLabel);
            AssertTrueIsDisplayed(checklistRegisteredPlanSelect);
            AssertTrueIsDisplayed(checklistALCOrderLabel);
            AssertTrueIsDisplayed(checklistALCOrderSelect);
            AssertTrueIsDisplayed(checklistCopyNewTitleLabel);
            AssertTrueIsDisplayed(checklistCopyNewTitleSelect);
            AssertTrueIsDisplayed(checklistBCTFANotifiedLabel);
            AssertTrueIsDisplayed(checklistBCTFANotifiedSelect);

            AssertTrueIsDisplayed(checklistCrownLandTitle);
            AssertTrueIsDisplayed(checklistCrownLandResearchLabel);
            AssertTrueIsDisplayed(checklistCrownLandResearchSelect);
            AssertTrueIsDisplayed(checklistSubmitCrownLandTenureApplicationLabel);
            AssertTrueIsDisplayed(checklistSubmitCrownLandTenureApplicationSelect);
            AssertTrueIsDisplayed(checklistCrownGrantPackageLabel);
            AssertTrueIsDisplayed(checklistCrownGrantPackageSelect);

            AssertTrueIsDisplayed(checklistSection3AgreementTitle);
            AssertTrueIsDisplayed(checklistSec3NoticeAdvancedPaymentForm8Label);
            AssertTrueIsDisplayed(checklistSec3NoticeAdvancedPaymentForm8Select);
            AssertTrueIsDisplayed(checklistReleaseClaimsLabel);
            AssertTrueIsDisplayed(checklistSec3ReleaseClaimsSelect);
            AssertTrueIsDisplayed(checklistConveyanceLetterLabel);
            AssertTrueIsDisplayed(checklistConveyanceLetterSelect);
            AssertTrueIsDisplayed(checklistCopyExpropriationActLabel);
            AssertTrueIsDisplayed(checklistCopyExpropriationActSelect);
            AssertTrueIsDisplayed(checklistSignedSection3AgreementLabel);
            AssertTrueIsDisplayed(checklistSignedSection3AgreementSelect);
            AssertTrueIsDisplayed(checklistTitleChargesLabel);
            AssertTrueIsDisplayed(checklistTitleChargesSelect);
            AssertTrueIsDisplayed(checklistCurrentYearPropertyAssessmentLabel);
            AssertTrueIsDisplayed(checklistCurrentYearPropertyAssessmentSelect);
            AssertTrueIsDisplayed(checklistSec3CopyChequeToOwnerLabel);
            AssertTrueIsDisplayed(checklistSec3CopyChequeToOwnerSelect);
            AssertTrueIsDisplayed(checklistSec3CopyAppraisalOwnerLabel);
            AssertTrueIsDisplayed(checklistCopyAppraisialOwnerSelect);

            AssertTrueIsDisplayed(checklistSection6ExpropriationTitle);
            AssertTrueIsDisplayed(checklistCopyOfferExtendedOwnerLabel);
            AssertTrueIsDisplayed(checklistSec6CopyOfferExtendedOwnerSelect);
            AssertTrueIsDisplayed(checklistCopyExpropriationApprovalPackagesLabel);
            AssertTrueIsDisplayed(checklistCopyExpropriationApprovalPackagesSelect);
            AssertTrueIsDisplayed(checklistNoticeExpropriationForm1Label);
            AssertTrueIsDisplayed(checklistNoticeExpropriationForm1Select);
            AssertTrueIsDisplayed(checklistApprovalExpropriationForm5Label);
            AssertTrueIsDisplayed(checklistApprovalExpropriationForm5Select);
            AssertTrueIsDisplayed(checklistSec6NoticeAdvancedPaymentForm8Label);
            AssertTrueIsDisplayed(checklistNoticeAdvancedPaymentForm8Select);
            AssertTrueIsDisplayed(checklistVestingNoticeForm9Label);
            AssertTrueIsDisplayed(checklistVestingNoticeForm9Select);
            AssertTrueIsDisplayed(checklistCopiesAffidavitsServiceLabel);
            AssertTrueIsDisplayed(checklistCopiesAffidavitsServiceSelect);
            AssertTrueIsDisplayed(checklistSec6CopyChequeToOwnerLabel);
            AssertTrueIsDisplayed(checklistSec6CopyChequeToOwnerSelect);
            AssertTrueIsDisplayed(checklistSec6CopyAppraisalToOwnerSelect);
            AssertTrueIsDisplayed(checklistSec6CopyAppraisalOwnerLabel);
            AssertTrueIsDisplayed(checklistSec6ReleaseClaimsLabel);
            AssertTrueIsDisplayed(checklistSec6ReleaseClaimsSelect);
            AssertTrueIsDisplayed(checklistCopyEasementLabel);
            AssertTrueIsDisplayed(checklistCopyEasementSelect);

            AssertTrueIsDisplayed(checklistAcquisitionCompletionTitle);
            AssertTrueIsDisplayed(checklistFileDataEnteredIntoPIMSLabel);
            AssertTrueIsDisplayed(checklistFileDataEnteredIntoPIMSSelect);
        }

        public void VerifyChecklistViewForm(AcquisitionFileChecklist checklist)
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistPreAcquisitionTitleLabel);
            AssertTrueContentEquals(checklistPreAcquisitionTitleContent, checklist.FileInitiationSelect1);
            AssertTrueIsDisplayed(checklistBCAssessmentLabel);
            AssertTrueContentEquals(checklistBCAssessmentContent, checklist.FileInitiationSelect2);
            AssertTrueIsDisplayed(checklistCompanySearchLabel);
            AssertTrueContentEquals(checklistCompanySearchContent, checklist.FileInitiationSelect3);
            AssertTrueIsDisplayed(checklistLandTitleDocsLabel);
            AssertTrueContentEquals(checklistLandTitleDocsContent, checklist.FileInitiationSelect4);
            AssertTrueIsDisplayed(checklistPopertyAcquisitionPlanLabel);
            AssertTrueContentEquals(checklistPropertyAcquisitionPlanContent, checklist.FileInitiationSelect5);

            AssertTrueIsDisplayed(checklistActiveFileManagementTitle);
            AssertTrueIsDisplayed(checklistPhotosLabel);
            AssertTrueContentEquals(checklistPhotosContent, checklist.ActiveFileManagementSelect1);
            AssertTrueIsDisplayed(checklistAppraisalsReviewsLabel);
            AssertTrueContentEquals(checklistAppraisalsReviewsContent, checklist.ActiveFileManagementSelect2);
            AssertTrueIsDisplayed(checklistLegalSurveyLabel);
            AssertTrueContentEquals(checklistLegalSurveyContent, checklist.ActiveFileManagementSelect3);
            AssertTrueIsDisplayed(checklistOtherReportsLabel);
            AssertTrueContentEquals(checklistOtherReportsContent, checklist.ActiveFileManagementSelect4);
            AssertTrueIsDisplayed(checklistRecordNegotiationLabel);
            AssertTrueContentEquals(checklistRecordNegotiationContent, checklist.ActiveFileManagementSelect5);
            AssertTrueIsDisplayed(checklistCopiesCorrespondenceLabel);
            AssertTrueContentEquals(checklistCopiesCorrespondenceContent, checklist.ActiveFileManagementSelect6);
            AssertTrueIsDisplayed(checklistSpendingAuthorityApprovalLabel);
            AssertTrueContentEquals(checklistSpendingAuthorityApprovalContent, checklist.ActiveFileManagementSelect7);
            AssertTrueIsDisplayed(checklistFinalAgreementsOwnerLabel);
            AssertTrueContentEquals(checklistFinalAgreementsOwnerContent, checklist.ActiveFileManagementSelect8);
            AssertTrueIsDisplayed(checklistH120ProcessedCompensationLabel);
            AssertTrueContentEquals(checklistH120ProcessedCompensationContent, checklist.ActiveFileManagementSelect9);
            AssertTrueIsDisplayed(checklistInstructionsSentLawyerLabel);
            AssertTrueContentEquals(checklistInstructionsSentLawyerContent, checklist.ActiveFileManagementSelect10);
            AssertTrueIsDisplayed(checklistCopyOwnersCompensationChequeLabel);
            AssertTrueContentEquals(checklistCopyOwnersCompensationChequeContent, checklist.ActiveFileManagementSelect11);
            AssertTrueIsDisplayed(checklistH0443Label);
            AssertTrueContentEquals(checklistH0443Content, checklist.ActiveFileManagementSelect12);
            AssertTrueIsDisplayed(checklistConveyanceClosingDocumentsLabel);
            AssertTrueContentEquals(checklistConveyanceClosingDocumentsContent, checklist.ActiveFileManagementSelect13);
            AssertTrueIsDisplayed(checklistRegisteredPlanLabel);
            AssertTrueContentEquals(checklistRegisteredPlanContent, checklist.ActiveFileManagementSelect14);
            AssertTrueIsDisplayed(checklistALCOrderLabel);
            AssertTrueContentEquals(checklistALCOrderContent, checklist.ActiveFileManagementSelect15);
            AssertTrueIsDisplayed(checklistCopyNewTitleLabel);
            AssertTrueContentEquals(checklistCopyNewTitleContent, checklist.ActiveFileManagementSelect16);
            AssertTrueIsDisplayed(checklistBCTFANotifiedLabel);
            AssertTrueContentEquals(checklistBCTFANotifiedContent, checklist.ActiveFileManagementSelect17);

            AssertTrueIsDisplayed(checklistCrownLandTitle);
            AssertTrueIsDisplayed(checklistCrownLandResearchLabel);
            AssertTrueContentEquals(checklistCrownLandResearchContent, checklist.CrownLandSelect1);
            AssertTrueIsDisplayed(checklistSubmitCrownLandTenureApplicationLabel);
            AssertTrueContentEquals(checklistSubmitCrownLandTenureApplicationContent, checklist.CrownLandSelect2);
            AssertTrueIsDisplayed(checklistCrownGrantPackageLabel);
            AssertTrueContentEquals(checklistCrownGrantPackageContent, checklist.CrownLandSelect3);

            AssertTrueIsDisplayed(checklistSection3AgreementTitle);
            AssertTrueIsDisplayed(checklistSec3NoticeAdvancedPaymentForm8Label);
            AssertTrueContentEquals(checklistSec3NoticeAdvancedPaymentForm8Content, checklist.Section3AgreementSelect1);
            AssertTrueIsDisplayed(checklistReleaseClaimsLabel);
            AssertTrueContentEquals(checklistReleaseClaimsContent, checklist.Section3AgreementSelect2);
            AssertTrueIsDisplayed(checklistConveyanceLetterLabel);
            AssertTrueContentEquals(checklistConveyanceLetterContent, checklist.Section3AgreementSelect3);
            AssertTrueIsDisplayed(checklistCopyExpropriationActLabel);
            AssertTrueContentEquals(checklistCopyExpropriationActContent, checklist.Section3AgreementSelect4);
            AssertTrueIsDisplayed(checklistSignedSection3AgreementLabel);
            AssertTrueContentEquals(checklistSignedSection3AgreementContent, checklist.Section3AgreementSelect5);
            AssertTrueIsDisplayed(checklistTitleChargesLabel);
            AssertTrueContentEquals(checklistTitleChargesContent, checklist.Section3AgreementSelect6);
            AssertTrueIsDisplayed(checklistCurrentYearPropertyAssessmentLabel);
            AssertTrueContentEquals(checklistCurrentYearPropertyAssessmentContent, checklist.Section3AgreementSelect7);
            AssertTrueIsDisplayed(checklistSec3CopyChequeToOwnerLabel);
            AssertTrueContentEquals(checklistSec3CopyChequeToOwnerContent, checklist.Section3AgreementSelect8);
            AssertTrueIsDisplayed(checklistSec3CopyAppraisalOwnerLabel);
            AssertTrueContentEquals(checklistSec3CopyAppraisalOwnerContent, checklist.Section3AgreementSelect9);

            AssertTrueIsDisplayed(checklistSection6ExpropriationTitle);
            AssertTrueIsDisplayed(checklistCopyOfferExtendedOwnerLabel);
            AssertTrueContentEquals(checklistCopyOfferExtendedOwnerContent, checklist.Section6ExpropriationSelect1);
            AssertTrueIsDisplayed(checklistCopyExpropriationApprovalPackagesLabel);
            AssertTrueContentEquals(checklistCopyExpropriationApprovalPackagesContent, checklist.Section6ExpropriationSelect2);
            AssertTrueIsDisplayed(checklistNoticeExpropriationForm1Label);
            AssertTrueContentEquals(checklistNoticeExpropriationForm1Content, checklist.Section6ExpropriationSelect3);
            AssertTrueIsDisplayed(checklistApprovalExpropriationForm5Label);
            AssertTrueContentEquals(checklistApprovalExpropriationForm5Content, checklist.Section6ExpropriationSelect4);
            AssertTrueIsDisplayed(checklistSec6NoticeAdvancedPaymentForm8Label);
            AssertTrueContentEquals(checklistSec6NoticeAdvancedPaymentForm8Content, checklist.Section6ExpropriationSelect5);
            AssertTrueIsDisplayed(checklistVestingNoticeForm9Label);
            AssertTrueContentEquals(checklistVestingNoticeForm9Content, checklist.Section6ExpropriationSelect6);
            AssertTrueIsDisplayed(checklistCopiesAffidavitsServiceLabel);
            AssertTrueContentEquals(checklistCopiesAffidavitsServiceContent, checklist.Section6ExpropriationSelect7);
            AssertTrueIsDisplayed(checklistSec6CopyChequeToOwnerLabel);
            AssertTrueContentEquals(checklistSec6CopyChequeToOwnerContent, checklist.Section6ExpropriationSelect8);
            AssertTrueIsDisplayed(checklistSec6CopyAppraisalOwnerLabel);
            AssertTrueContentEquals(checklistSec6CopyAppraisalOwnerContent, checklist.Section6ExpropriationSelect9);
            AssertTrueIsDisplayed(checklistSec6ReleaseClaimsLabel);
            AssertTrueContentEquals(checklistSec6ReleaseClaimsContent, checklist.Section6ExpropriationSelect10);
            AssertTrueIsDisplayed(checklistCopyEasementLabel);
            AssertTrueContentEquals(checklistCopyEasementContent, checklist.Section6ExpropriationSelect11);

            AssertTrueIsDisplayed(checklistAcquisitionCompletionTitle);
            AssertTrueIsDisplayed(checklistFileDataEnteredIntoPIMSLabel);
            AssertTrueContentEquals(checklistFileDataEnteredIntoPIMSContent, checklist.AcquisitionCompletionSelect1);
        }

        public void UpdateChecklist(AcquisitionFileChecklist checklist)
        {
            Wait();

            if(checklist.FileInitiationSelect1 != "")
                ChooseSpecificSelectOption(checklistPreAcquisitionTitleSelect, checklist.FileInitiationSelect1);
            if (checklist.FileInitiationSelect2 != "")
                ChooseSpecificSelectOption(checklistBCAssessmentSelect, checklist.FileInitiationSelect2);
            if (checklist.FileInitiationSelect3 != "")
                ChooseSpecificSelectOption(checklistCompanySearchSelect, checklist.FileInitiationSelect3);
            if (checklist.FileInitiationSelect4 != "")
                ChooseSpecificSelectOption(checklistLandTitleDocumentsSelect, checklist.FileInitiationSelect4);
            if (checklist.FileInitiationSelect5 != "")
                ChooseSpecificSelectOption(checklistPropertyAcquisitionPlanSelect, checklist.FileInitiationSelect5);

            if (checklist.ActiveFileManagementSelect1 != "")
                ChooseSpecificSelectOption(checklistPhotosSelect, checklist.ActiveFileManagementSelect1);
            if (checklist.ActiveFileManagementSelect2 != "")
                ChooseSpecificSelectOption(checklistAppraisalReviewsSelect, checklist.ActiveFileManagementSelect2);
            if (checklist.ActiveFileManagementSelect3 != "")
                ChooseSpecificSelectOption(checklistLegalSurveySelect, checklist.ActiveFileManagementSelect3);
            if (checklist.ActiveFileManagementSelect4 != "")
                ChooseSpecificSelectOption(checklistOtherReportsSelect, checklist.ActiveFileManagementSelect4);
            if (checklist.ActiveFileManagementSelect5 != "")
                ChooseSpecificSelectOption(checklistRecordOfNegotiationSelect, checklist.ActiveFileManagementSelect5);
            if (checklist.ActiveFileManagementSelect6 != "")
                ChooseSpecificSelectOption(checklistCopiesCorrespondenceSelect, checklist.ActiveFileManagementSelect6);
            if (checklist.ActiveFileManagementSelect7 != "")
                ChooseSpecificSelectOption(checklistSpendingAuthorityApprovalSelect, checklist.ActiveFileManagementSelect7);
            if (checklist.ActiveFileManagementSelect8 != "")
                ChooseSpecificSelectOption(checklistFinalAgreementWithOwnerSelect, checklist.ActiveFileManagementSelect8);
            if (checklist.ActiveFileManagementSelect9 != "")
                ChooseSpecificSelectOption(checklistH120ProcessedCompensationSelect, checklist.ActiveFileManagementSelect9);
            if (checklist.ActiveFileManagementSelect10 != "")
                ChooseSpecificSelectOption(checklistInstructionsDocumentsSentToLawyerSelect, checklist.ActiveFileManagementSelect10);
            if (checklist.ActiveFileManagementSelect11 != "")
                ChooseSpecificSelectOption(checklistCopyOwnersCompensationChequeSelect, checklist.ActiveFileManagementSelect11);
            if (checklist.ActiveFileManagementSelect12 != "")
                ChooseSpecificSelectOption(checklistH0443Select, checklist.ActiveFileManagementSelect12);
            if (checklist.ActiveFileManagementSelect13 != "")
                ChooseSpecificSelectOption(checklistConveyanceClosingDocumentsSelect, checklist.ActiveFileManagementSelect13);
            if (checklist.ActiveFileManagementSelect14 != "")
                ChooseSpecificSelectOption(checklistRegisteredPlanSelect, checklist.ActiveFileManagementSelect14);
            if (checklist.ActiveFileManagementSelect15 != "")
                ChooseSpecificSelectOption(checklistALCOrderSelect, checklist.ActiveFileManagementSelect15);
            if (checklist.ActiveFileManagementSelect16 != "")
                ChooseSpecificSelectOption(checklistCopyNewTitleSelect, checklist.ActiveFileManagementSelect16);
            if (checklist.ActiveFileManagementSelect17 != "")
                ChooseSpecificSelectOption(checklistBCTFANotifiedSelect, checklist.ActiveFileManagementSelect17);

            if (checklist.CrownLandSelect1 != "")
                ChooseSpecificSelectOption(checklistCrownLandResearchSelect, checklist.CrownLandSelect1);
            if (checklist.CrownLandSelect2 != "")
                ChooseSpecificSelectOption(checklistSubmitCrownLandTenureApplicationSelect, checklist.CrownLandSelect2);
            if (checklist.CrownLandSelect3 != "")
                ChooseSpecificSelectOption(checklistCrownGrantPackageSelect, checklist.CrownLandSelect3);

            if (checklist.Section3AgreementSelect1 != "")
                ChooseSpecificSelectOption(checklistSec3NoticeAdvancedPaymentForm8Select, checklist.Section3AgreementSelect1);
            if (checklist.Section3AgreementSelect2 != "")
                ChooseSpecificSelectOption(checklistSec3ReleaseClaimsSelect, checklist.Section3AgreementSelect2);
            if (checklist.Section3AgreementSelect3 != "")
                ChooseSpecificSelectOption(checklistConveyanceLetterSelect, checklist.Section3AgreementSelect3);
            if (checklist.Section3AgreementSelect4 != "")
                ChooseSpecificSelectOption(checklistCopyExpropriationActSelect, checklist.Section3AgreementSelect4);
            if (checklist.Section3AgreementSelect5 != "")
                ChooseSpecificSelectOption(checklistSignedSection3AgreementSelect, checklist.Section3AgreementSelect5);
            if (checklist.Section3AgreementSelect6 != "")
                ChooseSpecificSelectOption(checklistTitleChargesSelect, checklist.Section3AgreementSelect6);
            if (checklist.Section3AgreementSelect7 != "")
                ChooseSpecificSelectOption(checklistCurrentYearPropertyAssessmentSelect, checklist.Section3AgreementSelect7);
            if (checklist.Section3AgreementSelect8 != "")
                ChooseSpecificSelectOption(checklistSec3CopyChequeToOwnerSelect, checklist.Section3AgreementSelect8);
            if (checklist.Section3AgreementSelect9 != "")
                ChooseSpecificSelectOption(checklistCopyAppraisialOwnerSelect, checklist.Section3AgreementSelect9);

            if (checklist.Section6ExpropriationSelect1 != "")
                ChooseSpecificSelectOption(checklistSec6CopyOfferExtendedOwnerSelect, checklist.Section6ExpropriationSelect1);
            if (checklist.Section6ExpropriationSelect2 != "")
                ChooseSpecificSelectOption(checklistCopyExpropriationApprovalPackagesSelect, checklist.Section6ExpropriationSelect2);
            if (checklist.Section6ExpropriationSelect3 != "")
                ChooseSpecificSelectOption(checklistNoticeExpropriationForm1Select, checklist.Section6ExpropriationSelect3);
            if (checklist.Section6ExpropriationSelect4 != "")
                ChooseSpecificSelectOption(checklistApprovalExpropriationForm5Select, checklist.Section6ExpropriationSelect4);
            if (checklist.Section6ExpropriationSelect5 != "")
                ChooseSpecificSelectOption(checklistNoticeAdvancedPaymentForm8Select, checklist.Section6ExpropriationSelect5);
            if (checklist.Section6ExpropriationSelect6 != "")
                ChooseSpecificSelectOption(checklistVestingNoticeForm9Select, checklist.Section6ExpropriationSelect6);
            if (checklist.Section6ExpropriationSelect7 != "")
                ChooseSpecificSelectOption(checklistCopiesAffidavitsServiceSelect, checklist.Section6ExpropriationSelect7);
            if (checklist.Section6ExpropriationSelect8 != "")
                ChooseSpecificSelectOption(checklistSec6CopyChequeToOwnerSelect, checklist.Section6ExpropriationSelect8);
            if (checklist.Section6ExpropriationSelect9 != "")
                ChooseSpecificSelectOption(checklistSec6CopyAppraisalToOwnerSelect, checklist.Section6ExpropriationSelect9);
            if (checklist.Section6ExpropriationSelect10 != "")
                ChooseSpecificSelectOption(checklistSec6ReleaseClaimsSelect, checklist.Section6ExpropriationSelect10);
            if (checklist.Section6ExpropriationSelect11 != "")
                ChooseSpecificSelectOption(checklistCopyEasementSelect, checklist.Section6ExpropriationSelect11);

            if (checklist.AcquisitionCompletionSelect1 != "")
                ChooseSpecificSelectOption(checklistFileDataEnteredIntoPIMSSelect, checklist.AcquisitionCompletionSelect1);
        }

        public void SaveAcquisitionFileChecklist()
        {
            Wait();
            ButtonElement("Save");

            AssertTrueIsDisplayed(checklistEditBttn);
        }
    }
}
