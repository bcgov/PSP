

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedSearchProperties : PageObjectBase
    {
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

        private By searchPropertiesNoRowsResult = By.CssSelector("div[data-testid='map-properties'] div[class='no-rows-message']");
        private By searchProperties1stResultPropDiv = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchProperties1stResultPropCheckbox = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(1) input");

        public SharedSearchProperties(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateToSearchTab()
        {
            Wait();
            webDriver.FindElement(searchByTab).Click();
        }

        public void SelectPropertyByPID(string PID)
        {
            Wait();
            ChooseSelectSpecificOption("input-searchBy", "PID");

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
            ChooseSelectSpecificOption("input-searchBy", "PIN");

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
            ChooseSelectSpecificOption("input-searchBy", "Address");

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
            ChooseSelectSpecificOption("input-searchBy", "Plan #");

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
            ChooseSelectSpecificOption("input-searchBy", "Legal Description");

            WaitUntil(searchByLegalDescriptionInput);
            if (webDriver.FindElement(searchByLegalDescriptionInput).GetAttribute("value") != "")
            {
                ClearInput(searchByLegalDescriptionInput);
            }
            webDriver.FindElement(searchByLegalDescriptionInput).SendKeys(legalDescription);

            webDriver.FindElement(searchByButton).Click();
        }

        public void SelectFirstOption()
        {
            WaitUntil(searchProperties1stResultPropDiv);
            FocusAndClick(searchProperties1stResultPropCheckbox);

            ButtonElement("Add to selection");

        }

        public string noRowsResultsMessage()
        {
            WaitUntil(searchPropertiesNoRowsResult);
            return webDriver.FindElement(searchPropertiesNoRowsResult).Text;
        }

        //No results found for your search criteria.
        //Too many results (more than 15) match this criteria. Please refine your search. lot 97
        
    }
}
