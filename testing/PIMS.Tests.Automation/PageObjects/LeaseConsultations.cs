using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseConsultations : PageObjectBase
    {
        //Consultations Menu Elements
        private readonly By consultationLink = By.XPath("//a[contains(text(),'Approval/Consultations')]");

        //Consultation Init Form Elements
        private readonly By consultationTitle = By.XPath("//div[contains(text(),'Approval / Consultations')]");
        private readonly By consultationAddButton = By.XPath("//div[contains(text(),'Approval / Consultations')]/following-sibling::div/button");
        private readonly By consultationDistrictSubtitle = By.XPath("//span[contains(text(),'District')]");
        private readonly By consultationDistrictExpandBttn = By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/following-sibling::div/*");
        private readonly By consultationEngineeringSubtitle = By.XPath("//span[contains(text(),'Engineering')]");
        private readonly By consultationEngineeringExpandBttn = By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/following-sibling::div/*");
        private readonly By consultationFirstNationSubtitle = By.XPath("//span[contains(text(),'First Nation')]");
        private readonly By consultationFirstNationExpandBttn = By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/following-sibling::div/*");
        private readonly By consultationHeadquarterSubtitle = By.XPath("//span[contains(text(),'Headquarter (HQ)')]");
        private readonly By consultationHeadquarterExpandBttn = By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/following-sibling::div/*");
        private readonly By consultationRegionalPlanningSubtitle = By.XPath("//span[contains(text(),'Regional Planning')]");
        private readonly By consultationRegionalPlanningExpandBttn = By.XPath("//span[contains(text(),'Regional Planning')]/parent::div/parent::div/parent::div/following-sibling::div/*");
        private readonly By consultationRegionalPropServicesSubtitle = By.XPath("//span[contains(text(),'Regional Property Services')]");
        private readonly By consultationRegionalPropServicesExpandBttn = By.XPath("//span[contains(text(),'Regional Property Services')]/parent::div/parent::div/parent::div/following-sibling::div/*");
        private readonly By consultationSRESubtitle = By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]");
        private readonly By consultationSREExpandBttn = By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/following-sibling::div/*");
        private readonly By consultationOtherSubtitle = By.XPath("//span[contains(text(),'Other')]");
        private readonly By consultationOtherExpandBttn = By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/following-sibling::div/*");

        private readonly By consultationInitialState = By.XPath("//p[contains(text(),'There are no approvals / consultations.')]");

        //Consultations Create Form Elements
        private readonly By consultationsAddTitle = By.XPath("//div[contains(text(),'Add Approval / Consultation')]");
        private readonly By consultationTypeLabel = By.XPath("//label[contains(text(),'Approval / Consultation type')]");
        private readonly By consultationTypeTooltip = By.CssSelector("span[data-testid='tooltip-icon-lease-consultation-type-tooltip']");
        private readonly By consultationTypeSelect = By.Id("input-consultationTypeCode");
        private readonly By consultationOtherDescriptionLabel = By.XPath("//label[contains(text(),'Description')]");
        private readonly By consultationOtherDescriptionTooltip = By.CssSelector("span[data-testid='tooltip-icon-lease-consultation-otherdescription-tooltip']");
        private readonly By consultationOtherDescriptionInput = By.Id("input-otherDescription");
        private readonly By consultationRequestedOnLabel = By.XPath("//label[contains(text(),'Requested on')]");
        private readonly By consultationRequestedOnTooltip = By.CssSelector("span[data-testid='tooltip-icon-lease-consultation-requestedon-tooltip']");
        private readonly By consultationRequestedOnDate = By.Id("datepicker-requestedOn");
        private readonly By consultationContactLabel = By.XPath("//label[contains(text(),'Contact')]");
        private readonly By consultationContactTooltip = By.CssSelector("span[data-testid='tooltip-icon-lease-consultation-contact-tooltip']");
        private readonly By consultationContactPrimaryLabel = By.XPath("//label[contains(text(),'Primary contact')]");
        private readonly By consultationContactPrimarySelect = By.Id("input-primaryContactId");
        private readonly By consultationContactTypeBttn = By.CssSelector("button[title='Select Contact']");
        private readonly By consultationResponseReceivedLabel = By.XPath("//label[contains(text(),'Response received')]");
        private readonly By consultationResponseReceivedSelect = By.Id("input-isResponseReceived");
        private readonly By consultationReceivedOnLabel = By.XPath("//label[contains(text(),'Response received on')]");
        private readonly By consultationReceivedOnDate = By.Id("datepicker-responseReceivedDate");
        private readonly By consultationOutcomeLabel = By.XPath("//label[contains(text(),'Outcome')]");
        private readonly By consultationOutcomeTooltip = By.CssSelector("span[data-testid='tooltip-icon-lease-consultation-outcome-type-tooltip']");
        private readonly By consultationOutcomeSelect = By.Id("input-consultationOutcomeTypeCode");
        private readonly By consultationCommentsLabel = By.XPath("//label[contains(text(),'Comments')]");
        private readonly By consultationCommentsTooltip = By.CssSelector("span[data-testid='tooltip-icon-lease-consultation-comments-tooltip']");
        private readonly By consultationCommentsTextarea = By.Id("input-comment");

        //Consultation Types Counting Elements
        private readonly By consultationDistrictCount = By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private readonly By consultationEngineeringCount = By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private readonly By consultationFirstNationCount = By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private readonly By consultationHeadquarterCount = By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private readonly By consultationRegionalPlanningCount = By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private readonly By consultationRegionalPropServicesCount = By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private readonly By consultationSRECount = By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private readonly By consultationOtherCount = By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");

        //Leases Modal Element
        private readonly By licenseConsultationConfirmationModal = By.CssSelector("div[class='modal-content']");

        SharedModals sharedModals;
        private SharedSelectContact sharedSelectContact;

        public LeaseConsultations(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
        }

        public void NavigateToConsultationsTab()
        {
            Wait();
            webDriver.FindElement(consultationLink).Click();
        }

        public void AddConsultationBttn()
        {
            WaitUntilClickable(consultationAddButton);
            FocusAndClick(consultationAddButton);
        }

        public void AddUpdateConsultation(LeaseConsultation consultation)
        {
            Wait();

            ChooseSpecificSelectOption(consultationTypeSelect, consultation.leaseConsultationType);

            if (consultation.leaseConsultationType == "Other")
            {
                System.Diagnostics.Debug.WriteLine("OTHER: " + consultation.leaseConsultationOtherDescription);
                AssertTrueIsDisplayed(consultationOtherDescriptionLabel);
                AssertTrueIsDisplayed(consultationOtherDescriptionTooltip);
                ClearInput(consultationOtherDescriptionInput);
                webDriver.FindElement(consultationOtherDescriptionInput).SendKeys(consultation.leaseConsultationOtherDescription);
                webDriver.FindElement(consultationOtherDescriptionInput).SendKeys(Keys.Enter);
            }
            if (consultation.leaseConsultationRequestedOn != "")
            {
                ClearInput(consultationRequestedOnDate);
                webDriver.FindElement(consultationRequestedOnDate).SendKeys(consultation.leaseConsultationRequestedOn);
                webDriver.FindElement(consultationRequestedOnDate).SendKeys(Keys.Enter);
            }
            if (consultation.leaseConsultationContact != "")
            {
                webDriver.FindElement(consultationContactTypeBttn).Click();
                sharedSelectContact.SelectContact(consultation.leaseConsultationContact, consultation.leaseConsultationContactType);
            }
            if (consultation.leaseConsultationContactPrimaryContact != "")
            {
                AssertTrueIsDisplayed(consultationContactPrimaryLabel);
                ChooseSpecificSelectOption(consultationContactPrimarySelect, consultation.leaseConsultationContactPrimaryContact);
            }
            ChooseSpecificSelectOption(consultationResponseReceivedSelect, consultation.leaseConsultationReceived);
            if (consultation.leaseConsultationReceivedOn != "")
            {
                AssertTrueIsDisplayed(consultationReceivedOnLabel);

                ClearInput(consultationReceivedOnDate);
                webDriver.FindElement(consultationReceivedOnDate).SendKeys(consultation.leaseConsultationReceivedOn);
                webDriver.FindElement(consultationReceivedOnDate).SendKeys(Keys.Enter);
            }
            ChooseSpecificSelectOption(consultationOutcomeSelect, consultation.leaseConsultationOutcome);
            if (consultation.leaseConsultationComment != "")
            {
                ClearInput(consultationCommentsTextarea);
                webDriver.FindElement(consultationCommentsTextarea).SendKeys(consultation.leaseConsultationComment);
            }
        }

        public void EditLastConsultationByType(string consultationType)
        {
            Wait();
            switch (consultationType)
            {
                case "District":
                    ButtonElement(consultationDistrictExpandBttn);
                    Wait();
                    int lastDistrictConsultation = webDriver.FindElements(consultationDistrictCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
                case "Engineering":
                    ButtonElement(consultationEngineeringExpandBttn);
                    Wait();
                    int lastEngineerConsultation = webDriver.FindElements(consultationEngineeringCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
                case "First Nation":
                    ButtonElement(consultationFirstNationExpandBttn);
                    Wait();
                    int lastFirstNationConsultation = webDriver.FindElements(consultationFirstNationCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
                case "Headquarter (HQ)":
                    ButtonElement(consultationHeadquarterExpandBttn);
                    Wait();
                    int lastHeadquarterConsultation = webDriver.FindElements(consultationHeadquarterCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
                case "Regional planning":
                    ButtonElement(consultationRegionalPlanningExpandBttn);
                    Wait();
                    int lastRegionalPlanningConsultation = webDriver.FindElements(consultationRegionalPlanningCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
                case "Regional property services":
                    ButtonElement(consultationRegionalPropServicesExpandBttn);
                    Wait();
                    int lastRegionalPropServicesConsultation = webDriver.FindElements(consultationRegionalPropServicesCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
                case "Strategic Real Estate (SRE)":
                    ButtonElement(consultationSREExpandBttn);
                    Wait();
                    int lastSREConsultation = webDriver.FindElements(consultationSRECount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
                case "Other":
                    ButtonElement(consultationOtherExpandBttn);
                    Wait();
                    int lastOtherConsultation = webDriver.FindElements(consultationOtherCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/h2/div/div/div/div/div/div/button[@title='Edit Consultation']")).Click();
                    break;
            }
        }

        public void DeleteLastConsultationByType(string consultationType)
        {
            Wait();
            switch (consultationType)
            {
                case "District":
                    ButtonElement(consultationDistrictExpandBttn);
                    int lastDistrictConsultation = webDriver.FindElements(consultationDistrictCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
                case "Engineering":
                    ButtonElement(consultationEngineeringExpandBttn);
                    int lastEngineerConsultation = webDriver.FindElements(consultationEngineeringCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
                case "First Nation":
                    ButtonElement(consultationFirstNationExpandBttn);
                    int lastFirstNationConsultation = webDriver.FindElements(consultationFirstNationCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
                case "Headquarter (HQ)":
                    ButtonElement(consultationHeadquarterExpandBttn);
                    int lastHeadquarterConsultation = webDriver.FindElements(consultationHeadquarterCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
                case "Regional planning":
                    ButtonElement(consultationRegionalPlanningExpandBttn);
                    int lastRegionalPlanningConsultation = webDriver.FindElements(consultationRegionalPlanningCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
                case "Regional property services":
                    ButtonElement(consultationRegionalPropServicesExpandBttn);
                    int lastRegionalPropServicesConsultation = webDriver.FindElements(consultationRegionalPropServicesCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
                case "Strategic Real Estate (SRE)":
                    ButtonElement(consultationSREExpandBttn);
                    int lastSREConsultation = webDriver.FindElements(consultationSRECount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
                case "Other":
                    ButtonElement(consultationOtherExpandBttn);
                    int lastOtherConsultation = webDriver.FindElements(consultationOtherCount).Count;
                    webDriver.FindElement(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/h2/div/div/div/div/div/button[@title='Delete Consultation']")).Click();
                    break;
            }

            Wait();
            
            Assert.Equal("Delete Consultation", sharedModals.ModalHeader());
            Assert.Contains("You have selected to delete this Consultation.", sharedModals.ModalContent());
            Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());

            sharedModals.ModalClickOKBttn();
        }

        public void VerifyInitConsultationTab()
        {
            Wait();
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
            AssertTrueIsDisplayed(consultationRequestedOnDate);
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

        public void VerifyLastInsertedConsultationView(LeaseConsultation consultation)
        {
            Wait();
            switch (consultation.leaseConsultationType)
            {
                case "District":
                    ButtonElement(consultationDistrictExpandBttn);
                    int lastDistrictConsultation = webDriver.FindElements(consultationDistrictCount).Count;
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/h2/div/div/div/div/div[1]"), consultation.leaseConsultationOutcome);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"),TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if(consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'District')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastDistrictConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
                case "Engineering":
                    ButtonElement(consultationEngineeringExpandBttn);
                    int lastEngineerConsultation = webDriver.FindElements(consultationEngineeringCount).Count;
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if (consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Engineering')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastEngineerConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
                case "First Nation":
                    ButtonElement(consultationFirstNationExpandBttn);
                    int lastFirstNationConsultation = webDriver.FindElements(consultationFirstNationCount).Count;
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if (consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'First Nation')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastFirstNationConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
                case "Headquarter (HQ)":
                    ButtonElement(consultationHeadquarterExpandBttn);
                    int lastHeadquarterConsultation = webDriver.FindElements(consultationHeadquarterCount).Count;
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if (consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Headquarter (HQ)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastHeadquarterConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
                case "Regional planning":
                    ButtonElement(consultationRegionalPlanningExpandBttn);
                    int lastRegionalPlanningConsultation = webDriver.FindElements(consultationRegionalPlanningCount).Count;
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if (consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional planning')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPlanningConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
                case "Regional property services":
                    ButtonElement(consultationRegionalPropServicesExpandBttn);
                    int lastRegionalPropServicesConsultation = webDriver.FindElements(consultationRegionalPropServicesCount).Count;
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if (consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Regional property services')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastRegionalPropServicesConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
                case "Strategic Real Estate (SRE)":
                    ButtonElement(consultationSREExpandBttn);
                    int lastSREConsultation = webDriver.FindElements(consultationSRECount).Count;
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if (consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastSREConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
                case "Other":
                    ButtonElement(consultationOtherExpandBttn);
                    int lastOtherConsultation = webDriver.FindElements(consultationOtherCount).Count;
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Requested on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationRequestedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[1]/span"), consultation.leaseConsultationContact);
                    if (consultation.leaseConsultationContactPrimaryContact != "")
                        AssertTrueContentEquals(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Contact')]/parent::div/following-sibling::div/a[2]/span"), consultation.leaseConsultationContactPrimaryContact);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Response received')]/parent::div/following-sibling::div"), consultation.leaseConsultationReceived);

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Response received on')]/parent::div/following-sibling::div"), TransformDateFormat(consultation.leaseConsultationReceivedOn));

                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]"));
                    AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/span"));
                    AssertTrueContentEquals(By.XPath("//span[contains(text(),'Other')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ lastOtherConsultation +"]/div/div/div/div/label[contains(text(),'Comments')]/parent::div/following-sibling::div"), consultation.leaseConsultationComment);
                    break;
            }
        }

    }
}
