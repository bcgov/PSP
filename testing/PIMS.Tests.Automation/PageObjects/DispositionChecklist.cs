using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;


namespace PIMS.Tests.Automation.PageObjects
{
    public class DispositionChecklist : PageObjectBase
    {
        private By checklistLinkTab = By.XPath("//a[contains(text(),'Checklist')]");

        private By checklistEditBttn = By.CssSelector("button[title='Edit checklist']");
        private By checklistInfo = By.XPath("//div/em[contains(text(),'This checklist was last updated')]");

        //File Initiation Section View Elements
        private By checklistFileInitiationTitle = By.XPath("//h2/div/div[contains(text(),'File Initiation')]");
        private By checklistCurrentTitleLabel = By.XPath("//label[contains(text(),'Current title')]");
        private By checklistCurrentTitleContent = By.XPath("//label[contains(text(),'Current title')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistBCAssessmentLabel = By.XPath("//label[contains(text(),'BC Assessment')]");
        private By checklistBCAssessmentContent = By.XPath("//label[contains(text(),'BC Assessment')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistInitiatingDocumentLabel = By.XPath("//label[contains(text(),'Initiating Document')]");
        private By checklistInitiatingDocumentContent = By.XPath("//label[contains(text(),'Initiating Document')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistProofOfApplicationFeeLabel = By.XPath("//label[contains(text(),'Proof of Application Fee')]");
        private By checklistProofOfApplicationFeeContent = By.XPath("//label[contains(text(),'Proof of Application Fee')]/parent::div/following-sibling::div/div/div[2]/span");

        //Disposition Preparation section View Elements
        private By checklistDispositionPreparationTitle = By.XPath("//h2/div/div[contains(text(),'Disposition Preparation')]");
        private By checklistAppraisalLabel = By.XPath("//label[contains(text(),'Appraisal')]");
        private By checklistAppraisalContent = By.XPath("//label[contains(text(),'Appraisal')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistEnvironmentalLabel = By.XPath("//label[contains(text(),'Environmental')]");
        private By checklistEnvironmentalContent = By.XPath("//label[contains(text(),'Environmental')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLandUsePlanningLabel = By.XPath("//label[contains(text(),'Land use planning')]");
        private By checklistLandUsePlanningContent = By.XPath("//label[contains(text(),'Land use planning')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLegalSurveyLabel = By.XPath("//label[contains(text(),'Legal survey')]");
        private By checklistLegalSurveyContent = By.XPath("//label[contains(text(),'Legal survey')]/parent::div/following-sibling::div/div/div[2]/span");

        //Referrals and Consultations section View Elements
        private By checklistReferralsAndConsultationsTitle = By.XPath("//h2/div/div[contains(text(),'Referrals and Consultations')]");
        private By checklistEnhancedReferralLabel = By.XPath("//label[contains(text(),'Enhanced referral (Complete/Exempt)')]");
        private By checklistEnhancedReferralContent = By.XPath("//label[contains(text(),'Enhanced referral (Complete/Exempt)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistStrengthClaimAssessmentLabel = By.XPath("//label[contains(text(),'Strength of Claim assessment')]");
        private By checklistStrengthClaimAssessmentContent = By.XPath("//label[contains(text(),'Strength of Claim assessment')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFirstNationsConsultationLabel = By.XPath("//label[contains(text(),'First Nations consultation')]");
        private By checklistFirstNationsConsultationContent = By.XPath("//label[contains(text(),'First Nations consultation')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLocalGovernmentConsultationLabel = By.XPath("//label[contains(text(),'Local Government consultation')]");
        private By checklistLocalGovernmentConsultationContent = By.XPath("//label[contains(text(),'Local Government consultation')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistUtilityCompanyReferralLabel = By.XPath("//label[contains(text(),'Utility company referral')]");
        private By checklistUtilityCompanyReferralContent = By.XPath("//label[contains(text(),'Utility company referral')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAdjacentLandownerReferralLabel = By.XPath("//label[contains(text(),'Adjacent landowner referral')]");
        private By checklistAdjacentLandownerReferralContent = By.XPath("//label[contains(text(),'Adjacent landowner referral')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAdvertisingCompletedLabel = By.XPath("//label[contains(text(),'Advertising completed')]");
        private By checklistAdvertisingCompletedContent = By.XPath("//label[contains(text(),'Advertising completed')]/parent::div/following-sibling::div/div/div[2]/span");

        //Direct Sale or Road Closure section View Elements
        private By checklistDirectSaleRoadClosureTitle = By.XPath("//h2/div/div[contains(text(),'Direct Sale or Road Closure')]");
        private By checklistSurveyInstructionsLabel = By.XPath("//label[contains(text(),'Survey instructions issued to Applicant')]");
        private By checklistSurveyInstructionsContent = By.XPath("//label[contains(text(),'Survey instructions issued to Applicant')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSubmitLegalSurveyReceivedApprovedLabel = By.XPath("//label[contains(text(),'Legal survey received and approved')]");
        private By checklistSubmitLegalSurveyReceivedApprovedContent = By.XPath("//label[contains(text(),'Legal survey received and approved')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistRoadClosurePlanCompletedLabel = By.XPath("//label[contains(text(),'Road closure plan completed')]");
        private By checklistRoadClosurePlanCompletedContent = By.XPath("//label[contains(text(),'Road closure plan completed')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistConsolidationPlanCompletedLabel = By.XPath("//label[contains(text(),'Consolidation plan completed')]");
        private By checklistConsolidationPlanCompletedContent = By.XPath("//label[contains(text(),'Consolidation plan completed')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistCopyAppraisalSentBuyerLabel = By.XPath("//label[contains(text(),'Copy of appraisal sent to buyer(s)')]");
        private By checklistCopyAppraisalSentBuyerContent = By.XPath("//label[contains(text(),'Copy of appraisal sent to buyer(s)')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistDistrictHighwayManagerLabel = By.XPath("//label[contains(text(),'Gazette notice signed by District Highway Manager')]");
        private By checklistDistrictHighwayManagerContent = By.XPath("//label[contains(text(),'Gazette notice signed by District Highway Manager')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistGazetteNoticePublishedBcLabel = By.XPath("//label[contains(text(),'Gazette notice published in BC Gazette')]");
        private By checklistGazetteNoticePublishedBcContent = By.XPath("//label[contains(text(),'Gazette notice published in BC Gazette')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSignedFormTransferLabel = By.XPath("//label[contains(text(),'Signed Form A transfer')]");
        private By checklistSignedFormTransferContent = By.XPath("//label[contains(text(),'Signed Form A transfer')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSignedTransferClosedLabel = By.XPath("//label[contains(text(),'Signed Transfer Closed PPH Between Gov. Agencies')]");
        private By checklistSignedTransferClosedContent = By.XPath("//label[contains(text(),'Signed Transfer Closed PPH Between Gov. Agencies')]/parent::div/following-sibling::div/div/div[2]/span");

        //Sale Information section View Elements
        private By checklistSaleInformationTitle = By.XPath("//h2/div/div[contains(text(),'Sale Information')]");
        private By checklistSignedListingAgreementLabel = By.XPath("//label[contains(text(),'Signed Listing agreement')]");
        private By checklistSignedListingAgreementContent = By.XPath("//label[contains(text(),'Signed Listing agreement')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLawyerAssignedAttorneyGeneralLabel = By.XPath("//label[contains(text(),'Lawyer assigned by Attorney General')]");
        private By checklistLawyerAssignedAttorneyGeneralContent = By.XPath("//label[contains(text(),'Lawyer assigned by Attorney General')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistAcceptedOfferRecordedLabel = By.XPath("//label[contains(text(),'Accepted offer recorded')]");
        private By checklistAcceptedOfferRecordedContent = By.XPath("//label[contains(text(),'Accepted offer recorded')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistLetterIntentPreparedLawyerLabel = By.XPath("//label[contains(text(),'Letter of intent prepared by the lawyer')]");
        private By checklistLetterIntentPreparedLawyerContent = By.XPath("//label[contains(text(),'Letter of intent prepared by the lawyer')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistPsaPreparedLawyerLabel = By.XPath("//label[contains(text(),'PSA prepared by the lawyer')]");
        private By checklistPsaPreparedLawyerContent = By.XPath("//label[contains(text(),'PSA prepared by the lawyer')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistPSAFullyExecutedLabel = By.XPath("//label[contains(text(),'PSA fully executed')]");
        private By checklistPSAFullyExecutedContent = By.XPath("//label[contains(text(),'PSA fully executed')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFinalConditionsRemovedLabel = By.XPath("//label[contains(text(),'Final conditions have been removed')]");
        private By checklistFinalConditionsRemovedContent = By.XPath("//label[contains(text(),'Final conditions have been removed')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistTransferplanRegisteredLTSALabel = By.XPath("//label[contains(text(),'Transfer/plan registered with LTSA')]");
        private By checklistTransferplanRegisteredLTSAContent = By.XPath("//label[contains(text(),'Transfer/plan registered with LTSA')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistSalesProceedsReceivedLabel = By.XPath("//label[contains(text(),'Sales proceeds received')]");
        private By checklistSalesProceedsReceivedContent = By.XPath("//label[contains(text(),'Sales proceeds received')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistFinancialReportingCompletedLabel = By.XPath("//label[contains(text(),'Financial reporting completed')]");
        private By checklistFinancialReportingCompletedContent = By.XPath("//label[contains(text(),'Financial reporting completed')]/parent::div/following-sibling::div/div/div[2]/span");
        private By checklistChequeSentToBctfaLabel = By.XPath("//label[contains(text(),'Cheque sent to BCTFA')]");
        private By checklistChequeSentToBctfaContent = By.XPath("//label[contains(text(),'Cheque sent to BCTFA')]/parent::div/following-sibling::div/div/div[2]/span");

        // Tool tip element on checklist
        private By checklistTooltips = By.CssSelector("span[data-testid='tooltip-icon-section-field-tooltip']");

        //Checklist Edit Mode Elements
        private By checklistFileInitiationItem1Select = By.Id("input-checklistSections[0].items[0].statusType");
        private By checklistFileInitiationItem2Select = By.Id("input-checklistSections[0].items[1].statusType");
        private By checklistFileInitiationItem3Select = By.Id("input-checklistSections[0].items[2].statusType");
        private By checklistFileInitiationItem4Select = By.Id("input-checklistSections[0].items[3].statusType");

        private By checklistDispositionPreparationItem1Select = By.Id("input-checklistSections[1].items[0].statusType");
        private By checklistDispositionPreparationItem2Select = By.Id("input-checklistSections[1].items[1].statusType");
        private By checklistDispositionPreparationItem3Select = By.Id("input-checklistSections[1].items[2].statusType");
        private By checklistDispositionPreparationItem4Select = By.Id("input-checklistSections[1].items[3].statusType");

        private By checklistReferralsAndConsultationsItem1Select = By.Id("input-checklistSections[2].items[0].statusType");
        private By checklistReferralsAndConsultationsItem2Select = By.Id("input-checklistSections[2].items[1].statusType");
        private By checklistReferralsAndConsultationsItem3Select = By.Id("input-checklistSections[2].items[2].statusType");
        private By checklistReferralsAndConsultationsItem4Select = By.Id("input-checklistSections[2].items[3].statusType");
        private By checklistReferralsAndConsultationsItem5Select = By.Id("input-checklistSections[2].items[4].statusType");
        private By checklistReferralsAndConsultationsItem6Select = By.Id("input-checklistSections[2].items[5].statusType");
        private By checklistReferralsAndConsultationsItem7Select = By.Id("input-checklistSections[2].items[6].statusType");

        private By checklistDirectSaleRoadClosureItem1Select = By.Id("input-checklistSections[3].items[0].statusType");
        private By checklistDirectSaleRoadClosureItem2Select = By.Id("input-checklistSections[3].items[1].statusType");
        private By checklistDirectSaleRoadClosureItem3Select = By.Id("input-checklistSections[3].items[2].statusType");
        private By checklistDirectSaleRoadClosureItem4Select = By.Id("input-checklistSections[3].items[3].statusType");
        private By checklistDirectSaleRoadClosureItem5Select = By.Id("input-checklistSections[3].items[4].statusType");
        private By checklistDirectSaleRoadClosureItem6Select = By.Id("input-checklistSections[3].items[5].statusType");
        private By checklistDirectSaleRoadClosureItem7Select = By.Id("input-checklistSections[3].items[6].statusType");
        private By checklistDirectSaleRoadClosureItem8Select = By.Id("input-checklistSections[3].items[7].statusType");
        private By checklistDirectSaleRoadClosureItem9Select = By.Id("input-checklistSections[3].items[8].statusType");

        private By checklistSaleInformationItem1Select = By.Id("input-checklistSections[4].items[0].statusType");
        private By checklistSaleInformationItem2Select = By.Id("input-checklistSections[4].items[1].statusType");
        private By checklistSaleInformationItem3Select = By.Id("input-checklistSections[4].items[2].statusType");
        private By checklistSaleInformationItem4Select = By.Id("input-checklistSections[4].items[3].statusType");
        private By checklistSaleInformationItem5Select = By.Id("input-checklistSections[4].items[4].statusType");
        private By checklistSaleInformationItem6Select = By.Id("input-checklistSections[4].items[5].statusType");
        private By checklistSaleInformationItem7Select = By.Id("input-checklistSections[4].items[6].statusType");
        private By checklistSaleInformationItem8Select = By.Id("input-checklistSections[4].items[7].statusType");
        private By checklistSaleInformationItem9Select = By.Id("input-checklistSections[4].items[8].statusType");
        private By checklistSaleInformationItem10Select = By.Id("input-checklistSections[4].items[9].statusType");
        private By checklistSaleInformationItem11Select = By.Id("input-checklistSections[4].items[10].statusType");

        private SharedModals sharedModals;

        public DispositionChecklist(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

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

        public void SaveDispositionFileChecklist()
        {
            Wait();
            ButtonElement("Save");

            AssertTrueIsDisplayed(checklistEditBttn);
        }

        public void CancelDispositionFileChecklist()
        {
            Wait();
            ButtonElement("Cancel");

            sharedModals.CancelActionModal();
        }

        public void VerifyChecklistInitViewForm()
        {
            Wait(2000);

            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistCurrentTitleLabel);
            AssertTrueIsDisplayed(checklistCurrentTitleContent);
            AssertTrueIsDisplayed(checklistBCAssessmentLabel);
            AssertTrueIsDisplayed(checklistBCAssessmentContent);
            AssertTrueIsDisplayed(checklistInitiatingDocumentLabel);
            AssertTrueIsDisplayed(checklistInitiatingDocumentContent);
            AssertTrueIsDisplayed(checklistProofOfApplicationFeeLabel);
            AssertTrueIsDisplayed(checklistProofOfApplicationFeeContent);

            AssertTrueIsDisplayed(checklistDispositionPreparationTitle);
            AssertTrueIsDisplayed(checklistAppraisalLabel);
            AssertTrueIsDisplayed(checklistAppraisalContent);
            AssertTrueIsDisplayed(checklistEnvironmentalLabel);
            AssertTrueIsDisplayed(checklistEnvironmentalContent);
            AssertTrueIsDisplayed(checklistLandUsePlanningLabel);
            AssertTrueIsDisplayed(checklistLandUsePlanningContent);
            AssertTrueIsDisplayed(checklistLegalSurveyLabel);
            AssertTrueIsDisplayed(checklistLegalSurveyContent);

            AssertTrueIsDisplayed(checklistReferralsAndConsultationsTitle);
            AssertTrueIsDisplayed(checklistEnhancedReferralLabel);
            AssertTrueIsDisplayed(checklistEnhancedReferralContent);
            AssertTrueIsDisplayed(checklistStrengthClaimAssessmentLabel);
            AssertTrueIsDisplayed(checklistStrengthClaimAssessmentContent);
            AssertTrueIsDisplayed(checklistFirstNationsConsultationLabel);
            AssertTrueIsDisplayed(checklistFirstNationsConsultationContent);
            AssertTrueIsDisplayed(checklistLocalGovernmentConsultationLabel);
            AssertTrueIsDisplayed(checklistLocalGovernmentConsultationContent);
            AssertTrueIsDisplayed(checklistUtilityCompanyReferralLabel);
            AssertTrueIsDisplayed(checklistUtilityCompanyReferralContent);
            AssertTrueIsDisplayed(checklistAdjacentLandownerReferralLabel);
            AssertTrueIsDisplayed(checklistAdjacentLandownerReferralContent);
            AssertTrueIsDisplayed(checklistAdvertisingCompletedLabel);
            AssertTrueIsDisplayed(checklistAdvertisingCompletedContent);

            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureTitle);
            AssertTrueIsDisplayed(checklistSurveyInstructionsLabel);
            AssertTrueIsDisplayed(checklistSurveyInstructionsContent);
            AssertTrueIsDisplayed(checklistSubmitLegalSurveyReceivedApprovedLabel);
            AssertTrueIsDisplayed(checklistSubmitLegalSurveyReceivedApprovedContent);
            AssertTrueIsDisplayed(checklistRoadClosurePlanCompletedLabel);
            AssertTrueIsDisplayed(checklistRoadClosurePlanCompletedContent);
            AssertTrueIsDisplayed(checklistConsolidationPlanCompletedLabel);
            AssertTrueIsDisplayed(checklistConsolidationPlanCompletedContent);
            AssertTrueIsDisplayed(checklistCopyAppraisalSentBuyerLabel);
            AssertTrueIsDisplayed(checklistCopyAppraisalSentBuyerContent);
            AssertTrueIsDisplayed(checklistDistrictHighwayManagerLabel);
            AssertTrueIsDisplayed(checklistDistrictHighwayManagerContent);
            AssertTrueIsDisplayed(checklistGazetteNoticePublishedBcLabel);
            AssertTrueIsDisplayed(checklistGazetteNoticePublishedBcContent);
            AssertTrueIsDisplayed(checklistSignedFormTransferLabel);
            AssertTrueIsDisplayed(checklistSignedFormTransferContent);
            AssertTrueIsDisplayed(checklistSignedTransferClosedLabel);
            AssertTrueIsDisplayed(checklistSignedTransferClosedContent);

            AssertTrueIsDisplayed(checklistSaleInformationTitle);
            AssertTrueIsDisplayed(checklistSignedListingAgreementLabel);
            AssertTrueIsDisplayed(checklistSignedListingAgreementContent);
            AssertTrueIsDisplayed(checklistLawyerAssignedAttorneyGeneralLabel);
            AssertTrueIsDisplayed(checklistLawyerAssignedAttorneyGeneralContent);
            AssertTrueIsDisplayed(checklistAcceptedOfferRecordedLabel);
            AssertTrueIsDisplayed(checklistAcceptedOfferRecordedContent);
            AssertTrueIsDisplayed(checklistLetterIntentPreparedLawyerLabel);
            AssertTrueIsDisplayed(checklistLetterIntentPreparedLawyerContent);
            AssertTrueIsDisplayed(checklistPsaPreparedLawyerLabel);
            AssertTrueIsDisplayed(checklistPsaPreparedLawyerContent);
            AssertTrueIsDisplayed(checklistPSAFullyExecutedLabel);
            AssertTrueIsDisplayed(checklistPSAFullyExecutedContent);
            AssertTrueIsDisplayed(checklistFinalConditionsRemovedLabel);
            AssertTrueIsDisplayed(checklistFinalConditionsRemovedContent);
            AssertTrueIsDisplayed(checklistTransferplanRegisteredLTSALabel);
            AssertTrueIsDisplayed(checklistTransferplanRegisteredLTSAContent);
            AssertTrueIsDisplayed(checklistSalesProceedsReceivedLabel);
            AssertTrueIsDisplayed(checklistSalesProceedsReceivedContent);
            AssertTrueIsDisplayed(checklistFinancialReportingCompletedLabel);
            AssertTrueIsDisplayed(checklistFinancialReportingCompletedContent);
            AssertTrueIsDisplayed(checklistChequeSentToBctfaLabel);
            AssertTrueIsDisplayed(checklistChequeSentToBctfaContent);
        }

        public void VerifyChecklistEditForm()
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistCurrentTitleLabel);
            AssertTrueIsDisplayed(checklistFileInitiationItem1Select);
            AssertTrueIsDisplayed(checklistBCAssessmentLabel);
            AssertTrueIsDisplayed(checklistFileInitiationItem2Select);
            AssertTrueIsDisplayed(checklistInitiatingDocumentLabel);
            AssertTrueIsDisplayed(checklistFileInitiationItem3Select);
            AssertTrueIsDisplayed(checklistProofOfApplicationFeeLabel);
            AssertTrueIsDisplayed(checklistFileInitiationItem4Select);


            AssertTrueIsDisplayed(checklistDispositionPreparationTitle);
            AssertTrueIsDisplayed(checklistAppraisalLabel);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem1Select);
            AssertTrueIsDisplayed(checklistEnvironmentalLabel);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem2Select);
            AssertTrueIsDisplayed(checklistLandUsePlanningLabel);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem3Select);
            AssertTrueIsDisplayed(checklistLegalSurveyLabel);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem4Select);


            AssertTrueIsDisplayed(checklistReferralsAndConsultationsTitle);
            AssertTrueIsDisplayed(checklistEnhancedReferralLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem1Select);
            AssertTrueIsDisplayed(checklistStrengthClaimAssessmentLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem2Select);
            AssertTrueIsDisplayed(checklistFirstNationsConsultationLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem3Select);
            AssertTrueIsDisplayed(checklistLocalGovernmentConsultationLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem4Select);
            AssertTrueIsDisplayed(checklistUtilityCompanyReferralLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem5Select);
            AssertTrueIsDisplayed(checklistAdjacentLandownerReferralLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem6Select);
            AssertTrueIsDisplayed(checklistAdvertisingCompletedLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem7Select);

            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureTitle);
            AssertTrueIsDisplayed(checklistSurveyInstructionsLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem1Select);
            AssertTrueIsDisplayed(checklistSubmitLegalSurveyReceivedApprovedLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem2Select);
            AssertTrueIsDisplayed(checklistRoadClosurePlanCompletedLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem3Select);
            AssertTrueIsDisplayed(checklistConsolidationPlanCompletedLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem4Select);
            AssertTrueIsDisplayed(checklistCopyAppraisalSentBuyerLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem5Select);
            AssertTrueIsDisplayed(checklistDistrictHighwayManagerLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem6Select);
            AssertTrueIsDisplayed(checklistGazetteNoticePublishedBcLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem7Select);
            AssertTrueIsDisplayed(checklistSignedFormTransferLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem8Select);
            AssertTrueIsDisplayed(checklistSignedTransferClosedLabel);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem9Select);

            AssertTrueIsDisplayed(checklistSaleInformationTitle);
            AssertTrueIsDisplayed(checklistSignedListingAgreementLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem1Select);
            AssertTrueIsDisplayed(checklistLawyerAssignedAttorneyGeneralLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem2Select);
            AssertTrueIsDisplayed(checklistAcceptedOfferRecordedLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem3Select);
            AssertTrueIsDisplayed(checklistLetterIntentPreparedLawyerLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem4Select);
            AssertTrueIsDisplayed(checklistPsaPreparedLawyerLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem5Select);
            AssertTrueIsDisplayed(checklistPSAFullyExecutedLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem6Select);
            AssertTrueIsDisplayed(checklistFinalConditionsRemovedLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem7Select);
            AssertTrueIsDisplayed(checklistTransferplanRegisteredLTSALabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem8Select);
            AssertTrueIsDisplayed(checklistSalesProceedsReceivedLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem9Select);
            AssertTrueIsDisplayed(checklistFinancialReportingCompletedLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem10Select);
            AssertTrueIsDisplayed(checklistChequeSentToBctfaLabel);
            AssertTrueIsDisplayed(checklistSaleInformationItem11Select);
        }

        public void VerifyChecklistViewForm(DispositionFileChecklist checklist)
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistCurrentTitleLabel);
            AssertTrueContentEquals(checklistCurrentTitleContent, checklist.FileInitiationSelect1);
            AssertTrueIsDisplayed(checklistBCAssessmentLabel);
            AssertTrueContentEquals(checklistBCAssessmentContent, checklist.FileInitiationSelect2);
            AssertTrueIsDisplayed(checklistInitiatingDocumentLabel);
            AssertTrueContentEquals(checklistInitiatingDocumentContent, checklist.FileInitiationSelect3);
            AssertTrueIsDisplayed(checklistProofOfApplicationFeeLabel);
            AssertTrueContentEquals(checklistProofOfApplicationFeeContent, checklist.FileInitiationSelect4);

            AssertTrueIsDisplayed(checklistDispositionPreparationTitle);
            AssertTrueIsDisplayed(checklistAppraisalLabel);
            AssertTrueContentEquals(checklistAppraisalContent, checklist.DispositionPreparationSelect1);
            AssertTrueIsDisplayed(checklistEnvironmentalLabel);
            AssertTrueContentEquals(checklistEnvironmentalContent, checklist.DispositionPreparationSelect2);
            AssertTrueIsDisplayed(checklistLandUsePlanningLabel);
            AssertTrueContentEquals(checklistLandUsePlanningContent, checklist.DispositionPreparationSelect3);
            AssertTrueIsDisplayed(checklistLegalSurveyLabel);
            AssertTrueContentEquals(checklistLegalSurveyContent, checklist.DispositionPreparationSelect4);

            AssertTrueIsDisplayed(checklistReferralsAndConsultationsTitle);
            AssertTrueIsDisplayed(checklistEnhancedReferralLabel);
            AssertTrueContentEquals(checklistEnhancedReferralContent, checklist.ReferralsAndConsultationsSelect1);
            AssertTrueIsDisplayed(checklistStrengthClaimAssessmentLabel);
            AssertTrueContentEquals(checklistStrengthClaimAssessmentContent, checklist.ReferralsAndConsultationsSelect2);
            AssertTrueIsDisplayed(checklistFirstNationsConsultationLabel);
            AssertTrueContentEquals(checklistFirstNationsConsultationContent, checklist.ReferralsAndConsultationsSelect3);
            AssertTrueIsDisplayed(checklistLocalGovernmentConsultationLabel);
            AssertTrueContentEquals(checklistLocalGovernmentConsultationContent, checklist.ReferralsAndConsultationsSelect4);
            AssertTrueIsDisplayed(checklistUtilityCompanyReferralLabel);
            AssertTrueContentEquals(checklistUtilityCompanyReferralContent, checklist.ReferralsAndConsultationsSelect5);
            AssertTrueIsDisplayed(checklistAdjacentLandownerReferralLabel);
            AssertTrueContentEquals(checklistAdjacentLandownerReferralContent, checklist.ReferralsAndConsultationsSelect6);
            AssertTrueIsDisplayed(checklistAdvertisingCompletedLabel);
            AssertTrueContentEquals(checklistAdvertisingCompletedContent, checklist.ReferralsAndConsultationsSelect7);

            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureTitle);
            AssertTrueIsDisplayed(checklistSurveyInstructionsLabel);
            AssertTrueContentEquals(checklistSurveyInstructionsContent, checklist.DirectSaleRoadClosureSelect1);
            AssertTrueIsDisplayed(checklistSubmitLegalSurveyReceivedApprovedLabel);
            AssertTrueContentEquals(checklistSubmitLegalSurveyReceivedApprovedContent, checklist.DirectSaleRoadClosureSelect2);
            AssertTrueIsDisplayed(checklistRoadClosurePlanCompletedLabel);
            AssertTrueContentEquals(checklistRoadClosurePlanCompletedContent, checklist.DirectSaleRoadClosureSelect3);
            AssertTrueIsDisplayed(checklistConsolidationPlanCompletedLabel);
            AssertTrueContentEquals(checklistConsolidationPlanCompletedContent, checklist.DirectSaleRoadClosureSelect4);
            AssertTrueIsDisplayed(checklistCopyAppraisalSentBuyerLabel);
            AssertTrueContentEquals(checklistCopyAppraisalSentBuyerContent, checklist.DirectSaleRoadClosureSelect5);
            AssertTrueIsDisplayed(checklistDistrictHighwayManagerLabel);
            AssertTrueContentEquals(checklistDistrictHighwayManagerContent, checklist.DirectSaleRoadClosureSelect6);
            AssertTrueIsDisplayed(checklistGazetteNoticePublishedBcLabel);
            AssertTrueContentEquals(checklistGazetteNoticePublishedBcContent, checklist.DirectSaleRoadClosureSelect7);
            AssertTrueIsDisplayed(checklistSignedFormTransferLabel);
            AssertTrueContentEquals(checklistSignedFormTransferContent, checklist.DirectSaleRoadClosureSelect8);
            AssertTrueIsDisplayed(checklistSignedTransferClosedLabel);
            AssertTrueContentEquals(checklistSignedTransferClosedContent, checklist.DirectSaleRoadClosureSelect9);

            AssertTrueIsDisplayed(checklistSaleInformationTitle);
            AssertTrueIsDisplayed(checklistSignedListingAgreementLabel);
            AssertTrueContentEquals(checklistSignedListingAgreementContent, checklist.SaleInformationSelect1);
            AssertTrueIsDisplayed(checklistLawyerAssignedAttorneyGeneralLabel);
            AssertTrueContentEquals(checklistLawyerAssignedAttorneyGeneralContent, checklist.SaleInformationSelect2);
            AssertTrueIsDisplayed(checklistAcceptedOfferRecordedLabel);
            AssertTrueContentEquals(checklistAcceptedOfferRecordedContent, checklist.SaleInformationSelect3);
            AssertTrueIsDisplayed(checklistLetterIntentPreparedLawyerLabel);
            AssertTrueContentEquals(checklistLetterIntentPreparedLawyerContent, checklist.SaleInformationSelect4);
            AssertTrueIsDisplayed(checklistPsaPreparedLawyerLabel);
            AssertTrueContentEquals(checklistPsaPreparedLawyerContent, checklist.SaleInformationSelect5);
            AssertTrueIsDisplayed(checklistPSAFullyExecutedLabel);
            AssertTrueContentEquals(checklistPSAFullyExecutedContent, checklist.SaleInformationSelect6);
            AssertTrueIsDisplayed(checklistFinalConditionsRemovedLabel);
            AssertTrueContentEquals(checklistFinalConditionsRemovedContent, checklist.SaleInformationSelect7);
            AssertTrueIsDisplayed(checklistTransferplanRegisteredLTSALabel);
            AssertTrueContentEquals(checklistTransferplanRegisteredLTSAContent, checklist.SaleInformationSelect8);
            AssertTrueIsDisplayed(checklistSalesProceedsReceivedLabel);
            AssertTrueContentEquals(checklistSalesProceedsReceivedContent, checklist.SaleInformationSelect9);
            AssertTrueIsDisplayed(checklistFinancialReportingCompletedLabel);
            AssertTrueContentEquals(checklistFinancialReportingCompletedContent, checklist.SaleInformationSelect10);
            AssertTrueIsDisplayed(checklistChequeSentToBctfaLabel);
            AssertTrueContentEquals(checklistChequeSentToBctfaContent, checklist.SaleInformationSelect11);
        }

        public void UpdateChecklist(DispositionFileChecklist checklist)
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

            if (checklist.DispositionPreparationSelect1 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem1Select, checklist.DispositionPreparationSelect1);
            if (checklist.DispositionPreparationSelect2 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem2Select, checklist.DispositionPreparationSelect2);
            if (checklist.DispositionPreparationSelect3 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem3Select, checklist.DispositionPreparationSelect3);
            if (checklist.DispositionPreparationSelect4 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem4Select, checklist.DispositionPreparationSelect4);

            if (checklist.ReferralsAndConsultationsSelect1 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem1Select, checklist.ReferralsAndConsultationsSelect1);
            if (checklist.ReferralsAndConsultationsSelect2 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem2Select, checklist.ReferralsAndConsultationsSelect2);
            if (checklist.ReferralsAndConsultationsSelect3 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem3Select, checklist.ReferralsAndConsultationsSelect3);
            if (checklist.ReferralsAndConsultationsSelect4 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem4Select, checklist.ReferralsAndConsultationsSelect4);
            if (checklist.ReferralsAndConsultationsSelect5 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem5Select, checklist.ReferralsAndConsultationsSelect5);
            if (checklist.ReferralsAndConsultationsSelect6 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem6Select, checklist.ReferralsAndConsultationsSelect6);
            if (checklist.ReferralsAndConsultationsSelect7 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem7Select, checklist.ReferralsAndConsultationsSelect7);

            if (checklist.DirectSaleRoadClosureSelect1 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem1Select, checklist.DirectSaleRoadClosureSelect1);
            if (checklist.DirectSaleRoadClosureSelect2 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem2Select, checklist.DirectSaleRoadClosureSelect2);
            if (checklist.DirectSaleRoadClosureSelect3 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem3Select, checklist.DirectSaleRoadClosureSelect3);
            if (checklist.DirectSaleRoadClosureSelect4 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem4Select, checklist.DirectSaleRoadClosureSelect4);
            if (checklist.DirectSaleRoadClosureSelect5 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem5Select, checklist.DirectSaleRoadClosureSelect5);
            if (checklist.DirectSaleRoadClosureSelect6 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem6Select, checklist.DirectSaleRoadClosureSelect6);
            if (checklist.DirectSaleRoadClosureSelect7 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem7Select, checklist.DirectSaleRoadClosureSelect7);
            if (checklist.DirectSaleRoadClosureSelect8 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem8Select, checklist.DirectSaleRoadClosureSelect8);
            if (checklist.DirectSaleRoadClosureSelect9 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem9Select, checklist.DirectSaleRoadClosureSelect9);

            if (checklist.SaleInformationSelect1 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem1Select, checklist.SaleInformationSelect1);
            if (checklist.SaleInformationSelect2 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem2Select, checklist.SaleInformationSelect2);
            if (checklist.SaleInformationSelect3 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem3Select, checklist.SaleInformationSelect3);
            if (checklist.SaleInformationSelect4 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem4Select, checklist.SaleInformationSelect4);
            if (checklist.SaleInformationSelect5 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem5Select, checklist.SaleInformationSelect5);
            if (checklist.SaleInformationSelect6 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem6Select, checklist.SaleInformationSelect6);
            if (checklist.SaleInformationSelect7 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem7Select, checklist.SaleInformationSelect7);
            if (checklist.SaleInformationSelect8 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem8Select, checklist.SaleInformationSelect8);
            if (checklist.SaleInformationSelect9 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem9Select, checklist.SaleInformationSelect9);
            if (checklist.SaleInformationSelect10 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem10Select, checklist.SaleInformationSelect10);
            if (checklist.SaleInformationSelect11 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem11Select, checklist.SaleInformationSelect11);
        }
    }
}
