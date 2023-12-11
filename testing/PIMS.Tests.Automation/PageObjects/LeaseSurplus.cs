

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseSurplus : PageObjectBase
    {
        private By licenseSurplusLink = By.XPath("//a[contains(text(),'Surplus Declaration')]");

        private By licenseSurplusInstructions = By.XPath("//p[contains(text(),'Data shown is from the Surplus Declaration workflo')]");
        private By licenseSurplusTableBody = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody']");
        private By licenceSurplusPIDColumn = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PID / Identifier')]");
        private By licenseSurplusDeclarationColumn = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Surplus Declaration')]");
        private By licenseSurplusDeclarationDateColumn = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Declaration Date')]");
        private By licenseSurplusDeclarationCommentsColumn = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Declaration Comments')]");

        public LeaseSurplus(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Surplus Declaration Section
        public void NavigateToSurplusSection()
        {
            WaitUntilClickable(licenseSurplusLink);
            webDriver.FindElement(licenseSurplusLink).Click();
        }

        public void VerifySurplusForm()
        {
            AssertTrueIsDisplayed(licenseSurplusInstructions);
            AssertTrueIsDisplayed(licenseSurplusTableBody);
            AssertTrueIsDisplayed(licenceSurplusPIDColumn);
            AssertTrueIsDisplayed(licenseSurplusDeclarationColumn);
            AssertTrueIsDisplayed(licenseSurplusDeclarationDateColumn);
            AssertTrueIsDisplayed(licenseSurplusDeclarationCommentsColumn);
        }
    }
}
