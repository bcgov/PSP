using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class PropertyImprovements : PageObjectBase
    {
        private readonly By improvementLink = By.CssSelector("a[data-rb-event-key='improvements']");
        private readonly By improvementAddButton = By.XPath("//div[text()='Property Improvements']/following-sibling::div/button");

        private readonly By improvementDetailsTitle = By.XPath("//div[text()='Property Improvement Details']");

        private readonly By improvementDetailsNameLabel = By.XPath("//label[text()='Name']");
        private readonly By improvementDetailsNameInput = By.Id("input-name");

        private readonly By improvementDetailsTypeLabel = By.XPath("//label[text()='Improvement type']");
        private readonly By improvementDetailsTypeSelect = By.Id("input-improvementTypeCode");

        private readonly By improvementDetailsStatusLabel = By.XPath("//label[text()='Improvement status']");
        private readonly By improvementDetailsStatusSelect = By.Id("input-improvementStatusCode");

        private readonly By improvementDetailsDateLabel = By.XPath("//label[text()='Improvement date']");
        private readonly By improvementDetailsDateInput = By.Id("datepicker-improvementDate");

        private readonly By improvementDetailsDescriptionLabel = By.XPath("//label[text()='Description']");
        private readonly By improvementDetailsDescriptionInput = By.Id("input-description");

        private readonly By licenseImproTotal = By.XPath("//div[@role='tabpanel']/div/div[2]/div");

        private SharedModals sharedModals;

        public PropertyImprovements(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        //Navigate to Improvements section
        public void NavigateToImprovementSection()
        {
            WaitUntilClickable(improvementLink);
            FocusAndClick(improvementLink);
        }

        //Edit Improvements section
        public void EditNthImprovements(int index)
        {
            Wait();
            By editBttn = By.CssSelector("button[data-testid='improvements["+ index +"].edit-btn']");
            SafeClick(editBttn);

            WaitUntilSpinnerDisappear();
        }

        //Add New improvement
        public void AddImprovementBttn()
        {
            WaitUntilClickable(improvementAddButton);
            webDriver.FindElement(improvementAddButton).Click();
        }

        //Create Improvements
        public void AddUpdateImprovement(PropertyImprovement improvement)
        {
            WaitUntilVisible(improvementDetailsTitle);
            AssertTrueIsDisplayed(improvementDetailsTitle);

            AssertTrueIsDisplayed(improvementDetailsNameLabel);
            ClearInput(improvementDetailsNameInput);
            webDriver.FindElement(improvementDetailsNameInput).SendKeys(improvement.ImprovementName);

            AssertTrueIsDisplayed(improvementDetailsTypeLabel);
            ChooseSelectOption(improvementDetailsTypeSelect, improvement.ImprovementType);

            AssertTrueIsDisplayed(improvementDetailsStatusLabel);
            ChooseSelectOption(improvementDetailsStatusSelect, improvement.ImprovementStatus);

            AssertTrueIsDisplayed(improvementDetailsDateLabel);
            if (improvement.ImprovementDate != "")
            {
                ClearInput(improvementDetailsDateInput);
                webDriver.FindElement(improvementDetailsDateInput).SendKeys(improvement.ImprovementDate);
            }  

            AssertTrueIsDisplayed(improvementDetailsDescriptionLabel);
            if (improvement.ImprovementDescription != "")
            {
                ClearInput(improvementDetailsDescriptionInput);
                webDriver.FindElement(improvementDetailsDescriptionInput).SendKeys(improvement.ImprovementDescription);
            } 
        }

        public void VerifyImprovementView(int index, PropertyImprovement improvement)
        {
            int elementIdx = index + 1;

            Wait();

            AssertTrueIsDisplayed(improvementDetailsNameLabel);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='improvement["+ elementIdx +"].name']"), improvement.ImprovementName);

            AssertTrueIsDisplayed(improvementDetailsTypeLabel);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='improvement["+ elementIdx +"].type']"), improvement.ImprovementType);

            AssertTrueIsDisplayed(improvementDetailsDateLabel);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='improvement["+ elementIdx +"].date']"), TransformDateFormat(improvement.ImprovementDate));

            AssertTrueIsDisplayed(improvementDetailsStatusLabel);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='improvement["+ elementIdx +"].status']"), improvement.ImprovementStatus);

            AssertTrueIsDisplayed(improvementDetailsDescriptionLabel);
            if(improvement.ImprovementDescription != "")
                AssertTrueContentEquals(By.CssSelector("div[data-testid='improvement["+ elementIdx +"].description']"), improvement.ImprovementDescription);
        }

        public void SaveImprovement()
        {
            ButtonElement("Save");
        }

        public void CancelImprovement()
        {
            ButtonElement("Cancel");
            sharedModals.CancelActionModal();
        }

        public int ImprovementTotal()
        {
            WaitUntilVisible(licenseImproTotal);
            return webDriver.FindElements(licenseImproTotal).Count;
        }
    }
}
