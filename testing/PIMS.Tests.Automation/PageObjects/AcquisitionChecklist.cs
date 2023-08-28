using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using System.Runtime.InteropServices;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionChecklist : PageObjectBase
    {
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
        private By checklistCopyOwnersCompensationChequeLabel = By.XPath("//label[contains(text(),'Copy of owners' compensation cheque(s)')]");
        private By checklistCopyOwnersCompensationChequeContent = By.XPath("//label[contains(text(),'Copy of owners' compensation cheque(s)')]/parent::div/following-sibling::div/div/div[2]/span");
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
        private By checklistSec3CopyAppraisalOwnerLabel = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisial to owner(s)')]");
        private By checklistSec3CopyAppraisalOwnerContent = By.XPath("//div[contains(text(),'Section 3 - Agreement')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisial to owner(s))')]/parent::div/following-sibling::div/div/div[2]/span");

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
        private By checklistNoticeAdvancedPaymentForm8Label = By.XPath("//label[contains(text(),'Notice of Advanced Payment (Form 8)')]");
        private By checklistNoticeAdvancedPaymentForm8Content = By.XPath("//label[contains(text(),'Notice of Advanced Payment (Form 8)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSec6CopyAppraisalOwnerLabel = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisial to owner(s)')]");
        private By checklistSec6CopyAppraisalOwnerContent = By.XPath("//div[contains(text(),'Section 6 - Expropriation')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Copy of appraisial to owner(s))')]/parent::div/following-sibling::div/div/div[2]/span");
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

        }

        public void VerifyChecklistViewForm()
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
            AssertTrueIsDisplayed(checklistNoticeAdvancedPaymentForm8Label);
            AssertTrueIsDisplayed(checklistNoticeAdvancedPaymentForm8Content);
            AssertTrueIsDisplayed(checklistSec6CopyAppraisalOwnerLabel);
            AssertTrueIsDisplayed(checklistSec6CopyAppraisalOwnerContent);
            AssertTrueIsDisplayed(checklistCopyEasementLabel);
            AssertTrueIsDisplayed(checklistCopyEasementContent);

            AssertTrueIsDisplayed(checklistAcquisitionCompletionTitle);
            AssertTrueIsDisplayed(checklistFileDataEnteredIntoPIMSLabel);
            AssertTrueIsDisplayed(checklistFileDataEnteredIntoPIMSContent);

            Assert.True(webDriver.FindElements(checklistTooltips).Count == 23);
        }

        public void VerifyChecklistEditForm()
        {

        }
    }
}
