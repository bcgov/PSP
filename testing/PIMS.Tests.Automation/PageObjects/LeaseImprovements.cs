

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

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
        public void AddCommercialImprovement(Lease lease)
        {
            if (lease.CommercialImprovementUnit != "")
            {
                ClearInput(licenceImprovCommercialUnitNbrInput);
                webDriver.FindElement(licenceImprovCommercialUnitNbrInput).SendKeys(lease.CommercialImprovementUnit);
            }

            if (lease.CommercialImprovementBuildingSize != "")
            {
                ClearInput(licenceImprovCommercialSizeInput);
                webDriver.FindElement(licenceImprovCommercialSizeInput).SendKeys(lease.CommercialImprovementBuildingSize);
            }

            if (lease.CommercialImprovementDescription != "")
            {
                ClearInput(licenceImprovCommercialDescriptionTextarea);
                webDriver.FindElement(licenceImprovCommercialDescriptionTextarea).SendKeys(lease.CommercialImprovementDescription);
            }   
        }

        //Add Residetial Improvements
        public void AddResidentialImprovement(Lease lease)
        {
            if (lease.ResidentialImprovementUnit != "")
            {
                ClearInput(licenceImprovResidentialUnitNbrInput);
                webDriver.FindElement(licenceImprovResidentialUnitNbrInput).SendKeys(lease.ResidentialImprovementUnit);
            }

            if (lease.ResidentialImprovementBuildingSize != "")
            {
                ClearInput(licenceImprovResidentialSizeInput);
                webDriver.FindElement(licenceImprovResidentialSizeInput).SendKeys(lease.ResidentialImprovementBuildingSize);
            }

            if (lease.ResidentialImprovementDescription != "")
            {
                ClearInput(licenceImprovResidentialDescriptionTextarea);
                webDriver.FindElement(licenceImprovResidentialDescriptionTextarea).SendKeys(lease.ResidentialImprovementDescription);
            }
                
        }

        //Add Other Improvements
        public void AddOtherImprovement(Lease lease)
        {
            if (lease.OtherImprovementUnit != "")
            {
                ClearInput(licenceImprovOtherUnitNbrInput);
                webDriver.FindElement(licenceImprovOtherUnitNbrInput).SendKeys(lease.OtherImprovementUnit);
            }

            if (lease.OtherImprovementBuildingSize != "")
            {
                ClearInput(licenceImprovOtherSizeInput);
                webDriver.FindElement(licenceImprovOtherSizeInput).SendKeys(lease.OtherImprovementBuildingSize);
            }

            if (lease.OtherImprovementDescription != "")
            {
                ClearInput(licenceImprovOtherDescriptionTextarea);
                webDriver.FindElement(licenceImprovOtherDescriptionTextarea).SendKeys(lease.OtherImprovementDescription);
            }  
        }

        public void VerifyImprovementView(Lease lease)
        {
            Wait();
            Assert.True(webDriver.FindElement(licenceImprovCommecialSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovCommercialUnitNbrLabel).Displayed);
            if(lease.CommercialImprovementUnit != "")
                Assert.True(webDriver.FindElement(licenceImprovCommercialUnitNbrInput).GetAttribute("value") == lease.CommercialImprovementUnit);
            Assert.True(webDriver.FindElement(licenceImprovCommercialSizeLabel).Displayed);
            if (lease.CommercialImprovementBuildingSize != "")
                Assert.True(webDriver.FindElement(licenceImprovCommercialSizeInput).GetAttribute("value") == lease.CommercialImprovementBuildingSize);
            Assert.True(webDriver.FindElement(licenceImprovCommercialDescriptionLabel).Displayed);
            if (lease.CommercialImprovementDescription != "")
                Assert.True(webDriver.FindElement(licenceImprovCommercialDescriptionTextarea).Text == lease.CommercialImprovementDescription);

            Assert.True(webDriver.FindElement(licenceImprovResidentialSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovResidentialUnitNbrLabel).Displayed);
            if(lease.ResidentialImprovementUnit != "")
                Assert.True(webDriver.FindElement(licenceImprovResidentialUnitNbrInput).GetAttribute("value") == lease.ResidentialImprovementUnit);
            Assert.True(webDriver.FindElement(licenceImprovResidentialSizeLabel).Displayed);
            if (lease.ResidentialImprovementBuildingSize != "")
                Assert.True(webDriver.FindElement(licenceImprovResidentialSizeInput).GetAttribute("value") == lease.ResidentialImprovementBuildingSize);
            Assert.True(webDriver.FindElement(licenceImprovResidentialDescriptionLabel).Displayed);
            if(lease.ResidentialImprovementDescription != "")
                Assert.True(webDriver.FindElement(licenceImprovResidentialDescriptionTextarea).Text == lease.ResidentialImprovementDescription);

            Assert.True(webDriver.FindElement(licenceImprovOtherSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenceImprovOtherlUnitNbrLabel).Displayed);
            if(lease.OtherImprovementUnit != "")
                Assert.True(webDriver.FindElement(licenceImprovOtherUnitNbrInput).GetAttribute("value") == lease.OtherImprovementUnit);
            Assert.True(webDriver.FindElement(licenceImprovOtherSizeLabel).Displayed);
            if (lease.OtherImprovementBuildingSize != "")
                Assert.True(webDriver.FindElement(licenceImprovOtherSizeInput).GetAttribute("value") == lease.OtherImprovementBuildingSize);
            Assert.True(webDriver.FindElement(licenceImprovOtherDescriptionLabel).Displayed);
            if (lease.OtherImprovementDescription != "")
                Assert.True(webDriver.FindElement(licenceImprovOtherDescriptionTextarea).Text == lease.OtherImprovementDescription);
        }
    }
}
