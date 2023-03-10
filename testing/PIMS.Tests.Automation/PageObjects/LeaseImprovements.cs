

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseImprovements : PageObjectBase
    {

        private By licenseImprovementLink = By.XPath("//a[contains(text(),'Improvements')]");
        private By improvementEditIcon = By.XPath("//div[@role='tabpanel'][3]/div/div/button");

        private By licenceImprovCommecialSubtitle = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2");
        private By licenceImprovCommercialUnitNbrLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenceImprovCommercialUnitNbrInput = By.Id("input-improvements.0.address");
        private By licenceImprovCommercialSizeLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenceImprovCommercialSizeInput = By.Id("input-improvements.0.structureSize");
        private By licenceImprovCommercialDescriptionLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenceImprovCommercialDescriptionTextarea = By.Id("input-improvements.0.description");

        private By licenceImprovResidentialSubtitle = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2");
        private By licenceImprovResidentialUnitNbrLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenceImprovResidentialUnitNbrInput = By.Id("input-improvements.2.address");
        private By licenceImprovResidentialSizeLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenceImprovResidentialSizeInput = By.Id("input-improvements.2.structureSize");
        private By licenceImprovResidentialDescriptionLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenceImprovResidentialDescriptionTextarea = By.Id("input-improvements.2.description");

        private By licenceImprovOtherSubtitle = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2");
        private By licenceImprovOtherlUnitNbrLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenceImprovOtherUnitNbrInput = By.Id("input-improvements.1.address");
        private By licenceImprovOtherSizeLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenceImprovOtherSizeInput = By.Id("input-improvements.1.structureSize");
        private By licenceImprovOtherDescriptionLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenceImprovOtherDescriptionTextarea = By.Id("input-improvements.1.description");

        private By licenseImproSaveButton = By.XPath("//button/div[contains(text(),'Save')]/ancestor::button");

        public LeaseImprovements(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigate to Improvements section
        public void NavigateToImprovementSection()
        {
            Wait();
            webDriver.FindElement(licenseImprovementLink).Click();
        }

        //Edit Improvements section
        public void EditImprovements()
        {
            Wait();
            webDriver.FindElement(improvementEditIcon).Click();
        }

        //Add Commercial Improvements
        public void AddCommercialImprovement(string address, string size, string description)
        {
            webDriver.FindElement(licenceImprovCommercialUnitNbrInput).SendKeys(address);
            webDriver.FindElement(licenceImprovCommercialSizeInput).SendKeys(size);
            webDriver.FindElement(licenceImprovCommercialDescriptionTextarea).SendKeys(description);
        }

        //Add Residetial Improvements
        public void AddResidentialImprovement(string address, string size, string description)
        {
            webDriver.FindElement(licenceImprovResidentialUnitNbrInput).SendKeys(address);
            webDriver.FindElement(licenceImprovResidentialSizeInput).SendKeys(size);
            webDriver.FindElement(licenceImprovResidentialDescriptionTextarea).SendKeys(description);
        }

        //Add Other Improvements
        public void AddOtherImprovement(string address, string size, string description)
        {
            webDriver.FindElement(licenceImprovOtherUnitNbrInput).SendKeys(address);
            webDriver.FindElement(licenceImprovOtherSizeInput).SendKeys(size);
            webDriver.FindElement(licenceImprovOtherDescriptionTextarea).SendKeys(description);
        }

        public void VerifyImprovementView(string commUnitNbr, string commSize, string commDescription, string resUnitNbr, string resSize, string resDescription, string otherUnitNbr, string otherSize, string otherDescription)
        {
            Wait();
            Assert.True(webDriver.FindElement(licenceImprovCommecialSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovCommercialUnitNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovCommercialUnitNbrInput).GetAttribute("value") == commUnitNbr);
            Assert.True(webDriver.FindElement(licenceImprovCommercialSizeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovCommercialSizeInput).GetAttribute("value") == commSize);
            Assert.True(webDriver.FindElement(licenceImprovCommercialDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovCommercialDescriptionTextarea).Text == commDescription);

            Assert.True(webDriver.FindElement(licenceImprovResidentialSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovResidentialUnitNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovResidentialUnitNbrInput).GetAttribute("value") == resUnitNbr);
            Assert.True(webDriver.FindElement(licenceImprovResidentialSizeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovResidentialSizeInput).GetAttribute("value") == resSize);
            Assert.True(webDriver.FindElement(licenceImprovResidentialDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovResidentialDescriptionTextarea).Text == resDescription);

            Assert.True(webDriver.FindElement(licenceImprovOtherSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovOtherlUnitNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovOtherUnitNbrInput).GetAttribute("value") == otherUnitNbr);
            Assert.True(webDriver.FindElement(licenceImprovOtherSizeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovOtherSizeInput).GetAttribute("value") == otherSize);
            Assert.True(webDriver.FindElement(licenceImprovOtherDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovOtherDescriptionTextarea).Text == otherDescription);
        }
    }
}
