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
        private By financialCodeTableCodeSortBttn = By.CssSelector("div[data-testid='sort-column-code']");
        private By financialCodeTableHeaderCodeDescription = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Code description')]");
        private By financialCodeTableDescriptionSortBttn = By.CssSelector("div[data-testid='sort-column-description']");
        private By financialCodeTableHeaderCodeType = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Code type')]");
        private By financialCodeTableTypeSortBttn = By.CssSelector("div[data-testid='sort-column-type']");
        private By financialCodeTableHeaderEffectiveDate = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Effective date')]");
        private By financialCodeTableEffectiveDateSortBttn = By.CssSelector("div[data-testid='sort-column-effectiveDate']");
        private By financialCodeTableHeaderExpiryDate = By.XPath("//div[@data-testid='FinancialCodeTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expiry date')]");
        private By financialCodeTableExpiryDateSortBttn = By.CssSelector("div[data-testid='sort-column-expiryDate']");
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
        private By financialCodeModal = By.CssSelector("div[class='modal-content']");

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
            ChooseSpecificSelectOption(financialCodeFormTypeSelect, financialCode.FinnCodeType);
            webDriver.FindElement(financialCodeFormValueInput).SendKeys(financialCode.FinnCodeValue);
            webDriver.FindElement(financialCodeFormDescriptionInput).SendKeys(financialCode.FinnCodeDescription);
            webDriver.FindElement(financialCodeFormOrderInput).SendKeys(financialCode.FinnDisplayOrder);
        }

        public void UpdateFinancialCode(FinancialCode financialCode)
        {
            WaitUntilClickable(financialCodeFormDescriptionInput);
            ClearInput(financialCodeFormDescriptionInput);
            webDriver.FindElement(financialCodeFormDescriptionInput).SendKeys(financialCode.FinnCodeDescription);
            webDriver.FindElement(financialCodeFormExpiryDateInput).SendKeys(financialCode.FinnExpiryDate);
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
                Assert.Equal("Unsaved Changes", sharedModals.ModalHeader());
                Assert.Equal("You have made changes on this form. Do you wish to leave without saving?", sharedModals.ModalContent());

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

        public void FilterFinancialCodeByType(string value)
        {
            WaitUntilClickable(financialCodeResetBttn);
            webDriver.FindElement(financialCodeResetBttn).Click();

            WaitUntilVisible(financialCodeTypeSelect);
            ChooseSpecificSelectOption(financialCodeTypeSelect, value);
            webDriver.FindElement(financialCodeSearchBttn).Click();
        }

        public void OrderByFinancialCodeValue()
        {
            WaitUntilClickable(financialCodeTableCodeSortBttn);
            webDriver.FindElement(financialCodeTableCodeSortBttn).Click();
        }

        public void OrderByFinancialCodeDescription()
        {
            WaitUntilClickable(financialCodeTableDescriptionSortBttn);
            webDriver.FindElement(financialCodeTableDescriptionSortBttn).Click();
        }

        public void OrderByFinancialCodeType()
        {
            WaitUntilClickable(financialCodeTableTypeSortBttn);
            webDriver.FindElement(financialCodeTableTypeSortBttn).Click();
        }

        public void OrderByFinancialCodeEffectiveDate()
        {
            WaitUntilClickable(financialCodeTableEffectiveDateSortBttn);
            webDriver.FindElement(financialCodeTableEffectiveDateSortBttn).Click();
        }

        public void OrderByFinancialCodeExpiryDate()
        {
            WaitUntilClickable(financialCodeTableExpiryDateSortBttn);
            webDriver.FindElement(financialCodeTableExpiryDateSortBttn).Click();
        }

        public int CountTotalFinancialCodeResults()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(financialCodeTableResultsTotal).Count();
        }

        public Boolean DuplicateErrorMessageDisplayed()
        {
            WaitUntilVisible(financialCodeDuplicateErrorMessage);
            return webDriver.FindElement(financialCodeDuplicateErrorMessage).Displayed;
        }

        public string FirstFinancialCodeValue()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(financialResults1stResultCodeValue).Text;
        }

        public string FirstFinancialCodeDescription()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(financialResults1stResultCodeDescription).Text;
        }

        public string FirstFinancialCodeType()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(financialResults1stResultCodeType).Text;
        }

        public string FirstFinancialCodeEffectiveDate()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(financialResults1stResultEffectiveDate).Text;
        }

        public string FirstFinancialCodeExpiryDate()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(financialResults1stResultExpiryDate).Text;
        }

        public void ChooseFirstSearchCodeValue()
        {
            WaitUntilClickable(financialResults1stResultCodeValue);
            webDriver.FindElement(financialResults1stResultCodeValue).Click();
        }

        public void VerifyFinancialCodeListView()
        {
            AssertTrueIsDisplayed(financialCodeTitle);
            AssertTrueIsDisplayed(financialCodeTypeSelect);
            AssertTrueIsDisplayed(financialCodeDescriptionInput);
            AssertTrueIsDisplayed(financialCodeShowExpiredInput);
            AssertTrueIsDisplayed(financialCodeShowExpiredSpan);
            AssertTrueIsDisplayed(financialCodeSearchBttn);
            AssertTrueIsDisplayed(financialCodeResetBttn);
            AssertTrueIsDisplayed(financialCodeCreateNewBttn);

            AssertTrueIsDisplayed(financialCodeTableHeaderCodeValue);
            AssertTrueIsDisplayed(financialCodeTableHeaderCodeDescription);
            AssertTrueIsDisplayed(financialCodeTableHeaderCodeType);
            AssertTrueIsDisplayed(financialCodeTableHeaderEffectiveDate);
            AssertTrueIsDisplayed(financialCodeTableHeaderExpiryDate);

            Assert.True(webDriver.FindElements(financialCodeTableResultsTotal).Count() > 0);

            AssertTrueIsDisplayed(financialCodePaginationEntries);
            AssertTrueIsDisplayed(financialCodePaginationList);
        }

        public void VerifyCreateNewFinancialCodeForm()
        {

            AssertTrueIsDisplayed(financialCodeCreateTitle);

            AssertTrueIsDisplayed(financialCodeFormTypeLabel);
            AssertTrueIsDisplayed(financialCodeFormTypeSelect);

            webDriver.FindElement(financialCodeFormTypeSelect).Click();
            webDriver.FindElement(financialCodeFormTypeLabel).Click();
            AssertTrueIsDisplayed(financialCodeFormTypeErrorMessage);

            AssertTrueIsDisplayed(financialCodeFormValueLabel);
            AssertTrueIsDisplayed(financialCodeFormValueInput);

            webDriver.FindElement(financialCodeFormValueInput).Click();
            webDriver.FindElement(financialCodeFormValueLabel).Click();
            AssertTrueIsDisplayed(financialCodeFormValueErrorMessage);

            AssertTrueIsDisplayed(financialCodeFormDescriptionLabel);
            AssertTrueIsDisplayed(financialCodeFormDescriptionInput);

            webDriver.FindElement(financialCodeFormDescriptionInput).Click();
            webDriver.FindElement(financialCodeFormDescriptionLabel).Click();
            AssertTrueIsDisplayed(financialCodeFormDescriptionErrorMessage);

            AssertTrueIsDisplayed(financialCodeFormEffectiveDateLabel);
            AssertTrueIsDisplayed(financialCodeFormEffectiveDateTooltip);
            AssertTrueIsDisplayed(financialCodeFormEffectiveDateInput);

            AssertTrueIsDisplayed(financialCodeFormExpiryDateLabel);
            AssertTrueIsDisplayed(financialCodeFormExpiryDateTooltip);
            AssertTrueIsDisplayed(financialCodeFormExpiryDateInput);

            AssertTrueIsDisplayed(financialCodeFormOrderLabel);
            AssertTrueIsDisplayed(financialCodeFormOrderInput);

            AssertTrueIsDisplayed(financialCodeFormCancelBttn);
            AssertTrueIsDisplayed(financialCodeFormSaveBttn);
        }

        public void VerifyUpdateFinancialCodeForm()
        {
            AssertTrueIsDisplayed(financialCodeUpdateTitle);

            AssertTrueIsDisplayed(financialCodeFormTypeLabel);
            AssertTrueIsDisplayed(financialCodeFormValueContent);

            AssertTrueIsDisplayed(financialCodeFormValueLabel);
            AssertTrueIsDisplayed(financialCodeFormValueInput);

            AssertTrueIsDisplayed(financialCodeFormDescriptionLabel);
            AssertTrueIsDisplayed(financialCodeFormDescriptionInput);

            AssertTrueIsDisplayed(financialCodeFormEffectiveDateLabel);
            AssertTrueIsDisplayed(financialCodeFormEffectiveDateTooltip);
            AssertTrueIsDisplayed(financialCodeFormEffectiveDateInput);

            AssertTrueIsDisplayed(financialCodeFormExpiryDateLabel);
            AssertTrueIsDisplayed(financialCodeFormExpiryDateTooltip);
            AssertTrueIsDisplayed(financialCodeFormExpiryDateInput);

            AssertTrueIsDisplayed(financialCodeFormOrderLabel);
            AssertTrueIsDisplayed(financialCodeFormOrderInput);

            AssertTrueIsDisplayed(financialCodeFormCancelBttn);
            AssertTrueIsDisplayed(financialCodeFormSaveBttn);
        }
    }
}
