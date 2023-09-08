using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class FinancialCodes : PageObjectBase
    {
        //Main Menu Element
        private By financialCodeMainMenuLink = By.XPath("//a[contains(text(),'Manage Project and Financial Codes')]");

        //Financial Codes List View Elements
        //Financial Codes Filters Elements
        private By financialCodeTitle = By.XPath("//h3[contains(text(),'Financial Codes')]");
        private By financialCodeTypeSelect = By.Id("input-financialCodeType");
        private By financialCodeDescriptionInput = By.Id("input-codeValueOrDescription");
        private By financialCodeShowExpiredInput = By.Id("input-showExpiredCodes");
        private By financialCodeShowExpiredSpan = By.XPath("//span[contains(text(),'Show expired codes')]");
        private By financialCodeSearchBttn = By.Id("search-button");
        private By financialCodeResetBttn = By.Id("reset-button");

        private By financialCodeCreateNewBttn = By.XPath("//div[@data-testid='FinancialCodeTable']/preceding-sibling::button");

        //Financial Codes Table
        private By financialCodeTableHeaderCodeValue = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Code value')]");
        private By financialCodeTableHeaderCodeDescription = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Code description')]");
        private By financialCodeTableHeaderCodeType = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Code type')]");
        private By financialCodeTableHeaderEffectiveDate = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Effective date')]");
        private By financialCodeTableHeaderExpiryDate = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expiry date')]");
        private By financialCodeTableResultsTotal = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        private By financialResults1stResultCodeValue = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[1]/a");
        private By financialResults1stResultCodeDescription = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[2]");
        private By financialResults1stResultCodeType = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[3]");
        private By financialResults1stResultEffectiveDate = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]");
        private By financialResults1stResultExpiryDate = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[5]");

        private By financialCodePaginationEntries = By.CssSelector("div[class='Menu-root']");
        private By financialCodePaginationList = By.CssSelector("ul[class='pagination']");

        //Financial Codes Create/Update Form Elements
        private By financialCodeCreateTitle = By.XPath("//h1[contains(text(),'Create Financial Code')]");
        private By financialCodeUpdateTitle = By.XPath("//h1[contains(text(),'Update Financial Code')]");

        private By financialCodeFormTypeLabel = By.XPath("//label[contains(text(),'Code type')]");
        private By financialCodeFormTypeSelect = By.Id("input-type");
        private By financialCodeFormTypeErrorMessage = By.XPath("//div[contains(text(),'Code type is required')]");

        private By financialCodeFormValueLabel = By.XPath("//label[contains(text(),'Code value')]");
        private By financialCodeFormValueInput = By.Id("input-code");
        private By financialCodeFormValueErrorMessage = By.XPath("//div[contains(text(),'Code value is required')]");
        private By financialCodeFormValueContent = By.XPath("//label[contains(text(),'Code type')]/parent::div/following-sibling::div/span");

        private By financialCodeFormDescriptionLabel = By.XPath("//label[contains(text(),'Code description')]");
        private By financialCodeFormDescriptionInput = By.Id("input-description");
        private By financialCodeFormDescriptionErrorMessage = By.XPath("//div[contains(text(),'Code description is required')]");

        private By financialCodeFormEffectiveDateLabel = By.XPath("//label[contains(text(),'Effective date')]");
        private By financialCodeFormEffectiveDateTooltip = By.XPath("//label[contains(text(),'Effective date')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By financialCodeFormEffectiveDateInput = By.Id("datepicker-effectiveDate");

        private By financialCodeFormExpiryDateLabel = By.XPath("//label[contains(text(),'Expiry date')]");
        private By financialCodeFormExpiryDateTooltip = By.XPath("//label[contains(text(),'Expiry date')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By financialCodeFormExpiryDateInput = By.Id("datepicker-expiryDate");

        private By financialCodeFormOrderLabel = By.XPath("//label[contains(text(),'Display order')]");
        private By financialCodeFormOrderInput = By.Id("input-displayOrder");

        private By financialCodeFormCancelBttn = By.XPath("//div[contains(text(),'Cancel')]/parent::button");
        private By financialCodeFormSaveBttn = By.XPath("//div[contains(text(),'Save')]/parent::button");

        //Financial Code Confirmation Modal
        private By financialCodeModal = By.CssSelector("div[class='modal-dialog']");

        //Financial Code Error Message
        private By financialCodeDuplicateErrorMessage = By.XPath("//div[contains(text(),'Cannot create duplicate financial code')]");

        private SharedModals sharedModals;

        public FinancialCodes(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateAdminFinancialCodes()
        {
            WaitUntilClickable(financialCodeMainMenuLink);
            FocusAndClick(financialCodeMainMenuLink);
        }

        public void CreateNewFinancialCodeBttn()
        {
            WaitUntilClickable(financialCodeCreateNewBttn);
            webDriver.FindElement(financialCodeCreateNewBttn).Click();
        }

        public void CreateNewFinancialCode(FinancialCode financialCode)
        {
            WaitUntilClickable(financialCodeFormTypeSelect);
            ChooseSpecificSelectOption(financialCodeFormTypeSelect, financialCode.CodeType);
            webDriver.FindElement(financialCodeFormValueInput).SendKeys(financialCode.CodeValue);
            webDriver.FindElement(financialCodeFormDescriptionInput).SendKeys(financialCode.CodeDescription);
            webDriver.FindElement(financialCodeFormOrderInput).SendKeys(financialCode.DisplayOrder);
        }

        public void UpdateFinancialCode(FinancialCode financialCode)
        {
            WaitUntilClickable(financialCodeFormDescriptionInput);
            ClearInput(financialCodeFormDescriptionInput);
            webDriver.FindElement(financialCodeFormDescriptionInput).SendKeys(financialCode.CodeDescription);
            webDriver.FindElement(financialCodeFormExpiryDateInput).SendKeys(financialCode.ExpiryDate);
            webDriver.FindElement(financialCodeFormExpiryDateLabel).Click();
        }

        public void SaveFinancialCode()
        {
            WaitUntilClickable(financialCodeFormSaveBttn);
            webDriver.FindElement(financialCodeFormSaveBttn).Click();
        }

        public void CancelFinancialCode()
        {
            WaitUntilClickable(financialCodeFormCancelBttn);
            webDriver.FindElement(financialCodeFormCancelBttn).Click();

            Wait();
            if (webDriver.FindElements(financialCodeModal).Count() > 0)
            {
                Assert.True(sharedModals.ModalHeader().Equals("Unsaved Changes"));
                Assert.True(sharedModals.ModalContent().Equals("You have made changes on this form. Do you wish to leave without saving?"));

                sharedModals.ModalClickOKBttn();
            }
        }

        public void FilterFinancialCode(string value)
        {
            WaitUntilClickable(financialCodeResetBttn);
            webDriver.FindElement(financialCodeResetBttn).Click();

            WaitUntilVisible(financialCodeDescriptionInput);
            webDriver.FindElement(financialCodeDescriptionInput).SendKeys(value);
            webDriver.FindElement(financialCodeSearchBttn).Click();
        }

        public int CountTotalFinancialCodeResults()
        {
            Wait();
            return webDriver.FindElements(financialCodeTableResultsTotal).Count();
        }

        public Boolean DuplicateErrorMessageDisplayed()
        {
            WaitUntilVisible(financialCodeDuplicateErrorMessage);
            return webDriver.FindElement(financialCodeDuplicateErrorMessage).Displayed;
        }

        public void ChooseFirstSearchCodeValue()
        {
            WaitUntilClickable(financialResults1stResultCodeValue);
            webDriver.FindElement(financialResults1stResultCodeValue).Click();
        }

        public void VerifyFinancialCodeListView()
        {
            WaitUntilVisible(financialCodeTitle);
            Assert.True(webDriver.FindElement(financialCodeTitle).Displayed);
            Assert.True(webDriver.FindElement(financialCodeTypeSelect).Displayed);
            Assert.True(webDriver.FindElement(financialCodeDescriptionInput).Displayed);
            Assert.True(webDriver.FindElement(financialCodeShowExpiredInput).Displayed);
            Assert.True(webDriver.FindElement(financialCodeShowExpiredSpan).Displayed);
            Assert.True(webDriver.FindElement(financialCodeSearchBttn).Displayed);
            Assert.True(webDriver.FindElement(financialCodeResetBttn).Displayed);
            Assert.True(webDriver.FindElement(financialCodeCreateNewBttn).Displayed);

            Assert.True(webDriver.FindElement(financialCodeTableHeaderCodeValue).Displayed);
            Assert.True(webDriver.FindElement(financialCodeTableHeaderCodeDescription).Displayed);
            Assert.True(webDriver.FindElement(financialCodeTableHeaderCodeType).Displayed);
            Assert.True(webDriver.FindElement(financialCodeTableHeaderEffectiveDate).Displayed);
            Assert.True(webDriver.FindElement(financialCodeTableHeaderExpiryDate).Displayed);
            Assert.True(webDriver.FindElements(financialCodeTableResultsTotal).Count() > 1);
            
            Assert.True(webDriver.FindElement(financialCodePaginationEntries).Displayed);
            Assert.True(webDriver.FindElement(financialCodePaginationList).Displayed);
        }

        public void VerifyCreateNewFinancialCodeForm()
        {
            WaitUntilVisible(financialCodeCreateTitle);
            Assert.True(webDriver.FindElement(financialCodeCreateTitle).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormTypeSelect).Displayed);

            webDriver.FindElement(financialCodeFormTypeSelect).Click();
            webDriver.FindElement(financialCodeFormTypeLabel).Click();
            Assert.True(webDriver.FindElement(financialCodeFormTypeErrorMessage).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormValueLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormValueInput).Displayed);

            webDriver.FindElement(financialCodeFormValueInput).Click();
            webDriver.FindElement(financialCodeFormValueLabel).Click();
            Assert.True(webDriver.FindElement(financialCodeFormValueErrorMessage).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormDescriptionInput).Displayed);

            webDriver.FindElement(financialCodeFormDescriptionInput).Click();
            webDriver.FindElement(financialCodeFormDescriptionLabel).Click();
            Assert.True(webDriver.FindElement(financialCodeFormDescriptionErrorMessage).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormEffectiveDateLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormEffectiveDateTooltip).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormEffectiveDateInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormExpiryDateTooltip).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormExpiryDateInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormOrderLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormOrderInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormCancelBttn).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormSaveBttn).Displayed);
        }

        public void VerifyUpdateFinancialCodeForm()
        {
            WaitUntilVisible(financialCodeUpdateTitle);
            Assert.True(webDriver.FindElement(financialCodeUpdateTitle).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormValueContent).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormValueLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormValueInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormDescriptionInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormEffectiveDateLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormEffectiveDateTooltip).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormEffectiveDateInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormExpiryDateTooltip).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormExpiryDateInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormOrderLabel).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormOrderInput).Displayed);

            Assert.True(webDriver.FindElement(financialCodeFormCancelBttn).Displayed);
            Assert.True(webDriver.FindElement(financialCodeFormSaveBttn).Displayed);
        }
    }
}
