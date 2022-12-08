

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedSearchProperties : PageObjectBase
    {
        //Search Bar Elements
        private By searchByTab = By.XPath("//a[contains(text(),'Search')]");
        private By searchBySelect = By.Id("input-searchBy");
        private By searchByPIDInput = By.Id("input-pid");
        private By searchByPINInput = By.Id("input-pin");
        private By searchByAddressInput = By.Id("input-address");
        private By searchByAddressInputSuggestionList = By.CssSelector("div[class='suggestionList']");
        private By searchByAddressSuggestion1stOption = By.CssSelector("div[class='suggestionList'] option:nth-child(1)");
        private By searchByPlanInput = By.Id("input-planNumber");
        private By searchByLegalDescriptionInput = By.Id("input-legalDescription");
        private By searchByButton = By.Id("search-button");

        //Search Results Elements
        private By searchPropertiesNoRowsResult = By.CssSelector("div[data-testid='map-properties'] div[class='no-rows-message']");
        private By searchProperties1stResultPropDiv = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchProperties1stResultPropCheckbox = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(1) input");

        //Selected Properties Elements
        private By searchPropertiesSelectedPropertiesTotal = By.XPath("//h3[contains(text(),'Selected properties')]/following-sibling::div");
        private By searchPropertiesDelete1stPropBttn = By.XPath("(//span[contains(text(),'Remove')]/parent::div/parent::button)[1]");

        //Toast Element
        private By generalToastBody = By.CssSelector("div[class='Toastify__toast-body']");

        private SharedModals sharedModals;


        public SharedSearchProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToSearchTab()
        {
            Wait();
            webDriver.FindElement(searchByTab).Click();
        }

        public void SelectPropertyByPID(string PID)
        {
            Wait();
            ChooseSpecificSelectOption("input-searchBy", "PID");

            WaitUntil(searchByPIDInput);
            if (webDriver.FindElement(searchByPIDInput).GetAttribute("value") != "")
            {
                ClearInput(searchByPIDInput);
            }
            webDriver.FindElement(searchByPIDInput).SendKeys(PID);

            webDriver.FindElement(searchByButton).Click();
        }

        public void SelectPropertyByPIN(string PIN)
        {
            Wait();
            ChooseSpecificSelectOption("input-searchBy", "PIN");

            WaitUntil(searchByPINInput);
            if (webDriver.FindElement(searchByPINInput).GetAttribute("value") != "")
            {
                ClearInput(searchByPINInput);
            }
            webDriver.FindElement(searchByPINInput).SendKeys(PIN);

            webDriver.FindElement(searchByButton).Click();
        }

        public void SelectPropertyByAddress(string address)
        {
            Wait();
            ChooseSpecificSelectOption("input-searchBy", "Address");

            WaitUntil(searchByAddressInput);
            if (webDriver.FindElement(searchByAddressInput).GetAttribute("value") != "")
            {
                ClearInput(searchByAddressInput);
            }
            webDriver.FindElement(searchByAddressInput).SendKeys(address);

            WaitUntil(searchByAddressInputSuggestionList);
            FocusAndClick(searchByAddressSuggestion1stOption);

        }

        public void SelectPropertyByPlan(string plan)
        {
            Wait();
            ChooseSpecificSelectOption("input-searchBy", "Plan #");

            WaitUntil(searchByPlanInput);
            if (webDriver.FindElement(searchByPlanInput).GetAttribute("value") != "")
            {
                ClearInput(searchByPlanInput);
            }
            webDriver.FindElement(searchByPlanInput).SendKeys(plan);

            webDriver.FindElement(searchByButton).Click();
        }

        public void SelectPropertyByLegalDescription(string legalDescription)
        {
            Wait();
            ChooseSpecificSelectOption("input-searchBy", "Legal Description");

            WaitUntil(searchByLegalDescriptionInput);
            if (webDriver.FindElement(searchByLegalDescriptionInput).GetAttribute("value") != "")
            {
                ClearInput(searchByLegalDescriptionInput);
            }
            webDriver.FindElement(searchByLegalDescriptionInput).SendKeys(legalDescription);

            webDriver.FindElement(searchByButton).Click();
        }

        public void DeleteProperty()
        {
            var PropertiesTotal = webDriver.FindElements(searchPropertiesSelectedPropertiesTotal).Count() -1;

            webDriver.FindElement(searchPropertiesDelete1stPropBttn).Click();

            var PropertiesLeft = webDriver.FindElements(searchPropertiesSelectedPropertiesTotal).Count() -1;

            Assert.True(PropertiesTotal - PropertiesLeft == 1);
        }

        public void SelectFirstOption()
        {
            WaitUntil(searchProperties1stResultPropDiv);
            FocusAndClick(searchProperties1stResultPropCheckbox);

            ButtonElement("Add to selection");

            if (webDriver.FindElements(generalToastBody).Count() > 0)
            {
                Assert.True(sharedModals.ToastifyText().Equals("A property that the user is trying to select has already beed added to the selected properties list"));
            }
        }

        public string noRowsResultsMessage()
        {
            WaitUntil(searchPropertiesNoRowsResult);
            return webDriver.FindElement(searchPropertiesNoRowsResult).Text;
        }

        
        
    }
}
