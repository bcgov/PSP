

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseImprovements : PageObjectBase
    {
        private By licenseImprovementLink = By.XPath("//a[contains(text(),'Improvements')]");
        private By improvementEditIcon = By.XPath("//div[@role='tabpanel']/div/div/button");

        private By licenseImprovCommecialSubtitle = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2");
        private By licenseImprovCommercialUnitNbrLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenseImprovCommercialUnitNbrContent = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]/parent::div/following-sibling::div");
        private By licenseImprovCommercialUnitNbrInput = By.Id("input-improvements.0.address");
        private By licenseImprovCommercialSizeLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenseImprovCommercialSizeContent = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]/parent::div/following-sibling::div");
        private By licenseImprovCommercialSizeInput = By.Id("input-improvements.0.structureSize");
        private By licenseImprovCommercialDescriptionLabel = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenseImprovCommercialDescriptionContent = By.XPath("//div[contains(text(),'Commercial Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div");
        private By licenseImprovCommercialDescriptionTextarea = By.Id("input-improvements.0.description");

        private By licenseImprovResidentialSubtitle = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2");
        private By licenseImprovResidentialUnitNbrLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenceImprovResidentialUnitNbrInput = By.Id("input-improvements.1.address");
        private By licenseImprovResidentialUnitContent = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]/parent::div/following-sibling::div");
        private By licenseImprovResidentialSizeLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenceImprovResidentialSizeInput = By.Id("input-improvements.1.structureSize");
        private By licenseImprovResidentialSizeContent = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]/parent::div/following-sibling::div");
        private By licenseImprovResidentialDescriptionLabel = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenseImprovResidentialDescriptionTextarea = By.Id("input-improvements.1.description");
        private By licenseImprovResidentialDescriptionContent = By.XPath("//div[contains(text(),'Residential')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div");

        private By licenseImprovOtherSubtitle = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2");
        private By licenseImprovOtherlUnitNbrLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]");
        private By licenceImprovOtherUnitNbrInput = By.Id("input-improvements.2.address");
        private By licenseImprovOtherUnitNbrContent =  By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Unit #')]/parent::div/following-sibling::div");
        private By licenseImprovOtherSizeLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]");
        private By licenceImprovOtherSizeInput = By.Id("input-improvements.2.structureSize");
        private By licenseImprovOtherSizeContent = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Building size')]/parent::div/following-sibling::div");
        private By licenseImprovOtherDescriptionLabel = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private By licenceImprovOtherDescriptionTextarea = By.Id("input-improvements.2.description");
        private By licenseImprovOtherDescriptionContent = By.XPath("//div[contains(text(),'Other Improvements')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div");

        private By licenseImproSaveButton = By.XPath("//button/div[contains(text(),'Save')]/ancestor::button");

        private By licenseImproTotal = By.XPath("//div[@role='tabpanel']/div/form/div/div");

        public LeaseImprovements(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigate to Improvements section
        public void NavigateToImprovementSection()
        {
            WaitUntilClickable(licenseImprovementLink);
            webDriver.FindElement(licenseImprovementLink).Click();
        }

        //Edit Improvements section
        public void EditImprovements()
        {
            WaitUntilClickable(improvementEditIcon);
            webDriver.FindElement(improvementEditIcon).Click();

            WaitUntilSpinnerDisappear();
        }

        //Add Commercial Improvements
        public void AddCommercialImprovement(Lease lease)
        {
            Wait();

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
            Wait();

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
            Wait();

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
            //Commercial Improvements
            if (lease.CommercialImprovementUnit != "")
            {
                AssertTrueIsDisplayed(licenseImprovCommecialSubtitle);
                AssertTrueIsDisplayed(licenseImprovCommercialUnitNbrLabel);
                AssertTrueContentEquals(licenseImprovCommercialUnitNbrContent,lease.CommercialImprovementUnit);
            }

            if (lease.CommercialImprovementBuildingSize != "")
            {
                AssertTrueIsDisplayed(licenseImprovCommercialSizeLabel);
                AssertTrueContentEquals(licenseImprovCommercialSizeContent,lease.CommercialImprovementBuildingSize);
            }

            if (lease.CommercialImprovementDescription != "")
            {
                AssertTrueIsDisplayed(licenseImprovCommercialDescriptionLabel);
                AssertTrueContentEquals(licenseImprovCommercialDescriptionContent,lease.CommercialImprovementDescription);
            }
                
            //Residential Improvements
            if (lease.ResidentialImprovementUnit != "")
            {
                AssertTrueIsDisplayed(licenseImprovResidentialSubtitle);
                AssertTrueIsDisplayed(licenseImprovResidentialUnitNbrLabel);
                AssertTrueContentEquals(licenseImprovResidentialUnitContent,lease.ResidentialImprovementUnit);
            }

            if (lease.ResidentialImprovementBuildingSize != "")
            {
                AssertTrueIsDisplayed(licenseImprovResidentialSizeLabel);
                AssertTrueContentEquals(licenseImprovResidentialSizeContent,lease.ResidentialImprovementBuildingSize);
            }
                
            if (lease.ResidentialImprovementDescription != "")
            {
                AssertTrueIsDisplayed(licenseImprovResidentialDescriptionLabel);
                AssertTrueContentEquals(licenseImprovResidentialDescriptionContent,lease.ResidentialImprovementDescription);
            }

            //Other Improvements
            if (lease.OtherImprovementUnit != "")
            {
                AssertTrueIsDisplayed(licenseImprovOtherSubtitle);
                AssertTrueIsDisplayed(licenseImprovOtherlUnitNbrLabel);
                AssertTrueContentEquals(licenseImprovOtherUnitNbrContent,lease.OtherImprovementUnit);
            }

            if (lease.OtherImprovementBuildingSize != "")
            {
                AssertTrueIsDisplayed(licenseImprovOtherSizeLabel);
                AssertTrueContentEquals(licenseImprovOtherSizeContent,lease.OtherImprovementBuildingSize);
            }

            if (lease.OtherImprovementDescription != "")
            {
                AssertTrueIsDisplayed(licenseImprovOtherDescriptionLabel);
                AssertTrueContentEquals(licenseImprovOtherDescriptionContent, lease.OtherImprovementDescription);
            }
        }

        public int ImprovementTotal()
        {
            Wait();
            return webDriver.FindElements(licenseImproTotal).Count;
        }
    }
}
