

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseImprovements : PageObjectBase
    {

        private By licenseImprovementLink = By.XPath("//a[contains(text(),'Improvements')]");
        private By improvementEditIcon = By.CssSelector("a[class='float-right']");

        private By licenceImprovCommercialAddressInput = By.Id("input-improvements.0.address");
        private By licenceImprovCommercialSizeInput = By.Id("input-improvements.0.structureSize");
        private By licenceImprovCommercialDescriptionTextarea = By.Id("input-improvements.0.description");

        private By licenceImprovResidentialAddressInput = By.Id("input-improvements.1.address");
        private By licenceImprovResidentialSizeInput = By.Id("input-improvements.1.structureSize");
        private By licenceImprovResidentialDescriptionTextarea = By.Id("input-improvements.1.description");

        private By licenceImprovOtherAddressInput = By.Id("input-improvements.2.address");
        private By licenceImprovOtherSizeInput = By.Id("input-improvements.2.structureSize");
        private By licenceImprovOtherDescriptionTextarea = By.Id("input-improvements.2.description");

        private By licenseImproSaveButton = By.XPath("//button/div[contains(text(),'Save')]/ancestor::button");

        public LeaseImprovements(IWebDriver webDriver) : base(webDriver)
        {
        }

        //Navigate to Improvements section
        public void NavigateToImprovementSection()
        {
            Wait();
            webDriver.FindElement(licenseImprovementLink).Click();
        }

        //Edit Improvements section
        public void EditImprovements()
        {
            WaitUntil(improvementEditIcon);

            webDriver.FindElement(improvementEditIcon).Click();
        }

        //Add Commercial Improvements
        public void AddCommercialImprovement(string address, string size, string description)
        {
            webDriver.FindElement(licenceImprovCommercialAddressInput).SendKeys(address);
            webDriver.FindElement(licenceImprovCommercialSizeInput).SendKeys(size);
            webDriver.FindElement(licenceImprovCommercialDescriptionTextarea).SendKeys(description);
        }

        //Add Residetial Improvements
        public void AddResidentialImprovement(string address, string size, string description)
        {
            webDriver.FindElement(licenceImprovResidentialAddressInput).SendKeys(address);
            webDriver.FindElement(licenceImprovResidentialSizeInput).SendKeys(size);
            webDriver.FindElement(licenceImprovResidentialDescriptionTextarea).SendKeys(description);
        }

        //Add Other Improvements
        public void AddOtherImprovement(string address, string size, string description)
        {
            webDriver.FindElement(licenceImprovOtherAddressInput).SendKeys(address);
            webDriver.FindElement(licenceImprovOtherSizeInput).SendKeys(size);
            webDriver.FindElement(licenceImprovOtherDescriptionTextarea).SendKeys(description);
        }

        //Save Improvements
        public void SaveImproments()
        {
            Wait();

            ScrollToElement(licenseImproSaveButton);

            var saveButton = webDriver.FindElement(licenseImproSaveButton);
            saveButton.Enabled.Should().BeTrue();

            Wait();
            saveButton.Click();
        }
    }
}
