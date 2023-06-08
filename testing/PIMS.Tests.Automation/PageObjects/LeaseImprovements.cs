

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseImprovements : PageObjectBase
    {
        private By licenseImprovementLink = By.XPath("//a[contains(text(),'Improvements')]");
        private By improvementEditIcon = By.XPath("//div[@role='tabpanel'][3]/div/div/button");

        private By licenseImprovCommecialSubtitle = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2");
        private By licenseImprovCommercialUnitNbrLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenseImprovCommercialUnitNbrInput = By.Id("input-improvements.0.address");
        private By licenseImprovCommercialSizeLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenseImprovCommercialSizeInput = By.Id("input-improvements.0.structureSize");
        private By licenseImprovCommercialDescriptionLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenseImprovCommercialDescriptionTextarea = By.Id("input-improvements.0.description");

        private By licenseImprovResidentialSubtitle = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2");
        private By licenseImprovResidentialUnitNbrLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenceImprovResidentialUnitNbrInput = By.Id("input-improvements.1.address");
        private By licenseImprovResidentialUnitContent = By.Id("input-improvements.2.address");
        private By licenseImprovResidentialSizeLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenceImprovResidentialSizeInput = By.Id("input-improvements.1.structureSize");
        private By licenseImprovResidentialSizeContent = By.Id("input-improvements.2.structureSize");
        private By licenseImprovResidentialDescriptionLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenseImprovResidentialDescriptionTextarea = By.Id("input-improvements.1.description");
        private By licenseImprovResidentialDescriptionContent = By.Id("input-improvements.2.description");

        private By licenseImprovOtherSubtitle = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2");
        private By licenseImprovOtherlUnitNbrLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenceImprovOtherUnitNbrInput = By.Id("input-improvements.2.address");
        private By licenseImprovOtherUnitNbrContent = By.Id("input-improvements.1.address");
        private By licenseImprovOtherSizeLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenceImprovOtherSizeInput = By.Id("input-improvements.2.structureSize");
        private By licenseImprovOtherSizeContent = By.Id("input-improvements.1.structureSize");
        private By licenseImprovOtherDescriptionLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenceImprovOtherDescriptionTextarea = By.Id("input-improvements.2.description");
        private By licenseImprovOtherDescriptionContent = By.Id("input-improvements.1.description");

        private By licenseImproSaveButton = By.XPath("//button/div[contains(text(),'Save')]/ancestor::button");

        private By licenseImproTotal = By.XPath("//div[@role='tabpanel'][3]/div/form/div/div");

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
                ClearInput(licenseImprovCommercialUnitNbrInput);
                webDriver.FindElement(licenseImprovCommercialUnitNbrInput).SendKeys(lease.CommercialImprovementUnit);
            }

            if (lease.CommercialImprovementBuildingSize != "")
            {
                ClearInput(licenseImprovCommercialSizeInput);
                webDriver.FindElement(licenseImprovCommercialSizeInput).SendKeys(lease.CommercialImprovementBuildingSize);
            }

            if (lease.CommercialImprovementDescription != "")
            {
                ClearInput(licenseImprovCommercialDescriptionTextarea);
                webDriver.FindElement(licenseImprovCommercialDescriptionTextarea).SendKeys(lease.CommercialImprovementDescription);
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
                ClearInput(licenseImprovResidentialDescriptionTextarea);
                webDriver.FindElement(licenseImprovResidentialDescriptionTextarea).SendKeys(lease.ResidentialImprovementDescription);
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

            //Commercial Improvements
            if (lease.CommercialImprovementUnit != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovCommecialSubtitle).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovCommercialUnitNbrLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovCommercialUnitNbrInput).GetAttribute("value") == lease.CommercialImprovementUnit);
            }

            if (lease.CommercialImprovementBuildingSize != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovCommercialSizeLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovCommercialSizeInput).GetAttribute("value") == lease.CommercialImprovementBuildingSize);
            }

            if (lease.CommercialImprovementDescription != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovCommercialDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovCommercialDescriptionTextarea).Text == lease.CommercialImprovementDescription);
            }
                
            //Residential Improvements
            if (lease.ResidentialImprovementUnit != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovResidentialSubtitle).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovResidentialUnitNbrLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovResidentialUnitContent).GetAttribute("value") == lease.ResidentialImprovementUnit);
            }

            if (lease.ResidentialImprovementBuildingSize != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovResidentialSizeLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovResidentialSizeContent).GetAttribute("value") == lease.ResidentialImprovementBuildingSize);
            }
                
            if (lease.ResidentialImprovementDescription != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovResidentialDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovResidentialDescriptionContent).Text == lease.ResidentialImprovementDescription);
            }

            //Other Improvements
            if (lease.OtherImprovementUnit != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovOtherSubtitle).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovOtherlUnitNbrLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovOtherUnitNbrContent).GetAttribute("value") == lease.OtherImprovementUnit);
            }

            if (lease.OtherImprovementBuildingSize != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovOtherSizeLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovOtherSizeContent).GetAttribute("value") == lease.OtherImprovementBuildingSize);
            }

            if (lease.OtherImprovementDescription != "")
            {
                Assert.True(webDriver.FindElement(licenseImprovOtherDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(licenseImprovOtherDescriptionContent).Text == lease.OtherImprovementDescription);
            }
        }

        public int ImprovementTotal()
        {
            return webDriver.FindElements(licenseImproTotal).Count;
        }
    }
}
