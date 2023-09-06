using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionStakeholders : PageObjectBase
    {
        private By stakeholderLinkTab = By.XPath("//a[contains(text(),'Stakeholders')]");

        private By stakeholderInterestsSubtitle = By.XPath("//h2/div/div[contains(text(),'Interests')]");
        private By stakeholderInterestsEditBttn = By.CssSelector("button[title = 'Edit Interests']");
        private By stakeholderInterestInitP1 = By.XPath("//p[contains(text(),'There are no interest holders associated with this file.')]");
        private By stakeholderInterestInitP2 = By.XPath("//p[contains(text(),'To add an interest holder, click the edit button.')]");

        private By stakeholderNonInterestsSubtitle = By.XPath("//h2/div/div[contains(text(),'Non-interests Payees')]");
        private By stakeholderNonInterestsEditBttn = By.CssSelector("button[title = 'Edit Non-interest payees']");
        private By stakeholderNonInterestInitP1 = By.XPath("//p[contains(text(),'There are no non-interest payees associated with this file.')]");
        private By stakeholderNonInterestInitP2 = By.XPath("//p[contains(text(),'To add a non-interest payee, click the edit button.')]");

        private By stakeholderInterestsEditP1 = By.XPath("//p[contains(text(),'Interests will need to be in the')]");
        private By stakeholderInterestsEditP2 = By.XPath("//i[contains(text(),'No Interest holders to display')]");
        private By stakeholderInterestAddInterestHolderLink = By.XPath("//button/div[contains(text(),'Interest holder')]");

        private By stakeholderNonInterestsEditP1 = By.XPath("//p[contains(text(),'These are additional payees for the file who are not interest holders. (ex: construction for fences). Payees will need to be in the')]");
        private By stakeholderNonInterestsEditP2 = By.XPath("//i[contains(text(),'No Non-interest payees to display')]");
        private By stakeholderInterestAddNonInterestHolderLink = By.XPath("//button/div[contains(text(),'Add a Non-interest payee')]");


        public AcquisitionStakeholders(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateStakeholderTab()
        {
            WaitUntilClickable(stakeholderLinkTab);
            webDriver.FindElement(stakeholderLinkTab).Click();
        }

        public void EditStakeholderInterestsButton()
        {
            WaitUntilClickable(stakeholderInterestsEditBttn);
            webDriver.FindElement(stakeholderInterestsEditBttn).Click();
        }

        public void CreateInterestsStakeholder(AcquisitionStakeholder interest, int index)
        {
            AssertTrueIsDisplayed(By.XPath("//input[@id='input-interestHolders.0.contact.id']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Interest holder')]"));
            AssertTrueIsDisplayed(By.Id("input[id='input-interestHolders.0.contact.id']"));
            webDriver.FindElement(By.XPath("//input[@id='input-interestHolders.0.contact.id']/parent::div/parent::div/following-sibling::div/button[@title='Select Contact']")).Click();


            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Interests')]/parent::div/parent::h2/following-sibling::div/div[2]/div/label[contains(text(),'Interest type')]"));

            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Interests')]/parent::div/parent::h2/following-sibling::div/div[3]/div/label[contains(text(),'Impacted properties')]"));
        }

        public void VerifyStakeholdersInitView()
        {
            AssertTrueIsDisplayed(stakeholderInterestsSubtitle);
            AssertTrueIsDisplayed(stakeholderInterestsEditP1);
            AssertTrueIsDisplayed(stakeholderInterestsEditP2);
            AssertTrueIsDisplayed(stakeholderInterestAddInterestHolderLink);

            AssertTrueIsDisplayed(stakeholderNonInterestsSubtitle);
            AssertTrueIsDisplayed(stakeholderNonInterestsEditBttn);
            AssertTrueIsDisplayed(stakeholderNonInterestInitP1);
            AssertTrueIsDisplayed(stakeholderNonInterestInitP2);
        }

        public void VerifyStakeholderInitEditForm()
        {
            AssertTrueIsDisplayed(stakeholderInterestsSubtitle);
            AssertTrueIsDisplayed(stakeholderInterestsEditBttn);
            AssertTrueIsDisplayed(stakeholderInterestInitP1);
            AssertTrueIsDisplayed(stakeholderInterestInitP2);

            AssertTrueIsDisplayed(stakeholderNonInterestsSubtitle);
            AssertTrueIsDisplayed(stakeholderNonInterestsEditP1);
            AssertTrueIsDisplayed(stakeholderNonInterestsEditP2);
            AssertTrueIsDisplayed(stakeholderInterestAddNonInterestHolderLink);
        }
    }
}
