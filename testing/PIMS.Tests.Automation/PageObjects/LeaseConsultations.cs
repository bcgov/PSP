using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseConsultations : PageObjectBase
    {
        //Consultations Menu Elements
        private readonly By consultationInsuranceLink = By.XPath("//a[contains(text(),'Approval/Consultations')]");

        //Consultation Init Form Elements
        private readonly By consultationTitle = By.XPath("//div[contains(text(),'Approval / Consultations')]");
        private readonly By consultationAddButton = By.XPath("//div[contains(text(),'Approval / Consultations')]/following-sibling::div/button");
        private readonly By consultationDistrictSubtitle = By.XPath("//span[contains(text(),'District')]");
        private readonly By consultationEngineeringSubtitle = By.XPath("//span[contains(text(),'Engineering')]");
        private readonly By consultationFirstNationSubtitle = By.XPath("//span[contains(text(),'First Nation')]");
        private readonly By consultationHeadquarterSubtitle = By.XPath("//span[contains(text(),'Headquarter (HQ)')]");
        private readonly By consultationRegionalPlanningSubtitle = By.XPath("//span[contains(text(),'Regional planning')]");
        private readonly By consultationRegionalPropServicesSubtitle = By.XPath("//span[contains(text(),'Regional property services')]");
        private readonly By consultationSRESubtitle = By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]");
        private readonly By consultationOtherSubtitle = By.XPath("//span[contains(text(),'Other')]");
        private readonly By consultationInitialState = By.XPath("//p[contains(text(),'There are no approvals / consultations.')]");

        //Consultations Create Form Elements
        private readonly By consultationsAddTitle = By.XPath("//div[contains(text(),'Add Approval / Consultation')]");
        private readonly By consultationTypeLabel = By.XPath("//label[contains(text(),'Approval / Consultation type')]");
        private readonly By consultationTypeTooltip = By.CssSelector("span[date-testid='tooltip-icon-lease-consultation-type-tooltip']");
        private readonly By consultationTypeSelect = By.Id("input-consultationTypeCode");
        private readonly By consultationRequestedOnLabel = By.XPath("//label[contains(text(),'Requested on')]");
        private readonly By consultationRequestedOnTooltip = By.CssSelector("span[date-testid='tooltip-icon-lease-consultation-requestedon-tooltip']");
        private readonly By consultationRequestedOnSelect = By.Id("datepicker-requestedOn");
        private readonly By consultationContactLabel = By.XPath("//label[contains(text(),'Contact')]");
        private readonly By consultationContactTooltip = By.CssSelector("span[date-testid='tooltip-icon-lease-consultation-contact-tooltip']");
        private readonly By consultationContactTypeBttn = By.CssSelector("button[title='Select Contact']");
        private readonly By consultationResponseReceivedLabel = By.XPath("//label[contains(text(),'Response received')]");
        private readonly By consultationResponseReceivedSelect = By.Id("input-isResponseReceived");
        private readonly By consultationOutcomeLabel = By.XPath("//label[contains(text(),'Outcome')]");
        private readonly By consultationOutcomeTooltip = By.CssSelector("span[date-testid='tooltip-icon-lease-consultation-outcome-type-tooltip']");
        private readonly By consultationOutcomeSelect = By.Id("input-consultationOutcomeTypeCode");
        private readonly By consultationCommentsLabel = By.XPath("//label[contains(text(),'Comments')]");
        private readonly By consultationCommentsTooltip = By.CssSelector("span[date-testid='tooltip-icon-lease-consultation-comments-tooltip']");
        private readonly By consultationCommentsTextarea = By.Id("input-comment");


        SharedModals sharedModals;


        public LeaseConsultations(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToConsultationsSection()
        {
            Wait();
            webDriver.FindElement(consultationInsuranceLink).Click();
        }

        public void AddConsultationBttn()
        {
            WaitUntilClickable(consultationAddButton);
            FocusAndClick(consultationAddButton);
        }

        public void CancelConsultations()
        {
            sharedModals.ModalClickCancelBttn();
        }

        public void VerifyInitConsultationTab()
        {
            AssertTrueIsDisplayed(consultationTitle);
            AssertTrueIsDisplayed(consultationAddButton);
            AssertTrueIsDisplayed(consultationDistrictSubtitle);
            AssertTrueIsDisplayed(consultationEngineeringSubtitle);
            AssertTrueIsDisplayed(consultationFirstNationSubtitle);
            AssertTrueIsDisplayed(consultationHeadquarterSubtitle);
            AssertTrueIsDisplayed(consultationRegionalPlanningSubtitle);
            AssertTrueIsDisplayed(consultationRegionalPropServicesSubtitle);
            AssertTrueIsDisplayed(consultationSRESubtitle);
            AssertTrueIsDisplayed(consultationOtherSubtitle);
            Assert.Equal(8, webDriver.FindElements(consultationInitialState).Count);
        }

        public void VerifyConsultationCreateForm()
        {
            AssertTrueIsDisplayed(consultationsAddTitle);
            AssertTrueIsDisplayed(consultationTypeLabel);
            AssertTrueIsDisplayed(consultationTypeTooltip);
            AssertTrueIsDisplayed(consultationTypeSelect);
            AssertTrueIsDisplayed(consultationRequestedOnLabel);
            AssertTrueIsDisplayed(consultationRequestedOnTooltip);
            AssertTrueIsDisplayed(consultationRequestedOnSelect);
            AssertTrueIsDisplayed(consultationContactLabel);
            AssertTrueIsDisplayed(consultationContactTooltip);
            AssertTrueIsDisplayed(consultationContactTypeBttn);
            AssertTrueIsDisplayed(consultationResponseReceivedLabel);
            AssertTrueIsDisplayed(consultationResponseReceivedSelect);
            AssertTrueIsDisplayed(consultationOutcomeLabel);
            AssertTrueIsDisplayed(consultationOutcomeTooltip);
            AssertTrueIsDisplayed(consultationOutcomeSelect);
            AssertTrueIsDisplayed(consultationCommentsLabel);
            AssertTrueIsDisplayed(consultationCommentsTooltip);
            AssertTrueIsDisplayed(consultationCommentsTextarea);
        }

    }
}
