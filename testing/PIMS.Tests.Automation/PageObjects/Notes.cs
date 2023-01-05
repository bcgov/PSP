

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Notes : PageObjectBase
    {
        //Notes header
        private By notesTitle = By.XPath("//h2/div/div/div/div[contains(text(),'Notes')]");
        private By notesAddNote = By.XPath("//h2/div/div/div/div[contains(text(),'Notes')]/following-sibling::div/button");
        private By notesTable = By.CssSelector("div[data-testid='notesTable']");
        private By notesNoteColumn = By.XPath("//div[@data-testid='notesTable']/div/div/div/div[contains(text(),'Note')]");
        private By notesCreatedDateColumn = By.XPath("//div[@data-testid='notesTable']/div/div/div/div[contains(text(),'Created date')]");
        private By notesCreatedByColumn = By.XPath("//div[@data-testid='notesTable']/div/div/div/div[contains(text(),'Last updated by')]");
        private By notesBodyRows = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']");



        public Notes(IWebDriver webDriver) : base(webDriver)
        {}

        public void VerifyNotesListView()
        {
            Wait();
            Assert.True(webDriver.FindElement(notesTitle).Displayed);
            Assert.True(webDriver.FindElement(notesAddNote).Displayed);
            Assert.True(webDriver.FindElement(notesTable).Displayed);
            Assert.True(webDriver.FindElement(notesNoteColumn).Displayed);
            Assert.True(webDriver.FindElement(notesCreatedDateColumn).Displayed);
            Assert.True(webDriver.FindElement(notesCreatedByColumn).Displayed);
        }

        public void VerifyAutomaticNotes(string fromStatus, string toStatus)
        {
            Wait();
            var lastNoteIndex = webDriver.FindElements(notesBodyRows).Count();
            var lastNote = "div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastNoteIndex +") div[class='tr'] div[class='td']:nth-child(1)";
            Assert.True(webDriver.FindElement(By.CssSelector(lastNote)).Text == "Activity status changed from "+ fromStatus +" to " + toStatus);
        }
    }
}
